using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task1.Services.FileServices.Abstract
{
    public interface IFileMergeService
    {
        int RemovedLinesCount { get; }
        Task MergeFilesAsync(string path, string patternToRemove);
    }
}
