using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
    public class Transaction
    {
		public int Transaction_ID { get; set; }
        public int TransactionType_ID { get; set; }
        public string TransactionTypeName { get; set; }
        public DateTime PostingDate { get; set; }
        public string PostingDateStr { get; set; }
        public string AccountNumber { get; set; }
		public string Description { get; set; }
		public int AccountType { get; set; }
		public double Amount { get; set; }
		public string DestinationAccount { get; set; }
		public double RunningBalance { get; set; }
	}
}
