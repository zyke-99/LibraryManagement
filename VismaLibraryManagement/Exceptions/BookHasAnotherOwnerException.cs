using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaLibraryManagement.Exceptions
{
    public class BookHasAnotherOwnerException : BookException
    {
        public BookHasAnotherOwnerException()
        {
        }

        public BookHasAnotherOwnerException(string message) : base(message)
        {
        }

        public BookHasAnotherOwnerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
