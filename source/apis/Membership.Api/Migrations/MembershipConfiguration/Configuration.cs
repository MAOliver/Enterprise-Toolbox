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
            var um = new ApplicationUserManager(new ApplicationUserStore(context));
            var urm = new ApplicationRoleManager(new ApplicationRoleStore(context));

            //do not seed if the database contains users
            if (context.Users.Any())
                return;

            var role = new ApplicationRole
            {
                Name = "IdentityManagerAdministrator"
            };
            var user = new ApplicationUser
            {
                Email = "admin@example.com"
                , PhoneNumber = "1234567890"
            //    , PasswordHash = new PasswordHasher().HashPassword("password")
                , UserName = "admin@example.com"
            };
            var roleResult = await urm.CreateAsync(role);
            if (!roleResult.Succeeded)
            {
                throw new DataException("Failed to add admin role");
            }
            role = await urm.FindByNameAsync("IdentityManagerAdministrator");

            var identityResult = await um.CreateAsync(user, "password");
            if (!identityResult.Succeeded)
            {
                throw new DataException("Failed to add admin identity");

            }

            user = await um.FindByEmailAsync(user.Email);
            if (!um.AddToRole(user.Id, role.Name).Succeeded)
            {
                throw new DataException("Failed to add admin to admin role");
            }

            um.AddClaim(user.Id, new Claim("role", "IdentityManagerAdministrator"));
            um.AddClaim(user.Id, new Claim("email", user.Email));
            um.AddClaim(user.Id, new Claim("name", user.UserName));
        }
    }
}
