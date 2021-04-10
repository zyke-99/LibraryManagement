using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaLibraryManagement.Utils
{
    public interface IFileIO
    {
        public string ReadFile(string path);

        public void WriteFile(string path, string data);
    }
}
