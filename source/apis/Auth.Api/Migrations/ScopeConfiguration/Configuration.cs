using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using IdentityServer3.EntityFramework;

namespace Auth.Api.Migrations.ScopeConfiguration
{
    internal sealed class Configuration : DbMigrationsConfiguration<IdentityServer3.EntityFramework.ScopeConfigurationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\ScopeConfiguration";
        }

        protected override void Seed(IdentityServer3.EntityFramework.ScopeConfigurationDbContext context)
        {
            //if scopes are already contained in database, do not re-seed
            if(context.Scopes.Any()) return;

            var scopes =
                StandardScopes.All
                    .Union(StandardScopes.AllAlwaysInclude)
                    .Union(
                        new[]
                        {
                            new IdentityServer3.Core.Models.Scope
                            {
                                Name = "idmgr",
                                DisplayName = "IdentityManager",
                                Description = "Authorization for IdentityManager",
                                Type = ScopeType.Identity,
                                Claims = new List<IdentityServer3.Core.Models.ScopeClaim>
                                {
                                    new IdentityServer3.Core.Models.ScopeClaim(Constants.ClaimTypes.Name),
                                    new IdentityServer3.Core.Models.ScopeClaim(Constants.ClaimTypes.Role)
                                }
                            }
                        }
                    );
            
                foreach (var scope in scopes)
                {
                    context.Scopes.AddOrUpdate(sc=>sc.Name, scope.ToEntity());
                }
        }
    }
}
