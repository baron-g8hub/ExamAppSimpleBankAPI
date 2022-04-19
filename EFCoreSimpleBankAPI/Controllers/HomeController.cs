using EFCoreSimpleBankAPI.DataAccess;
using EFCoreSimpleBankAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EFCoreSimpleBankAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IAccountRepository _accountRepository;
        public HomeController(ILogger<HomeController> logger, IAccountRepository accountRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _accountRepository.GetAllAccountsAsync());
        }


        //public IActionResult Index()
        //{
        //    return View(_accountRepository.GetAll());
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}