using System.Data;
using System.IdentityModel.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using Identity.Core.Entities;
using Identity.Core.Managers;
using Identity.Core.Stores;
using Microsoft.AspNet.Identity;

namespace Membership.Api.Migrations.MembershipConfiguration
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Identity.Core.MembershipContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\MembershipConfiguration";
        }

        protected override void Seed(Identity.Core.MembershipContext context)
        {

            SeedAsync(context).Wait(10000);

        }

        private async Task SeedAsync(Identity.Core.MembershipContext context)
        {
            

            //do not seed if the database contains users
            if (context.Users.Any())
                return;
            
            await AddUserAsync(context, "admin@example.com", "1234567890", "P@ssw0rd", "IdentityManagerAdministrator", "IdentityAdminAdministrator");
            
        }

        private async Task AddUserAsync(Identity.Core.MembershipContext context, string email, string phoneNumber, string password,
            params string[] roleNames/*, params Tuple<string, string>[] claimTuples*/)
        {
            var aum = new ApplicationUserManager(new ApplicationUserStore(context));
            var arm = new ApplicationRoleManager(new ApplicationRoleStore(context));
            
            var user = new ApplicationUser
            {
                Email = email
                , PhoneNumber = phoneNumber
                , UserName = email
            };
            
            var identityResult = await aum.CreateAsync(user, password);
            if (!identityResult.Succeeded)
            {
                throw new DataException($"Failed to add identity: {user.UserName}");

            }

            user = await aum.FindByEmailAsync(user.Email);

            await AddUserToRolesAsync(aum, arm, user.Id, roleNames);
          
            aum.AddClaim(user.Id, new Claim("email", user.Email));
            aum.AddClaim(user.Id, new Claim("name", user.UserName));
        }

        private async Task AddUserToRolesAsync(ApplicationUserManager aum, ApplicationRoleManager arm, string userId,params string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                var applicationRole = await arm.FindByNameAsync(roleName);

                if (applicationRole == null)
                {
                    await CreateRole(arm, roleName);
                }

                if (!aum.AddToRole(userId, roleName).Succeeded)
                {
                    throw new DataException($"Failed to add {userId} to role {roleName}");
                }
                aum.AddClaim(userId, new Claim("role", roleName));
            }
        }

        private async Task CreateRole(ApplicationRoleManager arm, string roleName)
        {
            var applicationRole = new ApplicationRole
            {
                Name = roleName
            };
            var roleResult = await arm.CreateAsync(applicationRole);
            if (!roleResult.Succeeded)
            {
                throw new DataException($"Failed to add role: {applicationRole.Name}");
            }
        }
    }
}
