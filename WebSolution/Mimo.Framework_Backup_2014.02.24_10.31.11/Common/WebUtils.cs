using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace Mimo.Framework.Common
{
    public static class WebUtils
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool IsItem(RepeaterItem e)
        {
            return e != null && (e.ItemType == ListItemType.Item || e.ItemType == ListItemType.AlternatingItem);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringToEncode"></param>
        /// <returns></returns>
        public static string HtmlEncode(string stringToEncode)
        {
            if ( stringToEncode == null )
            {
                return string.Empty;
            }

            return HttpUtility.HtmlEncode(stringToEncode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encodedString"></param>
        /// <returns></returns>
        public static string HtmlDecode(string encodedString)
        {
            if ( encodedString == null )
            {
                return string.Empty;
            }

            return HttpUtility.HtmlDecode(encodedString);
        }

        #region Combo Utilities

        /// <summary>
        /// 
        /// </summary>
        /// <param name="combo"></param>
        /// <param name="ds"></param>
        /// <param name="dataValueField"></param>
        /// <param name="dataTextField"></param>
        public static void BindCombo(ListControl combo,
                                     DataSet ds,
                                     string dataValueField,
                                     string dataTextField)
        {
            combo.DataSource = ds;
            combo.DataValueField = dataValueField;
            combo.DataTextField = dataTextField;
            combo.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="combo"></param>
        /// <param name="ds"></param>
        public static void BindCombo(ListControl combo,
                                     DataSet ds)
        {
            BindCombo(combo,
                      ds,
                      ds.Tables[0].Columns[0].ColumnName,
                      ds.Tables[0].Columns[1].ColumnName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="combo"></param>
        /// <param name="dv"></param>
        /// <param name="dataValueField"></param>
        /// <param name="dataTextField"></param>
        public static void BindCombo(ListControl combo,
                                     DataView dv,
                                     string dataValueField,
                                     string dataTextField)
        {
            combo.DataSource = dv;
            combo.DataValueField = dataValueField;
            combo.DataTextField = dataTextField;
            combo.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="combo"></param>
        /// <param name="dv"></param>
        public static void BindCombo(ListControl combo,
                                     DataView dv)
        {
            BindCombo(combo,
                      dv,
                      dv.Table.Columns[0].ColumnName,
                      dv.Table.Columns[1].ColumnName);
        }

        #endregion

    }
}
