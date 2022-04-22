
using DataAccessLayer.DataContextEFCore;
using DataAccessLayer.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using DataAccessLayer.Contracts;
using System.Transactions;

namespace DataAccessLayer.Repository
{
    public class PostedTransactionsRepository : RepositoryBase<PostedTransaction>, IPostedTransactionsRepository
    {
        public PostedTransactionsRepository(ApplicationDBContext context) : base(context)
        {

        }

        public IQueryable<PostedTransaction> SelectAll()
        {
            return _context.Set<PostedTransaction>();
        }
        public IEnumerable<PostedTransaction> GetAll()
        {
            return _context.PostedTransactions.ToList();
        }
        public virtual async Task<ICollection<PostedTransaction>> GetAllAsync()
        {
            try
            {
                //var list = await RepositoryContext.PostedTransactions.ToListAsync();
                return await _context.Set<PostedTransaction>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }
        public virtual PostedTransaction GetById(int Id)
        {
            return _context.PostedTransactions.Find(Id);
        }
        public virtual async Task<PostedTransaction> GetByIdAsync(int? Id)
        {
            return await _context.PostedTransactions.FindAsync(Id.Value);
        }
        public virtual PostedTransaction GetTransactionById(int id)
        {
            return _context.PostedTransactions.Find(id);
        }

        public virtual void AddTransaction(PostedTransaction entity)
        {
            if (entity != null)
            {
                _context.PostedTransactions.Add(entity);
            }
        }
   
        public virtual async Task AddTransactionAsync(PostedTransaction entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddTransactionAsync)} entity must not be null");
            }
            try
            {
                if (entity != null)
                {
                    await _context.PostedTransactions.AddAsync(entity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }
        public virtual void Save()
        {
            _context.SaveChanges();
        }
        public virtual async Task<int> SaveAsync(CancellationToken ct)
        {
            int records = 0;
            IDbContextTransaction tx = null;
            //await Task.Delay(5000);
            if (ct.IsCancellationRequested)
            {
                ct.ThrowIfCancellationRequested();
            }
            try
            {
                using (tx = await _context.Database.BeginTransactionAsync())
                {
                    records = await _context.SaveChangesAsync();
                    await tx.CommitAsync();
                    return records;
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is Account)
                    {
                        var proposedValues = entry.CurrentValues;
                        var databaseValues = entry.GetDatabaseValues();

                        foreach (var property in proposedValues.Properties)
                        {
                            var proposedValue = proposedValues[property];
                            var databaseValue = databaseValues[property];
                        }

                        // Refresh original values to bypass next concurrency check
                        entry.OriginalValues.SetValues(databaseValues);
                    }
                    else
                    {
                        throw new NotSupportedException("Unable to save changes. The book details was updated by another user. " + entry.Metadata.Name);
                    }
                }
                throw ex;
            }
            catch (DbUpdateException ex)
            {
                SqlException s = ex.InnerException as SqlException;
                var errorMessage = $"{ex.Message}" + " {ex?.InnerException.Message}" + " rolling back…";
                tx.Rollback();
            }
            return records;
        }
        public static void ShowEntityState(ApplicationDBContext context)
        {
            foreach (EntityEntry entry in context.ChangeTracker.Entries())
            {
                //Discards are local variables which you can assign but cannot read from. i.e. they are “write-only” local variables.
                _ = ($"type: {entry.Entity.GetType().Name}, state: {entry.State}," + $" {entry.Entity}");
            }
        }
        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }


    }
}
