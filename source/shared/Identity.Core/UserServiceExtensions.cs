﻿using Identity.Core.Entities;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;

namespace Identity.Core
{
    public static class UserServiceExtensions
    {
        public static void ConfigureUserService(this IdentityServerServiceFactory factory, string connString)
        {
            factory.UserService = new Registration<IUserService, ApplicationUserService>();
            factory.Register(new Registration<ApplicationUserManager>());
            factory.Register(new Registration<ApplicationUserStore>());
            factory.Register(new Registration<IdentityContext>(resolver => new IdentityContext(connString)));
        }
    }
}
