using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.Web.Classes;
using SAM.Web.Common;
using Telerik.Web.UI;
using SAM.BusinessObjects.Materiales;
using SAM.Entities;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Validations;
using SAM.BusinessObjects.Excepciones;

namespace SAM.Web.Materiales
{
    public partial class ReqPinturaNumUnico : SamPaginaPrincipal
    {
        #region Propiedades ViewState
        
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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.mat_RequisicionesPintura);
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
                grdRequisicion.Visible = false;
            }
        }

        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            grdRequisicion.Visible = true;
            establecerDataSource();
            grdRequisicion.DataBind();
        }

        protected void grdRequisicion_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            establecerDataSource();
        }

        protected void grdRequisicion_OnItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                GridCommandItem item = (GridCommandItem)e.Item;

                HyperLink lnkRequisicion = (HyperLink)item.FindControl("lnkRequisicion");
                HyperLink imgRequisicion = (HyperLink)item.FindControl("imgRequisicion");

                string jsLink = string.Format("javascript:Sam.Materiales.AbrePopUpRequisicion('{0}','{1}');", grdRequisicion.ClientID, _proyecto);
                

                lnkRequisicion.NavigateUrl = jsLink;
                imgRequisicion.NavigateUrl = jsLink;
              
            }
        }

        private void establecerDataSource()
        {
            grdRequisicion.DataSource = NumeroUnicoBO.Instance.ObtenerParaReqPinturaNumUnico(_proyecto);
        }


        protected void btnActualiza_Click(object sender, EventArgs e)
        {
            grdRequisicion.ResetBind();
            establecerDataSource();
            grdRequisicion.DataBind();
        }

    }
}