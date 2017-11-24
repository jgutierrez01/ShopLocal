using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.MobileControls;
using System.Web.UI.WebControls;
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
using System.Configuration;
using System.Globalization;

namespace SAM.Mobile.Paginas
{
    public partial class DetalleArmado : PaginaMovilAutenticado
    {
        #region Propiedades
        Spool _spool;
        OrdenTrabajoSpool _ots;
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (EntityID != null)
                {
                    Carga();
                    CargaComboFecha();
                }

            }
        }

        protected void lstNoJunta_OnSelectedIndexChange(object sender, EventArgs e)
        {
            ViewState["juntaSpoolID"] = lstNoJunta.Selection.Value.SafeIntParse();
            if (ArmadoBO.Instance.ExisteJuntaArmado(ViewState["juntaSpoolID"].SafeIntParse()) == false)
            {
                lblError.Visible = false;
                CargaCombosNumeroUnicos();
                tblMenu.Visible = true;
                cmdOK.Visible = true;
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Armado_JuntaArmada;
                lstNoJunta.SelectedIndex = -1;
                tblMenu.Visible = false;
                cmdOK.Visible = false;
            }
        }

        protected void cmdOK_OnClik(object sender, EventArgs e)
        {
            //Revisar que no hayan cambiado combo de junta
            if (lstNoJunta.Selection == null)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Armado_SeleccionYActualización;
                return;
            }

            //Revisar que no hayan cambiado combo de junta
            if (lstNoJunta.Selection.Text == String.Empty)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Armado_SeleccionJunta;
                return;
            }
            
            //Revisar junta existente
            if (ArmadoBO.Instance.ExisteJuntaArmado(lstNoJunta.Selection.Value.SafeIntParse()))
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Armado_JuntaArmada;
                return;
            }
            
            //Revisar Datos en Combo
            if (lstTaller.Selection.Text == String.Empty || lstTuberos.Selection.Text == String.Empty)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Armado_SeleccionIncompleta;
                return;
            }
            
            //Revisar Formato Fecha
            if (lstFecha.Selection.Text.SafeDateAsStringParse() == null)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Armado_FechaInvalida;
                return;
            }

            //Verificar que la fecha de armado sea menor a la de soldadura
            JuntaWorkstatus juntaWorkStatus = ArmadoBO.Instance.ObtenerJuntaWorkstatusPorJuntaSpoolID(ViewState["juntaSpoolID"].SafeIntParse());
            if (juntaWorkStatus != null && juntaWorkStatus.SoldaduraAprobada)
            {
                DateTime fechaProcSoldadura = ValidaFechasBO.Instance.ObtenerFechaProcesoSoldadura(juntaWorkStatus.JuntaWorkstatusID);
                fechaProcSoldadura = Convert.ToDateTime(fechaProcSoldadura.ToString("dd/MM/yyyy"));

                DateTime fechaProcArmado = Convert.ToDateTime(lstFecha.Selection.Text);
                fechaProcArmado = Convert.ToDateTime(fechaProcArmado.ToString("dd/MM/yyyy"));

                if (fechaProcArmado > fechaProcSoldadura)
                {
                    lblError.Visible = true;
                    lblError.Text = MensajesError.Armado_FechaMayorSoldadura;
                    return;
                }
            }

            // si la junta no es de reparación validamos que la fecha de armado sea menor a la de liberación dimensional si es que lo tiene
            int versionJunta = juntaWorkStatus != null ? juntaWorkStatus.VersionJunta : 1;
            WorkstatusSpool ws = WorkstatusSpoolBO.Instance.ObtenerPorJuntaSpool(lstNoJunta.Selection.Value.SafeIntParse());
            if (versionJunta == 1 && ws != null && InspeccionDimensionalBO.Instance.TieneLiberacionDimensional(ws))
            {
                string fechaLiberacion = ValidaFechasBO.Instance.ObtenerFechaLiberacionDimensional(ws.WorkstatusSpoolID);

                DateTime tempFechaLiberacion = new DateTime();
                if (CultureInfo.CurrentCulture.Name != "en-US")
                {
                    string[] splitFechaLiberacion = fechaLiberacion.Split('/');
                    string newFechaLiberacion = splitFechaLiberacion[1] + "/" + splitFechaLiberacion[0] + "/" + splitFechaLiberacion[2];

                    DateTime.TryParse(newFechaLiberacion, out tempFechaLiberacion);
                }
                else
                {
                    DateTime.TryParse(fechaLiberacion, out tempFechaLiberacion);
                }

                DateTime fechaArmado = new DateTime();
                if (CultureInfo.CurrentCulture.Name == "en-US")
                {
                    DateTime.TryParse(Convert.ToDateTime(lstFecha.Selection.Text).ToString("MM/dd/yyyy"), out fechaArmado);
                }
                else
                {
                    DateTime.TryParse(Convert.ToDateTime(lstFecha.Selection.Text).ToString("dd/MM/yyyy"), out fechaArmado);
                }

                if (fechaArmado > tempFechaLiberacion)
                {
                    lblError.Visible = true;
                    lblError.Text = MensajesError.Excepcion_FechaProcAnteriorMayorLiberacion;
                    return;
                }
            }

            //Guardar
            try
            {
                Guarda();
                lblMensaje.Visible = true;
                lblMensaje.Text = MensajesError.Armado_OK;
                tblMenu.Visible = false;
                Carga();
            }
            catch (BaseValidationException bve)
            {
                lblError.Visible = true;
                lblError.Text = bve.Details[0].ToString();
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Armado_ErrorBasedeDatos;
            }
            
        }
        #endregion

        #region Metodos
        protected void Carga()
        {
            _spool = SpoolBO.Instance.Obtener(EntityID.Value);
            _ots = OrdenTrabajoSpoolBO.Instance.ObtenerOrdenTrabajoPorSpoolID(EntityID.Value);
            lblNoOdt2.Text = _ots.OrdenTrabajo.NumeroOrden;
            lblNoControl2.Text = _ots.NumeroControl;
            lblSpool2.Text = _spool.Nombre; //AQUI
            CargaComboJuntas();
            CargaComboTalleres();
            CargaComboTuberos();
            lstNumeroUnico1.Items.Insert(0, "     ");
            lstNumeroUnico2.Items.Insert(0, "     ");
            txtObservaciones.Text = string.Empty;
        }

        protected void CargaComboJuntas()
        {
            lstNoJunta.Items.Clear();
            List<Simple> lstJuntaSpool = JuntaSpoolBO.Instance.ObtenerJuntasPorSpoolIDYCodigoFabArea(EntityID.Value, FabAreas.SHOP);
            lstNoJunta.DataSource = lstJuntaSpool;
            lstNoJunta.DataTextField = "Valor";
            lstNoJunta.DataValueField = "ID";
            lstNoJunta.DataBind();
            lstNoJunta.Items.Insert(0, String.Empty);
        }

        protected void CargaComboTalleres()
        {
            lstTaller.Items.Clear();
            lstTaller.DataSource = UserScope.TalleresPorPatio(SessionFacade.PatioID.Value);
            lstTaller.DataValueField = "ID";
            lstTaller.DataTextField = "Nombre";
            lstTaller.DataBind();
            lstTaller.Items.Insert(0, String.Empty);
        }

        protected void CargaCombosNumeroUnicos()
        {
            int juntaSpoolID = ViewState["juntaSpoolID"].SafeIntParse();
            if (juntaSpoolID != -1)
            {
                lstNumeroUnico1.Items.Clear();
                lstNumeroUnico2.Items.Clear();

                _ots = OrdenTrabajoSpoolBO.Instance.ObtenerOrdenTrabajoPorSpoolID(EntityID.Value);

                JuntaSpool juntaSpool = JuntaSpoolBO.Instance.Obtener(juntaSpoolID);

                List<Simple> lstNoUnico1 = ArmadoBO.Instance.ObtenerNumeroUnicoPorEtiquetaMaterial(juntaSpool.EtiquetaMaterial1, _ots.OrdenTrabajoSpoolID, EntityID.Value);
                lstNumeroUnico1.DataSource = lstNoUnico1;
                lstNumeroUnico1.DataTextField = "Valor";
                lstNumeroUnico1.DataValueField = "ID";
                lstNumeroUnico1.DataBind();

                List<Simple> lstNoUnico2 = ArmadoBO.Instance.ObtenerNumeroUnicoPorEtiquetaMaterial(juntaSpool.EtiquetaMaterial2, _ots.OrdenTrabajoSpoolID, EntityID.Value);
                lstNumeroUnico2.DataSource = lstNoUnico2;
                lstNumeroUnico2.DataTextField = "Valor";
                lstNumeroUnico2.DataValueField = "ID";
                lstNumeroUnico2.DataBind();
            }
        }

        protected void CargaComboTuberos()
        {
            lstTuberos.Items.Clear();
            lstTuberos.DataSource = ArmadoBO.Instance.ObtenerTuberosPorPatio(SessionFacade.PatioID.Value);
            lstTuberos.DataTextField = "Valor";
            lstTuberos.DataValueField = "ID";
            lstTuberos.DataBind();
            lstTuberos.Items.Insert(0, String.Empty);
        }

        protected void Guarda()
        {

            //Verificar que la junta tenga registro en JuntaWorkstatus
            JuntaWorkstatus juntaWorkStatus = ArmadoBO.Instance.ObtenerJuntaWorkstatusPorJuntaSpoolID(ViewState["juntaSpoolID"].SafeIntParse());
            
            int juntaSpoolID = ViewState["juntaSpoolID"].SafeIntParse();
            JuntaSpool juntaSpool = JuntaSpoolBO.Instance.Obtener(juntaSpoolID);
            TipoJunta tipoJunta = TipoJuntaBO.Instance.Obtener(juntaSpool.TipoJuntaID);

            //Generar registro en JuntaArmado
            JuntaArmado juntaArmado = new JuntaArmado();
            juntaArmado.NumeroUnico1ID = lstNumeroUnico1.Selection != null ? lstNumeroUnico1.Selection.Value.SafeIntNullableParse() : null;
            juntaArmado.NumeroUnico2ID = lstNumeroUnico2.Selection != null ? lstNumeroUnico2.Selection.Value.SafeIntNullableParse() : null;
            juntaArmado.TallerID = lstTaller.Selection.Value.SafeIntParse();
            juntaArmado.TuberoID = lstTuberos.Selection.Value.SafeIntParse();
            juntaArmado.FechaArmado = Convert.ToDateTime(lstFecha.Selection.Text);
            juntaArmado.FechaReporte = DateTime.Now;
            juntaArmado.Observaciones = txtObservaciones.Text;
            juntaArmado.UsuarioModifica = SessionFacade.UserId;
            juntaArmado.FechaModificacion = DateTime.Now;

            if (juntaWorkStatus == null)
            {
                _ots = OrdenTrabajoSpoolBO.Instance.ObtenerOrdenTrabajoPorSpoolID(EntityID.Value);

                //No existe, hay que crear registro
                juntaWorkStatus = new JuntaWorkstatus();
                juntaWorkStatus.OrdenTrabajoSpoolID = _ots.OrdenTrabajoSpoolID;
                juntaWorkStatus.JuntaSpoolID = juntaSpoolID;
                juntaWorkStatus.EtiquetaJunta = juntaSpool.Etiqueta;
                juntaWorkStatus.ArmadoAprobado = true;
                juntaWorkStatus.SoldaduraAprobada = false;
                juntaWorkStatus.InspeccionVisualAprobada = false;
                juntaWorkStatus.VersionJunta = 1;
                juntaWorkStatus.JuntaFinal = true;
                juntaWorkStatus.ArmadoPagado = false;
                juntaWorkStatus.SoldaduraPagada = false;
                juntaWorkStatus.UltimoProcesoID = UltimoProcesoEnum.Armado.SafeIntParse();
                juntaWorkStatus.UsuarioModifica = SessionFacade.UserId;
                juntaWorkStatus.FechaModificacion = DateTime.Now;
            }
            else
            {
                juntaWorkStatus.StartTracking();
                juntaWorkStatus.UsuarioModifica = SessionFacade.UserId;
                juntaWorkStatus.FechaModificacion = DateTime.Now;
                juntaWorkStatus.ArmadoAprobado = true;
                juntaWorkStatus.UltimoProcesoID = UltimoProcesoEnum.Armado.SafeIntParse();
            }

            ArmadoBO.Instance.GuardaJuntaWorkstatus(juntaWorkStatus, juntaArmado);

            // soldamos junta si la misma es de tipo TW
            if (tipoJunta.Codigo == TipoJuntas.TW && juntaWorkStatus.JuntaSoldaduraID == null)
            {
                JuntaWorkstatus juntaWorkstatus = SoldadorProcesoBO.Instance.ObtenerWorkStatus(juntaSpoolID);
                JuntaSoldadura js = new JuntaSoldadura();
                js.FechaSoldadura = Convert.ToDateTime(lstFecha.Selection.Text);
                js.FechaReporte = DateTime.Now;
                js.TallerID = lstTaller.Selection.Value.SafeIntParse();
                js.UsuarioModifica = SessionFacade.UserId;
                js.FechaModificacion = DateTime.Now;

                juntaWorkstatus.StartTracking();
                juntaWorkstatus.SoldaduraAprobada = true;

                SoldaduraBO.Instance.GuardaJuntaWorkstatus(juntaWorkstatus, js, new List<int>());
            }
        }

        protected void CargaComboFecha()
        {
            int diasAtras = ConfigurationManager.AppSettings["FechaDiasAtras"].SafeIntParse();
            int diasAdelante = ConfigurationManager.AppSettings["FechaDiasAdelante"].SafeIntParse();

            DateTime fechaInicial = DateTime.Now.AddDays(diasAtras * -1);
            DateTime fechaFinal = DateTime.Now.AddDays(diasAdelante);

            for (DateTime fecha = fechaInicial; fecha <= fechaFinal;  fecha = fecha.AddDays(1))
            {
                lstFecha.Items.Add(fecha.ToShortDateString());
            }

        }
        #endregion
    }
}