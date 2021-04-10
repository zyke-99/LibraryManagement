using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaLibraryManagement.Exceptions
{
    public class BookReturnOverdueException : BookException
    {
        public BookReturnOverdueException()
        {
        }

        public BookReturnOverdueException(string message) : base(message)
        {
        }

        public BookReturnOverdueException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
