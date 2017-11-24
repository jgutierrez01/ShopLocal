using System.Text;
using System.Web;

namespace Mimo.Framework.Common
{
    /// <summary>
    /// Uses URL encoding/decoding to safely pass query string parameters
    /// </summary>
    public static class UrlEncoderDecoder
    {
        /// <summary>
        /// Safely encodes a string into another string that can be passed
        /// in QS and posts without problems.
        /// </summary>
        /// <param name="originalValue">Original string to transform</param>
        /// <returns>Safely encoded value</returns>
        public static string EncodeParameter(string originalValue)
        {
            if ( string.IsNullOrEmpty(originalValue) )
            {
                return string.Empty;
            }

            //use http encode
            return HttpUtility.UrlEncode(originalValue, Encoding.UTF8);
        }

        /// <summary>
        /// Safely decodes a string from an http post or get to obtain its original
        /// valu.
        /// </summary>
        /// <param name="encodedValue">Encoded value</param>
        /// <returns>Original string decoded</returns>
        public static string DecodeParameter(string encodedValue)
        {
            if ( string.IsNullOrEmpty(encodedValue) )
            {
                return string.Empty;    
            }

            return HttpUtility.UrlDecode(encodedValue, Encoding.UTF8);
        }
    }
}
