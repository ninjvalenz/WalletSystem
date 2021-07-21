using System;
using System.Collections.Generic;
using System.Text;

namespace WalletApp.Model.ViewModel
{
    public class TransactionHistoryViewModel : MethodResult
    {
        public string TransactionType { get; set; }
        public decimal TransactionAmount { get; set; }
        public long? FromToAccountNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal TransactionEndBalance { get; set; }
    }
}
