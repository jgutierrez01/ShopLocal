using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.Web.Controles.Cliente;
using SAM.BusinessLogic;
using Mimo.Framework.Data;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;
using SAM.Entities;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Proyectos;

namespace SAM.Web.Catalogos
{
    public partial class DetCliente : SamPaginaPrincipal
    {
        private Entities.Cliente _cliente;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Clientes);
                if (EntityID != null)
                {
                    carga();
                }
            }
        }

        private void carga()
        {
            _cliente = ClienteBO.Instance.ObtenerConContactoCliente(EntityID.Value);
            ctrlCliente.Map(_cliente);
            ctrlContacto.Map(_cliente.ContactoCliente);
            VersionRegistro = _cliente.VersionRegistro;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Entities.Cliente cliente;

            if (EntityID.HasValue)
            {
                cliente = ClienteBO.Instance.ObtenerConContactoCliente(EntityID.Value);
                cliente.VersionRegistro = VersionRegistro;
            }
            else
            {
                cliente = new Entities.Cliente();
            }

            cliente.StartTracking();
            ctrlCliente.Unmap(cliente);
            ctrlContacto.Unmap(cliente.ContactoCliente);
            cliente.UsuarioModifica = SessionFacade.UserId;
            cliente.FechaModificacion = DateTime.Now;
            cliente.StopTracking();

            try
            {
                ClienteBO.Instance.Guarda(cliente);
                Response.Redirect(WebConstants.CatalogoUrl.LST_CLIENTE);
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }
    }
}