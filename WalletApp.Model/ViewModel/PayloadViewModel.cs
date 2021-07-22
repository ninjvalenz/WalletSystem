using System;
using System.Collections.Generic;
using System.Text;

namespace WalletApp.Model.ViewModel
{
    public class PayloadViewModel
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
    }
}
