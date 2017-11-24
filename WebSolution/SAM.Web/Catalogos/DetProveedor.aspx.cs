using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.Entities;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Exceptions;
using SAM.Web.Common;

namespace SAM.Web.Catalogos
{
    public partial class DetProveedor : SamPaginaCatalogo
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Proveedores);
                if (EntityID != null)
                {
                    Carga();
                }

            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

            try
            {
                Proveedor p;

                if (EntityID != null)
                {
                    p = ProveedorBO.Instance.ObtenerConContacto(EntityID.Value);
                    p.VersionRegistro = VersionRegistro;
                }
                else
                {
                    p = new Proveedor();
                    p.Contacto = new Contacto();
                }

                p.StartTracking();
                p.Contacto.StartTracking();

                Unmap(p);
                p.UsuarioModifica = SessionFacade.UserId;
                p.FechaModificacion = DateTime.Now;
                p.Contacto.UsuarioModifica = SessionFacade.UserId;
                p.Contacto.FechaModificacion = DateTime.Now;
                p.Contacto.StopTracking();
                p.StopTracking();

                ProveedorBO.Instance.Guarda(p);
                Response.Redirect(WebConstants.CatalogoUrl.LST_PROVEEDOR);
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }

        }

        /// <summary>
        /// metodo para cargar los datos y mostrarlos en el webform cuando se edita un registro.
        /// </summary>
        private void Carga()
        {
            Proveedor proveedor = ProveedorBO.Instance.ObtenerConContacto(EntityID.Value);
            Map(proveedor);
            VersionRegistro = proveedor.VersionRegistro;
        }
    }
}