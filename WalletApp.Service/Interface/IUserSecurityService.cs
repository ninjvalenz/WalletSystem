using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WalletApp.Model.ViewModel;

namespace WalletApp.Service.Interface
{
    public interface IUserSecurityService
    {
        Task<AuthenticatedLoginViewModel> AuthenticateUser(string login, string password);
        Task<RegisterUserViewModel> RegisterUser(string login, string password);
        Task<QueueResultViewModel> InsertToQueue(string login, string password);
        Task<ProcessQueueResultViewModel> ProcessQueue();
        Task<UpdateQueueViewModel> UpdateQueue(long queueId,
                        int queueStatusId,
                        string message,
                        Guid userSecurityId,
                        long? walletAccountNumber);
    }
}
