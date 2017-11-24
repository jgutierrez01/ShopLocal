using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Proyectos;
using SAM.Entities;
using SAM.Entities.Cache;
using SAM.BusinessObjects.Utilerias;
using Mimo.Framework.Common;
using Microsoft.Reporting.WebForms;
using SAM.Web.Common;

namespace SAM.Web.WorkStatus
{ 
    public partial class DetReporteArmadoSoldadura : SamPaginaPrincipal
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

        private int _proyectoReporteID
        {
            get 
            {
                return (int)ViewState["proyectoReporte"];
            }
            set
            {
                ViewState["proyectoReporte"] = value;
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
                string rutaReporte = string.Empty;

                
                TipoReporteProyectoEnum tipoRep = (TipoReporteProyectoEnum)ddlTipoReporte.SelectedValue.SafeIntParse();
                rutaReporte = dameRutaReporte(_proyectoID, tipoRep);

                MyReportServerCredentials credenciales = new MyReportServerCredentials(Config.UsernameReportingServices, Config.PasswordReportingServices, Config.DomainReportingServices);

                rptVisorReporte.Reset();
                rptVisorReporte.ProcessingMode = ProcessingMode.Remote;
                rptVisorReporte.ServerReport.ReportServerCredentials = credenciales;
                rptVisorReporte.ServerReport.ReportServerUrl = new Uri(Config.UrlReportingServices);
                rptVisorReporte.ServerReport.ReportPath = rutaReporte;


                List<ReportParameter> aParams = new List<ReportParameter>();

                aParams.Add(new ReportParameter("ProyectoID", _proyectoID.ToString(), false));
                aParams.Add(new ReportParameter("FechaInicial", dtpDesde.SelectedDate.SafeDateAsStringParse(), false));
                aParams.Add(new ReportParameter("FechaFinal", dtpHasta.SelectedDate.SafeDateAsStringParse(), false));

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

        private string dameRutaReporte(int proyectoID, TipoReporteProyectoEnum tipoRep)
        {
            ProyectoReporteCache personalizado =
                CacheCatalogos.Instance
                              .ObtenerProyectoReporte()
                              .Where(x => x.ProyectoID == proyectoID && x.TipoReporte == tipoRep)
                              .SingleOrDefault();

            if (personalizado != null)
            {
                if (!personalizado.Nombre.StartsWith("/"))
                {
                    return "/" + personalizado.Nombre;
                }
                else
                {
                    return personalizado.Nombre;
                }
            }

            string reporteDefault = string.Empty;

            switch (tipoRep)
            {
                case TipoReporteProyectoEnum.Armado:
                    reporteDefault = WebConstants.ReportesAplicacion.Wkst_Armado;
                    break;
                case TipoReporteProyectoEnum.Soldadura:
                    reporteDefault = WebConstants.ReportesAplicacion.Wkst_Soldadura;
                    break;
            }

            if (LanguageHelper.CustomCulture == LanguageHelper.INGLES)
            {
                reporteDefault += "-ingles";
            }

            return string.Concat("/", Config.ReportingServicesDefaultFolder, "/", reporteDefault);
        }
    }
}
