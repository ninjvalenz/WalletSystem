using System;
using System.Collections.Generic;
using System.Text;

namespace WalletApp.Model.ViewModel.Exceptions
{
    public class RequiredFieldsException : Exception
    {
        public RequiredFieldsException() : base("All required fields should be filled in!")
        {

        }
    }
}
