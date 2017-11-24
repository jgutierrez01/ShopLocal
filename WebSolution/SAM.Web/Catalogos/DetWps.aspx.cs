using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Exceptions;
using SAM.BusinessLogic;
using Mimo.Framework.Extensions;
using SAM.Entities;
using SAM.BusinessObjects.Utilerias;

namespace SAM.Web.Catalogos
{
    public partial class DetWps : SamPaginaCatalogo
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Wps);
                CargaCombos();

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
                GuardaEntidadPorId(WebConstants.CatalogoUrl.LST_WPS,
                                WpsBO.Instance.Obtener,
                                WpsBO.Instance.Guarda);
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }

        }

        //metodo para cargar los combos.
        private void CargaCombos()
        {
            ddlMaterial1.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerFamiliasAcero());
            ddlMaterial2.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerFamiliasAcero());
            ddlProcesoRaiz.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerProcesosRaiz());
            ddlProcesoRelleno.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerProcesosRelleno());
        }

        //metodo para cargar los datos y mostrarlos en el webform cuando se edita un registro Wps.
        private void Carga()
        {
            Wps wps = WpsBO.Instance.Obtener(EntityID.Value);
            Map(wps);
            VersionRegistro = wps.VersionRegistro;
            lblRaizMax.Text = lblRaizMax.Text.Substring(0,lblRaizMax.Text.Length-1);
            lblRellenoMax.Text = lblRellenoMax.Text.Substring(0, lblRellenoMax.Text.Length - 1);
        }

    }
}