using System;
using System.Threading.Tasks;

namespace Auth.Owin.ResourceAuthorization
{
    public abstract class ResourceAuthorizationManager : IResourceAuthorizationManager
    {
        public virtual Task<bool> CheckAccessAsync(ResourceAuthorizationContext context)
        {
            throw new NotImplementedException();
        }

        protected virtual Task<bool> Ok()
        {
            return Task.FromResult(true);
        }

        protected virtual Task<bool> Nok()
        {
            return Task.FromResult(false);
        }

        protected virtual Task<bool> Eval(bool val)
        {
            return Task.FromResult(val);
        }
    }
}