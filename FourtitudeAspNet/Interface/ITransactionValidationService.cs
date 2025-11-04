using FourtitudeAspNet.Models;

namespace FourtitudeAspNet.Interface
{
    public interface ITransactionValidationService
    { TransactionResponse ValidateTransaction(TransactionRequest request); 
    }
}
