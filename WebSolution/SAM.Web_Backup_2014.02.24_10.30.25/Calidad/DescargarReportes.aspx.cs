using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Calidad;
using SAM.Entities;
using SAM.Entities.Grid;
using SAM.Web.Classes;

namespace SAM.Web.Calidad
{
    public partial class DescargarReportes : SamPaginaConSeguridad
    {

        private static int ProyectoID
        {
            get
            {
                return HttpContext.Current.Request.Params["PID"].SafeIntParse();
            }
        }


        private static string TipoReporteID
        {
            get
            {
                return HttpContext.Current.Request.Params["RID"];
            }
        }

        private static int SpoolID
        {
            get
            {
                return HttpContext.Current.Request.Params["ID"].SafeIntParse();
            }
        }

                
        protected override void OnLoad(EventArgs e)
        {
            //todos pueden entrar a esta página siempre y cuando estén loggeados
            RevisarSeguridad = false;
            base.OnLoad(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            List<GrdCertificacion> listado = CertificacionBL.Instance.ObtenerParaListadoCertificacion(ProyectoID, new string[7]);
            GrdCertificacion spool = listado.Where(x => x.SpoolID == SpoolID).Single();
            TipoReporte tipoReporte ;
            Enum.TryParse(TipoReporteID, true, out tipoReporte);

            CertificacionBL.Instance.ObtenCiertosPdfsSpool(spool, tipoReporte);
        }
    }
}