using System;
using System.Collections.Generic;
using System.Text;

namespace WalletApp.Model.ViewModel.RequestBodyModel
{
    public class HistoryByRangeViewModel
    {
        public long? AccountNumber { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
