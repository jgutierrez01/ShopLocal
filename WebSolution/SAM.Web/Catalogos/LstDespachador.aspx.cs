using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Data;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessLogic;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Excepciones;
using Mimo.Framework.Exceptions;
using Telerik.Web.UI;
using SAM.BusinessObjects.Administracion;
using SAM.Web.Common;

namespace SAM.Web.Catalogos
{
    public partial class LstDespachador : SamPaginaPrincipal
    {
        private bool permiso = false;
        public bool PermisoDetalleDespachadores
        {
            get
            {
                return permiso;
            }
            set
            {
                permiso = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PermisoDetalleDespachadores = UsuarioBO.Instance.ObtenerPermisoDetalleDespachadores(SessionFacade.PerfilID);
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Despachador);
                EstablecerDataSource();
                grdDespachador.DataBind();
            }
        }

        /// <summary>
        /// Se dispara cuando el grid debe vover a recalcular su contenido debido a eventos como los siguientes:
        /// + Paginación
        /// + Ordenamiento
        /// + Filtros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDespachador_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        /// <summary>
        /// Obtiene la lista de tuberos desde TuberoBO incluyendo los patios
        /// </summary>
        private void EstablecerDataSource()
        {
            grdDespachador.DataSource = DespachadorBO.Instance.ObtenerTodosConPatio();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDespachador_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int despachadorID = e.CommandArgument.SafeIntParse();
                try
                {
                    DespachadorBO.Instance.Borra(despachadorID);
                    EstablecerDataSource();
                    grdDespachador.DataBind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdDespachador_ItemDataBound(object source, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                HyperLink edita = e.Item.FindControl("hypEditar") as HyperLink;
                //GridDataItem dataItem = (GridDataItem)e.Item;
                SAM.Entities.Despachador despachador = (SAM.Entities.Despachador)e.Item.DataItem;
                int idDespachador = despachador.DespachadorID;//dataItem["TuberoID"].Text.SafeIntParse();
                edita.NavigateUrl = String.Format("/Catalogos/DetDespachador.aspx?ID={0}", idDespachador);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdDespachador.ResetBind();
            EstablecerDataSource();
            grdDespachador.DataBind();
        }
    }
}