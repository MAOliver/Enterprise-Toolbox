using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace IdentityApi.Configuration
{
    public static class GlobalConfiguration
    {
        public static Uri AuthorityUri => ConfigurationManager.AppSettings["authority.uri"].ToUri();

        public static bool? IgnoreSsl => ConfigurationManager.AppSettings["ignoreSSL"].ToBool();

        public static string AuthorityCertificateSubject => ConfigurationManager.AppSettings["authority.certificate.subject"];

        public static string AuthorityCertificatePassword => ConfigurationManager.AppSettings["authority.certificate.password"];

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