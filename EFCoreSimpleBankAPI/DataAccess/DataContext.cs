using EFCoreSimpleBankAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreSimpleBankAPI.DataAccess
{
    public  class DataContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<PostedTransaction> PostedTransactions { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            #region FluentAPI
            //Used for model Data Seed
            //modelBuilder.Entity<Book>().HasData(
            //new Book { BookId = 1, Publisher = "Publisher 1", Title = "Title 1" },
            //new Book { BookId = 2, Publisher = "Publisher 2", Title = "Title 2" },
            //new Book { BookId = 3, Publisher = "Publisher 3", Title = "Title 3" },
            //new Book { BookId = 4, Publisher = "Publisher 4", Title = "Title 4" }
            //);

            //Used to shape the DB
            //var book = modelBuilder.Entity<Book>();
            //book.HasKey(p => p.BookId);
            //book.Property(p => p.Title).HasMaxLength(120).IsRequired();
            //book.Property(p => p.Publisher).HasMaxLength(50);
            //book.Property(p => p.TimeStamp)
            //    .HasColumnType("timestamp")
            //    .ValueGeneratedOnAddOrUpdate()
            //    .IsConcurrencyToken();
            #endregion
        }
    }
}
