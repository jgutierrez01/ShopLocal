using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.Entities;
using SAM.BusinessObjects.Materiales;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Proyectos;
using SAM.BusinessObjects.Catalogos;

namespace SAM.Web.Controles.Materiales
{
    public partial class AltaNumeroUnicoProveedor : System.Web.UI.UserControl
    {          

        #region Propiedades Solo Lectura

        public int? Proveedor
        {
            get
            {
                if (ddlProveedor.SelectedValue.SafeIntParse() > 0)
                {
                    return ddlProveedor.SelectedValue.SafeIntParse();
                }
                else
                {
                    return null;
                }
            }
        }

        public int? Fabricante
        {
            get
            {
                if (ddlFabricante.SelectedValue.SafeIntParse() > 0)
                {
                    return ddlFabricante.SelectedValue.SafeIntParse();
                }
                else
                {
                    return null;
                }
            }
        }

        public string Factura
        {
            get
            {
                return txtFactura.Text;
            }
        }

        public string OrdenCompra
        {
            get
            {
                return txtOrdenCompra.Text;
            }
        }

        public string PartidaFactura
        {
            get
            {
                return txtPartidaFactura.Text;
            }
        }

        public string PartidaOrden
        {
            get
            {
                return txtPartidaOrden.Text;
            }
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Carga la información que se logre obtener en los campos del control
        /// </summary>
        public void CargaInformacion(NumeroUnico numUnico, NumeroUnico numAnterior)
        {             
            cargaCombos(numUnico.ProyectoID);

            if (numAnterior != null)
            {
                txtFactura.Text = numAnterior.Factura;
                txtOrdenCompra.Text = numAnterior.OrdenDeCompra;
                txtPartidaFactura.Text = numAnterior.PartidaFactura;
                txtPartidaOrden.Text = numAnterior.PartidaOrdenDeCompra;
                ddlProveedor.SelectedValue = numAnterior.ProveedorID.HasValue? numAnterior.ProveedorID.Value.ToString():string.Empty;
                ddlFabricante.SelectedValue = numAnterior.FabricanteID.HasValue ? numAnterior.FabricanteID.Value.ToString() : string.Empty;
            }
            else
            {
                txtFactura.Text = numUnico.Factura;
                txtOrdenCompra.Text = numUnico.OrdenDeCompra;
                txtPartidaFactura.Text = numUnico.PartidaFactura;
                txtPartidaOrden.Text = numUnico.PartidaOrdenDeCompra;
                ddlProveedor.SelectedValue = numUnico.ProveedorID.HasValue ? numUnico.ProveedorID.Value.ToString() : string.Empty;
                ddlFabricante.SelectedValue = numUnico.FabricanteID.HasValue ? numUnico.FabricanteID.Value.ToString() : string.Empty;
            }
        }

        /// <summary>
        /// Carga los combos de acuerdo al proyecto al que pertenece el numero unico.
        /// </summary>
        /// <param name="proyectoID"></param>
        private void cargaCombos(int proyectoID)
        {
           ddlFabricante.BindToEnumerableWithEmptyRow(FabricanteBO.Instance.ObtenerPorProyecto(proyectoID), "Nombre", "FabricanteID", -1);
           ddlProveedor.BindToEnumerableWithEmptyRow(ProveedorBO.Instance.ObtenerPorProyecto(proyectoID), "Nombre", "ProveedorID", -1);
        }

        /// <summary>
        /// Limpia los campos del control
        /// </summary>
        public void LimpiaDatos()
        {
            ddlFabricante.SelectedIndex = -1;
            ddlProveedor.SelectedIndex = -1;
            txtFactura.Text = string.Empty;
            txtOrdenCompra.Text = string.Empty;
            txtPartidaFactura.Text = string.Empty;
            txtPartidaOrden.Text = string.Empty;
        }
    }
}