using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SimpleBankWebAPI.Contracts;
using SimpleBankWebAPI.Models;

namespace SimpleBankWebAPI
{
    public class TransactionService
    {
        private IRepositoryWrapper _repository;

        public TransactionService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        //public async Task<int> PostTransactionAsync(CancellationToken ct, PostingTransactionWrapper wrapper)
        //{
        //    int records = 0;
        //    IDbContextTransaction tx = null;
        //    //await Task.Delay(5000);
        //    if (ct.IsCancellationRequested)
        //    {
        //        ct.ThrowIfCancellationRequested();
        //    }
        //    try
        //    {
        //        using (tx = await _repository.co.Database.BeginTransactionAsync())
        //        {
        //            try
        //            {
        //                // 1. TakeMoneyFromAccount
        //                var takeAmount = wrapper.PostedTransaction.Amount;
        //                TakeMoneyFromAccount(wrapper.SourceAccount, takeAmount);

        //                // 2. SendMoneyToAccount 
        //                SendMoneyToAccount(wrapper.DestinationAccount, takeAmount);

        //                // 3. Assemble Transaction Details
        //                PostedTransaction postedTransaction = wrapper.PostedTransaction;

        //                // 4. Add PostingTransaction 
        //                await AddAsync(postedTransaction);

        //                // 5. Save state changes
        //                records = await _context.SaveChangesAsync();

        //                // *** If we reach here then DBUPDATE is successfull. ***

        //                // 6. Commit changes from state to database
        //                await tx.CommitAsync();
        //                return records;
        //            }
        //            catch (Exception)
        //            {
        //                // NOTE: Apply Rollback when failed to process.
        //                //throw;
        //            }
        //        }
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        foreach (var entry in ex.Entries)
        //        {
        //            if (entry.Entity is Account)
        //            {
        //                var proposedValues = entry.CurrentValues;
        //                var databaseValues = entry.GetDatabaseValues();

        //                foreach (var property in proposedValues.Properties)
        //                {
        //                    var proposedValue = proposedValues[property];
        //                    var databaseValue = databaseValues[property];
        //                }

        //                // Refresh original values to bypass next concurrency check
        //                entry.OriginalValues.SetValues(databaseValues);
        //            }
        //            else
        //            {
        //                throw new NotSupportedException("Unable to save changes. The book details was updated by another user. " + entry.Metadata.Name);
        //            }
        //        }
        //        throw ex;
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        SqlException s = ex.InnerException as SqlException;
        //        var errorMessage = $"{ex.Message}" + " {ex?.InnerException.Message}" + " rolling back…";
        //        tx.Rollback();
        //    }
        //    return records;
        //}
        //public void TakeMoneyFromAccount(Account entity, decimal? deduction)
        //{
        //    // GET current balance.
        //    var current = entity.SavingsBalance;

        //    // DEDUCT from current.
        //    var remaining = (current - deduction);

        //    // APPLY remaining balance.
        //    entity.SavingsBalance = remaining;
        //    entity.UpdatedDate = DateTime.UtcNow;

        //    string amountStr = "0.00";
        //    amountStr = Convert.ToDouble(remaining).ToString("N2");
        //    entity.UpdatedBy = "Deducted amount: " + amountStr;

        //    // UPDATE Account state.
        //    ShowEntityState(_context);
        //    _context.Entry(entity).State = EntityState.Modified;
        //    _context.Entry(entity).Property(x => x.AccountName).IsModified = false;
        //    _context.Entry(entity).Property(x => x.AccountType).IsModified = false;
        //    _context.Entry(entity).Property(x => x.AccountNumber).IsModified = false;
        //    _context.Entry(entity).Property(x => x.AccountNumber).IsModified = false;
        //    _context.Entry(entity).Property(x => x.CheckingBalance).IsModified = false;
        //    _context.Entry(entity).Property(x => x.CreditBalance).IsModified = false;
        //    _context.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
        //    _context.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
        //    ShowEntityState(_context);
        //}
        //public void SendMoneyToAccount(Account entity, decimal? amount)
        //{
        //    // GET current balance.
        //    var current = entity.SavingsBalance;

        //    // ADD to current.
        //    var balance = (current + amount);

        //    // APPLY new balance.
        //    entity.SavingsBalance = balance;
        //    entity.UpdatedDate = DateTime.UtcNow;

        //    string amountStr = "0.00";
        //    amountStr = Convert.ToDouble(balance).ToString("N2");
        //    entity.UpdatedBy = "Added amount: " + amountStr;


        //    // UPDATE Account state.
        //    ShowEntityState(_context);
        //    _context.Entry(entity).State = EntityState.Modified;
        //    _context.Entry(entity).Property(x => x.AccountName).IsModified = false;
        //    _context.Entry(entity).Property(x => x.AccountType).IsModified = false;
        //    _context.Entry(entity).Property(x => x.AccountNumber).IsModified = false;
        //    _context.Entry(entity).Property(x => x.AccountNumber).IsModified = false;
        //    _context.Entry(entity).Property(x => x.CheckingBalance).IsModified = false;
        //    _context.Entry(entity).Property(x => x.CreditBalance).IsModified = false;
        //    _context.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
        //    _context.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
        //    ShowEntityState(_context);
        //}

    }
}
