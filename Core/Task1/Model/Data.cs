using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task1.Model
{
    public class Data
    {
        public long Id { get; set; }
        public DateTime Date {  get; set; }
        public string LatinChars {  get; set; }
        public string RussianChars {  get; set; }
        public int PositiveEvenNumber { get; set; }
        public double PositiveDoubleNumber { get; set; }
    }
}
