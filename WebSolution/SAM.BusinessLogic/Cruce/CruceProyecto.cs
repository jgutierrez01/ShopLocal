using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Diagnostics;
using System.Linq;
using System.Text;
using log4net;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessLogic.Produccion;
using SAM.Entities;
using SAM.Entities.Personalizadas;
using SAM.Entities.Reportes;

namespace SAM.BusinessLogic.Cruce
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class CruceProyecto
    {
        private int _proyectoID;
        private string _nombreProyecto;
        private List<string> _lstIsoCompletos;
        private List<Spool> _lstSpoolSinOdt;
        private List<MaterialSpool> _lstMaterialSpool;
        private List<NumeroUnico> _lstNumeroUnico, _lstNumerosUnicosCongelados, _lstNumeroUnicoRechazados, _lstNumeroUnicoCondicionados;
        private List<ItemCode> _lstItemCode;
        private List<CongeladoParcial> _lstCongeladoParcial;
        private List<ItemCodeEquivalente> _lstEquivalente;
        private List<ResumenIsometrico> _lstResumenIso;
        private List<CruceItemCode> _lstCondensadoIC;
        private Dictionary<int, ItemCode> _dicItemCodes;
        private Dictionary<int, Spool> _dicSpools;
        private Dictionary<ItemCodeIntegrado, CruceItemCode> _dicCodensados;
        private Dictionary<int, string> _familiasAcero;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(CruceProyecto));
        Stopwatch sw = new Stopwatch();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proyectoID"></param>
        public CruceProyecto(int proyectoID)
        {
            _proyectoID = proyectoID;
            _nombreProyecto = CacheCatalogos.Instance.ObtenerProyectos().Where(x => x.ID == _proyectoID).Single().Nombre;
            _familiasAcero = CacheCatalogos.Instance.ObtenerFamiliasAcero().ToDictionary(x => x.ID, y => y.Nombre);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="spoolHold"></param>
        /// <param name="sHoldHistorial"></param>
        /// <returns></returns>
        private bool EstaEnHold(SpoolHold spoolHold, SpoolHoldHistorial sHoldHistorial)
        {
            bool enHold = false;

            switch (sHoldHistorial.TipoHold)
            {
                case TipoHoldSpool.INGENIERIA:
                    enHold = spoolHold.TieneHoldIngenieria ? true : false;
                    break;
                case TipoHoldSpool.CALIDAD:
                    enHold = spoolHold.TieneHoldCalidad ? true : false;
                    break;
                case TipoHoldSpool.CONFINADO:
                    enHold = spoolHold.Confinado ? true : false;
                    break;
                default:
                    break;
            }

            return enHold;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="faltantes"></param>
        /// <param name="condensado"></param>
        /// <param name="spoolHoldIncluidos"></param>
        /// <returns></returns>
        public List<SpoolCruce> Procesa(out List<FaltanteCruce> faltantes, out List<CondensadoItemCode> condensado, bool spoolHoldIncluidos, bool crucePorIsometrico)
        {

            sw.Start();
            _logger.DebugFormat(String.Format("Iniciando cruce proyecto, solo Spools en revisión: {0}", _nombreProyecto));

            using (SamContext ctx = new SamContext())
            {
                ctx.Spool.MergeOption = MergeOption.NoTracking;
                ctx.MaterialSpool.MergeOption = MergeOption.NoTracking;
                ctx.NumeroUnico.MergeOption = MergeOption.NoTracking;
                ctx.NumeroUnicoSegmento.MergeOption = MergeOption.NoTracking;
                ctx.NumeroUnicoInventario.MergeOption = MergeOption.NoTracking;
                ctx.ItemCode.MergeOption = MergeOption.NoTracking;
                ctx.ItemCodeEquivalente.MergeOption = MergeOption.NoTracking;
                ctx.Colada.MergeOption = MergeOption.NoTracking;
                ctx.SpoolHold.MergeOption = MergeOption.NoTracking;


                // lista completa de materiales spool para posteriormente hacer un calculo de requerimiento total ingeniería
                _lstMaterialSpool = ctx.Spool.Where(x => x.ProyectoID == _proyectoID).SelectMany(x => x.MaterialSpool).ToList();


                //spools que tienen alguna especie de hold para el proyecto
                List<int> spoolsHold = new List<int>();
                if (!spoolHoldIncluidos)
                {
                    spoolsHold = (from sph in ctx.SpoolHold
                                  where sph.Spool.ProyectoID == _proyectoID &&
                                        (sph.TieneHoldCalidad || sph.TieneHoldIngenieria || sph.Confinado)
                                  select sph.SpoolID).ToList();
                }


                //Traer los spools que no tienen ODT de un proyecto, incluyendo sus materiales
                //que finalmente es en base a lo cual vamos a cruzar (de BD).
                //Posteriormente filtrar un poco más pero sólo aquellos que ya fueron aprobados para cruce
                //y que no tengan prioridad o la prioridad sea mayor a cero
                sw.Restart();
                _logger.DebugFormat("SpoolsSinOdtSinTracking");

                _lstSpoolSinOdt = (from spool in UtileriasCruce.SpoolsSinOdtSinTracking(ctx, _proyectoID)
                                                               .ToList()
                                                               .AsParallel()
                                   where !spoolsHold.Contains(spool.SpoolID)
                                   select spool).ToList();

                _logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);

                //Materiales congelados parcialmente
                sw.Restart();
                _logger.DebugFormat("_lstCongeladoParcial");
                _lstCongeladoParcial = (from cong in UtileriasCruce.CongParcialPorMat(ctx, _proyectoID)
                                                                   .ToList()
                                                                   .AsParallel()
                                        select cong).ToList();
                _logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);

                //Traer los números únicos del proyecto que tengan inventario disponible para cruce
                //mayor a cero de BD.
                //Filtrar en memoria sólo los aprobados y cuyas coladas no estén en hold.
                sw.Restart();
                _logger.DebugFormat("_lstNumeroUnico");
                _lstNumeroUnico = (from nu in UtileriasCruce.NumerosUnicoParaCruceSinTracking(ctx, _proyectoID)
                                                            .ToList()
                                                            .AsParallel()
                                    where  nu.Colada.HoldCalidad == false
                                         && nu.Estatus != null
                                         && nu.Estatus.Equals(EstatusNumeroUnico.APROBADO)
                                   select nu).ToList();
                _logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);
                //Traer los números únicos del proyecto que hayan sido rechazados para mostrar cantidades
                //en excel de faltantes
                sw.Restart();
                _logger.DebugFormat("_lstNumeroUnicoRechazados");
                _lstNumeroUnicoRechazados = (from nu in UtileriasCruce.NumerosUnicoParaCruceSinTrackingRechazado(ctx, _proyectoID)
                                                            .ToList()
                                                            .AsParallel()
                                             select nu).ToList();
                _logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);
                //Traer los números únicos del proyecto que hayan sido condicionados para mostrar cantidades
                //en excel de faltantes
                sw.Restart();
                _logger.DebugFormat("_lstNumeroUnicoCondicionados");
                _lstNumeroUnicoCondicionados = (from nu in UtileriasCruce.NumerosUnicoParaCruceSinTrackingCondicionado(ctx, _proyectoID)
                                                            .ToList()
                                                            .AsParallel()
                                                select nu).ToList();
                _logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);
                //Traer los nus congelados parciales ya que podrían no estar en la primera lista
                sw.Restart();
                _logger.DebugFormat("_lstNumerosUnicosCongelados");
                _lstNumerosUnicosCongelados = (from nu in UtileriasCruce.NumerosUnicosCongeladosParcialmente(ctx, _proyectoID)
                                                                        .ToList()
                                                                        .AsParallel()
                                               where !_lstNumeroUnico.Select(x => x.NumeroUnicoID).Contains(nu.NumeroUnicoID)
                                               select nu).ToList();
                _logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);
                //Traer de BD todos los I.C. del proyecto
                _lstItemCode = UtileriasCruce.ItemCodesPorProyecto(ctx, _proyectoID).ToList();

                //Traer de BD todos los I.C. equivalentes para el proyecto seleccionado
                _lstEquivalente = UtileriasCruce.ItemCodeEquivalentesPorProyecto(ctx, _proyectoID).ToList();

                //Traer el condensando que me dice cuantos spools tiene un isométrico
                _lstResumenIso = UtileriasCruce.AvanceIsometrico(ctx, _proyectoID).ToList();

                //Lista con un condensado de la disponibilidad de números únicos agrupados por item code
                _lstCondensadoIC = UtileriasCruce.InventariosCondensadosPorIC(ctx, _proyectoID).ToList();
            }

            _logger.DebugFormat("Tiempo Query= {0} ms", sw.ElapsedMilliseconds);

            _logger.Info("Comienzo ejecucion");
            //Verifica si es por isometrico o no
            if (!crucePorIsometrico)
            {
                sw.Restart();
                _logger.DebugFormat("variables temporales");
                estableceVariablesTemporales();
                _logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);

                sw.Restart();
                _logger.DebugFormat("ordenamos por prioridad");
                ordenaSpoolsPorPrioridad();
                _logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);

                sw.Restart();
                _logger.DebugFormat("generamos diccionario");
                generaDiccionarios();
                _logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);

                sw.Restart();
                _logger.DebugFormat("recorremos spools para congelar material");
                recorreSpoolsYCongela();
                _logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);

                sw.Restart();
                _logger.DebugFormat("generamos lista de faltantes");
                faltantes = generaListaFaltantes();
                _logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);

                sw.Restart();
                _logger.DebugFormat("generamos condensado IC");
                condensado = generaCondensadoItemCodes(faltantes);
                _logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);

                return generaListaSpoolsFabricables();
            }
            else
            {
                estableceVariablesTemporales();
                ordenaSpoolsPorIsometrico();
                generaDiccionarios();
                recorreIsometricosYCongela();
                faltantes = generaListaFaltantesPorIso();
                condensado = generaCondensadoItemCodes(faltantes);
                return generaListaSpoolsFabricables();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="faltantes"></param>
        /// <param name="condensado"></param>
        /// <param name="spoolHoldIncluidos"></param>
        /// <param name="crucePorIsometrico"></param>
        /// <returns></returns>
        public List<SpoolCruce> ProcesaSpoolRevision(out List<FaltanteCruce> faltantes, out List<CondensadoItemCode> condensado)
        {

            //sw.Start();
            //_logger.DebugFormat(String.Format("Iniciando cruce revision proyecto: {0}", _nombreProyecto));

            //using (SamContext ctx = new SamContext())
            //{
            //    ctx.Spool.MergeOption = MergeOption.NoTracking;
            //    ctx.MaterialSpool.MergeOption = MergeOption.NoTracking;
            //    ctx.NumeroUnico.MergeOption = MergeOption.NoTracking;
            //    ctx.NumeroUnicoSegmento.MergeOption = MergeOption.NoTracking;
            //    ctx.NumeroUnicoInventario.MergeOption = MergeOption.NoTracking;
            //    ctx.ItemCode.MergeOption = MergeOption.NoTracking;
            //    ctx.ItemCodeEquivalente.MergeOption = MergeOption.NoTracking;
            //    ctx.Colada.MergeOption = MergeOption.NoTracking;
            //    ctx.SpoolHold.MergeOption = MergeOption.NoTracking;


            //    // lista completa de materiales spool para posteriormente hacer un calculo de requerimiento total ingeniería
            //    //Solo spools en revision
            //    _lstMaterialSpool = ctx.Spool.Where(x => x.ProyectoID == _proyectoID
            //        && x.EsRevision == true && (x.UltimaOrdenTrabajoEspecial == null || x.UltimaOrdenTrabajoEspecial == "")).SelectMany(x => x.MaterialSpool).ToList();


            //    //spools que tienen alguna especie de hold para el proyecto
            //    List<int> spoolsHold = new List<int>();
                
            //    spoolsHold = (from sph in ctx.SpoolHold
            //                  where sph.Spool.ProyectoID == _proyectoID &&
            //                        (sph.TieneHoldCalidad || sph.TieneHoldIngenieria || sph.Confinado)
            //                        && sph.Spool.EsRevision == true && (sph.Spool.UltimaOrdenTrabajoEspecial == null || sph.Spool.UltimaOrdenTrabajoEspecial == "")
            //                  select sph.SpoolID).ToList();
                


            //    //Traer de la base de datos los Spool Marcados como Revision para realizar este cruce
            //    //en base a los cuales se hara el cruce, puesto que todos tienen una ODT
            //    _lstSpoolSinOdt = (from spool in UtileriasCruce.SpoolMarcadosComoRevision(ctx, _proyectoID)
            //                                                    .ToList()
            //                                                    .AsParallel()
            //                       where !spoolsHold.Contains(spool.SpoolID)
            //                       select spool).ToList();


            //    //Traer los números únicos del proyecto que tengan inventario disponible para cruce
            //    //mayor a cero de BD.
            //    //Filtrar en memoria sólo los aprobados y cuyas coladas no estén en hold.
            //    //-------- AUN NO SE SI DEBO MODIFICAR ESTE METODO LO DEJO PENDIENTE ---- JHT
            //    _lstNumeroUnico = (from nu in UtileriasCruce.NumerosUnicoParaCruceSinTracking(ctx, _proyectoID)
            //                                                .ToList()
            //                                                .AsParallel()
            //                       where nu.Colada.HoldCalidad == false
            //                             && nu.Estatus != null
            //                             && nu.Estatus.Equals(EstatusNumeroUnico.APROBADO)
            //                       select nu).ToList();

            //    //Traer los números únicos del proyecto que hayan sido rechazados para mostrar cantidades
            //    //en excel de faltantes
            //    _lstNumeroUnicoRechazados = (from nu in UtileriasCruce.NumerosUnicoParaCruceSinTracking(ctx, _proyectoID)
            //                                                .ToList()
            //                                                .AsParallel()
            //                                 where nu.Estatus != null
            //                                       && nu.Estatus.Equals(EstatusNumeroUnico.RECHAZADO)
            //                                 select nu).ToList();

            //    //Traer los números únicos del proyecto que hayan sido condicionados para mostrar cantidades
            //    //en excel de faltantes
            //    _lstNumeroUnicoCondicionados = (from nu in UtileriasCruce.NumerosUnicoParaCruceSinTracking(ctx, _proyectoID)
            //                                                .ToList()
            //                                                .AsParallel()
            //                                    where nu.Estatus != null
            //                                          && nu.Estatus.Equals(EstatusNumeroUnico.CONDICIONADO)
            //                                    select nu).ToList();

            //    //Traer los nus congelados parciales ya que podrían no estar en la primera lista
            //    _lstNumerosUnicosCongelados = (from nu in UtileriasCruce.NumerosUnicosCongeladosParcialmente(ctx, _proyectoID)
            //                                                            .ToList()
            //                                                            .AsParallel()
            //                                   where !_lstNumeroUnico.Select(x => x.NumeroUnicoID).Contains(nu.NumeroUnicoID)
            //                                   select nu).ToList();

            //    //Traer de BD todos los I.C. del proyecto
            //    _lstItemCode = UtileriasCruce.ItemCodesPorProyecto(ctx, _proyectoID).ToList();

            //    //Traer de BD todos los I.C. equivalentes para el proyecto seleccionado
            //    _lstEquivalente = UtileriasCruce.ItemCodeEquivalentesPorProyecto(ctx, _proyectoID).ToList();

            //    //Traer el condensando que me dice cuantos spools tiene un isométrico
            //    _lstResumenIso = UtileriasCruce.AvnceIsometricoMarcadosComoRevision(ctx, _proyectoID).ToList();

            //    //Lista con un condensado de la disponibilidad de números únicos agrupados por item code
            //    _lstCondensadoIC = UtileriasCruce.InventariosCondensadosPorIC(ctx, _proyectoID).ToList();
            //}

            //_logger.DebugFormat("Tiempo Query= {0} ms", sw.ElapsedMilliseconds);

            //_logger.Info("Comienzo ejecucion");
            ////Verifica si es por isometrico o no
            //sw.Restart();
            //_logger.DebugFormat("variables temporales");
            //estableceVariablesTemporales();
            //_logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);

            //sw.Restart();
            //_logger.DebugFormat("ordenamos por prioridad");
            //ordenaSpoolsPorPrioridadRevision();
            //_logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);

            //sw.Restart();
            //_logger.DebugFormat("generamos diccionario");
            //generaDiccionarios();
            //_logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);

            //sw.Restart();
            //_logger.DebugFormat("recorremos spools para congelar material");
            //recorreSpoolsYCongelaMarcadosRevision();
            //_logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);

            //sw.Restart();
            //_logger.DebugFormat("generamos lista de faltantes");
            //faltantes = null; // generaListaFaltantes();
            //_logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);

            //sw.Restart();
            //_logger.DebugFormat("generamos condensado IC");
            //condensado = null; // generaCondensadoItemCodes(faltantes);
            //_logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);

            //return generaListaSpoolsFabricables();
            faltantes = null;
            condensado = null;
            return null;
        }


        public List<SpoolCruce> ProcesaSpoolsEnRevisionSeleccionados(List<int> _spoolsIDs, Guid userID, int tallerID)
        {

            //sw.Start();
            //_logger.DebugFormat(String.Format("Iniciando cruce de Spool en revisión seleccionados: {0}", _nombreProyecto));
            //using (SamContext ctx = new SamContext())
            //{
            //    ctx.Spool.MergeOption = MergeOption.NoTracking;
            //    ctx.MaterialSpool.MergeOption = MergeOption.NoTracking;
            //    ctx.NumeroUnico.MergeOption = MergeOption.NoTracking;
            //    ctx.NumeroUnicoSegmento.MergeOption = MergeOption.NoTracking;
            //    ctx.NumeroUnicoInventario.MergeOption = MergeOption.NoTracking;
            //    ctx.ItemCode.MergeOption = MergeOption.NoTracking;
            //    ctx.ItemCodeEquivalente.MergeOption = MergeOption.NoTracking;
            //    ctx.Colada.MergeOption = MergeOption.NoTracking;
            //    ctx.SpoolHold.MergeOption = MergeOption.NoTracking;


            //    // lista completa de materiales spool para posteriormente hacer un calculo de requerimiento total ingeniería
            //    //Solo spools en revision
            //    _lstMaterialSpool = ctx.Spool.Where(x => _spoolsIDs.Contains(x.SpoolID)).SelectMany(x => x.MaterialSpool).ToList();

                
            //    //Traer de la base de datos los Spool Marcados como Revision para realizar este cruce
            //    //en base a los cuales se hara el cruce, puesto que todos tienen una ODT
            //    _lstSpoolSinOdt = ctx.Spool
            //                        .Include("MaterialSpool")
            //                        .Where(x => _spoolsIDs.Contains(x.SpoolID)).ToList();
                                
            //    //Traer los números únicos del proyecto que tengan inventario disponible para cruce
            //    //mayor a cero de BD.
            //    //Filtrar en memoria sólo los aprobados y cuyas coladas no estén en hold.
            //    //-------- AUN NO SE SI DEBO MODIFICAR ESTE METODO LO DEJO PENDIENTE ---- JHT
            //    _lstNumeroUnico = (from nu in UtileriasCruce.NumerosUnicoParaCruceSinTracking(ctx, _proyectoID)
            //                                                .ToList()
            //                                                .AsParallel()
            //                       where nu.Colada.HoldCalidad == false
            //                             && nu.Estatus != null
            //                             && nu.Estatus.Equals(EstatusNumeroUnico.APROBADO)
            //                       select nu).ToList();

            //    //Traer los números únicos del proyecto que hayan sido rechazados para mostrar cantidades
            //    //en excel de faltantes
            //    _lstNumeroUnicoRechazados = (from nu in UtileriasCruce.NumerosUnicoParaCruceSinTracking(ctx, _proyectoID)
            //                                                .ToList()
            //                                                .AsParallel()
            //                                 where nu.Estatus != null
            //                                       && nu.Estatus.Equals(EstatusNumeroUnico.RECHAZADO)
            //                                 select nu).ToList();

            //    //Traer los números únicos del proyecto que hayan sido condicionados para mostrar cantidades
            //    //en excel de faltantes
            //    _lstNumeroUnicoCondicionados = (from nu in UtileriasCruce.NumerosUnicoParaCruceSinTracking(ctx, _proyectoID)
            //                                                .ToList()
            //                                                .AsParallel()
            //                                    where nu.Estatus != null
            //                                          && nu.Estatus.Equals(EstatusNumeroUnico.CONDICIONADO)
            //                                    select nu).ToList();

            //    //Traer los nus congelados parciales ya que podrían no estar en la primera lista
            //    _lstNumerosUnicosCongelados = (from nu in UtileriasCruce.NumerosUnicosCongeladosParcialmente(ctx, _proyectoID)
            //                                                            .ToList()
            //                                                            .AsParallel()
            //                                   where !_lstNumeroUnico.Select(x => x.NumeroUnicoID).Contains(nu.NumeroUnicoID)
            //                                   select nu).ToList();

            //    //Traer de BD todos los I.C. del proyecto
            //    _lstItemCode = UtileriasCruce.ItemCodesPorProyecto(ctx, _proyectoID).ToList();

            //    //Traer de BD todos los I.C. equivalentes para el proyecto seleccionado
            //    _lstEquivalente = UtileriasCruce.ItemCodeEquivalentesPorProyecto(ctx, _proyectoID).ToList();

            //    //Traer el condensando que me dice cuantos spools tiene un isométrico
            //    _lstResumenIso = UtileriasCruce.AvnceIsometricoSpoolEnRevisionSeleccionados(ctx, _spoolsIDs).ToList();

            //    //Lista con un condensado de la disponibilidad de números únicos agrupados por item code
            //    _lstCondensadoIC = UtileriasCruce.InventariosCondensadosPorIC(ctx, _proyectoID).ToList();
            //}

            //_logger.DebugFormat("Tiempo Query= {0} ms", sw.ElapsedMilliseconds);

            //_logger.Info("Comienzo ejecucion");
            ////Verifica si es por isometrico o no
            //sw.Restart();
            //_logger.DebugFormat("variables temporales");
            //estableceVariablesTemporales();
            //_logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);

            //sw.Restart();
            //_logger.DebugFormat("ordenamos por prioridad");
            //ordenaSpoolsPorPrioridadRevision();
            //_logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);

            //sw.Restart();
            //_logger.DebugFormat("generamos diccionario");
            //generaDiccionarios();
            //_logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);

            //sw.Restart();
            //_logger.DebugFormat("recorremos spools para congelar material");
            //recorreSpoolsYCongelaMarcadosRevision();
            //_logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);

            //OrdenTrabajoEspecialBL.Instance.GenerarOrdenesDeTrabajoEspecial(_lstSpoolSinOdt, userID, _proyectoID, tallerID);

            //return generaListaSpoolsFabricables();
            return null;

        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<CondensadoItemCode> generaCondensadoItemCodes(List<FaltanteCruce> faltantes)
        {
            sw.Restart();
            _logger.DebugFormat("Obteniendo inventario congelado en este cruce");
            //Cuanto se congeló por la operación de cruce, es la diferencia de lo original disponible - lo actual disponible
            _lstNumeroUnico.ForEach(x =>
            {
                x.NumeroUnicoInventario.InfoCruce.InventarioCongeladoParaFabricacion =
                        x.NumeroUnicoInventario.InventarioCongelado -
                        x.NumeroUnicoInventario.InfoCruce.InventarioCongeladoOriginal;
            });

            _logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);

            List<MaterialSpool> materiales = _lstSpoolSinOdt.SelectMany(x => x.MaterialSpool).ToList();

            sw.Restart();
            _logger.DebugFormat("Obteniendo equivalencias de faltantes");
            var qEquivalencias = (from ing in
                                      (from flt in faltantes
                                       select new
                                       {
                                           ItemCodeID = flt.ItemCodeID,
                                           Diametro1 = flt.Diametro1,
                                           Diametro2 = flt.Diametro2
                                       })
                                  join eq in _lstEquivalente on
                                                             new { Icid = ing.ItemCodeID, D1 = ing.Diametro1, D2 = ing.Diametro2 }
                                                             equals
                                                             new { Icid = eq.ItemCodeID, D1 = eq.Diametro1, D2 = eq.Diametro2 }
                                  select new
                                  {
                                      Icid = ing.ItemCodeID,
                                      D1 = ing.Diametro1,
                                      D2 = ing.Diametro2,
                                      IcEq = eq.ItemEquivalenteID,
                                      D1e = eq.DiametroEquivalente1,
                                      D2e = eq.DiametroEquivalente2
                                  }).ToList();
            _logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);

            //traernos la suma de los inventarios por equivalencias de un item code integrado
            sw.Restart();
            _logger.DebugFormat("Obteniendo sumatoria de inventario original IC integrado");

            var inventariosEquivalencias = (from eq in qEquivalencias.Distinct()
                                            join inv in _lstNumeroUnico on
                                                                        new { Id = eq.IcEq, D1 = eq.D1e, D2 = eq.D2e }
                                                                        equals
                                                                        new { Id = inv.ItemCodeID.Value, D1 = inv.Diametro1, D2 = inv.Diametro2 }
                                            group inv by new { eq.Icid, eq.D1, eq.D2 }
                                                into grp
                                                select new
                                                {
                                                    Id = grp.Key.Icid,
                                                    D1 = grp.Key.D1,
                                                    D2 = grp.Key.D2,
                                                    Suma = grp.Sum(nueq => nueq.NumeroUnicoInventario.InfoCruce.InventarioDisponibleCruceOriginal)
                                                }).ToDictionary(k => new { ItemCodeID = k.Id, D1 = k.D1, D2 = k.D2 }, v => v.Suma);

            _logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);

            sw.Restart();
            _logger.DebugFormat("Obteniendo sumatoria de faltante por prioridad");
            var fltPrioridad = (from f in faltantes
                                where !f.Congelado
                                group f by new
                                {
                                    ItemCodeID = f.ItemCodeID,
                                    D1 = f.Diametro1,
                                    D2 = f.Diametro2,
                                    Prioridad = f.Prioridad
                                }
                                    into grupo
                                    select new
                                    {
                                        ItemCodeID = grupo.Key.ItemCodeID,
                                        D1 = grupo.Key.D1,
                                        D2 = grupo.Key.D2,
                                        Prioridad = grupo.Key.Prioridad,
                                        TotalPrioridad = grupo.Sum(x => x.Cantidad)
                                    }).ToList();
            _logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);


            //item codes integrados de inventarios + ingenierias
            sw.Restart();
            _logger.DebugFormat("Obteniendo congelados de equivalencias");
            var nus = (from flt in faltantes
                       join ic in _lstItemCode on flt.NumeroUnicoUtilizado != null && flt.MaterialEquivalente ? flt.NumeroUnicoUtilizado.ItemCodeID : flt.ItemCodeID equals ic.ItemCodeID
                       group flt by new
                       {
                           ItemCodeID = flt.NumeroUnicoUtilizado != null && flt.MaterialEquivalente ? flt.NumeroUnicoUtilizado.ItemCodeID.Value : flt.ItemCodeID,
                           Diametro1 = flt.NumeroUnicoUtilizado != null && flt.MaterialEquivalente ? flt.NumeroUnicoUtilizado.Diametro1 : flt.Diametro1,
                           Diametro2 = flt.NumeroUnicoUtilizado != null && flt.MaterialEquivalente ? flt.NumeroUnicoUtilizado.Diametro2 : flt.Diametro2,
                           ic.Codigo,
                           ic.DescripcionEspanol,
                           flt.MaterialEquivalente
                       }
                           into grp
                           select new
                           {
                               ItemCodeID = grp.Key.ItemCodeID,
                               Diametro1 = grp.Key.Diametro1,
                               Diametro2 = grp.Key.Diametro2,
                               CodigoItemCode = grp.Key.Codigo,
                               DescripcionItemCode = grp.Key.DescripcionEspanol,
                               Congelado = grp.Sum(x => x.Congelado ? x.Cantidad : 0),
                               Equivalente = grp.Key.MaterialEquivalente
                           }).ToList();
            _logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);

            sw.Restart();
            _logger.DebugFormat("Obteniendo sumatorias de congelados directos vs equivalencias");
            var icCo = (from p1 in nus
                        group p1 by new { p1.ItemCodeID, p1.Diametro1, p1.Diametro2, p1.CodigoItemCode, p1.DescripcionItemCode }
                            into grp
                            select new
                            {
                                grp.Key.ItemCodeID,
                                grp.Key.CodigoItemCode,
                                grp.Key.DescripcionItemCode,
                                grp.Key.Diametro1,
                                grp.Key.Diametro2,
                                CongeladoEnEsteCruceOriginal = grp.Sum(x => !x.Equivalente ? x.Congelado : 0),
                                CongeladoEnEsteCruceEquivalencia = grp.Sum(x => x.Equivalente ? x.Congelado : 0)
                            })
                             .Union
                        (
                           from flt in faltantes
                           where !nus.Select(x => new { ItemCodeID = x.ItemCodeID, Diametro1 = x.Diametro1, Diametro2 = x.Diametro2 })
                                              .Contains(new { ItemCodeID = flt.ItemCodeID, Diametro1 = flt.Diametro1, Diametro2 = flt.Diametro2 })
                           group flt by
                           new
                           {
                               flt.ItemCodeID,
                               flt.Diametro1,
                               flt.Diametro2,
                               flt.DescripcionItemCode,
                               flt.CodigoItemCode
                           }
                               into grp
                               select new
                               {
                                   ItemCodeID = grp.Key.ItemCodeID,
                                   CodigoItemCode = grp.Key.CodigoItemCode,
                                   DescripcionItemCode = grp.Key.DescripcionItemCode,
                                   Diametro1 = grp.Key.Diametro1,
                                   Diametro2 = grp.Key.Diametro2,
                                   CongeladoEnEsteCruceOriginal = 0,
                                   CongeladoEnEsteCruceEquivalencia = 0
                               }
                        ).ToList();

            _logger.DebugFormat("Tiempo= {0} ms", sw.ElapsedMilliseconds);

            sw.Restart();
            _logger.DebugFormat("ultimo return");
            return (from p2 in icCo
                    join nu in _lstNumeroUnico on
                    new { ItemCodeID = p2.ItemCodeID, Diametro1 = p2.Diametro1, Diametro2 = p2.Diametro2 } equals new { ItemCodeID = nu.ItemCodeID.Value, nu.Diametro1, nu.Diametro2 }
                    into numerosunico
                    from numerosunicos in numerosunico.DefaultIfEmpty()
                    join pri in fltPrioridad on new { ItemCodeID = p2.ItemCodeID, D1 = p2.Diametro1, D2 = p2.Diametro2 } equals new { pri.ItemCodeID, pri.D1, pri.D2 } into prioridad
                    from prioridades in prioridad.DefaultIfEmpty()
                    group numerosunicos by
                    new
                    {
                        p2.ItemCodeID,
                        p2.Diametro1,
                        p2.Diametro2,
                        p2.CodigoItemCode,
                        p2.DescripcionItemCode,
                        p2.CongeladoEnEsteCruceOriginal,
                        p2.CongeladoEnEsteCruceEquivalencia,
                        Prioridad = prioridades != null ? prioridades.Prioridad : 0,
                        TotalPrioridad = prioridades != null ? prioridades.TotalPrioridad : 0
                    }
                        into grp
                        select new CondensadoItemCode
                        {
                            ItemCodeID = grp.Key.ItemCodeID,
                            CodigoItemCode = grp.Key.CodigoItemCode,
                            DescripcionItemCode = grp.Key.DescripcionItemCode,
                            D1 = grp.Key.Diametro1,
                            D2 = grp.Key.Diametro2,
                            DisponibleCruceOriginal = grp.Sum(g => g != null ? g.NumeroUnicoInventario.InfoCruce.InventarioDisponibleCruceOriginal : 0),
                            CantidadRecibidaOriginal = grp.Sum(g => g != null ? g.NumeroUnicoInventario.InfoCruce.InventarioCantidadRecibidaOriginal : 0),
                            CantidadCondicionadosOriginal = _lstNumeroUnicoCondicionados.Where(x => x.ItemCodeID == grp.Key.ItemCodeID &&
                                                                                            x.Diametro1 == grp.Key.Diametro1 &&
                                                                                            x.Diametro2 == grp.Key.Diametro2).Sum(c => c.NumeroUnicoInventario.CantidadRecibida),
                            CantidadRechazadosOriginal = _lstNumeroUnicoRechazados.Where(x => x.ItemCodeID == grp.Key.ItemCodeID &&
                                                                                            x.Diametro1 == grp.Key.Diametro1 &&
                                                                                            x.Diametro2 == grp.Key.Diametro2).Sum(c => c.NumeroUnicoInventario.CantidadRecibida),
                            CongeladoEnEsteCruceOriginal = grp.Key.CongeladoEnEsteCruceOriginal,
                            CongeladoEnEsteCruceEquivalencia = grp.Key.CongeladoEnEsteCruceEquivalencia,
                            DisponiblePorEquivalencia = inventariosEquivalencias.ContainsKey(new { ItemCodeID = grp.Key.ItemCodeID, D1 = grp.Key.Diametro1, D2 = grp.Key.Diametro2 })
                                                ? inventariosEquivalencias[new { ItemCodeID = grp.Key.ItemCodeID, D1 = grp.Key.Diametro1, D2 = grp.Key.Diametro2 }]
                                                : 0,
                            RequeridaParaFabricacion = materiales.Where(m => m.ItemCodeID == grp.Key.ItemCodeID &&
                                                                            m.Diametro1 == grp.Key.Diametro1 &&
                                                                            m.Diametro2 == grp.Key.Diametro2).Sum(c => c.Cantidad),
                            Prioridad = grp.Key.Prioridad,
                            SumaPrioridad = grp.Key.TotalPrioridad,
                            RequeridaTotalIngenieria = _lstMaterialSpool.Where(m => m.ItemCodeID == grp.Key.ItemCodeID &&
                                                                            m.Diametro1 == grp.Key.Diametro1 &&
                                                                            m.Diametro2 == grp.Key.Diametro2).Sum(x => x.Cantidad)
                        }).ToList();
        }


        /// <summary>
        /// Genera diccionarios de item codes y spools para acceso rápido
        /// </summary>
        private void generaDiccionarios()
        {
            _dicItemCodes = _lstItemCode.ToDictionary(x => x.ItemCodeID);
            _dicSpools = _lstSpoolSinOdt.ToDictionary(x => x.SpoolID);
            _dicCodensados = _lstCondensadoIC.ToDictionary(x => new ItemCodeIntegrado { ItemCodeID = x.ItemCodeID, Diametro1 = x.Diametro1, Diametro2 = x.Diametro2 });
        }

        /// <summary>
        /// 
        /// </summary>
        private List<FaltanteCruce> generaListaFaltantes()
        {
            using (SamContext ctx = new SamContext())
            {
                //Los spools que nos piden
                int[] spoolIds = _lstSpoolSinOdt.Select(x => x.SpoolID).ToArray();

                // Se seleccionan los spools en hold junto con sus observaciones más recientes
                var spoolHoldHistorial = ctx.SpoolHoldHistorial.Where(x => spoolIds.Contains(x.SpoolID)).ToList();
                var spoolHolds = ctx.SpoolHold
                                    .Where(x => spoolIds.Contains(x.SpoolID))
                                    .Where(x => x.TieneHoldIngenieria || x.TieneHoldCalidad || x.Confinado).ToList();
                var spoolObservaciones = from sh in spoolHolds
                                         let spoolHistorial = (from shh in spoolHoldHistorial
                                                               where shh.SpoolID == sh.SpoolID
                                                               select shh).GroupBy(x => x.TipoHold).Select(g => g.Last())
                                         let spoolHoldHistorialObservaciones = from shh in spoolHistorial
                                                                               where this.EstaEnHold(sh, shh)
                                                                               select shh.Observaciones
                                         select new
                                         {
                                             ID = sh.SpoolID,
                                             Observaciones = String.Join(",", spoolHoldHistorialObservaciones)
                                         };

                List<Spool> lstSpoolsSinMaterial = (from spool in _lstSpoolSinOdt where !spool.InfoCruce.CruceExitoso select spool).ToList();
                List<FaltanteCruce> lstFaltantes = new List<FaltanteCruce>();

                foreach (Spool spool in lstSpoolsSinMaterial)
                {
                    List<NumeroUnico> inventarios = UtileriasCruce.NumerosUnicosDisponibles(spool, _lstNumeroUnico);
                    List<NumeroUnico> equivalentes = new List<NumeroUnico>();
                    NumeroUnicoInventario nui = null;
                    NumeroUnicoSegmento nuSegmento = null;

                    #region Iterar cada material del spool

                    foreach (MaterialSpool material in spool.MaterialSpool)
                    {
                        //Si es accesorio se busca directamente sobre la tabla NumeroUnicoInventario
                        if (_dicItemCodes[material.ItemCodeID].TipoMaterialID == (int)TipoMaterialEnum.Accessorio)
                        {
                            nui = UtileriasCruce.ObtenMejorCandidatoAccesorio(inventarios, material, _dicCodensados);

                            if (nui == null)
                            {
                                nui = UtileriasCruce.IntentaConEquivalenciaDeAccesorio(material, _lstEquivalente, _lstNumeroUnico, _dicCodensados);

                                if (nui != null)
                                {
                                    material.InfoCruce.EsEquivalente = true;
                                    spool.InfoCruce.UsoEquivalencia = true;
                                    equivalentes.Add(nui.NumeroUnico);
                                }
                            }

                            if (nui != null)
                            {
                                //Congelar temporalmente y disminuir la disponibilidad para cruce
                                nui.InfoCruce.InventarioCongeladoTemporal += material.Cantidad;
                                nui.InfoCruce.InventarioDisponibleCruceTemporal -= material.Cantidad;

                                CruceItemCode cond = _dicCodensados[new ItemCodeIntegrado(nui.NumeroUnico.ItemCodeID.Value, nui.NumeroUnico.Diametro1, nui.NumeroUnico.Diametro2)];

                                cond.InventarioCongeladoTemporal += material.Cantidad;
                                cond.InventarioDisponibleCruceTemporal -= material.Cantidad;
                            }

                            //aquí registramos los faltantes
                            string spoolObs = spoolObservaciones.SingleOrDefault(o => o.ID == material.SpoolID) != null ? spoolObservaciones.SingleOrDefault(o => o.ID == material.SpoolID).Observaciones : null;
                            bool isoCompleto = _lstIsoCompletos.Any(x => x == spool.Dibujo);

                            lstFaltantes.Add(UtileriasCruce.GeneraFaltante(material, _nombreProyecto, _dicItemCodes, _dicSpools, nui != null, nui != null ? nui.NumeroUnico : null, _familiasAcero, spoolObs, isoCompleto));
                        }
                        else
                        {
                            //si es tubo hay que buscar sobre la tabla NumeroUnicoSegmento
                            nuSegmento = UtileriasCruce.ObtenMejorCandidatoTubo(inventarios, material, _dicCodensados);

                            if (nuSegmento == null)
                            {
                                nuSegmento = UtileriasCruce.IntentaConEquivalenciaDeTubo(material, _lstEquivalente, _lstNumeroUnico, _dicCodensados);

                                if (nuSegmento != null)
                                {
                                    spool.InfoCruce.UsoEquivalencia = true;
                                    material.InfoCruce.EsEquivalente = true;
                                    equivalentes.Add(nuSegmento.NumeroUnico);
                                }
                            }

                            if (nuSegmento != null)
                            {
                                //si se encontró vamos a congelar temporalmente la cantidad
                                nuSegmento.NumeroUnico.NumeroUnicoInventario.InfoCruce.InventarioCongeladoTemporal += material.Cantidad;
                                nuSegmento.NumeroUnico.NumeroUnicoInventario.InfoCruce.InventarioDisponibleCruceTemporal -= material.Cantidad;
                                nuSegmento.InfoCruce.InventarioCongeladoTemporal += material.Cantidad;
                                nuSegmento.InfoCruce.InventarioDisponibleCruceTemporal -= material.Cantidad;

                                CruceItemCode cond = _dicCodensados[new ItemCodeIntegrado(nuSegmento.NumeroUnico.ItemCodeID.Value, nuSegmento.NumeroUnico.Diametro1, nuSegmento.NumeroUnico.Diametro2)];

                                cond.InventarioCongeladoTemporal += material.Cantidad;
                                cond.InventarioDisponibleCruceTemporal -= material.Cantidad;
                            }

                            //aquí registramos los faltantes
                            string spoolObs = spoolObservaciones.SingleOrDefault(o => o.ID == material.SpoolID) != null ? spoolObservaciones.SingleOrDefault(o => o.ID == material.SpoolID).Observaciones : null;
                            bool isoCompleto = _lstIsoCompletos.Any(x => x == spool.Dibujo);
                            lstFaltantes.Add(UtileriasCruce.GeneraFaltante(material, _nombreProyecto, _dicItemCodes, _dicSpools, nuSegmento != null, nuSegmento != null ? nuSegmento.NumeroUnico : null, _familiasAcero, spoolObs, isoCompleto));
                        }
                    }

                    #endregion

                    //siempre congelamos porque para determinar el resto de los faltantes hay
                    //que hacer de cuenta que el material ya no está
                    UtileriasCruce.CongelaInventarios(inventarios, _dicCodensados);
                    UtileriasCruce.CongelaInventarios(equivalentes, _dicCodensados);
                }

                var nus = _lstNumeroUnico.Union(_lstNumerosUnicosCongelados).ToDictionary(k => k.NumeroUnicoID, v => v);

                //spools completos, pero que como quiera se deben de incluir en la lista de faltantes
                (from spool in _lstSpoolSinOdt
                 where spool.InfoCruce.CruceExitoso
                 select spool).SelectMany(x => x.MaterialSpool)
                              .ToList()
                              .ForEach(x => lstFaltantes.Add(UtileriasCruce.GeneraFaltante(x,
                                                                            _nombreProyecto,
                                                                            _dicItemCodes,
                                                                            _dicSpools,
                                                                            true,
                                                                            nus[x.InfoCruce.NumeroUnicoID],
                                                                            _familiasAcero,
                                                                            spoolObservaciones.SingleOrDefault(o => o.ID == x.SpoolID) != null ? spoolObservaciones.SingleOrDefault(o => o.ID == x.SpoolID).Observaciones : null,
                                                                            _lstIsoCompletos.Any(i => i == x.Spool.Dibujo))));

                return lstFaltantes;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private List<FaltanteCruce> generaListaFaltantesPorIso()
        {
            using (SamContext ctx = new SamContext())
            {
                //Los spools que nos piden
                int[] spoolIds = _lstSpoolSinOdt.Select(x => x.SpoolID).ToArray();

                // Se seleccionan los spools en hold junto con sus observaciones más recientes
                var spoolHoldHistorial = ctx.SpoolHoldHistorial.Where(x => spoolIds.Contains(x.SpoolID)).ToList();
                var spoolHolds = ctx.SpoolHold
                                    .Where(x => spoolIds.Contains(x.SpoolID))
                                    .Where(x => x.TieneHoldIngenieria || x.TieneHoldCalidad || x.Confinado).ToList();
                var spoolObservaciones = from sh in spoolHolds
                                         let spoolHistorial = (from shh in spoolHoldHistorial
                                                               where shh.SpoolID == sh.SpoolID
                                                               select shh).GroupBy(x => x.TipoHold).Select(g => g.Last())
                                         let spoolHoldHistorialObservaciones = from shh in spoolHistorial
                                                                               where this.EstaEnHold(sh, shh)
                                                                               select shh.Observaciones
                                         select new
                                         {
                                             ID = sh.SpoolID,
                                             Observaciones = String.Join(",", spoolHoldHistorialObservaciones)
                                         };

                List<Spool> lstSpoolsSinMaterial = (from spool in _lstSpoolSinOdt where !spool.InfoCruce.CruceExitoso select spool).ToList();
                List<FaltanteCruce> lstFaltantes = new List<FaltanteCruce>();

                foreach (Spool spool in lstSpoolsSinMaterial)
                {
                    List<NumeroUnico> inventarios = UtileriasCruce.NumerosUnicosDisponibles(spool, _lstNumeroUnico);
                    List<NumeroUnico> equivalentes = new List<NumeroUnico>();
                    NumeroUnicoInventario nui = null;
                    NumeroUnicoSegmento nuSegmento = null;

                    #region Iterar cada material del spool

                    foreach (MaterialSpool material in spool.MaterialSpool)
                    {
                        //Si es accesorio se busca directamente sobre la tabla NumeroUnicoInventario
                        if (_dicItemCodes[material.ItemCodeID].TipoMaterialID == (int)TipoMaterialEnum.Accessorio)
                        {
                            nui = UtileriasCruce.ObtenMejorCandidatoAccesorio(inventarios, material, _dicCodensados);

                            if (nui == null)
                            {
                                nui = UtileriasCruce.IntentaConEquivalenciaDeAccesorio(material, _lstEquivalente, _lstNumeroUnico, _dicCodensados);

                                if (nui != null)
                                {
                                    material.InfoCruce.EsEquivalente = true;
                                    spool.InfoCruce.UsoEquivalencia = true;
                                    equivalentes.Add(nui.NumeroUnico);
                                }
                            }

                            if (nui != null)
                            {
                                //Congelar temporalmente y disminuir la disponibilidad para cruce
                                nui.InfoCruce.InventarioCongeladoTemporal += material.Cantidad;
                                nui.InfoCruce.InventarioDisponibleCruceTemporal -= material.Cantidad;

                                CruceItemCode cond = _dicCodensados[new ItemCodeIntegrado(nui.NumeroUnico.ItemCodeID.Value, nui.NumeroUnico.Diametro1, nui.NumeroUnico.Diametro2)];

                                cond.InventarioCongeladoTemporal += material.Cantidad;
                                cond.InventarioDisponibleCruceTemporal -= material.Cantidad;
                            }

                            //aquí registramos los faltantes
                            string spoolObs = spoolObservaciones.SingleOrDefault(o => o.ID == material.SpoolID) != null ? spoolObservaciones.SingleOrDefault(o => o.ID == material.SpoolID).Observaciones : null;
                            bool isoCompleto = _lstIsoCompletos.Any(x => x == spool.Dibujo);
                            lstFaltantes.Add(UtileriasCruce.GeneraFaltante(material, _nombreProyecto, _dicItemCodes, _dicSpools, nui != null, nui != null ? nui.NumeroUnico : null, _familiasAcero, spoolObs, isoCompleto));
                        }
                        else
                        {
                            //si es tubo hay que buscar sobre la tabla NumeroUnicoSegmento
                            nuSegmento = UtileriasCruce.ObtenMejorCandidatoTubo(inventarios, material, _dicCodensados);

                            if (nuSegmento == null)
                            {
                                nuSegmento = UtileriasCruce.IntentaConEquivalenciaDeTubo(material, _lstEquivalente, _lstNumeroUnico, _dicCodensados);

                                if (nuSegmento != null)
                                {
                                    spool.InfoCruce.UsoEquivalencia = true;
                                    material.InfoCruce.EsEquivalente = true;
                                    equivalentes.Add(nuSegmento.NumeroUnico);
                                }
                            }

                            if (nuSegmento != null)
                            {
                                //si se encontró vamos a congelar temporalmente la cantidad
                                nuSegmento.NumeroUnico.NumeroUnicoInventario.InfoCruce.InventarioCongeladoTemporal += material.Cantidad;
                                nuSegmento.NumeroUnico.NumeroUnicoInventario.InfoCruce.InventarioDisponibleCruceTemporal -= material.Cantidad;
                                nuSegmento.InfoCruce.InventarioCongeladoTemporal += material.Cantidad;
                                nuSegmento.InfoCruce.InventarioDisponibleCruceTemporal -= material.Cantidad;

                                CruceItemCode cond = _dicCodensados[new ItemCodeIntegrado(nuSegmento.NumeroUnico.ItemCodeID.Value, nuSegmento.NumeroUnico.Diametro1, nuSegmento.NumeroUnico.Diametro2)];

                                cond.InventarioCongeladoTemporal += material.Cantidad;
                                cond.InventarioDisponibleCruceTemporal -= material.Cantidad;
                            }

                            //aquí registramos los faltantes
                            string spoolObs = spoolObservaciones.SingleOrDefault(o => o.ID == material.SpoolID) != null ? spoolObservaciones.SingleOrDefault(o => o.ID == material.SpoolID).Observaciones : null;
                            bool isoCompleto = _lstIsoCompletos.Any(x => x == spool.Dibujo);
                            lstFaltantes.Add(UtileriasCruce.GeneraFaltante(material, _nombreProyecto, _dicItemCodes, _dicSpools, nuSegmento != null, nuSegmento != null ? nuSegmento.NumeroUnico : null, _familiasAcero, spoolObs, isoCompleto));
                        }
                    }

                    #endregion

                    //siempre congelamos porque para determinar el resto de los faltantes hay
                    //que hacer de cuenta que el material ya no está
                    UtileriasCruce.CongelaInventarios(inventarios, _dicCodensados);
                    UtileriasCruce.CongelaInventarios(equivalentes, _dicCodensados);
                }

                var nus = _lstNumeroUnico.Union(_lstNumerosUnicosCongelados).ToDictionary(k => k.NumeroUnicoID, v => v);

                //spools completos, pero que como quiera se deben de incluir en la lista de faltantes
                (from spool in _lstSpoolSinOdt
                 where spool.InfoCruce.CruceExitoso
                 select spool).SelectMany(x => x.MaterialSpool)
                              .ToList()
                              .ForEach(x => lstFaltantes.Add(UtileriasCruce.GeneraFaltante(x, _nombreProyecto, _dicItemCodes, _dicSpools, true, nus[x.InfoCruce.NumeroUnicoID], _familiasAcero, spoolObservaciones.SingleOrDefault(o => o.ID == x.SpoolID) != null ? spoolObservaciones.SingleOrDefault(o => o.ID == x.SpoolID).Observaciones : null, _lstIsoCompletos.Any(i => i == x.Spool.Dibujo))));

                return lstFaltantes;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private List<SpoolCruce> generaListaSpoolsFabricables()
        {
            return (from spool in _lstSpoolSinOdt
                    where spool.InfoCruce.CruceExitoso
                    select new SpoolCruce
                    {
                        SpoolID = spool.SpoolID,
                        UsoEquivalencias = spool.InfoCruce.UsoEquivalencia,
                        VersionRegistro = spool.VersionRegistro
                    }).ToList();
        }

        //private List<SpoolCruce> generaListaSpoolsFabricablesSinODTEspecial()
        //{

        //    List<int> spoolsHold = new List<int>();
        //    using (SamContext ctx = new SamContext())
        //    {
        //        //spoolConODTEspecial = (from odtes in ctx.OrdenTrabajoEspecialSpool
        //        //                       join spool in ctx.Spool on odtes.SpoolID equals spool.SpoolID
        //        //                       where spool.ProyectoID == _proyectoID && spool.EsRevision == true
        //        //                       select spool.SpoolID).ToList();

               
        //        spoolsHold = (from sph in ctx.SpoolHold
        //                      where sph.Spool.ProyectoID == _proyectoID &&
        //                            (sph.TieneHoldCalidad || sph.TieneHoldIngenieria || sph.Confinado)
        //                            && sph.Spool.EsRevision == true && (sph.Spool.UltimaOrdenTrabajoEspecial == null || sph.Spool.UltimaOrdenTrabajoEspecial == "")
        //                      select sph.SpoolID).ToList();

        //        //Traer de la base de datos los Spool Marcados como Revision para realizar este cruce
        //        //en base a los cuales se hara el cruce, puesto que todos tienen una ODT
        //        _lstSpoolSinOdt = (from spool in UtileriasCruce.SpoolMarcadosComoRevision(ctx, _proyectoID)
        //                                                        .ToList()
        //                                                        .AsParallel()
        //                           where spool.PendienteDocumental
        //                                 && spool.AprobadoParaCruce
        //                                 && (spool.Prioridad == null || spool.Prioridad > 0)
        //                                 && !spoolsHold.Contains(spool.SpoolID)
        //                           select spool).ToList();
        //        recorreSpoolsYCongelaMarcadosRevision();
        //    }

        //    List<SpoolCruce> resultados = (from spoolOdt in _lstSpoolSinOdt
        //                                   where spoolOdt.InfoCruce.CruceExitoso
        //                                   select new SpoolCruce
        //                                   {
        //                                       SpoolID = spoolOdt.SpoolID,
        //                                       UsoEquivalencias = spoolOdt.InfoCruce.UsoEquivalencia,
        //                                       VersionRegistro = spoolOdt.VersionRegistro
        //                                   }).ToList();

        //    return resultados;
        //}

        /// <summary>
        /// Para poder afectar los inventarios spool x spool necesitamos poder llevar tracking de esos cambios
        /// en otras variables.
        /// </summary>
        private void estableceVariablesTemporales()
        {
            //Copia congelados y disponibles a temporales
            _lstNumeroUnico.ForEach(x =>
            {
                x.NumeroUnicoInventario.InfoCruce.InventarioCongeladoTemporal = x.NumeroUnicoInventario.InventarioCongelado;
                x.NumeroUnicoInventario.InfoCruce.InventarioDisponibleCruceTemporal = x.NumeroUnicoInventario.InventarioDisponibleCruce;

                x.NumeroUnicoInventario.InfoCruce.InventarioCongeladoOriginal = x.NumeroUnicoInventario.InventarioCongelado;
                x.NumeroUnicoInventario.InfoCruce.InventarioDisponibleCruceOriginal = x.NumeroUnicoInventario.InventarioDisponibleCruce;
                x.NumeroUnicoInventario.InfoCruce.InventarioCantidadRecibidaOriginal = x.NumeroUnicoInventario.CantidadRecibida;
                x.NumeroUnicoInventario.InfoCruce.InventarioCantidadDanadaOriginal = x.NumeroUnicoInventario.CantidadDanada;

                foreach (NumeroUnicoSegmento nus in x.NumeroUnicoSegmento)
                {
                    nus.InfoCruce.InventarioCongeladoTemporal = nus.InventarioCongelado;
                    nus.InfoCruce.InventarioDisponibleCruceTemporal = nus.InventarioDisponibleCruce;
                    //nus.InfoCruce.EsCongeladoParcial = false;
                }
            });

            //asumimos que no se va a poder cruzar
            _lstSpoolSinOdt.ForEach(x =>
            {
                x.InfoCruce.CruceExitoso = false;
                x.InfoCruce.UsoEquivalencia = false;
            });

            //completar el resumen por isométrico
            _lstResumenIso.ForEach(x =>
            {
                x.SpoolsSinODT = x.TotalSpools - x.SpoolsConODT;
            });

            //copiar las cantidades de cruce y congelados a temporales
            _lstCondensadoIC.ForEach(x =>
            {
                x.InventarioDisponibleCruceTemporal = x.InventarioDisponibleCruce;
                x.InventarioCongeladoTemporal = x.InventarioCongelado;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EsRevision">True si es un proceso de Spool en revision</param>
        private void recorreSpoolsYCongelaMarcadosRevision()
        {
            _lstIsoCompletos = new List<string>();
           
            foreach (Spool spool in _lstSpoolSinOdt)
            {
                List<NumeroUnico> inventarios = UtileriasCruce.NumerosUnicosDisponibles(spool, _lstNumeroUnico);
                List<NumeroUnico> equivalentes = new List<NumeroUnico>();

                spool.InfoCruce.CruceExitoso = UtileriasCruce.BuscaMaterialSpoolMarcadosRevision(spool, inventarios, equivalentes, 
                    _lstEquivalente, _lstNumeroUnico, _dicCodensados, _dicItemCodes);

                if (spool.InfoCruce.CruceExitoso)
                {
                    UtileriasCruce.CongelaInventarios(inventarios, _dicCodensados);
                    UtileriasCruce.CongelaInventarios(equivalentes, _dicCodensados);
                }
                else
                {
                    UtileriasCruce.QuitaCongelados(inventarios, _dicCodensados);
                    UtileriasCruce.QuitaCongelados(equivalentes, _dicCodensados);
                }
            }

            // Checamos si los spools con cruce exitoso completan el isométrico
            var q = from s in _lstSpoolSinOdt
                    group s by s.Dibujo;

            foreach (var isometrico in q)
            {
                if (isometrico.All(x => x.InfoCruce.CruceExitoso))
                {
                    _lstIsoCompletos.Add(isometrico.Key);
                }
            }
        }

        private void recorreSpoolsYCongela()
        {
            _lstIsoCompletos = new List<string>();
            List<string> isometricos = _lstSpoolSinOdt.Select(x => x.Dibujo).Distinct().ToList();

            foreach (Spool spool in _lstSpoolSinOdt)
            {
                List<NumeroUnico> inventarios = UtileriasCruce.NumerosUnicosDisponibles(spool, _lstNumeroUnico);
                List<NumeroUnico> equivalentes = new List<NumeroUnico>();

                spool.InfoCruce.CruceExitoso = UtileriasCruce.BuscaMaterial(spool, inventarios, equivalentes, _lstCongeladoParcial,
                    _lstEquivalente, _lstNumeroUnico, _dicCodensados, _dicItemCodes);

                if (spool.InfoCruce.CruceExitoso)
                {
                    UtileriasCruce.CongelaInventarios(inventarios, _dicCodensados);
                    UtileriasCruce.CongelaInventarios(equivalentes, _dicCodensados);
                }
                else
                {
                    UtileriasCruce.QuitaCongelados(inventarios, _dicCodensados);
                    UtileriasCruce.QuitaCongelados(equivalentes, _dicCodensados);
                }
            }

            // Checamos si los spools con cruce exitoso completan el isométrico
            var q = from s in _lstSpoolSinOdt
                    group s by s.Dibujo;

            foreach (var isometrico in q)
            {
                if (isometrico.All(x => x.InfoCruce.CruceExitoso))
                {
                    _lstIsoCompletos.Add(isometrico.Key);
                }
            }
        }

        //private void recorreSpoolsYCongelaSpoolsRevisionSeleccionados(Guid userID)
        //{
        //    _lstIsoCompletos = new List<string>();
        //    List<string> isometricos = _lstSpoolSinOdt.Select(x => x.Dibujo).Distinct().ToList();

        //    foreach (Spool spool in _lstSpoolSinOdt)
        //    {
        //        List<NumeroUnico> inventarios = UtileriasCruce.NumerosUnicosDisponibles(spool, _lstNumeroUnico);
        //        List<NumeroUnico> equivalentes = new List<NumeroUnico>();

        //        spool.InfoCruce.CruceExitoso = UtileriasCruce.BuscaMaterialSpoolEnRevisionSeleccionados(spool, inventarios, equivalentes, _lstCongeladoParcial,
        //            _lstEquivalente, _lstNumeroUnico, _dicCodensados, _dicItemCodes, userID);

        //        if (spool.InfoCruce.CruceExitoso)
        //        {
        //            UtileriasCruce.CongelaInventarios(inventarios, _dicCodensados);
        //            UtileriasCruce.CongelaInventarios(equivalentes, _dicCodensados);
        //        }
        //        else
        //        {
        //            UtileriasCruce.QuitaCongelados(inventarios, _dicCodensados);
        //            UtileriasCruce.QuitaCongelados(equivalentes, _dicCodensados);
        //        }
        //    }

        //    // Checamos si los spools con cruce exitoso completan el isométrico
        //    var q = from s in _lstSpoolSinOdt
        //            group s by s.Dibujo;

        //    foreach (var isometrico in q)
        //    {
        //        if (isometrico.All(x => x.InfoCruce.CruceExitoso))
        //        {
        //            _lstIsoCompletos.Add(isometrico.Key);
        //        }
        //    }
        //}


        /// <summary>
        /// 
        /// </summary>
        private void recorreIsometricosYCongela()
        {
            _lstIsoCompletos = new List<string>();
            List<string> isometrico = _lstSpoolSinOdt.Select(x => x.Dibujo).Distinct().ToList();


            foreach (string iso in isometrico)
            {
                List<Spool> lstSpoolCruceExitoso = new List<Spool>();
                List<NumeroUnico> inventariosAcumulados = new List<NumeroUnico>();
                List<NumeroUnico> equivalentesAcumulados = new List<NumeroUnico>();

                bool cruceFaltante = false;

                foreach (Spool spool in _lstSpoolSinOdt.Where(x => x.Dibujo == iso))
                {
                    List<NumeroUnico> inventarios = UtileriasCruce.NumerosUnicosDisponibles(spool, _lstNumeroUnico);
                    List<NumeroUnico> equivalentes = new List<NumeroUnico>();

                    spool.InfoCruce.CruceExitoso = UtileriasCruce.BuscaMaterial(spool, inventarios, equivalentes, _lstCongeladoParcial, _lstEquivalente, _lstNumeroUnico, _dicCodensados, _dicItemCodes);

                    if (spool.InfoCruce.CruceExitoso)
                    {
                        inventarios.ForEach(x => inventariosAcumulados.Add(x));
                        equivalentes.ForEach(x => equivalentesAcumulados.Add(x));
                        lstSpoolCruceExitoso.Add(spool);
                    }
                    else
                    {
                        UtileriasCruce.QuitaCongelados(inventarios, _dicCodensados);
                        UtileriasCruce.QuitaCongelados(equivalentes, _dicCodensados);
                        cruceFaltante = true;
                        break;
                    }
                }

                if (cruceFaltante)
                {
                    foreach (Spool spool in lstSpoolCruceExitoso)
                    {
                        spool.InfoCruce.CruceExitoso = false;
                    }

                    UtileriasCruce.QuitaCongelados(inventariosAcumulados, _dicCodensados);
                    UtileriasCruce.QuitaCongelados(equivalentesAcumulados, _dicCodensados);
                }
                else
                {
                    _lstIsoCompletos.Add(iso);
                    UtileriasCruce.CongelaInventarios(inventariosAcumulados, _dicCodensados);
                    UtileriasCruce.CongelaInventarios(equivalentesAcumulados, _dicCodensados);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ordenaSpoolsPorPrioridad()
        {
            var spoolsParciales = (from cong in _lstCongeladoParcial
                                   join mat in _lstSpoolSinOdt.SelectMany(s => s.MaterialSpool) on cong.MaterialSpoolID equals mat.MaterialSpoolID
                                   select new { SpoolID = mat.SpoolID }).Distinct().ToList();

            //Hacemos el join con los isométricos para poder ordenar
            //Lo de abajo se traduce a lo siguiente:
            // 1. Ordenar por columna Prioridad
            // 2. Después por la menor cantidad de spools necesarios para terminar el isométrico
            // 3. Después por la continuidad del isométrico (etiquetas de material)
            // 4. Después por mayor cantidad de pulgadas diametrales del spool
            // 5. Después por menor número de juntas
            _lstSpoolSinOdt =
                (from spool in _lstSpoolSinOdt
                 join isometrico in _lstResumenIso on spool.Dibujo equals isometrico.Dibujo
                 join mat in spoolsParciales on spool.SpoolID equals mat.SpoolID into leftmat
                 from left in leftmat.DefaultIfEmpty()
                 orderby left == null ? 1 : 0,
                         spool.Prioridad ?? 99, //a los nulos ponerles 99
                         isometrico.SpoolsSinODT,
                         (
                            from material in spool.MaterialSpool
                            orderby material.Etiqueta
                            select material.Etiqueta
                         ).FirstOrDefault(),
                         spool.Pdis descending,
                         spool.JuntaSpool.Count
                 select spool).ToList();
        }

        private void ordenaSpoolsPorPrioridadRevision()
        {
            
            //Hacemos el join con los isométricos para poder ordenar
            //Lo de abajo se traduce a lo siguiente:
            // 1. Ordenar por columna Prioridad
            // 2. Después por la menor cantidad de spools necesarios para terminar el isométrico
            // 3. Después por la continuidad del isométrico (etiquetas de material)
            // 4. Después por mayor cantidad de pulgadas diametrales del spool
            // 5. Después por menor número de juntas
            _lstSpoolSinOdt =
                (from spool in _lstSpoolSinOdt
                 join isometrico in _lstResumenIso on spool.Dibujo equals isometrico.Dibujo
                 orderby spool.Prioridad ?? 999, //a los nulos ponerles 999
                         isometrico.SpoolsSinODT,                         
                         spool.Pdis descending,
                         spool.JuntaSpool.Count
                 select spool).ToList();
        }


        /// <summary>
        /// No toma en cuenta la prioridad de los spools sino que los ordena por 
        /// </summary>
        private void ordenaSpoolsPorIsometrico()
        {
            var spoolsParciales = (from cong in _lstCongeladoParcial
                                   join mat in _lstSpoolSinOdt.SelectMany(s => s.MaterialSpool) on cong.MaterialSpoolID equals mat.MaterialSpoolID
                                   select new { SpoolID = mat.SpoolID }).Distinct().ToList();

            //Hacemos el join con los isométricos para poder ordenar
            //Lo de abajo se traduce a lo siguiente:
            // 1. Ordenar por la menor cantidad de spools necesarios para terminar el isométrico
            // 2. Después por el nombre de isometrico
            // 3. Después por la continuidad del isométrico (etiquetas de material)
            // 4. Después por mayor cantidad de pulgadas diametrales del spool
            // 5. Después por menor número de juntas
            _lstSpoolSinOdt =
                (from spool in _lstSpoolSinOdt
                 join isometrico in _lstResumenIso on spool.Dibujo equals isometrico.Dibujo
                 join mat in spoolsParciales on spool.SpoolID equals mat.SpoolID into leftmat
                 from left in leftmat.DefaultIfEmpty()
                 orderby spool.Prioridad,
                         isometrico.SpoolsSinODT,
                         isometrico.Dibujo,
                         (
                            from material in spool.MaterialSpool
                            orderby material.Etiqueta
                            select material.Etiqueta
                         ).FirstOrDefault(),

                         spool.Pdis descending,
                         spool.JuntaSpool.Count
                 select spool).ToList();
        }
    }
}
