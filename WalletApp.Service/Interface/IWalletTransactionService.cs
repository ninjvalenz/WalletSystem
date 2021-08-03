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
        Task<DepositMoneyViewModel> DepositMoney(long accountNumber, long? fromAccountNumber, decimal amount, int transactionTypeId);
        Task<TransferMoneyViewModel> TransferMoney(long accountNumber, long fromToAccountNumber, decimal amount);

        Task<WithdrawMoneyViewModel> WithdrawMoney(long accountNumber, decimal amount);
        Task<QueueResultViewModel> InsertToQueue(long accountNumber, long? fromToAccountNumber, decimal amount, int transactionTypeId);
        Task<ProcessQueueResultViewModel> ProcessQueue();
        Task<UpdateQueueViewModel> UpdateQueue(
                        long queueId,
                        int queueStatusId,
                        string message);
    }
}
