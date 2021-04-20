using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using VectorIdentityAPI.Database;
using Microsoft.EntityFrameworkCore;

namespace VectorIdentityAPI.Services
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

            if (projectData.Status == "Accepted" || projectData.Status == "Processing")
            {
                //analyze file

                //change status
                projectData.Status = "Processing";
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

                    for (int iWaarden = iLine; iWaarden < iLine + 10; iWaarden++)
                    {
                        if (D[iWaarden] == " 10" && D[iWaarden + 2] == " 20")
                        {
                            //Here you can store the following data in a list for later use
                            //LayerName = Layer
                            //Xbegin = D[iWaarden + 1]
                            //Ybegin = D[iWaarden + 3]
                            //Zbegin = D[iWaarden + 5]
                            //Xend = D[iWaarden + 7]
                            //Yend = D[iWaarden + 9]
                            //Zend = D[iWaarden + 11]

                            double X1 = Convert.ToDouble(D[iWaarden + 1].PadRight(17, '0').Substring(0, D[iWaarden + 1].Length - 1));
                            double Y1 = Convert.ToDouble(D[iWaarden + 3].PadRight(17, '0').Substring(0, D[iWaarden + 1].Length - 1));
                            double Z1 = Convert.ToDouble(D[iWaarden + 5].PadRight(17, '0').Substring(0, D[iWaarden + 1].Length - 1));
                            double X2 = Convert.ToDouble(D[iWaarden + 7].PadRight(17, '0').Substring(0, D[iWaarden + 1].Length - 1));
                            double Y2 = Convert.ToDouble(D[iWaarden + 9].PadRight(17, '0').Substring(0, D[iWaarden + 1].Length - 1));
                            double Z2 = Convert.ToDouble(D[iWaarden + 11].PadRight(17, '0').Substring(0, D[iWaarden + 1].Length - 1));

                            double V1 = X1 - X2;
                            double V2 = Y1 - Y2;
                            double V3 = Z1 - Z2;

                            double Magnitude = Math.Sqrt(Math.Pow(V1, 2) + Math.Pow(V2, 2) + Math.Pow(V3, 2));
                            double DX = V1 / Magnitude;
                            double DY = V2 / Magnitude;
                            double DZ = V3 / Magnitude;

                            //string.format(new FormatProvider(), "{0:T(1)0,000.0", 1000.9999); // 1,000.9
                            var number = 10000000000000;
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

                    for (int ii = iCircle; ii < iCircle + 10; ii++)
                    {
                        if (D[ii] == " 10" && D[ii + 2] == " 20" && D[ii + 4] == " 30" && D[ii + 6] == " 40")
                        {

                            X = Convert.ToDouble(D[ii + 1]);
                            Y = Convert.ToDouble(D[ii + 3]);
                            Z = Convert.ToDouble(D[ii + 5]);
                            Radius = Convert.ToDouble(D[ii + 7]);
                        }

                        if (D[ii + 8] == "210" && D[ii + 10] == "220" && D[ii + 12] == "230")
                        {
                            DX = Convert.ToDouble(D[ii + 9]);
                            DY = Convert.ToDouble(D[ii + 10]);
                            DZ = Convert.ToDouble(D[ii + 11]);
                            Radius = Convert.ToDouble(D[ii + 7]);
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

            foreach (var line in lines)
            {
                foreach (var lineTest in lines)
                {
                    if (((line.DX == lineTest.DX && line.DY == lineTest.DY && line.DZ == lineTest.DZ) ||
                        (line.DX == -lineTest.DX && line.DY == -lineTest.DY && line.DZ == -lineTest.DZ)) &&
                        (line.Handle != lineTest.Handle))
                    {
                        _logger.LogInformation("{handleA} A\"{x1a} {y1a} {z1a}\" \"{x2a} {y2a} {z2a}\" \n {handleB} B\"{x1b} {y1b} {z1b}\" \"{x2b} {y2b} {z2b}\""
                            , line.Handle, line.X1, line.Y1, line.Z1, line.X2, line.Y2, line.Z2
                            , lineTest.Handle, lineTest.X1, lineTest.Y1, lineTest.Z1, lineTest.X2, lineTest.Y2, lineTest.Z2);

                        _logger.LogInformation("A\"{x1a} {y1a} {z1a}\"  \n B\"{x1b} {y1b} {z1b}\""
                            , line.DX, line.DY, line.DZ
                            , lineTest.DX, lineTest.DY, lineTest.DZ);
                    }
                }
            }
            //UPDATE DB
            _databaseContext.Line.AddRange(lines);
            _databaseContext.Arc.AddRange(arcs);
            await _databaseContext.SaveChangesAsync();
        }
    }
}
