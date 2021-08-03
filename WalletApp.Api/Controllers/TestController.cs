using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WalletApp.Model.Enums;
using WalletApp.Model.ViewModel;
using WalletApp.Model.ViewModel.Exceptions;
using WalletApp.Model.ViewModel.RequestBodyModel;
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
        private IWalletTransactionService walletTransactionService;


        public TestController(IUserSecurityService _userSecurityService,
            IUserWalletAccountService _userWalletAccountService,
            IConfiguration _config,
            IWalletTransactionService _walletTransactionService)
        {
            userSecurityService = _userSecurityService;
            userWalletAccountService = _userWalletAccountService;
            config = _config;
            walletTransactionService = _walletTransactionService;
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

        [HttpPost("testtransfer")]
        public async Task<IActionResult> TestTransfer([FromBody] NewTransferViewModel model)
        {
            QueueResultViewModel queueResult1;
            QueueResultViewModel queueResult2;
            QueueResultViewModel queueResult3;

            Parallel.Invoke
             (
                 () => queueResult1 = Transfer(model.AccountNumber, model.ToAccountNumber, model.Amount),
                 () => queueResult2 = Transfer(model.AccountNumber, model.ToAccountNumber, model.Amount),
                 () => queueResult3 = Transfer(model.AccountNumber, model.ToAccountNumber, model.Amount)
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

        private QueueResultViewModel Transfer(long? accountNo, long? fromAccountNo, decimal? amount)
        {
            try
            {
                if (!accountNo.HasValue || !fromAccountNo.HasValue || !amount.HasValue)
                    throw new RequiredFieldsException();

                var result = walletTransactionService.InsertToQueue(
                    accountNo.Value,
                    fromAccountNo.Value,
                    amount.Value,
                    (int)TransactionTypes.Transfer).Result;

                if (result != null)
                {
                    if (result.IsSuccess)
                        return result;


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
