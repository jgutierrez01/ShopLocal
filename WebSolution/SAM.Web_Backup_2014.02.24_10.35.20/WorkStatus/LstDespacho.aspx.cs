using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using Telerik.Web.UI;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities.Grid;
using Mimo.Framework.Exceptions;
using SAM.BusinessLogic.Workstatus;

namespace SAM.Web.WorkStatus
{
    public partial class LstDespacho : SamPaginaPrincipal
    {
        private const string JS_POPUP_DESPACHO = "javascript:Sam.Workstatus.AbrePopupDetalleDespacho('{0}');";

        #region Propiedades Privadas
        private int ProyectoID
        {
            get
            {
                return ViewState["ProyectoID"].SafeIntParse();                    
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }
        }

        private int OrdenTrabajoSpoolID
        {
            get
            {
                return ViewState["OrdenTrabajoSpoolID"].SafeIntParse();
            }
            set
            {
                ViewState["OrdenTrabajoSpoolID"] = value;
            }
        }

        private int NumeroUnicoID
        {
            get 
            {
                return ViewState["NumeroUnicoID"].SafeIntParse();
            }
            set 
            {
                ViewState["NumeroUnicoID"] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_Despachos);
            }
        }     

        /// <summary>
        /// Va a la capa de negocios para obtener la información requerida y la asigna al grid
        /// </summary>
        private void EstablecerDataSource()
        {
            if (OrdenTrabajoSpoolID > 0 & NumeroUnicoID > 0)
            {
                grdDespachos.DataSource = DespachoBO.Instance.ObtenerListaDespachosPorNumeroUnicoYODT(ProyectoID, NumeroUnicoID, OrdenTrabajoSpoolID);
            }
            else if (OrdenTrabajoSpoolID > 0)
            {
                grdDespachos.DataSource = DespachoBO.Instance.ObtenerListaDespachosPorODTS(ProyectoID, OrdenTrabajoSpoolID);
            }
            else if (NumeroUnicoID > 0)
            {
                grdDespachos.DataSource = DespachoBO.Instance.ObtenerListaDespachosPorNumeroUnico(ProyectoID, NumeroUnicoID);
            }
        }

        /// <summary>
        /// Metodo que recarga el data source al grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDespachos_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        protected void grdDespachos_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                HyperLink lnkVer = e.Item.FindControl("hlVer") as HyperLink;
                GrdDespacho item = e.Item.DataItem as GrdDespacho;
                lnkVer.NavigateUrl = string.Format(JS_POPUP_DESPACHO, item.DespachoID);

                if (item.Cancelado)
                {
                    ImageButton imgCancelar = e.Item.FindControl("imgCancelar") as ImageButton;
                    imgCancelar.Visible = false;
                }
            }
        }
              

        //Método para cargar el grid de acuerdo a los datos seleccionados
        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            ProyectoID = filtroGenerico.ProyectoSelectedValue.SafeIntParse();
            OrdenTrabajoSpoolID = filtroGenerico.NumeroControlSelectedValue.SafeIntParse();
            NumeroUnicoID = filtroGenerico.NumeroUnicoSelectedValue.SafeIntParse();
           
            Validate();
            if (IsValid)
            {
                
                EstablecerDataSource();
                grdDespachos.DataBind();
                grdDespachos.Visible = true;
            }
        }

        /// <summary>
        /// Metodo que se dispara al click del boton de ver detalle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDespachos_ItemCommand(object sender, GridCommandEventArgs e)
        {
            string command = e.CommandName;
            int despachoID = e.CommandArgument.SafeIntParse();

            try
            {
                if (command == "cancelar")
                {
                    DespachoBL.Instance.CancelaDespachoPorDespachoID(despachoID, SessionFacade.UserId);
                }
                //Actualizar el grid
                grdDespachos.Rebind();
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }

        protected void cvSeleccionFiltro_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            if (NumeroUnicoID <= 0 && OrdenTrabajoSpoolID <= 0)
            {
                e.IsValid = false;
            }
        }
       
    }
}