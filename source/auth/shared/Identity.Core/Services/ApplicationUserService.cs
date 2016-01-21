using Identity.Core.Entities;
using Identity.Core.Managers;
using IdentityServer3.AspNetIdentity;

namespace Identity.Core.Services
{
    /// <summary>
    /// The user service, wraps logical operations to the usermanager object
    /// </summary>
    public class ApplicationUserService : AspNetIdentityUserService<ApplicationUser, string>
    {
        /// <summary>
        /// .ctor for ApplicationUserService
        /// </summary>
        /// <param name="userMgr"><see cref="ApplicationUserManager"/>, contains logic operations for users</param>
        public ApplicationUserService( ApplicationUserManager userMgr)
            : base(userMgr)
        {
        }
    }
}