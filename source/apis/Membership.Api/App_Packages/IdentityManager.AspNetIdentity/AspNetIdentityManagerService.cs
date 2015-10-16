using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityManager;
using Microsoft.AspNet.Identity;
using Constants = IdentityManager.Constants;

namespace Membership.Api.App_Packages.IdentityManager.AspNetIdentity
{
    public class AspNetIdentityManagerService<TUser, TUserKey, TRole, TRoleKey> : IIdentityManagerService
       where TUser : class, IUser<TUserKey>, new()
       where TUserKey : IEquatable<TUserKey>
       where TRole : class, IRole<TRoleKey>, new()
       where TRoleKey : IEquatable<TRoleKey>
    {
        public string RoleClaimType { get; set; }

        protected UserManager<TUser, TUserKey> UserManager;
        protected Func<string, TUserKey> ConvertUserSubjectToKey;

        protected RoleManager<TRole, TRoleKey> RoleManager;
        protected Func<string, TRoleKey> ConvertRoleSubjectToKey;

        readonly Func<Task<IdentityManagerMetadata>> _metadataFunc;

        AspNetIdentityManagerService(UserManager<TUser, TUserKey> userManager, RoleManager<TRole, TRoleKey> roleManager, Func<string, TUserKey> parseUserSubject = null, Func<string, TRoleKey> parseRoleSubject = null)
        {
            if (userManager == null) throw new ArgumentNullException(nameof(userManager));
            if (roleManager == null) throw new ArgumentNullException(nameof(roleManager));

            if (!userManager.SupportsQueryableUsers)
            {
                throw new InvalidOperationException("UserManager must support queryable users.");
            }

            UserManager = userManager;
            RoleManager = roleManager;

            if (userManager.UserTokenProvider == null)
            {
                userManager.UserTokenProvider = new TokenProvider<TUser, TUserKey>();
            }

            if (parseUserSubject != null)
            {
                ConvertUserSubjectToKey = parseUserSubject;
            }
            else
            {
                var keyType = typeof(TUserKey);
                if (keyType == typeof(string)) ConvertUserSubjectToKey = subject => (TUserKey)ParseString(subject);
                else if (keyType == typeof(int)) ConvertUserSubjectToKey = subject => (TUserKey)ParseInt(subject);
                else if (keyType == typeof(uint)) ConvertUserSubjectToKey = subject => (TUserKey)ParseUInt32(subject);
                else if (keyType == typeof(long)) ConvertUserSubjectToKey = subject => (TUserKey)ParseLong(subject);
                else if (keyType == typeof(Guid)) ConvertUserSubjectToKey = subject => (TUserKey)ParseGuid(subject);
                else
                {
                    throw new InvalidOperationException("User Key type not supported");
                }
            }

            if (parseRoleSubject != null)
            {
                ConvertRoleSubjectToKey = parseRoleSubject;
            }
            else
            {
                var keyType = typeof(TRoleKey);
                if (keyType == typeof(string)) ConvertRoleSubjectToKey = subject => (TRoleKey)ParseString(subject);
                else if (keyType == typeof(int)) ConvertRoleSubjectToKey = subject => (TRoleKey)ParseInt(subject);
                else if (keyType == typeof(uint)) ConvertRoleSubjectToKey = subject => (TRoleKey)ParseUInt32(subject);
                else if (keyType == typeof(long)) ConvertRoleSubjectToKey = subject => (TRoleKey)ParseLong(subject);
                else if (keyType == typeof(Guid)) ConvertRoleSubjectToKey = subject => (TRoleKey)ParseGuid(subject);
                else
                {
                    throw new InvalidOperationException("Role Key type not supported");
                }
            }

            RoleClaimType = Constants.ClaimTypes.Role;
        }

        public AspNetIdentityManagerService(
            UserManager<TUser, TUserKey> userManager,
            RoleManager<TRole, TRoleKey> roleManager,
            bool includeAccountProperties = true,
            Func<string, TUserKey> parseUserSubject = null, Func<string, TRoleKey> parseRoleSubject = null)
            : this(userManager, roleManager, parseUserSubject, parseRoleSubject)
        {
            _metadataFunc = () => Task.FromResult(GetStandardMetadata(includeAccountProperties));
        }

        public AspNetIdentityManagerService(
           UserManager<TUser, TUserKey> userManager,
           RoleManager<TRole, TRoleKey> roleManager,
           IdentityManagerMetadata metadata,
           Func<string, TUserKey> parseUserSubject = null, Func<string, TRoleKey> parseRoleSubject = null)
            : this(userManager, roleManager, () => Task.FromResult(metadata), parseUserSubject, parseRoleSubject)
        {
        }

        public AspNetIdentityManagerService(
           UserManager<TUser, TUserKey> userManager,
           RoleManager<TRole, TRoleKey> roleManager,
           Func<Task<IdentityManagerMetadata>> metadataFunc,
           Func<string, TUserKey> parseUserSubject = null, Func<string, TRoleKey> parseRoleSubject = null)
            : this(userManager, roleManager, parseUserSubject, parseRoleSubject)
        {
            _metadataFunc = metadataFunc;
        }

        static object ParseString(string sub)
        {
            return sub;
        }

        static object ParseInt(string sub)
        {
            int key;
            if (!Int32.TryParse(sub, out key)) return 0;
            return key;
        }

        static object ParseLong(string sub)
        {
            long key;
            if (!Int64.TryParse(sub, out key)) return 0;
            return key;
        }

        static object ParseGuid(string sub)
        {
            Guid key;
            if (!Guid.TryParse(sub, out key)) return Guid.Empty;
            return key;
        }

        static object ParseUInt32(string sub)
        {
            uint key;
            if (!UInt32.TryParse(sub, out key)) return 0;
            return key;
        }

        public virtual IdentityManagerMetadata GetStandardMetadata(bool includeAccountProperties = true)
        {
            var update = new List<PropertyMetadata>();
            if (UserManager.SupportsUserPassword)
            {
                update.Add(PropertyMetadata.FromFunctions<TUser, string>(Constants.ClaimTypes.Password, x => null, SetPassword, name: "Password", dataType: PropertyDataType.Password, required: true));
            }
            if (UserManager.SupportsUserEmail)
            {
                update.Add(PropertyMetadata.FromFunctions<TUser, string>(Constants.ClaimTypes.Email, GetEmail, SetEmail, name: "Email", dataType: PropertyDataType.Email));
            }
            if (UserManager.SupportsUserPhoneNumber)
            {
                update.Add(PropertyMetadata.FromFunctions<TUser, string>(Constants.ClaimTypes.Phone, GetPhone, SetPhone, name: "Phone", dataType: PropertyDataType.String));
            }
            if (UserManager.SupportsUserTwoFactor)
            {
                update.Add(PropertyMetadata.FromFunctions<TUser, bool>("two_factor", GetTwoFactorEnabled, SetTwoFactorEnabled, name: "Two Factor Enabled", dataType: PropertyDataType.Boolean));
            }
            if (UserManager.SupportsUserLockout)
            {
                update.Add(PropertyMetadata.FromFunctions<TUser, bool>("locked_enabled", GetLockoutEnabled, SetLockoutEnabled, name: "Lockout Enabled", dataType: PropertyDataType.Boolean));
                update.Add(PropertyMetadata.FromFunctions<TUser, bool>("locked", GetLockedOut, SetLockedOut, name: "Locked Out", dataType: PropertyDataType.Boolean));
            }

            if (includeAccountProperties)
            {
                update.AddRange(PropertyMetadata.FromType<TUser>());
            }

            var create = new List<PropertyMetadata>
            {
                PropertyMetadata.FromProperty<TUser>(x => x.UserName, type: Constants.ClaimTypes.Username,required: true),
                PropertyMetadata.FromFunctions<TUser, string>(Constants.ClaimTypes.Password, x => null, SetPassword, name: "Password", dataType: PropertyDataType.Password, required: true)
            };

            var user = new UserMetadata
            {
                SupportsCreate = true,
                SupportsDelete = true,
                SupportsClaims = UserManager.SupportsUserClaim,
                CreateProperties = create,
                UpdateProperties = update
            };

            //have to modify this from original installed version otherwise it assumes the name is a guid
            var role = new RoleMetadata
            {
                RoleClaimType = RoleClaimType,
                SupportsCreate = true,
                SupportsDelete = true,
                CreateProperties = new[] {
                    PropertyMetadata.FromProperty<TRole>(x=>x.Name, type:Constants.ClaimTypes.Name, required:true, dataType: PropertyDataType.String),
                }
            };

            var meta = new IdentityManagerMetadata
            {
                UserMetadata = user,
                RoleMetadata = role
            };
            return meta;
        }

        public virtual PropertyMetadata GetMetadataForClaim(string type, string name = null, PropertyDataType dataType = PropertyDataType.String, bool required = false)
        {
            return PropertyMetadata.FromFunctions(type, GetForClaim(type), SetForClaim(type), name, dataType, required);
        }

        public virtual Func<TUser, string> GetForClaim(string type)
        {
            return user => UserManager.GetClaims(user.Id).Where(x => x.Type == type).Select(x => x.Value).FirstOrDefault();
        }

        public virtual Func<TUser, string, IdentityManagerResult> SetForClaim(string type)
        {
            return (user, value) =>
            {
                var claims = UserManager.GetClaims(user.Id).Where(x => x.Type == type).ToArray();
                foreach (var claim in claims)
                {
                    var result = UserManager.RemoveClaim(user.Id, claim);
                    if (!result.Succeeded)
                    {
                        return new IdentityManagerResult(result.Errors.First());
                    }
                }
                if (!String.IsNullOrWhiteSpace(value))
                {
                    var result = UserManager.AddClaim(user.Id, new Claim(type, value));
                    if (!result.Succeeded)
                    {
                        return new IdentityManagerResult(result.Errors.First());
                    }
                }
                return IdentityManagerResult.Success;
            };
        }

        public virtual IdentityManagerResult SetPassword(TUser user, string password)
        {
            var token = UserManager.GeneratePasswordResetToken(user.Id);
            var result = UserManager.ResetPassword(user.Id, token, password);
            return !result.Succeeded ? new IdentityManagerResult(result.Errors.First()) : IdentityManagerResult.Success;
        }

        public virtual string GetEmail(TUser user)
        {
            return UserManager.GetEmail(user.Id);
        }

        public virtual IdentityManagerResult SetEmail(TUser user, string email)
        {
            var result = UserManager.SetEmail(user.Id, email);
            if (!result.Succeeded)
            {
                return new IdentityManagerResult(result.Errors.First());
            }

            if (string.IsNullOrWhiteSpace(email)) return IdentityManagerResult.Success;

            var token = UserManager.GenerateEmailConfirmationToken(user.Id);
            result = UserManager.ConfirmEmail(user.Id, token);

            return !result.Succeeded ? new IdentityManagerResult(result.Errors.First()) : IdentityManagerResult.Success;
        }

        public virtual string GetPhone(TUser user)
        {
            return UserManager.GetPhoneNumber(user.Id);
        }

        public virtual IdentityManagerResult SetPhone(TUser user, string phone)
        {
            var result = UserManager.SetPhoneNumber(user.Id, phone);
            if (!result.Succeeded)
            {
                return new IdentityManagerResult(result.Errors.First());
            }

            if (string.IsNullOrWhiteSpace(phone)) return IdentityManagerResult.Success;

            var token = UserManager.GenerateChangePhoneNumberToken(user.Id, phone);
            result = UserManager.ChangePhoneNumber(user.Id, phone, token);

            return !result.Succeeded ? new IdentityManagerResult(result.Errors.First()) : IdentityManagerResult.Success;
        }

        public virtual bool GetTwoFactorEnabled(TUser user)
        {
            return UserManager.GetTwoFactorEnabled(user.Id);
        }
        public virtual IdentityManagerResult SetTwoFactorEnabled(TUser user, bool enabled)
        {
            var result = UserManager.SetTwoFactorEnabled(user.Id, enabled);
            if (!result.Succeeded)
            {
                return new IdentityManagerResult(result.Errors.First());
            }

            return IdentityManagerResult.Success;
        }

        public virtual bool GetLockoutEnabled(TUser user)
        {
            return UserManager.GetLockoutEnabled(user.Id);
        }
        public virtual IdentityManagerResult SetLockoutEnabled(TUser user, bool enabled)
        {
            var result = UserManager.SetLockoutEnabled(user.Id, enabled);
            if (!result.Succeeded)
            {
                return new IdentityManagerResult(result.Errors.First());
            }

            return IdentityManagerResult.Success;
        }

        public virtual bool GetLockedOut(TUser user)
        {
            return UserManager.GetLockoutEndDate(user.Id) > DateTimeOffset.UtcNow;
        }
        public virtual IdentityManagerResult SetLockedOut(TUser user, bool locked)
        {
            if (locked)
            {
                var result = UserManager.SetLockoutEndDate(user.Id, DateTimeOffset.MaxValue);
                if (!result.Succeeded)
                {
                    return new IdentityManagerResult(result.Errors.First());
                }
            }
            else
            {
                var result = UserManager.SetLockoutEndDate(user.Id, DateTimeOffset.MinValue);
                if (!result.Succeeded)
                {
                    return new IdentityManagerResult(result.Errors.First());
                }
            }

            return IdentityManagerResult.Success;
        }

        public virtual Task<IdentityManagerMetadata> GetMetadataAsync()
        {
            return _metadataFunc();
        }

        public virtual Task<IdentityManagerResult<QueryResult<UserSummary>>> QueryUsersAsync(string filter, int start, int count)
        {
            var query =
                from user in UserManager.Users
                orderby user.UserName
                select user;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query =
                    from user in query
                    where user.UserName.Contains(filter)
                    orderby user.UserName
                    select user;
            }

            var total = query.Count();
            var users = query.Skip(start).Take(count).ToArray();

            var result = new QueryResult<UserSummary>
            {
                Start = start,
                Count = count,
                Total = total,
                Filter = filter,
                Items = users.Select(x =>
                {
                    var user = new UserSummary
                    {
                        Subject = x.Id.ToString(),
                        Username = x.UserName,
                        Name = DisplayNameFromUser(x)
                    };

                    return user;
                }).ToArray()
            };

            return Task.FromResult(new IdentityManagerResult<QueryResult<UserSummary>>(result));
        }

        protected virtual string DisplayNameFromUser(TUser user)
        {
            if (!UserManager.SupportsUserClaim) return null;
            var claims = UserManager.GetClaims(user.Id);
            var name = claims.Where(x => x.Type == Constants.ClaimTypes.Name).Select(x => x.Value).FirstOrDefault();
            return !string.IsNullOrWhiteSpace(name) ? name : null;
        }

        public virtual async Task<IdentityManagerResult<CreateResult>> CreateUserAsync(IEnumerable<PropertyValue> properties)
        {
            var props = properties.ToArray();
            var usernameClaim = props.Single(x => x.Type == Constants.ClaimTypes.Username);
            var passwordClaim = props.Single(x => x.Type == Constants.ClaimTypes.Password);

            var username = usernameClaim.Value;
            var password = passwordClaim.Value;

            var exclude = new[] { Constants.ClaimTypes.Username, Constants.ClaimTypes.Password };
            var otherProperties = props.Where(x => !exclude.Contains(x.Type)).ToArray();

            var metadata = await GetMetadataAsync();
            var createProps = metadata.UserMetadata.GetCreateProperties();

            var user = new TUser { UserName = username };
            foreach (var prop in otherProperties)
            {
                var propertyResult = SetUserProperty(createProps, user, prop.Type, prop.Value);
                if (!propertyResult.IsSuccess)
                {
                    return new IdentityManagerResult<CreateResult>(propertyResult.Errors.ToArray());
                }
            }

            var result = await UserManager.CreateAsync(user, password);
            return !result.Succeeded 
                ? new IdentityManagerResult<CreateResult>(result.Errors.ToArray()) 
                : new IdentityManagerResult<CreateResult>(new CreateResult { Subject = user.Id.ToString() });
        }

        public virtual async Task<IdentityManagerResult> DeleteUserAsync(string subject)
        {
            var key = ConvertUserSubjectToKey(subject);
            var user = await UserManager.FindByIdAsync(key);
            if (user == null)
            {
                return new IdentityManagerResult("Invalid subject");
            }

            var result = await UserManager.DeleteAsync(user);

            return !result.Succeeded ? new IdentityManagerResult<CreateResult>(result.Errors.ToArray()) : IdentityManagerResult.Success;
        }

        public virtual async Task<IdentityManagerResult<UserDetail>> GetUserAsync(string subject)
        {
            var key = ConvertUserSubjectToKey(subject);
            var user = await UserManager.FindByIdAsync(key);
            if (user == null)
            {
                return new IdentityManagerResult<UserDetail>((UserDetail)null);
            }

            var result = new UserDetail
            {
                Subject = subject,
                Username = user.UserName,
                Name = DisplayNameFromUser(user),
            };

            var metadata = await GetMetadataAsync();

            var props =
                from prop in metadata.UserMetadata.UpdateProperties
                select new PropertyValue
                {
                    Type = prop.Type,
                    Value = GetUserProperty(prop, user)
                };

            result.Properties = props.ToArray();

            if (!UserManager.SupportsUserClaim) return new IdentityManagerResult<UserDetail>(result);

            var userClaims = await UserManager.GetClaimsAsync(key);
            var claims = new List<ClaimValue>();
            if (userClaims != null)
            {
                claims.AddRange(userClaims.Select(x => new ClaimValue { Type = x.Type, Value = x.Value }));
            }
            result.Claims = claims.ToArray();

            return new IdentityManagerResult<UserDetail>(result);
        }

        public virtual async Task<IdentityManagerResult> SetUserPropertyAsync(string subject, string type, string value)
        {
            var key = ConvertUserSubjectToKey(subject);
            var user = await UserManager.FindByIdAsync(key);
            if (user == null)
            {
                return new IdentityManagerResult("Invalid subject");
            }

            var errors = ValidateUserProperty(type, value).ToArray();
            if (errors.Any())
            {
                return new IdentityManagerResult(errors);
            }

            var metadata = await GetMetadataAsync();
            var propResult = SetUserProperty(metadata.UserMetadata.UpdateProperties, user, type, value);
            if (!propResult.IsSuccess)
            {
                return propResult;
            }

            var result = await UserManager.UpdateAsync(user);
            return !result.Succeeded 
                ? new IdentityManagerResult(result.Errors.ToArray()) 
                : IdentityManagerResult.Success;
        }

        public virtual async Task<IdentityManagerResult> AddUserClaimAsync(string subject, string type, string value)
        {
            var key = ConvertUserSubjectToKey(subject);
            var user = await UserManager.FindByIdAsync(key);
            if (user == null)
            {
                return new IdentityManagerResult("Invalid subject");
            }

            var existingClaims = await UserManager.GetClaimsAsync(key);
            if (existingClaims.Any(x => x.Type == type && x.Value == value)) return IdentityManagerResult.Success;

            var result = await UserManager.AddClaimAsync(key, new Claim(type, value));

            return !result.Succeeded 
                ? new IdentityManagerResult<CreateResult>(result.Errors.ToArray()) 
                : IdentityManagerResult.Success;
        }

        public virtual async Task<IdentityManagerResult> RemoveUserClaimAsync(string subject, string type, string value)
        {
            var key = ConvertUserSubjectToKey(subject);
            var user = await UserManager.FindByIdAsync(key);
            if (user == null)
            {
                return new IdentityManagerResult("Invalid subject");
            }

            var result = await UserManager.RemoveClaimAsync(key, new Claim(type, value));
            return !result.Succeeded 
                ? new IdentityManagerResult<CreateResult>(result.Errors.ToArray()) 
                : IdentityManagerResult.Success;
        }

        protected virtual IEnumerable<string> ValidateUserProperty(string type, string value)
        {
            return Enumerable.Empty<string>();
        }

        protected virtual string GetUserProperty(PropertyMetadata propMetadata, TUser user)
        {
            string val;
            if (propMetadata.TryGet(user, out val))
            {
                return val;
            }

            throw new Exception("Invalid property type " + propMetadata.Type);
        }

        protected virtual IdentityManagerResult SetUserProperty(IEnumerable<PropertyMetadata> propsMeta, TUser user, string type, string value)
        {
            IdentityManagerResult result;
            if (propsMeta.TrySet(user, type, value, out result))
            {
                return result;
            }

            throw new Exception("Invalid property type " + type);
        }


        protected virtual void ValidateSupportsRoles()
        {
            if (RoleManager == null)
            {
                throw new InvalidOperationException("Roles Not Supported");
            }
        }

        public virtual async Task<IdentityManagerResult<CreateResult>> CreateRoleAsync(IEnumerable<PropertyValue> properties)
        {
            ValidateSupportsRoles();

            var nameClaim = properties.Single(x => x.Type == Constants.ClaimTypes.Name);

            var name = nameClaim.Value;

            var exclude = new[] { Constants.ClaimTypes.Name };
            var otherProperties = properties.Where(x => !exclude.Contains(x.Type)).ToArray();

            var metadata = await GetMetadataAsync();
            var createProps = metadata.RoleMetadata.GetCreateProperties();

            var role = new TRole() { Name = name };
            foreach (var prop in otherProperties)
            {
                var roleResult = SetRoleProperty(createProps, role, prop.Type, prop.Value);
                if (!roleResult.IsSuccess)
                {
                    return new IdentityManagerResult<CreateResult>(roleResult.Errors.ToArray());
                }
            }

            var result = await RoleManager.CreateAsync(role);
            return !result.Succeeded 
                ? new IdentityManagerResult<CreateResult>(result.Errors.ToArray()) 
                : new IdentityManagerResult<CreateResult>(new CreateResult { Subject = role.Id.ToString() });
        }

        public virtual async Task<IdentityManagerResult> DeleteRoleAsync(string subject)
        {
            ValidateSupportsRoles();

            var key = ConvertRoleSubjectToKey(subject);
            var role = await RoleManager.FindByIdAsync(key);
            if (role == null)
            {
                return new IdentityManagerResult("Invalid subject");
            }

            var result = await RoleManager.DeleteAsync(role);
            return !result.Succeeded 
                ? new IdentityManagerResult<CreateResult>(result.Errors.ToArray()) 
                : IdentityManagerResult.Success;
        }

        public virtual async Task<IdentityManagerResult<RoleDetail>> GetRoleAsync(string subject)
        {
            ValidateSupportsRoles();

            var key = ConvertRoleSubjectToKey(subject);
            var role = await RoleManager.FindByIdAsync(key);
            if (role == null)
            {
                return new IdentityManagerResult<RoleDetail>((RoleDetail)null);
            }

            var result = new RoleDetail
            {
                Subject = subject,
                Name = role.Name,
                // Description
            };

            var metadata = await GetMetadataAsync();

            var props =
                from prop in metadata.RoleMetadata.UpdateProperties
                select new PropertyValue
                {
                    Type = prop.Type,
                    Value = GetRoleProperty(prop, role)
                };
            result.Properties = props.ToArray();

            return new IdentityManagerResult<RoleDetail>(result);
        }

        public virtual Task<IdentityManagerResult<QueryResult<RoleSummary>>> QueryRolesAsync(string filter, int start, int count)
        {
            ValidateSupportsRoles();

            if (start < 0) start = 0;
            if (count < 0) count = int.MaxValue;

            var query =
                from role in RoleManager.Roles
                orderby role.Name
                select role;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query =
                    from role in query
                    where role.Name.Contains(filter)
                    orderby role.Name
                    select role;
            }

            int total = query.Count();
            var roles = query.Skip(start).Take(count).ToArray();

            var result = new QueryResult<RoleSummary>
            {
                Start = start,
                Count = count,
                Total = total,
                Filter = filter,
                Items = roles.Select(x =>
                {
                    var user = new RoleSummary
                    {
                        Subject = x.Id.ToString(),
                        Name = x.Name,
                        // Description
                    };

                    return user;
                }).ToArray()
            };

            return Task.FromResult(new IdentityManagerResult<QueryResult<RoleSummary>>(result));
        }

        public virtual async Task<IdentityManagerResult> SetRolePropertyAsync(string subject, string type, string value)
        {
            ValidateSupportsRoles();

            var key = ConvertRoleSubjectToKey(subject);
            var role = await RoleManager.FindByIdAsync(key);
            if (role == null)
            {
                return new IdentityManagerResult("Invalid subject");
            }

            var errors = ValidateRoleProperty(type, value).ToArray();
            if (errors.Any())
            {
                return new IdentityManagerResult(errors);
            }

            var metadata = await GetMetadataAsync();
            var result = SetRoleProperty(metadata.RoleMetadata.UpdateProperties, role, type, value);
            if (!result.IsSuccess)
            {
                return result;
            }

            var updateResult = await RoleManager.UpdateAsync(role);
            if (!updateResult.Succeeded)
            {
                return new IdentityManagerResult(result.Errors.ToArray());
            }

            return IdentityManagerResult.Success;
        }

        protected virtual IEnumerable<string> ValidateRoleProperties(IEnumerable<PropertyValue> properties)
        {
            return properties.Select(x => ValidateRoleProperty(x.Type, x.Value)).Aggregate((x, y) => x.Concat(y));
        }

        protected virtual IEnumerable<string> ValidateRoleProperty(string type, string value)
        {
            return Enumerable.Empty<string>();
        }

        protected virtual string GetRoleProperty(PropertyMetadata propMetadata, TRole role)
        {
            string val;
            if (propMetadata.TryGet(role, out val))
            {
                return val;
            }

            throw new Exception("Invalid property type " + propMetadata.Type);
        }

        protected virtual IdentityManagerResult SetRoleProperty(IEnumerable<PropertyMetadata> propsMeta, TRole role, string type, string value)
        {
            IdentityManagerResult result;
            if (propsMeta.TrySet(role, type, value, out result))
            {
                return result;
            }

            throw new Exception("Invalid property type " + type);
        }
    }
}