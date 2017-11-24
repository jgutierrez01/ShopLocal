using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using System.Configuration;
using SAM.BusinessObjects.Ingenieria;
using Mimo.Framework.Common;
using Mimo.Framework.Extensions;
using System.IO;
using SAM.Common;

namespace SAM.Web.Ingenieria
{
    public partial class AbrirExcelHistoricoWorkStatus : SamPaginaConSeguridad
    {
        protected override void OnLoad(EventArgs e)
        {
            //todos pueden entrar a esta página siempre y cuando estén loggeados
            RevisarSeguridad = false;
            base.OnLoad(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string rutaArchivo = string.Empty;
            string directorio = Configuracion.DBWorkStatusReports;            
            int historicoWorkStatusID = Request.QueryString["ID"].SafeIntParse();
            bool isSpool = Request.QueryString["IsSpool"].SafeBoolParse();
            string nombreArchivo = SpoolBO.Instance.ObtenerNombreReporteWorkStatus(historicoWorkStatusID, isSpool);
            string nombreAttachment = string.Empty;

            if (Cultura == LanguageHelper.INGLES)
            {
                rutaArchivo = string.Concat(directorio, Path.DirectorySeparatorChar, nombreArchivo, "_en-US.xlsx");
                nombreAttachment = string.Format("attachment; filename=\"{0}\"", nombreArchivo + "_en-US.xlsx");
            }
            else
            {
                rutaArchivo = string.Concat(directorio, Path.DirectorySeparatorChar, nombreArchivo, ".xlsx");
                nombreAttachment = string.Format("attachment; filename=\"{0}\"", nombreArchivo + ".xlsx");
            }

            Response.Clear();
            Response.ContentType = "binary/octet-stream";
            Response.AddHeader("content-disposition", nombreAttachment);
            Response.WriteFile(rutaArchivo);
        }
    }
}