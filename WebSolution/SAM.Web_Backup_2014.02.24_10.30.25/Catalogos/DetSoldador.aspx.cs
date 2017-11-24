using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.WebControls;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Cache;
using SAM.Web.Classes;
using SAM.Entities;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic;
using Mimo.Framework.Exceptions;

namespace SAM.Web.Catalogos
{
    public partial class DetSoldador : SamPaginaCatalogo
    {
        private Entities.Soldador _soldador; 
    
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Soldadores);
                if (EntityID != null)
                {
                    carga();
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Entities.Soldador soldador;

            if (EntityID.HasValue)
            {
                soldador = SoldadorBO.Instance.ObtenerConWpqs(EntityID.Value);
                soldador.VersionRegistro = VersionRegistro;
            }
            else
            {
                soldador = new Entities.Soldador();
            }

            soldador.StartTracking();
            ctrlSoldador.Unmap(soldador);
            ctrlWpq.Unmap(soldador.Wpq);
            soldador.UsuarioModifica = SessionFacade.UserId;
            soldador.FechaModificacion = DateTime.Now;
            soldador.StopTracking();

            try
            {
                SoldadorBO.Instance.Guarda(soldador);
                Response.Redirect(WebConstants.CatalogoUrl.LST_SOLDADOR);
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }

        protected void carga()
        {
            _soldador = SoldadorBO.Instance.ObtenerConWpqs(EntityID.Value);
            ctrlSoldador.Map(_soldador);
            ctrlWpq.Map(_soldador.Wpq,_soldador.PatioID.ToString());
            VersionRegistro = _soldador.VersionRegistro;
        }

        protected void patioSeleccionado(object sender, EventArgs e)
        {
            MappableDropDown ddlPario = (MappableDropDown)sender;

            string PatioItem = ddlPario.SelectedValue;

            if (ctrlWpq.PatioID != PatioItem)
            {
                ctrlWpq.PatioID = PatioItem;
            }

            

        }
    }
}