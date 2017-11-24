using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.Entities;
using Mimo.Framework.Common;
using Microsoft.Reporting.WebForms;

namespace SAM.Web.Administracion
{
    public partial class DetReporteDestajos : SamPaginaPrincipal
    {

        #region Propiedades ViewState

        /// <summary>
        /// ID del proyecto seleccionado en el dropdown
        /// </summary>
        private int _proyectoID
        {
            get
            {
                return (int)ViewState["proyecto"];
            }
            set
            {
                ViewState["proyecto"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
            }
        }

        protected void ddlProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            _proyectoID = ddlProyecto.SelectedValue.SafeIntParse();

            if (_proyectoID > 0)
            {
                proyEncabezado.Visible = true;
                proyEncabezado.BindInfo(_proyectoID);
            }
            else
            {
                proyEncabezado.Visible = false;
            }
        }

        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            try
            {
                string reporteDefault = string.Empty;
                string rutaReporte = string.Empty;
                int idioma = 0;

                reporteDefault = WebConstants.ReportesAplicacion.Admon_Destajos;

                if (LanguageHelper.CustomCulture == LanguageHelper.INGLES)
                {
                    reporteDefault += "-ingles";
                    idioma = 1;
                }

                rutaReporte = string.Concat("/", Config.ReportingServicesDefaultFolder, "/", reporteDefault);

                MyReportServerCredentials credenciales = new MyReportServerCredentials(Config.UsernameReportingServices, Config.PasswordReportingServices, Config.DomainReportingServices);

                rptVisorReporte.Reset();
                rptVisorReporte.ProcessingMode = ProcessingMode.Remote;
                rptVisorReporte.ServerReport.ReportServerCredentials = credenciales;
                rptVisorReporte.ServerReport.ReportServerUrl = new Uri(Config.UrlReportingServices);
                rptVisorReporte.ServerReport.ReportPath = rutaReporte;


                List<ReportParameter> aParams = new List<ReportParameter>();

                aParams.Add(new ReportParameter("ProyectoID", _proyectoID.ToString(), false));
                aParams.Add(new ReportParameter("Idioma", idioma.ToString(), false));

                rptVisorReporte.ServerReport.SetParameters(aParams);
                rptVisorReporte.ServerReport.Refresh();
                rptVisorReporte.Visible = true;
                litReporteNoEncontrado.Visible = false;
            }
            catch (ReportServerException rse)
            {
                if (rse.Message.Contains("cannot be found") || rse.Message.Contains("No se encuentra"))
                {
                    litReporteNoEncontrado.Visible = true;
                    rptVisorReporte.Visible = false;
                }
                else
                {
                    throw rse;
                }
            }
        }


    }
}