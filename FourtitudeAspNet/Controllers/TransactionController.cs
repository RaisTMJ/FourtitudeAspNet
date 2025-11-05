using Microsoft.AspNetCore.Mvc;
using FourtitudeAspNet.Models;
using FourtitudeAspNet.Services;
using FourtitudeAspNet.Interface;

namespace FourtitudeAspNet.Controllers
{
    [ApiController]
    [Route("api")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionValidationService _transactionValidationService;
        private readonly ISignatureService _signatureService;

        public TransactionController(ITransactionValidationService transactionValidationService, ISignatureService signatureService)
        {
            _transactionValidationService = transactionValidationService;
            _signatureService = signatureService;
        }

        [HttpPost("submittrxmessage")]
        public IActionResult SubmitTransaction([FromBody] TransactionRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new TransactionResponse
                    {
                        Result = 0,
                        ResultMessage = "Invalid request format"
                    });
                }

                var response = _transactionValidationService.ValidateTransaction(request);

                return Ok(response);
            }
            catch (Exception)
            {
                return Ok(new TransactionResponse
                {
                    Result = 0,
                    ResultMessage = "Internal server error"
                });
            }
        }

        [HttpPost("GetSig")]
        public IActionResult GetSig([FromBody] SignatureRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid request format");
                }

                var signature = _signatureService.GenerateSignature(request.Timestamp, request.PartnerKey, request.PartnerRefNo, request.TotalAmount, request.PartnerPassword);

                return Ok(new { Signature = signature });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}