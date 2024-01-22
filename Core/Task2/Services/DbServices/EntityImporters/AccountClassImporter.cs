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
    public class AccountClassImporter : IImporterEntity
    {
        private AccountClass accountClass;
        public AccountClassImporter(AccountClass accountClass)
        {
            this.accountClass = accountClass;
        }

        public long Import()
        {
            using (var connection = new NpgsqlConnection(DbProperties.ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO tb.account_classes (number, description, bank_id, period_id) VALUES (@number, @description, @bankId, @periodId) RETURNING id";
                    command.Parameters.AddWithValue("number", accountClass.Number);
                    command.Parameters.AddWithValue("description", accountClass.Description);
                    command.Parameters.AddWithValue("bankId", accountClass.Bank.Id);
                    command.Parameters.AddWithValue("periodId", accountClass.Period.Id);

                    return (long)command.ExecuteScalar();
                }
            }
        }
    }
}
