using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.WebControls;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Ingenieria;
using Telerik.Web.UI;
using SAM.BusinessLogic;
using Mimo.Framework.Exceptions;
using SAM.Entities;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Produccion;
using SAM.Web.Controles.ConfinarSpool;
using SAM.BusinessObjects.Catalogos;
using SAM.Entities.Grid;

namespace SAM.Web.Materiales
{
    public partial class LstConfinarSpool : SamPaginaPrincipal
    {

        #region Filtros
        /// <summary>
        /// ID del proyecto seleccionado en el dropdown
        /// </summary>
        private int _proyecto
        {
            get
            {
                return (int)ViewState["proyecto"];
            }
            set
            {
                ViewState["proyecto"] = value;
            }
        }

        /// <summary>
        /// ID de la orden de trabajo seleccionada
        /// </summary>
        private int _ordenTrabajo
        {
            get 
            {
                return (int)ViewState["ordenTrabajo"];
            }
            set
            {
                ViewState["ordenTrabajo"] = value;
            }
        }

        /// <summary>
        /// ID del número de control seleccionado
        /// </summary>
        private int _numeroControl
        {
            get 
            {
                return (int)ViewState["numeroControl"];
            }
            set
            {
                ViewState["numeroControl"] = value;
            }
        }

        private bool _soloConfinados
        {
            get 
            {
                return (bool)ViewState["soloConfinados"];
            }
            set 
            {
                ViewState["soloConfinados"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.mat_ConfinarSpool);
            }
        }

        protected void grdSpools_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            establecerDataSource();
        }
             
        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            _proyecto = filtroGenerico.ProyectoSelectedValue.SafeIntParse();
            _ordenTrabajo = filtroGenerico.OrdenTrabajoSelectedValue.SafeIntParse();
            _numeroControl = filtroGenerico.NumeroControlSelectedValue.SafeIntParse();
            _soloConfinados = chkConfinados.Checked;

            grdSpools.Visible = true;
            establecerDataSource();
            grdSpools.DataBind();
        }

        protected void grdSpools_OnItemCommand(object source, GridCommandEventArgs e)
        {
            string cmdName = e.CommandName;

            //Sólo si el comando fue expandir/colapser
            if (cmdName == RadGrid.ExpandCollapseCommandName)
            {
                GridDataItem item = e.Item as GridDataItem;

                //Solo vamos por los datos si el usuario decidió expandir el elemento, si decide colapsarlo
                //no vamos a BD
                if (item != null && !item.Expanded)
                {
                    GridNestedViewItem nestedItem = (GridNestedViewItem)item.ChildItem;

                    RadMultiPage rmpDetalle = nestedItem.FindControl("rmpDetalle") as RadMultiPage;

                    if (rmpDetalle != null)
                    {
                        //Hacer los bindings a los controles hijos con la información del spool

                        int spoolID = item.GetDataKeyValue("SpoolID").SafeIntParse();

                        EntityID = spoolID;

                        SAM.Entities.Personalizadas.DetSpool spool = SpoolBO.Instance.ObtenerDetalleCompleto(spoolID);

                        RadPageView rpv = rmpDetalle.FindPageViewByID("rpvMateriales");

                        SAM.Web.Controles.Spool.MaterialRO materiales = (SAM.Web.Controles.Spool.MaterialRO)rpv.FindControl("materiales");
                        materiales.Map(spool.Materiales);

                        rpv = rmpDetalle.FindPageViewByID("rpvJuntas");
                        SAM.Web.Controles.Spool.JuntaRO juntas = (SAM.Web.Controles.Spool.JuntaRO)rpv.FindControl("juntas");
                        juntas.Map(spool.Juntas);

                        rpv = rmpDetalle.FindPageViewByID("rpvCortes");

                        SAM.Web.Controles.Spool.CorteRO cortes = (SAM.Web.Controles.Spool.CorteRO)rpv.FindControl("cortes");
                        cortes.Map(spool.Cortes);
                    }
                }
            }


            if (e.CommandName == "Borrar")
            {
                int clienteID = e.CommandArgument.SafeIntParse();

                try
                {
                    //SpoolBO.Instance.Borra(spoolID);
                    //establecerDataSource();
                    grdSpools.Rebind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void grdSpools_ItemDataBound(object source, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                GrdConfinarSpool dataItem = (GrdConfinarSpool)e.Item.DataItem;
                int idSpool = dataItem.SpoolID;
                HyperLink lnkConfinar = e.Item.FindControl("hypConfinar") as HyperLink;
                HyperLink lnkDesconfinar = e.Item.FindControl("hypDesconfinar") as HyperLink;
                string jsLink = string.Format("javascript:Sam.Materiales.AbrePopUpConfinarSpool('{0}','{1}');", idSpool, TipoHoldSpool.CONFINADO);
                
                if (dataItem.Confinado)
                {
                    lnkConfinar.Visible = false;
                    lnkDesconfinar.Visible = true;
                    lnkDesconfinar.NavigateUrl = jsLink;
                }
                else
                {
                    lnkDesconfinar.Visible = false;
                    lnkConfinar.Visible = true;
                    lnkConfinar.NavigateUrl = jsLink;
                }
               
               
            }
        }

        protected void lnkActualizar_OnClick(object sender, EventArgs e)
        {
            grdSpools.ResetBind();
            establecerDataSource();
            grdSpools.DataBind();
        }

        private void establecerDataSource()
        {
            grdSpools.DataSource = SpoolBO.Instance.ObtenerParaConfinarSpool(_proyecto, _ordenTrabajo, _numeroControl, _soloConfinados);
        }

        //protected void btnActualiza_Click(object sender, EventArgs e)
        //{
        //    establecerDataSource();
        //    grdSpools.DataBind();
        //}
               

    }
}