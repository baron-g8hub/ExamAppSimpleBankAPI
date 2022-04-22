using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DataAccessLayer.Contracts;
using Microsoft.EntityFrameworkCore;

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
            var orderedList = list.OrderByDescending(x => x.PostingDate).ToList();
            return Ok(orderedList);
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
                //_cts.CancelAfter(4000);

                //Fetch the Token
                CancellationToken ct = _cts.Token;

                if (ModelState.IsValid)
                {
                    var sourceAccount = _repository.Accounts.GetByAccountNumber(transaction.AccountNumber);
                    var recepientAccount = _repository.Accounts.GetByAccountNumber(transaction.DestinationAccount);

                    //NOTED: take - 50 from Account
                    var available = sourceAccount.SavingsBalance;
                    var remaining = (available - transaction.Amount);
                    sourceAccount.SavingsBalance = remaining;
                    //_repository.Accounts.UpdateAccount(sourceAccount);
                    //_repository.Accounts.Save();

                    //NOTED: put + 50 to Account
                    var current = recepientAccount.SavingsBalance;
                    var balance = (current + transaction.Amount);
                    recepientAccount.SavingsBalance = balance;
                    //_repository.Accounts.UpdateAccount(recepientAccount);
                    //_repository.Accounts.Save();

                    transaction.PostingDate = DateTime.UtcNow;
                    transaction.Description = "Send money to: " + recepientAccount.AccountNumber + " | " + recepientAccount.AccountName;
                    transaction.RunningBalance = remaining;
                    _repository.PostedTransactions.AddTransaction(transaction);
                    _repository.PostedTransactions.Save();

                    return CreatedAtAction("Get", new { id = transaction.TransactionId }, transaction);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message.ToString());
                return BadRequest();
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




        private bool IsExists(int id)
        {
            return _repository.PostedTransactions.GetAll().Any(e => e.TransactionId == id);
        }
    }
}
