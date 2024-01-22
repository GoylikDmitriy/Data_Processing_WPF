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
    public class TransactionImporter : IImporterEntity
    {
        private Transaction transaction;
        public TransactionImporter(Transaction transaction)
        {
            this.transaction = transaction;
        }

        public long Import()
        {
            using (var connection = new NpgsqlConnection(DbProperties.ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO tb.transactions (credit, debit, account_id) VALUES (@credit, @debit, @accountId) RETURNING id";
                    command.Parameters.AddWithValue("credit", transaction.Credit);
                    command.Parameters.AddWithValue("debit", transaction.Debit);
                    command.Parameters.AddWithValue("accountId", transaction.Account.Id);

                    return (long)command.ExecuteScalar();
                }
            }
        }
    }
}
