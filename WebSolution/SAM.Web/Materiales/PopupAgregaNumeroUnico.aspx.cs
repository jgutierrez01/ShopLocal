using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.Entities;
using SAM.BusinessObjects.Materiales;
using SAM.BusinessObjects.Proyectos;
using SAM.Web.Classes;
using SAM.BusinessLogic.Materiales;
using SAM.Web.Common;

namespace SAM.Web.Materiales
{
    public partial class PopupAgregaNumeroUnico : SamPaginaPopup
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

                if (!SeguridadQs.TieneAccesoARecepcion(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar una recepción {1} a la cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                Recepcion recepcion = RecepcionBO.Instance.ObtenerConProyectoConfiguracion(EntityID.Value);
                ProyectoID = recepcion.Proyecto.ProyectoID;
                lblCodigo.Text = recepcion.Proyecto.ProyectoConfiguracion.PrefijoNumeroUnico.ToString() + " - ";
                txtNumeroInicial.Text = ProyectoBO.Instance.SiguienteConsecutivoNumeroUnico(ProyectoID).ToString();
            }
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                int consecutivoFinal = txtNumeroInicial.Text.SafeIntParse() + txtCantidadNumUnicos.Text.SafeIntParse() -
                                       1;
                NumeroUnicoBO.Instance.ValidaNumeroInicial(txtNumeroInicial.Text.SafeIntParse(),
                                                           txtCantidadNumUnicos.Text.SafeIntParse(), ProyectoID);

                Recepcion recepcion = RecepcionBO.Instance.ObtenerConProyectoConfiguracion(EntityID.Value);
                List<NumeroUnico> numerosUnicos =
                    NumeroUnicoBL.Instance.GeneraNumerosUnicos(txtCantidadNumUnicos.Text.SafeIntParse(),
                                                               txtNumeroInicial.Text.SafeIntParse(), string.Empty,
                                                               string.Empty, ProyectoID, -1,
                                                               lblCodigo.Text.Substring(0, lblCodigo.Text.Length - 3),
                                                               SessionFacade.UserId, recepcion);
                NumeroUnicoBO.Instance.GeneraNumerosUnicos(EntityID.Value, numerosUnicos, consecutivoFinal);
                JsUtils.RegistraScriptRedirectAltaNumUnico(this, numerosUnicos[0].NumeroUnicoID, numerosUnicos.Count);
            }catch(BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }
    }
}