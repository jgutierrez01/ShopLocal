using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Mimo.Framework.Extensions;
using SAM.Web.Classes;
using SAM.BusinessObjects.Utilerias;
using Mimo.Framework.Exceptions;
using SAM.Entities;
using SAM.BusinessObjects.Workstatus;
using System.Globalization;

namespace SAM.Web.WorkStatus
{
    public partial class PopUpRequisicionSpoolPruebas : SamPaginaPopup
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
        private int TipoPruebaSpoolID
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

        private string NumerosControl
        {
            get { return ViewState["numerosControl"].ToString(); }
            set { ViewState["numerosControl"] = value; }
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
                TipoPruebaSpoolID = Request.QueryString["TP"].SafeIntParse();

                int[] WorkstatusSpoolIds = IDs.Split(',').Select(n => n.SafeIntParse()).ToArray();

                validaFechas(WorkstatusSpoolIds);

                cargaInfo();
            }
        }

        private void validaFechas(int[] workstatusSpoolIds)
        {
            NumerosControl = ValidaFechasBO.Instance.ObtenerNumerosControlPorWorkstatusSpoolID(workstatusSpoolIds);
            Fechas = ValidaFechasBO.Instance.ObtenerFechasDimensionales(workstatusSpoolIds);

            // btnRequisitar.OnClientClick = " return Sam.Workstatus.ValidacionFechasRequisicionSpool('" + numerosControl + "' ,'" + Fechas + "');";
        }

        private void validaFechas()
        {
            
            DateTime fechaReq = new DateTime(rdpFechaRequisicion.SelectedDate.Value.Year, rdpFechaRequisicion.SelectedDate.Value.Month,rdpFechaRequisicion.SelectedDate.Value.Day);
            string[] fechas = Fechas.Split(',');
            string[] numerosControl = NumerosControl.Split(',');
            string errores = "";
            for (int i = 0; i < fechas.Length; i++)
            {
                DateTime fechaPs = DateTime.ParseExact(fechas[i], "MM/dd/yyyy", null);
                if (fechaReq < fechaPs)
                {
                    errores += "<br />" + numerosControl[i];
                }
            }
            if (errores.Length > 0)
            {
                if (CultureInfo.CurrentCulture.Name == "en-US")
                {
                    throw new BaseValidationException(String.Format("The process date is lower than previous process date of control numbers {0}", errores));
                }
                else
                {
                    throw new BaseValidationException(String.Format("La fecha del proceso es menor a la fecha del proceso anterior de los números de control {0}", errores));
                }
            }
        }

        /// <summary>
        /// Carga la informacion de tipos de prueba
        /// </summary>
        private void cargaInfo()
        {
            txtTipoPrueba.Text = CacheCatalogos.Instance
                                               .ObtenerTiposPruebaSpool()
                                               .Where(x => x.ID == TipoPruebaSpoolID).Select(x => x.Nombre)
                                               .SingleOrDefault();
        }

        #region Eventos

        protected void btnRequisitar_Click(object sender, EventArgs e)
        {
            try
            {

                if (IsValid)
                {
                    validaFechas();
                    List<string> wSpools = IDs.Split(',').ToList();
                    wSpools.RemoveAll(new System.Predicate<string>(delegate(string val) { return (val == ""); }));

                    IDs = string.Join(",", wSpools);

                    RequisicionSpool requisicionSpool = new RequisicionSpool
                    {
                        ProyectoID = EntityID.Value,
                        TipoPruebaSpoolID = TipoPruebaSpoolID,
                        FechaRequisicion = rdpFechaRequisicion.SelectedDate.Value,
                        NumeroRequisicion = txtNumeroRequisicion.Text,
                        Observaciones = txtObservaciones.Text
                    };

                    RequisicionPruebasBO.Instance.GeneraRequisicionSpool(requisicionSpool, IDs, SessionFacade.UserId);

                    //Actualiza el grid de la ventana padre para que refleje que el reporte ya se generó
                    JsUtils.RegistraScriptActualizaGridGenerico(this);

                    lnkReporte.ProyectoID = EntityID.Value;
                    lnkReporte.NombresParametrosReporte = "NumeroRequisicion,TipoPruebaID";
                    lnkReporte.ValoresParametrosReporte = string.Format("{0},{1}", txtNumeroRequisicion.Text, TipoPruebaSpoolID);

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
        #endregion
    }
}