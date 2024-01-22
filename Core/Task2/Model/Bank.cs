using Core.Task2.Model.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task2.Model
{
    public class Bank : BaseEntity
    {
        public string Name { get; set; }
        public FileName FileName { get; set; }
    }
}
