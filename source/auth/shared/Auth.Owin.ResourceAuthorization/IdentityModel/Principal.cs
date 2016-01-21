using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Owin.ResourceAuthorization.IdentityModel
{

    public static class Principal
    {
        public static ClaimsPrincipal Anonymous => new ClaimsPrincipal(Identity.Anonymous);

        public static ClaimsPrincipal Create(string authenticationType, params Claim[] claims)
        {
            return new ClaimsPrincipal(Identity.Create(authenticationType, claims));
        }

        public static IEnumerable<Claim> CreateRoles(string[] roleNames)
        {
            if (roleNames == null || !roleNames.Any())
            {
                return new Claim[] { };
            }

            return new List<Claim>(from r in roleNames select new Claim(ClaimTypes.Role, r)).ToArray();
        }

        public static ClaimsPrincipal CreateFromCertificate(X509Certificate2 certificate, string authenticationType = "X.509", bool includeAllClaims = false)
        {
            return new ClaimsPrincipal(Identity.CreateFromCertificate(certificate, authenticationType, includeAllClaims));
        }
    }
}
