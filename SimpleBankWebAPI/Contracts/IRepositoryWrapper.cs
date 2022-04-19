namespace SimpleBankWebAPI.Contracts
{
    public interface IRepositoryWrapper
    {
        IAccountsServiceRepository Accounts { get; }
        IPostedTransactionsRepository  PostedTransactions  { get; }
        Task SaveAsync();
    }
}
