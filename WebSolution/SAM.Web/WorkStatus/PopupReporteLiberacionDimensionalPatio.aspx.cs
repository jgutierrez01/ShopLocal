using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Produccion;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities;
using SAM.Web.Classes;
using Mimo.Framework.Common;
using SAM.Web.Common;
using System.Web.Script.Serialization;
using SAM.BusinessObjects.Excepciones;

namespace SAM.Web.WorkStatus
{
    public partial class PopupReporteLiberacionDimensionalPatio : SamPaginaPopup
    {
        #region ViewStates
        /// <summary>
        /// ID del proyecto en cuestión
        /// </summary>
        private int ProyectoID
        {
            get
            {
                return (int)ViewState["ProyectoID"];
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }
        }

        /// <summary>
        /// CSV string que contiene los IDs de los spools a los cuales se le van a generar las ODTs
        /// </summary>
        private string InspDimensionalIDs
        {
            get
            {
                return ViewState["SIDs"].ToString();
            }
            set
            {
                ViewState["SIDs"] = value;
            }
        }

        private string Fechas
        {
            get
            {
                if (ViewState["Fechas"] == null)
                {
                    ViewState["Fechas"] = string.Empty;
                }

                return ViewState["Fechas"].ToString();
            }
            set
            {
                ViewState["Fechas"] = value;
            }
        }

        private string FechasReporte
        {
            get
            {
                if (ViewState["FechasReporte"] == null)
                {
                    ViewState["FechasReporte"] = string.Empty;
                }

                return ViewState["FechasReporte"].ToString();
            }
            set
            {
                ViewState["FechasReporte"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProyectoID = Request.QueryString["PID"].SafeIntParse();
                InspDimensionalIDs = Request.QueryString["SPIDS"];

                if (!SeguridadQs.TieneAccesoAProyecto(ProyectoID))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar un proyecto {1} al cual no tiene permisos", SessionFacade.UserId, ProyectoID);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }                                

                MostrarReporteNuevo();
                carga();
            }
        }

        /// <summary>
        /// genera el ddldel popup
        /// </summary>
        private void carga()
        {
            radNuevoReporte.Checked = true;

            using (SamContext ctx = new SamContext())
            {
                //se genera el ddl
                List<ReporteDimensional> InspeccionDimencional = ctx.ReporteDimensional.Where(x=> x.TipoReporteDimensionalID == 1).ToList();
                ddlNumeroReporteExistente.DataSource = InspeccionDimencional;
                ddlNumeroReporteExistente.DataTextField = "NumeroReporte";
                ddlNumeroReporteExistente.DataValueField = "ReporteDimensionalID";
                ddlNumeroReporteExistente.DataBind();
            }
        }

        protected void radNuevoReporte_OnCheckedChanged(object sender, EventArgs e)
        {
            MostrarReporteNuevo();
        }

        protected void radReporteExistente_OnCheckedChanged(object sender, EventArgs e)
        {
            MostrarReporteExistente();
        }

        /// <summary>
        /// oculta el lado de estimacion nueva 
        /// y muestra la de estimacion existente
        /// </summary>
        private void MostrarReporteExistente()
        {         
            lblNumeroReporteNueva.Visible = false;
            txtNumerioReporteNuevo.Visible = false;
            ReqNRN.Visible = false;           
            lblFechaReporte.Visible = false;
            dtpFechaReporte.Visible = false;
            ReqFR.Visible = false;            
            btnGuardarRN .Visible= false;
            lblNumeroReporteExistente.Visible = true;
            ddlNumeroReporteExistente.Visible = true;
            ReqNRE.Visible = true;           
            btnGuardarRE.Visible = true;            
        }

        /// <summary>
        /// oculta el lado de estimacion existente 
        /// y muestra la de estimacion nueva
        /// </summary>
        private void MostrarReporteNuevo()
        {
            lblNumeroReporteExistente.Visible = false;
            ddlNumeroReporteExistente.Visible = false;
            ReqNRE.Visible = false;           
            btnGuardarRE.Visible = false;
            lblNumeroReporteNueva.Visible = true;
            txtNumerioReporteNuevo.Visible = true;
            ReqNRN.Visible = true;        
            lblFechaReporte.Visible = true;
            dtpFechaReporte.Visible = true;
            ReqFR.Visible = true;           
            btnGuardarRN.Visible = true;
            
            int[] inspDimensionalIds = InspDimensionalIDs.Split(',').Select(n => n.SafeIntParse()).ToArray();
            List<int> spoolIds = InspeccionDimensionalBO.Instance.ObtenerSpoolIds(inspDimensionalIds);

            FechasReporte = ValidaFechasBO.Instance.ObtenerFechasSoldaduraIVPorInspeccionDimensionalPatio(InspDimensionalIDs);

            JavaScriptSerializer js = new JavaScriptSerializer();
            string[] fechas = null;

            if (!string.IsNullOrEmpty(FechasReporte))
            {
                fechas = FechasReporte.Split(',');
            }

            string jsonFechas = js.Serialize(fechas);
         
            string jLinkReporteNuevo = string.Format("return Sam.Workstatus.ValidaFechasLiberacionDimensionalPatio({0},{1});", dtpFechaReporte.ClientID, jsonFechas);
            string jLinkReporteExistente = string.Format("return Sam.Workstatus.ValidaFechasLiberacionDimensionalPatio({0},{1});", fechaReporteE.ClientID, jsonFechas);

            btnGuardarRN.OnClientClick = jLinkReporteNuevo;
            btnGuardarRE.OnClientClick = jLinkReporteExistente;
        
            dtpFechaReporte.SelectedDate = DateTime.Now;     
        }

        protected void ddlNumeroReporteExistente_OnDataBound(object sender, EventArgs e)
        {
            DropDownList list = sender as DropDownList;
            if (list != null)
            {
                list.Items.Insert(0, "");
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            List<string> mensajesError = new List<string>();
            int ReporteDimencionalID = 0;
            int[] ids = InspDimensionalIDs.Split(',').Select(n => int.Parse(n)).ToArray();

            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (Page.IsValid)
                    {
                        if (radNuevoReporte.Checked == true)
                        {
                            if (!string.IsNullOrEmpty(txtNumerioReporteNuevo.Text))
                            {
                                if ((LiberacionDimensionalPatioBO.ExisteInspeccionDimensional(ctx, txtNumerioReporteNuevo.Text)))
                                {
                                    LiberacionDimensionalPatioBO.Guarda(CreaReportePopUp());
                                    ReporteDimencionalID = LiberacionDimensionalPatioBO.ObtenerPorNumeroReporte(ctx, txtNumerioReporteNuevo.Text);
                                }
                            }
                            else
                            {
                                mensajesError.Add("El número de reporte es requerido");
                            }
                        }
                        else
                        {
                            if (ddlNumeroReporteExistente.SelectedIndex > 0)
                            {
                                ReporteDimencionalID = LiberacionDimensionalPatioBO.ObtenerPorNumeroReporte(ctx, ddlNumeroReporteExistente.SelectedItem.ToString());
                            }
                            else
                            {
                                mensajesError.Add("El número de reporte es requerido");
                            }
                        }

                        if (mensajesError.Count > 0)
                        {
                            throw new ExcepcionRelaciones(mensajesError);
                        }

                        LiberacionDimensionalPatioBO.GenerarReporteDimensionalDetalle(ReporteDimencionalID, ids, InspDimensionalIDs, SessionFacade.UserId);

                        phControles.Visible = false;
                        phMensaje.Visible = true;
                       
                    }
                }
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
            finally
            {
                JsUtils.RegistraScriptActualizaGridGenerico(this);    
            }
        }

        /// <summary>
        /// genera el objeto estimacion para despues mandarlo a guardar a la base de datos
        /// </summary>
        /// <param name="est"></param>
        /// <returns>Estimacion</returns>
        private ReporteDimensional CreaReportePopUp()
        {
            ReporteDimensional reporteDimensional = new ReporteDimensional();
            reporteDimensional.NumeroReporte = txtNumerioReporteNuevo.Text;
            reporteDimensional.FechaReporte = dtpFechaReporte.SelectedDate.Value;
            reporteDimensional.ProyectoID = ProyectoID;
            reporteDimensional.TipoReporteDimensionalID = 1;
            return reporteDimensional;
        }

        protected void btnAceptar_OnClick(object sender, EventArgs e)
        {
            JsUtils.RegistraScriptActualizaGridGenerico(this);
        }

        protected void ddlNumeroReporteExistente_SelectedItemChanged(object sender, EventArgs e)
        {        
            int reporteId = ddlNumeroReporteExistente.SelectedValue.SafeIntParse();
            DateTime fechaReporte;

            if (reporteId > 0)
            {
                fechaReporte = LiberacionDimensionalPatioBO.ObtenerFechaReporteDimensional(reporteId);
                fechaReporteE.Value = String.Format("{0:yyyy/MM/dd}", fechaReporte);
            }           
        }        
    }
}