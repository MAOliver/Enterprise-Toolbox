using System;
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using Xunit;

namespace Identity.Tests.xUnit
{
    public class CertificateTests
    {
        [Fact(DisplayName = "Should locate certficate")]
        public void TestFindCertificateByThumbprints()
        {
            Assert.NotNull(TestCert.Load());
        }

        [Fact(DisplayName = "Should validate token by certificate")]
        public void TestValidateCertSucceedsFromString()
        {
            const string token =
                "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Ik53UHZBc181UW1yRTBiMXpfaHlzT2FDMlQyQSIsImtpZCI6Ik53UHZBc181UW1yRTBiMXpfaHlzT2FDMlQyQSJ9.eyJub25jZSI6ImZkMmFiNzk1YTQyZDQ3Yzg5YmRjZjQwMDAwMDM5ZTUwIiwiaWF0IjoxNDQ1Mjc2OTYxLCJzdWIiOiI2MzIyODc2Ni05YTlhLTRkMWYtOTVjNC03ZmY5OTFhYzk2ODIiLCJhbXIiOiJwYXNzd29yZCIsImF1dGhfdGltZSI6MTQ0NTI3NjQyMSwiaWRwIjoiaWRzcnYiLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDMzMy9hdXRoL2lkcyIsImF1ZCI6ImNvZGVjbGllbnQiLCJleHAiOjE0NDUyNzcyNjEsIm5iZiI6MTQ0NTI3Njk2MX0.LMjy5DR1pTxEdOAfTGLSAyEOcSI5sKDjQGwsxbwL13wRyJ6oj9uGKYDnJVxKbUrb7C_GRbCK7xZ74q73xG4hgz77vQ7-XUHiEAIfs4JknfwyNn3ilIGW43umezPG8P71pzejvkJVa7BoMLeOTCzJUH5OSwxJF6JmFAlkDFwxL0xlad7swA8uvllIfi_MiN0kvqneGDj-i7odAennGsR_U-HHfBGblaQxb5FvWrFaje0Iokha0QL7QxdbRU8sImg1j6vk2Wl0vu9mTXX4MHH3wC0Lv0ZFaJtibI6yNJHmSq3gfm-pWy8y3ZbDxMaSdleLybJLcOSCA0QyhWavy4EUMw";
            var certString = Convert.ToBase64String(TestCert.Load().RawData);
            var cert = new X509Certificate2(Convert.FromBase64String(certString));

            var parameters = new TokenValidationParameters
            {
                ValidAudience = "codeclient",
                ValidIssuer = Constants.BaseAddress,
                IssuerSigningToken = new X509SecurityToken(cert)
            };

            SecurityToken jwt;
            var principal = new JwtSecurityTokenHandler().ValidateToken(token, parameters, out jwt);
            Assert.NotNull(principal);
        }

        static class TestCert
        {
            public static X509Certificate2 Load()
            {
                return new X509Certificate2(@".\IISAuthCert.pfx", "11553322");
            }
            
        }
    }
}