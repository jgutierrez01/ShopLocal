using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using Mimo.Framework.WebControls;
using SAM.BusinessLogic.Calidad;
using SAM.BusinessObjects.Proyectos;
using SAM.Entities;
using SAM.Entities.Grid;
using SAM.Web.Classes;
using Telerik.Web.UI;

namespace SAM.Web.Calidad
{
    public partial class LstCertificacion : SamPaginaPrincipal
    {
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


        protected List<GrdCertificacion> ListadoCertificacion
        {
            get
            {
                if (ProyectoID == 0)
                {
                    return new List<GrdCertificacion>();
                }
                return CertificacionBL.Instance.ObtenerParaListadoCertificacion(ProyectoID, obtenFiltros());
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
            set
            {
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cal_Certificacion);
                grdCertificacion.EnableHeaderContextMenu = false;
                if (EntityID.HasValue)
                {
                    ProyectoID = EntityID.Value;
                    if (SeguridadQs.TieneAccesoAProyecto(ProyectoID))
                    {
                        try
                        {                           

                            if (EmbarqueExiste)
                            {

                                if (Request.Params["IDs"] != null)
                                {
                                    IEnumerable<int> ids = from id in Request.Params["IDs"].Split(',') select id.SafeIntParse();


                                    CertificacionBL.Instance.DescargaReportes(
                                        ListadoCertificacion.Where(x => ids.Contains(x.SpoolID)).ToList(), ProyectoID, WebConstants.ReportesAplicacion.Wkst_Embarque);


                                }
                                else
                                {
                                    CertificacionBL.Instance.DescargaReportes(ListadoCertificacion, ProyectoID, WebConstants.ReportesAplicacion.Wkst_Embarque);
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
                        catch (BaseValidationException ex)
                        {
                            RenderErrors(ex);
                        }
                    }
                }
            }
        }

        protected void btnMostrarClick(object sender, EventArgs e)
        {
            ProyectoID = filtroGenerico.ProyectoSelectedValue.SafeIntParse();
            grdCertificacion.Rebind();


        }

        protected void grdCertificacion_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            if (ProyectoID > 0)
            {
                grdCertificacion.DataSource = ListadoCertificacion;
            }
        }



        protected void grdCertificacion_ItemCreated(object sender, GridItemEventArgs e)
        {
            //GridCommandItem item = e.Item as GridCommandItem;
            //if (item != null)
            //{
            //    ((HyperLink)item.FindControl("lnkDescargarTodos")).NavigateUrl = "LstCertificacion.aspx?ID=" +
            //                                                                      ProyectoID;
            //    ((HyperLink)item.FindControl("lnkImgDescargarTodos")).NavigateUrl = "LstCertificacion.aspx?ID=" +
            //                                                                                      ProyectoID;
            //}
            //establecemos los links de descarga

        }

        protected void grdCertificacion_ItemCommand(object source, GridCommandEventArgs e)
        {

        }

        protected void grdCertificacion_ItemDataBound(object sender, GridItemEventArgs e)
        {

            if (e.Item.IsItem())
            {
                GrdCertificacion item = (GrdCertificacion)e.Item.DataItem;
                GridDataItem dataItem = (GridDataItem)e.Item;

                Control control = dataItem.FindControl("SoldaduraImgPalomita");

                if (control != null)
                {
                    control.Visible = item.SoladoCompleto;
                }
                control = dataItem.FindControl("ArmadoImgPalomita");
                if (control != null)
                {
                    control.Visible = item.ArmadoCompletas;
                }
                control = dataItem.FindControl("InspVisImgPalomita");
                if (control != null)
                {
                    control.Visible = item.InspVisAprobada;
                }
                control = dataItem.FindControl("InspDimImgPalomita");
                if (control != null)
                {
                    control.Visible = item.InspDimAprobada;
                }
                control = dataItem.FindControl("EspesoresImgPalomita");
                if (control != null)
                {
                    control.Visible = item.InspEspesoresAprobada;
                }
                control = dataItem.FindControl("RtImgPalomita");
                if (control != null)
                {
                    control.Visible = item.RtCompleto;
                }
                control = dataItem.FindControl("PtImgPalomita");
                if (control != null)
                {
                    control.Visible = item.PtCompleto;
                }
                control = dataItem.FindControl("PwhtImgPalomita");
                if (control != null)
                {
                    control.Visible = item.PwhtCompleto;
                }
                control = dataItem.FindControl("DurezasImgPalomita");
                if (control != null)
                {
                    control.Visible = item.DurezasCompleto;
                }
                control = dataItem.FindControl("RtPostTtImgPalomita");
                if (control != null)
                {
                    control.Visible = item.RtPostCompleto;
                }
                control = dataItem.FindControl("PtPostTtImgPalomita");
                if (control != null)
                {
                    control.Visible = item.RtPostCompleto;
                }

                control = dataItem.FindControl("PinturaImgPalomita");
                if (control != null)
                {
                    control.Visible = item.PinturaCompleta;
                }

                control = dataItem.FindControl("PreheatImgPalomita");
                if (control != null)
                {
                    control.Visible = item.PreheatCompleto;
                }

                control = dataItem.FindControl("WpsImgPalomita");
                if (control != null)
                {
                    control.Visible = item.WpsCompleto;
                }

                control = dataItem.FindControl("MTRImgPalomita");
                if (control != null)
                {
                    control.Visible = item.MtrCompleto;
                }

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
                    link.Visible = item.RtPostImprimir;
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

                link = (HyperLink)dataItem.FindControl("EmbarqueLnkImprimir");
                if (link != null && EmbarqueExiste)
                {
                    link.Visible = item.EmbarqueCompleto;                    
                    link.NavigateUrl = string.Format("/Calidad/ImpresionEmbarque.aspx?ID={0}&NumeroEmbarque={1}&ProyectoID={2}", item.SpoolID, item.NumeroEmbarque, filtroGenerico.ProyectoSelectedValue);
                }

                link = (HyperLink)dataItem.FindControl("caratulaLnkImprimir");
                if (link != null)
                {
                    link.NavigateUrl = string.Format("/Calidad/ImpresionCaratula.aspx?ID={0}&NumeroControl={1}&ProyectoID={2}", item.SpoolID, item.NumeroControl, filtroGenerico.ProyectoSelectedValue);
                }
            }
        }

        private string construyeLiga(TipoReporte tipoReporte, int spoolID)
        {
            return string.Format(WebConstants.CalidadUrl.DESCARGA_REPORTES,
                                 spoolID, filtroGenerico.ProyectoSelectedValue,
                                 (int)tipoReporte);
        }

        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdCertificacion.Rebind();
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
            chkTrazabilidad.Checked = p.ProyectoDossier.Trazabilidad;
            chkWPS.Checked = p.ProyectoDossier.WPS;
            
            chkEmbarque.Checked = p.ProyectoDossier.Embarque;
            //el panel se hace visible 
            pnlEnDossier.Visible = true;

            //escondemos o mostramos las columnas segun se requiera
            estableceVisibilidad("Durezas", chkDurezas.Checked);
            estableceVisibilidad("InspVis", chkInspeccionVisual.Checked);
            estableceVisibilidad("InspDim", chkLibDimensional.Checked);
            estableceVisibilidad("Espesores", chkEspesores.Checked);
            estableceVisibilidad("Rt", chkRT.Checked);
            estableceVisibilidad("Pt", chkPT.Checked);
            estableceVisibilidad("Pwht", chkPWHT.Checked);
            estableceVisibilidad("RtPostTt", chkRTPostTT.Checked);
            estableceVisibilidad("PtPostTt", chkPTPostTT.Checked);
            estableceVisibilidad("Preheat", chkPreheat.Checked);
            estableceVisibilidad("Ut", chkUT.Checked);
            estableceVisibilidad("Pintura", chkPintura.Checked);
            estableceVisibilidad("Caratula", chkTrazabilidad.Checked);
            estableceVisibilidad("WPS", chkWPS.Checked);
            estableceVisibilidad("Embarque", chkEmbarque.Checked);

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

        private void estableceVisibilidad(string columna, bool visible)
        {
            if (visible)
            {
                grdCertificacion.Columns.Cast<GridColumn>().Single(x => x.UniqueName.EqualsIgnoreCase(columna)).
                    ItemStyle.CssClass = "dossier";
            }
        }

        protected void lnkDescargar_onClick(object sender, EventArgs e)
        {
            try
            {
                GridDataItem[] items = grdCertificacion.MasterTableView.GetSelectedItems();
                List<GrdCertificacion> dataItems = items.Select(x => x.DataItem).Cast<GrdCertificacion>().ToList();
                CertificacionBL.Instance.DescargaReportes(dataItems, null, WebConstants.ReportesAplicacion.Wkst_Embarque);
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
        }

        protected void lnkDescargarTodos_onClick(object sender, EventArgs e)
        {
            try
            {
                CertificacionBL.Instance.DescargaReportes(ListadoCertificacion, ProyectoID, WebConstants.ReportesAplicacion.Wkst_Embarque);
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
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
    }
}