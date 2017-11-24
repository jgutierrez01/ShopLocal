using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Produccion;
using SAM.BusinessObjects.Ingenieria;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;
using SAM.Entities;
using SAM.Web.Common;

namespace SAM.Web.Ingenieria
{
    public partial class PopUpAjustarLongitud : SamPaginaPopup
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (EntityID > 0)
                {
                    cargarDatos();
                }
            }
        }

        /// <summary>
        /// Ajusta la cantidad del material al nuevo valor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAjustar_OnClick(object sender, EventArgs e)
        {

            MaterialSpool material = MaterialSpoolBO.Instance.Obtener(EntityID.Value);
            material.StartTracking();
            material.Cantidad = txtNuevaLongitud.Text.SafeIntParse();
            material.FechaModificacion = DateTime.Now;
            material.UsuarioModifica = SessionFacade.UserId;
            material.VersionRegistro = VersionRegistro;

            try
            {
                if (txtLongitudCorte.Text != txtNuevaLongitud.Text)
                {
                    throw new BaseValidationException(MensajesErrorUI.Exception_DiferenciaEnLongitudes);
                }

                MaterialSpoolBO.Instance.Guarda(material);
                JsUtils.RegistraScriptActualizaCortesDeAjuste(this);
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
              
        }

        private void cargarDatos()
        {
            GrdCorteAjuste ajuste = CorteDetalleBO.Instance.ObtenerDetalleParaAjuste(EntityID.Value);

            if (ajuste != null)
            {
                lblSpoolData.Text = ajuste.Spool;
                lblNumeroControlData.Text = ajuste.NumeroControl;
                lblEtiquetaMaterialData.Text = ajuste.EtiquetaMaterial;
                lblDescripcionData.Text = ajuste.Descripcion;
                txtLongitudCorte.Text = ajuste.LongitudCorte.SafeStringParse();
                txtLongitudIngenieria.Text = ajuste.LongitudIngenieria.SafeStringParse();
            }
        }
    }
}