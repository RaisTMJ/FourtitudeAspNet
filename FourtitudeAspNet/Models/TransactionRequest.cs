using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FourtitudeAspNet.Models
{
    public class TransactionRequest
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

        [JsonPropertyName("items")]
        public List<ItemDetail>? Items { get; set; }

        [Required]
        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("sig")]
        public string Sig { get; set; } = string.Empty;
    }

    public class ItemDetail
    {
        [Required]
        [StringLength(50)]
        [JsonPropertyName("partneritemref")]
        public string PartnerItemRef { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("qty")]
        public int Qty { get; set; }

        [Required]
        [JsonPropertyName("unitprice")]
        public long UnitPrice { get; set; }
    }
//}