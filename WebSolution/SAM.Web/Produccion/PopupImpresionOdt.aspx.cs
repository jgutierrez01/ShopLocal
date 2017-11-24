using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessObjects.Produccion;
using SAM.Web.Common;
using SAM.Web.Produccion.App_LocalResources;

namespace SAM.Web.Produccion
{
    /// <summary>
    /// Muestra un popup con las alternativas de impresión para una orden de trabajo en particular.
    /// </summary>
    public partial class PopupImpresionOdt : SamPaginaPopup
    {

        /// <summary>
        /// Número de orden de trabajo
        /// </summary>
        private string NumeroOdt
        {
            get
            {
                return ViewState["NumeroOdt"].ToString();
            }
            set
            {
                ViewState["NumeroOdt"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAOrdenDeTrabajo(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando imprimir una ODT {1} para la cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                //Obtener el número de la ODT y guardar en ViewState
                NumeroOdt = OrdenTrabajoBO.Instance
                                          .Obtener(EntityID.Value)
                                          .NumeroOrden;

                litTitulo.Text = string.Format(MensajesPopupImpresion.Titulo_ImpresionOdt, NumeroOdt);
            }
        }

        /// <summary>
        /// Generar el PDF compuesto de los distintos reportes seleccionados y enviar ese PDF como el response de la
        /// página para que el browser lo abra.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            Validate();
            if (IsValid)
            {
                byte [] reporte = UtileriasReportes.ObtenReporteOdt(EntityID.Value, 
                                                                    chkCaratula.Checked, 
                                                                    chkJuntas.Checked, 
                                                                    chkListaCorte.Checked, 
                                                                    chkMateriales.Checked, 
                                                                    chkResumenMateriales.Checked,
                                                                    chkCaratulaPorEstacionTrabajo.Checked,
                                                                    chkJuntasDetalle.Checked,
                                                                    chkJuntasDetalleEstacion.Checked,
                                                                    chkMaterialesDetalle.Checked,
                                                                    chkMaterialesDetalleEstacion.Checked,
                                                                    chkResumenMaterialesEstacion.Checked,
                                                                    chkCaratulaDetallada.Checked,
                                                                    chkResumenMaterialesTaller.Checked,
                                                                    chkCortesEstacion.Checked,
                                                                    chkCorteTaller.Checked);

                UtileriasReportes.EnviaReporteComoPdf(this, reporte, string.Format("ODT #{0}.pdf", NumeroOdt));
            }
        }

        /// <summary>
        /// Validar que se haya mandado imprimir al menos uno de los 5 reportes posibles.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void cusCheck_ServerValidate(object sender, ServerValidateEventArgs args)
        {
            args.IsValid = chkCaratula.Checked || 
                           chkJuntas.Checked || 
                           chkListaCorte.Checked || 
                           chkMateriales.Checked || 
                           chkResumenMateriales.Checked ||
                           chkCaratulaPorEstacionTrabajo.Checked ||
                           chkJuntasDetalle.Checked ||
                           chkJuntasDetalleEstacion.Checked ||
                           chkMaterialesDetalle.Checked ||
                           chkMaterialesDetalleEstacion.Checked ||
                           chkResumenMaterialesEstacion.Checked ||
                           chkCaratulaDetallada.Checked ||
                           chkCortesEstacion.Checked ||
                           chkCorteTaller.Checked;
        }
    }
}