using System;
using System.Web;

namespace Mimo.Framework.Common
{
    public class LanguageHelper
    {
        public const string INGLES = "en-US";
        public const string ESPANOL = "es-MX";

        /// <summary>
        /// Gets or sets filtered set value of Culture stored on cookie
        /// returns either en-US, es-MX or blank
        /// </summary>
        public static string CustomCulture
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["Culture"];
                    if (cookie != null)
                    {
                        if (cookie.Value == INGLES || cookie.Value == ESPANOL)
                            return cookie.Value;
                    }
                }
                return ESPANOL;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || (value != INGLES && value != ESPANOL)) return;
                HttpCookie cookie = new HttpCookie("Culture")
                {
                    Value = value,
                    Expires = DateTime.Now.AddYears(1),
                    HttpOnly = true,
                    Path = "/"
                };
                HttpContext.Current.Request.Cookies.Add(cookie);
                HttpContext.Current.Response.SetCookie(cookie);
            }
        }
    }
}
