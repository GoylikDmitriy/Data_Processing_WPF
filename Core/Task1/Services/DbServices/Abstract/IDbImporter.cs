using Core.Task2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task1.Services.DbServices.Abstract
{
    public interface IDbImporter
    {
        int TotalRecords { get; }
        Task ImportDataAsync(string filePath, IProgress<int> progress, CancellationToken cancellationToken);
        Task<FileName> ImportTbsAsync(string filePath);
    }
}
