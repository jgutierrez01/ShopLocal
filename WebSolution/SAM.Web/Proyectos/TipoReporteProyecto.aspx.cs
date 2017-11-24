using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessObjects.Proyectos;
using Mimo.Framework.Extensions;
using Resources;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities;
using SAM.BusinessObjects.Administracion;
using Mimo.Framework.Exceptions;
using SAM.Web.Common;

namespace SAM.Web.Proyectos
{
    public partial class TipoReporteProyecto : SamPaginaPrincipal
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAProyecto(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar un proyecto {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                lblTitulo.NavigateUrl = string.Format("~/Proyectos/DetProyecto.aspx?ID={0}", EntityID.Value);
                cargarDatos();
                headerProyecto.BindInfo(EntityID.Value);
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.proy_TipoReportes, EntityID.Value);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Dictionary<int, string> dicTipo = CacheCatalogos.Instance.ObtenerTipoReporteProyecto().ToDictionary(x => x.ID, x => x.Nombre);
            bool tieneErrores = false;
            List<string> errores = new List<string>();
            List<ProyectoReporte> lstPr = new List<ProyectoReporte>();
            List<int> iDs = new List<int>();

            lstPr = ProyectoReporteBO.Instance.ObtenerListaPorProyecto(EntityID.Value);

            foreach (RepeaterItem item in repTiposReporte.Items)
            {
                if (item.IsItem())
                {
                    CheckBox chkUtilizarReporte = item.FindControl("chkReportePerzonalizado") as CheckBox;
                    TextBox txtRutaEspanol = item.FindControl("txtRutaEspanol") as TextBox;
                    TextBox txtRutaIngles = item.FindControl("txtRutaIngles") as TextBox;
                    HiddenField hdnTipoReporte = item.FindControl("hdnTipoReporteID") as HiddenField;
                    HiddenField hdnProyectoReporte = item.FindControl("hdnProyectoReporteID") as HiddenField;
                    int tipoReporteID = hdnTipoReporte.Value.SafeIntParse();
                    int proyectoReporteID = hdnProyectoReporte.Value.SafeIntParse();

                    if (chkUtilizarReporte.Checked == true)
                    {
                        if (string.IsNullOrWhiteSpace(txtRutaEspanol.Text) || string.IsNullOrWhiteSpace(txtRutaIngles.Text))
                        {
                            errores.Add(string.Format(MensajesErrorWeb.TipoReporte_DebeEspecificarse, dicTipo[tipoReporteID]));
                            tieneErrores = true;
                        }
                        else
                        {
                            ProyectoReporte pr;

                            pr = lstPr.Where(x => x.ProyectoReporteID == proyectoReporteID).SingleOrDefault();

                            if (pr != null)
                            {
                                pr.StartTracking();
                                pr.RutaEspaniol = txtRutaEspanol.Text;
                                pr.RutaIngles = txtRutaIngles.Text;
                                pr.FechaModificacion = DateTime.Now;
                                pr.UsuarioModifica = SessionFacade.UserId;
                                pr.StopTracking();
                            }
                            else
                            {
                                pr = new ProyectoReporte()
                                {
                                    ProyectoID = EntityID.Value,
                                    TipoReporteProyectoID = tipoReporteID,
                                    RutaEspaniol = txtRutaEspanol.Text,
                                    RutaIngles = txtRutaIngles.Text,
                                    FechaModificacion = DateTime.Now,
                                    UsuarioModifica = SessionFacade.UserId,
                                    VersionRegistro = VersionRegistro,
                                };

                                lstPr.Add(pr);
                            }
                        }
                    }
                    else
                    {
                        if (proyectoReporteID > 0)
                        {
                            iDs.Add(proyectoReporteID);
                        }
                    }
                }
            }

            if (!tieneErrores)
            {
                try
                {
                    ProyectoReporteBO.Instance.Borra(iDs);
                    ProyectoReporteBO.Instance.Guarda(lstPr);
                    Response.Redirect(String.Format(WebConstants.ProyectoUrl.DET_PROYECTO, EntityID.Value));
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex, "");
                }
            }
            else
            {
                RenderErrors(errores);
            }

        }

        private void cargarDatos()
        {
            repTiposReporte.DataSource = TipoReporteProyectoBO.Instance.ObtenerConProyectoReporte(EntityID.Value);
            repTiposReporte.DataBind();
        }

        protected void repTiposReporte_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if (item.IsItem())
            {
                CheckBox chkUtilizarReporte = item.FindControl("chkReportePerzonalizado") as CheckBox;
                TextBox txtRutaEspanol = item.FindControl("txtRutaEspanol") as TextBox;
                TextBox txtRutaIngles = item.FindControl("txtRutaIngles") as TextBox;

                if (txtRutaEspanol.Text != string.Empty || txtRutaIngles.Text != string.Empty)
                {
                    chkUtilizarReporte.Checked = true;
                }
            }

        }

    }
}
