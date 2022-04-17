using BusinessLogicLayer;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SimpleBankWebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountsAPIController : ControllerBase
    {
        public AccountsManager? _accountsManager;
        public IConfiguration _configuration;
        // IAccountService _accountService;


        public AccountsAPIController(IConfiguration configuration)
        {
            this._configuration = configuration;
            //_accountService = accountService;
            _accountsManager = new AccountsManager(configuration);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> Get()
        {
            List<Account> list = await _accountsManager.GetAccountsAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> Get(int id)
        {
            try
            {
                if (id == 0)
                {
                    return NotFound();
                }
                var entity = await _accountsManager.GetAccountByIdAsync(id);
                if (entity.Account_ID == 0)
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
            try
            {
                var result = await _accountsManager.AddAsync(entity);
                if (result == "ok")
                {
                    result = entity.AccountName + " account created successfully.";
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

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Account entity)
        {
            try
            {
                if (entity.Account_ID == 0)
                {
                    return NotFound("Item not found.");
                }

                var result = await _accountsManager.UpdateAsync(entity);
                if (result == "ok")
                {
                    result = entity.AccountName + " account updated successfully.";
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

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
