using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using IdentityServer3.Core;
using IdentityServer3.Core.Models;

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
            var clients = GetInfrastructure()
                .Union(GetDemoClients());

            foreach (var client in clients)
            {
                context.Clients.AddOrUpdate(cl => cl.ClientId, client.ToEntity());
            }
        }

        private static Client[] GetInfrastructure()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "idmgr_client"
                    , ClientName = "IdentityManager"
                    , Enabled = true
                    , Flow = Flows.Implicit
                    , RequireConsent = false
                    , RedirectUris = new List<string> { "http://localhost:8080/idm", "http://localhost:44334/idm", "https://localhost:44333/idm", "https://jdcsrsapp3a-t/auth/idm" }
                    , PostLogoutRedirectUris = new List<string> { "http://localhost:8080/idm", "http://localhost:44334/idm", "https://localhost:44333/idm", "https://jdcsrsapp3a-t/auth/idm" }
                    , IdentityProviderRestrictions = new List<string> { Constants.PrimaryAuthenticationType, "windows" }
                    , AllowedScopes = new List<string>
                    {
                        Constants.StandardScopes.OpenId
                        , "idmgr"
                    },
                },
                new Client
                {
                    ClientId = "idAdmin"
                    , ClientName = "IdentityAdmin"
                    , Enabled = true
                    , Flow = Flows.Implicit
                    , RequireConsent = false
                    , RedirectUris = new List<string> { "http://localhost:8080/idsadmin", "http://localhost:44334/idsadmin", "https://localhost:44333/idsadmin", "https://jdcsrsapp3a-t/auth/idsadmin" }
                    , PostLogoutRedirectUris = new List<string> { "http://localhost:8080/idsadmin", "http://localhost:44334/idsadmin", "https://localhost:44333/idsadmin", "https://jdcsrsapp3a-t/auth/idsadmin" }
                    , IdentityProviderRestrictions = new List<string> { Constants.PrimaryAuthenticationType, "windows" }
                    , AllowedScopes = new List<string>
                    {
                        Constants.StandardScopes.OpenId
                        , "idAdmin"
                    },
                }
            };
        }
        

        private static Client[] GetDemoClients()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "test_postman_codeclient"
                    ,ClientName = "Authorization Code Test Client"
                    ,Enabled = true
                    , ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    }

                    ,Flow = Flows.AuthorizationCode

                    ,AllowedScopes = new List<string>
                    {
                       
                    }
                    , AccessTokenType = AccessTokenType.Reference
                    , RequireConsent = true
                    , AllowRememberConsent = true
                    , RedirectUris = new List<string> { "https://www.getpostman.com/oauth2/callback" }
                    , IdentityProviderRestrictions = new List<string> { Constants.PrimaryAuthenticationType }
                },
                new Client
                {
                    ClientName = "Implicit Client Demo",
                    Enabled = true,

                    ClientId = "urn:api:swagger",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    Flow = Flows.Implicit,

                    AllowedScopes = new List<string>
                    {
                        Constants.StandardScopes.OpenId,
                        Constants.StandardScopes.Profile,
                        Constants.StandardScopes.Email,
                        Constants.StandardScopes.Roles,
                       
                    },

                    ClientUri = "http://www.example.com",
                    LogoUri = "",

                    RequireConsent = true,
                    AllowRememberConsent = true,

                    RedirectUris = new List<string>
                    {
                        "http://localhost/sampleapi/swagger/ui/o2c-html"
                    },

                    PostLogoutRedirectUris = new List<string>
                    {
                        "http://localhost/sampleapi/swagger/ui/o2c-html"
                    },

                    AllowedCorsOrigins = new List<string>
                    {
                        "http://localhost/sampleapi/swagger/ui/o2c-html"
                    },

                    IdentityTokenLifetime = 360,
                    AccessTokenLifetime = 3600
                }
            };
        }
    }
}
