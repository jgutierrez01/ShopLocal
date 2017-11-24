using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Produccion;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Cache;
using SAM.Entities.Grid;
using SAM.Entities.Personalizadas;
using SAM.Web.Classes;
using SAM.Web.Common;
using Telerik.Web.UI;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace SAM.Web.Produccion
{
    public partial class CongeladosNumeroUnico : SamPaginaPrincipal
    {

        private int _proyectoID
        {
            get
            {
                if (ViewState["_proyectoID"] == null)
                {
                    ViewState["_proyectoID"] = -1;
                }
                return ViewState["_proyectoID"].SafeIntParse();
            }
            set
            {
                ViewState["_proyectoID"] = value;
            }
        }

        private int _numeroUnicoID
        {
            get
            {
                if (ViewState["_numeroUnicoID"] == null)
                {
                    ViewState["_numeroUnicoID"] = -1;
                }
                return ViewState["_numeroUnicoID"].SafeIntParse();
            }
            set
            {
                ViewState["_numeroUnicoID"] = value;
            }
        }

        private string _script = @"$(function () {
            Sam.Utilerias.MuestraOverlay();
        });";
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.prod_CongeladosNumeroUnico);
                ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
                if (SessionFacade.EsAdministradorSistema)
                {
                    btnTranferenciaMasiva.Visible = true;
                }
            }
        }

        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid)
                {
                    if (radNumeroUnico.SelectedValue.SafeIntParse() <= 0)
                    {
                        throw new BaseValidationException(Cultura == "en-US" ? "The unique number is required" : "El número único es requerido");
                    }

                    _proyectoID = ddlProyecto.SelectedValue.SafeIntParse();
                    _numeroUnicoID = radNumeroUnico.SelectedValue.SafeIntParse();
                    MuestraGrid();
                }
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
        }

        protected void MuestraGrid()
        {            
            EstablecerDataSource();
            phSpools.Visible = true;
            phLabels.Visible = true;
            grdSpools.DataBind();
        }

        /// <summary>
        /// Se dispara cuando el grid necesita actualizar su datasource, por ejemplo cuando
        /// hay paginación, sorting o filtros.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSpools_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {           
            EstablecerDataSource();            
        }

        /// <summary>
        /// Usamos este método para configurar dinámicamente los links del header del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSpools_ItemCreated(object sender, GridItemEventArgs e)
        {
            GridCommandItem commandItem = e.Item as GridCommandItem;

            if (commandItem != null)
            {
                HyperLink lnkCong = (HyperLink)commandItem.FindControl("lnkTransfCongelado");
                HyperLink imgCong = (HyperLink)commandItem.FindControl("imgTransfCongelado");

                string jsLink = string.Format("javascript:Sam.Produccion.AbrePopupCongeladoNumeroUnico('{0}','{1}','{2}','{3}');",
                                                grdSpools.ClientID,_proyectoID,_numeroUnicoID,radNumeroUnico.Text);

                lnkCong.NavigateUrl = jsLink;
                imgCong.NavigateUrl = jsLink;
            }
        }

        /// <summary>
        /// Se dispara cuando el proyecto seleccionado cambia.  En caso de seleccionar
        /// un proyecto válido se muestra el project header control con sus datos, de lo contrario
        /// se oculta el control.
        /// </summary>
        protected void ddlProyecto_SelectedIndexChange(object sender, EventArgs e)
        {
            int proyectoID = ddlProyecto.SelectedValue.SafeIntParse();

            if (proyectoID <= 0)
            {                
                radNumeroUnico.Items.Clear();
            }
            else
            {
                headerProyecto.Visible = true;
                headerProyecto.BindInfo(ddlProyecto.SelectedValue.SafeIntParse());
                //radNumeroUnico.BindToEntiesWithEmptyRow(CongeladosBO.Instance.ObtenerNumeroUnico(proyectoID));                
            }
        }

        private void EstablecerDataSource()
        {
            CongeladosOdt congeladosodt = CongeladosBO.Instance.ObtenerInfoNumUnico(_proyectoID, _numeroUnicoID, radNumeroUnico.Text);
            ItemCodeValor.Text = congeladosodt.Codigo;
            DescripcionValor.Text = congeladosodt.Descripcion;
            Diametro1Valor.Text = congeladosodt.Diametro1.SafeStringParse();
            Diametro2Valor.Text = congeladosodt.Diametro2.SafeStringParse();
            InventarioFisicoValor.Text = congeladosodt.InvFisico.SafeStringParse();
            InventarioDañadoValor.Text = congeladosodt.InvDañado.SafeStringParse();
            InventarioCongeladoValor.Text = congeladosodt.InvCongelado.SafeStringParse();
            InventarioDisponibleValor.Text = congeladosodt.InvDisponible.SafeStringParse();

            List<GrdCongeladosNumeroUnico> Datasource = CongeladosBO.Instance.ObtenerListadoCruce(_numeroUnicoID);
            grdSpools.DataSource = Datasource;
        }

        protected void cusNumUnic_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = radNumeroUnico.SelectedValue.SafeIntParse() > 0;
        }

        protected void btnWrapper_Click(object sender, EventArgs e)
        {
            EstablecerDataSource();
            grdSpools.DataBind();
        }

        protected void btnTranferenciaMasiva_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid)
                {
                    _proyectoID = ddlProyecto.SelectedValue.SafeIntParse();
                    CongeladosBO.Instance.TransferenciaMasivaDeCongelados(_proyectoID, SessionFacade.UserId);
                    //PdfDocument documento;
                }
            }
            catch(BaseValidationException ex)
            {
                RenderErrors(ex);
            }
        }
    }
}