using System;
using System.Security.Cryptography;
using System.Text;

namespace Mimo.Framework.Cryptography
{
    public static class RsaUtils
    {
        /// <summary>
        /// Size in bits of the key to use
        /// </summary>
        private const int KEY_SIZE = 2048;

        /// <summary>
        /// Generates a new random RSA public-private key pair and returns the
        /// XML representation of them.
        /// </summary>
        /// <returns>XML representation of a RSA public-private key pair</returns>
        public static string GetNewRSAKey()
        {
            string xmlResult = string.Empty;

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(KEY_SIZE)) 
            {
                xmlResult = rsa.ToXmlString(true);
                rsa.Clear();
            }

            return xmlResult;
        }


        /// <summary>
        /// Gets only the public part of a RSA public-private key pair.
        /// We receive the XML representation of the RSA key and from it we obtain
        /// the part we need.
        /// </summary>
        /// <param name="xmlRsaKey">XML represantation of a RSA public-private key pair</param>
        /// <returns>The XML representation of ONLY the public part of the key</returns>
        public static string GetPublicKey(string xmlRsaKey)
        {
            string xmlResult = string.Empty;

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(KEY_SIZE))
            {
                rsa.FromXmlString(xmlRsaKey);
                xmlResult = rsa.ToXmlString(false);
                rsa.Clear();
            }

            return xmlResult;            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="hashedData"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static byte [] SignHashedData(byte [] hashedData, string privateKey)
        {
            byte[] signature;

            using ( RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(KEY_SIZE) )
            {
                rsa.FromXmlString(privateKey);

                RSAPKCS1SignatureFormatter formatter = new RSAPKCS1SignatureFormatter(rsa);
                
                formatter.SetHashAlgorithm("SHA1");

                signature = formatter.CreateSignature(hashedData);

                rsa.Clear();
            }

            return signature;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static byte [] SignData(byte [] data, string privateKey)
        {
            return SignHashedData(HashUtils.ComputeHash(data), privateKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalText"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static string SignText(string originalText, string privateKey)
        {
            byte[] signature = SignData(Encoding.UTF8.GetBytes(originalText), privateKey);
            return Convert.ToBase64String(signature);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalText"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static byte [] SignTextAsBytes(string originalText, string privateKey)
        {
            return SignData(Encoding.UTF8.GetBytes(originalText), privateKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hashedData"></param>
        /// <param name="signature"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static bool VerifySignatureFromHashedData(byte [] hashedData, byte [] signature, string publicKey)
        {
            bool validSignature = false;

            using ( RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(KEY_SIZE) )
            {
                rsa.FromXmlString(publicKey);

                RSAPKCS1SignatureDeformatter deformatter = new RSAPKCS1SignatureDeformatter(rsa);

                deformatter.SetHashAlgorithm("SHA1");

                validSignature = deformatter.VerifySignature(hashedData, signature);

                rsa.Clear();
            }

            return validSignature;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="signature"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static bool VerifySignatureFromOriginalData(byte [] data, byte [] signature, string publicKey)
        {
            return VerifySignatureFromHashedData(HashUtils.ComputeHash(data), signature, publicKey);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalText"></param>
        /// <param name="signature"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static bool VerifySignatureFromOriginalText(string originalText, string signature, string publicKey)
        {
            return VerifySignatureFromOriginalData( Encoding.UTF8.GetBytes(originalText), 
                                                    Convert.FromBase64String(signature),
                                                    publicKey);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static string Encrypt(string text, string privateKey)
        {
            byte[] encrypted = null;

            using ( RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(KEY_SIZE) )
            {
                rsa.FromXmlString(privateKey);

                encrypted = rsa.Encrypt(Encoding.UTF8.GetBytes(text), true);

                rsa.Clear();
            }

            return Convert.ToBase64String(encrypted);
        }



        public static string Decrypt(string cryptedText, string publicKey)
        {
            byte[] cryptedBytes = Convert.FromBase64String(cryptedText);
            byte[] decryptedBytes = null;

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(KEY_SIZE))
            {
                rsa.FromXmlString(publicKey);

                decryptedBytes = rsa.Decrypt(cryptedBytes, true);

                rsa.Clear();
            }

            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
