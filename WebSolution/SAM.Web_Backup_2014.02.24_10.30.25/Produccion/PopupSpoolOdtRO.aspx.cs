using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.Entities.Personalizadas;
using SAM.BusinessObjects.Produccion;
using Mimo.Framework.Extensions;

namespace SAM.Web.Produccion
{
    public partial class PopupSpoolOdtRO : SamPaginaPopup
    {
        /// <summary>
        /// Toma la variable del QS para saber de que spool se trata y va a base de datos por su información.
        /// Posteriormente hace el binding a todos los controles pertinentes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAOrdenDeTrabajoSpool(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar a un spool dentro de una orden de trabajo {1} para la cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                DetSpoolOdt spool = OrdenTrabajoBO.Instance.ObtenerDetalleDeSpool(EntityID.Value);
                cargaControles(spool);
            }
        }

        /// <summary>
        /// Manda llamar el mapping a cada uno de los controles para que los mismos 
        /// desplieguen la información que les compete.
        /// </summary>
        /// <param name="spool">Objeto de tipo DetSpool con toda la información requerida por los controles</param>
        private void cargaControles(DetSpoolOdt spool)
        {
            infoSpool.Map(spool);
            juntas.OrdenTrabajoSpoolID = EntityID.Value;
            materiales.Map(spool.Materiales);
            cortes.Map(spool.Cortes);
            holds.Map(spool);
        }
    }
}