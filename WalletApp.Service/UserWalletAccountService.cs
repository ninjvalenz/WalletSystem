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

        public async Task<QueueResultViewModel> InsertToQueue(Guid userSecurityId)
        {
            QueueResultViewModel queueResultViewModel = new QueueResultViewModel();

            try
            {
                var domainResult = await dBService.ExecuteQuery("InsertIntoUserWalletAcctQueue",
                    new SqlParameter[]
                    {
                        new SqlParameter() { ParameterName = "UserSecurityId", Value = userSecurityId }
                    }, CommandType.StoredProcedure);

                if (domainResult != null && domainResult.Rows != null && domainResult.Rows.Count > 0 && domainResult.Rows[0][0] != DBNull.Value)
                    queueResultViewModel = domainResult.ToQueueResultViewModel();
            }
            catch (Exception ex)
            {
                queueResultViewModel.Message = ex.Message;
            }

            return queueResultViewModel;
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
                    registerWalletViewModel = domainResult.ToRegisterWalletViewModel();
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
