using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessObjects.Ingenieria;
using SAM.Entities;


namespace SAM.Web.Ingenieria
{
    public partial class PopUpDatosNoEncontrados : SamPaginaPopup
    {
        private List<JuntaSpool> _PeqsNoEncontrados
        {
            get
            {
                if (ViewState["Peqs"] != null )
                {
                    return (List<JuntaSpool>)ViewState["Peqs"];
                }

                return new List<JuntaSpool>();
            }
            set
            {
                ViewState["Peqs"] = value;
            }
        }

        private List<JuntaSpool> _KgtNoEncontrados
        {
            get
            {
                if (ViewState["Kgt"] != null)
                {
                    return (List<JuntaSpool>)ViewState["Kgt"];
                }

                return new List<JuntaSpool>();
            }
            set
            {
                ViewState["Kgt"] = value;
            }
        }

        private List<JuntaSpool> _EspNoEncontrados
        {
            get
            {
                if (ViewState["Esp"] != null)
                {
                    return (List<JuntaSpool>)ViewState["Esp"];
                }

                return new List<JuntaSpool>();
            }
            set
            {
                ViewState["Esp"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _PeqsNoEncontrados = (List<JuntaSpool>)Session["Session.PeqsNoEncontrados"];
                _KgtNoEncontrados = (List<JuntaSpool>)Session["Session.KgtNoEncontrados"];
                _EspNoEncontrados = (List<JuntaSpool>)Session["Session.EspNoEncontrados"];

                //************************/

                Session.Remove("Session.PeqsNoEncontrados");
                Session.Remove("Session.KgtNoEncontrados");
                Session.Remove("Session.EspNoEncontrados");
            }

            establecerDataSource();
        }

        private void establecerDataSource()
        {
            grdPeqs.DataSource = JuntaSpoolBO.Instance.ObtenerPeqsNoEncontrados(_PeqsNoEncontrados);
            grdKgt.DataSource = JuntaSpoolBO.Instance.ObtenerKgtNoencontrados(_KgtNoEncontrados);
            grdEsp.DataSource = JuntaSpoolBO.Instance.ObtenerEspNoencontrados(_EspNoEncontrados);
        }

        protected void grd_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            establecerDataSource();
        }
    }
}