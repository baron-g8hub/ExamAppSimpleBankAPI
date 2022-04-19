using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using SimpleBankWebAPI.Contracts;
using SimpleBankWebAPI.Models;

namespace SimpleBankWebAPI.Repository
{
    public class AccountsServiceRepository : RepositoryBase<Account>, IAccountsServiceRepository
    {
        public AccountsServiceRepository(RepositoryContext context) : base(context)
        {
        }

        public IQueryable<Account> SelectAll()
        {
            return RepositoryContext.Set<Account>();
        }
        public IEnumerable<Account> GetAll()
        {
            return RepositoryContext.Accounts.ToList();
        }
        public virtual async Task<ICollection<Account>> GetAllAsync()
        {
            try
            {
                //var list = await RepositoryContext.Accounts.ToListAsync();
                return await RepositoryContext.Set<Account>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }
        public virtual Account GetById(int Id)
        {
            return RepositoryContext.Accounts.Find(Id);
        }
        public virtual async Task<Account> GetByIdAsync(int? Id)
        {
            return await RepositoryContext.Accounts.FindAsync(Id.Value);
        }
        public virtual async Task<Account> GetByAccountIdAsync(int? accountId)
        {
            var list = await RepositoryContext.Accounts.ToListAsync();

            Account entity = list.FirstOrDefault(x => x.AccountId == accountId);
            return entity;
        }
        public virtual async Task<Account> GetByAccountNumberAsync(string? number)
        {
            var list = await RepositoryContext.Accounts.ToListAsync();
            Account entity = list.FirstOrDefault(x => x.AccountNumber == number);
            return entity;
        }
        public virtual async Task<Account> GetByAccountNameAsync(string? name)
        {
            var list = await RepositoryContext.Accounts.ToListAsync();
            Account entity = list.FirstOrDefault(x => x.AccountName == name);
            return entity;
        }
        public virtual void Add(Account entity)
        {
            if (entity != null)
            {
                RepositoryContext.Accounts.Add(entity);
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
                    await RepositoryContext.Accounts.AddAsync(entity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }
        public virtual void Update(Account entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(Update)} entity must not be null");
            }
            try
            {
                if (entity != null)
                {
                    ShowEntityState(RepositoryContext);
                    RepositoryContext.Entry(entity).State = EntityState.Modified;
                    RepositoryContext.Entry(entity).Property(x => x.AccountId).IsModified = false;
                    ShowEntityState(RepositoryContext);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} state could not be updated: {ex.Message}");
            }
        }
        public virtual void Save()
        {
            RepositoryContext.SaveChanges();
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
                using (tx = await RepositoryContext.Database.BeginTransactionAsync())
                {
                    records = await RepositoryContext.SaveChangesAsync();
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
        public virtual void Delete(int Id)
        {
            Account AccountEntity = RepositoryContext.Accounts.Find(Id);
            RepositoryContext.Accounts.Remove(AccountEntity);
        }
        public static void ShowEntityState(RepositoryContext context)
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
                    RepositoryContext.Dispose();
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
            ShowEntityState(RepositoryContext);
            RepositoryContext.Entry(entity).State = EntityState.Modified;
            RepositoryContext.Entry(entity).Property(x => x.AccountName).IsModified = false;
            RepositoryContext.Entry(entity).Property(x => x.AccountType).IsModified = false;
            RepositoryContext.Entry(entity).Property(x => x.AccountNumber).IsModified = false;
            RepositoryContext.Entry(entity).Property(x => x.AccountNumber).IsModified = false;
            RepositoryContext.Entry(entity).Property(x => x.CheckingBalance).IsModified = false;
            RepositoryContext.Entry(entity).Property(x => x.CreditBalance).IsModified = false;
            RepositoryContext.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
            RepositoryContext.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
            ShowEntityState(RepositoryContext);
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
            ShowEntityState(RepositoryContext);
            RepositoryContext.Entry(entity).State = EntityState.Modified;
            RepositoryContext.Entry(entity).Property(x => x.AccountName).IsModified = false;
            RepositoryContext.Entry(entity).Property(x => x.AccountType).IsModified = false;
            RepositoryContext.Entry(entity).Property(x => x.AccountNumber).IsModified = false;
            RepositoryContext.Entry(entity).Property(x => x.AccountNumber).IsModified = false;
            RepositoryContext.Entry(entity).Property(x => x.CheckingBalance).IsModified = false;
            RepositoryContext.Entry(entity).Property(x => x.CreditBalance).IsModified = false;
            RepositoryContext.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
            RepositoryContext.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
            ShowEntityState(RepositoryContext);
        }
    }
}
