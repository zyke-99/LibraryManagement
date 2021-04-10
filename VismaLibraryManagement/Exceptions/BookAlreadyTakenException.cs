using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaLibraryManagement.Exceptions
{
    public class BookAlreadyTakenException : BookException
    {
        public BookAlreadyTakenException()
        {
        }

        public BookAlreadyTakenException(string message) : base(message)
        {
        }

        public BookAlreadyTakenException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
