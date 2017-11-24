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

namespace SAM.Web.Catalogos
{
    public partial class DetProcesoRaiz : SamPaginaCatalogo
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_ProcesoRaiz);
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
                GuardaEntidadPorId(WebConstants.CatalogoUrl.LST_PROCESO_RAIZ,
                                   ProcesoRaizBO.Instance.Obtener,
                                   ProcesoRaizBO.Instance.Guarda);
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
        }

        private void Carga()
        {
            ProcesoRaiz procesoRaiz = ProcesoRaizBO.Instance.Obtener(EntityID.Value);
            Map(procesoRaiz);
            VersionRegistro = procesoRaiz.VersionRegistro;
        }
    }
}