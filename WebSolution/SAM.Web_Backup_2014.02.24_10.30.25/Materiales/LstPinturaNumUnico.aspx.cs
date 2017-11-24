using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using Telerik.Web.UI;
using SAM.BusinessObjects.Materiales;
using SAM.Entities;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Validations;
using SAM.BusinessObjects.Excepciones;


namespace SAM.Web.Materiales
{
    public partial class LstPinturaNumUnico : SamPaginaPrincipal
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
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.mat_PinturaNumUnico);
                cargarCombo();
            }
        }

        private void cargarCombo()
        {
            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
        }

        protected void ddlProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            _proyecto = ddlProyecto.SelectedValue.SafeIntParse();

            if (_proyecto > 0)
            {
                proyEncabezado.Visible = true;
                proyEncabezado.BindInfo(_proyecto);
            }
            else
            {
                proyEncabezado.Visible = false;
            }
        }

        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            grdPintura.Visible = true;
            establecerDataSource();
            grdPintura.DataBind();
        }

        protected void grdPintura_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            establecerDataSource();
        }

        protected void grdPintura_OnItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                GridCommandItem item = (GridCommandItem)e.Item;

                HyperLink lnkRequisicion = (HyperLink)item.FindControl("lnkPintar");
                HyperLink imgRequisicion = (HyperLink)item.FindControl("imgPintar");

                string jsLink = string.Format("javascript:Sam.Materiales.AbrePopUpRequisitarPintura('{0}','{1}');", grdPintura.ClientID,_proyecto);

                lnkRequisicion.NavigateUrl = jsLink;
                imgRequisicion.NavigateUrl = jsLink;

            }
        }

        private void establecerDataSource()
        {
            grdPintura.DataSource = NumeroUnicoBO.Instance.ObtenerParaPinturaNumUnico(_proyecto);
        }

        protected void btnActualiza_Click(object sender, EventArgs e)
        {
            grdPintura.ResetBind();
            establecerDataSource();
            grdPintura.DataBind();
        }
        
    }
}