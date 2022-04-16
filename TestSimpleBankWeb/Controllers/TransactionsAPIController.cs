using BusinessLogicLayer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestSimpleBankWeb.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TransactionsAPIController : ControllerBase
    {

        private TransactionsManager _transactionsManager;

        private IConfiguration _configuration;
        public TransactionsAPIController(IConfiguration configuration)
        {
            _configuration = configuration;
            _transactionsManager = new TransactionsManager(_configuration);
        }



        // GET: api/<TransactionsAPIController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var list = await _transactionsManager.GetTransactionsAsync();
            return Ok(JsonConvert.SerializeObject(list).ToString());
        }

        // GET api/<TransactionsAPIController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TransactionsAPIController>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<TransactionsAPIController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<TransactionsAPIController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
