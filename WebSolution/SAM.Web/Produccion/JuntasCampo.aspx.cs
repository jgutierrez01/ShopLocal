using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Catalogos;
using SAM.Web.Common;
using Telerik.Web.UI;
using Mimo.Framework.Exceptions;
using SAM.Web.Classes;

namespace SAM.Web.Produccion
{
    public partial class JuntasCapo : SamPaginaPrincipal
    {
        #region Variables privadas página

        private int ProyectoID
        {
            get
            {
                if (ViewState["ProyectoID"] == null)
                {
                    ViewState["ProyectoID"] = -1;
                }
                return ViewState["ProyectoID"].SafeIntParse();
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }
        }

        private int OrdenTrabajoSpoolID
        {
            get
            {
                if (ViewState["OrdenTrabajoSpoolID"] == null)
                {
                    ViewState["OrdenTrabajoSpoolID"] = -1;
                }
                return ViewState["OrdenTrabajoSpoolID"].SafeIntParse();
            }
            set
            {
                ViewState["OrdenTrabajoSpoolID"] = value;
            }
        }

        private int OrdenTrabajoID
        {
            get
            {
                if (ViewState["OrdenTrabajoID"] == null)
                {
                    ViewState["OrdenTrabajoID"] = -1;
                }
                return ViewState["OrdenTrabajoID"].SafeIntParse();
            }
            set
            {
                ViewState["OrdenTrabajoID"] = value;
            }
        }

        private int SpoolID
        {
            get
            {
                if (ViewState["SpoolID"] == null)
                {
                    ViewState["SpoolID"] = -1;
                }
                return ViewState["SpoolID"].SafeIntParse();
            }
            set
            {
                ViewState["SpoolID"] = value;
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMostrarClick(object sender, EventArgs e)
        {
            ProyectoID = filtroGenerico.ProyectoSelectedValue.SafeIntParse();
            OrdenTrabajoSpoolID = filtroGenerico.NumeroControlSelectedValue.SafeIntParse();
            OrdenTrabajoID = filtroGenerico.OrdenTrabajoSelectedValue.SafeIntParse();
            SpoolID = rcbSpool.SelectedValue.SafeIntParse();
            MuestraGrid();
        }

        /// <summary>
        /// 
        /// </summary>
        public void MuestraGrid()
        {
            EstablecerDataSource();
            phSpools.Visible = true;
            grdSpools.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        public void EstablecerDataSource()
        {
            List<GrdJuntaCampo> datasource = JuntaCampoBO.Instance.ObtenerListadoJuntaCampo(ProyectoID, OrdenTrabajoID, OrdenTrabajoSpoolID, SpoolID);
            grdSpools.DataSource = datasource;
        }

        /// <summary>
        /// Se dispara cuando el proyecto seleccionado cambia.  En caso de seleccionar
        /// un proyecto válido se muestra el project header control con sus datos, de lo contrario
        /// se oculta el control.
        /// </summary>
        protected void proyecto_Cambio(object sender, EventArgs e)
        {
            hdnProyectoID.Value = filtroGenerico.ProyectoSelectedValue;

            int proyectoID = filtroGenerico.ProyectoSelectedValue.SafeIntParse();

            if (proyectoID > 0)
            {
                headerProyecto.Visible = true;
                headerProyecto.BindInfo(filtroGenerico.ProyectoSelectedValue.SafeIntParse());
            }
            else
            {
                headerProyecto.Visible = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdSpools_OnItemDataBound(object source, GridItemEventArgs e)
        {
            if (e.Item.DataItem != null)
            {
                GridDataItem item = e.Item as GridDataItem;

                if (item != null)
                {
                    HyperLink edita = item.FindControl("hypEditar") as HyperLink;
                    ImageButton imgCortar = item.FindControl("imgCortar") as ImageButton;
                    ImageButton imgEliminarCorte = item.FindControl("imgEliminarCorte") as ImageButton;
                    
                    GrdJuntaCampo jta = item.DataItem as GrdJuntaCampo;

                    edita.NavigateUrl = string.Format("javascript:Sam.Produccion.AbrePopupJuntasCampo({0});", jta.JuntaSpoolID);

                    if (jta.TieneHold)
                    {
                        //No podemos cortar la junta
                        imgCortar.Visible = false;
                        imgEliminarCorte.Visible = false;
                    }
                    else
                    {
                        //Aquí van los casos interesantes
                        string ladoDerechoEtiqueta = jta.EtiquetaProduccion.Substring(jta.EtiquetaIngenieria.Length);

                        if (ladoDerechoEtiqueta.ContainsIgnoreCase("c") && !ladoDerechoEtiqueta.ContainsIgnoreCase("r") && !jta.ArmadoAprobado)
                        {
                            //Habilitar la opción para que revierta el corte
                            imgEliminarCorte.Visible = true;
                        }
                        else if(jta.ArmadoAprobado)
                        {
                            imgCortar.Visible = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Metodo que se dispara cuando el usuario hace uso de algun comando del grid
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdSpools_ItemCommand(object source, GridCommandEventArgs e)
        {
            int juntaSpoolID = e.CommandArgument.SafeIntParse();

            try
            {
                switch (e.CommandName.ToString().ToLowerInvariant())
                {
                    case "cortar":
                        JuntaCampoBO.Instance.Cortar(juntaSpoolID, SessionFacade.UserId);
                        break;
                    case "eliminar_corte":
                        JuntaCampoBO.Instance.RevertirCorte(juntaSpoolID, SessionFacade.UserId);
                        break;
                }

                grdSpools.Rebind();
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSpools_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }
    }
}