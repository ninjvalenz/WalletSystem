using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WalletApp.Model.ViewModel;

namespace WalletApp.Service.Interface
{
    public interface IWalletTransactionService
    {
        Task<TransactionHistoryListViewModel> ViewTransactionHistoryAll(long accountNumber, int offset);
        Task<TransactionHistoryListViewModel> ViewTransactionHistoryByRange(
            long accountNumber, 
            DateTime fromDate, 
            DateTime toDate);
        Task<DepositMoneyViewModel> DepositMoney(long accountNumber, double amount);
        Task<TransferMoneyViewModel> TransferMoney(long accountNumber, long toAccountNumber, double amount);

        Task<WithdrawMoneyViewModel> WithdrawMoney(long accountNumber, double amount);
    }
}
