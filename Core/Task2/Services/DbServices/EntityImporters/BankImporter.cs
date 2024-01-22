using Core.Task1.Services.DbServices.Properties;
using Core.Task2.Model;
using Core.Task2.Model.Abstract;
using Core.Task2.Services.DbServices.Abstract;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task2.Services.DbServices.EntityImporters
{
    public class BankImporter : IImporterEntity
    {
        private Bank bank;
        public BankImporter(Bank bank)
        {
            this.bank = bank;
        }

        public long Import()
        {
            using (var connection = new NpgsqlConnection(DbProperties.ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO tb.banks (name, file_name_id) VALUES (@name, @fileNameId) RETURNING id";
                    command.Parameters.AddWithValue("name", bank.Name);
                    command.Parameters.AddWithValue("fileNameId", bank.FileName.Id);

                    return (long)command.ExecuteScalar();
                }
            }
        }
    }
}
