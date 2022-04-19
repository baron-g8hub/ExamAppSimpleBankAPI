using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace SimpleBankWebAPI.Controllers
{
    public class AccountsController : Controller
    {
        IConfiguration _configuration;
        public AccountsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var vm = new AccountsViewModel();
                var list = new List<Account>();
                using (var httpClient = new HttpClient())
                {
                    var url = "http://" + HttpContext.Request.Host.Value;
                    if (Request.Host.Host == "localhost")
                    {
                        url = "https://" + HttpContext.Request.Host.Value;
                    }
                    using (var response = await httpClient.GetAsync(url + "/AccountsApi/Get"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            vm.EntityList = JsonConvert.DeserializeObject<List<Account>>(apiResponse);
                        }
                    }
                }
                return View(vm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ActionResult> CreateAccount()
        {
            var id = RouteData.Values["id"];
            var vm = new AccountsViewModel();
            ViewBag.accountTypes = vm.LoadAccountTypes();
            if (id != null)
            {
                var entity = new Account();
                using (var httpClient = new HttpClient())
                {
                    var url = "http://" + HttpContext.Request.Host.Value;
                    if (Request.Host.Host == "localhost")
                    {
                        url = "https://" + HttpContext.Request.Host.Value;
                    }
                    using (var response = await httpClient.GetAsync(url + "/AccountsApi/Get/" + id.ToString()))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        entity = JsonConvert.DeserializeObject<Account>(apiResponse);
                        vm.AccountId = entity.AccountId;
                        vm.AccountName = entity.AccountName;
                        vm.AccountType = entity.AccountType;
                        vm.SavingsBalance = entity.SavingsBalance;
                    }
                }
                return View(vm);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccount(AccountsViewModel model)
        {
            try
            {
                var entity = new Account();
                entity.AccountName = model.AccountName.Trim();
                entity.AccountType = model.AccountType;
                entity.SavingsBalance = model.SavingsBalance;
                entity.AccountType_ID = model.AccountType;

                var response = string.Empty;
                using (var httpClient = new HttpClient())
                {
                    var url = "http://" + HttpContext.Request.Host.Value;
                    if (Request.Host.Host == "localhost")
                    {
                        url = "https://" + HttpContext.Request.Host.Value;
                    }
                    if (model.AccountId != 0 || entity.AccountId != 0)
                    {
                        entity.AccountId = model.AccountId;
                        entity.AccountNumber = model.AccountId.ToString();
                        url += "/AccountsApi/Update";
                    }
                    else
                    {
                        url += "/AccountsApi/Add";
                    }

                    var myContent = JsonConvert.SerializeObject(entity);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage result = await httpClient.PostAsync(url, byteContent);
                    if (result.IsSuccessStatusCode)
                    {
                        response = result.StatusCode.ToString();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        string apiResponse = await result.Content.ReadAsStringAsync();
                        if (apiResponse.ToLower().Contains("duplicate"))
                        {
                            ModelState.ClearValidationState("AccountName");
                            ModelState.AddModelError("AccountName", apiResponse);
                        }
                        ViewBag.accountTypes = model.LoadAccountTypes();
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
