using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleBankWebAPI.Controllers;
using SimpleBankWebAPI.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleBankWebAPI.Tests
{
    public class TransactionsAPIControllerTests
    {
        public IConfiguration? _configuration;
        TransactionsAPIController _controller;
        private readonly IPostedTransactionsRepository _repository;

        public TransactionsAPIControllerTests()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(Configuration);
            _controller = new TransactionsAPIController(_configuration, _repository);
        }


        [Fact]
        public async Task Get_Transactions_Returns_Correct_Count()
        {
            // Arrange
            int count = 0;

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);

            var list = result.Result as OkObjectResult;
            Assert.IsType<List<Transaction>>(list.Value);

            var returnTransactions = list == null ? new List<Transaction>() : list.Value as IEnumerable<Transaction>;
            count = returnTransactions == null ? 0 : returnTransactions.Count();
            Assert.Equal(count, returnTransactions?.Count());
        }


        [Theory]
        [InlineData(26, 0)]
        //[InlineData("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200", "ab2bd817-98cd-4cf3-a80a-53ea0cd9c111")]
        public async Task Get_TransactionByIdTest(int guid1, int guid2)
        {
            //Arrange
            var validGuid = guid1;      // new Guid(guid1);
            var invalidGuid = guid2;    // new Guid(guid2);

            //Act
            var okResult = await _controller.Get(validGuid);
            var notFoundResult = await _controller.Get(invalidGuid);

            //Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
            Assert.IsType<NotFoundResult>(notFoundResult.Result);

            //Now we need to check the value of the result for the ok object result.
            var item = okResult.Result as OkObjectResult;

            //We Expect to return a single item
            Assert.IsType<Account>(item?.Value);

            //Now, let us check the value itself.
            var value = item?.Value as Transaction;
            Assert.Equal(validGuid, value?.Transaction_ID);
            // Assert.Equal("Ron Testaccount", value?.AccountNumber);
        }


        [Fact]
        public async void Create_New_Transaction_Test()
        {
            //Arrange
            var completeAccount = new Transaction()
            {
                Transaction_ID = 0,
                AccountNumber = "10000004",
                DestinationAccount = "10000012",
                AccountType = 1,
                TransactionType_ID = 1,
                Description = "Unit Test in Transfer funds into another account.",
                PostingDateStr = "",
                TransactionTypeName = "",
                PostingDate = DateTime.UtcNow
            };
            //var responseDelete = await _controller.DeleteByAccountName(completeAccount.AccountName);


            //Act
            var createdResponse = await _controller.Transfer(completeAccount);


            //Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse);


            //value of the result
            var item = createdResponse as CreatedAtActionResult;
            Assert.IsType<Transaction>(item.Value);

            //check value of this transaction
            var transactionItem = item.Value as Transaction;
            Assert.Equal(completeAccount.AccountNumber, transactionItem.AccountNumber);
            Assert.Equal(completeAccount.DestinationAccount, transactionItem.DestinationAccount);
            Assert.Equal(completeAccount.Amount, transactionItem.Amount);


            //Arrange
            var incompleteAccount = new Transaction()
            {
                AccountNumber = "",
                DestinationAccount = "",
                AccountType = 0,
                TransactionType_ID = 0
            };

            //Act
            _controller.ModelState.AddModelError("AccountNumber", "Account number is requried.");
            var badResponse = await _controller.Transfer(incompleteAccount);

            //Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }






















        public IConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", optional: false);
                    //builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    //builder.AddJsonFile($"appsettings.Dev.json", optional: true);
                    //builder.AddEnvironmentVariables();
                    _configuration = builder.Build();
                }
                return _configuration;
            }
        }
    }
}
