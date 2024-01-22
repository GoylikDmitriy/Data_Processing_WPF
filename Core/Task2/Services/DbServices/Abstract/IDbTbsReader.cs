using Core.Task2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task2.Services.DbServices.Abstract
{
    public interface IDbTbsReader
    {
        IEnumerable<FileName> ReadAllFileNames();
        Bank ReadBankByFileNameId(FileName fileName);
        Period ReadPeriodByFileNameId(FileName fileName);
        IEnumerable<AccountClass> ReadAccountClassesByBankIdAndPeriodId(Bank bank, Period period);
        IEnumerable<Account> ReadAccountsByAccountClassId(AccountClass accountClass);
        OpeningBalance ReadOpeningBalanceByAccountId(Account account);
        Transaction ReadTransactionByAccountId(Account account);
        ClosingBalance ReadClosingBalanceByAccountId(Account account);
    }
}
