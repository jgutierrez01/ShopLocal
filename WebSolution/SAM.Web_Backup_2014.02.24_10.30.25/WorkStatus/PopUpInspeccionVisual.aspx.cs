using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Entities;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Validations;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Workstatus;
using System.Globalization;
using Mimo.Framework.Common;

namespace SAM.Web.WorkStatus
{
    public partial class PopUpInspeccionVisual : SamPaginaPopup
    {
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
        private string IDs
        {
            get
            {
                if (ViewState["IDs"] == null)
                {
                    ViewState["IDs"] = string.Empty;
                }

                return ViewState["IDs"].ToString();
            }
            set
            {
                ViewState["IDs"] = value;
            }
        }
        private string Fechas
        {
            get
            {
                if (ViewState["Fechas"] == null)
                {
                    ViewState["Fechas"] = string.Empty;
                }

                return ViewState["Fechas"].ToString();
            }
            set
            {
                ViewState["Fechas"] = value;
            }
        }
        private string FechasReporte
        {
            get
            {
                if (ViewState["FechasReporte"] == null)
                {
                    ViewState["FechasReporte"] = string.Empty;
                }

                return ViewState["FechasReporte"].ToString();
            }
            set
            {
                ViewState["FechasReporte"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IDs = Request.QueryString["IDs"];

                int[] juntaWorkstatusIds = IDs.Split(',').Select(n => n.SafeIntParse()).ToArray();

                if (!SeguridadQs.TieneAccesoATodosLasJuntasWorkstatus(juntaWorkstatusIds))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando inspeccionar visualmente alguna junta workstatus {1} a la cual no tiene permisos", SessionFacade.UserId, IDs);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                if (juntaWorkstatusIds.Count(x => x > 0) != 0)
                {
                    int[] juntasWorkstatusIDsReales = juntaWorkstatusIds.Where(x => x > 0).ToArray();
                    Fechas = ValidaFechasBO.Instance.ObtenerFechasProcesoSoldadura(juntasWorkstatusIDsReales);
                    FechasReporte = ValidaFechasBO.Instance.ObtenerFechasReporteSoldadura(juntasWorkstatusIDsReales);

                    string juntas = ValidaFechasBO.Instance.ObtenerJuntasPorJuntaWorkstatusIds(juntasWorkstatusIDsReales);
                    string numerosControl = ValidaFechasBO.Instance.ObtenerNumerosControlPorJuntaWorkStatusIds(juntasWorkstatusIDsReales);

                    btnGenerar.OnClientClick = "return Sam.Workstatus.ValidaFechasInspeccionVisual('" + numerosControl + "', '" + juntas + "' ,'" + Fechas + "','" + FechasReporte + "')";
                }

                cargaCombos();

            }
        }

        private void cargaCombos()
        {
            ddlDefecto.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerDefectos().Where(x => x.TipoPruebaID == (int)TipoPruebaEnum.InspeccionVisual));
        }

        /// <summary>
        /// Si el resultado es rechazado se mostrará el panel de defectos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlResultado_IndexChanged(object sender, EventArgs e)
        {
            if (ddlResultado.SelectedValue.SafeIntParse() == 0)
            {
                pnDefectos.Visible = true;
            }
            else
            {
                pnDefectos.Visible = false;
            }
        }

        /// <summary>
        /// Agrega el defecto seleccionado al listado de defectos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregar_Click(object sender, EventArgs e)
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
                RenderErrors(ex, "vgDefecto");
            }
        }

        /// <summary>
        /// Elimina el defecto enviado como argumento del grid de defectos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Genera el reporte de inspeccion visual para las juntas seleccionadas en el grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                List<string> juntas = IDs.Split(',').ToList();
                juntas.RemoveAll(new System.Predicate<string>(delegate(string val) { return (val == ""); }));

                IDs = string.Join(",", juntas);

                InspeccionVisual inspVisual = new InspeccionVisual
                {
                    ProyectoID = EntityID.Value,
                    NumeroReporte = txtNumeroReporte.Text,
                    FechaReporte = rdpFechaReporte.SelectedDate.Value
                };

                JuntaInspeccionVisual junta = new JuntaInspeccionVisual
                {
                    FechaInspeccion = rdpFechaInspeccion.SelectedDate.Value,
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

                try
                {
                    InspeccionVisualBO.Instance.GeneraReporte(junta, inspVisual, arrayDefecto, IDs, SessionFacade.UserId);

                    //Actualiza el grid de la ventana padre para que refleje que el reporte ya se generó
                    JsUtils.RegistraScriptActualizaGridGenerico(this);

                    lnkReporte.ProyectoID = EntityID.Value;
                    lnkReporte.NombresParametrosReporte = "NumeroReporte";
                    lnkReporte.ValoresParametrosReporte = txtNumeroReporte.Text;
                    lnkReporte.Tipo = TipoReporteProyectoEnum.InspeccionVisual;

                    phControles.Visible = false;
                    phMensajeExito.Visible = true;
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex, "vgGenerar");
                }
            }

        }
    }
}