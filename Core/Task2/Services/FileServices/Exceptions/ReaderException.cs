using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task2.Services.FileServices.Exceptions
{
    public class ReaderException : Exception
    {
        public ReaderException()
        {
        }

        public ReaderException(string? message) : base(message)
        {
        }

        public ReaderException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
