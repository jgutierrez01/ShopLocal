using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Utilerias;
using SAM.BusinessObjects.Produccion;
using SAM.Web.Classes;
using SAM.Web.Common;
using Telerik.Web.UI;
using SAM.Entities.Grid;

namespace SAM.Web.Administracion
{
    public partial class EstimacionDeJunta : SamPaginaPrincipal
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

        //metodo para cargar el combo "ddlProyecto".
        private void CargaCombo()
        {
            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
        }

        /// <summary>
        /// muestra el header
        /// </summary>
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
        /// traera los registros de la base de datos y se mandara a crear el dataset al final se asigna al grid
        /// </summary>
        private void EstablecerDataSource()
        {
            if (!string.IsNullOrEmpty(_proyecto.ToString()))
            {
                List<GrdEstimacionJuntaCompleta> listaGrid =
                    EstimacionJuntaBO.Instance.ObtenerEstimacionJuntaPorProyectoID(
                        _proyecto);

                grdEstimacionJunta.DataSource = PivoteEstimacion.PivotearDatosEstimacionJunta(listaGrid, _proyecto);
            }
            else
            {
                grdEstimacionJunta.DataSource = new List<object>();
            }
        }

        protected void grdEstimacionJunta_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        /// <summary>
        /// manda a llamar el rebind del grdEstimacion para que se generen los renglones
        /// </summary>
        protected void btnMostrar_OnClick(object sender, EventArgs e)
        {
            Validate();

            if (IsValid)
            {
                phGrid.Visible = true;
                _proyecto = ddlProyecto.SelectedValue.SafeIntParse();
                grdEstimacionJunta.Rebind();
            }
        }

        /// <summary>
        /// se les asigna el width del header a las columnas generadas por el dataset  asi 
        /// como ocultar el juntaworkstatus para trabajar con la tabla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdEstimacionJunta_OnColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            if (e.Column is GridBoundColumn && (e.Column as GridBoundColumn).DataField == "JuntaWorkstatusId")
            {
                (e.Column as GridBoundColumn).Visible = false;
            }
            //if (e.Column is GridBoundColumn && (e.Column as GridBoundColumn).DataField == "Diametro")
            //{
            //    (e.Column as GridBoundColumn).DataFormatString = "{0:#0.000}";
            //}
            else if (e.Column is GridCheckBoxColumn && (e.Column as GridCheckBoxColumn).DataField == TitulosColumnaEstimacionJunta.InspeccionDimensional)
            {
                (e.Column as GridCheckBoxColumn).HeaderStyle.Width = 145;
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

        protected void grdEstimacionJunta_ItemCreated(object sender, GridItemEventArgs e)
        {
            GridCommandItem commandItem = e.Item as GridCommandItem;

            if (commandItem != null)
            {
                HyperLink hlEstimado = (HyperLink)commandItem.FindControl("hlEstimado");
                HyperLink hlEstimadoImagen = (HyperLink)commandItem.FindControl("hlEstimadoImagen");

                string jsLink = string.Format("javascript:Sam.Administracion.AbrePopupEstimacionJunta('{0}');",
                                                grdEstimacionJunta.ClientID);

                hlEstimado.NavigateUrl = jsLink;
                hlEstimadoImagen.NavigateUrl = jsLink;
            }
        }

        protected void btnWrapper_Click(object sender, EventArgs e)
        {
            EstablecerDataSource();
            grdEstimacionJunta.DataBind();
        }

    }
}