using System;
using Auth.Api.Configuration;
using Xunit;

namespace Auth.Owin.ResourceAuthorization.Tests
{
    public class AuthConfigurationsTests : AuthConfigurationsTestKit
    {
        private readonly IResourceAuthorizationManager _subject;
        public AuthConfigurationsTests()
        {
            _subject = new ResourceAuthorizaionManagerTest();
        }

        [Fact(DisplayName = "Private scope can access rates")]
        public void Authenticated_Can_View_Album()
        {
            var ctx = new ResourceAuthorizationContext
                (
                User("test", Tuple.Create("scope", AuthScopeConfiguration.Apis.Rates.Private.Name)),QuoteResource.Actions.Create, QuoteResource.Name,QuoteResource.AccessLevels.Private
                );
             
            Assert.True(_subject.CheckAccessAsync(ctx).Result);
        }
    }
}