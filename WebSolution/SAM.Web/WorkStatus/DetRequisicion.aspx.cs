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
    public partial class DetRequisicion : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoARequisicion(EntityID.Value))
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
            Requisicion requisicion = RequisicionBO.Instance.DetalleRequisicion(EntityID.Value);
            proyHeader.BindInfo(requisicion.ProyectoID);
            lblNumeroRequisicionData.Text = requisicion.NumeroRequisicion;
            lblFechaRequisicionData.Text = DateTime.Parse(requisicion.FechaRequisicion.SafeStringParse()).ToShortDateString();
            lblObservacionesData.Text = requisicion.Observaciones;
            lblTipoPruebaData.Text = requisicion.TipoPrueba.Nombre;
            lblCodigoData.Text = requisicion.CodigoAsme;
            lblTotalJuntasData.Text = requisicion.JuntaRequisicion.Count().SafeStringParse();

            grdJuntas.Rebind();
            lblHeader.NavigateUrl += "?RID=" + EntityID.Value;
        }

        protected void grdJuntas_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            establecerDataSource();
        }

        protected void grdJuntas_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                try
                {
                    int juntaID = e.CommandArgument.SafeIntParse();
                    JuntaRequisicionBO.Instance.Borra(juntaID);
                    //JuntaInspeccionVisualBO.Instance.Borra(juntaID, SessionFacade.UserId);

                }
                catch (ExcepcionRelaciones ex)
                {
                    RenderErrors(ex, "");
                }
                finally
                {
                    cargarDatos();
                    grdJuntas.Rebind();
                }
            }
        }

        private void establecerDataSource()
        {
            grdJuntas.DataSource = JuntaRequisicionBO.Instance.DetalleRequisiciones(EntityID.Value);
        }

        protected void grdJuntas_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                GrdDetRequisiciones item = (GrdDetRequisiciones)e.Item.DataItem;

                if (item.TieneHold)
                {
                    ImageButton btnBorrar = e.Item.FindControl("btnBorrar") as ImageButton;
                    btnBorrar.Visible = false;
                }
            }
        }
    }
}