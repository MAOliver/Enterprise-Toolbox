using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

namespace WindowsAuthentication.Api.Configuration
{
    public static class GlobalConfiguration
    {
        public static Uri AuthorityUri => ConfigurationManager.AppSettings["authority.uri"].ToUri();
        public static string WinAdRoute => ConfigurationManager.AppSettings["winad.route"] ?? string.Empty;

        public static bool? IgnoreSsl => ConfigurationManager.AppSettings["dev.ignoreSSL"].ToBool();

        private static Uri ToUri(this string uriString)
        {
            Uri result;
            return Uri.TryCreate(uriString, UriKind.Absolute, out result) ? result : null;
        }

        private static bool? ToBool(this string boolString)
        {
            bool result;
            return bool.TryParse(boolString, out result) ? result : (bool?)null;
        }
    }
}