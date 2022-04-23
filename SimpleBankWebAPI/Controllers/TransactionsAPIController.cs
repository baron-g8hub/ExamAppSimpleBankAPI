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
        CancellationTokenSource? _cts;
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
            _cts = new CancellationTokenSource();
            _cts.CancelAfter(6000);
            //Fetch the Token
            CancellationToken ct = _cts.Token;

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

            try
            {
                transaction.PostingDate = DateTime.UtcNow;
                transaction.Description = "Transfered: " + transaction.Amount + " to " + recepientAccount.AccountNumber + " | " + recepientAccount.AccountName;
                transaction.RunningBalance = remaining;
                await _repository.PostedTransactions.AddTransactionAsync(transaction);
                var retmessage = await _repository.SaveTransactionAsync(ct);

                // NOTE: Testing Concurrency throw here when Source Account has been changed from other client.
                //var retmessage = await _repository.UnitTestSaveChangesPostTransactionConcurrency(transaction);
              
                return CreatedAtAction("Get", new { id = transaction.TransactionId }, transaction);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                var exceptionEntry = ex.Entries.Single();
                var clientValues = (Account)exceptionEntry.Entity;
                var databaseEntry = exceptionEntry.GetDatabaseValues();
                if (databaseEntry == null)
                {
                    var message = "Unable to save changes. The entity was deleted by another user.";
                    return StatusCode(StatusCodes.Status500InternalServerError, message);
                }
                else
                {
                    // NOTE: Check if the Concurrency issue is about "Accounts" and identify whether Sender or Recipient.
                    var databaseValues = (Account)databaseEntry.ToObject();
                    if (databaseValues.AccountNumber == transaction.AccountNumber)
                    {
                        if (databaseValues.SavingsBalance != clientValues.SavingsBalance)
                        {
                            // If Source Account current value is lesser than Amount to transfer then terminate the transaction here.
                            if (databaseValues.SavingsBalance < clientValues.SavingsBalance)
                            {
                                var message = $"Insufficient source account: {databaseValues.SavingsBalance}";
                                return StatusCode(StatusCodes.Status500InternalServerError, message);
                            }
                            else
                            {
                                // OPTION 1: Source Account current value is valid then we try to retry the transaction.
                                // We can also used the iterating function to enforced Optimistic SaveChangesAsync

                                //NOTED: take - Amount from Account
                                available = databaseValues.SavingsBalance;
                                remaining = (available - transaction.Amount);
                                databaseValues.SavingsBalance = remaining;
                                transaction.PostingDate = DateTime.UtcNow;
                                transaction.Description = "Transfered: " + transaction.Amount + " to " + recepientAccount.AccountNumber + " | " + recepientAccount.AccountName;
                                transaction.RunningBalance = remaining;

                                _cts = new CancellationTokenSource();
                                _cts.CancelAfter(6000);
                                //Fetch the Token
                                ct = _cts.Token;
                                await _repository.PostedTransactions.AddTransactionAsync(transaction);
                                var retmessage = await _repository.SaveTransactionAsync(ct);

                                //var message = $"Source Account current value: {databaseValues.SavingsBalance}";
                                //return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());

                                // OPTION 2: Just return to the Updated value to client.
                                //message = $"Source Account current value: {databaseValues.SavingsBalance}";
                                //return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
                            }
                        }
                    }
                }
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
            finally
            {
                if (_cts != null)
                {
                    _cts.Dispose();
                }
            }
            return NoContent();
        }

        private bool IsExists(int id)
        {
            return _repository.PostedTransactions.GetAll().Any(e => e.TransactionId == id);
        }


    }
}
