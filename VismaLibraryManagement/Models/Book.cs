using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaLibraryManagement.Models
{
    public class Book
    {
        public long id { get; set; }
        public string Name { get; }
        public string Author { get; }
        public string Category { get; }
        public string Language { get; }
        public DateTime PublicationDate { get; }
        public string ISBN { get; }
        public string TakenBy { get; set; }
        public DateTime? ReturnDate { get; set; }

        public Book(string name, string author, string category, string language, string isbn, DateTime publicationDate)
        {
            this.Name = name;
            this.Author = author;
            this.Category = category;
            this.Language = language;
            this.ISBN = isbn;
            this.PublicationDate = publicationDate;
        }

        public override string ToString()
        {
            string taken = string.Empty;
            if (this.TakenBy != null) taken = ", Taken by:" + this.TakenBy +
                       ", Return date:" + this.ReturnDate?.ToString("yyyy'-'MM'-'dd");

            return "id:" + this.id +
                ", Name:" + this.Name +
                ", Author:" + this.Author +
                ", Category:" + this.Category +
                ", Language:" + this.Language +
                ", ISBN:" + this.ISBN +
                ", Publication date:" + this.PublicationDate.ToString("yyyy'-'MM'-'dd") +
                taken;
        }
    }
}
