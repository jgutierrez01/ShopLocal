using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Common;
using SAM.Web.Common;

namespace SAM.Web.Materiales
{
    public partial class PopUpAgregaItemCode : SamPaginaPopup
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProyectoID = Request.QueryString["PID"].ToString().SafeIntParse();

                if (!SeguridadQs.TieneAccesoAProyecto(ProyectoID))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando agregar un item code a un proyecto {1} para el cual no tiene permisos", SessionFacade.UserId, ProyectoID);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                cargaCombos();
            }
        }

        /// <summary>
        /// Carga los combos de la página.
        /// </summary>
        private void cargaCombos()
        {
            ddlClasificacion.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerTipoMaterial(), -1);
            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos, -1);
        }

        /// <summary>
        /// Agrega el item code al catálogo y cierra el pop-up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            ItemCode ic = new ItemCode();
            ic.ProyectoID = ProyectoID;
            ic.TipoMaterialID = ddlClasificacion.SelectedValue.SafeIntParse();
            ic.Codigo = txtItemCode.Text.TrimEnd();
            ic.ItemCodeCliente = txtItemCodeCliente.Text;
            ic.DescripcionEspanol = txtDescripcionEspanol.Text;
            ic.UsuarioModifica = SessionFacade.UserId;
            ic.FechaModificacion = DateTime.Now;

            try
            {
                ItemCodeBO.Instance.Guarda(ic);
                pnlCampos.Visible = false;
                pnlMensaje.Visible = true;
                
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }

        /// <summary>
        /// Aparece el panel que permite ya sea dar de alta un nuevo item code o bien importar uno desde otro proyecto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radio_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbImportar.Checked)
            {
                pnImportar.Visible = true;
                pnNuevo.Visible = false;
            }
            else
            {
                pnImportar.Visible = false;
                pnNuevo.Visible = true;
            }
        }

        /// <summary>
        /// Carga la informacion del item code seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rcbItemCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rcbItemCode.SelectedValue.SafeIntParse() > 0)
            {
                pnDatosIC.Visible = true;
                ItemCode ic = ItemCodeBO.Instance.ObtenerConTipoMaterial(rcbItemCode.SelectedValue.SafeIntParse());
                lblDescripcionText.Text = ic.DescripcionEspanol;
                lblItemCodeClienteText.Text = ic.ItemCodeCliente;
                lblTipoMaterialText.Text = LanguageHelper.CustomCulture == LanguageHelper.ESPANOL? ic.TipoMaterial.Nombre: ic.TipoMaterial.NombreIngles;
            }
        }

        /// <summary>
        /// Valida que el radCombo de item code tenga un registro valido seleccionado.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        protected void cusItemCode_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = rcbItemCode.SelectedValue.SafeIntParse() > 0;
        }

        /// <summary>
        /// Importa el item code seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImportar_Click(object sender, EventArgs e)
        {
            ItemCode ic = ItemCodeBO.Instance.Obtener(rcbItemCode.SelectedValue.SafeIntParse());
            ItemCode icNuevo = new ItemCode();
            icNuevo.Codigo = ic.Codigo.TrimEnd();
            icNuevo.DescripcionEspanol = ic.DescripcionEspanol;
            icNuevo.DescripcionIngles = ic.DescripcionIngles;
            icNuevo.ItemCodeCliente = ic.ItemCodeCliente;
            icNuevo.TipoMaterialID = ic.TipoMaterialID;
            icNuevo.ProyectoID = ProyectoID;
            icNuevo.UsuarioModifica = SessionFacade.UserId;
            icNuevo.FechaModificacion = DateTime.Now;

            try
            {
                ItemCodeBO.Instance.Guarda(icNuevo);
                pnlCampos.Visible = false;
                pnlMensaje.Visible = true;

            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
            
        }
    }
}