using System.IO;

namespace VismaLibraryManagement.Utils
{
    public class JSONFileIO : IFileIO
    {
        public string ReadFile(string path)
        {
            string json;
            if (!File.Exists(path))
            {
                File.Create("books.json").Close();
            }
            json = File.ReadAllText(path);
            return json;
        }

        public void WriteFile(string path, string data)
        {
            if (!File.Exists(path))
            {
                File.Create("books.json").Close();
            }
            File.WriteAllText(path, data);
        }
    }
}
