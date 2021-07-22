using System;
using System.Collections.Generic;
using System.Text;

namespace WalletApp.Model.ViewModel.RequestBodyModel
{
    public class TransactionViewModel
    {
        public long? AccountNumber { get; set; }
        public decimal? Amount { get; set; }
    }
}
