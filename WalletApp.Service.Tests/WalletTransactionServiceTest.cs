using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using WalletApp.Model.Enums;
using WalletApp.Model.ViewModel.Exceptions;
using WalletApp.Service.ConnectionStrings;
using WalletApp.Service.Interface;

namespace WalletApp.Service.Tests
{
    [TestFixture]
    public class WalletTransactionServiceTest
    {
        private Mock<IDBService> dbService;

        [SetUp]
        public void Setup()
        {
            dbService = new Mock<IDBService>();
        }

        private IWalletTransactionService GetWalletTransactionService()
        {
            return new WalletTransactionService(dbService.Object);
        }

        #region Report

        [Test]
        public void ViewTransactionHistoryAll_Returns_TransactionHistoryListViewModel_EndOfLine()
        {
            var service = GetWalletTransactionService();
            //Arrange
            dbService.Setup(x => x.ExecuteQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), It.IsAny<CommandType>()).Result).
                Returns(new DataTable());

            //Act
            var result = service.ViewTransactionHistoryAll(It.IsAny<long>(), It.IsAny<int>()).Result;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.IsSuccess, true);
            Assert.AreEqual(result.InfoMessage, "No more records to be display");

        }

        [Test]
        public void ViewTransactionHistoryAll_Returns_TransactionHistoryListViewModel_Success()
        {
            var service = GetWalletTransactionService();
            //Arrange
            dbService.Setup(x => x.ExecuteQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), It.IsAny<CommandType>()).Result).
                Returns(GenerateTransactionHistoryMock());

            //Act
            var result = service.ViewTransactionHistoryAll(It.IsAny<long>(), 0).Result;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.IsSuccess, true);
            Assert.IsNull(result.Message);
            Assert.IsNull(result.InfoMessage);

        }

        [Test]
        public void ViewTransactionHistoryByRange_Returns_TransactionHistoryListViewModel_EndOfLine()
        {
            var service = GetWalletTransactionService();
            //Arrange
            dbService.Setup(x => x.ExecuteQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), It.IsAny<CommandType>()).Result).
                Returns(new DataTable());

            //Act
            var result = service.ViewTransactionHistoryByRange(It.IsAny<long>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()).Result;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.IsSuccess, true);
            Assert.AreEqual(result.InfoMessage, "No more records to be display");

        }

        [Test]
        public void ViewTransactionHistoryByRange_Returns_TransactionHistoryListViewModel_Success()
        {
            var service = GetWalletTransactionService();
            //Arrange
            dbService.Setup(x => x.ExecuteQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), It.IsAny<CommandType>()).Result).
                Returns(GenerateTransactionHistoryMock());

            //Act
            var result = service.ViewTransactionHistoryByRange(It.IsAny<long>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()).Result;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.IsSuccess, true);
            Assert.IsNull(result.Message);
            Assert.IsNull(result.InfoMessage);

        }

        #endregion

        #region Deposit

        [Test]
        public void DepositMoney_Returns_DepositMoneyViewModel_Success()
        {
            var service = GetWalletTransactionService();
            //Arrange
            dbService.Setup(x => x.ExecuteQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), It.IsAny<CommandType>()).Result)
                .Returns(GenerateSuccessCredit());

            //Act
            var result = service.DepositMoney(It.IsAny<long>(), null, 99999999, (int)TransactionTypes.Deposit).Result;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.IsSuccess, true);
            Assert.IsNull(result.Message);
            Assert.IsNotNull(result.InfoMessage);

        }

        [Test]
        public void DepositMoney_Returns_DepositMoneyViewModel_NotSuccess_TooLowAmountException()
        {
            var service = GetWalletTransactionService();
            //Arrange
            dbService.Setup(x => x.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), It.IsAny<CommandType>()));

            //Act
            var result = service.DepositMoney(It.IsAny<long>(), null, -100, (int)TransactionTypes.Deposit).Result;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, new TooLowAmountException().Message);

        }

        [Test]
        public void DepositMoney_Returns_DepositMoneyViewModel_NotSuccess_MaxAmountException()
        {
            var service = GetWalletTransactionService();
            //Arrange
            dbService.Setup(x => x.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), It.IsAny<CommandType>()));

            //Act
            var result = service.DepositMoney(It.IsAny<long>(), null, 999999999999, (int)TransactionTypes.Deposit).Result;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, new MaximumAllowableAmountException().Message);

        }

        #endregion

        #region Withdraw

        [Test]
        public void WithdrawMoney_Returns_WithdrawMoneyViewModel_Success()
        {
            var service = GetWalletTransactionService();
            //Arrange
            dbService.Setup(x => x.ExecuteQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), It.IsAny<CommandType>()).Result)
                .Returns(GenerateSuccessCredit());

            //Act
            var result = service.WithdrawMoney(It.IsAny<long>(), 99999999).Result;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.IsSuccess, true);
            Assert.IsNull(result.Message);
            Assert.IsNotNull(result.InfoMessage);

        }
        [Test]
        public void WithdrawMoney_Returns_WithdrawMoneyViewModel_NotSuccess_TooLowAmountException()
        {
            var service = GetWalletTransactionService();
            //Arrange
            dbService.Setup(x => x.ExecuteQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), It.IsAny<CommandType>()).Result)
                .Returns(GenerateNotSuccessCredit());

            //Act
            var result = service.WithdrawMoney(It.IsAny<long>(), -100).Result;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, new TooLowAmountException().Message);

        }
        [Test]
        public void WithdrawMoney_Returns_WithdrawMoneyViewModel_NotSuccess_MaxAmountException()
        {
            var service = GetWalletTransactionService();
            //Arrange
            dbService.Setup(x => x.ExecuteQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), It.IsAny<CommandType>()).Result)
                .Returns(GenerateNotSuccessCredit());

            //Act
            var result = service.WithdrawMoney(It.IsAny<long>(), 999999999999).Result;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, new MaximumAllowableAmountException().Message);

        }
        [Test]
        public void WithdrawMoney_Returns_WithdrawMoneyViewModel_NotSuccess_InsufficientBalException()
        {
            var service = GetWalletTransactionService();
            //Arrange
            dbService.Setup(x => x.ExecuteQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), It.IsAny<CommandType>()).Result)
                .Returns(GenerateNotSuccessCredit());

            //Act
            var result = service.WithdrawMoney(It.IsAny<long>(), 99999999).Result;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, new InsufficientWalletBalanceException().Message);

        }

        #endregion

        #region Transfer

        [Test]
        public void TransferMoney_Returns_TransferMoneyViewModel_Success()
        {
            var service = GetWalletTransactionService();
            //Arrange
            dbService.Setup(x => x.ExecuteQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), It.IsAny<CommandType>()).Result)
                .Returns(GenerateSuccessCredit());

            //Act
            var result = service.TransferMoney(It.IsAny<long>(), It.IsAny<long>(), 99999999).Result;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.IsSuccess, true);
            Assert.IsNull(result.Message);
            Assert.IsNotNull(result.InfoMessage);

        }
        [Test]
        public void TransferMoney_Returns_TransferMoneyViewModel_NotSuccess_TooLowAmountException()
        {
            var service = GetWalletTransactionService();
            //Arrange
            dbService.Setup(x => x.ExecuteQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), It.IsAny<CommandType>()).Result)
                .Returns(GenerateNotSuccessCredit());

            //Act
            var result = service.TransferMoney(It.IsAny<long>(), It.IsAny<long>() , - 100).Result;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, new TooLowAmountException().Message);

        }
        [Test]
        public void TransferMoney_Returns_TransferMoneyViewModel_NotSuccess_MaxAmountException()
        {
            var service = GetWalletTransactionService();
            //Arrange
            dbService.Setup(x => x.ExecuteQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), It.IsAny<CommandType>()).Result)
                .Returns(GenerateNotSuccessCredit());

            //Act
            var result = service.TransferMoney(It.IsAny<long>(), It.IsAny<long>(), 999999999999).Result;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, new MaximumAllowableAmountException().Message);

        }
        [Test]
        public void TransferMoney_Returns_TransferMoneyViewModel_NotSuccess_InsufficientBalException()
        {
            var service = GetWalletTransactionService();
            //Arrange
            dbService.Setup(x => x.ExecuteQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), It.IsAny<CommandType>()).Result)
                .Returns(GenerateNotSuccessCredit());

            //Act
            var result = service.TransferMoney(It.IsAny<long>(), It.IsAny<long>(), 99999999).Result;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, new InsufficientWalletBalanceException().Message);

        }

       

        #endregion

        #region GenerateTransactionHistoryMock
        public DataTable GenerateTransactionHistoryMock()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn() { ColumnName = "TransactionType", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Amount", DataType = typeof(decimal) });
            dt.Columns.Add(new DataColumn() { ColumnName = "FromToAccountNumber", DataType = typeof(long) });
            dt.Columns.Add(new DataColumn() { ColumnName = "TransactionDate", DataType = typeof(DateTime) });
            dt.Columns.Add(new DataColumn() { ColumnName = "EndBalance", DataType = typeof(decimal) });

            DataRow row1 = dt.NewRow();
            row1["TransactionType"] = "testType1";
            row1["Amount"] = 1000;
            row1["FromToAccountNumber"] = 111111111111;
            row1["TransactionDate"] = DateTime.Now;
            row1["EndBalance"] = 1000;
            dt.Rows.Add(row1);

            DataRow row2 = dt.NewRow();
            row2["TransactionType"] = "testType2";
            row2["Amount"] = 1000;
            row2["FromToAccountNumber"] = DBNull.Value;
            row2["TransactionDate"] = DateTime.Now;
            row2["EndBalance"] = 1000;
            dt.Rows.Add(row2);

            DataRow row3 = dt.NewRow();
            row3["TransactionType"] = "testType3";
            row3["Amount"] = 1000;
            row3["FromToAccountNumber"] = 22222222222;
            row3["TransactionDate"] = DateTime.Now;
            row3["EndBalance"] = 1000;
            dt.Rows.Add(row3);

            return dt;
        }
        public DataTable GenerateSuccessCredit()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn() { ColumnName = "IsSuccess", DataType = typeof(bool) });
            dt.Columns.Add(new DataColumn() { ColumnName = "EndBalance", DataType = typeof(decimal) });

            DataRow row1 = dt.NewRow();
            row1["IsSuccess"] = true;
            row1["EndBalance"] = 1500;
            dt.Rows.Add(row1);

            return dt;
        }

        public DataTable GenerateNotSuccessCredit()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn() { ColumnName = "IsSuccess", DataType = typeof(bool) });

            DataRow row1 = dt.NewRow();
            row1["IsSuccess"] = false;
            dt.Rows.Add(row1);

            return dt;
        }

        #endregion

    }
}
