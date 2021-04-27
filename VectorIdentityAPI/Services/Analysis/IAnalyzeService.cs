using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using VectorIdentityAPI.Database;

namespace VectorIdentityAPI.Services.Analysis
{
    public interface IAnalyzeService
    {
        Task Analyze(ProjectData projectData, CancellationToken cancellationToken = default);
    }
}
