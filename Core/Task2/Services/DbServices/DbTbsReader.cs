using Core.Task1.Services.DbServices.Properties;
using Core.Task2.Model;
using Core.Task2.Services.DbServices.Abstract;
using Core.Task2.Services.DbServices.Exceptions;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task2.Services.DbServices
{
    public class DbTbsReader : IDbTbsReader
    {
        public IEnumerable<AccountClass> ReadAccountClassesByBankIdAndPeriodId(Bank bank, Period period)
        {
            try
            {
                List<AccountClass> accountClasses = new List<AccountClass>();
                using (var connection = new NpgsqlConnection(DbProperties.ConnectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT id, number, description FROM tb.account_classes WHERE bank_id = @bankId AND period_id = @periodId";
                        command.Parameters.AddWithValue("bankId", bank.Id);
                        command.Parameters.AddWithValue("periodId", period.Id);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                long id = (long)reader["id"];
                                short number = (short)reader["number"];
                                string description = (string)reader["description"];
                                AccountClass accountClass = new AccountClass
                                {
                                    Id = id,
                                    Number = number,
                                    Description = description,
                                    Bank = bank,
                                    Period = period
                                };

                                accountClasses.Add(accountClass);
                            }
                        }
                    }
                }

                return accountClasses;
            }
            catch (Exception ex)
            {
                throw new DbReaderException($"Error occured while reading account classes: {ex.Message}", ex);
            }
        }

        public IEnumerable<Account> ReadAccountsByAccountClassId(AccountClass accountClass)
        {
            try
            {
                List<Account> accounts = new List<Account>();
                using (var connection = new NpgsqlConnection(DbProperties.ConnectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT id, number FROM tb.accounts WHERE account_class_id = @accountClassId";
                        command.Parameters.AddWithValue("accountClassId", accountClass.Id);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                long id = (long)reader["id"];
                                short number = (short)reader["number"];
                                Account account = new Account
                                {
                                    Id = id,
                                    Number = number,
                                    AccountClass = accountClass
                                };

                                accounts.Add(account);
                            }
                        }
                    }
                }

                return accounts;
            }
            catch (Exception ex)
            {
                throw new DbReaderException($"Error occured while reading accounts: {ex.Message}", ex);
            }
        }

        public IEnumerable<FileName> ReadAllFileNames()
        {
            try
            {
                List<FileName> fileNames = new List<FileName>();
                using (var connection = new NpgsqlConnection(DbProperties.ConnectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT id, name FROM tb.file_names";
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                long id = (long)reader["id"];
                                string name = (string)reader["name"];
                                FileName fileName = new FileName
                                {
                                    Id = id,
                                    Name = name
                                };

                                fileNames.Add(fileName);
                            }
                        }
                    }
                }

                return fileNames;
            }
            catch (Exception ex)
            {
                throw new DbReaderException($"Error occured while reading file names: {ex.Message}", ex);
            }
        }

        public Bank ReadBankByFileNameId(FileName fileName)
        {
            try
            {
                Bank bank = null;
                using (var connection = new NpgsqlConnection(DbProperties.ConnectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT id, name FROM tb.banks WHERE file_name_id = @fileNameId";
                        command.Parameters.AddWithValue("fileNameId", fileName.Id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                long id = (long)reader["id"];
                                string name = (string)reader["name"];

                                bank = new Bank
                                {
                                    Id = id,
                                    Name = name,
                                    FileName = fileName
                                };
                            }
                        }
                    }
                }

                return bank;
            }
            catch (Exception ex)
            {
                throw new DbReaderException($"Error occured while reading banks: {ex.Message}", ex);
            }
        }

        public ClosingBalance ReadClosingBalanceByAccountId(Account account)
        {
            try
            {
                ClosingBalance closingBalance = null;
                using (var connection = new NpgsqlConnection(DbProperties.ConnectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT id, assets, liabilities FROM tb.closing_balances WHERE account_id = @accountId";
                        command.Parameters.AddWithValue("accountId", account.Id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                long id = (long)reader["id"];
                                decimal assets = (decimal)reader["assets"];
                                decimal liabilities = (decimal)reader["liabilities"];
                                closingBalance = new ClosingBalance
                                {
                                    Id = id,
                                    Assets = assets,
                                    Liabilities = liabilities,
                                    Account = account
                                };
                            }
                        }
                    }
                }

                return closingBalance;
            }
            catch (Exception ex)
            {
                throw new DbReaderException($"Error occured while reading closing balance: {ex.Message}", ex);
            }
        }

        public OpeningBalance ReadOpeningBalanceByAccountId(Account account)
        {
            try
            {
                OpeningBalance openingBalance = null;
                using (var connection = new NpgsqlConnection(DbProperties.ConnectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT id, assets, liabilities FROM tb.opening_balances WHERE account_id = @accountId";
                        command.Parameters.AddWithValue("accountId", account.Id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                long id = (long)reader["id"];
                                decimal assets = (decimal)reader["assets"];
                                decimal liabilities = (decimal)reader["liabilities"];
                                openingBalance = new OpeningBalance
                                {
                                    Id = id,
                                    Assets = assets,
                                    Liabilities = liabilities,
                                    Account = account
                                };
                            }
                        }
                    }
                }

                return openingBalance;
            }
            catch (Exception ex)
            {
                throw new DbReaderException($"Error occured while reading opening balance: {ex.Message}", ex);
            }
        }

        public Period ReadPeriodByFileNameId(FileName fileName)
        {
            try
            {
                Period period = null;
                using (var connection = new NpgsqlConnection(DbProperties.ConnectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT id, start_date, end_date FROM tb.periods WHERE file_name_id = @fileNameId";
                        command.Parameters.AddWithValue("fileNameId", fileName.Id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                long id = (long)reader["id"];
                                DateTime startDate = (DateTime)reader["start_date"];
                                DateTime endDate = (DateTime)reader["end_date"];
                                period = new Period
                                {
                                    Id = id,
                                    StartDate = startDate,
                                    EndDate = endDate,
                                    FileName = fileName
                                };
                            }
                        }
                    }
                }

                return period;
            }
            catch (Exception ex)
            {
                throw new DbReaderException($"Error occured while reading period: {ex.Message}", ex);
            }
        }

        public Transaction ReadTransactionByAccountId(Account account)
        {
            try
            {
                Transaction transaction = null;
                using (var connection = new NpgsqlConnection(DbProperties.ConnectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT id, credit, debit FROM tb.transactions WHERE account_id = @accountId";
                        command.Parameters.AddWithValue("accountId", account.Id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                long id = (long)reader["id"];
                                decimal credit = (decimal)reader["credit"];
                                decimal debit = (decimal)reader["debit"];
                                transaction = new Transaction
                                {
                                    Id = id,
                                    Credit = credit,
                                    Debit = debit,
                                    Account = account
                                };
                            }
                        }
                    }
                }

                return transaction;
            }
            catch (Exception ex)
            {
                throw new DbReaderException($"Error occured while reading transaction: {ex.Message}", ex);
            }
        }
    }
}
