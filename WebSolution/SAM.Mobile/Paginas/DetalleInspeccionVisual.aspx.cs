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
using System.Configuration;
using Resources;
using System.Globalization;

namespace SAM.Mobile.Paginas
{
    public partial class DetalleInspeccionVisual : PaginaMovilAutenticado
    {
        #region Propiedades
        Spool _spool;
        OrdenTrabajoSpool _ots;
        string _fechas
        {
            get { return ViewState["Fechas"].ToString(); }
            set { ViewState["Fechas"] = value; }
        }
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

        protected void cmdAgregarDefecto_OnClick(object sender, EventArgs e)
        {
            if (lstDefectosDisponibles.Selection == null)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.InsVisual_SeleccionarDefectoDisponible;
                return;
            }
            lblError.Visible = false;
            AgregarDefectoDisponibleADefectoEncontrado();
        }

        protected void cmdRemoverDefecto_OnClick(object sender, EventArgs e)
        {
            if (lstDefectosEncontrados.Selection == null)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.InsVisual_SeleccionarDefectoEncontrado;
                return;
            }
            lblError.Visible = false;
            RemoverDefectoEncontradoADefectoDisponible();
        }

        protected void cmdOK_OnClick(object sender, EventArgs e)
        {
            //Revisar fecha de procesos
            if (_fechas != String.Empty && !cbConfirmacion.Checked && (Convert.ToDateTime(lstFecha.Selection.Text) < Convert.ToDateTime(_fechas)))
            {
                cbConfirmacion.Visible = true;
                lblError.Visible = true;
                lblError.Text = MensajesMobile.ConfirmacionFechaMenorLD;
                return;
            }
            cbConfirmacion.Visible = false;

            //Revisar que no hayan cambiado combo de junta
            if (lstNoJunta.Selection == null)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.InsVisual_SeleccionYActualización;
                return;
            }

            //Revisar que no hayan cambiado combo de junta
            if (lstNoJunta.Selection.Text == String.Empty)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.InsVisual_SeleccionJunta;
                return;
            }

            //Revisar junta existente
            if (InspeccionVisualBO.Instance.ExisteJuntaInspeccionVisual(lstNoJunta.Selection.Value.SafeIntParse()))
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.InsVisual_InspeccionRealizada;
                return;
            }

            //Revisar Formato Fecha
            if (lstFecha.Selection.Text.SafeDateAsStringParse() == null)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.InsVisual_FechaInvalida;
                return;
            }

            //Revisar que hayan seleccionado un resultado
            if (lstResultado.Selection.Text == String.Empty)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.InsVisual_SeleccionDeResultado;
                return;
            }

            //Revisamos que en caso de ser reprobado, existan errores.
            if (lstResultado.Selection.Value.SafeIntParse() == 0)
            {
                if (lstDefectosEncontrados.Items.Count == 0)
                {
                    lblError.Visible = true;
                    lblError.Text = MensajesError.InsVisual_ReprobadoNoErrores;
                    return;
                }
            }

            //En caso que haya sido aprobado, la lista de errores debe estar vacia
            if (lstResultado.Selection.Value.SafeIntParse() == 1)
            {
                if (lstDefectosEncontrados.Items.Count > 0)
                {
                    lblError.Visible = true;
                    lblError.Text = MensajesError.InsVisual_VaciarErrores;
                    return;
                }
            }

            // si la junta no es de reparación validamos que la fecha de armado sea menor a la de liberación dimensional si es que lo tiene
            JuntaWorkstatus juntaWorkStatus = ArmadoBO.Instance.ObtenerJuntaWorkstatusPorJuntaSpoolID(lstNoJunta.Selection.Value.SafeIntParse());
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
                if (Guardar())
                {
                    lblError.Text = MensajesError.InsVisual_ModificadaOK;                    
                }
                else
                {
                    lblError.Text = MensajesError.InsVisual_OK;
                }
                
                lblError.Visible = true;
               
                BorraDatos();
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
                lblError.Text = MensajesError.InsVisual_ErrorBD;
            }
        }

        protected void lstNoJunta_OnSelectedIndexChange(object sender, EventArgs e)
        {
            ViewState["juntaSpoolID"] = lstNoJunta.Selection.Value.SafeIntParse();
            string etiquetaJunta = lstNoJunta.Selection.Text;

            int jsId = ViewState["juntaSpoolID"].ToString().SafeIntParse();
            _ots = OrdenTrabajoSpoolBO.Instance.ObtenerOrdenTrabajoPorSpoolID(EntityID.Value);
            JuntaWorkstatus jws = JuntaWorkstatusBO.Instance.ObtenerPorOts(_ots.OrdenTrabajoSpoolID, jsId, etiquetaJunta);

            _fechas = String.Empty;
            if (jws != null)
            {
                _fechas = ValidaFechasBO.Instance.ObtenerFechasProcesoSoldadura(new int[] { jws.JuntaWorkstatusID });

                if (_fechas.Length > 10)
                {
                    _fechas = _fechas.Substring(0, 9);
                }
                else
                {
                    _fechas = String.Empty;
                }
            }
        }
        #endregion

        #region Metodos
        protected void BorraDatos()
        {
            ViewState["lstDefectosEncontradosBO"] = null;
            ViewState["lstDefectosBO"] = null;
            lstDefectosEncontrados.Items.Clear();
            lstDefectosDisponibles.Items.Clear();
            lstResultado.Items.Clear();
            txtObservaciones.Text = String.Empty;
        }

        protected void Carga()
        {
            _spool = SpoolBO.Instance.Obtener(EntityID.Value);
            _ots = OrdenTrabajoSpoolBO.Instance.ObtenerOrdenTrabajoPorSpoolID(EntityID.Value);
            lblNoOdt2.Text = _ots.OrdenTrabajo.NumeroOrden;
            lblNoControl2.Text = _ots.NumeroControl;
            lblSpool2.Text = _spool.Nombre; //AQUI
            CargaComboJuntas();
            CargaComboResultados();
            CargaComboDefectos();
        }

        protected void CargaComboJuntas()
        {
            lstNoJunta.Items.Clear();
            List<JuntaSpool> lstJuntaSpool = InspeccionVisualBO.Instance.ObtenerJuntasSpoolParaInspeccionVisualHH(EntityID.Value, CacheCatalogos.Instance.ShopFabAreaID, CacheCatalogos.Instance.TipoJuntaTHID, CacheCatalogos.Instance.TipoJuntaTWID);//JuntaSpoolBO.Instance.ObtenerJuntasPorSpoolID(EntityID.Value);

            lstNoJunta.DataSource = lstJuntaSpool;
            lstNoJunta.DataTextField = "Etiqueta";
            lstNoJunta.DataValueField = "JuntaSpoolID";
            lstNoJunta.DataBind();
            lstNoJunta.Items.Insert(0, String.Empty);
        }

        protected void CargaComboResultados()
        {
            lstResultado.Items.Add(new MobileListItem(MensajesMobile.Reprobado, "0"));
            lstResultado.Items.Add(new MobileListItem(MensajesMobile.Aprobado, "1"));
            lstResultado.Items.Insert(0, String.Empty);
        }

        protected void CargaComboDefectos()
        {
            if (ViewState["lstDefectosBO"] == null)
            {
                ViewState["lstDefectosBO"] = DefectoBO.Instance.ObtenerDefectosPorTipoDePruebaID(TipoPruebaEnum.InspeccionVisual.SafeIntParse());
            }

            List<Simple> defectosList = (List<Simple>)(ViewState["lstDefectosBO"]);
            lstDefectosDisponibles.DataSource = defectosList;
            lstDefectosDisponibles.DataTextField = "Valor";
            lstDefectosDisponibles.DataValueField = "ID";
            lstDefectosDisponibles.DataBind();
        }

        protected void AgregarDefectoDisponibleADefectoEncontrado()
        {
            if (ViewState["lstDefectosEncontradosBO"] == null)
            {
                ViewState["lstDefectosEncontradosBO"] = new List<Simple>();
            }

            //Agrego item a lista de defectos encontrados
            Simple item = new Simple();
            item.ID = lstDefectosDisponibles.Selection.Value.SafeIntParse();
            item.Valor = lstDefectosDisponibles.Selection.Text;
            ((List<Simple>)(ViewState["lstDefectosEncontradosBO"])).Add(item);

            //mando el bind de la lista
            lstDefectosEncontrados.Items.Clear();
            ((List<Simple>)(ViewState["lstDefectosEncontradosBO"])).OrderBy(x => x.ID);
            lstDefectosEncontrados.DataSource = ((List<Simple>)(ViewState["lstDefectosEncontradosBO"]));
            lstDefectosEncontrados.DataTextField = "Valor";
            lstDefectosEncontrados.DataValueField = "ID";
            lstDefectosEncontrados.DataBind();

            //borro de lista de defectos disponibles
            int indiceDefectoDisponible = lstDefectosDisponibles.Selection.Index;
            ((List<Simple>)(ViewState["lstDefectosBO"])).RemoveAt(indiceDefectoDisponible);
            ((List<Simple>)(ViewState["lstDefectosBO"])).OrderBy(x => x.ID);
            lstDefectosDisponibles.Items.Clear();
            lstDefectosDisponibles.DataSource = ((List<Simple>)(ViewState["lstDefectosBO"]));
            lstDefectosDisponibles.DataTextField = "Valor";
            lstDefectosDisponibles.DataValueField = "ID";
            lstDefectosDisponibles.DataBind();
        }

        protected void RemoverDefectoEncontradoADefectoDisponible()
        {
            if (ViewState["lstDefectosEncontradosBO"] == null)
            {
                ViewState["lstDefectosEncontradosBO"] = new List<Simple>();
            }

            //Agrego item a lista de defectos disponibles
            Simple item = new Simple();
            item.ID = lstDefectosEncontrados.Selection.Value.SafeIntParse();
            item.Valor = lstDefectosEncontrados.Selection.Text;
            ((List<Simple>)(ViewState["lstDefectosBO"])).Add(item);
            ((List<Simple>)(ViewState["lstDefectosBO"])).OrderBy(x => x.ID);

            //mando el bind de la lista
            lstDefectosDisponibles.Items.Clear();
            lstDefectosDisponibles.DataSource = ((List<Simple>)(ViewState["lstDefectosBO"]));
            lstDefectosDisponibles.DataTextField = "Valor";
            lstDefectosDisponibles.DataValueField = "ID";
            lstDefectosDisponibles.DataBind();

            //borro de lista de defectos encontrados
            int indiceDefectoEncontrado = lstDefectosEncontrados.Selection.Index;
            ((List<Simple>)(ViewState["lstDefectosEncontradosBO"])).RemoveAt(indiceDefectoEncontrado);
            ((List<Simple>)(ViewState["lstDefectosEncontradosBO"])).OrderBy(x => x.ID);
            lstDefectosEncontrados.Items.Clear();
            lstDefectosEncontrados.DataSource = ((List<Simple>)(ViewState["lstDefectosEncontradosBO"]));
            lstDefectosEncontrados.DataTextField = "Valor";
            lstDefectosEncontrados.DataValueField = "ID";
            lstDefectosEncontrados.DataBind();

        }

        protected bool Guardar()
        {
            bool modificada = true;
            //Verificar que la junta tenga registro en JuntaWorkstatus
            JuntaWorkstatus juntaWorkStatus = ArmadoBO.Instance.ObtenerJuntaWorkstatusPorJuntaSpoolID(ViewState["juntaSpoolID"].SafeIntParse());
            InspeccionVisualPatio inspeccionVisual = null;
            IQueryable<InspeccionVisualPatioDefecto> ivpds = null;

            if (juntaWorkStatus != null)
            {
                //Revisar si existe Inspeccion visual
                inspeccionVisual = InspeccionVisualBO.Instance.ExisteJuntaInspeccionVisualPatio(juntaWorkStatus.JuntaWorkstatusID);
            }

            if (inspeccionVisual == null)
            {
                //Generar Registro en InspeccionVisualPatio
                inspeccionVisual = new InspeccionVisualPatio();
                modificada = false;
            }
            else
            {
                ivpds = InspeccionVisualBO.Instance.ExisteInspeccionVisualPorDefecto(inspeccionVisual.InspeccionVisualPatioID).AsQueryable();              
            }

            inspeccionVisual.StartTracking();
            inspeccionVisual.FechaInspeccion = Convert.ToDateTime(lstFecha.Selection.Text);
            inspeccionVisual.Observaciones = txtObservaciones.Text;
            inspeccionVisual.FechaModificacion = DateTime.Now;
            inspeccionVisual.UsuarioModifica = SessionFacade.UserId;

            if (lstResultado.Selection.Value.SafeIntParse() == 1)
            {
                inspeccionVisual.Aprobado = true;                
            }
            else
            {
                List<Simple> itemList = ((List<Simple>)(ViewState["lstDefectosEncontradosBO"]));
               
         
                foreach (Simple item in itemList)
                {
                    InspeccionVisualPatioDefecto ivpd = null;

                    if (ivpds != null)
                    {
                        ivpd = ivpds.Where(x => x.DefectoID == item.ID).FirstOrDefault();
                    }                      

                    if (ivpd == null)
                    {
                        ivpd = new InspeccionVisualPatioDefecto();
                    }

                    ivpd.StartTracking();
                    ivpd.DefectoID = item.ID;
                    ivpd.FechaModificacion = DateTime.Now;
                    ivpd.UsuarioModifica = SessionFacade.UserId;
                                       
                    inspeccionVisual.InspeccionVisualPatioDefecto.Add(ivpd); 
                }                
            }

            //creamos o editamos junta workstatus
            if (juntaWorkStatus == null)
            {
                int juntaSpoolID = ViewState["juntaSpoolID"].SafeIntParse();
                _ots = OrdenTrabajoSpoolBO.Instance.ObtenerOrdenTrabajoPorSpoolID(EntityID.Value);
                JuntaSpool juntaSpool = JuntaSpoolBO.Instance.Obtener(juntaSpoolID);

                //No existe, hay que crear registro
                juntaWorkStatus = new JuntaWorkstatus();
                juntaWorkStatus.OrdenTrabajoSpoolID = _ots.OrdenTrabajoSpoolID;
                juntaWorkStatus.JuntaSpoolID = juntaSpoolID;
                juntaWorkStatus.EtiquetaJunta = juntaSpool.Etiqueta;
                juntaWorkStatus.ArmadoAprobado = false;
                juntaWorkStatus.SoldaduraAprobada = false;
                juntaWorkStatus.InspeccionVisualAprobada = inspeccionVisual.Aprobado;
                juntaWorkStatus.VersionJunta = 1;
                juntaWorkStatus.JuntaFinal = true;
                juntaWorkStatus.ArmadoPagado = false;
                juntaWorkStatus.SoldaduraPagada = false;
                juntaWorkStatus.UltimoProcesoID = UltimoProcesoEnum.InspeccionVisual.SafeIntParse();
                juntaWorkStatus.UsuarioModifica = SessionFacade.UserId;
                juntaWorkStatus.FechaModificacion = DateTime.Now;
            }
            else
            {
                juntaWorkStatus.StartTracking();
                juntaWorkStatus.UsuarioModifica = SessionFacade.UserId;
                juntaWorkStatus.FechaModificacion = DateTime.Now;
                juntaWorkStatus.InspeccionVisualAprobada = inspeccionVisual.Aprobado;
                juntaWorkStatus.UltimoProcesoID = UltimoProcesoEnum.InspeccionVisual.SafeIntParse();
                juntaWorkStatus.InspeccionVisualPatio.Add(inspeccionVisual);
       
            }

            InspeccionVisualBO.Instance.GuardaJuntaWorkstatus(juntaWorkStatus, inspeccionVisual );
            return modificada;
        }

        protected void CargaComboFecha()
        {
            int diasAtras = ConfigurationManager.AppSettings["FechaDiasAtras"].SafeIntParse();
            int diasAdelante = ConfigurationManager.AppSettings["FechaDiasAdelante"].SafeIntParse();

            DateTime fechaInicial = DateTime.Now.AddDays(diasAtras * -1);
            DateTime fechaFinal = DateTime.Now.AddDays(diasAdelante);

            for (DateTime fecha = fechaInicial; fecha <= fechaFinal; fecha = fecha.AddDays(1))
            {
                lstFecha.Items.Add(fecha.ToShortDateString());
            }

        }
        #endregion
    }
}