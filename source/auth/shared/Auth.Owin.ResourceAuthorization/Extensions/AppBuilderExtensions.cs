using Auth.Owin.ResourceAuthorization;
using Owin;

namespace Owin
{
    public static class AppBuilderExtensions
    {
        public static IAppBuilder UseResourceAuthorization(this IAppBuilder app, IResourceAuthorizationManager authorizationManager)
        {
            var options = new ResourceAuthorizationMiddlewareOptions
            {
                Manager = authorizationManager
            };

            app.UseResourceAuthorization(options);
            return app;
        }

        public static IAppBuilder UseResourceAuthorization(this IAppBuilder app, ResourceAuthorizationMiddlewareOptions options)
        {
            app.Use(typeof(ResourceAuthorizationManagerMiddleware), options);
            return app;
        }
    }
}