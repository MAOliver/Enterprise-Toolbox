using Identity.Core.Entities;
using Identity.Core.Managers;
using Membership.Api.App_Packages.IdentityManager.AspNetIdentity;

namespace Membership.Api.IdentityManager
{
    public class ApplicationIdentityManagerService : AspNetIdentityManagerService<ApplicationUser, string, ApplicationRole, string>
    {
        public ApplicationIdentityManagerService(ApplicationUserManager userMgr, ApplicationRoleManager roleMgr)
            : base(userMgr, roleMgr)
        {
        }
    }
}