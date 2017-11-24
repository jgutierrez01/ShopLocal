using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessObjects.Produccion;
using SAM.Entities;
using SAM.Entities.Cache;
using SAM.BusinessObjects.Utilerias;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Cruce;
using SAM.BusinessLogic.Produccion;
using Mimo.Framework.Exceptions;

namespace SAM.Web.Produccion
{
    /// <summary>
    /// Esta página permite agregar un spool (que no se encuentre en ODT)
    /// a una orden de trabajo existente.
    /// </summary>
    public partial class AgregaSpoolOdt : SamPaginaPrincipal
    {

        /// <summary>
        /// Carga los controles de la página con los datos básicos de la orden de trabajo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAOrdenDeTrabajo(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando agregar un spool a una ODT {1} para la cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.prod_CruceForzado);
                cargaControles();
                titulo.NavigateUrl = string.Format(WebConstants.ProduccionUrl.DetalleOdt, EntityID.Value);
            }
        }


        /// <summary>
        /// Toma el valor del QS para saber el ID de la orden de trabajo y en base a eso
        /// ejecuta las consultas necesarias para traerse la ODT y llenar los datos de los controles
        /// de la página.
        /// </summary>
        private void cargaControles()
        {
            OrdenTrabajo odt = OrdenTrabajoBO.Instance.Obtener(EntityID.Value);
            ProyectoCache p = UserScope.MisProyectos.Where(x => x.ID == odt.ProyectoID).Single();

            if (odt.EsAsignado)
            {
                //Esta página no se puede utilizar para agregar spools a una ODT generada con asignación
                throw new Exception(string.Format("No se puede agregar spool manualmente a la  ODT {0} ya que es una ODT con asignación.", odt.OrdenTrabajoID));
            }

            proyEncabezado.BindInfo(p.ID);
            lblOdt.Text = odt.NumeroOrden;
            lblFecha.Text = odt.FechaOrden.ToString("d");
            lblTaller.Text = UserScope.TalleresPorProyecto(p.ID).Where(x => x.ID == odt.TallerID).Single().Nombre;
            lblEstatus.Text = TraductorEnumeraciones.TextoEstatusOrdenDeTrabajo((EstatusOrdenDeTrabajo)odt.EstatusOrdenID);
            hdnProyectoID.Value = odt.ProyectoID.ToString();
            litNumOdt.Text = odt.NumeroOrden + "-";
            
            //partida sugerida
            txtNumControl.Text = OrdenTrabajoBO.Instance.SiguientePartida(odt.OrdenTrabajoID).ToString();

            VersionRegistro = odt.VersionRegistro;
        }

        /// <summary>
        /// Se dispara cuando el usuario da click en el botón de agregar para
        /// incluir el spool seleccionado en la ODT.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Validate();
            if (IsValid)
            {
                int spoolID = rcbSpools.SelectedValue.SafeIntParse();
                int partida = txtNumControl.Text.SafeIntParse();

                try
                {
                    OrdenTrabajoBL.Instance.AgregaSpool(EntityID.Value, spoolID, partida, SessionFacade.UserId, VersionRegistro);

                    Response.Redirect(string.Format(WebConstants.ProduccionUrl.DetalleOdt, EntityID.Value));
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }
        }

        /// <summary>
        /// Valida que se haya seleccionado un spool en el combo de telerik.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void cusCombo_ServerValidate(object sender, ServerValidateEventArgs args)
        {
            args.IsValid = rcbSpools.SelectedValue.SafeIntParse() > 0;
        }
    }
}
