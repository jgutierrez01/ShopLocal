using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.WebControls;
using Mimo.Framework.Extensions;
using SAM.Entities.Personalizadas;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Produccion;
using SAM.Entities;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Excepciones;
using Resources;
using SAM.Web.Classes;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Proyectos;
using SAM.Web.Common;

namespace SAM.Web.Controles.SpoolOdt
{
    /// <summary>
    /// Control que en modo sólo lectura despliega la información de las juntas de un spool
    /// </summary>
    public partial class JuntaOdtRO : UserControl
    {
        private readonly char[] alfabeto = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        public int OrdenTrabajoSpoolID
        {
            get
            {
                return (int)ViewState["OrdenTrabajoSpoolID"];

            }
            set
            {
                ViewState["OrdenTrabajoSpoolID"] = value;
            }
        }
        private OrdenTrabajoSpool OTS
        {
            get
            {
                return (OrdenTrabajoSpool)ViewState["OrdenTrabajoSpool"];

            }
            set
            {
                ViewState["OrdenTrabajoSpool"] = value;
            }
        }
        private int SpoolID
        {
            get
            {
                return (int)ViewState["SpoolID"];

            }
            set
            {
                ViewState["SpoolID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                OTS = OrdenTrabajoSpoolBO.Instance.ObtenerConOrdenTrabajo(OrdenTrabajoSpoolID);

                cargaJuntas();
                cargaTalleres();
                cargaEstaciones();
                cargaBastones();
            }
        }

        protected void btnAgrupar_OnClick(object sender, EventArgs e)
        {
            try
            {
                //Validar si no existe el nombre del baston para guardarlo.
                if (bastonExiste(txtLetraBaston.Text))
                {
                    throw new ExcepcionDuplicados(MensajesErrorWeb.Excepcion_NombreExiste);
                }

                Guarda();
            }
            catch (BaseValidationException ex)
            {
                foreach (string detail in ex.Details)
                {
                    var cv = new CustomValidator
                    {
                        ErrorMessage = detail,
                        IsValid = false,
                        Display = ValidatorDisplay.None,
                        ValidationGroup = "vgBaston"
                    };
                    Page.Form.Controls.Add(cv);
                }
            }
        }
        protected void btnTerminar_OnClick(object sender, EventArgs e)
        {
            // registramos las juntas manuales
            BastonSpool bastonSpool = BastonBO.Instance.ObtenerPorSpool(SpoolID)
                                                       .FirstOrDefault(x => x.LetraBaston == "MAN");
            if (bastonSpool != null)
            {
            }
            else
            {
               // Agregamos un nuevo baston manual
                int? estacionID = ddlEstacion.SelectedValue.SafeIntParse();

                BastonSpool bastonManual = new BastonSpool();
                bastonManual.SpoolID = SpoolID;
                bastonManual.LetraBaston = "MAN";
                bastonManual.EstacionID = estacionID != -1 ? estacionID : null;
                bastonManual.TallerID = ddlTaller.SelectedValue.SafeIntParse();
                bastonManual.UsuarioModifica = SessionFacade.UserId;
                bastonManual.FechaModificacion = DateTime.Now;

                // Obtenemos las juntas spools manuales para agregarlas al bastón
                List<JuntaSpool> juntasSpool = JuntaSpoolBO.Instance.ObtenerJuntasPorSpoolID(SpoolID)
                                                                    .Where(x => x.EsManual == true && x.FabArea.Codigo != "FIELD").ToList();
                foreach (JuntaSpool junta in juntasSpool)
                {
                    BastonSpoolJunta bastonSpoolJunta = new BastonSpoolJunta();
                    bastonSpoolJunta.JuntaSpool = junta;
                    bastonSpoolJunta.JuntaSpoolID = junta.JuntaSpoolID;
                    bastonSpoolJunta.UsuarioModifica = SessionFacade.UserId;
                    bastonSpoolJunta.FechaModificacion = DateTime.Now;

                    bastonManual.BastonSpoolJunta.Add(bastonSpoolJunta);
                }

                // Guardamos baston
                BastonBO.Instance.Guarda(bastonManual);
            }

            JsUtils.RegistraScriptActualizaGridGenerico(this.Page);
        }
        protected void grdBastones_ItemCommand(object sender, CommandEventArgs e)
        {
            int bastonSpoolID = e.CommandArgument.SafeIntParse();
            BastonSpool baston = BastonBO.Instance.Obtener(bastonSpoolID);

            if (e.CommandName == "Borrar")
            {
                if (baston.LetraBaston != "MAN")
                {
                    // Eliminar baston spool y mover juntas a baston manual en caso de que exista
                    Borra(bastonSpoolID);
                }
                else
                {
                    // Eliminar baston de manuales
                    BastonSpool bastonManual = BastonBO.Instance.ObtenerPorSpool(SpoolID)
                                                                .FirstOrDefault(x => x.LetraBaston == "MAN");
                    BastonBO.Instance.Borra(bastonManual);
                }

                //limpia datos de captura
                limpiarDatosDeCaptura();
                cargaJuntas();
                cargaBastones();
            }
        }
        protected void repJuntas_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // Deshabilitamos las juntas spool que cuenten con baston
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int juntaSpoolID = ((HiddenField)e.Item.FindControl("hdJuntaSpoolID")).Value.SafeIntParse();
                JuntaSpoolProduccion juntaSpool = JuntaSpoolBO.Instance.ObtenerConDatosDeProduccion(juntaSpoolID);

                // Si la junta no es de tipo BW no se puede agrupar
                // Si la junta es de Field no se puede agrupar
                CheckBox chkBaston = (CheckBox)e.Item.FindControl("chkBaston");
                if (juntaSpool.TipoJunta != "BW" || juntaSpool.FabArea == "FIELD")
                {
                    chkBaston.Visible = false;
                }
                else
                {
                    chkBaston.Enabled = !chkBaston.Checked;
                }
            }
        }

        protected void ddlTaller_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            cargaEstaciones();
        }

        private void cargaJuntas()
        {
            DetSpoolOdt spool = OrdenTrabajoBO.Instance.ObtenerDetalleDeSpool(OrdenTrabajoSpoolID);
            SpoolID = spool.SpoolID;

            // cargamos juntas spool
            repJuntas.DataSource = spool.Juntas.OrderBy(x => x.Etiqueta);
            repJuntas.DataBind();
        }

        private void cargaTalleres()
        {
            List<Taller> talleres = ProyectoBO.Instance.ObtenerTallers(OTS.OrdenTrabajo.ProyectoID);
            ddlTaller.DataSource = talleres;
            ddlTaller.DataValueField = "TallerID";
            ddlTaller.DataTextField = "Nombre";
            ddlTaller.DataBind();
            ddlTaller.SelectedValue = OTS.OrdenTrabajo.TallerID.ToString();
        }
        private void cargaBastones()
        {
            // cargamos bastones
            List<GrdBaston> bastones = BastonBO.Instance.ObtenerGrdBastonPorSpool(SpoolID)
                                                        .OrderBy(x => x.LetraBaston == "MAN")
                                                        .ThenBy(x => x.LetraBaston).ToList();
            grdBastones.DataSource = bastones;
            grdBastones.DataBind();
            grdBastones.Visible = bastones.Count > 0 ? true : false;

            // sugerimos nombre para baston
            char[] letrasUsadas = bastones.Where(x => x.LetraBaston != "MAN" && x.LetraBaston.Length == 1)
                                          .Select(x => Convert.ToChar(x.LetraBaston)).ToArray();
            string letraBaston = alfabeto.Except(letrasUsadas).FirstOrDefault().ToString();
            txtLetraBaston.Text = letraBaston;
        }
        private void cargaEstaciones()
        {
            List<Estacion> estaciones = EstacionBO.Instance.ObtenerPorTaller(ddlTaller.SelectedValue.SafeIntParse());
            var comboEstaciones = (from e in estaciones
                                   select new
                                   {
                                       EstacionID = e.EstacionID,
                                       Nombre = string.Format("{0}{1}", e.Taller.Nombre, e.Nombre)
                                   }).ToList();

            ddlEstacion.DataSource = comboEstaciones;
            ddlEstacion.DataValueField = "EstacionID";
            ddlEstacion.DataTextField = "Nombre";
            ddlEstacion.DataBind();
        }
        private void limpiarDatosDeCaptura()
        {
            txtLetraBaston.Text = String.Empty;
            ddlEstacion.SelectedIndex = ddlEstacion.SelectedIndex != -1 ? 0 : -1;
        }
        private bool bastonExiste(string nombre)
        {
            return BastonBO.Instance.ObtenerPorSpool(SpoolID).Any(x => x.LetraBaston == nombre);
        }
        private void Guarda()
        {
            List<JuntaSpool> juntasManualesARegistrar = new List<JuntaSpool>();
            BastonSpool bastonSpool = new BastonSpool();
            int? estacionID = ddlEstacion.SelectedValue.SafeIntParse();

            bastonSpool.SpoolID = SpoolID;
            bastonSpool.LetraBaston = txtLetraBaston.Text;
            bastonSpool.EstacionID = estacionID != -1 ? estacionID : null;
            bastonSpool.TallerID = ddlTaller.SelectedValue.SafeIntParse();
            bastonSpool.UsuarioModifica = SessionFacade.UserId;
            bastonSpool.FechaModificacion = DateTime.Now;

            // Obtenemos las juntas spools seleccionadas para agregarlas al bastón
            foreach (Control control in repJuntas.Controls)
            {
                CheckBox chkJunta = ((CheckBox)control.FindControl("chkBaston"));
                if (chkJunta.Enabled && chkJunta.Checked)
                {
                    int juntaSpoolID = ((HiddenField)control.FindControl("hdJuntaSpoolID")).Value.SafeIntParse();
                    JuntaSpool juntaSpool = JuntaSpoolBO.Instance.Obtener(juntaSpoolID);

                    //Actualizamos la junta spool
                    juntaSpool.StartTracking();
                    juntaSpool.EsManual = false;
                    juntaSpool.UsuarioModifica = SessionFacade.UserId;
                    juntaSpool.FechaModificacion = DateTime.Now;
                    juntaSpool.StopTracking();
                    JuntaSpoolBO.Instance.Guarda(juntaSpool);

                    juntasManualesARegistrar.Add(juntaSpool);
                }
            }

            // baston de juntas manuales
            BastonSpool bastonManual = BastonBO.Instance.ObtenerPorSpool(bastonSpool.SpoolID)
                                                        .FirstOrDefault(x => x.LetraBaston == "MAN");

            foreach (JuntaSpool juntaSpool in juntasManualesARegistrar)
            {
                if (bastonManual != null)
                {
                    // Quitamos la junta del baston manual para despues pasarla al bastón que se creara
                    BastonSpoolJunta bastonSpoolJuntaManual = bastonManual.BastonSpoolJunta
                                                                          .FirstOrDefault(x => x.JuntaSpoolID == juntaSpool.JuntaSpoolID);
                    if (bastonSpoolJuntaManual != null)
                    {
                        bastonManual.BastonSpoolJunta.Remove(bastonSpoolJuntaManual);
                        BastonBO.Instance.BorraJunta(bastonSpoolJuntaManual.BastonSpoolJuntaID);
                    }
                }

                // Agregamos la junta al nuevo bastón
                BastonSpoolJunta bastonSpoolJunta = new BastonSpoolJunta();
                bastonSpoolJunta.JuntaSpool = juntaSpool;
                bastonSpoolJunta.JuntaSpoolID = juntaSpool.JuntaSpoolID;
                bastonSpoolJunta.UsuarioModifica = SessionFacade.UserId;
                bastonSpoolJunta.FechaModificacion = DateTime.Now;

                bastonSpool.BastonSpoolJunta.Add(bastonSpoolJunta);
            }

            if (juntasManualesARegistrar.Count > 0)
            {
                // Guardamos baston
                BastonBO.Instance.Guarda(bastonSpool);

                //limpia datos de captura
                limpiarDatosDeCaptura();
                cargaJuntas();
                cargaBastones();
            }
            else
            {
                var cv = new CustomValidator
                {
                    ErrorMessage = MensajesErrorWeb.Excepcion_JuntasSeleccionadas,
                    IsValid = false,
                    Display = ValidatorDisplay.None,
                    ValidationGroup = "vgBaston"
                };
                Page.Form.Controls.Add(cv);
            }
        }
        private void Borra(int bastonSpoolID)
        {
            // baston a eliminar
            BastonSpool bastonSpool = BastonBO.Instance.Obtener(bastonSpoolID);

            foreach (BastonSpoolJunta bastonJunta in bastonSpool.BastonSpoolJunta)
            {
                //Actualizamos la junta spool
                JuntaSpool juntaSpool = bastonJunta.JuntaSpool;
                juntaSpool.StartTracking();
                juntaSpool.EsManual = true;
                juntaSpool.UsuarioModifica = SessionFacade.UserId;
                juntaSpool.FechaModificacion = DateTime.Now;
                juntaSpool.StopTracking();

                JuntaSpoolBO.Instance.Guarda(juntaSpool);
            }

            // Borramos baston seleccionado
            BastonBO.Instance.Borra(bastonSpool);

            // procedemos a mover las juntas manuales al baston de manuales
            BastonSpool bastonManual = BastonBO.Instance.ObtenerPorSpool(bastonSpool.SpoolID)
                                                        .FirstOrDefault(x => x.LetraBaston == "MAN");
            List<BastonSpoolJunta> bastonesJuntasManuales = new List<BastonSpoolJunta>();

            if (bastonManual != null)
            {
                var juntasManualesRegistradas = bastonManual.BastonSpoolJunta.Select(x => x.JuntaSpoolID).ToList();
                var juntasManualesARegistrar = JuntaSpoolBO.Instance.ObtenerJuntasPorSpoolID(bastonManual.SpoolID)
                                                                    .Where(x => x.EsManual == true && !juntasManualesRegistradas.Contains(x.JuntaSpoolID))
                                                                    .ToList();

                foreach (JuntaSpool juntaSpool in juntasManualesARegistrar)
                {
                    BastonSpoolJunta bastonSpoolJunta = new BastonSpoolJunta();
                    bastonSpoolJunta.JuntaSpool = juntaSpool;
                    bastonSpoolJunta.JuntaSpoolID = juntaSpool.JuntaSpoolID;
                    bastonSpoolJunta.UsuarioModifica = SessionFacade.UserId;
                    bastonSpoolJunta.FechaModificacion = DateTime.Now;

                    bastonesJuntasManuales.Add(bastonSpoolJunta);
                }

                // Actualizamos baston manual
                bastonesJuntasManuales.ForEach(x => bastonManual.BastonSpoolJunta.Add(x));
                BastonBO.Instance.Guarda(bastonManual);
            }
        }
    }
}