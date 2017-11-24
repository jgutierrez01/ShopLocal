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
    public partial class DetTubero : SamPaginaCatalogo
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Tuberos);
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
                GuardaEntidadPorId(WebConstants.CatalogoUrl.LST_TUBERO,
                                TuberoBO.Instance.Obtener,
                                TuberoBO.Instance.Guarda);
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }

        }

        /// <summary>
        /// metodo para cargar el combo "ddlPatio".
        /// </summary>
        private void CargaCombo()
        {
            ddlPatios.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerPatios());
        }

        /// <summary>
        /// metodo para cargar los datos y mostrarlos en el webform cuando se edita un registro.
        /// </summary>
        private void Carga()
        {
            Tubero defecto = TuberoBO.Instance.Obtener(EntityID.Value);
            Map(defecto);
            VersionRegistro = defecto.VersionRegistro;
        }
    }
}
