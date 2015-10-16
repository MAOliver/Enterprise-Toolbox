using System.Security.Cryptography.X509Certificates;
using Xunit;
using static Xunit.Assert;

namespace Identity.Tests.xUnit
{
    public class CertificateTests
    {
        [Fact(DisplayName = "Should locate certficate")]
        public void TestFindCertificateByThumbprints()
        {
            NotNull(TestCert.Load());
        }

        static class TestCert
        {
            public static X509Certificate2 Load()
            {
                return new X509Certificate2(@".\IISAuthCert.pfx", "11553322");
            }
            
        }
    }

    public class ClientTests
    {
        [Fact(DisplayName = "Should add client")]
        public void TestAddClient()
        {
           
            
        }
    }


}
