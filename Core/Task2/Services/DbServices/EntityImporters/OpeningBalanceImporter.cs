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
    public class OpeningBalanceImporter : IImporterEntity
    {
        public OpeningBalance ob;
        public OpeningBalanceImporter(OpeningBalance ob)
        {
            this.ob = ob;
        }

        public long Import()
        {
            using (var connection = new NpgsqlConnection(DbProperties.ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO tb.opening_balances (assets, liabilities, account_id) VALUES (@assets, @liabilities, @accountId) RETURNING id";
                    command.Parameters.AddWithValue("assets", ob.Assets);
                    command.Parameters.AddWithValue("liabilities", ob.Liabilities);
                    command.Parameters.AddWithValue("accountId", ob.Account.Id);

                    return (long)command.ExecuteScalar();
                }
            }
        }
    }
}
