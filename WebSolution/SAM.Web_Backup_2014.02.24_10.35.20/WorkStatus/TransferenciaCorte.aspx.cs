using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Entities;
using SAM.BusinessObjects.Produccion;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Materiales;
using SAM.Web.Classes;
using Telerik.Web.UI;
using SAM.BusinessObjects.Proyectos;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Exceptions;

namespace SAM.Web.WorkStatus
{
    public partial class TransferenciaCorte : SamPaginaPrincipal
    {
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

        private int OrdenTrabajoID
        {
            get
            {
                return ViewState["OrdenTrabajoID"].SafeIntParse();
            }
            set
            {
                ViewState["OrdenTrabajoID"] = value;
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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_DespachoACorte);
            }
        }

        /// <summary>
        /// Va a la capa de negocios para obtener la información requerida y la asigna al grid
        /// </summary>
        private void EstablecerDataSource()
        {
            grdNumeroUnicos.DataSource = NumeroUnicoBO.Instance.ObtenerNumerosUnicosATransferir(OrdenTrabajoID, OrdenTrabajoSpoolID, ProyectoID);

        }

        #region Eventos


        protected void grdNumeroUnicos_OnNeedDataSource(object sender, EventArgs e)
        {
            EstablecerDataSource();
        }

      
        protected void grdNumerosUnicos_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                GridCommandItem item = (GridCommandItem)e.Item;

                HyperLink hypTransferir = (HyperLink)item.FindControl("hypTransferir");
                HyperLink imgTransferir = (HyperLink)item.FindControl("imgTransferir");

                string jsLink = string.Format("javascript:Sam.Workstatus.AbrePopUpTranferenciaCorte('{0}','{1}','{2}');",
                                               ProyectoID, grdNumeroUnicos.ClientID, OrdenTrabajoID);

                hypTransferir.NavigateUrl = jsLink;                
                imgTransferir.NavigateUrl = jsLink;
            }
        }

        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            ProyectoID = filtroGenerico.ProyectoSelectedValue.SafeIntParse();
            OrdenTrabajoID = filtroGenerico.OrdenTrabajoSelectedValue.SafeIntParse();
            OrdenTrabajoSpoolID = filtroGenerico.NumeroControlSelectedValue.SafeIntParse();
                        
            EstablecerDataSource();
            grdNumeroUnicos.DataBind();
            grdNumeroUnicos.Visible = true;
        }

        protected void lnkActualizar_Click(object sender, EventArgs e)
        {
            EstablecerDataSource();
            grdNumeroUnicos.DataBind();
        }
       
        #endregion
    }
}