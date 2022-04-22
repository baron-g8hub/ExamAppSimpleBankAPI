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
        private IRepositoryWrapper _repository;

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
        public async Task<IActionResult> PostTransaction([FromBody] PostedTransaction transaction)
        {
            try
            {
                _cts = new CancellationTokenSource();
                _cts.CancelAfter(6000);

                //Fetch the Token
                CancellationToken ct = _cts.Token;

                if (ModelState.IsValid)
                {
                    var sourceAccount = await _repository.Accounts.GetByAccountNumberAsync(transaction.AccountNumber);
                    var recepientAccount = await _repository.Accounts.GetByAccountNumberAsync(transaction.DestinationAccount);

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
                    transaction.Description = "Transfered: " + transaction.Amount + " to " + recepientAccount.AccountNumber + " | " + recepientAccount.AccountName;
                    transaction.RunningBalance = remaining;
                    await _repository.PostedTransactions.AddTransactionAsync(transaction);
                    await _repository.SaveAsync(ct);

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
