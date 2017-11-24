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
            pnExistente.Visible = true;
            pnNuevo.Visible = false;
        }

        /// <summary>
        /// oculta el lado de estimacion existente 
        /// y muestra la de estimacion nueva
        /// </summary>
        private void MostrarReporteNuevo()
        {
            int[] inspDimensionalIds = InspDimensionalIDs.Split(',').Select(n => n.SafeIntParse()).ToArray();
            List<int> spoolIds = InspeccionDimensionalBO.Instance.ObtenerSpoolIds(inspDimensionalIds);

            FechasReporte = ValidaFechasBO.Instance.ObtenerFechasSoldaduraIVPorInspeccionDimensionalPatio(InspDimensionalIDs);
            btnGuardar.OnClientClick = "return Sam.Workstatus.ValidaFechasLiberacionDimensionalPatio('" + FechasReporte + "')";

            pnExistente.Visible = false;
            pnNuevo.Visible = true;
            dtpFechaReporte.SelectedDate = DateTime.Now;            
        }

        protected void ddlNumeroReporteExistente_OnDataBound(object sender, EventArgs e)
        {
            DropDownList list = sender as DropDownList;
            if (list != null)
                list.Items.Insert(0, "");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            int ReporteDimencionalID = 0;
            int[] ids = InspDimensionalIDs.Split(',').Select(n => int.Parse(n)).ToArray();
            using (SamContext ctx = new SamContext())
            {
                if (Page.IsValid)
                {
                    try
                    {
                        if (radNuevoReporte.Checked == true)
                        {
                            if ((LiberacionDimensionalPatioBO.ExisteInspeccionDimensional(ctx, txtNumerioReporteNuevo.Text)))                          
                            {
                                LiberacionDimensionalPatioBO.Guarda(CreaReportePopUp());
                                ReporteDimencionalID = LiberacionDimensionalPatioBO.ObtenerPorNumeroReporte(ctx, txtNumerioReporteNuevo.Text);
                            }
                        }
                        else
                        {
                            ReporteDimencionalID = LiberacionDimensionalPatioBO.ObtenerPorNumeroReporte(ctx, ddlNumeroReporteExistente.SelectedItem.ToString());
                        }

                        LiberacionDimensionalPatioBO.GenerarReporteDimensionalDetalle(ReporteDimencionalID, ids, InspDimensionalIDs, SessionFacade.UserId);

                        phControles.Visible = false;
                        phMensaje.Visible = true;
                        JsUtils.RegistraScriptActualizaGridGenerico(this);
                    }
                    catch (BaseValidationException bve)
                    {
                        RenderErrors(bve);
                    }
                }
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
    }
}