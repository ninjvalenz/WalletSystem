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
        private Mock<ISequelConnection> dbConnection;

        [SetUp]
        public void Setup()
        {
            dbConnection = new Mock<ISequelConnection>();
        }

        private IUserSecurityService GetUserSecurityService()
        {
            return new UserSecurityService(dbConnection.Object);
        }

        [Test]
        public void AuthenticateUser_Returns_AuthenticatedLoginViewModelTest()
        {
            var mockedDataReader = new Mock<IDataReader>();
          
        }

        //private static Mock<AuthenticatedLogin> MockExecuteReader(Dictionary<string, object> returnValues)
        //{
            //var mockedDataReader = new Mock<IDataReader>();
            //bool readFlag = true;
            //mockedDataReader.Setup(x => x.Read()).Returns(() => readFlag).Callback(() => readFlag = false);
            //foreach (KeyValuePair<string, object> keyVal in returnValues)
            //{
            //    mockedDataReader.Setup(x => x[keyVal.Key]).Returns(keyVal.Value);
            //}
            //Mock<DbProviderFactory> mockedDBFactory = new Mock<DbProviderFactory>();
            //Mock<DbCommand> mockedDB = new Mock<DbCommand>("MockedDB", mockedDBFactory.Object);

            //Queue<object> responseQueue = new Queue<object>();
            //responseQueue.Enqueue("First");
            //responseQueue.Enqueue("Second");
            //responseQueue.Enqueue("Third");
            //mockedService.Setup(x => x.myMethod(It.IsAny<string>())).Returns(() => responseQueue.Dequeue());

            //mockedDB.Setup(x => x.ExecuteReader(It.IsAny<AuthenticatedLogin>())).Returns(mockedDataReader.Object);
            //return mockedDB;
        //}


    }
}
