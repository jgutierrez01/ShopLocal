using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities;
using SAM.BusinessObjects.Validations;
using SAM.BusinessObjects.Workstatus;
using SAM.BusinessLogic.Workstatus;
using System.Globalization;
using Mimo.Framework.Common;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;
using Mimo.Framework.WebControls;

namespace SAM.Web.WorkStatus
{
    public partial class PopUpReportePND : SamPaginaPopup
    {
        #region Propiedades
        private List<JuntaReportePndSector> DefectosSector
        {
            get
            {
                if (ViewState["DefectosSector"] == null)
                {
                    ViewState["DefectosSector"] = new List<JuntaReportePndSector>();
                }
                return (List<JuntaReportePndSector>)ViewState["DefectosSector"];
            }
            set
            {
                ViewState["DefectosSector"] = value;
            }
        }

        private List<JuntaReportePndCuadrante> DefectosCuadrante
        {
            get
            {
                if (ViewState["DefectosCuadrante"] == null)
                {
                    ViewState["DefectosCuadrante"] = new List<JuntaReportePndCuadrante>();
                }
                return (List<JuntaReportePndCuadrante>)ViewState["DefectosCuadrante"];
            }
            set
            {
                ViewState["DefectosCuadrante"] = value;
            }
        }

        private int IDCounter
        {
            get
            {
                return ViewState["IDCunter"].SafeIntParse();
            }
            set
            {
                ViewState["IDCunter"] = value;
            }
        }

        private string IDs
        {
            get
            {
                if (ViewState["IDs"] == null)
                {
                    ViewState["IDs"] = string.Empty;
                }

                return ViewState["IDs"].ToString();
            }
            set
            {
                ViewState["IDs"] = value;
            }
        }

        private string RIDs
        {
            get
            {
                if (ViewState["RIDs"] == null)
                {
                    ViewState["RIDs"] = string.Empty;
                }

                return ViewState["RIDs"].ToString();
            }
            set
            {
                ViewState["RIDs"] = value;
            }
        }

        private int TipoPruebaID
        {
            get
            {
                return ViewState["TipoPruebaID"].SafeIntParse();
            }
            set
            {
                ViewState["TipoPruebaID"] = value;
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

        private string Juntas
        {
            get
            {
                if (ViewState["Juntas"] == null)
                {
                    ViewState["Juntas"] = string.Empty;
                }
                return ViewState["Juntas"].ToString();
            }
            set { ViewState["Juntas"] = value; }
        }

        private string NumerosControl
        {
            get
            {
                if (ViewState["NumerosControl"] == null)
                {
                    ViewState["NumerosControl"] = string.Empty;
                }
                return ViewState["NumerosControl"].ToString();
            }
            set
            {
                ViewState["NumerosControl"] = value;
            }
        }

        private string Proceso
        {
            get
            {
                if (ViewState["Proceso"] == null)
                {
                    ViewState["Proceso"] = string.Empty;
                }
                return ViewState["Proceso"].ToString();
            }
            set
            {
                ViewState["Proceso"] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAProyecto(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar un proyecto {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                IDs = Request.QueryString["IDs"];
                TipoPruebaID = Request.QueryString["TP"].SafeIntParse();
                RIDs = Request.QueryString["RID"];

                int[] juntaWorkstatusIds = IDs.Split(',').Select(n => n.SafeIntParse()).ToArray();

                Fechas = ValidaFechasBO.Instance.ObtenerFechasRequisiciones(juntaWorkstatusIds, RIDs);
                Juntas = ValidaFechasBO.Instance.ObtenerJuntasPorJuntaWorkstatusIds(juntaWorkstatusIds);
                NumerosControl = ValidaFechasBO.Instance.ObtenerNumerosControlPorJuntaWorkStatusIds(juntaWorkstatusIds);


                if (LanguageHelper.CustomCulture == LanguageHelper.INGLES)
                {
                    Proceso = "NDT Requisition";
                }
                else
                {
                    Proceso = "Requisiciones PND";
                
                }

                
                //btnAGenerar.OnClientClick = "return Sam.Workstatus.ValidacionFechas('" + numerosControl + "','" + juntas + "' ,'" + Fechas + "','" + Fechas + "','3','" + proceso + "')";
                //btnAutoGenerar.OnClientClick = "return Sam.Workstatus.ValidacionFechas('" + numerosControl + "','" + juntas + "' ,'" + Fechas + "','" + Fechas + "','3','" + proceso + "')";
                cargaCombos(TipoPruebaID);
            }

        }

        /// <summary>
        /// Carga la información de todos los combos de la pagina
        /// </summary>
        private void cargaCombos(int tipoPruebaID)
        {
            ddlDefectoCuadrante.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerDefectos().Where(x => x.TipoPruebaID == tipoPruebaID));
            ddlDefectoSector.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerDefectos().Where(x => x.TipoPruebaID == tipoPruebaID));

            if (TipoPruebaID == 10)
            {
                ddlResultado.SelectedIndex = 2;
            }

        }

        /// <summary>
        /// Si el resultado es rechazado se muestran los paneles de defectos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlResultado_IndexChanged(object sender, EventArgs e)
        {
            if (ddlResultado.SelectedValue == "1")
            {
                pnlDefecto.Visible = false;
                ddlTipoDefecto.SelectedIndex = -1;
                pnDefectosCuadrante.Visible = false;
                pnDefectosSector.Visible = false;
            }
            else
            {
                if (TipoPruebaID == 10)
                {
                    ddlTipoDefecto.SelectedIndex = 2;
                    ddlDefectoCuadrante.SelectedIndex = 1;

                }
                else
                {
                    pnlDefecto.Visible = true;
                }
            }
        }

        /// <summary>
        /// Dependiendo del tipo de defecto se muestran los paneles ya sea de cuadrante o sector
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoDefecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipoDefecto.SelectedValue.SafeIntParse() == (int)TipoRechazoEnum.Sector)
            {
                pnDefectosSector.Visible = true;
                repCuadrante.Visible = false;
                pnDefectosCuadrante.Visible = false;
            }
            else
            {
                pnDefectosSector.Visible = false;
                repDefectoSector.Visible = false;
                pnDefectosCuadrante.Visible = true;
            }
        }

        /// <summary>
        /// REALIZA LAS VALIDACIONES NECESARIOS PARA LAS FECHAS
        /// SUSTITUYE A LAS VALIDACIONES EN JAVASCRIPT
        /// </summary>
        protected void ValidaFechas()
        {
            
            string errores = "";
            DateTime fechaReporte = new DateTime(rdpFechaReporte.SelectedDate.Value.Year, rdpFechaReporte.SelectedDate.Value.Month, rdpFechaReporte.SelectedDate.Value.Day);
            DateTime fechaPrueba = new DateTime(rdpFechaPrueba.SelectedDate.Value.Year, rdpFechaPrueba.SelectedDate.Value.Month, rdpFechaPrueba.SelectedDate.Value.Day);
            string[] juntas = Juntas.Split(',');
            string[] numerosControl = NumerosControl.Split(',');
            string[] fechas = Fechas.Split(',');

            //Primer validacion de Fechas
            if (fechaReporte < fechaPrueba)
            {
                if (CultureInfo.CurrentCulture.Name == "en-US")
                {
                    throw new BaseValidationException("The report date must be greater than process date");
                }
                else
                {
                    throw new BaseValidationException("La fecha del reporte debe ser mayor o igual a la del proceso");
                }
            }

            //Se valida que ninguna de las fechas seleccionadas sea menor a la fecha del proceso
            int cuenta = 0;
            for (int i = 0; i < fechas.Length; i++)
            {
                DateTime tempfecha = DateTime.ParseExact(fechas[i], "MM/dd/yyyy", null);
                if (fechaReporte < tempfecha || fechaPrueba < tempfecha)
                {
                    errores += "<br />" + numerosControl[i] + ", " + juntas[i];
                    cuenta++;
                }
            }
            if (cuenta > 0)
            {
                errores += "<br />";
            }
            if (cuenta == fechas.Length)
            {
                if (CultureInfo.CurrentCulture.Name == "en-US")
                {
                    throw new BaseValidationException(
                        String.Format("The report can not be saved because there is an error with respect to the dates of {0} dates", Proceso));
                }
                else
                {
                    throw new BaseValidationException(
                        String.Format("El reporte no se puede guardar ya que hay un error con las fechas respecto a las de {0}", Proceso));
                }
            }

            else if (errores.Length > 0 && cuenta < fechas.Length)
            {
                if (CultureInfo.CurrentCulture.Name == "en-US")
                {
                    PopupLabel.Text = String.Format("The control number and joint{0}\n can't be save because have wrong date, Do you want to save the report for the others joints?", errores);
                }
                else
                {
                    PopupLabel.Text = String.Format("Los números de control y juntas{0}\n no se pueden guardar porque tiene fecha incorrecta, ¿Desea guardar el reporte para las demás juntas?", errores);
                }
                radwindowPopup.VisibleOnPageLoad = true;
            }
            else
            {
                GenerarReporte();
            }
        }

      

        protected void btnAgregarSector_Click(object sender, EventArgs e)
        {
            IDCounter++;
            JuntaReportePndSector defecto = new JuntaReportePndSector
            {
                JuntaReportePndID = IDCounter,
                Sector = txtSector.Text,
                SectorInicio = txtDeSector.Text,
                SectorFin = txtASector.Text,
                DefectoID = ddlDefectoSector.SelectedValue.SafeIntParse()
            };

            DefectosSector.Add(defecto);

            repDefectoSector.DataSource = DefectosSector;
            repDefectoSector.DataBind();
            repDefectoSector.Visible = true;

            txtSector.Text = string.Empty;
            txtASector.Text = string.Empty;
            txtDeSector.Text = string.Empty;
            ddlDefectoSector.SelectedIndex = -1;
        }

        protected void repDefectoSector_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                JuntaReportePndSector sector = e.Item.DataItem as JuntaReportePndSector;
                Literal litNombreDefecto = e.Item.FindControl("litNombreDefecto") as Literal;
                litNombreDefecto.Text = CacheCatalogos.Instance.ObtenerDefectos().Where(x => x.ID == sector.DefectoID).Single().Nombre;
            }
        }

        protected void btnAgregarCuadrante_Click(object sender, EventArgs e)
        {
            IDCounter++;
            JuntaReportePndCuadrante defecto = new JuntaReportePndCuadrante
            {
                JuntaReportePndID = IDCounter,
                Cuadrante = txtCuadrante.Text,
                Placa = txtPlaca.Text,
                DefectoID = ddlDefectoCuadrante.SelectedValue.SafeIntParse()
            };

            DefectosCuadrante.Add(defecto);

            repCuadrante.DataSource = DefectosCuadrante;
            repCuadrante.DataBind();
            repCuadrante.Visible = true;

            txtCuadrante.Text = string.Empty;
            txtPlaca.Text = string.Empty;
            ddlDefectoCuadrante.SelectedIndex = -1;
        }


        protected void repCuadrante_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                JuntaReportePndCuadrante cuadrante = e.Item.DataItem as JuntaReportePndCuadrante;
                Literal litNombreDefecto = e.Item.FindControl("litNombreDefecto") as Literal;
                litNombreDefecto.Text = CacheCatalogos.Instance.ObtenerDefectos().Where(x => x.ID == cuadrante.DefectoID).Single().Nombre;
            }
        }


        protected void repDefectoSector_ItemCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                for (int count = 0; count <= DefectosSector.Count; count++)
                {
                    if (DefectosSector[count].JuntaReportePndID == e.CommandArgument.SafeIntParse())
                    {
                        DefectosSector.RemoveAt(count);
                        break;
                    }
                }
            }

            repDefectoSector.DataSource = DefectosSector;
            repDefectoSector.DataBind();
        }

        protected void repCuadrante_ItemCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                for (int count = 0; count <= DefectosCuadrante.Count; count++)
                {
                    if (DefectosCuadrante[count].JuntaReportePndID == e.CommandArgument.SafeIntParse())
                    {
                        DefectosCuadrante.RemoveAt(count);
                        break;
                    }
                }
            }

            repCuadrante.DataSource = DefectosCuadrante;
            repCuadrante.DataBind();
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            radwindowPopup.VisibleOnPageLoad = false;
            GenerarReporte();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                radwindowPopup.VisibleOnPageLoad = false;
                throw new BaseValidationException("Cancelado");
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex, "vgGenerar");
            }
        }

        protected void GenerarReporte()
        {
            //if (IsValid)
            //{
            //LA VALIDACION DE LA PAGINA ES LANZADA POR EL CONTROL AUTODISABLEBUTTON, POR LO QUE NO ES NECESARIO
            //LANZAR LA VALIDACION DESDE ESTE METODO
            try
            {
                if (TipoPruebaID == 10 && ddlResultado.SelectedValue != "1")
                {
                    JuntaReportePndCuadrante defecto = new JuntaReportePndCuadrante
                    {
                        JuntaReportePndID = 1,
                        Cuadrante = "N/A",
                        Placa = "N/A",
                        DefectoID = ddlDefectoCuadrante.SelectedValue.SafeIntParse()
                    };

                    DefectosCuadrante.Add(defecto);
                }

                string[] arregloFechas = Fechas.Split(',');
                List<string> juntas = IDs.Split(',').ToList();
                List<string> reportes = RIDs.Split(',').ToList();
                for (int i = 0; i < arregloFechas.Length; i++)
                {
                    DateTime fecha = DateTime.ParseExact(arregloFechas[i], "MM/dd/yyyy", CultureInfo.InvariantCulture);

                    if (rdpFechaPrueba.SelectedDate.Value < fecha || rdpFechaReporte.SelectedDate.Value < fecha)
                    {
                        juntas[i] = "";
                        reportes[i] = "";

                    }
                }

                juntas.RemoveAll(new System.Predicate<string>(delegate(string val) { return (val == ""); }));
                reportes.RemoveAll(new System.Predicate<string>(delegate(string val) { return (val == ""); }));

                IDs = string.Join(",", juntas);
                RIDs = string.Join(",", reportes);

                if (ddlResultado.SelectedValue == "0")
                {
                    ValidacionesJuntaReportePnd.ValidaCantidadJuntasParaRechazo(juntas.Count());
                }

                ReportePnd reporte = new ReportePnd
                {
                    ProyectoID = EntityID.Value,
                    TipoPruebaID = TipoPruebaID,
                    NumeroReporte = txtNumeroReporte.Text,
                    FechaReporte = rdpFechaReporte.SelectedDate.Value
                };

                int? tipoDefecto = null;
                if (ddlTipoDefecto.SelectedValue.SafeIntParse() > 0)
                {
                    tipoDefecto = ddlTipoDefecto.SelectedValue.SafeIntParse();
                }

                bool aprobado = ddlResultado.SelectedValue == "0" ? false : true;

                JuntaReportePnd juntaReporte = new JuntaReportePnd
                {
                    TipoRechazoID = tipoDefecto,
                    FechaPrueba = rdpFechaPrueba.SelectedDate.Value,
                    Aprobado = aprobado,
                    Observaciones = txtObservaciones.Text
                };

                lnkReporte.ProyectoID = EntityID.Value;
                lnkReporte.NombresParametrosReporte = "NumeroReporte";
                lnkReporte.ValoresParametrosReporte = txtNumeroReporte.Text;

                //si es aprobacion o 1er o 2do rechazo               
                if (ReportesBL.Instance.GuardaReportePND(reporte, juntaReporte, DefectosSector, DefectosCuadrante, IDs, RIDs, SessionFacade.UserId))
                {

                    switch ((TipoPruebaEnum)TipoPruebaID)
                    {
                        case TipoPruebaEnum.ReporteRT:
                            lblMensajeExito.Visible = aprobado;
                            lblMensajeExitoRechazo.Visible = !aprobado;
                            lblMensajeExitoReporte.Visible = true;
                            lnkReporte.Visible = true;
                            lnkReporte.Tipo = TipoReporteProyectoEnum.RT;
                            break;
                        case TipoPruebaEnum.ReportePT:
                            lblMensajeExito.Visible = aprobado;
                            lblMensajeExitoRechazo.Visible = !aprobado;
                            lblMensajeExitoReporte.Visible = true;
                            lnkReporte.Visible = true;
                            lnkReporte.Tipo = TipoReporteProyectoEnum.PT;
                            break;
                        case TipoPruebaEnum.ReportePMI:
                            lblMensajeExito.Visible = aprobado;
                            lblMensajeExitoRechazo.Visible = !aprobado;
                            lblMensajeExitoReporte.Visible = true;
                            lnkReporte.Visible = true;
                            lnkReporte.Tipo = TipoReporteProyectoEnum.PMI;
                            break;
                        default:
                            lnkReporte.Visible = false;
                            break;
                    }
                }
                else //3er rechazo
                {
                    lblMensajeExitoTercerRechazo.Visible = true;
                    lblMensajeExito.Visible = false;

                    switch ((TipoPruebaEnum)TipoPruebaID)
                    {
                        case TipoPruebaEnum.ReporteRT:
                            lblMensajeExitoReporte.Visible = true;
                            lnkReporte.Visible = true;
                            lnkReporte.Tipo = TipoReporteProyectoEnum.RT;
                            break;
                        case TipoPruebaEnum.ReportePT:
                            lblMensajeExitoReporte.Visible = true;
                            lnkReporte.Visible = true;
                            lnkReporte.Tipo = TipoReporteProyectoEnum.PT;
                            break;
                        case TipoPruebaEnum.ReportePMI:
                            lblMensajeExitoReporte.Visible = true;
                            lnkReporte.Visible = true;
                            lnkReporte.Tipo = TipoReporteProyectoEnum.PMI;
                            break;
                        default:
                            lnkReporte.Visible = false;
                            break;
                    }
                }

                JsUtils.RegistraScriptActualizaGridGenerico(this);
                phControles.Visible = false;
                phMensaje.Visible = true;

            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex, "vgGenerar");
            }
            //}
        }

        protected void btnAGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid)
                {
                    ValidaFechas();
                }
               
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex, "vgGenerar");
            }
        }


    }
}