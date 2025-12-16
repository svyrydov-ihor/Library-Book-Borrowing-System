using FsCheck;
using FsCheck.Fluent;
using Library_Book_Borrowing_System.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Book_Borrowing_System.Tests.Arbitraries
{
    public class LibraryArbitraries
    {

        public static Arbitrary<string> Isbn()
        {
            return Gen.Choose(1, 999)
                     .Select(x => $"Isbn-{x}")
                     .ToArbitrary();
        }


        private static Gen<string> Title()
        {
            return Gen.Elements("Clean Code", "The Pragmatic Programmer", "C# in Depth", "Design Patterns: Elements of Reusable Object-Oriented Software");
        }


        public static Arbitrary<Book> Book()
        {
            var bookGen = from isbn in Isbn().Generator
                from title in Title()
                from copies in Gen.Choose(1, 20)
                select new Book(isbn, title, copies);

            return Arb.From(bookGen);
        }


        public static Arbitrary<User> User()
        {
            var userGen = from name in Gen.Elements("Alice", "Bob", "Charlie", "Dave")
                select new User(name);
            
            return Arb.From(userGen);
        }


        public static Arbitrary<LibraryOperation> Operation()
        {
            var opGen = from type in Gen.Elements(OperationType.Borrow, OperationType.Return)
                from isbn in Isbn().Generator
                select new LibraryOperation(type, isbn);
            
            return Arb.From(opGen);
        }


        public enum OperationType { Borrow, Return }

        public record LibraryOperation(OperationType Type, string Isbn);

    }
}
