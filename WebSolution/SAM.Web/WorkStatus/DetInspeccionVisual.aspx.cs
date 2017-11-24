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

namespace SAM.Web.WorkStatus
{
    public partial class DetInspeccionVisual : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAInspeccionVisual(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar una inspección visual {1} a la cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }
                titulo.NavigateUrl += "?" + Request.QueryString;
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_InspeccionVisual);
                cargarDatos();
            }
        }

        public void cargarDatos()
        {
            InspeccionVisual inspeccion = InspeccionVisualBO.Instance.DetalleInspeccionVisual(EntityID.Value);
            proyHeader.BindInfo(inspeccion.Proyecto.ProyectoID);
            lblNumeroReporteData.Text = inspeccion.NumeroReporte;            
            lblFechaReporteData.Text = DateTime.Parse(inspeccion.FechaReporte.ToString()).ToShortDateString();
            lblTotalJuntasData.Text = inspeccion.JuntaInspeccionVisual.Count().ToString();
            lblJuntasAprobadasData.Text = inspeccion.JuntaInspeccionVisual.Where(x => x.Aprobado == true).Count().ToString();
            lblJuntasRechazadasData.Text = inspeccion.JuntaInspeccionVisual.Where(x => x.Aprobado == false).Count().ToString();
            grdVisual.Rebind();
        }

        protected void grdVisual_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            establecerDataSource();
        }

        protected void grdVisual_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                try
                {
                    int juntaID = e.CommandArgument.SafeIntParse();
                    JuntaInspeccionVisualBO.Instance.Borra(juntaID, SessionFacade.UserId);
                    
                }
                catch (ExcepcionRelaciones ex)
                {
                    RenderErrors(ex, "");
                }
                finally
                {
                    cargarDatos();
                    grdVisual.Rebind();
                }
            }
        }

        private void establecerDataSource()
        {
            grdVisual.DataSource = InspeccionVisualBO.Instance.ObtenerDetalleInspeccionVisual(EntityID.Value);
        }
    }
}