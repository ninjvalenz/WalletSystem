using System;
using System.Collections.Generic;
using System.Text;

namespace WalletApp.Model.ViewModel.Exceptions
{
    public class TooLowAmountException : Exception
    {
        public TooLowAmountException() : base("Amount is too small!")
        {

        }
    }
}
