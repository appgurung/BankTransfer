using BankTransfer.Core.Interface;
using BankTransfer.Core.Models.Request.CBA;
using BankTransfer.Core.Models.Response.CBA;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BankTransfer.API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/core-banking")]
    
    public class BankTransferController : ControllerBase
    {
        private readonly ICoreBanking _cba;

        public BankTransferController(ICoreBanking cba)
        {
            _cba = cba;
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<BanksResponse>))]
        [HttpGet("banks")]
        public async Task<IActionResult> Banks([FromQuery]string provider, CancellationToken token)
        {
            var banks = await _cba.FetchBanks(provider);
            return banks.Count() > 0 ? Ok(banks) : NotFound(banks);
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(AccountLookUpResponse))]

        [HttpPost("validateBankAccount")]
        public async Task<IActionResult> ValidateBankAccount([FromBody] AccountLookUpRequest param)
        {
            var resp = await _cba.AccountNameEnquiry(param);

            return !string.IsNullOrEmpty(resp.accountNumber) ? Ok(resp) : NotFound(resp);
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(TransferResponse))]

        [HttpPost("bankTransfer")]
        public async Task<IActionResult> BankTransfer([FromBody] TransferRequest param)
        {
            var resp = await _cba.TransferFunds(param);

            return resp.responseCode == "00" ? Ok(resp) : BadRequest(resp);
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(TransferResponse))]

        [HttpGet("transaction/{transactionReference}")]
        public async Task<IActionResult> TransactionQuery([FromRoute] string transactionReference)
        {
            return Ok();
        }
    }
}