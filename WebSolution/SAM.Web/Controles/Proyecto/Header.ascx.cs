using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.BusinessObjects.Proyectos;
using SAM.BusinessLogic;
using SAM.Entities.Cache;
using System.Drawing;
using SAM.BusinessObjects.Utilerias;

namespace SAM.Web.Controles.Proyecto
{
    public partial class Header : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public void BindInfo(int proyectoID)
        {
            ProyectoCache proyecto = CacheCatalogos.Instance.ObtenerProyectos().Where(x => x.ID == proyectoID).Single();
            WebColorConverter colorConverter = new WebColorConverter();
            
            lblNombreProyecto.Text = proyecto.Nombre;
            lblClienteProyecto.Text = proyecto.NombreCliente;
            lblColorProyecto.Text = proyecto.NombreColor;
            lblPatioProyecto.Text = proyecto.NombrePatio;
            patioID = proyecto.PatioID; // new
            pnlColor.BackColor = (Color)colorConverter.ConvertFromString(proyecto.HexadecimalColor);
            patio = proyecto.PatioID;
        }

        public static int patio;
        public static int patioID;
    }
}