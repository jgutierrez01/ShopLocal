using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;

namespace Mimo.Framework.WebControls
{
    [SupportsEventValidation]
    [ToolboxData("<{0}:LanguageDropDown runat=server></{0}:MappableDropDown>")]
    public class LanguageDropDown : DropDownList
    {

        protected override void OnInit(EventArgs e)
        {
            Items.Add(new ListItem("Auto", ""));
            Items.Add(new ListItem("English", LanguageHelper.INGLES));
            Items.Add(new ListItem("Spanish", LanguageHelper.ESPANOL));
            Items.FindByValue(LanguageHelper.CustomCulture).Selected = true;
            AutoPostBack = true;
            base.OnInit(e);
        }


        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            LanguageHelper.CustomCulture = SelectedValue;
            base.OnSelectedIndexChanged(e);
            HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.ToString());
        }
    }
}
