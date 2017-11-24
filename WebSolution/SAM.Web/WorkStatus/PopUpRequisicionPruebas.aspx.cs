using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Utilerias;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities;
using System.Globalization;
using Mimo.Framework.Common;
using SAM.Web.Common;

namespace SAM.Web.WorkStatus
{
    public partial class PopUpRequisicionPruebas : SamPaginaPopup
    {
        private string IDs
        {
            get
            {
                if (ViewState["IDs"] == null)
                {
                    ViewState["IDs"] = string.Empty;
                }

                return ViewState["IDs"].ToString();
            }
            set
            {
                ViewState["IDs"] = value;
            }
        }
        private int TipoPruebaID
        {
            get
            {
                return ViewState["TipoPruebaID"].SafeIntParse();
            }
            set
            {
                ViewState["TipoPruebaID"] = value;
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
        private string FechasReporte
        {
            get
            {
                if (ViewState["FechasReporte"] == null)
                {
                    ViewState["FechasReporte"] = string.Empty;
                }

                return ViewState["FechasReporte"].ToString();
            }
            set
            {
                ViewState["FechasReporte"] = value;
            }
        }

        private bool TipoPreHeatTieneIV
        {
            get
            {
                if (ViewState["TipoPreHeatTieneIV"] == null)
                {
                    ViewState["TipoPreHeatTieneIV"] = false;
                }

                return (bool)ViewState["TipoPreHeatTieneIV"];
            }
            set
            {
                ViewState["TipoPreHeatTieneIV"] = value;
            }
        }

        private string Juntas
        {
            get
            {
                if (ViewState["Juntas"] == null)
                {
                    ViewState["Juntas"] = string.Empty;
                }

                return ViewState["Juntas"].ToString();
            }

            set { ViewState["Juntas"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAProyecto(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar un proyecto {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                IDs = Request.QueryString["IDs"];
                TipoPruebaID = Request.QueryString["TP"].SafeIntParse();

                int[] juntaWorkstatusIds = IDs.Split(',').Select(n => n.SafeIntParse()).ToArray();

                if (TipoPruebaID == (int)TipoPruebaEnum.Preheat)
                {
                    //btnRequisitar.OnClientClick = "return true;";
                    //btnAutoRequisitar.OnClientClick = "return true;";
                    cargaInfo();
                }
                else
                {
                    validaFechas(juntaWorkstatusIds);
                }
            }
        }

        private void validaFechas(int[] juntaWorkstatusIds)
        {
            Juntas = ValidaFechasBO.Instance.ObtenerNumControlJuntasPorJuntaWorkstatusIds(juntaWorkstatusIds);
            Fechas = ValidaFechasBO.Instance.ObtenerFechasProcesoSoldadura(juntaWorkstatusIds);

            //btnRequisitar.OnClientClick = "return Sam.Workstatus.ValidacionFechasRequisicion('" + juntas + "' ,'" + Fechas + "')";
            //btnAutoRequisitar.OnClientClick = "return Sam.Workstatus.ValidacionFechasRequisicion('" + juntas + "' ,'" + Fechas + "')";

            cargaInfo();
        }

        protected void validaFechas()
        {
            string errores = "";
            string[] fechas = Fechas.Split(',');
            string[] juntas = Juntas.Split(',');
            //DateTime fechaRequisicion = new DateTime(rdpFechaRequisicion.SelectedDate.Value.Year, rdpFechaRequisicion.SelectedDate.Value.Month, rdpFechaRequisicion.SelectedDate.Value.Day);
            DateTime fechaRequisicion = new DateTime();

            if (CultureInfo.CurrentCulture.Name == "en-US")
            {
                DateTime.TryParse(rdpFechaRequisicion.SelectedDate.Value.ToString("MM/dd/yyyy"), out fechaRequisicion);
            }else
            {
                DateTime.TryParse(rdpFechaRequisicion.SelectedDate.Value.ToString("dd/MM/yyyy"), out fechaRequisicion);
            }

            for (int i = 0; i < fechas.Length; i++)
            {
                if (fechas[i] != string.Empty)
                {
                    DateTime tempFecha = new DateTime();
                    if (CultureInfo.CurrentCulture.Name != "en-US")
                    {
                        string[] splitDate = fechas[i].Split('/');
                        string newDate = splitDate[1] + "/" + splitDate[0] + "/" + splitDate[2];

                        DateTime.TryParse(newDate, out tempFecha);
                    }
                    else
                    {
                        DateTime.TryParse(fechas[i], out tempFecha);
                    }

                    if (fechaRequisicion < tempFecha )//DateTime.ParseExact(fechas[i], "MM/dd/yyyy", CultureInfo.InvariantCulture))
                    {
                        errores += "<br />" + juntas[i];
                    }
                }
            }
            if (errores.Length > 0)
            {
                if (CultureInfo.CurrentCulture.Name == "en-US")
                {
                    throw new BaseValidationException(String.Format("The process date is lower than previous process date of joints {0}", errores));
                }
                else
                {
                    throw new BaseValidationException(String.Format("La fecha del proceso es menor a la fecha del proceso anterior de las juntas {0}", errores));
                }
            }
        }

        /// <summary>
        /// Carga la informacion de tipos de prueba
        /// </summary>
        private void cargaInfo()
        {
            txtTipoPrueba.Text = CacheCatalogos.Instance.ObtenerTiposPrueba().Where(x => x.ID == TipoPruebaID).Select(x => x.Nombre).SingleOrDefault();
        }

        #region Eventos

        protected void btnRequisitar_Click(object sender, EventArgs e)
        {
            try
            {
                if (TipoPruebaID == (int)TipoPruebaEnum.Preheat)
                {
                    //validaFechaArmado();
                }

                if (IsValid)
                {
                    validaFechas();
                    List<string> juntas = IDs.Split(',').ToList();
                    juntas.RemoveAll(new System.Predicate<string>(delegate(string val) { return (val == ""); }));

                    IDs = string.Join(",", juntas);

                    Requisicion requisicion = new Requisicion
                    {
                        ProyectoID = EntityID.Value,
                        TipoPruebaID = TipoPruebaID,
                        FechaRequisicion = rdpFechaRequisicion.SelectedDate.Value,
                        NumeroRequisicion = txtNumeroRequisicion.Text,
                        CodigoAsme = txtCodigo.Text,
                        Observaciones = txtObservaciones.Text
                    };

                    RequisicionPruebasBO.Instance.GeneraRequisicion(requisicion, IDs, SessionFacade.UserId);

                    //Actualiza el grid de la ventana padre para que refleje que el reporte ya se generó
                    JsUtils.RegistraScriptActualizaGridGenerico(this);

                    lnkReporte.ProyectoID = EntityID.Value;
                    lnkReporte.NombresParametrosReporte = "NumeroRequisicion,TipoPruebaID";
                    lnkReporte.ValoresParametrosReporte = string.Format("{0},{1}", txtNumeroRequisicion.Text, TipoPruebaID);

                    lnkReporte.Tipo = TipoReporteProyectoEnum.Requisicion;

                    phControles.Visible = false;
                    phMensajeExito.Visible = true;
                }
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
        }

        private void validaFechaArmado()
        {
            int[] juntaWorkstatusIds = IDs.Split(',').Select(n => n.SafeIntParse()).ToArray();
            string fechasArmado = ValidaFechasBO.Instance.ObtenerFechasReporteArmado(juntaWorkstatusIds);

            List<string> errores = (from fa in fechasArmado.Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries)
                                    let fecha = fa.Split("||".ToArray(), StringSplitOptions.RemoveEmptyEntries)[0]
                                    let etiquetaJunta = fa.Split("||".ToArray(), StringSplitOptions.RemoveEmptyEntries)[1]
                                    where rdpFechaRequisicion.SelectedDate.Value.Date < Convert.ToDateTime(fecha).Date
                                    select string.Format(GetLocalResourceObject("FechaReporteMenor_XArmado").ToString(),
                                                         etiquetaJunta)).ToList();
            if (errores.Count > 0)
            {
                RenderErrors(errores);
            }
        }

        #endregion
    }
}