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
    public class TransactionsManager
    {
        private TransactionsDataSource _dataSource;

        public IConfiguration _configuration;

        public TransactionsManager(IConfiguration configuration)
        {
            this._configuration = configuration;
            _dataSource = new TransactionsDataSource(_configuration);
        }

        public async Task<List<Transaction>> GetTransactionsAsync()
        {
            List<Transaction> list = new List<Transaction>();
            list = await _dataSource.SelectAsync();
            return list;
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            return await _dataSource.SelectAsync(id);
        }

        public async Task<string> AddTransferTransactionAsync(Transaction transaction)
        {
            transaction.TransactionType_ID = 1;
            transaction.AccountType = 1;
            transaction.Description = "Send money to " + transaction.DestinationAccount;
            string message = await _dataSource.InsertTransactionAsync(transaction);
            return message;
        }


    }
}
