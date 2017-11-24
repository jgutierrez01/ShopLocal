using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Produccion;
using SAM.Entities;
using SAM.Web.Classes;
using SAM.Web.Common;
using Telerik.Web.UI;
using SAM.Entities.Grid;
using Mimo.Framework.Exceptions;
using System.Web;

namespace SAM.Web.Administracion
{
    public partial class DetEstimacion : SamPaginaPrincipal
    {
        /// <summary>
        /// toma el QueryString de la estimacion Id y lo manda al metodo carga
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAEstimacion(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar una estimación {1} para la cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.adm_Estimaciones);
                carga(EntityID.Value);
                
            }
        }

        /// <summary>
        /// Carga los labels necesarios para mostrar la estimacion seleccionada
        /// </summary>
        /// <param name="estimacionID">estimacion Id</param>
        private void carga(int estimacionID)
        {
            Estimacion estimacion = EstimacionBO.Instance.ObtenerConProyectoYDetalle(estimacionID);


            proyHeader.BindInfo(estimacion.ProyectoID);
            lblFechaEstimacion.Text = estimacion.FechaEstimacion.ToString("d");
            lblNumeroEstimacion.Text= estimacion.NumeroEstimacion;
           // lblProyecto.Text = UserScope.MisProyectos.Where(x => x.ID == estimacion.ProyectoID).Single().Nombre;
            EstablecerDataSourceJunta(estimacionID);
            EstablecerDataSourceSpool(estimacionID);
            grdEstimacionJunta.DataBind();
            grdEstimacionSpool.DataBind();
        }

        /// <summary>
        /// Usamos este método para configurar dinámicamente los links del header del grid
        /// </summary>
        protected void grdEstimacionJunta_ItemCreated(object sender, GridItemEventArgs e)
        {
            GridCommandItem commandItem = e.Item as GridCommandItem;

            if (commandItem != null)
            {
                HyperLink lnkExportarExcelJunta = (HyperLink)commandItem.FindControl("hlExporta");
                HyperLink hlExportaImagen = (HyperLink)commandItem.FindControl("hlExportaImagen");

                string url = string.Format( WebConstants.ProduccionUrl.ExportaExcel, 
                                            EntityID.Value, 
                                            (int)TipoArchivoExcel.EstimacionJuntas);

                lnkExportarExcelJunta.NavigateUrl = url;
                hlExportaImagen.NavigateUrl = url;
            }
        }

        /// <summary>
        /// Usamos este método para configurar dinámicamente los links del header del grid
        /// </summary>
        protected void grdEstimacionSpool_ItemCreated(object sender, GridItemEventArgs e)
        {
            GridCommandItem commandItem = e.Item as GridCommandItem;

            if (commandItem != null)
            {
                HyperLink lnkExportarExcelSpool = (HyperLink)commandItem.FindControl("hlExportarSool");
                HyperLink hlExportaImagen = (HyperLink)commandItem.FindControl("hlExportaImagenSpool");

                string url = string.Format(WebConstants.ProduccionUrl.ExportaExcel,
                                            EntityID.Value,
                                            (int)TipoArchivoExcel.EstimacionSpools);

                lnkExportarExcelSpool.NavigateUrl = url;
                hlExportaImagen.NavigateUrl = url;
            }
        }

        /// <summary>
        /// llena el grdEstimacionJunta de la estimacion mandada de la pagina anterior
        ///  asi como establece el total de juntas en el encabezado
        /// </summary>
        /// <param name="estimacionID">estimacion Id</param>
        private void EstablecerDataSourceJunta(int estimacionID)
        {
            List<GrdEstimacionJunta> lst =  EstimacionBO.Instance.ObtenerEstimacionJuntaPorEstimacionID(estimacionID);
            grdEstimacionJunta.DataSource = lst;
            lblTotalJuntas.Text = lst.Count.ToString();
        }

        /// <summary>
        /// llena el grdEstimacionSpool de la estimacion mandada de la pagina anterior
        ///  asi como establece el total de Spools en el encabezado
        /// </summary>
        /// <param name="estimacionID">estimacion Id</param>
        private void EstablecerDataSourceSpool(int estimacionID)
        {
            grdEstimacionSpool.DataSource = EstimacionBO.Instance.ObtenerEstimacionSpoolPorEstimacionID(estimacionID);
            lblTotalSpools.Text = ((IEnumerable<object>)grdEstimacionSpool.DataSource).Count().ToString();
        }
        
        /// <summary>
        /// se llena el grid con la informacion requerida
        /// </summary>
        protected void grdEstimacionJunta_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            //se manda el ID de la estimacion para buscar el total de juntas relacionadas
            EstablecerDataSourceJunta(EntityID.Value);
        }

        /// <summary>
        /// se llena el grid con la informacion requerida
        /// </summary>
        protected void grdEstimacionSpool_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            //se manda el ID de la estimacion para buscar el total de spools relacionados
            EstablecerDataSourceSpool(EntityID.Value);
        }

        /// <summary>
        /// llama al metodo BorrarSeleccionJunta
        /// </summary>
        protected void imeBorrarSeleccionJunta_onClick(object sender, ImageClickEventArgs e)
        {
            BorrarSeleccionJunta();
        }

        /// <summary>
        /// llama al metodo BorrarSeleccionJunta
        /// </summary>
        protected void lnkBorrarSeleccionJunta_onClick(object sender, EventArgs e)
        {
            BorrarSeleccionJunta();
        }

        /// <summary>
        /// Elimina todas las juntas seleccionadas con el checkBox
        /// </summary>
        private void BorrarSeleccionJunta()
        {
           
            GridDataItem[] items = grdEstimacionJunta.MasterTableView.GetSelectedItems();

            int[] ids = items.Select(x => x.GetDataKeyValue("EstimadoJuntaID").SafeIntParse()).ToArray();

            try
            {
                 EstimacionBO.Instance.BorrarJuntasSeleccionados(ids);
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
            grdEstimacionJunta.Rebind();
        }

        /// <summary>
        /// llama al metodo BorrarSeleccionSpool
        /// </summary>
        protected void imeBorrarSeleccionSpool_onClick(object sender, ImageClickEventArgs e)
        {
            BorrarSeleccionSpool();
        }

        /// <summary>
        /// llama al metodo BorrarSeleccionSpool
        /// </summary>
        protected void lnkBorrarSeleccionSpool_onClick(object sender, EventArgs e)
        {
            BorrarSeleccionSpool();
        }

        /// <summary>
        /// Elimina todos los spools seleccionadas con el checkBox
        /// </summary>
        private void BorrarSeleccionSpool()
        {

            GridDataItem[] items = grdEstimacionSpool.MasterTableView.GetSelectedItems();

            int[] ids = items.Select(x => x.GetDataKeyValue("EstimacionSpoolID").SafeIntParse()).ToArray();

            try
            {
                EstimacionBO.Instance.BorrarSpoolsSeleccionados(ids);
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
            grdEstimacionSpool.Rebind();
        }
    }
}
