using Microsoft.AspNet.Identity;

namespace Identity.Core.Entities
{
    public class ApplicationUserManager : UserManager<ApplicationUser, string>
    {
        public ApplicationUserManager( ApplicationUserStore store )
            : base( store )
        {
            ClaimsIdentityFactory = new ApplicationClaimsFactory( );
        }
    }
}