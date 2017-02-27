using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Claims;
using System.Security.Principal;
using System.IdentityModel.Tokens;
using System.ServiceModel.Security;
using System.Xml;
using SamlHelperLib;

namespace WindowsIdentityToClaimsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var claims = new WindowsClaimSet(WindowsIdentity.GetCurrent());
            using (claims)
            {
                foreach (var claim in claims)
                {
                    Console.WriteLine(string.Format("Claim Type: {0}", claim.ClaimType));
                    Console.WriteLine(string.Format("Resource: {0}", claim.Resource));
                    Console.WriteLine(string.Format("Right: {0}", claim.Right));
                }

                var accessControlClaims = claims.FindClaims(ClaimTypes.Sid, Rights.PossessProperty);
                var assertion = CreateSamlAssertionFromWindowsIdentityClaims(accessControlClaims);
                var token = new SamlSecurityToken(assertion);
                SerializeSamlTokenToFile(token);
                Console.ReadKey();

                Console.ReadKey();
            }

        }

        private static SamlAssertion CreateSamlAssertionFromWindowsIdentityClaims(IEnumerable<Claim> claims)
        {
            var subject = new SamlSubject()
            {
                Name = "Windows Group Claim"
            };
            var statement = new SamlAttributeStatement()
            {
                SamlSubject = subject
            };
            var assertion = new SamlAssertion()
            {
                AssertionId = string.Format("_{0}", Guid.NewGuid().ToString()),
                Issuer = "System"
            };

            foreach (var claim in claims)
            {
                var samlClaim = new Claim(claim.ClaimType, GetResourceFromSid(claim.Resource as SecurityIdentifier), claim.Right);
                var attribute = new SamlAttribute(samlClaim);
                statement.Attributes.Add(attribute);
            }

            assertion.Statements.Add(statement);
            SignSamlAssertion(assertion);
            return assertion;
        }

        private static string GetResourceFromSid(SecurityIdentifier sid)
        {
            try
            {
                return sid.Translate(typeof(NTAccount)).ToString();
            }
            catch (Exception)
            {
                return sid.Value;
            }
        }

        private static void SignSamlAssertion(SamlAssertion assertion)
        {
            var certificate2 = GetCertificateFromStore(StoreLocation.CurrentUser, DateTime.Now, "CN=SamlTokenSigningCertificate");
            if (certificate2 != null)
            {
                var securityKey = new X509AsymmetricSecurityKey(certificate2);
                assertion.SigningCredentials = new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.RsaSha1Signature,
                    SecurityAlgorithms.Sha1Digest,
                    new SecurityKeyIdentifier(new X509ThumbprintKeyIdentifierClause(certificate2)));
            }
        }

        private static X509Certificate2 GetCertificateFromStore(StoreLocation location, DateTime timeValid, string subjectDistinguishedName)
        {
            var store = new X509Store(location);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                var validCertificates = store.Certificates.Find(X509FindType.FindByTimeValid, timeValid, false);
                var signingCertificate = validCertificates.Find(X509FindType.FindBySubjectDistinguishedName, subjectDistinguishedName, false);
                if (signingCertificate.Count == 0)
                    return null;
                return signingCertificate[0];
            }
            finally
            {
                store.Close();
            }
        }

        private static void SerializeSamlTokenToFile(SamlSecurityToken token)
        {
            using (var sw = new StreamWriter(@"C:\Data\SVN-Client\dialog\Dialog\WindowsIdentityToClaimsConsole\saml.xml"))
            {
                try
                {
                    var xml = Saml11Helper.SerializeSamlToken(token);
                    sw.Write(xml);
                    Console.WriteLine("Saml Token Successfully Written");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to save Saml Token to Disk");
                }
            }
        }
    }
}
