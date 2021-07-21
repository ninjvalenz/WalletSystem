using System;
using System.Collections.Generic;
using System.Text;

namespace WalletApp.Model.ViewModel
{
    public class TransactionHistoryListViewModel : MethodResult
    {
        public List<TransactionHistoryViewModel> TransactionHistoryViewModels { get; set; }
    }
}
