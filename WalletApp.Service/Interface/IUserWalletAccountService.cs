using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WalletApp.Model.ViewModel;

namespace WalletApp.Service.Interface
{
    public interface IUserWalletAccountService
    {
        Task<RegisterWalletViewModel> RegisterWallet(Guid userSecurityId);
        Task<QueueResultViewModel> InsertToQueue(Guid userSecurityId);
    }
}
