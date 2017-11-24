using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using Mimo.Framework.Extensions;
using Telerik.Web.UI;
using SAM.BusinessObjects.Catalogos;



namespace SAM.Web.WorkStatus
{
    public partial class LstCuadrantes : System.Web.UI.Page
    {
        private int ProyectoID
        {
            get {
                if (ViewState["ProyectoID"] == null)
                {
                    ViewState["ProyectoID"] = 0;
                }
                return ViewState["ProyectoID"].SafeIntParse(); 
            }
            set { ViewState["ProyectoID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            ProyectoID = filtroGenerico.ProyectoSelectedValue.SafeIntParse();
            int _numeroControl = filtroGenerico.NumeroControlSelectedValue.SafeIntParse();
            int _idCuadrante = filtroGenerico.CuadranteSelectedValue.SafeIntParse();
            CuadranteBO.Instance.GuardarCuadranteSpool(_idCuadrante, _numeroControl);
            filtroGenerico.LimpiarCombos();
        }
    }
}