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
                message = value;
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
