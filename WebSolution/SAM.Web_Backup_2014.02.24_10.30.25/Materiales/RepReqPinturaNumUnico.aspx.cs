using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.BusinessObjects.Produccion;
using Telerik.Web.UI;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.Web.Classes;
using SAM.BusinessObjects.Materiales;

namespace SAM.Web.Materiales
{
    public partial class RepReqPinturaNumUnico : SamPaginaPrincipal
    {
        #region ViewState de los filtros

        /// <summary>
        /// ID del proyecto seleccionado en el dropdown
        /// </summary>
        private int _proyecto
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

        /// <summary>
        /// fecha de inicio en la cual se hara el filtro
        /// </summary>
        private DateTime? _desde
        {
            get
            {
                if (ViewState["desde"] != null)
                {
                    return (DateTime)ViewState["desde"];
                }
                return null;
            }
            set
            {
                ViewState["desde"] = value;
            }
        }

        /// <summary>
        ///  fecha final en la cual se hara el filtro
        /// </summary>
        private DateTime? _hasta
        {
            get
            {
                if (ViewState["hasta"] != null)
                {
                    return (DateTime)ViewState["hasta"];
                }
                return null;
            }
            set
            {
                ViewState["hasta"] = value;
            }
        }

        /// <summary>
        /// numero de requisicion a buscar en el filtro
        /// </summary>
        private string _numeroRequisicion
        {
            get
            {
                if (ViewState["numeroRequisicion"] != null)
                {
                    return (string)ViewState["numeroRequisicion"];
                }
                return null;
            }
            set
            {
                ViewState["numeroRequisicion"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.mat_ReporteRequisicionesPintura);
                cargaCombo();
            }
        }

        //metodo para cargar el combo "ddlProyecto".
        private void cargaCombo()
        {
            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
        }

        /// <summary>
        /// manda a llamar el rebind del grdRequisiciones para que se generen los renglones
        /// </summary>
        protected void btnMostrar_OnClick(object sender, EventArgs e)
        {
            Validate();

            if (IsValid)
            {
                //Guardar en ViewState los filtros
                _proyecto = ddlProyecto.SelectedValue.SafeIntParse();
                _desde = dtpDesde.SelectedDate;
                _hasta = dtpHasta.SelectedDate;
                _numeroRequisicion = txtNumeroRequisicion.Text;

                phGrid.Visible = true;
                grdRequisiciones.Rebind();
            }
        }

        /// <summary>
        /// trae el DataSource filtrado con sus resultados para mostrar al grdRequisiciones
        /// </summary>
        private void EstablecerDataSource()
        {
            //llama a llenar el grid con los filtros
            grdRequisiciones.DataSource = RequisicionNumeroUnicoBO.Instance.ObtenerReportePinturaNumUnicoFiltrado(_proyecto,
                                                                                _desde,
                                                                                _hasta,
                                                                                _numeroRequisicion
                                                                               );
        }

        /// <summary>
        /// desplega el header del proyecto
        /// </summary>
        protected void ddlProyecto_SelectedItemChanged(object sender, EventArgs e)
        {
            int proyectoID = ddlProyecto.SelectedValue.SafeIntParse();

            if (proyectoID > 0)
            {
                proyEncabezado.BindInfo(proyectoID);
                proyEncabezado.Visible = true;
            }
            else
            {
                proyEncabezado.Visible = false;
            }
        }

        /// <summary>
        /// llena el grid con la informacion requerida
        /// </summary>
        protected void grdRequisiciones_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        /// <summary>
        /// actualiza el grdRequisiciones
        /// </summary>
        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdRequisiciones.Rebind();
        }

        protected void grdRequisiciones_ItemDataBound(object source, GridItemEventArgs e)
        {
        }

        /// <summary>
        /// Accion dependiendo al botton del templeate seleccionado
        /// Borrar. Borra el registro seleccionado
        /// </summary>
        protected void grdRequisiciones_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                try
                {
                    int requisicionNumeroUnicoID = e.CommandArgument.SafeIntParse();
                    RequisicionNumeroUnicoBO.Instance.Borra(requisicionNumeroUnicoID);
                    EstablecerDataSource();
                    grdRequisiciones.DataBind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex, "");
                }
            }
        }
    }
}