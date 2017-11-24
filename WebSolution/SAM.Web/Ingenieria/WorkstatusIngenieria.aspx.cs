using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.Web.Classes;
using SAM.BusinessObjects.Ingenieria;
using SAM.Common;
using System.Configuration;
using System.IO;
using Mimo.Framework.Common;
using Telerik.Web.UI;
using SAM.Entities.Grid;

namespace SAM.Web.Ingenieria
{
    public partial class WorkstatusIngenieria : SamPaginaPrincipal
    {
        private int _proyectoID
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

        private int _ordenTrabajoID
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

        private int _ordenTrabajoSpoolID
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

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnMostrarClick(object sender, EventArgs e)
        {
            _proyectoID = filtroGenerico.ProyectoSelectedValue.SafeIntParse();
            _ordenTrabajoID = filtroGenerico.OrdenTrabajoSelectedValue.SafeIntParse();
            _ordenTrabajoSpoolID = filtroGenerico.NumeroControlSelectedValue.SafeIntParse();
            MuestraGrid();
        }

        protected void MuestraGrid()
        {
            establecerDataSource();
            grdHistWorkStatus.Visible = true;
            grdHistWorkStatus.DataBind();
        }

        private void establecerDataSource()
        {
            grdHistWorkStatus.DataSource = SpoolBO.Instance.ObtenerHistoricoHomologacionWorkstatus(_proyectoID, _ordenTrabajoID, _ordenTrabajoSpoolID);
        }

        //protected void grdHistWorkStatus_ItemCreated(object sender, GridItemEventArgs e)
        //{
        //    GridCommandItem commandItem = e.Item as GridCommandItem;
            
        //    if (commandItem != null)
        //    {
        //        if (e.Item.IsItem())
        //        {
        //            GridDataItem item = (GridDataItem)e.Item;
        //            GrdHistWorkstatus Historico = (GrdHistWorkstatus)item.DataItem;

        //            HyperLink lnkExportarExcelJunta = (HyperLink)commandItem.FindControl("hlExporta");
        //            HyperLink hlExportaImagen = (HyperLink)commandItem.FindControl("hlExportaImagen");
        //            string urlSpool = string.Format("~/Ingenieria/AbrirExcelHistoricoWorkStatus?HWSID={0},IsSpool={1}",
        //                                        Historico.HistoricoWorkStatusID, true);
        //            string urlJunta = string.Format("~/Ingenieria/AbrirExcelHistoricoWorkStatus?HWSID={0},IsSpool={1}",
        //                                        Historico.HistoricoWorkStatusID, false);

        //            lnkExportarExcelJunta.NavigateUrl = urlSpool;
        //            hlExportaImagen.NavigateUrl = urlJunta;
        //        }
        //    }
        //}

        protected void grdHistWorkStatus_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                GrdHistWorkstatus Historico = (GrdHistWorkstatus)e.Item.DataItem;

                HyperLink lnkExcelSpool = (HyperLink)dataItem.FindControl("hypPorSpool");
                HyperLink lnkExcelJunta = (HyperLink)dataItem.FindControl("hypPorJunta");
                string urlSpool = string.Format("~/Ingenieria/AbrirExcelHistoricoWorkStatus.aspx?ID={0}&IsSpool={1}",
                                            Historico.HistoricoWorkStatusID, true);
                string urlJunta = string.Format("~/Ingenieria/AbrirExcelHistoricoWorkStatus.aspx?ID={0}&IsSpool={1}",
                                            Historico.HistoricoWorkStatusID, false);

                lnkExcelSpool.NavigateUrl = urlSpool;
                lnkExcelJunta.NavigateUrl = urlJunta;
            }            
        }
    }
}