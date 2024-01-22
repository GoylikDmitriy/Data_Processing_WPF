using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task1.Utilities.Generators.Abstract
{
    public interface IDataGenerator
    {
        DateTime GenerateDate(DateTime startDate, DateTime endDate);
        string GenerateLatinChars(int length);
        string GenerateRussianChars(int length);
        int GeneratePositiveEvenNumber(int minVal, int maxVal);
        double GeneratePositiveDoubleNumber(double minVal, double maxVal);
    }
}
