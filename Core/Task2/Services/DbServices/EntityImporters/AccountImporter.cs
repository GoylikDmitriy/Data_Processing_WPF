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
    public class AccountImporter : IImporterEntity
    {
        public Account account;
        public AccountImporter(Account account)
        {
            this.account = account;
        }

        public long Import()
        {
            using (var connection = new NpgsqlConnection(DbProperties.ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO tb.accounts (number, account_class_id) VALUES (@number, @accountClassId) RETURNING id";
                    command.Parameters.AddWithValue("number", account.Number);
                    command.Parameters.AddWithValue("accountClassId", account.AccountClass.Id);

                    return (long)command.ExecuteScalar();
                }
            }
        }
    }
}
