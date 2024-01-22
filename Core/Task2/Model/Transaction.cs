using Core.Task2.Model.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task2.Model
{
    public class Transaction : BaseEntity
    {
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public Account Account { get; set; }
    }
}
