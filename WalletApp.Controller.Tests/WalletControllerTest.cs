using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WalletApp.Api.Controllers;
using WalletApp.Model.ViewModel;
using WalletApp.Model.ViewModel.Exceptions;
using WalletApp.Service.Interface;

namespace WalletApp.Controller.Tests
{
    [TestFixture]
    public class WalletControllerTest
    {
        private Mock<IWalletTransactionService> dbWalletTransactionService;

        [SetUp]
        public void Setup()
        {
            dbWalletTransactionService = new Mock<IWalletTransactionService>();

        }

        #region Deposit

        [Test]
        public void WalletController_Deposit_Returns_BadRequest_TooLowAmountException()
        {
            var controller = GetWalletController();

            //Arrange
            dbWalletTransactionService.Setup(x => x.DepositMoney(It.IsAny<long>(), -1).Result)
               .Returns(GenerateDepositMoneyViewModel_TooLowAmountException());

            //Act
            var result = controller.Deposit(new Model.ViewModel.RequestBodyModel.TransactionViewModel() 
            { 
                AccountNumber = 111111111111, 
                Amount = -1 
            }).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.AreEqual(((ObjectResult)result).Value, new TooLowAmountException().Message);

        }

        [Test]
        public void WalletController_Deposit_Returns_BadRequest_MaximumAllowableAmountException()
        {
            var controller = GetWalletController();

            //Arrange
            dbWalletTransactionService.Setup(x => x.DepositMoney(It.IsAny<long>(), -1).Result)
                .Returns(GenerateDepositMoneyViewModel_MaximumAllowableAmountException());

            //Act
            var result = controller.Deposit(new Model.ViewModel.RequestBodyModel.TransactionViewModel()
            {
                AccountNumber = 111111111111,
                Amount = -1
            }).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.AreEqual(((ObjectResult)result).Value, new MaximumAllowableAmountException().Message);

        }

        [Test]
        public void WalletController_Deposit_Returns_BadRequest_RequiredFieldsException()
        {
            var controller = GetWalletController();

            //Arrange
            dbWalletTransactionService.Setup(x => x.DepositMoney(It.IsAny<long>(), -1).Result)
                .Returns(new DepositMoneyViewModel());

            //Act
            var result = controller.Deposit(new Model.ViewModel.RequestBodyModel.TransactionViewModel(){}).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.AreEqual(((ObjectResult)result).Value, new RequiredFieldsException().Message);

        }

        [Test]
        public void WalletController_Deposit_Returns_Ok()
        {
            var controller = GetWalletController();

            //Arrange
            dbWalletTransactionService.Setup(x => x.DepositMoney(It.IsAny<long>(), It.IsAny<decimal>()).Result)
                .Returns(new DepositMoneyViewModel());

            //Act
            var result = controller.Deposit(new Model.ViewModel.RequestBodyModel.TransactionViewModel() 
            {
                AccountNumber = 111111111111,
                Amount = 100
            }).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.AreEqual(((ObjectResult)result).Value.GetType(), typeof(DepositMoneyViewModel));

        }

        #endregion


        private WalletController GetWalletController()
        {
           return new WalletController
                (
                    dbWalletTransactionService.Object
                );

        }

        private DepositMoneyViewModel GenerateDepositMoneyViewModel_TooLowAmountException()
        {
            return new DepositMoneyViewModel()
            {
                Message = new TooLowAmountException().Message
            };
        }

        private DepositMoneyViewModel GenerateDepositMoneyViewModel_MaximumAllowableAmountException()
        {
            return new DepositMoneyViewModel()
            {
                Message = new MaximumAllowableAmountException().Message
            };
        }


    }
}
