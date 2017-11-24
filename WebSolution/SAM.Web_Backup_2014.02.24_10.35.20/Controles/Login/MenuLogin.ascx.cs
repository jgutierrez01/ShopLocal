using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAM.Web.Controles.Login
{
    public partial class MenuLogin : System.Web.UI.UserControl
    {
        public enum MenuItems
        {
            Login = 1,
            Recupera = 2,
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public void EstablecerItemActivo(MenuItems menu)
        {
            EstablecerInactivos();

            switch (menu)
            {
                case MenuItems.Login:
                    pnLogin.CssClass = "Activo";
                    break;
                case MenuItems.Recupera:
                    pnRecuperar.CssClass = "Activo";
                    break;
            }
        }

        public void EstablecerInactivos()
        {
            pnLogin.CssClass = "Elemento";
            pnRecuperar.CssClass = "Elemento";
        }

    }
}