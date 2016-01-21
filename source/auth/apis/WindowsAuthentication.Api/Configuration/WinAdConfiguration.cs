using System.Linq;
using IdentityModel;
using IdentityServer.WindowsAuthentication.Configuration;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;

namespace WindowsAuthentication.Api.Configuration
{
    internal static class WinAdConfiguration
    {
        public static WindowsAuthenticationOptions CreateWindowsAuthenticationOptions()
        {
            return new WindowsAuthenticationOptions
            {
                IdpRealm = "urn:idsrv3",
                IdpReplyUrl = GlobalConfiguration.AuthorityUri.ToString(),
                //EnableWsFederationMetadata = true,
                SigningCertificate = IdentityModel.X509.LocalMachine.My.SubjectDistinguishedName.Find("CN=sts").First()
            };
        }
        
    }
}