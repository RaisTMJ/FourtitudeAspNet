using FourtitudeAspNet.Interface;
using FourtitudeAspNet.Models;
using System.Text;

namespace FourtitudeAspNet.Services
{


    public class PartnerService : IPartnerService
    {
        private readonly List<Partner> _allowedPartners;

        public PartnerService()
        {
            // Initialize allowed partners as per requirements
            _allowedPartners = new List<Partner> { 
                new Partner { PartnerKey = "FG-00001", PartnerName = "FAKEGOOGLE", Password = "FAKEPASSWORD1234" }, 
                new Partner { PartnerKey = "FG-00002", PartnerName = "FAKEPEOPLE", Password = "FAKEPASSWORD4578" } 
            };
        }
        public bool ValidatePartner(string partnerKey, string partnerPassword)
        {
            var partner = GetPartner(partnerKey);
            if (partner == null)
                return false;

            try
            {
                var decodedPassword = Encoding.UTF8.GetString(Convert.FromBase64String(partnerPassword));
                return decodedPassword == partner.Password;
            }
            catch
            {
                return false;
            }
        }

        public Partner? GetPartner(string partnerKey)
        {
            return _allowedPartners.FirstOrDefault(p => p.PartnerKey == partnerKey);
        }
    }
}