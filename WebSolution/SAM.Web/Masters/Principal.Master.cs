using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;
using Mimo.Framework.Extensions;
using MenuItemEnum = SAM.Web.Classes.WebConstants.MenuItemEnum;
using SubMenuItemEnum = SAM.Web.Classes.WebConstants.SubMenuItemEnum;

namespace SAM.Web.Masters
{
    public partial class Principal : System.Web.UI.MasterPage
    {
        /// <summary>
        /// permite al master que hereda del principal
        /// establecer el menu que está activo, y tambien
        /// oculta los paneles de submenus que no deberán de mostrarse
        /// y muestra el panel necesario dependiendo del que recibe.
        /// </summary>
        /// <param name="item">master que se está utilizando</param>
        public void EstableceMenuActivo(MenuItemEnum tab)
        {
            menu.EstablecerItemActivo(tab);
            subMenu.EstablecerPanelActivo(tab);
        }

        /// <summary>
        /// recibe la página que esta siendo cargada y establece el 
        /// estilo correcto para esa opcion del submenu
        /// </summary>
        /// <param name="item">pagina activa</param>
        public void EstablecerSubMenuActivo(SubMenuItemEnum subTab)
        {
            subMenu.EstableceSubItemActivo(subTab);
        }
        
        /// <summary>
        /// recibe la página que esta siendo cargada y establece el 
        /// estilo correcto para esa opcion del submenu
        /// RECIBE TAMBIEN el identificador del proyecto que será despues concatenado al url del proyecto
        /// </summary>
        /// <param name="item">pagina activa</param>
        public void EstablecerSubMenuActivo(SubMenuItemEnum subTab, int proyID)
        {
            subMenu.EstableceSubItemActivo(subTab, proyID);
        }

        /// <summary>
        /// 
        /// </summary>
        public void QuitaLigasSubmenuProyecto()
        {
            subMenu.QuitaLigasSubmenuProyecto();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.UserAgent != null)
            {
                userAgentDependant.Visible = Request.UserAgent.IndexOf("MSIE 7") != -1;
            }
            if (!IsPostBack)
            {
                HttpCookie cookie = Request.Cookies["Sam.Width"];

                if (cookie == null)
                {
                    cookie = new HttpCookie("Sam.Width");
                    cookie.HttpOnly = false;
                    cookie.Path = "/";
                    cookie.Value = string.Empty;
                    Request.Cookies.Add(cookie);
                    Response.Cookies.Add(cookie);
                }

                cookie = Request.Cookies["Culture"];
                if (cookie == null)
                {
                    cookie = new HttpCookie("Culture")
                                 {
                                     HttpOnly = false,
                                     Path = "/",
                                     Value = LanguageHelper.CustomCulture
                                 };
                }
                Request.Cookies.Add(cookie);
                Response.Cookies.Add(cookie);


                //Comportamiento default denuestras ventanas
                wndMgr.Windows[0].Behaviors = Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize | Telerik.Web.UI.WindowBehaviors.Reload;

                
            }

            estableceAncho();
        }

        private void estableceAncho()
        {
            HttpCookie cookie = Request.Cookies["Sam.Width"];
            int width = 910;
            if (cookie != null && int.TryParse(cookie.Value, out width))
            {
                autoWrapper.Attributes["style"] = string.Format("width:{0}px;", width < 910 ? 910 : width);
            }
            else
            {
                autoWrapper.Attributes.Remove("style");
            }
        }
    }
}