using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using WalletApp.Model.DomainModel;
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
            decimal amount)
        {
            DepositMoneyViewModel depositMoneyViewModel = new DepositMoneyViewModel();

            try
            {
                if (amount <= 0) throw new TooLowAmountException();
                if (amount > 99999999) throw new MaximumAllowableAmountException();
                
                var domainResult = await dBService.ExecuteQuery("DepositMoney", new SqlParameter[]
                    {
                        new SqlParameter() { ParameterName = "AccountNumber", Value = accountNumber },
                        new SqlParameter() { ParameterName = "Amount", Value = amount }
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
            long toAccountNumber,
            decimal amount)
        {
            TransferMoneyViewModel transferMoneyViewModel = new TransferMoneyViewModel();

            try
            {
                if (amount <= 0) throw new TooLowAmountException();
                if (amount > 99999999) throw new MaximumAllowableAmountException();

                var domainResult = await dBService.ExecuteQuery("TransferMoney", new SqlParameter[]
                    {
                        new SqlParameter() { ParameterName = "AccountNumber", Value = accountNumber },
                        new SqlParameter() { ParameterName = "ToAccountNumber", Value = toAccountNumber },
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

                    }
                }
            }
            catch (Exception ex)
            {
                transferMoneyViewModel.Message = ex.Message;
            }

            return transferMoneyViewModel;

        }

        public async Task<QueueResultViewModel> InsertToQueue(long accountNumber, long? fromToAccountNumber, decimal amount, int transactionTypeId)
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
    }
}
