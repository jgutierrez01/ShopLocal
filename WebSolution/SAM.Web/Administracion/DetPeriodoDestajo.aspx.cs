using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessObjects.Administracion;
using SAM.Web.Common;
using Telerik.Web.UI;
using SAM.Entities.Grid;
using SAM.Entities;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Administracion;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Utilerias;

namespace SAM.Web.Administracion
{
    /// <summary>
    /// Página donde se despliega un resumen sobre un periodo de destajo, un resumen de un
    /// periodo de destajo consiste en lo siguiente:
    /// 
    /// + Datos del periodo (tabla periodo Destajo)
    /// + Listado con las personas dentro del periodo de destajo (DestajoTubero y DestajoSoldador)
    /// 
    /// El listado de personas incluye algunos totales.
    /// </summary>
    public partial class DetPeriodoDestajo : SamPaginaPrincipal
    {
        #region Variables ViewState

        /// <summary>
        /// Variable booleana que indica si el periodo está aprobado o no
        /// </summary>
        public bool PeriodoAprobado
        {
            get
            {
                return (bool)ViewState["PeriodoAprobado"];
            }
            set
            {
                ViewState["PeriodoAprobado"] = value;
            }
        }

        #endregion

        /// <summary>
        /// Carga los datos necesario sólo en la primera carga de la página (cuando no sea postback)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.adm_Destajos);
                cargaPagina();
            }
        }

        /// <summary>
        /// Carga los datos del periodo + listado de personas directo de BD
        /// en base al valor de EntityID = PeriodoDestajoID
        /// </summary>
        private void cargaPagina()
        {
            cargaDatosPanel();
            grdPersonasDestajo.ResetBind();
            establecerDataSourceGrid();
            grdPersonasDestajo.DataBind();
        }

        /// <summary>
        /// Carga los datos del periodo como tal
        /// </summary>
        private void cargaDatosPanel()
        {
            PeriodoDestajo periodo = DestajoBO.Instance.ObtenerPeriodo(EntityID.Value);
            VersionRegistro = periodo.VersionRegistro;
            PeriodoAprobado = periodo.Aprobado;
            lblAnio.Text = periodo.Anio.ToString();
            lblSemana.Text = periodo.Semana;
            lblFechaFin.Text = periodo.FechaFin.SafeDateAsStringParse();
            lblFechaIni.Text = periodo.FechaInicio.SafeDateAsStringParse();
            lblDiasFestivos.Text = periodo.CantidadDiasFestivos.ToString();
            lblEstatus.Text = TraductorEnumeraciones.AbiertoOCerrado(periodo.Aprobado);

            if (PeriodoAprobado)
            {
                btnCerrar.Enabled = false;
            }
        }

        /// <summary>
        /// Carga el listado de personas de BD ordenado por tuberos primero y luego
        /// por soldadores.
        /// </summary>
        private void establecerDataSourceGrid()
        {
            //Ordenar para que salgan primero los tuberos y luego los soldadores
            grdPersonasDestajo.DataSource = DestajoBO.Instance
                                                     .ObtenerPersonasPorPeriodo(EntityID.Value)
                                                     .OrderBy(x => x.EsTubero ? 0 : 1)
                                                     .ThenBy(x => x.Codigo);
        }

        /// <summary>
        /// Actualiza el grid con los datos de BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdPersonasDestajo.ResetBind();
            grdPersonasDestajo.Rebind();
        }

        /// <summary>
        /// Se dispara cada vez que se hace el binding para una persona en particular.
        /// En este método configuramos los íconos de cada renglón dependiendo de las reglas de negocio.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPersonasDestajo_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                GrdPersonaDestajo personaDestajo = (GrdPersonaDestajo)item.DataItem;

                HyperLink hlVer = (HyperLink)item["ver_h"].FindControl("hlVer");
                ImageButton imgBorrar = (ImageButton)item["eliminar_h"].FindControl("imgBorrar");
                ImageButton imgAprobar = (ImageButton)item["aprobar_h"].FindControl("imgAprobar");
                ImageButton imgRecalcular = (ImageButton)item["recalcular_h"].FindControl("imgRecalcular");

                //Liga para el detalle, se le manda el tipo y el ID
                hlVer.NavigateUrl = string.Format(WebConstants.AdminUrl.DetalleDestajo, personaDestajo.EsTubero ? "T": "S", personaDestajo.IdRegistroDetalle);

                //La T indica que es tubero la S soldador
                string commandArgument = (personaDestajo.EsTubero ? "T-" : "S-") + personaDestajo.IdRegistroDetalle;

                //Si el destajo ya está aprobado ya no se puede aprobar
                if (personaDestajo.Aprobado)
                {
                    imgAprobar.Visible = false;
                }
                else
                {
                    imgAprobar.CommandArgument =  commandArgument;
                }

                imgRecalcular.CommandArgument = commandArgument;
                imgBorrar.CommandArgument = commandArgument;

                //Si el periodo completo ya está aprobado entonces no se puede:
                //+ borrar
                //+ recalcular
                //+ aprobar
                if (PeriodoAprobado)
                {
                    imgBorrar.Visible = false;
                    imgRecalcular.Visible = false;
                    imgAprobar.Visible = false;
                }
            }
        }


        /// <summary>
        /// Lo dispara automáticamente Telerik cuando ocurre algún evento que requiera
        /// actualizar el datasource del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPersonasDestajo_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            establecerDataSourceGrid();
        }


        /// <summary>
        /// Se dispara cuando algún control interno del grid causa un postback, generalmente es una acción
        /// que tenemos que atender.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdPersonasDestajo_OnItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            string command = e.CommandName.ToLower();

            //Par evitar que el sorting, paginación etc causen broncas
            if (command == "borrar" || command == "aprobar" || command == "recalcular")
            {
                string[] args = e.CommandArgument.ToString().Split('-');
                bool esTubero = args[0] == "T";
                int idRegistro = args[1].SafeIntParse();

                try
                {
                    switch (command)
                    {
                        case "borrar":
                            DestajoBO.Instance.BorraPersona(esTubero, idRegistro);
                            break;
                        case "aprobar":
                            DestajoBO.Instance.ApruebaCalculoPersona(esTubero, idRegistro, SessionFacade.UserId);
                            break;
                        case "recalcular":
                            DestajoBL.Instance.RecalculaDestajoPersona(esTubero, idRegistro, SessionFacade.UserId);
                            break;
                    }
                    //Actualizar el grid
                    grdPersonasDestajo.Rebind();
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }
        }

        /// <summary>
        /// Marca un periodo como cerrado, la lógica de negocios se encarga de validar que sea posible.
        /// En caso que no se pueda manda una excepción que se atrapa aquí y desplegamos en error
        /// en el validation summary.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCerrar_OnClick(object sender, EventArgs e)
        {
            try
            {
                DestajoBO.Instance.CerrarPeriodo(EntityID.Value, VersionRegistro, SessionFacade.UserId);
                cargaPagina();
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }
    }
}