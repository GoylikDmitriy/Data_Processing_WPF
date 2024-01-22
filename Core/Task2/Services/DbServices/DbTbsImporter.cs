using Core.Task2.Model;
using Core.Task2.Model.Abstract;
using Core.Task2.Services.DbServices.Abstract;
using Core.Task2.Services.DbServices.Provider;
using Core.Task2.Services.FileServices;
using NPOI.OpenXmlFormats.Dml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Task2.Services.DbServices
{
    public class DbTbsImporter : IDbTbsImporter
    {
        private TrialBalanceSheetReader reader;

        public DbTbsImporter(TrialBalanceSheetReader reader)
        {
            this.reader = reader;
        }

        public async Task<FileName> ImportAsync(string filePath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    reader.ReadSheet(filePath);

                    var fileName = CreateFileName(filePath);
                    var bank = CreateBank(fileName);
                    var period = CreatePeriod(fileName);

                    List<AccountClass> accountClasses = new List<AccountClass>();
                    List<Account> accounts = new List<Account>();
                    List<OpeningBalance> openingBalances = new List<OpeningBalance>();
                    List<Transaction> transactions = new List<Transaction>();
                    List<ClosingBalance> closingBalances = new List<ClosingBalance>();
                    CreateEntities(bank, period, accountClasses, accounts, openingBalances, transactions, closingBalances);

                    fileName.Id = ImporterProvider.GetImporter(fileName).Import();
                    bank.Id = ImporterProvider.GetImporter(bank).Import();
                    period.Id = ImporterProvider.GetImporter(period).Import();
                    foreach (var accountClass in accountClasses)
                    {
                        accountClass.Id = ImporterProvider.GetImporter(accountClass).Import();
                    }

                    foreach (var account in accounts)
                    {
                        account.Id = ImporterProvider.GetImporter(account).Import();
                    }

                    for (int i = 0; i < openingBalances?.Count; i++)
                    {
                        openingBalances[i].Id = ImporterProvider.GetImporter(openingBalances[i]).Import();
                        transactions[i].Id = ImporterProvider.GetImporter(transactions[i]).Import();
                        closingBalances[i].Id = ImporterProvider.GetImporter(closingBalances[i]).Import();
                    }

                    return fileName;
                }
                catch (Exception ex)
                {
                    throw new Task1.Services.DbServices.Exceptions.DbImporterException($"Error occured while importing data to db: {ex.Message}", ex);
                }
            });
        }

        private FileName CreateFileName(string filePath)
        {
            return new FileName
            {
                Name = Path.GetFileName(filePath),
            };
        }

        private Bank CreateBank(FileName fileName)
        {
            Bank bank = reader.ReadBank();
            bank.FileName = fileName;
            return bank;
        }

        private Period CreatePeriod(FileName fileName)
        {
            Period period = reader.ReadPeriod();
            period.FileName = fileName;
            return period;
        }

        private void CreateEntities(Bank bank, Period period, List<AccountClass> accountClasses, List<Account> accounts,
            List<OpeningBalance> openingBalances, List<Transaction> transactions, List<ClosingBalance> closingBalances)
        {
            for (int i = 8; i < reader.RowCount; i++)
            {
                BaseEntity entity = reader.ReadAccountOrClass(i);
                if (entity != null)
                {
                    if (entity is AccountClass accountClass)
                    {
                        accountClass.Bank = bank;
                        accountClass.Period = period;
                        accountClasses.Add(accountClass);
                    }
                    else if (entity is Account account)
                    {
                        account.AccountClass = accountClasses.Last();
                        accounts.Add(account);

                        OpeningBalance ob = reader.ReadOpeningBalance(i);
                        ob.Account = account;
                        openingBalances.Add(ob);

                        Transaction transaction = reader.ReadTransaction(i);
                        transaction.Account = account;
                        transactions.Add(transaction);

                        ClosingBalance cb = reader.ReadClosingBalance(i);
                        cb.Account = account;
                        closingBalances.Add(cb);
                    }
                }
            }
        }
    }
}