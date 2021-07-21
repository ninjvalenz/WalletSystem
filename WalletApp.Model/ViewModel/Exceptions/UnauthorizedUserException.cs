using System;
using System.Collections.Generic;
using System.Text;

namespace WalletApp.Model.ViewModel.Exceptions
{
    public class UnauthorizedUserException : Exception
    {
        public UnauthorizedUserException() : base("Invalid credentials!")
        {
        }
    }
}
