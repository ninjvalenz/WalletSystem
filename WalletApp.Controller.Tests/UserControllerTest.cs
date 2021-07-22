

using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WalletApp.Api.Controllers;
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

		private UserController GetUserController()
		{
			var userController = new UserController
				(
					dbUserService.Object,
					dbWalletAcctService.Object
				);

			return userController;
		}

	}
}
