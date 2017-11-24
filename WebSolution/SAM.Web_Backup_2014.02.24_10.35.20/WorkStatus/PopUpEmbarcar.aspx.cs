using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.Entities;
using SAM.BusinessObjects.Workstatus;
using Mimo.Framework.Exceptions;
using System.Globalization;
using Mimo.Framework.Common;

namespace SAM.Web.WorkStatus
{
    public partial class PopUpEmbarcar : SamPaginaPopup
    {

        public int[] WorkstatusSpools
        {
            get
            {
                if (ViewState["WorkstatusSpools"] != null)
                {
                    return (int[])ViewState["WorkstatusSpools"];
                }
                return null;
            }
            set
            {
                ViewState["WorkstatusSpools"] = value;
            }
        }

        private string Fechas
        {
            get
            {
                if (ViewState["Fechas"] == null)
                {
                    ViewState["Fechas"] = string.Empty;
                }

                return ViewState["Fechas"].ToString();
            }
            set
            {
                ViewState["Fechas"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WorkstatusSpools = Request.QueryString["IDs"].Split(',').Select(n => n.SafeIntParse()).ToArray();

                Fechas = ValidaFechasBO.Instance.ObtenerFechasDimensionales(WorkstatusSpools);

                string numerosControl = ValidaFechasBO.Instance.ObtenerNumerosControlPorWorkstatusSpoolID(WorkstatusSpools);
                
                string proceso;
                if (LanguageHelper.CustomCulture == LanguageHelper.INGLES)
                {
                    proceso = "Dimensional Inspection";
                }
                else
                {
                    proceso = "Liberación Dimensional";
                }

                btnEmbarcar.OnClientClick = "return Sam.Workstatus.ValidacionFechasPruebas('" + numerosControl + "','" + Fechas + "', '3','" + proceso + "')";
            }
        }

        /// <summary>
        /// Guarda el número de embarque y fecha de embarque en los spools seleccionados.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEmbarcar_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid)
                {
                    Embarque embarque = new Embarque
                    {
                        ProyectoID = EntityID.Value,
                        FechaEmbarque = mdpFechaEmbarque.SelectedDate.Value,
                        NumeroEmbarque = txtNumeroEmbarque.Text,
                        Observaciones = txtObservaciones.Text,
                        Nota1 = txtNota1.Text,
                        Nota2 = txtNota2.Text,
                        Nota3 = txtNota3.Text,
                        Nota4 = txtNota4.Text,
                        Nota5 = txtNota5.Text
                    };

                    string[] arregloFechas = Fechas.Split(',');
                    for (int i = 0; i < arregloFechas.Length; i++)
                    {
                        DateTime fecha = DateTime.ParseExact(arregloFechas[i], "MM/dd/yyyy", CultureInfo.InvariantCulture);

                        if (mdpFechaEmbarque.SelectedDate.Value < fecha)
                        {
                            WorkstatusSpools[i] = -1;
                        }
                    }

                    EmbarqueBO.Instance.GuardaEmbarque(embarque, WorkstatusSpools, SessionFacade.UserId);
                    JsUtils.RegistraScriptActualizaGridGenerico(this);

                    lnkReporte.ProyectoID = EntityID.Value;
                    lnkReporte.NombresParametrosReporte = "NumeroEmbarque";
                    lnkReporte.ValoresParametrosReporte = txtNumeroEmbarque.Text;
                    lnkReporte.Tipo = TipoReporteProyectoEnum.Embarque;

                    phControles.Visible = false;
                    phMensaje.Visible = true;
                }
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
            
        }
    }
}