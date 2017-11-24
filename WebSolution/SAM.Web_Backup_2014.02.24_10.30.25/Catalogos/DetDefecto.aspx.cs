using System;
using SAM.Web.Classes;
using SAM.Entities;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Utilerias;

namespace SAM.Web.Catalogos
{
    public partial class DetDefecto : SamPaginaCatalogo
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Defectos);
                cargaCombo();
                if (EntityID.HasValue)
                {
                    carga();
                }
            }
        }

        private void carga()
        {
            Defecto defecto = DefectoBO.Instance.Obtener(EntityID.Value);
            Map(defecto);
            VersionRegistro = defecto.VersionRegistro;
        }

        private void cargaCombo()
        {
            ddlTipoPrueba.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerTiposPrueba());
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
                    GuardaEntidadPorId(WebConstants.CatalogoUrl.LST_DEFECTO,
                                        DefectoBO.Instance.Obtener,
                                        DefectoBO.Instance.Guarda);
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }
    }
}
