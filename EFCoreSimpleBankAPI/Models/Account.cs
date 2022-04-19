using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreSimpleBankAPI.Models
{
    public  class Account
    {
        [Column("Account_ID")]
        public int AccountId { get; set; }
        [Key]
        [StringLength(150)]
        public string AccountName { get; set; } = null!;
        public int? AccountType { get; set; }
        public string? AccountNumber { get; set; }
        [Column(TypeName = "money")]
        public decimal? SavingsBalance { get; set; }
        [Column(TypeName = "money")]
        public decimal? CheckingBalance { get; set; }
        [Column(TypeName = "money")]
        public decimal? CreditBalance { get; set; }
        public bool? IsActive { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string? UpdatedBy { get; set; }
    }
}
