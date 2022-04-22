using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DataAccessLayer.Contracts;


namespace SimpleBankWebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TransactionsAPIController : ControllerBase
    {
        public IConfiguration _configuration;
        //private readonly IAccountsServiceRepository _repoAccounts;
        //private readonly IPostedTransactionsRepository _repoPostedTransactions;
        private IRepositoryWrapper _repository;
        // public TransactionService _service;

        // Define the cancellation token.
        CancellationTokenSource _cts = null;
        public TransactionsAPIController(IConfiguration configuration, IRepositoryWrapper repository)
        {
            _configuration = configuration;
            _repository = repository;
            //_service = new TransactionService(context, repository);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostedTransaction>>> Get()
        {
            var list = await _repository.PostedTransactions.GetAllAsync();
            return Ok(list);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<PostedTransaction>> Get(int? id)
        {
            try
            {
                if (id == 0)
                {
                    return NotFound();
                }
                var entity = await _repository.PostedTransactions.GetByIdAsync(id);
                if (IsExists(entity.TransactionId))
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


        [HttpPost]
        public IActionResult Transfer([FromBody] PostedTransaction transaction)
        {
            try
            {
                //if (!ModelState.IsValid || (transaction.AccountNumber == "" || transaction.AccountNumber == "string"))
                //{
                //    return BadRequest(ModelState);
                //}
                //var mgr = new TransactionsManager(_configuration);
                //transaction.TransactionType_ID = 1;
                //transaction.AccountType = 1;
                //transaction.Description = "Send money to " + transaction.DestinationAccount;
                //var result = await mgr.AddTransferTransactionAsync(transaction);
                //if (result == "ok")
                //{
                //    return CreatedAtAction("Get", new { name = transaction.AccountNumber }, transaction);
                //}
                //else
                //{
                //    return BadRequest(result);
                //}
            }
            catch (Exception ex)
            {
                return this.StatusCode(400, ex.Message);
            }
            return CreatedAtAction("Get", new { name = transaction.AccountNumber }, transaction);
        }


        [HttpPost]
        public async Task<IActionResult> PostTransaction([FromBody] PostedTransaction transaction)
        {
            try
            {
                _cts = new CancellationTokenSource();
                // send a cancel after 4000 ms or call cts.Cancel();
                //_cts.CancelAfter(4000);
                CancellationToken ct = _cts.Token;
                if (ModelState.IsValid)
                {
                    //NOTED: take - 50 from Account
                    var sourceAccount = new Account();
                    sourceAccount = await _repository.Accounts.GetByAccountNumberAsync (transaction.AccountNumber);
                    var available = sourceAccount.SavingsBalance;
                    sourceAccount.SavingsBalance = (available - transaction.Amount);
                    _repository.Accounts.UpdateAccount(sourceAccount);

                    //NOTED: put + 50 to Account
                    var recepientAccount = new Account();
                    recepientAccount = await _repository.Accounts.GetByAccountNumberAsync(transaction.DestinationAccount);
                    var current = sourceAccount.SavingsBalance;
                    sourceAccount.SavingsBalance = (current + transaction.Amount);
                    _repository.Accounts.UpdateAccount(recepientAccount);

                    await _repository.PostedTransactions.AddAsync(transaction);
                    await _repository.PostedTransactions.SaveAsync(ct);




                    return CreatedAtAction("Get", new { id = transaction.TransactionId }, transaction);
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(400, ex.Message);
            }
            finally
            {

            }
            return CreatedAtAction("Get", new { id = transaction.TransactionId }, transaction);
        }




        private bool IsExists(int id)
        {
            return _repository.PostedTransactions.GetAll().Any(e => e.TransactionId == id);
        }
    }
}
