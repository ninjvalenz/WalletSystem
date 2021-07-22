using System;
using System.Collections.Generic;
using System.Text;

namespace WalletApp.Model.ViewModel.RequestBodyModel
{
    public class HistoryAllViewModel
    {
        public long? AccountNumber { get; set; }
        public int? Offset { get; set; }
    }
}
