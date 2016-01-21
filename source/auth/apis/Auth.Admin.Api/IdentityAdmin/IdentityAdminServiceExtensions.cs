using IdentityAdmin.Configuration;
using IdentityAdmin.Core;

namespace Auth.Admin.Api.IdentityAdmin
{
    public static class IdentityAdminServiceExtensions
    {
        public static void Configure(this IdentityAdminServiceFactory factory)
        {
            factory.IdentityAdminService = new Registration<IIdentityAdminService, IdentityAdminManagerService>();
        }
    }
}