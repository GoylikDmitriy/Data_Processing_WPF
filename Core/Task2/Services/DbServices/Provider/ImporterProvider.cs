using Core.Task2.Model;
using Core.Task2.Model.Abstract;
using Core.Task2.Services.DbServices.Abstract;
using Core.Task2.Services.DbServices.EntityImporters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task2.Services.DbServices.Provider
{
    public static class ImporterProvider
    {
        public static IImporterEntity GetImporter(BaseEntity entity)
        {
            return entity switch
            {
                FileName fileName => new FileNameImporter(fileName),
                Bank bank => new BankImporter(bank),
                Period period => new PeriodImporter(period),
                AccountClass accountClass => new AccountClassImporter(accountClass),
                Account account => new AccountImporter(account),
                OpeningBalance ob => new OpeningBalanceImporter(ob),
                Transaction transaction => new TransactionImporter(transaction),
                ClosingBalance cb => new ClosingBalanceImporter(cb),
                _ => throw new NotSupportedException($"Importer not found for entity type: {entity.GetType().Name}")
            };
        }
    }
}
