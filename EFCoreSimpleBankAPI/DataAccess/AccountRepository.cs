using EFCoreSimpleBankAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EFCoreSimpleBankAPI.DataAccess
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;
        public AccountRepository(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Account> GetAll()
        {
            return _context.Accounts.ToList();
        }

        public IQueryable<Account> GetAllAccounts()
        {
            return _context.Set<Account>();
        }

        public virtual async Task<ICollection<Account>> GetAllAccountsAsync()
        {
            try
            {
                //return await _context.Accounts.ToListAsync();
                return await _context.Set<Account>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }

        }
        public virtual Account GetAccountById(int AccountId)
        {
            return _context.Accounts.Find(AccountId);
        }

        public virtual async Task<Account> GetAccountByIdAsync(int? AccountId)
        {
            return await _context.Accounts.FindAsync(AccountId);
        }
        public virtual void AddAccount(Account AccountEntity)
        {
            if (AccountEntity != null)
            {
                _context.Accounts.Add(AccountEntity);
            }
        }

        public virtual async Task AddAccountAsync(Account AccountEntity)
        {
            if (AccountEntity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAccountAsync)} entity must not be null");
            }
            try
            {
                if (AccountEntity != null)
                {
                    await _context.Accounts.AddAsync(AccountEntity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(AccountEntity)} could not be saved: {ex.Message}");
            }
        }
        public virtual void UpdateAccount(Account AccountEntity)
        {
            if (AccountEntity == null)
            {
                throw new ArgumentNullException($"{nameof(UpdateAccount)} entity must not be null");
            }

            try
            {
                if (AccountEntity != null)
                {
                    ShowEntityState(_context);
                    _context.Entry(AccountEntity).State = EntityState.Modified;
                    ShowEntityState(_context);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(AccountEntity)} state could not be updated: {ex.Message}");
            }

        }

        public virtual void DeleteAccount(int AccountId)
        {
            Account AccountEntity = _context.Accounts.Find(AccountId);
            _context.Accounts.Remove(AccountEntity);
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
                    tx.Commit();
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
                        throw new NotSupportedException("Unable to save changes. The Account details was updated by another user. " + entry.Metadata.Name);
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
        public static void ShowEntityState(DataContext context)
        {
            foreach (EntityEntry entry in context.ChangeTracker.Entries())
            {
                //Discards are local variables which you can assign but cannot read from. i.e. they are “write-only” local variables.
                _ = ($"type: {entry.Entity.GetType().Name}, state: {entry.State}," + $" {entry.Entity}");
            }
        }

        private bool disposed = false;
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
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
    }
}
