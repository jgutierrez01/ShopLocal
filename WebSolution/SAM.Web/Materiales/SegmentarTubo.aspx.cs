using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.Web.Common;
using Telerik.Web.UI;
using SAM.Entities;
using SAM.BusinessObjects.Produccion;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Materiales;
using SAM.BusinessObjects.Utilerias;
using SAM.Web.Produccion.App_LocalResources;


namespace SAM.Web.Materiales
{
    public partial class SegmentarTubo : SamPaginaPrincipal
    {
        #region Propiedades privadas en ViewState
        private int _proyectoID
        {
            get
            {
                if (ViewState["ProyectoID"] == null)
                {
                    ViewState["ProyectoID"] = -1;
                }

                return (int)ViewState["ProyectoID"];
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }
        }

        private int _numeroUnicoID
        {
            get
            {
                if (ViewState["NumeroUnicoID"] == null)
                {
                    ViewState["NumeroUnicoID"] = -1;
                }

                return (int)ViewState["NumeroUnicoID"];
            }

            set
            {
                ViewState["NumeroUnicoID"] = value;
            }
        }

        private List<NumeroUnicoSegmento> _segmentos
        {
            get 
            {
                return (List<NumeroUnicoSegmento>)ViewState["Segmentos"];
            }

            set 
            {
                ViewState["Segmentos"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {   
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.mat_SegmentarTubo);
            }
        }

        protected void ddlSegmento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSegmento.SelectedValue.SafeIntParse() > 0)
            {
                NumeroUnicoSegmento nus = _segmentos.Where(x => x.NumeroUnicoSegmentoID == ddlSegmento.SelectedValue.SafeIntParse()).SingleOrDefault();

                if (nus != null)
                {
                    lblInventarioFisicoData.Text = String.Format("{0:N0}", nus.InventarioFisico);
                    lblInventarioBuenEdoData.Text = String.Format("{0:N0}", nus.InventarioBuenEstado);
                }
             
            }
        }

        protected void btnSegmentar_Click(object sender, EventArgs e)
        {
            Validate();
            if (!IsValid) return;
           
            try
            {
                NumeroUnico numUnico = NumeroUnicoBO.Instance.ObtenerParaMovimientoInventarios(_numeroUnicoID);
                numUnico.StartTracking();

                NumeroUnicoMovimiento nume = new NumeroUnicoMovimiento()
                {
                    TipoMovimientoID = (int)TipoMovimientoEnum.SalidaSegmentacion,
                    Cantidad = txtLongitudSegmento.Text.SafeIntParse(),
                    Segmento = ddlSegmento.SelectedItem.Text,
                    NumeroUnicoID = _numeroUnicoID,
                    ProyectoID = _proyectoID,
                    FechaMovimiento = DateTime.Now,
                    Estatus = "A",
                    UsuarioModifica = SessionFacade.UserId,
                    FechaModificacion = DateTime.Now
                };
            
                //Se agrega el movimiento del segmento existente
                numUnico.NumeroUnicoMovimiento.Add(nume);

                NumeroUnicoMovimiento numn = new NumeroUnicoMovimiento() 
                {
                    TipoMovimientoID = (int)TipoMovimientoEnum.EntradasSegmentacion,
                    Cantidad = txtLongitudSegmento.Text.SafeIntParse(),
                    Segmento = txtNombreSegmento.Text,
                    NumeroUnicoID = _numeroUnicoID,
                    ProyectoID = _proyectoID,
                    FechaMovimiento = DateTime.Now,
                    Estatus = "A",
                    UsuarioModifica = SessionFacade.UserId,
                    FechaModificacion = DateTime.Now,
                };
            
                //Se agrega el movimiento del segmento nuevo
                numUnico.NumeroUnicoMovimiento.Add(numn);

                NumeroUnicoSegmento nuse = numUnico.NumeroUnicoSegmento.Where(X => X.NumeroUnicoSegmentoID == ddlSegmento.SelectedValue.SafeIntParse()).SingleOrDefault();

                if (nuse != null)
                {
                    nuse.StartTracking();
                    nuse.InventarioFisico = nuse.InventarioFisico - txtLongitudSegmento.Text.SafeIntParse();
                    nuse.InventarioBuenEstado = nuse.InventarioBuenEstado - txtLongitudSegmento.Text.SafeIntParse();
                    nuse.InventarioDisponibleCruce = nuse.InventarioBuenEstado - nuse.InventarioCongelado;
                    nuse.UsuarioModifica = SessionFacade.UserId;
                    nuse.FechaModificacion = DateTime.Now;
                    nuse.VersionRegistro = VersionRegistro;
                    nuse.StopTracking();
                }

                NumeroUnicoSegmento nusn = new NumeroUnicoSegmento();
                nusn.NumeroUnicoID = nuse.NumeroUnicoID;
                nusn.ProyectoID = nuse.ProyectoID;
                nusn.Segmento = txtNombreSegmento.Text;
                nusn.CantidadDanada = 0;
                nusn.InventarioFisico = txtLongitudSegmento.Text.SafeIntParse();
                nusn.InventarioBuenEstado = nusn.InventarioFisico - nusn.CantidadDanada;
                nusn.InventarioCongelado = 0;
                nusn.InventarioDisponibleCruce = nusn.InventarioBuenEstado - nusn.InventarioCongelado;
                nusn.UsuarioModifica = SessionFacade.UserId;
                nusn.FechaModificacion = DateTime.Now;
            
                //Se agrega el segmento nuevo
                numUnico.NumeroUnicoSegmento.Add(nusn);

           
                    NumeroUnicoBO.Instance.Guarda(numUnico, SessionFacade.UserId);

                    UtileriaRedireccion.RedireccionaExitoProduccion(MensajesProduccion.TituloSegmentarTubo, MensajesProduccion.DetalleSegmentar,
                                                                   new List<LigaMensaje>()
                                                                     {
                                                                        new LigaMensaje
                                                                        {
                                                                            Texto = MensajesProduccion.NuevoSegmento, 
                                                                            Url = WebConstants.MaterialesUrl.SegmentarTubo
                                                                        },
                                                                    });
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex, "vgSegmentar");
            }
        }

        protected void cvLongitudes_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            if (ddlSegmento.SelectedValue.SafeIntParse() > 0)
            {
                e.IsValid = true;

                NumeroUnicoSegmento nus = _segmentos.Where(x => x.NumeroUnicoSegmentoID == ddlSegmento.SelectedValue.SafeIntParse()).SingleOrDefault();

                if (nus != null)
                {
                    if (txtLongitudSegmento.Text.SafeIntParse() >= nus.InventarioFisico)
                    {
                        e.IsValid = false;
                    }
                }
            }
        }

        protected void cvNombreSegmento_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = true;

            if (_segmentos.Any(x => x.Segmento == txtNombreSegmento.Text))
            {
                e.IsValid = false;
            }
        }

        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            pnlDatos.Visible = true;
            phBoton.Visible = true;

            _proyectoID = filtroGenerico.ProyectoSelectedValue.SafeIntParse();
            _numeroUnicoID = filtroGenerico.NumeroUnicoSelectedValue.SafeIntParse();

            NumeroUnico numUnico = NumeroUnicoBO.Instance.ObtenerParaMovimientoInventarios(_numeroUnicoID);
            _segmentos = (List<NumeroUnicoSegmento>)((TrackableCollection<NumeroUnicoSegmento>)numUnico.NumeroUnicoSegmento).ToList();

            ddlSegmento.BindToEnumerableWithEmptyRow(_segmentos, "Segmento", "NumeroUnicoSegmentoID", -1);

            lblItemCodeData.Text = numUnico.ItemCode.Codigo;
            lblDescripcionData.Text = numUnico.ItemCode.DescripcionEspanol;
            lblDiametro1Data.Text = String.Format("{0:N3}", numUnico.Diametro1);
            lblDiametro2Data.Text = String.Format("{0:N3}", numUnico.Diametro2);
            
        }
    }
}
