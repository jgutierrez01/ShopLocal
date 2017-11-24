using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessLogic;
using SAM.BusinessObjects.Materiales;
using SAM.BusinessObjects.Proyectos;
using SAM.BusinessObjects;
using SAM.Entities;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Data;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;
using SAM.Web.Common;
using Telerik.Web.UI;
using SAM.BusinessLogic.Materiales;
using Mimo.Framework.WebControls;

namespace SAM.Web.Materiales
{
    public partial class NuevaRecepcion : SamPaginaPrincipal
    {
        private LabeledTextBox[] camposRecepcion;


        protected void Page_Load(object sender, EventArgs e)
        {
            camposRecepcion = new LabeledTextBox[] { txtCampoRecepcion1, txtCampoRecepcion2, txtCampoRecepcion3, txtCampoRecepcion4, txtCampoRecepcion5 };

            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.mat_RecepcionMateriales);
                CargaComboProyecto();
                estableceVisibilidadCamposRecepcion();
                pnlCamposRecepcion.Visible = false;
            }
        }

        //metodo para cargar el combo "ddlProyecto".
        private void CargaComboProyecto()
        {
            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
            dtpFechaRecepcion.SelectedDate = DateTime.Now;
        }

        //Metodo para cargar el combo "ddlTransportista" y "ddlProveedor" basado en la selección del proyecto
        protected void ddlProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            int proyectoID = ddlProyecto.SelectedValue.SafeIntParse();

            if (proyectoID > 0)
            {
                ddlTransportista.BindToEnumerableWithEmptyRow(TransportistaBO.Instance.ObtenerPorProyecto(proyectoID), "Nombre", "TransportistaID", -1);
                ddlProveedor.BindToEnumerableWithEmptyRow(ProveedorBO.Instance.ObtenerPorProyecto(proyectoID), "Nombre", "ProveedorID", -1);

                Proyecto proyecto = ProyectoBO.Instance.ObtenerConConfiguracion(proyectoID);

                lblCodigo.Text = proyecto.ProyectoConfiguracion.PrefijoNumeroUnico.ToString() + " - ";
                txtNumeroInicial.Text = ProyectoBO.Instance.SiguienteConsecutivoNumeroUnico(proyectoID).ToString();
                txtNumeroInicial.CssClass = "required smaller";

                proyEncabezado.BindInfo(proyectoID);
                proyEncabezado.Visible = true;

                estableceVisibilidadCamposRecepcion();
                cargarCamposRecepcion();
            }
            else
            {
                proyEncabezado.Visible = false;
                lblCodigo.Text = string.Empty;
                txtNumeroInicial.Text = string.Empty;
                txtNumeroInicial.CssClass = "required";
                ddlProveedor.Items.Clear();
                ddlTransportista.Items.Clear();
            }
        }

        /// <summary>
        /// Genera una nueva recepcion y nos redirecciona a la alta de los numeros unicos que se agregaron
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                //Validamos que el numero inicial sea valido
                NumeroUnicoBO.Instance.ValidaNumeroInicial(txtNumeroInicial.Text.SafeIntParse(), txtCantidadNumUnicos.Text.SafeIntParse(), ddlProyecto.SelectedValue.SafeIntParse());

                Recepcion recepcion = new Recepcion();
                recepcion.FechaRecepcion = dtpFechaRecepcion.SelectedDate.Value;
                recepcion.ProyectoID = ddlProyecto.SelectedValue.SafeIntParse();
                recepcion.TransportistaID = ddlTransportista.SelectedValue.SafeIntParse();
                recepcion.UsuarioModifica = SessionFacade.UserId;
                recepcion.FechaModificacion = DateTime.Now;
                int consecutivoFinal = txtNumeroInicial.Text.SafeIntParse() + txtCantidadNumUnicos.Text.SafeIntParse() - 1;

                #region Guardar Campos

                if (camposRecepcion[0].Visible)
                {
                    recepcion.CampoLibre1 = camposRecepcion[0].Text;
                }

                if (camposRecepcion[1].Visible)
                {
                    recepcion.CampoLibre2 = camposRecepcion[1].Text;
                }

                if (camposRecepcion[2].Visible)
                {
                    recepcion.CampoLibre3 = camposRecepcion[2].Text;
                }

                if (camposRecepcion[3].Visible)
                {
                    recepcion.CampoLibre4 = camposRecepcion[3].Text;
                }

                if (camposRecepcion[4].Visible)
                {
                    recepcion.CampoLibre5 = camposRecepcion[4].Text;
                }

                #endregion

                List<NumeroUnico> numerosUnicos = NumeroUnicoBL.Instance.GeneraNumerosUnicos(txtCantidadNumUnicos.Text.SafeIntParse(), txtNumeroInicial.Text.SafeIntParse(), txtOrdenCompra.Text, txtFactura.Text, recepcion.ProyectoID, ddlProveedor.SelectedValue.SafeIntParse(), lblCodigo.Text.Substring(0, lblCodigo.Text.Length - 3), SessionFacade.UserId, recepcion);
                RecepcionBL.Instance.GeneraRecepcion(recepcion, numerosUnicos, consecutivoFinal);
                Response.Redirect(String.Format(WebConstants.MaterialesUrl.AltaNumeroUnico, numerosUnicos[0].NumeroUnicoID, numerosUnicos.Count.ToString(), 1, recepcion.ProyectoID, 0, -1, string.Empty));
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
        }

        private void cargarCamposRecepcion()
        {
            int proyectoID = ddlProyecto.SelectedValue.SafeIntParse();
            Proyecto proyecto = ProyectoBO.Instance.ObtenerConCamposRecepcion(proyectoID);

            #region Cargar Campos
            
            pnlCamposRecepcion.Visible = true;

            if (!string.IsNullOrEmpty(proyecto.ProyectoCamposRecepcion.CampoRecepcion1))
            {
                txtCampoRecepcion1.Label = string.Format("{0}:", proyecto.ProyectoCamposRecepcion.CampoRecepcion1);
                txtCampoRecepcion1.Visible = true;
            }

            if (!string.IsNullOrEmpty(proyecto.ProyectoCamposRecepcion.CampoRecepcion2))
            {
                txtCampoRecepcion2.Label = string.Format("{0}:", proyecto.ProyectoCamposRecepcion.CampoRecepcion2);
                txtCampoRecepcion2.Visible = true;
            }

            if (!string.IsNullOrEmpty(proyecto.ProyectoCamposRecepcion.CampoRecepcion3))
            {
                txtCampoRecepcion3.Label = string.Format("{0}:", proyecto.ProyectoCamposRecepcion.CampoRecepcion3);
                txtCampoRecepcion3.Visible = true;
            }

            if (!string.IsNullOrEmpty(proyecto.ProyectoCamposRecepcion.CampoRecepcion4))
            {
                txtCampoRecepcion4.Label = string.Format("{0}:", proyecto.ProyectoCamposRecepcion.CampoRecepcion4);
                txtCampoRecepcion4.Visible = true;
            }

            if (!string.IsNullOrEmpty(proyecto.ProyectoCamposRecepcion.CampoRecepcion5))
            {
                txtCampoRecepcion5.Label = string.Format("{0}:", proyecto.ProyectoCamposRecepcion.CampoRecepcion5);
                txtCampoRecepcion5.Visible = true;
            }

            if (!camposRecepcion.Any(x => x.Visible == true))
            {
                pnlCamposRecepcion.Visible = false;
            }

            #endregion
        }

        private void estableceVisibilidadCamposRecepcion()
        {
            for (int i = 0; i < camposRecepcion.Count(); i++)
            {
                camposRecepcion[i].Visible = false;
            }
        }
    }
}