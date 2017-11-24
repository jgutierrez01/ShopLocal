using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities.Grid;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Validations;
using SAM.Entities;
using SAM.Entities.Parciales;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Exceptions;
using SAM.Web.Classes;
using SAM.BusinessLogic.Workstatus;
using SAM.Entities.Cache;

namespace SAM.Web.Controles.Workstatus.Soldadura
{
    public partial class SoldadorRaiz : System.Web.UI.UserControl
    {
        public event EventHandler ProcesoRaizSeleccionado;
        public event EventHandler WpsSeleccionado;

        private List<GrdSoldadorProceso> SoldadoresList
        {
            get
            {
                if (ViewState["Soldadores"] == null)
                {
                    ViewState["Soldadores"] = new List<GrdSoldadorProceso>();
                }

                return (List<GrdSoldadorProceso>)ViewState["Soldadores"];
            }
            set
            {
                ViewState["Soldadores"] = value;
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

        public int ProcesoRaiz
        {
            get
            {
                return ddlProcesoRaiz.SelectedValue.SafeIntParse();
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

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Carga la informacion necesaria del control
        /// </summary>
        /// <param name="patioID">ID del patio</param>
        /// <param name="proyectoID">ID del proyecto</param>
        public void CargaInformacion(int patioID, int proyectoID)
        {
            hdnPatioID.Value = patioID.ToString();
            hdnProyectoID.Value = proyectoID.ToString();
            ddlProcesoRaiz.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerProcesosRaiz());
        }

        /// <summary>
        /// Carga la informacion en los controles y deshabilita la edicion
        /// </summary>
        /// <param name="jwks"></param>
        /// <param name="js"></param>
        public void CargaInformacion(JuntaWorkstatus jwks, JuntaSoldadura js, bool readOnly)
        {
            ddlProcesoRaiz.SelectedValue = js.ProcesoRaizID.ToString();

            IEnumerable<WpsCache> wps = CacheCatalogos.Instance.ObtenerWps().Where(x => x.ID == js.WpsID);
            if (wps.Any())
            {
                ddlWps.BindToEntiesWithEmptyRow(wps);
                ddlWps.SelectedIndex = 1;
            }

            IEnumerable<GrdSoldadorProceso> soldadores = from jtaSol in js.JuntaSoldaduraDetalle
                                                         where jtaSol.TecnicaSoldadorID == (int)TecnicaSoldadorEnum.Raiz
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
            pnlWps.Visible = !readOnly;
            pnlSoldador.Visible = !readOnly;
            ddlProcesoRaiz.Enabled = !readOnly;

        }

        /// <summary>
        /// Llena el listado con los detalles de los soldadores agregados en el grid
        /// </summary>
        /// <param name="listado">Listado de soldadores</param>
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
                    detalle.TecnicaSoldadorID = soldador.TipoProceso;
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

        /// <summary>
        /// Metodo que se levanta al borrar un soldador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSoldadores_ItemCommand(object sender, CommandEventArgs e)
        {
            string[] argumentos = e.CommandArgument.ToString().Split(new string[] {"::"}, StringSplitOptions.RemoveEmptyEntries);
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
        /// Metodo que agrega un soldador al listado
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
                soldador.ConsumibleID = consumibleID;
                soldador.TipoProceso = (int)TecnicaSoldadorEnum.Raiz;                

                SoldadoresList.Add(soldador);

                grdSoldadores.DataSource = SoldadoresList;
                grdSoldadores.DataBind();

                ddlConsumibles.Text = string.Empty;
                rcbSoldador.Text = string.Empty;

                grdSoldadores.Visible = true;
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex, "valRaiz");
            }

        }

        protected void ddlProcesoRaiz_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProcesoRaizSeleccionado != null)
            {
                ProcesoRaizSeleccionado(sender, e);
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

        public void CargaWPS(int juntaSpoolID, int procesoRaizID, int procesoRellenoID, string material1, string material2, decimal? espesorJunta, bool wpsDiferentes)
        {
            if (wpsDiferentes)
            {
                if (procesoRaizID > 0)
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
                                                    true);
                    ddlWps.Enabled = true;
                    ddlWps.BindToEntiesWithEmptyRow(origen);

                }
            }
            else
            {
                if (procesoRaizID > 0 && procesoRellenoID > 0)
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
                                                    true);
                    ddlWps.Enabled = true;
                    ddlWps.BindToEntiesWithEmptyRow(origen);

                }
            }
        }

        public void LimpiaCombos()
        {
            ddlProcesoRaiz.SelectedIndex = -1;
            ddlWps.Items.Clear();
        }
    }
}