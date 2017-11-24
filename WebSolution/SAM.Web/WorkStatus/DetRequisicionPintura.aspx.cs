using System;
using System.Collections.Generic;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities;
using SAM.Entities.Grid;
using SAM.Web.Classes;
using SAM.Web.Common;
using Telerik.Web.UI;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAM.Web.WorkStatus
{
    public partial class DetRequisicionPintura : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoARequisicionPintura(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar una requisición de pintura {1} a la cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_RequisicionesPinturaSpool);
                carga(EntityID.Value);
            }
        }

        /// <summary>
        /// Carga los labels necesarios para mostrar el reporte seleccionada
        /// </summary>
        /// <param name="requisicionPinturaID"></param>
        private void carga(int requisicionPinturaID)
        {
            RequisicionPintura requisicionPintura = ReporteRequisicionPinturaBO.Instance.ObtenerDetalleRequisicionPintura(requisicionPinturaID);
            proyHeader.BindInfo(requisicionPintura.ProyectoID);
            lblNumeroDeRequisicion.Text = requisicionPintura.NumeroRequisicion;
            lblFechaRequisicion.Text = requisicionPintura.FechaRequisicion.ToString("d");

            EstablecerDataSource(requisicionPinturaID);
            grdDetRequisicionPintura.DataBind();

            titulo.NavigateUrl += "?RID=" + requisicionPinturaID;
        }

        protected void grdDetRequisicionPintura_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource(EntityID.Value);
        }

        /// <summary>
        /// establece el datasource con el reporte establecido
        /// </summary>
        /// <param name="requisicionPinturaID"></param>
        private void EstablecerDataSource(int requisicionPinturaID)
        {
            List<GrdDetReporteReqPintura> NumeroJuntaReportePnd = DetRequisicionPinturaBO.Instance.ObtenerDetalleRequisicionPintura(requisicionPinturaID);
            grdDetRequisicionPintura.DataSource = NumeroJuntaReportePnd;
            lblTotalSpools.Text = NumeroJuntaReportePnd.Count.ToString();
           
        }
       

        protected void grdDetRequisicionPintura_OnItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                try
                {
                    int DetReqPinturaID = e.CommandArgument.SafeIntParse();

                    DetRequisicionPinturaBO.Instance.Borra(DetReqPinturaID);
                    EstablecerDataSource(EntityID.Value);
                    grdDetRequisicionPintura.DataBind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void grdDetRequisicionPintura_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                GrdDetReporteReqPintura item = (GrdDetReporteReqPintura)e.Item.DataItem;
                if (item.TieneHold)
                {
                    ImageButton btnBorrar = e.Item.FindControl("btnBorrar") as ImageButton;
                    btnBorrar.Visible = false;
                }
            }
        }
    }
}