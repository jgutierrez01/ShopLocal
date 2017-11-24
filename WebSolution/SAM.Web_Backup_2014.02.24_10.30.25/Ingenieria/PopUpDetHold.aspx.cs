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

namespace SAM.Web.Ingenieria
{
    public partial class PopUpDetHold : SamPaginaPopup
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoASpool(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar a un spool {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                cargarDatos();
            }
        }

        private void cargarDatos()
        {
            hdnTipoHold.Value = Request.QueryString["TH"];
            SpoolInfo infoSpool = SpoolBO.Instance.ObtenerDetalleCompleto(EntityID.Value);
            lblSpoolData.Text = infoSpool.Nombre;
            lblEspecificacionData.Text = infoSpool.Especificacion;
            lblCedulaData.Text = infoSpool.Cedula;
            lblMaterialData.Text = infoSpool.FamiliasAcero;
            lblPesoData.Text = infoSpool.Peso.SafeStringParse();
            lblAreaData.Text = infoSpool.Area.SafeStringParse();
            lblPndData.Text = infoSpool.PorcentajePnd.SafeStringParse();
            lblPdiData.Text = infoSpool.Pdis.SafeStringParse();

            if (hdnTipoHold.Value == TipoHoldSpool.INGENIERIA)
            {
                if (infoSpool.TieneHoldIngenieria == true)
                {
                    btnHold.Visible = false;
                    btnNoHold.Visible = true;
                }
            }
            else if (hdnTipoHold.Value == TipoHoldSpool.CALIDAD)
            {
                if (infoSpool.TieneHoldCalidad == true)
                {
                    btnHold.Visible = false;
                    btnNoHold.Visible = true;
                }
            }
        }

        protected void btnHold_Click(object sender, EventArgs e)
        {
            procesaSpoolHold(true,hdnTipoHold.Value);
        }

        protected void btnNoHold_Click(object sender, EventArgs e)
        {
            procesaSpoolHold(false,hdnTipoHold.Value);
        }

        private void procesaSpoolHold(bool hold, string tipoHold)
        {
            SpoolHold spoolHold = SpoolHoldBO.Instance.Obtener(EntityID.Value);

            if (spoolHold == null)
            {
                spoolHold = new SpoolHold();
                spoolHold.SpoolID = EntityID.Value;
            }

            spoolHold.StartTracking();
            if (hdnTipoHold.Value == TipoHoldSpool.INGENIERIA) 
            {
                spoolHold.TieneHoldIngenieria = hold; 
            }
            if (hdnTipoHold.Value == TipoHoldSpool.CALIDAD) 
            {
                spoolHold.TieneHoldCalidad = hold; 
            }
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
                TipoHold = hdnTipoHold.Value,
            };
            spoolHold.StopTracking();
            try
            {
                SpoolHoldBO.Instance.Guarda(spoolHold, historial);
                JsUtils.RegistraScriptActualizaHoldIngenieria(this);

            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex, "vgHold");
            }
        }
    }
}
