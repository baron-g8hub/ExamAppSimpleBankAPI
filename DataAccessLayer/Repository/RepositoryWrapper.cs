using DataAccessLayer.DataContextEFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using DataAccessLayer.Contracts;
using DataAccessLayer.Models;
using Microsoft.Data.SqlClient;

namespace DataAccessLayer.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {

        private ApplicationDBContext _repoContext;
        private IAccountsServiceRepository _account;
        private IPostedTransactionsRepository _transaction;

        public RepositoryWrapper(ApplicationDBContext context)
        {
            _repoContext = context;
        }

        public IAccountsServiceRepository Accounts
        {
            get
            {
                if (_account == null)
                {
                    _account = new AccountsServiceRepository(_repoContext);
                }
                return _account;
            }
        }
        public IPostedTransactionsRepository PostedTransactions
        {
            get
            {
                if (_transaction == null)
                {
                    _transaction = new PostedTransactionsRepository(_repoContext);
                }
                return _transaction;
            }
        }


        public async Task SaveAsync()
        {
            await _repoContext.SaveChangesAsync();
        }

        public async Task<int> SaveAsync(CancellationToken ct)
        {
            int records = 0;
            IDbContextTransaction? tx = null;
            //await Task.Delay(5000);
            if (ct.IsCancellationRequested)
            {
                ct.ThrowIfCancellationRequested();
            }

            try
            {
                using (tx = await _repoContext.Database.BeginTransactionAsync())
                {
                    records = await _repoContext.SaveChangesAsync();
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
                        throw new NotSupportedException("Unable to save changes. The Entity details was updated by another user. " + entry.Metadata.Name);
                    }
                }
                throw ex;
            }
            catch (DbUpdateException ex)
            {
                SqlException? s = ex.InnerException as SqlException;
                var errorMessage = $"{ex.Message}" + " {ex?.InnerException.Message}" + " rolling back…";
                tx.Rollback();
                throw s;
            }
            //  return records;
        }
    }
}
