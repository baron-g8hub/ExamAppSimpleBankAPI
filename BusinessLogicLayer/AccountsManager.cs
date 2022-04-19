﻿using DataAccessLayer;
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
        private AccountsDataSource _dataSource;
        public IConfiguration _configuration;

        public AccountsManager(IConfiguration configuration)
        {
            this._configuration = configuration;
            _dataSource = new AccountsDataSource(_configuration);
        }

        public async Task<List<Account>> GetAccountsAsync()
        {
            List<Account> list = new List<Account>();
            list = await _dataSource.SelectAsync();
            return list;
        }

        public async Task<Account> GetAccountByIdAsync(int id)
        {
            return await _dataSource.SelectAsync(id);
        }

        public async Task<Account> GetByAccountNameAsync(string  name)
        {
            return await _dataSource.SelectAsync(name);
        }

        public async Task<string> AddAsync(Account entity)
        {
            if (entity.AccountNumber != "string" && entity.AccountId != 0 && entity.AccountNumber != "")
            {
                return await UpdateAsync(entity);
            }
            else
            {
                entity.CreatedDate = DateTime.UtcNow;
                entity.UpdatedDate = DateTime.UtcNow;
                entity.CreatedBy = "Admin";
                entity.UpdatedBy = "Admin";
                string message = await _dataSource.InsertAsync(entity);
                return message;
            }
        }

        public async Task<string> UpdateAsync(Account entity)
        {
            entity.UpdatedDate = DateTime.UtcNow;
            entity.UpdatedBy = "Admin";
            string message = await _dataSource.UpdateAsync(entity);
            return message;
        }

        public async Task<string> DeleteByAccountNumberAsync(string number)
        {
            string message = await _dataSource.DeleteByAccountNumberAsync(number);
            return message;
        }

        public async Task<string> DeleteByAccountNameAsync(string name)
        {
            string message = await _dataSource.DeleteByAccountNameAsync(name);
            return message;
        }
    }
}
