using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.BusinessObjects.Ingenieria;
using Mimo.Framework.Extensions;
using SAM.Entities;
using SAM.Entities.Personalizadas;
using SAM.BusinessObjects.Catalogos;
using SAM.Web.Classes;
using Mimo.Framework.Exceptions;
using SpoolInfo = SAM.Entities.Personalizadas.DetSpool;

namespace SAM.Web.Materiales
{
    public partial class PopUpConfinarSpool : SamPaginaPopup
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoASpool(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar un spool {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                cargarDatos();
            }
        }

        private void cargarDatos()
        {
            hdnTipoConfinado.Value = Request.QueryString["TH"];
            SpoolInfo infoSpool = SpoolBO.Instance.ObtenerDetalleCompleto(EntityID.Value);
            lblSpoolData.Text = infoSpool.Nombre;
            lblEspecificacionData.Text = infoSpool.Especificacion;
            lblCedulaData.Text = infoSpool.Cedula;
            lblMaterialData.Text = infoSpool.FamiliasAcero;
            lblPesoData.Text = infoSpool.Peso.SafeStringParse();
            lblAreaData.Text = infoSpool.Area.SafeStringParse();
            lblPndData.Text = infoSpool.PorcentajePnd.SafeStringParse();
            lblPdiData.Text = infoSpool.Pdis.SafeStringParse();

            if (infoSpool.Confinado == true)
            {
                btnConfinar.Visible = false;
                btnDesconfinar.Visible = true;
            }
            else
            {
                btnConfinar.Visible = true;
                btnDesconfinar.Visible = false;
            }
           
        }

        protected void btnConfinar_Click(object sender, EventArgs e)
        {
            procesaSpoolConfinado(true, hdnTipoConfinado.Value);
        }

        protected void btnDesconfinar_Click(object sender, EventArgs e)
        {
            procesaSpoolConfinado(false, hdnTipoConfinado.Value);
        }

        private void procesaSpoolConfinado(bool confinado, string tipoConfinado)
        {
            SpoolHold spoolHold = SpoolHoldBO.Instance.Obtener(EntityID.Value);

            if (spoolHold == null)
            {
                spoolHold = new SpoolHold();
                spoolHold.SpoolID = EntityID.Value;
            }

            spoolHold.StartTracking();
            spoolHold.Confinado = confinado;
            spoolHold.FechaModificacion = DateTime.Now;
            spoolHold.UsuarioModifica = SessionFacade.UserId;

            SpoolHoldHistorial historial = new SpoolHoldHistorial
            {
                UsuarioModifica = SessionFacade.UserId,
                VersionRegistro = VersionRegistro,
                Observaciones = txtMotivo.Text,
                FechaHold = DateTime.Now,
                FechaModificacion = DateTime.Now,
                SpoolID = spoolHold.SpoolID,
                TipoHold = hdnTipoConfinado.Value,
            };
            spoolHold.StopTracking();
            try
            {
                SpoolHoldBO.Instance.Guarda(spoolHold, historial);
                JsUtils.RegistraScriptActualizaHoldIngenieria(this);

            }
            catch (Exception ex)
            {
                var cv = new CustomValidator
                {
                    ErrorMessage = ex.Message,
                    IsValid = false,
                    Display = ValidatorDisplay.None,
                    ValidationGroup = "vgConfinado"
                };
                Page.Form.Controls.Add(cv);
            }
        }
    }
}