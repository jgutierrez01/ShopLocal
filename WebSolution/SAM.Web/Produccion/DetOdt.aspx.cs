using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.Web.Common;
using Telerik.Web.UI;
using SAM.Entities.Grid;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Produccion;
using Mimo.Framework.Exceptions;
using SAM.Entities;
using SAM.Entities.Cache;
using SAM.BusinessLogic.Produccion;
using SAM.BusinessObjects.Catalogos;
using System.Data;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessLogic.Ingenieria;

namespace SAM.Web.Produccion
{
    /// <summary>
    /// Muestra el detalle de una orden de trabajo en particular.
    /// Lo que se muestra son los spools que pertenecen a la misma así como los datos generales de la orden de trabajo.
    /// </summary>
    public partial class DetOdt : SamPaginaPrincipal
    {
        #region Campos Privados

        private const string LETRA_BASTON_MANUAL = "MAN";
        private Dictionary<int, bool> DetalleSpool
        {
            get
            {
                if (ViewState["DetalleSpool"] != null)
                {
                    return (Dictionary<int, bool>)ViewState["DetalleSpool"];
                }
                return new Dictionary<int, bool>();
            }
            set
            {
                ViewState["DetalleSpool"] = value;
            }
        }

        /// <summary>
        /// JS de confirmación para actualizar un bastón
        /// </summary>
        private const string JS_ACTUALIZA_BASTON = "return Sam.Confirma(50)";
        /// <summary>
        /// JS de confirmación para eliminar un spool de una ODT
        /// </summary>
        private const string JS_BORRAR = "return Sam.Confirma(4,'{0}');";
        /// <summary>
        /// JS para abrir el popup con el detalle del spool en modo sólo lectura
        /// </summary>
        private const string JS_POPUP_SPOOL = "javascript:Sam.Produccion.AbrePopupSpoolOdtRO('{0}');";
        /// <summary>
        /// JS de confirmación para que el usuario confirme si quiere llevar a cabo la reingeniería del spool
        /// </summary>
        private const string JS_REINGENIERIA = "return Sam.Confirma(5,'{0}');";
        /// <summary>
        /// Indica si la ODT es asignada o no, en base a esto generamos el link para ver a que página mandamos al usuario
        /// cuando quiera agregar un spool manualmente a la ODT.
        /// </summary>
        private bool EsAsignada
        {
            get
            {
                return (bool)ViewState["EsAsignada"];
            }
            set
            {
                ViewState["EsAsignada"] = value;
            }
        }
        #endregion

        /// <summary>
        /// Carga los controles de la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAOrdenDeTrabajo(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar a una ODT {1} para la cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.prod_CruceForzado);
                cargaControles();
            }
        }

        #region Métodos
        /// <summary>
        /// Obtiene la orden de trabajo de la base de datos y la utiliza para llenar el encabezado de
        /// la página.
        /// </summary>
        private void cargaControles()
        {
            OrdenTrabajo odt = OrdenTrabajoBO.Instance.Obtener(EntityID.Value);
            ProyectoCache p = UserScope.MisProyectos.Where(x => x.ID == odt.ProyectoID).Single();

            EsAsignada = odt.EsAsignado;

            // lblProyecto.Text = p.Nombre;
            lblOdt.Text = odt.NumeroOrden;
            dtpFecha.SelectedDate = odt.FechaOrden;
            //lblPatio.Text = p.NombrePatio;

            ddlTaller.BindToEntiesWithEmptyRow(UserScope.TalleresPorProyecto(p.ID));
            ddlTaller.SelectedValue = odt.TallerID.ToString();
            ddlEstatus.SelectedValue = odt.EstatusOrdenID.ToString();

            proyEncabezado.BindInfo(odt.ProyectoID);

            VersionRegistro = odt.VersionRegistro;
            titulo.NavigateUrl += "?PID=" + p.ID;
        }
        /// <summary>
        /// Va a la BD por los spools de una orden de trabajo en particular y los orden por partida.
        /// Asigna el resultado al datasource del grid.
        /// </summary>
        public void EstablecerDataSource()
        {
            grdSpools.DataSource = OrdenTrabajoBO.Instance
                                                 .ObtenerSpoolsPorOdt(EntityID.Value)
                                                 .OrderBy(x => x.Partida);
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Guarda únicamente los cambios a los datos del encabezado de la ODT.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Validate("vgOdt");

            if (IsValid)
            {
                OrdenTrabajo odt = OrdenTrabajoBO.Instance.Obtener(EntityID.Value);
                odt.VersionRegistro = VersionRegistro;

                odt.StartTracking();

                odt.EstatusOrdenID = ddlEstatus.SelectedValue.SafeIntParse();
                odt.FechaOrden = dtpFecha.SelectedDate.Value;
                odt.TallerID = ddlTaller.SelectedValue.SafeIntParse();
                odt.UsuarioModifica = SessionFacade.UserId;
                odt.FechaModificacion = DateTime.Now;

                odt.StopTracking();

                try
                {
                    OrdenTrabajoBO.Instance.Guarda(odt);
                    Response.Redirect(WebConstants.ProduccionUrl.ListaOrdenesDeTrabajo);
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve, "vgOdt");
                }
            }
        }

        /// <summary>
        /// Limpiar filtros, sortings y paginación del grid y volver a cargar sus datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void lnkActualizar_Click(object sender, EventArgs e)
        {
            grdSpools.ResetBind();
            grdSpools.Rebind();
        }

        public void lnkNuevaVersion_Click(object sender, EventArgs e)
        {
            OrdenTrabajo odt = OrdenTrabajoBO.Instance.Obtener(EntityID.Value);
            byte[] reporteODT = null;
            reporteODT = UtileriasReportes.ObtenReporteOdt(EntityID.Value, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true);
            IngenieriaBL.Instance.ActualizarOrdenTrabajo(EntityID.Value, reporteODT, SessionFacade.UserId);

        }

        /// <summary>
        /// Se dispara cuando el grid de telerik detecta que necesita actualizar su datasource, puede ser
        /// por paginación, filtering y otros.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void grdSpools_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        /// <summary>
        /// Se dispara cada vez que se hace el binding a un elemento del repeater que contiene
        /// los spools de la orden de trabajo.  Utilizamos este método para configurar los botones
        /// de acción tal como reinigeniería, eliminar y ver detalle del spool.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSpools_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem && (e.Item.DataItem is GrdSpoolOdt) && e.Item.IsItem())
            {
                GridDataItem item = (GridDataItem)e.Item;
                GrdSpoolOdt spOdt = (GrdSpoolOdt)item.DataItem;

                int spoolID = item.GetDataKeyValue("SpoolID").SafeIntParse();

                if (spoolID != -1)
                {
                    //Configurar el botón de borrar/eliminar
                    ImageButton imgBorrar = (ImageButton)item["borrar_h"].FindControl("imgBorrar");
                    imgBorrar.CommandArgument = spOdt.OrdenTrabajoSpoolID.ToString();
                    imgBorrar.OnClientClick = string.Format(JS_BORRAR, spOdt.NumeroControl);

                    //Configurar la liga para ver del detalle del spool
                    HyperLink hlSpool = (HyperLink)item["nombreSpool"].FindControl("hlSpool");
                    hlSpool.Text = spOdt.NombreSpool;
                    hlSpool.NavigateUrl = string.Format(JS_POPUP_SPOOL, spOdt.OrdenTrabajoSpoolID);

                    if (spOdt.DifiereDeIngenieria)
                    {
                        //Mostrar el botón de reingeniería con su JS de confirmación y el comando server-side con su valor
                        ImageButton imgAdvertencia = (ImageButton)item["DifiereDeIngenieria"].FindControl("imgAdvertencia");
                        imgAdvertencia.Visible = true;
                        imgAdvertencia.OnClientClick = string.Format(JS_REINGENIERIA, spOdt.NumeroControl);
                        imgAdvertencia.CommandArgument = spOdt.OrdenTrabajoSpoolID.ToString();
                        imgAdvertencia.CommandName = "reingenieria";
                    }
                    else if (spOdt.FueReingenieria)
                    {
                        Image imgFueReingenieria = (Image)item["DifiereDeIngenieria"].FindControl("imgFueReingenieria");
                        imgFueReingenieria.Visible = true;
                    }
                }

                // Ocultamos boton de expandir/colapsar para aquellos spools que no cuenten con bastones
                if (!SpoolBO.Instance.TieneBastones(spoolID))
                {
                    e.Item.Controls.IterateRecursively(c =>
                    {
                        if (!string.IsNullOrEmpty(c.ID) &&
                            c.ID.IndexOf("BtnExpandColumn") != -1)
                        {
                            c.Visible = false;
                        }
                    });
                }
            }


            // Configuramos los controles para el modo edición y el modo no edición
            if (e.Item.DataItem is GrdBaston && e.Item.IsInEditMode)
            {
                GridEditableItem item = (GridEditableItem)e.Item;
                int spoolID = DataBinder.Eval(e.Item.DataItem, "SpoolID").SafeIntParse();
                string letraBaston = DataBinder.Eval(e.Item.DataItem, "LetraBaston").ToString();
                string nombreEstacion = DataBinder.Eval(e.Item.DataItem, "Estacion").ToString();

                int bastonTallerID = BastonBO.Instance.ObtenerPorSpool(spoolID)
                                                      .Single(x => x.LetraBaston == letraBaston)
                                                      .TallerID.SafeIntParse();
                List<Estacion> estaciones = EstacionBO.Instance.ObtenerPorTaller(bastonTallerID).ToList();
                var comboEstaciones = (from es in estaciones
                                       select new
                                       {
                                           EstacionID = es.EstacionID,
                                           Nombre = string.Format("{0}{1}", es.Taller.Nombre, es.Nombre)
                                       }).ToList();
                Estacion estacion = estaciones.FirstOrDefault(x => string.Format("{0}{1}", x.Taller.Nombre, x.Nombre) == nombreEstacion);

                ImageButton btnActualizar = (ImageButton)((GridDataItem)e.Item)["EditColumn"].Controls[0];
                btnActualizar.OnClientClick = JS_ACTUALIZA_BASTON;

                HiddenField hfNombreBaston = (HiddenField)item["EstacionEdit"].Controls[3];
                hfNombreBaston.Value = letraBaston;

                RadComboBox rcbEstacion = (RadComboBox)item["EstacionEdit"].Controls[1];
                rcbEstacion.DataSource = comboEstaciones;
                rcbEstacion.DataTextField = "Nombre";
                rcbEstacion.DataValueField = "EstacionID";
                rcbEstacion.DataBind();

                if (estacion != null)
                {
                    rcbEstacion.SelectedValue = estacion.EstacionID.ToString();
                }
            }
            if (e.Item.DataItem is GrdBaston & !e.Item.IsInEditMode)
            {
                GridDataItem item = (GridDataItem)e.Item;
                LiteralControl litEstacion = (LiteralControl)item["EstacionEdit"].Controls[0];
                litEstacion.Text = DataBinder.Eval(e.Item.DataItem, "Estacion").ToString();
            }
        }

        /// <summary>
        /// Se dispara después del databind del grid padre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSpools_OnDetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            // Obtenemos el spoolID y cargamos el subgrid con los bastones del spool
            GridDataItem parentItem = e.DetailTableView.ParentItem as GridDataItem;
            int spoolID = parentItem.GetDataKeyValue("SpoolID").SafeIntParse();

            List<GrdBaston> bastones = BastonBO.Instance.ObtenerGrdBastonPorSpool(spoolID)
                                                        .OrderBy(x => x.LetraBaston == "MAN")
                                                        .ThenBy(x => x.LetraBaston).ToList();
            e.DetailTableView.DataSource = bastones;

            // Ocultamos grid si no existen elementos o si solo cuenta con juntas manuales
            if (!bastones.Any() || (bastones.Count == 1 && bastones.Any(x => x.LetraBaston == LETRA_BASTON_MANUAL)))
            {
                e.DetailTableView.ParentItem.Expanded = false;
            }

            // Asignamos el valor actual del viewstate
            if(DetalleSpool.Any(x => x.Key == spoolID))
            {
                e.DetailTableView.ParentItem.Expanded = DetalleSpool[spoolID];
            }
        }

        /// <summary>
        /// Se dispara una vez que la fila ha sido aprobada para su actualización
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSpools_OnUpdateCommand(object sender, GridCommandEventArgs e)
        {
            if ((e.Item is GridEditableItem) && (e.Item.IsInEditMode))
            {
                GridEditableItem item = (GridEditableItem)e.Item;

                int spoolID = e.Item.OwnerTableView.ParentItem.GetDataKeyValue("SpoolID").SafeIntParse();
                int? estacionID = ((RadComboBox)item["EstacionEdit"].Controls[1]).SelectedValue.SafeIntParse();
                string nombreBaston = ((HiddenField)item["EstacionEdit"].Controls[3]).Value;

                BastonSpool bastonSpool = BastonBO.Instance.ObtenerPorSpool(spoolID).FirstOrDefault(x => x.LetraBaston == nombreBaston);
                //Actualizamos BastonSpool
                bastonSpool.StartTracking();
                bastonSpool.EstacionID = estacionID != -1 ? estacionID : null;
                bastonSpool.UsuarioModifica = SessionFacade.UserId;
                bastonSpool.FechaModificacion = DateTime.Now;
                bastonSpool.StopTracking();

                BastonBO.Instance.Guarda(bastonSpool);

                e.Item.Edit = false;
                grdSpools.MasterTableView.Rebind();
            }
        }

        /// <summary>
        /// Se dispara cuando el usuario hace click ya sea en la acción de borrar o de reingeniería
        /// para un spool en particular.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSpools_ItemCommand(object sender, GridCommandEventArgs e)
        {
            //Obtener el ID del registro OrdenTrabajoSpool que pasan como argumento
            int ordenTrabajoSpoolID = e.CommandArgument.SafeIntParse();

            try
            {
                switch (e.CommandName)
                {
                    case "borrar":
                        //Eliminar el spool de la ODT
                        OrdenTrabajoBO.Instance.EliminaSpool(ordenTrabajoSpoolID, SessionFacade.UserId);
                        break;
                    case "reingenieria":
                        //Llevar a cabo el proceso de reingeniería para el spool seleccionado
                        OrdenTrabajoBL.Instance.Reingenieria(ordenTrabajoSpoolID, SessionFacade.UserId);
                        break;
                    case "Edit":
                        // Habilitamos el modo edición de la fila
                        e.Item.Edit = true;
                        break;
                    case "Cancel":
                        // Deshabilitamos el modo edición de la fila
                        e.Item.Edit = false;
                        break;
                    case "ExpandCollapse":
                        // Para no perder el estado de la tabla por el rebind del grid padre,
                        //guardamos la información en viewstate
                        int spoolID = ((GridDataItem)e.Item).GetDataKeyValue("SpoolID").SafeIntParse();
                        var _detalleSpool = DetalleSpool;
                        if (!DetalleSpool.Any(x => x.Key == spoolID))
                        {
                            _detalleSpool.Add(spoolID, true);
                            DetalleSpool = _detalleSpool;
                        }
                        else
                        {
                            DetalleSpool[spoolID] = !DetalleSpool[spoolID];
                        }
                        break;
                    default:
                        break;
                }

                //Actualizar el grid
                if (e.CommandName != "Edit")
                {
                    grdSpools.Rebind();
                }
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve, "vgSpools");
            }
        }

        /// <summary>
        /// Configurar el URL desde el cual se agregan spools a la ODT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSpools_ItemCreated(object sender, GridItemEventArgs e)
        {
            GridCommandItem commandItem = e.Item as GridCommandItem;

            if (commandItem != null)
            {
                HyperLink lnkAgregar = (HyperLink)commandItem.FindControl("lnkAgregar");
                HyperLink imgAgregar = (HyperLink)commandItem.FindControl("imgAgregar");

                string url = EsAsignada ? string.Format(WebConstants.ProduccionUrl.AgregarSpoolOdtAsignado, EntityID.Value)
                                        : string.Format(WebConstants.ProduccionUrl.AgregarSpoolOdt, EntityID.Value);

                lnkAgregar.NavigateUrl = url;
                imgAgregar.NavigateUrl = url;
            }
        }
        #endregion
    }
}
