using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using Mimo.Framework.WebControls;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities;
using SAM.Web.Classes;
using SAM.Web.Common;
using Telerik.Web.UI;

namespace SAM.Web.WorkStatus
{
    public partial class PopUpEdicionEspecialDespacho : SamPaginaPopup
    {

        private int DespachoId
        {
            get
            {
                return ViewState["DespachoEdicionId"].SafeIntParse();
            }
            set
            {
                ViewState["DespachoEdicionId"] = value;
            }
        }

        private Entities.Despacho DetalleDespacho
        {
            get
            {
                return (Entities.Despacho)ViewState["DetalleEdicionDespacho"];
            }
            set
            {
                ViewState["DetalleEdicionDespacho"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DespachoId = Request.QueryString["ID"].SafeIntParse();
                DetalleDespacho = DespachoBO.Instance.ObtenDespachoDetalle(DespachoId);
                CargarDatos();
            }
        }

        private void CargarDatos()
        {
            string unidades = DetalleDespacho.NumeroUnico.ItemCode.TipoMaterialID == (int)TipoMaterialEnum.Accessorio ?
                LanguageHelper.CustomCulture == LanguageHelper.INGLES ? " units" : " piezas" : " mm";
            radCmbNumeroUnico.BindToEntiesWithEmptyRow(
                DespachoBO.Instance.ObtenerNumerosUnicosEdicionEspecialDespacho(DetalleDespacho.NumeroUnicoID, DetalleDespacho.Cantidad, "")
                , DetalleDespacho.NumeroUnico.NumeroUnicoID.SafeIntNullableParse());

            rdpFechaDespacho.SelectedDate = DetalleDespacho.FechaDespacho;
            txtEstatus.Text = TraductorEnumeraciones.TextoCanceladoActivo(!DetalleDespacho.Cancelado);
            
            txtCantidadDespachada.Text = DetalleDespacho.Cancelado ? "NA" : DetalleDespacho.MaterialSpool.OrdenTrabajoMaterial[0].CantidadDespachada.ToString() + unidades;
            txtDescripcion.Text = DetalleDespacho.NumeroUnico.ItemCode.DescripcionEspanol;
            txtDiametro1.Text = DetalleDespacho.NumeroUnico.Diametro1.ToString() + " ''";
            txtDiametro2.Text = DetalleDespacho.NumeroUnico.Diametro2.ToString() + " ''";
            txtItemCode.Text = DetalleDespacho.NumeroUnico.ItemCode.Codigo;

            txtCantidadRequerida.Text = DetalleDespacho.MaterialSpool.Cantidad.ToString() + unidades;
            txtDescripcionIng.Text = DetalleDespacho.MaterialSpool.ItemCode.DescripcionEspanol;
            txtDiametro1Despachado.Text = DetalleDespacho.MaterialSpool.Diametro1.ToString() + " ''";
            txtDiametro2Despachado.Text = DetalleDespacho.MaterialSpool.Diametro2.ToString() + " ''";
            txtItemCodeE.Text = DetalleDespacho.MaterialSpool.ItemCode.Codigo;
            txtEtiqueta.Text = DetalleDespacho.MaterialSpool.Etiqueta;

            chkEquivalente.Checked = DetalleDespacho.EsEquivalente;

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Guardar();
            //JsUtils.RegistraScriptActualizayCierraVentana(this);
            this.ClientScript.RegisterStartupScript(typeof(Page),
                                                        "ScriptActualizarPerzonalizado",
                                                        "$(function(){Sam.Popup.VentanaPadre().Sam.Workstatus.ActualizaGridPersonalizado();});",
                                                        true);
            this.ClientScript.RegisterStartupScript(typeof(Page),
                                                        "ScriptCerrarPopUpGenerico",
                                                        "$(function(){Sam.Popup.VentanaPadre().Sam.Workstatus.CierraPopUpGenerico();});",
                                                        true);
            //JsUtils.RegistraScriptActualizaGridGenerico(this);
        }

        private void Guardar()
        {
            try
            {
                if (radCmbNumeroUnico.SelectedValue.SafeIntParse() > 0)
                {
                    bool exito = DespachoBO.Instance.GuardarEdicionEspecialDespacho(DetalleDespacho, radCmbNumeroUnico.SelectedValue.SafeIntParse(), SessionFacade.UserId);
                    if (exito)
                    {

                    }
                    else
                    {
                        throw new BaseValidationException(CultureInfo.CurrentCulture.Name == "en-US" ? "SalidaInventarioID in table Dispatch does not exists"
                                    : "SalidaInventarioID no existe en la tabla Despacho");
                    }
                }
                else
                {
                    throw new BaseValidationException(CultureInfo.CurrentCulture.Name == "en-US" ? "The unique number is required." : "El número único es requerido.");
                }
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
        }

        protected void rfvNumUnico_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }

        protected void radCmbNumeroUnico_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            radCmbNumeroUnico.BindToEnties(
               DespachoBO.Instance.ObtenerNumerosUnicosEdicionEspecialDespacho(DetalleDespacho.NumeroUnicoID, DetalleDespacho.Cantidad, e.Text)
               , e.Value.SafeIntNullableParse());
        }

        protected void radCmbNumeroUnico_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            radCmbNumeroUnico.BindToEnties(
                 DespachoBO.Instance.ObtenerNumerosUnicosEdicionEspecialDespacho(DetalleDespacho.NumeroUnicoID, DetalleDespacho.Cantidad, e.Text)
                 , e.Value.SafeIntNullableParse());
        }
    }
}