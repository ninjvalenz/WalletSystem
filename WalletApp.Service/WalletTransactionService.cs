﻿using System;
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

                //using (SqlConnection connection =
                //    new SqlConnection(dbConnection.ConnectionString))
                //{
                //    await connection.OpenAsync();

                //    using (SqlCommand command = new SqlCommand("ViewTransactionHistoryByRange", connection))
                //    {
                //        command.CommandType = CommandType.StoredProcedure;
                //        command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                //        command.Parameters.AddWithValue("@FromDate", fromDate);
                //        command.Parameters.AddWithValue("@ToDate", toDate);

                //        //using (SqlDataReader sqlDataReader = await command.ExecuteReaderAsync())
                //        //{
                //        //    while (await sqlDataReader.ReadAsync())
                //        //    {
                //        //        var transactionInstance = new TransactionHistoryViewModel();
                //        //        try
                //        //        {
                //        //            transactionInstance.TransactionType = sqlDataReader.GetFieldValue<string>("TransactionType");
                //        //            transactionInstance.TransactionAmount = sqlDataReader.GetFieldValue<decimal>("Amount");
                //        //            transactionInstance.FromToAccountNumber = (sqlDataReader["FromToAccountNumber"] as long?) ?? 0;
                //        //            transactionInstance.TransactionDate = sqlDataReader.GetFieldValue<DateTime>("TransactionDate");
                //        //            transactionInstance.TransactionEndBalance = sqlDataReader.GetFieldValue<decimal>("EndBalance");
                //        //        }
                //        //        catch (Exception ex)
                //        //        {
                //        //            transactionInstance.Message = ex.Message;
                //        //        }
                //        //        finally
                //        //        {
                //        //            transactionListViewModel.TransactionHistoryViewModels.Add(transactionInstance);
                //        //        }
                //        //    }
                //        //}
                //    }
                //}

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
            double amount)
        {
            DepositMoneyViewModel depositMoneyViewModel = new DepositMoneyViewModel();

            try
            {
                //using (SqlConnection connection =
                //  new SqlConnection(dbConnection.ConnectionString))
                //{
                //    await connection.OpenAsync();

                //    using (SqlCommand command = new SqlCommand("DepositMoney", connection))
                //    {
                //        command.CommandType = CommandType.StoredProcedure;
                //        command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                //        command.Parameters.AddWithValue("@Amount", amount);

                //        await command.ExecuteNonQueryAsync();
                //    }
                //}
            }
            catch(Exception ex)
            {
                depositMoneyViewModel.Message = ex.Message;
            }

            return depositMoneyViewModel;
        }

        public async Task<WithdrawMoneyViewModel> WithdrawMoney(
            long accountNumber, 
            double amount)
        {
            WithdrawMoneyViewModel withdrawMoneyViewModel = new WithdrawMoneyViewModel();

           // try
           // {
           //     using (SqlConnection connection =
           //new SqlConnection(dbConnection.ConnectionString))
           //     {
           //         await connection.OpenAsync();

           //         using (SqlCommand command = new SqlCommand("WithdrawMoney", connection))
           //         {
           //             command.CommandType = CommandType.StoredProcedure;
           //             command.Parameters.AddWithValue("@AccountNumber", accountNumber);
           //             command.Parameters.AddWithValue("@Amount", amount);

           //             using (SqlDataReader sqlDataReader = await command.ExecuteReaderAsync())
           //             {
           //                 if (await sqlDataReader.ReadAsync())
           //                 {
           //                     if (!sqlDataReader.GetFieldValue<bool>("IsSuccess"))
           //                         throw new InsufficientWalletBalanceException();
           //                 }
           //             }
           //         }
           //     }

           // }
           // catch (Exception ex)
           // {
           //     withdrawMoneyViewModel.Message = ex.Message;
           // }

            return withdrawMoneyViewModel;
        }

        public async Task<TransferMoneyViewModel> TransferMoney(
            long accountNumber, 
            long toAccountNumber, 
            double amount)
        {
            TransferMoneyViewModel transferMoneyViewModel = new TransferMoneyViewModel();

            try
            {
             //   using (SqlConnection connection =
             //new SqlConnection(dbConnection.ConnectionString))
             //   {
             //       await connection.OpenAsync();

             //       using (SqlCommand command = new SqlCommand("TransferMoney", connection))
             //       {
             //           command.CommandType = CommandType.StoredProcedure;
             //           command.Parameters.AddWithValue("@AccountNumber", accountNumber);
             //           command.Parameters.AddWithValue("@ToAccountNumber", toAccountNumber);
             //           command.Parameters.AddWithValue("@Amount", amount);

             //           using (SqlDataReader sqlDataReader = await command.ExecuteReaderAsync())
             //           {
             //               if (await sqlDataReader.ReadAsync())
             //               {
             //                   if (!sqlDataReader.GetFieldValue<bool>("IsSuccess"))
             //                       throw new InsufficientWalletBalanceException();
             //               }
             //           }
             //       }
             //   }
            }
            catch (Exception ex)
            {
                transferMoneyViewModel.Message = ex.Message;
            }

            return transferMoneyViewModel;

        }
    }
}
