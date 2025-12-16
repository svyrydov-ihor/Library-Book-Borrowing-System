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
    public class UserProperties
    {
        // Новий користувач не має позичених книг
        [Property(Arbitrary = new[] {typeof(LibraryArbitraries)})]
        public void NewUser_HasNoBorrowedBooks(User user)
        {
            Assert.Empty(user.BorrowedIsbns);
        }

        // Користувач має унікальний ID
        [Property(Arbitrary = new[] {typeof(LibraryArbitraries)})]
        public void User_AlwaysHasId(User user)
        {
            Assert.NotEqual(Guid.Empty, user.Id);
        }
    }

}
