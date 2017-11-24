using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities;
using SAM.BusinessObjects.Produccion;
using SAM.BusinessObjects.Utilerias;
using Mimo.Framework.Common;
using SAM.Web.Common;

namespace SAM.Web.WorkStatus
{
    public partial class PopUpDetalleDespacho : SamPaginaPopup
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoADespacho(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está ver un despacho {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                cargaDatos(EntityID.Value);
            }
        }

        /// <summary>
        /// Obtiene los datos del despacho y carga los datos.
        /// </summary>
        /// <param name="despachoID">ID del despacho</param>
        private void cargaDatos(int despachoID)
        {
            Entities.Despacho despacho = DespachoBO.Instance.ObtenDespachoDetalle(despachoID);
            lblFecha.Text = despacho.FechaDespacho.ToString("d");
            lblEstatus.Text = TraductorEnumeraciones.TextoCanceladoActivo(!despacho.Cancelado);

            string unidades = despacho.NumeroUnico.ItemCode.TipoMaterialID == (int)TipoMaterialEnum.Accessorio ? LanguageHelper.CustomCulture == LanguageHelper.INGLES ? " units" : " piezas" : " mm";


            lblCantidadDespachada.Text = despacho.Cancelado ? "NA" : despacho.MaterialSpool.OrdenTrabajoMaterial[0].CantidadDespachada.ToString() + unidades;
            lblDescripcion.Text = despacho.NumeroUnico.ItemCode.DescripcionEspanol;
            lblDiametro1.Text = despacho.NumeroUnico.Diametro1.ToString() + " ''";
            lblDiametro2.Text = despacho.NumeroUnico.Diametro2.ToString() + " ''"; ;
            lblItemCode.Text = despacho.NumeroUnico.ItemCode.Codigo;
            lblNumeroUnico.Text = despacho.NumeroUnico.Codigo;

            lblCantidadRequerida.Text = despacho.MaterialSpool.Cantidad.ToString() + unidades;
            lblDescripcionIng.Text = despacho.MaterialSpool.ItemCode.DescripcionEspanol;
            lblDiametro1Ing.Text = despacho.MaterialSpool.Diametro1.ToString() + " ''"; ;
            lblDiametro2Ing.Text = despacho.MaterialSpool.Diametro2.ToString() + " ''"; ;
            lblItemCodeIng.Text = despacho.MaterialSpool.ItemCode.Codigo;
            lblEtiqueta.Text = despacho.MaterialSpool.Etiqueta;

            chkEsEquivalente.Checked = despacho.EsEquivalente;
            
        }
    }
}