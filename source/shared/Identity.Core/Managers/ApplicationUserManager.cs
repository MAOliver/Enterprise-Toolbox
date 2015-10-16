using Identity.Core.Entities;
using Identity.Core.Factories;
using Identity.Core.Stores;
using Microsoft.AspNet.Identity;

namespace Identity.Core.Managers
{
    /// <summary>
    /// The application user manager, controls logic operations to the <see cref="ApplicationUserStore"/>
    /// </summary>
    public class ApplicationUserManager : UserManager<ApplicationUser, string>
    {
        /// <summary>
        /// .ctor for ApplicationUserManager
        /// </summary>
        /// <param name="store"><see cref="ApplicationUserStore"/>, the user repository object</param>
        public ApplicationUserManager( ApplicationUserStore store )
            : base( store )
        {
            ClaimsIdentityFactory = new ApplicationClaimsFactory( );
        }
    }
}