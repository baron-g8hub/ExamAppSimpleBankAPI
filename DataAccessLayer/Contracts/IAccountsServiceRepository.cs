using DataAccessLayer.Models;


namespace DataAccessLayer.Contracts
{
    public interface IAccountsServiceRepository : IRepositoryBase<Account>
    {
        IQueryable<Account> GetAllAccounts();
        Task<ICollection<Account>> GetAllAccountsAsync();
        Account GetAccountById(string id);
        Task<Account> GetAccountByIdAsync(string id);
        Task<Account> GetByAccountNumberAsync(string? number);
        
        void Add(Account Entity);
        Task AddAsync(Account Entity);
        void UpdateAccount(Account Entity);
        void Delete(string id);
        void Save();
        Task<int> SaveAsync(CancellationToken cancellationtoken);
        void TakeMoneyFromAccount(Account entity, decimal? deduction);
        void SendMoneyToAccount(Account entity, decimal? amount);

    }
}
