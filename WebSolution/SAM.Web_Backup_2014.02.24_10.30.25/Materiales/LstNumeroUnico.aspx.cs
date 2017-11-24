using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.BusinessObjects.Catalogos;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using Telerik.Web.UI;
using SAM.BusinessObjects.Materiales;
using SAM.BusinessObjects.Proyectos;
using SAM.Entities;
using SAM.Entities.Grid;

namespace SAM.Web.Materiales
{
    public partial class LstNumeroUnico : SamPaginaPrincipal
    {

        #region Propiedades Privadas

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

        private string Colada
        {
            get
            {
                if (ViewState["Colada"] != null)
                {
                    return ViewState["Colada"].ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                ViewState["Colada"] = value;
            }
        }

        private string ItemCode
        {
            get
            {
                return ViewState["ItemCode"].ToString();
            }
            set
            {
                ViewState["ItemCode"] = value;
            }
        }

        private String NumeroInicial
        {
            get
            {
                if (ViewState["NumeroInicial"] != null)
                {
                    return ViewState["NumeroInicial"].ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                ViewState["NumeroInicial"] = value;
            }
        }

        private string NumeroFinal
        {
            get
            {
                if (ViewState["NumeroFinal"] != null)
                {
                    return ViewState["NumeroFinal"].ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                ViewState["NumeroFinal"] = value;
            }

        }


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.mat_NumUnicos);
                cargaCombo();
                precargarNumUnicos();
            }
        }

        private void precargarNumUnicos()
        {

            if (Request.QueryString["ProyID"] != null && Request.QueryString["IcID"] != null)
            {
                ProyectoID = Request.QueryString["ProyID"].SafeIntParse();
                ddlProyecto.Items.FindByValue(ProyectoID.ToString()).Selected = true;

                ItemCode = ItemCodeBO.Instance.Obtener(Request.QueryString["IcID"].SafeIntParse()).Codigo;
                txtItemCode.Text = ItemCode;

                EstablecerDataSource();
                grdNumeroUnicos.DataBind();
                grdNumeroUnicos.Visible = true;
            }
        }

        //metodo para cargar el combo "ddlProyecto".
        private void cargaCombo()
        {
            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos, -1);
        }

        /// <summary>
        /// Método que carga el encabezado del proyecto una vez seleccionada la opción.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProyectoSelectedItemChanged(object sender, EventArgs e)
        {
            int proyectoID = ddlProyecto.SelectedValue.SafeIntParse();

            if (proyectoID > 0)
            {
                Proyecto proyecto = ProyectoBO.Instance.ObtenerConConfiguracion(proyectoID);
                lblCodigo2.Text = lblCodigo.Text = proyecto.ProyectoConfiguracion.PrefijoNumeroUnico + "-";

                proyEncabezado.BindInfo(proyectoID);
                proyEncabezado.Visible = true;
            }
            else
            {
                proyEncabezado.Visible = false;
            }
        }

        //Método para cargar el grid de acuerdo a los datos seleccionados
        protected void btnMOstrar_Click(object sender, EventArgs e)
        {
            ProyectoID = ddlProyecto.SelectedValue.SafeIntParse();
            Colada = txtColada.Text;
            ItemCode = txtItemCode.Text;
            NumeroInicial = String.IsNullOrEmpty(txtNumUnicoInicial.Text) ? string.Empty : lblCodigo.Text + txtNumUnicoInicial.Text;
            NumeroFinal = string.IsNullOrEmpty(txtNumUnicoFinal.Text) ? string.Empty : lblCodigo.Text + txtNumUnicoFinal.Text;

            EstablecerDataSource();
            grdNumeroUnicos.DataBind();
            grdNumeroUnicos.Visible = true;
        }

        /// <summary>
        /// Método que muestra las columnas personalizadas del proyecto par la nomenclatura del spool
        /// </summary>
        private void configurarColumnasProyecto()
        {
            Proyecto proyecto = ProyectoBO.Instance.ObtenerConCamposRecepcion(ProyectoID);

            if (proyecto != null)
            {
                ProyectoCamposRecepcion campos = proyecto.ProyectoCamposRecepcion;

                #region Asignar titulos personalizados
                if (!string.IsNullOrEmpty(campos.CampoRecepcion1))
                {
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre1").HeaderText = campos.CampoRecepcion1;
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre1").Visible = true;
                }
                else
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre1").Visible = false;

                if (!string.IsNullOrEmpty(campos.CampoRecepcion2))
                {
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre2").HeaderText = campos.CampoRecepcion2;
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre2").Visible = true;
                }
                else
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre2").Visible = false;

                if (!string.IsNullOrEmpty(campos.CampoRecepcion3))
                {
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre3").HeaderText = campos.CampoRecepcion3;
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre3").Visible = true;
                }
                else
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre3").Visible = false;

                if (!string.IsNullOrEmpty(campos.CampoRecepcion4))
                {
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre4").HeaderText = campos.CampoRecepcion4;
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre4").Visible = true;
                }
                else
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre4").Visible = false;

                if (!string.IsNullOrEmpty(campos.CampoRecepcion5))
                {
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre5").HeaderText = campos.CampoRecepcion5;
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre5").Visible = true;
                }
                else
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre5").Visible = false;

                if (!string.IsNullOrEmpty(campos.CampoNumeroUnico1))
                {
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre6").HeaderText = campos.CampoNumeroUnico1;
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre6").Visible = true;
                }
                else
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre6").Visible = false;

                if (!string.IsNullOrEmpty(campos.CampoNumeroUnico2))
                {
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre7").HeaderText = campos.CampoNumeroUnico2;
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre7").Visible = true;
                }
                else
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre7").Visible = false;

                if (!string.IsNullOrEmpty(campos.CampoNumeroUnico3))
                {
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre8").HeaderText = campos.CampoNumeroUnico3;
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre8").Visible = true;
                }
                else
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre8").Visible = false;

                if (!string.IsNullOrEmpty(campos.CampoNumeroUnico4))
                {
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre9").HeaderText = campos.CampoNumeroUnico4;
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre9").Visible = true;
                }
                else
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre9").Visible = false;

                if (!string.IsNullOrEmpty(campos.CampoNumeroUnico5))
                {
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre10").HeaderText = campos.CampoNumeroUnico5;
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre10").Visible = true;
                }
                else
                    grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre10").Visible = false;

                #endregion
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    GridColumn col = grdNumeroUnicos.MasterTableView.Columns.FindByUniqueName("hdCampoLibre" + (i + 1));
                    col.Visible = false;
                }
            }
        }

        /// <summary>
        /// Va a la capa de negocios para obtener la información requerida y la asigna al grid
        /// </summary>
        private void EstablecerDataSource()
        {
            configurarColumnasProyecto();
            grdNumeroUnicos.DataSource = NumeroUnicoBO.Instance.ObtenerNumerosUnicosPorProyecto(ProyectoID, Colada, ItemCode, NumeroInicial, NumeroFinal);
            //ItemCodeBO.Instance
        }

        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            EstablecerDataSource();
        }

        protected void grdNumeroUnicos_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        protected void btnActualiza_Click(object sender, EventArgs e)
        {
            EstablecerDataSource();
            grdNumeroUnicos.DataBind();
        }

        /// <summary>
        /// Método que genera el URL para edición de la recepción.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdNumeroUnicos_ItemDataBound(object source, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                HyperLink lnkInventario = e.Item.FindControl("hypInventarios") as HyperLink;
                GrdNumerosUnicosCompleto item = (GrdNumerosUnicosCompleto)e.Item.DataItem;
                string jsLink = string.Format("javascript:Sam.Materiales.AbrePopUpInventarios('{0}');",
                                              item.NumeroUnicoID);
                lnkInventario.NavigateUrl = jsLink;

                HyperLink lnkEditar = (HyperLink)e.Item.FindControl("lnkEditar");

                string jsLinkEdicion = string.Format("javascript:Sam.Materiales.AbrePopUpEdicionNumUnicoConProyecto('{0}', '{1}');",
                                               item.NumeroUnicoID,
                                               ProyectoID);
                lnkEditar.NavigateUrl = jsLinkEdicion;
            }
        }

        protected void grdNumeroUnicos_ItemCreated(object source, GridItemEventArgs e)
        {
            GridCommandItem commandItem = e.Item as GridCommandItem;

            int proyectoID = ddlProyecto.SelectedValue.SafeIntParse();
            string colada = txtColada.Text;
            string itemCode = txtItemCode.Text;
            string numUnicoInicial = txtNumUnicoInicial.Text;
            string numUnicoFinal = txtNumUnicoFinal.Text;

            if(commandItem != null)
            {
                HyperLink lnkExportarExcelLstNumeroUnico = (HyperLink)commandItem.FindControl("lnkExportar");
                HyperLink lnkExportaImagen = (HyperLink)commandItem.FindControl("imgExportar");

                string url = string.Format(WebConstants.MaterialesUrl.ExportaExcelLstNumeroUnico,
                                           proyectoID,
                                           (int) TipoArchivoExcel.LstNumeroUnico,
                                           colada, itemCode, numUnicoInicial, numUnicoFinal
                                           );
                lnkExportarExcelLstNumeroUnico.NavigateUrl = url;
                lnkExportaImagen.NavigateUrl = url;
            }
        }
    }
}