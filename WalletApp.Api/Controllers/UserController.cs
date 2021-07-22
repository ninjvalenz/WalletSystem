using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletApp.Model.ViewModel;
using WalletApp.Model.ViewModel.Exceptions;
using WalletApp.Model.ViewModel.RequestBodyModel;
using WalletApp.Service.Interface;

namespace WalletApp.Api.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserSecurityService userSecurityService;
        private IUserWalletAccountService userWalletAccountService;

        public UserController(IUserSecurityService _userSecurityService, 
            IUserWalletAccountService _userWalletAccountService)
        {
            userSecurityService = _userSecurityService;
            userWalletAccountService = _userWalletAccountService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginViewModel model)
        {
            var user = await userSecurityService.AuthenticateUser(model.Username, model.Password);
            var resultViewModel = new PayloadViewModel();

            if (user != null)
            {
                resultViewModel.IsSuccess = user.IsSuccess;

                if (!user.IsSuccess)
                {
                    resultViewModel.Message = user.Message;

                    return BadRequest(resultViewModel);
                }
                else
                {
                    resultViewModel.Message = $"You are now logged in using login name {user}";
                }
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] NewRegisterUserViewModel model)
        {
            try
            {
                if (model == null || string.IsNullOrEmpty(model.Login) || string.IsNullOrEmpty(model.Password))
                    throw new RequiredFieldsException();

                //Register user 
                var userResult = await userSecurityService.RegisterUser(model.Login, model.Password);
                if (userResult != null)
                {
                    if (userResult.IsSuccess)
                    {
                        //Register initial wallet account
                        var walletResult = await userWalletAccountService.RegisterWallet(userResult.UserSecurityID);
                        if(walletResult != null)
                        {
                            if (walletResult.IsSuccess)
                                return Ok(walletResult);

                            throw new Exception(walletResult.Message);
                        }
                    }

                    throw new Exception(userResult.Message);

                }

                //if everything fails...(kahit ginawa mo na lahat, di pa din ikaw ang mahal nya wahahah)
                throw new ServerProblemException();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
         
        }
    }
}
