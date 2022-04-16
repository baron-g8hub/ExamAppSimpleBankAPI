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

        public async Task<List<Account>> GetAccounts()
        {
            DataSource = new AccountsDataSource(_configuration);
            return await DataSource.Get();
        }

    }
}
