using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.Web.Common;
using Telerik.Web.UI;

namespace SAM.Web.Proyectos
{
    public partial class Coladas : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAProyecto(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar coladas para un proyecto {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.proy_Coladas, EntityID.Value);
                headerProyecto.BindInfo(EntityID.Value);
                EstablecerDataSource();
                grdColadas.DataBind();
            }
        }

        /// <summary>
        /// establece la fuente de información para el grid.
        /// obtiene todos los item codes para cierto proyecto.
        /// </summary>
        private void EstablecerDataSource()
        {
            grdColadas.DataSource = ColadasBO.Instance.ObtenerConFabricanteYAceroPorProyecto(EntityID.Value).OrderBy(x => x.NumeroColada);
        }

        /// <summary>
        /// establece la fuente de información en caso de que se necesite recargar.
        /// se genera con eventos del grid de telerik
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdColadas_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        protected void grdColadas_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                GridCommandItem commandItem = e.Item as GridCommandItem;
                HyperLink hlAgregar = commandItem.FindControl("hlAgregar") as HyperLink;
                HyperLink imgAgregar = commandItem.FindControl("imgAgregar") as HyperLink;
                HyperLink hlRegTxt = commandItem.FindControl("hlRegTxt") as HyperLink;
                HyperLink hlRegresar = commandItem.FindControl("hlRegresar") as HyperLink;

                hlRegTxt.NavigateUrl = string.Format(WebConstants.ProyectoUrl.DET_PROYECTO, EntityID.Value);
                hlRegresar.NavigateUrl = string.Format(WebConstants.ProyectoUrl.DET_PROYECTO, EntityID.Value);
                hlAgregar.NavigateUrl = string.Format(WebConstants.ProyectoUrl.DET_COLADAS_AGREGAR, EntityID.Value);
                imgAgregar.NavigateUrl = string.Format(WebConstants.ProyectoUrl.DET_COLADAS_AGREGAR, EntityID.Value);
            }
        }

        protected void grdColadas_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int coladaID = e.CommandArgument.SafeIntParse();
                try
                {
                    ColadasBO.Instance.Borra(coladaID);
                    EstablecerDataSource();
                    grdColadas.Rebind();
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }
        }

        protected void lnkActualizar_OnClick(object sender, EventArgs e)
        {
            grdColadas.ResetBind();
            grdColadas.Rebind();
        }
    }
}