using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.Model.ViewModel;
using WalletApp.Model.ViewModel.Exceptions;
using WalletApp.Model.ViewModel.RequestBodyModel;
using WalletApp.Service.Interface;

namespace WalletApp.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserSecurityService userSecurityService;
        private IUserWalletAccountService userWalletAccountService;
        private readonly IConfiguration config;

        public UserController(IUserSecurityService _userSecurityService, 
            IUserWalletAccountService _userWalletAccountService,
            IConfiguration _config)
        {
            userSecurityService = _userSecurityService;
            userWalletAccountService = _userWalletAccountService;
            config = _config;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginViewModel model)
        {
            var resultViewModel = new PayloadViewModel();

            try
            {
                if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                    throw new RequiredFieldsException();

                var user = await userSecurityService.AuthenticateUser(model.Username, model.Password);
              
                if (user != null)
                {
                    resultViewModel.IsSuccess = user.IsSuccess;

                    if (!user.IsSuccess)
                    {
                        resultViewModel.Message = user.Message;

                        throw new Exception(resultViewModel.Message);
                    }
                    else
                    {

                        var tokenString = GenerateJSONWebToken();
                        resultViewModel.Message = $"You are now logged in using login name {user.Login}. ";

                        if (user.AccountNumber != null && user.AccountNumber.Count > 0)
                        {
                            var accounts = string.Join(",", user.AccountNumber);
                            if (!string.IsNullOrEmpty(accounts))
                                resultViewModel.Message += $"Wallet account(s): {accounts}";
                        }
                        resultViewModel.Token = tokenString;
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
            return Ok(resultViewModel);
        }
       
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

                //if everything fails...
                throw new ServerProblemException();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
         
        }

        [HttpGet]

        private string GenerateJSONWebToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue<string>("Jwt:Key")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(config.GetValue<string>("Jwt:Issuer"),
              config.GetValue<string>("Jwt:Issuer"),
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
