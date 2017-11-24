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
using Mimo.Framework.Extensions;
using SAM.Entities.Grid;
using SAM.Entities;
using Mimo.Framework.Exceptions;

namespace SAM.Web.Administracion
{
    /// <summary>
    /// Muestra un listado en base a filtros de los periodos de destajo dentro del sistema.
    /// En la lista de periodos se muestran los datos básicos del periodo así como su estatus
    /// y monto total a pagar.
    /// </summary>
    public partial class LstPeriodosDestajo : SamPaginaPrincipal
    {
        #region Variables ViewState

        /// <summary>
        /// Valor del filtro fecha de inicio al momento de dar click en el botón mostrar
        /// </summary>
        private DateTime? FechaInicio
        {
            get
            {
                if (ViewState["FechaInicio"] != null)
                {
                    return (DateTime)ViewState["FechaInicio"];
                }
                return null;
            }
            set
            {
                ViewState["FechaInicio"] = value;
            }
        }

        /// <summary>
        /// Valor del filtro fecha final al momento de dar click en el botón mostrar
        /// </summary>
        private DateTime? FechaFin
        {
            get
            {
                if (ViewState["FechaFin"] != null)
                {
                    return (DateTime)ViewState["FechaFin"];
                }
                return null;
            }
            set
            {
                ViewState["FechaFin"] = value;
            }
        }

        #endregion

        /// <summary>
        /// Configurar menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.adm_Destajos);
            }
        }

        /// <summary>
        /// Establece la propiedad del datasource del grid
        /// </summary>
        private void establecerDataSourceGrid()
        {
            grdPeriodos.DataSource = DestajoBO.Instance
                                              .ObtenerListaPeriodosFiltrado(FechaInicio, FechaFin)
                                              .OrderBy(x => x.FechaInicio);
        }

        /// <summary>
        /// Limpia los filtros y sortings del grid y vuelve a hacer el binding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdPeriodos.ResetBind();
            grdPeriodos.Rebind();
        }

        /// <summary>
        /// Para cada periodo de destajo que se muestre en el listado mostrar los íconos en cada
        /// renglón dependiendo de las reglas de negocio.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPeriodos_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                GrdPeriodoDestajo periodo = (GrdPeriodoDestajo)item.DataItem;

                ImageButton imgBorrar = (ImageButton)item["eliminar_h"].FindControl("imgBorrar");
                ImageButton imgAprobar = (ImageButton)item["cerrar_h"].FindControl("imgCerrar");

                //Si el periodo ya está cerrado no permitimos borrar el periodo ni aprobarlo
                if (periodo.Cerrado)
                {
                    imgBorrar.Visible = false;
                    imgAprobar.Visible = false;
                }

                //Si aún hay destajos sin aprobar el periodo no se puede cerrar
                if (periodo.Estatus == EstatusPeriodoDestajo.Pendiente)
                {
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
        protected void grdPeriodos_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            establecerDataSourceGrid();
        }


        /// <summary>
        /// Se dispara cuando algún control interno del grid causa un postback, generalmente es una acción
        /// que tenemos que atender.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdPeriodos_OnItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            string command = e.CommandName.ToLower();

            //Par evitar que el sorting, paginación etc causen broncas
            if (command == "borrar" || command == "cerrar")
            {
                int periodoDestajoID = e.CommandArgument.SafeIntParse();

                try
                {
                    switch (command)
                    {
                        case "borrar":
                            DestajoBO.Instance.BorrarPeriodo(periodoDestajoID);
                            break;
                        case "cerrar":
                            DestajoBO.Instance.CerrarPeriodo(periodoDestajoID, null, SessionFacade.UserId);
                            break;
                    }
                    //Actualizar el grid
                    grdPeriodos.Rebind();
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }
        }

        /// <summary>
        /// Guarda en ViewState el valor de los filtros, muestra el panel con el grid y hace
        /// el binding correspondiente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            FechaInicio = dtpFechaInicio.SelectedDate;
            FechaFin = dtpFechaFin.SelectedDate;
            phListado.Visible = true;
            establecerDataSourceGrid();
            grdPeriodos.ResetBind();
            grdPeriodos.DataBind();
        }

    }
}