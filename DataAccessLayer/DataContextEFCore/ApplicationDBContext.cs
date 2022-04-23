using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.DataContextEFCore
{
    public class ApplicationDBContext : DbContext
    {
        private DbSet<Account>? accounts;

        public DbSet<Account>? Accounts { get => accounts; set => accounts = value; }
        public DbSet<PostedTransaction>? PostedTransactions { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().Property(p => p.RowVersion).IsRowVersion();
            modelBuilder.Entity<PostedTransaction>().Property(p => p.RowVersion).IsRowVersion();
        }
    }
}
