using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task1.Services.DbServices.Abstract
{
    public interface IDbDataImporter
    {
        int TotalRecords { get; }
        Task ImportAsync(string filePath, IProgress<int> progress, CancellationToken cancellationToken);
    }
}
