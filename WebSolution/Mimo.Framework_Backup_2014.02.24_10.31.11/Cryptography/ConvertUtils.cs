using System;
using System.Text;

namespace Mimo.Framework.Cryptography
{
    public static class ConvertUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static string Base64ToUTF8String(string base64String)
        {
            byte[] arr = Convert.FromBase64String(base64String);
            return Encoding.UTF8.GetString(arr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="utf8String"></param>
        /// <returns></returns>
        public static string UTF8StringToBase64(string utf8String)
        {
            byte[] arr = Encoding.UTF8.GetBytes(utf8String);
            return Convert.ToBase64String(arr);
        }
    }
}
