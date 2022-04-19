using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
    public class Account : AccountType
    {
        public Account()
        {
            Account_ID = 0;
            AccountId = 0;
            AccountName = "";
            AccountType = 1;
            AccountType_ID = 1;
            UpdatedBy = "Admin";
            CreatedBy = "Admin";
            CreatedDateStr = "";
            UpdatedDateStr = "";
            AccountTypeName = "Savings";
        }

        public int Account_ID { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public int AccountType { get; set; }
        public string AccountNumber { get; set; }
        public double SavingsBalance { get; set; }
        public double CheckingBalance { get; set; }
        public double CreditBalance { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDateStr { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedDateStr { get; set; }
        public string UpdatedBy { get; set; }


    }
}
