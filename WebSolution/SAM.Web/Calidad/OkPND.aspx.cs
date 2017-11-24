using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using Mimo.Framework.WebControls;
using SAM.BusinessLogic.Calidad;
using SAM.BusinessObjects.Materiales;
using SAM.BusinessObjects.Produccion;
using SAM.BusinessObjects.Proyectos;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities;
using SAM.Entities.Grid;
using SAM.Web.Classes;
using SAM.Web.Common;
using Telerik.Web.UI;
using System.Threading;

namespace SAM.Web.WorkStatus
{
    public partial class OkPND : SamPaginaPrincipal
    {
        private string Permiso = Thread.CurrentThread.CurrentUICulture.Name == LanguageHelper.INGLES ? "Delete Ok PND" : "Borrar Ok PND";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rdpFechaLiberacion.SelectedDate = DateTime.Now.Date;
                ValidarPermisosBorrar();
            }
        }


        private void ValidarPermisosBorrar()
        {
            string cultura = Thread.CurrentThread.CurrentUICulture.Name;

            if (OkPNDBO.Instance.TienePermisoBorrarOkPND(SessionFacade.PerfilID.SafeIntParse(), cultura, Permiso, SessionFacade.EsAdministradorSistema))
                btnBorrar.Visible = true;
            else
                btnBorrar.Visible = false;
        }

        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                panelFechas.Visible = true;
                valSummary.Visible = true;
            }
        }

        protected void radCmbNumeroControl_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rdpFechaLiberacion.SelectedDate = DateTime.Now;
        }

        protected void radCmbNumeroControl_DataBound(object sender, EventArgs e)
        {

        }

        protected void rfvNumControl_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = radCmbNumeroControl.SelectedValue.SafeIntParse() > 0;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid)
                {
                    if (rdpFechaLiberacion.SelectedDate.Value == null)
                    {
                        rdpFechaLiberacion.SelectedDate = DateTime.Now.Date;
                    }
                    int ordenTrabajoSpoolID = radCmbNumeroControl.SelectedValue.SafeIntParse();
                    OkPNDBO.Instance.GuardarOkPnd(rdpFechaLiberacion.SelectedDate.Value, ordenTrabajoSpoolID, SessionFacade.UserId);
                    radCmbNumeroControl.ClearSelection();
                    radCmbNumeroControl.Text = string.Empty;
                    rdpFechaLiberacion.SelectedDate = DateTime.Now.Date;
                }
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex, "valLiberacion");
            }
        }

        protected void btnBorrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid)
                {
                    if (rdpFechaLiberacion.SelectedDate.Value == null)
                    {
                        rdpFechaLiberacion.SelectedDate = DateTime.Now.Date;
                    }
                    int ordenTrabajoSpoolID = radCmbNumeroControl.SelectedValue.SafeIntParse();
                    OkPNDBO.Instance.BorrarOkPnd(rdpFechaLiberacion.SelectedDate.Value, ordenTrabajoSpoolID, SessionFacade.UserId);
                    radCmbNumeroControl.ClearSelection();
                    radCmbNumeroControl.Text = string.Empty;
                    rdpFechaLiberacion.SelectedDate = DateTime.Now.Date;
                }
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex, "valLiberacion");
            }
        }
        
    }
}