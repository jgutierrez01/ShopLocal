using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Produccion;
using SAM.Entities;
using Telerik.Web.UI;

namespace SAM.Web.Produccion
{
    public partial class CongeladosOrdenTrabajo : SamPaginaPrincipal
    {
        private int _proyectoID
        {
            get
            {
                if (ViewState["_proyectoID"] == null)
                {
                    ViewState["_proyectoID"] = -1;
                }
                return ViewState["_proyectoID"].SafeIntParse();
            }
            set
            {
                ViewState["_proyectoID"] = value;
            }
        }

        private int _numeroControl
        {
            get
            {
                if (ViewState["_numeroControl"] == null)
                {
                    ViewState["_numeroControl"] = -1;
                }
                return ViewState["_numeroControl"].SafeIntParse();
            }
            set
            {
                ViewState["_numeroControl"] = value;
            }
        }

        private int _ordenTrabajo
        {
            get
            {
                if (ViewState["_ordenTrabajo"] == null)
                {
                    ViewState["_ordenTrabajo"] = -1;
                }
                return ViewState["_ordenTrabajo"].SafeIntParse();
            }
            set
            {
                ViewState["_ordenTrabajo"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.prod_CongeladosOrdenTrabajo);
            }
        }

        protected void btnMostrarClick(object sender, EventArgs e)
        {
            _proyectoID = filtroGenerico.ProyectoSelectedValue.SafeIntParse();
            _ordenTrabajo = filtroGenerico.OrdenTrabajoSelectedValue.SafeIntParse();
            _numeroControl = filtroGenerico.NumeroControlSelectedValue.SafeIntParse();
            MuestraGrid();
            
        }
        public void MuestraGrid()
        {
            EstablecerDataSource();            
            phSpools.Visible = true;
            grdSpools.DataBind();
        }

        public void EstablecerDataSource()
        {
            List<GrdCongeladosOrdenTrabajo> Datasource = CongeladosBO.Instance.obtenerListadoCongeladoOrdenTrabajo(_numeroControl , _ordenTrabajo, _proyectoID);
            grdSpools.DataSource = Datasource;

        }

        /// <summary>
        /// Se dispara cuando el proyecto seleccionado cambia.  En caso de seleccionar
        /// un proyecto válido se muestra el project header control con sus datos, de lo contrario
        /// se oculta el control.
        /// </summary>
        protected void proyecto_Cambio(object sender, EventArgs e)
        {
            int proyectoID = filtroGenerico.ProyectoSelectedValue.SafeIntParse();

            if (proyectoID > 0)
            {
                headerProyecto.Visible = true;
                headerProyecto.BindInfo(filtroGenerico.ProyectoSelectedValue.SafeIntParse());
            }
            else
            {
                headerProyecto.Visible = false;              
            }
        }

        protected void grdSpools_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                GrdCongeladosOrdenTrabajo item = (GrdCongeladosOrdenTrabajo)e.Item.DataItem;
                GridDataItem grdItem = (GridDataItem)e.Item;
                if(item.Estatus != "Congelado")
                    grdItem["editar_h"].Controls.Clear();
            }
        }

        protected void grdSpools_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        protected void btnWrapper_Click(object sender, EventArgs e)
        {
            EstablecerDataSource();
            grdSpools.DataBind();
        }
    }
}