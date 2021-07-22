using System;
using System.Collections.Generic;
using System.Text;

namespace WalletApp.Model.ViewModel.Exceptions
{
    public class ServerProblemException : Exception
    {
        public ServerProblemException() : base ("Problem occured with the server!")
        {

        }
    }
}
