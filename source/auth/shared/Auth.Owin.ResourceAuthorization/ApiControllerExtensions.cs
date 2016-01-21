using System;
using System.Web.Http;

namespace Auth.Owin.ResourceAuthorization
{
    public static class ApiControllerExtensions
    {
        public static IHttpActionResult AccessDenied(this ApiController controller)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));

            return new AccessDeniedResult(controller.Request);
        }
    }
}
