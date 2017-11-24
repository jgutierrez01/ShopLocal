using System;
using System.Security.Cryptography;
using System.Text;

namespace Mimo.Framework.Cryptography
{
    public static class RsaEncrypter
    {
        private const int KEY_SIZE = 2048;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataToEncrypt"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] dataToEncrypt, string publicKey)
        {
            byte[] arr = null;

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(KEY_SIZE))
            {
                rsa.FromXmlString(publicKey);
                arr = rsa.Encrypt(dataToEncrypt, true);
                rsa.Clear();
            }

            return arr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringToEncrypt"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static string Encrypt(string stringToEncrypt, string publicKey)
        {
            byte[] arr = Encoding.UTF8.GetBytes(stringToEncrypt);
            byte[] encrypted = Encrypt(arr, publicKey);
            return Convert.ToBase64String(encrypted);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataToEncrypt"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static string EncryptFromBytesToBase64(byte[] dataToEncrypt, string publicKey)
        {
            byte[] crypted = Encrypt(dataToEncrypt, publicKey);
            return Convert.ToBase64String(crypted);
        }

    }
}
