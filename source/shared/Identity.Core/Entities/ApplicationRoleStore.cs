using Microsoft.AspNet.Identity.EntityFramework;

namespace Identity.Core.Entities
{
    public class ApplicationRoleStore : RoleStore<ApplicationRole>
    {
        public ApplicationRoleStore( IdentityContext ctx )
            : base( ctx )
        {
        }
    }
}