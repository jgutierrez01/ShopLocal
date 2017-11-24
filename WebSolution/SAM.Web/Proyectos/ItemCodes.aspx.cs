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
using SAM.BusinessObjects.Proyectos;
using SAM.Entities;

namespace SAM.Web.Proyectos
{
    public partial class ItemCodes : SamPaginaPrincipal
    {
        private string _urlAgregar = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            _urlAgregar = string.Format(WebConstants.ProyectoUrl.DETALLE_ITEM_CODE_AGREGAR, EntityID.Value);

            if (!Page.IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAProyecto(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar item codes de un proyecto {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.proy_ItemCodes, EntityID.Value);
                cargaInformacion(EntityID.Value);
                EstablecerDataSource();
                grdItemCodes.DataBind();
            }
        }


        public void cargaInformacion(int proyectoID)
        {
            headerProyecto.BindInfo(proyectoID);
        }

        /// <summary>
        /// establece la fuente de información para el grid.
        /// obtiene todos los item codes para cierto proyecto.
        /// </summary>
        private void EstablecerDataSource()
        {
            grdItemCodes.DataSource = ItemCodeBO.Instance.ObtenerListaPorProyecto(EntityID.Value).OrderBy(x => x.Codigo);
        }

        /// <summary>
        /// establece la fuente de información en caso de que se necesite recargar.
        /// se genera con eventos del grid de telerik
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdItemCodes_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdItemCodes_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                GridCommandItem commandItem = e.Item as GridCommandItem;
                HyperLink hlAgregar = commandItem.FindControl("hlAgregar") as HyperLink;
                HyperLink imgAgregar = commandItem.FindControl("imgAgregar") as HyperLink;
                HyperLink hlRegTxt = commandItem.FindControl("hlRegTxt") as HyperLink;
                HyperLink hlRegresar = commandItem.FindControl("hlRegresar") as HyperLink;
                HyperLink lnkActualizaPeso = commandItem.FindControl("lnkActualizaPeso") as HyperLink;
                HyperLink imgActualizaPeso = commandItem.FindControl("imgActualizaPeso") as HyperLink;

                hlRegTxt.NavigateUrl = string.Format(WebConstants.ProyectoUrl.DET_PROYECTO,EntityID.Value);
                hlRegresar.NavigateUrl = string.Format(WebConstants.ProyectoUrl.DET_PROYECTO, EntityID.Value);
                lnkActualizaPeso.NavigateUrl = string.Format(WebConstants.ProyectoUrl.PESO_ITEMCODE, EntityID.Value);
                imgActualizaPeso.NavigateUrl = string.Format(WebConstants.ProyectoUrl.PESO_ITEMCODE, EntityID.Value);
                hlAgregar.NavigateUrl = _urlAgregar;
                imgAgregar.NavigateUrl = _urlAgregar;

                if (commandItem != null)
                {
                    HyperLink lnkBajaArchivo = (HyperLink)commandItem.FindControl("lnkBajaArchivo");
                    HyperLink lnkExportaImagen = (HyperLink)commandItem.FindControl("imgBajarArchivo");

                    string url = string.Format(WebConstants.ProyectoUrl.ExportaExcelItemCodePeso,
                                               EntityID.Value,
                                               (int)TipoArchivoExcel.ItemCodePeso
                                               );
                    lnkBajaArchivo.NavigateUrl = url;
                    lnkExportaImagen.NavigateUrl = url;
                }
            }
        } 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdItemCodes_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int itemCodeID = e.CommandArgument.SafeIntParse();
                try
                {
                    ItemCodeBO.Instance.Borra(itemCodeID);
                    grdItemCodes.Rebind();
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }

        }

        protected void lnkActualizar_OnClick(object sender, EventArgs e)
        {
            grdItemCodes.ResetBind();
            grdItemCodes.Rebind();
        }
    }
}