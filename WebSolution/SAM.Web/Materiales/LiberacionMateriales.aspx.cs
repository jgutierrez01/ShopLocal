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
using SAM.Entities;
using SAM.Entities.Grid;
using SAM.Web.Classes;
using SAM.Web.Common;
using Telerik.Web.UI;

namespace SAM.Web.Materiales
{
    public partial class LiberacionMateriales : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                rdpFechaLiberacion.SelectedDate = DateTime.Now.Date;
            }
        }

        protected void radCmbNumeroControl_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //DateTime? fechaLiberacion = WorkstatusSpoolBO.Instance.ObtnerFechaLiberacionMateriales(
            //    radCmbNumeroControl.SelectedValue.SafeIntParse());
            //if (fechaLiberacion.HasValue)
            //{
            //    rdpFechaLiberacion.SelectedDate = fechaLiberacion.Value;
            //}
            //else
            //{
            //    rdpFechaLiberacion.SelectedDate = DateTime.Now;
            //}
            rdpFechaLiberacion.SelectedDate = DateTime.Now;
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
                    WorkstatusSpoolBO.Instance.GuardarFechaLiberacionMaterial(rdpFechaLiberacion.SelectedDate.Value, ordenTrabajoSpoolID, SessionFacade.UserId);
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

        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                panelFechas.Visible = true;
                valSummary.Visible = true;
            }
        }

        protected void radCmbNumeroControl_DataBound(object sender, EventArgs e)
        {

        }
    }
}