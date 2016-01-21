using System.Threading.Tasks;

namespace Auth.Owin.ResourceAuthorization
{
    public interface IResourceAuthorizationManager
    {
        Task<bool> CheckAccessAsync(ResourceAuthorizationContext context);
    }
}