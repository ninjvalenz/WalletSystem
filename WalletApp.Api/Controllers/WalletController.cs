using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletApp.Model.ViewModel.Exceptions;
using WalletApp.Model.ViewModel.RequestBodyModel;
using WalletApp.Service.Interface;

namespace WalletApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private IWalletTransactionService walletTransactionService;

        public WalletController(IWalletTransactionService _walletTransactionService)
        {
            walletTransactionService = _walletTransactionService;
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] TransactionViewModel model)
        {
            try
            {
                if (model == null || !model.AccountNumber.HasValue || !model.Amount.HasValue)
                    throw new RequiredFieldsException();

                var result = await walletTransactionService.DepositMoney(model.AccountNumber.Value, model.Amount.Value);
                if (result != null)
                {
                    if (result.IsSuccess)
                        return Ok(result);

                    throw new Exception(result.Message);
                }

                throw new ServerProblemException();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [AllowAnonymous]
        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] TransactionViewModel model)
        {
            try
            {
                if (model == null || !model.AccountNumber.HasValue || !model.Amount.HasValue)
                    throw new RequiredFieldsException();

                var result = await walletTransactionService.WithdrawMoney(model.AccountNumber.Value, model.Amount.Value);
                if (result != null)
                {
                    if (result.IsSuccess)
                        return Ok(result);

                    throw new Exception(result.Message);
                }

                throw new ServerProblemException();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] NewTransferViewModel model)
        {
            try
            {
                if (model == null || 
                    !model.AccountNumber.HasValue || 
                    !model.ToAccountNumber.HasValue || 
                    !model.Amount.HasValue)
                    throw new RequiredFieldsException();

                var result = await walletTransactionService.TransferMoney(model.AccountNumber.Value, model.ToAccountNumber.Value, model.Amount.Value);
                if (result != null)
                {
                    if (result.IsSuccess)
                        return Ok(result);

                    throw new Exception(result.Message);
                }

                throw new ServerProblemException();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
