using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EncryptDecrypt
{
    public class Program
    {
        static void Main(string[] args)
        {
            //PublicKeyInfrastructure pki = new PublicKeyInfrastructure();
            //Cryptograph crypto = new Cryptograph();
            //RSAParameters privateKey = crypto.GenerateKeys("simlanghoff@gmail.com");
            //const string PlainText = "This is really sent by me, really!";
            //RSAParameters publicKey = crypto.GetPublicKey("simlanghoff@gmail.com");
            //string encryptedText = Cryptograph.Encrypt(PlainText, publicKey);
            //Console.WriteLine("This is the encrypted Text:" + "\n " + encryptedText);
            //string decryptedText = Cryptograph.Decrypt(encryptedText, privateKey);
            //Console.WriteLine("This is the decrypted text: " + decryptedText);
            //string messageToSign = encryptedText;
            //string signedMessage = Cryptograph.SignData(messageToSign, privateKey);
            ////// Is this message really, really, REALLY sent by me?
            //bool success = Cryptograph.VerifyData(messageToSign, signedMessage, publicKey);
            //Console.WriteLine("Is this message really, really, REALLY sent by me? " + success);

            test1();
            // test2();
        }

        private static void test1()
        {
            try
            {
                var cert2full = new X509Certificate2(@"C:\Data\SVN-Client\dialog\Dialog\EncryptDecrypt\SamlTokenSigningCertificate.pfx");
                // Sign text
                var signature = Sign("<?xml version='1.0' encodeing='utf-8'><root></root>", cert2full);
                var cert2 = new X509Certificate2(@"C:\Data\SVN-Client\dialog\Dialog\EncryptDecrypt\SamlTokenSigningCertificate.cer");
                if (Verify("<?xml version='1.0' encodeing='utf-8'><root></root>", signature, cert2))
                {
                    Console.WriteLine("Signature verified");
                }
                else
                {
                    Console.WriteLine("ERROR: Signature not valid!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION: " + ex.Message);
            }
            Console.ReadKey();
        }

        static string Sign(string text, X509Certificate2 cert2)
        {
            var csp = (RSACryptoServiceProvider)cert2.PrivateKey;
            if (csp == null)
            {
                throw new Exception("No valid cert was found");
            }
            // Hash the data
            var sha1 = new SHA1Managed();
            var encoding = new UnicodeEncoding();
            var data = encoding.GetBytes(text);
            var hash = sha1.ComputeHash(data);
            // Sign the hash
            return Convert.ToBase64String(csp.SignHash(hash, CryptoConfig.MapNameToOID("SHA1")));
        }

        static bool Verify(string text, string signature, X509Certificate2 cert2)
        {
            // Get its associated CSP and public key
            var csp = (RSACryptoServiceProvider)cert2.PublicKey.Key;
            // Hash the data
            var sha1 = new SHA1Managed();
            var encoding = new UnicodeEncoding();
            var data = encoding.GetBytes(text);
            var hash = sha1.ComputeHash(data);
            // Verify the signature with the hash
            return csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), Convert.FromBase64String(signature));
        }
    
        private static void test2()
        { 
            var cert2 = new X509Certificate2(@"C:\Data\SVN-Client\dialog\Dialog\EncryptDecrypt\SamlTokenSigningCertificate.pfx", "");
            var str1 = Encrypt("Hola mundo", cert2);
            var str2 = Decrypt(str1, cert2);
            Console.ReadKey();
        }

        private static string Encrypt(string message, X509Certificate2 cert2)
        {
            try
            {
                RSACryptoServiceProvider rsaEncryptor = (RSACryptoServiceProvider)cert2.PrivateKey;
                byte[] cipherData = rsaEncryptor.Encrypt(Encoding.UTF8.GetBytes(message), true);
                return Convert.ToBase64String(cipherData);
            }
            catch (CryptographicException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static string Decrypt(string message, X509Certificate2 cert2)
        {
            try
            {
                RSACryptoServiceProvider rsaEncryptor = (RSACryptoServiceProvider)cert2.PrivateKey;
                byte[] plainData = rsaEncryptor.Decrypt(Convert.FromBase64String(message), true);
                return Encoding.UTF8.GetString(plainData);
            }
            catch (CryptographicException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
