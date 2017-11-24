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
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Utilerias;
using SAM.Web.Common;

namespace SAM.Web.Catalogos
{
    public partial class DetConsumibles : SamPaginaPrincipal
    {
        private int PatioID
        {
            get
            {
                return (int)ViewState["PatioID"];
            }
            set
            {
                ViewState["PatioID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Consumibles);
                if (EntityID != null)
                {
                    Consumible cons = ConsumiblesBO.Instance.Obtener(EntityID.Value);
                    PatioID = cons.PatioID;
                    VersionRegistro = cons.VersionRegistro;
                    lblPatio.Text = cons.Patio.Nombre;
                    Map(cons);                    
                }
                else
                {
                    PatioID = Request.QueryString["PID"].SafeIntParse();
                    lblPatio.Text = CacheCatalogos.Instance.ObtenerPatios().Where(x => x.ID == PatioID).SingleOrDefault().Nombre;
                }

                titulo.NavigateUrl = String.Format(WebConstants.CatalogoUrl.LST_CONSUMIBLES, PatioID);
                
            }
        }

        public void btnGuardar_OnClick(object sender, EventArgs e)
        {
            if (IsValid)
            {
                Consumible cons = new Consumible();
                if (EntityID != null)
                {
                    cons = ConsumiblesBO.Instance.Obtener(EntityID.Value);
                }

                cons.StartTracking();
                Unmap(cons);
                cons.PatioID = PatioID;
                cons.UsuarioModifica = SessionFacade.UserId;
                cons.FechaModificacion = DateTime.Now;
                cons.StopTracking();

                try
                {
                    ConsumiblesBO.Instance.Guarda(cons);
                    Response.Redirect(String.Format(WebConstants.CatalogoUrl.LST_CONSUMIBLES,PatioID));
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }
        }
    }
}