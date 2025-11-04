using FourtitudeAspNet.Models;

namespace FourtitudeAspNet.Interface
{
    public interface IPartnerService
    {
        bool ValidatePartner(string partnerKey, string partnerPassword); 
        Partner? GetPartner(string partnerKey);
    }
}
