using Entities;
using System.ComponentModel.DataAnnotations;

namespace SimpleBankWebAPI
{
    public class TransferViewModel
    {

        public TransferViewModel()
        {
            Entity = new Transaction();
            EntityList = new List<Transaction>();
        }

        public Transaction Entity { get; set; }

        public List<Transaction> EntityList { get; set; }

        [Required]
        [Display(Name = "Source Account")]
        public string SourceAccount { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public double Amount { get; set; }

        [Required]
        [Display(Name = "Destination Account")]
        public string DestinationAccount { get; set; }
    }
}
