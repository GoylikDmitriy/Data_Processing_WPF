using Core.Task1.Utilities.Generators.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task1.Utilities.Generators
{
    public class DataGenerator : IDataGenerator
    {
        private Random random = new Random();

        public DateTime GenerateDate(DateTime startDate, DateTime endDate)
        {
            int range = (endDate - startDate).Days;
            return startDate.AddDays(random.Next(range));
        }

        public string GenerateLatinChars(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return GenerateChars(length, chars);
        }

        public string GenerateRussianChars(int length)
        {
            const string chars = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            return GenerateChars(length, chars);
        }

        private string GenerateChars(int length, string chars)
        {
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public int GeneratePositiveEvenNumber(int minVal, int maxVal)
        {
            int number = random.Next(minVal, maxVal + 1);
            return number % 2 == 0 ? number : number + 1;
        }

        public double GeneratePositiveDoubleNumber(double minVal, double maxVal)
        {
            double val = random.NextDouble() * (maxVal - minVal) + minVal;
            return Math.Round(val, 8);
        }
    }
}
