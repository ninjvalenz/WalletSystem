using System;
using System.Collections.Generic;
using System.Text;

namespace WalletApp.Model.ViewModel.Exceptions
{
    public class UnableToRegisterWalletException : Exception
    {
        public UnableToRegisterWalletException() : base("Unable to register new wallet account. Contact the developer.")
        {
        }
    }
}
