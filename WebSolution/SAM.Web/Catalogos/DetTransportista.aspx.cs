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
    public partial class DetTransportista : SamPaginaCatalogo
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Transportistas);
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
                Transportista t;

                if (EntityID != null)
                {
                    t = TransportistaBO.Instance.ObtenerConContacto(EntityID.Value);
                    t.VersionRegistro = VersionRegistro;
                }
                else
                {
                    t = new Transportista();
                    t.Contacto = new Contacto();
                }

                t.StartTracking();
                t.Contacto.StartTracking();

                Unmap(t);
                t.UsuarioModifica = SessionFacade.UserId;
                t.FechaModificacion = DateTime.Now;
                t.Contacto.UsuarioModifica = SessionFacade.UserId;
                t.Contacto.FechaModificacion = DateTime.Now;
                t.Contacto.StopTracking();
                t.StopTracking();

                TransportistaBO.Instance.Guarda(t);
                Response.Redirect(WebConstants.CatalogoUrl.LST_TRANSPORTISTA);
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
            Transportista transportista = TransportistaBO.Instance.ObtenerConContacto(EntityID.Value);
            Map(transportista);
            VersionRegistro = transportista.VersionRegistro;
        }
    }
}