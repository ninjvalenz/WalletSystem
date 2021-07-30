using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using WalletApp.Model.DomainModel;
using WalletApp.Model.Enums;
using WalletApp.Model.ViewModel;
using WalletApp.Model.ViewModel.Exceptions;
using WalletApp.Service.ConnectionStrings;
using WalletApp.Service.Helper;
using WalletApp.Service.Interface;

namespace WalletApp.Service
{
    public class UserSecurityService : IUserSecurityService
    {
        IDBService dBService;
        IUserWalletAccountService userWalletAccountService;
        public UserSecurityService(IDBService _dBService, IUserWalletAccountService _userWalletAccountService)
        {
            dBService = _dBService;
            userWalletAccountService = _userWalletAccountService;
        }
        public async Task<AuthenticatedLoginViewModel> AuthenticateUser(string login, string password)
        {
            AuthenticatedLoginViewModel authenticatedLoginViewModel = new AuthenticatedLoginViewModel();

            try
            {

                var domainResult = await dBService.ExecuteQuery("AuthenticateLogin",
                    new SqlParameter[]
                    {
                        new SqlParameter() { ParameterName = "Login", Value = login },
                        new SqlParameter() { ParameterName = "Password", Value = password}
                    }, CommandType.StoredProcedure);

                if (domainResult != null && domainResult.Rows != null && domainResult.Rows.Count > 0)
                    authenticatedLoginViewModel = domainResult.ToViewModel();
                else throw new UnauthorizedUserException();
            }
            catch (Exception ex)
            {
                authenticatedLoginViewModel.Message = ex.Message;
            }

            return authenticatedLoginViewModel;
        }

        public async Task<QueueResultViewModel> InsertToQueue(string login, string password)
        {
            QueueResultViewModel queueResultViewModel = new QueueResultViewModel();

            try
            {
                var domainResult = await dBService.ExecuteQuery("InsertToUserSecurityQueue",
                    new SqlParameter[]
                    {
                        new SqlParameter() { ParameterName = "Login", Value = login },
                        new SqlParameter() { ParameterName = "Password", Value = password}
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
        public async Task<ProcessQueueResultViewModel> ProcessQueue()
        {
            ProcessQueueResultViewModel processQueueResultView = new ProcessQueueResultViewModel();
            processQueueResultView.QueueResultViewModels = new List<QueueResultViewModel>();

            try
            {
                //Call process queue
                var domainResult = await dBService.ExecuteQuery("ProcessUserSecurityQueue",
                    null, CommandType.StoredProcedure);

                if (domainResult != null && domainResult.Rows != null && domainResult.Rows.Count > 0)
                {
                    foreach (DataRow row in domainResult.Rows)
                    {
                        var queueId = (long)row["queueId"];
                        var login = row["Login"].ToString();
                        var password = row["Password"].ToString();
                        string message = string.Empty;
                        bool isSuccess = false;
                      
                        //Call registeruser
                        var registerResult = await RegisterUser(login, password);
                        if(registerResult != null)
                        {
                            var queueItem = new QueueResultViewModel()
                            {
                                QueueId = queueId
                            };

                            //Call register wallet
                            if (registerResult.IsSuccess)
                            {
                                var registerWalletResult = await userWalletAccountService.RegisterWallet(registerResult.UserSecurityID);
                                if (registerWalletResult != null && !registerWalletResult.IsSuccess)
                                    queueItem.Message = registerWalletResult.Message;
                            }
                            else
                                queueItem.Message = registerResult.Message;

                            isSuccess = registerResult.IsSuccess;

                            processQueueResultView.QueueResultViewModels.Add(queueItem);
                            message = queueItem.Message;
                        }

                        //Update queue
                        await UpdateQueue(
                                       queueId,
                                       isSuccess ? (int)QueueStatusType.Success : (int)QueueStatusType.Failed,
                                       message);
                    }
                }
               
            }
            catch (Exception ex)
            {

                processQueueResultView.Message = ex.Message;
            }

            return processQueueResultView;
        }

        public async Task<UpdateQueueViewModel> UpdateQueue(long queueId, int queueStatusId, string message)
        {
            UpdateQueueViewModel updateQueueViewModel = new UpdateQueueViewModel();
            try
            {
                
                await dBService.ExecuteNonQuery("UpdateUserSecurityQueue",
                       new SqlParameter[]
                    {
                        new SqlParameter() { ParameterName = "QueueId", Value = queueId },
                        new SqlParameter() { ParameterName = "QueueStatusId", Value = queueStatusId},
                        new SqlParameter() { ParameterName = "Message", Value = message },

                    }, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                updateQueueViewModel.Message = ex.Message;
            }

            return updateQueueViewModel;
        }

        public async Task<RegisterUserViewModel> RegisterUser(string login, string password)
        {
            RegisterUserViewModel registerUserViewModel = new RegisterUserViewModel();

            try
            {
                var domainResult = await dBService.ExecuteQuery("RegisterUser",
                    new SqlParameter[]
                    {
                        new SqlParameter() { ParameterName = "Login", Value = login },
                        new SqlParameter() { ParameterName = "Password", Value = password}
                    }, CommandType.StoredProcedure);

                if (domainResult != null && domainResult.Rows != null && domainResult.Rows.Count > 0 && domainResult.Rows[0][0] != DBNull.Value)
                    registerUserViewModel = domainResult.ToRegisterUserToViewModel();
                else
                    throw new UnableToRegisterUserException();

            }
            catch(Exception ex)
            {
                registerUserViewModel.Message = ex.Message;
            }

            return registerUserViewModel;

        }

    
    }
}
