using DataAccessLayer.Models;

namespace SimpleBankWebAPI.ViewModels
{
    public class PostingTransactionWrapper
    {
        public PostingTransactionWrapper()
        {
            PostedTransaction = null;
            SourceAccount = null;
            DestinationAccount = null;
        }


        public PostedTransaction PostedTransaction { get; set; }
        public Account SourceAccount { get; set; }
        public Account DestinationAccount { get; set; }
    }
}
