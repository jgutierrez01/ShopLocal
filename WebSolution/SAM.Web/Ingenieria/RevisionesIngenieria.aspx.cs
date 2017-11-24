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
using System.Linq.Dynamic;
using SAM.BusinessObjects.Catalogos;
using SAM.Entities.Cache;
using SAM.Entities.Grid;
using SAM.BusinessLogic.Ingenieria;

namespace SAM.Web.Ingenieria
{
    public partial class RevisionesIngenieria : SamPaginaPrincipal
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
                if (!IsPostBack)
                {                    
                    Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.ing_IngenieriaProyecto);
                    ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
                    if (Request.QueryString["PID"] != null)
                    {
                        _proyectoID = Request.QueryString["PID"].SafeIntParse();
                        ddlProyecto.SelectedValue = _proyectoID.SafeStringParse();
                        proyEncabezado.Visible = true;
                        proyEncabezado.BindInfo(_proyectoID);
                        MuestraGrid();
                    }
                }
        }

        protected void grdSpools_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            establecerDataSource();
        }

        protected void ddlProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            _proyectoID = ddlProyecto.SelectedValue.SafeIntParse();

            if (_proyectoID > 0)
            {
                proyEncabezado.Visible = true;
                proyEncabezado.BindInfo(_proyectoID);
            }
            else
            {
                proyEncabezado.Visible = false;
                
            }

        }
        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            MuestraGrid();
        }

        protected void MuestraGrid()
        {
            configurarColumnasProyecto();
            establecerDataSource();
            grdSpools.Visible = true;
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

                        SAM.Entities.Personalizadas.DetSpoolHistorico spool = SpoolBO.Instance.ObtenerDetalleCompletoHistorico(spoolID);

                        RadPageView rpv; 
                        
                        rpv = rmpDetalle.FindPageViewByID("rpvMateriales");

                        MaterialROHistorico materiales = (MaterialROHistorico)rpv.FindControl("materiales");
                        materiales.Map(spool.Materiales);

                        rpv = rmpDetalle.FindPageViewByID("rpvJuntas");

                        JuntaROHistorico juntas = (JuntaROHistorico)rpv.FindControl("juntas");
                        juntas.Map(spool.Juntas);

                        rpv = rmpDetalle.FindPageViewByID("rpvCortes");

                        CorteROHistorico cortes = (CorteROHistorico)rpv.FindControl("cortes");
                        cortes.Map(spool.Cortes);
                    }
                }
            }
        }

        protected void grdSpools_ItemDataBound(object source, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                GrdIngSpool ingSpool = (GrdIngSpool)e.Item.DataItem;
                int idSpool = dataItem["SpoolID"].Text.SafeIntParse();
            }
        }

        /// <summary>
        /// Método que muestra las columnas personalizadas del proyecto par la nomenclatura del spool
        /// </summary>
        private void configurarColumnasProyecto()
        {
            ProyectoCache p = CacheCatalogos.Instance.ObtenerProyectos().Where(x => x.ID == _proyectoID).Single();

            if (p.ColumnasNomenclatura > 0)
            {
                foreach (NomenclaturaStruct n in p.Nomenclatura)
                {
                    GridColumn col = grdSpools.MasterTableView.Columns.FindByUniqueName("Segmento" + n.Orden);
                    col.HeaderText = n.NombreColumna;
                    col.Visible = true;
                    col.HeaderStyle.Width = Unit.Pixel(100);
                    col.FilterControlWidth = Unit.Pixel(60);
                }
            }
        }

        private void establecerDataSource()
        {           
            grdSpools.DataSource = SpoolBO.Instance.ObtenerHistoricoSpoolPorProyecto(_proyectoID, rcbSpool.Text);
        }

        protected void grdSpools_OnItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                GridCommandItem item = (GridCommandItem)e.Item;
            }
        }
             
        protected void cusRcbSpool_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = rcbSpool.SelectedValue.SafeIntParse() > 0;
        }
    }
}