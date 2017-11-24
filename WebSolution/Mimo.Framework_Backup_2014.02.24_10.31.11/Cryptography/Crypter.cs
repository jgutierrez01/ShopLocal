using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Mimo.Framework.Common;

namespace Mimo.Framework.Cryptography
{
    /// <summary>
    /// Summary description for Crypto
    /// </summary>
    public static class Crypter
    {
        private static RijndaelManaged rijM;

        /// <summary>
        /// Initialize static variables, in this case the encryptor
        /// </summary>
        static Crypter()
        {
            rijM = new RijndaelManaged();

            rijM.KeySize = Config.SecurityKeySize;
            rijM.Key = Config.SecurityKey;
            rijM.IV = Config.SecurityIV;
        }

        /// <summary>
        /// Encrypts a string and safely encodes it for http transmission.
        /// </summary>
        /// <param name="plainText">String to encrypt and encode</param>
        /// <returns>Encrypted and encoded value of the string</returns>
        public static string EncryptAndEncode(string plainText)
        {
            string encryptedValue = Encrypt(plainText);
            return UrlEncoderDecoder.EncodeParameter(encryptedValue);
        }

        /// <summary>
        /// Encrypts a string using Rijndael's algorithm.
        /// Strings that are encrypted using this method should be decrypted using
        /// the Decrypt method in this same class.
        /// </summary>
        /// <param name="plainText">Text to encrypt</param>
        /// <returns>Base 64 string of the encrypted byte array</returns>
        public static string Encrypt(string plainText)
        {
            //UTF8 encoding is used
            UTF8Encoding textConverter = new UTF8Encoding();

            //Convert string to bytes
            byte[] words = textConverter.GetBytes(plainText);

            //Encrypt bytes
            byte[] encrypted = Encrypt(words);

            //Convert to base64
            string base64 = Convert.ToBase64String(encrypted);

            return base64;
        }


        /// <summary>
        /// Encrypts an array of bytes using Rijndael's
        /// algorithm.  Before the array is encrypted 4 bytes are prepended to it.
        /// These 4 bytes contain the original length of the array.
        /// </summary>
        /// <param name="rawBytes">Array of bytes to encrypt</param>
        /// <returns>Encrypted byte array</returns>
        public static byte[] Encrypt(byte[] rawBytes)
        {
            //Create the encryptor based in the key and initialization vector passed
            ICryptoTransform encryptor = rijM.CreateEncryptor();

            //byte that will hold the encrypted information
            byte[] encrypted = null;

            //Create crypter stream
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt,
                                                                 encryptor,
                                                                 CryptoStreamMode.Write))
                {
                    //encrypt the bytes, we write the length of the original 
                    //size in the beginning of the stream
                    byte[] length = BitConverter.GetBytes(rawBytes.Length);
                    csEncrypt.Write(length,
                                    0,
                                    length.Length);
                    csEncrypt.Write(rawBytes,
                                    0,
                                    rawBytes.Length);
                    csEncrypt.FlushFinalBlock();

                    //Obtain encrypted bytes
                    encrypted = msEncrypt.ToArray();

                    //Release resources
                    csEncrypt.Close();
                    msEncrypt.Close();
                    csEncrypt.Dispose();
                    msEncrypt.Dispose();
                }
            }

            return encrypted;
        }

        /// <summary>
        /// Safely decodes and decrypts a previouls http encoded paramter that was encrypted
        /// with this same class.
        /// </summary>
        /// <param name="encodedCrypted">Econded and crypted string</param>
        /// <returns>Decoded and decrypted string</returns>
        public static string DecryptAndDecode(string encodedCrypted)
        {
            string decoded = UrlEncoderDecoder.DecodeParameter(encodedCrypted);

            return Decrypt(decoded);
        }

        /// <summary>
        /// This method is used to decrypt an array of bytes that was previously encrypted
        /// using the method encrypt in this class.
        /// </summary>
        /// <param name="encrypted">Encrypted array of bytes</param>
        /// <returns>Array of bytes decrypted</returns>
        public static byte[] Decrypt(byte[] encrypted)
        {
            //Create the decryptor bases in the key and initialization vector passed
            ICryptoTransform decryptor = rijM.CreateDecryptor();

            //byte that will hold the decrypted array
            byte[] retArr = null;

            //Create the decrypter streams
            using (MemoryStream msDecrypt = new MemoryStream(encrypted))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                                                                 decryptor,
                                                                 CryptoStreamMode.Read))
                {
                    //Decrypt the informatio and store in in "fromEncrypt"
                    //remember that we added 4 bytes at the beginning to indicate
                    //the original length, so here we read them.
                    byte[] length = new byte[4];
                    byte[] fromEncrypt = new byte[encrypted.Length - 4];

                    csDecrypt.Read(length,
                                   0,
                                   4);
                    csDecrypt.Read(fromEncrypt,
                                   0,
                                   fromEncrypt.Length);

                    //Release resources
                    csDecrypt.Close();
                    msDecrypt.Close();
                    csDecrypt.Dispose();
                    msDecrypt.Dispose();

                    //make sure we only return the bytes that were originally
                    //encrypted by removind any escape characters at the end
                    retArr = new byte[BitConverter.ToInt32(length,
                                                           0)];
                    Array.Copy(fromEncrypt,
                               retArr,
                               retArr.Length);
                }
            }

            return retArr;
        }

        /// <summary>
        /// Decrypts a base64 string that was previously encrypted using the encrypt
        /// method in this same class.
        /// </summary>
        /// <param name="encryptedBase64">Base64 representation of an encrypted string</param>
        /// <returns>Original plain text that was encrypted</returns>
        public static string Decrypt(string encryptedBase64)
        {
            //UTF8 decoding is used
            UTF8Encoding textConverter = new UTF8Encoding();

            //First obtain the encrypted byte array by transforming from base 64
            byte[] encrypted = Convert.FromBase64String(encryptedBase64);

            //Decrypt
            byte[] decrypted = Decrypt(encrypted);

            //Convert to plainText
            string plainText = textConverter.GetString(decrypted);

            return plainText;
        }
    }
}
