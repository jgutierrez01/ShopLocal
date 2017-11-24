using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Administracion;
using SAM.BusinessLogic.Utilerias;
using SAM.BusinessLogic.Administracion;
using SAM.Web.Common;

namespace SAM.Web.Administracion
{
    public partial class PopUpNuevoPendiente : SamPaginaPopup
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarCombos();
            }
        }

        private void cargarCombos()
        {
            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
            ddlArea.BindToEnumerableWithEmptyRow(CacheCatalogos.Instance.ObtenerCategoriaPendiente(),"Nombre","ID",-1);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Pendiente p = new Pendiente();
                
            p.StartTracking();
            p.CategoriaPendienteID = ddlArea.SelectedValue.SafeIntParse();
            p.TipoPendienteID = 4;
            p.ProyectoID = ddlProyecto.SelectedValue.SafeIntParse();
            p.Estatus = EstatusPendiente.Abierto;
            p.Titulo = txtTitulo.Text;
            p.Descripcion = txtDescripcion.Text;
            p.FechaApertura = DateTime.Now;
            p.GeneradoPor = SessionFacade.UserId;
            p.AsignadoA = Guid.Parse(rcbResponsable.SelectedValue);
            p.UsuarioModifica = SessionFacade.UserId;
            p.FechaModificacion = DateTime.Now;
            
            PendienteDetalle pd = new PendienteDetalle();
                
            pd.CategoriaPendienteID = p.CategoriaPendienteID;
            pd.EsAlta = true;
            pd.Responsable = p.AsignadoA;
            pd.Estatus = EstatusPendiente.Abierto;
            pd.UsuarioModifica = p.UsuarioModifica;
            pd.FechaModificacion = DateTime.Now;
                
            p.PendienteDetalle.Add(pd);
            p.StopTracking();

            try
            {
                PendienteBL.Instance.Guarda(p, p.AsignadoA, ddlProyecto.SelectedItem.Text, true);
                JsUtils.RegistraScriptActualizaGridGenerico(this);
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex, "vgPendiente");
            }
        }
    }
}