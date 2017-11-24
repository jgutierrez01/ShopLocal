using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.WebControls;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Ingenieria;
using SAM.Web.Common;
using Telerik.Web.UI;
using SAM.BusinessLogic;
using Mimo.Framework.Exceptions;
using SAM.Entities;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Produccion;
using SAM.Web.Controles.Spool;
//using System.Linq.Dynamic;
using SAM.BusinessObjects.Catalogos;
using SAM.Entities.Grid;

namespace SAM.Web.Calidad
{
    public partial class RevisionSpools : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cal_RevisionSpools);
                ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
            }
        }

        protected void grdSpools_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            establecerDataSource();
        }

        protected void ddlProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            EntityID = ddlProyecto.SelectedValue.SafeIntParse();
            proyEncabezado.Visible = true;
            proyEncabezado.BindInfo(EntityID.Value);
        }
        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            EntityID = ddlProyecto.SelectedValue.SafeIntParse();
            grdSpools.Visible = true;
            establecerDataSource();
            grdSpools.DataBind();
        }

        protected void grdSpools_OnItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            string cmdName = e.CommandName;

            //Sólo si el comando fue expandir/colapser
            if (cmdName == RadGrid.ExpandCollapseCommandName)
            {
                GridDataItem item = e.Item as GridDataItem;

                //Solo vamos por los datos si el usuario decidió expandir el elemento, si decide colapsarlo
                //no vamos a BD
                if (item != null && !item.Expanded)
                {
                    GridNestedViewItem nestedItem = (GridNestedViewItem)item.ChildItem;

                    RadMultiPage rmpDetalle = nestedItem.FindControl("rmpDetalle") as RadMultiPage;

                    if (rmpDetalle != null)
                    {
                        //Hacer los bindings a los controles hijos con la información del spool

                        int spoolID = item.GetDataKeyValue("SpoolID").SafeIntParse();

                        EntityID = spoolID;

                        SAM.Entities.Personalizadas.DetSpool spool = SpoolBO.Instance.ObtenerDetalleCompleto(spoolID);

                        RadPageView rpv = rmpDetalle.FindPageViewByID("rpvMateriales");

                        MaterialRO materiales = (MaterialRO)rpv.FindControl("materiales");
                        materiales.Map(spool.Materiales);

                        rpv = rmpDetalle.FindPageViewByID("rpvJuntas");
                        JuntaRO juntas = (JuntaRO)rpv.FindControl("juntas");
                        juntas.Map(spool.Juntas);

                        rpv = rmpDetalle.FindPageViewByID("rpvCortes");
                        CorteRO cortes = (CorteRO)rpv.FindControl("cortes");
                        cortes.Map(spool.Cortes);
                    }
                }
            }


            if (e.CommandName == "Borrar")
            {
                int clienteID = e.CommandArgument.SafeIntParse();

                try
                {
                    //SpoolBO.Instance.Borra(spoolID);
                    //establecerDataSource();
                    grdSpools.DataBind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void grdSpools_ItemDataBound(object source, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                
                GrdIngSpool dataItem = (GrdIngSpool)e.Item.DataItem;
                int idSpool = dataItem.SpoolID;
                HyperLink lnkPonerHold = e.Item.FindControl("hypPonerHold") as HyperLink;
                HyperLink lnkQuitarHold = e.Item.FindControl("hypQuitarHold") as HyperLink;
                string jsLink = string.Format("javascript:Sam.Ingenieria.AbrePopupDetalleHold('{0}','{1}');", idSpool, TipoHoldSpool.CALIDAD);
                

                if (dataItem.TieneHoldCalidad)
                {
                    lnkPonerHold.Visible = false;
                    lnkQuitarHold.Visible = true;
                    lnkQuitarHold.NavigateUrl = jsLink;
                }
                else
                {
                    lnkQuitarHold.Visible = false;
                    lnkPonerHold.Visible = true;
                    lnkPonerHold.NavigateUrl = jsLink;
                }
            }
        }

        private void establecerDataSource()
        {
            grdSpools.DataSource = SpoolBO.Instance.ObtenerIngPorProyecto(EntityID.Value);
        }

        protected void lnkActualizar_OnClick(object sender, EventArgs e)
        {
            establecerDataSource();
        }

        protected void btnActualiza_Click(object sender, EventArgs e)
        {
            establecerDataSource();
            grdSpools.DataBind();
        }
    }
}