using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using WalletApp.Model.DomainModel;
using WalletApp.Model.ViewModel.Exceptions;
using WalletApp.Service.ConnectionStrings;
using WalletApp.Service.Interface;

namespace WalletApp.Service.Tests
{
    [TestFixture]
    public class UserSecurityServiceTest
    {
        private Mock<IDBService> dbService;
        private Mock<IUserWalletAccountService> userWalletAccountService;

        [SetUp]
        public void Setup()
        {
            dbService = new Mock<IDBService>();
            userWalletAccountService = new Mock<IUserWalletAccountService>();
                
        }

        private IUserSecurityService GetUserSecurityService()
        {
            return new UserSecurityService(dbService.Object, userWalletAccountService.Object);
        }

        [Test]
        public void AuthenticateUser_Returns_AuthenticatedLoginViewModel_NotSuccess()
        {
            var service = GetUserSecurityService();
            //Arrange
            dbService.Setup(x => x.ExecuteQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), It.IsAny<CommandType>()).Result).
                Returns(new DataTable());

            //Act
            var result = service.AuthenticateUser("login", "pass").Result;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, new UnauthorizedUserException().Message);

        }


        [Test]
        public void AuthenticateUser_Returns_AuthenticatedLoginViewModel_Success()
        {
            var service = GetUserSecurityService();
            //Arrange
            dbService.Setup(x => x.ExecuteQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), It.IsAny<CommandType>()).Result).
                Returns(GenerateUserSecurityMock());

            //Act
            var result = service.AuthenticateUser("login", "pass").Result;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.IsSuccess, true);
            Assert.IsNull(result.Message);

        }

        [Test]
        public void RegisterUser_Returns_RegisterUserViewModel_NotSuccess()
        {
            var service = GetUserSecurityService();
            //Arrange
            dbService.Setup(x => x.ExecuteQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), It.IsAny<CommandType>()).Result).
                Returns(GenerateUserRegisterMock_Null());

            //Act
            var result = service.RegisterUser("login", "pass").Result;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, new UnableToRegisterUserException().Message);

        }

        [Test]
        public void RegisterUser_Returns_RegisterUserViewModel_Success()
        {
            var service = GetUserSecurityService();
            //Arrange
            dbService.Setup(x => x.ExecuteQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), It.IsAny<CommandType>()).Result).
                Returns(GenerateUserRegisterMock());

            //Act
            var result = service.RegisterUser("login", "pass").Result;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.IsSuccess, true);
            Assert.IsNull(result.Message);

        }

        [Test]
        public void InsertToQueue_Returns_QueueResultViewModel_Success()
        {
            var service = GetUserSecurityService();
            //Arrange
            dbService.Setup(x => x.ExecuteQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), It.IsAny<CommandType>()).Result).
                Returns(GenerateInsertToQueueMock());

            //Act
            var result = service.InsertToQueue("login", "pass").Result;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.IsSuccess, true);
            Assert.IsNull(result.Message);

        }

        [Test]
        public void ProcessQueue_Returns_QueueResultViewModel_Success()
        {
            var service = GetUserSecurityService();
            //Arrange
            dbService.Setup(x => x.ExecuteQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), It.IsAny<CommandType>()).Result).
                Returns(GenerateProcessQueueMock());

            //Act
            var result = service.ProcessQueue().Result;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.IsSuccess, true);
            Assert.IsNull(result.Message);

        }

        [Test]
        public void UpdateQueue_Returns_QueueResultViewModel_Success()
        {
            var service = GetUserSecurityService();
            //Arrange
            dbService.Setup(x => x.ExecuteQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), It.IsAny<CommandType>()).Result).
                Returns(GenerateUpdateQueueMock());

            //Act
            var result = service.UpdateQueue(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<long>()).Result;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.IsSuccess, true);
            Assert.IsNull(result.Message);

        }

        #region GenerateMockData
        private DataTable GenerateUserSecurityMock()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn() { ColumnName = "Id", DataType = typeof(Guid) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Login", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "AccountNumber", DataType = typeof(long) });

            DataRow row1 = dt.NewRow();
            row1["Id"] = Guid.NewGuid();
            row1["Login"] = "user1";
            row1["AccountNumber"] = 111111111111;

            DataRow row2 = dt.NewRow();
            row2["Id"] = Guid.NewGuid();
            row2["Login"] = "user2";
            row2["AccountNumber"] = 111111111112;

            dt.Rows.Add(row1);
            dt.Rows.Add(row2);
            return dt;
        }

        private DataTable GenerateInsertToQueueMock()
        {
            DataTable dt = new DataTable();
           
            dt.Columns.Add(new DataColumn() { ColumnName = "QueueId", DataType = typeof(long) });

            DataRow row1 = dt.NewRow();
            row1["QueueId"] = 111111111111;

            dt.Rows.Add(row1);
         
            return dt;
        }

        private DataTable GenerateProcessQueueMock()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn() { ColumnName = "Password", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Login", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "queueId", DataType = typeof(long) });

            DataRow row1 = dt.NewRow();
            row1["queueId"] = 111111111111;
            row1["Login"] = "jvalezona";
            row1["Password"] = "jvalenzona";

            DataRow row2 = dt.NewRow();
            row2["queueId"] = 111111111112;
            row2["Login"] = "jvalezona";
            row2["Password"] = "jvalenzona";

            dt.Rows.Add(row1);
            dt.Rows.Add(row2);
            return dt;
        }

        private DataTable GenerateUpdateQueueMock()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn() { ColumnName = "QueueId", DataType = typeof(long) });
            dt.Columns.Add(new DataColumn() { ColumnName = "QueueStatusId", DataType = typeof(int) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Message", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "RegisteredUserId", DataType = typeof(Guid) });
            dt.Columns.Add(new DataColumn() { ColumnName = "RegisteredWalletAcctNo", DataType = typeof(long) });

            DataRow row1 = dt.NewRow();
            row1["QueueId"] = 12345;
            row1["QueueStatusId"] = 1;
            row1["Message"] = "test";
            row1["RegisteredUserId"] = Guid.NewGuid();
            row1["RegisteredWalletAcctNo"] = 111111111111;

            dt.Rows.Add(row1);

            return dt;
        }

        private DataTable GenerateUserRegisterMock_Null()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn() { ColumnName = "UserSecurityID", DataType = typeof(Guid) });

            DataRow row1 = dt.NewRow();
            row1["UserSecurityID"] = DBNull.Value;
            dt.Rows.Add(row1);
            return dt;
        }

        private DataTable GenerateUserRegisterMock()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn() { ColumnName = "UserSecurityID", DataType = typeof(Guid) });

            DataRow row1 = dt.NewRow();
            row1["UserSecurityID"] = Guid.NewGuid();
            dt.Rows.Add(row1);
            return dt;
        }
        #endregion

    }
}
