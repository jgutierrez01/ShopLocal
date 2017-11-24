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
using SAM.Web.Common;

namespace SAM.Web.Catalogos
{
    public partial class DetCortador : SamPaginaCatalogo
    {        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Cortador);
                CargaCombo();

                if (EntityID != null)
                {
                    Carga();
                }
                CargarTaller();
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                GuardaEntidadPorId(WebConstants.CatalogoUrl.LST_CORTADOR,
                                CortadorBO.Instance.Obtener,
                                CortadorBO.Instance.Guarda);
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

        private void CargarTaller()
        {
            int patio = ddlPatios.SelectedValue.SafeIntParse();
            ddlTaller.BindToEntiesWithEmptyRow(UserScope.TalleresPorPatio(patio));
        }

        /// <summary>
        /// metodo para cargar los datos y mostrarlos en el webform cuando se edita un registro.
        /// </summary>
        private void Carga()
        {
            Cortador defecto = CortadorBO.Instance.Obtener(EntityID.Value);
            Map(defecto);
            VersionRegistro = defecto.VersionRegistro;
        }

        protected void ddlPatios_SelectedIndexChanged(object sender, EventArgs e)
        {
            int patio = ddlPatios.SelectedValue.SafeIntParse();
            ddlTaller.BindToEntiesWithEmptyRow(UserScope.TalleresPorPatio(patio));
        }


    }
}