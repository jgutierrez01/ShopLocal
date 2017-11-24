using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.BusinessObjects.Catalogos;
using SAM.Web.Common;
using Telerik.Web.UI;
using Mimo.Framework.Extensions;
using SAM.Web.Classes;
using Mimo.Framework.Exceptions;
using SAM.BusinessLogic.Proyectos;

namespace SAM.Web.Proyectos
{
    public partial class IcEquivalentes : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAProyecto(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar item codes equivalentes para un proyecto {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.proy_ICEquivalentes, EntityID.Value);
                cargaInformacion(EntityID.Value);
                EstablecerDataSource();
                grdIcEquivalentes.DataBind();
            }
        }

        public void cargaInformacion(int proyectoID)
        {
            headerProyecto.BindInfo(proyectoID);
        }

        protected void grdIcEquivalentes_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        protected void grdIcEquivalentes_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
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
                hlAgregar.NavigateUrl = string.Format(WebConstants.ProyectoUrl.DET_IC_EQUIVALENTE_AGREGAR, EntityID.Value);
                imgAgregar.NavigateUrl = string.Format(WebConstants.ProyectoUrl.DET_IC_EQUIVALENTE_AGREGAR, EntityID.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdIcEquivalentes_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int itemCodeEquivalenteID = e.CommandArgument.SafeIntParse();

                try
                {
                    ItemCodeEquivalenteBL.Instance.BorraGrupoDeEquivalencias(itemCodeEquivalenteID, SessionFacade.UserId);
                    grdIcEquivalentes.Rebind();
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }
        }

        protected void EstablecerDataSource()
        {
            grdIcEquivalentes.DataSource = ItemCodeEquivalentesBO.Instance.ObtenerAgrupadosPorProyecto(EntityID.Value);
        }

        protected void grdIcEquivalentes_OnDetailTableDataBind(object sender, Telerik.Web.UI.GridDetailTableDataBindEventArgs e)
        {
            GridDataItem parentItem = e.DetailTableView.ParentItem as GridDataItem;
            int itemCodeEquivalenteID = parentItem.GetDataKeyValue("MinItemCodeEquivalenteID").SafeIntParse();

            e.DetailTableView.DataSource =
                ItemCodeEquivalentesBO.Instance
                                      .ObtenerGrupoDeEquivalencias(itemCodeEquivalenteID)
                                      .OrderBy(x => x.CodigoEq)
                                      .ThenBy(x => x.D1Eq);
        }

        protected void lnkActualizar_OnClick(object sender, EventArgs e)
        {
            grdIcEquivalentes.ResetBind();
            grdIcEquivalentes.Rebind();
        }

    }
}