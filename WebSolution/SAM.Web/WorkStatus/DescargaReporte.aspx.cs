using System;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Workstatus;
using SAM.Entities;
using SAM.Web.Classes;

namespace SAM.Web.WorkStatus
{
    public partial class DescargaReporte : SamPaginaConSeguridad
    {
        protected override void OnLoad(EventArgs e)
        {
            RevisarSeguridad = false;
            base.OnLoad(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int reporteID  = Request.QueryString["ReporteID"].SafeIntParse();
            int proyectoID = Request.QueryString["ProyectoID"].SafeIntParse();
            string numeroReporte = string.Empty;
            int tipoPrueba = Request.QueryString["TipoPrueba"].SafeIntParse();
            int tipoReporteID = Request.QueryString["TipoReporte"].SafeIntParse();
            byte[] reporte = null;
            TipoReporte tipoReporte = ObtenerTipoReporte(tipoReporteID).Value;

            if (tipoReporte == TipoReporte.ReportePintura)
            {
                int tipoPintura = Request.QueryString["TipoPintura"].SafeIntParse();
                numeroReporte = Request.QueryString["NR"];
                reporte = ReportesBL.Instance.ObtenReportePinturaDeFileSystem(proyectoID, (TipoPinturaEnum)tipoPintura, numeroReporte);
            }
            else
            {

                 reporte = ReportesBL.Instance.ObtenReporteDeFileSystem(proyectoID,
                                                                                tipoReporte,
                                                                                reporteID,
                                                                                tipoPrueba,
                                                                                out numeroReporte);
            }

            EnviaRepPdf(reporte, numeroReporte);
        }

        private void EnviaRepPdf(byte[] reporte, string numeroReporte)
        {
            UtileriasReportes.EnviaReporteComoPdf(this, reporte, numeroReporte);
        }

        public TipoReporte? ObtenerTipoReporte(int tipoReporte)
        {
            if((int)TipoReporte.ReportePND == tipoReporte)
            {
                return TipoReporte.ReportePND;
            }
            else if ((int)TipoReporte.ReporteTT == tipoReporte)
            {
                return TipoReporte.ReporteTT;
            }
            else if((int)TipoReporte.ReporteDimensional == tipoReporte)
            {
                return TipoReporte.ReporteDimensional;
            }
            else if ((int)TipoReporte.RequisicionPintura == tipoReporte)
            {
                return TipoReporte.RequisicionPintura;
            }
            else if ((int)TipoReporte.InspeccionVisual == tipoReporte)
            {
                return TipoReporte.InspeccionVisual;
            }
            else if ((int)TipoReporte.Requisicion == tipoReporte)
            {
                return TipoReporte.Requisicion;
            }
            else if ((int)TipoReporte.ReportePintura == tipoReporte)
            {
                return TipoReporte.ReportePintura;
            }
            else if ((int)TipoReporte.ReporteEspesores == tipoReporte)
            {
                return TipoReporte.ReporteEspesores;
            }
            else if ((int)TipoReporte.RequisicionSpool == tipoReporte)
            {
                return TipoReporte.RequisicionSpool;
            }
            else if ((int)TipoReporte.ReporteSpoolPND == tipoReporte)
            {
                return TipoReporte.ReporteSpoolPND;
            }
            
            return null;
        }
    }
}