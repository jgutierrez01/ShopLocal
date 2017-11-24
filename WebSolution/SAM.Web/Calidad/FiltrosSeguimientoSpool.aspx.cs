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
using SAM.Web.Common;
using SAM.BusinessLogic.Excel;

namespace SAM.Web.Calidad
{
    public partial class FiltrosSeguimientoSpool : SamPaginaPrincipal
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                repModulos.DataSource = CacheCatalogos.Instance.ObtenerModulosSeguimientoSpool().OrderBy(x=> x.OrdenUI).ToList();
                repModulos.DataBind();
                CargaComboPersonalizacion();
            }
        }

        protected void repModulos_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                ModuloSeguimientoSpoolCache modulo = e.Item.DataItem as ModuloSeguimientoSpoolCache;
                CheckBoxList chkList = e.Item.FindControl("chkModulos") as CheckBoxList;
                Label lblModulo = e.Item.FindControl("lblModulo") as Label;
                lblModulo.Text = modulo.Nombre;

                chkList.DataSource = CacheCatalogos.Instance
                                                   .ObtenerCamposSeguimientoSpool()
                                                   .Where(x => x.ModuloSeguimientoSpoolID == modulo.ModuloSeguimientoSpoolID).OrderBy(x => x.OrdenUI);

                chkList.DataTextField = "Nombre";
                chkList.DataValueField = "ID";
                chkList.DataBind();
            }
        }

        protected void btnGuardar_onClick(object sender, EventArgs e)
        {

            PersonalizacionSeguimientoSpool Pers = new PersonalizacionSeguimientoSpool();

            if (!string.IsNullOrEmpty(txtNombre.Text))
            {
                Pers.StartTracking();
                Pers.Nombre = txtNombre.Text;
                Pers.UserId = SessionFacade.UserId;
                Pers.FechaModificacion = DateTime.Now;
                Pers.UsuarioModifica = SessionFacade.UserId;
                Pers.StopTracking();

                SeguimientoSpoolBL.Instance.GuardaPersonalizacionSegmentoSpool(Pers);
                int PersSegSpoolID = SeguimientoSpoolBL.Instance.ObtenerPersonalizacionSeguimentoSpoolID(txtNombre.Text);
                GuardarCamposPersonalizacion(PersSegSpoolID);
                CargaComboPersonalizacion();
            }
        }


        public void GuardarCamposPersonalizacion(int persSegSpoolID)
        {
            List<DetallePersonalizacionSeguimientoSpool> Campos = new List<DetallePersonalizacionSeguimientoSpool>();
            foreach (RepeaterItem item in repModulos.Items)
            {
                if (item.IsItem())
                {
                    CheckBoxList chkLst = (CheckBoxList)item.FindControl("chkModulos");

                    foreach (ListItem chk in chkLst.Items)
                    {
                        if (chk.Selected)
                        {
                            DetallePersonalizacionSeguimientoSpool DetPersonalizacion = new DetallePersonalizacionSeguimientoSpool();
                            DetPersonalizacion.PersonalizacionSeguimientoSpoolID = persSegSpoolID;
                            DetPersonalizacion.CampoSeguimientoSpoolID = chk.Value.SafeIntParse();
                            DetPersonalizacion.UsuarioModifica = SessionFacade.UserId;
                            DetPersonalizacion.FechaModificacion = DateTime.Now;
                            Campos.Add(DetPersonalizacion);

                        }
                    }
                }
            }
            SeguimientoSpoolBL.Instance.GuardaDetallePersonalizacionSeguimientoSpool(Campos);
        }

        public void CargaComboPersonalizacion()
        {
            ddlMisReportes.BindToEnumerableWithEmptyRow(SeguimientoSpoolBL.Instance.ObtenerPersonalizacion(SessionFacade.UserId), "Nombre", "PersonalizacionSeguimientoSpoolID", null);
        }

        protected void btnCargar_onClick(object sender, EventArgs e)
        {
            List<DetallePersonalizacionSeguimientoSpool> Personalizacion = SeguimientoSpoolBL.Instance.ObtenerDetallePersonalizacion(ddlMisReportes.SelectedValue.SafeIntParse());
            foreach (RepeaterItem item in repModulos.Items)
            {
                if (item.IsItem())
                {
                    CheckBoxList chkLst = (CheckBoxList)item.FindControl("chkModulos");

                    foreach (ListItem chk in chkLst.Items)
                    {
                        int CampoID = chk.Value.SafeIntParse();
                        chk.Selected = Personalizacion.Any(x => x.CampoSeguimientoSpoolID == CampoID);
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
                    string url = string.Format(WebConstants.ProduccionUrl.ExportaExcelCalidad,
                                            filtroGenerico.ProyectoSelectedValue.SafeIntParse(),
                                            (int)TipoArchivoExcel.SeguimientoSpools, filtroGenerico.NumeroControlSelectedValue, chkEmbarcados.Checked,
                                            filtroGenerico.OrdenTrabajoSelectedValue, IDS);

                    Response.Redirect(url);

                }
                SeguimientoSpoolBL.Instance.MensajeErrorChk();    
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
               
           

           
        }

        protected void btnEliminar_onClick(object sender, EventArgs e)
        {
            int PersSegSpoolID = SeguimientoSpoolBL.Instance.ObtenerPersonalizacionSeguimentoSpoolID(ddlMisReportes.SelectedItem.ToString());

            SeguimientoSpoolBL.Instance.BorrarPersonalizacionSeguimientoSpool(PersSegSpoolID);
            CargaComboPersonalizacion();
        }
    }
}