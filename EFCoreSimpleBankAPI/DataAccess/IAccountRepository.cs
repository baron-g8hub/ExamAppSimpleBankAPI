using EFCoreSimpleBankAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EFCoreSimpleBankAPI.DataAccess
{
    public interface IAccountRepository : IDisposable
    {
        IEnumerable<Account> GetAll();
        IQueryable<Account> GetAllAccounts();
        Task<ICollection<Account>> GetAllAccountsAsync();
        Account GetAccountById(int AccountId);
        Task<Account> GetAccountByIdAsync(int? AccountId);
        void AddAccount(Account AccountEntity);
        Task AddAccountAsync(Account AccountEntity);
        void UpdateAccount(Account AccountEntity);
        void DeleteAccount(int AccountId);
        void Save();
        Task<int> SaveAsync(CancellationToken cancellationtoken);
    }
}
