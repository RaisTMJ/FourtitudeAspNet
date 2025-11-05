namespace FourtitudeAspNet.Interface
{
    public interface ISignatureService
    {
        bool ValidateSignature(string timestamp, string partnerKey, string partnerRefNo, long totalAmount, string partnerPassword, string providedSignature);
        string GenerateSignature(string? timestamp, string partnerKey, string partnerRefNo, long totalAmount, string partnerPassword);
    }
}
