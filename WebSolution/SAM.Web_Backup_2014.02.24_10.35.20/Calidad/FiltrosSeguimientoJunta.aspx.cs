using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Calidad;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities;
using SAM.Entities.Cache;
using SAM.Web.Classes;

namespace SAM.Web.Calidad
{
    public partial class FiltrosSeguimientoJunta : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                repModulos.DataSource = CacheCatalogos.Instance.ObtenerModulosSeguimientoJunta().OrderBy(x=> x.OrdenUI).ToList();
                repModulos.DataBind();
                CargaComboPersonalizacion();
            }
        }

        protected void repModulo_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                ModuloSeguimientoJuntaCache modulo = e.Item.DataItem as ModuloSeguimientoJuntaCache;
                CheckBoxList chkList = e.Item.FindControl("chkModulos") as CheckBoxList;
                Label lblModulo = e.Item.FindControl("lblModulo") as Label;
                lblModulo.Text = modulo.Nombre;
               
                chkList.DataSource = CacheCatalogos.Instance
                                                   .ObtenerCamposSeguimientoJunta()
                                                   .Where(x => x.ModuloSeguimientoJuntaID == modulo.ModuloSeguimientoJuntaID).OrderBy(x => x.OrdenUI);

                chkList.DataTextField = "Nombre";
                chkList.DataValueField = "ID";
                chkList.DataBind();
            }
        }

        protected void btnGuardar_onClick(object sender, EventArgs e)
        {

            PersonalizacionSeguimientoJunta Pers = new PersonalizacionSeguimientoJunta();

            if (!string.IsNullOrEmpty(txtNombre.Text))
            {
                Pers.StartTracking();
                Pers.Nombre = txtNombre.Text;
                Pers.UserId = SessionFacade.UserId;
                Pers.FechaModificacion = DateTime.Now;
                Pers.UsuarioModifica = SessionFacade.UserId;
                Pers.StopTracking();

                SeguimientoJuntaBL.Instance.GuardaPersonalizacionSegmentoJunta(Pers);
                int PersSegJuntaID = SeguimientoJuntaBL.Instance.ObtenerPersonalizacionSeguimentoJuntaID(txtNombre.Text);
                GuardarCamposPersonalizacion(PersSegJuntaID);
                CargaComboPersonalizacion();
            }
        }


        public void GuardarCamposPersonalizacion(int persSegJuntaID)
        {
            List<DetallePersonalizacionSeguimientoJunta> Campos = new List<DetallePersonalizacionSeguimientoJunta>();
            foreach (RepeaterItem item in repModulos.Items)
            {
                if (item.IsItem())
                {
                    CheckBoxList chkLst = (CheckBoxList)item.FindControl("chkModulos");

                    foreach (ListItem chk in chkLst.Items)
                    {
                        if (chk.Selected)
                        {
                            DetallePersonalizacionSeguimientoJunta DetPersonalizacion = new DetallePersonalizacionSeguimientoJunta();
                            DetPersonalizacion.PersonalizacionSeguimientoJuntaID = persSegJuntaID;
                            DetPersonalizacion.CampoSeguimientoJuntaID = chk.Value.SafeIntParse();
                            DetPersonalizacion.UsuarioModifica = SessionFacade.UserId;
                            DetPersonalizacion.FechaModificacion = DateTime.Now;
                            Campos.Add(DetPersonalizacion);

                        }
                    }
                }
            }
            SeguimientoJuntaBL.Instance.GuardaDetallePersonalizacionSeguimientoJunta(Campos);
        }

        public void CargaComboPersonalizacion()
        {
            ddlMisReportes.BindToEnumerableWithEmptyRow(SeguimientoJuntaBL.Instance.ObtenerPersonalizacion(SessionFacade.UserId), "Nombre", "PersonalizacionSeguimientoJuntaID", null);
        }

        protected void btnCargar_onClick(object sender, EventArgs e)
        {
            List<DetallePersonalizacionSeguimientoJunta> Personalizacion = SeguimientoJuntaBL.Instance.ObtenerDetallePersonalizacion(ddlMisReportes.SelectedValue.SafeIntParse());
            foreach (RepeaterItem item in repModulos.Items)
            {
                if (item.IsItem())
                {
                    CheckBoxList chkLst = (CheckBoxList)item.FindControl("chkModulos");

                    foreach (ListItem chk in chkLst.Items)
                    {
                        int CampoID = chk.Value.SafeIntParse();
                        chk.Selected = Personalizacion.Any(x => x.CampoSeguimientoJuntaID == CampoID);
                    }
                }
            }
        }

        protected void btnMostrar_onClick(object sender, EventArgs e)
        {
            string IDS = string.Empty;
            try
            {
                foreach (RepeaterItem item in repModulos.Items)
                {
                    if (item.IsItem())
                    {
                        CheckBoxList chkLst = (CheckBoxList) item.FindControl("chkModulos");

                        foreach (ListItem chk in chkLst.Items)
                        {
                            if (chk.Selected)
                            {
                                IDS += chk.Value.SafeIntParse() + ",";
                            }
                        }
                    }
                }

                if (IDS.EndsWith(","))
                {
                    IDS = IDS.Substring(0, IDS.Length - 1);
                }
                if (!string.IsNullOrEmpty(IDS))
                {
                    string url = string.Format(WebConstants.CalidadUrl.SEGUIMIENTO_JUNTA,
                                               filtroGenerico.ProyectoSelectedValue.SafeIntParse(),
                                               filtroGenerico.NumeroControlSelectedValue,
                                               chkEmbarcados.Checked,
                                               chkHistorialRep.Checked,
                                               filtroGenerico.OrdenTrabajoSelectedValue,
                                               IDS);

                    Response.Redirect(url);
                }

                SeguimientoJuntaBL.Instance.MensajeErrorChk();
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }

        protected void btnEliminar_onClick(object sender, EventArgs e)
        {
            int PersSegJuntaID = SeguimientoJuntaBL.Instance.ObtenerPersonalizacionSeguimentoJuntaID(ddlMisReportes.SelectedItem.ToString());

            SeguimientoJuntaBL.Instance.BorrarPersonalizacionSeguimientoJunta(PersSegJuntaID);
            CargaComboPersonalizacion();
        }
    }
}