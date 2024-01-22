using Core.Task2.Model.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task2.Model
{
    public class OpeningBalance : BaseEntity
    {
        public decimal Assets { get; set; }
        public decimal Liabilities { get; set; }
        public Account Account { get; set; }
    }
}
