using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities.Grid;
using SAM.Web.Classes;

namespace SAM.Web.WorkStatus
{
    public partial class LstReqPintura : SamPaginaPrincipal
    {
        public HyperLink es;
        public HyperLink gr;
        public ImageButton esBut;
        public ImageButton grBut;

        #region variables javascript
        /// <summary>
        /// Js que abre el popup armado en modo read-only
        /// </summary>
        private const string JS_POPUP_ESPECIFICARSISTEMA = "javascript:Sam.Workstatus.AbrePopupEspecificarSistema('{0}');";

        /// <summary>
        /// js que abre el popup de armado
        /// </summary>
        private const string JS_POPUP_GENERARREQUISICION = "javascript:Sam.Workstatus.AbrePopupGenerarRequisicion('{0}','{1}');";
        #endregion

        #region ViewState

        /// <summary>
        /// variable donde se guarda la informacion del proyecto seleccionado     
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
        /// variable donde se guarda la informacion de la orden de trabajo seleccionada
        /// </summary>
        private int ? OrdenTrabajoID
        {
            get
            {
                return (int?)ViewState["OrdenTrabajoID"];
            }
            set
            {
                ViewState["OrdenTrabajoID"] = value;
            }
        }

        private int AccionID
        {
            get
            {
                return (int)ViewState["AccionID"];
            }
            set
            {
                ViewState["AccionID"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_RequisicionesPinturaSpool);
            }
        }

       
        /// <summary>
        /// se dispara al dar clic en el botón de mostrar, después de seleccionar los filtros deseados.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            //Guardar esto en ViewState para los rebinds del grid
            ProyectoID = filtroGenerico.ProyectoSelectedValue.SafeIntParse();
            OrdenTrabajoID = filtroGenerico.OrdenTrabajoSelectedValue.SafeIntNullableParse();
            AccionID = ddlSeleccion.SelectedValue.SafeIntParse();

            EstablecerDataSource();
            grdReqNum.DataBind();
            phReqNumerosUnicos.Visible = true;

        }

        protected void EstablecerDataSource()
        {
            grdReqNum.DataSource = RequisicionPinturaBO.Instance.ObtenerListadoRequisicionPintura(ProyectoID, OrdenTrabajoID, AccionID);
        }

        /// <summary>
        /// se dispara cuando el grid requiere de datos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdReqNum_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        /// <summary>
        /// se dispara cuando se crean los links del header del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="?"></param>
        protected void grdReqNum_OnItemCreated(object sender, GridItemEventArgs e)
        {
            GridCommandItem commandItem = e.Item as GridCommandItem;

            if (commandItem != null)
            {
                es = (HyperLink)commandItem.FindControl("lnkEspecificar");
                gr = (HyperLink)commandItem.FindControl("lnkGenerar");
                esBut = (ImageButton)commandItem.FindControl("imgEspecificar");
                grBut = (ImageButton)commandItem.FindControl("imgGenerar");

                ///acciones
                es.NavigateUrl = String.Format(JS_POPUP_ESPECIFICARSISTEMA, grdReqNum.ClientID);
                gr.NavigateUrl = String.Format(JS_POPUP_GENERARREQUISICION, ProyectoID, grdReqNum.ClientID);
                ///

                if (AccionID == 1)
                {
                    es.Visible = true;
                    esBut.Visible = true;
                    gr.Visible = false;
                    grBut.Visible = false;
                }
                else
                {
                    es.Visible = false;
                    esBut.Visible = false;
                    gr.Visible = true;
                    grBut.Visible = true;
                }
            }
        }

        protected void lnkActualizar_Click(object sender, EventArgs e)
        {
            EstablecerDataSource();
            grdReqNum.DataBind();
        }


    }
}