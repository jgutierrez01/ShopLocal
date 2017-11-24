using System;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.WebControls;
using SAM.Entities;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;

namespace SAM.Web.Catalogos
{
    public partial class DetDiametro : SamPaginaCatalogo
    {
        /// Inicializar los controles con el contenido de BD
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Diametro);
                if (EntityID != null)
                {
                    carga();
                }
            }
        }

        private void carga()
        {
            Diametro diametro = DiametroBO.Instance.Obtener(EntityID.Value);
            Map(diametro);
            VersionRegistro = diametro.VersionRegistro;
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
                    GuardaEntidadPorId(WebConstants.CatalogoUrl.LST_DIAMETRO,
                                       DiametroBO.Instance.Obtener,
                                       DiametroBO.Instance.Guarda);
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }
    }
}