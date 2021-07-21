using System;
using System.Collections.Generic;
using System.Data;
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

        public static AuthenticatedLoginViewModel ToViewModel(this DataTable userdomain)
        {
            
            AuthenticatedLoginViewModel authenticatedLogin = new AuthenticatedLoginViewModel()
            {
                AccountNumber = new List<long>()
            };

            foreach (DataRow domainRow in userdomain.Rows)
            {
                authenticatedLogin.UserId = domainRow["Id"] != null ? (Guid)domainRow["Id"] : Guid.Empty;
                authenticatedLogin.Login = domainRow["Login"] != null ? domainRow["Login"].ToString() : string.Empty;

                if (domainRow["AccountNumber"] != null)
                    authenticatedLogin.AccountNumber.Add((long)domainRow["AccountNumber"]);
            }

            return authenticatedLogin;
        }



    }
}
