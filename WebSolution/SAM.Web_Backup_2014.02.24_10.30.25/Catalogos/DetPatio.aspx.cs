using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.Web.Controles.Patios;
using SAM.BusinessLogic;
using Mimo.Framework.Data;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;
using SAM.Entities;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Proyectos;
namespace SAM.Web.Catalogos
{
    public partial class DetPatio : SamPaginaPrincipal
    {
        private Entities.Patio _patio;
 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Patios);
                if (EntityID != null)
                {
                    carga();
                }
            }
        }

        private void carga()
        {
            _patio = PatioBO.Instance.ObtenerConMaquinaTallerUbicacion(EntityID.Value);
            ctrlPatio.Map(_patio);
            ctrlTalleres.Map(_patio.Taller);
            ctrlEstacion.Map(_patio.Taller);
            ctrlMaquinas.Map(_patio.Maquina);
            ctrlLocalizaciones.Map(_patio.UbicacionFisica);
            VersionRegistro = _patio.VersionRegistro;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Entities.Patio patio;

            if (EntityID.HasValue)
            {
                patio = PatioBO.Instance.ObtenerConMaquinaTallerUbicacion(EntityID.Value);
                patio.VersionRegistro = VersionRegistro;
            }
            else
            {
                patio = new Entities.Patio();
            }

            patio.StartTracking();
            ctrlPatio.Unmap(patio);
            ctrlTalleres.Unmap(patio.Taller);
            ctrlEstacion.Unmap(patio.Taller);
            ctrlMaquinas.Unmap(patio.Maquina);
            ctrlLocalizaciones.Unmap(patio.UbicacionFisica);
            patio.UsuarioModifica = SessionFacade.UserId;
            patio.FechaModificacion = DateTime.Now;
            patio.StopTracking();

            try
            {
                PatioBO.Instance.Guarda(patio);
                Response.Redirect(WebConstants.CatalogoUrl.LST_PATIO);
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
        }
    }
}