using Microsoft.EntityFrameworkCore;
using SimpleBankWebAPI.Models;

namespace SimpleBankWebAPI.EFCoreDataAccess
{
    public class BankingContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public BankingContext(DbContextOptions<BankingContext> options)
            : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            //if (optionsBuilder.IsConfigured) return;
            ////Called by parameterless ctor Usually Migrations
            //var environmentName = Environment.GetEnvironmentVariable("EnvironmentName") ?? "local";

            //optionsBuilder.UseSqlServer(
            //    new ConfigurationBuilder()
            //        .SetBasePath(Path.GetDirectoryName(GetType().GetTypeInfo().Assembly.Location))
            //        .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: false)
            //        .Build()
            //        .GetConnectionString("BookContext")
            //);            
            ////optionsBuilder.UseSqlServer(ConnectionString);
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
            //var entity = modelBuilder.Entity<Account>();
            //entity.Property(p => p.AccountNumber).ValueGeneratedOnAdd().UseIdentityColumn();

            //entity.Property(p => p.Title).HasMaxLength(120).IsRequired();
            //entity.Property(p => p.Publisher).HasMaxLength(50);
            //entity.Property(p => p.TimeStamp)
            //    .HasColumnType("timestamp")
            //    .ValueGeneratedOnAddOrUpdate()
            //    .IsConcurrencyToken();
            #endregion
        }
    }
}
