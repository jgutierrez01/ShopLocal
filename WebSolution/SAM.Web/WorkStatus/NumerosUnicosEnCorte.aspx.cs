using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Materiales;
using SAM.Web.Common;
using Telerik.Web.UI;
using Mimo.Framework.Exceptions;

namespace SAM.Web.WorkStatus
{
    public partial class NumerosUnicosEnCorte : SamPaginaPrincipal
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

        private int OrdenTrabajo
        {
            get
            {
                return ViewState["OrdenTrabajo"].SafeIntParse();
            }
            set
            {
                ViewState["OrdenTrabajo"] = value;
            }
        }

        private int NumeroUnicoID
        {
            get
            {
                return ViewState["NumeroUnicoID"].SafeIntParse();
            }
            set
            {
                ViewState["NumeroUnicoID"] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_NumUnicoTransferencia);
            }
        }
        

        /// <summary>
        /// Va a la capa de negocios para obtener la información requerida y la asigna al grid
        /// </summary>
        private void EstablecerDataSource()
        {
            grdNumeroUnicos.DataSource = NumeroUnicoBO.Instance.ObtenerNumerosUnicosEnTransferencia(ProyectoID,
                                                                                                    OrdenTrabajo,
                                                                                                    NumeroUnicoID);

        }

        #region Eventos

        protected void grdNumeroUnicos_OnNeedDataSource(object sender, EventArgs e)
        {
            EstablecerDataSource();
        }
        
       

        /// <summary>
        /// Muestra el grid en base a los filtros seleccionados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            ProyectoID = filtroGenerico.ProyectoSelectedValue.SafeIntParse();
            OrdenTrabajo = filtroGenerico.OrdenTrabajoSelectedValue.SafeIntParse();
            NumeroUnicoID = filtroGenerico.NumeroUnicoSelectedValue.SafeIntParse();

            EstablecerDataSource();
            grdNumeroUnicos.DataBind();
            grdNumeroUnicos.Visible = true;
        }

        /// <summary>
        /// Elimina el registro seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdNumerosUnicos_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                try
                {
                    int numeroUnicoSegmentoID = e.CommandArgument.SafeIntParse();
                    NumeroUnicoBO.Instance.BorraNumeroUnicoDeTransferencia(numeroUnicoSegmentoID, SessionFacade.UserId);
                    EstablecerDataSource();
                    grdNumeroUnicos.DataBind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                    return;
                }

            }
        }

        #endregion
    }
}