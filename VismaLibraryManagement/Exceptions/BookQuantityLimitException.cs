using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaLibraryManagement.Exceptions
{
    public class BookQuantityLimitException : BookException
    {
        public BookQuantityLimitException()
        {
        }

        public BookQuantityLimitException(string message) : base(message)
        {
        }

        public BookQuantityLimitException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
