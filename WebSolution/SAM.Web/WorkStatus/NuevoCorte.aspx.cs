using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using Resources;
using SAM.BusinessLogic.Workstatus;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Materiales;
using SAM.BusinessObjects.Produccion;
using SAM.BusinessObjects.Proyectos;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Validations;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities;
using SAM.Entities.Cache;
using SAM.Entities.Personalizadas;
using SAM.Entities.RadCombo;
using SAM.Web.Classes;
using SAM.Web.Common;
using Telerik.Web.UI;

namespace SAM.Web.WorkStatus
{
    public partial class NuevoCorte : SamPaginaPrincipal
    {
        /// <summary>
        /// Listado de Cortes 
        /// </summary>
        private List<CorteDetalle> CorteDetalleLista
        {
            get
            {
                if (ViewState["CorteDetalleLista"] == null)
                {
                    ViewState["CorteDetalleLista"] = new List<CorteDetalle>();
                }

                return (List<CorteDetalle>)ViewState["CorteDetalleLista"];
            }
            set
            {
                ViewState["CorteDetalleLista"] = value;
            }
        }

        /// <summary>
        /// Contador de cortes agregados
        /// </summary>
        private int CorteIDCount
        {
            get
            {
                if (ViewState["CorteIDCount"] == null)
                {
                    ViewState["CorteIDCount"] = -1;
                }
                return (int)ViewState["CorteIDCount"];
            }
            set
            {
                ViewState["CorteIDCount"] = value;
            }
        }

        private List<Simple> NumeroControl
        {
            get
            {
                if (ViewState["NumeroControl"] == null)
                {
                    ViewState["NumeroControl"] = new List<Simple>();
                }

                return (List<Simple>)ViewState["NumeroControl"];
            }
            set
            {
                ViewState["NumeroControl"] = value;
            }
        
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_NuevoCorte);
            }
            else
            {
                if (filtroGenerico.OrdenTrabajoSelectedValue != null)
                {
                    radNumUnico.Enabled = true;
                }
            }
        }

        protected void grdCorte_OnNeedDataSource(object sender, EventArgs e)
        {
        }

        private void cargaCombos(int tallerID)
        {
            List<MaquinaCache> maquinas = CacheCatalogos.Instance.ObtenerMaquinas().Where(x => x.PatioID == hdnPatioID.Value.SafeIntParse()).ToList();
            ddlMaquina.BindToEntiesWithEmptyRow(maquinas);
        }

        /// <summary>
        /// Limpia los campos de captura para agregar un nuevo corte
        /// </summary>
        private void limpiaCamposAgregaCorte()
        {
            radNumeroControl.Text = string.Empty;
            radEtiquetaMaterial.Text = string.Empty;
            radEtiquetaMaterial.Enabled = false;
            chkCorteAjuste.Checked = false;
            chkTramoCompleto.Checked = false;
            ddlMaquina.SelectedIndex = -1;
            ddlMaquina.CssClass = "required";
            pnRequerido.Visible = true;
            valMaquina.Enabled = true;
            ddlMaquina.Enabled = true;
            txtCantidadReal.Text = string.Empty;
            txtCantidadRequerida.Text = string.Empty;
            radNumeroControl.ClearSelection();
            rcbCortadores.Text = string.Empty;
            rcbCortadores.SelectedValue = "";
            rcbCortadores.SelectedIndex = -1;
            rcbCortadores.Items.Clear();
        }

        #region Eventos

        /// <summary>
        /// Al seleccionar una ODT se cargarán los combos Numero Unico
        /// Además se cargará el control de proyecto y la información del taller y fecha de ODT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cargaDatos()
        {
                int ordenTrabajoID = filtroGenerico.OrdenTrabajoSelectedValue.SafeIntParse();
                OrdenTrabajo odt = OrdenTrabajoBO.Instance.Obtener(ordenTrabajoID);

                phDatos.Visible = true;
                lblFecha.Text = odt.FechaOrden.ToString("d");
                lblTaller.Text = odt.Taller.Nombre;

                Proyecto proyecto = ProyectoBO.Instance.ObtenerConConfiguracion(filtroGenerico.ProyectoSelectedValue.SafeIntParse());

                hdnProyectoID.Value = proyecto.ProyectoID.ToString();
                hdnPatioID.Value = odt.Taller.PatioID.ToString();
                hdnTolerancia.Value = proyecto.ProyectoConfiguracion.ToleranciaCortes.ToString();

                radNumUnico.Enabled = true;

                cargaCombos(odt.Taller.TallerID);
        }

        /// <summary>
        /// Obtiene el número unico seleccionado así como su detalle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radNumUnico_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radNumUnico.SelectedValue != string.Empty)
            {
                cargaDatos();
                int numeroUnicoID = radNumUnico.SelectedValue.SafeIntParse();
                string segmento = radNumUnico.Text.Substring(radNumUnico.Text.Length - 1);
                NumeroUnico numUnico = NumeroUnicoBO.Instance.ObtenerConTransferenciaCorteIC(numeroUnicoID);

                lblDescripcionNum.Text = numUnico.ItemCode.DescripcionEspanol;
                lblDiam1Num.Text = string.Format("{0}\"", numUnico.Diametro1.ToString());
                lblDiam2Num.Text = string.Format("{0}\"", numUnico.Diametro2.ToString());
                lblICNum.Text = numUnico.ItemCode.Codigo;

                NumeroUnicoCorte corte = numUnico.NumeroUnicoCorte.Where(x => !x.TieneCorte && x.Segmento == segmento).FirstOrDefault();
                NumeroUnicoSegmento numSegmento = NumeroUnicoSegmentoBO.Instance.ObtenerPorNumeroUnico(numeroUnicoID).Where(x => x.Segmento == segmento).FirstOrDefault();
                lblInv.Text = (corte.Longitud - numSegmento.CantidadDanada).ToString();
                lblInvFisico.Text = corte.Longitud.ToString();

                phDatosNumUnico.Visible = true;
            }
        }

        /// <summary>
        /// Guarda los datos del nuevo corte, los agrega a la lista y hace el bind al grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregar_OnClick(object sender, EventArgs e)
        {
            try
            {
                //Verificar que el spool no se encuentre en hold
                OrdenTrabajoSpool ots = OrdenTrabajoSpoolBO.Instance.Obtener(radNumeroControl.SelectedValue.SafeIntParse());
                ValidacionesSpool.SpoolEnHold(ots.SpoolID);                

                //Si no es corte de ajuste entonces validar que el corte este dentro de tolerancia de proyecto
                if (!chkCorteAjuste.Checked)
                {
                    ValidacionesNumeroUnico.CorteDentroDeTolerancia(txtCantidadReal.Text.SafeIntParse(), txtCantidadRequerida.Text.SafeIntParse(), hdnTolerancia.Value.SafeIntParse());
                }

                //Validar que el corte esté dentro del inventario disponible.
                int total = 0;
                if (grdCorte.Items.Count > 0)
                {
                    GridFooterItem footerItem = grdCorte.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
                    total = footerItem["Cantidad"].Text.Substring(8).SafeIntParse();
                }
                int totalDisponible = lblInv.Text.SafeIntParse() - total;
                ValidacionesNumeroUnico.CorteDentroDeInventario(txtCantidadReal.Text.SafeIntParse(), totalDisponible);

                int materialSpoolID = radEtiquetaMaterial.SelectedValue.SafeIntParse();

                validaNoTengaCorte(materialSpoolID);

                //Agregar valores al grid
                CorteDetalle corte = new CorteDetalle();
                corte.CorteID = CorteIDCount;
                corte.OrdenTrabajoSpoolID = radNumeroControl.SelectedValue.SafeIntParse();
                corte.MaterialSpoolID = materialSpoolID;
                corte.Cantidad = txtCantidadReal.Text.SafeIntParse();
                corte.FechaCorte = DateTime.Now;
                corte.EsAjuste = chkCorteAjuste.Checked;
                                
                if (!chkTramoCompleto.Checked)
                {
                    corte.MaquinaID = ddlMaquina.SelectedValue.SafeIntParse();
                }

                corte.UsuarioModifica = SessionFacade.UserId;
                corte.FechaModificacion = DateTime.Now;

                CorteDetalleLista.Add(corte);
                CorteIDCount = CorteIDCount - 1;
                NumeroControl.Add(new Simple { ID = corte.CorteID, Valor = radNumeroControl.Text });

                grdCorte.DataSource = CorteDetalleLista;
                grdCorte.DataBind();

                limpiaCamposAgregaCorte();

                if (!grdCorte.Visible)
                {
                    grdCorte.Visible = true;
                }

                if (!chkTramoCompleto.Checked)
                {
                    calcularSobrante();
                }
                else {
                    txtSobrante.Text = String.Empty;
                }
               
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex, "vgAgregar");
            }

        }

        /// <summary>
        /// Valida que no se esté tratando de cortar el # único más de una vez para el mismo material.
        /// </summary>
        /// <param name="materialSpoolID">ID del material (etiqueta) para el cual se corte el # único.</param>
        private void validaNoTengaCorte(int materialSpoolID)
        {
            bool yaExiste = CorteDetalleLista.Any(x => x.MaterialSpoolID == materialSpoolID);

            if (yaExiste)
            {
                throw new BaseValidationException(MensajesErrorWeb.Material_YaTieneCorte);
            }
        }

        /// <summary>
        /// Agrega los campos especificos al grid de Corte en el DataBound de cada elemento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdCorte_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                GridDataItem dataItem = e.Item as GridDataItem;

                int idMaquinaID = ((CorteDetalle)e.Item.DataItem).MaquinaID.Value;
                int idMaterialSpoolId = ((CorteDetalle)e.Item.DataItem).MaterialSpoolID;
                int idOrdenTrabajoSpoolId = ((CorteDetalle)e.Item.DataItem).OrdenTrabajoSpoolID;
                int mermaTeorica = MaquinaBO.Instance.ObtenerMaquinaConPatio(idMaquinaID).MermaTeorica.SafeIntParse();
                dataItem["MaterialSpoolID"].Text = idMaterialSpoolId.ToString();
                dataItem["Maquina"].Text = CacheCatalogos.Instance.ObtenerMaquinas().Where(x => x.ID == idMaquinaID).Select(y => y.Nombre).SingleOrDefault();

                MaterialSpool material = MaterialSpoolBO.Instance.Obtener(idMaterialSpoolId);
                OrdenTrabajoSpool odt = OrdenTrabajoSpoolBO.Instance.Obtener(idOrdenTrabajoSpoolId);
                dataItem["NumeroControl"].Text = odt.NumeroControl;
                dataItem["Etiqueta"].Text = material.Etiqueta;
                dataItem["CantidadRequerida"].Text = material.Cantidad.ToString();
                dataItem["MermaTeorica"].Text = mermaTeorica != 0 ? mermaTeorica.SafeStringParse() : "";
            }
        }

        /// <summary>
        /// Se dispara el evento al borrar un corte del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdCorte_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                CorteDetalle corte = CorteDetalleLista.Where(x => x.CorteID == e.CommandArgument.SafeIntParse()).SingleOrDefault();
                CorteDetalleLista.Remove(corte);
                NumeroControl.Remove(NumeroControl.Single(x => x.ID == corte.CorteID));
                grdCorte.DataSource = CorteDetalleLista;
                grdCorte.DataBind();

                calcularSobrante();

            }
        }

        private void calcularSobrante() 
        {
            txtSobrante.Text = string.Empty;           
            int merma = lblInv.Text.SafeIntParse();

            foreach (GridDataItem item in grdCorte.Items)
            {
                merma = merma - item["Cantidad"].Text.SafeIntParse();
                merma = merma - (item["MermaTeorica"].Text.SafeIntParse() != -1 ? item["MermaTeorica"].Text.SafeIntParse() : 0);                                           
            }

            if (grdCorte.Items.Count > 0)
            {
                int calculoSobrante = merma;
                if (calculoSobrante != lblInv.Text.SafeIntParse() && calculoSobrante > 0)
                {
                    txtSobrante.Text = calculoSobrante.ToString();
                }
            }

        }
        /// <summary>
        /// Guarda todos los cortes generados en base de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_OnClick(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    string rack = txtRack.Text;
                    string segmento = radNumUnico.Text.Substring(radNumUnico.Text.Length - 1);
                    int total = 0;
                    int cortadorID = rcbCortadores.SelectedValue.SafeIntParse(); // ddlCortador.SelectedValue.SafeIntParse();
                    if (grdCorte.Items.Count > 0)
                    {
                        GridFooterItem footerItem = grdCorte.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
                        total = footerItem["Cantidad"].Text.Substring(8).SafeIntParse();
                    }

                    CorteBL.Instance.GeneraNuevoCorte(CorteDetalleLista, txtSobrante.Text.SafeIntParse(), rack, radNumUnico.SelectedValue.SafeIntParse(), segmento, total, NumeroControl, SessionFacade.UserId, cortadorID);

                    //Redireccionar a la misma página al terminar un corte
                    Response.Redirect(WebConstants.WorkstatusUrl.NUEVO_CORTE);
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex, "vgGuardar");
                }
            }
        }

        /// <summary>
        /// Habilita y pone en visible todos los campos de captura para el nuevo corte
        /// Deshabilita los campos de orden de trabajo y numero unico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnComenzar_Click(object sender, EventArgs e)
        {
            plhCorte.Visible = true;
            radNumUnico.Enabled = false;
            filtroGenerico.ProyectoEnabled= false;
            filtroGenerico.OrdenTrabajoEnabled = false;
        
            btnReiniciar.Visible = true;
            btnComenzar.Visible = false;
        }

        /// <summary>
        /// Limpia todos los campos de captura para iniciar con un nuevo corte.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReiniciar_Click(object sender, EventArgs e)
        {
            btnReiniciar.Visible = false;
            btnComenzar.Visible = true;
            plhCorte.Visible = false;
            phDatos.Visible = false;
            phDatosNumUnico.Visible = false;
            lblTaller.Text = string.Empty;
            lblFecha.Text = string.Empty;
            lblICNum.Text = string.Empty;
            lblInv.Text = string.Empty;
            lblInvFisico.Text = string.Empty;
            lblDescripcionNum.Text = string.Empty;
            lblDiam1Num.Text = string.Empty;
            lblDiam2Num.Text = string.Empty;
            filtroGenerico.ProyectoEnabled = true;
            filtroGenerico.OrdenTrabajoEnabled = true;
            radNumUnico.Text = string.Empty;
            limpiaCamposAgregaCorte();

            CorteDetalleLista = null;
            CorteIDCount = -1;            
            grdCorte.DataSource = new string[] { }; ;
            grdCorte.DataBind();
            grdCorte.Visible = false;
            txtRack.Text = string.Empty;
            txtSobrante.Text = string.Empty;
            rcbCortadores.Items.Clear();
            rcbCortadores.SelectedValue = string.Empty;
            rcbCortadores.SelectedIndex = -1;

        }

        /// <summary>
        /// Si tramo completo es seleccionado, no se deberá seleccionar máquina
        /// Por lo tanto se deshabilitará el combo y se quitara el estilo de required.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTramoCompleto_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTramoCompleto.Checked)
            {
                ddlMaquina.CssClass = string.Empty;
                pnRequerido.Visible = false;
                valMaquina.Enabled = false;
                valMaquina.ValidationGroup = string.Empty;

                ddlMaquina.SelectedIndex = -1;
                ddlMaquina.Enabled = false;
                txtCantidadReal.Text = lblInvFisico.Text;
                txtCantidadReal.Enabled = false;
            }
            else
            {
                ddlMaquina.CssClass = "required";
                pnRequerido.Visible = true;
                valMaquina.Enabled = true;
                ddlMaquina.Enabled = true;
                valMaquina.ValidationGroup = "vgAgregar";
                txtCantidadReal.Text = string.Empty;
                txtCantidadReal.Enabled = true;
            }
        }

        /// <summary>
        /// Habilita el combo de etiqueta material una vez que se selecciona el numero de control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radNumeroControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radNumeroControl.SelectedValue != string.Empty)
            {
                radEtiquetaMaterial.Enabled = true;
            }
        }

        /// <summary>
        /// Se obtiene la cantidad requerida en base al material seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radEtiquetaMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            MaterialSpool material = MaterialSpoolBO.Instance.Obtener(radEtiquetaMaterial.SelectedValue.SafeIntParse());
            txtCantidadRequerida.Text = material.Cantidad.ToString();
        }

        #endregion

        protected void cusRadCmbOrdenTrabajo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = filtroGenerico.OrdenTrabajoSelectedValue.SafeIntParse() > 0;
        }

        protected void cusRadNumeroControl_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = radNumeroControl.SelectedValue.SafeIntParse() > 0;
        }

        protected void cusRadEtiquetaMaterial_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = radEtiquetaMaterial.SelectedValue.SafeIntParse() > 0;
        }

        protected void OnDdlProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(filtroGenerico.OrdenTrabajoSelectedValue != string.Empty)
            {
                cargaDatos();
            }
        }

    }
}