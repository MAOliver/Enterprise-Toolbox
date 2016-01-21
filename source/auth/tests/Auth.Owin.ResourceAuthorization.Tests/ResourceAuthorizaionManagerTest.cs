using System.Linq;
using System.Threading.Tasks;
using Auth.Api.Configuration;

namespace Auth.Owin.ResourceAuthorization.Tests
{
    public class ResourceAuthorizaionManagerTest : ResourceAuthorizationManager
    {
        const string ScopeClaimType = "scope";
        
        public override Task<bool> CheckAccessAsync(ResourceAuthorizationContext context)
        {            
            var resource = context.Resource.First().Value;

            if (resource == QuoteResource.Name)
            {
                return CheckQuoteActionAccess(context);
            }
            return Nok();
        }


        private Task<bool> CheckQuoteActionAccess(ResourceAuthorizationContext context)
        {
            var action = context.Action.First().Value;
            if ( action == QuoteResource.Actions.Create)
            {
                return CheckQuoteActionAccessByLevel(context);
            }
            return Nok();
        }

        private Task<bool> CheckQuoteActionAccessByLevel(ResourceAuthorizationContext context)
        {
            var resource = context.Resource.Skip(1).Take(1).First().Value;
            var grantedScopes = context.Principal.FindAll(ScopeClaimType).Select(c => c.Value).ToList();

            if (resource == QuoteResource.AccessLevels.Private)
            {
                return Eval(grantedScopes.Any(scope => scope == AuthScopeConfiguration.Apis.Rates.Private.Name));
            }
            if (resource == QuoteResource.AccessLevels.Default)
            {
                return Eval(grantedScopes.Any(scope => scope == AuthScopeConfiguration.Apis.Rates.Default.Name));
            }
            return Nok();
        }
    }
}