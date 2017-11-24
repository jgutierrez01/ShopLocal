using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.Entities.Grid;
using SAM.BusinessLogic.Calidad;
using SAM.Entities;
using Mimo.Framework.Common;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Proyectos;
using System.Data;
using Mimo.Framework.WebControls;
using System.Collections;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using log4net;

namespace SAM.Web.Calidad
{
    public partial class CertificacionLigero : SamPaginaPrincipal
    {
        private string _direccionOrdenamientoDefault = "ASC";
        private string _columnaOrdenamientoDefault = "NumeroControl";
        private static readonly ILog _logger = LogManager.GetLogger(typeof(CertificacionLigero));

        private string DireccionOrdenamiento
        {
            get
            {
                if (ViewState["DireccionOrdenamiento"] != null)
                {
                    return ViewState["DireccionOrdenamiento"].ToString();
                }

                return _direccionOrdenamientoDefault;
            }
            set
            {
                ViewState["DireccionOrdenamiento"] = value;
            }
        }

        private string ColumnaOrdenamiento
        {
            get
            {
                if (ViewState["ColumnaOrdenamiento"] != null)
                {
                    return ViewState["ColumnaOrdenamiento"].ToString();
                }

                return _columnaOrdenamientoDefault;
            }
            set
            {
                ViewState["ColumnaOrdenamiento"] = value;
            }
        }

        private int ProyectoID
        {
            get
            {
                if (ViewState["ProyectoID"] != null)
                    return ViewState["ProyectoID"].SafeIntParse();
                return -1;
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }
        }

        /// <summary>
        /// ID de la orden de trabajo seleccionada
        /// </summary>
        private int _ordenTrabajo
        {
            get
            {
                if (ViewState["ordenTrabajo"] != null)
                    return (int)ViewState["ordenTrabajo"].SafeIntParse();
                return -1;
            }
            set
            {
                ViewState["ordenTrabajo"] = value;
            }
        }

        /// <summary>
        /// ID del número de control seleccionado
        /// </summary>
        private int _numeroControl
        {
            get
            {
                if (ViewState["numeroControl"] != null)
                    return (int)ViewState["numeroControl"].SafeIntParse();
                return -1;
            }
            set
            {
                ViewState["numeroControl"] = value;
            }
        }

        private string _numEmbarque
        {
            get
            {
                if (ViewState["_numEmbarque"] != null)
                    return ViewState["_numEmbarque"].ToString();
                return "";
            }
            set
            {
                ViewState["_numEmbarque"] = value;
            }
        }

        protected List<GrdCertificacion> ListadoCertificacion
        {
            get
            {
                if (ProyectoID == 0)
                {
                    return new List<GrdCertificacion>();
                }
                _logger.Info("inicio obtener Certificacion: "+ DateTime.Now);
                return CertificacionBL.Instance.ObtenerParaListadoCertificacion(ProyectoID, _ordenTrabajo, _numeroControl, _numEmbarque, obtenFiltros());
            }
        }

        private bool EmbarqueExiste
        {
            get
            {
                if (ViewState["EmbarqueExiste"] == null)
                {
                    string rutaEmbarque = UtileriasReportes.DameRutaReporte(ProyectoID, TipoReporteProyectoEnum.Embarque);
                    ViewState["EmbarqueExiste"] = UtileriasReportes.ReporteExiste(rutaEmbarque);
                }

                return ViewState["EmbarqueExiste"].SafeBoolParse();
            }
        }

        private bool CertificacionExiste
        {
            get
            {
                if (ViewState["CertificacionExiste"] == null)
                {
                    string rutaTrazabilidad = UtileriasReportes.DameRutaReporte(ProyectoID, TipoReporteProyectoEnum.Trazabilidad);
                    if (rutaTrazabilidad.ContainsIgnoreCase(Config.ReportingServicesDefaultFolder))
                    {
                        ViewState["CertificacionExiste"] = true;
                    }
                    else
                    {
                        ViewState["CertificacionExiste"] = UtileriasReportes.ReporteExiste(rutaTrazabilidad);
                    }
                }

                return ViewState["CertificacionExiste"].SafeBoolParse();
            }
        }

        private string _script = @"$(function () {
            Sam.Calidad.AttachHandlers();
            Sam.Calidad.MuestraGrid();
        });";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdnMenuColumnasID.Value = menuColumnas.ClientID;

                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cal_Certificacion);
            }
        }

        protected void menuColumnas_ItemClick(object sender, RadMenuEventArgs e)
        {
            RadMenuItem itemClicked = e.Item;
            string columnaSeleccionada = hdnColumnaSeleccionada.Value;

            switch (itemClicked.Value)
            {
                case "ordAsc":
                    agregaOrden(columnaSeleccionada, "ASC");
                    break;
                case "ordDesc":
                    agregaOrden(columnaSeleccionada, "DESC");
                    break;
                case "remove":
                    quitaOrden(columnaSeleccionada);
                    break;                
                default:
                    break;
            }

            pager.Reset();
            cargaDatos();
        }

        private void agregaOrden(string nombreColumna, string tipo)
        {
            ColumnaOrdenamiento = nombreColumna;
            DireccionOrdenamiento = tipo;
        }

        private void quitaOrden(string nombreColumna)
        {
            DireccionOrdenamiento = null;
            ColumnaOrdenamiento = null;
        }

        protected void btnMostrarClick(object sender, EventArgs e)
        {
            try
            {
                ProyectoID = filtroGenerico.ProyectoSelectedValue.SafeIntParse();
                _ordenTrabajo = filtroGenerico.OrdenTrabajoSelectedValue.SafeIntParse();
                _numeroControl = filtroGenerico.NumeroControlSelectedValue.SafeIntParse();
                _numEmbarque = txtEmbarque.Text;
                pager.Reset();
                _logger.Info("inicio carga datos :" + DateTime.Now);
                cargaDatos();
                _logger.Info("fin carga datos :" + DateTime.Now);
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }

        protected void repCertificacion_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                GrdCertificacion item = (GrdCertificacion)e.Item.DataItem;
                RepeaterItem dataItem = (RepeaterItem)e.Item;

                Control control = dataItem.FindControl("SoldaduraImgPalomita");
                HtmlTableCell cell;

                if (chkTrazabilidad.Checked)
                {
                    cell = dataItem.FindControl("tdTra") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";
                    cell = dataItem.FindControl("tdTra2") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";
                }

                if (control != null)
                {
                    control.Visible = item.SoladoCompleto;
                }
                control = dataItem.FindControl("ArmadoImgPalomita");
                if (control != null)
                {
                    control.Visible = item.ArmadoCompletas;
                }
                if (chkInspeccionVisual.Checked)
                {

                    cell = dataItem.FindControl("tdIV") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";

                    cell = dataItem.FindControl("tdIV2") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";

                }
                control = dataItem.FindControl("InspVisImgPalomita");
                if (control != null)
                {
                    control.Visible = item.InspVisAprobada;
                }
                if (chkLibDimensional.Checked)
                {
                    cell = dataItem.FindControl("tdID") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";

                    cell = dataItem.FindControl("tdID2") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";
                }
                control = dataItem.FindControl("InspDimImgPalomita");
                if (control != null)
                {
                    control.Visible = item.InspDimAprobada;
                }
                if (chkEspesores.Checked)
                {
                    cell = dataItem.FindControl("tdEsp") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";

                    cell = dataItem.FindControl("tdEsp2") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";
                }
                control = dataItem.FindControl("EspesoresImgPalomita");
                if (control != null)
                {
                    control.Visible = item.InspEspesoresAprobada;
                }
                if (chkRT.Checked)
                {
                    cell = dataItem.FindControl("tdRT") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";

                    cell = dataItem.FindControl("tdRT2") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";
                }
                control = dataItem.FindControl("RtImgPalomita");
                if (control != null)
                {
                    control.Visible = item.RtCompleto;
                }
                if (chkPT.Checked)
                {
                    cell = dataItem.FindControl("tdPT") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";

                    cell = dataItem.FindControl("tdPT2") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";
                }
                control = dataItem.FindControl("PtImgPalomita");
                if (control != null)
                {
                    control.Visible = item.PtCompleto;
                }
                if (chkPWHT.Checked)
                {
                    cell = dataItem.FindControl("tdPWHT") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";

                    cell = dataItem.FindControl("tdPWHT2") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";
                }
                control = dataItem.FindControl("PwhtImgPalomita");
                if (control != null)
                {
                    control.Visible = item.PwhtCompleto;
                }
                if (chkDurezas.Checked)
                {
                    cell = dataItem.FindControl("tdDur") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";

                    cell = dataItem.FindControl("tdDur2") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";
                }
                control = dataItem.FindControl("DurezasImgPalomita");
                if (control != null)
                {
                    control.Visible = item.DurezasCompleto;
                }
                if (chkRTPostTT.Checked)
                {
                    cell = dataItem.FindControl("tdRTTT") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";

                    cell = dataItem.FindControl("tdRTTT2") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";
                }
                control = dataItem.FindControl("RtPostTtImgPalomita");
                if (control != null)
                {
                    control.Visible = item.RtPostCompleto;
                }
                if (chkPTPostTT.Checked)
                {
                    cell = dataItem.FindControl("tdPTTT") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";

                    cell = dataItem.FindControl("tdPTTT2") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";
                }
                control = dataItem.FindControl("PtPostTtImgPalomita");
                if (control != null)
                {
                    control.Visible = item.PtPostCompleto;
                }
                if (chkPintura.Checked)
                {
                    cell = dataItem.FindControl("tdPin") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";

                    cell = dataItem.FindControl("tdPin2") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";
                }
                control = dataItem.FindControl("PinturaImgPalomita");
                if (control != null)
                {
                    control.Visible = item.PinturaCompleta;
                }
                if (chkPreheat.Checked)
                {
                    cell = dataItem.FindControl("tdPre") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";

                    cell = dataItem.FindControl("tdPre2") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";
                }
                if (chkUT.Checked)
                {
                    cell = dataItem.FindControl("tdUT") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";

                    cell = dataItem.FindControl("tdUT2") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";
                }
                if (control != null)
                {
                    control.Visible = item.UtCompleto;
                }
                if (chkPMI.Checked)
                {
                    cell = dataItem.FindControl("tdPMI") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";

                    cell = dataItem.FindControl("tdPMI2") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";
                }
                control = dataItem.FindControl("PMIImgPalomita");
                if (control != null)
                {
                    control.Visible = item.PMICompleto;
                }

                if (chkPruebaHidro.Checked)
                {
                    cell = dataItem.FindControl("tdPruebaHidro") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";

                    cell = dataItem.FindControl("tdPruebaHidro2") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";
                }
                control = dataItem.FindControl("PruebaHidroImgPalomita");
                if (control != null)
                {
                    control.Visible = item.PruebaHidroAprobada;
                }

                control = dataItem.FindControl("PreheatImgPalomita");
                if (control != null)
                {
                    control.Visible = item.PreheatCompleto;
                }

                if (chkWPS.Checked)
                {
                    cell = dataItem.FindControl("tdWPS") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";

                    cell = dataItem.FindControl("tdWPS2") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";
                }
                control = dataItem.FindControl("WpsImgPalomita");
                if (control != null)
                {
                    control.Visible = item.WpsCompleto;
                }

                if (chkMTR.Checked)
                {
                    cell = dataItem.FindControl("tdMTR") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";

                    cell = dataItem.FindControl("tdMTR2") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";
                }
                control = dataItem.FindControl("MTRImgPalomita");
                if (control != null)
                {
                    control.Visible = item.MtrCompleto;
                }

                control = dataItem.FindControl("MTRSoldImgPalomita");
                if (control != null)
                {
                    control.Visible = item.MtrSoldCompleto;
                }

                if (chkEmbarque.Checked)
                {
                    cell = dataItem.FindControl("tdEmb") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";

                    cell = dataItem.FindControl("tdEmb2") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";

                    cell = dataItem.FindControl("tdEmb3") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";
                }

                if (chkMTRSold.Checked)
                {
                    cell = dataItem.FindControl("tdMTRSold") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";

                    cell = dataItem.FindControl("tdMTRSold2") as HtmlTableCell;
                    cell.Attributes["class"] = "dossiercorta";
                }

                //if (chkDrawing.Checked)
                //{
                //    cell = dataItem.FindControl("tdDra") as HtmlTableCell;
                //    cell.Attributes["class"] = "dossiercorta";
                //}

                control = dataItem.FindControl("EmbarqueImgPalomita");
                if (control != null)
                {
                    control.Visible = item.EmbarqueCompleto;
                }

                HyperLink link = (HyperLink)dataItem.FindControl("InspVisLnkImprimir");
                if (link != null)
                {
                    link.Visible = item.InspVisImprimir;
                    link.NavigateUrl = construyeLiga(TipoReporte.InspeccionVisual, item.SpoolID);
                }
                link = (HyperLink)dataItem.FindControl("InspDimLnkImprimir");
                if (link != null)
                {
                    link.Visible = item.InspDimImprimir;
                    link.NavigateUrl = construyeLiga(TipoReporte.ReporteLiberacionDimensional, item.SpoolID);
                }
                link = (HyperLink)dataItem.FindControl("EspesoresLnkImprimir");
                if (link != null)
                {
                    link.Visible = item.InspEspImprimir;
                    link.NavigateUrl = construyeLiga(TipoReporte.ReporteEspesores, item.SpoolID);
                }
                link = (HyperLink)dataItem.FindControl("RtLnkImprimir");
                if (link != null)
                {
                    link.Visible = item.RtImprimir;
                    link.NavigateUrl = construyeLiga(TipoReporte.ReporteRT, item.SpoolID);
                }
                link = (HyperLink)dataItem.FindControl("PtLnkImprimir");
                if (link != null)
                {
                    link.Visible = item.PtImprimir;
                    link.NavigateUrl = construyeLiga(TipoReporte.ReportePT, item.SpoolID);
                }
                link = (HyperLink)dataItem.FindControl("PwhtLnkImprimir");
                if (link != null)
                {
                    link.Visible = item.PwhtImprimir;
                    link.NavigateUrl = construyeLiga(TipoReporte.ReportePWHT, item.SpoolID);
                }
                link = (HyperLink)dataItem.FindControl("DurezasLnkImprimir");
                if (link != null)
                {
                    link.Visible = item.DurezasImprimir;
                    link.NavigateUrl = construyeLiga(TipoReporte.ReporteDurezas, item.SpoolID);
                }
                link = (HyperLink)dataItem.FindControl("RtPostTtLnkImprimir");
                if (link != null)
                {
                    link.Visible = item.RtPostImprimir;
                    link.NavigateUrl = construyeLiga(TipoReporte.ReportePostRT, item.SpoolID);
                }
                link = (HyperLink)dataItem.FindControl("PtPostTtLnkImprimir");
                if (link != null)
                {
                    link.Visible = item.PtPostImprimir;
                    link.NavigateUrl = construyeLiga(TipoReporte.ReportePostPT, item.SpoolID);
                }

                link = (HyperLink)dataItem.FindControl("PinturaLnkImprimir");
                if (link != null)
                {
                    link.Visible = item.PinturaImprimir;
                    link.NavigateUrl = construyeLiga(TipoReporte.ReportePintura, item.SpoolID);
                }

                link = (HyperLink)dataItem.FindControl("PreheatLnkImprimir");
                if (link != null)
                {
                    link.Visible = item.PreheatImprimir;
                    link.NavigateUrl = construyeLiga(TipoReporte.ReportePreheat, item.SpoolID);
                }

                link = (HyperLink)dataItem.FindControl("UtLnkImprimir");
                if (link != null)
                {
                    link.Visible = item.UtImprimir;
                    link.NavigateUrl = construyeLiga(TipoReporte.ReporteUT, item.SpoolID);
                }

                link = (HyperLink)dataItem.FindControl("PMILnkImprimir");
                if (link != null)
                {
                    link.Visible = item.PMIImprimir;
                    link.NavigateUrl = construyeLiga(TipoReporte.ReportePMI, item.SpoolID);
                }

                link = (HyperLink)dataItem.FindControl("PruebaHidroLnkImprimir");
                if (link != null)
                {
                    link.Visible = item.PruebaHidroImprimir;
                    link.NavigateUrl = construyeLiga(TipoReporte.ReporteSpoolPND, item.SpoolID);
                }

                link = (HyperLink)dataItem.FindControl("WpsLnkImprimir");
                if (link != null)
                {
                    link.Visible = item.WpsImprimir;
                    link.NavigateUrl = construyeLiga(TipoReporte.ReporteWps, item.SpoolID);
                }

                link = (HyperLink)dataItem.FindControl("MTRLnkImprimir");
                if (link != null)
                {
                    link.Visible = item.MTRImprimir;
                    link.NavigateUrl = construyeLiga(TipoReporte.ReporteMTR, item.SpoolID);
                }

                link = (HyperLink)dataItem.FindControl("MTRSoldLnkImprimir");
                if (link != null)
                {
                    link.Visible = item.MTRSoldImprimir;
                    link.NavigateUrl = construyeLiga(TipoReporte.ReporteMTRSoldadura, item.SpoolID);
                }

                link = (HyperLink)dataItem.FindControl("dibujoLnkEscaneo");
                if (link != null)
                {
                    link.Visible = item.DibujoImprimir;
                    link.NavigateUrl = string.Format("/Calidad/ImpresionDibujo.aspx?ID={0}&Dibujo={1}&ProyectoID={2}", item.SpoolID, item.Dibujo, filtroGenerico.ProyectoSelectedValue);
                }

                link = (HyperLink)dataItem.FindControl("EmbarqueLnkImprimir");
                if (link != null && EmbarqueExiste)
                {                    
                    link.Visible = item.EmbarqueCompleto;
                    link.NavigateUrl = string.Format("/Calidad/ImpresionEmbarque.aspx?ID={0}&NumeroEmbarque={1}&ProyectoID={2}", item.SpoolID, item.NumeroEmbarque, filtroGenerico.ProyectoSelectedValue);
                }
                link = (HyperLink)dataItem.FindControl("EmbarqueLnkEsc");
                if (link != null && EmbarqueExiste)
                {                    
                    link.Visible = item.EmbarqueCompleto;
                    if (item.EmbarqueEscaneado)
                    {
                        link.ImageUrl = "~/Imagenes/Iconos/pdfEscaneado.png";
                    }
                    link.NavigateUrl = string.Format("/Calidad/ImpresionEmbarque.aspx?ID={0}&NumeroEmbarque={1}&ProyectoID={2}&Escaneado=1", item.SpoolID, item.NumeroEmbarque, filtroGenerico.ProyectoSelectedValue, item.Spool);
                }

                link = (HyperLink)dataItem.FindControl("caratulaLnkImprimir");
                if (link != null)
                {                    
                    link.NavigateUrl = string.Format("/Calidad/ImpresionCaratula.aspx?ID={0}&NumeroControl={1}&ProyectoID={2}", item.SpoolID, item.NumeroControl, filtroGenerico.ProyectoSelectedValue);
                }
                
                link = (HyperLink)dataItem.FindControl("caratulaLnkEscaneo");
                if (link != null)
                {
                    if (item.TrazabilidadEscaneado)
                    {
                        link.ImageUrl = "~/Imagenes/Iconos/pdfEscaneado.png";
                    }
                    link.NavigateUrl = string.Format("/Calidad/ImpresionCaratula.aspx?ID={0}&NumeroControl={1}&ProyectoID={2}&Spool={3}", item.SpoolID, item.NumeroControl, filtroGenerico.ProyectoSelectedValue,item.Spool);
                }
            }
        }

        protected void lnkDescargar_Click(object sender, EventArgs e)
        {
            List<int> ids = new List<int>();

            try
            {
                foreach (RepeaterItem item in repCongelados.Items)
                {
                    if (item.IsItem())
                    {
                        CheckBox chkSpool = item.FindControl("chkSpool") as CheckBox;
                        HiddenField hdnSpoolID = item.FindControl("hdnSpoolID") as HiddenField;

                        if (chkSpool.Checked)
                        {
                            ids.Add(hdnSpoolID.Value.SafeIntParse());
                        }
                    }
                }

                if (ids.Count > 0)
                {
                    if (EmbarqueExiste)
                    {
                        if (CertificacionExiste)
                        {
                            CertificacionBL.Instance.DescargaReportes(
                                                   ListadoCertificacion.Where(x => ids.Contains(x.SpoolID)).ToList(), ProyectoID, WebConstants.ReportesAplicacion.Wkst_Embarque);
                        }
                        else
                        {
                            string mensaje = "El reporte de Trazabilidad configurado en el proyecto no existe en el servidor de reportes. Por favor contacte a su administrador para que lo configure correctamente.";

                            if (LanguageHelper.CustomCulture == LanguageHelper.INGLES)
                            {
                                mensaje = "The tracking report that has been configured to the project does not exists in the reporting services server. Please contact your administrator so he can configure it correctly";
                            }
                            throw new BaseValidationException(mensaje);
                        }
                    }
                    else
                    {
                        string mensaje = "El reporte de Embarque configurado en el proyecto no existe en el servidor de reportes. Por favor contacte a su administrador para que lo configure correctamente.";

                        if (LanguageHelper.CustomCulture == LanguageHelper.INGLES)
                        {
                            mensaje = "The shipping report that has been configured to the project does not exists in the reporting services server. Please contact your administrator so he can configure it correctly";
                        }
                        throw new BaseValidationException(mensaje);
                    }
                }
                else
                {
                    string mensaje = "Al menos un Spool debe estar seleccionado.";

                    if (LanguageHelper.CustomCulture == LanguageHelper.INGLES)
                    {
                        mensaje = "At least one spool must be selected";
                    }
                    throw new BaseValidationException(mensaje);
                }
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }

            ClientScript.RegisterStartupScript(typeof(CertificacionLigero), "load", _script, true);
        }

        private string construyeLiga(TipoReporte tipoReporte, int spoolID)
        {
            return string.Format(WebConstants.CalidadUrl.DESCARGA_REPORTES,
                                 spoolID, filtroGenerico.ProyectoSelectedValue,
                                 (int)tipoReporte);
        }


        /// <summary>
        /// Se dispara cuando el usuario cambia la seleccion de proyecto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void proyecto_Cambio(object sender, EventArgs e)
        {
            pnlEnDossier.Visible = false;
            pnlFiltrosSegmentos.Visible = false;

            int proyectoID = filtroGenerico.ProyectoSelectedValue.SafeIntParse();
            if (proyectoID == -1) return;

            //Obtenemos la configuracion dossier del proyecto y la reflejamos en pantalla
            Proyecto p = ProyectoBO.Instance.ObtenerConDossierYSegmentos(proyectoID);

            chkDurezas.Checked = p.ProyectoDossier.ReporteDurezas;
            chkEspesores.Checked = p.ProyectoDossier.ReporteEspesores;
            chkInspeccionVisual.Checked = p.ProyectoDossier.ReporteInspeccionVisual;
            chkLibDimensional.Checked = p.ProyectoDossier.ReporteLiberacionDimensional;
            chkPintura.Checked = p.ProyectoDossier.ReportesPintura;
            chkPreheat.Checked = p.ProyectoDossier.ReportePreheat;
            chkPT.Checked = p.ProyectoDossier.ReportePT;
            chkRT.Checked = p.ProyectoDossier.ReporteRT;
            chkPTPostTT.Checked = p.ProyectoDossier.ReportePTPostTT;
            chkRTPostTT.Checked = p.ProyectoDossier.ReporteRTPostTT;
            chkPWHT.Checked = p.ProyectoDossier.ReportePwht;
            chkRTPostTT.Checked = p.ProyectoDossier.ReporteRTPostTT;
            chkUT.Checked = p.ProyectoDossier.ReporteUT;
            chkPMI.Checked = p.ProyectoDossier.ReportePMI;
            chkPruebaHidro.Checked = p.ProyectoDossier.ReportePruebaHidrostatica != null ? p.ProyectoDossier.ReportePruebaHidrostatica.Value : false;
            chkTrazabilidad.Checked = p.ProyectoDossier.Trazabilidad;
            chkWPS.Checked = p.ProyectoDossier.WPS;
            chkMTR.Checked = p.ProyectoDossier.MTR;
            chkEmbarque.Checked = p.ProyectoDossier.Embarque;
            chkMTRSold.Checked = p.ProyectoDossier.MTRSoldadura;
            chkDrawing.Checked = p.ProyectoDossier.Drawing;
            //el panel se hace visible 
            pnlEnDossier.Visible = true;

            pnlFiltrosSegmentos.Visible = true;
            repFiltros.DataSource = generaFiltros(p.ProyectoNomenclaturaSpool);
            repFiltros.DataBind();
        }

        /// <summary>
        /// Método de ayuda para generar cada unos de los filtros de segmentos
        /// </summary>
        /// <param name="nomenclatura"></param>
        /// <returns></returns>
        private DataSet generaFiltros(ProyectoNomenclaturaSpool nomenclatura)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add();
            ds.Tables[0].Columns.Add("Segmento");
            ds.Tables[0].Columns.Add("NombreSegmento");

            int seg = 0;
            if (nomenclatura.CantidadSegmentosSpool > seg)
            {
                agregaRenglon(ds.Tables[0], seg, nomenclatura.SegmentoSpool1);
                seg++;
                if (nomenclatura.CantidadSegmentosSpool > seg)
                {
                    agregaRenglon(ds.Tables[0], seg, nomenclatura.SegmentoSpool2);
                    seg++;
                    if (nomenclatura.CantidadSegmentosSpool > seg)
                    {
                        agregaRenglon(ds.Tables[0], seg, nomenclatura.SegmentoSpool3);
                        seg++;
                        if (nomenclatura.CantidadSegmentosSpool > seg)
                        {
                            agregaRenglon(ds.Tables[0], seg, nomenclatura.SegmentoSpool4);
                            seg++;
                            if (nomenclatura.CantidadSegmentosSpool > seg)
                            {
                                agregaRenglon(ds.Tables[0], seg, nomenclatura.SegmentoSpool5);
                                seg++;
                                if (nomenclatura.CantidadSegmentosSpool > seg)
                                {
                                    agregaRenglon(ds.Tables[0], seg, nomenclatura.SegmentoSpool6);
                                    seg++;
                                    if (nomenclatura.CantidadSegmentosSpool > seg)
                                    {
                                        agregaRenglon(ds.Tables[0], seg, nomenclatura.SegmentoSpool7);
                                        seg++;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return ds;
        }

        /// <summary>
        /// Metodo de ayuda para generar los renglones del DataSet de Segmentos
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="segmento"></param>
        /// <param name="nombre"></param>
        private void agregaRenglon(DataTable dt, int segmento, string nombre)
        {
            dt.Rows.Add(dt.NewRow());
            dt.Rows[segmento]["Segmento"] = segmento;
            dt.Rows[segmento]["NombreSegmento"] = nombre + ":";
        }

        private string[] obtenFiltros()
        {
            string[] filtros = new string[7];
            int seg = 0;

            foreach (RepeaterItem item in repFiltros.Items)
            {
                if (item.IsItem())
                {
                    LabeledTextBox txtFiltro = item.FindControl("txtFiltro") as LabeledTextBox;
                    filtros[seg] = txtFiltro.Text;
                    seg++;
                }
            }

            return filtros;
        }

        protected void pager_PaginaCambio(object sender, ArgumentosPaginador args)
        {
            cargaDatos();
        }

        private void cargaDatos()
        {
            List<GrdCertificacion> resultados = ListadoCertificacion;
            var query = resultados.AsEnumerable();

            if (DireccionOrdenamiento == "ASC")
            {
                if (ColumnaOrdenamiento == "NumeroControl")
                {
                    query = query.OrderBy(x => x.NumeroControl);
                }
                else if (ColumnaOrdenamiento == "Spool")
                {
                    query = query.OrderBy(x => x.Spool);
                }
            }
            else if (DireccionOrdenamiento == "DESC")
            {
                if (ColumnaOrdenamiento == "NumeroControl")
                {
                    query = query.OrderByDescending(x => x.NumeroControl);
                }
                else if (ColumnaOrdenamiento == "Spool")
                {
                    query = query.OrderByDescending(x => x.Spool);
                }
            }

            List<GrdCertificacion> spools = query.Skip(pager.TamanioPagina * pager.PaginaActual)
                                                 .Take(pager.TamanioPagina)
                                                 .ToList();
            
            repCongelados.DataSource = spools;
            repCongelados.DataBind();

            repCertificacion.DataSource = spools;
            repCertificacion.DataBind();


            pager.Bind(pager.PaginaActual, resultados.Count);

            ClientScript.RegisterStartupScript(typeof(CertificacionLigero), "load", _script, true);
        }
    }
}