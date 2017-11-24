using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.Entities.Personalizadas;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Utilerias;
using Mimo.Framework.Common;
using SAM.BusinessObjects.Proyectos;
using SAM.Entities;
using SAM.BusinessObjects.Validations;
using SAM.Entities.Grid;
using Mimo.Framework.Exceptions;
using SAM.Entities.Cache;
using SAM.BusinessLogic.Workstatus;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Produccion;

namespace SAM.Web.Produccion.JuntasCampo
{
    public partial class Soldadura : SamPaginaPopup
    {

        private int JuntaSpoolID
        {
            get
            {
                return ViewState["JuntaSpoolID"].ToString().SafeIntParse();
            }
            set
            {
                ViewState["JuntaSpoolID"] = value;
            }
        }

        private int JuntaCampoID
        {
            get
            {
                return ViewState["JuntaCampoID"].SafeIntParse();
            }
            set
            {
                ViewState["JuntaCampoID"] = value;
            }
        }

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

        private string FechasArmado
        {
            get
            {
                if (ViewState["FechasArmado"] == null)
                {
                    ViewState["FechasArmado"] = string.Empty;
                }

                return ViewState["FechasArmado"].ToString();
            }
            set
            {
                ViewState["FechasArmado"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                JuntaSpoolID = Request.QueryString["JuntaSpoolID"].SafeIntParse();
                cargaDatos();                
            }
        }

        protected void cargaDatos()
        {
            if (JuntaSpoolID > 0)
            {
                FechasArmado = string.Empty;
                JuntasCampoDTO datos = JuntaCampoBO.Instance.DetalleJunta(JuntaSpoolID);
                litSpool.Text = datos.Spool;
                litJunta.Text = datos.Junta;
                litTipoJunta.Text = datos.TipoJunta;
                litNumeroControl.Text = datos.NumeroControl;
                litLocalizacion.Text = datos.EtiquetaMaterial1 + "-" + datos.EtiquetaMaterial2;
                litEspesor.Text = datos.Espesor.SafeStringParse();

                JuntaCampoID = datos.JuntaCampoID;

                if (JuntaCampoID > -1)
                {
                    FechasArmado = ValidaFechasJuntasCampoBO.Instance.ObtenerFechasArmadoJuntasCampo(JuntaCampoID);
                }

                if (FechasArmado != string.Empty)
                {
                    mdpFechaProcesoAnterior.SelectedDate = ValidaFechasJuntasCampoBO.Instance.ObtenerFechaArmado(JuntaCampoID);
                    mdpFechaReporteProcesoAnterior.SelectedDate = ValidaFechasJuntasCampoBO.Instance.ObtenerFechaReporteArmado(JuntaCampoID);

                    btnGuardar.OnClientClick = "return Sam.Workstatus.CambioFechas('" + rdwCambiarFechaProcesoAnterior.ClientID + "', '" + FechasArmado + "','1')";
                    btnGuardarPopUp.OnClientClick = "return Sam.Produccion.ValidaNuevasFechas('0','" + mdpFechaSoldadura.ClientID + "')";
                    hdnCambiaFechas.Value = "0";
                }

                Proyecto proyecto = ProyectoBO.Instance.Obtener(datos.ProyectoID);
                hdnPatioID.Value = proyecto.PatioID.ToString();
                hdnProyectoID.Value = proyecto.ProyectoID.ToString();

                JuntaCampo juntaCampo = JuntaCampoBO.Instance.ObtenerPorID(JuntaCampoID);
                JuntaCampoArmado jtaArmado = JuntaCampoBO.Instance.ObtenerArmado(JuntaCampoID);

                if (jtaArmado != null)
                {                    
                    txtSpool1.Text = jtaArmado.Spool.Nombre;
                    txtSpool2.Text = jtaArmado.Spool1.Nombre;
                    txtFamiliaAcero1.Text = CacheCatalogos.Instance.ObtenerFamiliasAcero().Where(x => x.ID == juntaCampo.JuntaSpool.FamiliaAceroMaterial1ID).Single().Nombre;

                    if (juntaCampo.JuntaSpool.FamiliaAceroMaterial2ID != null)
                    {
                        txtFamiliaAcero2.Text = CacheCatalogos.Instance.ObtenerFamiliasAcero().Where(x => x.ID == juntaCampo.JuntaSpool.FamiliaAceroMaterial2ID).Single().Nombre;
                    }
                    else
                    {
                        txtFamiliaAcero2.Text = txtFamiliaAcero1.Text;
                    }
                }
            }


            ddlProcesoRaiz.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerProcesosRaiz());
            ddlProcesoRelleno.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerProcesosRelleno());
            ddlProceso.Items.Add(new ListItem("", "-1"));
            ddlProceso.Items.Add(new ListItem(Cultura == LanguageHelper.INGLES ? "Root" : "Raíz", "1"));
            ddlProceso.Items.Add(new ListItem(Cultura == LanguageHelper.INGLES ? "Fill" : "Relleno", "2"));

            JuntaCampoSoldadura jtaSoldadura = JuntaCampoBO.Instance.ObtenerSoldadura(JuntaCampoID);
            if (jtaSoldadura != null)
            {
                cargaInformacionReadOnly(jtaSoldadura);
            }
        }
        
        private void cargaInformacionReadOnly(JuntaCampoSoldadura jtaSoldadura)
        {
            string wpsRaiz = CacheCatalogos.Instance.ObtenerWps().Where(x => x.ID == jtaSoldadura.WpsRaizID).Single().Nombre;
            string wpsRelleno = CacheCatalogos.Instance.ObtenerWps().Where(x => x.ID == jtaSoldadura.WpsRellenoID).Single().Nombre;
            mdpFechaReporte.SelectedDate = jtaSoldadura.FechaReporte;
            mdpFechaSoldadura.SelectedDate = jtaSoldadura.FechaSoldadura;
            ddlProcesoRaiz.SelectedValue = jtaSoldadura.ProcesoRaizID.ToString();
            ddlProcesoRelleno.SelectedValue = jtaSoldadura.ProcesoRellenoID.ToString();
            ddlWpsRaiz.Items.Clear();
            ddlWpsRaiz.Items.Add(wpsRaiz);
            ddlWpsRaiz.SelectedIndex = 0;
            ddlWpsRelleno.Items.Clear();
            ddlWpsRelleno.Items.Add(wpsRelleno);
            ddlWpsRelleno.SelectedIndex = 0;
            txtObservaciones.Text = jtaSoldadura.Observaciones;

            vistaReadOnly(true);

            IEnumerable<GrdSoldadorProceso> soldadores = from jtaSol in jtaSoldadura.JuntaCampoSoldaduraDetalle                                                         
                                                         select new GrdSoldadorProceso
                                                         {
                                                             CodigoConsumible = jtaSol.Consumible.Codigo,
                                                             CodigoSoldador = jtaSol.Soldador.Codigo,
                                                             NombreCompleto = jtaSol.Soldador.Nombre + " " + jtaSol.Soldador.ApPaterno + " " + jtaSol.Soldador.ApMaterno,
                                                             Proceso = obtenProceso((TecnicaSoldadorEnum)jtaSol.TecnicaSoldadorID)
                                                         };

            repSoldadoresReadOnly.DataSource = soldadores;
            repSoldadoresReadOnly.DataBind();

            repSoldadoresReadOnly.Visible = true;
          
        }

        public void CargaWPS(int procesoRaizID, int procesoRellenoID)
        {

            if (chkWpsDiferentes.Checked)
            {
                if (procesoRaizID > 0)
                {

                    IEnumerable<WpsCache> origen = SoldaduraBL.Instance.ObtenerWps(
                                                    JuntaSpoolID,
                                                    hdnProyectoID.Value.SafeIntParse(),
                                                    procesoRaizID,
                                                    procesoRellenoID,
                                                    txtFamiliaAcero1.Text,
                                                    txtFamiliaAcero2.Text,
                                                    lblEspesor.Text.SafeDecimalNullableParse(),
                                                    chkWpsDiferentes.Checked,
                                                    true);
                    ddlWpsRaiz.Enabled = true;
                    ddlWpsRaiz.BindToEntiesWithEmptyRow(origen);

                }

                if (procesoRellenoID > 0)
                {
                    IEnumerable<WpsCache> origen = SoldaduraBL.Instance.ObtenerWps(
                                                     JuntaSpoolID,
                                                     hdnProyectoID.Value.SafeIntParse(),
                                                     procesoRaizID,
                                                     procesoRellenoID,
                                                     txtFamiliaAcero1.Text,
                                                    txtFamiliaAcero2.Text,
                                                    lblEspesor.Text.SafeDecimalNullableParse(),
                                                    chkWpsDiferentes.Checked,
                                                     false);
                    ddlWpsRelleno.Enabled = true;
                    ddlWpsRelleno.BindToEntiesWithEmptyRow(origen);
                }
            }
            else
            {
                if (procesoRaizID > 0 && procesoRellenoID > 0)
                {

                    IEnumerable<WpsCache> origen = SoldaduraBL.Instance.ObtenerWps(
                                                    JuntaSpoolID,
                                                    hdnProyectoID.Value.SafeIntParse(),
                                                    procesoRaizID,
                                                    procesoRellenoID,
                                                    txtFamiliaAcero1.Text,
                                                    txtFamiliaAcero2.Text,
                                                    lblEspesor.Text.SafeDecimalNullableParse(),
                                                    chkWpsDiferentes.Checked,
                                                    true);
                    ddlWpsRaiz.Enabled = true;
                    ddlWpsRaiz.BindToEntiesWithEmptyRow(origen);

                    origen = SoldaduraBL.Instance.ObtenerWps(
                                                     JuntaSpoolID,
                                                     hdnProyectoID.Value.SafeIntParse(),
                                                     procesoRaizID,
                                                     procesoRellenoID,
                                                      txtFamiliaAcero1.Text,
                                                    txtFamiliaAcero2.Text,
                                                    lblEspesor.Text.SafeDecimalNullableParse(),
                                                    chkWpsDiferentes.Checked,
                                                     false);
                    ddlWpsRelleno.Enabled = true;
                    ddlWpsRelleno.BindToEntiesWithEmptyRow(origen);

                }
            }
        }

        protected void ddlProcesoRaiz_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProcesoRaiz.SelectedValue.SafeIntParse() > 0 && ddlProcesoRelleno.SelectedValue.SafeIntParse() > 0)
            {
                CargaWPS(ddlProcesoRaiz.SelectedValue.SafeIntParse(), ddlProcesoRelleno.SelectedValue.SafeIntParse());
            }
        }

        protected void ddlProcesoRelleno_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProcesoRaiz.SelectedValue.SafeIntParse() > 0 && ddlProcesoRelleno.SelectedValue.SafeIntParse() > 0)
            {
                CargaWPS(ddlProcesoRaiz.SelectedValue.SafeIntParse(), ddlProcesoRelleno.SelectedValue.SafeIntParse());
            }
        }

        protected void chkWpsDiferentes_OnCheckedChanged(object sender, EventArgs e)
        {
            if (ddlProcesoRaiz.SelectedValue.SafeIntParse() > 0 && ddlProcesoRelleno.SelectedValue.SafeIntParse() > 0)
            {
                CargaWPS(ddlProcesoRaiz.SelectedValue.SafeIntParse(), ddlProcesoRelleno.SelectedValue.SafeIntParse());
            }
        }


        protected void cusRcbSoldador_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = ddlConsumibles.SelectedValue.SafeIntParse() > 0;
        }

        protected void cusRcbConsumibles_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = ddlConsumibles.SelectedValue.SafeIntParse() > 0;
        }


        protected void btnAgregar_OnClick(object sender, EventArgs e)
        {
            Soldador sold = SoldadorBO.Instance.Obtener(rcbSoldador.SelectedValue.SafeIntParse());

            try
            {
                ValidacionesSoldador.CodigoDuplicado(sold.Codigo, ddlProceso.SelectedValue.SafeIntParse(), SoldadoresList);

                if (ddlProceso.SelectedValue.SafeIntParse() == (int)TecnicaSoldadorEnum.Raiz)
                {
                    ValidacionesSoldador.ValidaWpq(sold.SoldadorID, ddlWpsRaiz.SelectedValue.SafeIntParse(), mdpFechaSoldadura.SelectedDate.Value);
                }
                else
                {
                    ValidacionesSoldador.ValidaWpq(sold.SoldadorID, ddlWpsRelleno.SelectedValue.SafeIntParse(), mdpFechaSoldadura.SelectedDate.Value);
                }

                GrdSoldadorProceso soldador = new GrdSoldadorProceso();
                soldador.SoldadorID = sold.SoldadorID;
                soldador.CodigoSoldador = sold.Codigo;
                soldador.NombreCompleto = sold.Nombre + " " + sold.ApPaterno + " " + sold.ApMaterno;
                soldador.CodigoConsumible = ddlConsumibles.Text;
                soldador.ConsumibleID = ddlConsumibles.SelectedValue.SafeIntParse();
                soldador.TipoProceso = ddlProceso.SelectedValue.SafeIntParse();
                soldador.Proceso = obtenProceso((TecnicaSoldadorEnum)soldador.TipoProceso);

                SoldadoresList.Add(soldador);

                grdSoldadores.DataSource = SoldadoresList;
                grdSoldadores.DataBind();

                ddlConsumibles.Text = string.Empty;
                rcbSoldador.Text = string.Empty;

                grdSoldadores.Visible = true;
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex, "soldadores");
            }

        }


        private string obtenProceso(TecnicaSoldadorEnum tipoProceso)
        {
            switch (tipoProceso)
            {
                case TecnicaSoldadorEnum.Raiz:
                    return Cultura == LanguageHelper.ESPANOL ? "Raíz" : "Root";
                case TecnicaSoldadorEnum.Relleno:
                    return Cultura == LanguageHelper.ESPANOL ? "Relleno" : "Fill";
            }

            return string.Empty;
        }

        protected void grdSoldadores_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            int soldadorID = e.CommandArgument.SafeIntParse();
            int proceso = e.CommandName.SafeIntParse();

            GrdSoldadorProceso s = SoldadoresList.Where(x => x.SoldadorID == soldadorID && x.TipoProceso == proceso).SingleOrDefault();
            SoldadoresList.Remove(s);
            grdSoldadores.DataSource = SoldadoresList;
            grdSoldadores.DataBind();

        }

        protected void btnGuardarPopUp_OnClick(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    JuntaCampo junta = JuntaCampoBO.Instance.ObtenerPorID(JuntaCampoID);
                    ValidacionesJuntaCampo.ValidaArmadoAprobado(junta);

                    if (hdnCambiaFechas.Value == "1")
                    {
                        JuntaCampoBO.Instance.CambiaFechasJuntaCampoArmado(junta.JuntaCampoArmadoID.Value, mdpFechaProcesoAnterior.SelectedDate.Value, mdpFechaReporteProcesoAnterior.SelectedDate.Value, SessionFacade.UserId);
                    }
                    FechasArmado = ValidaFechasJuntasCampoBO.Instance.ObtenerFechasArmadoJuntasCampo(JuntaCampoID);

                    btnGuardar.OnClientClick = "return Sam.Workstatus.CambioFechas('" + rdwCambiarFechaProcesoAnterior.ClientID + "', '" + FechasArmado + "','1')";
                    junta.StartTracking();
                    JuntaCampoSoldadura juntaSoldadura = new JuntaCampoSoldadura();
                    juntaSoldadura.FechaSoldadura = mdpFechaSoldadura.SelectedDate.Value;
                    juntaSoldadura.FechaReporte = mdpFechaReporte.SelectedDate.Value;
                    juntaSoldadura.ProcesoRaizID = ddlProcesoRaiz.SelectedValue.SafeIntParse();
                    juntaSoldadura.ProcesoRellenoID = ddlProcesoRelleno.SelectedValue.SafeIntParse();
                    juntaSoldadura.WpsRaizID = ddlWpsRaiz.SelectedValue.SafeIntParse();
                    juntaSoldadura.WpsRellenoID = ddlWpsRelleno.SelectedValue.SafeIntParse();
                    juntaSoldadura.Observaciones = txtObservaciones.Text;
                    juntaSoldadura.UsuarioModifica = SessionFacade.UserId;
                    juntaSoldadura.FechaModificacion = DateTime.Now;

                    foreach (GrdSoldadorProceso soldador in SoldadoresList)
                    {
                        JuntaCampoSoldaduraDetalle detalle = new JuntaCampoSoldaduraDetalle();
                        detalle.SoldadorID = soldador.SoldadorID;
                        detalle.ConsumibleID = soldador.ConsumibleID.SafeIntParse();
                        detalle.TecnicaSoldadorID = soldador.TipoProceso;
                        detalle.UsuarioModifica = SessionFacade.UserId;
                        detalle.FechaModificacion = DateTime.Now;

                        juntaSoldadura.JuntaCampoSoldaduraDetalle.Add(detalle);
                    }
                    ValidacionesJuntaCampo.ValidaSoldadores(juntaSoldadura);

                    junta.SoldaduraAprobada = true;
                    junta.UltimoProcesoID = (int)UltimoProcesoEnum.Soldado;
                    junta.UsuarioModifica = SessionFacade.UserId;
                    junta.FechaModificacion = DateTime.Now;

                    JuntaCampoBO.Instance.GuardaSoldadura(junta, juntaSoldadura);

                    //limpiaCampos();
                    cargaDatos();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void btnEliminar_Clici(object sender, EventArgs e)
        {
            try
            {
                JuntaCampoBO.Instance.BorraSoldadura(JuntaCampoID, SessionFacade.UserId);
                vistaReadOnly(false);
                limpiaCampos();
                cargaDatos();
                grdSoldadores.Visible = false;
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
                        
        }

        private void vistaReadOnly(bool readOnly)
        {
            mdpFechaReporte.Enabled = !readOnly;
            mdpFechaSoldadura.Enabled = !readOnly;
            ddlProcesoRaiz.Enabled = !readOnly;
            ddlProcesoRelleno.Enabled = !readOnly;
            chkWpsDiferentes.Enabled = !readOnly;
            ddlWpsRaiz.Enabled = !readOnly;
            ddlWpsRelleno.Enabled = !readOnly;
            pnlChecks.Visible = !readOnly;
            pnlChecks2.Visible = !readOnly;
            pnlSoldador.Visible = !readOnly;
            repSoldadoresReadOnly.Visible = readOnly;
            grdSoldadores.Visible = !readOnly;
            txtObservaciones.ReadOnly = readOnly;
            btnGuardar.Visible = !readOnly;
            btnEliminar.Visible = readOnly;
        }

        private void limpiaCampos()
        {
            mdpFechaReporte.SelectedDate = null;
            mdpFechaSoldadura.SelectedDate = null;
            ddlProcesoRaiz.SelectedIndex = -1;
            ddlProcesoRelleno.SelectedIndex = -1;
            chkWpsDiferentes.Checked = false;
            ddlProceso.Items.Clear();
            ddlWpsRaiz.Items.Clear();
            ddlWpsRelleno.Items.Clear();
            txtObservaciones.Text = string.Empty;
        }
    }
}