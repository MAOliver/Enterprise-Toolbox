using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using IdentityServer3.Core.Models;
using Xunit;
using static Xunit.Assert;

namespace Identity.Tests.xUnit
{
    public class ClientTests
    {
        [Fact(DisplayName = "Should map client to EF Client")]
        public void TestAddClient()
        {

            var entity = new Client()
            {
                ClientId = "test",
                ClientName = "test2",
                AllowedScopes = new List<string> {"item1", "item2"},
                RedirectUris = new List<string> {"http://redirect1"},
                AccessTokenType = AccessTokenType.Jwt,
                Flow = Flows.ResourceOwner,
                ClientSecrets = new List<Secret> {new Secret("secret")}

                
            }.ToEntity();

            NotNull(entity);
        }
    }


}
