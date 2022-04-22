using DataAccessLayer.Contracts;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;



namespace SimpleBankWebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountsAPIController : ControllerBase
    {
        public IConfiguration _configuration;
        private IRepositoryWrapper _repository;

        // Define the cancellation token.
        CancellationTokenSource? _cts = null;
        public AccountsAPIController(IConfiguration configuration, IRepositoryWrapper repository)
        {
            _configuration = configuration;
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            // List<Account> list = await _accountsManager.GetAccountsAsync();
            var list = await _repository.Accounts.GetAllAccountsAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(string id)
        {
            var account = await _repository.Accounts.GetAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return account;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(string id, Account account)
        {
            _cts = new CancellationTokenSource();

            // send a cancel after 4000 ms or call cts.Cancel();
            _cts.CancelAfter(4000);

            //Fetch the Token
            CancellationToken ct = _cts.Token;

            if (id != account.AccountName)
            {
                return BadRequest();
            }
            //account.UpdatedDate = DateTime.UtcNow;
            //account.UpdatedBy = "Admin";
            //account.CreatedDate = DateTime.UtcNow;
            //account.CreatedBy = "Admin";
            var entity = _repository.Accounts.GetAccountById(id);
            entity.SavingsBalance = account.SavingsBalance;
            _repository.Accounts.UpdateAccount(entity);
            try
            {
                await _repository.SaveAsync(ct);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!AccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, ex.Message.ToString());
                    return BadRequest();
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message.ToString());
                return BadRequest();
            }
            finally
            {
                _cts.Dispose();
            }
            return NoContent();
        }





        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(Account account)
        {
            _cts = new CancellationTokenSource();
            // send a cancel after 4000 ms or call cts.Cancel();
            // _cts.CancelAfter(4000);
            CancellationToken ct = _cts.Token;

            try
            {
                account.CreatedDate = DateTime.UtcNow;
                account.UpdatedDate = DateTime.UtcNow;
                account.CreatedBy = "Admin";
                account.UpdatedBy = "Admin";
                account.AccountType = 1;
                await _repository.Accounts.AddAsync(account);
                int ret = await _repository.SaveAsync(ct);
                if (ret == 1)
                {
                    account.AccountNumber = account.AccountId.ToString();
                    _repository.Accounts.UpdateAccount(account);
                    await _repository.SaveAsync(ct);
                }
                return CreatedAtAction("GetAccount", new { id = account.AccountName }, account);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpDelete("{id}/{number}")]
        public async Task<IActionResult> DeleteByAccountNumber(int id, string number)
        {
            try
            {
                _cts = new CancellationTokenSource();
                // send a cancel after 4000 ms or call cts.Cancel();
                _cts.CancelAfter(4000);
                CancellationToken ct = _cts.Token;
                var entity = await _repository.Accounts.GetByAccountNumberAsync(number);
                _repository.Accounts.Delete(entity.AccountName);
                await _repository.SaveAsync(ct);
                var result = "Account deleted successfully.";
                if (!AccountExists(number))
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(400, ex.Message);
            }
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteByAccountName(string name)
        {
            try
            {
                _cts = new CancellationTokenSource();
                // send a cancel after 4000 ms or call cts.Cancel();
                _cts.CancelAfter(4000);
                CancellationToken ct = _cts.Token;

                var entity = await _repository.Accounts.GetAccountByIdAsync(name);
                _repository.Accounts.Delete(entity.AccountName);
                await _repository.SaveAsync(ct);
                var result = "Account deleted successfully.";
                if (!AccountExists(name))
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(400, ex.Message);
            }
        }

        private bool AccountExists(string id)
        {
            return _repository.Accounts.GetAllAccounts().Any(e => e.AccountName == id);
        }
    }
}
