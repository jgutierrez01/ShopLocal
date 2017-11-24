using System;
using System.Security.Cryptography;
using System.Text;

namespace Mimo.Framework.Cryptography
{
    public static class RsaDecrypter
    {
        private const int KEY_SIZE = 2048;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataToDecrypt"></param>
        /// <returns></returns>
        /// <param name="privateKey"></param>
        public static byte[] Decrypt(byte[] dataToDecrypt, string privateKey)
        {
            byte[] decrypted = null;

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(KEY_SIZE))
            {
                rsa.FromXmlString(privateKey);
                decrypted = rsa.Decrypt(dataToDecrypt, true);
                rsa.Clear();
            }

            return decrypted;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringToDecrypt"></param>
        /// <returns></returns>
        /// <param name="privateKey"></param>
        public static string Decrypt(string stringToDecrypt, string privateKey)
        {
            byte[] arr = Convert.FromBase64String(stringToDecrypt);
            byte[] decrypted = Decrypt(arr, privateKey);
            return Encoding.UTF8.GetString(decrypted);
        }

        /// <summary>
        /// Decrypts a string and returns its byte representation.  This method assumes that
        /// the string to decrypt is in base 64.  It first obtains the 
        /// </summary>
        /// <param name="stringToDecrypt"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static byte[] DecryptFromBase64ToBytes(string stringToDecrypt, string privateKey)
        {
            byte[] arr = Convert.FromBase64String(stringToDecrypt);
            return Decrypt(arr, privateKey);
        }
    }
}
