using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Proyectos;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.Entities.Grid;
using SAM.Web.Classes;
using SAM.Web.Common;
using Telerik.Web.UI;
using SAM.BusinessObjects.Excepciones;
using Resources;

namespace SAM.Web.Controles.ImportarProyecto
{
    public partial class ICEquivalentes : System.Web.UI.UserControl
    {
        #region variables publicas / privadas
        public int ProyectoID
        {
            get { return hdnProyectoID.Value.SafeIntParse(); }
            set { hdnProyectoID.Value = value.ToString(); }
        }

        private int ProyExportaID
        {
            get
            {
                return ViewState["ProyExportaID"].SafeIntParse();
            }
            set
            {
                ViewState["ProyExportaID"] = value;
            }
        }

        private List<ItemCodeEquivalente> ListadoGrid
        {
            get
            {
                if (ViewState["ListadoGrid"] == null)
                {
                    ViewState["ListadoGrid"] = new List<ItemCodeEquivalente>();
                }

                return ViewState["ListadoGrid"] as List<ItemCodeEquivalente>;
            }
            set
            {
                ViewState["ListadoGrid"] = value;
            }
        }
        


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                cargaCombos();
            }
        }

        protected void grdICEquivalentes_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        protected void grdICEquivalentes_OnDetailTableDataBind(object sender, Telerik.Web.UI.GridDetailTableDataBindEventArgs e)
        {
            GridDataItem parentItem = e.DetailTableView.ParentItem;
            int itemCodeEquivalenteID = parentItem.GetDataKeyValue("MinItemCodeEquivalenteID").SafeIntParse();

            e.DetailTableView.DataSource =
                ItemCodeEquivalentesBO.Instance
                                      .ObtenerGrupoDeEquivalencias(itemCodeEquivalenteID)
                                      .OrderBy(x => x.CodigoEq)
                                      .ThenBy(x => x.D1Eq);
        }

        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            try
            {
                ProyExportaID = ddlProyecto.SelectedValue.SafeIntParse();

                if (ProyExportaID > 0)
                {
                    ValidaItemCodes();
                    EstablecerDataSource();
                    grdICEquivalentes.DataBind();
                    grdICEquivalentes.Visible = true;
                }
            }
            catch (BaseValidationException ex)
            {
                foreach (string detail in ex.Details)
                {
                    var cv = new CustomValidator
                    {
                        ErrorMessage = detail,
                        IsValid = false,
                        Display = ValidatorDisplay.None,
                        ValidationGroup = "vgEquivalentes"
                    };
                    Page.Form.Controls.Add(cv);
                }
            }
        }

        protected void lnkImportar_OnClick(object sender, EventArgs e)
        {
            List<int> ids =
                grdICEquivalentes.Items.Cast<GridDataItem>().Where(x => x.Selected).Select(
                    x => x.GetDataKeyValue("MinItemCodeEquivalenteID").SafeIntParse()).ToList();

            try
            {
                if (ids.Count > 0)
                {
                    ImportaIcEq(ids);
                    phICEquivalentes.Visible = false;
                    phMensajeExito.Visible = true;
                }
                else
                {
                    throw new ExcepcionDuplicados(MensajesErrorWeb.Exception_Error);
                }
            }
            catch (BaseValidationException ex)
            {
                foreach (string detail in ex.Details)
                {
                    var cv = new CustomValidator
                    {
                        ErrorMessage = detail,
                        IsValid = false,
                        Display = ValidatorDisplay.None,
                        ValidationGroup = "vgEquivalentes"
                    };
                    Page.Form.Controls.Add(cv);
                }
            }
        }

        private void EstablecerDataSource()
        {
            grdICEquivalentes.DataSource = ItemCodeEquivalentesBO.Instance.ObtenerAgrupadosPorItemCodes(ListadoGrid);
        }

        private void cargaCombos()
        {
            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
        }

        private void ValidaItemCodes()
        {
            List<ItemCode> itemCode = ItemCodeBO.Instance.ObtenerListaPorProyectoID(ProyectoID).ToList();
            List<ItemCodeEquivalente> proyExport = ItemCodeEquivalentesBO.Instance.ObtenerAgrupadosPorProyectoID(ProyExportaID).ToList();

            if (itemCode.Count == 0)
            {
                throw new ExcepcionConcordancia(MensajesErrorWeb.Exception_ItemCodesX);
            }
            if (proyExport.Count == 0)
            {
                throw new ExcepcionConcordancia(MensajesErrorWeb.Exception_ItemCodesEqX);
            }

            ListadoGrid = ItemCodeEquivalentesBO.Instance.ObtenEquivalenciasRelacionadas(ProyectoID, ProyExportaID, proyExport);
        }

        private void ImportaIcEq(List<int> ids)
        {

            List<ItemCodeEquivalente> lst = ListadoGrid.Where(x => ids.Contains(x.ItemCodeEquivalenteID)).ToList();

            ItemCodeEquivalentesBO.Instance.ImportaIcEq(lst, ProyectoID, ProyExportaID, SessionFacade.UserId);

        }
    }
}
