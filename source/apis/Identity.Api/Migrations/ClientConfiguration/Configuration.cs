using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using IdentityServer3.Core.Models;
using IdentityServer3.EntityFramework;

namespace Auth.Api.Migrations.ClientConfiguration
{
    internal sealed class Configuration : DbMigrationsConfiguration<IdentityServer3.EntityFramework.ClientConfigurationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\ClientConfiguration";
        }

        protected override void Seed(IdentityServer3.EntityFramework.ClientConfigurationDbContext context)
        {   
            var client = new IdentityServer3.Core.Models.Client
            {
                ClientId = "idmgr_client"
                , ClientName = "IdentityManager"
                , Enabled = true
                , Flow = Flows.Implicit
                , RequireConsent = false
                , RedirectUris = new List<string> { "https://localhost:8086/members"}
                , PostLogoutRedirectUris = new List<string> { "https://localhost:8086/members/idm" }
                , IdentityProviderRestrictions = new List<string> { IdentityServer3.Core.Constants.PrimaryAuthenticationType }
                , AllowedScopes = new List<string>
                {
                    IdentityServer3.Core.Constants.StandardScopes.OpenId
                    , "idmgr"
                },
            };
            context.Clients.AddOrUpdate(cl => cl.ClientId, client.ToEntity());

        }
    }
}
