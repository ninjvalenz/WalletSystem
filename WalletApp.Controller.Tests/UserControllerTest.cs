

using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using WalletApp.Api.Controllers;
using WalletApp.Model.ViewModel;
using WalletApp.Model.ViewModel.Exceptions;
using WalletApp.Service.Interface;

namespace WalletApp.Controller.Tests
{
    [TestFixture]
    public class UserControllerTest
    {
        private Mock<IUserSecurityService> dbUserService;
		private Mock<IUserWalletAccountService> dbWalletAcctService;

		[SetUp]
        public void Setup()
        {
            dbUserService = new Mock<IUserSecurityService>();
			dbWalletAcctService = new Mock<IUserWalletAccountService>();

		}

		[Test]
		public void UserController__RegisterUser_Returns_BadRequest_UnableToRegisterUserException()
        {
			var controller = GetUserController();

			//Arrange
			dbUserService.Setup(x => x.RegisterUser(It.IsAny<string>(), It.IsAny<string>()).Result)
				.Returns(GenerateRegisterUserViewModelData_UnableToRegisterUserException());


			//Act
			var result = controller.Register(new NewRegisterUserViewModel() { Login = "test", Password = "password" }).Result;

			//Assert
			Assert.NotNull(result);
			Assert.IsInstanceOf<IActionResult>(result);
			Assert.AreEqual(((ObjectResult)result).Value, new UnableToRegisterUserException().Message);
			
		}

		[Test]
		public void UserController_RegisterUser_Returns_BadRequest_UnableToRegisterWalletException()
		{
			var controller = GetUserController();

			//Arrange
			dbUserService.Setup(x => x.RegisterUser(It.IsAny<string>(), It.IsAny<string>()).Result)
				.Returns(GenerateRegisterUserViewModelData());

			dbWalletAcctService.Setup(x => x.RegisterWallet(It.IsAny<Guid>()).Result)
				.Returns(GenerateRegisterWalletViewModelData_UnableToRegisterWalletException());


			//Act
			var result = controller.Register(new NewRegisterUserViewModel() { Login = "test", Password = "password" }).Result;

			//Assert
			Assert.NotNull(result);
			Assert.IsInstanceOf<IActionResult>(result);
			Assert.AreEqual(((ObjectResult)result).Value, new UnableToRegisterWalletException().Message);

		}

		[Test]
		public void UserController_RegisterUser_Returns_BadRequest_RequiredFieldsException()
		{
			var controller = GetUserController();

			//Arrange
			dbUserService.Setup(x => x.RegisterUser(It.IsAny<string>(), It.IsAny<string>()).Result)
				.Returns(new RegisterUserViewModel());


			//Act
			var result = controller.Register(new NewRegisterUserViewModel() {}).Result;

			//Assert
			Assert.NotNull(result);
			Assert.IsInstanceOf<IActionResult>(result);
			Assert.AreEqual(((ObjectResult)result).Value, new RequiredFieldsException().Message);

		}

		[Test]
		public void UserController_RegisterUser_Returns_Ok()
		{
			var controller = GetUserController();

			//Arrange
			dbUserService.Setup(x => x.RegisterUser(It.IsAny<string>(), It.IsAny<string>()).Result)
				.Returns(GenerateRegisterUserViewModelData());

			dbWalletAcctService.Setup(x => x.RegisterWallet(It.IsAny<Guid>()).Result)
				.Returns(GenerateRegisterWalletViewModelData());


			//Act
			var result = controller.Register(new NewRegisterUserViewModel() { Login = "test", Password = "password" }).Result;

			//Assert
			Assert.NotNull(result);
			Assert.IsInstanceOf<IActionResult>(result);
			Assert.AreEqual(((ObjectResult)result).Value.GetType(), typeof(RegisterWalletViewModel));

		}




		private UserController GetUserController()
		{
			var userController = new UserController
				(
					dbUserService.Object,
					dbWalletAcctService.Object
				);

			return userController;
		}

		#region GenerateMockData
		private RegisterUserViewModel GenerateRegisterUserViewModelData()
        {
			return new RegisterUserViewModel()
			{
				InfoMessage = null,
				Message = null,
				UserSecurityID = Guid.NewGuid()
			};

		}

		private RegisterUserViewModel GenerateRegisterUserViewModelData_UnableToRegisterUserException()
		{
			return new RegisterUserViewModel()
			{
				InfoMessage = null,
				Message = new UnableToRegisterUserException().Message,
				UserSecurityID = Guid.Empty
			};
		}

		private RegisterWalletViewModel GenerateRegisterWalletViewModelData_UnableToRegisterWalletException()
		{
			return new RegisterWalletViewModel()
			{
				InfoMessage = null,
				Message = new UnableToRegisterWalletException().Message
			};
		}

		private RegisterWalletViewModel GenerateRegisterWalletViewModelData()
		{
			return new RegisterWalletViewModel()
			{
				InfoMessage = null,
				Message = null,
				AccountNumber = 111111111111
			};

		}


		#endregion

	}
}
