using FourtitudeAspNet.Interface;
using FourtitudeAspNet.Models;
using System.Globalization;

namespace FourtitudeAspNet.Services
{

    public class TransactionValidationService : ITransactionValidationService
    {
        private readonly IPartnerService _partnerService;
        private readonly ISignatureService _signatureService;
        private readonly IDiscountService _discountService;

        public TransactionValidationService(IPartnerService partnerService, ISignatureService signatureService, IDiscountService discountService)
        {
            _partnerService = partnerService;
            _signatureService = signatureService;
            _discountService = discountService;
        }

        public TransactionResponse ValidateTransaction(TransactionRequest request)
        {
            if (!_partnerService.ValidatePartner(request.PartnerKey, request.PartnerPassword))
            {
                return new TransactionResponse
                {
                    Result = 0,
                    ResultMessage = "Access Denied!"
                };
            }

            if (!_signatureService.ValidateSignature(request.Timestamp, request.PartnerKey, request.PartnerRefNo, request.TotalAmount, request.PartnerPassword, request.Sig))
            {
                return new TransactionResponse
                {
                    Result = 0,
                    ResultMessage = "Access Denied!"
                };
            }

            var validationResult = ValidateBusinessRules(request);
            if (!validationResult.IsValid)
            {
                return new TransactionResponse
                {
                    Result = 0,
                    ResultMessage = validationResult.ErrorMessage
                };
            }

            var discountResult = _discountService.CalculateDiscount(request.TotalAmount);

            return new TransactionResponse
            {
                Result = 1,
                TotalAmount = request.TotalAmount,
                TotalDiscount = discountResult.TotalDiscount,
                FinalAmount = discountResult.FinalAmount
            };
        }

        private (bool IsValid, string ErrorMessage) ValidateBusinessRules(TransactionRequest request)
        {
            // Validate required fields
            if (string.IsNullOrEmpty(request.PartnerKey))
                return (false, "PartnerKey is Required.");
            if (string.IsNullOrEmpty(request.PartnerPassword))
                return (false, "PartnerPassword is Required.");
            if (string.IsNullOrEmpty(request.PartnerRefNo))
                return (false, "PartnerRefNo is Required.");
            if (request.TotalAmount == 0)
                return (false, "TotalAmount is Required.");
            if (string.IsNullOrEmpty(request.Timestamp))
                return (false, "Timestamp is Required.");
            if (string.IsNullOrEmpty(request.Sig))
                return (false, "Sig is Required.");

            // Validate timestamp is within +- 5 minutes of server time
            if (DateTime.TryParse(request.Timestamp, out DateTime timestamp))
            {
                DateTime timestampUtc = DateTime.Parse(request.Timestamp, null, DateTimeStyles.AdjustToUniversal);
                if (timestampUtc < DateTime.Now.AddMinutes(-5) || timestampUtc > DateTime.Now.AddMinutes(5))
                {
                    return (false, "Expired.");
                }
            }
            else
            {
                return (false, "Invalid Timestamp format.");
            }

            // Validate total amount is positive
            if (request.TotalAmount <= 0)
            {
                return (false, "Total amount must be a positive value");
            }

            // Validate items if provided
            if (request.Items != null)
            {
                decimal calculatedTotalAmount = 0;
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
                    calculatedTotalAmount += item.Qty * item.UnitPrice;
                }

                if (calculatedTotalAmount != request.TotalAmount)
                {
                    return (false, "Invalid Total Amount.");
                }
            }

            return (true, string.Empty);
        }
    }
}