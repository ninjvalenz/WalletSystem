using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WalletApp.Model.ViewModel;
using WalletApp.Model.ViewModel.Exceptions;
using WalletApp.Service.Interface;

namespace WalletApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private IUserSecurityService userSecurityService;
        private IUserWalletAccountService userWalletAccountService;
        private readonly IConfiguration config;


        public TestController(IUserSecurityService _userSecurityService,
            IUserWalletAccountService _userWalletAccountService,
            IConfiguration _config)
        {
            userSecurityService = _userSecurityService;
            userWalletAccountService = _userWalletAccountService;
            config = _config;
        }

        [HttpPost("testregister")]
        public async Task<IActionResult> TestRegister([FromBody] NewRegisterUserViewModel model)
        {
            QueueResultViewModel queueResult1;
            QueueResultViewModel queueResult2;
            QueueResultViewModel queueResult3;

            Parallel.Invoke
             (
                 () => queueResult1 = RegisterUser(model.Login, model.Password),
                 () => queueResult2 = RegisterUser(model.Login, model.Password),
                 () => queueResult3 = RegisterUser(model.Login, model.Password)
             );
            return Ok();
        }

        private QueueResultViewModel RegisterUser(string login, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                    throw new RequiredFieldsException();

            

                var userResult = userSecurityService.InsertToQueue(login, password).Result;
                if (userResult != null)
                {
                    if (userResult.IsSuccess)
                        return userResult;
                    else throw new Exception(userResult.Message);
                }

                //if everything fails...
                throw new ServerProblemException();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


    }
}
