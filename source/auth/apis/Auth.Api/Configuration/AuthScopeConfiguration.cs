using System.Collections.Generic;
using IdentityServer3.Core;
using IdentityServer3.Core.Models;

namespace Auth.Api.Configuration
{
    public static class AuthScopeConfiguration
    {
        public static class Identity
        {
            public static Scope[] GetScopes()
            {
                return new[]
                {
                    StandardScopes.OpenId,
                    StandardScopes.Profile,
                    StandardScopes.Email,
                    StandardScopes.Address,
                    StandardScopes.Phone,
                    StandardScopes.OfflineAccess,
                    StandardScopes.RolesAlwaysInclude,
                    StandardScopes.AllClaims,
                    new Scope
                    {
                        Name = "idmgr",
                        DisplayName = "IdentityManager",
                        Description = "Authorization for IdentityManager",
                        Type = ScopeType.Identity,
                        Claims = new List<ScopeClaim>
                        {
                            new ScopeClaim(Constants.ClaimTypes.Name),
                            new ScopeClaim(Constants.ClaimTypes.Role)
                        }
                    },new Scope
                    {
                        Name = "idAdmin",
                        DisplayName = "IdentityAdmin",
                        Description = "Authorization for IdentityAdmin",
                        Type = ScopeType.Identity,
                        Claims = new List<ScopeClaim>
                        {
                            new ScopeClaim(Constants.ClaimTypes.Name),
                            new ScopeClaim(Constants.ClaimTypes.Role)
                        }
                    },
                };
            }
        }
        

    }
}