using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using WalletApp.Model.ViewModel.Exceptions;
using WalletApp.Service.Interface;

namespace WalletApp.Service.Tests
{
    [TestFixture]
    public class UserWalletAccountServiceTest
    {
        private Mock<IDBService> dbService;

        [SetUp]
        public void Setup()
        {
            dbService = new Mock<IDBService>();
        }

        private IUserWalletAccountService GetUserWalletAccountService()
        {
            return new UserWalletAccountService(dbService.Object);
        }

        [Test]
        public void RegisterWallet_Returns_RegisterWalletViewModel_NotSuccess()
        {
            var service = GetUserWalletAccountService();
            //Arrange
            dbService.Setup(x => x.ExecuteQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), It.IsAny<CommandType>()).Result).
                Returns(new DataTable());

            //Act
            var result = service.RegisterWallet(Guid.NewGuid()).Result;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, new UnableToRegisterWalletException().Message);

        }

        [Test]
        public void RegisterWallet_Returns_RegisterWalletViewModel_Success()
        {
            var service = GetUserWalletAccountService();
            //Arrange
            dbService.Setup(x => x.ExecuteQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), It.IsAny<CommandType>()).Result).
                Returns(GenerateRegisterWalletAccountMock());

            //Act
            var result = service.RegisterWallet(Guid.NewGuid()).Result;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.IsSuccess, true);
            Assert.IsNull(result.Message);

        }

        #region GenerateMockData
        public DataTable GenerateRegisterWalletAccountMock()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn() { ColumnName = "AccountNumber", DataType = typeof(long) });
            DataRow row1 = dt.NewRow();
            row1["AccountNumber"] = 111111111111;
            dt.Rows.Add(row1);
          
            return dt;
        }
        #endregion
    }
}
