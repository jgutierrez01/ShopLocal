using System;
using MenuItemEnum = SAM.Web.Classes.WebConstants.MenuItemEnum;
using SubMenuItemEnum = SAM.Web.Classes.WebConstants.SubMenuItemEnum;

namespace SAM.Web.Masters
{
    public partial class Calidad : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstableceMenuActivo(MenuItemEnum.Calidad);
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
    }
}