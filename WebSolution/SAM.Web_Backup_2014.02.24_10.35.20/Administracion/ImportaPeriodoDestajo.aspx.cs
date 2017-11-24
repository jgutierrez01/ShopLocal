using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessLogic.Excel;

namespace SAM.Web.Administracion
{
    public partial class ImportaPeriodoDestajo : SamPaginaConSeguridad
    {
        protected override void OnLoad(EventArgs e)
        {
            //todos pueden entrar a esta página siempre y cuando estén loggeados
            RevisarSeguridad = false;
            base.OnLoad(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                byte[] archivo = null;
                string nombreArchivo = string.Empty;

                archivo = ExcelNomina.Instance.ObtenerNominaPorPeriodo(EntityID.Value);
                nombreArchivo = "Nomina.xlsx";
                
                if (archivo != null)
                {
                    string nombreAttachment = string.Format("attachment; filename=\"{0}\"", nombreArchivo);
                    Response.Clear();
                    Response.ContentType = "binary/octet-stream";
                    Response.AddHeader("content-disposition", nombreAttachment);
                    Response.BinaryWrite(archivo);
                }
                else
                {
                    throw new ExcelException("El archivo de Excel que se desea generar no existe");
                }
            }
        }
    }
}