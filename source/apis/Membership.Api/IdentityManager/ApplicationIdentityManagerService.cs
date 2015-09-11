using Identity.Core.Entities;

namespace Membership.Api.IdentityManager
{
    public class ApplicationIdentityManagerService : App_Packages.IdentityManager.AspNetIdentity.AspNetIdentityManagerService<ApplicationUser, string, ApplicationRole, string>
    {
        public ApplicationIdentityManagerService(ApplicationUserManager userMgr, ApplicationRoleManager roleMgr)
            : base(userMgr, roleMgr)
        {
        }
    }
}