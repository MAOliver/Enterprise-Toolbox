using System.Net;
using System.Net.Http;

namespace Auth.Owin.ResourceAuthorization
{
    public class AccessDeniedResult : System.Web.Http.Results.StatusCodeResult
    {
        public AccessDeniedResult(HttpRequestMessage request)
            : base(GetCode(request), request)
        {
        }

        public static HttpStatusCode GetCode(HttpRequestMessage request)
        {
            var ctx = request.GetRequestContext();
            var principal = ctx.Principal;
            var identity = principal != null ? principal.Identity : null;

            return
                identity != null 
                && identity.IsAuthenticated
                ? HttpStatusCode.Forbidden
                : HttpStatusCode.Unauthorized;
        }
    }
}
