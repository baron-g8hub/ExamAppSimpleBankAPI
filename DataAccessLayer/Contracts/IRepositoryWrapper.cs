﻿namespace DataAccessLayer.Contracts
{
    public interface IRepositoryWrapper
    {
        IAccountsServiceRepository Accounts { get; }
        IPostedTransactionsRepository PostedTransactions { get; }


        Task SaveAsync();
        Task<int> SaveAsync(CancellationToken ct);
    }
}
