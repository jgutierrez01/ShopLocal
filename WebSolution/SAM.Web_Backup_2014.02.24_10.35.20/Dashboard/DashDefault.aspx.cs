using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.Entities.Cache;
using Mimo.Framework.Common;

namespace SAM.Web.Dashboard
{
    public partial class DashDefault : SamPaginaPrincipal
    {
        //private int PatioID
        //{
        //    get
        //    {
        //        return ViewState["PatioID"].SafeIntParse();
        //    }
        //    set
        //    {
        //        ViewState["PatioID"] = value;
        //    }
        //}

        //private int ProyectoID
        //{
        //    get
        //    {
        //        return ViewState["ProyectoID"].SafeIntParse();
        //    }
        //    set
        //    {
        //        ViewState["ProyectoID"] = value;
        //    }
        //}

        public string _currentCulture = LanguageHelper.CustomCulture;
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
               
              
            }
        }

        //metodo para cargar el combo "ddlPatio".
        //private void CargaCombo()
        //{
        //    ddlPatio.BindToEntiesWithEmptyRow(UserScope.MisPatios);
        //    if (EntityID.HasValue)
        //    {
        //        List<ProyectoCache> proyecto = UserScope.MisProyectos;
        //        int patioID = proyecto.Where(x => x.ID == EntityID.Value.SafeIntParse()).Select(x => x.PatioID).Single();
        //        ddlPatio.SelectedValue = patioID.ToString();

        //        ddlProyecto.BindToEntiesWithEmptyRow(proyecto.Where(x => x.PatioID == patioID));
        //        ddlProyecto.SelectedValue = EntityID.Value.ToString();

        //        PatioID = ddlPatio.SelectedValue.SafeIntParse();
        //        ProyectoID = ddlProyecto.SelectedValue.SafeIntParse();               
        //    }
        //}

        ////Metodo para cargar el combo "ddlProyectos" basado en la selección del patio
        //protected void DdlPatioOnSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int patioID = ddlPatio.SelectedValue.SafeIntParse();
        //    if (patioID < 1)
        //    {
        //        proyEncabezado.Visible = false;
        //    }

        //    ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos.Where(x => x.PatioID == patioID));
        //}

        ///// <summary>
        ///// Método que carga el encabezado del proyecto una vez seleccionada la opción.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void ddlProyectoSelectedItemChanged(object sender, EventArgs e)
        //{
        //    int proyectoID = ddlProyecto.SelectedValue.SafeIntParse();

        //    if (proyectoID > 0)
        //    {
        //        proyEncabezado.BindInfo(proyectoID);
        //        proyEncabezado.Visible = true;
        //    }
        //    else
        //    {
        //        proyEncabezado.Visible = false;
        //    }
        //}

        ////Método para cargar el grid de acuerdo a los datos seleccionados
        //protected void btnMostrar_Click(object sender, EventArgs e)
        //{
        //    PatioID = ddlPatio.SelectedValue.SafeIntParse();
        //    ProyectoID = ddlProyecto.SelectedValue.SafeIntParse();
            
        //}
    }
}