using DataAccessLayer.DataContextEFCore;
using DataAccessLayer.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using DataAccessLayer.Contracts;


namespace DataAccessLayer.Repository
{
    public class AccountsServiceRepository : RepositoryBase<Account>, IAccountsServiceRepository
    {
        public AccountsServiceRepository(ApplicationDBContext context) : base(context)
        {

        }

        public IQueryable<Account> GetAllAccounts()
        {
            return _context.Set<Account>();
        }
        public virtual async Task<ICollection<Account>> GetAllAccountsAsync()
        {
            try
            {
                //var list = await RepositoryContext.Accounts.ToListAsync();
                return await _context.Set<Account>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public virtual Account GetAccountById(string id)
        {
            return _context.Accounts.Find(id);
        }
        public virtual Account GetByAccountNumber(string? id)
        {
            return _context.Accounts.FirstOrDefault(x => x.AccountNumber == id);
        }

        public virtual async Task<Account> GetAccountByIdAsync(string id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public virtual async Task<Account> GetByAccountNumberAsync(string? number)
        {
            var list = await _context.Accounts.ToListAsync();
            Account entity = list.FirstOrDefault(x => x.AccountNumber == number);
            return entity;
        }

        public virtual void Add(Account entity)
        {
            if (entity != null)
            {
                _context.Accounts.Add(entity);
            }
        }
        public virtual async Task AddAsync(Account entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }
            try
            {
                if (entity != null)
                {
                    await _context.Accounts.AddAsync(entity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }

        public virtual  void UpdateAccount(Account entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(UpdateAccount)} entity must not be null");
            }
            try
            {
                if (entity != null)
                {
                    ShowEntityState(_context);
                    _context.Entry(entity).State = EntityState.Modified;
                    _context.Entry(entity).Property(x => x.AccountId).IsModified = false;
                    _context.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                    _context.Entry(entity).Property(x => x.RowVersion).IsModified = false;
                    _context.Entry(entity).Property(x => x.AccountName).IsModified = false;
                    ShowEntityState(_context);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} state could not be updated: {ex.Message}");
            }
        }

        public virtual void Save()
        {
            _context.SaveChanges();
        }
        public virtual async Task<int> SaveAsync(CancellationToken ct)
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
        public virtual void Delete(string id)
        {
            Account AccountEntity = _context.Accounts.Find(id);
            _context.Accounts.Remove(AccountEntity);
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

        public virtual void TakeMoneyFromAccount(Account entity, decimal? deduction)
        {
            // GET current balance.
            var current = entity.SavingsBalance;

            // DEDUCT from current.
            var remaining = (current - deduction);

            // APPLY remaining balance.
            entity.SavingsBalance = remaining;
            entity.UpdatedDate = DateTime.UtcNow;

            string amountStr = "0.00";
            amountStr = Convert.ToDouble(remaining).ToString("N2");
            entity.UpdatedBy = "Deducted amount: " + amountStr;

            // UPDATE Account state.
            ShowEntityState(_context);
            _context.Entry(entity).State = EntityState.Modified;
            _context.Entry(entity).Property(x => x.AccountName).IsModified = false;
            _context.Entry(entity).Property(x => x.AccountType).IsModified = false;
            _context.Entry(entity).Property(x => x.CheckingBalance).IsModified = false;
            _context.Entry(entity).Property(x => x.CreditBalance).IsModified = false;
            _context.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
            ShowEntityState(_context);
        }
        public virtual void SendMoneyToAccount(Account entity, decimal? amount)
        {
            // GET current balance.
            var current = entity.SavingsBalance;

            // ADD to current.
            var balance = (current + amount);

            // APPLY new balance.
            entity.SavingsBalance = balance;
            entity.UpdatedDate = DateTime.UtcNow;

            string amountStr = "0.00";
            amountStr = Convert.ToDouble(balance).ToString("N2");
            entity.UpdatedBy = "Added amount: " + amountStr;


            // UPDATE Account state.
            ShowEntityState(_context);
            _context.Entry(entity).State = EntityState.Modified;
            _context.Entry(entity).Property(x => x.AccountName).IsModified = false;
            _context.Entry(entity).Property(x => x.AccountType).IsModified = false;
            _context.Entry(entity).Property(x => x.CheckingBalance).IsModified = false;
            _context.Entry(entity).Property(x => x.CreditBalance).IsModified = false;
            _context.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
            ShowEntityState(_context);
        }


    }
}
