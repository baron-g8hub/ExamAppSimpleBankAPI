using BusinessLogicLayer;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TestSimpleBankWeb.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    //[Route("[controller]")]
    public class AccountsAPIController : ControllerBase
    {
        private AccountsManager _accountsManager;

        private IConfiguration _configuration;
        public AccountsAPIController(IConfiguration configuration)
        {
            _configuration = configuration;
            _accountsManager = new AccountsManager(_configuration);
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var list = await _accountsManager.GetAccountsAsync();
            return Ok(JsonConvert.SerializeObject(list).ToString());
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id == 0)
                {
                    return NotFound("Item not found.");
                }
                var entity = await _accountsManager.GetAccountByIdAsync(id);
                if (entity.Account_ID == 0)
                {
                    return NotFound("Item Not Found.");
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
        public async Task<IActionResult> Update(int id, [FromBody] Account entity)
        {
            try
            {
                if (id == 0)
                {
                    return NotFound("Item not found.");
                }

                var result = await _accountsManager.UpdateAsync(entity);
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

        // PUT api/<AccountsAPIController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AccountsAPIController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
