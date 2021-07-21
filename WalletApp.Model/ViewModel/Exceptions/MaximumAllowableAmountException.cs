using System;
using System.Collections.Generic;
using System.Text;

namespace WalletApp.Model.ViewModel.Exceptions
{
    public class MaximumAllowableAmountException : Exception
    {
        public MaximumAllowableAmountException() : base("Maximum allowable amount exceeded!")
        {

        }
    }
}
