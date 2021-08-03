using System;
using System.Collections.Generic;
using System.Text;

namespace WalletApp.Model.ViewModel.Exceptions
{
    public class AccountNotExistingException : Exception
    {
        public AccountNotExistingException() : base("Account not existing!")
        {

        }
    }
}
