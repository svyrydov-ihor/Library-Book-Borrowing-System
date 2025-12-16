using FsCheck.Xunit;
using Library_Book_Borrowing_System.Domain;
using Library_Book_Borrowing_System.Tests.Arbitraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Book_Borrowing_System.Tests.Properties
{
    public class InvariantProperties
    {
        // Успішна видача книги додає ISBN користувачу і зменшує копії книги
        [Property(Arbitrary = new[] {typeof(LibraryArbitraries)})]
        public void BorrowingBook_UpdatesStateCorrectly(Book book, User user)
        {
            var repo = new InMemoryLibraryRepository();
            repo.AddBook(book);
            repo.AddUser(user);
            var service = new LibraryService(repo);
            int initialCopies = book.AvailableCopies;
            
            service.BorrowBook(user.Id, book.Isbn);
            
            var updatedUser = repo.GetUser(user.Id);
            var updatedBook = repo.GetBook(book.Isbn);
            
            Assert.Contains(book.Isbn, updatedUser.BorrowedIsbns);
            Assert.Equal(initialCopies - 1, updatedBook.AvailableCopies);
        }

        // Успішне повернення книги видаляє ISBN у користувача і збільшує копії
        [Property(Arbitrary = new[] {typeof(LibraryArbitraries)})]
        public void ReturningBook_UpdatesStateCorrectly(Book book, User user)
        {
            var repo = new InMemoryLibraryRepository();
            repo.AddBook(book);
            repo.AddUser(user);
            var service = new LibraryService(repo);
            
            service.BorrowBook(user.Id, book.Isbn);
            int copiesBeforeReturn = book.AvailableCopies;
            
            service.ReturnBook(user.Id, book.Isbn);
            
            var updatedUser = repo.GetUser(user.Id);
            var updatedBook = repo.GetBook(book.Isbn);

            Assert.DoesNotContain(book.Isbn, updatedUser.BorrowedIsbns);
            Assert.Equal(copiesBeforeReturn + 1, updatedBook.AvailableCopies);
        }

        // Кількість позичених книг у користувача завжди >= 0
        [Property(Arbitrary = new[] {typeof(LibraryArbitraries)})]
        public void UserBorrowedList_NeverNegativeLength(User user, Book book)
        {
            var repo = new InMemoryLibraryRepository();
            repo.AddBook(book);
            repo.AddUser(user);
            var service = new LibraryService(repo);

            service.BorrowBook(user.Id, book.Isbn);
            
            Assert.True(user.BorrowedIsbns.Count >= 0);
            
            service.ReturnBook(user.Id, book.Isbn);

            Assert.True(user.BorrowedIsbns.Count >= 0);
        }
    }
}
