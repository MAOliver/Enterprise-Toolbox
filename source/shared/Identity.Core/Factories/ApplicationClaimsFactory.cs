using System.Security.Claims;
using Identity.Core.Entities;
using Microsoft.AspNet.Identity;

namespace Identity.Core.Factories
{
    /// <summary>
    /// The application claims factory, responsible for creating the <see cref="ClaimsIdentity"/> and setting the claims from the <see cref="ApplicationUser"/> object
    /// </summary>
    public class ApplicationClaimsFactory : ClaimsIdentityFactory<ApplicationUser, string>
    {
        public ApplicationClaimsFactory( )
        {
            UserIdClaimType = IdentityServer3.Core.Constants.ClaimTypes.Subject;
            UserNameClaimType = IdentityServer3.Core.Constants.ClaimTypes.PreferredUserName;
            RoleClaimType = IdentityServer3.Core.Constants.ClaimTypes.Role;
        }

        /// <summary>
        /// Create the <see cref="ClaimsIdentity"/> object
        /// </summary>
        /// <param name="manager">The <see cref="UserManager&lt;TUser,TKey&gt;"/> </param>
        /// <param name="user">The <see cref="ApplicationUser"/> to create the claimsidentity for</param>
        /// <param name="authenticationType">The authentication type</param>
        /// <returns></returns>
        public override async System.Threading.Tasks.Task<ClaimsIdentity> CreateAsync( UserManager<ApplicationUser, string> manager, ApplicationUser user, string authenticationType )
        {
            // can add additional claims from user object here if we need to
            return await base.CreateAsync(manager, user, authenticationType);
        }

    }
}