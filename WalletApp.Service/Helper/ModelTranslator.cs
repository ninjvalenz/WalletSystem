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

        public static RegisterUserViewModel ToRegisterUserToViewModel(this DataTable userdomain)
           => new RegisterUserViewModel()
           {
               UserSecurityID = userdomain.Rows[0]["UserSecurityID"] != null ? (Guid)userdomain.Rows[0]["UserSecurityID"] : Guid.Empty
           };

        public static RegisterWalletViewModel ToRegisterWalletViewModel(this DataTable walletdomain)
        => new RegisterWalletViewModel()
        {
            AccountNumber = walletdomain.Rows[0]["AccountNumber"] != null ? (long)walletdomain.Rows[0]["AccountNumber"] : 0
        };

        public static TransactionHistoryListViewModel ToTransactionHistoryListViewModel(this DataTable transactdomain)
        {
            TransactionHistoryListViewModel historyViewModels = new TransactionHistoryListViewModel()
            {
                TransactionHistoryViewModels = new List<TransactionHistoryViewModel>()
            };

            foreach (DataRow domainRow in transactdomain.Rows)
            {
                historyViewModels.TransactionHistoryViewModels.Add(new TransactionHistoryViewModel()
                {
                    TransactionType = domainRow["TransactionType"] != null ? domainRow["TransactionType"].ToString() : string.Empty,
                    TransactionAmount = domainRow["Amount"] != null ? (decimal)domainRow["Amount"] : 0,
                    FromToAccountNumber = domainRow["FromToAccountNumber"] != null && domainRow["FromToAccountNumber"] != DBNull.Value ? (long?)domainRow["FromToAccountNumber"] : 0,
                    TransactionDate = domainRow["TransactionDate"] != null ? ((DateTime)domainRow["TransactionDate"]).ToShortDateString() : string.Empty,
                    TransactionEndBalance = domainRow["EndBalance"] != null ? (decimal)domainRow["EndBalance"] : 0,

                });
            }

            return historyViewModels;
        }

       



    }
}
