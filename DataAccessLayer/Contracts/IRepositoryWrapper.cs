using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataAccessLayer.Contracts
{
    public interface IRepositoryWrapper
    {
        IAccountsServiceRepository Accounts { get; }
        IPostedTransactionsRepository PostedTransactions { get; }


        Task SaveAsync();
        Task<int> SaveAsync(CancellationToken ct);
        Task<int> SaveTransactionAsync(CancellationToken ct);
        Task<bool> UnitTestSaveChangesPostTransactionConcurrency(PostedTransaction postedTransaction);
        
    }
}
