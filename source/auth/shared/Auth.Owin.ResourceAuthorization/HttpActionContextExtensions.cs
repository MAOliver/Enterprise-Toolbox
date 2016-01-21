using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http.Controllers;

namespace Auth.Owin.ResourceAuthorization
{
    public static class HttpActionContextExtensions 
    {
        public static IEnumerable<Claim> ResourcesFromRouteParameters(this HttpActionContext actionContext)
        {
            return actionContext.ControllerContext.RouteData.Values.Select(arg => new Claim(arg.Key, arg.Value.ToString()));
        }

        public static List<Claim> ResourceFromController(this HttpActionContext actionContext)
        {
            return new List<Claim> { new Claim("controller", actionContext.ControllerContext.ControllerDescriptor.ControllerName) };
        }

        public static Claim ActionFromController(this HttpActionContext actionContext)
        {
            return new Claim("action", actionContext.ActionDescriptor.ActionName);
        }
    }
}