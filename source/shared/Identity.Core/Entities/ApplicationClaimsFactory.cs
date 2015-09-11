using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace Identity.Core.Entities
{
    public class ApplicationClaimsFactory : ClaimsIdentityFactory<ApplicationUser, string>
    {
        public ApplicationClaimsFactory( )
        {
            UserIdClaimType = IdentityServer3.Core.Constants.ClaimTypes.Subject;
            UserNameClaimType = IdentityServer3.Core.Constants.ClaimTypes.PreferredUserName;
            RoleClaimType = IdentityServer3.Core.Constants.ClaimTypes.Role;
        }

        public override async System.Threading.Tasks.Task<ClaimsIdentity> CreateAsync( UserManager<ApplicationUser, string> manager, ApplicationUser user, string authenticationType )
        {
            var ci = await base.CreateAsync( manager, user, authenticationType );
            /*if ( !String.IsNullOrWhiteSpace( user.FirstName ) )
            {
                ci.AddClaim( new Claim( IdentityServer3.Core.Constants.ClaimTypes.GivenName, user.FirstName ) );
            }
            if ( !String.IsNullOrWhiteSpace( user.LastName ) )
            {
                ci.AddClaim( new Claim( IdentityServer3.Core.Constants.ClaimTypes.FamilyName, user.LastName ) );
            }*/
            return ci;
        }
    }
}