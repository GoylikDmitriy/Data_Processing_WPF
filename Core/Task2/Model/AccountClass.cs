using Core.Task2.Model.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task2.Model
{
    public class AccountClass : BaseEntity
    {
        public short Number { get; set; }
        public string Description { get; set; }
        public Bank Bank { get; set; }
        public Period Period { get; set; }
    }
}
