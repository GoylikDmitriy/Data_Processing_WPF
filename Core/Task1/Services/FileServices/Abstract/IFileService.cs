using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task1.Services.FileServices.Abstract
{
    public interface IFileService
    {
        Task CreateFilesAsync(string path, int numberOfFiles, int numberOfLines);
        Task<int> MergeFilesAsync(string path, string patternToRemove);
    }
}
