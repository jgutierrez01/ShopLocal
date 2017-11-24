using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.Web.Common;
using Telerik.Web.UI;
using SAM.BusinessObjects.Administracion;
using SAM.Entities.Grid;

namespace SAM.Web.Administracion
{
    public partial class LstPendientes : SamPaginaPrincipal
    {
        #region Propiedades privadas en el ViewState
        
        private int _patioID
        {
            get
            {
                return ViewState["PatioID"].SafeIntParse();
            }
            set
            {
                ViewState["PatioID"] = value;
            }
        }

        private int _proyectoID
        {
            get
            {
                return ViewState["ProyectoID"].SafeIntParse();
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }
        }

        private bool _todos
        {
            get
            {
                return ViewState["Todos"].SafeBoolParse();
            }
            set
            {
                ViewState["Todos"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.adm_Pendientes);
                cargaCombos();
            }
        }

        private void cargaCombos()
        {
            ddlPatio.BindToEntiesWithEmptyRow(UserScope.MisPatios);
            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
        }

        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            Validate();
            if (IsValid)
            {
                _patioID = ddlPatio.SelectedValue.SafeIntParse();
                _proyectoID = ddlProyecto.SelectedValue.SafeIntParse();
                _todos = chkMostrarTodos.Checked;

                establecerDataSource();
                grdPendientes.DataBind();
                grdPendientes.Visible = true;
            }
        }

        //Metodo para cargar el combo "ddlProyectos" basado en la selección del patio
        protected void DdlPatioOnSelectedIndexChanged(object sender, EventArgs e)
        {
            int patioID = ddlPatio.SelectedValue.SafeIntParse();
            //if (patioID < 1)
            //{
            //    proyEncabezado.Visible = false;
            //}

            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos.Where(x => x.PatioID == patioID));
        }

        protected void grdPendientes_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            establecerDataSource();
        }

        protected void grdPendientes_OnItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                
                GridCommandItem i = (GridCommandItem)e.Item;

                HyperLink hlAgregar = i.FindControl("lnkAgregar") as HyperLink;
                HyperLink imgAgregar = i.FindControl("imgAgregar") as HyperLink;

          
                string jsLink = string.Format("javascript:Sam.Produccion.AbrePopUpNuevoPendiente();");

                hlAgregar.NavigateUrl = jsLink;
                imgAgregar.NavigateUrl = jsLink;
            }

            
        }

        protected void grdPendientes_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                HyperLink edita = e.Item.FindControl("hypEditar") as HyperLink;
                GrdPendientes pendiente = (GrdPendientes)e.Item.DataItem;
                edita.NavigateUrl = string.Format("javascript:Sam.Produccion.AbrePopUpEdicionPendiente('{0}');", pendiente.PendienteID);
            }
        }

        protected void lnkActualizar_OnClick(object sender, EventArgs e)
        {
            grdPendientes.ResetBind();
            establecerDataSource();
            grdPendientes.DataBind();
        }

        private void establecerDataSource()
        {
            grdPendientes.DataSource = PendienteBO.Instance.ObtenerPendientes(_proyectoID, _patioID, _todos);
        }

        protected void cvSeleccionFiltro_OnServerValidate(object sender, ServerValidateEventArgs args)
        {
            if (ddlPatio.SelectedValue.SafeIntParse() <= 0 && ddlProyecto.SelectedValue.SafeIntParse() <= 0)
            {
                args.IsValid = false;
            }
        }
    }
}