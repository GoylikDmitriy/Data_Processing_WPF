using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task2.Services.DbServices.Exceptions
{
    public class DbReaderException : Exception
    {
        public DbReaderException()
        {
        }

        public DbReaderException(string? message) : base(message)
        {
        }

        public DbReaderException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
