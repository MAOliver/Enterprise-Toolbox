using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Auth.Owin.ResourceAuthorization.Tests
{
    public class AuthConfigurationsTestKit
    {
        protected ClaimsPrincipal User(string name, params Tuple<string,string>[] claims)
        {
            var ci = new ClaimsIdentity("Password");
            ci.AddClaim(new Claim(ClaimTypes.Name, name));
            foreach (var claim in claims)
            {
                ci.AddClaim(new Claim(claim.Item1, claim.Item2));
            }
            return new ClaimsPrincipal(ci);
        }
    }
}
