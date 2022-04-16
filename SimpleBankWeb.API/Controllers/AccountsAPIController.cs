using BusinessLogicLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SimpleBankWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsAPIController : ControllerBase
    {

        private  IConfiguration _configuration;
        public AccountsAPIController(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        // GET: api/<AccountsAPIController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var datasource = new AccountsManager(_configuration);
            var list = await datasource.GetAccounts();
            return Ok(JsonConvert.SerializeObject(list).ToString());
        }


        // GET api/<AccountsAPIController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AccountsAPIController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
