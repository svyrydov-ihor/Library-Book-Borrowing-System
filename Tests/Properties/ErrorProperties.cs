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
    public class ErrorProperties
    {
        // Неможливо створити книгу з 0 або менше копій
        [Property(Arbitrary = new[] {typeof(LibraryArbitraries)})]
        public void CannotCreateBook_WithZeroOrNegativeCopies(string isbn, string title, int copies)
        {
            int invalidCopies = copies <= 0 ? copies : -copies;
            
            Assert.Throws<ArgumentException>(() => new Book(isbn, title, invalidCopies));
        }

        // Неможливо позичити книгу, якщо доступних копій 0
        [Property(Arbitrary = new[] {typeof(LibraryArbitraries)})]
        public void CannotBorrow_WhenNoCopiesAvailable(string isbn, string title)
        {
            var book = new Book(isbn, title, 1);
            
            book.Borrow();
            
            Assert.Throws<InvalidOperationException>(() => book.Borrow());
        }

        // Неможливо повернути книгу, якщо всі копії вже в бібліотеці
        [Property(Arbitrary = new[] {typeof(LibraryArbitraries)})]
        public void CannotReturn_WhenFull(Book book)
        {
            Assert.Throws<InvalidOperationException>(() => book.Return());
        }

        // Неможливо повернути книгу, яку користувач не брав
        [Property(Arbitrary = new[] {typeof(LibraryArbitraries)})]
        public void Service_CannotReturnUnborrowedBook(Book book, User user)
        {
            var repo = new InMemoryLibraryRepository();
            repo.AddBook(book);
            repo.AddUser(user);
            var service = new LibraryService(repo);

            Assert.Throws<InvalidOperationException>(() => service.ReturnBook(user.Id, book.Isbn));
        }
    }

}
