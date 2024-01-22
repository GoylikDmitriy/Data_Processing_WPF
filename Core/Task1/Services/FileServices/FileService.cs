using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Task1.Services.FileServices.Abstract;

namespace Core.Task1.Services.FileServices
{
    public class FileService : IFileService
    {
        private readonly IFileGeneratorService fileGeneratorService;
        private readonly IFileMergeService fileMergeService;

        public FileService(IFileGeneratorService fileGeneratorService, IFileMergeService fileMergeService)
        {
            this.fileGeneratorService = fileGeneratorService;
            this.fileMergeService = fileMergeService;
        }

        public async Task CreateFilesAsync(string path, int numberOfFiles, int numberOfLines)
        {
            await fileGeneratorService.CreateFilesAsync(path, numberOfFiles, numberOfLines);
        }

        public async Task<int> MergeFilesAsync(string path, string patternToRemove)
        {
            await fileMergeService.MergeFilesAsync(path, patternToRemove);
            return fileMergeService.RemovedLinesCount;
        }
    }
}
