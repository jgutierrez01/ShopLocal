using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using Resources;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessLogic.Ingenieria;
using SAM.BusinessObjects.Ingenieria;
using SAM.Entities;
using SAM.Web.Classes;
using SAM.Web.Common;


namespace SAM.Web.Ingenieria
{
    public partial class HomologacionSpool : SamPaginaConSeguridad
    {
        protected int ProyectoID
        {
            get
            {
                return (int)ViewState["ProyectoID"];
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }
        }

        protected int SiguienteSpool
        {
            get
            {
                return (int)ViewState["siguiente"];
            }
            set
            {
                ViewState["siguiente"] = value;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            divError.Visible = false;
            if (!IsPostBack)
            {
                SpoolPendiente spoolPendiente = IngenieriaBL.Instance.ObtenerPendientePorHomologar(EntityID.Value);
                ProyectoID = spoolPendiente.ProyectoID;
                SiguienteSpool = SpoolPendienteHelper.ObtenSiguienteEnListaPendientes(EntityID.Value);
                SpoolPendienteHelper.EliminaDePendientesSessionSpool(EntityID.Value);

            }
            cargarDatos();
        }

        private void cargarDatos()
        {
            SpoolPendiente spoolPendiente = IngenieriaBL.Instance.ObtenerPendientePorHomologar(EntityID.Value);
            int spoolId = IngenieriaBL.Instance.ObtenerSpoolIdSegunSpoolPendienteId(EntityID.Value);
            Spool spoolBD = SpoolBO.Instance.ObtenerDetalleHomologacion(spoolId);

            if (spoolPendiente != null)
            {
                CorteRO1.MapGeneric(spoolBD, spoolPendiente);
                if (!IsPostBack)
                {
                    MaterialRO1.MapGeneric(spoolBD, spoolPendiente);
                }
                JuntaRO1.MapGeneric(spoolBD, spoolPendiente);
                SpoolRO1.MapGeneric(spoolBD, spoolPendiente);
            }
        }

        private bool CancelaHomologacion()
        {
            try
            {
                return IngenieriaBL.Instance.ElminaSpoolPendienteXHomologar(EntityID.Value, SessionFacade.UserId);
            }
            catch (Exception ex)
            {
                divError.Visible = true;
                litError.Text = ex.Message.ToString();
                return false;
            }
        }

        private int OrdenTrabajo(int SpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                OrdenTrabajoSpool ordenTrabajoSpool = ctx.OrdenTrabajoSpool.SingleOrDefault(p => p.SpoolID == SpoolID);
                OrdenTrabajo ordenTrabajo = ctx.OrdenTrabajo.SingleOrDefault(p => p.OrdenTrabajoID == ordenTrabajoSpool.OrdenTrabajoID);
                if (ordenTrabajo != null)
                {
                    return ordenTrabajo.OrdenTrabajoID;
                }
                else
                {
                    return -1;
                }
            }
        }

        private bool EjecutaHomologacion()
        {
            try
            {
                int OrdenTrabajoID;
                byte[] ReporteODTAnterior = null;
                OrdenTrabajoID = OrdenTrabajo(EntityID.Value);
                if (OrdenTrabajoID != -1)
                {
                    ReporteODTAnterior = UtileriasReportes.ObtenReporteOdt(OrdenTrabajoID, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true);
                }

                ArchivosWorkstatus archivos = GeneraArchivosHistoricoWorkStatus(EntityID.Value, OrdenTrabajoID);
                return IngenieriaBL.Instance.EjecutaAccionesHomologacion(EntityID.Value, OrdenTrabajoID, ReporteODTAnterior, archivos, SessionFacade.UserId);

            }
            catch (Exception ex)
            {
                divError.Visible = true;
                litError.Text = string.Format(MensajesIngenieria.Homologacion_ErrorProceso + ": " + ex.Message + ", " + ex.StackTrace);
                return false;
            }
        }

        private ArchivosWorkstatus GeneraArchivosHistoricoWorkStatus(int spoolID, int OrdenTrabajoID)
        {
            ArchivosWorkstatus archivos = new ArchivosWorkstatus();

            using (SamContext ctx = new SamContext())
            {
                Spool spool = ctx.Spool.Where(x => x.SpoolID == spoolID).SingleOrDefault();

                OrdenTrabajo odt = ctx.OrdenTrabajo.Where(x => x.OrdenTrabajoID == OrdenTrabajoID).SingleOrDefault();

                int OrdenTrabajoSpoolID = ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajoID == OrdenTrabajoID && x.SpoolID == spoolID).Select(x => x.OrdenTrabajoSpoolID).SingleOrDefault();

                archivos = IngenieriaBL.Instance.GeneraArchivosWorkStatus(odt.ProyectoID, spoolID, odt.OrdenTrabajoID.ToString(), OrdenTrabajoSpoolID.ToString(), spool.RevisionCliente, spool.Revision);
            }

            return archivos;
        }

        private void Terminar()
        {
            List<LigaMensaje> lista = new List<LigaMensaje>
                                {
                                    new LigaMensaje
                                        {
                                            Texto = MensajesIngenieria.HomologacionSpool_LigaSpoolsPendientes,
                                            Url = string.Format(WebConstants.IngenieriaUrl.PENDIENTES_HOMOLOGAR , ProyectoID)
                                        }
                                };
            UtileriaRedireccion.RedireccionaExitoIngenieria(MensajesIngenieria.HomologacionSpool_Titulo, MensajesIngenieria.HomologacionSpool_Mensaje, lista);
        }

        private bool Siguiente()
        {

            if (SiguienteSpool != -1)
            {
                Response.Redirect(Request.Url.LocalPath + "?ID=" + SiguienteSpool);
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void btnGuardar_OnClick(object sender, EventArgs e)
        {
            if (EjecutaHomologacion() == true)
            {

                Terminar();
            }
        }

        protected void btnSiguiente_OnClick(object sender, EventArgs e)
        {
            if (Siguiente() == false)
            {
                Terminar();
            }
        }

        protected void btnGuardarContinuar_OnClick(object sender, EventArgs e)
        {
            if (EjecutaHomologacion() == true)
            {
                if (Siguiente() == false)
                {
                    Terminar();
                }
            }
        }

        protected void btnCancelar_OnClick(object sender, EventArgs e)
        {
            if (CancelaHomologacion() == true)
            {
                if (Siguiente() == false)
                {
                    Terminar();
                }
            }
        }

    }
}