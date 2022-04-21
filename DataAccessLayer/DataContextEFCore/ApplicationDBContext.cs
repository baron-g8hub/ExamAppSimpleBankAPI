using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataContextEFCore
{
    public class ApplicationDBContext : DbContext
    {

        public DbSet<Account> Accounts { get; set; }
        public DbSet<PostedTransaction> PostedTransactions { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

      
    }
}
