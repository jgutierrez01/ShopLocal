using System;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Proyectos;
using SAM.Entities;
using SAM.BusinessObjects.Produccion;
using Mimo.Framework.Exceptions;
using SAM.BusinessLogic.Produccion;
using SAM.Web.Common;

namespace SAM.Web.Produccion
{
    /// <summary>
    /// Popup que se encarga de generar una nueva ODT en base a una serie de spools pasados por QS.
    /// </summary>
    public partial class PopupCruceOdt : SamPaginaPopup
    {

        #region Variables privadas en ViewState

        /// <summary>
        /// ID del proyecto en cuestión
        /// </summary>
        private int ProyectoID
        {
            get
            {
                return (int)ViewState["ProyectoID"];
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }
        }

        /// <summary>
        /// CSV string que contiene los IDs de los spools a los cuales se le van a generar las ODTs
        /// </summary>
        private string SpoolIDs
        {
            get
            {
                return ViewState["SpoolIDs"].ToString();
            }
            set
            {
                ViewState["SpoolIDs"] = value;
            }
        }

        private string SortOrder
        {
            get
            {
                return ViewState["SortOrder"].ToString();
            }
            set
            {
                ViewState["SortOrder"] = value;
            }
        }

        #endregion

        /// <summary>
        /// Obtiene los parámetros esperados del QS, los guarda en ViewState y luego carga los controles de la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                radODT.SelectedIndexChanged += new Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventHandler(radioButtonList_OnSelectedIndexChanged);

                ProyectoID = Request.QueryString["PID"].SafeIntParse();
                
                hdnProyectoID.Value = ProyectoID.SafeStringParse();

                if (!SeguridadQs.TieneAccesoAProyecto(ProyectoID))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando generar una ODT en base a un cruce para un proyecto {1} al cual no tiene permisos", SessionFacade.UserId, ProyectoID);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                SpoolIDs = Request.QueryString["SPIDS"];

                SortOrder = Request.QueryString["SortOrder"];
                carga();
            }
        }

        /// <summary>
        /// Carga los elementos de la forma con sus valores default.
        /// </summary>
        private void carga()
        {
            txtOdt.Text = ProyectoBO.Instance.SiguienteConsecutivoOdt(ProyectoID).ToString();
            ddlTaller.BindToEntiesWithEmptyRow(UserScope.TalleresPorProyecto(ProyectoID));
        }


        /// <summary>
        /// En base a los IDs de los spools que se tiene en ViewState se genera una orden
        /// de trabajo que contenga esos spools.
        /// 
        /// Se envía un mensaje de éxito (en caso que asi sea) al usuario informándole sobre la
        /// nueva ODT que se acaba de crear.  Al hacer lo anterior se emite JS a la página
        /// que actualiza el grid de la página padre en caso que exista para quitar
        /// los spools que se acaban de utilizar para la ODT.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            //Esto no lo vamos a validar, si truena es porque alguien le anda moviendo a los QS
            int [] ids = SpoolIDs.Split(',').Select( n => int.Parse(n)).ToArray();

            if (Page.IsValid)
            {
                try
                {
                    OrdenTrabajo odt;
                    if (rdbODT.SelectedIndex == 0)
                    {
                        odt =
                        OrdenTrabajoBL.Instance.GeneraNueva(ProyectoID,
                                                                ids,
                                                                ddlTaller.SelectedValue.SafeIntParse(),
                                                                txtOdt.Text.SafeIntParse(),
                                                                DateTime.Now,
                                                                SessionFacade.UserId,
                                                                false,
                                                                SortOrder,
                                                                false);
                    }
                    else
                    {
                        odt =
                            OrdenTrabajoBL.Instance.GeneraCruceODTExistente(ProyectoID, ids, radODT.SelectedValue.SafeIntParse(), DateTime.Now, SessionFacade.UserId, SortOrder, false);
                    }
                    byte[] reporte = UtileriasReportes.ObtenReporteOdt(odt.OrdenTrabajoID, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true);
                    //ACTIVAR LINEA EN CASO DE QUE SE DESEEE GUARDAR EL PDF DE FORMA AUTOMATICA EL MOMENTO DE GENERAR UN ODT NUEVO 
                    //File.WriteAllBytes(ConfigurationManager.AppSettings["Sam.Produccion.ODTFilesDirectory"] + "\\ODT_" + odt.NumeroOrden + ".PDF", reporte);   

                    enviaMensajeExito(odt);
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }
        }


        /// <summary>
        /// Muestra un mensaje en pantalla con la información de la nueva ODT y diciendo que
        /// se acaba de generar
        /// </summary>
        /// <param name="odt">Entidad de la ODT recientemente generada</param>
        private void enviaMensajeExito(OrdenTrabajo odt)
        {
            phControles.Visible = false;
            phMensaje.Visible = true;
            litNumOdt.Text = odt.NumeroOrden;

            hlReporteOdt.NavigateUrl = WebConstants.ProduccionUrl.ReporteODT + string.Format("?ID={0}&PDF=true", odt.OrdenTrabajoID);

            //Emite javascript para que la página padre actualice su grid y quite los spools de los cuales
            //se acaba de generar ODT.
            JsUtils.RegistraScriptActualizaGridCruce(this);
        }

        protected void radioButtonList_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbODT.SelectedIndex == 0)
            {
                textODT.Visible = true;
                radcomboODT.Visible = false;
            }
            else
            {
                textODT.Visible = false;
                radcomboODT.Visible = true;
            }
        }
    }
}