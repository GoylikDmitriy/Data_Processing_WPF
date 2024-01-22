using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task1.Services.DbServices.Exceptions
{
    public class DbImporterException : Exception
    {
        public DbImporterException()
        {
        }

        public DbImporterException(string? message) : base(message)
        {
        }

        public DbImporterException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
