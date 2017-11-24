using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessObjects.Catalogos;
using SAM.Entities;
using Mimo.Framework.Exceptions;

namespace SAM.Web.Catalogos
{
    public partial class DetFamiliaMaterial : SamPaginaCatalogo
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_FamMateriales);
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
                GuardaEntidadPorId(WebConstants.CatalogoUrl.LST_FAMILIA_MATERIAL,
                                    FamiliaMaterialBO.Instance.Obtener,
                                    FamiliaMaterialBO.Instance.Guarda);
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
        }

        private void Carga()
        {
            FamiliaMaterial familia = FamiliaMaterialBO.Instance.Obtener(EntityID.Value);
            Map(familia);
            VersionRegistro = familia.VersionRegistro;
        }
    }
}