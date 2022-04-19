using SimpleBankWebAPI.Models;

namespace SimpleBankWebAPI.Contracts
{
    public interface IAccountsServiceRepository : IRepositoryBase<Account>
    {
        IQueryable<Account> SelectAll();
        IEnumerable<Account> GetAll();
        Task<ICollection<Account>> GetAllAsync();
        Account GetById(int Id);
        Task<Account> GetByIdAsync(int? Id);
        Task<Account> GetByAccountIdAsync(int? number);
        Task<Account> GetByAccountNumberAsync(string? number);
        Task<Account> GetByAccountNameAsync(string? name);
        void Add(Account Entity);
        Task AddAsync(Account Entity);
        void Update(Account Entity);
        void Delete(int Id);
        void Save();
        Task<int> SaveAsync(CancellationToken cancellationtoken);

        void TakeMoneyFromAccount(Account entity, decimal? deduction);
        void SendMoneyToAccount(Account entity, decimal? amount);

    }
}
