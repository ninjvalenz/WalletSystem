using System;
using System.Collections.Generic;
using System.Text;

namespace WalletApp.Model.ViewModel.RequestBodyModel
{
    public class NewTransferViewModel : TransactionViewModel
    { 
        public long? ToAccountNumber { get; set; }
      
    }
}
