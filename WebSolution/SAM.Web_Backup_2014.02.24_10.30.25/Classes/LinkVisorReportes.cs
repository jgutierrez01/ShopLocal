using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI;
using SAM.Entities;

namespace SAM.Web.Classes
{
    [DefaultProperty("ID")]
    [ToolboxData("<{0}:LinkVisorReportes runat=server></{0}:LinkVisorReportes>")]
    public class LinkVisorReportes : HyperLink
    {
        public int ? ProyectoID
        {
            get
            {
                if (ViewState["ProyectoID"] != null)
                {
                    return (int)ViewState["ProyectoID"];
                }

                return null;
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }
        }

        public TipoReporteProyectoEnum ? Tipo
        {
            get
            {
                if (ViewState["Tipo"] != null)
                {
                    return (TipoReporteProyectoEnum)ViewState["Tipo"];
                }

                return null;
            }
            set
            {
                ViewState["Tipo"] = value;
            }
        }

        public bool EsReportePersonalizado
        {
            get
            {
                if (ViewState["EsReportePersonalizado"] != null)
                {
                    return (bool)ViewState["EsReportePersonalizado"];
                }

                return false;
            }
            set
            {
                ViewState["EsReportePersonalizado"] = value;
            }
        }

        public string RutaReportePersonalizado
        {
            get
            {
                if (ViewState["RutaReportePersonalizado"] != null)
                {
                    return (string)ViewState["RutaReportePersonalizado"];
                }

                return string.Empty;
            }
            set
            {
                ViewState["RutaReportePersonalizado"] = value;
            }
        }

        public string NombresParametrosReporte
        {
            get
            {
                if (ViewState["NombreParametrosReporte"] != null)
                {
                    return (string)ViewState["NombreParametrosReporte"];
                }

                return string.Empty;
            }
            set
            {
                ViewState["NombreParametrosReporte"] = value;
            }
        }

        public string ValoresParametrosReporte
        {
            get
            {
                if (ViewState["ValoresParametrosReporte"] != null)
                {
                    return (string)ViewState["ValoresParametrosReporte"];
                }

                return string.Empty;
            }
            set
            {
                ViewState["ValoresParametrosReporte"] = value;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (Visible)
            {
                if (!EsReportePersonalizado)
                {
                    this.NavigateUrl = string.Format("javascript:Sam.Reportes.AbreVisorReporte({0},'{1}','{2}','{3}');", (int)Tipo, NombresParametrosReporte, ValoresParametrosReporte, ProyectoID.Value);
                }
                else
                {
                    this.NavigateUrl = string.Format("javascript:Sam.Reportes.AbreVisorReportePersonalizado('{0}','{1}','{2}');", RutaReportePersonalizado, NombresParametrosReporte, ValoresParametrosReporte);
                }
            }
        }
    }
}