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
    public class BookProperties
    {
        // Нова книга завжди має AvailableCopies == TotalCopies
        [Property(Arbitrary = new[] {typeof(LibraryArbitraries)})]
        public void NewBook_HasAvailableCopiesEqualToTotal(Book book)
        {
            Assert.Equal(book.TotalCopies, book.AvailableCopies);
        }

        // Успішна видача книги зменшує доступну кількість на 1
        [Property(Arbitrary = new[] {typeof(LibraryArbitraries)})]
        public void Borrow_DecreasesAvailableCopies(Book book)
        {
            var initial = book.AvailableCopies;
            
            book.Borrow();
            
            Assert.Equal(initial - 1, book.AvailableCopies);
        }

        // Повернення книги збільшує доступну кількість на 1
        [Property(Arbitrary = new[] {typeof(LibraryArbitraries)})]
        public void Return_IncreasesAvailableCopies(Book book)
        {
            book.Borrow(); 
            var copiesAfterBorrow = book.AvailableCopies;
            
            book.Return();
            
            Assert.Equal(copiesAfterBorrow + 1, book.AvailableCopies);
        }

        // Кількість доступних книг ніколи не перевищує загальну кількість (після циклу Borrow/Return)
        [Property(Arbitrary = new[] {typeof(LibraryArbitraries)})]
        public void AvailableCopies_NeverExceedTotal(Book book)
        {
            book.Borrow();
            
            book.Return();
            
            Assert.True(book.AvailableCopies <= book.TotalCopies);
        }
    }
}
