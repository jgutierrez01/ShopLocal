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
    public partial class DetFabricante : SamPaginaCatalogo
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Fabricantes);
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
                Fabricante f;

                if (EntityID != null)
                {
                    f = FabricanteBO.Instance.ObtenerConContacto(EntityID.Value);
                    f.VersionRegistro = VersionRegistro;
                }
                else
                {
                    f = new Fabricante();
                    f.Contacto = new Contacto();
                }

                f.StartTracking();
                f.Contacto.StartTracking();

                Unmap(f);
                f.UsuarioModifica = SessionFacade.UserId;
                f.FechaModificacion = DateTime.Now;
                f.Contacto.UsuarioModifica = SessionFacade.UserId;
                f.Contacto.FechaModificacion = DateTime.Now;
                f.Contacto.StopTracking();
                f.StopTracking();

                FabricanteBO.Instance.Guarda(f);
                Response.Redirect(WebConstants.CatalogoUrl.LST_FABRICANTE);
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
            Fabricante f = FabricanteBO.Instance.ObtenerConContacto(EntityID.Value);
            Map(f);
            VersionRegistro = f.VersionRegistro;
        }
    }
}