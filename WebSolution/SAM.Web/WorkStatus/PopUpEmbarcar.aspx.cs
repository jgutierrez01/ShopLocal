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
using SAM.Web.Common;
using SAM.BusinessObjects.Produccion;

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

        private string FechasDimension
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

        private string FechasPreparacion
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

        private string Proceso 
        {
            get
            {
                if (ViewState["UltimoProceso"] == null)
                {
                    ViewState["UltimoProceso"] = string.Empty;
                }

                return ViewState["UltimoProceso"].ToString();
            }
            set
            {
                ViewState["UltimoProceso"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WorkstatusSpools = Request.QueryString["IDs"].Split(',').Select(n => n.SafeIntParse()).ToArray();
                string numerosControl = ValidaFechasBO.Instance.ObtenerNumerosControlPorWorkstatusSpoolID(WorkstatusSpools);
                FechasDimension = ValidaFechasBO.Instance.ObtenerFechasDimensionales(WorkstatusSpools);
                FechasPreparacion = ValidaFechasBO.Instance.ObtenerFechasPreparacion(WorkstatusSpools);
                
                if (LanguageHelper.CustomCulture == LanguageHelper.INGLES)
                {
                    Proceso = "Dimensional Inspection";
                }
                else
                {
                    Proceso = "Liberación Dimensional";
                }
                mdpFechaEmbarqueCarga.SelectedDate = DateTime.Now;
                btnEmbarcar.OnClientClick = "return Sam.Workstatus.ValidacionFechasEmbarque('" + numerosControl + "', '" + FechasDimension + "', '" + FechasPreparacion + "', '" + Proceso + "')";
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
                ErrorProcesoAnterior();
                if (IsValid)
                {
                    Embarque embarque = new Embarque
                    {
                        ProyectoID = EntityID.Value,
                        FechaEstimada = mdpFechaEmbarqueCarga.SelectedDate.Value,
                        NumeroEmbarque = txtNumeroEmbarque.Text,
                        Observaciones = txtObservaciones.Text,                        
                        Nota1 = txtNota1.Text,
                        Nota2 = txtNota2.Text,
                        Nota3 = txtNota3.Text,
                        Nota4 = txtNota4.Text,
                        Nota5 = txtNota5.Text  
                    };
                   

                    string[] arregloFechasDimension = FechasDimension.Split(',');
                    for (int i = 0; i < arregloFechasDimension.Length; i++)
                    {
                        DateTime fecha;

                        if (!DateTime.TryParseExact(arregloFechasDimension[i], "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha))
                        {
                            ErrorProcesoAnterior();
                        }
                        
                        if (mdpFechaEmbarqueCarga.SelectedDate.Value < fecha)
                        {
                            WorkstatusSpools[i] = WorkstatusSpools[i] * -1;                                                 
                        }
                    }

                    string[] arregloFechasPreparacion = FechasPreparacion.Split(',');
                    for (int i = 0; i < arregloFechasPreparacion.Length; i++)
                    {
                        DateTime fecha;

                        if (!DateTime.TryParseExact(arregloFechasPreparacion[i], "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha))
                        {
                            ErrorProcesoAnterior();
                        }
                    
                        if ( mdpFechaEmbarqueCarga.SelectedDate.Value < fecha && WorkstatusSpools[i] > 0)
                        {
                            WorkstatusSpools[i] = WorkstatusSpools[i] * -1;
                        }  
                    }


                    if (mdpVigenciaAduana.SelectedDate != null)
                    {
                        embarque.VigenciaAduana = mdpVigenciaAduana.SelectedDate.Value;
                    }

                    bool mostrarMensaje =  EmbarqueBO.Instance.GuardaEmbarque(embarque, WorkstatusSpools, SessionFacade.UserId);
                   
                   
                    lnkReporte.ProyectoID = EntityID.Value;
                    lnkReporte.NombresParametrosReporte = "NumeroEmbarque";
                    lnkReporte.ValoresParametrosReporte = txtNumeroEmbarque.Text;
                    lnkReporte.Tipo = TipoReporteProyectoEnum.Embarque;

                    phControles.Visible = false;
                    
                    if (mostrarMensaje)
                    {
                        lblMensajeSinActualizacionLocalizacion.Visible = true;
                    }

                    phMensaje.Visible = true;

                }
            }
            catch (BaseValidationException ex)
            {
                for(int i = 0; i < WorkstatusSpools.Length; i++)
                {
                    if (WorkstatusSpools[i] < 0)
                    {
                        WorkstatusSpools[i] = WorkstatusSpools[i] * -1;
                    }
                }
                RenderErrors(ex);
            }
            finally
            {
                JsUtils.RegistraScriptActualizaGridGenerico(this);
            }

        }

        private void ErrorProcesoAnterior()
        {
            try
            {
                if (String.IsNullOrEmpty(FechasDimension))
                {
                    if (CultureInfo.CurrentCulture.Name == "en-US")
                    {
                        throw new BaseValidationException(string.Format
                            ("There is an error with the date of process: {0}, you must correct before doing this process.", Proceso));
                    }
                    else
                    {
                        throw new BaseValidationException(string.Format
                            ("Existe un error con la  proceso anterior: {0}, debe corregir antes de poder realizar este proceso.", Proceso));
                    }
                }
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
        }
    }
}