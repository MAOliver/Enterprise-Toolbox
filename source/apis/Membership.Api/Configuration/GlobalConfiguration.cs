using System;
using System.Configuration;

namespace Membership.Api.Configuration
{
    public static class GlobalConfiguration
    {
        public static Uri AuthorityUri => ConfigurationManager.AppSettings["authority.uri"].ToUri();
        public static Uri RedirectUri => ConfigurationManager.AppSettings["redirect.uri"].ToUri();

        public static bool? IgnoreSsl => ConfigurationManager.AppSettings["dev.ignoreSSL"].ToBool();

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