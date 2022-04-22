using DataAccessLayer.DataContextEFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using DataAccessLayer.Contracts;


namespace DataAccessLayer.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {

        private ApplicationDBContext _repoContext;
        private IAccountsServiceRepository _account;
        private IPostedTransactionsRepository _transaction;

        public IAccountsServiceRepository Accounts
        {
            get
            {
                if (_account == null)
                {
                    _account = new AccountsServiceRepository(_repoContext);
                }
                return _account;
            }
        }
        public IPostedTransactionsRepository PostedTransactions
        {
            get
            {
                if (_transaction == null)
                {
                    _transaction = new PostedTransactionsRepository(_repoContext);
                }
                return _transaction;
            }
        }

        public RepositoryWrapper(ApplicationDBContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }


     
     


        public async Task SaveAsync()
        {
            await _repoContext.SaveChangesAsync();
        }
    }
}
