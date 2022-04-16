using DataAccessLayer;
using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class AccountsManager
    {
        private AccountsDataSource dataSource;

        public IConfiguration _configuration;

        public AccountsDataSource DataSource { get => dataSource; set => dataSource = value; }

        public AccountsManager(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public async Task<List<Account>> GetAccountsAsync()
        {
            DataSource = new AccountsDataSource(_configuration);
            return await DataSource.SelectAsync();
        }

        public async Task<Account> GetAccountByIdAsync(int id)
        {
            DataSource = new AccountsDataSource(_configuration);
            return await DataSource.SelectAsync(id);
        }

        public async Task<string> AddAsync(Account entity)
        {
            if (entity.AccountNumber != "" || entity.Account_ID != 0)
            {
                return await UpdateAsync(entity);
            }
            else
            {
                entity.CreatedDate = DateTime.UtcNow;
                entity.UpdatedDate = DateTime.UtcNow;
                entity.CreatedBy = "Admin";
                entity.UpdatedBy = "Admin";
                string message = await DataSource.InsertAsync(entity);
                return message;
            }
        }

        public async Task<string> UpdateAsync(Account entity)
        {
            entity.UpdatedDate = DateTime.UtcNow;
            entity.UpdatedBy = "Admin";
            string message = await DataSource.UpdateAsync(entity);
            return message;
        }


    }
}
