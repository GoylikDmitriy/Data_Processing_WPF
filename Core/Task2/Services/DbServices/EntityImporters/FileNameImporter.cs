using Core.Task1.Services.DbServices.Properties;
using Core.Task2.Model;
using Core.Task2.Services.DbServices.Abstract;
using Npgsql;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task2.Services.DbServices.EntityImporters
{
    public class FileNameImporter : IImporterEntity
    {
        private FileName fileName;
        public FileNameImporter(FileName fileName)
        {
            this.fileName = fileName;
        }

        public long Import()
        {
            using (var connection = new NpgsqlConnection(DbProperties.ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO tb.file_names (name) VALUES (@name) RETURNING id";
                    command.Parameters.AddWithValue("name", fileName.Name);

                    return (long)command.ExecuteScalar();
                }
            }
        }
    }
}
