using Microsoft.AspNet.Identity;

namespace Identity.Core.Entities
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager( ApplicationRoleStore roleStore )
            : base( roleStore )
        {
        }
    }
}