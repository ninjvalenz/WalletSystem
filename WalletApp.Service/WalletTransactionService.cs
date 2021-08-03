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
    public class WalletTransactionService : IWalletTransactionService
    {
        IDBService dBService;
        public WalletTransactionService(IDBService _dBService)
        {
            dBService = _dBService;

        }

        public async Task<TransactionHistoryListViewModel> ViewTransactionHistoryAll(
            long accountNumber, 
            int offset)
        {
            TransactionHistoryListViewModel transactionListViewModel
                = new TransactionHistoryListViewModel();

            try
            {
                var domainResult = await dBService.ExecuteQuery("ViewTransactionHistoryAll", new SqlParameter[]
                    {
                        new SqlParameter() { ParameterName = "AccountNumber", Value = accountNumber },
                        new SqlParameter() { ParameterName = "Offset", Value = offset }
                    }, CommandType.StoredProcedure);

                if (domainResult != null && domainResult.Rows != null && domainResult.Rows.Count > 0)
                    transactionListViewModel = domainResult.ToTransactionHistoryListViewModel();
                else
                    transactionListViewModel.InfoMessage = "No more records to be display";

            }
            catch (Exception ex)
            {
                transactionListViewModel.Message = ex.Message;
            }
          
            return transactionListViewModel;
        }

        public async Task<TransactionHistoryListViewModel> ViewTransactionHistoryByRange(
            long accountNumber, 
            DateTime fromDate, 
            DateTime toDate)
        {
            TransactionHistoryListViewModel transactionListViewModel =
                        new TransactionHistoryListViewModel();
            try
            {
                var domainResult = await dBService.ExecuteQuery("ViewTransactionHistoryByRange", new SqlParameter[]
                    {
                        new SqlParameter() { ParameterName = "AccountNumber", Value = accountNumber },
                        new SqlParameter() { ParameterName = "FromDate", Value = fromDate },
                        new SqlParameter() { ParameterName = "ToDate", Value = toDate }
                    }, CommandType.StoredProcedure);

                if (domainResult != null && domainResult.Rows != null && domainResult.Rows.Count > 0)
                    transactionListViewModel = domainResult.ToTransactionHistoryListViewModel();
                else
                    transactionListViewModel.InfoMessage = "No more records to be display";
            }
            catch (Exception ex)
            {
                transactionListViewModel.Message = ex.Message;
            }

            return transactionListViewModel;
        }

        public async Task<DepositMoneyViewModel> DepositMoney(
            long accountNumber, 
            long? fromAccountNumber, 
            decimal amount, 
            int transactionTypeId)
        {
            DepositMoneyViewModel depositMoneyViewModel = new DepositMoneyViewModel();

            try
            {
                if (amount <= 0) throw new TooLowAmountException();
                if (amount > 99999999) throw new MaximumAllowableAmountException();
                
                var domainResult = await dBService.ExecuteQuery("DepositMoney", new SqlParameter[]
                    {
                        new SqlParameter() { ParameterName = "AccountNumber", Value = accountNumber },
                        new SqlParameter() { ParameterName = "ToAccountNumber", Value = fromAccountNumber },
                        new SqlParameter() { ParameterName = "Amount", Value = amount },
                        new SqlParameter() { ParameterName = "TransactionTypeId", Value = transactionTypeId }
                    }, CommandType.StoredProcedure);

                if (domainResult != null &&
                    domainResult.Rows != null &&
                    domainResult.Rows.Count > 0)
                {
                    var endBal = domainResult.Rows[0]["EndBalance"] != null ?
                            (decimal)domainResult.Rows[0]["EndBalance"] : 0;

                    depositMoneyViewModel.InfoMessage = $"Success! End balance is now {endBal}";
                }

            }
            catch(Exception ex)
            {
                depositMoneyViewModel.Message = ex.Message;
            }

            return depositMoneyViewModel;
        }

        public async Task<WithdrawMoneyViewModel> WithdrawMoney(
             long accountNumber,
             decimal amount)
        {
            WithdrawMoneyViewModel withdrawMoneyViewModel = new WithdrawMoneyViewModel();

            try
            {
                if (amount <= 0) throw new TooLowAmountException();
                if (amount > 99999999) throw new MaximumAllowableAmountException();

                var domainResult = await dBService.ExecuteQuery("WithdrawMoney", new SqlParameter[]
                    {
                        new SqlParameter() { ParameterName = "AccountNumber", Value = accountNumber },
                        new SqlParameter() { ParameterName = "Amount", Value = amount }
                    }, CommandType.StoredProcedure);

                if (domainResult != null &&
                    domainResult.Rows != null &&
                    domainResult.Rows.Count > 0)
                {
                    if(!(bool)domainResult.Rows[0]["IsSuccess"])
                        throw new InsufficientWalletBalanceException();
                    else
                    {
                        var endBal = domainResult.Rows[0]["EndBalance"] != null ?
                            (decimal)domainResult.Rows[0]["EndBalance"] : 0;

                        withdrawMoneyViewModel.InfoMessage = $"Success! End balance is now {endBal}";

                    }
                }

            }
            catch (Exception ex)
            {
                withdrawMoneyViewModel.Message = ex.Message;
            }
            return withdrawMoneyViewModel;
        }

        public async Task<TransferMoneyViewModel> TransferMoney(
            long accountNumber, 
            long fromToAccountNumber,
            decimal amount)
        {
            TransferMoneyViewModel transferMoneyViewModel = new TransferMoneyViewModel();
          
            try
            {
                if (amount <= 0) throw new TooLowAmountException();
                if (amount > 99999999) throw new MaximumAllowableAmountException();

                //deduct from source
                var domainResult = await dBService.ExecuteQuery("Transfer_Deduct", new SqlParameter[]
                    {
                        new SqlParameter() { ParameterName = "AccountNumber", Value = accountNumber },
                        new SqlParameter() { ParameterName = "ToAccountNumber", Value = fromToAccountNumber },
                        new SqlParameter() { ParameterName = "Amount", Value = amount }
                    }, CommandType.StoredProcedure);

                if (domainResult != null &&
                    domainResult.Rows != null &&
                    domainResult.Rows.Count > 0)
                {
                    if (!(bool)domainResult.Rows[0]["IsSuccess"])
                        throw new InsufficientWalletBalanceException();
                    else
                    {
                        var endBal = domainResult.Rows[0]["EndBalance"] != null ?
                            (decimal)domainResult.Rows[0]["EndBalance"] : 0;

                        transferMoneyViewModel.InfoMessage = $"Success! End balance is now {endBal}";

                        //Proceed to tranfer to destination
                        var depositResult = await DepositMoney(fromToAccountNumber, accountNumber, amount, (int)TransactionTypes.Deposit);

                        //Refund if not success
                        if (depositResult != null &&
                            !depositResult.IsSuccess)
                        {
                            await DepositMoney(accountNumber, null, amount, (int)TransactionTypes.Refund);
                            throw new Exception(depositResult.Message);
                        }

                    }
                }

                

              
            }
            catch (Exception ex)
            {
                transferMoneyViewModel.Message = ex.Message;
            }

            return transferMoneyViewModel;

        }

        public async Task<QueueResultViewModel> InsertToQueue(
                            long accountNumber, 
                            long? fromToAccountNumber, 
                            decimal amount, 
                            int transactionTypeId)
        {
            QueueResultViewModel queueResultViewModel = new QueueResultViewModel();
            try
            {
                var domainResult = await dBService.ExecuteQuery("InsertIntoUserWalletTransacQueue",
                    new SqlParameter[]
                    {
                        new SqlParameter() { ParameterName = "UserWalletAccountNumber", Value = accountNumber },
                        new SqlParameter() { ParameterName = "FromToAccountNumber", Value = fromToAccountNumber },
                        new SqlParameter() { ParameterName = "TransactionTypeId", Value = transactionTypeId },
                        new SqlParameter() { ParameterName = "Amount", Value = amount }
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
                var domainResult = await dBService.ExecuteQuery("ProcessUserWalletTransacQueue",
                  null, CommandType.StoredProcedure);


                if (domainResult != null && domainResult.Rows != null && domainResult.Rows.Count > 0)
                {
                    foreach (DataRow row in domainResult.Rows)
                    {
                        var queueId = (long)row["queueId"];
                        var userWalletAcctNo = (long)row["UserWalletAccountNumber"];
                        var fromToAccountNumber = row["FromToAccountNumber"] == DBNull.Value ? 0 : (long)row["FromToAccountNumber"];
                        var transactionTypeId = (int)row["TransactionTypeId"];
                        var amount = (decimal)row["Amount"];

                        var queueItem = new QueueResultViewModel()
                        {
                            QueueId = queueId
                        };

                        //Call transact process for each type (deposit, withdraw, transfer)
                        if ((TransactionTypes)transactionTypeId == TransactionTypes.Deposit)
                        {
                            var depositResult = await DepositMoney(userWalletAcctNo, null, amount, transactionTypeId);
                            if(depositResult != null)
                            {
                                queueItem.Message = depositResult.Message;
                                queueItem.InfoMessage = depositResult.InfoMessage;
                            }
                        }
                        else if((TransactionTypes)transactionTypeId == TransactionTypes.Withdraw)
                        {
                            var withdrawResult = await WithdrawMoney(userWalletAcctNo, amount);
                            if (withdrawResult != null)
                            {
                                queueItem.Message = withdrawResult.Message;
                                queueItem.InfoMessage = withdrawResult.InfoMessage;
                            }
                        }
                        else
                        {
                            var transferResult = await TransferMoney(userWalletAcctNo, fromToAccountNumber, amount);
                            if (transferResult != null)
                            {
                                queueItem.Message = transferResult.Message;
                                queueItem.InfoMessage = transferResult.InfoMessage;
                            }
                        }



                        //Call update queue
                        await UpdateQueue(
                                       queueId,
                                       queueItem.IsSuccess ? (int)QueueStatusType.Success : (int)QueueStatusType.Failed,
                                       queueItem.Message);
                    }
                }
               
            }
            catch (Exception ex)
            {
                processQueueResultView.Message = ex.Message;
            }
            
           
            return processQueueResultView;
        }

        public async Task<UpdateQueueViewModel> UpdateQueue(
                        long queueId,
                        int queueStatusId,
                        string message)
        {
            UpdateQueueViewModel updateQueueViewModel = new UpdateQueueViewModel();
            try
            {

                await dBService.ExecuteNonQuery("UpdateUserWalletTransactionQueue",
                       new SqlParameter[]
                    {
                        new SqlParameter() { ParameterName = "QueueId", Value = queueId },
                        new SqlParameter() { ParameterName = "QueueStatusId", Value = queueStatusId},
                        new SqlParameter() { ParameterName = "Message", Value = message }

                    }, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                updateQueueViewModel.Message = ex.Message;
            }

            return updateQueueViewModel;
        }
    }
}
