using System;
using System.Collections.Generic;
using System.Text;

namespace WalletApp.Model.ViewModel.Exceptions
{
    public class UnableToRegisterUserException : Exception
    {
        public UnableToRegisterUserException() : base("Unable to register. Login already exists!")
        {

        }
    }
}
