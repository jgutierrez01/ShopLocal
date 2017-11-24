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

namespace SAM.Web.Materiales
{
    public partial class PopUpAgregaColada : SamPaginaPopup
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
                    string mensaje = string.Format("El usuario {0} está intentando agregar una colada a un proyecto {1} para el cual no tiene permisos", SessionFacade.UserId, ProyectoID);
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
            ddlAcero.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerAceros(), null);
            ddlFabricante.BindToEnumerableWithEmptyRow(FabricanteBO.Instance.ObtenerPorProyecto(ProyectoID).OrderBy(x => x.Nombre), "Nombre", "FabricanteID", null);
        }

        /// <summary>
        /// Agrega el item code al catálogo y cierra el pop-up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Colada colada = new Colada();
            colada.ProyectoID = ProyectoID;
            colada.AceroID = ddlAcero.SelectedValue.SafeIntParse();
            if (ddlFabricante.SelectedValue.SafeIntParse() > 0)
            {
                colada.FabricanteID = ddlFabricante.SelectedValue.SafeIntParse();
            }
            colada.FechaModificacion = DateTime.Now;
            colada.HoldCalidad = chkHold.Checked;
            colada.NumeroCertificado = txtNumeroCertificado.Text;
            colada.NumeroColada = txtNumeroColada.Text;
            colada.UsuarioModifica = SessionFacade.UserId;

            try
            {
                ColadasBO.Instance.Guarda(colada);
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