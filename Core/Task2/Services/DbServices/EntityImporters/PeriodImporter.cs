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
    public class PeriodImporter : IImporterEntity
    {
        private Period period;
        public PeriodImporter(Period period)
        {
            this.period = period;
        }

        public long Import()
        {
            using (var connection = new NpgsqlConnection(DbProperties.ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO tb.periods (start_date, end_date, file_name_id) VALUES (@startDate, @endDate, @fileNameId) RETURNING id";
                    command.Parameters.AddWithValue("startDate", period.StartDate);
                    command.Parameters.AddWithValue("endDate", period.EndDate);
                    command.Parameters.AddWithValue("filenameId", period.FileName.Id);

                    return (long)command.ExecuteScalar();
                }
            }
        }
    }
}
