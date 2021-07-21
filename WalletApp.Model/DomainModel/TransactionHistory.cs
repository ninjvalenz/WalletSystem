using System;
using System.Collections.Generic;
using System.Text;

namespace WalletApp.Model.DomainModel
{
    public class TransactionHistory
    {
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public long FromToAccountNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal EndBalance { get; set; }
    }
}
