using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using System.Data.Objects;
using SAM.BusinessObjects.Validations;
using SAM.Entities.Personalizadas;
using Mimo.Framework.Exceptions;
using SAM.Entities.Grid;
using Mimo.Framework.Common;
using SAM.BusinessObjects.Utilerias;

namespace SAM.BusinessObjects.Proyectos
{
    public class ProyectoBO
    {
        public event TableChangedHandler ProyectoCambio;
        private static readonly object _mutex = new object();
        private static ProyectoBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private ProyectoBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase ProyectoBO
        /// </summary>
        /// <returns></returns>
        public static ProyectoBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ProyectoBO();
                    }
                }
                return _instance;
            }
        }


        /// <summary>
        /// Regresa la entidad proyecto con el ID enviado
        /// así mismo regresa el proyecto con las entidades de 
        /// contacto, cliente, patio, color y proyectoconfiguracio
        /// La información es utilizada para la información del dashboard del proyecto.
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public Proyecto ObtenerInfoDashboard(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                Proyecto p = ctx.Proyecto.Where(x => x.ProyectoID == proyectoID).Single();

                ctx.LoadProperty<Proyecto>(p, x => x.Cliente);
                ctx.LoadProperty<Proyecto>(p, x => x.Contacto);
                ctx.LoadProperty<Proyecto>(p, x => x.Patio);
                ctx.LoadProperty<Proyecto>(p, x => x.Color);
                ctx.LoadProperty<Proyecto>(p, x => x.ProyectoConfiguracion);

                return p;
            }
        }

        /// <summary>
        /// Regresa la entidad proyecto con el ID enviado
        /// así mismo regresa el proyecto con las entidades de 
        /// contacto, cliente, patio, color, proyectoconfiguracion y proyectonomenclaturaspool
        /// La información es utilizada en la pantalla de configuración del proyecto.
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public Proyecto ObtenerProyectoConfiguracion(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                Proyecto p = ctx.Proyecto.Where(x => x.ProyectoID == proyectoID).Single();

                ctx.LoadProperty<Proyecto>(p, x => x.Cliente);
                ctx.LoadProperty<Proyecto>(p, x => x.Contacto);
                ctx.LoadProperty<Proyecto>(p, x => x.Patio);
                ctx.LoadProperty<Proyecto>(p, x => x.Color);
                ctx.LoadProperty<Proyecto>(p, x => x.ProyectoConfiguracion);
                ctx.LoadProperty<Proyecto>(p, x => x.ProyectoNomenclaturaSpool);
                ctx.LoadProperty<Proyecto>(p, x => x.ProyectoCamposRecepcion);
                //ctx.LoadProperty<Proyecto>(p, x => x.ProyectoConfiguracionCorreo);

                return p;
            }
        }

        /// <summary>
        /// Método que obtiene un proyecto dependiendo de el proyectoId y regresa un objeto proyecto
        /// </summary>
        /// <param name="proyectoID">identificador unico de proyecto</param>
        /// <returns></returns>
        public Proyecto Obtener(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Proyecto.Where(x => x.ProyectoID == proyectoID).SingleOrDefault();
            }
        }

        /// <summary>
        /// Método que obtiene un proyecto dependiendo de el proyectoId y regresa un objeto proyecto
        /// junto con la informacion de la entidad proveedores
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public Proyecto ObtenerConProveedores(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                Proyecto p = ctx.Proyecto.Where(x => x.ProyectoID == proyectoID).Single();
                ctx.LoadProperty<Proyecto>(p, x => x.ProveedorProyecto);

                return p;
            }
        }

        /// <summary>
        /// Método que obtiene un proyecto dependiendo de el proyectoId y regresa una entidad proyecto
        /// junto con la informacion de los fabricantes de ese proyecto
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public Proyecto ObtenerConFabricantes(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                Proyecto p = ctx.Proyecto.Where(x => x.ProyectoID == proyectoID).Single();
                ctx.LoadProperty<Proyecto>(p, x => x.FabricanteProyecto);

                return p;
            }
        }

        /// <summary>
        /// Método que obtiene un proyecto dependiendo de el proyectoId y regresa una entidad proyecto
        /// junto con la informacion de los transportistas de ese proyecto
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public Proyecto ObtenerConWps(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                Proyecto p = ctx.Proyecto.Where(x => x.ProyectoID == proyectoID).Single();
                ctx.LoadProperty<Proyecto>(p, x => x.WpsProyecto);

                return p;
            }
        }

        /// <summary>
        /// Método que obtiene un proyecto dependiendo de el proyectoId y regresa una entidad proyecto
        /// junto con la informacion de los WPS de ese proyecto
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public Proyecto ObtenerConTransportistas(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                Proyecto p = ctx.Proyecto.Where(x => x.ProyectoID == proyectoID).Single();
                ctx.LoadProperty<Proyecto>(p, x => x.TransportistaProyecto);

                return p;
            }
        }

        /// <summary>
        /// Método que obtiene un proyecto dependiendo de el proyectoId y regresa una entidad proyecto
        /// junto con la informacion de reportes de Dossier de ese proyecto
        public Proyecto ObtenerConDossier(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                Proyecto p = ctx.Proyecto.Where(x => x.ProyectoID == proyectoID).Single();
                ctx.LoadProperty<Proyecto>(p, x => x.ProyectoDossier);


                return p;
            }
        }

        /// <summary>
        /// Método que obtiene un proyecto dependiendo de el proyectoId y regresa una entidad proyecto
        /// junto con la informacion de reportes de Dossier de ese proyecto y los segmentos
        public Proyecto ObtenerConDossierYSegmentos(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                Proyecto p = ctx.Proyecto.Where(x => x.ProyectoID == proyectoID).Single();
                ctx.LoadProperty<Proyecto>(p, x => x.ProyectoDossier);
                ctx.LoadProperty<Proyecto>(p, x => x.ProyectoNomenclaturaSpool);

                return p;
            }
        }


        /// <summary>
        /// Obtiene los datos del proyecto con su configuracion
        /// </summary>
        /// <param name="proyectoID">ProyectoID</param>
        /// <returns></returns>
        public Proyecto ObtenerConConfiguracion(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                Proyecto p = ctx.Proyecto.Where(x => x.ProyectoID == proyectoID).Single();
                ctx.LoadProperty<Proyecto>(p, x => x.ProyectoConfiguracion);

                return p;
            }
        }

        public Proyecto ObtenerConCamposRecepcion(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                Proyecto p = ctx.Proyecto.Where(x => x.ProyectoID == proyectoID).Single();
                ctx.LoadProperty<Proyecto>(p, x => x.ProyectoCamposRecepcion);

                return p;
            }
        }

        /// <summary>
        /// Obtiene la nomenclatura de los spools
        /// </summary>
        /// <param name="proyectoID">ID del proyecto</param>
        /// <returns></returns>
        public ProyectoNomenclaturaSpool ObtenerNomenclaturaSpool(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                ProyectoNomenclaturaSpool p = ctx.ProyectoNomenclaturaSpool.Where(x => x.ProyectoID == proyectoID).Single();
                return p;
            }
        }

        /// <summary>
        /// Obtiene la lista de proyectos
        /// </summary>
        /// <returns></returns>
        public List<Proyecto> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Proyecto.ToList();
            }
        }

        /// <summary>
        /// metodo que regresa todos los proyectos pero incluye tambien el cliente y el color del proyecto.
        /// </summary>
        /// <returns></returns>
        public List<Proyecto> ObtenerTodosConClienteColorPatioNomenclatura()
        {
            using (SamContext ctx = new SamContext())
            {
                //Segun documentacion esto es mucho más rápido que hacer los includes
                List<Proyecto> proyectos = ctx.Proyecto.Where(p => p.Activo).ToList();
                ctx.Cliente.ToList();
                ctx.Color.ToList();
                ctx.Patio.ToList();
                ctx.ProyectoNomenclaturaSpool.ToList();
                ctx.ProyectoConfiguracion.ToList();
                return proyectos;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<GrdProyecto> ObtenerListadoDeProyectos()
        {
            List<Proyecto> proyectos = new List<Proyecto>();

            using (SamContext ctx = new SamContext())
            {
                ctx.Proyecto.MergeOption = MergeOption.NoTracking;

                proyectos = ctx.Proyecto
                               .Include("Cliente")
                               .Include("Color")
                               .Include("ProyectoConfiguracion")
                               .ToList();
            }

            return (from p in proyectos
                    select new GrdProyecto
                    {
                        Proyecto = p.Nombre,
                        ProyectoID = p.ProyectoID,
                        NombreClienteCompleto = p.Cliente.Nombre,
                        PrefijoNumeroUnico = p.ProyectoConfiguracion.PrefijoNumeroUnico,
                        PrefijoOdt = p.ProyectoConfiguracion.PrefijoOrdenTrabajo,
                        NombreColor = LanguageHelper.CustomCulture == LanguageHelper.INGLES ? p.Color.NombreIngles : p.Color.Nombre,
                        Estatus = TraductorEnumeraciones.TextoActivoInactivo(p.Activo)
                    }).ToList();

        }

        /// <summary>
        /// el método recibe un identificador de usuario, mediante el cual hace una busqueda de proyectos 
        /// válidos para mostrarle a determinado usuario.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<Proyecto> ObtenerPorUsuario(Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                //Ejemplo de como hacer un nested query en dos partes
                //var pids =  from usp in ctx.UsuarioProyecto
                //            where usp.UserId == userID
                //            select usp.ProyectoID;

                //Ejemplo de como hacerlo en un solo query (ver en el delegado)
                return ObtenerPorUsuarioCompiled(ctx, userID).ToList();
            }
        }

        /// <summary>
        /// guarda el proyecto, si existe uno duplicado regresa una excepcion... guarda los cambios del proyecto
        /// </summary>
        /// <param name="proyecto"></param>
        /// <param name="userID"></param>
        public void AltaProyecto(Proyecto proyecto, Guid userID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (ValidacionesProyecto.ProyectoDuplicado(ctx, proyecto.Nombre, proyecto.ProyectoID))
                    {
                        throw new ExcepcionDuplicados(MensajesError.Excepcion_NombreDuplicado);
                    }

                    proyecto.ProyectoDossier = new ProyectoDossier();
                    proyecto.ProyectoNomenclaturaSpool = new ProyectoNomenclaturaSpool();
                    proyecto.ProyectoCamposRecepcion = new ProyectoCamposRecepcion();
                    proyecto.ProyectoNomenclaturaSpool.CantidadSegmentosSpool = 0;
                    proyecto.ProyectoCamposRecepcion = new ProyectoCamposRecepcion();
                    proyecto.ProyectoCamposRecepcion.CantidadCamposRecepcion = 0;
                    proyecto.ProyectoCamposRecepcion.CantidadCamposNumeroUnico = 0;
                    proyecto.ProyectoConsecutivo = new ProyectoConsecutivo();
                    proyecto.ProyectoConsecutivo.ConsecutivoNumeroUnico = 0;
                    proyecto.ProyectoConsecutivo.ConsecutivoODT = 0;

                    List<TipoPendiente> tipoPendiente = ctx.TipoPendiente.Where(x => x.EsAutomatico == true).ToList();
                    foreach (TipoPendiente tp in tipoPendiente)
                    {
                        ProyectoPendiente pp = new ProyectoPendiente();
                        pp.TipoPendienteID = tp.TipoPendienteID;
                        pp.Responsable = userID;
                        pp.UsuarioModifica = userID;
                        pp.FechaModificacion = DateTime.Now;

                        proyecto.ProyectoPendiente.Add(pp);
                    }

                    proyecto.ProyectoDossier.LDCertificado = false;
                    proyecto.ProyectoDossier.UsuarioModifica = userID;
                    proyecto.ProyectoDossier.FechaModificacion = DateTime.Now;
                    proyecto.ProyectoConsecutivo.UsuarioModifica = userID;
                    proyecto.ProyectoConsecutivo.FechaModificacion = DateTime.Now;
                    proyecto.ProyectoNomenclaturaSpool.UsuarioModifica = userID;
                    proyecto.ProyectoNomenclaturaSpool.FechaModificacion = DateTime.Now;
                    proyecto.ProyectoCamposRecepcion.UsuarioModifica = userID;
                    proyecto.ProyectoCamposRecepcion.FechaModificacion = DateTime.Now;

                    ProyectoPrograma programa = new ProyectoPrograma();
                    programa.FechaModificacion = DateTime.Now;
                    programa.UsuarioModifica = userID;
                    proyecto.ProyectoPrograma.Add(programa);

                    //Darle permisos al usuario para el proyecto
                    UsuarioProyecto usp = new UsuarioProyecto();
                    usp.Proyecto = proyecto;
                    usp.UserId = userID;

                    ctx.UsuarioProyecto.ApplyChanges(usp);
                    ctx.Proyecto.ApplyChanges(proyecto);
                    ctx.SaveChanges();
                }

                if (ProyectoCambio != null)
                {
                    ProyectoCambio();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// Guarda la entidad proyecto en la BD.
        /// </summary>
        /// <param name="proyecto"></param>
        public void Guarda(Proyecto proyecto)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (ValidacionesProyecto.ProyectoDuplicado(ctx, proyecto.Nombre, proyecto.ProyectoID))
                    {
                        throw new ExcepcionDuplicados(MensajesError.Excepcion_NombreDuplicado);
                    }

                    ctx.Proyecto.ApplyChanges(proyecto);
                    ctx.SaveChanges();
                }

                if (ProyectoCambio != null)
                {
                    ProyectoCambio();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// permite eliminar proyectos con el id de dicho proyecto, 
        /// lo que genera que se elimine el renglon de la tabla de proyectos.
        /// </summary>
        /// <param name="proyectoID"></param>
        public void Borra(int proyectoID)
        {
            try
            {
                List<string> errores = new List<string>();

                using (SamContext ctx = new SamContext())
                {
                    #region Validaciones

                    if (ValidacionesProyecto.TieneSpools(ctx, proyectoID))
                    {
                        errores.Add(MensajesError.Proyecto_TieneSpools);
                    }

                    if (ValidacionesProyecto.TieneFabricantes(ctx, proyectoID))
                    {
                        errores.Add(MensajesError.Proyecto_TieneFabricantes);
                    }

                    if (ValidacionesProyecto.TieneItemCodes(ctx, proyectoID))
                    {
                        errores.Add(MensajesError.Proyecto_TieneItemCodes);
                    }

                    if (ValidacionesProyecto.TieneNumerosUnicos(ctx, proyectoID))
                    {
                        errores.Add(MensajesError.Proyecto_TieneNumerosUnicos);
                    }

                    if (ValidacionesProyecto.TieneProveedores(ctx, proyectoID))
                    {
                        errores.Add(MensajesError.Proyecto_TieneProveedores);
                    }

                    if (ValidacionesProyecto.TieneRecepciones(ctx, proyectoID))
                    {
                        errores.Add(MensajesError.Proyecto_TieneRecepciones);
                    }

                    if (ValidacionesProyecto.TieneTransportistas(ctx, proyectoID))
                    {
                        errores.Add(MensajesError.Proyecto_TieneTransportistas);
                    }

                    if (ValidacionesProyecto.TieneWps(ctx, proyectoID))
                    {
                        errores.Add(MensajesError.Proyecto_TieneWps);
                    }

                    #endregion

                    if (errores.Count > 0)
                    {
                        throw new BaseValidationException(errores);
                    }

                    Proyecto p = ctx.Proyecto.Where(x => x.ProyectoID == proyectoID).Single();
                    ctx.LoadProperty<Proyecto>(p, x => x.ProyectoConfiguracion);
                    ctx.LoadProperty<Proyecto>(p, x => x.ProyectoConsecutivo);
                    ctx.LoadProperty<Proyecto>(p, x => x.ProyectoDossier);
                    ctx.LoadProperty<Proyecto>(p, x => x.ProyectoNomenclaturaSpool);
                    ctx.LoadProperty<Proyecto>(p, x => x.Pendiente);//
                    ctx.LoadProperty<Proyecto>(p, x => x.ProyectoPendiente);//
                    ctx.LoadProperty<Proyecto>(p, x => x.UsuarioProyecto);



                    p.StartTracking();

                    foreach (var pend in p.Pendiente)
                    {
                        IList<PendienteDetalle> pendienteDetalle =
                            ctx.PendienteDetalle.Where(x => x.PendienteID == pend.PendienteID).ToList();
                        pendienteDetalle.ToList().ForEach(ctx.PendienteDetalle.DeleteObject);
                    }//
                    p.Pendiente.ToList().ForEach(ctx.Pendiente.DeleteObject);//
                    p.ProyectoPendiente.ToList().ForEach(ctx.ProyectoPendiente.DeleteObject);//
                    ctx.ProyectoConfiguracion.DeleteObject(p.ProyectoConfiguracion);
                    ctx.ProyectoConsecutivo.DeleteObject(p.ProyectoConsecutivo);
                    ctx.ProyectoDossier.DeleteObject(p.ProyectoDossier);
                    ctx.ProyectoNomenclaturaSpool.DeleteObject(p.ProyectoNomenclaturaSpool);
                    p.UsuarioProyecto.ToList().ForEach(ctx.UsuarioProyecto.DeleteObject);
                    ctx.Proyecto.DeleteObject(p);

                    p.StopTracking();

                    ctx.SaveChanges();
                }

                if (ProyectoCambio != null)
                {
                    ProyectoCambio();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// Obtiene una entidad que contiene el total de spools subidos por
        /// ingeniería, el total de spools que aún no tienen ODT y el total de spools
        /// que ya tienene ODT.
        /// </summary>
        /// <param name="proyectoID">ID del proyecto del cual se desea obtener la información mencionada</param>
        /// <returns>Entidad con la información que contiene el resumen de spools</returns>
        public ResumenSpool ObtenResumenSpoolPorProyecto(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                ResumenSpool resumen = ResumenSpools(ctx, proyectoID).SingleOrDefault();
                resumen.SpoolsSinOdt = resumen.SpoolsTotales - resumen.SpoolsConOdt;
                return resumen;
            }
        }


        /// <summary>
        /// Contiene una lista con los talleres del proyecto especificado
        /// (en realidad es la lista de los talleres del patio al que pertenece
        /// el proyecto)
        /// </summary>
        /// <param name="proyectoID">ID del proyecto</param>
        /// <returns>Lista con los talleres encontrados</returns>
        public List<Taller> ObtenerTallers(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return
                ctx.Taller.Where(x => x.Patio
                                       .Proyecto
                                       .Contains(ctx.Proyecto
                                                    .Where(y => y.ProyectoID == proyectoID)
                                                    .FirstOrDefault()
                                                 )
                                ).ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public int SiguienteConsecutivoOdt(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                int actual = ctx.ProyectoConsecutivo
                                .Where(x => x.ProyectoID == proyectoID)
                                .Select(x => x.ConsecutivoODT)
                                .Single();

                return actual + 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public int SiguienteConsecutivoNumeroUnico(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                int actual = ctx.ProyectoConsecutivo
                                .Where(x => x.ProyectoID == proyectoID)
                                .Select(x => x.ConsecutivoNumeroUnico)
                                .Single();

                return actual + 1;
            }
        }

        /// <summary>
        /// Actualiza el consecutivo del numeroUnico del proyecto.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="proyectoID"></param>
        /// <param name="nuevoConsecutivo"></param>
        /// <param name="userID"></param>
        public void ActualizaConsecutivoNumeroUnicos(SamContext ctx, int proyectoID, int nuevoConsecutivo, Guid userID)
        {
            ProyectoConsecutivo cons = ctx.ProyectoConsecutivo.Where(x => x.ProyectoID == proyectoID && nuevoConsecutivo > x.ConsecutivoNumeroUnico).SingleOrDefault();
            if (cons != null)
            {
                cons.StartTracking();
                cons.FechaModificacion = DateTime.Now;
                cons.UsuarioModifica = userID;
                cons.ConsecutivoNumeroUnico = nuevoConsecutivo;
                cons.StopTracking();
                ctx.ProyectoConsecutivo.ApplyChanges(cons);
            }
        }

        /// <summary>
        /// Obtiene la entidad proyecto junto con su configuracion y consecutivos
        /// </summary>
        /// <param name="proyectoId"></param>
        /// <returns></returns>
        public Proyecto ObtenerConfiguracionYConsecutivosPorWKSID(int workStatusSpoolId)
        {
            using (SamContext ctx = new SamContext())
            {
                int proyectoId = ctx.WorkstatusSpool.Where(x => x.WorkstatusSpoolID == workStatusSpoolId)
                                    .Select(x => x.OrdenTrabajoSpool.OrdenTrabajo.ProyectoID).SingleOrDefault();

                return ctx.Proyecto.Include("ProyectoConfiguracion")
                                   .Include("ProyectoConsecutivo")
                                   .Where(x => x.ProyectoID == proyectoId)
                                   .SingleOrDefault();
            }
        }

        #region Queries compilados

        /// <summary>
        /// Delegado de la versión compilada del query para obtener los proyectos por usuario
        /// </summary>
        public static readonly Func<SamContext, Guid, IEnumerable<Proyecto>> ObtenerPorUsuarioCompiled =
        CompiledQuery.Compile<SamContext, Guid, IEnumerable<Proyecto>>
        (
            (ctx, id) =>
                    from p in ctx.Proyecto
                    let pids = (from usp in ctx.UsuarioProyecto
                                where usp.UserId == id
                                select usp.ProyectoID)
                    where pids.Contains(p.ProyectoID)
                    select p
        );

        /// <summary>
        /// Delegado para obtener el resumen de spools por proyecto
        /// </summary>
        public static readonly Func<SamContext, int, IQueryable<ResumenSpool>> ResumenSpools =
        CompiledQuery.Compile<SamContext, int, IQueryable<ResumenSpool>>
        (
            (ctx, id) => from proyecto in ctx.Proyecto
                         where proyecto.ProyectoID == id
                         select new ResumenSpool
                         {
                             SpoolsTotales = (from spools in proyecto.Spool select spools).Count(),
                             SpoolsConOdt = (from odt in proyecto.OrdenTrabajo
                                             from odts in odt.OrdenTrabajoSpool
                                             select odts).Count()
                         }
        );

        #endregion
    }
}
