using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Transactions;
using System.Web;
using log4net;
using Mimo.Framework.Common;
using Mimo.Framework.Data;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic;
using SAM.BusinessLogic.Excel;
using SAM.BusinessLogic.Produccion;
using SAM.BusinessLogic.Workstatus;
using SAM.BusinessObjects.Administracion;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.Entities.Cache;
using SAM.Entities.Personalizadas;
using SAM.BusinessObjects.Utilerias;
using SAM.Common;
using SAM.Web;
using SAM.Web.Classes;
using System.Web.Util;



namespace SAM.BusinessLogic.Ingenieria
{
    public class IngenieriaBL
    {
        private static IngenieriaBL _instance;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(IngenieriaBL));
        private static readonly object _mutex = new object();
        

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private IngenieriaBL()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase IngenieriaBL
        /// </summary>
        /// <returns></returns>
        public static IngenieriaBL Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new IngenieriaBL();
                    }
                }
                return _instance;
            }
        }


        public void ActualizarPeqKgtEsp(int proyectoID, string imgPath, int proyectoid, Guid usuarioID)
        {
          List<JuntaSpool> peqNoEncontrados = new List<JuntaSpool>();
          List<JuntaSpool> kgtNoEncontrados = new List<JuntaSpool>();
          List<JuntaSpool> espNoEncontrados = new List<JuntaSpool>();

            DateTime hoy = DateTime.Now;
            Guid NombreArchivo = Guid.NewGuid();
            string idioma = LanguageHelper.CustomCulture;

            List<CedulaCache> cedulas = CacheCatalogos.Instance.ObtenerCedulas();
            _logger.DebugFormat("cedulas");

            List<DiametroCache> diametros = CacheCatalogos.Instance.ObtenerDiametros();
            _logger.DebugFormat("diametros");

            List<Espesor> espesores = EspesorBO.Instance.ObtenerTodos();
            _logger.DebugFormat("espesores");

            List<KgTeorico> kgTeoricos = KgTeoricoBO.Instance.ObtenerTodos();
            _logger.DebugFormat("kgTeoricos");

            List<Peq> peqs = PeqBO.Instance.ObtenerPorProyecto(proyectoID);
            _logger.DebugFormat("peqs");

            Dictionary<string, int> dicCedulas = cedulas.ToDictionary(x => x.Nombre, y => y.ID);
            Dictionary<decimal, int> dicDiametros = diametros.ToDictionary(x => x.Valor, y => y.ID);




            var dicPeqs = peqs.ToDictionary(x => new
            {
                FamiliaAceroID = x.FamiliaAceroID,
                TipoJuntaID = x.TipoJuntaID,
                CedulaID = x.CedulaID,
                DiametroID = x.DiametroID
            },
                                            y => y.Equivalencia);


            var dicKgT = kgTeoricos.ToDictionary(x => new
            {
                CedulaID = x.CedulaID,
                DiametroID = x.DiametroID
            },
                                                      y => y.Valor);


            var dicEsp = espesores.ToDictionary(x => new
            {
                CedulaID = x.CedulaID,
                DiametroID = x.DiametroID
            }, y => y.Valor);


            List<JuntaSpool> juntaSpools = JuntaSpoolBO.Instance.ObtenerPorProyecto(proyectoID);
            _logger.DebugFormat("juntaSpools");

            List<JuntaSpool> JuntaSpoolsEditados = new List<JuntaSpool>();

            #region Actualiza Peq, Kgt, Esp

            _logger.DebugFormat("comienza el ciclo de juntaSpool");

            foreach (JuntaSpool js in juntaSpools)
            {
                if (dicCedulas.ContainsKey(js.Cedula) && dicDiametros.ContainsKey(js.Diametro))
                {
                    bool agrega = false;
                    //Actualiza Peq
                    var peqKey = new
                    {
                        FamiliaAceroID = js.FamiliaAceroMaterial1ID,
                        TipoJuntaID = js.TipoJuntaID,
                        CedulaID = dicCedulas[js.Cedula],
                        DiametroID = dicDiametros[js.Diametro]
                    };


                    if (dicPeqs.ContainsKey(peqKey))
                    {
                        decimal valor = dicPeqs[peqKey];
                        js.StartTracking();
                        js.Peqs = valor;
                        agrega = true;
                    }
                    else
                    {
                        _logger.DebugFormat("peqNoEncontrados");
                        peqNoEncontrados.Add(js);
                    }

                    //Actualiza KgTeorico
                    var kgtKey = new
                    {
                        CedulaID = dicCedulas[js.Cedula],
                        DiametroID = dicDiametros[js.Diametro]
                    };

                    if (dicKgT.ContainsKey(kgtKey))
                    {
                        decimal valor = dicKgT[kgtKey];
                        js.StartTracking();
                        js.KgTeoricos = valor;
                        agrega = true;
                    }
                    else
                    {
                        _logger.DebugFormat("kgtNoEncontrados");
                        kgtNoEncontrados.Add(js);
                    }

                    //Actualiza Espesor
                    var espKey = new
                    {
                        CedulaID = dicCedulas[js.Cedula],
                        DiametroID = dicDiametros[js.Diametro]
                    };

                    if (dicEsp.ContainsKey(espKey))
                    {
                        decimal valor = dicEsp[espKey];
                        js.StartTracking();
                        js.Espesor = valor;
                        agrega = true;
                    }
                    else
                    {
                        _logger.DebugFormat("espNoEncontrados");
                        espNoEncontrados.Add(js);
                    }

                    if (agrega)
                    {
                        JuntaSpoolsEditados.Add(js);
                    }
                }

            }
            _logger.DebugFormat("termina el ciclo de juntaSpool");
            #endregion

            _logger.DebugFormat("inicia el Guarda juntaSpool ");
            JuntaSpoolBO.Instance.Guarda(JuntaSpoolsEditados);
            _logger.DebugFormat("fin el Guarda juntaSpool ");


            _logger.DebugFormat("GeneraExcelPeqKgtEspNoEncontrados");
            ExcelPeqKgtEspNoEncontrados.Instance.GeneraExcelPeqKgtEspNoEncontrados(peqNoEncontrados, 
                                                    kgtNoEncontrados, espNoEncontrados, NombreArchivo,
                                                    hoy.ToShortDateString(), hoy.ToShortTimeString(),
                                                    imgPath, idioma, proyectoid,usuarioID);

        }

        /// <summary>
        /// Metodo que almacena en BD los spools procesados por el interprete de datos, realiza algunas operaciones importantes como eliminacion de materiales y ordenes de trabajo
        /// </summary>
        /// <param name="itemCodeNoEncontrados"></param>
        /// <param name="cedulasNoEncontradas">La lista de las ceduals que son nuevas</param>
        /// <param name="diametrosNoEncontrados">La lista de los diametros que son nuevos</param>
        /// <param name="fabAreasNoEncontradas"></param>
        /// <param name="tipoCortesCatalogoNoEncontrados"></param>
        /// <param name="tipoJuntasNoEncontradas"></param>
        /// <param name="familiasAceroNoEncontradas"></param>
        /// <param name="proyectoId"></param>
        /// <param name="lineaDtsxConArgumentos"></param>
        public void RegistraCatalogos(IEnumerable<string> itemCodeNoEncontrados, IEnumerable<string> cedulasNoEncontradas, IEnumerable<string> diametrosNoEncontrados, IEnumerable<string> fabAreasNoEncontradas, IEnumerable<string> tipoCortesCatalogoNoEncontrados, IEnumerable<string> tipoJuntasNoEncontradas, List<FamiliaAcero> familiasAceroNoEncontradas, int proyectoId, string lineaDtsxConArgumentos, IEnumerable<string> itemCodesAModificar)
        {
            using (SamContext ctx = new SamContext())
            {
                //using(TransactionScope scope = new TransactionScope())
                //{
                //    ctx.Connection.Open();

                //hacemos una lista de cedulas y guardamos
                cedulasNoEncontradas.ToList().Select(x => new Cedula { Codigo = x }).ToList().ForEach(
                    ctx.Cedula.ApplyChanges);

                //hacemos una lista de diametros
                diametrosNoEncontrados.ToList().Select(
                    x => new Diametro { Valor = x.SafeDecimalParse(), VerificadoPorCalidad = false }).ToList().ForEach(
                        ctx.Diametro.ApplyChanges);


                
                itemCodeNoEncontrados.ToList().Select(
                    x =>
                    new ItemCode
                    {
                        Codigo = x.Split('|')[0],
                        DescripcionEspanol = x.Split('|')[1],
                        DescripcionIngles = x.Split('|')[1],
                        TipoMaterialID = x.Split('|')[2].Trim().ToLower().StartsWith("pipe") ? (int)TipoMaterialEnum.Tubo : (int)TipoMaterialEnum.Accessorio,
                        ProyectoID = proyectoId,
                        FechaModificacion = DateTime.Now
                    }).ToList().ForEach(ctx.ItemCode.ApplyChanges);
                fabAreasNoEncontradas.ToList().Select(x => new FabArea { Nombre = x }).ToList().ForEach(ctx.FabArea.ApplyChanges);
                tipoCortesCatalogoNoEncontrados.ToList().Select(x => new TipoCorte { Codigo = x, Nombre = x }).ToList().ForEach(ctx.TipoCorte.ApplyChanges);
                tipoJuntasNoEncontradas.ToList().Select(x => new TipoJunta { Codigo = x, Nombre = x }).ToList().ForEach(ctx.TipoJunta.ApplyChanges);
                familiasAceroNoEncontradas.ToList().ForEach(ctx.FamiliaAcero.ApplyChanges);

                foreach (string item in itemCodesAModificar)
                {
                    string codigo = item.Split('|')[0];
                    string descripcion = item.Split('|')[1];

                    ItemCode ic = ctx.ItemCode.Where(x => x.ProyectoID == proyectoId && x.Codigo == codigo).SingleOrDefault();

                    ic.DescripcionEspanol = descripcion;
                    ic.DescripcionIngles = descripcion;

                    ctx.ItemCode.ApplyChanges(ic);
                }

                ctx.SaveChanges();

                string nombreProc = "EjecutaDTSCargaIngenieria";
                using (IDbConnection connection = DataAccessFactory.CreateConnection("SamDB"))
                {
                    connection.Open();
                    IDbDataParameter[] parameters = DataAccess.GetSpParameterSet("SamDB", nombreProc);
                    parameters[0].Value = lineaDtsxConArgumentos;
                    _logger.Debug(lineaDtsxConArgumentos);

                    int result = (int)DataAccess.ExecuteScalar(connection, CommandType.StoredProcedure, nombreProc, parameters);

                    connection.Close();

                    if (result == 0) // si no hay error
                    {
                        //scope.Complete();
                    }
                }
                //Quita los catalogos de cache 
                CacheCatalogos.Instance.LimpiaCacheIngenieria();
                // }
                //ctx.Connection.Close();
            }
        }

        public List<SpoolIng> ObtenerInfoArchivosSubidos(int proyectoID, Guid userId)
        {
            ArchivoSimple[] archivos = InterpreteDatos.ObtenerArchivosSubidos();
            InterpreteDatos interpreteDatos = new InterpreteDatos(proyectoID, archivos, null, userId);
            List<SpoolIng> spoolIngs = interpreteDatos.SpoolsArchivoCache;

            return spoolIngs;
        }

      

        public SpoolPendiente ObtenerPendientePorHomologar(int spoolPendienteID)
        {

            SpoolPendiente sp = null;

            using (SamContext ctx = new SamContext())
            {
                //Traerme el spool
                sp = ctx.SpoolPendiente.Where(x => x.SpoolPendienteID == spoolPendienteID).Single();

                //Llenar sus colecciones
                ctx.LoadProperty<SpoolPendiente>(sp, x => x.MaterialSpoolPendiente);
                ctx.LoadProperty<SpoolPendiente>(sp, x => x.CorteSpoolPendiente);
                ctx.LoadProperty<SpoolPendiente>(sp, x => x.JuntaSpoolPendiente);

                //Preparar para subquery de item codes
                IQueryable<MaterialSpoolPendiente> iqMateriales = ctx.MaterialSpoolPendiente
                                                  .Where(x => x.SpoolPendienteID == spoolPendienteID);

                IQueryable<int> icCortes = ctx.CorteSpoolPendiente
                                              .Where(x => x.SpoolPendienteID == spoolPendienteID)
                                              .Select(y => y.ItemCodeID);



                //Traer los item codes al contexto, esto va a hacer que el grafo del spool se
                //complete con los item codes que necesita
                IQueryable<int> itemcode = from item in ctx.ItemCode
                                                where iqMateriales.Select(y => y.ItemCodeID).Contains(item.ItemCodeID) || icCortes.Contains(item.ItemCodeID)
                                                select item.ItemCodeID;

                ctx.ItemCode.Where(
                    x =>
                    itemcode.Contains(x.ItemCodeID)).
                    ToList();

                //Agregamos el tipo de corte para tener la descripcion
                ctx.TipoCorte.ToList();
                //Agregamos el tipo de Junta para tener la descripcion
                ctx.TipoJunta.ToList();
                ctx.FabArea.ToList();
                ctx.FamiliaAcero.ToList();


            }

            return sp;

        }


        public List<SpoolPendiente> ObtenerPendientesPorHomologar(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.SpoolPendiente.Where(x => x.ProyectoID == proyectoID).ToList();
            }
        }

        public int ObtenerSpoolIdSegunSpoolPendienteId(int spoolPendienteID)
        {
            using (SamContext ctx = new SamContext())
            {
                SpoolPendiente spoolPend =
                    ctx.SpoolPendiente.SingleOrDefault(x => x.SpoolPendienteID == spoolPendienteID);
                if (spoolPend == null)
                {
                    throw new Exception(MensajesError.Excepcion_SinSpoolsPendientes.ToString() + " " + spoolPendienteID);
                }

                return ctx.Spool.Single(x => x.Nombre == spoolPend.Nombre && x.ProyectoID == spoolPend.ProyectoID).SpoolID;
            }
        }

        /// <summary>
        /// El material cambia de cantidad (menor)
        /// Si el material sólo se encuentra congelado, ésta cantidad debe modificarse en el inventario del número único congelado para que éste sea consistente con la nueva cantidad.
        /// </summary>
        /// <param name="ctx">Contexto que se abrió para la transaccion</param>
        /// <param name="material">MaterialPendientePorHomologar</param>
        public void MaterialCambiaCantidadCongelado(SamContext ctx, int MaterialSpoolPendienteID, int MaterialSpoolID, bool esTubo, Guid userID)
        {
            //Se modifica la cantidad en OrdenTrabajoMaterial
            //Se modifica el inventario del número unico congelado (si es tubo tambien hay que modificar al segmento)

            int diferenciaCongelado = 0;
            MaterialSpoolPendiente pendiente = ctx.MaterialSpoolPendiente.Where(x => x.MaterialSpoolPendienteID == MaterialSpoolPendienteID).SingleOrDefault();

            OrdenTrabajoMaterial otm = ctx.OrdenTrabajoMaterial.Where(x => x.MaterialSpoolID == MaterialSpoolID).SingleOrDefault();
            diferenciaCongelado = otm.CantidadCongelada.Value - pendiente.Cantidad;

            otm.StartTracking();
            otm.CantidadCongelada = pendiente.Cantidad;
            otm.UsuarioModifica = userID;
            otm.FechaModificacion = DateTime.Now;

            NumeroUnico numUnicoCongelado = ctx.NumeroUnico.Include("NumeroUnicoInventario").Include("NumeroUnicoSegmento").Where(x => x.NumeroUnicoID == otm.NumeroUnicoCongeladoID).SingleOrDefault();
            numUnicoCongelado.StartTracking();
            numUnicoCongelado.NumeroUnicoInventario.StartTracking();
            numUnicoCongelado.NumeroUnicoInventario.InventarioCongelado -= diferenciaCongelado;
            numUnicoCongelado.NumeroUnicoInventario.InventarioDisponibleCruce = numUnicoCongelado.NumeroUnicoInventario.InventarioBuenEstado - numUnicoCongelado.NumeroUnicoInventario.InventarioCongelado;
            numUnicoCongelado.NumeroUnicoInventario.UsuarioModifica = userID;
            numUnicoCongelado.NumeroUnicoInventario.FechaModificacion = DateTime.Now;

            if (esTubo == true)
            {
                NumeroUnicoSegmento numCongeladoSegmento = numUnicoCongelado.NumeroUnicoSegmento.Where(x => x.Segmento == otm.SegmentoCongelado).SingleOrDefault();

                numCongeladoSegmento.StartTracking();

                numCongeladoSegmento.InventarioCongelado -= diferenciaCongelado;
                numCongeladoSegmento.InventarioDisponibleCruce = numCongeladoSegmento.InventarioBuenEstado - numCongeladoSegmento.InventarioCongelado;
                numUnicoCongelado.UsuarioModifica = userID;
                numUnicoCongelado.FechaModificacion = DateTime.Now;

                numUnicoCongelado.StopTracking();
                ctx.NumeroUnicoSegmento.ApplyChanges(numCongeladoSegmento);

            }

            numUnicoCongelado.NumeroUnicoInventario.StopTracking();
            ctx.NumeroUnicoInventario.ApplyChanges(numUnicoCongelado.NumeroUnicoInventario);

            numUnicoCongelado.StopTracking();
            ctx.NumeroUnico.ApplyChanges(numUnicoCongelado);

            otm.StopTracking();
            ctx.OrdenTrabajoMaterial.ApplyChanges(otm);
        }

        /// <summary>
        /// Si el material se encuentra despachado se debe regresar a inventario la diferencia de lo que se homologa.
        /// </summary>
        /// <param name="ctx">Contexto que se abrió para la transacción</param>
        /// <param name="material">MaterialPendientePorHomologar</param>
        /// <param name="tipoMaterialID">Tipo de material</param>
        /// <param name="userID">Usuario Logeado</param>
        public void MaterialCambiaCantidadDespachado(SamContext ctx, int MaterialSpoolPendienteID, int MaterialSpoolID, bool esTubo, Guid userID)
        {
            //Se modifica la cantidad despachada en OrdenTrabajoMaterial

            int diferencia = 0;

            MaterialSpoolPendiente pendiente = ctx.MaterialSpoolPendiente.Where(x => x.MaterialSpoolPendienteID == MaterialSpoolPendienteID).SingleOrDefault();

            OrdenTrabajoMaterial otm = ctx.OrdenTrabajoMaterial.Where(x => x.MaterialSpoolID == MaterialSpoolID).SingleOrDefault();
            diferencia = otm.CantidadDespachada.Value - pendiente.Cantidad;

            otm.StartTracking();
            otm.CantidadDespachada = pendiente.Cantidad;
            otm.UsuarioModifica = userID;
            otm.FechaModificacion = DateTime.Now;

            //Se regresa a inventario la cantidad sobrante 
            NumeroUnico numUnicoDespachado = ctx.NumeroUnico.Include("NumeroUnicoInventario").Include("NumeroUnicoSegmento").Where(x => x.NumeroUnicoID == otm.NumeroUnicoDespachadoID).SingleOrDefault();
            numUnicoDespachado.StartTracking();
            numUnicoDespachado.NumeroUnicoInventario.StartTracking();
            numUnicoDespachado.NumeroUnicoInventario.InventarioFisico += diferencia;
            numUnicoDespachado.NumeroUnicoInventario.InventarioBuenEstado = numUnicoDespachado.NumeroUnicoInventario.InventarioFisico - numUnicoDespachado.NumeroUnicoInventario.CantidadDanada;
            numUnicoDespachado.NumeroUnicoInventario.InventarioDisponibleCruce = numUnicoDespachado.NumeroUnicoInventario.InventarioBuenEstado - numUnicoDespachado.NumeroUnicoInventario.InventarioCongelado;
            numUnicoDespachado.NumeroUnicoInventario.UsuarioModifica = userID;
            numUnicoDespachado.NumeroUnicoInventario.FechaModificacion = DateTime.Now;

            if (esTubo)
            {
                //Al número único se le genera un segmento nuevo.
                GeneraSegmento(ctx, diferencia, otm.SegmentoDespachado.ToCharArray(0, 1)[0], numUnicoDespachado, userID);
            }

            //Se actualiza el registro del despacho
            Despacho despacho = ctx.Despacho.Where(x => x.DespachoID == otm.DespachoID).SingleOrDefault();
            despacho.StartTracking();
            despacho.Cantidad = pendiente.Cantidad;
            despacho.UsuarioModifica = userID;
            despacho.FechaModificacion = DateTime.Now;

            //Se actualiza el registro del movimiento de salida del despacho solo si es accesorio
            if (!esTubo)
            {
                NumeroUnicoMovimiento movimiento = ctx.NumeroUnicoMovimiento.Where(x => x.NumeroUnicoMovimientoID == despacho.SalidaInventarioID).SingleOrDefault();
                string referenciaCambio = string.Format("Actualización por homologación: Cantidad Anterior = {0}", movimiento.Cantidad);
                movimiento.Cantidad = pendiente.Cantidad;
                movimiento.Referencia = string.IsNullOrEmpty(movimiento.Referencia) ? referenciaCambio : movimiento.Referencia + "\n" + referenciaCambio;
                movimiento.UsuarioModifica = userID;
                movimiento.FechaModificacion = DateTime.Now;
                movimiento.StopTracking();
                ctx.NumeroUnicoMovimiento.ApplyChanges(movimiento);
            }

            despacho.StopTracking();
            ctx.Despacho.ApplyChanges(despacho);

            numUnicoDespachado.NumeroUnicoInventario.StopTracking();
            ctx.NumeroUnicoInventario.ApplyChanges(numUnicoDespachado.NumeroUnicoInventario);

            numUnicoDespachado.StopTracking();
            ctx.NumeroUnico.ApplyChanges(numUnicoDespachado);

            otm.StopTracking();
            ctx.OrdenTrabajoMaterial.ApplyChanges(otm);
        }

        /// <summary>
        /// Genera un nuevo segmento para el material que se regresa a inventario y genera un movimiento de Salida por segmentacion del numero unico principal.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="cantidad"></param>
        /// <param name="segmento"></param>
        /// <param name="numeroUnico"></param>
        /// <param name="userID"></param>
        public void GeneraSegmento(SamContext ctx, int cantidad, char segmento, NumeroUnico numeroUnico, Guid userID)
        {
            IEnumerable<NumeroUnicoSegmento> seg = ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == numeroUnico.NumeroUnicoID);
            char siguienteSegmento = (char)(((int)seg.OrderByDescending(x => x.Segmento).FirstOrDefault().Segmento.ToCharArray()[0]) + 1);
                        
            NumeroUnicoSegmento nusn = new NumeroUnicoSegmento();
            nusn.NumeroUnicoID = numeroUnico.NumeroUnicoID;
            nusn.ProyectoID = numeroUnico.ProyectoID;
            nusn.Segmento = siguienteSegmento.ToString();
            nusn.CantidadDanada = 0;
            nusn.InventarioFisico = cantidad;
            nusn.InventarioBuenEstado = nusn.InventarioFisico - nusn.CantidadDanada;
            nusn.InventarioCongelado = 0;
            nusn.InventarioDisponibleCruce = nusn.InventarioBuenEstado - nusn.InventarioCongelado;
            nusn.UsuarioModifica = userID;
            nusn.FechaModificacion = DateTime.Now;

            NumeroUnicoMovimiento numn = new NumeroUnicoMovimiento()
            {
                TipoMovimientoID = (int)TipoMovimientoEnum.EntradasSegmentacion,
                Cantidad = cantidad,
                Segmento = siguienteSegmento.ToString(),
                NumeroUnicoID = numeroUnico.NumeroUnicoID,
                ProyectoID = numeroUnico.ProyectoID,
                FechaMovimiento = DateTime.Now,
                Referencia = "Segmento regresa por Homologación",
                Estatus = "A",
                UsuarioModifica = userID,
                FechaModificacion = DateTime.Now,
            };



            numeroUnico.NumeroUnicoSegmento.Add(nusn);
            numeroUnico.NumeroUnicoMovimiento.Add(numn);
        }

        /// <summary>
        /// Cuando una junta que ya se encontraba armada y/o soldada tuvo homologación y no hay material para abastecer alguno de sus materiales se debe cortar.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="juntaSpoolID"></param>
        public void GeneraCorteJunta(SamContext ctx, int juntaSpoolID, Guid userId)
        {
            JuntaWorkstatus juntaWks = ctx.JuntaWorkstatus.Where(x => x.JuntaSpoolID == juntaSpoolID && x.JuntaFinal).SingleOrDefault();
            JuntaWorkstatusBL.Instance.Cortar(juntaWks.JuntaWorkstatusID, userId);
        }

        /// <summary>
        /// AGREO AL LISTADO DE CORTES DESDE PENDIENTE POR HOMOLOGAR 
        /// </summary>
        /// <param name="ctx">Contexto de la base de datos</param>
        /// <param name="JuntaSpoolPendienteID">Identificador de la junta en JUNSTASPOOPENDIENTE a agregar</param>
        /// <param name="userID">Identificador del Usuario que ejecuta las acciones</param>
        public void AgregoCorteSpoolDesdeCorteSpoolPendiente(SamContext ctx, int CorteSpoolPendienteID, Guid userID)
        {
            CorteSpoolPendiente corteSpoolPendiente = ctx.CorteSpoolPendiente.SingleOrDefault(p => p.CorteSpoolPendienteID == CorteSpoolPendienteID);
            CorteSpool corteSpool = new CorteSpool();
            corteSpool.StartTracking();
            corteSpool.SpoolID = corteSpoolPendiente.SpoolPendienteID;
            corteSpool.ItemCodeID = corteSpoolPendiente.ItemCodeID;
            corteSpool.TipoCorte1ID = corteSpoolPendiente.TipoCorte1ID;
            corteSpool.TipoCorte2ID = corteSpoolPendiente.TipoCorte2ID;
            corteSpool.EtiquetaMaterial = corteSpoolPendiente.EtiquetaMaterial;
            corteSpool.EtiquetaSeccion = corteSpoolPendiente.EtiquetaSeccion;
            corteSpool.Diametro = corteSpoolPendiente.Diametro;
            corteSpool.InicioFin = corteSpoolPendiente.InicioFin;
            corteSpool.Cantidad = corteSpoolPendiente.Cantidad;
            corteSpool.Observaciones = corteSpoolPendiente.Observaciones;
            corteSpool.UsuarioModifica = userID;
            corteSpool.FechaModificacion = DateTime.Now;
            corteSpool.StopTracking();
            ctx.CorteSpool.AddObject(corteSpool);
            ctx.CorteSpool.ApplyChanges(corteSpool);
            ctx.SaveChanges();
        }

        /// <summary>
        /// IGUALO AL LISTADO DE CORTES DESDE PENDIENTE POR HOMOLOGAR 
        /// </summary>
        /// <param name="ctx">Contexto de la base de datos</param>
        /// <param name="JuntaSpoolPendienteID">Identificador de la junta en JUNSTASPOOPENDIENTE a agregar</param>
        /// <param name="userID">Identificador del Usuario que ejecuta las acciones</param>
        public void IgualoCorteSpoolDesdeCorteSpoolPendiente(SamContext ctx, int CorteSpoolPendienteID, int CorteSpoolID, Guid userID)
        {
            CorteSpoolPendiente corteSpoolPendiente = ctx.CorteSpoolPendiente.SingleOrDefault(p => p.CorteSpoolPendienteID == CorteSpoolPendienteID);
            CorteSpool corteSpool = ctx.CorteSpool.SingleOrDefault(p => p.CorteSpoolID == CorteSpoolID);
            corteSpool.StartTracking();
            corteSpool.ItemCodeID = corteSpoolPendiente.ItemCodeID;
            corteSpool.TipoCorte1ID = corteSpoolPendiente.TipoCorte1ID;
            corteSpool.TipoCorte2ID = corteSpoolPendiente.TipoCorte2ID;
            corteSpool.EtiquetaMaterial = corteSpoolPendiente.EtiquetaMaterial;
            corteSpool.EtiquetaSeccion = corteSpoolPendiente.EtiquetaSeccion;
            corteSpool.Diametro = corteSpoolPendiente.Diametro;
            corteSpool.InicioFin = corteSpoolPendiente.InicioFin;
            corteSpool.Cantidad = corteSpoolPendiente.Cantidad;
            corteSpool.Observaciones = corteSpoolPendiente.Observaciones;
            corteSpool.UsuarioModifica = userID;
            corteSpool.FechaModificacion = DateTime.Now;
            corteSpool.StopTracking();
            ctx.CorteSpool.ApplyChanges(corteSpool);
            ctx.SaveChanges();
        }

        /// <summary>
        /// ELIMINO DE CORTESPOOL LOS CORTES NO EXISTES EN LA NUEVA INGENIERIA
        /// </summary>
        /// <param name="ctx">Contexto de la base de datos</param>
        /// <param name="JuntaSpoolPendienteID">Identificador de la junta en JUNSTASPOOPENDIENTE a agregar</param>
        /// <param name="userID">Identificador del Usuario que ejecuta las acciones</param>
        public void EliminoCorteSpool(SamContext ctx, int CorteSpoolID, Guid userID)
        {
            CorteSpool corteSpool = ctx.CorteSpool.SingleOrDefault(p => p.CorteSpoolID == CorteSpoolID);
            ctx.CorteSpool.DeleteObject(corteSpool);
            ctx.CorteSpool.ApplyChanges(corteSpool);
            ctx.SaveChanges();
        }

        /// <summary>
        /// RECORRE LA LISTA DE JUNTASPOOLPENDIENTE VS JUNTASPOOL Y EJECUTA LAS ACCIONES APLICABLES A CADA CASO
        /// </summary>
        /// <param name="ctx">Contexto de la base de datos</param>
        /// <param name="SpoolID">Identificador del SPOOL cuyas juntas serán homologadas</param>
        /// <param name="userID">Identificador del Usuario que ejecuta las acciones</param>
        public void HomologaCorteSpool(SamContext ctx, int SpoolID, DataTable corteSpoolTemporal, Guid userID)
        {
            /* PRIMERO COMPARO JUNTASPOOLPENDIENTE VS JUNTASPOOL */
            //BUSCO TODOS LOS REGISTROS EN JUNTASPOOLPENDIENTE DEL SPOOL EN HOMOLOGACION  
            var cortesSpoolPendiente =
                (
                    from a in ctx.CorteSpoolPendiente
                    where a.SpoolPendienteID == SpoolID
                    select a
                );

            //RECORREMOS TODOS LOS REGISTROS EN JUNTASPOOLPENDIENTE PARA VERIFICAR LA ACCION QUE DEBE REALIZARSE 
            foreach (CorteSpoolPendiente corteSpoolPendiente in cortesSpoolPendiente)
            {
                //BUSCO TODOS LOS REGISTROS EN JUNTASPOOL CON LA COMBINACION DE ETIQUETAS DE MATERIAL 1 Y 2 EN JUNTA SPOOLPENDIENTE 

                DataRow[] cortesSpoolVSPendientes = corteSpoolTemporal.Select("EtiquetaMaterial = '" + corteSpoolPendiente.EtiquetaMaterial + "' AND EtiquetaSeccion = '" + corteSpoolPendiente.EtiquetaSeccion + "' AND Diametro = " + corteSpoolPendiente.Diametro.ToString());

                if (cortesSpoolVSPendientes.Count() == 0)
                {
                    //JUNTA EN JUNTASPOOLPENDIENTE NO SE ENCUENTRA EN JUNTASPOOL, AGREGAR COMO NUEVA
                    AgregoCorteSpoolDesdeCorteSpoolPendiente(ctx, corteSpoolPendiente.CorteSpoolPendienteID, userID);
                }
                else
                {
                    if (cortesSpoolVSPendientes.Count() >= 1)
                    {
                        //ACTUALIZO ETIQUETA DE JUNTASPOOLPENDIENTE A JUNTASPOOL 
                        IgualoCorteSpoolDesdeCorteSpoolPendiente(ctx, corteSpoolPendiente.CorteSpoolPendienteID, (int)cortesSpoolVSPendientes[0]["CorteSpoolID"], userID);

                    }
                }
            }

            /* SEGUNDO COMPARO JUNTASPOOL VS JUNTASPOOLPENDIENTE*/
            //BUSCO TODOS LOS REGISTROS EN JUNTASPOOL DEL SPOOL EN HOMOLOGACION  

            DataRow[] cortesSpool = corteSpoolTemporal.Select("SpoolID = " + SpoolID.ToString());

            //RECORREMOS TODOS LOS REGISTROS EN JUNTASPOOLPENDIENTE PARA VERIFICAR LA ACCION QUE DEBE REALIZARSE 
            foreach (DataRow corteSpool in cortesSpool)
            {
                /*RESGUARDO VALORES EN VARIBLES PARA EVITAR EL PASO DIRECTO DESDE LA TABLA YA QUE GENERA UN ERROR DE TIPO 
                 LINQ to Entities does not recognize the method 'Int32 get_Item(System.String)' method, and this method cannot be translated into a store expression
                 */
                int corteSpoolID = (int)corteSpool["CorteSpoolID"];
                string etiquetaMaterial = (string)corteSpool["EtiquetaMaterial"];
                string etiquetaSeccion = (string)corteSpool["EtiquetaSeccion"];
                decimal diametro = (decimal)corteSpool["Diametro"];

                //BUSCO TODOS LOS REGISTROS EN JUNTASPOOL CON LA COMBINACION DE ETIQUETAS DE MATERIAL 1 Y 2 EN JUNTA SPOOLPENDIENTE 
                var cortesSpoolPendienteVSActuales =
                    (
                        from a in ctx.CorteSpoolPendiente
                        where a.SpoolPendienteID == SpoolID &&
                        a.EtiquetaMaterial == etiquetaMaterial &&
                        a.EtiquetaSeccion == etiquetaSeccion &&
                        a.Diametro == diametro
                        select a
                    );
                if (cortesSpoolPendienteVSActuales.Count() == 0)
                {
                    //ELIMINO LA JUNTA DE LA RELACION DE JUNTAS DEL SPOOL 
                    EliminoCorteSpool(ctx, (int)corteSpool["CorteSpoolID"], userID);
                }
                else
                {
                    if (cortesSpoolPendienteVSActuales.Count() >= 1)
                    {
                        //ACTUALIZO ETIQUETA DE JUNTASPOOLPENDIENTE A JUNTASPOOL
                        IgualoCorteSpoolDesdeCorteSpoolPendiente(ctx, cortesSpoolPendienteVSActuales.FirstOrDefault().CorteSpoolPendienteID, (int)corteSpool["CorteSpoolID"], userID);
                    }
                }
            }
        }

        /// <summary>
        /// ACTUALIZO DESDE JUNTASPOOL (CONTEXTO, NO DB) LOS MATERIALES CUYA ETIQUETA FUE CAMBIADA 
        /// </summary>
        /// <param name="ctx">Contexto de la base de datos</param>
        /// <param name="SpoolID">Identificador del Spool que se esta homologando</param>
        /// <param name="EtiquetaMaterial">Etiqueta del material que fue eliminado</param>
        /// <param name="userID">Identificador del Usuario que ejecuta las acciones</param>
        public bool EliminoJuntaSpoolXEtiquetaDesdeMaterialSpool(SamContext ctx, int SpoolID, string EtiquetaMaterial, DataTable juntasSpoolTemporal, DataTable cortesSpoolTemporal, Guid userID)
        {
            //BUSCO TODAS LAS JUNTAS DONDE SE ENCUENTRE LA ETIQUETA DEL MATERIAL 
            DataRow[] juntasSpool = juntasSpoolTemporal.Select("EtiquetaMaterial1 = '" + EtiquetaMaterial + "' OR EtiquetaMaterial2 = '" + EtiquetaMaterial + "'");

            //RECORRO CADA JUNTA QUE CONTIENE EL MATERIAL 
            foreach (DataRow juntaSpool in juntasSpool)
            {
                int juntaSpoolID = (int)juntaSpool["JuntaSpoolID"];

               
                //ELIMINO EL WORKSTATUS 
                EliminoJuntaWorkstatus(ctx, juntaSpoolID);

                //ELIMINO LA ORDEN DE TRABAJO  DE LA JUNTA
                OrdenTrabajoJunta ordenTrabajoJunta = ctx.OrdenTrabajoJunta.SingleOrDefault(p => p.JuntaSpoolID == juntaSpoolID);
                if (ordenTrabajoJunta != null)
                {
                    ctx.OrdenTrabajoJunta.DeleteObject(ordenTrabajoJunta);
                    ctx.OrdenTrabajoJunta.ApplyChanges(ordenTrabajoJunta);
                }
                
            }

            ctx.SaveChanges();
            return true;
        }

        /// <summary>
        /// AGREO AL LISTADO DE JUNTAS DESDE PENDIENTE POR HOMOLOGAR 
        /// </summary>
        /// <param name="ctx">Contexto de la base de datos</param>
        /// <param name="JuntaSpoolPendienteID">Identificador de la junta en JUNSTASPOOPENDIENTE a agregar</param>
        /// <param name="userID">Identificador del Usuario que ejecuta las acciones</param>
        public void AgregoJuntaSpoolDesdeJuntaSpoolPendiente(SamContext ctx, int JuntaSpoolPendienteID, DataTable juntasSpool, Guid userID)
        {
            JuntaSpoolPendiente juntaSpoolPendiente = ctx.JuntaSpoolPendiente.SingleOrDefault(p => p.JuntaSpoolPendienteID == JuntaSpoolPendienteID);

            VerificaNoExistanEtiquetas(ctx, juntaSpoolPendiente, juntasSpool);

            JuntaSpool juntaSpool = new JuntaSpool();
            juntaSpool.StartTracking();
            juntaSpool.SpoolID = juntaSpoolPendiente.SpoolPendienteID;
            juntaSpool.TipoJuntaID = juntaSpoolPendiente.TipoJuntaID;
            juntaSpool.FabAreaID = juntaSpoolPendiente.FabAreaID;
            juntaSpool.Etiqueta = juntaSpoolPendiente.Etiqueta;
            juntaSpool.EtiquetaMaterial1 = juntaSpoolPendiente.EtiquetaMaterial1;
            juntaSpool.EtiquetaMaterial2 = juntaSpoolPendiente.EtiquetaMaterial2;
            juntaSpool.Cedula = juntaSpoolPendiente.Cedula;
            juntaSpool.FamiliaAceroMaterial1ID = juntaSpoolPendiente.FamiliaAceroMaterial1ID;
            juntaSpool.FamiliaAceroMaterial2ID = juntaSpoolPendiente.FamiliaAceroMaterial2ID;
            juntaSpool.Diametro = juntaSpoolPendiente.Diametro;
            juntaSpool.Espesor = juntaSpoolPendiente.Espesor;
            juntaSpool.KgTeoricos = juntaSpoolPendiente.KgTeoricos;
            juntaSpool.Peqs = juntaSpoolPendiente.Peqs;
            juntaSpool.UsuarioModifica = userID;
            juntaSpool.FechaModificacion = DateTime.Now;
            juntaSpool.StopTracking();
            ctx.JuntaSpool.AddObject(juntaSpool);
            ctx.JuntaSpool.ApplyChanges(juntaSpool);

            //Guardo los cambios
            ctx.SaveChanges();
        }

        /// <summary>
        /// CUANDO UNA JUNTA EN JUNTASPOOL COINCIDE EN AMBAS ETIQUETAS, LA CONSIDERO COMO UNA MISMA JUNTO Y SE HOMOLOGA CON LA JUNTA DE PENDIENTE 
        /// </summary>
        /// <param name="ctx">Contexto de la base de datos</param>
        /// <param name="JuntaSpoolPendienteID">Identificador de la junta en JUNSTASPOOPENDIENTE</param>
        /// <param name="JuntaSpoolID">Identificador de la junta en JUNSTASPOOL a hollogar</param>
        /// <param name="userID">Identificador del Usuario que ejecuta las acciones</param>
        public void IgualoJuntaSpoolDesdeJuntaSpoolPendiente(SamContext ctx, int JuntaSpoolPendienteID, int JuntaSpoolID, DataTable juntasSpool, Guid userID)
        {
            JuntaSpoolPendiente juntaSpoolPendiente = ctx.JuntaSpoolPendiente.SingleOrDefault(p => p.JuntaSpoolPendienteID == JuntaSpoolPendienteID);
            JuntaSpool juntaSpool = ctx.JuntaSpool.SingleOrDefault(p => p.JuntaSpoolID == JuntaSpoolID);

            var juntasWorkStatus =
                (
                    from a in ctx.JuntaWorkstatus
                    where a.JuntaSpoolID == JuntaSpoolID
                    select a
                );
            foreach (JuntaWorkstatus juntaWorkStatus in juntasWorkStatus)
            {
                if (juntaSpool.Etiqueta == juntaWorkStatus.EtiquetaJunta.Substring(0, juntaSpool.Etiqueta.Length))
                {
                    juntaWorkStatus.StartTracking();
                    juntaWorkStatus.EtiquetaJunta = juntaSpoolPendiente.Etiqueta + juntaWorkStatus.EtiquetaJunta.Substring(juntaSpool.Etiqueta.Length, juntaWorkStatus.EtiquetaJunta.Length - juntaSpool.Etiqueta.Length);
                    juntaWorkStatus.StopTracking();
                    ctx.JuntaWorkstatus.ApplyChanges(juntaWorkStatus);
                }
                else
                {
                    throw new Exception(MensajesError.Excepcion_EtiquetasWorkstatus.ToString());
                }
            }

            VerificaNoExistanEtiquetas(ctx, juntaSpoolPendiente, juntasSpool);

            juntaSpool.StartTracking();
            juntaSpool.TipoJuntaID = juntaSpoolPendiente.TipoJuntaID;
            juntaSpool.FabAreaID = juntaSpoolPendiente.FabAreaID;
            juntaSpool.Etiqueta = juntaSpoolPendiente.Etiqueta;
            juntaSpool.EtiquetaMaterial1 = juntaSpoolPendiente.EtiquetaMaterial1;
            juntaSpool.EtiquetaMaterial2 = juntaSpoolPendiente.EtiquetaMaterial2;
            juntaSpool.Cedula = juntaSpoolPendiente.Cedula;
            juntaSpool.FamiliaAceroMaterial1ID = juntaSpoolPendiente.FamiliaAceroMaterial1ID;
            juntaSpool.FamiliaAceroMaterial2ID = juntaSpoolPendiente.FamiliaAceroMaterial2ID;
            juntaSpool.Diametro = juntaSpoolPendiente.Diametro;
            juntaSpool.Espesor = juntaSpoolPendiente.Espesor;
            juntaSpool.KgTeoricos = juntaSpoolPendiente.KgTeoricos;
            juntaSpool.Peqs = juntaSpoolPendiente.Peqs;
            juntaSpool.UsuarioModifica = userID;
            juntaSpool.FechaModificacion = DateTime.Now;
            juntaSpool.StopTracking();
            ctx.JuntaSpool.ApplyChanges(juntaSpool);

            ctx.SaveChanges();
        }

        private void VerificaNoExistanEtiquetas(SamContext ctx, JuntaSpoolPendiente jsp, DataTable juntaspoolTable)
        {
            DataRow[] juntaspool = juntaspoolTable.Select("Etiqueta = '" + jsp.Etiqueta + "' AND SpoolID = '" + jsp.SpoolPendienteID + "'");
            
            //JuntaSpool  = ctx.JuntaSpool.SingleOrDefault(x => x.Etiqueta == jsp.Etiqueta && x.SpoolID == jsp.SpoolPendienteID);
            if (juntaspool.Count() > 0)
            {
                int juntaspoolID = (int)juntaspool[0]["JuntaSpoolID"];
                string etiqueta1 = juntaspool[0]["EtiquetaMaterial1"].ToString();
                string etiqueta2 = juntaspool[0]["EtiquetaMaterial2"].ToString();

                JuntaSpool juntaSpoolAmodificar = ctx.JuntaSpool.Where(x => x.JuntaSpoolID == juntaspoolID).SingleOrDefault();

                JuntaSpoolPendiente juntaspoolpendiente = ctx.JuntaSpoolPendiente.SingleOrDefault(x => x.SpoolPendienteID == jsp.SpoolPendienteID && x.EtiquetaMaterial1 == etiqueta1 && x.EtiquetaMaterial2 == etiqueta2);
                if (juntaspoolpendiente != jsp)
                {
                    if (juntaspoolpendiente != null)
                    {
                        if (ctx.JuntaSpool.Any(x => x.Etiqueta == juntaspoolpendiente.Etiqueta && x.SpoolID == juntaspoolpendiente.SpoolPendienteID))
                        {
                            VerificaNoExistanEtiquetas(ctx, juntaspoolpendiente, juntaspoolTable);
                        }

                        List<JuntaWorkstatus> jws = ctx.JuntaWorkstatus.Where(x => x.JuntaSpoolID == juntaSpoolAmodificar.JuntaSpoolID).ToList();
                        jws.ForEach(x =>
                        {
                            x.StartTracking();
                            x.EtiquetaJunta = juntaspoolpendiente.Etiqueta + x.EtiquetaJunta.Substring(juntaSpoolAmodificar.Etiqueta.Length, x.EtiquetaJunta.Length - juntaSpoolAmodificar.Etiqueta.Length);
                            x.StopTracking();
                            ctx.JuntaWorkstatus.ApplyChanges(x);
                        });

                        juntaSpoolAmodificar.StartTracking();
                        juntaSpoolAmodificar.Etiqueta = juntaspoolpendiente.Etiqueta;
                        juntaSpoolAmodificar.StopTracking();
                        ctx.JuntaSpool.ApplyChanges(juntaSpoolAmodificar);
                        ctx.SaveChanges();

                    }
                    else
                    {
                        if (juntaSpoolAmodificar != null)
                        {
                            //ELIMINO AGRUPADORES POR JUNTA
                            EliminoAgrupadoresPorJunta(ctx, juntaSpoolAmodificar.JuntaSpoolID);

                            //ELIMINO EL WORKSTATUS DE LA JUNTA 
                            EliminoJuntaWorkstatus(ctx, juntaSpoolAmodificar.JuntaSpoolID);

                            //ELIMINO LA JUNTA DE LA RELACION DE JUNTAS DEL SPOOL 
                            EliminoJuntaSpool(ctx, juntaSpoolAmodificar.JuntaSpoolID);
                            //ctx.JuntaSpool.DeleteObject(juntaSpoolAmodificar);
                            ctx.SaveChanges();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ELIMINO DE JUNTAWORKSTATUS LAS JUNTAS QUE YA NO PRESENTAN ESTATUS DE ARMADO 
        /// </summary>
        /// <param name="ctx">Contexto de la base de datos</param>
        /// <param name="JuntaSpoolID">Identificador de la junta en JUNSTASPOOL a eliminar</param>
        /// <param name="userID">Identificador del Usuario que ejecuta las acciones</param>
        public void EliminoJuntaWorkstatus(SamContext ctx, int juntaSpoolID)
        {
            IEnumerable<int> juntaWorkstatusIDs = ctx.JuntaWorkstatus.Where(x => x.JuntaSpoolID == juntaSpoolID).Select(x => x.JuntaWorkstatusID);

            List<JuntaWorkstatus> juntaWorkstatus = ctx.JuntaWorkstatus.Where(x => juntaWorkstatusIDs.Contains(x.JuntaWorkstatusID)).ToList();

            //Elimino las relaciones.
            juntaWorkstatus.ForEach(x =>
            {
                x.JuntaArmadoID = null;
                x.JuntaSoldaduraID = null;
                x.JuntaInspeccionVisualID = null;
                x.JuntaWorkstatusAnteriorID = null;
            });
            juntaWorkstatus.ForEach(x => ctx.JuntaWorkstatus.ApplyChanges(x));
            ctx.SaveChanges();

            // Elimino todos los armados que pudieran existir
            List<JuntaArmado> juntaArmado = ctx.JuntaArmado.Where(x => juntaWorkstatusIDs.Contains(x.JuntaWorkstatusID)).ToList();
            juntaArmado.ForEach(x => ctx.JuntaArmado.DeleteObject(x));
            ctx.SaveChanges();

            // Elimino todos los soldados que pudieran existir
            List<JuntaSoldadura> juntaSoldadura = ctx.JuntaSoldadura.Where(x => juntaWorkstatusIDs.Contains(x.JuntaWorkstatusID)).ToList();
            IEnumerable<int> soldaduraIDs = juntaSoldadura.Select(x => x.JuntaSoldaduraID);
            List<JuntaSoldaduraDetalle> juntaSoldaduraDet = ctx.JuntaSoldaduraDetalle.Where(x => soldaduraIDs.Contains(x.JuntaSoldaduraID)).ToList();
            juntaSoldaduraDet.ForEach(x => ctx.JuntaSoldaduraDetalle.DeleteObject(x));
            ctx.SaveChanges();
            juntaSoldadura.ForEach(x => ctx.JuntaSoldadura.DeleteObject(x));
            ctx.SaveChanges();

            //Elimino todas las inspecciones visuales
            List<JuntaInspeccionVisual> jtaInsVis = ctx.JuntaInspeccionVisual.Where(x => juntaWorkstatusIDs.Contains(x.JuntaWorkstatusID)).ToList();
            IEnumerable<int> insVisIDs = jtaInsVis.Select(x => x.JuntaInspeccionVisualID);
            List<JuntaInspeccionVisualDefecto> jtaInsVisDef = ctx.JuntaInspeccionVisualDefecto.Where(x => insVisIDs.Contains(x.JuntaInspeccionVisualID)).ToList();
            jtaInsVisDef.ForEach(x => ctx.JuntaInspeccionVisualDefecto.DeleteObject(x));
            ctx.SaveChanges();
            jtaInsVis.ForEach(x => ctx.JuntaInspeccionVisual.DeleteObject(x));
            ctx.SaveChanges();

            List<InspeccionVisualPatio> jtaInsVisPatio = ctx.InspeccionVisualPatio.Where(x => juntaWorkstatusIDs.Contains(x.JuntaWorkstatusID)).ToList();
            IEnumerable<int> insVisPatioIDs = jtaInsVisPatio.Select(x => x.InspeccionVisualPatioID);
            List<InspeccionVisualPatioDefecto> jtaInsVisDefPatio = ctx.InspeccionVisualPatioDefecto.Where(x => insVisPatioIDs.Contains(x.InspeccionVisualPatioID)).ToList();
            jtaInsVisDefPatio.ForEach(x => ctx.InspeccionVisualPatioDefecto.DeleteObject(x));
            ctx.SaveChanges();
            jtaInsVisPatio.ForEach(x => ctx.InspeccionVisualPatio.DeleteObject(x));
            ctx.SaveChanges();

            //Elimino todos los reportes PND
            List<JuntaReportePnd> jtaPND = ctx.JuntaReportePnd.Where(x => juntaWorkstatusIDs.Contains(x.JuntaWorkstatusID)).ToList();
            IEnumerable<int> pndIDs = jtaPND.Select(x => x.JuntaReportePndID);
            List<JuntaReportePndCuadrante> jtaCuadrante = ctx.JuntaReportePndCuadrante.Where(x => pndIDs.Contains(x.JuntaReportePndID)).ToList();
            List<JuntaReportePndSector> jtaSector = ctx.JuntaReportePndSector.Where(x => pndIDs.Contains(x.JuntaReportePndID)).ToList();
            jtaCuadrante.ForEach(x => ctx.JuntaReportePndCuadrante.DeleteObject(x));
            jtaSector.ForEach(x => ctx.JuntaReportePndSector.DeleteObject(x));
            ctx.SaveChanges();
            jtaPND.ForEach(x => ctx.JuntaReportePnd.DeleteObject(x));

            //Elimino todos los reportes TT
            List<JuntaReporteTt> jtaTT = ctx.JuntaReporteTt.Where(x => juntaWorkstatusIDs.Contains(x.JuntaWorkstatusID)).ToList();
            jtaTT.ForEach(x => ctx.JuntaReporteTt.DeleteObject(x));

            //Elimino todas las requisiciones
            List<JuntaRequisicion> jtaReq = ctx.JuntaRequisicion.Where(x => juntaWorkstatusIDs.Contains(x.JuntaWorkstatusID)).ToList();
            jtaReq.ForEach(x => ctx.JuntaRequisicion.DeleteObject(x));

            //Elimino Estimaciones
            List<EstimacionJunta> estJta = ctx.EstimacionJunta.Where(x => juntaWorkstatusIDs.Contains(x.JuntaWorkstatusID)).ToList();
            estJta.ForEach(x => ctx.EstimacionJunta.DeleteObject(x));

            //Elimino Destajos
            List<DestajoTuberoDetalle> destajo = ctx.DestajoTuberoDetalle.Where(x => juntaWorkstatusIDs.Contains(x.JuntaWorkstatusID)).ToList();
            destajo.ForEach(x => ctx.DestajoTuberoDetalle.DeleteObject(x));

            List<DestajoSoldadorDetalle> destajosSoldador = ctx.DestajoSoldadorDetalle.Where(x => juntaWorkstatusIDs.Contains(x.JuntaWorkstatusID)).ToList();
            destajosSoldador.ForEach(x => ctx.DestajoSoldadorDetalle.DeleteObject(x));

            ctx.SaveChanges();

            //ELIMINO TODOS LOS REGISTROS DE JUNTA WORKSTATUS
            foreach (JuntaWorkstatus juntaWks in juntaWorkstatus)
            {
                ctx.JuntaWorkstatus.DeleteObject(juntaWks);
                ctx.JuntaWorkstatus.ApplyChanges(juntaWks);
            }

            ctx.SaveChanges();
        }

        /// <summary>
        /// ELIMINO DE AGRUPADORESPORJUNTA LAS JUNTAS CUYOS MATERIALES DESAPAARECIERON DE LA HOMOLOGACION Y QUE NO TIENEN ORDEN DE TRABAJO
        /// </summary>
        /// <param name="ctx">Contexto de la base de datos</param>
        /// <param name="JuntaSpoolID">Identificador de la junta en JUNSTASPOOL a eliminar</param>
        /// <param name="userID">Identificador del Usuario que ejecuta las acciones</param>
        public void EliminoAgrupadoresPorJunta(SamContext ctx, int JuntaSpoolID)
        {
            //Elimino Agrupadores por junta           
            List<AgrupadoresPorJunta> agrupadoresporjunta = ctx.AgrupadoresPorJunta.Where(x => x.JuntaSpoolID == JuntaSpoolID).ToList();
            agrupadoresporjunta.ForEach(x => ctx.AgrupadoresPorJunta.DeleteObject(x));

            ctx.SaveChanges();
        }
      
        /// <summary>
        /// ELIMINO DE JUNTASPOOL LAS JUNTAS CUYOS MATERIALES DESAPAARECIERON DE LA HOMOLOGACION Y QUE NO TIENEN ORDEN DE TRABAJO
        /// </summary>
        /// <param name="ctx">Contexto de la base de datos</param>
        /// <param name="JuntaSpoolID">Identificador de la junta en JUNSTASPOOL a eliminar</param>
        /// <param name="userID">Identificador del Usuario que ejecuta las acciones</param>
        public void EliminoJuntaSpool(SamContext ctx, int JuntaSpoolID)
        {
            JuntaSpool juntaSpool = ctx.JuntaSpool.SingleOrDefault(p => p.JuntaSpoolID == JuntaSpoolID);
            if (juntaSpool != null)
            {
                OrdenTrabajoJunta ordenTrabajoJunta = ctx.OrdenTrabajoJunta.SingleOrDefault(p => p.JuntaSpoolID == juntaSpool.JuntaSpoolID);
                if (ordenTrabajoJunta != null)
                {
                    ctx.OrdenTrabajoJunta.DeleteObject(ordenTrabajoJunta);
                    ctx.OrdenTrabajoJunta.ApplyChanges(ordenTrabajoJunta);
                }

                BastonSpoolJunta bastonSpoolJunta = ctx.BastonSpoolJunta.SingleOrDefault(x => x.JuntaSpoolID == juntaSpool.JuntaSpoolID);
                if (bastonSpoolJunta != null)
                {
                    ctx.BastonSpoolJunta.DeleteObject(bastonSpoolJunta);
                    ctx.BastonSpoolJunta.ApplyChanges(bastonSpoolJunta);
                }

                ////ELIMINAR LOS REGISTROS DE AGRUPADORES POR JUNTA ANTES DE ELIMINAR LAS JUNTAS
                //List<AgrupadoresPorJunta> agrupadores = ctx.AgrupadoresPorJunta.Where(x => x.JuntaSpoolID == juntaSpool.JuntaSpoolID).ToList();
                //agrupadores.ForEach(x => ctx.DeleteObject(x));

                ctx.JuntaSpool.DeleteObject(juntaSpool);
                ctx.JuntaSpool.ApplyChanges(juntaSpool);

                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// RECORRE LA LISTA DE JUNTASPOOLPENDIENTE VS JUNTASPOOL Y EJECUTA LAS ACCIONES APLICABLES A CADA CASO
        /// </summary>
        /// <param name="ctx">Contexto de la base de datos</param>
        /// <param name="SpoolID">Identificador del SPOOL cuyas juntas serán homologadas</param>
        /// <param name="userID">Identificador del Usuario que ejecuta las acciones</param>
        public void HomologaJuntaSpool(SamContext ctx, int SpoolID, DataTable juntasSpoolTemporal, Guid userID)
        {
            /* PRIMERO COMPARO JUNTASPOOLPENDIENTE VS JUNTASPOOL */
            //BUSCO TODOS LOS REGISTROS EN JUNTASPOOLPENDIENTE DEL SPOOL EN HOMOLOGACION  
            var juntasSpoolPendiente =
                (
                    from a in ctx.JuntaSpoolPendiente
                    where a.SpoolPendienteID == SpoolID
                    select a
                );
            //RECORREMOS TODOS LOS REGISTROS EN JUNTASPOOLPENDIENTE PARA VERIFICAR LA ACCION QUE DEBE REALIZARSE 
            foreach (JuntaSpoolPendiente juntaSpoolPendiente in juntasSpoolPendiente)
            {
                //BUSCO TODOS LOS REGISTROS EN JUNTASPOOL CON LA COMBINACION DE ETIQUETAS DE MATERIAL 1 Y 2 EN JUNTA SPOOLPENDIENTE 
                DataRow[] juntasSpoolVSPendientes = juntasSpoolTemporal.Select("EtiquetaMaterial1 = '" + juntaSpoolPendiente.EtiquetaMaterial1 + "' AND EtiquetaMaterial2 = '" + juntaSpoolPendiente.EtiquetaMaterial2 + "'");

                if (juntasSpoolVSPendientes.Count() == 0)
                {
                    //JUNTA EN JUNTASPOOLPENDIENTE NO SE ENCUENTRA EN JUNTASPOOL, AGREGAR COMO NUEVA
                    AgregoJuntaSpoolDesdeJuntaSpoolPendiente(ctx, juntaSpoolPendiente.JuntaSpoolPendienteID, juntasSpoolTemporal, userID);
                }
                else
                {
                    if (juntasSpoolVSPendientes.Count() == 1)
                    {
                        //ACTUALIZO ETIQUETA DE JUNTASPOOLPENDIENTE A JUNTASPOOL 
                        IgualoJuntaSpoolDesdeJuntaSpoolPendiente(ctx, juntaSpoolPendiente.JuntaSpoolPendienteID, (int)juntasSpoolVSPendientes[0]["JuntaSpoolID"], juntasSpoolTemporal, userID);
                    }
                    else
                    {
                        //CONFLICTO, EXISTE MAS DE UN REGISITRO EN JUNTASPOOL CON ESA COMBINACION DE ETIQUETAS
                        //DE NO EXISTIR UN MECANISMO DE SOLUCION, DEVOLVER EL PROCESO COMO NO PERMITIDO 
                        throw new Exception(MensajesError.Excepcion_CombinacionEtiquetasRepetida.ToString());
                    }
                }
            }

            /* SEGUNDO COMPARO JUNTASPOOL VS JUNTASPOOLPENDIENTE*/
            //BUSCO TODOS LOS REGISTROS EN JUNTASPOOL DEL SPOOL EN HOMOLOGACION  

            DataRow[] juntasSpool = juntasSpoolTemporal.Select("SpoolID = " + SpoolID.ToString());

            //RECORREMOS TODOS LOS REGISTROS EN JUNTASPOOLPENDIENTE PARA VERIFICAR LA ACCION QUE DEBE REALIZARSE 
            foreach (DataRow juntaSpool in juntasSpool)
            {
                /*RESGUARDO VALORES EN VARIBLES PARA EVITAR EL PASO DIRECTO DESDE LA TABLA YA QUE GENERA UN ERROR DE TIPO 
                 LINQ to Entities does not recognize the method 'Int32 get_Item(System.String)' method, and this method cannot be translated into a store expression
                 */
                int juntaSpoolID = (int)juntaSpool["JuntaSpoolID"];

                string EtiquetaMaterial1 = (string)juntaSpool["EtiquetaMaterial1"];
                string EtiquetaMaterial2 = (string)juntaSpool["EtiquetaMaterial2"];

                //BUSCO TODOS LOS REGISTROS EN JUNTASPOOL CON LA COMBINACION DE ETIQUETAS DE MATERIAL 1 Y 2 EN JUNTA SPOOLPENDIENTE 
                var juntasSpoolPendienteVSActuales =
                    (
                        from a in ctx.JuntaSpoolPendiente
                        where a.SpoolPendienteID == SpoolID &&
                        a.EtiquetaMaterial1 == EtiquetaMaterial1 &&
                        a.EtiquetaMaterial2 == EtiquetaMaterial2
                        select a
                    );
                if (juntasSpoolPendienteVSActuales.Count() == 0)
                {
                        //ElIMINO AGRUPADORES POR JUNTA
                        EliminoAgrupadoresPorJunta(ctx, juntaSpoolID);

                        //ELIMINO EL WORKSTATUS DE LA JUNTA 
                        EliminoJuntaWorkstatus(ctx, juntaSpoolID);

                        //ELIMINO LA JUNTA DE LA RELACION DE JUNTAS DEL SPOOL 
                        EliminoJuntaSpool(ctx, (int)juntaSpool["JuntaSpoolID"]);
                    
                }
                else
                {
                    if (juntasSpoolPendienteVSActuales.Count() == 1)
                    {
                        //ACTUALIZO ETIQUETA DE JUNTASPOOLPENDIENTE A JUNTASPOOL
                        IgualoJuntaSpoolDesdeJuntaSpoolPendiente(ctx, juntasSpoolPendienteVSActuales.FirstOrDefault().JuntaSpoolPendienteID, (int)juntaSpool["JuntaSpoolID"], juntasSpoolTemporal, userID);
                    }
                    else
                    {
                        //CONFLICTO, EXISTE MAS DE UN REGISITRO EN JUNTASPOOLPENDIENTE CON ESA COMBINACION DE ETIQUETAS
                        //DE NO EXISTIR UN MECANISMO DE SOLUCION, DEVOLVER EL PROCESO COMO NO PERMITIDO 
                        throw new Exception(MensajesError.Excepcion_CombinacionEtiquetasRepetida.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// DEVUELVE MATERIAL CONGELADO EN VASE A SU ORDEN DE TRABAJO
        /// </summary>
        /// <param name="ctx">Contexto de la base de datos</param>
        /// <param name="MaterialSpoolPendienteID">Material de pendiente que sera removido</param>
        /// <param name="userID">Identificador del Usuario que ejecuta las acciones</param>
        public void EliminaMaterialSpoolPendiente(SamContext ctx, int MaterialSpoolPendienteID, Guid userID)
        {
            MaterialSpoolPendiente materialSpoolPendiente = ctx.MaterialSpoolPendiente.SingleOrDefault(p => p.MaterialSpoolPendienteID == MaterialSpoolPendienteID);
            ctx.MaterialSpoolPendiente.DeleteObject(materialSpoolPendiente);
            ctx.MaterialSpoolPendiente.ApplyChanges(materialSpoolPendiente);
        }

        /// <summary>
        /// DEVUELVE MATERIAL CONGELADO EN VASE A SU ORDEN DE TRABAJO
        /// </summary>
        /// <param name="ctx">Contexto de la base de datos</param>
        /// <param name="NumeroUnicoACongelarID">Identificador del MaterialSpool unico que se va a congelarr</param>
        /// <param name="CantidadCongelada">Cantidad que ser congelada de inventario</param>
        /// <param name="EsTubo">Identifico si el material es tubo o accesorio</param>
        /// <param name="SegmentoCongelado">Si es un tubo, identifico el segmento que se ira a congelar</param>
        /// <param name="userID">Identificador del Usuario que ejecuta las acciones</param>
        public void DevuelveMaterialCongeladoPorOrdenTrabajoMaterial(SamContext ctx, int NumeroUnicoCongeladoID, int CantidadCongelada, bool EsTubo, string SegmentoCongelado, Guid userID)
        {
            //BUSCO EL NUMERO UNICO QUE CORRESPONDE AL MATERIAL CONGELADO 
            NumeroUnicoInventario numeroUnicoInventario = ctx.NumeroUnicoInventario.SingleOrDefault(p => p.NumeroUnicoID == NumeroUnicoCongeladoID);
            numeroUnicoInventario.StartTracking();
            //RETIRO DEL INVENTARIO CONGELADO LA CANTIDAD CONGELADA POR LA ORDEN DE TRABAJO 
            numeroUnicoInventario.InventarioCongelado -= CantidadCongelada;
            //DEVUELVO AL INVENTARIO DISPONIBLE LA CANTIDAD CONGELADA
            numeroUnicoInventario.InventarioDisponibleCruce = numeroUnicoInventario.InventarioBuenEstado - numeroUnicoInventario.InventarioCongelado;
            if (EsTubo == true)
            {
                //SI ES UN TUBO, BUSCO EL NUMERO UNICO DEL SEGMENTO QUE CORRESPONDE AL NUMERO UNICO 
                NumeroUnicoSegmento numeroUnicoSegmento = ctx.NumeroUnicoSegmento.SingleOrDefault(p => p.NumeroUnicoID == NumeroUnicoCongeladoID && p.Segmento == SegmentoCongelado);
                numeroUnicoSegmento.StartTracking();
                //RETIRO DEL INVENTARIO CONGELADO DEL SEGMENTO LA CANTIDAD CONGELADA POR LA ORDEN DE TRABAJO 
                numeroUnicoSegmento.InventarioCongelado -= CantidadCongelada;
                //DEVUELVO AL INVENTARIO DISPONIBLE DEL SEGMENTO LA CANTIDAD CONGELADA
                numeroUnicoSegmento.InventarioDisponibleCruce = numeroUnicoSegmento.InventarioBuenEstado - numeroUnicoSegmento.InventarioCongelado;
                numeroUnicoSegmento.StopTracking();
                ctx.NumeroUnicoSegmento.ApplyChanges(numeroUnicoSegmento);
            }
            numeroUnicoInventario.StopTracking();
            ctx.NumeroUnicoInventario.ApplyChanges(numeroUnicoInventario);
            ctx.SaveChanges();
        }


        /// <summary>
        /// DEVUELVE MATERIAL CONGELADO EN VASE A SU ORDEN DE TRABAJO
        /// </summary>
        /// <param name="ctx">Contexto de la base de datos</param>
        /// <param name="NumeroUnicoACongelarID">Identificador del MaterialSpool unico que se va a congelarr</param>
        /// <param name="CantidadCongelada">Cantidad que ser congelada de inventario</param>
        /// <param name="EsTubo">Identifico si el material es tubo o accesorio</param>
        /// <param name="SegmentoCongelado">Si es un tubo, identifico el segmento que se ira a congelar</param>
        /// <param name="userID">Identificador del Usuario que ejecuta las acciones</param>
        public void DevuelveMaterialCongeladoParcialmente(SamContext ctx, int NumeroUnicoCongeladoID, int CantidadCongelada, bool EsTubo, string SegmentoCongelado, Guid userID)
        {
            //BUSCO EL NUMERO UNICO QUE CORRESPONDE AL MATERIAL CONGELADO 
            NumeroUnicoInventario numeroUnicoInventario = ctx.NumeroUnicoInventario.SingleOrDefault(p => p.NumeroUnicoID == NumeroUnicoCongeladoID);
            numeroUnicoInventario.StartTracking();
            //RETIRO DEL INVENTARIO CONGELADO LA CANTIDAD CONGELADA POR LA ORDEN DE TRABAJO 
            numeroUnicoInventario.InventarioCongelado -= CantidadCongelada;
            //DEVUELVO AL INVENTARIO DISPONIBLE LA CANTIDAD CONGELADA
            numeroUnicoInventario.InventarioDisponibleCruce = numeroUnicoInventario.InventarioBuenEstado - numeroUnicoInventario.InventarioCongelado;
            if (EsTubo == true)
            {
                //SI ES UN TUBO, BUSCO EL NUMERO UNICO DEL SEGMENTO QUE CORRESPONDE AL NUMERO UNICO 
                NumeroUnicoSegmento numeroUnicoSegmento = ctx.NumeroUnicoSegmento.SingleOrDefault(p => p.NumeroUnicoID == NumeroUnicoCongeladoID && p.Segmento == SegmentoCongelado);
                numeroUnicoSegmento.StartTracking();
                //RETIRO DEL INVENTARIO CONGELADO DEL SEGMENTO LA CANTIDAD CONGELADA POR LA ORDEN DE TRABAJO 
                numeroUnicoSegmento.InventarioCongelado -= CantidadCongelada;
                //DEVUELVO AL INVENTARIO DISPONIBLE DEL SEGMENTO LA CANTIDAD CONGELADA
                numeroUnicoSegmento.InventarioDisponibleCruce = numeroUnicoSegmento.InventarioBuenEstado - numeroUnicoSegmento.InventarioCongelado;
                numeroUnicoSegmento.StopTracking();
                ctx.NumeroUnicoSegmento.ApplyChanges(numeroUnicoSegmento);
            }
            numeroUnicoInventario.StopTracking();
            ctx.NumeroUnicoInventario.ApplyChanges(numeroUnicoInventario);
            ctx.SaveChanges();
        }


        /// <summary>
        /// CONGELO UN MATERIAL AGREGANDO SU ORDEN DE TRABAJO
        /// </summary>
        /// <param name="ctx">Contexto de la base de datos</param>
        /// <param name="MaterialSpoolID">Identificador del MaterialSpool a homologar</param>
        /// <param name="NumeroUnicoACongelarID">Identificador del MaterialSpool unico que se va a congelarr</param>
        /// <param name="EsEquivalente">Identifico si el item code es un equivalente por falta de existencias</param>
        /// <param name="EsTubo">Identifico si el material es tubo o accesorio</param>
        /// <param name="SegmentoACongelar">Si es un tubo, identifico el segmento que se ira a congelar</param>
        /// <param name="userID">Identificador del Usuario que ejecuta las acciones</param>
        public void CongeloMaterialConOrdenTrabajoMaterial(SamContext ctx, int SpoolID, int MaterialSpoolID, int NumeroUnicoACongelarID, bool EsEquivalente, int CantidadACongelar, bool EsTubo, string SegmentoACongelar, Guid userID)
        {
            //BUSCO EL NUMERO UNICO QUE CORRESPONDE AL MATERIAL CONGELADO 
            NumeroUnicoInventario numeroUnicoInventario = ctx.NumeroUnicoInventario.SingleOrDefault(p => p.NumeroUnicoID == NumeroUnicoACongelarID);
            numeroUnicoInventario.StartTracking();
            //RETIRO DEL INVENTARIO CONGELADO LA CANTIDAD CONGELADA POR LA ORDEN DE TRABAJO 
            numeroUnicoInventario.InventarioCongelado += CantidadACongelar;
            //DEVUELVO AL INVENTARIO DISPONIBLE LA CANTIDAD CONGELADA
            numeroUnicoInventario.InventarioDisponibleCruce = numeroUnicoInventario.InventarioBuenEstado - numeroUnicoInventario.InventarioCongelado;
            if (EsTubo == true)
            {
                //SI ES UN TUBO, BUSCO EL NUMERO UNICO DEL SEGMENTO QUE CORRESPONDE AL NUMERO UNICO 
                NumeroUnicoSegmento numeroUnicoSegmento = ctx.NumeroUnicoSegmento.SingleOrDefault(p => p.NumeroUnicoID == NumeroUnicoACongelarID && p.Segmento == SegmentoACongelar);
                numeroUnicoSegmento.StartTracking();
                //RETIRO DEL INVENTARIO CONGELADO DEL SEGMENTO LA CANTIDAD CONGELADA POR LA ORDEN DE TRABAJO 
                numeroUnicoSegmento.InventarioCongelado += CantidadACongelar;
                //DEVUELVO AL INVENTARIO DISPONIBLE DEL SEGMENTO LA CANTIDAD CONGELADA
                numeroUnicoSegmento.InventarioDisponibleCruce = numeroUnicoSegmento.InventarioBuenEstado - numeroUnicoSegmento.InventarioCongelado;
                numeroUnicoSegmento.StopTracking();
                ctx.NumeroUnicoSegmento.ApplyChanges(numeroUnicoSegmento);
            }
            numeroUnicoInventario.StopTracking();
            ctx.NumeroUnicoInventario.ApplyChanges(numeroUnicoInventario);
            OrdenTrabajoSpool ordenTrabajoSpool = ctx.OrdenTrabajoSpool.SingleOrDefault(p => p.SpoolID == SpoolID);
            //CREO LA ORDEN DE TRABAJO DEL MATERIAL 
            OrdenTrabajoMaterial ordenTrabajoMaterial = new OrdenTrabajoMaterial();
            ordenTrabajoMaterial.StartTracking();
            ordenTrabajoMaterial.OrdenTrabajoSpoolID = ordenTrabajoSpool.OrdenTrabajoSpoolID;
            ordenTrabajoMaterial.MaterialSpoolID = MaterialSpoolID;
            ordenTrabajoMaterial.TieneInventarioCongelado = true;
            ordenTrabajoMaterial.CantidadCongelada = CantidadACongelar;
            ordenTrabajoMaterial.CongeladoEsEquivalente = EsEquivalente;
            ordenTrabajoMaterial.NumeroUnicoCongeladoID = NumeroUnicoACongelarID;
            ordenTrabajoMaterial.SegmentoCongelado = SegmentoACongelar;
            ordenTrabajoMaterial.SugeridoEsEquivalente = false;
            ordenTrabajoMaterial.TieneDespacho = false;
            ordenTrabajoMaterial.DespachoEsEquivalente = false;
            ordenTrabajoMaterial.FueReingenieria = false;
            ordenTrabajoMaterial.EsAsignado = false;
            ordenTrabajoMaterial.UsuarioModifica = userID;
            ordenTrabajoMaterial.FechaModificacion = DateTime.Now;
            ordenTrabajoMaterial.StopTracking();
            ctx.OrdenTrabajoMaterial.AddObject(ordenTrabajoMaterial);
            ctx.OrdenTrabajoMaterial.ApplyChanges(ordenTrabajoMaterial);

            //Guardar los cambios
            ctx.SaveChanges();
        }

        /// <summary>
        /// Elimina un MaterialSpool 
        /// </summary>
        /// <param name="ctx">Contexto de la base de datos</param>
        /// <param name="MaterialSpoolID">Identificador del MaterialSpool a homologar</param>
        /// <param name="userID">Identificador del Usuario que ejecuta las acciones</param>
        public void EliminaMaterialSpool(SamContext ctx, int MaterialSpoolID, DataTable juntasSpoolTemporal, DataTable cortesSpoolTemporal, Guid userID)
        {
            //BUSCO EL MATERIAL POR SU IDENTIFICADOR
            MaterialSpool materialSpool = ctx.MaterialSpool.SingleOrDefault(p => p.MaterialSpoolID == MaterialSpoolID);
            if (materialSpool != null)
            {
                //VERIFICO SI EL MATERIAL ES TUBO O ACCESORIO 
                bool EsTubo = false;
                ItemCode itemCode = ctx.ItemCode.SingleOrDefault(p => p.ItemCodeID == materialSpool.ItemCodeID);
                if (itemCode.TipoMaterialID == (int)TipoMaterialEnum.Tubo) { EsTubo = true; }

                //PROCEDIMIENTO ELIMINA LAS JUNTAS DONDE SE ENCUENTRA LA ETIQUETA DEL MATERIAL 
                //BUSCO ORDEN DE TRABAJO DEL MATERIAL 
                OrdenTrabajoMaterial ordenTrabajoMaterial = ctx.OrdenTrabajoMaterial.SingleOrDefault(p => p.MaterialSpoolID == MaterialSpoolID);

                if (ordenTrabajoMaterial != null)
                {
                    if (EliminoJuntaSpoolXEtiquetaDesdeMaterialSpool(ctx, materialSpool.SpoolID, materialSpool.Etiqueta, juntasSpoolTemporal, cortesSpoolTemporal, userID) == true)
                    {

                        CongeladoParcial congeladoParcial = ctx.CongeladoParcial.SingleOrDefault(p => p.MaterialSpoolID == MaterialSpoolID);

                        if (congeladoParcial != null)
                        {
                            DevuelveMaterialCongeladoParcialmente(ctx, congeladoParcial.NumeroUnicoCongeladoID, materialSpool.Cantidad, EsTubo, congeladoParcial.SegmentoCongelado, userID);
                            ctx.CongeladoParcial.DeleteObject(congeladoParcial);
                            ctx.CongeladoParcial.ApplyChanges(congeladoParcial);
                            ctx.SaveChanges();
                        }

                        int? ordenTrabajoMaterialNumeroUnicoID = null;

                        //ORDEN DE TRABAJO CONGELO EL MATERIAL 
                        if (ordenTrabajoMaterial.TieneInventarioCongelado == true)
                        {
                            //DEVUELVE MATERIAL CONGELADO POR ORDEN DE TRABAJO A INVENTARIO 
                            DevuelveMaterialCongeladoPorOrdenTrabajoMaterial(ctx, ordenTrabajoMaterial.NumeroUnicoCongeladoID.SafeIntParse(), ordenTrabajoMaterial.CantidadCongelada.SafeIntParse(), EsTubo, ordenTrabajoMaterial.SegmentoCongelado, userID);
                            ordenTrabajoMaterialNumeroUnicoID = ordenTrabajoMaterial.NumeroUnicoCongeladoID.SafeIntParse();
                        }
                        else
                        {
                            if (ordenTrabajoMaterial.TieneDespacho == true)
                            {
                                //CANCELA EL DESPACHO Y REGRESA A INVENTARIO 
                                DespachoBL.Instance.CancelaDespachoPorOrdenTrabajoMaterialID(ctx, ordenTrabajoMaterial.OrdenTrabajoMaterialID, userID, true);
                                //DEVUELVE MATERIAL CONGELADO POR ORDEN DE TRABAJO A INVENTARIO 
                                DevuelveMaterialCongeladoPorOrdenTrabajoMaterial(ctx, ordenTrabajoMaterial.NumeroUnicoCongeladoID.SafeIntParse(), ordenTrabajoMaterial.CantidadCongelada.SafeIntParse(), EsTubo, ordenTrabajoMaterial.SegmentoCongelado, userID);
                                ordenTrabajoMaterialNumeroUnicoID = ordenTrabajoMaterial.NumeroUnicoCongeladoID.SafeIntParse();
                            }
                        }

                        //BUSCO SI TIENE UN REGISTRO DE DESPACHO RELACIONADO (pueden ser más de uno si el despacho estaba cancelado)
                        List<Despacho> despachos = ctx.Despacho.Where(p => p.MaterialSpoolID == MaterialSpoolID)
                                                               .ToList();

                        bool despachoActivo = despachos.Any(d => !d.Cancelado);

                        if (despachoActivo)
                        {
                            throw new Exception(string.Format("El material {0} cuenta con un despacho activo que no ha sido cancelado", MaterialSpoolID));
                        }

                        //ELIMONO DESPACHO (SI EXISTE)
                        if (despachos != null && despachos.Count > 0)
                        {
                            despachos.ForEach(ctx.Despacho.DeleteObject);
                            ctx.SaveChanges();
                        }

                        //BUSCO SI TIENE UN DETALLE DE CORTE RELACIONADO                     
                        //CorteDetalle corteDetalle = ctx.CorteDetalle.SingleOrDefault(p => p.CorteDetalleID == ordenTrabajoMaterial.CorteDetalleID);

                        List<CorteDetalle> corteList = ctx.CorteDetalle.Where(p => (p.CorteDetalleID == ordenTrabajoMaterial.CorteDetalleID || (p.MaterialSpoolID == MaterialSpoolID && p.Cancelado == true))).ToList();


                        //ELIMINO DETALLE DEL CORTE (SI EXISTE)
                        foreach (CorteDetalle cd in corteList)
                        {
                            ctx.CorteDetalle.DeleteObject(cd);
                            ctx.CorteDetalle.ApplyChanges(cd);
                            ctx.SaveChanges();
                        }


                        //ELIMINO ORDEN DE TRABAJO 
                        ctx.OrdenTrabajoMaterial.DeleteObject(ordenTrabajoMaterial);
                        ctx.OrdenTrabajoMaterial.ApplyChanges(ordenTrabajoMaterial);
                        ctx.SaveChanges();

                        //MATERIAL NO TIENE ORDEN DE TRABAJO, ELIMINO EL MATERIAL 
                        ctx.MaterialSpool.DeleteObject(materialSpool);
                        ctx.MaterialSpool.ApplyChanges(materialSpool);
                        ctx.SaveChanges();

                    }
                    else
                    {
                        throw new Exception(MensajesError.Excepcion_EliminaJuntaArmada.ToString());
                    }
                }
                else
                {

                    CongeladoParcial congeladoParcial = ctx.CongeladoParcial.SingleOrDefault(p => p.MaterialSpoolID == MaterialSpoolID);

                    if (congeladoParcial != null)
                    {
                        DevuelveMaterialCongeladoParcialmente(ctx, congeladoParcial.NumeroUnicoCongeladoID, materialSpool.Cantidad, EsTubo, congeladoParcial.SegmentoCongelado, userID);
                        ctx.CongeladoParcial.DeleteObject(congeladoParcial);
                        ctx.CongeladoParcial.ApplyChanges(congeladoParcial);
                        ctx.SaveChanges();
                    }

                    int? ordenTrabajoMaterialNumeroUnicoID = null;

                    //ORDEN DE TRABAJO CONGELO EL MATERIAL 
                    if (ordenTrabajoMaterial.TieneInventarioCongelado == true)
                    {
                        //DEVUELVE MATERIAL CONGELADO POR ORDEN DE TRABAJO A INVENTARIO 
                        DevuelveMaterialCongeladoPorOrdenTrabajoMaterial(ctx, ordenTrabajoMaterial.NumeroUnicoCongeladoID.SafeIntParse(), ordenTrabajoMaterial.CantidadCongelada.SafeIntParse(), EsTubo, ordenTrabajoMaterial.SegmentoCongelado, userID);
                        ordenTrabajoMaterialNumeroUnicoID = ordenTrabajoMaterial.NumeroUnicoCongeladoID.SafeIntParse();
                    }
                    else
                    {
                        if (ordenTrabajoMaterial.TieneDespacho == true)
                        {
                            //CANCELA EL DESPACHO Y REGRESA A INVENTARIO 
                            DespachoBL.Instance.CancelaDespachoPorOrdenTrabajoMaterialID(ctx, ordenTrabajoMaterial.OrdenTrabajoMaterialID, userID, true);
                            //DEVUELVE MATERIAL CONGELADO POR ORDEN DE TRABAJO A INVENTARIO 
                            DevuelveMaterialCongeladoPorOrdenTrabajoMaterial(ctx, ordenTrabajoMaterial.NumeroUnicoCongeladoID.SafeIntParse(), ordenTrabajoMaterial.CantidadCongelada.SafeIntParse(), EsTubo, ordenTrabajoMaterial.SegmentoCongelado, userID);
                            ordenTrabajoMaterialNumeroUnicoID = ordenTrabajoMaterial.NumeroUnicoCongeladoID.SafeIntParse();
                        }
                    }

                    //BUSCO SI TIENE UN REGISTRO DE DESPACHO RELACIONADO (pueden ser más de uno si el despacho estaba cancelado)
                    List<Despacho> despachos = ctx.Despacho.Where(p => p.MaterialSpoolID == MaterialSpoolID)
                                                           .ToList();

                    bool despachoActivo = despachos.Any(d => !d.Cancelado);

                    if (despachoActivo)
                    {
                        throw new Exception(string.Format("El material {0} cuenta con un despacho activo que no ha sido cancelado", MaterialSpoolID));
                    }

                    //ELIMONO DESPACHO (SI EXISTE)
                    if (despachos != null && despachos.Count > 0)
                    {
                        despachos.ForEach(ctx.Despacho.DeleteObject);
                        ctx.SaveChanges();
                    }

                    //BUSCO SI TIENE UN DETALLE DE CORTE RELACIONADO                     
                    //CorteDetalle corteDetalle = ctx.CorteDetalle.SingleOrDefault(p => p.CorteDetalleID == ordenTrabajoMaterial.CorteDetalleID);
                    List<CorteDetalle> corteList = ctx.CorteDetalle.Where(p => (p.CorteDetalleID == ordenTrabajoMaterial.CorteDetalleID || (p.MaterialSpoolID == MaterialSpoolID && p.Cancelado == true))).ToList();
                    
                    //ELIMINO DETALLE DEL CORTE (SI EXISTE)
                    foreach (CorteDetalle cd in corteList)
                    {
                        ctx.CorteDetalle.DeleteObject(cd);
                        ctx.CorteDetalle.ApplyChanges(cd);
                        ctx.SaveChanges();
                    }


                    //ELIMINO ORDEN DE TRABAJO 
                    ctx.OrdenTrabajoMaterial.DeleteObject(ordenTrabajoMaterial);
                    ctx.OrdenTrabajoMaterial.ApplyChanges(ordenTrabajoMaterial);
                    ctx.SaveChanges();

                    //MATERIAL NO TIENE ORDEN DE TRABAJO, ELIMINO EL MATERIAL 
                    ctx.MaterialSpool.DeleteObject(materialSpool);
                    ctx.MaterialSpool.ApplyChanges(materialSpool);
                    ctx.SaveChanges();
                }
            }
        }

        /// <summary>
        /// ACTUALIZO DESDE JUNTASPOOL (CONTEXTO, NO DB) LOS MATERIALES CUYA ETIQUETA FUE CAMBIADA 
        /// </summary>
        /// <param name="ctx">Contexto de la base de datos</param>
        /// <param name="SpoolID">Identificador del Spool que se esta homologando</param>
        /// <param name="EtiquetaMaterialActual">Etiqueta que actulmente se encuentra registrada</param>
        /// <param name="EtiquetaMaterialNueva">Etiqueta que reemplazara la etiqueta del material actual</param>
        /// <param name="userID">Identificador del Usuario que ejecuta las acciones</param>
        public void ActualizoEtiquetaMaterialesTemporalesDesdeMaterialSpool(SamContext ctx, int SpoolID, string EtiquetaMaterialActual, string EtiquetaMaterialNueva, DataTable juntasSpoolTemporal, DataTable cortesSpoolTemporal, Guid userID)
        {
            foreach (DataRow juntaSpool in juntasSpoolTemporal.Rows)
            {
                if ((string)juntaSpool["EtiquetaMaterial1"] == EtiquetaMaterialActual) { juntaSpool["EtiquetaMaterial1"] = EtiquetaMaterialNueva; }
                if ((string)juntaSpool["EtiquetaMaterial2"] == EtiquetaMaterialActual) { juntaSpool["EtiquetaMaterial2"] = EtiquetaMaterialNueva; }
            }
            juntasSpoolTemporal.AcceptChanges();

            foreach (DataRow corteSpool in cortesSpoolTemporal.Rows)
            {
                if ((string)corteSpool["EtiquetaMaterial"] == EtiquetaMaterialActual) { corteSpool["EtiquetaMaterial"] = EtiquetaMaterialNueva; }
            }
            cortesSpoolTemporal.AcceptChanges();

        }

        /// <summary>
        /// Agrego a MaterialSpool en base a la informacion almacenada en MaterialSpoolPendiente
        /// </summary>
        /// <param name="ctx">Contexto de la base de datos</param>
        /// <param name="SpoolID">Identificador del MaterialSpoolPendiente utlilizado para homologar</param>
        /// <param name="MaterialSpoolPendienteID">Identificador del MaterialSpoolPendiente utlilizado para homologar</param>
        /// <param name="MaterialSpoolID">Identificador del MaterialSpool a homologar</param>
        /// <param name="userID">Identificador del Usuario que ejecuta las acciones</param>
        public void AsemejoMaterialSpoolDesdeMaterialSpoolPendiente(SamContext ctx, int MaterialSpoolPendienteID, int MaterialSpoolID, DataTable juntasSpoolTemporal, DataTable cortesSpoolTemporal, Guid userID)
        {
            MaterialSpoolPendiente materialSpoolPendiente = ctx.MaterialSpoolPendiente.SingleOrDefault(p => p.MaterialSpoolPendienteID == MaterialSpoolPendienteID);
            MaterialSpool materialSpool = ctx.MaterialSpool.SingleOrDefault(p => p.MaterialSpoolID == MaterialSpoolID);
            materialSpool.StartTracking();
            if (materialSpool.Etiqueta != materialSpoolPendiente.Etiqueta)
            {
                ActualizoEtiquetaMaterialesTemporalesDesdeMaterialSpool(ctx, materialSpool.SpoolID, materialSpool.Etiqueta, materialSpoolPendiente.Etiqueta, juntasSpoolTemporal, cortesSpoolTemporal, userID);
                materialSpool.Etiqueta = materialSpoolPendiente.Etiqueta;
            }
            materialSpool.Peso = materialSpoolPendiente.Peso;
            materialSpool.Area = materialSpoolPendiente.Area;
            materialSpool.Especificacion = materialSpoolPendiente.Especificacion;
            materialSpool.Grupo = materialSpoolPendiente.Grupo;
            materialSpool.DescripcionMaterial = materialSpoolPendiente.DescripcionMaterial;
            materialSpool.UsuarioModifica = userID;
            materialSpool.FechaModificacion = DateTime.Now;

            //LA CANTIDAD DE MATERIAL ES MENOR? (VERIFICAR MENOR VS MAYOR)
            if (materialSpool.Cantidad > materialSpoolPendiente.Cantidad)
            {
                //ACTUALIZO LA CANTIDAD 
                materialSpool.Cantidad = materialSpoolPendiente.Cantidad;

                //VERIFICAMOS SI EXISTE ORDEN DE TRABAJO PARA EL MATERIAL 
                OrdenTrabajoMaterial ordenTrabajoMaterial = ctx.OrdenTrabajoMaterial.SingleOrDefault(p => p.MaterialSpoolID == MaterialSpoolID);
                if (ordenTrabajoMaterial != null)
                {
                    bool EsTubo = false;
                    ItemCode itemCode = ctx.ItemCode.SingleOrDefault(p => p.ItemCodeID == materialSpool.ItemCodeID);
                    if (itemCode.TipoMaterialID == (int)TipoMaterialEnum.Tubo) { EsTubo = true; }

                    //VERIFICAMOS SI TIENE INVENTARIO CONGELADO 
                    if (ordenTrabajoMaterial.TieneInventarioCongelado == true)
                    {
                        //EJECUTO PROCEDIMIENTO DE LILIAN PARA MATERIAL CON INVENTARIO CONGELADO 
                        MaterialCambiaCantidadCongelado(ctx, materialSpoolPendiente.MaterialSpoolPendienteID, materialSpool.MaterialSpoolID, EsTubo, userID);
                    }
                    else
                    {
                        if (ordenTrabajoMaterial.TieneDespacho == true)
                        {
                            //EJECUTO PROCEDIMIENTO DE LILIAN PARA MATERIAL CON DESPACHO
                            MaterialCambiaCantidadDespachado(ctx, materialSpoolPendiente.MaterialSpoolPendienteID, materialSpool.MaterialSpoolID, EsTubo, userID);
                        }
                    }
                }
            }

            materialSpool.StopTracking();
            ctx.MaterialSpool.ApplyChanges(materialSpool);
            ctx.SaveChanges();
        }

        /// <summary>
        /// Agrego a MaterialSpool en base a la informacion almacenada en MaterialSpoolPendiente
        /// </summary>
        /// <param name="ctx">Contexto de la base de datos</param>
        /// <param name="SpoolID">Identificador del Spool que se desea homologar</param>
        /// <param name="MaterialSpoolPendienteID">Identificador del MaterialSpoolPendiente que se agregara a MaterialSpool</param>
        /// <param name="userID">Identificador del Usuario que ejecuta las acciones</param>
        public void IgualoMaterialSpoolDesdeMaterialSpoolPendiente(SamContext ctx, int MaterialSpoolPendienteID, int MaterialSpoolID, DataTable juntasSpoolTemporal, DataTable cortesSpoolTemporal, Guid userID)
        {
            MaterialSpoolPendiente materialSpoolPendiente = ctx.MaterialSpoolPendiente.SingleOrDefault(p => p.MaterialSpoolPendienteID == MaterialSpoolPendienteID);
            MaterialSpool materialSpool = ctx.MaterialSpool.SingleOrDefault(p => p.MaterialSpoolID == MaterialSpoolID);
            materialSpool.StartTracking();
            if (materialSpool.Etiqueta != materialSpoolPendiente.Etiqueta)
            {
                ActualizoEtiquetaMaterialesTemporalesDesdeMaterialSpool(ctx, materialSpool.SpoolID, materialSpool.Etiqueta, materialSpoolPendiente.Etiqueta, juntasSpoolTemporal, cortesSpoolTemporal, userID);
                materialSpool.Etiqueta = materialSpoolPendiente.Etiqueta;
            }
            materialSpool.Peso = materialSpoolPendiente.Peso;
            materialSpool.Area = materialSpoolPendiente.Area;
            materialSpool.Especificacion = materialSpoolPendiente.Especificacion;
            materialSpool.Grupo = materialSpoolPendiente.Grupo;
            materialSpool.DescripcionMaterial = materialSpoolPendiente.DescripcionMaterial;
            materialSpool.UsuarioModifica = userID;
            materialSpool.FechaModificacion = DateTime.Now;
            materialSpool.StopTracking();
            ctx.MaterialSpool.ApplyChanges(materialSpool);
            ctx.SaveChanges();
        }

        /// <summary>
        /// Agrego a MaterialSpool en base a la informacion almacenada en MaterialSpoolPendiente
        /// </summary>
        /// <param name="ctx">Contexto de la base de datos</param>
        /// <param name="SpoolID">Identificador del Spool que se desea homologar</param>
        /// <param name="MaterialSpoolPendienteID">Identificador del MaterialSpoolPendiente que se agregara a MaterialSpool</param>
        /// <param name="userID">Identificador del Usuario que ejecuta las acciones</param>
        public void AgregoMaterialSpoolDesdeMaterialSpoolPendiente(SamContext ctx, int MaterialSpoolPendienteID, Guid userID)
        {
            MaterialSpoolPendiente materialSpoolPendiente = ctx.MaterialSpoolPendiente.SingleOrDefault(p => p.MaterialSpoolPendienteID == MaterialSpoolPendienteID);
            MaterialSpool materialSpool = new MaterialSpool();
            materialSpool.StartTracking();
            materialSpool.SpoolID = materialSpoolPendiente.SpoolPendienteID;
            materialSpool.ItemCodeID = materialSpoolPendiente.ItemCodeID;
            materialSpool.Diametro1 = materialSpoolPendiente.Diametro1;
            materialSpool.Diametro2 = materialSpoolPendiente.Diametro2;
            materialSpool.Cantidad = materialSpoolPendiente.Cantidad;
            materialSpool.Etiqueta = materialSpoolPendiente.Etiqueta;
            materialSpool.Peso = materialSpoolPendiente.Peso;
            materialSpool.Area = materialSpoolPendiente.Area;
            materialSpool.Especificacion = materialSpoolPendiente.Especificacion;
            materialSpool.Grupo = materialSpoolPendiente.Grupo;
            materialSpool.DescripcionMaterial = materialSpoolPendiente.DescripcionMaterial;
            materialSpool.UsuarioModifica = userID;
            materialSpool.FechaModificacion = DateTime.Now;
            materialSpool.StopTracking();
            ctx.MaterialSpool.AddObject(materialSpool);
            ctx.MaterialSpool.ApplyChanges(materialSpool);

            //Guardar el material recientemente agregado
            ctx.SaveChanges();

            bool EsTubo = false;
            //VERIFICO EL TIPO DE MATERIAL (TUBO O ACCESORIO)
            ItemCode itemCode = ctx.ItemCode.SingleOrDefault(p => p.ItemCodeID == materialSpoolPendiente.ItemCodeID);
            if (itemCode.TipoMaterialID == (int)TipoMaterialEnum.Tubo)
            {

                EsTubo = true;

                //PARA TUBOS, BUSCO SI EXISTE ALGUN NUMERO UNICO CON INVENTARIO DISPONIBLE PARA CRUCE SUFICIENTE EN LA TABLA DE SEGMENTOS 
                var numeroUnicoXSegmentoDisponible =
                (
                    from a in ctx.NumeroUnicoSegmento
                    orderby a.NumeroUnicoID descending
                    where a.ProyectoID == (int)materialSpoolPendiente.SpoolPendiente.ProyectoID &&
                            a.NumeroUnico.ItemCodeID == materialSpoolPendiente.ItemCodeID &&
                            a.NumeroUnico.Diametro1 == materialSpoolPendiente.Diametro1 &&
                            a.NumeroUnico.Diametro2 == materialSpoolPendiente.Diametro2 &&
                            a.InventarioDisponibleCruce >= materialSpoolPendiente.Cantidad
                    select a
                ).FirstOrDefault();

                //VERIFICO SI LA CONSULTA REGRESO REGISTROS
                if (numeroUnicoXSegmentoDisponible != null)
                {
                    //EJECUTO RUTINA QUE HACE LOS RETIROS DEL INVENTARIO COMO MATERIALES CONGELADOS Y CREO LA ORDEN DE TRABAJO PARA EL MATERIAL
                    CongeloMaterialConOrdenTrabajoMaterial(ctx, materialSpool.SpoolID, materialSpool.MaterialSpoolID, numeroUnicoXSegmentoDisponible.NumeroUnicoID, false, materialSpoolPendiente.Cantidad, EsTubo, numeroUnicoXSegmentoDisponible.Segmento, userID);
                }
                else
                {
                    //SI NO SE ENCONTRARON REGISTROS, BUSCAMOS LOS ITEMCODES EQUIVALENTES 
                    var numeroUnicoXSegmentoEquivalenteDisponible =
                    (
                        from a in ctx.NumeroUnicoSegmento.Include("NumeroUnico")
                        join b in
                            (
                               from eqs in ctx.ItemCodeEquivalente
                               where eqs.ItemCodeID == materialSpoolPendiente.ItemCodeID &&
                                       eqs.Diametro1 == materialSpoolPendiente.Diametro1 &&
                                       eqs.Diametro2 == materialSpoolPendiente.Diametro2
                               select new { ItemCodeID = eqs.ItemEquivalenteID, Diametro1 = eqs.DiametroEquivalente1, Diametro2 = eqs.DiametroEquivalente2 }
                                )
                        on new { ItemCodeID = a.NumeroUnico.ItemCodeID, Diametro1 = a.NumeroUnico.Diametro1, Diametro2 = a.NumeroUnico.Diametro2 }
                                    equals
                                    new { ItemCodeID = (int?)b.ItemCodeID, Diametro1 = b.Diametro1, Diametro2 = b.Diametro2 }
                        orderby a.NumeroUnicoID descending
                        where a.ProyectoID == (int)materialSpoolPendiente.SpoolPendiente.ProyectoID &&
                                    a.InventarioDisponibleCruce >= materialSpoolPendiente.Cantidad
                        select a
                    ).FirstOrDefault();
                    if (numeroUnicoXSegmentoEquivalenteDisponible != null)
                    {
                        //EJECUTO RUTINA QUE HACE LOS RETIROS DEL INVENTARIO COMO MATERIALES CONGELADOS Y CREO LA ORDEN DE TRABAJO PARA EL MATERIAL
                        CongeloMaterialConOrdenTrabajoMaterial(ctx, materialSpool.SpoolID, materialSpool.MaterialSpoolID, numeroUnicoXSegmentoEquivalenteDisponible.NumeroUnicoID, true, materialSpoolPendiente.Cantidad, EsTubo, numeroUnicoXSegmentoEquivalenteDisponible.Segmento, userID);
                    }
                }
            }
            else
            {
                //PARA ACCESORIOS, BUSCO SI EXISTE ALGUN NUMERO UNICO CON INVENTARIO DISPONIBLE PARA CRUCE SUFICIENTE EN LA TABLA DE INVNETARIOS
                var numeroUnicoXInventarioDisponible =
                (
                    from a in ctx.NumeroUnicoInventario
                    orderby a.NumeroUnicoID descending
                    where a.ProyectoID == (int)materialSpoolPendiente.SpoolPendiente.ProyectoID &&
                            a.NumeroUnico.ItemCodeID == materialSpoolPendiente.ItemCodeID &&
                            a.NumeroUnico.Diametro1 == materialSpoolPendiente.Diametro1 &&
                            a.NumeroUnico.Diametro2 == materialSpoolPendiente.Diametro2 &&
                            a.InventarioDisponibleCruce >= materialSpoolPendiente.Cantidad
                    select a
                ).FirstOrDefault();

                //VERIFICO SI LA CONSULTA REGRESO REGISTROS
                if (numeroUnicoXInventarioDisponible != null)
                {
                    //EJECUTO RUTINA QUE HACE LOS RETIROS DEL INVENTARIO COMO MATERIALES CONGELADOS Y CREO LA ORDEN DE TRABAJO PARA EL MATERIAL
                    CongeloMaterialConOrdenTrabajoMaterial(ctx, materialSpool.SpoolID, materialSpool.MaterialSpoolID, numeroUnicoXInventarioDisponible.NumeroUnicoID, false, materialSpoolPendiente.Cantidad, EsTubo, string.Empty, userID);
                }
                else
                {
                    //SI NO SE ENCONTRARON REGISTROS, BUSCAMOS LOS ITEMCODES EQUIVALENTES 
                    var numeroUnicoXInventarioEquivalenteDisponible =
                    (
                        from a in ctx.NumeroUnicoInventario.Include("NumeroUnico")
                        join b in
                            (
                               from eqs in ctx.ItemCodeEquivalente
                               where eqs.ItemCodeID == materialSpoolPendiente.ItemCodeID &&
                                       eqs.Diametro1 == materialSpoolPendiente.Diametro1 &&
                                       eqs.Diametro2 == materialSpoolPendiente.Diametro2
                               select new { ItemCodeID = eqs.ItemEquivalenteID, Diametro1 = eqs.DiametroEquivalente1, Diametro2 = eqs.DiametroEquivalente2 }
                                )
                        on new { ItemCodeID = a.NumeroUnico.ItemCodeID, Diametro1 = a.NumeroUnico.Diametro1, Diametro2 = a.NumeroUnico.Diametro2 }
                                    equals
                                    new { ItemCodeID = (int?)b.ItemCodeID, Diametro1 = b.Diametro1, Diametro2 = b.Diametro2 }
                        orderby a.NumeroUnicoID descending
                        where a.ProyectoID == (int)materialSpoolPendiente.SpoolPendiente.ProyectoID &&
                                    a.InventarioDisponibleCruce >= materialSpoolPendiente.Cantidad
                        select a
                    ).FirstOrDefault();
                    if (numeroUnicoXInventarioEquivalenteDisponible != null)
                    {
                        //EJECUTO RUTINA QUE HACE LOS RETIROS DEL INVENTARIO COMO MATERIALES CONGELADOS Y CREO LA ORDEN DE TRABAJO PARA EL MATERIAL
                        CongeloMaterialConOrdenTrabajoMaterial(ctx, materialSpool.SpoolID, materialSpool.MaterialSpoolID, numeroUnicoXInventarioEquivalenteDisponible.NumeroUnicoID, true, materialSpoolPendiente.Cantidad, EsTubo, string.Empty, userID);
                    }
                }
            }
        }

        /// <summary>
        /// Inicia la ejecución de homologación sobre MaterialSSpool vs MaterialSpoolPendiente tomando como base las acciones solicitadas del usuario en la lista List<MaterialPendientePorHomologar>
        /// </summary>
        /// <param name="ctx">Contexto de la base de datos</param>
        /// <param name="SpoolID">Identificador del Spool que se desea homologar</param>
        /// <param name="materialesPendientesXHomologar">Relacion de que material se homologara con los materiales pendiente por homologar</param>
        /// <param name="userID">Identificador del Usuario que ejecuta las acciones</param>
        public void HomologaMaterialSpool(SamContext ctx, int SpoolID, List<MaterialPendientePorHomologar> materialesPendientesXHomologar, DataTable juntasSpoolTemporal, DataTable cortesSpoolTemporal, Guid userID)
        {
            foreach (MaterialPendientePorHomologar materialPendientesXHomologar in materialesPendientesXHomologar)
            {
                if (materialPendientesXHomologar.PasoValidacion == true)
                {
                    switch (materialPendientesXHomologar.Accion)
                    {
                        case Entities.AccionesHomologacion.Nuevo:
                            //SI ES UN NUEVO MATERIAL LO AGREGO A MATERIALESSPOOL 
                            AgregoMaterialSpoolDesdeMaterialSpoolPendiente(ctx, materialPendientesXHomologar.MaterialSpoolPendienteID, userID);
                            break;
                        case Entities.AccionesHomologacion.Igual:
                            //SI EL MATERIAL ES IGUAL
                            IgualoMaterialSpoolDesdeMaterialSpoolPendiente(ctx, materialPendientesXHomologar.MaterialSpoolPendienteID, materialPendientesXHomologar.MaterialSpoolID, juntasSpoolTemporal, cortesSpoolTemporal, userID);
                            break;
                        case Entities.AccionesHomologacion.Similar:
                            //SI EL MATERIAL ES SIMILAR
                            AsemejoMaterialSpoolDesdeMaterialSpoolPendiente(ctx, materialPendientesXHomologar.MaterialSpoolPendienteID, materialPendientesXHomologar.MaterialSpoolID, juntasSpoolTemporal, cortesSpoolTemporal, userID);
                            break;
                        case Entities.AccionesHomologacion.Eliminar:
                            //SI EL MATERIAL YA NO SE ENCUENTRA EN LA LISTA DE MATERIALES PENDIENTES POR HOMOLOGAR 
                            EliminaMaterialSpool(ctx, materialPendientesXHomologar.MaterialSpoolID, juntasSpoolTemporal, cortesSpoolTemporal, userID);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Inicia la ejecución de homologación sobre el Spool en base a la informacion almacenada en SpoolPendiente
        /// </summary>
        /// <param name="ctx">Contexto de la base de datos</param>
        /// <param name="SpoolID">Identificador del Spool que se desea homologar</param>
        /// <param name="userID">Identificador del Usuario que ejecuta las acciones</param>
        public void HomologaSpool(SamContext ctx, int SpoolID, Guid userID)
        {
            SpoolPendiente spoolPendiente = ctx.SpoolPendiente.SingleOrDefault(p => p.SpoolPendienteID == SpoolID);
            Spool spool = ctx.Spool.SingleOrDefault(p => p.SpoolID == SpoolID);
            spool.StartTracking();
            spool.ProyectoID = spoolPendiente.ProyectoID;
            spool.FamiliaAcero1ID = spoolPendiente.FamiliaAcero1ID;
            spool.FamiliaAcero2ID = spoolPendiente.FamiliaAcero2ID;
            spool.Nombre = spoolPendiente.Nombre;
            spool.Dibujo = spoolPendiente.Dibujo;
            spool.Especificacion = spoolPendiente.Especificacion;
            spool.Cedula = spoolPendiente.Cedula;
            spool.Pdis = spoolPendiente.Pdis;
            spool.DiametroPlano = spoolPendiente.DiametroPlano;
            spool.Peso = spoolPendiente.Peso;
            spool.Area = spoolPendiente.Area;
            spool.PorcentajePnd = spoolPendiente.PorcentajePnd;
            spool.RequierePwht = spoolPendiente.RequierePwht;
            //Las tres propiedades de abajo no se deben de cambiar
            //spool.PendienteDocumental = spoolPendiente.PendienteDocumental;
            //spool.AprobadoParaCruce = spoolPendiente.AprobadoParaCruce;
            spool.Prioridad = spoolPendiente.Prioridad;
            spool.Revision = spoolPendiente.Revision;
            spool.RevisionCliente = spoolPendiente.RevisionCliente;
            spool.Segmento1 = spoolPendiente.Segmento1;
            spool.Segmento2 = spoolPendiente.Segmento2;
            spool.Segmento3 = spoolPendiente.Segmento3;
            spool.Segmento4 = spoolPendiente.Segmento4;
            spool.Segmento5 = spoolPendiente.Segmento5;
            spool.Segmento6 = spoolPendiente.Segmento6;
            spool.Segmento7 = spoolPendiente.Segmento7;
            spool.UsuarioModifica = userID;
            spool.FechaModificacion = DateTime.Now;
            //Las propiedades de abajo tampoco se deben modificar
            //spool.VersionRegistro = spoolPendiente.VersionRegistro;
            //spool.SistemaPintura = spoolPendiente.SistemaPintura;
            //spool.ColorPintura = spoolPendiente.ColorPintura;
            //spool.CodigoPintura = spoolPendiente.CodigoPintura;            
            
            //Agregamos revision
            //Esta funcionalidad aun no es necesaria
            //spool.EsRevision = true;
            //spool.UltimaOrdenTrabajoEspecial = null;
            //spool.ConteoRevisiones = spool.ConteoRevisiones.SafeIntParse() + 1;
            
            spool.StopTracking();
            ctx.Spool.ApplyChanges(spool);
            ctx.SaveChanges();

            EliminaReportesPorSpool(ctx, SpoolID);
        }

        public void ActualizarOrdenTrabajo(SamContext ctx, int OrdenTrabajoID, byte[] ReporteODTAnterior, Guid userID)
        {
            OrdenTrabajo ordenTrabajo = ctx.OrdenTrabajo.SingleOrDefault(p => p.OrdenTrabajoID == OrdenTrabajoID);
            if (ReporteODTAnterior != null)
            {
                string historicoODT = "ODT_" + ordenTrabajo.NumeroOrden.ToString() + "_" + ordenTrabajo.VersionOrden.ToString() + "_" +
                                         DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "_" +
                                         DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString() + ".pdf";
                _logger.Debug("historicoODT: " + historicoODT);
                FileStream fileStream = new System.IO.FileStream(ConfigurationManager.AppSettings["Sam.Produccion.ODTFilesDirectory"] + "\\" + historicoODT, FileMode.Create, FileAccess.Write);
                fileStream.Write(ReporteODTAnterior, 0, ReporteODTAnterior.Length);
                _logger.Debug("filestrem: " + fileStream.Length);
                fileStream.Close();

            }
            ordenTrabajo.StartTracking();
            ordenTrabajo.VersionOrden = ordenTrabajo.VersionOrden + 1;
            ordenTrabajo.UsuarioModifica = userID;
            ordenTrabajo.FechaModificacion = DateTime.Now;
            ordenTrabajo.StopTracking();
            ctx.OrdenTrabajo.ApplyChanges(ordenTrabajo);
            _logger.Debug("agrega version orden");
            ctx.SaveChanges();
        }

        public void ActualizarOrdenTrabajo(int ordenTrabajoID, byte[] reporteODT, Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                ActualizarOrdenTrabajo(ctx, ordenTrabajoID, reporteODT, userID);
            }
        }
        /// <summary>
        /// Elimina de base de datos el spool pendiente una vez terminado el proceso de homologación
        /// </summary>
        /// <param name="ctx">Contexto de la base de datos</param>
        /// <param name="SpoolID">Identificador del Spool que se desea eliminar</param>
        /// <param name="userID">Identificador del Usuario que ejecuta las acciones</param>
        public bool ElminaSpoolPendienteXHomologar(SamContext ctx, int SpoolID, Guid userID)
        {
            SpoolPendiente spoolPendiente = ctx.SpoolPendiente.SingleOrDefault(p => p.SpoolPendienteID == SpoolID);
            spoolPendiente.StartTracking();
            var materialesSpoolPendiente =
                (
                    from a in ctx.MaterialSpoolPendiente
                    where a.SpoolPendienteID == SpoolID
                    select a
                );
            foreach (MaterialSpoolPendiente materialSpoolPendiente in materialesSpoolPendiente)
            {
                materialSpoolPendiente.StartTracking();
                ctx.MaterialSpoolPendiente.DeleteObject(materialSpoolPendiente);
                materialSpoolPendiente.StopTracking();
                ctx.MaterialSpoolPendiente.ApplyChanges(materialSpoolPendiente);
            }
            var juntasSpoolPendiente =
                (
                    from a in ctx.JuntaSpoolPendiente
                    where a.SpoolPendienteID == SpoolID
                    select a
                );
            foreach (JuntaSpoolPendiente juntaSpoolPendiente in juntasSpoolPendiente)
            {
                juntaSpoolPendiente.StartTracking();
                ctx.JuntaSpoolPendiente.DeleteObject(juntaSpoolPendiente);
                juntaSpoolPendiente.StopTracking();
                ctx.JuntaSpoolPendiente.ApplyChanges(juntaSpoolPendiente);
            }
            var cortesSpoolPendiente =
                (
                    from a in ctx.CorteSpoolPendiente
                    where a.SpoolPendienteID == SpoolID
                    select a
                );
            foreach (CorteSpoolPendiente corteSpoolPendiente in cortesSpoolPendiente)
            {
                corteSpoolPendiente.StartTracking();
                ctx.CorteSpoolPendiente.DeleteObject(corteSpoolPendiente);
                corteSpoolPendiente.StopTracking();
                ctx.CorteSpoolPendiente.ApplyChanges(corteSpoolPendiente);
            }
            spoolPendiente.StopTracking();
            ctx.SpoolPendiente.DeleteObject(spoolPendiente);
            ctx.SpoolPendiente.ApplyChanges(spoolPendiente);

            ctx.SaveChanges();
            return true;
        }

        public bool ElminaSpoolPendienteXHomologar(int SpoolID, Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                SpoolPendiente spoolPendiente = ctx.SpoolPendiente.SingleOrDefault(p => p.SpoolPendienteID == SpoolID);
                spoolPendiente.StartTracking();
                var materialesSpoolPendiente =
                    (
                        from a in ctx.MaterialSpoolPendiente
                        where a.SpoolPendienteID == SpoolID
                        select a
                    );
                foreach (MaterialSpoolPendiente materialSpoolPendiente in materialesSpoolPendiente)
                {
                    materialSpoolPendiente.StartTracking();
                    ctx.MaterialSpoolPendiente.DeleteObject(materialSpoolPendiente);
                    materialSpoolPendiente.StopTracking();
                    ctx.MaterialSpoolPendiente.ApplyChanges(materialSpoolPendiente);
                }
                var juntasSpoolPendiente =
                    (
                        from a in ctx.JuntaSpoolPendiente
                        where a.SpoolPendienteID == SpoolID
                        select a
                    );
                foreach (JuntaSpoolPendiente juntaSpoolPendiente in juntasSpoolPendiente)
                {
                    juntaSpoolPendiente.StartTracking();
                    ctx.JuntaSpoolPendiente.DeleteObject(juntaSpoolPendiente);
                    juntaSpoolPendiente.StopTracking();
                    ctx.JuntaSpoolPendiente.ApplyChanges(juntaSpoolPendiente);
                }
                var cortesSpoolPendiente =
                    (
                        from a in ctx.CorteSpoolPendiente
                        where a.SpoolPendienteID == SpoolID
                        select a
                    );
                foreach (CorteSpoolPendiente corteSpoolPendiente in cortesSpoolPendiente)
                {
                    corteSpoolPendiente.StartTracking();
                    ctx.CorteSpoolPendiente.DeleteObject(corteSpoolPendiente);
                    corteSpoolPendiente.StopTracking();
                    ctx.CorteSpoolPendiente.ApplyChanges(corteSpoolPendiente);
                }
                spoolPendiente.StopTracking();
                ctx.SpoolPendiente.DeleteObject(spoolPendiente);
                ctx.SpoolPendiente.ApplyChanges(spoolPendiente);
                ctx.SaveChanges();
                return true;
            }
        }

        public DataTable CreaTablaCorteSpoolTemporal(SamContext ctx, int SpoolID)
        {
            DataTable table = new DataTable();
            table.Columns.Add("CorteSpoolID", typeof(int));
            table.Columns.Add("SpoolID", typeof(int));
            table.Columns.Add("EtiquetaMaterial", typeof(string));
            table.Columns.Add("EtiquetaSeccion", typeof(string));
            table.Columns.Add("Diametro", typeof(decimal));

            var cortesSpool =
                (
                    from a in ctx.CorteSpool
                    where a.SpoolID == SpoolID
                    select a
                );
            //RECORREMOS TODOS LOS REGISTROS EN JUNTASPOOLPENDIENTE PARA VERIFICAR LA ACCION QUE DEBE REALIZARSE 
            foreach (CorteSpool corteSpool in cortesSpool)
            {
                table.Rows.Add(corteSpool.CorteSpoolID, corteSpool.SpoolID, corteSpool.EtiquetaMaterial, corteSpool.EtiquetaSeccion, corteSpool.Diametro);
            }
            return table;
        }

        public DataTable CreaTablaJuntaSpoolTemporal(SamContext ctx, int SpoolID)
        {
            DataTable table = new DataTable();
            table.Columns.Add("JuntaSpoolID", typeof(int));
            table.Columns.Add("SpoolID", typeof(int));
            table.Columns.Add("Etiqueta", typeof(string));
            table.Columns.Add("EtiquetaMaterial1", typeof(string));
            table.Columns.Add("EtiquetaMaterial2", typeof(string));
            var juntasSpool =
                (
                    from a in ctx.JuntaSpool
                    where a.SpoolID == SpoolID
                    select a
                );
            //RECORREMOS TODOS LOS REGISTROS EN JUNTASPOOLPENDIENTE PARA VERIFICAR LA ACCION QUE DEBE REALIZARSE 
            foreach (JuntaSpool juntaSpool in juntasSpool)
            {
                table.Rows.Add(juntaSpool.JuntaSpoolID, juntaSpool.SpoolID, juntaSpool.Etiqueta, juntaSpool.EtiquetaMaterial1, juntaSpool.EtiquetaMaterial2);
            }
            return table;
        }

        public bool PreExecuteAccionesHomologacion(SamContext ctx)
        {
            string sqlStatement = string.Empty;
            try
            {
                sqlStatement = sqlStatement + "IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_NAME = 'UQ_MaterialSpool_SpoolID_Etiqueta' AND  TABLE_NAME = 'MaterialSpool') ";
                sqlStatement = sqlStatement + "ALTER TABLE [dbo].[MaterialSpool] DROP CONSTRAINT [UQ_MaterialSpool_SpoolID_Etiqueta] ";
                ctx.ExecuteStoreCommand(sqlStatement);
                ctx.SaveChanges();
                sqlStatement = sqlStatement + "IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_NAME = 'UQ_JuntaSpool_SpoolID_Etiqueta' AND  TABLE_NAME = 'JuntaSpool') ";
                sqlStatement = sqlStatement + "ALTER TABLE [dbo].[JuntaSpool] DROP CONSTRAINT [UQ_JuntaSpool_SpoolID_Etiqueta] ";
                ctx.ExecuteStoreCommand(sqlStatement);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool PosExecuteAccionesHomologacion(SamContext ctx)
        {
            string sqlStatement = string.Empty;
            try
            {
                sqlStatement = sqlStatement + "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_NAME = 'UQ_MaterialSpool_SpoolID_Etiqueta' AND  TABLE_NAME = 'MaterialSpool') ";
                sqlStatement = sqlStatement + "ALTER TABLE [dbo].[MaterialSpool] ADD CONSTRAINT [UQ_MaterialSpool_SpoolID_Etiqueta] UNIQUE NONCLUSTERED ([SpoolID] ASC,[Etiqueta] ASC) ON [PRIMARY] ";
                ctx.ExecuteStoreCommand(sqlStatement);
                ctx.SaveChanges();
                sqlStatement = sqlStatement + "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_NAME = 'UQ_JuntaSpool_SpoolID_Etiqueta' AND  TABLE_NAME = 'JuntaSpool') ";
                sqlStatement = sqlStatement + "ALTER TABLE [dbo].[JuntaSpool] ADD CONSTRAINT [UQ_JuntaSpool_SpoolID_Etiqueta] UNIQUE NONCLUSTERED ([SpoolID] ASC,[Etiqueta] ASC) ON [PRIMARY] ";
                ctx.ExecuteStoreCommand(sqlStatement);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Elimina los reportes a nivel de spool
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="spoolID"></param>
        private void EliminaReportesPorSpool(SamContext ctx, int spoolID)
        {
            OrdenTrabajoSpool ordenTrabajoSpool = ctx.OrdenTrabajoSpool.SingleOrDefault(ots => ots.SpoolID == spoolID);

            if (ordenTrabajoSpool != null)
            {
                WorkstatusSpool workstatusSpool = ctx.WorkstatusSpool.SingleOrDefault(wss => wss.OrdenTrabajoSpoolID == ordenTrabajoSpool.OrdenTrabajoSpoolID);

                if (workstatusSpool != null)
                {
                    int workstatusSpoolID = workstatusSpool.WorkstatusSpoolID;

                    workstatusSpool.StartTracking();
                    workstatusSpool.TieneLiberacionDimensional = false;
                    workstatusSpool.TieneRequisicionPintura = false;
                    workstatusSpool.TienePintura = false;
                    workstatusSpool.LiberadoPintura = false;
                    workstatusSpool.StopTracking();
                    ctx.WorkstatusSpool.ApplyChanges(workstatusSpool);

                    List<ReporteDimensionalDetalle> reportesDimensionales = ctx.ReporteDimensionalDetalle.Where(rdd => rdd.WorkstatusSpoolID == workstatusSpoolID).ToList();
                    reportesDimensionales.ForEach(x => ctx.ReporteDimensionalDetalle.DeleteObject(x));

                    List<PinturaSpool> pinturaSpool = ctx.PinturaSpool.Where(x => x.WorkstatusSpoolID == workstatusSpoolID).ToList();
                    pinturaSpool.ForEach(x => x.RequisicionPinturaDetalleID = null);

                    List<RequisicionPinturaDetalle> requisicionesPintura = ctx.RequisicionPinturaDetalle.Where(rp => rp.WorkstatusSpoolID == workstatusSpoolID).ToList();                    
                    requisicionesPintura.ForEach(x => ctx.RequisicionPinturaDetalle.DeleteObject(x));

                    ctx.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Inicia la ejecución de las acciones de homologación de materiales en base a los parametros del usuario
        /// Devuelve verdadero si todas las cciones de homologación fueron exitosas y falso si existieron errores 
        /// </summary>
        /// <param name="SpoolID">Identificador del Spool que se desea homologar</param>
        /// <param name="userID">Identificador del Usuario que ejecuta las acciones</param>
        public bool EjecutaAccionesHomologacion(int SpoolID, int OrdenTrabajoID, byte[] ReporteODTAnterior, ArchivosWorkstatus archivos, Guid userID)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 1, 30)))
            {
                using (SamContext ctx = new SamContext())
                {
                    try
                    {
                        #region try
                        _logger.Debug("Iniciando homologacion");
                        PreExecuteAccionesHomologacion(ctx);
                        var result = ctx.GeneraSpoolHistorico((int?)SpoolID);
                        //CREO TABLA TEMPORAL PARA DAR SEGUIMIENTO A LOS CAMBIOS GENERADOS EN JUNTAS DEBIDO A CAMBIOS EN LOS MATERIALES 
                        DataTable juntasSpoolTemporal = CreaTablaJuntaSpoolTemporal(ctx, SpoolID);
                        //CREO TABLA TEMPORAL PARA DAR SEGUIMIENTO A LOS CAMBIOS GENERADOS EN JUNTAS DEBIDO A CAMBIOS EN LOS MATERIALES 
                        DataTable cortesSpoolTemporal = CreaTablaCorteSpoolTemporal(ctx, SpoolID);
                        //HOMOLOGO DATOS DEL SPOOL VS SPOOLPENDIENTE 
                        _logger.Debug("Homologa Spool");
                        HomologaSpool(ctx, SpoolID, userID);
                        //LISTA QUE CONTIENE LAS ACCIONES SELECCIONADAS POR EL USUARIO SOBRE LE HOMOLOGACION DE MATERIALES 
                        List<MaterialPendientePorHomologar> materialesPendientesXHomologar = MaterialPendienteHelper.MaterialesPendientesPorHomologar;
                        //EJECUTO LAS DISTINTAS ACCIONES SELECCIONADAS POR EL USUARIO PARA HOMOLOGAR MATERIALES 
                        _logger.Debug("Homologa Materiales");
                        HomologaMaterialSpool(ctx, SpoolID, materialesPendientesXHomologar, juntasSpoolTemporal, cortesSpoolTemporal, userID);
                        //EJECUTO LAS DISTINCAS ACCIONES NECESARIAS PARA HOMOLOGAR MATERIAL EN BASE A LA INFO DE MATERIALES SELECCIONADA 
                        _logger.Debug("Homologa Juntas");
                        HomologaJuntaSpool(ctx, SpoolID, juntasSpoolTemporal, userID);
                        //EJECUTO LAS DISTINCAS ACCIONES NECESARIAS PARA HOMOLOGAR MATERIAL EN BASE A LA INFO DE MATERIALES SELECCIONADA 
                        _logger.Debug("Homologa Corte");
                        HomologaCorteSpool(ctx, SpoolID, cortesSpoolTemporal, userID);
                        //SI NO EXISTIERON ERRORES, ELIMINO EL SPOOL PENDIENTE POR HOMOLOGAR
                        _logger.Debug("Elimino spool pendiente");
                        ElminaSpoolPendienteXHomologar(ctx, SpoolID, userID);
                        if (OrdenTrabajoID != -1)
                        {
                            //ACTUALIZO VERSION DE ODT Y CREO SU ARCHIVO PARA HISTORICO 
                            _logger.Debug("Actualizo ODT");
                            ActualizarOrdenTrabajo(ctx, OrdenTrabajoID, ReporteODTAnterior, userID);
                        }


                        GuardaReportesWorkstatus(ctx, archivos, SpoolID, userID);   
                     
                        //GUARDO LOS CAMBIOS 
                        _logger.Debug("Save changes");
                        ctx.SaveChanges();

                        #endregion
                    }
                    catch (Exception e)
                    {
                        _logger.Error("Error al homologar", e);
                        throw;
                    }
                    finally
                    {
                        _logger.Debug("Finally");
                        PosExecuteAccionesHomologacion(ctx);
                        _logger.Debug("After Finally");
                    }

                }
                _logger.Debug("scopecomplete");
                scope.Complete();
                _logger.Debug("after scope complete");
                return true;
            }
        }

       

        public void GeneraHistoriales(int SpoolID)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 1, 30)))
            {
                using (SamContext ctx = new SamContext())
                {
                    try
                    {
                        var result = ctx.GeneraSpoolHistorico((int?)SpoolID);
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                    finally
                    {
                    }

                }
            }
        }
        /// <summary>
        /// Este Metodo realiza las validaciones para saber si dos materiales se pueden homologar como lo indica el usuario
        /// </summary>
        /// <param name="materialSpoolPendienteId"></param>
        /// <param name="materialSpoolId"></param>
        /// <param name="accion"></param>
        public void AccionesHomologacion(int materialSpoolPendienteId, int materialSpoolId, AccionesHomologacion accion)
        {
            using (SamContext ctx = new SamContext())
            {
                switch (accion)
                {
                    case Entities.AccionesHomologacion.Nuevo:
                        MaterialPendienteHelper.AgregarMaterialesPendientesXHomologar(materialSpoolPendienteId, materialSpoolId, accion, true, string.Empty);

                        break;
                    case Entities.AccionesHomologacion.Igual:
                        //BUSCO AMBOS MATERIALES DE TABLAS EN BASE DE DATOS 
                        MaterialSpool materialSpool = ctx.MaterialSpool.SingleOrDefault(p => p.MaterialSpoolID == materialSpoolId);
                        MaterialSpoolPendiente materialSpoolPendiente = ctx.MaterialSpoolPendiente.SingleOrDefault(p => p.MaterialSpoolPendienteID == materialSpoolPendienteId);

                        //COMPARO QUE LOS CAMPOS TENGAN LA MISMA INFORMACION EN ITEMCODE, CANTIDAD, DIAMETRO1 Y DIAMETRO2
                        if (materialSpool.ItemCodeID == materialSpoolPendiente.ItemCodeID &&
                            materialSpool.Cantidad == materialSpoolPendiente.Cantidad &&
                            materialSpool.Diametro1 == materialSpoolPendiente.Diametro1 &&
                            materialSpool.Diametro2 == materialSpoolPendiente.Diametro2)
                        {
                            //VERIFICO SI EXISTE UN CAMBIO EN LA ETIQUETA
                            MaterialPendienteHelper.AgregarMaterialesPendientesXHomologar(materialSpoolPendienteId,
                                                                                             materialSpoolId, accion, true,
                                                                                             string.Empty);
                        }
                        else
                        {
                            //CASO CONTRARIO, REGRESO UN ERROR
                            MaterialPendienteHelper.AgregarMaterialesPendientesXHomologar(materialSpoolPendienteId,
                                                                                            materialSpoolId, accion, false,
                                                                                            MensajesError.IngenieriaHomologacionErrorCantidades
                                                                                            );
                        }
                        break;

                    case Entities.AccionesHomologacion.Similar:
                        //BUSCO AMBOS MATERIALES DE TABLAS EN BASE DE DATOS 
                        materialSpool = ctx.MaterialSpool.SingleOrDefault(p => p.MaterialSpoolID == materialSpoolId);
                        materialSpoolPendiente = ctx.MaterialSpoolPendiente.SingleOrDefault(p => p.MaterialSpoolPendienteID == materialSpoolPendienteId);

                        //COMPARO QUE LOS CAMPOS TENGAN LA MISMA INFORMACION EN ITEMCODE, DIAMETRO1 Y DIAMETRO2
                        if (materialSpool.ItemCodeID == materialSpoolPendiente.ItemCodeID &&
                            materialSpool.Diametro1 == materialSpoolPendiente.Diametro1 &&
                            materialSpool.Diametro2 == materialSpoolPendiente.Diametro2)
                        {
                            //(VERIFICAR MENOR VS MAYOR)
                            if (materialSpool.Cantidad >= materialSpoolPendiente.Cantidad)
                            {
                                //SI EN AMBOS MATERIALES TODOS LOS CAMPOS SON IGUALES, AGREGO MATERIALSPOOL A TABLA TEMPORAL SIN CAMBIOS 
                                MaterialPendienteHelper.AgregarMaterialesPendientesXHomologar(materialSpoolPendienteId,
                                                                                                materialSpoolId,
                                                                                                accion, true, string.Empty
                                                                                                );
                            }
                            else
                            {
                                //CASO CONTRARIO, REGRESO UN ERROR
                                MaterialPendienteHelper.AgregarMaterialesPendientesXHomologar(materialSpoolPendienteId,
                                                                                                materialSpoolId, accion, false,
                                                                                                MensajesError.IngenieriaHomologacionErrorMaterialMenor
                                                                                                );
                            }
                        }
                        else
                        {
                            //CASO CONTRARIO, REGRESO UN ERROR
                            MaterialPendienteHelper.AgregarMaterialesPendientesXHomologar(materialSpoolPendienteId,
                                                                                            materialSpoolId, accion, false,
                                                                                            MensajesError.IngenieriaHomologacionErrorCampos
                                                                                             );
                        }

                        break;
                    case Entities.AccionesHomologacion.Eliminar:
                        MaterialPendienteHelper.AgregarMaterialesPendientesXHomologar(materialSpoolPendienteId,
                                                                                        materialSpoolId, accion, true,
                                                                                        string.Empty
                                                                                        );
                        break;
                    default:
                        //REGRESO UN ERROR, NO ES UNA ACCION VALIDA 
                        break;
                }
            }
        }

        public ArchivosWorkstatus GeneraArchivosWorkStatus(int ProyectoID, int spoolID, string OrdenDeTrabajo, string NumeroControl, string RevCliente, string Rev)
        {
            ArchivosWorkstatus archivos = new ArchivosWorkstatus();                     

            //Creamos versión en Español
            CultureInfo actualCulture = Thread.CurrentThread.CurrentCulture;
            CultureInfo actualUICulture = Thread.CurrentThread.CurrentUICulture;

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageHelper.ESPANOL);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageHelper.ESPANOL);

            archivos.WorkstatusSpoolEspanol = ExcelSeguimientoSpool.Instance.ObtenerExcelPorIDs(ProyectoID, OrdenDeTrabajo, NumeroControl, false, null);
            archivos.WorkstatusJuntaEspanol = ExcelSeguimientoJuntas.Instance.ObtenerCsvPorIDs(ProyectoID, OrdenDeTrabajo, NumeroControl, false, false, null, null);
                        
            //Generamos primero el nombre sin extensión para mandarlo guardar a BD            
            archivos.NombreArchivoWksSpool = "WksSpool_" + spoolID.ToString() + "_" + RevCliente + "_" + Rev; ;
            archivos.NombreArchivoWksJunta = "WksJunta_" + spoolID.ToString() + "_" + RevCliente + "_" + Rev;

            //Creamos versión en Inglés
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageHelper.INGLES);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageHelper.INGLES);

            archivos.WorkstatusSpoolIngles = ExcelSeguimientoSpool.Instance.ObtenerExcelPorIDs(ProyectoID, OrdenDeTrabajo, NumeroControl, false, null);
            archivos.WorkstatusJuntaIngles = ExcelSeguimientoJuntas.Instance.ObtenerCsvPorIDs(ProyectoID, OrdenDeTrabajo, NumeroControl, false, false, null, null);

            Thread.CurrentThread.CurrentCulture = actualCulture;
            Thread.CurrentThread.CurrentUICulture = actualUICulture;

            return archivos;
        }

        public void GuardaReportesWorkstatus(SamContext ctx, ArchivosWorkstatus archivos, int spoolID, Guid userid)
        {
            HistoricoWorkstatus hist = new HistoricoWorkstatus();
            Spool spool = ctx.Spool.Where(x => x.SpoolID == spoolID).SingleOrDefault();

            FileStream fileStream = null;

            fileStream = new System.IO.FileStream(Configuracion.DBWorkStatusReports + "\\" + archivos.NombreArchivoWksSpool + ".xlsx", FileMode.Create, FileAccess.Write);
            fileStream.Write(archivos.WorkstatusSpoolEspanol, 0, archivos.WorkstatusSpoolEspanol.Length);
            fileStream.Close();

            fileStream = new System.IO.FileStream(Configuracion.DBWorkStatusReports + "\\" + archivos.NombreArchivoWksJunta + ".xlsx", FileMode.Create, FileAccess.Write);
            fileStream.Write(archivos.WorkstatusJuntaEspanol, 0, archivos.WorkstatusJuntaEspanol.Length);
            fileStream.Close();

            fileStream = new System.IO.FileStream(Configuracion.DBWorkStatusReports + "\\" + archivos.NombreArchivoWksSpool + "_en-US.xlsx", FileMode.Create, FileAccess.Write);
            fileStream.Write(archivos.WorkstatusSpoolIngles, 0, archivos.WorkstatusSpoolIngles.Length);
            fileStream.Close();

            fileStream = new System.IO.FileStream(Configuracion.DBWorkStatusReports + "\\" + archivos.NombreArchivoWksJunta + "_en-US.xlsx", FileMode.Create, FileAccess.Write);
            fileStream.Write(archivos.WorkstatusJuntaIngles, 0, archivos.WorkstatusJuntaIngles.Length);
            fileStream.Close();

            hist.SpoolID = spoolID;
            hist.RevisionCliente = spool.RevisionCliente;
            hist.Revision = spool.Revision;
            hist.ArchivoSpool = archivos.NombreArchivoWksSpool;
            hist.ArchivoJuntas = archivos.NombreArchivoWksJunta;
            hist.UsuarioModifica = userid;
            hist.FechaModificacion = DateTime.Now;
            hist.FechaHomologacion = DateTime.Now;

            ctx.HistoricoWorkstatus.AddObject(hist);
            ctx.SaveChanges();
        }
    }
}
