using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessObjects;
using SAM.Entities;
using SAM.BusinessObjects.Administracion;
using Mimo.Framework.Data;

namespace SAM.Web.Administracion
{
    public partial class LstUsuario : SamPaginaPrincipal
    {
        /// <summary>
        /// Carga el listado en la primera página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.adm_Usuarios);
                EstablecerDataSource();
                grdUsuarios.DataBind();
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
        protected void grdUsuarios_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        /// <summary>
        /// Va a la capa de negocios para obtener la información requerida y la asigna al grid
        /// </summary>
        /// <param name="tamanoPagina">Tamaño de página seleccionado por el grid</param>
        /// <param name="indicePagina">Página que debe recuperarse</param>
        private void EstablecerDataSource()
        {
            grdUsuarios.DataSource = UsuarioBO.Instance.ObtenerTodosConPerfil()
                                                       .OrderBy(x => x.ApPaterno)
                                                       .ThenBy(x => x.Nombre);
        }

        protected void lnkActualizar_OnClick(object sender, EventArgs e)
        {
            grdUsuarios.ResetBind();
            grdUsuarios.Rebind();
        }
    }
}