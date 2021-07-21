using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using WalletApp.Model.DomainModel;
using WalletApp.Service.ConnectionStrings;
using WalletApp.Service.Interface;

namespace WalletApp.Service.Tests
{
    [TestFixture]
    public class UserSecurityServiceTest
    {
        private Mock<IDBService> dbService;

        [SetUp]
        public void Setup()
        {
            dbService = new Mock<IDBService>();
        }

        private IUserSecurityService GetUserSecurityService()
        {
            return new UserSecurityService(dbService.Object);
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
            Assert.AreEqual(result.Message, "Invalid credentials!");

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
            Assert.AreEqual(result.Message, "Unable to register. Login already exists!");

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
