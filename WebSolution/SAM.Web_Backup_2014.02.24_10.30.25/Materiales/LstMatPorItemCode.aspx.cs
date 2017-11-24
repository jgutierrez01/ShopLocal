using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Vml;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Catalogos;
using SAM.Entities;
using SAM.Web.Classes;
using Telerik.Web.UI;
using TableCell = System.Web.UI.WebControls.TableCell;

namespace SAM.Web.Materiales
{
    public partial class lstMatrialesPorItemCode : SamPaginaPrincipal
    {

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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.mat_ItemCode);
                cargaCombo();
            }
        }

        protected void ddlProyectoSelectedItemChanged(object sender, EventArgs e)
        {
            int proyectoID = ddlProyecto.SelectedValue.SafeIntParse();

            if (proyectoID > 0)
            {
                proyEncabezado.BindInfo(proyectoID);
                proyEncabezado.Visible = true;
            }
            else
            {
                proyEncabezado.Visible = false;
            }
        }

        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            EstablecerDataSource();
        }

        protected void btnMOstrar_Click(object sender, EventArgs e)
        {
            ProyectoID = ddlProyecto.SelectedValue.SafeIntParse();

            EstablecerDataSource();
            grdItemCode.DataBind();
            grdItemCode.Visible = true;
        }

        private void cargaCombo()
        {
            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos, -1);
        }

        private void EstablecerDataSource()
        {
            grdItemCode.DataSource = ItemCodeBO.Instance.ObtenerLstMatItemCodePorProyecto(ProyectoID);
        }

        protected void grdItemCode_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        protected void grdItemCode_OnDetailTableDataBind(object source, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem parentItem = e.DetailTableView.ParentItem as GridDataItem;
            int itemCodeID = parentItem.GetDataKeyValue("ItemCodeID").SafeIntParse();
            decimal diam1 = parentItem.GetDataKeyValue("Diametro1").SafeDecimalParse();
            decimal diam2 = parentItem.GetDataKeyValue("Diametro2").SafeDecimalParse();

            int ItemCodeEquivalenteID = ItemCodeEquivalentesBO.Instance
                                            .ObtenerItemCodeEquivalentePorItemCode(itemCodeID, diam1, diam2);


            if (ItemCodeEquivalenteID > 0)
            {
                e.DetailTableView.DataSource =
                    ItemCodeEquivalentesBO.Instance
                        .ObtenerGrupoDeEquivalencias(ItemCodeEquivalenteID)
                        .OrderBy(x => x.CodigoEq)
                        .ThenBy(x => x.D1Eq);
            }
            else
            {
                Control c = grdItemCode.MasterTableView.FindControl("BtnExpandColumn");
                c.Visible = false;
            }

        }

        protected void grdItemCode_OnDataBound(object sender, GridItemEventArgs e)
        {
            
            if (e.Item.IsItem() && (e.Item.DataItem) is DataRowView)
            {
                GridDataItem item = e.Item as GridDataItem;

                int itemCodeID = item.GetDataKeyValue("ItemCodeID").SafeIntParse();
                decimal diam1 = item.GetDataKeyValue("Diametro1").SafeDecimalParse();
                decimal diam2 = item.GetDataKeyValue("Diametro2").SafeDecimalParse();

                HyperLink hInfo = (HyperLink)item["numerosUnicos_h"].Controls[1];
                hInfo.NavigateUrl = string.Format("javascript:Sam.Materiales.AbreVentanaNumUnicos({0},{1});",
                                                  ProyectoID, itemCodeID);

                if (!ItemCodeEquivalentesBO.Instance.TieneItemCodeEquivalente(itemCodeID, diam1, diam2))
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
        }

        protected void grdItemCode_ItemCreated(object source, GridItemEventArgs e)
        {
            GridCommandItem commandItem = e.Item as GridCommandItem;
            int proyectoId = ddlProyecto.SelectedValue.SafeIntParse();

            if (commandItem != null)
            {
                HyperLink lnkExportarExcelItemCode = (HyperLink)commandItem.FindControl("lnkExportar");
                HyperLink lnkExportaImagen = (HyperLink)commandItem.FindControl("imgExportar");

                string url = string.Format(WebConstants.MaterialesUrl.ExportaExcelItemCode,
                                           proyectoId,
                                           (int)TipoArchivoExcel.LstItemCode
                                           );
                lnkExportarExcelItemCode.NavigateUrl = url;
                lnkExportaImagen.NavigateUrl = url;
            }
        }
    }
}