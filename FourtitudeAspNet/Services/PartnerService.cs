using FourtitudeAspNet.Interface;
using FourtitudeAspNet.Models;
using FourtitudeAspNet.Data;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace FourtitudeAspNet.Services
{
    public class PartnerService : IPartnerService
    {
      private readonly FourtitudeDbContext _context;

        public PartnerService(FourtitudeDbContext context)
  {
    _context = context;
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
return _context.Partners.FirstOrDefault(p => p.PartnerKey == partnerKey);
        }
    }
}