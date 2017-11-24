using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.BusinessObjects.Utilerias;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Validations;
using System.Data.Objects;
using System.Transactions;

namespace SAM.BusinessObjects.Administracion
{
    public class DestajoBO
    {
        private static readonly object _mutex = new object();
        private static DestajoBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private DestajoBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase DestajoBO
        /// </summary>
        /// <returns></returns>
        public static DestajoBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new DestajoBO();
                    }
                }
                return _instance;
            }
        }


        /// <summary>
        /// Obtiene un periodo de destajo en particular en base a su ID.
        /// </summary>
        /// <param name="periodoDestajoID">ID del periodo destajo deseado</param>
        /// <returns>Entidad con la información del periodo de destajo solicitado</returns>
        public PeriodoDestajo ObtenerPeriodo(int periodoDestajoID)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.PeriodoDestajo.MergeOption = MergeOption.NoTracking;
                return ctx.PeriodoDestajo.Where(x => x.PeriodoDestajoID == periodoDestajoID).Single();
            }
        }

        /// <summary>
        /// Obtiene una lista de personas incluidas dentro de un periodo de destajo en particular.
        /// </summary>
        /// <param name="periodoDestajoID">ID del periodo del cual se desea obtener las personas</param>
        /// <returns>Lista totalizada con la información básica de cada persona</returns>
        public List<GrdPersonaDestajo> ObtenerPersonasPorPeriodo(int periodoDestajoID)
        {
            List<GrdPersonaDestajo> lst;

            using (SamContext ctx = new SamContext())
            {
                //Traer los tuberos de un periodo destajo en particular a un lista genérica de personas
                lst = (from tb in ctx.DestajoTubero
                       join p in ctx.Tubero on tb.TuberoID equals p.TuberoID
                       where tb.PeriodoDestajoID == periodoDestajoID
                       select new GrdPersonaDestajo
                       {
                           ApMaterno = p.ApMaterno,
                           ApPaterno = p.ApPaterno,
                           Aprobado = tb.Aprobado,
                           EsTubero = true,
                           Codigo = p.Codigo,
                           IdPersona = p.TuberoID,
                           IdRegistroDetalle = tb.DestajoTuberoID,
                           Nombre = p.Nombre,
                           NumEmpleado = p.NumeroEmpleado,
                           SumaPdis = tb.DestajoTuberoDetalle.Sum(x => (decimal?)x.PDIs) ?? 0,
                           TotalAPagar = tb.GranTotal,
                       }).ToList();

                //Agregar a la lista de personas los soldadores de un periodo destajo en particular
                lst.AddRange((from sold in ctx.DestajoSoldador
                              join p in ctx.Soldador on sold.SoldadorID equals p.SoldadorID
                              where sold.PeriodoDestajoID == periodoDestajoID
                              select new GrdPersonaDestajo
                              {
                                  ApMaterno = p.ApMaterno,
                                  ApPaterno = p.ApPaterno,
                                  Aprobado = sold.Aprobado,
                                  EsTubero = false,
                                  Codigo = p.Codigo,
                                  IdPersona = p.SoldadorID,
                                  IdRegistroDetalle = sold.DestajoSoldadorID,
                                  Nombre = p.Nombre,
                                  NumEmpleado = p.NumeroEmpleado,
                                  SumaPdis = sold.DestajoSoldadorDetalle.Sum(x => (decimal?)x.PDIs) ?? 0,
                                  TotalAPagar = sold.GranTotal
                              }).ToList());
            }

            //Post procesamiento para llenar la entidad con los textos de internacionalización requeridos
            lst.ForEach(x =>
            {
                x.CategoriaPuestoTexto = TraductorEnumeraciones.TextoCategoriaPuesto(x.EsTubero);
                x.EstatusTexto = TraductorEnumeraciones.TextoEstatusDestajoPersona(x.Aprobado);
            });

            return lst;
        }

        /// <summary>
        /// Elimina a una persona de un periodo de destajo en particular.
        /// El método requiere que se le indique si se trata de un tubero o de un soldador
        /// ya que el ID que recibe irá a diferente tabla (DestajoTubero o DestajoSoldador)
        /// </summary>
        /// <param name="esTubero">Indica si el ID pasado corresponde a un tubero</param>
        /// <param name="idRegistro">Debe enviarse DestajoTuberoID si esTubero=true, se debe enviar DestajoSoldadorID si esTubero=false</param>
        public void BorraPersona(bool esTubero, int idRegistro)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (esTubero)
                    {
                        //Buscar el destajo del tubero
                        DestajoTubero dtT = ctx.DestajoTubero.Where(x => x.DestajoTuberoID == idRegistro).Single();
                        //cargar sus juntas
                        ctx.LoadProperty<DestajoTubero>(dtT, x => x.DestajoTuberoDetalle);

                        //eliminar juntas y destajo
                        for (int i = dtT.DestajoTuberoDetalle.Count - 1; i >= 0; i--)
                        {
                            ctx.DeleteObject(dtT.DestajoTuberoDetalle[i]);
                        }

                        ctx.DeleteObject(dtT);
                    }
                    else
                    {
                        //Buscar el destajo del soldador
                        DestajoSoldador dtS = ctx.DestajoSoldador.Where(x => x.DestajoSoldadorID == idRegistro).Single();
                        //Cargar sus juntas
                        ctx.LoadProperty<DestajoSoldador>(dtS, x => x.DestajoSoldadorDetalle);

                        //eliminar juntas y destajo
                        for (int i = dtS.DestajoSoldadorDetalle.Count - 1; i >= 0; i--)
                        {
                            ctx.DeleteObject(dtS.DestajoSoldadorDetalle[i]);
                        }

                        ctx.DeleteObject(dtS);
                    }

                    //enviar estatutos a la BD
                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// Marca el destajo de una persona en particular como aprobado.
        /// Si es tubero se debe enviar DestajoTuberoID, si es soldador se debe
        /// enviar DestajoSoldadorID.
        /// </summary>
        /// <param name="esTubero">Indica si el ID es de un destajo de tubero o de soldador</param>
        /// <param name="idRegistro">DestajoSoldadorID si esTubero=false, DestajoTuberoID si esTubero=true</param>
        /// <param name="userID">ID del usuario llevando a cabo la operación</param>
        public void ApruebaCalculoPersona(bool esTubero, int idRegistro, Guid userID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (esTubero)
                    {
                        DestajoTubero dtT = ctx.DestajoTubero.Where(x => x.DestajoTuberoID == idRegistro).Single();
                        dtT.StartTracking();
                        dtT.Aprobado = true;
                        dtT.UsuarioModifica = userID;
                        dtT.FechaModificacion = DateTime.Now;
                        ctx.DestajoTubero.ApplyChanges(dtT);
                    }
                    else
                    {
                        DestajoSoldador dtS = ctx.DestajoSoldador.Where(x => x.DestajoSoldadorID == idRegistro).Single();
                        dtS.StartTracking();
                        dtS.Aprobado = true;
                        dtS.UsuarioModifica = userID;
                        dtS.FechaModificacion = DateTime.Now;
                        ctx.DestajoSoldador.ApplyChanges(dtS);
                    }

                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// Lleva a cabo el cierre de periodo de destajo.  Cuando un periodo de destajo se cierra ocurre lo siguiente:
        /// 1. Se marca el periodo como cerrado lo cual se traduce a qe en el UI ya no se puede borrar ni modificar.
        /// 2. Se actualizan los registros de JuntaWorkstatus correspondientes con los totales pagados por cada concepto
        ///    soldadura y armado.
        /// </summary>
        /// <param name="periodoDestajoID">ID del periodo a cerrar</param>
        /// <param name="versionRegistro">Version del registro que se desea cerrar</param>
        /// <param name="userID">ID del usuario que realiza la operación</param>
        public void CerrarPeriodo(int periodoDestajoID, byte[] versionRegistro, Guid userID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ctx.DestajoSoldador.MergeOption = MergeOption.NoTracking;
                    ctx.DestajoTubero.MergeOption = MergeOption.NoTracking;
                    ctx.DestajoSoldadorDetalle.MergeOption = MergeOption.NoTracking;
                    ctx.DestajoTuberoDetalle.MergeOption = MergeOption.NoTracking;
                    ctx.JuntaWorkstatus.MergeOption = MergeOption.NoTracking;

                    bool puedeCerrar = ValidacionesDestajo.PuedeCerrarDestajo(ctx, periodoDestajoID);

                    if (!puedeCerrar)
                    {
                        throw new BaseValidationException(MensajesError.Excepcion_DebeAprobarTodosLosDestajos);
                    }

                    PeriodoDestajo periodo = ctx.PeriodoDestajo.Where(x => x.PeriodoDestajoID == periodoDestajoID).Single();

                    //si la instrucción se envía desde una página que puede manejar concurrencia lo tomamos en cuenta
                    if (versionRegistro != null)
                    {
                        periodo.VersionRegistro = versionRegistro;
                    }

                    //Si el periodo ya está aprobado mandamos la excepción
                    if (periodo.Aprobado)
                    {
                        throw new BaseValidationException(MensajesError.Excepcion_PeriodoDestajo_YaAprobado);
                    }

                    periodo.StartTracking();
                    periodo.UsuarioModifica = userID;
                    periodo.FechaModificacion = DateTime.Now;
                    periodo.Aprobado = true;

                    //tipo anónimo con la suma de armado por Jwid
                    var pagadoTubero = (from destajo in ctx.DestajoTubero
                                        join detalle in ctx.DestajoTuberoDetalle on destajo.DestajoTuberoID equals detalle.DestajoTuberoID
                                        where destajo.PeriodoDestajoID == periodoDestajoID
                                        group detalle by detalle.JuntaWorkstatusID into grupo
                                        select new
                                        {
                                            JuntaWorkstatusID = grupo.Key,
                                            Total = grupo.Sum(x => x.Total)
                                        }).ToList();

                    //tipo anónimo con la suma de soldadura por Jwid
                    var pagadoSoldador = (from destajo in ctx.DestajoSoldador
                                          join detalle in ctx.DestajoSoldadorDetalle on destajo.DestajoSoldadorID equals detalle.DestajoSoldadorID
                                          where destajo.PeriodoDestajoID == periodoDestajoID
                                          group detalle by detalle.JuntaWorkstatusID into grupo
                                          select new
                                          {
                                              JuntaWorkstatusID = grupo.Key,
                                              Total = grupo.Sum(x => x.Total)
                                          }).ToList();

                    //Ids de las juntas workstatus que se van a pagar por alguno de los conceptos
                    IQueryable<int> jwIds = pagadoTubero.Select(x => x.JuntaWorkstatusID)
                                                        .Union(pagadoSoldador.Select(y=>y.JuntaWorkstatusID))
                                                        .Distinct()
                                                        .AsQueryable();

                    //Traer las juntas workstatus que se van a pagar por algún concepto
                    List<JuntaWorkstatus> lstJwPagadas = ctx.JuntaWorkstatus
                                                            .Where(x => jwIds.Contains(x.JuntaWorkstatusID))
                                                            .ToList();

                    //Recorrer cada jw y actualizar la cantidad pagada por armada y/o soldadura así como
                    //la bandera de si el concepto ya fue pagado o no según se necesite
                    foreach (JuntaWorkstatus jw in lstJwPagadas)
                    {
                        decimal armado =
                            pagadoTubero.Where(x => x.JuntaWorkstatusID == jw.JuntaWorkstatusID)
                                        .Select(y => y.Total)
                                        .SingleOrDefault();

                        decimal soldadura =
                            pagadoSoldador.Where(x => x.JuntaWorkstatusID == jw.JuntaWorkstatusID)
                                          .Select(y => y.Total)
                                          .SingleOrDefault();

                        jw.StartTracking();

                        if (armado > 0)
                        {
                            jw.ArmadoPagado = true;
                            jw.TotalPagadoArmado = armado;
                        }

                        if (soldadura > 0)
                        {
                            jw.SoldaduraPagada = true;
                            jw.TotalPagadoSoldadura = soldadura;
                        }

                        jw.TotalPagado = (jw.TotalPagadoArmado ?? 0) + (jw.TotalPagadoSoldadura ?? 0);

                        jw.UsuarioModifica = userID;
                        jw.FechaModificacion = DateTime.Now;

                        //anexar al grafo del contexto
                        ctx.JuntaWorkstatus.ApplyChanges(jw);
                    }

                    ctx.PeriodoDestajo.ApplyChanges(periodo);
                    //Enviar estatutos a la BD
                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// Obtiene una lista de strings con los códigos de los tuberos y soldadores que forman parte de un
        /// mismo periodo de destajo. Sin embargo el método recibe el ID de alguno de los detalles del periodo,
        /// puede recibir DestajoSoldadorID o DetajoTuberoID y en conjunto con la variable esTubero hace el query
        /// "hacia arriba" para saber el periodo y luego obtener a todas las personas.
        /// 
        /// La lista de strings tiene una nomenclatura y ordenamiento especial que se debe respetar pues este método
        /// está pensado para usarse dentro del pager de la página de destajos, la nomenclatura es la siguiente:
        /// T-DestajoTuberoID, S-DestajoSoldadorID, es decir que los destajos de tubero se les antepone "T-" y a los de
        /// soldador "S-", la lista después está ordenada por tuberos primero y luego por soldadores, los tuberos se ordenan
        /// por código de tubero y los soldadores por código de soldador.
        /// 
        /// Ejemplo: T-56, T-28, S-15 --> DestajoTuberoID = 56, DestajoTuberoID = 28, DestajoSoldadorID =15
        /// </summary>
        /// <param name="esTubero">Indica si el ID es de un destajo de tubero o de soldador</param>
        /// <param name="idRegistro">DestajoSoldadorID si esTubero=false, DestajoTuberoID si esTubero=true</param>
        public List<string> ObtenerResumenPersonas(bool esTubero, int idRegistroDetalle)
        {
            List<string> lstIds = new List<string>();

            using (SamContext ctx = new SamContext())
            {
                ctx.DestajoSoldador.MergeOption = MergeOption.NoTracking;
                ctx.DestajoTubero.MergeOption = MergeOption.NoTracking;
                ctx.Tubero.MergeOption = MergeOption.NoTracking;
                ctx.Soldador.MergeOption = MergeOption.NoTracking;

                int periodoDestajoID = -1;

                //Traer el ID del periodo de destajo, se trae de distinta tabla dependiendo el caso
                if (esTubero)
                {
                    periodoDestajoID = ctx.DestajoTubero
                                          .Where(x => x.DestajoTuberoID == idRegistroDetalle)
                                          .Select(x => x.PeriodoDestajoID).Single();
                }
                else
                {
                    periodoDestajoID = ctx.DestajoSoldador
                                          .Where(x => x.DestajoSoldadorID == idRegistroDetalle)
                                          .Select(x => x.PeriodoDestajoID).Single();
                }

                //Tipo anónimo intencional
                var tuberos = (from dt in ctx.DestajoTubero
                               join tub in ctx.Tubero on dt.TuberoID equals tub.TuberoID
                               where dt.PeriodoDestajoID == periodoDestajoID
                               select new { Codigo = tub.Codigo, IdRegistro = dt.DestajoTuberoID }).ToList();

                //Tipo anónimo intencional
                var soldadores = (from dt in ctx.DestajoSoldador
                                  join sold in ctx.Soldador on dt.SoldadorID equals sold.SoldadorID
                                  where dt.PeriodoDestajoID == periodoDestajoID
                                  select new {Codigo = sold.Codigo, IdRegistro = dt.DestajoSoldadorID}).ToList();

                //Unión entre ambos queries
                lstIds.AddRange(tuberos.OrderBy(x => x.Codigo).Select(x => "T-" + x.IdRegistro));
                lstIds.AddRange(soldadores.OrderBy(x => x.Codigo).Select(x => "S-" + x.IdRegistro));
            }

            return lstIds;
        }

        /// <summary>
        /// Obtiene una entidad de tipo DestajoTubero basado en el ID de la misma tabla.
        /// A su vez regresa la relación con el tubero.
        /// </summary>
        /// <param name="destajoTuberoID">ID del destajo que se desea</param>
        /// <returns>Entidad de tipo DestajoTubero con su relación a Tubero llena</returns>
        public DestajoTubero ObtenerDestajoTuberoConDatosTubero(int destajoTuberoID)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.DestajoTubero.MergeOption = MergeOption.NoTracking;
                ctx.Tubero.MergeOption = MergeOption.NoTracking;

                return ctx.DestajoTubero
                          .Include("Tubero")
                          .Where(x => x.DestajoTuberoID == destajoTuberoID).Single();
            }
        }

        /// <summary>
        /// Obtiene una entidad de tipo DestajoTubero basado en el ID de la misma tabla.
        /// A su vez regresa la relación con las juntas de ese destajo.
        /// </summary>
        /// <param name="destajoTuberoID">ID del destajo que se desea</param>
        /// <returns>Entidad DestajoTubero con su colección de DestajoTuberoDetalle</returns>
        public DestajoTubero ObtenerDestajoTuberoConDetalle(int destajoTuberoID)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.DestajoTubero.MergeOption = MergeOption.NoTracking;
                ctx.DestajoTuberoDetalle.MergeOption = MergeOption.NoTracking;

                return ctx.DestajoTubero
                          .Include("DestajoTuberoDetalle")
                          .Where(x => x.DestajoTuberoID == destajoTuberoID).Single();
            }
        }
        
        /// <summary>
        /// Obtiene las juntas del destajo de un tubero en particular para mostrarlas en un grid.
        /// Se hace join con varias tablas para traer información almacenada en la junta y/o
        /// en la orden de trabajo.
        /// </summary>
        /// <param name="destajoTuberoID">ID del destajo del cual se desean las juntas</param>
        /// <returns>Lista con una entidad personalizada que contiene la información de las juntas para el UI</returns>
        public List<GrdDetalleDestajoTubero> ObtenerDetalleDestajoTubero(int destajoTuberoID)
        {
            List<GrdDetalleDestajoTubero> lst;

            using (SamContext ctx = new SamContext())
            {
                IQueryable<DestajoTuberoDetalle> iqDetalle = ctx.DestajoTuberoDetalle.Where(x => x.DestajoTuberoID == destajoTuberoID);
                IQueryable<JuntaWorkstatus> iqJw = ctx.JuntaWorkstatus.Where(x => iqDetalle.Select(y => y.JuntaWorkstatusID).Contains(x.JuntaWorkstatusID));

                lst = (from detalle in iqDetalle
                       join jw in iqJw on detalle.JuntaWorkstatusID equals jw.JuntaWorkstatusID
                       join js in ctx.JuntaSpool on jw.JuntaSpoolID equals js.JuntaSpoolID
                       join spool in ctx.Spool on js.SpoolID equals spool.SpoolID
                       join odts in ctx.OrdenTrabajoSpool on spool.SpoolID equals odts.SpoolID
                       select new GrdDetalleDestajoTubero
                       {
                           Ajuste = detalle.Ajuste,
                           ComentariosArmado = ctx.JuntaArmado.Where(x => x.JuntaWorkstatusID == jw.JuntaWorkstatusID).Select(y => y.Observaciones).FirstOrDefault(),
                           ComentariosDestajo = detalle.Comentarios,
                           CostoUnitario = detalle.CostoUnitario,
                           Destajo = detalle.Destajo,
                           DestajoTuberoDetalleID = detalle.DestajoTuberoDetalleID,
                           Diametro = detalle.PDIs,
                           EtiquetaJunta = jw.EtiquetaJunta,
                           FamiliaAceroID = js.FamiliaAceroMaterial1ID,
                           JuntaWorkstatusID = jw.JuntaWorkstatusID,
                           NumeroControl = odts.NumeroControl,
                           ProrrateoCuadro = detalle.ProrrateoCuadro,
                           ProrrateoDiasFestivos = detalle.ProrrateoDiasFestivos,
                           ProrrateoOtros = detalle.ProrrateoOtros,
                           Spool = spool.Nombre,
                           TipoJuntaID = js.TipoJuntaID,
                           Total = detalle.Total,
                           CostoDestajoVacio = detalle.CostoDestajoVacio
                       }).ToList();
            }

            //Usar cache para llenar el nombre de los tipos de junta y familias de acero
            Dictionary<int, string> tipoJta = CacheCatalogos.Instance.ObtenerTiposJunta().ToDictionary(x => x.ID, y => y.Nombre);
            Dictionary<int, string> famAcero = CacheCatalogos.Instance.ObtenerFamiliasAcero().ToDictionary(x => x.ID, y => y.Nombre);

            lst.ForEach(x =>
            {
                x.FamiliaAcero = famAcero[x.FamiliaAceroID];
                x.TipoJunta = tipoJta[x.TipoJuntaID];
            });

            return lst;
        }

        /// <summary>
        /// Obtiene una entidad de PeriodoDestajo en base al ID ya sea de DestajoTuberoID
        /// o de DestajoSoldadorID según sea el caso.
        /// </summary>
        /// <param name="esTubero">Indica si el ID es de un destajo de tubero o de soldador</param>
        /// <param name="idRegistro">DestajoSoldadorID si esTubero=false, DestajoTuberoID si esTubero=true</param>
        /// <returns>Entidad de tipo PeriodoDestajo con la información del periodo al cual pertenece el destajo</returns>
        public PeriodoDestajo ObtenerPeriodoEnBaseADetalle(bool esTubero, int idRegistroDetalle)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.PeriodoDestajo.MergeOption = MergeOption.NoTracking;
                ctx.DestajoTubero.MergeOption = MergeOption.NoTracking;
                ctx.DestajoSoldador.MergeOption = MergeOption.NoTracking;

                if (esTubero)
                {
                    return
                        ctx.PeriodoDestajo.Where(x => ctx.DestajoTubero
                                                         .Where(y => y.DestajoTuberoID == idRegistroDetalle)
                                                         .Select(z => z.PeriodoDestajoID)
                                                         .Contains(x.PeriodoDestajoID)).Single();
                }
                else
                {
                    return
                        ctx.PeriodoDestajo.Where(x => ctx.DestajoSoldador
                                                         .Where(y => y.DestajoSoldadorID == idRegistroDetalle)
                                                         .Select(z => z.PeriodoDestajoID)
                                                         .Contains(x.PeriodoDestajoID)).Single();
                }
            }
        }

        /// <summary>
        /// Obtiene una entidad de tipo DestajoSoldador basado en el ID de la misma tabla.
        /// A su vez regresa la relación con el soldador.
        /// </summary>
        /// <param name="destajoSoldadorID">ID del destajo que se desea</param>
        /// <returns>Entidad de tipo DestajoSoldador con su relación a Soldador llena</returns>
        public DestajoSoldador ObtenerDestajoSoldadorConDatosSoldador(int destajoSoldadorID)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.DestajoSoldador.MergeOption = MergeOption.NoTracking;
                ctx.Soldador.MergeOption = MergeOption.NoTracking;

                return ctx.DestajoSoldador.Include("Soldador").Where(x => x.DestajoSoldadorID == destajoSoldadorID).Single();
            }
        }

        /// <summary>
        /// Obtiene una entidad de tipo DestajoSoldador basado en el ID de la misma tabla.
        /// A su vez regresa la relación con las juntas de ese destajo.
        /// </summary>
        /// <param name="destajoSoldadorID">ID del destajo que se desea</param>
        /// <returns>Entidad DestajoSoldador con su colección de DestajoSoldadorDetalle</returns>
        public DestajoSoldador ObtenerDestajoSoldadorConDetalle(int destajoSoldadorID)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.DestajoSoldador.MergeOption = MergeOption.NoTracking;
                ctx.DestajoSoldadorDetalle.MergeOption = MergeOption.NoTracking;

                return ctx.DestajoSoldador
                          .Include("DestajoSoldadorDetalle")
                          .Where(x => x.DestajoSoldadorID == destajoSoldadorID).Single();
            }
        }

        /// <summary>
        /// Obtiene las juntas del destajo de un soldador en particular para mostrarlas en un grid.
        /// Se hace join con varias tablas para traer información almacenada en la junta y/o
        /// en la orden de trabajo.
        /// </summary>
        /// <param name="destajoSoldadorID">ID del destajo del cual se desean las juntas</param>
        /// <returns>Lista con una entidad personalizada que contiene la información de las juntas para el UI</returns>
        public List<GrdDetalleDestajoSoldador> ObtenerDetalleDestajoSoldador(int destajoSoldadorID)
        {
            List<GrdDetalleDestajoSoldador> lst;

            using (SamContext ctx = new SamContext())
            {
                IQueryable<DestajoSoldadorDetalle> iqDetalle = ctx.DestajoSoldadorDetalle.Where(x => x.DestajoSoldadorID == destajoSoldadorID);
                IQueryable<JuntaWorkstatus> iqJw = ctx.JuntaWorkstatus.Where(x => iqDetalle.Select(y => y.JuntaWorkstatusID).Contains(x.JuntaWorkstatusID));

                lst = (from detalle in iqDetalle
                       join jw in iqJw on detalle.JuntaWorkstatusID equals jw.JuntaWorkstatusID
                       join js in ctx.JuntaSpool on jw.JuntaSpoolID equals js.JuntaSpoolID
                       join spool in ctx.Spool on js.SpoolID equals spool.SpoolID
                       join odts in ctx.OrdenTrabajoSpool on spool.SpoolID equals odts.SpoolID
                       select new GrdDetalleDestajoSoldador
                       {
                           Ajuste = detalle.Ajuste,
                           ComentariosSoldadura = ctx.JuntaSoldadura.Where(x => x.JuntaWorkstatusID == jw.JuntaWorkstatusID).Select(y => y.Observaciones).FirstOrDefault(),
                           ComentariosDestajo = detalle.Comentarios,
                           CostoUnitarioRaiz = detalle.CostoUnitarioRaiz,
                           CostoUnitarioRelleno = detalle.CostoUnitarioRelleno,
                           DestajoRaiz = detalle.DestajoRaiz,
                           DestajoRelleno = detalle.DestajoRelleno,
                           DestajoSoldadorDetalleID = detalle.DestajoSoldadorDetalleID,
                           Diametro = detalle.PDIs,
                           EsDePeriodoAnterior = detalle.EsDePeriodoAnterior,
                           EtiquetaJunta = jw.EtiquetaJunta,
                           FamiliaAceroID = js.FamiliaAceroMaterial1ID,
                           JuntaWorkstatusID = jw.JuntaWorkstatusID,
                           NumeroControl = odts.NumeroControl,
                           NumeroFondeadores = detalle.NumeroFondeadores,
                           NumeroRellenadores = detalle.NumeroRellenadores,
                           ProrrateoCuadro = detalle.ProrrateoCuadro,
                           ProrrateoDiasFestivos = detalle.ProrrateoDiasFestivos,
                           ProrrateoOtros = detalle.ProrrateoOtros,
                           RaizDividida = detalle.RaizDividida,
                           RellenoDividido = detalle.RellenoDividido,
                           Spool = spool.Nombre,
                           TipoJuntaID = js.TipoJuntaID,
                           Total = detalle.Total,
                           CostoRaizVacio = detalle.CostoRaizVacio,
                           CostoRellenoVacio = detalle.CostoRellenoVacio
                       }).ToList();
            }

            Dictionary<int, string> tipoJta = CacheCatalogos.Instance.ObtenerTiposJunta().ToDictionary(x => x.ID, y => y.Nombre);
            Dictionary<int, string> famAcero = CacheCatalogos.Instance.ObtenerFamiliasAcero().ToDictionary(x => x.ID, y => y.Nombre);

            lst.ForEach(x =>
            {
                x.FamiliaAcero = famAcero[x.FamiliaAceroID];
                x.TipoJunta = tipoJta[x.TipoJuntaID];
            });

            return lst;
        }

        /// <summary>
        /// Elimina una junta en particular del destajo de un soldador.
        /// Cuando se eliminar un junta en particular se tienen que recalcular los prorrateos por junta de los
        /// siguientes conceptos: cuadro, días festivos y otros.
        /// 
        /// Se tiene que recalcular el total a pagar por destajo y por ajuste. En consecuencia todas estas
        /// operaciones afectan el total a pagar por cada junta y como total de la persona.
        /// </summary>
        /// <param name="destajoSoldadorDetalleID">ID de la junta que se desea eliminar del destajo</param>
        /// <param name="destajoSoldadorID">ID del registro padre al cual pertenece la junta</param>
        /// <param name="versionRegistro">version actual del registro padre (para manejo de concurrencia)</param>
        /// <param name="userID">Usuario que lleva a cabo la operación</param>
        public void EliminaDetalleDeSoldadura(int destajoSoldadorDetalleID, int destajoSoldadorID, byte [] versionRegistro, Guid userID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    //Cargar el registro padre y sus juntas
                    DestajoSoldador destajo = ctx.DestajoSoldador.Where(x => x.DestajoSoldadorID == destajoSoldadorID).Single();
                    ctx.LoadProperty<DestajoSoldador>(destajo, x => x.DestajoSoldadorDetalle);
                 
                    destajo.VersionRegistro = versionRegistro;
                    destajo.StartTracking();
                    destajo.FechaModificacion = DateTime.Now;
                    destajo.UsuarioModifica = userID;

                    //Detalle (junta) que se desea borrar del destajo
                    DestajoSoldadorDetalle detalleBorrar = destajo.DestajoSoldadorDetalle
                                                                  .Where(x => x.DestajoSoldadorDetalleID == destajoSoldadorDetalleID)
                                                                  .Single();

                    //Los detalles que no se van a borrar
                    List<DestajoSoldadorDetalle> detalles = destajo.DestajoSoldadorDetalle
                                                                   .Where(x => x.DestajoSoldadorDetalleID != detalleBorrar.DestajoSoldadorDetalleID)
                                                                   .ToList();

                    #region Volver a recalcular prorrateos

                    decimal totalPulgadas = detalles.Sum(x => (decimal ?)x.PDIs) ?? 0;
                    decimal factor;

                    if (totalPulgadas > 0)
                    {
                        detalles.ForEach(x =>
                        {
                            factor = x.PDIs / totalPulgadas;
                            x.StartTracking();
                            x.ProrrateoCuadro = destajo.TotalCuadro * factor;
                            x.ProrrateoDiasFestivos = destajo.TotalDiasFestivos * factor;
                            x.ProrrateoOtros = destajo.TotalOtros * factor;
                            x.UsuarioModifica = userID;
                            x.FechaModificacion = DateTime.Now;
                        });
                    }

                    #endregion

                    //Los totales de destajo y ajuste se recalculan restando la junta eliminada
                    destajo.TotalDestajoRaiz -= detalleBorrar.DestajoRaiz;
                    destajo.TotalDestajoRelleno -= detalleBorrar.DestajoRelleno;
                    destajo.TotalAjuste -= detalleBorrar.Ajuste;

                    //El gran total es la suma de todos los subtotales
                    destajo.GranTotal = destajo.TotalAjuste + destajo.TotalCuadro + destajo.TotalDestajoRaiz + destajo.TotalDestajoRelleno + destajo.TotalDiasFestivos + destajo.TotalOtros;

                    //borrar la junta
                    ctx.DeleteObject(detalleBorrar);
                    ctx.DestajoSoldador.ApplyChanges(destajo);
                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// Elimina una junta en particular del destajo de un tubero.
        /// Cuando se eliminar un junta en particular se tienen que recalcular los prorrateos por junta de los
        /// siguientes conceptos: cuadro, días festivos y otros.
        /// 
        /// Se tiene que recalcular el total a pagar por destajo y por ajuste. En consecuencia todas estas
        /// operaciones afectan el total a pagar por cada junta y como total de la persona.
        /// </summary>
        /// <param name="destajoTuberoDetalleID">ID de la junta que se desea eliminar del destajo</param>
        /// <param name="destajoTuberoID">ID del registro padre al cual pertenece la junta</param>
        /// <param name="versionRegistro">version actual del registro padre (para manejo de concurrencia)</param>
        /// <param name="userID">Usuario que lleva a cabo la operación</param>
        public void EliminaDetalleDeArmado(int destajoTuberoDetalleID, int destajoTuberoID, byte[] versionRegistro, Guid userID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    //Traer el registro padre y sus juntas
                    DestajoTubero destajo = ctx.DestajoTubero.Where(x => x.DestajoTuberoID == destajoTuberoID).Single();
                    ctx.LoadProperty<DestajoTubero>(destajo, x => x.DestajoTuberoDetalle);

                    destajo.VersionRegistro = versionRegistro;
                    destajo.StartTracking();
                    destajo.FechaModificacion = DateTime.Now;
                    destajo.UsuarioModifica = userID;

                    //Junta en particular que se desea eliminar del destajo
                    DestajoTuberoDetalle detalleBorrar = destajo.DestajoTuberoDetalle
                                                                .Where(x => x.DestajoTuberoDetalleID == destajoTuberoDetalleID)
                                                                .Single();

                    //Los detalles que no se van a borrar
                    List<DestajoTuberoDetalle> detalles = destajo.DestajoTuberoDetalle
                                                                 .Where(x => x.DestajoTuberoDetalleID != detalleBorrar.DestajoTuberoDetalleID)
                                                                 .ToList();

                    #region Volver a recalcular prorrateos

                    decimal totalPulgadas = detalles.Sum(x => (decimal?)x.PDIs) ?? 0;
                    decimal factor;

                    if (totalPulgadas > 0)
                    {
                        detalles.ForEach(x =>
                        {
                            factor = x.PDIs / totalPulgadas;
                            x.StartTracking();
                            x.ProrrateoCuadro = destajo.TotalCuadro * factor;
                            x.ProrrateoDiasFestivos = destajo.TotalDiasFestivos * factor;
                            x.ProrrateoOtros = destajo.TotalOtros * factor;
                            x.UsuarioModifica = userID;
                            x.FechaModificacion = DateTime.Now;
                        });
                    }

                    #endregion

                    //Los totales de destajo y ajuste se recalculan restando la junta eliminada
                    destajo.TotalDestajo -= detalleBorrar.Destajo;
                    destajo.TotalAjuste -= detalleBorrar.Ajuste;

                    //El gran total es la suma de todos los subtotales
                    destajo.GranTotal = destajo.TotalAjuste + destajo.TotalCuadro + destajo.TotalDestajo + destajo.TotalDiasFestivos + destajo.TotalOtros;

                    ctx.DeleteObject(detalleBorrar);
                    ctx.DestajoTubero.ApplyChanges(destajo);
                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// Marca un destajo como "no aprobado".  Puede ser el destajo de un soldador o de un tubero
        /// dependiendo del valor de la variable esTubero.
        /// </summary>
        /// <param name="esTubero">Indica si el ID es de un destajo de tubero o de soldador</param>
        /// <param name="idRegistro">DestajoSoldadorID si esTubero=false, DestajoTuberoID si esTubero=true</param>
        /// <param name="version">Version del registro DestajoSoldador o DestajoTubero (según sea el caso) para manejo de concurrencia</param>
        /// <param name="userID">Usuario que lleva a cabo la operación</param>
        public void CancelarAprobacionDestajoPersona(bool esTubero, int idRegistroDetalle, byte[] version, Guid userID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (esTubero)
                    {
                        DestajoTubero destajo = ctx.DestajoTubero.Where(x => x.DestajoTuberoID == idRegistroDetalle).Single();
                        destajo.VersionRegistro = version;
                        destajo.StartTracking();

                        destajo.FechaModificacion = DateTime.Now;
                        destajo.UsuarioModifica = userID;
                        destajo.Aprobado = false;

                        ctx.DestajoTubero.ApplyChanges(destajo);
                    }
                    else
                    {
                        DestajoSoldador destajo = ctx.DestajoSoldador.Where(x => x.DestajoSoldadorID == idRegistroDetalle).Single();
                        destajo.VersionRegistro = version;
                        destajo.StartTracking();

                        destajo.FechaModificacion = DateTime.Now;
                        destajo.UsuarioModifica = userID;
                        destajo.Aprobado = false;

                        ctx.DestajoSoldador.ApplyChanges(destajo);
                    }
                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// Guarda la entidad DestajoTubero en la BD así como todo lo que traiga
        /// en el grafo.
        /// </summary>
        /// <param name="destajo">Entidad con la información del destajo del tubero</param>
        public void GuardaDestajoTubero(DestajoTubero destajo)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ctx.DestajoTubero.ApplyChanges(destajo);
                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// Guarda la entidad DestajoSoldador en la BD así como todo lo que traiga
        /// en el grafo.
        /// </summary>
        /// <param name="destajo">Entidad con la información del destajo del soldador</param>
        public void GuardaDestajoSoldador(DestajoSoldador destajo)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ctx.DestajoSoldador.ApplyChanges(destajo);
                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// Obtiene la lista de periodos de destajo en la BD, la entidad que regresa se utiliza
        /// en el UI para un grid.
        /// </summary>
        /// <param name="fechaInicio">Si es nulo no afecta, si tiene algún valor se trae sólo aquellos periodos con fechaInicio >= valor parámetro</param>
        /// <param name="fechaFin">Si es nulo no afecta, si tiene algún valor se trae sólo aquellos periodos con fechaFin menor o igual al valor parámetro</param>
        /// <returns>Lista de periodos de destajo filtrado en base a los valores pasados</returns>
        public List<GrdPeriodoDestajo> ObtenerListaPeriodosFiltrado(DateTime? fechaInicio, DateTime? fechaFin)
        {
            List<GrdPeriodoDestajo> lst;

            using (SamContext ctx = new SamContext())
            {
                ctx.PeriodoDestajo.MergeOption = MergeOption.NoTracking;
                ctx.DestajoSoldador.MergeOption = MergeOption.NoTracking;
                ctx.DestajoTubero.MergeOption = MergeOption.NoTracking;

                IQueryable<PeriodoDestajo> queryPeriodos = ctx.PeriodoDestajo;

                if (fechaInicio.HasValue)
                {
                    queryPeriodos = queryPeriodos.Where(x => x.FechaInicio >= fechaInicio.Value);
                }

                if (fechaFin.HasValue)
                {
                    queryPeriodos = queryPeriodos.Where(x => x.FechaFin <= fechaFin.Value);
                }

                lst = (from periodo in queryPeriodos
                       select new GrdPeriodoDestajo
                       {
                           PeriodoDestajoID = periodo.PeriodoDestajoID,
                           Anio = periodo.Anio,
                           CantidadDiasFestivos = periodo.CantidadDiasFestivos,
                           CantidadSoldadores = periodo.DestajoSoldador.Count(),
                           CantidadTuberos = periodo.DestajoTubero.Count(),
                           Cerrado = periodo.Aprobado,
                           FechaFin = periodo.FechaFin,
                           FechaInicio = periodo.FechaInicio,
                           Semana = periodo.Semana,
                           TotalAPagar = (periodo.DestajoTubero.Sum(x => (decimal?)x.GranTotal) ?? 0)
                                         + (periodo.DestajoSoldador.Sum(x => (decimal?)x.GranTotal) ?? 0),
                           DestajosSoldadorAprobados = periodo.DestajoSoldador.Count(x => x.Aprobado),
                           DestajosTuberoAprobados = periodo.DestajoTubero.Count(x => x.Aprobado)
                       }).ToList();
            }

            lst.ForEach(x =>
            {
                if (x.Cerrado)
                {
                    x.Estatus = EstatusPeriodoDestajo.Cerrado;
                }
                else if (x.CantidadSoldadores == x.DestajosSoldadorAprobados && x.CantidadTuberos == x.DestajosTuberoAprobados)
                {
                    x.Estatus = EstatusPeriodoDestajo.ListoParaCierre;
                }
                else
                {
                    x.Estatus = EstatusPeriodoDestajo.Pendiente;
                }

                x.EstatusTexto = TraductorEnumeraciones.TextoEstatusPeriodoDestajo(x.Estatus);
            });

            return lst;
        }

        /// <summary>
        /// Eliminar un periodo de destajo de la BD así como todas sus relaciones.
        /// Por cuestiones de desempeño se hace a través de un stored procedure
        /// por lo cual estamos obligados a englobarlo en una transacción.
        /// </summary>
        /// <param name="periodoDestajoID"></param>
        public void BorrarPeriodo(int periodoDestajoID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (SamContext ctx = new SamContext())
                {
                    if (!ValidacionesDestajo.PuedeEliminarPeriodo(ctx, periodoDestajoID))
                    {
                        throw new BaseValidationException(MensajesError.Excepcion_PeriodoDestajo_YaAprobado);
                    }

                    ctx.BorraPeriodoDestajo(periodoDestajoID);
                }
                
                scope.Complete();
            }
        }

        /// <summary>
        /// Obtiene una lista de las personas dentro de un periodo de destajo en particular.
        /// La lista contiene la información solicitada por el archivo de Excel.
        /// </summary>
        /// <param name="periodoDestajoID">ID del periodo de destajo deseado</param>
        /// <returns>Lista con un registro por cada persona dentro del periodo</returns>
        public List<GrdPersonaDestajoExcel> ObtenerPersonasPorPeriodoParaExcel(int periodoDestajoID)
        {
            List<GrdPersonaDestajoExcel> lst;

            using (SamContext ctx = new SamContext())
            {
                //Traer tuberos a una lista genérica de personas
                lst = (from tb in ctx.DestajoTubero
                       join p in ctx.Tubero on tb.TuberoID equals p.TuberoID
                       where tb.PeriodoDestajoID == periodoDestajoID
                       select new GrdPersonaDestajoExcel
                       {
                           ApMaterno = p.ApMaterno,
                           ApPaterno = p.ApPaterno,
                           Aprobado = tb.Aprobado,
                           EsTubero = true,
                           Codigo = p.Codigo,
                           IdPersona = p.TuberoID,
                           IdRegistroDetalle = tb.DestajoTuberoID,
                           Nombre = p.Nombre,
                           NumEmpleado = p.NumeroEmpleado,
                           SumaPdis = tb.DestajoTuberoDetalle.Sum(x => (decimal?)x.PDIs) ?? 0,
                           TotalAPagar = tb.GranTotal,
                           Comentarios = tb.Comentarios
                       }).ToList();

                //Anexar a la lista los soldadores
                lst.AddRange((from sold in ctx.DestajoSoldador
                              join p in ctx.Soldador on sold.SoldadorID equals p.SoldadorID
                              where sold.PeriodoDestajoID == periodoDestajoID
                              select new GrdPersonaDestajoExcel
                              {
                                  ApMaterno = p.ApMaterno,
                                  ApPaterno = p.ApPaterno,
                                  Aprobado = sold.Aprobado,
                                  EsTubero = false,
                                  Codigo = p.Codigo,
                                  IdPersona = p.SoldadorID,
                                  IdRegistroDetalle = sold.DestajoSoldadorID,
                                  Nombre = p.Nombre,
                                  NumEmpleado = p.NumeroEmpleado,
                                  SumaPdis = sold.DestajoSoldadorDetalle.Sum(x => (decimal?)x.PDIs) ?? 0,
                                  TotalAPagar = sold.GranTotal,
                                  Comentarios = sold.Comentarios
                              }).ToList());
            }

            //Calcular propiedades fuera del contexto de BD
            lst.ForEach(x =>
            {
                x.CategoriaPuestoTexto = TraductorEnumeraciones.TextoCategoriaPuesto(x.EsTubero);
                x.EstatusTexto = TraductorEnumeraciones.TextoEstatusDestajoPersona(x.Aprobado);
            });

            return lst;
        }

        /// <summary>
        /// Obtiene una lista con el detalle de todas las juntas armadas dentro de un periodo de destajo en particular.
        /// El método hace un par de joins en la BD que hasta el momento no han resultado pesados, falta ver
        /// como se comporta con más información.
        /// </summary>
        /// <param name="periodoDestajoID">ID del periodo del cual se desean las juntas</param>
        public List<GrdDetalleDestajoTuberoExcel> ObtenerDetalleDestajoTuberoPorPeriodo(int periodoDestajoID)
        {
            List<GrdDetalleDestajoTuberoExcel> lst;

            using (SamContext ctx = new SamContext())
            {
                ctx.DestajoTuberoDetalle.MergeOption = MergeOption.NoTracking;
                ctx.DestajoTuberoDetalle.MergeOption = MergeOption.NoTracking;
                ctx.JuntaWorkstatus.MergeOption = MergeOption.NoTracking;
                ctx.JuntaSpool.MergeOption = MergeOption.NoTracking;
                ctx.Spool.MergeOption = MergeOption.NoTracking;
                ctx.OrdenTrabajoSpool.MergeOption = MergeOption.NoTracking;
                ctx.JuntaArmado.MergeOption = MergeOption.NoTracking;

                IQueryable<DestajoTuberoDetalle> iqDetalle =
                    ctx.DestajoTuberoDetalle
                       .Where(x => ctx.DestajoTubero
                                      .Where(s => s.PeriodoDestajoID == periodoDestajoID)
                                      .Select(y => y.DestajoTuberoID)
                                      .Contains(x.DestajoTuberoID));

                IQueryable<JuntaWorkstatus> iqJw =
                    ctx.JuntaWorkstatus
                       .Where(x => iqDetalle.Select(y => y.JuntaWorkstatusID).Contains(x.JuntaWorkstatusID));

                lst = (from detalle in iqDetalle
                       join destajo in ctx.DestajoTubero.Where(x => x.PeriodoDestajoID == periodoDestajoID) on detalle.DestajoTuberoID equals destajo.DestajoTuberoID
                       join jw in iqJw on detalle.JuntaWorkstatusID equals jw.JuntaWorkstatusID
                       join armado in ctx.JuntaArmado on jw.JuntaWorkstatusID equals armado.JuntaWorkstatusID
                       join js in ctx.JuntaSpool on jw.JuntaSpoolID equals js.JuntaSpoolID
                       join spool in ctx.Spool on js.SpoolID equals spool.SpoolID
                       join odts in ctx.OrdenTrabajoSpool on spool.SpoolID equals odts.SpoolID
                       select new GrdDetalleDestajoTuberoExcel
                       {
                           Ajuste = detalle.Ajuste,
                           ComentariosArmado = armado.Observaciones,
                           ComentariosDestajo = detalle.Comentarios,
                           CostoUnitario = detalle.CostoUnitario,
                           Destajo = detalle.Destajo,
                           DestajoTuberoDetalleID = detalle.DestajoTuberoDetalleID,
                           Diametro = detalle.PDIs,
                           EtiquetaJunta = jw.EtiquetaJunta,
                           FamiliaAceroID = js.FamiliaAceroMaterial1ID,
                           JuntaWorkstatusID = jw.JuntaWorkstatusID,
                           NumeroControl = odts.NumeroControl,
                           ProrrateoCuadro = detalle.ProrrateoCuadro,
                           ProrrateoDiasFestivos = detalle.ProrrateoDiasFestivos,
                           ProrrateoOtros = detalle.ProrrateoOtros,
                           Spool = spool.Nombre,
                           TipoJuntaID = js.TipoJuntaID,
                           Total = detalle.Total,
                           Isometrico = spool.Dibujo,
                           FechaArmado = armado.FechaArmado,
                           TuberoID = destajo.TuberoID,
                           Cedula = js.Cedula
                       }).ToList();
            }

            Dictionary<int, string> tipoJta = CacheCatalogos.Instance.ObtenerTiposJunta().ToDictionary(x => x.ID, y => y.Nombre);
            Dictionary<int, string> famAcero = CacheCatalogos.Instance.ObtenerFamiliasAcero().ToDictionary(x => x.ID, y => y.Nombre);

            lst.ForEach(x =>
            {
                x.FamiliaAcero = famAcero[x.FamiliaAceroID];
                x.TipoJunta = tipoJta[x.TipoJuntaID];
            });

            return lst;
        }

        /// <summary>
        /// Obtiene una lista con el detalle de todas las juntas soldadas dentro de un periodo de destajo en particular.
        /// El método hace un par de joins en la BD que hasta el momento no han resultado pesados, falta ver
        /// como se comporta con más información.
        /// </summary>
        /// <param name="periodoDestajoID">ID del periodo del cual se desean las juntas</param>
        public List<GrdDetalleDestajoSoldadorExcel> ObtenerDetalleDestajoSoldadorPorPeriodo(int periodoDestajoID)
        {
            List<GrdDetalleDestajoSoldadorExcel> lst;

            using (SamContext ctx = new SamContext())
            {
                ctx.DestajoSoldadorDetalle.MergeOption = MergeOption.NoTracking;
                ctx.DestajoSoldador.MergeOption = MergeOption.NoTracking;
                ctx.JuntaWorkstatus.MergeOption = MergeOption.NoTracking;
                ctx.JuntaSpool.MergeOption = MergeOption.NoTracking;
                ctx.Spool.MergeOption = MergeOption.NoTracking;
                ctx.OrdenTrabajoSpool.MergeOption = MergeOption.NoTracking;
                ctx.JuntaSoldadura.MergeOption = MergeOption.NoTracking;

                IQueryable<DestajoSoldadorDetalle> iqDetalle =
                    ctx.DestajoSoldadorDetalle
                       .Where(x => ctx.DestajoSoldador
                                      .Where(p => p.PeriodoDestajoID == periodoDestajoID)
                                      .Select(y => y.DestajoSoldadorID)
                                      .Contains(x.DestajoSoldadorID));
                
                IQueryable<JuntaWorkstatus> iqJw =
                    ctx.JuntaWorkstatus
                       .Where(x => iqDetalle.Select(y => y.JuntaWorkstatusID).Contains(x.JuntaWorkstatusID));

                lst = (from detalle in iqDetalle
                       join destajo in ctx.DestajoSoldador.Where(x => x.PeriodoDestajoID == periodoDestajoID) on detalle.DestajoSoldadorID equals destajo.DestajoSoldadorID
                       join jw in iqJw on detalle.JuntaWorkstatusID equals jw.JuntaWorkstatusID
                       join soldadura in ctx.JuntaSoldadura on jw.JuntaWorkstatusID equals soldadura.JuntaWorkstatusID
                       join js in ctx.JuntaSpool on jw.JuntaSpoolID equals js.JuntaSpoolID
                       join spool in ctx.Spool on js.SpoolID equals spool.SpoolID
                       join odts in ctx.OrdenTrabajoSpool on spool.SpoolID equals odts.SpoolID
                       select new GrdDetalleDestajoSoldadorExcel
                       {
                           Ajuste = detalle.Ajuste,
                           ComentariosSoldadura = soldadura.Observaciones,
                           ComentariosDestajo = detalle.Comentarios,
                           CostoUnitarioRaiz = detalle.CostoUnitarioRaiz,
                           CostoUnitarioRelleno = detalle.CostoUnitarioRelleno,
                           DestajoRaiz = detalle.DestajoRaiz,
                           DestajoRelleno = detalle.DestajoRelleno,
                           DestajoSoldadorDetalleID = detalle.DestajoSoldadorDetalleID,
                           Diametro = detalle.PDIs,
                           EsDePeriodoAnterior = detalle.EsDePeriodoAnterior,
                           EtiquetaJunta = jw.EtiquetaJunta,
                           FamiliaAceroID = js.FamiliaAceroMaterial1ID,
                           JuntaWorkstatusID = jw.JuntaWorkstatusID,
                           NumeroControl = odts.NumeroControl,
                           NumeroFondeadores = detalle.NumeroFondeadores,
                           NumeroRellenadores = detalle.NumeroRellenadores,
                           ProrrateoCuadro = detalle.ProrrateoCuadro,
                           ProrrateoDiasFestivos = detalle.ProrrateoDiasFestivos,
                           ProrrateoOtros = detalle.ProrrateoOtros,
                           RaizDividida = detalle.RaizDividida,
                           RellenoDividido = detalle.RellenoDividido,
                           Spool = spool.Nombre,
                           TipoJuntaID = js.TipoJuntaID,
                           Total = detalle.Total,
                           Isometrico = spool.Dibujo,
                           FechaSoldadura = soldadura.FechaSoldadura,
                           ProcesoRaizID = soldadura.ProcesoRaizID ?? -1,
                           ProcesoRellenoID = soldadura.ProcesoRellenoID ?? -1,
                           SoldadorID = destajo.SoldadorID,
                           Cedula = js.Cedula
                       }).ToList();
            }

            Dictionary<int, string> tipoJta = CacheCatalogos.Instance.ObtenerTiposJunta().ToDictionary(x => x.ID, y => y.Nombre);
            Dictionary<int, string> famAcero = CacheCatalogos.Instance.ObtenerFamiliasAcero().ToDictionary(x => x.ID, y => y.Nombre);
            Dictionary<int, string> raiz = CacheCatalogos.Instance.ObtenerProcesosRaiz().ToDictionary(x => x.ID, y => y.Nombre);
            Dictionary<int, string> relleno = CacheCatalogos.Instance.ObtenerProcesosRelleno().ToDictionary(x => x.ID, y => y.Nombre);

            lst.ForEach(x =>
            {
                x.FamiliaAcero = famAcero[x.FamiliaAceroID];
                x.TipoJunta = tipoJta[x.TipoJuntaID];
                
                if (x.ProcesoRaizID > 0)
                {
                    x.ProcesoRaiz = raiz[x.ProcesoRaizID];
                }

                if (x.ProcesoRellenoID > 0)
                {
                    x.ProcesoRelleno = relleno[x.ProcesoRellenoID];
                }
            });

            return lst;
        }
    }
}
