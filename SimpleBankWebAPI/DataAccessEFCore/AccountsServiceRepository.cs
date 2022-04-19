using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using SimpleBankWebAPI.Models;

namespace SimpleBankWebAPI.EFCoreDataAccess
{
    public class AccountsServiceRepository : IAccountsServiceRepository
    {
        private readonly BankingContext _context;
        public AccountsServiceRepository(BankingContext context)
        {
            _context = context;
        }

        public IQueryable<Account> SelectAll()
        {
            return _context.Set<Account>();
        }
        public IEnumerable<Account> GetAll()
        {
            return _context.Accounts.ToList();
        }
        public async Task<ICollection<Account>> GetAllAsync()
        {
            try
            {
                //var list = await _context.Accounts.ToListAsync();
                return await _context.Set<Account>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }
        public virtual Account GetById(int Id)
        {
            return _context.Accounts.Find(Id);
        }
        public virtual async Task<Account> GetByIdAsync(int? Id)
        {
            return await _context.Accounts.FindAsync(Id.Value);
        }
        public virtual async Task<Account> GetByAccountIdAsync(int? accountId)
        {
            var list = await _context.Accounts.ToListAsync();
            
            Account entity = list.FirstOrDefault(x => x.AccountId == accountId);
            return entity;
        }
        public virtual async Task<Account> GetByAccountNumberAsync(string? number)
        {
            var list = await _context.Accounts.ToListAsync();
            Account entity = list.FirstOrDefault(x => x.AccountNumber == number);
            return entity;
        }
        public virtual async Task<Account> GetByAccountNameAsync(string? name)
        {
            var list = await _context.Accounts.ToListAsync();
            Account entity = list.FirstOrDefault(x => x.AccountName == name);
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
                    ShowEntityState(_context);
                    _context.Entry(entity).State = EntityState.Modified;
                    _context.Entry(entity).Property(x => x.AccountId).IsModified = false;
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
        public virtual void Delete(int Id)
        {
            Account AccountEntity = _context.Accounts.Find(Id);
            _context.Accounts.Remove(AccountEntity);
        }
        public static void ShowEntityState(BankingContext context)
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
