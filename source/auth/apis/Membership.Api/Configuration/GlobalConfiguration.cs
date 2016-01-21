using System;
using System.Configuration;

namespace Membership.Api.Configuration
{
    public static class GlobalConfiguration
    {
        public static Uri AuthorityUri => ConfigurationManager.AppSettings["authority.uri"].ToUri();
        public static Uri RedirectUri => ConfigurationManager.AppSettings["membership.redirect.uri"].ToUri();
        public static string MembershipRoute => ConfigurationManager.AppSettings["membership.route"] ?? string.Empty;

        public static bool? RequireSSL => ConfigurationManager.AppSettings["membership.requireSSL"].ToBool();
        public static Uri PublicOrigin => ConfigurationManager.AppSettings["membership.publicOrigin"].ToUri();

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