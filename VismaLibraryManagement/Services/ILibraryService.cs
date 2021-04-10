using System.Collections.Generic;
using VismaLibraryManagement.Models;

namespace VismaLibraryManagement.Services
{
    public interface ILibraryService
    {
        public void AddBook(Book book);

        public void DeleteBook(long bookId);

        public void TakeBook(string name, int days, long bookId);

        public void ReturnBook(string name, long bookId);

        public List<Book> GetAllBooks();

        public List<Book> GetBooksByFilter(string property, string value);

        public List<Book> GetBooksByReader(string name);

    }
}
