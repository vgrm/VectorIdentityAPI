using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using vector_control_system_api.Database;
using Microsoft.EntityFrameworkCore;
using vector_control_system_api.Models.ProjectData;

namespace vector_control_system_api.Services.Analysis
{
    public class AnalyzeService : IAnalyzeService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger<AnalyzeService> _logger;


        public AnalyzeService(DatabaseContext databaseContext, ILogger<AnalyzeService> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        public async Task Analyze(ProjectData projectData, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Doing heavy analyzer logic ...");

            if (projectData.Status == "Accepted" || projectData.Status == "New"
                //|| projectData.Status == "Processing"
                )
            {
                //analyze file

                //change status
                projectData.Status = "Processing";
                projectData.StateId = -2;
                _databaseContext.Entry(projectData).State = EntityState.Modified;

                //save changes
                try
                {
                    await _databaseContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                //update data
                await UpdateData(projectData);
            }

            await CalculateCorrectnessScore(projectData);
            await CalculateIdentityScore(projectData);

            projectData.Status = "Evaluated";
            _databaseContext.Entry(projectData).State = EntityState.Modified;
            await _databaseContext.SaveChangesAsync();

            await Task.Delay(250, cancellationToken);
            _logger.LogInformation("\"{Name} by {Author}\" has been published!", projectData.Name, projectData.Owner);


        }

        private async Task UpdateData(ProjectData projectData)
        {
            _databaseContext.Line.RemoveRange(_databaseContext.Line.Where(x => x.ProjectId == projectData.Id));
            _databaseContext.Arc.RemoveRange(_databaseContext.Arc.Where(x => x.ProjectId == projectData.Id));

            // From byte array to string
            string fileData = System.Text.Encoding.ASCII.GetString(projectData.FileData);
            List<Line> lines = new List<Line>();
            List<Arc> arcs = new List<Arc>();

            //find lines and arcs
            string Layer = "";
            string Handle = "";

            string[] D = fileData.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            int iEntities = 0; for (int i = 0; i < D.Length; i++) { if (D[i] == "ENTITIES") { iEntities = i; break; } }
            for (int i = iEntities; i < D.Length; i++)
            {
                if (D[i] == "POINT" || D[i] == "AcDbPoint")
                {
                    int iEntity = i; if (D[i].StartsWith("AcDb")) { for (iEntity = i; D[iEntity] != "AcDbEntity"; iEntity--) ; }
                    Layer = ""; for (int iLayer = iEntity; iLayer < i + 10 && Layer == ""; iLayer++) { if (D[iLayer] == "  8") { Layer = D[iLayer + 1]; }; }
                    for (int iWaarden = i; iWaarden < i + 8; iWaarden++)
                    {
                        if (D[iWaarden] == " 10" && D[iWaarden + 2] == " 20")
                        {
                            //Here you can store the following data in a list for later use
                            //LayerName = Layer
                            //X = D[iWaarden + 1]
                            //Y = D[iWaarden + 3]
                            //Z = D[iWaarden + 5]
                        }
                    }
                }

                //Line
                if (D[i] == "LINE")
                {
                    double X1 = 0;
                    double Y1 = 0;
                    double Z1 = 0;

                    double X2 = 0;
                    double Y2 = 0;
                    double Z2 = 0;

                    double Magnitude = 0;

                    double DX = 0;
                    double DY = 0;
                    double DZ = 0;

                    //find entity index
                    int iLine = i;
                    int iEntity = i;

                    for (iEntity = i; D[iEntity] != "AcDbEntity" && iEntity < i + 10; iEntity++) ;
                    for (iLine = i; D[iLine] != "AcDbLine" && iLine < i + 100; iLine++) ;

                    //find LINE index
                    //int iLine = i;
                    //for (iLine = i; D[iLine] != "LINE" && iLine < i + 100; iLine++) ;

                    //find layer name
                    Layer = "";
                    for (int iLayer = iEntity; iLayer < i + 10 && Layer == ""; iLayer++)
                    {
                        if (D[iLayer] == "  8")
                        {
                            Layer = D[iLayer + 1];
                        }
                    }

                    //find handle id
                    Handle = "";
                    for (int iHandle = i; iHandle < i + 10 && Handle == ""; iHandle++)
                    {
                        if (D[iHandle] == "  5")
                        {
                            Handle = D[iHandle + 1];
                        }
                    }

                    for (int ii = iLine; ii < iLine + 10; ii++)
                    {
                        if (D[ii] == " 10" && D[ii + 2] == " 20")
                        {
                            var number = 10000000000000;
                            //raw as accurate as we can get from dxf
                            double tempX1 = Convert.ToDouble(D[ii + 1]);
                            double tempY1 = Convert.ToDouble(D[ii + 3]);
                            double tempZ1 = Convert.ToDouble(D[ii + 5]);
                            double tempX2 = Convert.ToDouble(D[ii + 7]);
                            double tempY2 = Convert.ToDouble(D[ii + 9]);
                            double tempZ2 = Convert.ToDouble(D[ii + 11]);

                            //lower accuracy to minimize all the floating points
                            //X1 = Convert.ToDouble(D[ii + 1].PadRight(17, '0').Substring(0, D[ii + 1].Length - 2));
                            //Y1 = Convert.ToDouble(D[ii + 3].PadRight(17, '0').Substring(0, D[ii + 3].Length - 2));
                            //Z1 = Convert.ToDouble(D[ii + 5].PadRight(17, '0').Substring(0, D[ii + 5].Length - 2));
                            //X2 = Convert.ToDouble(D[ii + 7].PadRight(17, '0').Substring(0, D[ii + 7].Length - 2));
                            //Y2 = Convert.ToDouble(D[ii + 9].PadRight(17, '0').Substring(0, D[ii + 9].Length - 2));
                            //Z2 = Convert.ToDouble(D[ii + 11].PadRight(17, '0').Substring(0, D[ii + 11].Length - 2));
                            X1 = Convert.ToDouble(string.Format("{0:0.################}", Math.Truncate(tempX1 * number) / number));
                            Y1 = Convert.ToDouble(string.Format("{0:0.################}", Math.Truncate(tempY1 * number) / number));
                            Z1 = Convert.ToDouble(string.Format("{0:0.################}", Math.Truncate(tempZ1 * number) / number));
                            X2 = Convert.ToDouble(string.Format("{0:0.################}", Math.Truncate(tempX2 * number) / number));
                            Y2 = Convert.ToDouble(string.Format("{0:0.################}", Math.Truncate(tempY2 * number) / number));
                            Z2 = Convert.ToDouble(string.Format("{0:0.################}", Math.Truncate(tempZ2 * number) / number));

                            double V1 = tempX1 - tempX2;
                            double V2 = tempY1 - tempY2;
                            double V3 = tempZ1 - tempZ2;

                            Magnitude = Math.Sqrt(Math.Pow(V1, 2) + Math.Pow(V2, 2) + Math.Pow(V3, 2));
                            DX = V1 / Magnitude;
                            DY = V2 / Magnitude;
                            DZ = V3 / Magnitude;

                            //string.format(new FormatProvider(), "{0:T(1)0,000.0", 1000.9999); // 1,000.9

                            DX = Convert.ToDouble(string.Format("{0:0.################}", Math.Truncate(DX * number) / number));
                            DY = Convert.ToDouble(string.Format("{0:0.################}", Math.Truncate(DY * number) / number));
                            DZ = Convert.ToDouble(string.Format("{0:0.################}", Math.Truncate(DZ * number) / number));

                            //DX = Convert.ToDouble(DX.ToString("0.##############"));
                            //DY = Convert.ToDouble(DY.ToString("0.##############"));
                            //DZ = Convert.ToDouble(DZ.ToString("0.##############"));
                            /*
                            if (DX>0)DX = Convert.ToDouble(DX.ToString().PadRight(20, '0').Substring(0, 16));
                            else DX = Convert.ToDouble(DX.ToString().PadRight(20, '0').Substring(0, 17));

                            if (DY > 0) DX = Convert.ToDouble(DY.ToString().PadRight(20, '0').Substring(0, 16));
                            else DY = Convert.ToDouble(DY.ToString().PadRight(20, '0').Substring(0, 17));

                            if (DZ > 0) DX = Convert.ToDouble(DZ.ToString().PadRight(20, '0').Substring(0, 16));
                            else DZ = Convert.ToDouble(DZ.ToString().PadRight(20, '0').Substring(0, 17));
                            */
                            _logger.LogInformation("VECTOR: {v1} {v2} {v3}", V1, V2, V3);

                            Line newLine = new Line
                            {
                                ProjectId = projectData.Id,
                                Handle = Handle,
                                Layer = Layer,
                                Correct = false,
                                X1 = X1,
                                Y1 = Y1,
                                Z1 = Z1,
                                X2 = X2,
                                Y2 = Y2,
                                Z2 = Z2,

                                Magnitude = Magnitude,
                                DX = DX,
                                DY = DY,
                                DZ = DZ
                            };
                            lines.Add(newLine);
                        }
                    }
                }

                //Arc
                if (D[i] == "ARC" || D[i] == "CIRCLE")
                {
                    double X = 0;
                    double Y = 0;
                    double Z = 0;

                    double Radius = 0;
                    double AngleStart = 0;
                    double AngleEnd = 0;

                    double DX = 0;
                    double DY = 0;
                    double DZ = 1;

                    //find entity index
                    int iEntity = i;
                    int iCircle = i;
                    int iArc = i;



                    for (iEntity = i; D[iEntity] != "AcDbEntity" && iEntity < i + 100; iEntity++) ;
                    for (iCircle = i; D[iCircle] != "AcDbCircle" && iCircle < i + 100; iCircle++) ;
                    if (D[i] == "ARC")
                    {
                        for (iArc = i; D[iArc] != "AcDbArc" && iArc < i + 100; iArc++) ;
                    }
                    //int iEntity = i; if (D[i].StartsWith("AcDb")) { for (iEntity = i; D[iEntity] != "AcDbEntity"; iEntity--) ; }
                    //Layer = ""; for (int iLayer = iEntity; iLayer < i + 10 && Layer == ""; iLayer++) { if (D[iLayer] == "  8") { Layer = D[iLayer + 1]; }; }

                    //find layer name
                    Layer = "";
                    for (int iLayer = iEntity; iLayer < iEntity + 10 && Layer == ""; iLayer++)
                    {
                        if (D[iLayer] == "  8")
                        {
                            Layer = D[iLayer + 1];
                            //_logger.LogInformation("\"{0} entity {1}\" logged \n LayerName:{Layer}\n D[iLayer]:{D}", i, iEntity, Layer, D[iLayer]);
                        }
                    }
                    //_logger.LogInformation("\"{0} entity {1}\" logged \n LayerName:{Layer}\n D[iLayer]:{D}", i, iEntity, Layer, D[iEntity+1]);

                    //find handle id
                    Handle = "";
                    for (int iHandle = i; iHandle < i + 10 && Handle == ""; iHandle++)
                    {
                        if (D[iHandle] == "  5")
                        {
                            Handle = D[iHandle + 1];
                        }
                    }
                    var number = 10000000000000;
                    for (int ii = iCircle; ii < iCircle + 10; ii++)
                    {
                        if (D[ii] == " 10" && D[ii + 2] == " 20" && D[ii + 4] == " 30" && D[ii + 6] == " 40")
                        {

                            X = Convert.ToDouble(string.Format("{0:0.################}", Math.Truncate((Convert.ToDouble(D[ii + 1]) * number)) / number));
                            Y = Convert.ToDouble(string.Format("{0:0.################}", Math.Truncate((Convert.ToDouble(D[ii + 3]) * number)) / number));
                            Z = Convert.ToDouble(string.Format("{0:0.################}", Math.Truncate((Convert.ToDouble(D[ii + 5]) * number)) / number));
                            Radius = Convert.ToDouble(string.Format("{0:0.################}", Math.Truncate((Convert.ToDouble(D[ii + 7]) * number)) / number));

                            //X = Convert.ToDouble(D[ii + 1]);
                            //Y = Convert.ToDouble(D[ii + 3]);
                            //Z = Convert.ToDouble(D[ii + 5]);
                            //Radius = Convert.ToDouble(D[ii + 7]);
                        }

                        if (D[ii + 8] == "210" && D[ii + 10] == "220" && D[ii + 12] == "230")
                        {

                            DX = Convert.ToDouble(string.Format("{0:0.################}", Math.Truncate((Convert.ToDouble(D[ii + 9]) * number)) / number));
                            DY = Convert.ToDouble(string.Format("{0:0.################}", Math.Truncate((Convert.ToDouble(D[ii + 11]) * number)) / number));
                            DZ = Convert.ToDouble(string.Format("{0:0.################}", Math.Truncate((Convert.ToDouble(D[ii + 13]) * number)) / number));
                            Radius = Convert.ToDouble(string.Format("{0:0.################}", Math.Truncate((Convert.ToDouble(D[ii + 7]) * number)) / number));
                            //DX = Convert.ToDouble(D[ii + 9]);
                            //DY = Convert.ToDouble(D[ii + 10]);
                            //DZ = Convert.ToDouble(D[ii + 11]);
                            //Radius = Convert.ToDouble(D[ii + 7]);
                        }
                    }

                    if (iArc != i)
                    {
                        for (int ii = iArc; ii < iArc + 10; ii++)
                        {
                            if (D[ii] == " 50" && D[ii + 2] == " 51")
                            {
                                AngleStart = Convert.ToDouble(D[ii + 1]);
                                AngleEnd = Convert.ToDouble(D[ii + 3]);
                            }
                        }
                    }

                    Arc newArc = new Arc
                    {
                        ProjectId = projectData.Id,
                        Handle = Handle,
                        Layer = Layer,
                        Correct = false,
                        X = X,
                        Y = Y,
                        Z = Z,

                        Radius = Radius,
                        AngleStart = AngleStart,
                        AngleEnd = AngleEnd,

                        DX = DX,
                        DY = DY,
                        DZ = DZ
                    };
                    arcs.Add(newArc);
                }
            }



            //string s = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);

            //projectData.FileData;

            await MinimizeData(lines, arcs);


        }


        private async Task MinimizeData(List<Line> lines, List<Arc> arcs)
        {
            bool minimizedLines = false;
            bool minimizedArcs = false;

            while (!minimizedLines)
            {
                int linesCount = lines.Count();
                lines = MinimizeLines(lines);
                int linesCount2 = lines.Count();
                if (linesCount == linesCount2) minimizedLines = true;
            }

            while (!minimizedArcs)
            {
                int arcsCount = arcs.Count();
                arcs = MinimizeArcs(arcs);
                int arcsCount2 = arcs.Count();
                if (arcsCount == arcsCount2) minimizedArcs = true;
            }

            //UPDATE DB
            _databaseContext.Line.AddRange(lines);
            _databaseContext.Arc.AddRange(arcs);
            await _databaseContext.SaveChangesAsync();
        }

        private List<Line> MinimizeLines(List<Line> lines)
        {
            Line line1 = new Line();
            Line line2 = new Line();
            Line newLine = new Line();
            var number = 10000000000000;
            bool wasMatch = false;

            foreach (var line in lines)
            {

                foreach (var lineTest in lines)
                {
                    //find lines with same or opposite direction
                    if (((line.DX == lineTest.DX && line.DY == lineTest.DY && line.DZ == lineTest.DZ) ||
                        (line.DX == -lineTest.DX && line.DY == -lineTest.DY && line.DZ == -lineTest.DZ)) &&
                        (line.Handle != lineTest.Handle))
                    {

                        //set line boundaries
                        double minX = 0;
                        double maxX = 0;
                        double minY = 0;
                        double maxY = 0;
                        double minZ = 0;
                        double maxZ = 0;

                        if (lineTest.X1 >= lineTest.X2) { minX = lineTest.X2; maxX = lineTest.X1; }
                        else { minX = lineTest.X1; maxX = lineTest.X2; }
                        if (lineTest.Y1 >= lineTest.Y2) { minY = lineTest.Y2; maxY = lineTest.Y1; }
                        else { minY = lineTest.Y1; maxY = lineTest.Y2; }
                        if (lineTest.Z1 >= lineTest.Z2) { minZ = lineTest.Z2; maxZ = lineTest.Z1; }
                        else { minZ = lineTest.Z1; maxZ = lineTest.Z2; }

                        //check if point is in set boundaries
                        if (line.X1 >= minX && line.X1 <= maxX &&
                            line.Y1 >= minY && line.Y1 <= maxY &&
                            line.Z1 >= minZ && line.Z1 <= maxZ)
                        {
                            wasMatch = true;

                            double newX2 = line.X2;
                            if (newX2 < maxX) newX2 = maxX;

                            double newY2 = line.Y2;
                            if (newY2 < maxY) newY2 = maxY;

                            double newZ2 = line.Z2;
                            if (newZ2 < maxZ) newZ2 = maxZ;

                            double V1 = minX - newX2;
                            double V2 = minY - newY2;
                            double V3 = minZ - newZ2;

                            double Magnitude = Math.Sqrt(Math.Pow(V1, 2) + Math.Pow(V2, 2) + Math.Pow(V3, 2));
                            double DX = V1 / Magnitude;
                            double DY = V2 / Magnitude;
                            double DZ = V3 / Magnitude;

                            DX = Convert.ToDouble(string.Format("{0:0.################}", Math.Truncate(DX * number) / number));
                            DY = Convert.ToDouble(string.Format("{0:0.################}", Math.Truncate(DY * number) / number));
                            DZ = Convert.ToDouble(string.Format("{0:0.################}", Math.Truncate(DZ * number) / number));

                            //create new vector
                            //min x2
                            Line newLineTemp = new Line
                            {
                                ProjectId = line.ProjectId,
                                Handle = line.Handle,
                                Layer = line.Layer,
                                Correct = false,
                                X1 = minX,
                                Y1 = minY,
                                Z1 = minZ,
                                X2 = newX2,
                                Y2 = newY2,
                                Z2 = newZ2,
                                /*
                                Magnitude = line.Magnitude + lineTest.Magnitude,
                                DX = line.DX,
                                DY = line.DY,
                                DZ = line.DZ
                                */
                                Magnitude = Magnitude,
                                DX = DX,
                                DY = DY,
                                DZ = DZ

                            };

                            newLine = newLineTemp;
                            line1 = line;
                            line2 = lineTest;
                            break;
                        }

                        //check if point is in set boundaries
                        else if (line.X2 >= minX && line.X2 <= maxX &&
                                 line.Y2 >= minY && line.Y2 <= maxY &&
                                 line.Z2 >= minZ && line.Z2 <= maxZ)
                        {
                            wasMatch = true;

                            double newX1 = line.X1;
                            if (newX1 > minX) newX1 = minX;

                            double newY1 = line.Y1;
                            if (newY1 > minY) newY1 = minY;

                            double newZ1 = line.Z1;
                            if (newZ1 > minZ) newZ1 = minZ;

                            double V1 = newX1 - maxX;
                            double V2 = newY1 - maxY;
                            double V3 = newZ1 - maxZ;

                            double Magnitude = Math.Sqrt(Math.Pow(V1, 2) + Math.Pow(V2, 2) + Math.Pow(V3, 2));
                            double DX = V1 / Magnitude;
                            double DY = V2 / Magnitude;
                            double DZ = V3 / Magnitude;

                            DX = Convert.ToDouble(string.Format("{0:0.################}", Math.Truncate(DX * number) / number));
                            DY = Convert.ToDouble(string.Format("{0:0.################}", Math.Truncate(DY * number) / number));
                            DZ = Convert.ToDouble(string.Format("{0:0.################}", Math.Truncate(DZ * number) / number));

                            //create new vector
                            //x1 max
                            Line newLineTemp = new Line
                            {
                                ProjectId = line.ProjectId,
                                Handle = line.Handle,
                                Layer = line.Layer,
                                Correct = false,
                                X1 = newX1,
                                Y1 = newY1,
                                Z1 = newZ1,
                                X2 = maxX,
                                Y2 = maxY,
                                Z2 = maxZ,
                                /*
                                Magnitude = line.Magnitude + lineTest.Magnitude,
                                DX = line.DX,
                                DY = line.DY,
                                DZ = line.DZ
                                */
                                Magnitude = Magnitude,
                                DX = DX,
                                DY = DY,
                                DZ = DZ
                            };
                            newLine = newLineTemp;
                            line1 = line;
                            line2 = lineTest;
                            break;
                        }
                        else
                        {
                            //same direction but not same coords
                            _logger.LogInformation("VECTORS ARE NOT THE SAME");
                        }
                    }
                }
                if (wasMatch) break;
            }
            if (wasMatch)
            {
                lines.Add(newLine);
                lines.Remove(line1);
                lines.Remove(line2);
            }
            return lines;
        }

        private List<Arc> MinimizeArcs(List<Arc> arcs)
        {
            Arc arc1 = new Arc();
            Arc arc2 = new Arc();
            Arc newArc = new Arc();

            bool wasMatch = false;

            foreach (var arc in arcs)
            {
                foreach (var arcTest in arcs)
                {
                    //find arcs with same R coords and plane
                    if (arc.X == arcTest.X && arc.Y == arcTest.Y && arc.Z == arcTest.Z &&
                        arc.Radius == arcTest.Radius &&
                        arc.DX == arcTest.DX && arc.DY == arcTest.DY && arc.DZ == arcTest.DZ
                        && arc.Handle != arcTest.Handle)
                    {

                        double angleStart = 0;
                        double angleEnd = 0;

                        //if CW
                        if (arc.AngleStart < arc.AngleEnd)
                        {
                            //if CW
                            if (arcTest.AngleStart < arcTest.AngleEnd)
                            {
                                if ((arc.AngleStart < arcTest.AngleStart && arc.AngleEnd > arcTest.AngleStart) ||
                                        (arc.AngleStart < arcTest.AngleEnd && arc.AngleEnd > arcTest.AngleEnd))
                                {
                                    //find smallest and biggest angles for ends
                                    if (arc.AngleStart < arcTest.AngleStart) angleStart = arc.AngleStart;
                                    else angleStart = arcTest.AngleStart;
                                    if (arc.AngleEnd > arcTest.AngleEnd) angleEnd = arc.AngleEnd;
                                    else angleEnd = arcTest.AngleEnd;

                                    wasMatch = true;

                                    Arc newArcTemmp = new Arc
                                    {
                                        ProjectId = arc.ProjectId,
                                        Handle = arc.Handle,
                                        Layer = arc.Layer,
                                        X = arc.X,
                                        Y = arc.Y,
                                        Z = arc.Z,

                                        Radius = arc.Radius,
                                        AngleStart = angleStart,
                                        AngleEnd = angleEnd,

                                        DX = arc.DX,
                                        DY = arc.DY,
                                        DZ = arc.DZ
                                    };

                                    newArc = newArcTemmp;
                                    arc1 = arc;
                                    arc2 = arcTest;
                                    break;
                                }
                            }
                            //if CCW
                            else if (arcTest.AngleStart > arcTest.AngleEnd)
                            {

                                if ((arc.AngleStart < arcTest.AngleStart && arc.AngleEnd > arcTest.AngleStart) ||
                                        (arc.AngleStart < arcTest.AngleEnd && arc.AngleEnd > arcTest.AngleEnd))
                                {

                                    //find smallest and biggest angles for ends
                                    if (arc.AngleStart >= arcTest.AngleStart) angleStart = arc.AngleStart;
                                    else angleStart = arcTest.AngleStart;
                                    if (arc.AngleEnd >= arcTest.AngleEnd) angleEnd = arc.AngleEnd;
                                    else angleEnd = arcTest.AngleEnd;

                                    //its a circle now
                                    if (arc.AngleStart <= arcTest.AngleEnd && arc.AngleEnd >= arcTest.AngleStart)
                                    {
                                        angleStart = 0;
                                        angleEnd = 0;
                                    }
                                    wasMatch = true;
                                    Arc newArcTemmp = new Arc
                                    {
                                        ProjectId = arc.ProjectId,
                                        Handle = arc.Handle,
                                        Layer = arc.Layer,
                                        Correct = false,
                                        X = arc.X,
                                        Y = arc.Y,
                                        Z = arc.Z,

                                        Radius = arc.Radius,
                                        AngleStart = angleStart,
                                        AngleEnd = angleEnd,

                                        DX = arc.DX,
                                        DY = arc.DY,
                                        DZ = arc.DZ
                                    };

                                    newArc = newArcTemmp;
                                    arc1 = arc;
                                    arc2 = arcTest;
                                    break;
                                }
                            }
                        }
                        //if CCW
                        else if (arc.AngleStart < arc.AngleEnd)
                        {
                            //if CW
                            if (arcTest.AngleStart < arcTest.AngleEnd)
                            {
                                if (arc.AngleStart <= arcTest.AngleEnd)
                                {
                                    //circle
                                    if (arc.AngleEnd >= arcTest.AngleStart)
                                    {
                                        angleStart = 0;
                                        angleEnd = 0;
                                    }
                                    //else arc
                                    else
                                    {
                                        angleStart = arcTest.AngleStart;
                                        angleEnd = arc.AngleEnd;
                                    }
                                    wasMatch = true;
                                    Arc newArcTemmp = new Arc
                                    {
                                        ProjectId = arc.ProjectId,
                                        Handle = arc.Handle,
                                        Layer = arc.Layer,
                                        Correct = false,
                                        X = arc.X,
                                        Y = arc.Y,
                                        Z = arc.Z,

                                        Radius = arc.Radius,
                                        AngleStart = angleStart,
                                        AngleEnd = angleEnd,

                                        DX = arc.DX,
                                        DY = arc.DY,
                                        DZ = arc.DZ
                                    };

                                    newArc = newArcTemmp;
                                    arc1 = arc;
                                    arc2 = arcTest;
                                    break;
                                }

                                if (arc.AngleEnd >= arcTest.AngleStart)
                                {
                                    //circle
                                    if (arcTest.AngleEnd >= arc.AngleStart)
                                    {
                                        angleStart = 0;
                                        angleEnd = 0;
                                    }
                                    //else arc
                                    else
                                    {
                                        angleStart = arc.AngleStart;
                                        angleEnd = arcTest.AngleEnd;
                                    }
                                    wasMatch = true;
                                    Arc newArcTemmp = new Arc
                                    {
                                        ProjectId = arc.ProjectId,
                                        Handle = arc.Handle,
                                        Layer = arc.Layer,
                                        Correct = false,
                                        X = arc.X,
                                        Y = arc.Y,
                                        Z = arc.Z,

                                        Radius = arc.Radius,
                                        AngleStart = angleStart,
                                        AngleEnd = angleEnd,

                                        DX = arc.DX,
                                        DY = arc.DY,
                                        DZ = arc.DZ
                                    };

                                    newArc = newArcTemmp;
                                    arc1 = arc;
                                    arc2 = arcTest;
                                    break;
                                }

                            }

                            //if CCW
                            else if (arcTest.AngleStart > arcTest.AngleEnd)
                            {
                                if ((arc.AngleStart >= arcTest.AngleStart && arc.AngleEnd >= arcTest.AngleEnd) ||
                                    (arc.AngleStart <= arcTest.AngleStart && arc.AngleEnd <= arcTest.AngleEnd))
                                {
                                    if (arc.AngleStart < arcTest.AngleStart) angleStart = arc.AngleStart;
                                    else angleStart = arcTest.AngleStart;
                                    if (arc.AngleEnd > arcTest.AngleEnd) angleEnd = arc.AngleEnd;
                                    else angleEnd = arcTest.AngleEnd;

                                    if ((arc.AngleStart >= arcTest.AngleStart && arc.AngleStart >= arcTest.AngleEnd
                                        && arc.AngleEnd >= arcTest.AngleStart && arc.AngleEnd >= arcTest.AngleEnd) ||
                                        (arc.AngleStart <= arcTest.AngleStart && arc.AngleStart <= arcTest.AngleEnd
                                        && arc.AngleEnd <= arcTest.AngleStart && arc.AngleEnd <= arcTest.AngleEnd))
                                    {
                                        angleStart = 0;
                                        angleEnd = 0;
                                    }
                                    wasMatch = true;
                                    Arc newArcTemmp = new Arc
                                    {
                                        ProjectId = arc.ProjectId,
                                        Handle = arc.Handle,
                                        Layer = arc.Layer,
                                        Correct = false,
                                        X = arc.X,
                                        Y = arc.Y,
                                        Z = arc.Z,

                                        Radius = arc.Radius,
                                        AngleStart = angleStart,
                                        AngleEnd = angleEnd,

                                        DX = arc.DX,
                                        DY = arc.DY,
                                        DZ = arc.DZ
                                    };

                                    newArc = newArcTemmp;
                                    arc1 = arc;
                                    arc2 = arcTest;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (wasMatch) break;
            }
            if (wasMatch)
            {
                arcs.Add(newArc);
                arcs.Remove(arc1);
                arcs.Remove(arc2);
            }
            return arcs;
        }

        private async Task CalculateCorrectnessScore(ProjectData testProject)
        {
            //int setId = testProject.ProjectSetId;
            ProjectData originalProject = _databaseContext.ProjectData.Where(x => x.ProjectSetId == testProject.ProjectSetId && x.Original && x.Id != testProject.Id).FirstOrDefault();
            if (originalProject == null) return;

            //_databaseContext.Match.RemoveRange(_databaseContext.Match.Where(x => x.TestProjectId == testProject.Id));

            List<ProjectMatchModel> matchesLine = new List<ProjectMatchModel>();
            List<ProjectMatchModel> matchesArc = new List<ProjectMatchModel>();


            List<Line> testLines = _databaseContext.Line.Where(x => x.ProjectId == testProject.Id).ToList();
            List<Line> originalLines = _databaseContext.Line.Where(x => x.ProjectId == originalProject.Id).ToList();
            List<Arc> testArcs = _databaseContext.Arc.Where(x => x.ProjectId == testProject.Id).ToList();
            List<Arc> originalArcs = _databaseContext.Arc.Where(x => x.ProjectId == originalProject.Id).ToList();

            matchesLine = FindMatchingLines(originalLines, testLines, originalProject.Id, testProject.Id);
            matchesArc = FindMatchingArcs(originalArcs, testArcs, originalProject.Id, testProject.Id);

            double scoreCorrectness = (matchesLine.Count() * 2 + matchesArc.Count() * 2) / (testLines.Count() + originalLines.Count() + testArcs.Count() + originalArcs.Count());
            testProject.ScoreCorrectness = scoreCorrectness;
            //testProject.Status = "Evaluated";
            //_databaseContext.ProjectData.Update(testProject);
            _databaseContext.Entry(testProject).State = EntityState.Modified;
            //_databaseContext.Match.AddRange(matchesLine);
            await _databaseContext.SaveChangesAsync();

            /*
            Match match = new Match
            {
                Name = "name",
                Info = "info",
                Type = "line match",

            };

            _databaseContext.Match.Add(match);
            await _databaseContext.SaveChangesAsync();
            */
        }

        private List<ProjectMatchModel> FindMatchingLines(List<Line> originalLines, List<Line> testLines, int originalProjectId, int testProjectId)
        {
            List<ProjectMatchModel> matches = new List<ProjectMatchModel>();

            foreach (var originalLine in originalLines)
            {
                foreach (var testLine in testLines)
                {
                    if (originalLine.Magnitude == testLine.Magnitude &&
                       originalLine.DX == testLine.DX &&
                       originalLine.DY == testLine.DY &&
                       originalLine.DZ == testLine.DZ)
                    {
                        testLine.Correct = true;
                        _databaseContext.Entry(testLine).State = EntityState.Modified;

                        ProjectMatchModel match = new ProjectMatchModel
                        {
                            Name = "name",
                            Info = "info",
                            Type = "line match",
                            LineOriginalId = originalLine.Id,
                            LineTestId = testLine.Id,

                            ArcOriginalId = 0,
                            ArcTestId = 0,

                            //OriginalProjectId = originalProjectId,
                            //TestProjectId = testProjectId
                        };
                        matches.Add(match);
                    }
                }
            }

            return matches;
        }

        private List<ProjectMatchModel> FindMatchingArcs(List<Arc> originalArcs, List<Arc> testArcs, int originalProjectId, int testProjectId)
        {
            List<ProjectMatchModel> matches = new List<ProjectMatchModel>();

            foreach (var originalArc in originalArcs)
            {
                foreach (var testArc in testArcs)
                {
                    if (originalArc.Radius == testArc.Radius &&
                       originalArc.DX == testArc.DX &&
                       originalArc.DY == testArc.DY &&
                       originalArc.DZ == testArc.DZ &&
                       originalArc.X == testArc.X &&
                       originalArc.Y == testArc.Y &&
                       originalArc.Z == testArc.Z &&
                       originalArc.AngleStart == testArc.AngleStart &&
                       originalArc.AngleEnd == testArc.AngleEnd)
                    {
                        ProjectMatchModel match = new ProjectMatchModel
                        {
                            Name = "name",
                            Info = "info",
                            Type = "arc match",
                            LineOriginalId = 0,
                            LineTestId = 0,

                            ArcOriginalId = 0,
                            ArcTestId = 0,

                            //OriginalProjectId = originalProjectId,
                            //TestProjectId = testProjectId
                        };
                        matches.Add(match);
                    }
                }
            }

            return matches;
        }

        private async Task CalculateIdentityScore(ProjectData projectData)
        {
            List<ProjectData> projects = _databaseContext.ProjectData.Where(x => x.ProjectSetId == projectData.ProjectSetId && !x.Original && x.Id != projectData.Id).ToList();
            if (projects == null) return;

            List<Line> lines = _databaseContext.Line.Where(x => x.ProjectId == projectData.Id).ToList();
            List<Arc> arcs = _databaseContext.Arc.Where(x => x.ProjectId == projectData.Id).ToList();

            List<Line> matchingLines = new List<Line>();
            List<Arc> matchingArcs = new List<Arc>();

            foreach (var project in projects)
            {
                List<Line> linesTemp = _databaseContext.Line.Where(x => x.ProjectId == project.Id).ToList();
                List<Arc> arcsTemp = _databaseContext.Arc.Where(x => x.ProjectId == project.Id).ToList();
                var itemsLine = (from x in lines
                                 join y in linesTemp
                                 on new
                                 { x.Handle, x.DX, x.DY, x.DZ, x.Magnitude }
                                 equals new
                                 { y.Handle, y.DX, y.DY, y.DZ, y.Magnitude }
                                 select x)
                                 .ToList();

                var itemsArc = (from x in arcs
                                join y in arcsTemp
                                on new
                                { x.Handle, x.DX, x.DY, x.DZ, x.Radius, x.AngleStart, x.AngleEnd }
                                equals new
                                { y.Handle, y.DX, y.DY, y.DZ, y.Radius, y.AngleStart, y.AngleEnd }
                                select x)
                                .ToList();

                matchingLines.AddRange(itemsLine);
                matchingArcs.AddRange(itemsArc);
            }
            double scoreIdentity = matchingLines.Count() + matchingArcs.Count();

            projectData.ScoreIdentity = scoreIdentity;
            _databaseContext.Entry(projectData).State = EntityState.Modified;
            //_databaseContext.Match.AddRange(matchesLine);
            await _databaseContext.SaveChangesAsync();
        }
    }
}


