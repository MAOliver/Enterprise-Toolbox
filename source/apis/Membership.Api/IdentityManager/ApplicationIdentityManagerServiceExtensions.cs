﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Identity.Core;
using Identity.Core.Entities;
using IdentityManager;
using IdentityManager.Configuration;

namespace Membership.Api.IdentityManager
{
    public static class ApplicationIdentityManagerServiceExtensions
    {
        public static void ConfigureIdentityManagerService(this IdentityManagerServiceFactory factory, string connectionString)
        {
            factory.IdentityManagerService = new Registration<IIdentityManagerService, ApplicationIdentityManagerService>();
            factory.Register(new Registration<MembershipContext>(resolver => new MembershipContext(connectionString)));
            factory.Register(new Registration<ApplicationUserManager>());
            factory.Register(new Registration<ApplicationUserStore>());
            factory.Register(new Registration<ApplicationRoleManager>());
            factory.Register(new Registration<ApplicationRoleStore>());
            factory.Register(new Registration<MembershipContext>());
        }
    }
}