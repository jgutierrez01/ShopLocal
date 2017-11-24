using System;
using SAM.Web.Classes;
using SAM.BusinessObjects.Catalogos;
using SAM.Entities;
using Mimo.Framework.Exceptions;

namespace SAM.Web.Catalogos
{
    public partial class DetCedula : SamPaginaCatalogo
    {
        /// Inicializar los controles con el contenido de BD
        /// 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Cedulas);
                if (EntityID.HasValue)
                {
                    carga();
                }
            }
        }

        private void carga()
        {
            Cedula cedula = CedulaBO.Instance.Obtener(EntityID.Value);
            Map(cedula);
            VersionRegistro = cedula.VersionRegistro;
        }

        /// Hace unmapping de los controles básicos y del repeater hacia la entidad perfil.
        /// Luego toma esta entidad y es la que persiste hacia los business objects.
        /// 
        public void btnGuardar_OnClick(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    GuardaEntidadPorId(WebConstants.CatalogoUrl.LST_CEDULA,
                                        CedulaBO.Instance.Obtener,
                                        CedulaBO.Instance.Guarda);
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }
    }
}