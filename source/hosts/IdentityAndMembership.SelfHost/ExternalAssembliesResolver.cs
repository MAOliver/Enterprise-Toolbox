using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http.Dispatcher;

namespace IdentityAndMembership.SelfHost
{
    public class ExternalAssembliesResolver : DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies()
        {
            var startupTypes = new List<Type>
            {
                typeof (Membership.Api.Startup),
                typeof (Auth.Api.Startup)
            };
            
            return base.GetAssemblies().Union(startupTypes.Select(Assembly.GetAssembly)).ToArray();
        }
    }
}