using FsCheck.Xunit;
using Library_Book_Borrowing_System.Domain;
using Library_Book_Borrowing_System.Tests.Arbitraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Library_Book_Borrowing_System.Tests.Arbitraries.LibraryArbitraries;

namespace Library_Book_Borrowing_System.Tests.Properties
{
    public class SequenceProperties
    {
        // Виконання довільної послідовності операцій не ламає інваріанти цілісності
        // Доступні копії + Позичені копії = Загальні копії
        [Property(Arbitrary = new[] {typeof(LibraryArbitraries)})]
        public void RandomOperations_MaintainCopyConsistency(Book book, User user, LibraryOperation[] operations)
        {
            var repo = new InMemoryLibraryRepository();
            repo.AddBook(book);
            repo.AddUser(user);
            var service = new LibraryService(repo);
            
            foreach (var op in operations)
            {
                try
                {
                    // Для спрощення тесту працюємо тільки з однією книгою
                    if (op.Type == OperationType.Borrow)
                    {
                        service.BorrowBook(user.Id, book.Isbn);
                    }
                    else
                    {
                        service.ReturnBook(user.Id, book.Isbn);
                    }
                }
                catch (InvalidOperationException)
                {
                    // Ігноруємо спроби взяти недоступну або повернути не взяту книгу
                }
            }
            
            int userBorrowed = user.BorrowedIsbns.Count(isbn => isbn == book.Isbn);
            
            Assert.Equal(book.TotalCopies, book.AvailableCopies + userBorrowed);
        }
    }

}
