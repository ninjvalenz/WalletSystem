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
        public string TransactionDate { get; set; }
        public decimal TransactionEndBalance { get; set; }
    }
}
