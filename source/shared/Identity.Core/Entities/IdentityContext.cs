using Microsoft.AspNet.Identity.EntityFramework;

namespace Identity.Core.Entities
{
    public class IdentityContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public IdentityContext() : this("IdSvr3Config")
        {
        }

        public IdentityContext( string connString )
            : base( connString )
        {
        }
    }
}