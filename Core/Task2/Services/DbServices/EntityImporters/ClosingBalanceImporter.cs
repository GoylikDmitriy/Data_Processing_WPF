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
    public class ClosingBalanceImporter : IImporterEntity
    {
        private ClosingBalance cb;
        public ClosingBalanceImporter(ClosingBalance closingBalance)
        {
            this.cb = closingBalance;
        }

        public long Import()
        {
            using (var connection = new NpgsqlConnection(DbProperties.ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO tb.closing_balances (assets, liabilities, account_id) VALUES (@assets, @liabilities, @accountId) RETURNING id";
                    command.Parameters.AddWithValue("assets", cb.Assets);
                    command.Parameters.AddWithValue("liabilities", cb.Liabilities);
                    command.Parameters.AddWithValue("accountId", cb.Account.Id);

                    return (long)command.ExecuteScalar();
                }
            }
        }
    }
}
