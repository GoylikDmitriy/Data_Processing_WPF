using Core.Task1.Services.DbServices.Abstract;
using Core.Task2.Model;
using Core.Task2.Services.DbServices.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task1.Services.DbServices
{
    public class DbImporter : IDbImporter
    {
        private IDbDataImporter dataImporter;
        private IDbTbsImporter tbsImporter;

        public DbImporter(IDbDataImporter dataImporter, IDbTbsImporter tbsImporter)
        {
            this.dataImporter = dataImporter;
            this.tbsImporter = tbsImporter;
        }

        public int TotalRecords { get => dataImporter.TotalRecords; }

        public async Task ImportDataAsync(string filePath, IProgress<int> progress, CancellationToken cancellationToken) => await dataImporter.ImportAsync(filePath, progress, cancellationToken);

        public async Task<FileName> ImportTbsAsync(string filePath) => await tbsImporter.ImportAsync(filePath);
    }
}
