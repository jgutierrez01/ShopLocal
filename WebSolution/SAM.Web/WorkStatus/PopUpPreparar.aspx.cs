using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Workstatus;
using SAM.Web.Common;
using SAM.BusinessObjects.Proyectos;
using SAM.Entities;

namespace SAM.Web.WorkStatus
{
    public partial class PopUpPreparar : SamPaginaPopup
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

        public Proyecto ProyectoActual
        {
            get
            {
                return (Proyecto)ViewState["ProyectoActual"];
            }
            set
            {
                ViewState["ProyectoActual"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WorkstatusSpools = Request.QueryString["IDs"].Split(',').Select(n => n.SafeIntParse()).ToArray();
                //CargarSugerenciaFolio();
            }
        }

        //protected void CargarSugerenciaFolio()
        //{
        //    ProyectoActual = ProyectoBO.Instance.ObtenerConfiguracionYConsecutivosPorWKSID(WorkstatusSpools[0]);
        //    int consecutivo = 0;
        //    string formato = "";
        //    for (int i = 0; i < ProyectoActual.ProyectoConfiguracion.DigitosFolioPreparacion.SafeIntParse(); i++)
        //    {
        //        formato += "0";
        //    }
        //    consecutivo = ProyectoActual.ProyectoConsecutivo.ConsecutivoFolioPreparacion.SafeIntParse() + 1;
        //    txtFolioPreparacion.Text = DateTime.Now.Date.ToString("yyyy-MM-dd") + "-" + consecutivo.ToString(formato);
        //}

        /// <summary>
        /// Evento que etiqueta los spools enviados por query string
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPreparar_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid)
                {
                    string[] folio = txtFolioPreparacion.Text.Split('-');
                    string fecha = folio[0] + "-" + folio[1] + "-" + folio[2];
                    DateTime fechaCapturada = new DateTime();

                    if (!DateTime.TryParse(fecha, out fechaCapturada))
                    {
                        throw new BaseValidationException(Cultura == "en-US" ?
                            string.Format("{0} is not a valid date.", fecha)
                            : string.Format("{0} no es una fecha valida.", fecha));
                    }
                    int nuevoFolio = folio[3].SafeIntParse();
                    EmbarqueBO.Instance.PrepararSpools(WorkstatusSpools, nuevoFolio, SessionFacade.UserId, fechaCapturada);
                    JsUtils.RegistraScriptActualizayCierraVentana(this);
                }

            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
        }
    }
}