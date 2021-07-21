using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalletApp.Model.DomainModel;
using WalletApp.Model.ViewModel;

namespace WalletApp.Service.Helper
{
    public static class ModelTranslator
    {
        public static AuthenticatedLoginViewModel ToViewModel(this List<AuthenticatedLogin> userdomain)
        {
            AuthenticatedLoginViewModel authenticatedLogin = new AuthenticatedLoginViewModel
            {
                UserId = userdomain.FirstOrDefault().Id,
                Login = userdomain.FirstOrDefault().Login
            };

            authenticatedLogin.AccountNumber = new List<long>();

            foreach (var user in userdomain)
            {
                authenticatedLogin.AccountNumber.Add(user.AccountNumber);
            }

            return authenticatedLogin;
        }

        public static RegisterUserViewModel ToViewModel(this Guid userIdDomain)
            => new RegisterUserViewModel
            {
                UserSecurityID = userIdDomain
            };

        public static RegisterWalletViewModel ToViewModel(this long accountNumberDomain)
          => new RegisterWalletViewModel
          {
              AccountNumber = accountNumberDomain
          };


    }
}
