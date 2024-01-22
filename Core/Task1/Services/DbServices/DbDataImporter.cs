using Core.Task1.Services.DbServices.Exceptions;
using Core.Task1.Model;
using Core.Task1.Services.DbServices.Abstract;
using Core.Task1.Utilities.Mappers.Abstract;
using Npgsql;
using PostgreSQLCopyHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Task1.Services.DbServices.Properties;

namespace Core.Task1.Services.DbServices
{
    public class DbDataImporter : IDbDataImporter
    {
        private readonly IDataMapper mapper;

        private const int BatchSize = 10_000;

        private int totalRecords = 0;
        public int TotalRecords { get => totalRecords; }

        public DbDataImporter(IDataMapper mapper)
        {
            this.mapper = mapper;
        }

        public async Task ImportAsync(string filePath, IProgress<int> progress, CancellationToken cancellationToken)
        {
            int importedLines = 0;
            progress.Report(importedLines);
            await Task.Run(async () =>
            {
                try
                {
                    var lines = ReadLinesFromFile(filePath);
                    List<Data> batchData = new List<Data>(BatchSize);
                    foreach (var line in lines)
                    {
                        var data = mapper.Map(line);
                        batchData.Add(data);
                        if (batchData.Count >= BatchSize)
                        {
                            await ImportBatchDataAsync(batchData, cancellationToken);
                            ProgressReport(progress, ref importedLines, batchData.Count);
                            batchData.Clear();
                        }
                    }

                    if (batchData.Count > 0)
                    {
                        await ImportBatchDataAsync(batchData, cancellationToken);
                        ProgressReport(progress, ref importedLines, batchData.Count);
                    }
                }
                catch (IOException ex)
                {
                    throw new DbImporterException($"Error occurred while reading file: '{filePath}'.\n{ex.Message}", ex);
                }
                catch (OperationCanceledException ex)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new DbImporterException(ex.Message, ex);
                }
            });
        }

        private async Task ImportBatchDataAsync(IEnumerable<Data> data, CancellationToken cancellationToken)
        {
            using (var connection = new NpgsqlConnection(DbProperties.ConnectionString))
            {
                connection.Open();
                using (var writer = connection.BeginBinaryImport("COPY data (date, latin_chars, russian_chars, positive_even_number, positive_double_number) FROM STDIN (FORMAT BINARY)"))
                {
                    foreach (var item in data)
                    {
                        writer.StartRow();
                        writer.Write(item.Date, NpgsqlTypes.NpgsqlDbType.Date);
                        writer.Write(item.LatinChars, NpgsqlTypes.NpgsqlDbType.Text);
                        writer.Write(item.RussianChars, NpgsqlTypes.NpgsqlDbType.Text);
                        writer.Write(item.PositiveEvenNumber, NpgsqlTypes.NpgsqlDbType.Integer);
                        writer.Write(item.PositiveDoubleNumber, NpgsqlTypes.NpgsqlDbType.Double);
                    }

                    await writer.CompleteAsync(cancellationToken);
                }
            }
        }

        private IEnumerable<string> ReadLinesFromFile(string path)
        {
            var lines = File.ReadLines(path);
            totalRecords = lines.Count();
            return lines;
        }

        private void ProgressReport(IProgress<int> progress, ref int importedLines, int importedCount)
        {
            importedLines += importedCount;
            progress.Report(importedLines);
        }
    }
}
