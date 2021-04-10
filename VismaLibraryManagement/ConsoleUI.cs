using System;
using System.Collections.Generic;
using VismaLibraryManagement.Exceptions;
using VismaLibraryManagement.Models;
using VismaLibraryManagement.Services;

namespace VismaLibraryManagement
{
    class ConsoleUI
    {
        private ILibraryService _libraryService;

        public ConsoleUI(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }
        public void Init()
        {
            string option;
            bool running = true;
            Console.WriteLine("Welcome to the library management system!");
            while(running)
            {
                Console.WriteLine("Please select an option:" +
                    "\nlist - List all books" +
                    "\nlistf - List books by filter" +
                    "\nadd - Add a book" +
                    "\ntake - Take a book" +
                    "\nreturn - Return a book" +
                    "\ndelete - Delete a book" +
                    "\nquit - Quit the application\n");
                option = Console.ReadLine();
                switch (option)
                {
                    case "listf":
                        ListByFilter();
                        break;
                    case "list":
                        ShowBooks();
                        break;
                    case "add":
                        AddBook();
                        break;
                    case "take":
                        TakeBook();
                        break;
                    case "return":
                        ReturnBook();
                        break;
                    case "delete":
                        RemoveBooks();
                        break;
                    case "quit":
                        Console.WriteLine("See you next time!");
                        running = false;
                        break;
                    default:
                        Console.WriteLine("No such command :/");
                        break;
                }
            }
        }

        private void ShowBooks()
        {
            Console.Clear();
            List<Book> bookList = _libraryService.GetAllBooks();
            if(bookList != null && bookList.Count > 0)
            {
                PrintBooks(bookList);
            }
            else
            {
                Console.WriteLine("There are no books to show");
            }
        }

        private void AddBook()
        {
            Console.WriteLine("Enter the name of the book");
            string name = Console.ReadLine();
            Console.WriteLine("Enter the author of the book");
            string author = Console.ReadLine();
            Console.WriteLine("Enter the category of the book");
            string category = Console.ReadLine();
            Console.WriteLine("Enter the language of the book");
            string language = Console.ReadLine();
            Console.WriteLine("Enter the ISBN of the book");
            string isbn = Console.ReadLine();
            Console.WriteLine("Enter the publishing date of the book (YYYY-MM-DD format)");
            string publishingDate = Console.ReadLine();
            try
            {
                _libraryService.AddBook(new Book(name, author, category, language, isbn, DateTime.Parse(publishingDate)));
                Console.WriteLine("Successfully added a book!");
            }
            catch(Exception ex)
            {
                if(ex is BookException)
                {
                    Console.WriteLine(ex.Message);
                }
                else if(ex is ArgumentNullException || ex is FormatException)
                {
                    Console.WriteLine("Please enter a correct date format");
                    AddBook();
                }
                else
                {
                    Console.WriteLine("Something went wrong");
                }

            }
        }

        private void RemoveBooks()
        {
            string choice;
            long id;
            Console.WriteLine("Enter the id of the book you want to remove or enter \"back\" to return");
            choice = Console.ReadLine();
            switch(choice)
            {
                case "back":
                    return;
                default:
                    break;
            }
            try
            {
                id = long.Parse(choice);
                _libraryService.DeleteBook(id);
                Console.Clear();
                Console.WriteLine("Book deleted successfully!");
            }
            catch(Exception ex)
            {
                if(ex is BookException)
                {
                    Console.WriteLine(ex.Message);
                }
                else
                {
                    Console.WriteLine("Please enter a valid number as an id");
                    RemoveBooks();
                }
            }

        }

        private void ListByFilter()
        {
            string property;
            string value = string.Empty;
            Console.Clear();
            Console.WriteLine("Enter by which property you want to filter:" +
                "\nauthor - filter by author" +
                "\ncategory - filter by category" +
                "\nlanguage - filter by language" +
                "\nisbn - filter by isbn" +
                "\nname - filter by book name" +
                "\navailable - filter available books" +
                "\ntaken - filter taken books\n");
            property = Console.ReadLine();
            switch (property)
            {
                case "author":
                    Console.WriteLine("Enter the author name:");
                    break;
                case "category":
                    Console.WriteLine("Enter the category name:");
                    break;
                case "language":
                    Console.WriteLine("Enter the language name:");
                    break;
                case "isbn":
                    Console.WriteLine("Enter the isbn");
                    break;
                case "name":
                    Console.WriteLine("Enter the book name");
                    break;
                case "available":
                    value = "available";
                    break;
                case "taken":
                    value = "taken";
                    break;
                default:
                    Console.WriteLine("No such filter criteria");
                    return;
            }
            if (string.IsNullOrEmpty(value))
            {
                value = Console.ReadLine();
            }
            try
            {
                List<Book> bookList = _libraryService.GetBooksByFilter(property, value);
                if(bookList!=null&& bookList.Count > 0)
                {
                    PrintBooks(bookList);
                } else
                {
                    Console.WriteLine("There are no such books");
                }
            }
            catch
            {
                Console.WriteLine("Something went wrong");
            }
            
        }

        private void TakeBook()
        {
            string name;
            string choice;
            long id;
            int days;
            Console.WriteLine("Enter your name");
            name = Console.ReadLine();
            List<Book> bookList = _libraryService.GetAllBooks();
                Console.WriteLine("Enter the id of the book you want to take");
                choice = Console.ReadLine();
            try
            {
                id = long.Parse(choice);
                Console.WriteLine("Enter the number of days for how long you'll take the book");
                choice = Console.ReadLine();
                days = int.Parse(choice);
                _libraryService.TakeBook(name, days, id);
                Console.Clear();
                Console.WriteLine("You have successfully taken a book!");
            }
            catch (Exception ex)
            {
                if (ex is BookException)
                {
                    Console.WriteLine(ex.Message);
                }
                else if(ex is ArgumentNullException || ex is FormatException)
                {
                    Console.WriteLine("Invalid number entered!");
                    TakeBook();
                }
                else
                {
                    Console.WriteLine("Something went wrong :/");
                }
            }  
                            
        }

        private void ReturnBook()
        {
            string name;
            string choice;
            Console.WriteLine("Enter your name or enter \"back\" to return");
            name = Console.ReadLine();
            List<Book> bookList = _libraryService.GetBooksByReader(name);
            if(bookList != null && bookList.Count > 0)
            {
                PrintBooks(bookList);
                Console.WriteLine("Enter the id of the book you want to return");
                choice = Console.ReadLine();
                try
                {
                    _libraryService.ReturnBook(name, long.Parse(choice));
                    Console.Clear();
                    Console.WriteLine("Book successfully returned");
                }
                catch (Exception ex)
                {
                    if(ex is BookException)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    else if(ex is ArgumentNullException || ex is FormatException || ex is OverflowException)
                    {
                        Console.WriteLine("Invalid number entered!");
                        ReturnBook();
                    }
                    else
                    {
                        Console.WriteLine("Something went wrong");
                    }
                }

            }
            else
            {
                Console.WriteLine("You have no books to return!");
            }
        }

        private void PrintBooks(List<Book> bookList)
        {
            foreach(var book in bookList)
            {
                Console.WriteLine(book);
            }
            Console.WriteLine();
        }
    }
}
