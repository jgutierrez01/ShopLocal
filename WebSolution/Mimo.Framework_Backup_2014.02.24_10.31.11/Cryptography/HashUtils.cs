using System.Security.Cryptography;

namespace Mimo.Framework.Cryptography
{
    public static class HashUtils
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] ComputeHash(byte [] data)
        {
            byte[] hash;

            using(SHA1Managed sha = new SHA1Managed())
            {
                hash = sha.ComputeHash(data);
                sha.Clear();
            }

            return hash;
        }

    }
}
