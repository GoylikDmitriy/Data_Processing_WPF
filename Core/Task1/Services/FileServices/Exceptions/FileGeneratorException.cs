using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task1.Services.FileServices.Exceptions
{
    public class FileGeneratorException : Exception
    {
        public string FilePath { get; }

        public FileGeneratorException(string message, string filePath)
            : base(message)
        {
            FilePath = filePath;
        }

        public FileGeneratorException(string message, string filePath, Exception innerException)
            : base(message, innerException)
        {
            FilePath = filePath;
        }
    }
}
