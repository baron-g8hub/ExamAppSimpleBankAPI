namespace EFCoreSimpleBankAPI.DataAccess
{

    public static class DbInitializer
    {
        public static void Initialize(DataContext context)
        {
            context.Database.EnsureCreated();
            //// Look for any Books.
            //if (context.Accounts.Any())
            //{
            //    return;   // DB has been seeded
            //}

            //var books = new Accounts[]
            //{
            //new Book{Publisher="Publisher 1",Title="Title 1",Author="Author1",Price=10000, Source="Store1",PurchaseUrl="books.com"},
            //new Book{Publisher="Publisher 2",Title="Title 1",Author="Author2",Price=15000, Source="Store2",PurchaseUrl="mybooks.com"},
            //new Book{Publisher="Publisher 3",Title="Title 2",Author="Author3",Price=20000, Source="Store1",PurchaseUrl="bookslib.com"}
            //};

            //foreach (Book s in books)
            //{
            //    context.Books.Add(s);
            //}
            context.SaveChanges();
        }
    }
}
