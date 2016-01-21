using Identity.Core.Managers;
using Identity.Core.Services;
using Identity.Core.Stores;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;

namespace Identity.Core.Extensions
{
    /// <summary>
    /// Extensions for the UserService object
    /// </summary>
    public static class UserServiceExtensions
    {
        public static void ConfigureUserService(this IdentityServerServiceFactory factory, string connString)
        {
            factory.UserService = new Registration<IUserService, ApplicationUserService>();
            factory.Register(new Registration<ApplicationUserManager>());
            factory.Register(new Registration<ApplicationUserStore>());
            factory.Register(new Registration<MembershipContext>(resolver => new MembershipContext(connString)));           
        }
    }
}
