using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WalletApp.Model.ViewModel;
using WalletApp.Model.ViewModel.Exceptions;
using WalletApp.Service.ConnectionStrings;
using WalletApp.Service.Helper;
using WalletApp.Service.Interface;

namespace WalletApp.Service
{
    
    public class UserWalletAccountService : IUserWalletAccountService
    {
        IDBService dBService;
        public UserWalletAccountService(IDBService _dBService)
        {
            dBService = _dBService;

        }

        public async Task<RegisterWalletViewModel> RegisterWallet(Guid userSecurityId)
        {
            RegisterWalletViewModel registerWalletViewModel = new RegisterWalletViewModel();

            try
            {
               var domainResult = await dBService.ExecuteQuery("RegisterWallet", new SqlParameter[]
                    {
                        new SqlParameter() { ParameterName = "UserSecurityId", Value = userSecurityId }
                    }, CommandType.StoredProcedure);

                if (domainResult != null && domainResult.Rows != null && domainResult.Rows.Count > 0)
                    registerWalletViewModel = domainResult.RegisterWalletViewModel();
                else throw new UnableToRegisterWalletException();
            }
            catch (Exception ex)
            {
                registerWalletViewModel.Message = ex.Message;
            }
           
            return registerWalletViewModel;
        }
    }
}
