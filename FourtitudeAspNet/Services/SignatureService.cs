using System.Security.Cryptography;
using System.Text;
using System.Globalization;
using FourtitudeAspNet.Interface;

namespace FourtitudeAspNet.Services
{

    public class SignatureService : ISignatureService
    {
        public bool ValidateSignature(string timestamp, string partnerKey, string partnerRefNo, long totalAmount, string partnerPassword, string providedSignature)
        {
            try
            {
                // Step 1: Format timestamp from ISO 8601 to yyyyMMddHHmmss
                // Step 2: Build concatenated string in exact order
                // Step 3: Apply SHA-256 hashing/
                // Step 4: Format hash as lowercase hexadecimal
                // Step 5: Encode the lowercase hex hash to Base64
                // Step 4: Format hash as lowercase hexadecimal

                var parsedTimestamp = DateTime.Parse(timestamp, null, DateTimeStyles.RoundtripKind);
                var formattedTimestamp = parsedTimestamp.ToString("yyyyMMddHHmmss");
                var concatenatedString = $"{formattedTimestamp}{partnerKey}{partnerRefNo}{totalAmount}{partnerPassword}";
                using var sha256 = SHA256.Create();
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(concatenatedString));
                var hexHash = Convert.ToHexString(hashBytes).ToLowerInvariant();
                var calculatedSignature = Convert.ToBase64String(Encoding.UTF8.GetBytes(hexHash));

                return calculatedSignature == providedSignature;
            }
            catch
            {
                return false;
            }
        }
    }
}