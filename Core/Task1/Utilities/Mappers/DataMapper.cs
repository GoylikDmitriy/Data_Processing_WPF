using Core.Task1.Model;
using Core.Task1.Utilities.Mappers.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task1.Utilities.Mappers
{
    public class DataMapper : IDataMapper
    {
        private const string Separator = "||";

        public Data Map(string data)
        {
            string[] parts = data.Split(new[] { Separator }, StringSplitOptions.None);
            DateTime date = DateTime.ParseExact(parts[0], "dd.MM.yyyy", null);
            string latinChars = parts[1];
            string russianChars = parts[2];
            int positiveEvenNumber = int.Parse(parts[3]);
            double positiveDoubleNumber = double.Parse(parts[4]);

            return new Data
            {
                Date = DateTime.SpecifyKind(date, DateTimeKind.Utc),
                LatinChars = latinChars,
                RussianChars = russianChars,
                PositiveEvenNumber = positiveEvenNumber,
                PositiveDoubleNumber = positiveDoubleNumber
            };
        }

        public string Map(Data data)
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}",
                Separator,
                data.Date.ToString("dd.MM.yyyy"),
                data.LatinChars,
                data.RussianChars,
                data.PositiveEvenNumber,
                data.PositiveDoubleNumber
            );
        }
    }
}
