using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Produccion;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities;
using SAM.Web.Classes;
using System.Globalization;

namespace SAM.Web.WorkStatus
{
    public partial class PopupReporteLiberacionVisualPatio : SamPaginaPopup
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
        private string LiberacionVisualIds
        {
            get
            {
                return ViewState["LiberacionVisualIds"].ToString();
            }
            set
            {
                ViewState["LiberacionVisualIds"] = value;
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
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProyectoID = Request.QueryString["PID"].SafeIntParse();
                LiberacionVisualIds = Request.QueryString["JWIDS"];

                if (!SeguridadQs.TieneAccesoAProyecto(ProyectoID))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar un proyecto {1} al cual no tiene permisos", SessionFacade.UserId, ProyectoID);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                string juntas = ValidaFechasBO.Instance.ObtenerJuntasPorInspeccionVisualPatioID(LiberacionVisualIds);
                Fechas = ValidaFechasBO.Instance.ObtenerFechaSoldaduraPorInspeccionVisualPatioID(LiberacionVisualIds);

                btnGuardar.OnClientClick = "return Sam.Workstatus.ValidacionFechasLiberacionVisualPatio('" + juntas + "' ,'" + Fechas + "')";

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
                List<InspeccionVisual> InspeccionVisual = ctx.InspeccionVisual.Where(x => x.ProyectoID == ProyectoID).ToList();
                ddlNumeroReporteExistente.DataSource = InspeccionVisual;
                ddlNumeroReporteExistente.DataTextField = "NumeroReporte";
                ddlNumeroReporteExistente.DataValueField = "InspeccionVisualID";
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
            pnExistente.Visible = false;
            pnNuevo.Visible = true;
            dtpFechaReporte.SelectedDate = DateTime.Now;
        }

        protected void ddlNumeroReporteExistente_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            InspeccionVisual iv = LiberacionVisualPatioBO.ObtenerPorNumeroReporte(ProyectoID, ddlNumeroReporteExistente.SelectedItem.ToString());
            hfdtpFechaReporte.Value = iv != null ? iv.FechaReporte.ToString("MM/dd/yyyy") + "4," : string.Empty;
        }

        protected void ddlNumeroReporteExistente_OnDataBound(object sender, EventArgs e)
        {
            DropDownList list = sender as DropDownList;
            if (list != null)
                list.Items.Insert(0, ""); 
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            int inspeccionVisualID = 0;
            int[] ids = LiberacionVisualIds.Split(',').Select(n => int.Parse(n)).ToArray();
            using (SamContext ctx = new SamContext())
            {
                if (Page.IsValid)
                {
                    try
                    {
                        if (radNuevoReporte.Checked == true)
                        {
                            if ((LiberacionVisualPatioBO.ExisteInspeccionVisual(ctx, txtNumerioReporteNuevo.Text)))
                            {
                                LiberacionVisualPatioBO.Guarda(CreaReportePopUp());
                                inspeccionVisualID =  LiberacionVisualPatioBO.ObtenerPorNumeroReporte(ctx, txtNumerioReporteNuevo.Text);
                            }
                        }
                        else
                        {
                            inspeccionVisualID = LiberacionVisualPatioBO.ObtenerPorNumeroReporte(ctx, ddlNumeroReporteExistente.SelectedItem.ToString());
                        }

                        LiberacionVisualPatioBO.GenerarJuntaInspeccionVisual(inspeccionVisualID, ids, LiberacionVisualIds, SessionFacade.UserId);
                       
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
        private InspeccionVisual CreaReportePopUp()
        {
            InspeccionVisual inspeccionVisual = new InspeccionVisual();
            inspeccionVisual.NumeroReporte = txtNumerioReporteNuevo.Text;
            inspeccionVisual.FechaReporte = dtpFechaReporte.SelectedDate.Value;
            inspeccionVisual.ProyectoID = ProyectoID;
            return inspeccionVisual;
        }

        protected void btnAceptar_OnClick(object sender, EventArgs e)
        {
            JsUtils.RegistraScriptActualizaGridGenerico(this);
        }
    }
}