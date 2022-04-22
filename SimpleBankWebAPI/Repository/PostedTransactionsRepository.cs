
using DataAccessLayer.DataContextEFCore;
using DataAccessLayer.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using SimpleBankWebAPI.Contracts;
using SimpleBankWebAPI.ViewModels;
using System.Transactions;

namespace SimpleBankWebAPI.Repository
{
    public class PostedTransactionsRepository : RepositoryBase<PostedTransaction>, IPostedTransactionsRepository
    {
        public PostedTransactionsRepository(ApplicationDBContext context) : base(context)
        {
            this.Repository = context;
        }

        public IQueryable<PostedTransaction> SelectAll()
        {
            return Repository.Set<PostedTransaction>();
        }
        public IEnumerable<PostedTransaction> GetAll()
        {
            return Repository.PostedTransactions.ToList();
        }
        public virtual async Task<ICollection<PostedTransaction>> GetAllAsync()
        {
            try
            {
                //var list = await RepositoryContext.PostedTransactions.ToListAsync();
                return await Repository.Set<PostedTransaction>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }
        public virtual PostedTransaction GetById(int Id)
        {
            return Repository.PostedTransactions.Find(Id);
        }
        public virtual async Task<PostedTransaction> GetByIdAsync(int? Id)
        {
            return await Repository.PostedTransactions.FindAsync(Id.Value);
        }
        public virtual PostedTransaction GetTransactionById(int id)
        {
            return Repository.PostedTransactions.Find(id);
        }

        public virtual async Task<int> PostTransactionAsync(CancellationToken ct, PostingTransactionWrapper wrapper)
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
                using (tx = await Repository.Database.BeginTransactionAsync())
                {
                    //try
                    //{
                    //    // 1. TakeMoneyFromAccount
                    //    var takeAmount = wrapper.PostedTransaction.Amount;
                    //    TakeMoneyFromAccount(wrapper.SourceAccount, takeAmount);

                    //    // 2. SendMoneyToAccount 
                    //    SendMoneyToAccount(wrapper.DestinationAccount, takeAmount);

                    //    // 3. Assemble Transaction Details
                    //    PostedTransaction postedTransaction = wrapper.PostedTransaction;

                    //    // 4. Add PostingTransaction 
                    //    await AddAsync(postedTransaction);

                    //    // 5. Save state changes
                    //    records = await RepositoryContext.SaveChangesAsync();

                    //    // *** If we reach here then DBUPDATE is successfull. ***

                    //    // 6. Commit changes from state to database
                    //    await tx.CommitAsync();
                    //    return records;
                    //}
                    //catch (Exception)
                    //{
                    //    // NOTE: Apply Rollback when failed to process.
                    //    //throw;
                    //}
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
       

        public void Update(Account entity)
        {
            throw new NotImplementedException();
        }
        public async Task AddAsync(PostedTransaction entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }
            try
            {
                if (entity != null)
                {
                    await Repository.PostedTransactions.AddAsync(entity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }
        public virtual void Save()
        {
            Repository.SaveChanges();
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
                using (tx = await Repository.Database.BeginTransactionAsync())
                {
                    records = await Repository.SaveChangesAsync();
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
                    Repository.Dispose();
                }
            }
            this.disposed = true;
        }





    }
}
