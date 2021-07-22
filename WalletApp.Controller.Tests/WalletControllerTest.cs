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
            dbWalletTransactionService.Setup(x => x.DepositMoney(It.IsAny<long>(), 999999999999).Result)
                .Returns(GenerateDepositMoneyViewModel_MaximumAllowableAmountException());

            //Act
            var result = controller.Deposit(new Model.ViewModel.RequestBodyModel.TransactionViewModel()
            {
                AccountNumber = 111111111111,
                Amount = 999999999999
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
            dbWalletTransactionService.Setup(x => x.DepositMoney(It.IsAny<long>(), 100).Result)
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
            Assert.IsNull(((MethodResult)((ObjectResult)result).Value).InfoMessage);
            Assert.IsNull(((MethodResult)((ObjectResult)result).Value).Message);

        }

        #endregion

        #region Withdraw

        [Test]
        public void WalletController_Withdraw_Returns_BadRequest_TooLowAmountException()
        {
            var controller = GetWalletController();

            //Arrange
            dbWalletTransactionService.Setup(x => x.WithdrawMoney(It.IsAny<long>(), -1).Result)
               .Returns(GenerateWithdrawMoneyViewModel_TooLowAmountException());

            //Act
            var result = controller.Withdraw(new Model.ViewModel.RequestBodyModel.TransactionViewModel()
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
        public void WalletController_Withdraw_Returns_BadRequest_MaximumAllowableAmountException()
        {
            var controller = GetWalletController();

            //Arrange
            dbWalletTransactionService.Setup(x => x.WithdrawMoney(It.IsAny<long>(), 999999999999).Result)
                .Returns(GenerateWithdrawMoneyViewModel_MaximumAllowableAmountException());

            //Act
            var result = controller.Withdraw(new Model.ViewModel.RequestBodyModel.TransactionViewModel()
            {
                AccountNumber = 111111111111,
                Amount = 999999999999
            }).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.AreEqual(((ObjectResult)result).Value, new MaximumAllowableAmountException().Message);

        }

        [Test]
        public void WalletController_Withdraw_Returns_BadRequest_InsufficientWalletBalanceException()
        {
            var controller = GetWalletController();

            //Arrange
            dbWalletTransactionService.Setup(x => x.WithdrawMoney(It.IsAny<long>(), 100).Result)
                .Returns(GenerateWithdrawMoneyViewModel_InsufficientWalletBalanceException());

            //Act
            var result = controller.Withdraw(new Model.ViewModel.RequestBodyModel.TransactionViewModel()
            {
                AccountNumber = 111111111111,
                Amount = 100
            }).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.AreEqual(((ObjectResult)result).Value, new InsufficientWalletBalanceException().Message);

        }

        [Test]
        public void WalletController_Withdraw_Returns_BadRequest_RequiredFieldsException()
        {
            var controller = GetWalletController();

            //Arrange
            dbWalletTransactionService.Setup(x => x.WithdrawMoney(It.IsAny<long>(), -1).Result)
                .Returns(new WithdrawMoneyViewModel());

            //Act
            var result = controller.Withdraw(new Model.ViewModel.RequestBodyModel.TransactionViewModel() { }).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.AreEqual(((ObjectResult)result).Value, new RequiredFieldsException().Message);

        }

        [Test]
        public void WalletController_Withdraw_Returns_Ok()
        {
            var controller = GetWalletController();

            //Arrange
            dbWalletTransactionService.Setup(x => x.WithdrawMoney(It.IsAny<long>(), 100).Result)
                .Returns(new WithdrawMoneyViewModel());

            //Act
            var result = controller.Withdraw(new Model.ViewModel.RequestBodyModel.TransactionViewModel()
            {
                AccountNumber = 111111111111,
                Amount = 100
            }).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.AreEqual(((ObjectResult)result).Value.GetType(), typeof(WithdrawMoneyViewModel));
            Assert.IsNull(((MethodResult)((ObjectResult)result).Value).InfoMessage);
            Assert.IsNull(((MethodResult)((ObjectResult)result).Value).Message);

        }

        #endregion

        #region Transfer

        [Test]
        public void WalletController_Transfer_Returns_BadRequest_TooLowAmountException()
        {
            var controller = GetWalletController();

            //Arrange
            dbWalletTransactionService.Setup(x => x.TransferMoney(It.IsAny<long>(), It.IsAny<long>(), -1).Result)
               .Returns(GenerateTransferMoneyViewModel_TooLowAmountException());

            //Act
            var result = controller.Transfer(new Model.ViewModel.RequestBodyModel.NewTransferViewModel()
            {
                AccountNumber = 111111111111,
                ToAccountNumber = 222222222222,
                Amount = -1
            }).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.AreEqual(((ObjectResult)result).Value, new TooLowAmountException().Message);

        }

        [Test]
        public void WalletController_Transfer_Returns_BadRequest_MaximumAllowableAmountException()
        {
            var controller = GetWalletController();

            //Arrange
            dbWalletTransactionService.Setup(x => x.TransferMoney(It.IsAny<long>(), It.IsAny<long>(), 999999999999).Result)
                .Returns(GenerateTransferMoneyViewModel_MaximumAllowableAmountException());

            //Act
            var result = controller.Transfer(new Model.ViewModel.RequestBodyModel.NewTransferViewModel()
            {
                AccountNumber = 111111111111,
                ToAccountNumber = 222222222222,
                Amount = 999999999999
            }).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.AreEqual(((ObjectResult)result).Value, new MaximumAllowableAmountException().Message);

        }

        [Test]
        public void WalletController_Transfer_Returns_BadRequest_InsufficientWalletBalanceException()
        {
            var controller = GetWalletController();

            //Arrange
            dbWalletTransactionService.Setup(x => x.TransferMoney(It.IsAny<long>(), It.IsAny<long>(), 100).Result)
                .Returns(GenerateTransferMoneyViewModel_InsufficientWalletBalanceException());

            //Act
            var result = controller.Transfer(new Model.ViewModel.RequestBodyModel.NewTransferViewModel()
            {
                AccountNumber = 111111111111,
                ToAccountNumber = 222222222222,
                Amount = 100
            }).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.AreEqual(((ObjectResult)result).Value, new InsufficientWalletBalanceException().Message);

        }

        [Test]
        public void WalletController_Transfer_Returns_BadRequest_RequiredFieldsException()
        {
            var controller = GetWalletController();

            //Arrange
            dbWalletTransactionService.Setup(x => x.TransferMoney(It.IsAny<long>(), It.IsAny<long>(), -1).Result)
                .Returns(new TransferMoneyViewModel());

            //Act
            var result = controller.Transfer(new Model.ViewModel.RequestBodyModel.NewTransferViewModel() { }).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.AreEqual(((ObjectResult)result).Value, new RequiredFieldsException().Message);

        }

        [Test]
        public void WalletController_Transfer_Returns_Ok()
        {
            var controller = GetWalletController();

            //Arrange
            dbWalletTransactionService.Setup(x => x.TransferMoney(It.IsAny<long>(), It.IsAny<long>(), 100).Result)
                .Returns(new TransferMoneyViewModel());

            //Act
            var result = controller.Transfer(new Model.ViewModel.RequestBodyModel.NewTransferViewModel()
            {
                AccountNumber = 111111111111,
                ToAccountNumber = 222222222222,
                Amount = 100
            }).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.AreEqual(((ObjectResult)result).Value.GetType(), typeof(TransferMoneyViewModel));
            Assert.IsNull(((MethodResult)((ObjectResult)result).Value).InfoMessage);
            Assert.IsNull(((MethodResult)((ObjectResult)result).Value).Message);

        }

        #endregion

        #region Report

        [Test]
        public void WalletController_HistoryAll_BadRequest_RequiredFieldsException()
        {
            var controller = GetWalletController();

            //Arrange
            dbWalletTransactionService.Setup(x => x.ViewTransactionHistoryAll(It.IsAny<long>(), It.IsAny<int>()).Result)
               .Returns(new TransactionHistoryListViewModel() { });

            //Act
            var result = controller.HistoryAll(new Model.ViewModel.RequestBodyModel.HistoryAllViewModel(){}).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.AreEqual(((ObjectResult)result).Value, new RequiredFieldsException().Message);

        }

        [Test]
        public void WalletController_HistoryAll_Ok_EndOfLine()
        {
            var controller = GetWalletController();

            //Arrange
            dbWalletTransactionService.Setup(x => x.ViewTransactionHistoryAll(It.IsAny<long>(), It.IsAny<int>()).Result)
               .Returns(new TransactionHistoryListViewModel()
               {
                   InfoMessage = "No more records to be display"
               });

            //Act
            var result = controller.HistoryAll(new Model.ViewModel.RequestBodyModel.HistoryAllViewModel()
            {
                AccountNumber = 111111111111,
                Offset = 10000
            }).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.AreEqual(((ObjectResult)result).Value.GetType(), typeof(TransactionHistoryListViewModel));
            Assert.AreEqual(((MethodResult)((ObjectResult)result).Value).InfoMessage, "No more records to be display");
           
        }

        [Test]
        public void WalletController_HistoryAll_Ok()
        {
            var controller = GetWalletController();

            //Arrange
            dbWalletTransactionService.Setup(x => x.ViewTransactionHistoryAll(It.IsAny<long>(), It.IsAny<int>()).Result)
               .Returns(new TransactionHistoryListViewModel(){});

            //Act
            var result = controller.HistoryAll(new Model.ViewModel.RequestBodyModel.HistoryAllViewModel()
            {
                AccountNumber = 111111111111,
                Offset = 10000
            }).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.AreEqual(((ObjectResult)result).Value.GetType(), typeof(TransactionHistoryListViewModel));
            Assert.IsNull(((MethodResult)((ObjectResult)result).Value).InfoMessage);
            Assert.IsNull(((MethodResult)((ObjectResult)result).Value).Message);

        }

        [Test]
        public void WalletController_HistoryByRange_BadRequest_RequiredFieldsException()
        {
            var controller = GetWalletController();

            //Arrange
            dbWalletTransactionService.Setup(x => x.ViewTransactionHistoryByRange(It.IsAny<long>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()).Result)
               .Returns(new TransactionHistoryListViewModel() { });

            //Act
            var result = controller.HistoryByRange(new Model.ViewModel.RequestBodyModel.HistoryByRangeViewModel() { }).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.AreEqual(((ObjectResult)result).Value, new RequiredFieldsException().Message);

        }

        [Test]
        public void WalletController_HistoryByRange_Ok_EndOfLine()
        {
            var controller = GetWalletController();

            //Arrange
            dbWalletTransactionService.Setup(x => x.ViewTransactionHistoryByRange(It.IsAny<long>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()).Result)
               .Returns(new TransactionHistoryListViewModel()
               {
                   InfoMessage = "No more records to be display"
               });

            //Act
            var result = controller.HistoryByRange(new Model.ViewModel.RequestBodyModel.HistoryByRangeViewModel()
            {
                AccountNumber = 111111111111,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now
            }).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.AreEqual(((ObjectResult)result).Value.GetType(), typeof(TransactionHistoryListViewModel));
            Assert.AreEqual(((MethodResult)((ObjectResult)result).Value).InfoMessage, "No more records to be display");

        }

        [Test]
        public void WalletController_HistoryByRange_Ok()
        {
            var controller = GetWalletController();

            //Arrange
            dbWalletTransactionService.Setup(x => x.ViewTransactionHistoryByRange(It.IsAny<long>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()).Result)
               .Returns(new TransactionHistoryListViewModel() { });

            //Act
            var result = controller.HistoryByRange(new Model.ViewModel.RequestBodyModel.HistoryByRangeViewModel()
            {
                AccountNumber = 111111111111,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now
            }).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.AreEqual(((ObjectResult)result).Value.GetType(), typeof(TransactionHistoryListViewModel));
            Assert.IsNull(((MethodResult)((ObjectResult)result).Value).InfoMessage);
            Assert.IsNull(((MethodResult)((ObjectResult)result).Value).Message);

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

        private WithdrawMoneyViewModel GenerateWithdrawMoneyViewModel_TooLowAmountException()
        {
            return new WithdrawMoneyViewModel()
            {
                Message = new TooLowAmountException().Message
            };
        }

        private WithdrawMoneyViewModel GenerateWithdrawMoneyViewModel_MaximumAllowableAmountException()
        {
            return new WithdrawMoneyViewModel()
            {
                Message = new MaximumAllowableAmountException().Message
            };
        }

        private WithdrawMoneyViewModel GenerateWithdrawMoneyViewModel_InsufficientWalletBalanceException()
        {
            return new WithdrawMoneyViewModel()
            {
                Message = new InsufficientWalletBalanceException().Message
            };
        }

        private TransferMoneyViewModel GenerateTransferMoneyViewModel_TooLowAmountException()
        {
            return new TransferMoneyViewModel()
            {
                Message = new TooLowAmountException().Message
            };
        }

        private TransferMoneyViewModel GenerateTransferMoneyViewModel_MaximumAllowableAmountException()
        {
            return new TransferMoneyViewModel()
            {
                Message = new MaximumAllowableAmountException().Message
            };
        }

        private TransferMoneyViewModel GenerateTransferMoneyViewModel_InsufficientWalletBalanceException()
        {
            return new TransferMoneyViewModel()
            {
                Message = new InsufficientWalletBalanceException().Message
            };
        }


    }
}
