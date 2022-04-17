using BusinessLogicLayer;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SimpleBankWebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TransactionsAPIController : ControllerBase
    {
        public TransactionsManager _transactionsManager;

        public IConfiguration _configuration;
        public TransactionsAPIController(IConfiguration configuration)
        {
            _configuration = configuration;
            _transactionsManager = new TransactionsManager(_configuration);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> Get()
        {
            var list = await _transactionsManager.GetTransactionsAsync();
            return Ok(list);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> Get(int id)
        {
            try
            {
                if (id == 0)
                {
                    return NotFound();
                }
                var entity = await _transactionsManager.GetTransactionByIdAsync(id);
                if (entity.Transaction_ID == 0)
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
        public async Task<IActionResult> Transfer([FromBody] Transaction transaction)
        {
            try
            {
                if (!ModelState.IsValid || (transaction.AccountNumber == "" || transaction.AccountNumber == "string"))
                {
                    return BadRequest(ModelState);
                }
                var mgr = new TransactionsManager(_configuration);
                var result = await mgr.AddTransferTransactionAsync(transaction);
                if (result == "ok")
                {
                    return CreatedAtAction("Get", new { name = transaction.AccountNumber }, transaction);
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

    }
}
