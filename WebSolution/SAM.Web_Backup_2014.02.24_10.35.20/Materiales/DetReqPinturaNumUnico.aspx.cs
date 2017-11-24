using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.Entities;
using SAM.BusinessObjects.Materiales;
using Telerik.Web.UI;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;

namespace SAM.Web.Materiales
{
    public partial class DetReqPinturaNumUnico : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoARequisicionNumeroUnico(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar una requisición de pintura de número único {1} a la cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.mat_PinturaNumUnico);
                cargarDatos();
            }
        }

        public void cargarDatos()
        {
            RequisicionNumeroUnico reqNumUnico = RequisicionNumeroUnicoBO.Instance.DetalleRequisicionPinturaNumUnico(EntityID.Value);

            
            lblNumeroRequisicionData.Text = reqNumUnico.NumeroRequisicion;
            lblFechaRequisicionData.Text = DateTime.Parse(reqNumUnico.FechaRequisicion.ToString()).ToShortDateString();
            lblTotalNumUnicosData.Text = String.Format("{0:N0}", reqNumUnico.RequisicionNumeroUnicoDetalle.Count());
            proyEncabezado.BindInfo(reqNumUnico.ProyectoID);
            establecerDataSource();
            grdNumUnicos.DataBind();
        }

        protected void grdNumUnicos_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            establecerDataSource();
            grdNumUnicos.DataBind();
        }

        protected void grdNumUnicos_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                try
                {
                    int requisicionNumeroUnicoDetalleID = e.CommandArgument.SafeIntParse();
                    RequisicionNumeroUnicoDetalleBO.Instance.Borra(requisicionNumeroUnicoDetalleID);
                    cargarDatos();
                    establecerDataSource();
                    grdNumUnicos.DataBind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex, "");
                }
            }
        }

        private void establecerDataSource()
        {
            grdNumUnicos.DataSource = NumeroUnicoBO.Instance.ObtenerDetalleReqPinturaNumUnicos(EntityID.Value);
        }

    }
}