using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
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

        [Test]
        public void ViewTransactionHistoryAll_Returns_TransactionHistoryListViewModel_EndOfLine()
        {
            var service = GetWalletTransactionService();
            //Arrange
            dbService.Setup(x => x.ExecuteQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), It.IsAny<CommandType>()).Result).
                Returns(new DataTable());

            //Act
            var result = service.ViewTransactionHistoryAll(111111111111, 0).Result;

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
            var result = service.ViewTransactionHistoryAll(111111111111, 0).Result;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.IsSuccess, true);
            Assert.IsNull(result.Message);
            Assert.IsNull(result.InfoMessage);

        }

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

        #endregion

    }
}
