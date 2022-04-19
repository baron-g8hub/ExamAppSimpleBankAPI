using BusinessLogicLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SimpleBankWebAPI.EFCoreDataAccess;
using SimpleBankWebAPI.Models;

namespace SimpleBankWebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountsAPIController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly IAccountsServiceRepository _repository;

        // Define the cancellation token.
        CancellationTokenSource _cts = null;
        public AccountsAPIController(IConfiguration configuration, IAccountsServiceRepository accountsServiceRepository)
        {
            this._configuration = configuration;
            this._repository = accountsServiceRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> Get()
        {
            // List<Account> list = await _accountsManager.GetAccountsAsync();
            var list = await _repository.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> Get(int? id)
        {
            try
            {
                if (id == 0)
                {
                    return NotFound();
                }
                var entity = await _repository.GetByAccountIdAsync(id);
                if (AccountExists(entity.AccountId))
                {
                    return Ok(entity);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return this.StatusCode(400, ex.Message);
            }
        }


        [HttpGet("{id}/{name}")]
        public async Task<ActionResult<Account>> Get(int id, string name)
        {
            try
            {
                if (name == "")
                {
                    return NotFound();
                }
                var entity = await _repository.GetByAccountNameAsync(name);
                if (entity.AccountId == 0)
                {
                    return NotFound();
                }
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return this.StatusCode(400, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Account entity)
        {
            _cts = new CancellationTokenSource();
            // send a cancel after 4000 ms or call cts.Cancel();
            // _cts.CancelAfter(4000);
            CancellationToken ct = _cts.Token;
            try
            {
                var result = "";
                if (!ModelState.IsValid || (entity.AccountName == "" || entity.AccountName == "string"))
                {
                    return BadRequest(ModelState);
                }
                entity.CreatedDate = DateTime.UtcNow;
                entity.UpdatedDate = DateTime.UtcNow;
                entity.CreatedBy = "Admin";
                entity.UpdatedBy = "Admin";
                entity.AccountType = 1;
                if (AccountExists(entity.AccountId))
                {
                    _repository.Update(entity);
                }
                else
                {
                    await _repository.AddAsync(entity);
                }

                int ret = await _repository.SaveAsync(ct);
                if (ret == 1)
                {
                    entity.AccountNumber = entity.AccountId.ToString();
                    _repository.Update(entity);
                    await _repository.SaveAsync(ct);
                }

                if (AccountNameExists(entity.AccountName))
                {
                    result = entity.AccountName + " account created successfully.";
                    return CreatedAtAction("Get", new { name = entity.AccountName }, entity);
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

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Account entity)
        {
            _cts = new CancellationTokenSource();
            // send a cancel after 4000 ms or call cts.Cancel();
            _cts.CancelAfter(4000);
            //Fetch the Token
            CancellationToken ct = _cts.Token;

            try
            {
                if (entity.AccountId == 0 || !AccountExists(entity.AccountId))
                {
                    return NotFound();
                }
                try
                {
                    entity.UpdatedDate = DateTime.UtcNow;
                    entity.UpdatedBy = "Admin";
                    _repository.Update(entity);
                    await _repository.SaveAsync(ct);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes. The Book details was updated by another user, Please reload to get the latest record!");
                    return BadRequest();
                }
                finally
                {
                    _cts.Dispose();
                }
                return Ok("Account updated successfully.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(400, ex.Message);
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
                var entity = await _repository.GetByAccountNumberAsync(number);
                _repository.Delete(entity.AccountId);
                await _repository.SaveAsync(ct);
                var result = "Account deleted successfully.";
                if (!AccountNumberExists(number))
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

                var entity = await _repository.GetByAccountNameAsync(name);
                _repository.Delete(entity.AccountId);
                await _repository.SaveAsync(ct);
                var result = "Account deleted successfully.";
                if (!AccountNameExists(name))
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

        private bool AccountExists(int id)
        {
            return _repository.GetAll().Any(e => e.AccountId == id);
        }
        private bool AccountNameExists(string name)
        {
            return _repository.GetAll().Any(e => e.AccountName == name);
        }
        private bool AccountNumberExists(string number)
        {
            return _repository.GetAll().Any(e => e.AccountNumber == number);
        }
    }
}
