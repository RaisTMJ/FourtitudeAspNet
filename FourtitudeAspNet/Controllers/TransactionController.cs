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

        public TransactionController(ITransactionValidationService transactionValidationService)
        {
            _transactionValidationService = transactionValidationService;
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
            catch (Exception ex)
            {
                return Ok(new TransactionResponse
                {
                    Result = 0,
                    ResultMessage = "Internal server error"
                });
            }
        }
    }
}