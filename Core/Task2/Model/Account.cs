using Core.Task2.Model.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task2.Model
{
    public class Account : BaseEntity
    {
        public short Number { get; set; }
        public AccountClass AccountClass { get; set; }
    }
}
