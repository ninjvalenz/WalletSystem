using System;

namespace WalletApp.Model.DomainModel
{
    public class AuthenticatedLogin
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public long AccountNumber { get; set; }
    }
}
