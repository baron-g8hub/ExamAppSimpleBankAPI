
namespace DataAccessLayer.Models
{
    public class PostingTransactionWrapper
    {
        public PostingTransactionWrapper()
        {
            PostedTransaction = new PostedTransaction();
            SourceAccount = new Account();
            DestinationAccount = new Account();
        }

        public double Amount { get; set; }
        public PostedTransaction PostedTransaction { get; set; }
        public Account SourceAccount { get; set; }
        public Account DestinationAccount { get; set; }
    }
}
