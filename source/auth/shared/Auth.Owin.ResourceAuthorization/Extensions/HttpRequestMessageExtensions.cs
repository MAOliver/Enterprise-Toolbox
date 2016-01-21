/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see LICENSE
 */

using Microsoft.Owin;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth.Owin.ResourceAuthorization;
using Auth.Owin.ResourceAuthorization.IdentityModel;

namespace System.Net.Http
{
    public static class HttpRequestMessageExtensions
    {
        public static bool CheckAccess(this HttpRequestMessage request, string action, params string[] resources)
        {
            return AsyncHelper.RunSync(() => request.CheckAccessAsync(action, resources));
        }

        public static Task<bool> CheckAccessAsync(this HttpRequestMessage request, string action, params string[] resources)
        {
            var ctx = new ResourceAuthorizationContext(request.ToClaimsPrincipal(), action, resources);
            
            return request.CheckAccessAsync(ctx);
        }

        public static ClaimsPrincipal ToClaimsPrincipal(this HttpRequestMessage request)
        {
            var user = request.GetRequestContext().Principal as ClaimsPrincipal;
            user = user ?? Principal.Anonymous;
            return user;
        }
        
        public static Task<bool> CheckAccessAsync(this HttpRequestMessage request, IEnumerable<Claim> actions, IEnumerable<Claim> resources)
        {
            var authorizationContext = new ResourceAuthorizationContext(
                request.GetOwinContext().Authentication.User ?? Principal.Anonymous,
                actions,
                resources);

            return request.CheckAccessAsync(authorizationContext);
        }

        public static Task<bool> CheckAccessAsync(this HttpRequestMessage request, ResourceAuthorizationContext authorizationContext)
        {
            return request.GetOwinContext().CheckAccessAsync(authorizationContext);
        }

        private static async Task<bool> CheckAccessAsync(this IOwinContext context, ResourceAuthorizationContext authorizationContext)
        {
            return await context.GetAuthorizationManager().CheckAccessAsync(authorizationContext);
        }

        private static IResourceAuthorizationManager GetAuthorizationManager(this IOwinContext context)
        {
            var am = context.Get<IResourceAuthorizationManager>(ResourceAuthorizationManagerMiddleware.Key);

            if (am == null)
            {
                throw new InvalidOperationException("No AuthorizationManager set.");
            }

            return am;
        }
    }
}