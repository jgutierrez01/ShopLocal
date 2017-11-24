using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities.Personalizadas;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Excepciones;
using Mimo.Framework.Exceptions;
using SAM.Web.Classes;

namespace SAM.Web.WorkStatus
{
    public partial class RepEmbarque : SamPaginaPrincipal
    {
        public DateTime? FechaEmbarque { get; set; }
        public DateTime? FechaEstimada { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            DateTime? fechaEstimada = rdpFechaEstimada.SelectedDate;
            DateTime? fechaEmbarque = rdpFechaEmbarque.SelectedDate;
            Guid usuarioModifica = SAM.Web.Common.SessionFacade.UserId;

            try
            {
                EmbarqueBO.Instance.GuardarFechasEmbarque(Convert.ToInt32(filtroGenerico.EmbarqueSelectedValue)
                , fechaEstimada, fechaEmbarque, usuarioModifica);
                filtroGenerico.LimpiarCombosEmbarques();
                filtroGenerico.EmbarqueEnabled = true;
                filtroGenerico.ProyectoEnabled = true;
                panelFechas.Visible = false;

            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
           
           
        }

        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            FechasEmbarque fechas = EmbarqueBO.Instance.ObtenerFechaEmbarque(filtroGenerico.EmbarqueSelectedValue.SafeIntParse());
            FechaEmbarque = fechas.FechaEmbarque;
            FechaEstimada = fechas.FechaEstimada;
       
            if (FechaEmbarque.HasValue)
            {
                rdpFechaEmbarque.SelectedDate = FechaEmbarque.Value;
            }
            if (FechaEstimada.HasValue)
            {
                rdpFechaEstimada.SelectedDate = FechaEstimada.Value;            
            }
            else
            {
                rdpFechaEstimada.SelectedDate = null;
            }

            filtroGenerico.ProyectoEnabled = false;
            filtroGenerico.EmbarqueEnabled = false;
            panelFechas.Visible = true;
        }
    }
}