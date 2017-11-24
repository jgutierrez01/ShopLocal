using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Mimo.Framework.Cryptography
{
    /// <summary>
    /// The purpose of this class is to expose some convenience methods that can be used
    /// to encrypt/decrypt using Rijndael algorithm easily.
    /// </summary>
    public static class RijndaelImpl
    {
        /// <summary>
        /// Encrypts a string using the passed key and initialization vector.
        /// Strings that are encrypted using this method should be decrypted using
        /// the Decrypt method in this same class.
        /// </summary>
        /// <param name="key">Key that the symmetric algorith uses</param>
        /// <param name="iv">Initialization vector of the symmetric algorithm</param>
        /// <param name="plainText">Text to encrypt</param>
        /// <returns>Base 64 string of the encrypted byte array</returns>
        public static string Encrypt(byte [] key, byte [] iv, string plainText)
        {
            //UTF8 encoding is used
            UTF8Encoding textConverter = new UTF8Encoding();

            //Convert string to bytes
            byte[] words = textConverter.GetBytes(plainText);

            //Encrypt bytes
            byte[] encrypted = Encrypt(key, iv, words);

            //Convert to base64
            string base64 = Convert.ToBase64String(encrypted);

            return base64;
        }


        /// <summary>
        /// Encrypts an array of bytes using the passed key and IV using Rijndael's
        /// algorithm.  Before the array is encrypted 4 bytes are prepended to it.
        /// These 4 bytes contain the original length of the array.
        /// </summary>
        /// <param name="key">Key that the symmetric algorith uses</param>
        /// <param name="iv">Initialization vector of the symmetric algorithm</param>
        /// <param name="rawBytes">Array of bytes to encrypt</param>
        /// <returns>Encrypted byte array</returns>
        public static byte [] Encrypt(byte [] key, byte [] iv, byte [] rawBytes)
        {
            //Managed algorithm used to encrypt
            RijndaelManaged rijM = new RijndaelManaged();

            //Create the encryptor based in the key and initialization vector passed
            ICryptoTransform encryptor = rijM.CreateEncryptor(key, iv);

            //byte that will hold the encrypted information
            byte[] encrypted = null;

            //Create crypter stream
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    //encrypt the bytes, we write the length of the original 
                    //size in the beginning of the stream
                    byte[] length = BitConverter.GetBytes(rawBytes.Length);
                    csEncrypt.Write(length, 0, length.Length);
                    csEncrypt.Write(rawBytes, 0, rawBytes.Length);
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

            rijM.Clear();
            rijM = null;
            
            return encrypted;            
        }


        /// <summary>
        /// This method is used to decrypt an array of bytes that was previously encrypted
        /// using the method encrypt in this class.  You should pass the same key and
        /// initialization vector that was used to encrypt in order to decrypt.
        /// </summary>
        /// <param name="key">Key that the symmetric algorithm will use (should be the same key that was used to encrypt)</param>
        /// <param name="iv">Initialization Vector that the symmetric algorithm will use (should be the same IV that was used to encrypt)</param>
        /// <param name="encrypted">Encrypted array of bytes</param>
        /// <returns>Array of bytes decrypted</returns>
        public static byte [] Decrypt(byte[] key, byte[] iv, byte [] encrypted)
        {
            //Managed algorithm implementation
            RijndaelManaged rijM = new RijndaelManaged();

            //Create the decryptor bases in the key and initialization vector passed
            ICryptoTransform decryptor = rijM.CreateDecryptor(key, iv);

            //byte that will hold the decrypted array
            byte[] retArr = null;

            //Create the decrypter streams
            using(MemoryStream msDecrypt = new MemoryStream(encrypted))
            {
                using(CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    //Decrypt the informatio and store in in "fromEncrypt"
                    //remember that we added 4 bytes at the beginning to indicate
                    //the original length, so here we read them.
                    byte[] length = new byte[4];
                    byte[] fromEncrypt = new byte[encrypted.Length - 4];

                    csDecrypt.Read(length, 0, 4);
                    csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

                    //Release resources
                    csDecrypt.Close();
                    msDecrypt.Close();
                    csDecrypt.Dispose();
                    msDecrypt.Dispose();

                    //make sure we only return the bytes that were originally
                    //encrypted by removind any escape characters at the end
                    retArr = new byte[BitConverter.ToInt32(length, 0)];
                    Array.Copy(fromEncrypt, retArr, retArr.Length);
                }
            }
            
            rijM.Clear();
            rijM = null;

            return retArr;
        }

        /// <summary>
        /// Decrypts a base64 string that was previously encrypted using the encrypt
        /// method in this same class.  In order to be able to decryp the string the
        /// same key and IV vector used to encrypt should be passed to this
        /// method. 
        /// </summary>
        /// <param name="key">Key that the symmetric algorithm will use (should be the same key that was used to encrypt)</param>
        /// <param name="iv">Initialization Vector that the symmetric algorithm will use (should be the same IV that was used to encrypt)</param>
        /// <param name="encryptedBase64">Base64 representation of an encrypted string</param>
        /// <returns>Original plain text that was encrypted</returns>
        public static string Decrypt(byte [] key, byte [] iv, string encryptedBase64)
        {
            //UTF8 decoding is used
            UTF8Encoding textConverter = new UTF8Encoding();

            //First obtain the encrypted byte array by transforming from base 64
            byte[] encrypted = Convert.FromBase64String(encryptedBase64);

            //Decrypt
            byte [] decrypted = Decrypt(key, iv, encrypted);

            //Convert to plainText
            string plainText = textConverter.GetString(decrypted);

            return plainText;
        }

        /// <summary>
        /// Creates a new random symmetric key and initialization vector that
        /// can be used for any sequence of encryptions/decryptions.
        /// </summary>
        /// <param name="keySize">Size in bits of the key to generate</param>
        /// <returns>Wrapper class object that contains the key and iv of the symmetric algorithm</returns>
        public static RijndaelParameters CreateNewRandomSymmetricKey(int keySize)
        {
            RijndaelManaged rijM = new RijndaelManaged();

            //Generate new random key and initialization vector
            rijM.KeySize = keySize;
            rijM.GenerateKey();
            rijM.GenerateIV();

            //Store generated information in a Wrapper
            //information is cloned to be able to dispose
            //the rijndael object quickly.
            RijndaelParameters symPars = new RijndaelParameters();
            symPars.Key = (byte [])rijM.Key.Clone();
            symPars.IV = (byte [])rijM.IV.Clone();

            //release resources and indicate garbage collector
            //this is ready to be disposed
            rijM.Clear();
            rijM = null;

            return symPars;
        }

        /// <summary>
        /// Tests encrypting and decrypting the same string and see if we obtain
        /// the same.
        /// </summary>
        /// <param name="keySize">Size in bits of the key to generate</param>
        /// <param name="str">String to encrypt/decrypt</param>
        public static bool Test(string str, int keySize)
        {
            RijndaelParameters symPars = CreateNewRandomSymmetricKey(keySize);

            string crypted = Encrypt(symPars.Key, symPars.IV, str);
            string decrypted = Decrypt(symPars.Key, symPars.IV, crypted);

            Console.WriteLine("Encrypted: " + crypted);
            Console.WriteLine("Decrypted: " + decrypted);

            return str == decrypted;
        }
    }
}
