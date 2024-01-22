using Core.Task1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task1.Utilities.Mappers.Abstract
{
    public interface IMapper<T>
    {
        T Map(string data);
        string Map(T data);
    }
}
