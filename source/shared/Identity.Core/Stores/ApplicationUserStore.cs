using System.Data.Entity;
using Identity.Core.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Identity.Core.Stores
{
    /// <summary>
    /// The user persist layer
    /// </summary>
    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        /// <summary>
        /// .ctor for ApplicationUserStore
        ///
        /// NOTE: Changing input to DbContext causes this to fail to resolve when you deploy it.
        /// </summary>
        /// <param name="ctx"><see cref="MembershipContext"/>, the DBContext for membership</param>
        public ApplicationUserStore( MembershipContext ctx )
            : base( ctx )
        {
        }
    }
}