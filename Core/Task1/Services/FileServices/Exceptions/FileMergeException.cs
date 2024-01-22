using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task1.Services.FileServices.Exceptions
{
    public class FileMergeException : Exception
    {
        public FileMergeException(string message)
            : base(message)
        {
        }

        public FileMergeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
