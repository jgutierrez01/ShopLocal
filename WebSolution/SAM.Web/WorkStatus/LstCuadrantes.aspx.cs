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
using Mimo.Framework.Exceptions;
using SAM.Web.Classes;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Administracion;
using SAM.Web.Common;

namespace SAM.Web.WorkStatus
{
    public partial class LstCuadrantes : SamPaginaPrincipal
    {
        public DateTime? FechaCuadrante { get; set; }

        private int ProyectoID
        {
            get
            {
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
            FechaCuadrante = DateTime.Now;
            if (!mdpFechaCuadrante.SelectedDate.HasValue)
            {
                mdpFechaCuadrante.SelectedDate = FechaCuadrante.Value;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            ProyectoID = filtroGenerico.ProyectoSelectedValue.SafeIntParse();
            int _numeroControl = filtroGenerico.NumeroControlSelectedValue.SafeIntParse();
            int _idCuadrante = filtroGenerico.CuadranteSelectedValue.SafeIntParse();
            
            try
            {
                CuadranteBO.Instance.GuardarCuadranteSpool(_idCuadrante, _numeroControl, mdpFechaCuadrante.SelectedDate,SessionFacade.UserId);
                filtroGenerico.LimpiarCombos();
                mdpFechaCuadrante.SelectedDate = FechaCuadrante.Value;
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }            
        }
    }    
}