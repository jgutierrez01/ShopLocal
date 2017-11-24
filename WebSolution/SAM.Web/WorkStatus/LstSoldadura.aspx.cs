using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.Entities.Personalizadas;
using SAM.Web.Common;
using Telerik.Web.UI;
using SAM.Entities.Reportes;
using System.Threading;
using Mimo.Framework.Common;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities.Grid;
using SAM.Entities;
using Mimo.Framework.Exceptions;
using SAM.Entities.Cache;
using SAM.BusinessObjects.Utilerias;

namespace SAM.Web.WorkStatus
{
    public partial class LstSoldadura : SamPaginaPrincipal
    {

        /// <summary>
        /// Js que abre el popup soldadura en modo read-only
        /// </summary>
        private const string JS_POPUP_SOLDADURARO = "javascript:Sam.Workstatus.AbrePopupSoldadura('{0}','true');";

        /// <summary>
        /// js que abre el popup de soldadura
        /// </summary>
        private const string JS_POPUP_SOLDADURA = "javascript:Sam.Workstatus.AbrePopupSoldadura('{0}','false');";

        private const string JS_POPUP_SOLDAURAEDICION = "javascript:Sam.Workstatus.AbrePopUpSoldaduraEdicion({0});";


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
        private int OrdenTrabajoID
        {
            get
            {
                return (int)ViewState["OrdenTrabajoID"];
            }
            set
            {
                ViewState["OrdenTrabajoID"] = value;
            }
        }

        /// <summary>
        /// variable donde se guarda la informacion del numero control seleccionado      
        /// </summary>
        private int OrdenTrabajoSpoolID
        {
            get
            {
                return (int)ViewState["OrdenTrabajoSpoolID"];
            }
            set
            {
                ViewState["OrdenTrabajoSpoolID"] = value;
            }
        }

        private bool EdicionEspecial
        {
            get
            {
                return ViewState["EdicionEspecialSoldadura"].SafeBoolParse();
            }
            set
            {
                ViewState["EdicionEspecialSoldadura"] = value;
            }
        }

        /// <summary>
        /// carga el combo de los proyectos con los proyectos a los que tiene acceso el usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_Soldadura);
            }
        }


        /// <summary>
        /// Se dispara cuando el usuario decide mostrar las órdenes de trabajo en base
        /// a sus filtros especificados.
        /// </summary>
        protected void btnMostrar_Click(object sender, EventArgs e)
        {
           
                //Guardar esto en ViewState para los rebinds del grid
                ProyectoID = filtroGenerico.ProyectoSelectedValue.SafeIntParse();
                OrdenTrabajoID = filtroGenerico.OrdenTrabajoSelectedValue.SafeIntParse();
                OrdenTrabajoSpoolID = filtroGenerico.NumeroControlSelectedValue.SafeIntParse();

                EstablecerDataSource();
                grdSoldadura.DataBind();

                phSoldadura.Visible = true;
           
        }
        /// <summary>
        /// Limpia los filtros, sortings y paginación y actualiza los datos del grid desde BD.
        /// </summary>
        public void lnkActualizar_Click(object sender, EventArgs e)
        {
            grdSoldadura.ResetBind();
            grdSoldadura.Rebind();
        }


        /// <summary>
        /// inicializa los links de "VER", "SOLDAR", o "CANCELAR" en el grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSoldadura_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                GrdSoldadura soldadura = (GrdSoldadura)item.DataItem;

                HyperLink hlVer = (HyperLink)item["Ver_h"].FindControl("hlVer");
                HyperLink hlSoldar = (HyperLink)item["Soldar_h"].FindControl("hlSoldar");
                ImageButton hlCancelar = (ImageButton)item["Soldar_h"].FindControl("hlCancelar");

                if (soldadura.TipoJunta == TipoJuntas.TH || soldadura.TipoJunta == TipoJuntas.TW)
                {
                    Literal lit = new Literal();
                    lit.Text = "NA";
                    item["hdSoldadura"].Controls.Clear();
                    item["hdSoldadura"].Controls.Add(lit);
                }     

                //si si está se habilita link de ver y deshabilita el de armar
                if (soldadura.EstatusID == (int)EstatusSoldadura.Soldado)
                {
                    hlVer.Visible = true;
                    hlVer.NavigateUrl = string.Format(JS_POPUP_SOLDADURARO, soldadura.JuntaSpoolID);
                    hlSoldar.Visible = false;

                    //Solo se habilita si no esta en hold
                    hlCancelar.Visible = !soldadura.Hold;
                }
                else
                {// si ya se encuentra armado y no tiene hold y no es una junta tipo tw o th se habilita el link para soldar.
                    //if (soldadura.EstatusID == (int)EstatusArmado.Armado && !soldadura.Hold && (soldadura.TipoJunta != TipoJuntas.TH || soldadura.TipoJunta != TipoJuntas.TW))
                    //{
                    //    hlSoldar.Visible = true;
                    //    hlSoldar.NavigateUrl = string.Format(JS_POPUP_SOLDADURA, soldadura.JuntaSpoolID);
                    //}

                    if (!soldadura.Hold && (soldadura.TipoJunta != TipoJuntas.TH || soldadura.TipoJunta != TipoJuntas.TW))
                    {
                        hlSoldar.Visible = true;
                        hlSoldar.NavigateUrl = string.Format(JS_POPUP_SOLDADURA, soldadura.JuntaSpoolID);
                    }

                    hlVer.Visible = false;
                    hlCancelar.Visible = false;
                }

                if (SessionFacade.PermisoEdicionesEspeciales && !soldadura.Hold && soldadura.EstatusID == (int)EstatusSoldadura.Soldado)
                {
                    HyperLink lnEditar = (HyperLink)item["editarRegistro"].FindControl("hlEditarSoldadura");
                    lnEditar.NavigateUrl = string.Format(JS_POPUP_SOLDAURAEDICION, soldadura.JuntaSpoolID);
                    lnEditar.Visible = true;
                }

                if (string.IsNullOrEmpty(soldadura.NumeroControl) )
                {
                    hlSoldar.Visible = false;
                }
            }
        }

        /// <summary>
        /// utilizado cuando el grid manda llamar la propiedad on need data source 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSoldadura_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        /// <summary>
        /// va a soldaduraBO y obtiene la lista de GrdSoldadura que rellenará el grid.
        /// </summary>
        private void EstablecerDataSource()
        {
            grdSoldadura.DataSource = SoldaduraBO.Instance.ObtenerListaSoldadura(ProyectoID, OrdenTrabajoID, OrdenTrabajoSpoolID);
        }

        /// <summary>
        /// Metodo que se dispara al click del boton de eliminar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSoldadura_ItemCommand(object sender, GridCommandEventArgs e)
        {
            string command = e.CommandName;
            int juntaSoldaduraID = e.CommandArgument.SafeIntParse();
            if (command == "cancelar")
            {
                try
                {
                    SoldaduraBO.Instance.BorraSoldadura(juntaSoldaduraID, SessionFacade.UserId);
                    grdSoldadura.Rebind();
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }
        }
    }
}