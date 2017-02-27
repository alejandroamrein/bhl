using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel.Security;
using System.Text;
using System.Xml;

namespace SamlHelperLib
{
    public static class Saml11Helper
    {

        public static SamlAssertion CreateSamlAssertionFromUserNameClaims(IEnumerable<Claim> claims)
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
            var certificate2 = GetCertificateFromStore(System.Configuration.ConfigurationManager.AppSettings["TokenSigningCeritificate"]);
            var securityKey = new X509AsymmetricSecurityKey(certificate2);
            assertion.SigningCredentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.RsaSha1Signature,
                SecurityAlgorithms.Sha1Digest,
                new SecurityKeyIdentifier(new X509ThumbprintKeyIdentifierClause(certificate2)));
        }

        private static X509Certificate2 GetCertificateFromStore(string certName)
        {
            var store = new X509Store(StoreLocation.CurrentUser);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                var certCollection = store.Certificates;
                var currentCerts = certCollection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                var signingCert = currentCerts.Find(X509FindType.FindBySubjectDistinguishedName, certName, false);
                if (signingCert.Count == 0)
                    return null;
                return signingCert[0];
            }
            finally
            {
                store.Close();
            }
        }

        public static string SerializeSamlToken(SamlSecurityToken token)
        {
            var samlBuilder = new StringBuilder();
            using (var writer = XmlWriter.Create(samlBuilder))
            {
                try
                {
                    var keyInfoSerializer = new WSSecurityTokenSerializer();
                    keyInfoSerializer.WriteToken(writer, token);
                    Console.WriteLine("Saml Token Successfully Created");
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to seralize token");
                }
            }
            return samlBuilder.ToString();
        }
    }
}
