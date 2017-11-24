using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Entities;
using SAM.BusinessObjects.Workstatus;
using SAM.Web.Classes;
using SAM.Web.Common;
using Telerik.Web.UI;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Excepciones;
using SAM.Entities.Grid;

namespace SAM.Web.WorkStatus
{
    public partial class DetRequisicionSpool : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoARequisicionSpool(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar una requisición {1} a la cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_RequisicionesPruebas);
                cargarDatos();
            }
        }

        public void cargarDatos()
        {
            RequisicionSpool requisicionSpool = RequisicionBO.Instance.DetalleRequisicionSpool(EntityID.Value);
            proyHeader.BindInfo(requisicionSpool.ProyectoID);
            lblNumeroRequisicionData.Text = requisicionSpool.NumeroRequisicion;
            lblFechaRequisicionData.Text = DateTime.Parse(requisicionSpool.FechaRequisicion.SafeStringParse()).ToShortDateString();
            lblObservacionesData.Text = requisicionSpool.Observaciones;
            lblTipoPruebaData.Text = requisicionSpool.TipoPruebaSpool.Nombre;
            lblTotalSpoolsData.Text = requisicionSpool.SpoolRequisicion.Count().SafeStringParse();

            grdSpools.Rebind();
            lblHeader.NavigateUrl += "?RID=" + EntityID.Value;
        }

        protected void grdSpools_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            establecerDataSource();
        }

        protected void grdSpools_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                try
                {
                    int spoolRequisicionID = e.CommandArgument.SafeIntParse();
                    SpoolRequisicionBO.Instance.Borra(spoolRequisicionID);
                }
                catch (ExcepcionRelaciones ex)
                {
                    RenderErrors(ex, "");
                }
                finally
                {
                    cargarDatos();
                    grdSpools.Rebind();
                }
            }
        }

        private void establecerDataSource()
        {
            grdSpools.DataSource = SpoolRequisicionBO.Instance.DetalleRequisicionesSpool(EntityID.Value);
        }

        protected void grdSpools_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                GrdDetRequisicionesSpool item = (GrdDetRequisicionesSpool)e.Item.DataItem;
                if (item.TieneHold)
                {
                    ImageButton btnBorrar = e.Item.FindControl("btnBorrar") as ImageButton;
                    btnBorrar.Visible = false;
                }
            }
        }
    }
}