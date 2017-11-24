using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.Entities.Personalizadas;
using SAM.BusinessObjects.Ingenieria;
using SAM.Web.Common;

namespace SAM.Web.Produccion
{
    public partial class PopupSpoolRO : SamPaginaPopup
    {
        public bool EsRevision
        {
            get
            {
                return ViewState["DetSpoolRevision"].SafeBoolParse();
            }
            set
            {
                ViewState["DetSpoolRevision"] = value;
            }
        }

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
                if (!SeguridadQs.TieneAccesoASpool(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar a un spool {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                EsRevision = Request.QueryString["SPREV"].SafeBoolParse();

                DetSpool spool = SpoolBO.Instance.ObtenerDetalleCompleto(EntityID.Value);
                cargaControles(spool);
            }
        }

        /// <summary>
        /// Manda llamar el mapping a cada uno de los controles para que los mismos 
        /// desplieguen la información que les compete.
        /// </summary>
        /// <param name="spool">Objeto de tipo DetSpool con toda la información requerida por los controles</param>
        private void cargaControles(DetSpool spool)
        {
            infoSpool.Map(spool);
            juntas.Map(spool.Juntas);
            if (EsRevision)
            {
                materiales.Map(spool.Materiales, true);
            }
            else
            {
                materiales.Map(spool.Materiales);
            }
            cortes.Map(spool.Cortes);
            holds.Map(spool);
        }
    }
}