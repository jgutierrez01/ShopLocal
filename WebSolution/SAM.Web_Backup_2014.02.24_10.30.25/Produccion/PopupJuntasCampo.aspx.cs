using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Telerik.Web.UI;
using Mimo.Framework.Extensions;

namespace SAM.Web.Produccion
{
    public partial class PopupJuntasCampo : SamPaginaPopup
    {
        private int juntaSpoolID
        {
            get
            {
                if (ViewState["juntaSpoolID"] == null)
                {
                    ViewState["juntaSpoolID"] = -1;
                }
                return ViewState["juntaSpoolID"].SafeIntParse();
            }
            set
            {
                ViewState["juntaSpoolID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                juntaSpoolID = Request.QueryString["JuntaSpoolID"].SafeIntParse();            
                rpArmado.ContentUrl = string.Format("~/Produccion/JuntasCampo/Armado.aspx?JuntaSpoolID={0}", juntaSpoolID);
                rpSoldadura.ContentUrl = string.Format("~/Produccion/JuntasCampo/Soldadura.aspx?JuntaSpoolID={0}", juntaSpoolID);
                rpInspeccionVisual.ContentUrl = string.Format("~/Produccion/JuntasCampo/InspeccionVisual.aspx?JuntaSpoolID={0}", juntaSpoolID);
                rpRequisiciones.ContentUrl = string.Format("~/Produccion/JuntasCampo/Requisiciones.aspx?JuntaSpoolID={0}", juntaSpoolID);
                rpPND.ContentUrl = string.Format("~/Produccion/JuntasCampo/PruebasPND.aspx?JuntaSpoolID={0}", juntaSpoolID);
                rpTT.ContentUrl = string.Format("~/Produccion/JuntasCampo/PruebasTT.aspx?JuntaSpoolID={0}", juntaSpoolID);
            }
        }


        /// <summary>
        /// Este evento es necesario para obligar a que los tabs se "recarguen" cada que se le da click a uno, de lo contrario
        /// el markup se queda client-side.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tab_TabClick(object sender, RadTabStripEventArgs e)
        {

        }
    }
}
