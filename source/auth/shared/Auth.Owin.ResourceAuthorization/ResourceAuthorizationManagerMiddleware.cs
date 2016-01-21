using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auth.Owin.ResourceAuthorization
{
    public class ResourceAuthorizationManagerMiddleware
    {
        public const string Key = "idm:resourceAuthorizationManager";

        private readonly Func<IDictionary<string, object>, Task> _next;
        private readonly ResourceAuthorizationMiddlewareOptions _options;

        public ResourceAuthorizationManagerMiddleware(Func<IDictionary<string, object>, Task> next, ResourceAuthorizationMiddlewareOptions options)
        {
            _options = options;
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> env)
        {
            env[Key] = _options.Manager ?? _options.ManagerProvider(env);
            await _next(env);
        }
    }
}