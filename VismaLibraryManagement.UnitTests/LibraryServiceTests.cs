using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using VismaLibraryManagement.Exceptions;
using VismaLibraryManagement.Models;
using VismaLibraryManagement.Services;
using VismaLibraryManagement.Utils;

namespace VismaLibraryManagement.UnitTests
{
    [TestClass]
    public class LibraryServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(BookQuantityLimitException))]
        public void TakeBook_HasThreeBooksAlreadyTaken_ThrowsException()
        {
            string readerName = "readername";
            DateTime returnDate = DateTime.Today.AddDays(1);
            List<Book> mockBooks = new List<Book>();
            Book mockBook = new Book("name", "author", "category", "language", "isbn", DateTime.Today);
            mockBook.id = 0; 
            mockBooks.Add(mockBook);
            for(int i = 1; i<4; i++)
            {
                mockBooks.Add(new Book("name", "author", "category", "language", "isbn", DateTime.Today));
                mockBooks[i].id = i;
                mockBooks[i].TakenBy = readerName;
                mockBooks[i].ReturnDate = returnDate;
            }
            foreach(var book in mockBooks)
            {
                Console.WriteLine(book);
            }
            var mockFileIO = new Mock<IFileIO>();
            mockFileIO.Setup(x => x.ReadFile(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(mockBooks));
            ILibraryService libraryService = new LibraryService(mockFileIO.Object);
            libraryService.TakeBook(readerName, 15, 0);
            
        }

        [TestMethod]
        [ExpectedException(typeof(BookReturnTimeLimitException))]
        public void TakeBook_TriesToTakeForMoreThanTwoMonths_ThrowsException()
        {
            List<Book> mockBooks = new List<Book>();
            Book mockBook = new Book("name", "author", "category", "language", "isbn", DateTime.Today);
            mockBook.id = 1;
            mockBooks.Add(mockBook);
            var mockFileIO = new Mock<IFileIO>();
            mockFileIO.Setup(x => x.ReadFile(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(mockBooks));
            ILibraryService libraryService = new LibraryService(mockFileIO.Object);
            libraryService.TakeBook("betkas", 61, 1);
        }

        [TestMethod]
        public void TakeBook_HasLessThanThreeBooksAndForLessThanTwoMonths_DoesNotThrowException()
        {
            List<Book> mockBooks = new List<Book>();
            Book mockBook = new Book("name", "author", "category", "language", "isbn", DateTime.Today);
            mockBook.id = 0;
            mockBooks.Add(mockBook);
            var mockFileIO = new Mock<IFileIO>();
            mockFileIO.Setup(x => x.ReadFile(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(mockBooks));
            ILibraryService libraryService = new LibraryService(mockFileIO.Object);
            libraryService.TakeBook("readername", 15, 0);
            mockFileIO.Verify(x => x.WriteFile(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(BookAlreadyTakenException))]
        public void TakeBook_TriesToTakeAlreadyTakenBook_ThrowsException()
        {
            List<Book> mockBooks = new List<Book>();
            Book mockBook = new Book("name", "author", "category", "language", "isbn", DateTime.Today);
            mockBook.id = 0;
            mockBook.TakenBy = "readername";
            mockBook.ReturnDate = DateTime.Today.AddDays(1);
            mockBooks.Add(mockBook);
            var mockFileIO = new Mock<IFileIO>();
            mockFileIO.Setup(x => x.ReadFile(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(mockBooks));
            ILibraryService libraryService = new LibraryService(mockFileIO.Object);
            libraryService.TakeBook("readername", 15, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(BookDoesNotExistException))]
        public void TakeBook_TriesToTakeNotExistantBook_ThrowsException()
        {
            var mockFileIO = new Mock<IFileIO>();
            mockFileIO.Setup(x => x.ReadFile(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(string.Empty));
            ILibraryService libraryService = new LibraryService(mockFileIO.Object);
            libraryService.TakeBook(string.Empty,1,1);
        }

        [TestMethod]
        [ExpectedException(typeof(BookDoesNotExistException))]
        public void ReturnBook_TriesToReturnNotExistantBook_ThrowsException()
        {
            var mockFileIO = new Mock<IFileIO>();
            mockFileIO.Setup(x => x.ReadFile(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(string.Empty));
            ILibraryService libraryService = new LibraryService(mockFileIO.Object);
            libraryService.ReturnBook(string.Empty,1);
        }

        [TestMethod]
        [ExpectedException(typeof(BookHasAnotherOwnerException))]
        public void ReturnBook_TriesToReturnABookThatIsNotTaken_ThrowsException()
        {
            List<Book> mockBooks = new List<Book>();
            Book mockBook = new Book("name", "author", "category", "language", "isbn", DateTime.Today);
            mockBook.id = 0;
            mockBooks.Add(mockBook);
            var mockFileIO = new Mock<IFileIO>();
            mockFileIO.Setup(x => x.ReadFile(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(mockBooks));
            ILibraryService libraryService = new LibraryService(mockFileIO.Object);
            libraryService.ReturnBook("readername", 0);
        }

        [TestMethod]
        [ExpectedException(typeof(BookReturnOverdueException))]
        public void ReturnBook_TriesToReturnBookThatIsOverdue_ThrowsExceptionButReturnsSuccessfully()
        {
            string readerName = "readername";
            DateTime returnDate = DateTime.Today.AddDays(-10);
            List<Book> mockBooks = new List<Book>();
            Book mockBook = new Book("name", "author", "category", "language", "isbn", DateTime.Today);
            mockBook.id = 0;
            mockBook.TakenBy = readerName;
            mockBook.ReturnDate = returnDate;
            mockBooks.Add(mockBook);
            var mockFileIO = new Mock<IFileIO>();
            mockFileIO.Setup(x => x.ReadFile(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(mockBooks));
            ILibraryService libraryService = new LibraryService(mockFileIO.Object);
            libraryService.ReturnBook(readerName, 0);
            mockFileIO.Verify(x => x.WriteFile(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ReturnBook_TriesToReturnBookThatHeHasTaken_DoesNotThrowException()
        {
            string readerName = "readername";
            DateTime returnDate = DateTime.Today.AddDays(1);
            List<Book> mockBooks = new List<Book>();
            Book mockBook = new Book("name", "author", "category", "language", "isbn", DateTime.Today);
            mockBook.id = 0;
            mockBook.TakenBy = readerName;
            mockBook.ReturnDate = returnDate;
            mockBooks.Add(mockBook);
            var mockFileIO = new Mock<IFileIO>();
            mockFileIO.Setup(x => x.ReadFile(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(mockBooks));
            ILibraryService libraryService = new LibraryService(mockFileIO.Object);
            libraryService.ReturnBook(readerName, 0);
            mockFileIO.Verify(x => x.WriteFile(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(BookDoesNotExistException))]
        public void DeleteBook_TriesToDeleteNotExistantBook_ExceptionThrown()
        {
            var mockFileIO = new Mock<IFileIO>();
            mockFileIO.Setup(x => x.ReadFile(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(string.Empty));
            ILibraryService libraryService = new LibraryService(mockFileIO.Object);
            libraryService.DeleteBook(1);
        }

        [TestMethod]
        public void DeleteBook_TriesToDeleteExistingBook_DoesNotThrowException()
        {
            List<Book> mockBooks = new List<Book>();
            Book mockBook = new Book("name", "author", "category", "language", "isbn", DateTime.Today);
            mockBook.id = 1;
            mockBooks.Add(mockBook);
            var mockFileIO = new Mock<IFileIO>();
            mockFileIO.Setup(x => x.ReadFile(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(mockBooks));
            ILibraryService libraryService = new LibraryService(mockFileIO.Object);
            libraryService.DeleteBook(1);
            mockFileIO.Verify(x => x.WriteFile(It.IsAny<string>(), It.IsAny<string>()), Times.Once());

        }

    }
}
