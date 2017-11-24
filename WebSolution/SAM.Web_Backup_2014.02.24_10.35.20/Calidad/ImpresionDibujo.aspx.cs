using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Utilerias;
using SAM.Common;

namespace SAM.Web.Calidad
{
    public partial class ImpresionDibujo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int proyectoID = Request.QueryString["ProyectoID"].SafeIntParse();

            string nombreProyecto = CacheCatalogos.Instance.ObtenerProyectos().Where(x => x.ID == proyectoID).Select(x => x.Nombre).SingleOrDefault();

            string dibujo = Request.QueryString["Dibujo"];

            dibujo = dibujo.Replace(@"""", "");

            string directorio = Configuracion.CalidadRutaDossier + @"\" + nombreProyecto + @"\Reportes\Dibujo" + @"\" + dibujo + ".pdf";

            string nombreAttachment = string.Format("attachment; filename=\"{0}\"", dibujo + ".pdf");

            Response.Clear();
            Response.ContentType = "binary/octet-stream";
            Response.AddHeader("content-disposition", nombreAttachment);
            Response.WriteFile(directorio);
        }
    }
}