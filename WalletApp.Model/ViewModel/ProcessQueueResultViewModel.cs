using System;
using System.Collections.Generic;
using System.Text;

namespace WalletApp.Model.ViewModel
{
    public class ProcessQueueResultViewModel : MethodResult
    {
        public List<QueueResultViewModel> QueueResultViewModels { get; set; }
    }
}
