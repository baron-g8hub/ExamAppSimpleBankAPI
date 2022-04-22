using DataAccessLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace SimpleBankWebAPI
{
    public class TransferViewModel
    {

        public TransferViewModel()
        {
            Entity = new PostedTransaction();
            EntityList = new List<PostedTransaction>();
        }

        public PostedTransaction Entity { get; set; }

        public List<PostedTransaction> EntityList { get; set; }

        [Required]
        [Display(Name = "Source Account")]
        public string SourceAccount { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public double Amount { get; set; }

        [Required]
        [Display(Name = "Destination Account")]
        public string DestinationAccount { get; set; }

        public byte[]? RowVersion { get; set; }
    }
}
