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
    public partial class DetFamAcero : SamPaginaCatalogo
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_FamAceros);
                cargaCombo();

                if (EntityID != null)
                {
                    carga();
                }

            }

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try 
            {
                GuardaEntidadPorId(WebConstants.CatalogoUrl.LST_FAMILIA_ACERO,
                                FamiliaAceroBO.Instance.Obtener,
                                FamiliaAceroBO.Instance.Guarda);
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }

        }

       
        
        /// <summary>
        ///metodo para cargar el combo "ddlFamiliaMaterial".
        /// </summary>
        private void cargaCombo()
        {
            ddlFamiliaMaterial.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerFamiliasMaterial());
        }

        /// <summary>
        /// metodo para cargar los datos y mostrarlos en el webform cuando se edita un registro.
        /// </summary>
        private void carga()
        {
            FamiliaAcero familiaAcero = FamiliaAceroBO.Instance.Obtener(EntityID.Value);
            Map(familiaAcero);
            VersionRegistro = familiaAcero.VersionRegistro;
        }
    }
}