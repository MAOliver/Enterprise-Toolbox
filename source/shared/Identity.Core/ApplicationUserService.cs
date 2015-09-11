using Identity.Core.Entities;
using IdentityServer3.AspNetIdentity;

namespace Identity.Core
{
    public class ApplicationUserService : AspNetIdentityUserService<ApplicationUser, string>
    {
        public ApplicationUserService(ApplicationUserManager userMgr)
            : base(userMgr)
        {
        }
    }
}