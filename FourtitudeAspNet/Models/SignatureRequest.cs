using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FourtitudeAspNet.Models
{
    public class SignatureRequest
    {
        [Required]
        [StringLength(50)]
        [JsonPropertyName("partnerkey")]
        public string PartnerKey { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [JsonPropertyName("partnerrefno")]
        public string PartnerRefNo { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [JsonPropertyName("partnerpassword")]
        public string PartnerPassword { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("totalamount")]
        public long TotalAmount { get; set; }

        [JsonPropertyName("timestamp")]
        public string? Timestamp { get; set; }
    }
}