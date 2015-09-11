using Microsoft.AspNet.Identity.EntityFramework;

namespace Identity.Core.Entities
{
    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public ApplicationUserStore( IdentityContext ctx )
            : base( ctx )
        {
        }
    }
}