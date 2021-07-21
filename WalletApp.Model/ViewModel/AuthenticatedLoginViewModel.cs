using System;
using System.Collections.Generic;
using System.Text;

namespace WalletApp.Model.ViewModel
{
    public class AuthenticatedLoginViewModel : MethodResult
    {
        public Guid UserId { get; set; }
        public string Login { get; set; }

        public List<long> AccountNumber { get; set; }
    }
}
