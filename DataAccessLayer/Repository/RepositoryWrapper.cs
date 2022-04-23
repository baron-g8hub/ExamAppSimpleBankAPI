using DataAccessLayer.DataContextEFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using DataAccessLayer.Contracts;
using DataAccessLayer.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private ApplicationDBContext _repoContext;
        private IAccountsServiceRepository? _account;
        private IPostedTransactionsRepository? _transaction;

        public RepositoryWrapper(ApplicationDBContext context)
        {
            _repoContext = context ?? throw new ArgumentNullException(nameof(context));
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
            IDbContextTransaction? transaction = null;
            //await Task.Delay(5000);
            if (ct.IsCancellationRequested)
            {
                ct.ThrowIfCancellationRequested();
            }
            try
            {
                using (transaction = await _repoContext.Database.BeginTransactionAsync())
                {
                    records = await _repoContext.SaveChangesAsync();
                    await transaction.CommitAsync();
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
                            var databaseValue = databaseValues?[property];
                        }

                        // Refresh original values to bypass next concurrency check
                        if (databaseValues != null)
                        {
                            entry.OriginalValues.SetValues(databaseValues);
                        }
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
                transaction?.Rollback();
                //  throw s;
            }
            return records;
        }



        public async Task<int> SaveTransactionAsync(CancellationToken ct)
        {

            int records = 0;
            IDbContextTransaction? transaction = null;
            //await Task.Delay(5000);
            if (ct.IsCancellationRequested)
            {
                ct.ThrowIfCancellationRequested();
            }
            try
            {
                using (transaction = await _repoContext.Database.BeginTransactionAsync())
                {
                    records = await _repoContext.SaveChangesAsync();
                    await transaction.CommitAsync();
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
                            var databaseValue = databaseValues?[property];
                        }

                        // Refresh original values to bypass next concurrency check
                        if (databaseValues != null)
                        {
                            entry.OriginalValues.SetValues(databaseValues);
                        }
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
                transaction?.Rollback();
                //throw s;
            }
            return records;
        }


        public async Task<bool> UnitTestSaveChangesPostTransactionConcurrency(PostedTransaction postedTransaction)
        {
            var entity = _repoContext.Accounts?.Single(s => s.AccountNumber == postedTransaction.AccountNumber);
            if (entity != null)
            {
                entity.SavingsBalance = 300;
                postedTransaction.Description = "UnitTestSaveChangesPostTransactionConcurrency set Available Balance = 300";
            }

            // Change the person's name in the database to simulate a concurrency conflict
            _repoContext.Database.ExecuteSqlRaw("UPDATE dbo.Accounts SET SavingsBalance = 5 WHERE AccountName = 'UnitTestAccount'");

            var saved = false;
            while (!saved)
            {
                try
                {
                    // Attempt to save changes to the database
                    await _repoContext.SaveChangesAsync();
                    saved = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (EntityEntry entry in ex.Entries)
                    {
                        if (entry.Entity is Account)
                        {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();

                            foreach (var property in proposedValues.Properties)
                            {
                                var proposedValue = proposedValues[property];
                                var databaseValue = databaseValues?[property];
                            }
                            // Refresh original values to bypass next concurrency check
                            if (databaseValues != null)
                            {
                                entry.OriginalValues.SetValues(databaseValues);
                            }
                        }
                        else
                        {
                            throw new NotSupportedException("Don't know how to handle concurrency conflicts for " + entry.Metadata.Name);
                        }
                    }
                }
            }
            return saved;
        }






        public void ValidateConcurrencyConflict(DbUpdateConcurrencyException ex, EntityEntry entityEntry)
        {
            //var exceptionEntry = ex.Entries.Single();
            //var clientValues = nameof()exceptionEntry.Entity;
            //var databaseEntry = exceptionEntry.GetDatabaseValues();
            //if (databaseEntry == null)
            //{
            //    ModelState.AddModelError(string.Empty, "Unable to save changes. The Entity was deleted by another user.");
            //}
            //else
            //{
            //    var databaseValues = (Department)databaseEntry.ToObject();

            //    if (databaseValues.Name != clientValues.Name)
            //    {
            //        ModelState.AddModelError("Name", $"Current value: {databaseValues.Name}");
            //    }
            //    if (databaseValues.Budget != clientValues.Budget)
            //    {
            //        ModelState.AddModelError("Budget", $"Current value: {databaseValues.Budget:c}");
            //    }
            //    if (databaseValues.StartDate != clientValues.StartDate)
            //    {
            //        ModelState.AddModelError("StartDate", $"Current value: {databaseValues.StartDate:d}");
            //    }
            //    if (databaseValues.InstructorID != clientValues.InstructorID)
            //    {
            //        Instructor databaseInstructor = await _context.Instructors.FirstOrDefaultAsync(i => i.ID == databaseValues.InstructorID);
            //        ModelState.AddModelError("InstructorID", $"Current value: {databaseInstructor?.FullName}");
            //    }

            //    ModelState.AddModelError(string.Empty, "The record you attempted to edit "
            //            + "was modified by another user after you got the original value. The "
            //            + "edit operation was canceled and the current values in the database "
            //            + "have been displayed. If you still want to edit this record, click "
            //            + "the Save button again. Otherwise click the Back to List hyperlink.");
            //    departmentToUpdate.RowVersion = (byte[])databaseValues.RowVersion;
            //    ModelState.Remove("RowVersion");
            //}
        }


    }
}
