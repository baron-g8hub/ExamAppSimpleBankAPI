using Entities;

namespace SimpleBankWebAPI
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAll();
        Account Add(Account account);
        Account GetById(Guid id);
        void Remove(Guid id);
    }
}
