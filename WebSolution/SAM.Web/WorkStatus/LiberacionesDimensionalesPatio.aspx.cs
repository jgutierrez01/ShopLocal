using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities.Grid;
using SAM.Web.Classes;
using SAM.Web.Common;
using Telerik.Web.UI;

namespace SAM.Web.WorkStatus
{
    public partial class LiberacionesDimensionalesPatio : SamPaginaPrincipal
    {
        #region ViewState de los filtros

        /// <summary>
        /// ID del proyecto seleccionado en el dropdown
        /// </summary>
        private int _proyecto
        {
            get
            {
                return (int)ViewState["proyecto"];
            }
            set
            {
                ViewState["proyecto"] = value;
            }
        }

        /// <summary>
        /// fecha de inicio en la cual se hara el filtro
        /// </summary>
        private DateTime? _desde
        {
            get
            {
                if (ViewState["desde"] != null)
                {
                    return (DateTime)ViewState["desde"];
                }
                return null;
            }
            set
            {
                ViewState["desde"] = value;
            }
        }

        /// <summary>
        ///  fecha final en la cual se hara el filtro
        /// </summary>
        private DateTime? _hasta
        {
            get
            {
                if (ViewState["hasta"] != null)
                {
                    return (DateTime)ViewState["hasta"];
                }
                return null;
            }
            set
            {
                ViewState["hasta"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_LiberacionDimensionalPatio);
                CargaCombo();
            }
        }

        /// <summary>
        /// desplega el header al seleccionar el proyecto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProyecto_SelectedItemChanged(object sender, EventArgs e)
        {
            int proyectoID = ddlProyecto.SelectedValue.SafeIntParse();

            if (proyectoID > 0)
            {
                proyHeader.BindInfo(proyectoID);
                proyHeader.Visible = true;
            }
            else
            {
                proyHeader.Visible = false;
            }
        }

        /// <summary>
        /// guarda los filtros en viewState para despues generar el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMostrar_OnClick(object sender, EventArgs e)
        {
            Validate();

            if (IsValid)
            {
                //Guardar en ViewState los filtros
                _proyecto = ddlProyecto.SelectedValue.SafeIntParse();
                _desde = dtpDesde.SelectedDate;
                _hasta = dtpHasta.SelectedDate;

                phGrid.Visible = true;
                grdInspeccionDimencionalPatio.Rebind();
            }
        }

        /// <summary>
        /// establece el datasource filtrado en el grid
        /// </summary>
        private void EstablecerDataSource()
        {
            //llama a llenar el grid con los filtros
            grdInspeccionDimencionalPatio.DataSource = LiberacionDimensionalPatioBO.Instance.ObtenerConFiltros(_proyecto,
                                                                                                                _desde,
                                                                                                                _hasta
                                                                                                               );
        }

        /// <summary>
        /// llena los ddl con sus datos correspondientes
        /// </summary>
        private void CargaCombo()
        {
            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);

        }

        /// <summary>
        /// manda a llamar el metodo de establecerDataSource para llenar el grid
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdInspeccionDimencionalPatio_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        protected void grdInspeccionDimansionalPatio_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                GrdLiberacionDimansionalPatio Liberacion = (GrdLiberacionDimansionalPatio)item.DataItem;

                CheckBox hlSelect = (CheckBox)item["chk_h"].Controls[0];

                if (hlSelect != null)
                {
                    if (Liberacion.Hold == true)
                    {
                        hlSelect.Enabled = false;
                    }
                }
            }
        }

        protected void grdInspeccionDimansionalPatio_ItemCreated(object sender, GridItemEventArgs e)
        {
            GridCommandItem commandItem = e.Item as GridCommandItem;

            if (commandItem != null)
            {
                HyperLink hlReporte = (HyperLink)commandItem.FindControl("hlReporte");
                HyperLink hlReporteImagen = (HyperLink)commandItem.FindControl("hlReporteImagen");

                string jsLink = string.Format("javascript:Sam.Workstatus.AbrePopupReporteLiberacionDimencionalPatio('{0}');",
                                                grdInspeccionDimencionalPatio.ClientID);

                hlReporte.NavigateUrl = jsLink;
                hlReporteImagen.NavigateUrl = jsLink;
            }
        }


        protected void btnWrapper_Click(object sender, EventArgs e)
        {
            EstablecerDataSource();
            grdInspeccionDimencionalPatio.DataBind();
        }
    }
}