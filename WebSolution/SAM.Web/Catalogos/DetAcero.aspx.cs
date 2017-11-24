using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.Entities;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Utilerias;

namespace SAM.Web.Catalogos
{
    public partial class DetAcero : SamPaginaCatalogo
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Aceros);
                CargaCombo();

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
                GuardaEntidadPorId(WebConstants.CatalogoUrl.LST_ACERO,
                                               AceroBO.Instance.Obtener,
                                               AceroBO.Instance.Guarda);
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
            
        }

        //metodo para cargar el combo "ddlFamiliaMaterial".
        private void CargaCombo()
        {

            ddlFamiliaAcero.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerFamiliasAcero());
        }

        private void Carga()
        {
            Acero acero = AceroBO.Instance.Obtener(EntityID.Value);
            Map(acero);
            VersionRegistro = acero.VersionRegistro;
        }
    }
}