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
    public partial class LstCortador : SamPaginaPrincipal
    {
        private bool permiso = false;
        public bool PermisoDetalleCortadores 
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
                if (SessionFacade.EsAdministradorSistema)
                {
                    PermisoDetalleCortadores = true;
                }
                else
                {

                    PermisoDetalleCortadores = UsuarioBO.Instance.ObtenerPermisoDetalleCortadores(SessionFacade.PerfilID);
                }
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Cortador);
                EstablecerDataSource();
                grdCortador.DataBind();
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
        protected void grdCortador_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        /// <summary>
        /// Obtiene la lista de tuberos desde TuberoBO incluyendo los patios
        /// </summary>
        private void EstablecerDataSource()
        {
            grdCortador.DataSource = CortadorBO.Instance.ObtenerTodosConPatio();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdCortador_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int cortadorID = e.CommandArgument.SafeIntParse();
                try
                {
                    CortadorBO.Instance.Borra(cortadorID);
                    EstablecerDataSource();
                    grdCortador.DataBind();
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
        protected void grdCortador_ItemDataBound(object source, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                HyperLink edita = e.Item.FindControl("hypEditar") as HyperLink;
                //GridDataItem dataItem = (GridDataItem)e.Item;
                SAM.Entities.Cortador cortador = (SAM.Entities.Cortador)e.Item.DataItem;
                int idCortador = cortador.CortadorID;//dataItem["TuberoID"].Text.SafeIntParse();
                edita.NavigateUrl = String.Format("/Catalogos/DetCortador.aspx?ID={0}", idCortador);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdCortador.ResetBind();
            EstablecerDataSource();
            grdCortador.DataBind();
        }
    }
}