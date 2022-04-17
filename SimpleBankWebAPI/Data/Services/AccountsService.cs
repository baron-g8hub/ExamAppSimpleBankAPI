using BusinessLogicLayer;
using Entities;

namespace SimpleBankWebAPI
{
    public class AccountService : IAccountService
    {
        AccountsManager _accountsManager;
        List<Account> _accounts;


        public AccountService(IConfiguration configuration)
        {
            _accountsManager = new AccountsManager(configuration);
        }

        async Task<IEnumerable<Account>> IAccountService.GetAll()
        {
            _accounts = await _accountsManager.GetAccountsAsync();
            return _accounts;
        }


        Account IAccountService.Add(Account newBook)
        {
            return newBook;
        }

       

        Account IAccountService.GetById(Guid id)
        {
            return new Account();
        }

        void IAccountService.Remove(Guid id)
        {
            //throw new NotImplementedException();
        }
    }
}
