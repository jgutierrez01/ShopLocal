using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.MobileControls;
using SAM.Mobile.Clases;
using SAM.Entities;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Produccion;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;
using SAM.Entities.Personalizadas;
using SAM.BusinessObjects.Workstatus;
using SAM.BusinessObjects.Catalogos;
using SAM.Mobile.Paginas.App_LocalResources;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Cache;
using SAM.Entities.Grid;
using System.Data;

namespace SAM.Mobile.Paginas
{
    public partial class DetalleSeguimientoSpool : PaginaMovilAutenticado
    {
        #region Propiedades
        Spool _spool;
        OrdenTrabajoSpool _ots;
        WorkstatusSpool _wsspool;
        #endregion

       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (EntityID != null)
                {
                    Carga();
                }
            }
        }

        protected void cmdOK_OnClick(object sender, EventArgs e)
        {
            Response.Redirect(WebConstants.MobileUrl.DASHBOARD);
        }

        protected void Carga()
        {
            _spool = SpoolBO.Instance.Obtener(EntityID.Value);
            _ots = OrdenTrabajoSpoolBO.Instance.ObtenerOrdenTrabajoPorSpoolID(EntityID.Value);
            _wsspool = SeguimientoSpoolBO.Instance.ObtenerWorkstatusSpoolPorOrdenDeTrabajoSpoolID(_ots.OrdenTrabajoSpoolID);
            
            lblSpool2.Text = _spool.Nombre;
            lblRqPWHT2.Text = TraductorEnumeraciones.TextoSiNo(_spool.RequierePwht);
            CargaHolds();
            CargaPruebasNoDestructivas();

            if (_ots != null)
            {
                lblNoOdt2.Text = _ots.OrdenTrabajo.NumeroOrden;
                lblNoControl2.Text = _ots.NumeroControl;  
                CargaGridJuntas();           
            }

            if (_wsspool != null)
            {
                
                CargaReporteDimensional();
                CargaDetallePintura();
                CargaEmbarque();
                
            }
        }

        protected void CargaGridJuntas()
        {
            List<GrdMobileDetalleSeguimientoSpool> grdJuntas = SeguimientoSpoolBO.Instance.ObtenerDetalleJuntas(EntityID.Value);
            oblstJuntas.DataSource = grdJuntas;
            oblstJuntas.DataBind();
            oblstJuntas.LabelStyle = this.StyleSheet1["TableCells"];
        }

        protected void CargaHolds()
        {
            SpoolHold spoolHold = SeguimientoSpoolBO.Instance.ObtenerHoldPorSpoolID(EntityID.Value);
            if (spoolHold != null)
            {
                lblHldCalidad.Text = TraductorEnumeraciones.TextoSiNo(spoolHold.TieneHoldCalidad);
                lblHldIngenieria.Text = TraductorEnumeraciones.TextoSiNo(spoolHold.TieneHoldIngenieria);
                lblConfinado2.Text = TraductorEnumeraciones.TextoSiNo(spoolHold.Confinado);
            }
            else
            {
                lblHldCalidad.Text = TraductorEnumeraciones.TextoSiNo(false);
                lblHldIngenieria.Text = TraductorEnumeraciones.TextoSiNo(false);
                lblConfinado2.Text = TraductorEnumeraciones.TextoSiNo(false);
            }
        }

        protected void CargaReporteDimensional()
        {
            GrdMobileDetalleLiberaciónDimensional infoReporte = SeguimientoSpoolBO.Instance.ObtenerReporteDimensional(_wsspool.WorkstatusSpoolID);
            if (infoReporte != null)
            {
                lblFechaLiberacion2.Text = infoReporte.FechaLiberacion.SafeDateAsStringParse();
                lblFechaReporte2.Text = infoReporte.FechaReporte.SafeDateAsStringParse();
                lblNumeroReporte2.Text = infoReporte.NumeroReporte;
            }
        }

        protected void CargaDetallePintura()
        {
            RequisicionPintura infoPintura = SeguimientoSpoolBO.Instance.ObtenerRequisicionPintura(_wsspool.WorkstatusSpoolID);
            if (infoPintura != null)
            {
                lblFechaRequisicion2.Text = infoPintura.FechaRequisicion.SafeDateAsStringParse();
                lblNumeroRequisicion2.Text = infoPintura.NumeroRequisicion;
            }

            lblCodigoPintura2.Text = _spool.CodigoPintura;
            lblSistemaPintura2.Text = _spool.SistemaPintura;
            lblColorPintura2.Text = _spool.ColorPintura;

            PinturaSpool spoolPintura = SeguimientoSpoolBO.Instance.ObtenerPinturaSpool(_wsspool.WorkstatusSpoolID);
            if (spoolPintura != null)
            {
                lblFechaSandBlast.Text = spoolPintura.FechaSandblast.SafeDateAsStringParse();
                lblReporteSandBlast.Text = spoolPintura.ReporteSandblast;

                lblFechaPrimarios.Text = spoolPintura.FechaPrimarios.SafeDateAsStringParse();
                lblReportePrimarios.Text = spoolPintura.ReportePrimarios;

                lblFechaIntermedios.Text = spoolPintura.FechaIntermedios.SafeDateAsStringParse();
                lblReporteIntermedios.Text = spoolPintura.ReporteIntermedios;

                lblFechaAcabadoVisual.Text = spoolPintura.FechaAcabadoVisual.SafeDateAsStringParse();
                lblReporteAcabadoVisual.Text = spoolPintura.ReporteAcabadoVisual;

                lblFechaAdherencia.Text = spoolPintura.FechaAdherencia.SafeDateAsStringParse();
                lblReporteAdherencia.Text = spoolPintura.ReporteAdherencia;

                lblFechaPullOff.Text = spoolPintura.FechaPullOff.SafeDateAsStringParse();
                lblReportePullOff.Text = spoolPintura.ReportePullOff;
            }
        }

        protected void CargaEmbarque()
        {
            Embarque embarqueSpool = SeguimientoSpoolBO.Instance.ObtenerEmbarqueParaSpool(_wsspool.WorkstatusSpoolID);
            if (embarqueSpool != null)
            {
                lblNumeroEmbarque2.Text = embarqueSpool.NumeroEmbarque;
                lblFechaEmbarque2.Text = embarqueSpool.FechaEmbarque.SafeDateAsStringParse();
            }

            lblFechaPreparacionEmbarque2.Text = _wsspool.FechaPreparacion.SafeDateAsStringParse();
            lblEtiquetaEmbarque2.Text = _wsspool.NumeroEtiqueta;
            lblFechaEtiquetaEmbarque2.Text = _wsspool.FechaEtiqueta.SafeDateAsStringParse();
        }

        protected void CargaPruebasNoDestructivas()
        {
            DataSet ds = SeguimientoSpoolBO.Instance.ObtenerPruebasNoDestructivasPorSpoolID(EntityID.Value);
            lblPctPND2.Text = ds.Tables[0].Rows[0]["PorcantajePnd"].SafeIntParse() + "%";
            lblRadiadas2.Text = ds.Tables[0].Rows[0]["NumJuntasRTAprobado"].ToString();
            lblLiquidos2.Text = ds.Tables[0].Rows[0]["NumJuntasPTAprobado"].ToString();
            lblRadiadasFaltantes2.Text = ds.Tables[0].Rows[0]["FaltantesRT"].ToString();
            lblLiquidosFaltantes2.Text = ds.Tables[0].Rows[0]["FaltantesPT"].ToString();
        }
    }
}