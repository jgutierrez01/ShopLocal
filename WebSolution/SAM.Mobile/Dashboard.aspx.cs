using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mobile;
using System.Web.UI;
using System.Web.UI.MobileControls;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;
using SAM.Mobile.Clases;


namespace SAM.Mobile
{
    public partial class Dashboard : PaginaMovilAutenticado
    {
        protected override void OnLoad(EventArgs e)
        {
            RevisarSeguridad = false;
            base.OnLoad(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SessionFacade.PatioID > 0)
            {
                hypArmado.Visible = true;
                hypSoldadura.Visible = true;
                hypInspeccionVisual.Visible = true;
                hypInspeccionDimensional.Visible = true;
                hypSeguimientoSpool.Visible = true;
            }
            else
            {
                hypArmado.Visible = false;
                hypSoldadura.Visible = false;
                hypInspeccionVisual.Visible = false;
                hypInspeccionDimensional.Visible = false;
                hypSeguimientoSpool.Visible = false;
            }
        }

        protected void hypInspeccionDimensional_OnClick(object sender, EventArgs e)
        {
            Response.Redirect(WebConstants.MobileUrl.INSPECCIONDIMENSIONAL);
        }

        protected void hypCambiarPatio_OnClick(object sender, EventArgs e)
        {
            Response.Redirect(WebConstants.MobileUrl.CAMBIOPATIO);
        }

        protected void hypSoldadura_OnClick(object sender, EventArgs e)
        {
            Response.Redirect(WebConstants.MobileUrl.SOLDADURA);
        }

        protected void hypArmado_OnClick(object sender, EventArgs e)
        {
            Response.Redirect(WebConstants.MobileUrl.ARMADO);
        }

        protected void hypInspeccionVisual_OnClick(object sender, EventArgs e)
        {
            Response.Redirect(WebConstants.MobileUrl.INSPECCIONVISUAL);
        }

        protected void hypSeguimientoSpool_OnClick(object sender, EventArgs e)
        {
            Response.Redirect(WebConstants.MobileUrl.SEGUIMIENTOSPOOL);
        }
    }
}