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
			AccountNumber = "";
			AccountName = "";
			AccountType = 1;
        }

		public int Account_ID { get; set; }
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
