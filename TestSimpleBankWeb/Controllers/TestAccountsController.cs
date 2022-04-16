using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace TestSimpleBankWeb.Controllers
{
    public class TestAccountsController : Controller
    {
        IConfiguration _configuration;
        public  TestAccountsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // GET: TestAccountsController
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
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        vm.EntityList = JsonConvert.DeserializeObject<List<Account>>(apiResponse);
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
                        vm.Account_ID = entity.Account_ID;
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


                var response = string.Empty;
                using (var httpClient = new HttpClient())
                {
                    var url = "http://" + HttpContext.Request.Host.Value;
                    if (Request.Host.Host == "localhost")
                    {
                        url = "https://" + HttpContext.Request.Host.Value;
                    }
                    if (model.Account_ID != 0 || entity.Account_ID != 0)
                    {
                        entity.Account_ID = model.Account_ID;
                        entity.AccountNumber = model.Account_ID.ToString();
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


        // GET: TestAccountsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TestAccountsController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: TestAccountsController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: TestAccountsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TestAccountsController/Edit/5
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

        // GET: TestAccountsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TestAccountsController/Delete/5
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
