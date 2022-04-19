using EFCoreSimpleBankAPI.DataAccess;
using EFCoreSimpleBankAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EFCoreSimpleBankAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountsAPIController : ControllerBase
    {
        public IAccountRepository _accountRepository;
        // Define the cancellation token.
        CancellationTokenSource _cts = null;

        public AccountsAPIController(IAccountRepository accountRepository)
        {
            #region MyRegion
            //MongoCRUD db = new MongoCRUD("Books");
            //var record = db.GetBookById("Book",new Guid("c16d4bf3-8328-4563-91b6-4a501c0329c2"));
            #endregion
            _accountRepository = accountRepository;
        }




        //public async Task<ActionResult<IEnumerable<Account>>> LoadAll()
        //{
        //    return await _accountRepository.GetAllAccountsAsync();
        //}

        //// GET: api/AccountsAPI
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Account>>> Get()
        //{
        //    return await _accountRepository.GetAllAccountsAsync();
        //}

        //// GET: api/AccountsAPI/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Account>> GetAccount(string id)
        //{
        //    var account = await _context.Account.FindAsync(id);

        //    if (account == null)
        //    {
        //        return NotFound();
        //    }

        //    return account;
        //}

        //// PUT: api/AccountsAPI/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutAccount(string id, Account account)
        //{
        //    if (id != account.AccountName)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(account).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!AccountExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/AccountsAPI
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Account>> PostAccount(Account account)
        //{
        //    _context.Account.Add(account);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (AccountExists(account.AccountName))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetAccount", new { id = account.AccountName }, account);
        //}

        //// DELETE: api/AccountsAPI/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteAccount(string id)
        //{
        //    var account = await _context.Account.FindAsync(id);
        //    if (account == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Account.Remove(account);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool AccountExists(string id)
        //{
        //    return _accountRepository.GetAllAccounts().Any(e => e.AccountName == id);
        //}

        //private bool BookExists(int id)
        //{
        //    return _bookRepository.GetAllBooks().Any(e => e.BookId == id);
        //}
    }
}
