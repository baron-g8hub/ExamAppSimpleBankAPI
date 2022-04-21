using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    
    [Table("Posted_Transactions")]
    public  class PostedTransaction
    {

        [Key]
        [Column("Transaction_ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PostingDate { get; set; }
        [StringLength(50)]
        public string? AccountNumber { get; set; }
        [StringLength(150)]
        public string? Description { get; set; }
        public int? AccountType { get; set; }
        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }
        [StringLength(50)]
        public string? DestinationAccount { get; set; }
        [StringLength(10)]
        public string? RunningBalance { get; set; }
        [Column("TransactionType_ID")]
        public int? TransactionTypeId { get; set; }

        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}
