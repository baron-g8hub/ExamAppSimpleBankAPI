using Entities;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleBankWebAPI.Controllers;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SimpleBankWebAPI.Tests
{
    public class AccountsAPIControllerTests
    {
        IConfiguration _configuration;

        public AccountsAPIControllerTests()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(Configuration);
        }


        [Fact]
        public async Task GetAccounts_Returns_Correct_Count()
        {
            // Arrange
            int count = 5;
            //var dataStore = A.Fake<IAccountsService>();
            //A.CallTo(() => 
            var controller = new AccountsAPIController(_configuration);
            //var fakeAccounts = A.CollectionOfDummy<Account>(count).AsEnumerable();

            // Act
            var actionResult = await controller.Get();

            // Assert
            var result = actionResult.Result as OkObjectResult;
            var returnAccounts = result.Value as IEnumerable<Account>;
            count = returnAccounts.Count();

            Assert.Equal(count, returnAccounts.Count());
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