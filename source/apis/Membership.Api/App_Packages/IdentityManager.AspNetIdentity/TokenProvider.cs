using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Membership.Api.App_Packages.IdentityManager.AspNetIdentity
{
    class TokenProvider<TUser, TKey> : IUserTokenProvider<TUser, TKey>
        where TUser : class, IUser<TKey>
        where TKey : System.IEquatable<TKey>
    {
        public Task<string> GenerateAsync(string purpose, UserManager<TUser, TKey> manager, TUser user)
        {
            return Task.FromResult(purpose + user.Id);
        }

        public Task<bool> IsValidProviderForUserAsync(UserManager<TUser, TKey> manager, TUser user)
        {
            return Task.FromResult(true);
        }

        public Task NotifyAsync(string token, UserManager<TUser, TKey> manager, TUser user)
        {
            return Task.FromResult(0);
        }

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser, TKey> manager, TUser user)
        {
            return Task.FromResult((purpose + user.Id) == token);
        }
    }
}