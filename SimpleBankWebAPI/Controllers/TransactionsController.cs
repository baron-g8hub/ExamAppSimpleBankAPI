using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace SimpleBankWebAPI.Controllers
{
    public class TransactionsController : Controller
    {
        IConfiguration _configuration;
        public TransactionsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
       
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var vm = new TransferViewModel();
                var list = new List<PostedTransaction>();
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
                        var transactions = JsonConvert.DeserializeObject<List<PostedTransaction>>(apiResponse);
                        vm.EntityList = transactions;
                    }
                }
                return View(vm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      
        public async Task<IActionResult> Details(int id)
        {
            var vm = new TransferViewModel();
            var entity = new PostedTransaction();

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
                    entity= JsonConvert.DeserializeObject<PostedTransaction>(apiResponse);
                }
            }
            vm.Entity = entity;
            return View(vm);
        }

        public async Task<ActionResult> Transfer()
        {
            try
            {
                var vm = new TransferViewModel();
                var list = new List<Account>();
                var accounts = new List<SelectListItem>()
                {
                    new SelectListItem { Value = "0", Text = " Select account number " },
                };
                using (var httpClient = new HttpClient())
                {
                    var url = "http://" + HttpContext.Request.Host.Value;
                    if (Request.Host.Host == "localhost")
                    {
                        url = "https://" + HttpContext.Request.Host.Value;
                    }
                    using (var response = await httpClient.GetAsync(url + "/AccountsApi/GetAccounts"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            list = JsonConvert.DeserializeObject<List<Account>>(apiResponse);
                        }
                    }
                }
                accounts.AddRange(list.Select(x => new SelectListItem { Value = x.AccountId.ToString(), Text = x.AccountName }).ToList());

                ViewBag.Accounts = accounts.ToArray();
                return View(vm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transfer(TransferViewModel model)
        {
            try
            {
                var entity = new PostedTransaction();
                entity.AccountNumber = model.SourceAccount;
                entity.Amount = Convert.ToDecimal(model.Amount);
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
                    HttpResponseMessage result = await httpClient.PostAsync(url + "/TransactionsApi/PostTransaction", byteContent);
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
