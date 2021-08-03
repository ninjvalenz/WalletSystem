

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
		private Mock<IConfiguration> config;

		[SetUp]
        public void Setup()
        {
            dbUserService = new Mock<IUserSecurityService>();
			dbWalletAcctService = new Mock<IUserWalletAccountService>();
			config = new Mock<IConfiguration>();
		}

		
	

		[Test]
		public void UserController_RegisterUser_Returns_BadRequest_RequiredFieldsException()
		{
			var controller = GetUserController();

			//Arrange
			dbUserService.Setup(x => x.InsertToQueue(It.IsAny<string>(), It.IsAny<string>()).Result)
				.Returns(new QueueResultViewModel());


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
			dbUserService.Setup(x => x.InsertToQueue(It.IsAny<string>(), It.IsAny<string>()).Result)
				.Returns(GenerateRegisterUserViewModelData());


			//Act
			var result = controller.Register(new NewRegisterUserViewModel() { Login = "test", Password = "password" }).Result;

			//Assert
			Assert.NotNull(result);
			Assert.IsInstanceOf<IActionResult>(result);
			Assert.AreEqual(((ObjectResult)result).Value.GetType(), typeof(QueueResultViewModel));

		}




		private UserController GetUserController()
		{
			var userController = new UserController
				(
					dbUserService.Object,
					dbWalletAcctService.Object,
					config.Object
				);

			return userController;
		}

		#region GenerateMockData
		private QueueResultViewModel GenerateRegisterUserViewModelData()
        {
			return new QueueResultViewModel()
			{
				InfoMessage = null,
				Message = null
			};

		}

		#endregion

	}
}
