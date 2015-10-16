using System;
using System.Configuration;

namespace Auth.Api.Configuration
{
    public static class GlobalConfiguration
    {
        public static Uri AuthorityUri => ConfigurationManager.AppSettings["authority.uri"].ToUri();

        public static string AuthorityRoute => ConfigurationManager.AppSettings["authority.route"];

        public static bool? IgnoreSsl => ConfigurationManager.AppSettings["dev.ignoreSSL"].ToBool();

        public static string AuthorityCertificateSubject => ConfigurationManager.AppSettings["authority.certificate.subject"];

        public static string AuthorityCertificatePassword => ConfigurationManager.AppSettings["authority.certificate.password"];

        public static string AuthorityCertificateThumbprint => ConfigurationManager.AppSettings["authority.certificate.thumbprint"];

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