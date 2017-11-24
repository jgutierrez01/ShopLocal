using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Materiales;
using SAM.Entities;
using SAM.Web.Classes;
using Telerik.Web.UI;
using SAM.Entities.Grid;
using Mimo.Framework.Exceptions;

namespace SAM.Web.Materiales
{
    public partial class DetRecepcion : SamPaginaPrincipal
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
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.mat_Recepciones);

                if (!SeguridadQs.TieneAccesoARecepcion(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar una recepción {1} a la cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                Carga();
            }
        }

        //metodo para cargar el combo "ddlTransportista" con proyectos de usuario.
        private void CargaCombo(int ProyectoID, int TransportistaID)
        {
            ddlTransportista.BindToEnumerableWithEmptyRow(TransportistaBO.Instance.ObtenerPorProyecto(ProyectoID),
                                                            "Nombre",
                                                            "TransportistaID",
                                                            TransportistaID);

        }

        //carga valores de información de la recepción
        private void Carga()
        {
            Recepcion recepcion = RecepcionBO.Instance.Obtener(EntityID.Value);
            ProyectoID = recepcion.ProyectoID;
            proyEncabezado.BindInfo(ProyectoID);
            CargaCombo(recepcion.ProyectoID, recepcion.TransportistaID);
            dtpFechaRecepcion.SelectedDate = recepcion.FechaRecepcion;
            VersionRegistro = recepcion.VersionRegistro;

            titulo.NavigateUrl = string.Format("~/Materiales/LstRecepcion.aspx?ID={0}", ProyectoID);
        }

        private void Guarda()
        {
            Recepcion recepcion = RecepcionBO.Instance.Obtener(EntityID.Value);
            recepcion.VersionRegistro = VersionRegistro;
            recepcion.StartTracking();
            recepcion.TransportistaID = ddlTransportista.SelectedValue.SafeIntParse();
            recepcion.FechaRecepcion = dtpFechaRecepcion.SelectedDate.Value;
            recepcion.UsuarioModifica = SessionFacade.UserId;
            recepcion.FechaModificacion = DateTime.Today;
            recepcion.StopTracking();
            RecepcionBO.Instance.Guarda(recepcion);

            EstablecerDataSource();
        }

        protected void btnGuardar_OnClick(object sender, EventArgs e)
        {
            Guarda();
        }

        /// <summary>
        /// Va a la capa de negocios para obtener la información requerida y la asigna al grid
        /// </summary>
        private void EstablecerDataSource()
        {
            List<GrdNumerosUnicos> lst = NumeroUnicoBO.Instance
                                                      .ObtenerNumerosUnicosPorRecepcionID(EntityID.Value);

            grdNumerosUnicos.DataSource = lst;
            lblCantidadNoUnicosAsignados.Text = lst.Count().ToString();
        }

        protected void grdRecepcion_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        protected void grdNumerosUnicos_ItemCreated(object sender, GridItemEventArgs e)
        {
            GridCommandItem commandItem = e.Item as GridCommandItem;

            if (commandItem != null)
            {
                HyperLink lnkAgrega = (HyperLink)commandItem.FindControl("lnkAgregar");
                HyperLink imgAgrega = (HyperLink)commandItem.FindControl("imgAgregar");

                string jsLink = string.Format("javascript:Sam.Materiales.AbrePopupAgregaNumUnicos('{0}');",
                                                EntityID.Value);
                lnkAgrega.NavigateUrl = jsLink;
                imgAgrega.NavigateUrl = jsLink;

                HyperLink lnkImprimir = (HyperLink)commandItem.FindControl("lnkImprimir");
                HyperLink imgImprimir = (HyperLink)commandItem.FindControl("imgImprimir");

                string linkImprimir = string.Format("/Materiales/EtiquetasPDF.aspx?ID={0}&ProyectoID={1}", EntityID.Value, ProyectoID);
                lnkImprimir.NavigateUrl = linkImprimir;
                imgImprimir.NavigateUrl = linkImprimir;
            }
        }

        protected void grdRecepcion_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                HyperLink lnkEditar = (HyperLink)e.Item.FindControl("lnkEditar");
                GrdNumerosUnicos item = (GrdNumerosUnicos)e.Item.DataItem;

                string jsLink = string.Format("javascript:Sam.Materiales.AbrePopUpEdicionNumUnicoConProyecto('{0}', '{1}');",
                                               item.NumeroUnicoID,
                                               ProyectoID);
                lnkEditar.NavigateUrl = jsLink;
            }
        }

        protected void btnRedirectAltaNumUnicos_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format(WebConstants.MaterialesUrl.AltaNumeroUnico, hdnNumeroUnicoID.Value.SafeIntParse(), hdnCantidadNumUnicos.Value, 1, ProyectoID, 0, -1, string.Empty));
        }

        protected void btnActualiza_Click(object sender, EventArgs e)
        {
            EstablecerDataSource();
            grdNumerosUnicos.DataBind();
        }


        /// <summary>
        /// Método que elimina el número único
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdRecepcion_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int numeroUnicoID = e.CommandArgument.SafeIntParse();

                try
                {
                    NumeroUnicoBO.Instance.BorraNumeroUnico(numeroUnicoID);
                    grdNumerosUnicos.Rebind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex, "vgGrid");
                }
            }
        }
    }
}