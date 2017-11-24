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
using Mimo.Framework.Exceptions;
using Mimo.Framework.Common;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Cache;
using System.Transactions;
using SAM.Entities.Personalizadas;
using System.Text.RegularExpressions;

namespace SAM.BusinessObjects.Produccion
{
    public class OrdenTrabajoBO
    {
        private static readonly  object _mutex = new object();
        private static OrdenTrabajoBO _instance;

        /// <summary>
        /// constructro privado para implementar patron singleton
        /// </summary>
        private OrdenTrabajoBO()
        {
        }

        /// <summary>
        /// obtiene la instancia de la clase OrdenTrabajoBO
        /// </summary>
        public static OrdenTrabajoBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new OrdenTrabajoBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// permite obtener una orden de trabajo dependiendo del ordenTrabajoID que se le envía.
        /// </summary>
        /// <param name="ordenTrabajoID"></param>
        /// <returns></returns>
        public OrdenTrabajo Obtener(int ordenTrabajoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.OrdenTrabajo.Include("Taller")
                          .Where(x => x.OrdenTrabajoID == ordenTrabajoID)
                          .SingleOrDefault();
            }
        }

        //public OrdenTrabajoEspecial ObtenerODTEspecial(int ordentrabajoEspecialID)
        //{
        //    using (SamContext ctx = new SamContext())
        //    {
        //        return ctx.OrdenTrabajoEspecial.Include("Taller")
        //                    .Where(x => x.OrdenTrabajoEspecialID == ordentrabajoEspecialID)
        //                    .SingleOrDefault();
        //    }
        //}

        public bool verificaSpoolConCongeladoParcial(int spool)
        {
            using (SamContext ctx = new SamContext())
            {
                 bool query = (from cong in ctx.CongeladoParcial
                         join mat in ctx.MaterialSpool on cong.MaterialSpoolID equals mat.MaterialSpoolID
                         join sp in ctx.Spool.Where(x => x.SpoolID == spool) on mat.SpoolID equals sp.SpoolID
                         select cong).Any();
                 return query;
            }
        }
       
        /// <summary>
        /// Regresa una entidad de tipo OrdenTrabajo con todos sus registro de
        /// OrdenTrabajoSpool relacionados.
        /// </summary>
        /// <param name="ordenTrabajoID">ID de la orden de trabajo que se desea regresar</param>
        /// <returns>Entidad OrdenTrabajo con sus hijos (colección) de objetos OrdenTrabajoSpool</returns>
        public OrdenTrabajo ObtenerConOdtSpool(int ordenTrabajoID)
        {
            using (SamContext ctx = new SamContext())
            {
                OrdenTrabajo odt = ctx.OrdenTrabajo
                                      .Where(x => x.OrdenTrabajoID == ordenTrabajoID)
                                      .Single();

                ctx.LoadProperty<OrdenTrabajo>(odt, x => x.OrdenTrabajoSpool);

                return odt;
            }
        
        }

        public int DigitsOnly(string strData) 
        {
            string data = Regex.Replace(strData, "[^0-9]","");
            return int.Parse(data);
        }
        /// <summary>
        /// Regresa una lista de objetos personalizados utilizados para el grid de órdenes de trabajo
        /// que se despliega en la página web.
        /// </summary>
        /// <param name="patioID">ID del patio del cual se desean las Odts</param>
        /// <param name="proyectoID">ID del proyecto del cual se desean las Odts</param>
        /// <param name="tallerID">ID del taller</param>
        /// <param name="pids">Ids de los proyectos a los cuales tenemos permisos</param>
        /// <returns>Objetos con la información necesaria relativa a una Odt</returns>
        public List<GrdOdt> ObtenerListaParaGrid(int? patioID, int? proyectoID, int? tallerID, int [] pids)
        {
            //Los filtros no pueden venir completamente vacíos o nos traeríamos todas las
            //ODTs de la BD
            if (patioID == null && proyectoID == null)
            {
                throw new ArgumentException("El patio o el proyecto son requeridos");
            }
            
            //shop fab area
            int fabAreaID = CacheCatalogos.Instance.ShopFabAreaID;

            List<GrdOdt> lstOrdenes = null;

            using (SamContext ctx = new SamContext())
            {
                IQueryable<OrdenTrabajo> query = ctx.OrdenTrabajo.AsQueryable();

                #region aplicar filtros

                if (patioID.HasValue && patioID.Value > 0 && !(proyectoID.HasValue && proyectoID.Value > 0))
                {
                    query = query.Where(x => x.Proyecto.PatioID == patioID.Value);
                }

                if (proyectoID.HasValue && proyectoID.Value > 0)
                {
                    query = query.Where(x => x.ProyectoID == proyectoID.Value);
                }
                else
                {
                    IQueryable<int> iqPids = pids.AsQueryable();
                    query = query.Where(x => iqPids.Contains(x.ProyectoID));
                }

                if (tallerID.HasValue && tallerID.Value > 0)
                {
                    query = query.Where(x => x.TallerID == tallerID.Value);
                }

                #endregion

                lstOrdenes = (from odts in query
                              select new GrdOdt
                              {
                                  CantidadSpools = odts.OrdenTrabajoSpool.Count(),
                                  EstatusOrdenID = odts.EstatusOrdenID,
                                  Fecha = odts.FechaOrden,
                                  NumeroOrden = odts.NumeroOrden,
                                  OrdenDeTrabajoID = odts.OrdenTrabajoID,
                                  ProyectoID = odts.ProyectoID,
                                  TallerID = odts.TallerID,
                                  Version = (int)odts.VersionOrden                                  
                              }).ToList();

                //Query que obtiene solo las órdenes de trabajo spool que nos interesan
                IQueryable<OrdenTrabajoSpool> ots = ctx.OrdenTrabajoSpool
                                                       .Where(os => query.SelectMany(x => x.OrdenTrabajoSpool)
                                                                         .Select(y => y.OrdenTrabajoSpoolID)
                                                                         .Contains(os.OrdenTrabajoSpoolID));

                //Cuantas juntas y materiales hay en cada orden de trabajo spool
                var grupo = (from o in ots
                            select new
                            {
                                OrdenTrabajoID = o.OrdenTrabajoID,
                                OrdenTrabajoSpoolID = o.OrdenTrabajoSpoolID,
                                SpoolID = o.SpoolID,
                                CantidadMateriales = o.OrdenTrabajoMaterial.Count(),
                                CantidadJuntas = o.OrdenTrabajoJunta.Count(),
                                CantidadMaterialesConReingenieria = o.OrdenTrabajoMaterial.Count(x => x.FueReingenieria),
                                CantidadJuntasConReingenieria = o.OrdenTrabajoJunta.Count(x => x.FueReingenieria),
                                MaterialesDespachados = o.OrdenTrabajoMaterial.Count(y => y.TieneDespacho)
                            }).ToList();

                //Cuales son los spools involucrados en las ODts que nos estamos trayendo
                IQueryable<int> spools = ots.Select(y => y.SpoolID);

                //Cuantas juntas y materiales hay en cada spool, contar únicamente las juntas tipo SHOP
                var grupoIng = (from s in ctx.Spool
                                where spools.Contains(s.SpoolID)
                                select new
                                {
                                    SpoolID = s.SpoolID,
                                    CantidadJuntas = s.JuntaSpool.Count(x => x.FabAreaID == fabAreaID),
                                    CantidadMateriales = s.MaterialSpool.Count()
                                }).ToList();


                //Spool x spool nos dice el estatus de su despacho y si difiere entre ingeniería y producción
                var joinGrupos = (from grpO in grupo
                                  join grpIng in grupoIng on grpO.SpoolID equals grpIng.SpoolID
                                  select new
                                  {
                                      OrdenTrabajoID =grpO.OrdenTrabajoID,
                                      OrdenTrabajoSpoolID = grpO.OrdenTrabajoSpoolID,
                                      SpoolID = grpO.SpoolID,
                                      CantidadMaterialesEnOdt = grpO.CantidadMateriales,
                                      CantidadJuntasEnOdt = grpO.CantidadJuntas,
                                      CantidadMaterialesConReingenieria = grpO.CantidadMaterialesConReingenieria,
                                      CantidadJuntasConReingenieria = grpO.CantidadJuntasConReingenieria,
                                      MaterialesDespachados = grpO.MaterialesDespachados,
                                      CantidadJuntasIngenieria = grpIng.CantidadJuntas,
                                      CantidadMaterialesIngenieria = grpIng.CantidadMateriales,
                                      EstatusDespachoSpool = grpO.MaterialesDespachados == grpO.CantidadMateriales
                                                             ? EstatusDespachoOdt.Despachada
                                                             : (grpO.MaterialesDespachados == 0 ? EstatusDespachoOdt.SinDespacho : EstatusDespachoOdt.Parcial),
                                      Difieren = grpO.CantidadMateriales != grpIng.CantidadMateriales || grpIng.CantidadJuntas != grpO.CantidadJuntas
                                  }).ToList();


                #region Calcular la propiedades faltantes del objeto que vamos a regresar

                List<ProyectoCache> pc = CacheCatalogos.Instance.ObtenerProyectos();
                List<PatioCache> pac = CacheCatalogos.Instance.ObtenerPatios();
                ProyectoCache proyecto;

                lstOrdenes.ForEach(o =>
                {
                    //tipo anónimo necesario, esto tiene varios registros para una sola ODT
                    var filtro = joinGrupos.Where(x => x.OrdenTrabajoID == o.OrdenDeTrabajoID);

                    //Con que algún spool difiera es suficiente para marcar la ODT como que difiere
                    o.DifiereDeIngenieria = filtro.Any(y => y.Difieren);
                    o.FueReingenieria = filtro.Any(y => y.CantidadJuntasConReingenieria > 0 || y.CantidadMaterialesConReingenieria > 0);

                    if (filtro.Count()== 0 || filtro.All(x => x.EstatusDespachoSpool == EstatusDespachoOdt.SinDespacho)) 
                    {
                        o.EstatusDespacho = EstatusDespachoOdt.SinDespacho; 
                    }
                    else if (filtro.All(x => x.EstatusDespachoSpool == EstatusDespachoOdt.Despachada))
                    {
                        o.EstatusDespacho = EstatusDespachoOdt.Despachada;
                    }
                    else
                    {
                        o.EstatusDespacho = EstatusDespachoOdt.Parcial;
                    }

                    //Traer el proyecto de Cache
                    proyecto = pc.Where(x => x.ID == o.ProyectoID).Single();
                    
                    o.Proyecto = proyecto.Nombre;
                    o.PatioID = proyecto.PatioID;
                    o.Patio = proyecto.NombrePatio;

                    //Calcular el taller con la info de Cache
                    o.Taller = pac.Where(x => x.ID == o.PatioID)
                                  .Single()
                                  .Talleres
                                  .Where(y => y.ID == o.TallerID)
                                  .Single().Nombre;

                    o.Estatus = TraductorEnumeraciones.TextoEstatusOrdenDeTrabajo((EstatusOrdenDeTrabajo)o.EstatusOrdenID);
                    o.EstatusDespachoTexto = TraductorEnumeraciones.TextoEstatusDespacho(o.EstatusDespacho);
                    o.Orden = DigitsOnly(o.NumeroOrden);
                });

                #endregion
            }

            return lstOrdenes;
        }

        /// <summary>
        /// Elimina una ODT en caso que las reglas de negocio lo permitan, las reglas de negocio son
        /// las siguientes:
        /// - No debe haber ningún corte relacionado con la ODT
        /// - No debe haber ningún despacho relacionado con la ODT
        /// </summary>
        /// <param name="ordenTrabajoID">ID de la ODT que se desea eliminar</param>
        /// <param name="userID">Usuario que lleva a cabo la opción</param>
        public void Borra(int ordenTrabajoID, Guid userID)
        {
            try
            {
                List<string> errores = new List<string>();
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SamContext ctx = new SamContext())
                    {
                        #region validar cortes y despachos para saber si podemos borrar la ODT

                        if (!ValidacionesOdt.ValidaOdtNoTengaCortes(ctx, ordenTrabajoID))
                        {
                            errores.Add(MensajesError.Excepcion_OdtTieneCortes);
                        }

                        if (!ValidacionesOdt.ValidaOdtNoTengaDespachos(ctx, ordenTrabajoID))
                        {
                            errores.Add(MensajesError.Excepcion_OdtTieneDespachos);
                        }

                        if (!ValidacionesOdt.ValidaOdtNoTengaNumerosUnicosEnCorte(ctx, ordenTrabajoID))
                        {
                            errores.Add(MensajesError.Excepcion_OdtTieneNusEnCorte);
                        }
                        if (!ValidacionesOdt.ValidaOdtNoTengaArmadoSoldado(ctx, ordenTrabajoID))
                        {
                            errores.Add(MensajesError.Excepcion_OdtConArmadoSoldado);
                        }
                        if (!ValidacionesOdt.ValidaOdtNoTengaRequisiciones(ctx, ordenTrabajoID))
                        {
                            errores.Add(MensajesError.Excepcion_OdtRequisiciones);
                        }
                        if (!ValidacionesOdt.ValidaOdtNoTengaRequisicionesPintura(ctx, ordenTrabajoID))
                        {
                            errores.Add(MensajesError.Excpecion_OdtConRequisicionPintura);
                        }
                        if (!ValidacionesOdt.ValidaOdtNoTengaLiberacionDimensional(ctx, ordenTrabajoID))
                        {
                            errores.Add(MensajesError.Excepcion_OdtConLiberacionDimensional);
                        }
                        if(!ValidacionesOdt.ValidaOdtNoTengaEmbarque(ctx,ordenTrabajoID))
                        {
                            errores.Add(MensajesError.Excepcion_OdtConEmbarque);
                        }

                        if (errores.Count > 0)
                        {
                            throw new ExcepcionOdt(errores);
                        }

                        #endregion

                        #region Traer los objetos necesarios de la BD

                        //Traer la ODT
                        OrdenTrabajo odt = ctx.OrdenTrabajo.Where(x => x.OrdenTrabajoID == ordenTrabajoID).Single();

                        IQueryable<int> odtSpoolIds = ctx.OrdenTrabajoSpool
                                                            .Where(x => x.OrdenTrabajoID == ordenTrabajoID)
                                                            .Select(y => y.OrdenTrabajoSpoolID);
                        
                        //Traer las juntasWks que se hayan podido haber generado pero que ya deberían estar vacías
                        List<JuntaWorkstatus> juntaWks = ctx.JuntaWorkstatus.Where(x => odtSpoolIds.Contains(x.OrdenTrabajoSpoolID)).ToList();

                        //Traer los registros de wks que se hayan podido haber generado pero ya deberían estar vacios.
                        List<WorkstatusSpool> wksSpool = ctx.WorkstatusSpool.Where(x => odtSpoolIds.Contains(x.OrdenTrabajoSpoolID)).ToList();

                        //Traer los spools de la ODT
                        List<OrdenTrabajoSpool> spools = ctx.OrdenTrabajoSpool
                                                            .Where(x => x.OrdenTrabajoID == ordenTrabajoID)
                                                            .ToList();

                        //Las juntas de todos los spools de la ODT
                        List<OrdenTrabajoJunta> juntas = ctx.OrdenTrabajoJunta
                                                            .Where(x => odtSpoolIds.Contains(x.OrdenTrabajoSpoolID))
                                                            .ToList();

                        //Materiales involucrados
                        IQueryable<OrdenTrabajoMaterial> queryMateriales = ctx.OrdenTrabajoMaterial
                                                                                .Where(x => odtSpoolIds.Contains(x.OrdenTrabajoSpoolID));

                        //Los materiales de todos los spools de la ODT
                        List<OrdenTrabajoMaterial> materiales = queryMateriales.ToList();

                        //Los despachos relacionados con algún material de la ODT que hayan sido cancelados
                        List<Despacho> dp = ctx.Despacho
                                                .Where(x => odtSpoolIds.Contains(x.OrdenTrabajoSpoolID) && x.Cancelado)
                                                .ToList();

                        //Los cortes relacionados con algún material de la ODT que hayan sido cancelados
                        List<CorteDetalle> cd = ctx.CorteDetalle
                                                    .Where(x => odtSpoolIds.Contains(x.OrdenTrabajoSpoolID) && x.Cancelado)
                                                    .ToList();

                        //Las transferencias a corte que hayan sido canceladas
                        IQueryable<NumeroUnicoCorte> nuc = ctx.NumeroUnicoCorte.Where(x => x.OrdenTrabajoID == ordenTrabajoID && x.NumeroUnicoMovimiento.Estatus == EstatusNumeroUnicoMovimiento.CANCELADO);
                        List<NumeroUnicoCorte> nucList = nuc.ToList();

                        //Los cortes relacionados con las transferencias a corte a eliminar
                        List<Corte> corte = ctx.Corte.Where(x => nuc.Select(y => y.NumeroUnicoCorteID).Contains(x.NumeroUnicoCorteID)).ToList();

                        //Traer los inventarios para quitar lo congelado
                        List<NumeroUnicoInventario> lstNui = ctx.NumeroUnicoInventario
                                                                .Where(x => queryMateriales.Select(y => y.NumeroUnicoCongeladoID).Contains(x.NumeroUnicoID))
                                                                .ToList();

                        //Traer los segmentos para quitar los congelados
                        List<NumeroUnicoSegmento> lstNus = ctx.NumeroUnicoSegmento
                                                                .Where(x => queryMateriales.Select(y => y.NumeroUnicoCongeladoID).Contains(x.NumeroUnicoID))
                                                                .ToList();

                        #endregion

                        #region marcar como borrados los objetos relacionados

                        NumeroUnicoInventario nui;
                        NumeroUnicoSegmento nus;

                        juntaWks.ForEach(ctx.DeleteObject);
                        juntas.ForEach(ctx.DeleteObject);

                        #region Quitar inventarios congelados de la ODT

                        materiales.ForEach(x =>
                        {
                            if (x.CantidadCongelada.HasValue)
                            {
                                nui = lstNui.Where(y => y.NumeroUnicoID == x.NumeroUnicoCongeladoID).SingleOrDefault();

                                if (nui != null)
                                {
                                    if (!nui.ChangeTracker.ChangeTrackingEnabled)
                                    {
                                        nui.StartTracking();
                                    }

                                    nui.InventarioDisponibleCruce += x.CantidadCongelada.Value;
                                    nui.InventarioCongelado -= x.CantidadCongelada.Value;
                                    nui.UsuarioModifica = userID;
                                    nui.FechaModificacion = DateTime.Now;
                                }

                                nus = lstNus.Where(y => y.NumeroUnicoID == x.NumeroUnicoCongeladoID && y.Segmento == x.SegmentoCongelado).SingleOrDefault();

                                if (nus != null)
                                {
                                    if (!nus.ChangeTracker.ChangeTrackingEnabled)
                                    {
                                        nus.StartTracking();
                                    }

                                    nus.InventarioDisponibleCruce += x.CantidadCongelada.Value;
                                    nus.InventarioCongelado -= x.CantidadCongelada.Value;
                                    nus.UsuarioModifica = userID;
                                    nus.FechaModificacion = DateTime.Now;
                                }
                            }

                            ctx.DeleteObject(x);
                        });

                        #endregion

                        wksSpool.ForEach(ctx.DeleteObject);
                        cd.ForEach(ctx.DeleteObject);
                        corte.ForEach(ctx.DeleteObject);
                        dp.ForEach(ctx.DeleteObject);
                        nucList.ForEach(ctx.DeleteObject);
                        
                        spools.ForEach(ctx.DeleteObject);
                        ctx.DeleteObject(odt);

                        #endregion

                        lstNui.ForEach(ctx.NumeroUnicoInventario.ApplyChanges);
                        lstNus.ForEach(ctx.NumeroUnicoSegmento.ApplyChanges);

                        //borrar odt y relaciones
                        ctx.SaveChanges();
                    }

                    scope.Complete();
                }

            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// Obtiene la OrdenTrabajo en base a un Numero de Control
        /// </summary>
        /// <param name="noControl">Numero de Control de Orden Trabajo Spool</param>
        /// <returns></returns>
        public OrdenTrabajo ObtenerPorNumeroDeControl(string noControl)
        {
            using (SamContext ctx = new SamContext())
            {
                //No debieran de repetirse los numeros de control en diferentes ordenes de trabajo 
                return ctx.OrdenTrabajoSpool.Where(x => x.NumeroControl == noControl).Select(y => y.OrdenTrabajo).FirstOrDefault();
            }
        }

        /// <summary>
        /// Obtiene una lista de objetos personalizados que contienen información sobre los spools
        /// de una orden de trabajo.
        /// </summary>
        /// <param name="ordenTrabajoID">ID de la orden de trabajo de la cual se desea obtener los spools</param>
        /// <returns>Lista de objetos con información sobre los spools de una ODT</returns>
        public List<GrdSpoolOdt> ObtenerSpoolsPorOdt(int ordenTrabajoID)
        {
            List<GrdSpoolOdt> lstSpools = null;

            //shop fab area
            int fabAreaID = CacheCatalogos.Instance.ShopFabAreaID;

            using (SamContext ctx = new SamContext())
            {
                //Query que obtiene solo las órdenes de trabajo spool que nos interesan
                IQueryable<OrdenTrabajoSpool> ots = ctx.OrdenTrabajoSpool
                                                       .Where(x => x.OrdenTrabajoID == ordenTrabajoID);

                //Cuantas juntas y materiales hay en cada orden de trabajo spool
                var grupo = (from o in ots
                             select new
                             {
                                 OrdenTrabajoID = o.OrdenTrabajoID,
                                 OrdenTrabajoSpoolID = o.OrdenTrabajoSpoolID,
                                 SpoolID = o.SpoolID,
                                 CantidadMateriales = o.OrdenTrabajoMaterial.Count(),
                                 CantidadJuntas = o.OrdenTrabajoJunta.Count(),
                                 CantidadMaterialesConReingenieria = o.OrdenTrabajoMaterial.Count(x => x.FueReingenieria),
                                 CantidadJuntasConReingenieria = o.OrdenTrabajoJunta.Count(x => x.FueReingenieria),
                                 MaterialesDespachados = o.OrdenTrabajoMaterial.Count(y => y.TieneDespacho)
                             }).ToList();


                //Cuales son los spools involucrados en las ODts que nos estamos trayendo
                IQueryable<int> spools = ots.Select(y => y.SpoolID);

                //Cuantas juntas y materiales hay en cada spool (desde ingeniería)
                var grupoIng = (from s in ctx.Spool
                                where spools.Contains(s.SpoolID)
                                select new
                                {
                                    SpoolID = s.SpoolID,
                                    CantidadJuntas = s.JuntaSpool.Count(x => x.FabAreaID == fabAreaID),
                                    CantidadMateriales = s.MaterialSpool.Count()
                                }).ToList();


                //Spool x spool nos dice el estatus de su despacho y si difiere entre ingeniería y producción
                var joinGrupos = (from grpO in grupo
                                  join grpIng in grupoIng on grpO.SpoolID equals grpIng.SpoolID
                                  select new
                                  {
                                      OrdenTrabajoSpoolID = grpO.OrdenTrabajoSpoolID,
                                      SpoolID = grpO.SpoolID,
                                      CantidadJuntasConReingenieria = grpO.CantidadJuntasConReingenieria,
                                      CantidadMaterialesConReingenieria = grpO.CantidadMaterialesConReingenieria,
                                      CantidadMaterialesEnOdt = grpO.CantidadMateriales,
                                      CantidadJuntasEnOdt = grpO.CantidadJuntas,
                                      MaterialesDespachados = grpO.MaterialesDespachados,
                                      CantidadJuntasIngenieria = grpIng.CantidadJuntas,
                                      CantidadMaterialesIngenieria = grpIng.CantidadMateriales,
                                      EstatusDespachoSpool = grpO.MaterialesDespachados == grpO.CantidadMateriales
                                                             ? EstatusDespachoOdt.Despachada
                                                             : (grpO.MaterialesDespachados == 0 ? EstatusDespachoOdt.SinDespacho : EstatusDespachoOdt.Parcial),
                                      Difieren = grpO.CantidadMateriales != grpIng.CantidadMateriales || grpIng.CantidadJuntas != grpO.CantidadJuntas
                                  }).ToList();

                lstSpools = (from o in ots
                             let spool = o.Spool
                             select new GrdSpoolOdt
                             {
                                 NombreSpool = spool.Nombre,
                                 SpoolID = o.SpoolID,
                                 NumeroControl = o.NumeroControl,
                                 OrdenTrabajoSpoolID = o.OrdenTrabajoSpoolID,
                                 Partida = o.Partida,
                                 Pdis = spool.Pdis ?? 0
                             }).ToList();

                #region Calcular la propiedades faltantes del objeto que vamos a regresar


                lstSpools.ForEach(o =>
                {
                    //tipo anónimo necesario
                    var filtro = joinGrupos.Where(x => x.OrdenTrabajoSpoolID == o.OrdenTrabajoSpoolID).Single();
                    
                    o.DifiereDeIngenieria = filtro.Difieren;
                    o.FueReingenieria = filtro.CantidadJuntasConReingenieria > 0 || filtro.CantidadMaterialesConReingenieria > 0;
                    o.EstatusDespacho = filtro.EstatusDespachoSpool;
                    o.EstatusDespachoTexto = TraductorEnumeraciones.TextoEstatusDespacho(o.EstatusDespacho);
                });

                #endregion            
            }

            return lstSpools;
        }


        /// <summary>
        /// Quita un spool de la ODT.  Existen una serie de reglas de negocio que son 
        /// validadas previamente a quitar un spool de la ODT, dichas reglas son:
        /// + Ese spool en particular no debe tener ningún material asociado a un corte vigente.
        /// + Ese spool en particular no debe tener ningún material asociado a un despacho vigente.
        /// 
        /// Al eliminar un spool de la ODT lo siguiente sucede:
        /// + Se elimina el registro de la tabla OrdenTrabajoSpool
        /// + Se eliminan los registros relacionados de la tabla OrdenTrabajoMaterial
        /// + Se eliminan los registros relacionados de la tabla OrdenTrabajoJunta
        /// + Se eliminan los registro de la tabla CorteDetalle que estén cancelados y relacionados
        ///   a alguno de los materiales que se van a eliminar.
        /// + Se eliminan los registros de la tabla Despacho que estén cancelados y relacionados
        ///   a alguno de los materiales que se van a eliminar.
        /// + Descongelar los materiales congelados por ese spool (si aún siguen congelados)
        /// </summary>
        /// <param name="ordenTrabajoSpoolID">ID del registro OrdenTrabajoSpool que se desea elminar</param>
        public void EliminaSpool(int ordenTrabajoSpoolID, Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                List<string> errores = new List<string>();

                OrdenTrabajoSpool odts = ctx.OrdenTrabajoSpool
                                            .Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID)
                                            .Single();

                #region validar cortes y despachos para saber si podemos borrar el spool de la ODT

                if (!ValidacionesOdt.ValidaOdtSpoolNoTengaCortes(ctx, ordenTrabajoSpoolID))
                {
                    errores.Add(MensajesError.Excepcion_NumControlTieneCortes);
                }

                if (!ValidacionesOdt.ValidaOdtSpoolNoTengaDespachos(ctx, ordenTrabajoSpoolID))
                {
                    errores.Add(MensajesError.Excepcion_NumControlTieneDespachos);
                }

                if (!ValidacionesOdt.ValidaOdtSpoolNoTengaRequisicionesPintura(ctx, ordenTrabajoSpoolID))
                {
                    errores.Add(MensajesError.Excepcion_NumControlTieneRequisicionesPintura);
                }

                if (!ValidacionesOdt.ValidaOdtSpoolNoTengaEmbarque(ctx, ordenTrabajoSpoolID))
                {
                    errores.Add(MensajesError.Excepcion_NumControlTieneEmbarque);
                }

                if (errores.Count > 0)
                {
                    throw new ExcepcionOdt(errores);
                }

                #endregion

                #region Traer al contexto las entidades relacionadas que necesitamos borrar

                //Workstatus spool en caso de que sea necesario
                WorkstatusSpool wks = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID).SingleOrDefault();
                if(wks != null)
                    ctx.DeleteObject(wks);

                //Las juntas del spool
                List<OrdenTrabajoJunta> juntas = ctx.OrdenTrabajoJunta
                                                    .Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID)
                                                    .ToList();

                IQueryable<OrdenTrabajoMaterial> queryMateriales = ctx.OrdenTrabajoMaterial
                                                                      .Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID);

                List<JuntaWorkstatus> juntasWks = ctx.JuntaWorkstatus
                                                    .Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID)
                                                    .ToList();

                //Los materiales del spool
                List<OrdenTrabajoMaterial> materiales = queryMateriales.ToList();

                //Los despachos relacionados con algún material de la ODT que hayan sido cancelados
                List<Despacho> dp = ctx.Despacho
                                       .Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID && x.Cancelado)
                                       .ToList();

                //Los cortes relacionados con algún material de la ODT que hayan sido cancelados
                List<CorteDetalle> cd = ctx.CorteDetalle
                                           .Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID && x.Cancelado)
                                           .ToList();

                //Traer los inventarios para quitar lo congelado
                List<NumeroUnicoInventario> lstNui = ctx.NumeroUnicoInventario
                                                        .Where(x => queryMateriales.Select(y => y.NumeroUnicoCongeladoID).Contains(x.NumeroUnicoID))
                                                        .ToList();

                //Traer los segmentos para quitar los congelados
                List<NumeroUnicoSegmento> lstNus = ctx.NumeroUnicoSegmento
                                                      .Where(x => queryMateriales.Select(y => y.NumeroUnicoCongeladoID).Contains(x.NumeroUnicoID))
                                                      .ToList();

                #endregion
                NumeroUnicoInventario nui;
                NumeroUnicoSegmento nus;

                juntas.ForEach(ctx.DeleteObject);
                #region A los materiales hay que quitarles lo congelado

                materiales.ForEach(x =>
                {
                    if (x.CantidadCongelada.HasValue)
                    {
                        nui = lstNui.Where(y => y.NumeroUnicoID == x.NumeroUnicoCongeladoID).SingleOrDefault();

                        if (nui != null)
                        {
                            if (!nui.ChangeTracker.ChangeTrackingEnabled)
                            {
                                nui.StartTracking();
                            }

                            nui.InventarioDisponibleCruce += x.CantidadCongelada.Value;
                            nui.InventarioCongelado -= x.CantidadCongelada.Value;
                            nui.UsuarioModifica = userID;
                            nui.FechaModificacion = DateTime.Now;
                        }

                        nus = lstNus.Where(y => y.NumeroUnicoID == x.NumeroUnicoCongeladoID && y.Segmento == x.SegmentoCongelado).SingleOrDefault();

                        if (nus != null)
                        {
                            if (!nus.ChangeTracker.ChangeTrackingEnabled)
                            {
                                nus.StartTracking();
                            }

                            nus.InventarioDisponibleCruce += x.CantidadCongelada.Value;
                            nus.InventarioCongelado -= x.CantidadCongelada.Value;
                            nus.UsuarioModifica = userID;
                            nus.FechaModificacion = DateTime.Now;
                        }
                    }

                    ctx.DeleteObject(x);
                });

                #endregion
                cd.ForEach(ctx.DeleteObject);
                dp.ForEach(ctx.DeleteObject);
                juntasWks.ForEach(ctx.DeleteObject);
                ctx.DeleteObject(odts);


                //Cambios a inventario hay que ponerlos en el contexto
                lstNui.ForEach(ctx.NumeroUnicoInventario.ApplyChanges);
                lstNus.ForEach(ctx.NumeroUnicoSegmento.ApplyChanges);

                //Quitamos etiqueta embarque
                Spool spool = ctx.Spool.Where(x => x.SpoolID == odts.SpoolID).SingleOrDefault();
                spool.FechaEtiqueta = null;
                spool.NumeroEtiqueta = null;
                spool.UsuarioModifica = userID;
                spool.FechaModificacion = DateTime.Now;
                ctx.Spool.ApplyChanges(spool);

                //Guardar todos los cambios
                ctx.SaveChanges();
            }
        }


        /// <summary>
        /// Obtiene todo el detalle sobre un spool que ya se encuentra en una orden de trabajo:
        /// + Juntas
        /// + Materiales
        /// + Cortes
        /// + Holds
        /// + Saber si el material/junta existe en la ODT o sólo en ingeniería.
        /// </summary>
        /// <param name="ordenTrabajoSpoolID">ID del registro OrdenTrabajoSpool que se desea ver</param>
        /// <returns>Detalle del spool en una entidad de tipo DetSpoolOdt</returns>
        public DetSpoolOdt ObtenerDetalleDeSpool(int ordenTrabajoSpoolID)
        {
            OrdenTrabajoSpool ots;
            //shop fab area
            int fabAreaID = CacheCatalogos.Instance.ShopFabAreaID;

            using (SamContext ctx = new SamContext())
            {
                IQueryable<OrdenTrabajoSpool> iqOts = ctx.OrdenTrabajoSpool.Include("Spool.JuntaSpool.BastonSpoolJunta.BastonSpool").Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID);

                ots = iqOts.Single();
                ctx.LoadProperty<OrdenTrabajoSpool>(ots, x => x.OrdenTrabajoMaterial);
                ctx.LoadProperty<OrdenTrabajoSpool>(ots, x => x.OrdenTrabajoJunta);
                ctx.LoadProperty<OrdenTrabajoSpool>(ots, x => x.Spool);
                ctx.LoadProperty<Spool>(ots.Spool, x => x.JuntaSpool);
                ctx.LoadProperty<Spool>(ots.Spool, x => x.MaterialSpool);
                ctx.LoadProperty<Spool>(ots.Spool, x => x.CorteSpool);
                ctx.LoadProperty<Spool>(ots.Spool, x => x.SpoolHold);
                ctx.LoadProperty<Spool>(ots.Spool, x => x.SpoolHoldHistorial);

                //Preparar para subquery de item codes
                IQueryable<int> icMateriales = ctx.MaterialSpool
                                                  .Where(x => iqOts.Select(y=>y.SpoolID).Contains(x.SpoolID))
                                                  .Select(y => y.ItemCodeID);

                IQueryable<int> icCortes = ctx.CorteSpool
                                              .Where(x => iqOts.Select(y => y.SpoolID).Contains(x.SpoolID))
                                              .Select(y => y.ItemCodeID);

                //Traer los item codes al contexto, esto va a hacer que el grafo del spool se
                //complete con los item codes que necesita
                ctx.ItemCode.Where(x => icMateriales.Contains(x.ItemCodeID) || icCortes.Contains(x.ItemCodeID)).ToList();
            }

            CacheCatalogos instance = CacheCatalogos.Instance;

            Dictionary<int, string> tiposJunta = instance.ObtenerTiposJunta().ToDictionary(x=> x.ID, y => y.Nombre);
            Dictionary<int, string> tiposCorte = instance.ObtenerTiposCorte().ToDictionary(x => x.ID, y => y.Nombre);
            Dictionary<int, string> fabAreas = instance.ObtenerFabAreas().ToDictionary(x => x.ID, y => y.Nombre);
            Dictionary<int, string> familiasAcero = instance.ObtenerFamiliasAcero().ToDictionary(x => x.ID, y => y.Nombre);

            return new DetSpoolOdt(ots, tiposJunta, tiposCorte, fabAreas, familiasAcero, fabAreaID);
        }

        /// <summary>
        /// Guarda una orden de trabajo y todos los objetos que traiga colgados en su grafo que
        /// hayan sido modificados y/o agregados.
        /// </summary>
        /// <param name="odt">Entidad de la orden de trabajo que se desea guardar</param>
        public void Guarda(OrdenTrabajo odt)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ctx.OrdenTrabajo.ApplyChanges(odt);
                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }


        /// <summary>
        /// Obtiene el siguiente número de partida disponible para una orden de trabajo en particular.
        /// </summary>
        /// <param name="ordenTrabajoID">ID de la orden de trabajo</param>
        /// <returns>Entero que contiene la siguiente partida disponible para una ODT en particular</returns>
        public int SiguientePartida(int ordenTrabajoID)
        {
            using (SamContext ctx = new SamContext())
            {
                int ? maxPartida = ctx.OrdenTrabajoSpool
                                      .Where(x => x.OrdenTrabajoID == ordenTrabajoID)
                                      .Max(x => (int?)x.Partida);

                if (!maxPartida.HasValue)
                {
                    maxPartida = 0;
                }

                return maxPartida.Value + 1;
            }
        }

        public IEnumerable<Simple> ObtenerOdtsPorPermiso(int? proyectoID, int skip, int take, string numeroOrdenFiltro, bool esAdministradorSistema, Guid userID)
        {
            List<Simple> lst;

            using (SamContext ctx = new SamContext())
            {
                ctx.OrdenTrabajo.MergeOption = MergeOption.NoTracking;
                IQueryable<OrdenTrabajo> iOdt = ctx.OrdenTrabajo.AsQueryable();

                if (proyectoID.HasValue && proyectoID.Value > 0)
                {
                    //aquí asumimos que tiene permisos
                    iOdt = iOdt.Where(x => x.ProyectoID == proyectoID);
                }
                else if (!esAdministradorSistema)
                {
                    //aqui traemos unicamente por permisos
                    iOdt = iOdt.Where(x => ctx.UsuarioProyecto
                                              .Where(up => up.UserId == userID)
                                              .Select(up => up.ProyectoID)
                                              .Contains(x.ProyectoID));
                }

                lst =
                    iOdt.Select(x => new Simple { ID = x.OrdenTrabajoID, Valor = x.NumeroOrden })
                        .ToList();
            }

            return lst.Where(x => x.Valor.StartsWith(numeroOrdenFiltro, StringComparison.InvariantCultureIgnoreCase))
                      .OrderBy(x => x.Valor)
                      .Skip(skip)
                      .Take(take);
        }

        public IEnumerable<Simple> ObtenerOdtsPorProyecto(int? proyectoID, int skip, int take, string numeroOrdenFiltro, Guid userID)
        {
            List<Simple> lst;

            using (SamContext ctx = new SamContext())
            {
                ctx.OrdenTrabajo.MergeOption = MergeOption.NoTracking;
                IQueryable<OrdenTrabajo> iOdt = ctx.OrdenTrabajo.AsQueryable();

                if (proyectoID.HasValue && proyectoID.Value > 0)
                {                    
                    iOdt = iOdt.Where(x => x.ProyectoID == proyectoID);

                    lst =
                    iOdt.Select(x => new Simple { ID = x.OrdenTrabajoID, Valor = x.NumeroOrden })
                        .ToList();

                    return lst.Where(x => x.Valor.StartsWith(numeroOrdenFiltro, StringComparison.InvariantCultureIgnoreCase))
                      .OrderBy(x => x.Valor)
                      .Skip(skip)
                      .Take(take);
                }
                else
                {
                    return null;
                }
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordenTrabajoID"></param>
        /// <returns></returns>
        public int ObtenerProyectoID(int ordenTrabajoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ProyectoOrdenTrabajo(ctx, ordenTrabajoID);
            }
        }

        /// <summary>
        /// Versión compilada del query para permisos de ODT
        /// </summary>
        private static readonly Func<SamContext, int, int> ProyectoOrdenTrabajo =
        CompiledQuery.Compile<SamContext, int, int>
        (
            (ctx, id) => ctx.OrdenTrabajo
                            .Where(x => x.OrdenTrabajoID == id)
                            .Select(x => x.ProyectoID)
                            .Single()
        );
    }
}
