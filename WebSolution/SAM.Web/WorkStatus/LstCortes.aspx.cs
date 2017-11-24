using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Workstatus;
using SAM.Web.Common;
using Telerik.Web.UI;
using SAM.Entities.Grid;
using Mimo.Framework.Exceptions;
using SAM.BusinessLogic.Workstatus;

namespace SAM.Web.WorkStatus
{
    public partial class LstCortes : SamPaginaPrincipal
    {
        private const string JS_POPUP_CORTE = "javascript:Sam.Workstatus.AbrePopupDetalleCorte('{0}');";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_ListadoCortes);
                cargaCombo();
            }
        }

        //metodo para cargar el combo de proyectos.
        private void cargaCombo()
        {
            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
        }
        
        /// <summary>
        /// Va a la capa de negocios para obtener la información requerida y la asigna al grid
        /// </summary>
        private void EstablecerDataSource()
        {
            grdCortes.DataSource = CorteBO.Instance.ObtenerListaCorte(ddlProyecto.SelectedValue.SafeIntParse());
        }

        /// <summary>
        /// Metodo que recarga el data source al grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdCortes_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        protected void grdCortes_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                HyperLink lnkVer = e.Item.FindControl("hlVer") as HyperLink;
                GrdCorte item = e.Item.DataItem as GrdCorte;
                lnkVer.NavigateUrl = string.Format(JS_POPUP_CORTE, item.CorteID);

                if (item.Cancelado)
                {
                    ImageButton imgCancelar = e.Item.FindControl("imgCancelar") as ImageButton;
                    imgCancelar.Visible = false;
                }
            }
        }

        /// <summary>
        /// Método que carga el encabezado del proyecto una vez seleccionada la opción.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            int proyectoID = ddlProyecto.SelectedValue.SafeIntParse();

            if (proyectoID > 0)
            {
                proyEncabezado.BindInfo(proyectoID);
                proyEncabezado.Visible = true;
            }
            else
            {
                proyEncabezado.Visible = false;
            }
        }

        //Método para cargar el grid de acuerdo a los datos seleccionados
        protected void btnMostrar_Click(object sender, EventArgs e)
        {            
            EstablecerDataSource();
            grdCortes.DataBind();
            grdCortes.Visible = true;
        }

        /// <summary>
        /// Metodo que se dispara al click del boton de ver detalle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdCortes_ItemCommand(object sender, GridCommandEventArgs e)
        {
            string command = e.CommandName;
            int corteID = e.CommandArgument.SafeIntParse();

            try
            {
                if (command == "cancelar")
                {
                    CorteBL.Instance.CancelaCorte(corteID, SessionFacade.UserId);
                }
                //Actualizar el grid
                grdCortes.Rebind();
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }

       

    }
}