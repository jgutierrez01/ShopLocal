using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MenuItemEnum = SAM.Web.Classes.WebConstants.MenuItemEnum;
using SubMenuItemEnum = SAM.Web.Classes.WebConstants.SubMenuItemEnum;

namespace SAM.Web.Masters
{
    public partial class Administracion : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstableceMenuActivo(MenuItemEnum.Administracion);
            }
        }

        /// <summary>
        /// permite a la pagina que hereda de Administracion establecer el submenu que está activo.
        /// </summary>
        /// <param name="item">pagina activa</param>
        public void EstablecerSubMenuActivo(SubMenuItemEnum item)
        {
            Master.EstablecerSubMenuActivo(item);
        }

        //public void DesacivarMenuYSubmenu()
        //{
        //    Master.EstableceMenuActivo(0);
        //    Master.EstablecerSubMenuActivo(0);
        //}
    }
}