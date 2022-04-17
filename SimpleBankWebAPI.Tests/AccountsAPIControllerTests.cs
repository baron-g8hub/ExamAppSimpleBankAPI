using Entities;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleBankWebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SimpleBankWebAPI.Tests
{
    public class AccountsAPIControllerTests
    {
        public IConfiguration? _configuration;
        AccountsAPIController _controller;


        public AccountsAPIControllerTests()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(Configuration);
            _controller = new AccountsAPIController(_configuration);
        }


        [Fact]
        public async Task GetAccounts_Returns_Correct_Count()
        {
            // Arrange
            int count = 0;
            //var dataStore = A.Fake<IAccountsService>();
            //A.CallTo(() => 
            //var fakeAccounts = A.CollectionOfDummy<Account>(count).AsEnumerable();

            // Act
            var actionResult = await _controller.Get();

            // Assert
            var result = actionResult.Result as OkObjectResult;
            var returnAccounts = result == null ? new List<Account>() : result.Value as IEnumerable<Account>;
            count = returnAccounts == null ? 0 : returnAccounts.Count();
            Assert.Equal(count, returnAccounts?.Count());
        }


        [Theory]
        [InlineData(10000004, 0)]
        //[InlineData("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200", "ab2bd817-98cd-4cf3-a80a-53ea0cd9c111")]
        public async Task GetAccountByIdTest(int guid1, int guid2)
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
            var value = item?.Value as Account;
            Assert.Equal(validGuid, value?.Account_ID);
            Assert.Equal("Ron Testaccount", value?.AccountName);
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