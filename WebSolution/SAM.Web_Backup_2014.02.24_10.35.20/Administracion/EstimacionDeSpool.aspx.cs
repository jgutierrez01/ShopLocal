using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Utilerias;
using SAM.BusinessObjects.Produccion;
using SAM.Entities.Grid;
using SAM.Web.Classes;
using Telerik.Web.UI;

namespace SAM.Web.Administracion
{
    public partial class EstimacionDeSpool : SamPaginaPrincipal
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.adm_Estimaciones);
                CargaCombo();
            }
        }

        private void CargaCombo()
        {
            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
        }

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

        protected void btnMostrar_OnClick(object sender, EventArgs e)
        {
            Validate();

            if (IsValid)
            {
                phGrid.Visible = true;
                _proyecto = ddlProyecto.SelectedValue.SafeIntParse();
                grdEstimacionSpool.Rebind();
            }
        }

        protected void grdEstimacionSpool_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        protected void grdEstimacionSpool_OnColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            if (e.Column is GridBoundColumn && (e.Column as GridBoundColumn).DataField == "WorkstatusSpoolId")
            {
                (e.Column as GridBoundColumn).Visible = false;
            }
            else if (e.Column is GridCheckBoxColumn && (e.Column as GridCheckBoxColumn).DataField == TitulosColumnaEstimacionSpool.InspeccionDimensional)
            {
                (e.Column as GridCheckBoxColumn).HeaderStyle.Width = 145;
                if (LanguageHelper.CustomCulture == LanguageHelper.INGLES)
                {
                    (e.Column as GridCheckBoxColumn).HeaderText = TitulosColumnaEstimacionSpool.InspeccionDimensional;
                }
            }
            else if (e.Column is GridBoundColumn)
            {
                (e.Column as GridBoundColumn).FilterControlWidth = 100;
                (e.Column as GridBoundColumn).HeaderStyle.Width = 150;
            }
            else if (e.Column is GridCheckBoxColumn)
            {
                (e.Column as GridCheckBoxColumn).HeaderStyle.Width = 120;
            }
        }

        protected void grdEstimacionSpool_ItemCreated(object sender, GridItemEventArgs e)
        {
            GridCommandItem commandItem = e.Item as GridCommandItem;

            if (commandItem != null)
            {
                HyperLink hlEstimado = (HyperLink)commandItem.FindControl("hlEstimado");
                HyperLink hlEstimadoImagen = (HyperLink)commandItem.FindControl("hlEstimadoImagen");

                string jsLink = string.Format("javascript:Sam.Administracion.AbrePopupEstimacionSpool('{0}');",
                                                grdEstimacionSpool.ClientID);

                hlEstimado.NavigateUrl = jsLink;
                hlEstimadoImagen.NavigateUrl = jsLink;
            }
        }

        private void EstablecerDataSource()
        {
            if (!string.IsNullOrEmpty(_proyecto.ToString()))
            {
                List<GrdEstimacionSpoolCompleta> listaGrid =
                    EstimacionSpoolBO.Instance.ObtenerEstimacionSpoolPorProyectoID(
                        _proyecto);

                grdEstimacionSpool.DataSource = PivoteEstimacion.PivotearDatosEstimacionSpool(listaGrid, _proyecto);
            }
            else
            {
                grdEstimacionSpool.DataSource = new List<object>();
            }
        }

        protected void btnWrapper_Click(object sender, EventArgs e)
        {
            EstablecerDataSource();
            grdEstimacionSpool.DataBind();
        }
    }
}