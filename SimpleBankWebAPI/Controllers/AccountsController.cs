using DataAccessLayer.Models;
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
                    using (var response = await httpClient.GetAsync(url + "/AccountsApi/GetAccounts"))
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
                    using (var response = await httpClient.GetAsync(url + "/AccountsApi/GetAccount/" + id.ToString()))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        entity = JsonConvert.DeserializeObject<Account>(apiResponse);
                        vm.AccountId = entity.AccountId;
                        vm.AccountName = entity.AccountName;
                        vm.AccountType = 1;
                        vm.SavingsBalance = Convert.ToDouble(entity.SavingsBalance);
                        vm.RowVersion = entity.RowVersion;
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
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = new Account();
                    entity.AccountName = model.AccountName.Trim();
                    entity.AccountType = model.AccountType;
                    entity.SavingsBalance = Convert.ToDecimal(model.SavingsBalance);
                    entity.AccountType = model.AccountType;
                    var response = string.Empty;
                    using (var httpClient = new HttpClient())
                    {
                        var url = "http://" + HttpContext.Request.Host.Value;
                        if (Request.Host.Host == "localhost")
                        {
                            url = "https://" + HttpContext.Request.Host.Value;
                        }
                        if (entity.AccountName != "" && model.RowVersion != null)
                        {
                            entity.AccountId = model.AccountId;
                            entity.AccountNumber = model.AccountId.ToString();
                            url += "/AccountsApi/PutAccount/" + model.AccountName.ToString();
                            var myContent = JsonConvert.SerializeObject(entity);
                            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                            var byteContent = new ByteArrayContent(buffer);
                            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            HttpResponseMessage result = await httpClient.PutAsync(url, byteContent);
                            if (result.IsSuccessStatusCode)
                            {
                                response = result.StatusCode.ToString();
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                string apiResponse = await result.Content.ReadAsStringAsync();
                                ModelState.AddModelError(string.Empty, apiResponse);
                                //ModelState.ClearValidationState("AccountName");
                                //ModelState.AddModelError("AccountName", apiResponse);
                                ViewBag.accountTypes = model.LoadAccountTypes();
                            }
                        }
                        else
                        {
                            url += "/AccountsApi/PostAccount";
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
                                ModelState.AddModelError(string.Empty, apiResponse);
                                //ModelState.ClearValidationState("AccountName");
                                //ModelState.AddModelError("AccountName", apiResponse);
                                ViewBag.accountTypes = model.LoadAccountTypes();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return View(model);
        }


        public ActionResult Details(int id)
        {
            return View();
        }


        //public async Task<ActionResult> EditAccount(string id)
        //{
        //    try
        //    {
        //        var vm = new AccountsViewModel();
        //        var list = new List<Account>();
        //        using (var httpClient = new HttpClient())
        //        {
        //            var url = "http://" + HttpContext.Request.Host.Value;
        //            if (Request.Host.Host == "localhost")
        //            {
        //                url = "https://" + HttpContext.Request.Host.Value;
        //            }
        //            using (var response = await httpClient.GetAsync(url + "/AccountsApi/GetAccounts"))
        //            {
        //                if (response.IsSuccessStatusCode)
        //                {
        //                    string apiResponse = await response.Content.ReadAsStringAsync();
        //                    vm.EntityList = JsonConvert.DeserializeObject<List<Account>>(apiResponse);
        //                }
        //            }
        //        }
        //        return View(vm);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
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
