using SimpleBankWebAPI.Models;

namespace SimpleBankWebAPI.EFCoreDataAccess
{
    public interface IAccountsServiceRepository
    {

        IEnumerable<Account> GetAll();
        IQueryable<Account> SelectAll();
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
    }
}
