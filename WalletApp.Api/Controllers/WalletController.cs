using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletApp.Model.Enums;
using WalletApp.Model.ViewModel.Exceptions;
using WalletApp.Model.ViewModel.RequestBodyModel;
using WalletApp.Service.Interface;

namespace WalletApp.Api.Controllers
{
    [Authorize]
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

                var result = await walletTransactionService.InsertToQueue(
                    model.AccountNumber.Value,
                    null,
                    model.Amount.Value,
                    (int)TransactionTypes.Deposit);
                
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

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] TransactionViewModel model)
        {
            try
            {
                if (model == null || !model.AccountNumber.HasValue || !model.Amount.HasValue)
                    throw new RequiredFieldsException();

                //var result = await walletTransactionService.WithdrawMoney(model.AccountNumber.Value, model.Amount.Value);
                var result = await walletTransactionService.InsertToQueue(
                  model.AccountNumber.Value,
                  null,
                  model.Amount.Value,
                  (int)TransactionTypes.Withdraw);

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

                //var result = await walletTransactionService.TransferMoney(model.AccountNumber.Value, model.ToAccountNumber.Value, model.Amount.Value);
                var result = await walletTransactionService.InsertToQueue(
                  model.AccountNumber.Value,
                  model.ToAccountNumber.Value,
                  model.Amount.Value,
                  (int)TransactionTypes.Transfer);

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

        [HttpPost("historyall")]
        public async Task<IActionResult> HistoryAll([FromBody] HistoryAllViewModel model)
        {
            try
            {
                if (model == null ||
                    !model.AccountNumber.HasValue ||
                    !model.Offset.HasValue)
                    throw new RequiredFieldsException();

                var result = await walletTransactionService.ViewTransactionHistoryAll(model.AccountNumber.Value, model.Offset.Value);
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

        [HttpPost("historybyrange")] //Date format from the body should be like this: "2021-07-21"
        public async Task<IActionResult> HistoryByRange([FromBody] HistoryByRangeViewModel model)
        {
            try
            {
                if (model == null ||
                    !model.AccountNumber.HasValue ||
                    !model.FromDate.HasValue ||
                    !model.ToDate.HasValue)
                    throw new RequiredFieldsException();

                var result = await walletTransactionService.ViewTransactionHistoryByRange(
                    model.AccountNumber.Value, model.FromDate.Value, model.ToDate.Value);
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
