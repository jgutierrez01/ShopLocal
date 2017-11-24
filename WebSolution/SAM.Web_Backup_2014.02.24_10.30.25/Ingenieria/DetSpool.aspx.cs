using System;
using System.Linq;
using SAM.Web.Classes;
using SAM.Entities;
using SAM.BusinessObjects.Ingenieria;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Validations;
using log4net;
using System.Diagnostics;

namespace SAM.Web.Ingenieria
{
    public partial class DetSpool : SamPaginaPrincipal
    {
        private Entities.Personalizadas.DetSpoolHold _spoolDet;
        private Entities.Spool _spool;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(DetSpool));
        private Stopwatch sw = new Stopwatch();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.ing_IngenieriaProyecto);


                if (EntityID.HasValue)
                {
                    if (!SeguridadQs.TieneAccesoASpool(EntityID.Value))
                    {
                        //Generar error 401 (Unauthorized access)
                        string mensaje = string.Format("El usuario {0} está intentando accesar a un spool {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                        UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                    }
                    
                    carga();
                }
            }
        }

        protected void lnkActualizar_Click(object sender, EventArgs e)
        {
            carga();
            tabMenu.Tabs[2].Selected = true;
        }

        private void carga()
        {
            sw.Start();
            _logger.DebugFormat("Obten Holds");
            _spoolDet = SpoolBO.Instance.ObtenerHolds(EntityID.Value);
            _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);

            sw.Restart();
            _logger.DebugFormat("Obten detalle");
            _spool = SpoolBO.Instance.ObtenerDetalle(EntityID.Value);
            _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);

            sw.Restart();
            _logger.DebugFormat("Mapea control Info");
            ctrlInfo.Map(_spool);
            _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);

            sw.Restart();
            _logger.DebugFormat("Mapea control Junta");
            ctrlJunta.Map(_spool.JuntaSpool);
            ctrlJunta.ProyectoID = _spool.ProyectoID;
            _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);

            sw.Restart();
            _logger.DebugFormat("Mapea control material");            
            ctrlMaterial.Map(_spool.MaterialSpool);
            ctrlMaterial.ProyectoID = _spool.ProyectoID;
            _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);

            sw.Restart();
            _logger.DebugFormat("Mapea control corte");            
            ctrlCorte.Map(_spool.CorteSpool, _spool.ProyectoID);
            _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);

            sw.Restart();
            _logger.DebugFormat("Mapea control holds");            
            ctrlHold.Map(_spoolDet);
            _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);
            VersionRegistro = _spool.VersionRegistro;            

            titulo.NavigateUrl += "?PID=" + _spool.ProyectoID;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            
            Entities.Spool spool;

            if (EntityID.HasValue)
            {
                spool = SpoolBO.Instance.ObtenerDetalle(EntityID.Value);
                spool.VersionRegistro = VersionRegistro;
            }
            else
            {
                spool = new Entities.Spool();
            }

            spool.StartTracking();

            sw.Restart();
            _logger.DebugFormat("Unmap control info");
            ctrlInfo.Unmap(spool);
            _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);

            sw.Restart();
            _logger.DebugFormat("Unmap control junta"); 
            ctrlJunta.Unmap(spool.JuntaSpool);
            _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);

            sw.Restart();
            _logger.DebugFormat("Unmap control material"); 
            ctrlMaterial.Unmap(spool.MaterialSpool);
            _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);

            sw.Restart();
            _logger.DebugFormat("Unmap control corte"); 
            ctrlCorte.Unmap(spool.CorteSpool);
            _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);
            spool.UsuarioModifica = SessionFacade.UserId;
            spool.FechaModificacion = DateTime.Now;

            ctrlHold.Unmap(spool);

            spool.StopTracking();

            

            try
            {             
            
                //Verificar si tiene ODT
                if (ValidacionesSpool.TieneODT(spool.SpoolID))
                {
                    int numJuntaSpool, numMaterialSpool;

                    numJuntaSpool = spool.JuntaSpool.Where(x => x.JuntaSpoolID <= 0).Count();
                    numMaterialSpool = spool.MaterialSpool.Where(x => x.MaterialSpoolID <= 0).Count();

                    //Verificamos si se eliminaron juntas o materiales
                    if (ctrlJunta.lstJuntasEliminadasIds.Count > 0 || ctrlMaterial.lstMaterialesEliminadasIds.Count > 0)
                    {
                        sw.Restart();
                        _logger.DebugFormat("guarda spool editado con odt (eliminando juntas o materiales)"); 
                        //Se cumple cuando solamente se eliminaron Juntas
                        SpoolBO.Instance.GuardaSpoolEditadoConOdt(spool, ctrlMaterial.lstMatIds, ctrlMaterial.lstMaterialesEliminadasIds, ctrlJunta.lstJuntasEliminadasIds, SessionFacade.UserId);
                        _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);
                        string url = String.Format(WebConstants.IngenieriaUrl.REDIRECT_NOMBRADO_SPOOL, spool.ProyectoID);
                        
                        Response.Redirect(url);
                        

                    } //Verificamos si se agregaron Juntas, Materiales ó si se editarón Materiales ó Juntas
                    else if (numJuntaSpool > 0 || numMaterialSpool > 0 || ctrlMaterial.lstMatIds.Count > 0 || ctrlJunta.lstJuntaIds.Count > 0)                              
                    {
                        sw.Restart();
                        _logger.DebugFormat("guarda spool editado con odt (agregando juntas o materiales)"); 
                        //Se muestra el mensaje de Exito para informar que se necesita reingenieria debido a los cambios
                        SpoolBO.Instance.GuardaSpoolEditadoConOdt(spool, ctrlMaterial.lstMatIds, ctrlMaterial.lstMaterialesEliminadasIds, ctrlJunta.lstJuntasEliminadasIds, SessionFacade.UserId);
                        _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);                         
                        phDetSpool.Visible = false;
                        phMensajeExito.Visible = true;
                        hlIngenieria.NavigateUrl = string.Format(hlIngenieria.NavigateUrl, spool.ProyectoID);

                        string url = String.Format(WebConstants.IngenieriaUrl.REDIRECT_NOMBRADO_SPOOL, spool.ProyectoID);

                        Response.Redirect(url);
                        
                    }

                    else
                    {
                        sw.Restart();
                        _logger.DebugFormat("guarda spool sin cambios"); 
                        //No se realizaron cambios
                        SpoolBO.Instance.Guarda(spool);
                        string url = String.Format(WebConstants.IngenieriaUrl.REDIRECT_NOMBRADO_SPOOL, spool.ProyectoID);
                        _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);                        
                        Response.Redirect(url);
                    }

                }
                else
                {
                    //es un spool sin ODT
                    SpoolBO.Instance.Guarda(spool);
                    string url = String.Format(WebConstants.IngenieriaUrl.REDIRECT_NOMBRADO_SPOOL, spool.ProyectoID);
                    Response.Redirect(url);
                }
                //SpoolBO.Instance.ActualizardiametroMayor(spool);
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }
    }
}