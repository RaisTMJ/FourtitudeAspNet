using System.ComponentModel.DataAnnotations;

namespace FourtitudeAspNet.Models
{
    public class Partner
    {
        [Key]
        public string PartnerKey { get; set; } = string.Empty;
        public string PartnerName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}