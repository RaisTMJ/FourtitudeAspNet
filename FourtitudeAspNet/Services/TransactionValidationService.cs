using FourtitudeAspNet.Interface;
using FourtitudeAspNet.Models;

namespace FourtitudeAspNet.Services
{

    public class TransactionValidationService : ITransactionValidationService
    {
        private readonly IPartnerService _partnerService;
        private readonly ISignatureService _signatureService;

        public TransactionValidationService(IPartnerService partnerService, ISignatureService signatureService)
        {
            _partnerService = partnerService;
            _signatureService = signatureService;
        }

        public TransactionResponse ValidateTransaction(TransactionRequest request)
        {
            // Validate partner authentication
            if (!_partnerService.ValidatePartner(request.PartnerKey, request.PartnerPassword))
            {
                return new TransactionResponse
                {
                    Result = 0,
                    ResultMessage = "Access Denied!"
                };
            }

            // Validate signature
            if (!_signatureService.ValidateSignature(request.Timestamp, request.PartnerKey, request.PartnerRefNo, request.TotalAmount, request.PartnerPassword, request.Sig))
            {
                return new TransactionResponse
                {
                    Result = 0,
                    ResultMessage = "Access Denied!"
                };
            }

            // Validate business logic
            var validationResult = ValidateBusinessRules(request);
            if (!validationResult.IsValid)
            {
                return new TransactionResponse
                {
                    Result = 0,
                    ResultMessage = validationResult.ErrorMessage
                };
            }

            // If all validations pass, return success
            return new TransactionResponse
            {
                Result = 1,
                TotalAmount = request.TotalAmount,
                TotalDiscount = 0,
                FinalAmount = request.TotalAmount
            };
        }

        private (bool IsValid, string ErrorMessage) ValidateBusinessRules(TransactionRequest request)
        {
            // Validate total amount is positive
            if (request.TotalAmount <= 0)
            {
                return (false, "Total amount must be a positive value");
            }

            // Validate items if provided
            if (request.Items != null)
            {
                foreach (var item in request.Items)
                {
                    // Validate required fields are not null or empty
                    if (string.IsNullOrEmpty(item.PartnerItemRef))
                    {
                        return (false, "Partner item reference cannot be null or empty");
                    }

                    if (string.IsNullOrEmpty(item.Name))
                    {
                        return (false, "Item name cannot be null or empty");
                    }

                    // Validate quantity does not exceed 5
                    if (item.Qty > 5)
                    {
                        return (false, "Quantity must not exceed 5");
                    }

                    // Validate unit price is positive
                    if (item.UnitPrice <= 0)
                    {
                        return (false, "Unit price must be a positive value");
                    }
                }
            }

            return (true, string.Empty);
        }
    }
}