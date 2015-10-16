using System.Data.Entity;
using Identity.Core.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Identity.Core.Stores
{
    /// <summary>
    /// The ApplicationRoleStore, handles persistance of role objects
    /// </summary>
    public class ApplicationRoleStore : RoleStore<ApplicationRole>
    {
        /// <summary>
        /// .ctor for ApplicationRoleStore
        /// 
        /// NOTE: Changing input to DbContext causes this to fail to resolve when you deploy it.
        /// </summary>
        /// <param name="ctx"><see cref="MembershipContext"/>, the database context for Membership</param>
        public ApplicationRoleStore( MembershipContext ctx )
            : base( ctx )
        {
        }
    }
}