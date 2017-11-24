using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.MobileControls;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using Resources;
using SAM.BusinessLogic.Administracion;
using SAM.BusinessLogic.Workstatus;
using SAM.BusinessObjects.Administracion;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Produccion;
using SAM.BusinessObjects.Proyectos;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities;
using SAM.Entities.Cache;
using SAM.Entities.Personalizadas;
using SAM.Mobile.Clases;
using SAM.Mobile.Paginas.App_LocalResources;
using System.Globalization;

namespace SAM.Mobile.Paginas
{
    public partial class DetalleSoldadura : PaginaMovilAutenticado
    {
        #region Propiedades
        Spool _spool;
        OrdenTrabajoSpool _ots;

        public bool WpsDiferentesPresionado = false;

        private int _proyectoID
        {
            get
            {
                if (ViewState["ProyectoID"] == null)
                {
                    ViewState["ProyectoID"] = -1;
                }
                return ViewState["ProyectoID"].SafeIntParse();
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }
        }

        private bool TerminadoConRaiz
        {
            get
            {
                if (ViewState["TerminadoConRaiz"] == null)
                {
                    ViewState["TerminadoConRaiz"] = false;
                }
                return ViewState["TerminadoConRaiz"].SafeBoolParse();
            }
            set
            {
                ViewState["TerminadoConRaiz"] = value;
            }
            
        }

        private int _procesoRellenoID
        {
            get
            {
                if (ViewState["procesoRellenoID"] == null)
                {
                    ViewState["procesoRellenoID"] = -1;
                }
                return ViewState["procesoRellenoID"].SafeIntParse();
            }
            set
            {
                ViewState["procesoRellenoID"] = value;
            }
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
                    CrearListaSoldadores();
                }
            }
        }

        protected void lstNoJunta_OnSelectedIndexChange(object sender, EventArgs e)
        {
            ViewState["juntaSpoolID"] = lstNoJunta.Selection.Value.SafeIntParse();
            if (SoldaduraBO.Instance.ExisteJuntaSoldadura(ViewState["juntaSpoolID"].SafeIntParse()) == false)
            {
                lblError.Visible = false;
                CargaMateriales();
                BorraDatos();
                CrearListaSoldadores();
                CargaBlancos();
                pnlGeneral.Visible = true;
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Soldadura_JuntaSoldada;
                lblMensaje.Visible = false;
                lstNoJunta.SelectedIndex = -1;
                pnlGeneral.Visible = false;
            }
        }

        protected void cmUpdateWPS_OnClick(object sender, EventArgs e)
        {

            if (SoldaduraBO.Instance.ExisteJuntaSoldadura(ViewState["juntaSpoolID"].SafeIntParse()) == true)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Soldadura_JuntaSoldada;
                lstNoJunta.SelectedIndex = -1;
                return;
            }

            if (lstProcRaiz.Selection.Text == String.Empty && TerminadoConRaiz == false)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Soldadura_SeleccionarProcesoRaiz;
                return;
            }

            if (lstProcRelleno.Selection.Text == String.Empty && TerminadoConRaiz == false)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Soldadura_SeleccionarProcesoRelleno;
                return;
            }

            lblError.Visible = true;
            CargaComboWpsRaizYRelleno();
        }

        protected void lstWpsRaiz_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["wpsID"] = lstWpsRaiz.Selection != null ? lstWpsRaiz.Selection.Value.SafeIntParse() : 0;
            if (ViewState["wpsID"] != null || ViewState["wpsID"].SafeIntParse() != 0)
            {
                lblError.Visible = false;
                CargaComboSoldadorRaiz();
                tlbSoldadorRaiz.Visible = true;
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Soldadora_SeleccionarWPS;
                tlbSoldadorRaiz.Visible = false;
            }
        }

        protected void lstWpsRelleno_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["wpsRellenoID"] = lstWpsRelleno.Selection != null ? lstWpsRelleno.Selection.Value.SafeIntParse() : 0;
            if (ViewState["wpsRellenoID"] != null || ViewState["wpsRellenoID"].SafeIntParse() != 0)
            {
                lblError.Visible = false;
                CargaComboSoldadorRelleno();
                tblSolRelleno.Visible = true;
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Soldadora_SeleccionarWPS;
                tblSolRelleno.Visible = false;
            }
        }

        protected void cmdAgregarSolRelleno_OnClick(object sender, EventArgs e)
        {
            ViewState["SoldadorRellenoID"] = lstSoldadorRelleno.Selection.Value.SafeIntParse();

            if (ViewState["SoldadorRellenoID"] == null || ViewState["SoldadorRellenoID"].SafeIntParse() == -1)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Soldadura_SeleccionarSoldadorRelleno;
                return;
            }

            if (txtColadaSoldadorRelleno.Text.Trim() == String.Empty)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Soldadura_AgregarColada;
                return;
            }

            if (RevisarColada(txtColadaSoldadorRelleno.Text) == false)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Soldadura_ColadaNoExiste;
                return;
            }

            if (RevisarSoldadorRellenoExistente(ViewState["SoldadorRellenoID"].SafeIntParse()) == false)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Soldadura_SoldadorColadaDuplicado;
                return;
            }

            lblError.Visible = true;
            AgregarSoldadorRelleno();

            txtColadaSoldadorRelleno.Text = String.Empty;
            lstSoldadorRelleno.SelectedIndex = -1;
        }

        protected void cmdAgregarSolRaiz_OnClick(object sender, EventArgs e)
        {
            ViewState["SoldadorRaizID"] = lstSoldadorRaiz.Selection.Value.SafeIntParse();

            if (ViewState["SoldadorRaizID"] == null || ViewState["SoldadorRaizID"].SafeIntParse() == -1)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Soldadura_SeleccionarSoldadorRaiz;
                return;
            }

            if (txtColadaSoldadorRaiz.Text.Trim() == String.Empty)
            {
                lblError.Text = MensajesError.Soldadura_AgregarColada;
                return;
            }

            if (RevisarColada(txtColadaSoldadorRaiz.Text) == false)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Soldadura_ColadaNoExiste;
                return;
            }

            if (RevisarSoldadorRaizExistente(ViewState["SoldadorRaizID"].SafeIntParse()) == false)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Soldadura_SoldadorColadaDuplicado;
                return;
            }

            lblError.Visible = false;
            AgregarSoldadorRaiz();

            txtColadaSoldadorRaiz.Text = String.Empty;
            lstSoldadorRaiz.SelectedIndex = -1;
        }

        protected void oblstSoldadoresRelleno_CommandClick(object sender, ObjectListCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int indice = e.ListItem.Index;

                string codigo = oblstSoldadoresRelleno.Items[indice]["Codigo"].SafeStringParse();
                string colada = oblstSoldadoresRelleno.Items[indice]["Colada"].SafeStringParse();

                List<SoldadorMobile> tempList = (List<SoldadorMobile>)ViewState["objSoldadoresRellenoBO"];
                int indiceLista = tempList.FindIndex(x => x.Codigo == codigo && x.Colada == colada);

                tempList.RemoveAt(indiceLista);

                oblstSoldadoresRelleno.Items.Clear();
                oblstSoldadoresRelleno.DataSource = tempList;
                oblstSoldadoresRelleno.DataBind();
            }
        }

        protected void oblstSoldadoresRelleno_ItemCommands_Show(Object sender, ObjectListShowCommandsEventArgs e)
        {
            if (oblstSoldadoresRelleno.Commands.Count < 1)
                oblstSoldadoresRelleno.Commands.Add(new
                    ObjectListCommand("Borrar", "Borrar"));


        }

        protected void oblstSoldadoresRaiz_CommandClick(object sender, ObjectListCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int indice = e.ListItem.Index;

                string codigo = oblstSoldadoresRaiz.Items[indice]["Codigo"].SafeStringParse();
                string colada = oblstSoldadoresRaiz.Items[indice]["Colada"].SafeStringParse();

                List<SoldadorMobile> tempList = (List<SoldadorMobile>)ViewState["objSoldadoresRaizBO"];
                int indiceLista = tempList.FindIndex(x => x.Codigo == codigo && x.Colada == colada);

                tempList.RemoveAt(indiceLista);

                oblstSoldadoresRaiz.Items.Clear();
                oblstSoldadoresRaiz.DataSource = tempList;
                oblstSoldadoresRaiz.DataBind();
            }
        }

        protected void oblstSoldadoresRaiz_ItemCommands_Show(Object sender, ObjectListShowCommandsEventArgs e)
        {
            if (oblstSoldadoresRaiz.Commands.Count < 1)
                oblstSoldadoresRaiz.Commands.Add(new
                    ObjectListCommand("Borrar", "Borrar"));
        }

        protected void cmdOK_OnClick(object sender, EventArgs e)
        {
            bool EsValido = true;

            //Revisar que no hayan cambiado combo de junta
            if (lstNoJunta.Selection == null)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Soldadura_SeleccionYActualización;
                EsValido = false;
            }

            //Revisar que no hayan cambiado combo de junta
            if (lstNoJunta.Selection.Text == String.Empty)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Soldadura_SeleccionJunta;
                EsValido = false;
            }

            //Revisar junta existente
            if (SoldaduraBO.Instance.ExisteJuntaSoldadura(lstNoJunta.Selection.Value.SafeIntParse()))
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Soldadura_JuntaSoldada;
                EsValido = false;
            }

            //Revisar Taller
            if (lstTaller.Selection.Text == String.Empty)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Soldadura_SeleccionarTaller;
                EsValido = false;
            }

            //Revisar Formato Fecha
            if (lstFecha.Selection.Text.SafeDateAsStringParse() == null)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Soldadura_FechaInvalida;
                EsValido = false;
            }

            //Revisar al menos un Soldador Relleno

            if (((List<SoldadorMobile>)(ViewState["objSoldadoresRellenoBO"])).Count == 0 && TerminadoConRaiz == false)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Soldadura_AlMenosUnSoldadorRelleno;
                EsValido = false;
            }

            //Revisar al menos un Soldador Raiz
            if (((List<SoldadorMobile>)(ViewState["objSoldadoresRaizBO"])).Count == 0 && TerminadoConRaiz == false)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.Soldadura_AlMenosUnSoldadorRaiz;
                EsValido = false;
            }

            if (WpsDiferentesPresionado)
            {
                lblError.Visible = false;
            }

            //Verificar que la fecha de soldadura sea mayor a la de armado en caso de que exista
            JuntaWorkstatus juntaWorkStatus = ArmadoBO.Instance.ObtenerJuntaWorkstatusPorJuntaSpoolID(ViewState["juntaSpoolID"].SafeIntParse());
            if (juntaWorkStatus != null && juntaWorkStatus.ArmadoAprobado)
            {
                string fechaProcArmado = ValidaFechasBO.Instance.ObtenerFechaProcesoArmado(juntaWorkStatus.JuntaWorkstatusID);
                DateTime fechaProcArm = Convert.ToDateTime(fechaProcArmado);

                string fechaProcSoldadura = Convert.ToDateTime(lstFecha.Selection.Text).ToShortDateString();
                DateTime fechaProcSold = Convert.ToDateTime(fechaProcSoldadura);

                if (fechaProcArm > fechaProcSold)
                {
                    lblError.Visible = true;
                    lblError.Text = MensajesError.Armado_FechaMayorSoldadura;
                    return;
                }
            }

            // si la junta no es de reparación validamos que la fecha de soldadura sea menor a la de liberación dimensional si es que lo tiene
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

                DateTime fechaSoldadura = new DateTime();
                if (CultureInfo.CurrentCulture.Name == "en-US")
                {
                    DateTime.TryParse(Convert.ToDateTime(lstFecha.Selection.Text).ToString("MM/dd/yyyy"), out fechaSoldadura);
                }
                else
                {
                    DateTime.TryParse(Convert.ToDateTime(lstFecha.Selection.Text).ToString("dd/MM/yyyy"), out fechaSoldadura);
                }

                if (fechaSoldadura > tempFechaLiberacion)
                {
                    lblError.Visible = true;
                    EsValido = false;
                    lblError.Text = MensajesError.Excepcion_FechaProcAnteriorMayorLiberacion;
                    return;
                }
            }

            if (EsValido)
            {
                //Guardar
                try
                {
                    Guardar();
                    lblMensaje.Visible = true;
                    lblMensaje.Text = MensajesError.Soldadura_OK;
                    BorraDatos();
                    Carga();
                    CrearListaSoldadores();
                }
                catch (BaseValidationException bve)
                {
                    lblError.Visible = true;
                    lblError.Text = bve.Details[0].ToString();
                }
                catch (Exception ex)
                {
                    lblError.Visible = true;
                    lblError.Text = MensajesError.Soldadura_ErrorBD;
                }
            }
        }

        protected void slWpsDiferentes_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            WpsDiferentesPresionado = true;
            lstWpsRaiz.Items.Clear();
            lstWpsRelleno.Items.Clear();
            lstWpsRaiz.Items.Insert(0, String.Empty);
            lstWpsRelleno.Items.Insert(0, String.Empty);
        }
        #endregion

        #region Metodos
        protected void BorraDatos()
        {
            ViewState["objSoldadoresRellenoBO"] = null;
            ViewState["objSoldadoresRaizBO"] = null;
            ViewState["wpsID"] = null;
            ViewState["wpsRellenoID"] = null;
            TerminadoConRaiz = false;
            lstWpsRaiz.Items.Clear();
            lstWpsRelleno.Items.Clear();
            oblstSoldadoresRaiz.Items.Clear();
            oblstSoldadoresRelleno.Items.Clear();
            lstSoldadorRaiz.Items.Clear();
            lstSoldadorRelleno.Items.Clear();
            txtObservaciones.Text = String.Empty;
            lblError.Text = string.Empty;
        }

        protected void Carga()
        {
            _spool = SpoolBO.Instance.Obtener(EntityID.Value);
            _ots = OrdenTrabajoSpoolBO.Instance.ObtenerOrdenTrabajoPorSpoolID(EntityID.Value);
            _proyectoID = _spool.ProyectoID;

            lblNoOdt2.Text = _ots.OrdenTrabajo.NumeroOrden;
            lblNoControl2.Text = _ots.NumeroControl;
            lblSpool2.Text = _spool.Nombre; //AQUI
            lblPwhtTexto.Text = TraductorEnumeraciones.TextoSiNo(_spool.RequierePwht);
            CargaComboJuntas();
            CargaComboTalleres();
            CargaProcesos();
            CargaBlancos();
        }

        protected void CargaBlancos()
        {
            lstWpsRaiz.Items.Insert(0, String.Empty);
            lstWpsRelleno.Items.Insert(0, String.Empty);
            lstSoldadorRaiz.Items.Insert(0, String.Empty);
            lstSoldadorRelleno.Items.Insert(0, String.Empty);
        }

        protected void CargaComboJuntas()
        {
            lstNoJunta.Items.Clear();
            List<Simple> lstJuntaSpool = JuntaSpoolBO.Instance.ObtenerJuntasPorSpoolIDYCodigoFabAreaFiltroTiposSoldables(EntityID.Value, CacheCatalogos.Instance.ShopFabAreaID, CacheCatalogos.Instance.TipoJuntaTHID, CacheCatalogos.Instance.TipoJuntaTWID);
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

        protected void CargaMateriales()
        {
            int juntaSpoolID = ViewState["juntaSpoolID"].SafeIntParse();
            if (juntaSpoolID != -1)
            {
                JuntaSpool juntaSpool = JuntaSpoolBO.Instance.Obtener(juntaSpoolID);

                List<FamAceroCache> lstFamAceros = CacheCatalogos.Instance.ObtenerFamiliasAcero();
                lblMaterialBase1.Text = lstFamAceros.Where(x => x.ID == juntaSpool.FamiliaAceroMaterial1ID).Select(y => y.Nombre).SingleOrDefault();
                lblMaterialBase2.Text = juntaSpool.FamiliaAceroMaterial2ID != null ? lstFamAceros.Where(x => x.ID == juntaSpool.FamiliaAceroMaterial2ID).Select(y => y.Nombre).SingleOrDefault() : lblMaterialBase1.Text;
                ViewState["MaterialBase1"] = lstFamAceros.Where(x => x.ID == juntaSpool.FamiliaAceroMaterial1ID).Select(y => y.ID).SingleOrDefault();
                ViewState["MaterialBase1Nombre"] = lstFamAceros.Where(x => x.ID == juntaSpool.FamiliaAceroMaterial1ID).Select(y => y.Nombre).SingleOrDefault();
                ViewState["MaterialBase2"] = lstFamAceros.Where(x => x.ID == juntaSpool.FamiliaAceroMaterial2ID).Select(y => y.ID).SingleOrDefault();
                ViewState["MaterialBase2Nombre"] = lblMaterialBase2.Text;
            }

        }

        protected void CargaProcesos()
        {
            lstProcRaiz.DataSource = CacheCatalogos.Instance.ObtenerProcesosRaiz();
            lstProcRaiz.DataTextField = "Nombre";
            lstProcRaiz.DataValueField = "ID";
            lstProcRaiz.DataBind();
            lstProcRaiz.Items.Insert(0, String.Empty);

            lstProcRelleno.DataSource = CacheCatalogos.Instance.ObtenerProcesosRelleno();
            lstProcRelleno.DataTextField = "Nombre";
            lstProcRelleno.DataValueField = "ID";
            lstProcRelleno.DataBind();
            lstProcRelleno.Items.Insert(0, String.Empty);
        }

        protected void CargaComboWpsRaizYRelleno()
        {
            int juntaSpooID = lstNoJunta.Selection.Value.SafeIntParse();
            OrdenTrabajoJunta odtJ = SoldaduraBO.Instance.ObtenerInformacionParaSoldadura(juntaSpooID);

            if (TerminadoConRaiz)
            {
                CargaWpsTerminadoConRaiz(lstProcRaiz.Selection.Value.SafeIntParse(),
                         ViewState["MaterialBase1Nombre"].SafeStringParse(), 
                         ViewState["MaterialBase2Nombre"].SafeStringParse(),
                         odtJ.JuntaSpool.Espesor);
            }
            else
            {
                CargaWps(lstProcRaiz.Selection.Value.SafeIntParse(), lstProcRelleno.Selection.Value.SafeIntParse(),
                         ViewState["MaterialBase1Nombre"].SafeStringParse(), ViewState["MaterialBase2Nombre"].SafeStringParse(),
                         odtJ.JuntaSpool.Espesor, slWpsDiferentes.Items[0].Selected);
            }
        }

        protected void CargaComboSoldadorRaiz()
        {
            if (ViewState["wpsID"] != null || ViewState["wpsID"].SafeIntParse() != 0)
            {
                List<Simple> lstSoldadorRaizBO = SoldaduraBO.Instance.ObtenerSoldadoresParaSoldadura(SessionFacade.PatioID.Value, ViewState["wpsID"].SafeIntParse());
                lstSoldadorRaiz.DataSource = lstSoldadorRaizBO;
                lstSoldadorRaiz.DataTextField = "Valor";
                lstSoldadorRaiz.DataValueField = "ID";
                lstSoldadorRaiz.DataBind();
                lstSoldadorRaiz.Items.Insert(0, String.Empty);
            }
        }

        protected void CargaComboSoldadorRelleno()
        {
            if (ViewState["wpsRellenoID"] != null || ViewState["wpsRellenoID"].SafeIntParse() != 0)
            {
                List<Simple> lstSoldadorRellenoBO = SoldaduraBO.Instance.ObtenerSoldadoresParaSoldadura(SessionFacade.PatioID.Value, ViewState["wpsRellenoID"].SafeIntParse());
                lstSoldadorRelleno.DataSource = lstSoldadorRellenoBO;
                lstSoldadorRelleno.DataTextField = "Valor";
                lstSoldadorRelleno.DataValueField = "ID";
                lstSoldadorRelleno.DataBind();
                lstSoldadorRelleno.Items.Insert(0, String.Empty);
            }
        }

        protected void CrearListaSoldadores()
        {
            if (ViewState["objSoldadoresRellenoBO"] == null)
            {
                ViewState["objSoldadoresRellenoBO"] = new List<SoldadorMobile>();
            }

            if (ViewState["objSoldadoresRaizBO"] == null)
            {
                ViewState["objSoldadoresRaizBO"] = new List<SoldadorMobile>();
            }
        }

        protected void AgregarSoldadorRelleno()
        {
            oblstSoldadoresRelleno.Items.Clear();

            SoldadorMobile miSoldadorRelleno = new SoldadorMobile();
            miSoldadorRelleno.ID = ViewState["SoldadorRellenoID"].SafeIntParse();
            miSoldadorRelleno.Nombre = SoldadorBO.Instance.Obtener(ViewState["SoldadorRellenoID"].SafeIntParse()).Nombre;
            miSoldadorRelleno.Codigo = SoldadorBO.Instance.Obtener(ViewState["SoldadorRellenoID"].SafeIntParse()).Codigo;
            miSoldadorRelleno.Colada = txtColadaSoldadorRelleno.Text;

            List<SoldadorMobile> tempList = (List<SoldadorMobile>)ViewState["objSoldadoresRellenoBO"];
            tempList.Add(miSoldadorRelleno);

            oblstSoldadoresRelleno.Fields[0].Title = MensajesError.Tabla_Codigo;
            oblstSoldadoresRelleno.Fields[1].Title = MensajesError.Tabla_Nombre;
            oblstSoldadoresRelleno.Fields[2].Title = MensajesError.Tabla_Colada;
            oblstSoldadoresRelleno.DataSource = tempList;
            oblstSoldadoresRelleno.DataBind();
        }

        protected void AgregarSoldadorRaiz()
        {
            oblstSoldadoresRaiz.Items.Clear();

            SoldadorMobile miSoldadorRaiz = new SoldadorMobile();
            miSoldadorRaiz.ID = ViewState["SoldadorRaizID"].SafeIntParse();
            miSoldadorRaiz.Nombre = SoldadorBO.Instance.Obtener(ViewState["SoldadorRaizID"].SafeIntParse()).Nombre;
            miSoldadorRaiz.Codigo = SoldadorBO.Instance.Obtener(ViewState["SoldadorRaizID"].SafeIntParse()).Codigo;
            miSoldadorRaiz.Colada = txtColadaSoldadorRaiz.Text;

            List<SoldadorMobile> tempList = (List<SoldadorMobile>)ViewState["objSoldadoresRaizBO"];
            tempList.Add(miSoldadorRaiz);

            oblstSoldadoresRaiz.Fields[0].Title = MensajesError.Tabla_Codigo;
            oblstSoldadoresRaiz.Fields[1].Title = MensajesError.Tabla_Nombre;
            oblstSoldadoresRaiz.Fields[2].Title = MensajesError.Tabla_Colada;
            oblstSoldadoresRaiz.DataSource = tempList;
            oblstSoldadoresRaiz.DataBind();
        }

        protected bool RevisarColada(string codigoColada)
        {
            if (SoldaduraBO.Instance.ExisteCodigoColadaEnPatio(codigoColada, SessionFacade.PatioID.Value))
            {
                return true;
            }
            return false;
        }

        protected bool RevisarSoldadorRellenoExistente(int soldadorID)
        {
            List<SoldadorMobile> tempList = (List<SoldadorMobile>)ViewState["objSoldadoresRellenoBO"];
            if (tempList.Any(x => x.ID == soldadorID))
            {
                return false;
            }
            return true;
        }

        protected bool RevisarSoldadorRaizExistente(int soldadorID)
        {
            List<SoldadorMobile> tempList = (List<SoldadorMobile>)ViewState["objSoldadoresRaizBO"];
            if (tempList.Any(x => x.ID == soldadorID))
            {
                return false;
            }
            return true;
        }

        protected void Guardar()
        {
            //Verificar que la junta tenga registro en JuntaWorkstatus
            JuntaWorkstatus juntaWorkStatus = ArmadoBO.Instance.ObtenerJuntaWorkstatusPorJuntaSpoolID(ViewState["juntaSpoolID"].SafeIntParse());

            //Generar registro en JuntaSoldadura
            JuntaSoldadura juntaSoldadura = new JuntaSoldadura();
            juntaSoldadura.FechaSoldadura = Convert.ToDateTime(lstFecha.Selection.Text);
            juntaSoldadura.FechaReporte = DateTime.Now;
            juntaSoldadura.TallerID = lstTaller.Selection.Value.SafeIntParse();
            juntaSoldadura.ProcesoRaizID = lstProcRaiz.Selection.Value.SafeIntParse();
            juntaSoldadura.WpsID = lstWpsRaiz.Selection.Value.SafeIntParse();
            juntaSoldadura.Observaciones = txtObservaciones.Text;

            if (TerminadoConRaiz)
            {
                juntaSoldadura.ProcesoRellenoID = _procesoRellenoID;
                juntaSoldadura.WpsRellenoID = lstWpsRaiz.Selection.Value.SafeIntParse();
            }
            else
            {
                juntaSoldadura.ProcesoRellenoID = lstProcRelleno.Selection.Value.SafeIntParse();
                juntaSoldadura.WpsRellenoID = lstWpsRelleno.Selection.Value.SafeIntParse();
            }
            List<SoldadorMobile> tempListSoldadorRelleno;
            //Generar JunstaSoldadorDetalle para cada soldador
            if (TerminadoConRaiz)
            {
                tempListSoldadorRelleno = (List<SoldadorMobile>)ViewState["objSoldadoresRaizBO"];
            }
            else
            {
                tempListSoldadorRelleno = (List<SoldadorMobile>)ViewState["objSoldadoresRellenoBO"];
            }

            foreach (SoldadorMobile soldador in tempListSoldadorRelleno)
            {
                JuntaSoldaduraDetalle jsd = new JuntaSoldaduraDetalle();
                jsd.ConsumibleID = SoldaduraBO.Instance.ObtenerColadaIDporCodigoYPatioID(soldador.Colada, SessionFacade.PatioID.Value);
                jsd.SoldadorID = soldador.ID;
                jsd.TecnicaSoldadorID = TecnicaSoldadorEnum.Relleno.SafeIntParse();
                jsd.UsuarioModifica = SessionFacade.UserId;
                jsd.FechaModificacion = DateTime.Now;
                juntaSoldadura.JuntaSoldaduraDetalle.Add(jsd);
            }

            List<SoldadorMobile> tempListSoldadorRaiz = (List<SoldadorMobile>)ViewState["objSoldadoresRaizBO"];
            foreach (SoldadorMobile soldador in tempListSoldadorRaiz)
            {
                JuntaSoldaduraDetalle jsd = new JuntaSoldaduraDetalle();
                jsd.ConsumibleID = SoldaduraBO.Instance.ObtenerColadaIDporCodigoYPatioID(soldador.Colada, SessionFacade.PatioID.Value);
                jsd.SoldadorID = soldador.ID;
                jsd.TecnicaSoldadorID = TecnicaSoldadorEnum.Raiz.SafeIntParse();
                jsd.UsuarioModifica = SessionFacade.UserId;
                jsd.FechaModificacion = DateTime.Now;
                juntaSoldadura.JuntaSoldaduraDetalle.Add(jsd);
            }

            _ots = OrdenTrabajoSpoolBO.Instance.ObtenerOrdenTrabajoPorSpoolID(EntityID.Value);
                
            //creamos o editamos junta workstatus
            if (juntaWorkStatus == null)
            {
                int juntaSpoolID = ViewState["juntaSpoolID"].SafeIntParse();
                JuntaSpool juntaSpool = JuntaSpoolBO.Instance.Obtener(juntaSpoolID);

                //No existe, hay que crear registro
                juntaWorkStatus = new JuntaWorkstatus();
                juntaWorkStatus.OrdenTrabajoSpoolID = _ots.OrdenTrabajoSpoolID;
                juntaWorkStatus.JuntaSpoolID = juntaSpoolID;
                juntaWorkStatus.EtiquetaJunta = juntaSpool.Etiqueta;
                juntaWorkStatus.ArmadoAprobado = false;
                juntaWorkStatus.SoldaduraAprobada = true;
                juntaWorkStatus.InspeccionVisualAprobada = false;
                juntaWorkStatus.VersionJunta = 1;
                juntaWorkStatus.JuntaFinal = true;
                juntaWorkStatus.ArmadoPagado = false;
                juntaWorkStatus.SoldaduraPagada = false;
                juntaWorkStatus.UltimoProcesoID = UltimoProcesoEnum.Soldado.SafeIntParse();
                juntaWorkStatus.UsuarioModifica = SessionFacade.UserId;
                juntaWorkStatus.FechaModificacion = DateTime.Now;
            }
            else
            {
                juntaWorkStatus.StartTracking();
                juntaWorkStatus.UsuarioModifica = SessionFacade.UserId;
                juntaWorkStatus.FechaModificacion = DateTime.Now;
                juntaWorkStatus.SoldaduraAprobada = true;
                juntaWorkStatus.UltimoProcesoID = UltimoProcesoEnum.Soldado.SafeIntParse();
            }

            List<int> lista = new List<int>();
            SoldaduraBO.Instance.GuardaJuntaWorkstatus(juntaWorkStatus, juntaSoldadura, lista);

            //Consultamos si la Junta tiene armado sino para generar el pendiente
            bool tieneArmado;
            tieneArmado = ArmadoBO.Instance.ObtenerJuntaWorkstatusPorID(juntaWorkStatus.JuntaWorkstatusID);

            if (!tieneArmado)
            {
                int categoriaPendienteID = (int)CategoriaPendienteEnum.Produccion;
                int tipoPendienteID = (int)TipoPendienteEnum.SoldaduraSinArmado;
                string nombreProyecto = ProyectoBO.Instance.Obtener(_proyectoID).Nombre;
                string idiomaUsuario = UsuarioBO.Instance.Obtener(SessionFacade.UserId).Idioma;

                Pendiente p = new Pendiente();

                p.StartTracking();
                p.ProyectoID = _proyectoID;
                p.TipoPendienteID = tipoPendienteID;
                p.Estatus = EstatusPendiente.Abierto;
                p.FechaApertura = DateTime.Now;
                p.GeneradoPor = SessionFacade.UserId;
                p.FechaModificacion = DateTime.Now;

                //Obtenemos el usuario responsable
                ProyectoPendiente pp = ProyectoPendienteBO.Instance.ObtenerPorProyectoIDyTipoPendiente(_proyectoID, tipoPendienteID);

                if (pp != null)
                {
                    p.AsignadoA = pp.Responsable;
                    p.CategoriaPendienteID = categoriaPendienteID;

                    TipoPendiente tipo = TipoPendienteBO.Instance.Obtener(tipoPendienteID);

                    p.Descripcion = string.Format(MensajesMobile.NumeroControlEtiqueta, _ots.NumeroControl, juntaWorkStatus.EtiquetaJunta);
                    p.Titulo = LanguageHelper.INGLES == idiomaUsuario ? tipo.NombreIngles : tipo.Nombre;
                }

                PendienteDetalle pd = new PendienteDetalle();

                pd.CategoriaPendienteID = categoriaPendienteID;
                pd.EsAlta = true;
                pd.Responsable = pp.Responsable;
                pd.Estatus = EstatusPendiente.Abierto;
                pd.UsuarioModifica = SessionFacade.UserId;
                pd.FechaModificacion = DateTime.Now;

                p.StopTracking();
                p.PendienteDetalle.Add(pd);

                PendienteBL.Instance.Guarda(p, pp.Responsable, nombreProyecto, true);
            }

            pnlGeneral.Visible = false;

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

        protected void CargaWpsTerminadoConRaiz(int procesoRaizID, string material1, string material2, decimal? espesorjunta)
        {
            if (procesoRaizID > 0)
            {
                // Cargamos raíz
                IEnumerable<WpsCache>  origen = SoldaduraBL.Instance.ObtenerWpsterminadoConRaiz(
                        ViewState["juntaSpoolID"].SafeIntParse(),
                        _proyectoID,
                        procesoRaizID,
                        material1,
                        material2, espesorjunta);

                    lstWpsRaiz.DataSource = origen;
                    lstWpsRaiz.DataTextField = "Nombre";
                    lstWpsRaiz.DataValueField = "ID";
                    lstWpsRaiz.DataBind();
                    lstWpsRaiz.Items.Insert(0, String.Empty);

                    _procesoRellenoID = (from wps in origen
                                         where wps.ProcesoRellenoNombre.Equals(wps.ProcesoRaizNombre)
                                         && wps.ProcesoRaizID.Equals(procesoRaizID)
                                         select wps.ProcesoRellenoID).FirstOrDefault();
            }
        }

        protected void CargaWps(int procesoRaizID, int procesoRellenoID, string material1, string material2, decimal? espesorJunta, bool wpsDiferentes)
        {
            if (wpsDiferentes)
            {
                if (procesoRellenoID > 0)
                {
                    // Cargamos relleno
                    IEnumerable<WpsCache> origen = SoldaduraBL.Instance.ObtenerWps(
                                                     ViewState["juntaSpoolID"].SafeIntParse(),
                                                     _proyectoID,
                                                     procesoRaizID,
                                                     procesoRellenoID,
                                                     material1,
                                                     material2,
                                                     espesorJunta,
                                                     wpsDiferentes,
                                                     false);

                    lstWpsRelleno.DataSource = origen;
                    lstWpsRelleno.DataTextField = "Nombre";
                    lstWpsRelleno.DataValueField = "ID";
                    lstWpsRelleno.DataBind();
                    lstWpsRelleno.Items.Insert(0, String.Empty);

                    // Cargamos raíz
                    origen = SoldaduraBL.Instance.ObtenerWps(
                            ViewState["juntaSpoolID"].SafeIntParse(),
                            _proyectoID,
                            procesoRaizID,
                            procesoRellenoID,
                            material1,
                            material2,
                            espesorJunta,
                            wpsDiferentes,
                            true);

                    lstWpsRaiz.DataSource = origen;
                    lstWpsRaiz.DataTextField = "Nombre";
                    lstWpsRaiz.DataValueField = "ID";
                    lstWpsRaiz.DataBind();
                    lstWpsRaiz.Items.Insert(0, String.Empty);
                }
            }
            else
            {
                if (procesoRellenoID > 0 && procesoRaizID > 0)
                {
                    // Cargamos relleno
                    IEnumerable<WpsCache> origen = SoldaduraBL.Instance.ObtenerWps(
                                                     ViewState["juntaSpoolID"].SafeIntParse(),
                                                     _proyectoID,
                                                     procesoRaizID,
                                                     procesoRellenoID,
                                                     material1,
                                                     material2,
                                                     espesorJunta,
                                                     wpsDiferentes,
                                                     false);

                    lstWpsRelleno.DataSource = origen;
                    lstWpsRelleno.DataTextField = "Nombre";
                    lstWpsRelleno.DataValueField = "ID";
                    lstWpsRelleno.DataBind();
                    lstWpsRelleno.Items.Insert(0, String.Empty);

                    // Cargamos raíz
                    origen = SoldaduraBL.Instance.ObtenerWps(
                            ViewState["juntaSpoolID"].SafeIntParse(),
                            _proyectoID,
                            procesoRaizID,
                            procesoRellenoID,
                            material1,
                            material2,
                            espesorJunta,
                            wpsDiferentes,
                            true);

                    lstWpsRaiz.DataSource = origen;
                    lstWpsRaiz.DataTextField = "Nombre";
                    lstWpsRaiz.DataValueField = "ID";
                    lstWpsRaiz.DataBind();
                    lstWpsRaiz.Items.Insert(0, String.Empty);
                }
            }
        }

        #endregion

        protected void slTermidadoConRaiz_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TerminadoConRaiz == false)
            {
                TerminadoConRaiz = true;
                lstWpsRaiz.Items.Clear();
                lstWpsRelleno.Items.Clear();
                lstWpsRaiz.Items.Insert(0, String.Empty);
                lstWpsRelleno.Items.Insert(0, String.Empty);
                lstProcRelleno.Visible = false;
                lstWpsRelleno.Visible = false;
                lblWpsRelleno.Visible = false;
                lblProcRelleno.Visible = false;
                slWpsDiferentes.Visible = false;
            }
            else
            {
                TerminadoConRaiz = false;
                lstWpsRaiz.Items.Clear();
                lstWpsRelleno.Items.Clear();
                lstWpsRaiz.Items.Insert(0, String.Empty);
                lstWpsRelleno.Items.Insert(0, String.Empty);
                lstProcRelleno.Visible = true;
                lstWpsRelleno.Visible = true;
                lblWpsRelleno.Visible = true;
                lblProcRelleno.Visible = true;
                slWpsDiferentes.Visible = true;
            }
        }

    }

}