using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.Web.Common;
using Telerik.Web.UI;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Produccion;
using SAM.Entities;
using SAM.Entities.Grid;
using Mimo.Framework.Exceptions;
using SAM.BusinessLogic.Workstatus;

namespace SAM.Web.WorkStatus
{

    /// <summary>
    /// Esta página se encarga de permitir el despacho y cancelación de despacho
    /// de cada uno de los materiales de un número de control en particular.
    /// 
    /// La orden de trabajo y el número de control se seleccionan con los filtros de arriba
    /// y posteriormente el grid despliega los materiales para poder hacer los despachos o cancelarlos.
    /// </summary>
    public partial class Despacho : SamPaginaPrincipal
    {
        /// <summary>
        /// JS que se encarga de abrir el popup de despaho en base al ID de OrdenTrabajoMaterial
        /// </summary>
        private const string JS_POPUP_DESPACHO = "javascript:Sam.Workstatus.AbrePopupDespacho('{0}');";

        /// <summary>
        /// Mensaje de confirmación client-side para cancelar un despacho
        /// </summary>
        private const string JS_CONFIRMA_CANCELA_DESPACHO = "return Sam.Confirma(6,'{0}');";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_Despachos);
            }
        }


        /// <summary>
        /// Muestra el grid con el detalle de los materiales del número de control seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            //Guardar en ViewState el ID del número de control seleccionado
            EntityID = filtroGenerico.NumeroControlSelectedValue.SafeIntParse();
            phListado.Visible = true;
            establecerDataSourceGrid();
            grdMateriales.DataBind();

        }

        /// <summary>
        /// Actualizar el grid quitando filtros y ordenamiendo primero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkActualizar_Click(object sender, EventArgs e)
        {
            grdMateriales.ResetBind();
            grdMateriales.Rebind();
        }

        /// <summary>
        /// Se dispara cada que se hace el binding de un renglón del grid.
        /// Se configura que botón de acción se debe desplegar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdMateriales_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                GrdMaterialesDespacho despacho = (GrdMaterialesDespacho)item.DataItem;

                HyperLink hlDespacho = (HyperLink)item["accion_h"].FindControl("hlDespacho");
                ImageButton imgCancelar = (ImageButton)item["accion_h"].FindControl("imgCancelar");

                //Si ya tiene despacho se habilita la opción de cancelar el despacho.
                if (despacho.TieneDespacho)
                {
                    imgCancelar.Visible = true;
                    imgCancelar.CommandArgument = despacho.OrdenTrabajoMaterialID.ToString();
                    imgCancelar.OnClientClick = string.Format(JS_CONFIRMA_CANCELA_DESPACHO, despacho.EtiquetaMaterial);
                }
                else
                {
                    //Si no tiene despacho hay que revisar en base al estatus si está listo para despacharse o no.
                    //en caso de estar listo se habilita la liga para despachar
                    bool despachar = despacho.PerteneceAOdt && ((despacho.EsTubo && despacho.TieneCorte) || !despacho.EsTubo);

                    if (despachar)
                    {
                        hlDespacho.Visible = true;
                        hlDespacho.NavigateUrl = string.Format(JS_POPUP_DESPACHO, despacho.OrdenTrabajoMaterialID);
                    }
                }

                if (despacho.TieneHold)
                {
                    imgCancelar.Visible = false;
                    hlDespacho.Visible = false;
                }
            }
        }


        /// <summary>
        /// Lo dispara automáticamente Telerik cuando ocurre algún evento que requiera
        /// actualizar el datasource del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdMateriales_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            establecerDataSourceGrid();
        }


        /// <summary>
        /// Se dispara cuando algún control interno del grid causa un postback, generalmente es una acción
        /// que tenemos que atender.
        /// 
        /// En este caso sólo la acción de cancelar despacho se dispara.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdMateriales_OnItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            string command = e.CommandName;
            int ordenTrabajoMaterialID = e.CommandArgument.SafeIntParse();

            try
            {
                if (command == "cancelar")
                {
                    DespachoBL.Instance.CancelaDespachoPorOrdenTrabajoMaterialID(ordenTrabajoMaterialID, SessionFacade.UserId);
                }
                //Actualizar el grid
                grdMateriales.Rebind();
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }

        /// <summary>
        /// Fijar la fuente de datos para el grid que despliega los materiales.
        /// </summary>
        private void establecerDataSourceGrid()
        {
            grdMateriales.DataSource = OrdenTrabajoSpoolBO.Instance
                                                          .ObtenerMaterialesParaDespacho(EntityID.Value)
                                                          .OrderBy(x => x.EtiquetaMaterial.SafeIntParse(99));
        }
    }
}