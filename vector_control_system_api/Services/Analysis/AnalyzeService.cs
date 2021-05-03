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
        const double min = -0.00000000000019;
        const double max = 0.00000000000019;

        public AnalyzeService(DatabaseContext databaseContext, ILogger<AnalyzeService> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        public AnalyzeService()
        {

        }

        public async Task Analyze(ProjectData projectData, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Doing heavy analyzer logic ...");

            if (projectData.Status == "Accepted" || projectData.Status == "New"
                || projectData.Status == "Evaluated"
                )
            {
                //analyze file

                //change status
                projectData.Status = "Processing";
                projectData.DateUpdated = DateTime.UtcNow;
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

            if (!projectData.Original)
            {
                await CalculateCorrectnessScore(projectData);
                await CalculateIdentityScore(projectData);
            }

            projectData.Status = "Evaluated";
            _databaseContext.Entry(projectData).State = EntityState.Modified;
            await _databaseContext.SaveChangesAsync();

            await Task.Delay(250, cancellationToken);
            _logger.LogInformation("\"{Name} by {Owner}\" has been published!", projectData.Name, projectData.OwnerId);


        }

        public async Task UpdateData(ProjectData projectData)
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
                /* POINT CALCULATIONS FUTURE IMPLEMENTATION
                if (D[i] == "POINT" || D[i] == "AcDbPoint")
                {
                    int iEntity = i; if (D[i].StartsWith("AcDb")) { for (iEntity = i; D[iEntity] != "AcDbEntity"; iEntity--) ; }
                    Layer = ""; for (int iLayer = iEntity; iLayer < i + 10 && Layer == ""; iLayer++) { if (D[iLayer] == "  8") { Layer = D[iLayer + 1]; }; }
                    for (int iWaarden = i; iWaarden < i + 8; iWaarden++)
                    {
                        if (D[iWaarden] == " 10" && D[iWaarden + 2] == " 20")
                        {
                            //Here you can store the following data in a list for later use
                        }
                    }
                }
                */
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

                    for (iEntity = i; D[iEntity] != "AcDbEntity" && iEntity < i + 100; iEntity++) ;
                    for (iLine = i; D[iLine] != "AcDbLine" && iLine < i + 100; iLine++) ;

                    //find layer name
                    Layer = "";
                    for (int iLayer = iEntity; iLayer < iEntity + 10 && Layer == ""; iLayer++)
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
                            var number = 1000000000000;
                            //raw as accurate as we can get from dxf
                            double tempX1 = Convert.ToDouble(D[ii + 1]);
                            double tempY1 = Convert.ToDouble(D[ii + 3]);
                            double tempZ1 = Convert.ToDouble(D[ii + 5]);
                            double tempX2 = Convert.ToDouble(D[ii + 7]);
                            double tempY2 = Convert.ToDouble(D[ii + 9]);
                            double tempZ2 = Convert.ToDouble(D[ii + 11]);

                            //lower accuracy to minimize all the floating points
                            X1 = Convert.ToDouble(string.Format("{0:0.############}", Math.Truncate(tempX1 * number) / number));
                            Y1 = Convert.ToDouble(string.Format("{0:0.############}", Math.Truncate(tempY1 * number) / number));
                            Z1 = Convert.ToDouble(string.Format("{0:0.############}", Math.Truncate(tempZ1 * number) / number));
                            X2 = Convert.ToDouble(string.Format("{0:0.############}", Math.Truncate(tempX2 * number) / number));
                            Y2 = Convert.ToDouble(string.Format("{0:0.############}", Math.Truncate(tempY2 * number) / number));
                            Z2 = Convert.ToDouble(string.Format("{0:0.############}", Math.Truncate(tempZ2 * number) / number));

                            double V1 = tempX1 - tempX2;
                            double V2 = tempY1 - tempY2;
                            double V3 = tempZ1 - tempZ2;

                            Magnitude = Math.Sqrt(Math.Pow(V1, 2) + Math.Pow(V2, 2) + Math.Pow(V3, 2));
                            DX = V1 / Magnitude;
                            DY = V2 / Magnitude;
                            DZ = V3 / Magnitude;

                            //string.format(new FormatProvider(), "{0:T(1)0,000.0", 1000.9999); // 1,000.9

                            DX = Convert.ToDouble(string.Format("{0:0.############}", Math.Truncate(DX * number) / number));
                            DY = Convert.ToDouble(string.Format("{0:0.############}", Math.Truncate(DY * number) / number));
                            DZ = Convert.ToDouble(string.Format("{0:0.############}", Math.Truncate(DZ * number) / number));

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
                    var number = 1000000000000;
                    for (int ii = iCircle; ii < iCircle + 10; ii++)
                    {
                        if (D[ii] == " 10" && D[ii + 2] == " 20" && D[ii + 4] == " 30" && D[ii + 6] == " 40")
                        {

                            X = Convert.ToDouble(string.Format("{0:0.############}", Math.Truncate((Convert.ToDouble(D[ii + 1]) * number)) / number));
                            Y = Convert.ToDouble(string.Format("{0:0.############}", Math.Truncate((Convert.ToDouble(D[ii + 3]) * number)) / number));
                            Z = Convert.ToDouble(string.Format("{0:0.############}", Math.Truncate((Convert.ToDouble(D[ii + 5]) * number)) / number));
                            Radius = Convert.ToDouble(string.Format("{0:0.############}", Math.Truncate((Convert.ToDouble(D[ii + 7]) * number)) / number));

                            //X = Convert.ToDouble(D[ii + 1]);
                            //Y = Convert.ToDouble(D[ii + 3]);
                            //Z = Convert.ToDouble(D[ii + 5]);
                            //Radius = Convert.ToDouble(D[ii + 7]);
                        }

                        if (D[ii + 8] == "210" && D[ii + 10] == "220" && D[ii + 12] == "230")
                        {

                            DX = Convert.ToDouble(string.Format("{0:0.############}", Math.Truncate((Convert.ToDouble(D[ii + 9]) * number)) / number));
                            DY = Convert.ToDouble(string.Format("{0:0.############}", Math.Truncate((Convert.ToDouble(D[ii + 11]) * number)) / number));
                            DZ = Convert.ToDouble(string.Format("{0:0.############}", Math.Truncate((Convert.ToDouble(D[ii + 13]) * number)) / number));
                            Radius = Convert.ToDouble(string.Format("{0:0.############}", Math.Truncate((Convert.ToDouble(D[ii + 7]) * number)) / number));
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
                                AngleStart = Convert.ToDouble(string.Format("{0:0.############}", Math.Truncate((Convert.ToDouble(D[ii + 1]) * number)) / number));
                                AngleEnd = Convert.ToDouble(string.Format("{0:0.############}", Math.Truncate((Convert.ToDouble(D[ii + 3]) * number)) / number));
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

        public double CalculateDistance(double x1,double x2)
        {
            double V1 = x1 - x2;
            return Math.Abs(V1);
        }

        public double CalculateMagnitude(double V1, double V2, double V3)
        {
            double Magnitude = Math.Sqrt(Math.Pow(V1, 2) + Math.Pow(V2, 2) + Math.Pow(V3, 2));
            return Magnitude;
        }

        public double CalculateDirection(double V1, double Magnitude)
        {
            double DX = V1 / Magnitude;
            return DX;
        }

        public async Task MinimizeData(List<Line> linesAll, List<Arc> arcsAll)
        {
            bool minimizedLines = false;
            bool minimizedArcs = false;
            //for now filter only main lines
            List<Line> lines = linesAll.Where(x => x.Layer == "Pagrindinis").ToList();
            List<Arc> arcs = arcsAll.Where(x => x.Layer == "Pagrindinis").ToList();

            //List<Line> lines = linesAll.ToList();
            //List<Arc> arcs = arcsAll.ToList();

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

        public List<Arc> MinimizeArcs(List<Arc> arcs)
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
                    if (arc.Handle != arcTest.Handle && ArcsSameCircle(arc, arcTest))
                    {
                        if (ArcsInclude(arc.AngleStart, arc.AngleEnd, arcTest.AngleStart, arcTest.AngleEnd))
                        {
                            wasMatch = true;
                            List<double> angles = NewAngles(arc.AngleStart, arc.AngleEnd, arcTest.AngleStart, arcTest.AngleEnd);

                            Arc newArcTemmp = new Arc
                            {
                                ProjectId = arc.ProjectId,
                                Handle = arc.Handle,
                                Layer = arc.Layer,
                                X = arc.X,
                                Y = arc.Y,
                                Z = arc.Z,

                                Radius = arc.Radius,
                                AngleStart = angles.FirstOrDefault(),
                                AngleEnd = angles.LastOrDefault(),

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

        public bool ArcsSameCircle(Arc arc1, Arc arc2)
        {
            //find arcs with same R coords and plane
            if (arc1.DX - arc2.DX is >= min and <= max &&
                 arc1.DY - arc2.DY is >= min and <= max &&
                 arc1.DZ - arc2.DZ is >= min and <= max &&
                 arc1.X - arc2.X is >= min and <= max &&
                 arc1.Y - arc2.Y is >= min and <= max &&
                 arc1.Z - arc2.Z is >= min and <= max &&
                 arc1.Radius - arc2.Radius is >= min and <= max)
            {
                return true;
            }
                return false;
        }
        public bool ArcsInclude(double aStart, double aEnd, double bStart, double bEnd)
        {
            if (ArcCase1(aStart, aEnd, bStart, bEnd))
            {
                if (DoesInclude(aStart, aEnd, bStart))
                {
                    return true;
                }
                else if (DoesInclude(aStart, aEnd, bEnd))
                {
                    return true;
                }
            }
            else if (ArcCase2(aStart, aEnd, bStart, bEnd))
            {
                if (DoesInclude(aStart, aEnd, bStart))
                {
                    return true;
                }
                else if (DoesInclude(aStart, aEnd, bEnd))
                {
                    return true;
                }
            }
            else if (ArcCase3(aStart, aEnd, bStart, bEnd))
            {
                if (DoesInclude(bStart, bEnd, aStart))
                {
                    return true;
                }
                else if (DoesInclude(bStart, bEnd, aEnd))
                {
                    return true;
                }
            }
            else if (ArcCase4(aStart, aEnd, bStart, bEnd))
            {
                return true;
            }

            return false;

        }
        public List<double> NewAngles(double aStart, double aEnd, double bStart, double bEnd)
        {
            List<double> start = Union(new List<double> { aStart, bStart });
            List<double> end = Union(new List<double> { aEnd, bEnd });
            List<double> newAngles = new List<double>();

            if (ArcCase1(aStart, aEnd, bStart, bEnd))
            {
                newAngles.Add(start.FirstOrDefault());
                newAngles.Add(end.LastOrDefault());
            }
            else if (ArcCase2(aStart, aEnd, bStart, bEnd))
            {
                newAngles.Add(start.LastOrDefault());
                newAngles.Add(end.LastOrDefault());
            }
            else if (ArcCase3(aStart, aEnd, bStart, bEnd))
            {
                newAngles.Add(start.FirstOrDefault());
                newAngles.Add(end.FirstOrDefault());
            }
            else if (ArcCase4(aStart, aEnd, bStart, bEnd))
            {
                newAngles.Add(start.FirstOrDefault());
                newAngles.Add(end.LastOrDefault());
            }
            return newAngles;
        }

        public bool ArcCase1(double aStart, double aEnd, double bStart, double bEnd)
        {
            bool pairA = false;
            bool pairB = false;

            if (aStart <= aEnd) pairA = true;
            if (bStart <= bEnd) pairB = true;

            if (pairA && pairB) return true;

            return false;
        }

        public bool ArcCase2(double aStart, double aEnd, double bStart, double bEnd)
        {
            bool pairA = false;
            bool pairB = false;

            if (aStart <= aEnd) pairA = true;
            if (bStart >= bEnd) pairB = true;

            if (pairA && pairB) return true;

            return false;
        }

        public bool ArcCase3(double aStart, double aEnd, double bStart, double bEnd)
        {
            bool pairA = false;
            bool pairB = false;

            if (aStart >= aEnd) pairA = true;
            if (bStart <= bEnd) pairB = true;

            if (pairA && pairB) return true;

            return false;
        }

        public bool ArcCase4(double aStart, double aEnd, double bStart, double bEnd)
        {
            bool pairA = false;
            bool pairB = false;

            if (aStart >= aEnd) pairA = true;
            if (bStart >= bEnd) pairB = true;

            if (pairA && pairB) return true;

            return false;
        }

        public List<Line> MinimizeLines(List<Line> lines)
        {
            Line line1 = new Line();
            Line line2 = new Line();
            Line newLine = new Line();
            var number = 1000000000000;
            bool wasMatch = false;

            foreach (var line in lines)
            {

                foreach (var lineTest in lines)
                {
                    //find lines with same or opposite direction
                    if (line.Handle != lineTest.Handle && LinesSameDirection(line,lineTest))
                    {
                        //check if lines include eachother
                        if (LinesInclude(line.X1,line.X2, line.Y1,line.Y2,line.Z1,line.Z2,
                            lineTest.X1, lineTest.X2, lineTest.Y1, lineTest.Y2, lineTest.Z1, lineTest.Z2))
                        {
                            wasMatch = true;

                            //get new end coords
                            List<double> X = Union(new List<double> { line.X1, line.X2, lineTest.X1, lineTest.X2 });
                            List<double> Y = Union(new List<double> { line.Y1, line.Y2, lineTest.Y1, lineTest.Y2 });
                            List<double> Z = Union(new List<double> { line.Z1, line.Z2, lineTest.Z1, lineTest.Z2 });

                            //calculate new params
                            double V1 = X.FirstOrDefault() - X.LastOrDefault();
                            double V2 = Y.FirstOrDefault() - Y.LastOrDefault();
                            double V3 = Z.FirstOrDefault() - Z.LastOrDefault();

                            double Magnitude = Math.Sqrt(Math.Pow(V1, 2) + Math.Pow(V2, 2) + Math.Pow(V3, 2));
                            double DX = V1 / Magnitude;
                            double DY = V2 / Magnitude;
                            double DZ = V3 / Magnitude;

                            //lower accuracy for floating points
                            DX = Convert.ToDouble(string.Format("{0:0.############}", Math.Truncate(DX * number) / number));
                            DY = Convert.ToDouble(string.Format("{0:0.############}", Math.Truncate(DY * number) / number));
                            DZ = Convert.ToDouble(string.Format("{0:0.############}", Math.Truncate(DZ * number) / number));

                            //create new line with new params
                            Line newLineTemp = new Line
                            {
                                ProjectId = line.ProjectId,
                                Handle = line.Handle,
                                Layer = line.Layer,
                                Correct = false,
                                X1 = X.FirstOrDefault(),
                                Y1 = Y.FirstOrDefault(),
                                Z1 = Z.FirstOrDefault(),
                                X2 = X.LastOrDefault(),
                                Y2 = Y.LastOrDefault(),
                                Z2 = Z.LastOrDefault(),
                                
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
        public bool LinesInclude(double aX1, double aX2, double aY1, double aY2, double aZ1, double aZ2, double bX1, double bX2, double bY1, double bY2, double bZ1, double bZ2)
        {
            if (DoesInclude(aX1, aX2, bX1) && DoesInclude(aY1, aY2, bY1) && DoesInclude(aZ1, aZ2, bZ1))
            {
                return true;
            }
            else if (DoesInclude(aX1, aX2, bX2) && DoesInclude(aY1, aY2, bY2) && DoesInclude(aZ1, aZ2, bZ2))
            {
                return true;
            }
            return false;
        }
        public bool LinesSameDirection(Line line1, Line line2)
        {
            if ((line1.DX - line2.DX is >= min and <= max &&
                    line1.DY - line2.DY is >= min and <= max &&
                    line1.DZ - line2.DZ is >= min and <= max)
                    ||
                   (line1.DX + line2.DX is >= min and <= max &&
                    line1.DY + line2.DY is >= min and <= max &&
                    line1.DZ + line2.DZ is >= min and <= max))
            {
                return true;
            }
                return false;
        }
        public bool DoesInclude(double a, double b, double c)
        {
            // a and b range
            if ((a <= c && c <= b) ||
                (a >= c && c >= b))
            {
                return true;
            }
            return false;
        }
        public List<double> Union(List<double> values)
        {
            List<double> union = new List<double>();
            values.Sort();
            union.Add(values.FirstOrDefault());
            union.Add(values.LastOrDefault());
            union.Sort();
            return union;
        }
        public async Task CalculateCorrectnessScore(ProjectData testProject)
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

            Offset offset = FindOffset(originalLines, testLines, originalArcs, testArcs);

            matchesLine = FindMatchingLines(offset, originalLines, testLines, originalProject.Id, testProject.Id);
            matchesArc = FindMatchingArcs(offset, originalArcs, testArcs, originalProject.Id, testProject.Id);


            double correctCount = matchesLine.Count * 2 + matchesArc.Count * 2;
            double allCount = testLines.Count + originalLines.Count + testArcs.Count + originalArcs.Count;
            double scoreCorrectness = correctCount / allCount;

            testProject.ScoreCorrectness = scoreCorrectness;
            testProject.OffsetX = offset.X;
            testProject.OffsetY = offset.Y;
            testProject.OffsetZ = offset.Z;
            _databaseContext.Entry(testProject).State = EntityState.Modified;
            await _databaseContext.SaveChangesAsync();
        }

        public Offset FindOffset(List<Line> originalLines, List<Line> testLines, List<Arc> originalArcs, List<Arc> testArcs)
        {
            List<Offset> offsetLines = FindOffsetLines(originalLines, testLines);
            List<Offset> offsetArcs = FindOffsetArcs(originalArcs, testArcs);

            Offset offset = FindCommonOffset(offsetLines, offsetArcs);

            return offset;
        }

        public List<Offset> FindOffsetLines(List<Line> originalLines, List<Line> testLines)
        {
            List<Offset> offsetLines = new List<Offset>();

            foreach (var originalLine in originalLines)
            {
                foreach (var testLine in testLines)
                {
                    if (
(originalLine.Magnitude - testLine.Magnitude is >= min and <= max &&
originalLine.DX - testLine.DX is >= min and <= max &&
originalLine.DY - testLine.DY is >= min and <= max &&
originalLine.DZ - testLine.DZ is >= min and <= max)
||
(testLine.Magnitude - originalLine.Magnitude is >= min and <= max &&
testLine.DX - originalLine.DX is >= min and <= max &&
testLine.DY - originalLine.DY is >= min and <= max &&
testLine.DZ - originalLine.DZ is >= min and <= max)
)
                    {
                        double tempOffsetX = testLine.X1 - originalLine.X1;
                        double tempOffsetY = testLine.Y1 - originalLine.Y1;
                        double tempOffsetZ = testLine.Z1 - originalLine.Z1;

                        offsetLines.Add(new Offset(tempOffsetX, tempOffsetY, tempOffsetZ));
                    }
                }
            }
            return offsetLines;
        }

        public List<Offset> FindOffsetArcs(List<Arc> originalArcs, List<Arc> testArcs)
        {
            List<Offset> offsetArcs = new List<Offset>();

            foreach (var originalArc in originalArcs)
            {
                foreach (var testArc in testArcs)
                {
                    if (originalArc.Radius == testArc.Radius &&
                       originalArc.DX == testArc.DX &&
                       originalArc.DY == testArc.DY &&
                       originalArc.DZ == testArc.DZ &&
                       //originalArc.X == testArc.X &&
                       //originalArc.Y == testArc.Y &&
                       //originalArc.Z == testArc.Z &&
                       originalArc.AngleStart == testArc.AngleStart &&
                       originalArc.AngleEnd == testArc.AngleEnd)
                    {
                        double tempOffsetX = testArc.X - originalArc.X;
                        double tempOffsetY = testArc.Y - originalArc.Y;
                        double tempOffsetZ = testArc.Z - originalArc.Z;

                        offsetArcs.Add(new Offset(tempOffsetX, tempOffsetY, tempOffsetZ));
                    }
                }
            }
            return offsetArcs;
        }

        public Offset FindCommonOffset(List<Offset> offsetLines, List<Offset> offsetArcs)
        {
            Offset offset = new Offset();
            List<Offset> offsetList = new List<Offset>();

            offsetList.AddRange(offsetLines);
            offsetList.AddRange(offsetArcs);

            var offsetGrouped = offsetList
                .GroupBy(x => new { x.X, x.Y, x.Z })
                .Select(x => new
                {
                    Count = x.Count(),
                    X = x.Key.X,
                    Y = x.Key.Y,
                    Z = x.Key.Z
                }).OrderByDescending(x => x.Count);

            var commonOffset = offsetGrouped.FirstOrDefault();

            if (commonOffset != null)
            {
                offset = new Offset(commonOffset.X, commonOffset.Y, commonOffset.Z);
            }
            else
            {
                offset = new Offset(0, 0, 0);
            }

            return offset;
        }
        public List<ProjectMatchModel> FindMatchingLines(Offset offset, List<Line> originalLines, List<Line> testLines, int originalProjectId, int testProjectId)
        {
            List<ProjectMatchModel> matches = new List<ProjectMatchModel>();

            foreach (var originalLine in originalLines)
            {
                foreach (var testLine in testLines)
                {
                    if (testLine.Handle == "245")
                    {
                        int x = 0;
                        double a = originalLine.X1 - testLine.X1 + offset.X;
                        double b = originalLine.X1;
                        double c = testLine.X1 - offset.X;

                        double VALUE_1 = originalLine.Magnitude - testLine.Magnitude;
                        double VALUE_2 = originalLine.DX - testLine.DX;
                        double VALUE_3 = originalLine.DY - testLine.DY;
                        double VALUE_4 = originalLine.DZ - testLine.DZ;
                        double VALUE_5 = originalLine.X1 - testLine.X1 - offset.X;
                        double VALUE_6 = originalLine.Y1 - testLine.Y1 - offset.Y;
                        double VALUE_7 = originalLine.Z1 - testLine.Z1 - offset.Z;

                        double VALUE2_1 = testLine.Magnitude - originalLine.Magnitude;
                        double VALUE2_2 = testLine.DX - originalLine.DX;
                        double VALUE2_3 = testLine.DY - originalLine.DY;
                        double VALUE2_4 = testLine.DZ - originalLine.DZ;
                        double VALUE2_5 = testLine.X1 - originalLine.X1 - offset.X;
                        double VALUE2_6 = testLine.Y1 - originalLine.Y1 - offset.Y;
                        double VALUE2_7 = testLine.Z1 - originalLine.Z1 - offset.Z;
                    }

                    if (
                    (originalLine.Magnitude - testLine.Magnitude is >= min and <= max &&
                    originalLine.DX - testLine.DX is >= min and <= max &&
                    originalLine.DY - testLine.DY is >= min and <= max &&
                    originalLine.DZ - testLine.DZ is >= min and <= max &&
                    originalLine.X1 - testLine.X1 - offset.X is >= min and <= max &&
                    originalLine.Y1 - testLine.Y1 - offset.Y is >= min and <= max &&
                    originalLine.Z1 - testLine.Z1 - offset.Z is >= min and <= max)
                    ||
                   (testLine.Magnitude - originalLine.Magnitude is >= min and <= max &&
                    testLine.DX - originalLine.DX is >= min and <= max &&
                    testLine.DY - originalLine.DY is >= min and <= max &&
                    testLine.DZ - originalLine.DZ is >= min and <= max &&
                    testLine.X1 - originalLine.X1 - offset.X is >= min and <= max &&
                    testLine.Y1 - originalLine.Y1 - offset.Y is >= min and <= max &&
                    testLine.Z1 - originalLine.Z1 - offset.Z is >= min and <= max)
                    )
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
                        };
                        matches.Add(match);
                    }
                }
            }

            return matches;
        }

        public List<ProjectMatchModel> FindMatchingArcs(Offset offset, List<Arc> originalArcs, List<Arc> testArcs, int originalProjectId, int testProjectId)
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
                       originalArc.X == testArc.X - offset.X &&
                       originalArc.Y == testArc.Y - offset.Y &&
                       originalArc.Z == testArc.Z - offset.Z &&
                       originalArc.AngleStart == testArc.AngleStart &&
                       originalArc.AngleEnd == testArc.AngleEnd)


                    {
                        testArc.Correct = true;
                        _databaseContext.Entry(testArc).State = EntityState.Modified;

                        ProjectMatchModel match = new ProjectMatchModel
                        {
                            Name = "name",
                            Info = "info",
                            Type = "arc match",
                            LineOriginalId = 0,
                            LineTestId = 0,

                            ArcOriginalId = 0,
                            ArcTestId = 0,

                        };
                        matches.Add(match);
                    }
                }
            }

            return matches;
        }

        public async Task CalculateIdentityScore(ProjectData projectData)
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


