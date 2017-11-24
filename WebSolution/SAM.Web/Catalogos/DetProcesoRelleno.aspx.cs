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
    public partial class DetProcesoRelleno : SamPaginaCatalogo
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_ProcesoRelleno);
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
                GuardaEntidadPorId(WebConstants.CatalogoUrl.LST_PROCESO_RELLENO,
                               ProcesoRellenoBO.Instance.Obtener,
                               ProcesoRellenoBO.Instance.Guarda);
            }
            catch (BaseValidationException ex) 
            {
                RenderErrors(ex);
            }
        }

        private void Carga()
        {
            ProcesoRelleno procesoRelleno = ProcesoRellenoBO.Instance.Obtener(EntityID.Value);
            Map(procesoRelleno);
            VersionRegistro = procesoRelleno.VersionRegistro;
        }
    }
}