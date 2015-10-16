using Identity.Core.Entities;
using Identity.Core.Stores;
using Microsoft.AspNet.Identity;

namespace Identity.Core.Managers
{
    /// <summary>
    /// User role manager, controls role operations to the persistance object
    /// </summary>
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        /// <summary>
        /// .ctor for ApplicationRoleManager
        /// </summary>
        /// <param name="roleStore"><see cref="ApplicationRoleStore"/>, repository object for user roles</param>
        public ApplicationRoleManager( ApplicationRoleStore roleStore )
            : base( roleStore )
        {
        }
    }
}