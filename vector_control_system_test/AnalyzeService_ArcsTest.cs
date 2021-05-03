using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using vector_control_system_api.Services.Analysis;
using System.Threading.Tasks;

namespace vector_control_system_test
{
    public class AnalyzeService_ArcsTest
    {
        private readonly AnalyzeService _analyzeService;

        public AnalyzeService_ArcsTest()
        {
            _analyzeService = new AnalyzeService();
        }

        [Theory]
        [InlineData(50, 100, 70, 150)]
        [InlineData(70, 150, 50, 170)]
        public async Task ArcCase1(double aStart, double aEnd, double bStart, double bEnd)
        {
            var analyzeService = _analyzeService.ArcCase1(aStart, aEnd, bStart, bEnd);
            Assert.True(analyzeService);
        }
        [Theory]
        [InlineData(50, 100, 300, 70)]
        [InlineData(10, 150, 200, 20)]
        public async Task ArcCase2(double aStart, double aEnd, double bStart, double bEnd)
        {
            var analyzeService = _analyzeService.ArcCase2(aStart, aEnd, bStart, bEnd);
            Assert.True(analyzeService);
        }
        [Theory]
        [InlineData(300, 70, 250, 320)]
        [InlineData(200, 100, 150, 200)]
        public async Task ArcCase3(double aStart, double aEnd, double bStart, double bEnd)
        {
            var analyzeService = _analyzeService.ArcCase3(aStart, aEnd, bStart, bEnd);
            Assert.True(analyzeService);
        }

        [Theory]
        [InlineData(300, 100, 250, 50)]
        [InlineData(333, 111, 100, 10)]
        public async Task ArcCase4(double aStart, double aEnd, double bStart, double bEnd)
        {
            var analyzeService = _analyzeService.ArcCase4(aStart, aEnd, bStart, bEnd);
            Assert.True(analyzeService);
        }

        [Theory]
        [InlineData(50, 100, 70, 150)]
        [InlineData(50, 100, 300, 70)]
        [InlineData(300, 70, 250, 320)]
        [InlineData(300, 100, 250, 50)]
        public async Task ArcsInclude_True(double aStart, double aEnd, double bStart, double bEnd)
        {
            var analyzeService = _analyzeService.ArcsInclude(aStart, aEnd, bStart, bEnd);
            Assert.True(analyzeService);
        }

        [Theory]
        [InlineData(50, 100, 110, 150)]
        [InlineData(50, 100, 300, 40)]
        [InlineData(300, 70, 200, 250)]
        [InlineData(10, 180, 190, 350)]
        public async Task ArcsInclude_False(double aStart, double aEnd, double bStart, double bEnd)
        {
            var analyzeService = _analyzeService.ArcsInclude(aStart, aEnd, bStart, bEnd);
            Assert.False(analyzeService);
        }

        [Theory]
        [InlineData(50, 150, 50, 100, 70, 150)]
        [InlineData(300, 100, 50, 100, 300, 70)]
        [InlineData(250, 70, 300, 70, 250, 320)]
        [InlineData(250, 100, 300, 100, 250, 50)]
        public async Task NewAngles(double newStart, double newEnd, double aStart, double aEnd, double bStart, double bEnd)
        {
            List<double> newAngles = new List<double> { newStart, newEnd };

            var analyzeService = _analyzeService.NewAngles(aStart, aEnd, bStart, bEnd);
            Assert.Equal(newAngles, analyzeService);
        }
    }
}
