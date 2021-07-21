using System;
using System.Collections.Generic;
using System.Text;

namespace WalletApp.Model.ViewModel.Exceptions
{
    public class InsufficientWalletBalanceException : Exception
    {
        public InsufficientWalletBalanceException() : base("Insufficient balance on the wallet!")
        {

        }
    }
}
