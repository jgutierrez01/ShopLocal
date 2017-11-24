using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using Mimo.Framework.WebControls;
using SAM.BusinessLogic.Cruce;
using SAM.BusinessLogic.Produccion;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Produccion;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Cache;
using SAM.Entities.Grid;
using SAM.Entities.Personalizadas;
using SAM.Entities.Reportes;
using SAM.Entities;
using SAM.Web.Classes;
using SAM.Web.Common;
using Telerik.Web.UI;

namespace SAM.Web.Produccion
{
    public partial class OrdenTrabajoEspecial : SamPaginaPrincipal
    {
        /// <summary>
        /// Variable de ViewState que contiene un arreglo con los IDs de los spools que se
        /// pueden fabricar, inicialmente esto lo determina el proceso de cruce, conforme
        /// se van generando ODTs los IDs de los spools que ya se fueron a ODT se van
        /// quitando de esta lista.
        /// </summary>
        private int[] SpoolsFabricables
        {
            get
            {
                return (int[])ViewState["SpoolsFabricables"];
            }
            set
            {
                ViewState["SpoolsFabricables"] = value;
            }
        }

        private int ProyectoID
        {
            get
            {
                return (int)ViewState["ProyectoID"];
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }

        }

        private Stopwatch sw = new Stopwatch();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int _proyectoID = Request.QueryString["PID"].SafeIntParse();
                if (_proyectoID > 0)
                {
                    Cargar(_proyectoID);
                }
            }
        }

        protected void Cargar(int proyectoId)
        {
            CruceProyecto cruce = new CruceProyecto(proyectoId);

            List<FaltanteCruce> faltantes;
            List<CondensadoItemCode> condensado;

            SpoolsFabricables = cruce.ProcesaSpoolRevision(out faltantes, out condensado)
                                     .Select(x => x.SpoolID)
                                     .ToArray();

            EstablecerDataSource();
            grdSpools.DataBind();
            grdSpools.Visible = true;
            filtroGenerico.ProyectoSelectedValue = proyectoId.ToString();

        }

        protected void btnMostrar_Click(object sender, EventArgs e)
        {
           
            ProyectoID = filtroGenerico.ProyectoSelectedValue.SafeIntParse();
            EstablecerDataSourceAntesDeCruce(ProyectoID);
            grdSpoolsCompleto.DataBind();
            grdSpoolsCompleto.Visible = true;

        }

        protected void grdSpools_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        private void EstablecerDataSource(bool incluyeHold = false)
        {
            //grdSpools.DataSource = SpoolBO.Instance.ObtenerIngPorProyecto(filtroGenerico.ProyectoSelectedValue.SafeIntParse(), true);
            List<GrdCruce> DataSource = SpoolBO.Instance.ObtenerDespuesDeCruce(SpoolsFabricables, incluyeHold);
            grdSpools.DataSource = DataSource;

        }

        private void EstablecerDataSourceAntesDeCruce(int ProyectoID)
        {
            List<GrdCruce> dataSource = SpoolBO.Instance.ObtenerAntesDeCruceRevisionesEspeciales(ProyectoID);
            grdSpoolsCompleto.DataSource = dataSource;
        }

        private void configurarColumnasProyecto()
        {
            ProyectoCache p = CacheCatalogos.Instance.ObtenerProyectos().Where(x => x.ID == filtroGenerico.ProyectoSelectedValue.SafeIntParse()).Single();

            if (p.ColumnasNomenclatura > 0)
            {
                foreach (NomenclaturaStruct n in p.Nomenclatura)
                {
                    GridColumn col = grdSpools.MasterTableView.Columns.FindByUniqueName("Segmento" + n.Orden);
                    col.HeaderText = n.NombreColumna;
                    col.Visible = true;
                    col.HeaderStyle.Width = Unit.Pixel(100);
                    col.FilterControlWidth = Unit.Pixel(60);
                }
            }
        }

        protected void recargaSpools()
        {
            //Traer los elementos seleccionados a través del API de telerik
            GridDataItem[] items = grdSpools.MasterTableView.GetSelectedItems();

            //Obtener los ids de los spools seleccionados
            int[] ids = items.Select(x => x.GetDataKeyValue("SpoolID").SafeIntParse()).ToArray();

            //quita los spools que ya se enviaron a ODT
            SpoolsFabricables = SpoolsFabricables.Except(SpoolsFabricables.Where(x => ids.Contains(x))).ToArray();

            //Actualiza la información del grid
            EstablecerDataSource();
            grdSpools.ResetBind();
            grdSpools.DataBind();
        }

        protected void grdSpools_ItemCreated(object sender, GridItemEventArgs e)
        {
            GridCommandItem commandItem = e.Item as GridCommandItem;

            if (commandItem != null)
            {
                HyperLink lnkOdt = (HyperLink)commandItem.FindControl("lnGenerarRequisición");
                HyperLink imgOdt = (HyperLink)commandItem.FindControl("imgAgregarOrden");

                string jsLink = string.Format("javascript:Sam.Produccion.AbrePopUpTaller('{0}','{1}','{2}','{3}');",
                                                grdSpools.ClientID,
                                                CultureInfo.CurrentCulture.Name,
                                                rdwTalleres.ClientID,
                                                filtroGenerico.ProyectoSelectedValue);

                lnkOdt.NavigateUrl = jsLink;
                imgOdt.NavigateUrl = jsLink;
            }
        }

        protected void btnQuitaSeleccionados_Click(object sender, EventArgs e)
        {
            recargaSpools();
        }

        protected void lnkMostrarConCruce_Click(object sender, EventArgs e)
        {
            CruceProyecto cruce = new CruceProyecto(ProyectoID);

            List<FaltanteCruce> faltantes;
            List<CondensadoItemCode> condensado;

            //Lleva a cabo el proceso de cruce, guarda en ViewState los ids de aquellos spools
            //que si se pueden cruzar. Posteriormente del Grid utiliza este arreglo para determinar que spools
            //se muestran en el grid.

            SpoolsFabricables = cruce.ProcesaSpoolRevision(out faltantes, out condensado)
                                     .Select(x => x.SpoolID)
                                     .ToArray();


            EstablecerDataSource();
            grdSpools.DataBind();
            grdSpools.Visible = true;
        }

    }
}