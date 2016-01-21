using System;
using System.Configuration;

namespace Auth.Api.Configuration
{
    public static class GlobalConfiguration
    {
        public static string AuthorityRoute => ConfigurationManager.AppSettings["authority.route"] ?? string.Empty;
        public static Uri WinAdRedirectUri => ConfigurationManager.AppSettings["winad.redirect.uri"].ToUri();
        public static Uri WinAdMetadataUri => ConfigurationManager.AppSettings["winad.metadata.uri"].ToUri();

        public static bool? RequireSSL => ConfigurationManager.AppSettings["authority.requireSSL"].ToBool();
        public static Uri PublicOrigin => ConfigurationManager.AppSettings["authority.publicOrigin"].ToUri();
        
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