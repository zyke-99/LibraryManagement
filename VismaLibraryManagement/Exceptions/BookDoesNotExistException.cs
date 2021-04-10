using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaLibraryManagement.Exceptions
{
    public class BookDoesNotExistException : BookException
    {
        public BookDoesNotExistException()
        {
        }

        public BookDoesNotExistException(string message) : base(message)
        {
        }

        public BookDoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
