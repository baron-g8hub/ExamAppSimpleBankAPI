using Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

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


        // GET: TestAccountsController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var vm = new TransferViewModel();
            var entity = new Transaction();

            if (id == 0)
            {
                return RedirectToAction(nameof(Index));
            }
            using (var httpClient = new HttpClient())
            {
                var url = "http://" + HttpContext.Request.Host.Value;
                if (Request.Host.Host == "localhost")
                {
                    url = "https://" + HttpContext.Request.Host.Value;
                }
                using (var response = await httpClient.GetAsync(url + "/TransactionsAPI/Get/" + id.ToString()))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    entity= JsonConvert.DeserializeObject<Transaction>(apiResponse);
                }
            }
            vm.Entity = entity;
            return View(vm);
        }


        public ActionResult Transfer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transfer(TransferViewModel model)
        {
            try
            {
                var entity = new Transaction();
                entity.AccountNumber = model.SourceAccount;
                entity.Amount = model.Amount;
                entity.DestinationAccount = model.DestinationAccount;

                var myContent = JsonConvert.SerializeObject(entity);
                var response = string.Empty;
                using (var httpClient = new HttpClient())
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var url = "http://" + HttpContext.Request.Host.Value;
                    if (Request.Host.Host == "localhost")
                    {
                        url = "https://" + HttpContext.Request.Host.Value;
                    }
                    HttpResponseMessage result = await httpClient.PostAsync(url + "/TransactionsApi/Transfer", byteContent);
                    if (result.IsSuccessStatusCode)
                    {
                        response = result.StatusCode.ToString();
                    }
                    else
                    {
                        string apiResponse = await result.Content.ReadAsStringAsync();
                        if (apiResponse.ToLower().Contains("insufficient"))
                        {
                            ModelState.ClearValidationState("Amount");
                            ModelState.AddModelError("Amount", apiResponse);
                        }
                        else if (apiResponse.ToLower().Contains("source"))
                        {
                            ModelState.ClearValidationState("SourceAccount");
                            ModelState.AddModelError("SourceAccount", apiResponse);
                        }
                        else if (apiResponse.ToLower().Contains("destination"))
                        {
                            ModelState.ClearValidationState("DestinationAccount");
                            ModelState.AddModelError("DestinationAccount", apiResponse);
                        }
                        return View();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
