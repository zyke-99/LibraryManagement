using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using VismaLibraryManagement.Exceptions;
using VismaLibraryManagement.Models;
using VismaLibraryManagement.Services;
using VismaLibraryManagement.Utils;

namespace VismaLibraryManagement
{
    public class LibraryService : ILibraryService
    {
        private const double MAX_DAYS_FOR_RETURN = 60;
        private const int MAX_NUMBER_OF_BOOKS = 3;
        private IFileIO _fileIO;

        public LibraryService(IFileIO fileIO)
        {
            _fileIO = fileIO;
        }
        public void AddBook(Book book)
        {
            List<Book> bookList = GetAllBooks();
            if(bookList.Count() == 0)
            {
                book.id = 0;
            } else
            {
                book.id = bookList.Select(b => b.id).Max() + 1;
            }

            bookList.Add(book);

            this._fileIO.WriteFile(ConfigurationManager.AppSettings.Get("books"),
                JsonConvert.SerializeObject(bookList));

        }
        public void DeleteBook(long bookId)
        {
            List<Book> bookList = GetAllBooks();

            if(bookList.Any(b => b.id == bookId))
            {
                bookList.RemoveAll(b => b.id == bookId);
                this._fileIO.WriteFile(ConfigurationManager.AppSettings.Get("books"),
                    JsonConvert.SerializeObject(bookList));
            }
            else
            {
                throw new BookDoesNotExistException("Such a book does not exist");
            }

        }

        public void TakeBook(string name, int days, long bookId)
        {

            List<Book> bookList = GetAllBooks();
            Book book = bookList.Find(b => b.id == bookId);
            if(book == null)
            {
                throw new BookDoesNotExistException("Such a book does not exist");
            }
            else
            {
                if(book.TakenBy == null)
                {
                    if (days > MAX_DAYS_FOR_RETURN || days < 1)
                    {
                        throw new BookReturnTimeLimitException("We only give out books for 2 months or less"); //might need to change this one a bit
                    }
                    else
                    {

                        if (bookList.Where(b => b.TakenBy != null && b.TakenBy.ToLower() == name.ToLower()).Count() >= MAX_NUMBER_OF_BOOKS)
                        {
                            throw new BookQuantityLimitException("You have exceeded your book limit, we cannot provide more books until some are returned"); //defo will need to change this
                        }
                        else
                        {
                            foreach(var b in bookList)
                            {
                                if(b.id == bookId)
                                {
                                    b.TakenBy = name;
                                    b.ReturnDate = DateTime.Today.AddDays(days);
                                    break;
                                }
                            }
                            this._fileIO.WriteFile(ConfigurationManager.AppSettings.Get("books"),
                                JsonConvert.SerializeObject(bookList));
                        }
                    }
                }
                else
                {
                    throw new BookAlreadyTakenException("This book is already taken!");
                }
            }

        }
        
        public void ReturnBook(string name, long bookId)
        {
            List<Book> bookList = GetAllBooks();
            DateTime? returnDate = null;
            Book book = bookList.Find(b => b.id == bookId);
            if(book != null)
            {
                if(book.TakenBy!=null && book.TakenBy.ToLower().Equals(name.ToLower()))
                {
                    foreach(var b in bookList)
                    {
                        if(b.id == bookId)
                        {
                            returnDate = b.ReturnDate;
                            b.TakenBy = null;
                            b.ReturnDate = null;
                            break;
                        }
                    }

                    this._fileIO.WriteFile(ConfigurationManager.AppSettings.Get("books"),
                        JsonConvert.SerializeObject(bookList));
                    if(DateTime.Today > returnDate)
                    {
                        throw new BookReturnOverdueException("Seems like you are a bit overdue on the return, funny message");
                    }
                }
                else
                {
                    throw new BookHasAnotherOwnerException("This book does not belong to you!");
                }
            }
            else
            {
                throw new BookDoesNotExistException("Such a book does not exist!");
            }
        }

        public List<Book> GetAllBooks()
        {
            List<Book> bookList = JsonConvert.DeserializeObject<List<Book>>(
                        this._fileIO.ReadFile(ConfigurationManager.AppSettings.Get("books")));
            if (bookList != null)
            {
                return bookList;
            }
            else
            {
                return new List<Book>();
            }
        }

        public List<Book> GetBooksByFilter(string property, string value)
        {
            List<Book> bookList = GetAllBooks();

            switch (property.ToLower())
            {
                case "author":
                    return bookList
                        .Where(b => b.Author.ToLower().Contains(value.ToLower()))
                        .ToList();
                case "category":
                    return bookList
                        .Where(b => b.Category.ToLower().Contains(value.ToLower()))
                        .ToList();
                case "language":
                    return bookList
                        .Where(b => b.Language.ToLower().Contains(value.ToLower()))
                        .ToList();
                case "isbn":
                    return bookList
                        .Where(b => b.ISBN.ToLower().Contains(value.ToLower()))
                        .ToList();
                case "name":
                    return bookList
                        .Where(b => b.Name.ToLower().Contains(value.ToLower()))
                        .ToList();
                case "taken":
                    return bookList
                        .Where(b => b.TakenBy != null)
                        .ToList();
                case "available":
                    return bookList
                        .Where(b => b.TakenBy == null)
                        .ToList();
                default:
                    throw new Exception();

            }

        }

        public List<Book> GetBooksByReader(string name)
        {
            List<Book> bookList = GetAllBooks()
                .Where(b => b.TakenBy != null && b.TakenBy.ToLower().Equals(name.ToLower()))
                .ToList();

            return bookList;
        }

    }
}
