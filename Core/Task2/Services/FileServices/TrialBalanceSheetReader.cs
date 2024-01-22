using Core.Task2.Model;
using Core.Task2.Model.Abstract;
using Core.Task2.Services.FileServices.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Task2.Services.FileServices
{
    public class TrialBalanceSheetReader
    {
        private ExcelReader reader;

        private IEnumerable<IEnumerable<string>> sheet;
        public int RowCount { get => sheet.Count(); }

        public TrialBalanceSheetReader(ExcelReader reader)
        {
            this.reader = reader;
        }

        public void ReadSheet(string filePath)
        {
            sheet = reader.Read(filePath);
        }

        public Bank ReadBank()
        {
            try
            {
                Bank bank = new Bank();
                bank.Name = sheet.ElementAt(0).ElementAt(0);
                return bank;
            }
            catch (Exception ex)
            {
                throw new ReaderException($"Error occured while reading 'Bank': {ex.Message}", ex);
            }
        }

        public Period ReadPeriod()
        {
            try
            {
                string dateFormat = "dd.MM.yyyy";
                string datePattern = @"\d{2}\.\d{2}\.\d{4}";
                MatchCollection matches = Regex.Matches(sheet.ElementAt(2).ElementAt(0), datePattern);
                string startDateString = matches[0].Value;
                string endDateString = matches[1].Value;
                Period period = new Period();
                period.StartDate = DateTime.ParseExact(startDateString, dateFormat, CultureInfo.InvariantCulture);
                period.EndDate = DateTime.ParseExact(endDateString, dateFormat, CultureInfo.InvariantCulture);
                return period;
            }
            catch (Exception ex)
            {
                throw new ReaderException($"Error occured while reading 'Period': {ex.Message}", ex);
            }
        }

        public BaseEntity ReadAccountOrClass(int row)
        {
            try
            {
                var cell = sheet.ElementAt(row).ElementAt(0);

                if (cell.StartsWith("КЛАСС"))
                {
                    return ReadAccountClass(cell);
                }

                string accountNumberPattern = @"\d{4}";
                if (Regex.IsMatch(cell, accountNumberPattern))
                {
                    return ReadAccount(cell);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new ReaderException($"Error occured while reading 'AccountClass or Account': {ex.Message}", ex);
            }
        }

        private AccountClass ReadAccountClass(string cell)
        {
            try
            {
                string pattern = @"(\d+) (.+)$";
                Match match = Regex.Match(cell, pattern);
                if (match.Success)
                {
                    AccountClass accountClass = new AccountClass();
                    accountClass.Number = short.Parse(match.Groups[1].Value);
                    accountClass.Description = match.Groups[2].Value;
                    return accountClass;
                }
            }
            catch (Exception ex)
            {
                throw new ReaderException($"Error occured while reading 'AccountClass': {ex.Message}", ex);
            }

            throw new ReaderException("Invalid cell format when reading 'AccountClass'.");
        }

        private Account ReadAccount(string cell)
        {
            try
            {
                Account account = new Account();
                var accountNumber = cell;
                account.Number = short.Parse(accountNumber);
                return account;
            }
            catch (Exception ex)
            {
                throw new ReaderException($"Error occured while reading 'Account': {ex.Message}", ex);
            }
        }

        public OpeningBalance ReadOpeningBalance(int row)
        {
            try
            {
                OpeningBalance ob = new OpeningBalance();
                var obAssets = sheet.ElementAt(row).ElementAt(1);
                var obLiabilities = sheet.ElementAt(row).ElementAt(2);
                ob.Assets = decimal.Parse(obAssets);
                ob.Liabilities = decimal.Parse(obLiabilities);
                return ob;
            }
            catch (Exception ex)
            {
                throw new ReaderException($"Error occured while reading 'OpeningBalance': {ex.Message}", ex);
            }
        }

        public Transaction ReadTransaction(int row)
        {
            try
            {
                Transaction transaction = new Transaction();
                var credit = sheet.ElementAt(row).ElementAt(3);
                var debit = sheet.ElementAt(row).ElementAt(4);
                transaction.Credit = decimal.Parse(credit);
                transaction.Debit = decimal.Parse(debit);
                return transaction;
            }
            catch (Exception ex)
            {
                throw new ReaderException($"Error occured while reading 'Transaction': {ex.Message}", ex);
            }
        }

        public ClosingBalance ReadClosingBalance(int row)
        {
            try
            {
                ClosingBalance cb = new ClosingBalance();
                var cbAssets = sheet.ElementAt(row).ElementAt(5);
                var cbLiabilities = sheet.ElementAt(row).ElementAt(6);
                cb.Assets = decimal.Parse(cbAssets);
                cb.Liabilities = decimal.Parse(cbLiabilities);
                return cb;
            }
            catch (Exception ex)
            {
                throw new ReaderException($"Error occured while reading 'ClosingBalance': {ex.Message}", ex);
            }
        }
    }
}
