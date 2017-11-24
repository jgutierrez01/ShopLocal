using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Mimo.Framework.Exceptions;
using SAM.BusinessLogic.Produccion;
using SAM.Entities.Grid;
using SAM.Web.Classes;
using Telerik.Web.UI;
using Mimo.Framework.Extensions;

namespace SAM.Web.Produccion
{
    public partial class CorteJunta : SamPaginaPrincipal
    {
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

        private int NumeroControlID
        {
            get
            {
                return ViewState["NumeroControlID"].SafeIntParse();
            }
            set
            {
                ViewState["NumeroControlID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.prod_CortesJunta);
            }
        }
        
        /// <summary>
        /// Metodo que se dispara cuando el usuario presiona el boton de mostrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMostrarClick(object sender, EventArgs e)
        {
            ProyectoID = filtroGenerico.ProyectoSelectedValue.SafeIntParse();
            NumeroControlID = filtroGenerico.NumeroControlSelectedValue.SafeIntParse();

            grdCorteJunta.Rebind();
            grdCorteJunta.Visible = true;
        }

        /// <summary>
        /// Metodo que se dispuara cuendo el grid requiere su datasource
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdCorteJunta_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            establecerDataSource();
        }

        /// <summary>
        /// Establece el datasource del grid
        /// </summary>
        private void establecerDataSource()
        {   
                grdCorteJunta.DataSource =
                    JuntaWorkstatusBL.Instance.ObtenerPorOrdenTrabajoSpoolID(ProyectoID, NumeroControlID);
        }
        
        /// <summary>
        /// Metodo que se dispara cuando el usuario hace uso de algun comando del grid
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdCorteJunta_ItemCommand(object source, GridCommandEventArgs e)
        {
            int juntaWorkstatusID = e.CommandArgument.SafeIntParse();

            try
            {
                if (e.CommandName == "Eliminar")
                {
                    JuntaWorkstatusBL.Instance.Eliminar(juntaWorkstatusID);
                }
                else if (e.CommandName == "Cortar")
                {
                    JuntaWorkstatusBL.Instance.Cortar(juntaWorkstatusID, SessionFacade.UserId);
                }
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
            grdCorteJunta.Rebind();
        }

        /// <summary>
        /// Metodo que se dispara para cada elemento en el datasource cuando se asocia al grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdCorteJunta_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.DataItem != null)
            {
                //Obtenemos juntaWorkStatus
                GrdJuntaWorkstatus juntaWorkstatus = ((GrdJuntaWorkstatus)e.Item.DataItem);
                GridDataItem gridItem = e.Item as GridDataItem;
                if(gridItem != null)
                {
                    //Obtenemos los links para saber si deben ser visibles o no
                    ImageButton imgCortar = gridItem.FindControl("imgCortar") as ImageButton;
                    ImageButton imgBorrar = gridItem.FindControl("imgBorrar") as ImageButton;
                    if (imgCortar != null)
                    {
                        //Si el Spool esta en Hold la junta no se debe poder cortar
                        imgCortar.Visible = !juntaWorkstatus.SpoolHold;
                    }

                    if (imgBorrar != null)
                    {
                        //Si Spool no esta en hold y la junta tiene corte mostramos el link de eliminar
                        imgBorrar.Visible = !juntaWorkstatus.SpoolHold && juntaWorkstatus.TieneCorte;
                    }
                    
                }
            }
        }

        /// <summary>
        /// Metodo que se dispara cuando el usuario actualiza el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdCorteJunta.Rebind();
        }
    }
}
