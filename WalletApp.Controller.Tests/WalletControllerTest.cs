using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WalletApp.Api.Controllers;
using WalletApp.Model.Enums;
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
            dbWalletTransactionService.Setup(x => x.InsertToQueue(It.IsAny<long>(), null, -1, (int)TransactionTypes.Deposit).Result)
               .Returns(GenerateQueueResultViewModel_TooLowAmountException());

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
            dbWalletTransactionService.Setup(x => x.InsertToQueue(It.IsAny<long>(), null, 999999999999, (int)TransactionTypes.Deposit).Result)
                .Returns(GenerateQueueResultViewModel_MaximumAllowableAmountException());

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
            dbWalletTransactionService.Setup(x => x.InsertToQueue(It.IsAny<long>(), null, -1, (int)TransactionTypes.Deposit).Result)
                .Returns(new QueueResultViewModel() { Message = "" });

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
            dbWalletTransactionService.Setup(x => x.InsertToQueue(It.IsAny<long>(), null, 100, (int)TransactionTypes.Deposit).Result)
                .Returns(new QueueResultViewModel() { Message = null });

            //Act
            var result = controller.Deposit(new Model.ViewModel.RequestBodyModel.TransactionViewModel() 
            {
                AccountNumber = 111111111111,
                Amount = 100
            }).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.AreEqual(((ObjectResult)result).Value.GetType(), typeof(QueueResultViewModel));
            Assert.IsNull(((MethodResult)((ObjectResult)result).Value).Message);

        }

        #endregion

        #region Withdraw

        [Test]
        public void WalletController_Withdraw_Returns_BadRequest_TooLowAmountException()
        {
            var controller = GetWalletController();

            //Arrange
            dbWalletTransactionService.Setup(x => x.InsertToQueue(It.IsAny<long>(), null, -1, (int)TransactionTypes.Withdraw).Result)
               .Returns(GenerateQueueResultViewModel_TooLowAmountException());


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
            dbWalletTransactionService.Setup(x => x.InsertToQueue(It.IsAny<long>(), null, 999999999999, (int)TransactionTypes.Withdraw).Result)
                .Returns(GenerateQueueResultViewModel_MaximumAllowableAmountException());

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
            dbWalletTransactionService.Setup(x => x.InsertToQueue(It.IsAny<long>(), null, 100, (int)TransactionTypes.Withdraw).Result)
                 .Returns(GenerateQueueResultViewModel_InsufficientWalletBalanceException());

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
            dbWalletTransactionService.Setup(x => x.InsertToQueue(It.IsAny<long>(), null, -1, (int)TransactionTypes.Withdraw).Result)
                 .Returns(new QueueResultViewModel() { Message = "" });

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
            dbWalletTransactionService.Setup(x => x.InsertToQueue(It.IsAny<long>(), null, 100, (int)TransactionTypes.Withdraw).Result)
                .Returns(new QueueResultViewModel() { Message = null });

            //Act
            var result = controller.Withdraw(new Model.ViewModel.RequestBodyModel.TransactionViewModel()
            {
                AccountNumber = 111111111111,
                Amount = 100
            }).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.AreEqual(((ObjectResult)result).Value.GetType(), typeof(QueueResultViewModel));
            Assert.IsNull(((MethodResult)((ObjectResult)result).Value).Message);

        }

        #endregion

        #region Transfer

        [Test]
        public void WalletController_Transfer_Returns_BadRequest_TooLowAmountException()
        {
            var controller = GetWalletController();

            //Arrange
            dbWalletTransactionService.Setup(x => x.InsertToQueue(It.IsAny<long>(), It.IsAny<long>(), -1, (int)TransactionTypes.Transfer).Result)
              .Returns(GenerateQueueResultViewModel_TooLowAmountException());


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
            dbWalletTransactionService.Setup(x => x.InsertToQueue(It.IsAny<long>(), It.IsAny<long>(), 999999999999, (int)TransactionTypes.Transfer).Result)
              .Returns(GenerateQueueResultViewModel_MaximumAllowableAmountException());


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
            dbWalletTransactionService.Setup(x => x.InsertToQueue(It.IsAny<long>(), It.IsAny<long>(), 100, (int)TransactionTypes.Transfer).Result)
                .Returns(GenerateQueueResultViewModel_InsufficientWalletBalanceException());

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
            dbWalletTransactionService.Setup(x => x.InsertToQueue(It.IsAny<long>(), It.IsAny<long>(), -1, (int)TransactionTypes.Transfer).Result)
               .Returns(new QueueResultViewModel());

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
            dbWalletTransactionService.Setup(x => x.InsertToQueue(It.IsAny<long>(), It.IsAny<long>(), 100, (int)TransactionTypes.Transfer).Result)
                .Returns(new QueueResultViewModel() { Message = null });

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
            Assert.AreEqual(((ObjectResult)result).Value.GetType(), typeof(QueueResultViewModel));
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

        private QueueResultViewModel GenerateQueueResultViewModel_TooLowAmountException()
        {
            return new QueueResultViewModel()
            {
                Message = new TooLowAmountException().Message
            };
        }

        private QueueResultViewModel GenerateQueueResultViewModel_MaximumAllowableAmountException()
        {
            return new QueueResultViewModel()
            {
                Message = new MaximumAllowableAmountException().Message
            };
        }

  
        private QueueResultViewModel GenerateQueueResultViewModel_InsufficientWalletBalanceException()
        {
            return new QueueResultViewModel()
            {
                Message = new InsufficientWalletBalanceException().Message
            };
        }

      



    }
}
