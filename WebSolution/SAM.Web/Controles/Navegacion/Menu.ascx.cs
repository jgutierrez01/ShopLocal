using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Common;
using MenuItemEnum = SAM.Web.Classes.WebConstants.MenuItemEnum;
using SAM.Web.Classes;
using SAM.BusinessObjects.Administracion;
using SAM.Entities;

namespace SAM.Web.Controles.Navegacion
{
    public partial class Menu : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            EstablecerIconoDePendientes();   
        }

        private void EstablecerIconoDePendientes()
        {
            List<Pendiente> lst = PendienteBO.Instance.ObtenerPendientesActivosPorUsuarioID(SessionFacade.UserId);

            if (lst.Count == 0)
            {
                hlIconoPendiente.ImageUrl = "~/Imagenes/Iconos/taskReport.png";
            }
        }

        public void EstablecerItemActivo(MenuItemEnum menu)
        {
            EstablecerInactivos();

            switch (menu)
            {
                case MenuItemEnum.Dashboard:
                    pnDashboard.CssClass = "Activo";
                    break;
                case MenuItemEnum.Workstatus:
                    pnWorkstatus.CssClass = "Activo";
                    break;
                case MenuItemEnum.Materiales:
                    pnMateriales.CssClass = "Activo";
                    break;
                case MenuItemEnum.Ingenieria:
                    pnIngenieria.CssClass = "Activo";
                    break;
                case MenuItemEnum.Produccion:
                    pnProduccion.CssClass = "Activo";
                    break;
                case MenuItemEnum.Calidad:
                    pnCalidad.CssClass = "Activo";
                    break;
                case MenuItemEnum.Proyecto:
                    pnProyecto.CssClass = "Activo";
                    break;
                case MenuItemEnum.Catalogos:
                    pnCatalogo.CssClass = "Activo";
                    break;
                case MenuItemEnum.Administracion:
                    pnAdministracion.CssClass = "ElementoEspecialActive";
                    break;
            }
        }

        public void EstablecerInactivos()
        {
            pnDashboard.CssClass = "Elemento";
            pnWorkstatus.CssClass = "Elemento";
            pnMateriales.CssClass = "Elemento";
            pnIngenieria.CssClass = "Elemento";
            pnProduccion.CssClass = "Elemento";
            pnCalidad.CssClass = "Elemento";
            pnProyecto.CssClass = "Elemento";
            pnCatalogo.CssClass = "Elemento";
            pnAdministracion.CssClass = "ElementoEspecial";
        }
    }
}