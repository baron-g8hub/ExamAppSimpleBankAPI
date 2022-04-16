using Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TestSimpleBankWeb.Controllers
{
    public class TransactionsController : Controller
    {
        IConfiguration _configuration;
        public TransactionsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // GET: TestAccountsController
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var vm = new TransferViewModel();
                var list = new List<Transaction>();
                using (var httpClient = new HttpClient())
                {
                    var url = "http://" + HttpContext.Request.Host.Value;
                    if (Request.Host.Host == "localhost")
                    {
                        url = "https://" + HttpContext.Request.Host.Value;
                    }

                    using (var response = await httpClient.GetAsync(url + "/TransactionsAPI/Get"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        vm.EntityList = JsonConvert.DeserializeObject<List<Transaction>>(apiResponse);
                    }
                }
                return View(vm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
