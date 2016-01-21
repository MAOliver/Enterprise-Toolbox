using Identity.Core.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Identity.Core
{
    /// <summary>
    /// The membership database context object
    /// </summary>
    public class MembershipContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public MembershipContext() : this("IdSvr3Config")
        {
        }

        public MembershipContext( string connString )
            : base( connString )
        {
        }
    }
}