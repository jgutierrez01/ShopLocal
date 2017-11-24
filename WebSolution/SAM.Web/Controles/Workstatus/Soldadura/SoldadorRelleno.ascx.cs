using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Common;
using Telerik.Web.UI;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities.Grid;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Validations;
using SAM.Entities;
using SAM.Web.Classes;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Exceptions;
using SAM.Entities.Cache;
using SAM.BusinessLogic.Workstatus;
using SAM.BusinessObjects.Ingenieria;

namespace SAM.Web.Controles.Workstatus.Soldadura
{
    public partial class SoldadorRelleno : System.Web.UI.UserControl
    {
        public event EventHandler ProcesoRellenoSeleccionado;
        public event EventHandler WpsSeleccionado;

        public List<GrdSoldadorProceso> SoldadoresList
        {
            get
            {
                if (ViewState["SoldadoresRelleno"] == null)
                {
                    ViewState["SoldadoresRelleno"] = new List<GrdSoldadorProceso>();
                }

                return (List<GrdSoldadorProceso>)ViewState["SoldadoresRelleno"];
            }
            set
            {
                ViewState["SoldadoresRelleno"] = value;
            }
        }

        private List<int> SoldaduraDetalleID
        {
            get
            {
                if (ViewState["SoldaduraDetalleID"] == null)
                {
                    ViewState["SoldaduraDetalleID"] = new List<int>();
                }

                return (List<int>)ViewState["SoldaduraDetalleID"];
            }
            set
            {
                ViewState["SoldaduraDetalleID"] = value;
            }
        }

        public int ProcesoRelleno
        {
            get
            {
                return ddlProcesoRelleno.SelectedValue.SafeIntParse();
            }
        }

        public string wpsItem
        {
            get
            {
                return ddlWps.SelectedValue.SafeStringParse();
            }
            set
            {
                ddlWps.SelectedValue = value;
            }
        }

        public string ProcesoRellenoSelectedItem
        {
            get
            {
                return ddlProcesoRelleno.SelectedItem.Text;
            }
            set
            {
                ddlProcesoRelleno.SelectedItem.Text = value;
            }
        }

        public string WpsSelectedItem
        {
            get
            {
                return ddlWps.SelectedItem.Text;
            }
            set
            {
                ddlWps.SelectedItem.Text = value;
            }
        }

        public bool ProcesorellenoEnabled
        {
            get
            {
                return ddlProcesoRelleno.Enabled;
            }
            set
            {
                ddlProcesoRelleno.Enabled = value;
            }
        }
        public bool WpsRellenoEnabled
        {
            get
            {
                return ddlWps.Enabled;
            }
            set
            {
                ddlWps.Enabled = value;
            }
        }

        public bool ColadaEnabled
        {
            get
            {
                return ddlConsumibles.Enabled;
            }
            set
            {
                ddlConsumibles.Enabled = value;
            }
        }

        public bool btnAgregarEnabled
        {
            get
            {
                return btnAgregar.Enabled;
            }
            set
            {
                btnAgregar.Enabled = value;
            }
        }

        public bool ListadoSoldadoresEnabled
        {
            get
            {
                return rcbSoldador.Enabled;
            }
            set
            {
                rcbSoldador.Enabled = value;
            }
        }

        public bool SoldarConRaiz
        {
            get
            {
                if (ViewState["SoldarConRaiz"] == null)
                {
                    ViewState["SoldarConRaiz"] = false;
                }
                return ViewState["SoldarConRaiz"].SafeBoolParse();
            }
            set
            {
                ViewState["SoldarConRaiz"] = value;
            }
        }

        private bool EdicionEspecial
        {
            get
            {
                return ViewState["EdicionEspecialSoldadora"].SafeBoolParse();
            }
            set
            {
                ViewState["EdicionEspecialSoldadora"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        public void CargaInformacion(int patioID, int proyectoID)
        {
            hdnPatioID.Value = patioID.ToString();
            hdnProyectoID.Value = proyectoID.ToString();
            ddlProcesoRelleno.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerProcesosRelleno());
        }

        /// <summary>
        /// Carga la informacion en los controles y deshabilita la edicion
        /// </summary>
        /// <param name="jwks"></param>
        /// <param name="js"></param>
        public void CargaInformacion(JuntaWorkstatus jwks, JuntaSoldadura js, bool readOnly, string wpsRellenoSelected = "", bool edicionEspecial = false)
        {
            ddlProcesoRelleno.SelectedValue = js.ProcesoRellenoID.ToString();

            IEnumerable<WpsCache> wps = CacheCatalogos.Instance.ObtenerWps().Where(x => x.ID == js.WpsRellenoID);
            if (wps.Any())
            {
                ddlWps.BindToEntiesWithEmptyRow(wps);
                ddlWps.SelectedIndex = 1;
            }

            IEnumerable<GrdSoldadorProceso> soldadores = from jtaSol in js.JuntaSoldaduraDetalle
                                                         where jtaSol.TecnicaSoldadorID == (int)TecnicaSoldadorEnum.Relleno
                                                         select new GrdSoldadorProceso
                                                         {
                                                             CodigoConsumible = jtaSol.Consumible != null ? jtaSol.Consumible.Codigo : string.Empty,
                                                             CodigoSoldador = jtaSol.Soldador.Codigo,
                                                             NombreCompleto = jtaSol.Soldador.Nombre + " " + jtaSol.Soldador.ApPaterno + " " + jtaSol.Soldador.ApMaterno,
                                                             SoldadorID = jtaSol.SoldadorID,
                                                             ConsumibleID = jtaSol.ConsumibleID,
                                                             TipoProceso = jtaSol.TecnicaSoldadorID,
                                                             JuntaSoldaduraDetalleID = jtaSol.JuntaSoldaduraDetalleID
                                                         };
            if (readOnly)
            {
                repSoldadoresReadOnly.DataSource = soldadores;
                repSoldadoresReadOnly.DataBind();
            }
            else
            {
                if (soldadores.Any())
                {
                    SoldadoresList = soldadores.ToList();
                    grdSoldadores.DataSource = soldadores;
                    grdSoldadores.DataBind();
                    grdSoldadores.Visible = true;
                }
            }
            repSoldadoresReadOnly.Visible = readOnly;
            pnlSoldador.Visible = !readOnly;
            pnlWps.Visible = !readOnly;
            ddlProcesoRelleno.Enabled = !readOnly;

            if (edicionEspecial)
            {
                JuntaSpool juntaSpool = JuntaSpoolBO.Instance.Obtener(jwks.JuntaSpoolID);
                EdicionEspecial = edicionEspecial;
                ddlWps.Enabled = true;
                List<FamAceroCache> tm = CacheCatalogos.Instance.ObtenerFamiliasAcero();
                string material1 = juntaSpool.FamiliaAceroMaterial1ID != null ? tm.Single(x => x.ID == juntaSpool.FamiliaAceroMaterial1ID).Nombre : string.Empty;
                string material2 = juntaSpool.FamiliaAceroMaterial2ID != null ? tm.Single(x => x.ID == juntaSpool.FamiliaAceroMaterial2ID).Nombre : string.Empty;
                CargaWPS(jwks.JuntaSpoolID, js.ProcesoRaizID.SafeIntParse(), js.ProcesoRellenoID.SafeIntParse(), material1, material2, juntaSpool.Espesor, false);
                if (wpsRellenoSelected != "")
                {
                    ddlWps.SelectedValue = wpsRellenoSelected;
                }
            }

        }

        public void UnBindInformacion(TrackableCollection<JuntaSoldaduraDetalle> listado, List<int> listadoParaBorrar)
        {
            //ValidacionesSoldador.AlMenosUnSoldador(SoldadoresList.Count);
            JuntaSoldaduraDetalle detalle = null;
            foreach (GrdSoldadorProceso soldador in SoldadoresList)
            {
                if (!soldador.JuntaSoldaduraDetalleID.HasValue)
                {
                    detalle = new JuntaSoldaduraDetalle();
                    detalle.SoldadorID = soldador.SoldadorID;
                    detalle.ConsumibleID = soldador.ConsumibleID.SafeIntParse() > 0 ? soldador.ConsumibleID : null;
                    if (SoldarConRaiz)
                    {
                        detalle.TecnicaSoldadorID = 2;
                    }
                    else
                    {
                        detalle.TecnicaSoldadorID = soldador.TipoProceso;
                    }
                    detalle.UsuarioModifica = SessionFacade.UserId;
                    detalle.FechaModificacion = DateTime.Now;
                    listado.Add(detalle);
                }
            }

            foreach (int id in SoldaduraDetalleID)
            {
                listadoParaBorrar.Add(id);
                JuntaSoldaduraDetalle SoldaduraDetalle = listado.Where(x => x.JuntaSoldaduraDetalleID == id).SingleOrDefault();
                listado.Remove(SoldaduraDetalle);
            }

        }

        protected void grdSoldadores_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            grdSoldadores.DataSource = SoldadoresList;
        }

        public void AgregaSoldadorRaiz()
        {
            grdSoldadores.DataSource = SoldadoresList;
            grdSoldadores.DataBind();
            grdSoldadores.Visible = true;

            foreach (RepeaterItem item in grdSoldadores.Items)
            {
                if (item.FindControl("lnkBorrar") != null)
                {
                    ((LinkButton)item.FindControl("lnkBorrar")).Enabled = false;
                    ((LinkButton)item.FindControl("lnkBorrar")).Visible = false;
                }
            }
        }

        public void LimpiarGridSoldadores()
        {
            SoldadoresList = new List<GrdSoldadorProceso>();
            List<GrdSoldadorProceso> listaVacia = new List<GrdSoldadorProceso>();
            grdSoldadores.DataSource = listaVacia;
            grdSoldadores.DataBind();
            grdSoldadores.Visible = false;
        }


        protected void grdSoldadores_ItemCommand(object sender, CommandEventArgs e)
        {
            string[] argumentos = e.CommandArgument.ToString().Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
            int soldadorID = argumentos[0].SafeIntParse();
            int consumibleID = argumentos[1].SafeIntParse();

            if (e.CommandName == "Borrar")
            {
                GrdSoldadorProceso s = SoldadoresList.Where(x => x.SoldadorID == soldadorID && x.ConsumibleID == consumibleID).SingleOrDefault();
                if (s.JuntaSoldaduraDetalleID.HasValue)
                {
                    SoldaduraDetalleID.Add(s.JuntaSoldaduraDetalleID.Value);
                }
                SoldadoresList.Remove(s);
                grdSoldadores.DataSource = SoldadoresList;
                grdSoldadores.DataBind();
            }
        }

        /// <summary>
        /// se 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregar_OnClick(object sender, EventArgs e)
        {
            Soldador sold = SoldadorBO.Instance.Obtener(rcbSoldador.SelectedValue.SafeIntParse());
            int consumibleID = ddlConsumibles.SelectedValue.SafeIntParse();

            try
            {
                ValidacionesSoldador.CodigoYConsumibleDuplicados(sold.Codigo, consumibleID, SoldadoresList);


                GrdSoldadorProceso soldador = new GrdSoldadorProceso();
                soldador.SoldadorID = sold.SoldadorID;
                soldador.CodigoSoldador = sold.Codigo;
                soldador.NombreCompleto = sold.Nombre + " " + sold.ApPaterno + " " + sold.ApMaterno;
                soldador.CodigoConsumible = ddlConsumibles.Text;
                soldador.ConsumibleID = ddlConsumibles.SelectedValue.SafeIntParse();
                soldador.TipoProceso = (int)TecnicaSoldadorEnum.Relleno;

                SoldadoresList.Add(soldador);

                grdSoldadores.DataSource = SoldadoresList;
                grdSoldadores.DataBind();

                ddlConsumibles.Text = string.Empty;
                rcbSoldador.Text = string.Empty;

                grdSoldadores.Visible = true;
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex, "valRelleno");
            }

        }

        protected void ddlProcesoRelleno_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProcesoRellenoSeleccionado != null)
            {
                ProcesoRellenoSeleccionado(sender, e);
            }
        }

        protected void ddlWps_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (WpsSeleccionado != null)
            {
                WpsSeleccionado(sender, e);
            }
        }

        protected void RenderErrors(BaseValidationException ex, string validationGroup)
        {
            foreach (string detail in ex.Details)
            {
                var cv = new CustomValidator
                {
                    ErrorMessage = detail,
                    IsValid = false,
                    Display = ValidatorDisplay.None,
                    ValidationGroup = validationGroup
                };
                Page.Form.Controls.Add(cv);
            }
        }

        protected void cusRcbSoldador_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = rcbSoldador.SelectedValue.SafeIntParse() > 0;
        }

        public void CargaWPS(int juntaSpoolID, int procesoRaizID, string material1, string material2, decimal? espesorJunta, string psRaiz)
        {
            if (procesoRaizID > 0)
            {
                IEnumerable<WpsCache> origen = SoldaduraBL.Instance.ObtenerWpsterminadoConRaiz(
                                                juntaSpoolID,
                                                hdnProyectoID.Value.SafeIntParse(),
                                                procesoRaizID,
                                                material1,
                                                material2,
                                                espesorJunta);
                espesorJunta = espesorJunta.GetValueOrDefault(0);
                ddlWps.Enabled = true;
                ddlWps.BindToEntiesWithEmptyRow(origen);

                int proceso = SoldaduraBL.Instance.ObtenerProcesoRelleno(psRaiz);

                Session["SoldadorProcesoRellenoID"] = proceso;
            }
        }

        public void CargaWPS(int juntaSpoolID, int procesoRaizID, int procesoRellenoID, string material1, string material2, decimal? espesorJunta, bool wpsDiferentes)
        {
            if (wpsDiferentes)
            {
                if (procesoRellenoID > 0)
                {
                    IEnumerable<WpsCache> origen = SoldaduraBL.Instance.ObtenerWps(
                                                     juntaSpoolID,
                                                     hdnProyectoID.Value.SafeIntParse(),
                                                     procesoRaizID,
                                                     procesoRellenoID,
                                                     material1,
                                                     material2,
                                                     espesorJunta,
                                                     wpsDiferentes,
                                                     false);
                    ddlWps.Enabled = true;
                    ddlWps.BindToEntiesWithEmptyRow(origen);
                }
            }
            else
            {
                if (procesoRellenoID > 0 && procesoRaizID > 0)
                {
                    IEnumerable<WpsCache> origen = SoldaduraBL.Instance.ObtenerWps(
                                                     juntaSpoolID,
                                                     hdnProyectoID.Value.SafeIntParse(),
                                                     procesoRaizID,
                                                     procesoRellenoID,
                                                     material1,
                                                     material2,
                                                     espesorJunta,
                                                     wpsDiferentes,
                                                     false);
                    ddlWps.Enabled = true;
                    ddlWps.BindToEntiesWithEmptyRow(origen);
                }
            }
        }

        public void LimpiaCombos()
        {
            ddlProcesoRelleno.Items.Clear();
            ddlProcesoRelleno.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerProcesosRelleno());
            ddlWps.Items.Clear();
        }

        internal bool validaGridSoldadores()
        {
            if (grdSoldadores.Items.Count > 0)
            {
                return true;
            }

            return false;
        }

        internal bool validaProcesoRelleno()
        {
            if (string.IsNullOrEmpty(ProcesoRellenoSelectedItem))
            {
                return false;
            }

            return true;
        }
   }
}