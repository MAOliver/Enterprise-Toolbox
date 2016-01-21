using System;
using System.Configuration;

namespace Auth.Admin.Api.Config
{
    public static class GlobalConfiguration
    {
        public static Uri AuthorityUri => ConfigurationManager.AppSettings["authority.uri"].ToUri();
        public static Uri AuthAdminRedirectUri => ConfigurationManager.AppSettings["auth.admin.redirect.uri"].ToUri();
        public static string AuthAdminRoute => ConfigurationManager.AppSettings["auth.admin.route"] ?? string.Empty;

        public static bool? RequireSSL => ConfigurationManager.AppSettings["auth.admin.requireSSL"].ToBool();

        private static Uri ToUri(this string uriString)
        {
            Uri result;
            return Uri.TryCreate(uriString, UriKind.Absolute, out result) ? result : null;
        }

        private static bool? ToBool(this string boolString)
        {
            bool result;
            return bool.TryParse(boolString, out result) ? result : (bool?) null;
        }
    }
}