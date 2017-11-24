namespace Mimo.Framework.Common
{
    public static class TextUtils
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ArrayToCsvString(string [] list)
        {
            return ArrayToCsvString(list, ", ");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ArrayToCsvString(string [] list, string separator)
        {
            if (list != null)
            {
                return string.Join(separator, list);
            }

            return string.Empty;
        }
    }
}
