using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Globalization;
using System.Collections.Generic;

namespace SAM.Dashboard.Utils
{
    public static class ObjectExtensions
    {
        public static bool EqualsIgnoreCase(this string s, string value)
        {
            return string.Equals(s, value, StringComparison.InvariantCultureIgnoreCase);
        }

        public static decimal SafeMoneyParse(this object o)
        {
            decimal defaultReturn = 0;

            if (o != null && !o.Equals(DBNull.Value))
            {
                try
                {
                    defaultReturn = decimal.Parse(o.ToString(), NumberStyles.Currency);
                }
                catch { }
            }

            return defaultReturn;
        }

        public static decimal SafeDecimalParse(this object o)
        {
            decimal defaultReturn = 0;

            if (o != null && !o.Equals(DBNull.Value))
            {
                try
                {
                    defaultReturn = Convert.ToDecimal(o);
                }
                catch { }
            }

            return defaultReturn;
        }

        public static decimal? SafeDecimalNullableParse(this object o)
        {
            decimal? defaultReturn = null;

            if (o != null && !o.Equals(DBNull.Value))
            {
                try
                {
                    defaultReturn = Convert.ToDecimal(o);
                }
                catch { }
            }

            return defaultReturn;
        }

        public static int SafeIntParse(this object o)
        {
            int defaultReturn = -1;

            if (o != null && !o.Equals(DBNull.Value))
            {
                try
                {
                    defaultReturn = Convert.ToInt32(o);
                }
                catch { }
            }

            return defaultReturn;
        }

        public static int SafeIntParse(this string s, int defaultValue)
        {
            int defaultReturn = defaultValue;

            if (!string.IsNullOrEmpty(s))
            {
                if (!int.TryParse(s, out defaultReturn))
                {
                    defaultReturn = defaultValue;
                }
            }

            return defaultReturn;
        }

        public static string SafeStringParse(this object o)
        {
            string defaultReturn = string.Empty;

            if (o != null && !o.Equals(DBNull.Value))
            {
                try
                {
                    defaultReturn = o.ToString();
                }
                catch { }
            }

            return defaultReturn;
        }

        public static bool SafeBoolParse(this object o)
        {
            bool defaultReturn = false;

            if (o != null && !o.Equals(DBNull.Value))
            {
                try
                {
                    defaultReturn = Convert.ToBoolean(o);
                }
                catch { }
            }

            return defaultReturn;
        }

        /// <summary>
        /// Returns the object cast as date to ShortDate if possible, null otherwise
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string SafeDateAsStringParse(this object o)
        {
            string defaultReturn = null;

            if (o != null && !o.Equals(DBNull.Value))
            {
                try
                {
                    defaultReturn = Convert.ToDateTime(o).ToShortDateString();
                }
                catch { }
            }

            return defaultReturn;
        }

        public static TValue SafeTryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            try
            {
                return dictionary[key];
            }
            catch
            {
                return defaultValue;
            }
        }

        public static string SafeTryGetValue(this IDictionary<int, string> dictionary, int key)
        {
            try
            {
                return dictionary[key];
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="original"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ContainsIgnoreCase(this string original, string value)
        {
            return original.IndexOf(value, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }


        private const string DIAMETER_FORMAT = "{0:#0.000}";
        public static string DiameterFormat(this string original)
        {
            return string.Format(DIAMETER_FORMAT, original.Replace(@"""", "").SafeDecimalParse());
        }


        public static Guid SafeGuidParse(this object o)
        {
            Guid defaultReturn = new Guid();

            if (o != null && !o.Equals(DBNull.Value))
            {
                try
                {
                    defaultReturn = new Guid(o.ToString());
                }
                catch { }
            }

            return defaultReturn;
        }

        public static string DiameterFormat(this decimal original)
        {
            return string.Format(DIAMETER_FORMAT, original);
        }
    }
}