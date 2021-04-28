using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using vector_control_system_api.Database;

namespace vector_control_system_api.Services.Analysis
{
    public interface IAnalyzeService
    {
        Task Analyze(ProjectData projectData, CancellationToken cancellationToken = default);
    }
}
