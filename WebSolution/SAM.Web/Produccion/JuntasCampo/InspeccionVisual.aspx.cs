using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.Web.Common;
using Telerik.Web.UI;
using SAM.Entities;
using Mimo.Framework.Extensions;
using SAM.Entities.Personalizadas;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Validations;
using Mimo.Framework.Exceptions;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Produccion;
using System.Globalization;

namespace SAM.Web.Produccion.JuntasCampo
{
    public partial class InspeccionVisual : SamPaginaPopup
    {
        /// <summary>
        /// JS para confirmar que se desea eliminar una congelado parcial.
        /// </summary>
        private const string JS_BORRAR = "return Sam.Confirma(30,'{0}');";

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

        private List<Defecto> Defectos
        {
            get
            {
                if (ViewState["Defectos"] == null)
                {
                    ViewState["Defectos"] = new List<Defecto>();
                }

                return (List<Defecto>)ViewState["Defectos"];
            }
            set
            {
                ViewState["Defectos"] = value;
            }
        }

        private string FechasSoldadura
        {
            get
            {
                if (ViewState["FechasSoldadura"] == null)
                {
                    ViewState["FechasSoldadura"] = string.Empty;
                }

                return ViewState["FechasSoldadura"].ToString();
            }
            set
            {
                ViewState["FechasSoldadura"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                JuntaSpoolID = Request.QueryString["JuntaSpoolID"].SafeIntParse();
                cargaDatos(JuntaSpoolID);
            }
        }

        protected void cargaDatos(int juntaSpoolID)
        {
            if (juntaSpoolID > 0)
            {
                FechasSoldadura = string.Empty;
                JuntasCampoDTO datos = JuntaCampoBO.Instance.DetalleJunta(juntaSpoolID);
                litSpool.Text = datos.Spool;
                litJunta.Text = datos.Junta;
                litTipoJunta.Text = datos.TipoJunta;
                litNumeroControl.Text = datos.NumeroControl;
                litLocalizacion.Text = datos.EtiquetaMaterial1 + "-" + datos.EtiquetaMaterial2;
                litEspesor.Text = datos.Espesor.SafeStringParse();

                JuntaCampoID = datos.JuntaCampoID;

                if (JuntaCampoID > -1)
                {
                    FechasSoldadura = ValidaFechasJuntasCampoBO.Instance.ObtenerFechasSoldaduraJuntasCampo(JuntaCampoID);
                }

                if (FechasSoldadura != string.Empty)
                {
                    mdpFechaProcesoAnterior.SelectedDate = ValidaFechasJuntasCampoBO.Instance.ObtenerFechaJuntaCampoSoldadura(JuntaCampoID);
                    mdpFechaReporteProcesoAnterior.SelectedDate = ValidaFechasJuntasCampoBO.Instance.ObtenerFechaJuntaCampoSoldaduraReporte(JuntaCampoID);
                    string fechasArmado = ValidaFechasJuntasCampoBO.Instance.ObtenerFechasArmadoJuntasCampo(JuntaCampoID);
                    btnGuardar.OnClientClick = "return Sam.Workstatus.CambioFechas('" + rdwCambiarFechaProcesoAnterior.ClientID + "', '" + FechasSoldadura + "','2')";
                    btnGuardarPopUp.OnClientClick = "return Sam.Produccion.ValidaNuevasFechas('1','" + mdpFechaInspeccionVisual.ClientID + "','" + fechasArmado + "')";
                    hdnCambiaFechas.Value = "0";
                }

                hdnProyectoID.Value = datos.ProyectoID.ToString();

                ddlDefecto.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerDefectos().Where(x => x.TipoPruebaID == (int)TipoPruebaEnum.InspeccionVisual));

                cargaReportes();
            }
        }


        protected void ddlResultado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlResultado.SelectedValue.SafeIntParse() == 0)
            {
                pnlDefecto.Visible = true;
                pnlDefecto2.Visible = true;
            }
            else
            {
                pnlDefecto.Visible = false;
                pnlDefecto2.Visible = false;
            }
        }

        protected void btnDefecto_Click(object sender, EventArgs e)
        {
            try
            {
                Defecto def = new Defecto
                {
                    Nombre = ddlDefecto.SelectedItem.Text,
                    DefectoID = ddlDefecto.SelectedValue.SafeIntParse()
                };

                if (!ValidacionesDefecto.ExistenDuplicados(Defectos, def))
                {
                    Defectos.Add(def);
                    repDefectos.DataSource = Defectos;
                    repDefectos.DataBind();

                    ddlDefecto.SelectedIndex = -1;
                }
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex, "defecto");
            }
        }

        protected void repDefectos_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                for (int count = 0; count <= Defectos.Count; count++)
                {
                    if (Defectos[count].DefectoID == e.CommandArgument.SafeIntParse())
                    {
                        Defectos.RemoveAt(count);
                        break;
                    }
                }
            }

            repDefectos.DataSource = Defectos;
            repDefectos.DataBind();
        }

        protected void btnGuardarPopUp_OnClick(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    JuntaCampo juntaCampo = JuntaCampoBO.Instance.ObtenerPorID(JuntaCampoID);
                    ValidacionesJuntaCampo.ValidaSoldaduraAprobada(juntaCampo);

                    if (hdnCambiaFechas.Value == "1")
                    {
                        JuntaCampoBO.Instance.CambiaFechasJuntaCampoSoldadura(juntaCampo.JuntaCampoSoldaduraID.Value, mdpFechaProcesoAnterior.SelectedDate.Value, mdpFechaReporteProcesoAnterior.SelectedDate.Value, SessionFacade.UserId);
                    }
                    else if (hdnCambiaFechas.Value == "2")
                    {
                        DateTime fechaSoldadura = DateTime.ParseExact(hdnProcesoAnterior2.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        DateTime fechaReporteSoldadura = DateTime.ParseExact(hdnProcesoReporteAnterior2.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        JuntaCampoBO.Instance.CambiaFechasJuntaCampoSoldadura(juntaCampo.JuntaCampoSoldaduraID.Value, fechaSoldadura, fechaReporteSoldadura, SessionFacade.UserId);
                        JuntaCampoBO.Instance.CambiaFechasJuntaCampoArmado(juntaCampo.JuntaCampoArmadoID.Value, mdpFechaProcesoAnterior.SelectedDate.Value, mdpFechaReporteProcesoAnterior.SelectedDate.Value, SessionFacade.UserId);
                    }

                    FechasSoldadura = ValidaFechasJuntasCampoBO.Instance.ObtenerFechasSoldaduraJuntasCampo(JuntaCampoID);

                    btnGuardar.OnClientClick = "return Sam.Workstatus.CambioFechas('" + rdwCambiarFechaProcesoAnterior.ClientID + "', '" + FechasSoldadura + "','2')";

                    InspeccionVisualCampo inspVisual = new InspeccionVisualCampo
                    {
                        ProyectoID = hdnProyectoID.Value.SafeIntParse(),
                        NumeroReporte = txtNumeroReporte.Text,
                        FechaReporte = mdpFechaReporte.SelectedDate.Value
                    };

                    JuntaCampoInspeccionVisual junta = new JuntaCampoInspeccionVisual
                    {
                        FechaInspeccion = mdpFechaInspeccionVisual.SelectedDate.Value,
                        Aprobado = ddlResultado.SelectedValue.SafeIntParse() == 0 ? false : true,
                        Observaciones = txtObservaciones.Text
                    };

                    int[] arrayDefecto = new int[Defectos.Count];
                    int count = 0;

                    foreach (Defecto def in Defectos)
                    {
                        arrayDefecto[count] = Defectos[count].DefectoID;
                        count++;
                    }


                    JuntaCampoBO.Instance.GeneraReporteInspeccionVisual(junta, inspVisual, arrayDefecto, JuntaCampoID, SessionFacade.UserId);

                    cargaReportes();
                    limpiaCampos();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex, "defecto");
                }
            }
        }

        private void cargaReportes()
        {
            List<GrdInspeccionVisualCampo> lista = JuntaCampoBO.Instance.ObtenerListadoInspeccionVisual(JuntaCampoID);

            if (lista != null)
            {
                repVisual.Visible = true;
                repVisual.DataSource = lista;
                repVisual.DataBind();
            }
            else
                repVisual.Visible = false;
        }

        private void limpiaCampos()
        {
            mdpFechaInspeccionVisual.SelectedDate = null;
            mdpFechaReporte.SelectedDate = null;
            txtNumeroReporte.Text = string.Empty;
            txtObservaciones.Text = string.Empty;
            ddlResultado.SelectedIndex = -1;
            pnlDefecto.Visible = false;
            pnlDefecto2.Visible = false;
            Defectos.Clear();
            repDefectos.DataSource = Defectos;
            repDefectos.DataBind();
        }

        protected void repVisual_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int JuntaCampoInspeccionVisualID = e.CommandArgument.SafeIntParse();

                try
                {
                    JuntaCampoBO.Instance.EliminarReporteInspeccionVisual(JuntaCampoInspeccionVisualID);
                    cargaReportes();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void repVisual_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }        
    }
}