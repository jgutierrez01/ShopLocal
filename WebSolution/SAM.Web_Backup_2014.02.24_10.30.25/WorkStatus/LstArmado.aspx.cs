using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.Entities.Personalizadas;
using Telerik.Web.UI;
using SAM.Entities.Reportes;
using System.Threading;
using Mimo.Framework.Common;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities.Grid;
using SAM.Entities;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Ingenieria;

namespace SAM.Web.WorkStatus
{
    public partial class LstArmado : SamPaginaPrincipal
    {
        /// <summary>
        /// Js que abre el popup armado en modo read-only
        /// </summary>
        private const string JS_POPUP_ARMADORO = "javascript:Sam.Workstatus.AbrePopupArmado('{0}','true');";

        /// <summary>
        /// js que abre el popup de armado
        /// </summary>
        private const string JS_POPUP_ARMADO = "javascript:Sam.Workstatus.AbrePopupArmado('{0}','false');";

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

        private Dictionary<int, int> JuntasSpool
        {
            get
            {
                if (ViewState["JuntasSpool"] != null)
                {
                    return (Dictionary<int, int>)ViewState["JuntasSpool"];
                }

                return null;
            }
            set
            {
                ViewState["JuntasSpool"] = value;
            }
        }
        #endregion

        /// <summary>
        /// Cargar el combo de proyectos con aquellos proyectos a los cuales tiene permiso/acceso el usuario
        /// loggeado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_Armado);
                JuntasSpool = new Dictionary<int, int>();
            }
        }


        /// <summary>
        /// inicializa los links de "VER" "ARMAR" "CANCELAR" del grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdArmado_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                GrdArmado armado = (GrdArmado)item.DataItem;

                HyperLink hlVer = (HyperLink)item["Ver_h"].FindControl("hlVer");
                HyperLink hlArmar = (HyperLink)item["Armar_h"].FindControl("hlArmar");
                ImageButton hlCancelar = (ImageButton)item["Armar_h"].FindControl("hlCancelar");
                
                int juntaArmadoID = armado.JuntaArmadoID.SafeIntParse();

                if (armado.TipoJunta == TipoJuntas.TH || armado.TipoJunta == TipoJuntas.TW)
                {
                    Literal lit = new Literal();
                    lit.Text = "NA";
                    item["hdSoldadura"].Controls.Clear();
                    item["hdSoldadura"].Controls.Add(lit);
                }

                //si si está se habilita link de ver y deshabilita el de armar
                if (armado.EstatusID == (int)EstatusArmado.Armado)
                {
                    hlVer.Visible = true;
                    hlVer.NavigateUrl = string.Format(JS_POPUP_ARMADORO, armado.JuntaSpoolID);
                    hlArmar.Visible = false;

                    //Solo se habilita si no esta en hold
                    hlCancelar.Visible = !armado.Hold;

                }//si no, se habilita link para armar.
                else
                {
                    //if (armado.EstatusID == (int)EstatusArmado.Despachado && !armado.Hold)
                    //{
                    //    hlArmar.Visible = true;
                    //    hlArmar.NavigateUrl = string.Format(JS_POPUP_ARMADO, armado.JuntaSpoolID);
                    //}
                    if ((armado.EstatusID == (int)EstatusArmado.Despachado || armado.EstatusID == (int)EstatusArmado.SinDespacho) && !armado.Hold)
                    {
                        hlArmar.Visible = true;
                        hlArmar.NavigateUrl = string.Format(JS_POPUP_ARMADO, armado.JuntaSpoolID);
                    }

                    hlVer.Visible = false;
                    hlCancelar.Visible = false;
                }

                if (juntaArmadoID != -1 && !JuntasSpool.ContainsKey(juntaArmadoID))
                {
                    JuntasSpool.Add(juntaArmadoID, armado.JuntaSpoolID);
                }
            }
        }

        /// <summary>
        /// se manda llamar a la hora de que se necesite recargar la informacion del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdArmado_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        /// <summary>
        /// establece el datasource del grid de juntas armado
        /// </summary>
        private void EstablecerDataSource()
        {
            grdArmado.DataSource = ArmadoBO.Instance.ObtenerListaArmado(ProyectoID, OrdenTrabajoID, OrdenTrabajoSpoolID);
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
            grdArmado.DataBind();

            phArmado.Visible = true;

        }

        protected void lnkActualizar_Click(object sender, EventArgs e)
        {
            EstablecerDataSource();
            grdArmado.DataBind();
        }

        /// <summary>
        /// Metodo que se dispara al click del boton de ver detalle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdArmado_ItemCommand(object sender, GridCommandEventArgs e)
        {
            string command = e.CommandName;
            int juntaArmadoID = e.CommandArgument.SafeIntParse();

            if (command == "cancelar")
            {
                try
                {
                    int juntaSpoolID = JuntasSpool[juntaArmadoID];
                    JuntaSpool juntaSpool = JuntaSpoolBO.Instance.Obtener(juntaSpoolID);
                    TipoJunta tipoJunta = TipoJuntaBO.Instance.Obtener(juntaSpool.TipoJuntaID);
                    JuntaWorkstatus jws = ArmadoBO.Instance.ObtenerJuntaWorkstatusPorJuntaSpoolID(juntaSpoolID);

                    JuntasSpool.Remove(juntaArmadoID);
                    // Quitamos soldadura de junta si la misma es de tipo TW
                    if (tipoJunta.Codigo == TipoJuntas.TW && jws.JuntaSoldaduraID != null)
                    {
                        SoldaduraBO.Instance.BorraSoldadura(jws.JuntaSoldaduraID.SafeIntParse(), SessionFacade.UserId);
                    }

                    ArmadoBO.Instance.BorraArmado(juntaArmadoID, SessionFacade.UserId);
                    grdArmado.Rebind();
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }
        }
    }
}