using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaLibraryManagement.Exceptions
{
    public class BookReturnTimeLimitException : BookException
    {
        public BookReturnTimeLimitException()
        {
        }

        public BookReturnTimeLimitException(string message) : base(message)
        {
        }

        public BookReturnTimeLimitException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
