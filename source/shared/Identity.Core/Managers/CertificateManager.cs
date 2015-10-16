using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using IdentityModel;
using Serilog;

namespace Identity.Core.Managers
{
    public class CertificateManager
    {
        public X509Certificate2 GetCert( string thumbprint )
        {
            return X509.LocalMachine.My.Thumbprint.Find(thumbprint).First();
        }

        public X509Certificate2 GetTestCert()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IISAuthCert.pfx");
            Log.Information(path);
            return new X509Certificate2(path, "11553322");
        }
    }
}