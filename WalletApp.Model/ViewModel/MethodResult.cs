using System;
using System.Collections.Generic;
using System.Text;

namespace WalletApp.Model.ViewModel
{
    public class MethodResult
    {
        private string message;
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                Message = value;
            }

        }

        public bool IsSuccess
        {
            get
            {
                return string.IsNullOrEmpty(message);
            }
        }
    }
}
