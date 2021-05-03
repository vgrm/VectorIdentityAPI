using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vector_control_system_api.Services.Analysis;
using Xunit;

namespace vector_control_system_test
{
    public class AnalyzeService_LinesTest
    {
        private readonly AnalyzeService _analyzeService;

        public AnalyzeService_LinesTest()
        {
            _analyzeService = new AnalyzeService();
        }

        [Theory]
        [InlineData(0, 0, 0, 5, 0, 0,
                    0, 0, 5, 10, 0, 0)]
        [InlineData(0, 5, 0, 5, 0, 0,
                    5, 10, 5, 10, 0, 0)]
        [InlineData(0, 5, 0, 0, 0, 0,
                    5, 10, 0, 0, 0, 0)]
        public async Task LinesInclude_True(double aX1, double aX2, double aY1, double aY2, double aZ1, double aZ2,
            double bX1, double bX2, double bY1, double bY2, double bZ1, double bZ2)
        {
            var analyzeService = _analyzeService.LinesInclude(aX1, aX2, aY1, aY2, aZ1, aZ2, bX1, bX2, bY1, bY2, bZ1, bZ2);
            Assert.True(analyzeService);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(10, 0, 10)]
        [InlineData(30, 20, 50)]
        [InlineData(50, -20, 30)]
        public async Task CalculateDistance(double distance, double x1, double x2)
        {
            var analyzeService = _analyzeService.CalculateDistance(x1, x2);
            Assert.Equal(distance, analyzeService);
        }

        [Theory]
        [InlineData(5, 5, 0, 0)]
        [InlineData(5, 0, 5, 0)]
        [InlineData(5, 0, 0, 5)]
        public async Task CalculateMagnitude(double magnitude, double V1, double V2, double V3)
        {
            var analyzeService = _analyzeService.CalculateMagnitude(V1, V2, V3);
            Assert.Equal(magnitude, analyzeService);
        }

        [Theory]
        [InlineData(0.5, 2, 4)]
        [InlineData(1, 5, 5)]
        [InlineData(-1, 5, -5)]
        public async Task CalculateDirection(double direction, double V1, double Magnitude)
        {
            var analyzeService = _analyzeService.CalculateDirection(V1, Magnitude);
            Assert.Equal(direction, analyzeService);
        }

        [Theory]
        [InlineData(0, 10, 0, 5, 5, 10)]
        [InlineData(-10, 10, 0, -10, -5, 10)]
        public async Task LinesUnion(double newStart, double newEnd, double aX1, double aX2, double bX1, double bX2)
        {
            List<double> newCoords = new List<double> { newStart, newEnd };
            List<double> coords = new List<double> { newStart, newEnd };

            var analyzeService = _analyzeService.Union(coords);
            Assert.Equal(newCoords, analyzeService);
        }
    }
}
