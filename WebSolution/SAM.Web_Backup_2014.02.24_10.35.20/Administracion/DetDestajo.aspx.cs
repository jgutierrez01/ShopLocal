using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessObjects.Administracion;
using Mimo.Framework.Extensions;
using SAM.Entities;
using SAM.Entities.Grid;
using Resources;
using Mimo.Framework.Exceptions;

namespace SAM.Web.Administracion
{
    /// <summary>
    /// Esta página muestra el detalle de juntas pagadas para una persona en particular dentro
    /// de un periodo de destajo en específico.  La página recibe por QS dos parámetros, uno con
    /// el tipo (soldador o tubero) y el otro con el ID (DestajoTuberoID o DestajoSoldadorID)
    /// dependiendo del caso.
    /// </summary>
    public partial class DetDestajo : SamPaginaPrincipal
    {
        #region Propiedades de ViewState

        /// <summary>
        /// String separado por comas con el total de personas involucradas en el
        /// periodo de destajo que nos interesa.  Los soldadores van con un prefijo de "S-"
        /// y los tuberos con "T-", el ID después del guión es el identificador de DestajoTuberoID
        /// o DestajoSoldadorID
        /// </summary>
        private string IdsPersonas
        {
            get
            {
                return (string)ViewState["IdsPersonas"];
            }
            set
            {
                ViewState["IdsPersonas"] = value;
            }
        }

        /// <summary>
        /// Número de personas del periodo de destajo en cuestión
        /// </summary>
        private int CantidadPersonas
        {
            get
            {
                return (int)ViewState["CantidadPersonas"];
            }
            set
            {
                ViewState["CantidadPersonas"] = value;
            }
        }

        /// <summary>
        /// Índice en base cero de la persona que se está visualizando, es el que
        /// corresponde al índice del arreglo IdsPersonas
        /// </summary>
        private int IndiceActual
        {
            get
            {
                return (int)ViewState["IndiceActual"];
            }
            set
            {
                ViewState["IndiceActual"] = value;
            }
        }

        /// <summary>
        /// Variable booleana que nos indica si el periodo de destajo (Registro padre)
        /// está cerrado o no.  Una vez que el periodo esté cerrado ya no se puede
        /// modificar el detalle
        /// </summary>
        private bool PeriodoCerrado
        {
            get
            {
                return (bool)ViewState["PeriodoCerrado"];
            }
            set
            {
                ViewState["PeriodoCerrado"] = value;
            }
        }

        /// <summary>
        /// Variable de instancia que nos indica si el destajo de la persona en cuestión está 
        /// aprobado o no.
        /// </summary>
        private bool DestajoAprobado
        {
            get
            {
                return (bool)ViewState["DestajoAprobado"];
            }
            set
            {
                ViewState["DestajoAprobado"] = value;
            }
        }

        #endregion

        private const string JS_CMTS_ARMADO = "javascript:Sam.Destajo.AbreComentariosArmado('{0}','{1}');";
        private const string JS_CMTS_SOLDADURA = "javascript:Sam.Destajo.AbreComentariosSoldadura('{0}','{1}');";
        private const string JS_CMTS_DESTAJO = "javascript:Sam.Destajo.AbreComentariosDestajo('{0}','{1}');";


        /// <summary>
        /// La primera vez que se carga la página se cargan valores en ViewState que se reutilizarán a lo
        /// largo de los postabacks (principalmente por el pager de personas).  En la carga incial se generan
        /// los valores en ViewState de lo siguiente:
        /// - IdsPersonas
        /// - CantidadPersonas
        /// - PeriodoCerrado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.adm_Destajos);
                
                //DestajoSoldadorID o DestajoTuberoID depende del caso
                int idRegistroDetalle = EntityID.Value;
                //T= tubero, S=soldador
                char tipo = Request.QueryString["Tipo"][0];

                //Traer los datos del periodo
                PeriodoDestajo periodo = DestajoBO.Instance.ObtenerPeriodoEnBaseADetalle(tipo == 'T', idRegistroDetalle);
                cargaDatosPeriodo(periodo);
                titulo.NavigateUrl = string.Format(WebConstants.AdminUrl.DetallePeriodoDestajo, periodo.PeriodoDestajoID);

                //Obtener los ids de todas las personas del periodo
                List<string> ids = DestajoBO.Instance.ObtenerResumenPersonas(tipo == 'T', idRegistroDetalle);
                CantidadPersonas = ids.Count;
                IdsPersonas = string.Join(",", ids.ToArray());
                IndiceActual = buscaPersona(tipo + "-" + idRegistroDetalle);
                cargaDatosDestajo(IndiceActual);

                //Configurar la ventana de Telerik para los comentarios
                rdwComentariosProceso.Behaviors = Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize;
            }
        }

        /// <summary>
        /// Carga en los controls de UI los datos correspondientes del periodo
        /// </summary>
        /// <param name="periodo">Entidad con los datos del periodo</param>
        private void cargaDatosPeriodo(PeriodoDestajo periodo)
        {
            lblEstatusPeriodo.Text = periodo.Aprobado ? MensajesAplicacion.Destajos_TextoEstatusPeriodoCerrado : MensajesAplicacion.Destajos_TextoEstatusPeriodoAbierto;
            lblSemana.Text = periodo.Semana + " - " + periodo.Anio;
            lblFechas.Text = string.Format("{0:d} - {1:d}", periodo.FechaInicio, periodo.FechaFin);
            lblDiasFest.Text = periodo.CantidadDiasFestivos.ToString();
            PeriodoCerrado = periodo.Aprobado;
        }

        /// <summary>
        /// En base a un código (T-56, S-25 etc...) regresa el índice
        /// dentro del cual se encuentra dicho código.
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns>Índice dentro del CSV string de IdsPersonas donde se encuentra la persona pasada</returns>
        private int buscaPersona(string codigo)
        {
            return IdsPersonas.Split(',')
                              .Select((v, i) => new { Codigo = v, Indice = i })
                              .Where(v => v.Codigo.EqualsIgnoreCase(codigo))
                              .Select(t => t.Indice)
                              .Single();
        }

        /// <summary>
        /// En base a un índice del arreglo de personas carga los datos del destajo para ese individuo
        /// en particular.
        /// </summary>
        /// <param name="indice">Índice del arreglo IdsPersonas del destajo a cargar</param>
        private void cargaDatosDestajo(int indice)
        {
            //obtener el código (T56, S23 etc...)
            string codigo = IdsPersonas.Split(',').Where((v,i) => i == indice).Select((v,i) => v).Single();
            string[] arr = codigo.Split('-');

            //Guardar en un hidden el "tipo" de persona, este hidden se usa desde JS así que es importante
            hdnTipoDestajo.Value = arr[0];

            if (arr[0] == "T")
            {
                cargaDestajoTubero(arr[1].SafeIntParse());
            }
            else
            {
                cargaDestajoSoldador(arr[1].SafeIntParse());
            }

            //Configurar el pager para que diga que número de registro de cuántos se está mostrando
            litTextoPager.Text = string.Format(MensajesAplicacion.Destajos_TextoPager, IndiceActual + 1, CantidadPersonas);
        }

        /// <summary>
        /// Carga los datos en el UI para el destajo de un soldador en particular.
        /// </summary>
        /// <param name="p">ID del destajo del soldador a cargar</param>
        private void cargaDestajoSoldador(int destajoSoldadorID)
        {
            //Obtiene la información del encabezado
            DestajoSoldador destajo = DestajoBO.Instance.ObtenerDestajoSoldadorConDatosSoldador(destajoSoldadorID);
            
            //Obtiene la información de las juntas a pagar
            List<GrdDetalleDestajoSoldador> detalle = DestajoBO.Instance.ObtenerDetalleDestajoSoldador(destajoSoldadorID);

            //Actualizar al valor de EntityID con el ID del registro en particular que se está viendo
            //también la versión registro para poder manejar concurrencia
            EntityID = destajo.DestajoSoldadorID;
            VersionRegistro = destajo.VersionRegistro;

            //Cargar todos los valores de los textboxes de totales, la mayoría en
            //formato de moneda
            txtDestajoRaiz.Text = destajo.TotalDestajoRaiz.ToString("C");
            txtDestajoRelleno.Text = destajo.TotalDestajoRelleno.ToString("C");
            txtCostoDiaFS.Text = destajo.CostoDiaFestivo.ToString("C");
            txtCantidadDiasFS.Text = destajo.CantidadDiasFestivos.ToString();
            txtCuadroS.Text = destajo.TotalCuadro.ToString("C");
            txtOtrosS.Text = destajo.TotalOtros.ToString("C");
            txtAjusteS.Text = destajo.TotalAjuste.ToString("C");
            txtDiasFestivosS.Text = destajo.TotalDiasFestivos.ToString("C");
            txtTotalS.Text = destajo.GranTotal.ToString("C");
            txtComentariosSoldador.Text = destajo.Comentarios;

            //Cargar la información que va en el encabezado
            lblCategoria.Text = MensajesAplicacion.Destajos_TextoSoldador;
            lblCodigo.Text = destajo.Soldador.Codigo;
            lblNumEmpleado.Text = destajo.Soldador.NumeroEmpleado;
            lblNombre.Text = formatoNombre(destajo.Soldador.Nombre, destajo.Soldador.ApPaterno, destajo.Soldador.ApMaterno);
            lblCostoCuadro.Text = destajo.ReferenciaCuadro.ToString("C");
            lblAprobado.Text = destajo.Aprobado ? MensajesAplicacion.Destajos_TextoAprobado : MensajesAplicacion.Destajos_TextoNoAprobado;

            //Guardar en ViewState si el destajo está aprobado o no
            DestajoAprobado = destajo.Aprobado;

            //Cargar el repeater con las juntas del soldador
            rpSoldadores.DataSource = detalle.OrderBy(x => x.EtiquetaJunta);
            rpSoldadores.DataBind();

            phSoldador.Visible = true;
            phTubero.Visible = false;

            //Permisos de solo lectura dependiendo de estatus para mostrar ocultar botones y
            //permitir o no captura en los textboxes
            bool soloLectura = PeriodoCerrado || DestajoAprobado;

            btnCmts.Visible = !soloLectura;
            txtOtrosS.ReadOnly = soloLectura;
            txtCantidadDiasFS.ReadOnly = soloLectura;
            txtCostoDiaFS.ReadOnly = soloLectura;
            txtCuadroS.ReadOnly = soloLectura;
            txtComentariosSoldador.ReadOnly = soloLectura;
            txtCmtsDestajo.ReadOnly = soloLectura;

            btnAprobar.Visible = !PeriodoCerrado && !DestajoAprobado;
            btnCancelarAprobacion.Visible = !PeriodoCerrado && DestajoAprobado;
        }

        /// <summary>
        /// Regresa el nombre completo de una persona:
        /// Nombre + ApPaterno + ApMaterno
        /// Hace el chequeo para el apellido paterno en caso que sea nulo o vacío
        /// </summary>
        /// <param name="nombre">Nombre de la persona</param>
        /// <param name="apPaterno">Apellido paterno de la persona</param>
        /// <param name="apMaterno">Apellido materno de la persona</param>
        /// <returns>Nombre completo de la persona</returns>
        private string formatoNombre(string nombre, string apPaterno, string apMaterno)
        {
            if (!string.IsNullOrWhiteSpace(apMaterno))
            {
                return string.Concat(nombre, ' ', apPaterno, ' ', apMaterno);
            }
            return string.Concat(nombre, ' ', apPaterno);
        }

        /// <summary>
        /// Carga los datos en el UI para el destajo de un tubero en particular.
        /// </summary>
        /// <param name="p">ID del destajo del tubero a cargar</param>
        private void cargaDestajoTubero(int destajoTuberoID)
        {
            //Obtiene la información del encabezado
            DestajoTubero destajo = DestajoBO.Instance.ObtenerDestajoTuberoConDatosTubero(destajoTuberoID);

            //Obtiene la información de las juntas a pagar
            List<GrdDetalleDestajoTubero> detalle = DestajoBO.Instance.ObtenerDetalleDestajoTubero(destajoTuberoID);

            //Actualizar al valor de EntityID con el ID del registro en particular que se está viendo
            //también la versión registro para poder manejar concurrencia
            EntityID = destajo.DestajoTuberoID;
            VersionRegistro = destajo.VersionRegistro;

            //Cargar todos los valores de los textboxes de totales, la mayoría en
            //formato de moneda
            txtDestajo.Text = destajo.TotalDestajo.ToString("C");
            txtCostoDiaF.Text = destajo.CostoDiaFestivo.ToString("C");
            txtCantidadDiasF.Text = destajo.CantidadDiasFestivos.ToString();
            txtCuadro.Text = destajo.TotalCuadro.ToString("C"); ;
            txtOtros.Text = destajo.TotalOtros.ToString("C");
            txtAjuste.Text = destajo.TotalAjuste.ToString("C");
            txtTotal.Text = destajo.GranTotal.ToString("C");
            txtDiasFestivos.Text = destajo.TotalDiasFestivos.ToString("C");
            txtComentariosTubero.Text = destajo.Comentarios;

            //Cargar la información que va en el encabezado
            lblCategoria.Text = MensajesAplicacion.Destajos_TextoTubero;
            lblCodigo.Text = destajo.Tubero.Codigo;
            lblNumEmpleado.Text = destajo.Tubero.NumeroEmpleado;
            lblNombre.Text = formatoNombre(destajo.Tubero.Nombre, destajo.Tubero.ApPaterno, destajo.Tubero.ApMaterno);
            lblCostoCuadro.Text = destajo.ReferenciaCuadro.ToString("C");
            lblAprobado.Text = destajo.Aprobado ? MensajesAplicacion.Destajos_TextoAprobado : MensajesAplicacion.Destajos_TextoNoAprobado;

            //Guardar en ViewState si el destajo está aprobado o no
            DestajoAprobado = destajo.Aprobado;

            //Cargar el repeater con las juntas del tubero
            rpTubero.DataSource = detalle.OrderBy(x => x.EtiquetaJunta);
            rpTubero.DataBind();

            phSoldador.Visible = false;
            phTubero.Visible = true;

            //Permisos de solo lectura dependiendo de estatus para mostrar ocultar botones y
            //permitir o no captura en los textboxes
            bool soloLectura = PeriodoCerrado || DestajoAprobado;

            btnCmts.Visible = !soloLectura;
            txtOtros.ReadOnly = soloLectura;
            txtCantidadDiasF.ReadOnly = soloLectura;
            txtCostoDiaF.ReadOnly = soloLectura;
            txtCuadro.ReadOnly = soloLectura;
            txtComentariosTubero.ReadOnly = soloLectura;
            txtCmtsDestajo.ReadOnly = soloLectura;

            btnAprobar.Visible = !PeriodoCerrado && !DestajoAprobado;
            btnCancelarAprobacion.Visible = !PeriodoCerrado && DestajoAprobado;
        }
        
        /// <summary>
        /// Se dispara al momento del binding de cada junta armada que se le va a pagar al tubero.
        /// En este método configuramos los íconos que se muestran así como tooltips y otros menesteres
        /// de UI principalmente.  Ver comentarios inline para más detalle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rpTubero_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            bool soloLectura = PeriodoCerrado || DestajoAprobado;

            if (e.Item.IsItem())
            {
                RepeaterItem item = (RepeaterItem)e.Item;
                GrdDetalleDestajoTubero data = (GrdDetalleDestajoTubero)item.DataItem;

                //Si tiene comentarios de armado se muestra el ícono que permite
                //abrir un popup con la info de armado
                if (!string.IsNullOrWhiteSpace(data.ComentariosArmado))
                {
                    HyperLink hlCmtProceso = (HyperLink)item.FindControl("hlCmtProceso");
                    HiddenField hdnCmtsArmado = (HiddenField)item.FindControl("hdnCmtsArmado");

                    hlCmtProceso.Visible = true;
                    hlCmtProceso.NavigateUrl = string.Format(JS_CMTS_ARMADO, rdwComentariosProceso.ClientID, hdnCmtsArmado.ClientID);
                }

                HyperLink hlCmtsDestajo = (HyperLink)item.FindControl("hlCmtsDestajo");
                HiddenField hdnCmtsDestajo = (HiddenField)item.FindControl("hdnCmtsDestajo");
                hlCmtsDestajo.NavigateUrl = string.Format(JS_CMTS_DESTAJO, rdwComentariosProceso.ClientID, hdnCmtsDestajo.ClientID);

                //Si el destajo ya está aprobado, o el periodo cerrado ya no permitimos eliminar
                //juntas y marcamos el textbox de ajuste como solo lectura.
                if (soloLectura)
                {
                    ImageButton lnkEliminarArmado = (ImageButton)item.FindControl("lnkEliminarArmado");
                    lnkEliminarArmado.Visible = false;
                    
                    TextBox txtAjuste = (TextBox)item.FindControl("txtAjuste");
                    txtAjuste.ReadOnly = true;
                }

                if (data.CostoDestajoVacio)
                {
                    Image imgExclamacionArmado = (Image)item.FindControl("imgExclamacionArmado");
                    imgExclamacionArmado.Visible = true;
                }
            }
        }

        /// <summary>
        /// Se dispara al momento del binding de cada junta soldada que se le va a pagar al soldador.
        /// En este método configuramos los íconos que se muestran así como tooltips y otros menesteres
        /// de UI principalmente.  Ver comentarios inline para más detalle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rpSoldadores_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            bool soloLectura = PeriodoCerrado || DestajoAprobado;

            if (e.Item.IsItem())
            {
                RepeaterItem item = (RepeaterItem)e.Item;
                GrdDetalleDestajoSoldador data = (GrdDetalleDestajoSoldador)item.DataItem;

                //Si tiene comentarios de soldadura se muestra el ícono que permite
                //abrir un popup con la info de soldadura
                if (!string.IsNullOrWhiteSpace(data.ComentariosSoldadura))
                {
                    HyperLink hlCmtProceso = (HyperLink)item.FindControl("hlCmtProceso");
                    HiddenField hdnCmtsSoldadura = (HiddenField)item.FindControl("hdnCmtsSoldadura");

                    hlCmtProceso.Visible = true;
                    hlCmtProceso.NavigateUrl = string.Format(JS_CMTS_SOLDADURA, rdwComentariosProceso.ClientID, hdnCmtsSoldadura.ClientID);
                }

                Image imgRaizEquipo = (Image)item.FindControl("imgRaizEquipo");
                Image imgRellenoEquipo = (Image)item.FindControl("imgRellenoEquipo");
                HyperLink hlCmtsDestajo = (HyperLink)item.FindControl("hlCmtsDestajo");
                HiddenField hdnCmtsDestajo = (HiddenField)item.FindControl("hdnCmtsDestajo");
                hlCmtsDestajo.NavigateUrl = string.Format(JS_CMTS_DESTAJO, rdwComentariosProceso.ClientID, hdnCmtsDestajo.ClientID);

                //Mostrar el ícono de trabajo en equipo para raíz si el proceso se hizo entre 2 o más personas
                if (data.NumeroFondeadores > 1)
                {
                    imgRaizEquipo.Visible = true;
                    imgRaizEquipo.ToolTip = string.Format(MensajesAplicacion.Destajos_ProcesoRaizEquipo, data.NumeroFondeadores);
                }

                //Mostrar el ícono de trabajo en equipo para relleno si el proceso se hizo entre 2 o más personas
                if (data.NumeroRellenadores > 1)
                {
                    imgRellenoEquipo.Visible = true;
                    imgRellenoEquipo.ToolTip = string.Format(MensajesAplicacion.Destajos_ProcesoRellenoEquipo, data.NumeroRellenadores);
                }

                //Si el destajo ya está aprobado, o el periodo cerrado ya no permitimos eliminar
                //juntas y marcamos el textbox de ajuste como solo lectura.
                if (soloLectura)
                {
                    ImageButton lnkElminarSoldadura = (ImageButton)item.FindControl("lnkElminarSoldadura");
                    lnkElminarSoldadura.Visible = false;

                    TextBox txtAjuste = (TextBox)item.FindControl("txtAjuste");
                    txtAjuste.ReadOnly = true;
                }

                if (data.CostoRellenoVacio)
                {
                    Image imgExclamacionRelleno = (Image)item.FindControl("imgExclamacionRelleno");
                    imgExclamacionRelleno.Visible = true;
                }

                if (data.CostoRaizVacio)
                {
                    Image imgExclamacionRaiz = (Image)item.FindControl("imgExclamacionRaiz");
                    imgExclamacionRaiz.Visible = true;
                }

            }
        }

        /// <summary>
        /// Se dispara el momento de eliminar una junta de soldadura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void rpSoldadores_OnItemCommand(object sender, CommandEventArgs args)
        {
            try
            {
                if (args.CommandName == "eliminar")
                {
                    int destajoSoldadorDetalleID = args.CommandArgument.SafeIntParse();
                    
                    //Mandar eliminar el detalle en particular, pero por cuestiones de concurrencia mandamos también el ID del
                    //registro padre, así como su versión
                    DestajoBO.Instance.EliminaDetalleDeSoldadura(destajoSoldadorDetalleID, EntityID.Value, VersionRegistro, SessionFacade.UserId);

                    //Volver a ir a la BD por la información del destajo
                    cargaDatosDestajo(IndiceActual);
                }
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }

        /// <summary>
        /// Se dispara el momento de eliminar una junta de armado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void rpTubero_OnItemCommand(object sender, CommandEventArgs args)
        {
            try
            {
                if (args.CommandName == "eliminar")
                {
                    int destajoTuberoDetalleID = args.CommandArgument.SafeIntParse();
                    
                    //Mandar eliminar el detalle en particular, pero por cuestiones de concurrencia mandamos también el ID del
                    //registro padre, así como su versión
                    DestajoBO.Instance.EliminaDetalleDeArmado(destajoTuberoDetalleID, EntityID.Value, VersionRegistro, SessionFacade.UserId);
                    
                    //Volver a ir a la BD por la información del destajo
                    cargaDatosDestajo(IndiceActual);
                }
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }

        /// <summary>
        /// Se dispara al hacer click en el ícono de "Anterior" para ir a ver a la persona anterior
        /// del mismo periodo de destajo.  El pager tiene funcionalidad de lista circular
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgAnterior_Click(object sender, EventArgs e)
        {
            if (IndiceActual == 0)
            {
                IndiceActual = CantidadPersonas - 1;
            }
            else
            {
                IndiceActual--;
            }
            
            //Carga el nuevo destajo en particular
            cargaDatosDestajo(IndiceActual);
        }

        /// <summary>
        /// Se dispara al hacer click en el ícono de "Siguiente" para ir a ver a la persona siguiente
        /// del mismo periodo de destajo.  El pager tiene funcionalidad de lista circular
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgSiguiente_Click(object sender, EventArgs e)
        {
            AvanzaUno();
            //Carga el nuevo destajo en particular
            cargaDatosDestajo(IndiceActual);
        }

        /// <summary>
        /// Actualiza la variable de IndicaActual para ir al siguiente indice, tomando en cuenta un comportamiento
        /// de lista circular
        /// </summary>
        private void AvanzaUno()
        {
            IndiceActual = (IndiceActual + 1) % CantidadPersonas;
        }

        /// <summary>
        /// Se dispara en el momento que el usuario aprueba el destajo de una persona.
        /// Al momento de aprobar se guardan todos los datos capturados en UI en la base
        /// de datos y automáticamente se "avanza" al registro de destajo de la siguiente persona
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAprobar_Click(object sender, EventArgs e)
        {
            try
            {
                //Usar el valor del hidden para ver si se trata de un soldador o de un tubero
                bool esTubero = hdnTipoDestajo.Value == "T";

                if (esTubero)
                {
                    //Hacer el unbind
                    DestajoTubero destajo = unbindDetalleTubero();
                    //Marcarlo como aprobado
                    destajo.Aprobado = true;
                    //Guardar en BD
                    DestajoBO.Instance.GuardaDestajoTubero(destajo);
                }
                else
                {
                    //Hacer el unbind
                    DestajoSoldador destajo = unbindDetalleSoldador();
                    //Marcarlo como aprobado
                    destajo.Aprobado = true;
                    //Guardar en BS
                    DestajoBO.Instance.GuardaDestajoSoldador(destajo);
                }

                //Avanzar al registro de la siguiente persona
                AvanzaUno();
                
                //Cargar el siguiente destajo
                cargaDatosDestajo(IndiceActual);
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }

        /// <summary>
        /// Se encarga de tomar todos los datos del UI y cargarlos sobre la entidad tipo DestajoTubero
        /// correspondiente.  Esta entidad se puede mandar guardar después a la BD ya que traera la información
        /// del destajo y juntas actualizadas.
        /// 
        /// El método se encarga de hacer las cuestiones de start tracking para cada entidad
        /// </summary>
        /// <returns>Entidad de tipo DetajoTubero lista para mandarse guardar en BD con los datos del UI</returns>
        private DestajoTubero unbindDetalleTubero()
        {
            DestajoTubero destajo = DestajoBO.Instance.ObtenerDestajoTuberoConDetalle(EntityID.Value);
            DestajoTuberoDetalle detalle;
            int detalleID;
            
            destajo.VersionRegistro = VersionRegistro;
            destajo.StartTracking();
            destajo.UsuarioModifica = SessionFacade.UserId;
            destajo.FechaModificacion = DateTime.Now;

            //Variables para calcular totales
            decimal totalAjuste = 0;
            decimal total = 0;
            decimal totalRenglon = 0;
            decimal ajusteRenglon = 0;

            //Se hace el unbind de cada fila del repeater pues a través de JS se va actualizando
            //en hiddens los valors de prorrateos por junta así como los comentarios.  De igual manera el valor del ajuste
            //por renglón se tiene que tomar del repeater
            foreach (RepeaterItem item in rpTubero.Items)
            {
                if (item.IsItem())
                {
                    HiddenField hdnCuadro = (HiddenField)item.FindControl("hdnCuadro");
                    HiddenField hdnDiasF = (HiddenField)item.FindControl("hdnDiasF");
                    HiddenField hdnOtros = (HiddenField)item.FindControl("hdnOtros");
                    HiddenField hdnTotal = (HiddenField)item.FindControl("hdnTotal");
                    HiddenField hdnDetalleID = (HiddenField)item.FindControl("hdnDetalleID");
                    HiddenField hdnCmtsDestajo = (HiddenField)item.FindControl("hdnCmtsDestajo");

                    TextBox txtAjusteFila = (TextBox)item.FindControl("txtAjuste");

                    detalleID = hdnDetalleID.Value.SafeIntParse();

                    detalle = destajo.DestajoTuberoDetalle.Where(x => x.DestajoTuberoDetalleID == detalleID).Single();

                    ajusteRenglon = txtAjusteFila.Text.SafeMoneyParse();
                    totalRenglon = hdnTotal.Value.SafeDecimalParse();
                    total += totalRenglon;
                    totalAjuste += ajusteRenglon;
                    
                    detalle.StartTracking();
                    detalle.ProrrateoCuadro = hdnCuadro.Value.SafeDecimalParse();
                    detalle.ProrrateoDiasFestivos = hdnDiasF.Value.SafeDecimalParse();
                    detalle.ProrrateoOtros = hdnOtros.Value.SafeDecimalParse();
                    detalle.Total = totalRenglon;
                    detalle.Ajuste = ajusteRenglon;
                    detalle.FechaModificacion = DateTime.Now;
                    detalle.Comentarios = hdnCmtsDestajo.Value;
                    detalle.UsuarioModifica = SessionFacade.UserId;
                }
            }

            //No lo podemos tomar de txtAjuste por ser Read-Only y no traer el valor en el post
            destajo.TotalAjuste = totalAjuste;
            destajo.TotalCuadro = txtCuadro.Text.SafeMoneyParse();
            destajo.TotalDestajo = txtDestajo.Text.SafeMoneyParse();
            destajo.CantidadDiasFestivos = txtCantidadDiasF.Text.SafeIntParse();
            destajo.CostoDiaFestivo = txtCostoDiaF.Text.SafeMoneyParse();
            destajo.TotalDiasFestivos =  destajo.CostoDiaFestivo * destajo.CantidadDiasFestivos;
            destajo.TotalOtros = txtOtros.Text.SafeMoneyParse();
            
            //Como el repeater puede estar vacío hay que calcularlo así
            destajo.GranTotal = destajo.TotalAjuste + destajo.TotalCuadro + destajo.TotalDestajo + destajo.TotalDiasFestivos + destajo.TotalOtros;
            destajo.Comentarios = txtComentariosTubero.Text;

            return destajo;
        }

        /// <summary>
        /// Se encarga de tomar todos los datos del UI y cargarlos sobre la entidad tipo DestajoSoldador
        /// correspondiente.  Esta entidad se puede mandar guardar después a la BD ya que traera la información
        /// del destajo y juntas actualizadas.
        /// 
        /// El método se encarga de hacer las cuestiones de start tracking para cada entidad
        /// </summary>
        /// <returns>Entidad de tipo DestajoSoldador lista para mandarse guardar en BD con los datos del UI</returns>
        private DestajoSoldador unbindDetalleSoldador()
        {
            DestajoSoldador destajo = DestajoBO.Instance.ObtenerDestajoSoldadorConDetalle(EntityID.Value);
            DestajoSoldadorDetalle detalle;
            int detalleID;

            destajo.VersionRegistro = VersionRegistro;
            destajo.StartTracking();
            destajo.UsuarioModifica = SessionFacade.UserId;
            destajo.FechaModificacion = DateTime.Now;

            //Variables para calcular totales
            decimal totalAjuste = 0;
            decimal total = 0;
            decimal totalRenglon = 0;
            decimal ajusteRenglon = 0;

            //Se hace el unbind de cada fila del repeater pues a través de JS se va actualizando
            //en hiddens los valors de prorrateos por junta así como los comentarios.  De igual manera el valor del ajuste
            //por renglón se tiene que tomar del repeater
            foreach (RepeaterItem item in rpSoldadores.Items)
            {
                if (item.IsItem())
                {
                    HiddenField hdnCuadro = (HiddenField)item.FindControl("hdnCuadro");
                    HiddenField hdnDiasF = (HiddenField)item.FindControl("hdnDiasF");
                    HiddenField hdnOtros = (HiddenField)item.FindControl("hdnOtros");
                    HiddenField hdnTotal = (HiddenField)item.FindControl("hdnTotal");
                    HiddenField hdnDetalleID = (HiddenField)item.FindControl("hdnDetalleID");
                    HiddenField hdnCmtsDestajo = (HiddenField)item.FindControl("hdnCmtsDestajo");

                    TextBox txtAjusteFila = (TextBox)item.FindControl("txtAjuste");

                    detalleID = hdnDetalleID.Value.SafeIntParse();

                    detalle = destajo.DestajoSoldadorDetalle.Where(x => x.DestajoSoldadorDetalleID == detalleID).Single();

                    ajusteRenglon = txtAjusteFila.Text.SafeMoneyParse();
                    totalRenglon = hdnTotal.Value.SafeDecimalParse();
                    total += totalRenglon;
                    totalAjuste += ajusteRenglon;

                    detalle.StartTracking();
                    detalle.ProrrateoCuadro = hdnCuadro.Value.SafeDecimalParse();
                    detalle.ProrrateoDiasFestivos = hdnDiasF.Value.SafeDecimalParse();
                    detalle.ProrrateoOtros = hdnOtros.Value.SafeDecimalParse();
                    detalle.Total = totalRenglon;
                    detalle.Ajuste = ajusteRenglon;
                    detalle.Comentarios = hdnCmtsDestajo.Value;
                    detalle.FechaModificacion = DateTime.Now;
                    detalle.UsuarioModifica = SessionFacade.UserId;
                }
            }

            //No lo podemos tomar de txtAjusteS por ser Read-Only y no traer el valor en el post
            destajo.TotalAjuste = totalAjuste;
            destajo.TotalCuadro = txtCuadroS.Text.SafeMoneyParse();
            destajo.TotalDestajoRaiz = txtDestajoRaiz.Text.SafeMoneyParse();
            destajo.TotalDestajoRelleno = txtDestajoRelleno.Text.SafeMoneyParse();
            destajo.CantidadDiasFestivos = txtCantidadDiasFS.Text.SafeIntParse();
            destajo.CostoDiaFestivo = txtCostoDiaFS.Text.SafeMoneyParse();
            destajo.TotalDiasFestivos = destajo.CostoDiaFestivo * destajo.CantidadDiasFestivos;
            destajo.TotalOtros = txtOtrosS.Text.SafeMoneyParse();

            //Como el repeater puede estar vacío hay que calcularlo así
            destajo.GranTotal = destajo.TotalAjuste + destajo.TotalCuadro + destajo.TotalDestajoRaiz + destajo.TotalDestajoRelleno + destajo.TotalDiasFestivos + destajo.TotalOtros;
            destajo.Comentarios = txtComentariosSoldador.Text;

            return destajo;
        }

        /// <summary>
        /// Marca un destajo "aprobado" como "no aprobado" y vuelve a cargar los datos del destajo
        /// en el UI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarAprobacion_Click(object sender, EventArgs e)
        {
            try
            {
                DestajoBO.Instance.CancelarAprobacionDestajoPersona(hdnTipoDestajo.Value == "T", EntityID.Value, VersionRegistro, SessionFacade.UserId);
                cargaDatosDestajo(IndiceActual);
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }
        

        /// <summary>
        /// Esta página tiene una peculiaridad importante que es el hecho de que hay varios cálculos que se hacen en el
        /// UI del cliente a través de jQuery.  Estos cálculos afectan hiddens y spans que después perderían sus valores
        /// en el postback (los hiddens no, pero los spans si).
        /// 
        /// De igual manera estos cálculos afectan algunos textboxes que están en modo sólo lectura y por cuestiones de seguridad
        /// .NET bloquea del post los valores de los textboxes que se modifican a través de JS que eran de solo lectura.
        /// Por todo lo anterior el Pre_Render es necesario en esta página para asegurarnos de recalcular en el servidor
        /// en base a los valores de los hiddens (y de aquellos textboxes en los que se puede confiar) cada span, textbox
        /// y demás que se tiene que renderear al principio.
        /// 
        /// Este método es el que permite que el usuario haga posts sobre el mismo destajo sin "perder" sus valores entre
        /// uno y otro.  Hoy en día (24/Nov/2010) el único escenario para el cual esto es requerido es por si ocurre un error
        /// de concurrencia (o de validación de regla de negocio) a la hora de guardar.  De ahí en fuera los eventos normales
        /// de la página siempre ocasiona que se vuelva a cargar desde BD por lo cual este código no es necesario el 99.99% de
        /// las veces.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Variables para calcular totales
            decimal totalAjuste = 0;
            decimal total = 0;
            decimal totalRenglon = 0;

            //Traer los items del repeater que corresponde
            RepeaterItemCollection items = hdnTipoDestajo.Value == "T" ? rpTubero.Items : rpSoldadores.Items;

            //Iterar sobre los elementos del repeater, se trata igual para soldador que para tubero
            //Recuperar a través de los hiddens los valores para las columas del repeater y los textbox readonly
            foreach (RepeaterItem item in items)
            {
                if (item.IsItem())
                {
                    HiddenField hdnCuadro = (HiddenField)item.FindControl("hdnCuadro");
                    HiddenField hdnDiasF = (HiddenField)item.FindControl("hdnDiasF");
                    HiddenField hdnOtros = (HiddenField)item.FindControl("hdnOtros");
                    HiddenField hdnTotal = (HiddenField)item.FindControl("hdnTotal");

                    Label colCuadro = (Label)item.FindControl("colCuadro");
                    Label colDiasF = (Label)item.FindControl("colDiasF");
                    Label colOtros = (Label)item.FindControl("colOtros");
                    Label colTotal = (Label)item.FindControl("colTotal");

                    TextBox txtAjusteFila = (TextBox)item.FindControl("txtAjuste");

                    totalRenglon = hdnTotal.Value.SafeDecimalParse();

                    colCuadro.Text = hdnCuadro.Value.SafeDecimalParse().ToString("C");
                    colDiasF.Text = hdnDiasF.Value.SafeDecimalParse().ToString("C");
                    colOtros.Text = hdnOtros.Value.SafeDecimalParse().ToString("C");
                    colTotal.Text = totalRenglon.ToString("C");

                    totalAjuste += txtAjusteFila.Text.SafeMoneyParse();
                    total += totalRenglon;
                }
            }

            if (hdnTipoDestajo.Value == "T")
            {
                txtAjuste.Text = totalAjuste.ToString("C");
                txtDiasFestivos.Text = (txtCostoDiaF.Text.SafeMoneyParse() * txtCantidadDiasF.Text.SafeIntParse()).ToString("C");
                
                //Es importante que sea en base al valor de los textboxes
                txtTotal.Text = (txtAjuste.Text.SafeMoneyParse() +
                                txtDestajo.Text.SafeMoneyParse() +
                                txtOtros.Text.SafeMoneyParse() +
                                txtDiasFestivos.Text.SafeMoneyParse() +
                                txtCuadro.Text.SafeMoneyParse()).ToString("C");

                //footer del repeater
                lblTotalAjusteT.Text = txtAjuste.Text;
                lblTotalCuadroT.Text = txtCuadro.Text;
                lblTotalDiasFT.Text = txtDiasFestivos.Text;
                lblTotalDestajoT.Text = txtDestajo.Text;
                lblGranTotalT.Text = txtTotal.Text;
                lblTotalOtrosT.Text = txtOtros.Text;
            }
            else
            {
                txtAjusteS.Text = totalAjuste.ToString("C");
                txtDiasFestivosS.Text = (txtCostoDiaFS.Text.SafeMoneyParse() * txtCantidadDiasFS.Text.SafeIntParse()).ToString("C");

                //Es importante que sea en base al valor de los textboxes
                txtTotalS.Text = (txtAjusteS.Text.SafeMoneyParse() +
                                 txtDestajoRaiz.Text.SafeMoneyParse() +
                                 txtDestajoRelleno.Text.SafeMoneyParse() +
                                 txtOtrosS.Text.SafeMoneyParse() +
                                 txtDiasFestivosS.Text.SafeMoneyParse() +
                                 txtCuadroS.Text.SafeMoneyParse()).ToString("C");

                //footer del repeater
                lblTotalAjusteS.Text = txtAjusteS.Text;
                lblTotalCuadroS.Text = txtCuadroS.Text;
                lblTotalDiasFS.Text = txtDiasFestivosS.Text;
                lblTotalDestajoRaizS.Text = txtDestajoRaiz.Text;
                lblTotalDestajoRellenoS.Text = txtDestajoRelleno.Text;
                lblGranTotalS.Text = txtTotalS.Text;
                lblTotalOtrosS.Text = txtOtrosS.Text;
            }
        }
    }
}