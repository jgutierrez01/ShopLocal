using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.BusinessLogic;
using SAM.BusinessObjects.Materiales;
using SAM.BusinessObjects.Proyectos;
using SAM.Web.Classes;
using SAM.BusinessObjects;
using SAM.Entities;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Data;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using SAM.Entities.Grid;
using SAM.Entities.Cache;

namespace SAM.Web.Materiales
{
    public partial class LstRecepcion : SamPaginaPrincipal
    {
        private readonly List<ObjectSetOrder> ORDEN_DEFAULT = new List<ObjectSetOrder>(new[] { new ObjectSetOrder { ColumnName = "Fecha", Order = SortOrder.Ascending } });

        #region Propiedades Privadas
        private int PatioID
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

        private int ProyectoID
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

        private DateTime? FechaInicial
        {
            get
            {
                if (ViewState["FechaInicial"] == null)
                {
                    ViewState["FechaInicial"] = DateTime.MinValue;
                }
                return (DateTime)ViewState["FechaInicial"];
            }
            set
            {
                ViewState["FechaInicial"] = value;
            }
        }

        private DateTime? FechaFinal
        {
            get
            {
                if (ViewState["FechaFinal"] == null)
                {
                    ViewState["FechaFinal"] = DateTime.MaxValue;
                }
                return (DateTime)ViewState["FechaFinal"];
            }
            set
            {
                ViewState["FechaFinal"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.mat_Recepciones);
                CargaCombo();
            }
        }

        //metodo para cargar el combo "ddlPatio".
        private void CargaCombo()
        {
            ddlPatio.BindToEntiesWithEmptyRow(UserScope.MisPatios);
            if (EntityID.HasValue)
            {
                List<ProyectoCache> proyecto = UserScope.MisProyectos;
                int patioID = proyecto.Where(x => x.ID == EntityID.Value.SafeIntParse()).Select(x => x.PatioID).Single();
                ddlPatio.SelectedValue = patioID.ToString();

                ddlProyecto.BindToEntiesWithEmptyRow(proyecto.Where(x => x.PatioID == patioID));
                ddlProyecto.SelectedValue = EntityID.Value.ToString();

                PatioID = ddlPatio.SelectedValue.SafeIntParse();
                ProyectoID = ddlProyecto.SelectedValue.SafeIntParse();
                FechaInicial = dtpDesde.SelectedDate;
                FechaFinal = dtpHasta.SelectedDate;

                EstablecerDataSource();
                grdRecepcion.DataBind();
                grdRecepcion.Visible = true;
            }
        }

        //Metodo para cargar el combo "ddlProyectos" basado en la selección del patio
        protected void DdlPatioOnSelectedIndexChanged(object sender, EventArgs e)
        {
            int patioID = ddlPatio.SelectedValue.SafeIntParse();
            if (patioID < 1)
            {
                proyEncabezado.Visible = false;
            }

            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos.Where(x => x.PatioID == patioID));
        }

        /// <summary>
        /// Método que carga el encabezado del proyecto una vez seleccionada la opción.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProyectoSelectedItemChanged(object sender, EventArgs e)
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

        //Método para cargar el grid de acuerdo a los datos seleccionados
        protected void btnMOstrar_Click(object sender, EventArgs e)
        {
            PatioID = ddlPatio.SelectedValue.SafeIntParse();
            ProyectoID = ddlProyecto.SelectedValue.SafeIntParse();
            FechaInicial = dtpDesde.SelectedDate;
            FechaFinal = dtpHasta.SelectedDate;

            EstablecerDataSource();
            grdRecepcion.DataBind();
            grdRecepcion.Visible = true;
        }

        /// <summary>
        /// Va a la capa de negocios para obtener la información requerida y la asigna al grid
        /// </summary>
        private void EstablecerDataSource()
        {
            //proyectos a los cuales tiene permiso el usuario
            int [] pids = UserScope.MisProyectos.Select(x => x.ID).ToArray();

            grdRecepcion.DataSource = RecepcionBO.Instance
                                                 .ObtenerConFiltros(PatioID, ProyectoID, FechaInicial, FechaFinal, pids)
                                                 .OrderBy(x => x.FechaRecepcion)
                                                 .ThenBy(x => x.RecepcionID);


        }

        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            EstablecerDataSource();
        }

        protected void grdRecepcion_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        /// <summary>
        /// Método que genera el URL para edición de la recepción.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdRecepcion_ItemDataBound(object source, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                HyperLink edita = e.Item.FindControl("hypEditar") as HyperLink;
                GrdRecepcion recepcion = (GrdRecepcion)e.Item.DataItem;
                edita.NavigateUrl = String.Format("/Materiales/DetRecepcion.aspx?ID={0}", recepcion.RecepcionID);
            }
        }

        /// <summary>
        /// Método que elimina un registro del grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdRecepcion_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                try
                {
                    int recepcionID = e.CommandArgument.SafeIntParse();
                    RecepcionBO.Instance.BorraRecepcion(recepcionID);
                    EstablecerDataSource();
                    grdRecepcion.DataBind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }               

            }
        }

     
    }
}