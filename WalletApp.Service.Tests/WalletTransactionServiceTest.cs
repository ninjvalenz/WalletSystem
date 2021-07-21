using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WalletApp.Service.ConnectionStrings;
using WalletApp.Service.Interface;

namespace WalletApp.Service.Tests
{
    [TestFixture]
    public class WalletTransactionServiceTest
    {
        private Mock<ISequelConnection> dbConnection;

        [SetUp]
        public void Setup()
        {
            dbConnection = new Mock<ISequelConnection>();
        }

        private IWalletTransactionService GetUserSecurityService()
        {
            return new WalletTransactionService(dbConnection.Object);
        }

        [Test]
        public void DepositMoney_Returns_DepositMoneyViewModel()
        {

        }
    }
}
