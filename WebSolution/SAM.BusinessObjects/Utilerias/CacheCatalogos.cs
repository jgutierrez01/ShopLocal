using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects;
using SAM.Entities.Cache;
using System.Web;
using System.Web.Caching;
using SAM.BusinessObjects.Administracion;
using SAM.Entities;
using SAM.Common;
using SAM.BusinessObjects.Proyectos;
using System.Threading;
using Mimo.Framework.Common;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Materiales;
using SAM.BusinessObjects.Produccion;
using SAM.BusinessObjects.Workstatus;

namespace SAM.BusinessObjects.Utilerias
{
    /// <summary>
    /// Clase encargada de manejar el Cache para los catálogos y/o tablas poco cambiantes del sistema
    /// </summary>
    public class CacheCatalogos
    {
        #region Variables de instancia y constructor con patrón singlenton

        private static readonly object _mutex = new object();
        private static CacheCatalogos _instance;

        private const string CACHE_ACEROS = "Cache.Aceros";
        private const string CACHE_CEDULA = "Cache.Cedula";
        private const string CACHE_CLIENTES = "Cache.Clientes";
        private const string CACHE_COLORES = "Cache.Colores";
        private const string CACHE_DEFECTOS = "Cache.Defectos";
        private const string CACHE_DIAMETRO = "Cache.Diametro";
        private const string CACHE_FABRICANTES = "Cache.Fabricantes";
        private const string CACHE_FAMILIAS_MATERIAL = "Cache.FamiliasMaterial";
        private const string CACHE_FAMILIAS_ACERO = "Cache.FamiliasAcero";
        private const string CACHE_PATIOS = "Cache.Patios";
        private const string CACHE_PERFILES = "Cache.Perfiles";
        private const string CACHE_PROCESOS_RAIZ = "Cache.ProcesosRaiz";
        private const string CACHE_PROCESOS_RELLENO = "Cache.ProcesosRelleno";
        private const string CACHE_PROVEEDORES = "Cache.Proveedores";
        private const string CACHE_PROYECTOS = "Cache.Proyectos";
        private const string CACHE_REPORTES_PROYECTO = "Cache.Reportes";
        private const string CACHE_TIPOS_CORTE = "Cache.TiposDeCorte";
        private const string CACHE_TIPOS_JUNTA = "Cache.TiposDeJunta";
        private const string CACHE_TRANSPORTISTAS = "Cache.Transportistas";
        private const string CACHE_WPS = "Cache.Wps";
        private const string CACHE_TALLERES = "Cache.Talleres";

        //No se pueden modificar a través del UI
        private const string CACHE_FABAREAS = "Cache.FabAreas";
        private const string CACHE_CONCEPTO_ESTIMACION = "Cache.ConceptoEstimacion";
        private const string CACHE_PERMISOS = "Cache.Permisos";
        private const string CACHE_TIPO_MATERIALES = "Cache.TipoMateriales";
        private const string CACHE_TIPOS_MOVIMIENTO = "Cache.TipoMovimientos";
        private const string CACHE_TIPO_REPORTE_DIMENSIONAL = "Cache.TipoReporteDimensional";
        private const string CACHE_TIPO_PRUEBA = "Cache.TipoPrueba";
        private const string CACHE_TIPO_PRUEBA_SPOOL = "Cache.TipoPruebaSpool";
        private const string CACHE_ULTIMO_PROCESO = "Cache.UltimoProceso";
        private const string CACHE_TIPO_REPORTE_PROYECTO = "Cache.TipoReporteProyecto";
        private const string CACHE_CAMPOS_SEGUIMIENTO_SPOOL = "Cache.CamposSeguimientoSpool";
        private const string CACHE_MODULOS_SEGUIMIENTO_SPOOL = "Cache.ModulosSeguimientoSpool";
        private const string CACHE_CAMPOS_SEGUIMIENTO_JUNTA = "Cache.CamposSeguimientoJunta";
        private const string CACHE_MODULOS_SEGUIMIENTO_JUNTA = "Cache.ModulosSeguimientoJunta";
        private const string CACHE_PAGINAS = "Cache.Paginas";
        private const string CACHE_CATEGORIA_PENDIENTE = "Cache.CategoriaPendiente";

        //Se expira/modifica a través de cambios al patio
        private const string CACHE_MAQUINA = "Cache.Maquina";
        private const string CACHE_CORTADOR = "Cache.Cortador";
        private const string CACHE_DESPACHADOR = "Cache.Despachador";

        /// <summary>
        /// Suscribirme a todos los eventos para hacer expirar cache cuando sea necesario.
        /// </summary>
        private CacheCatalogos()
        {
            AceroBO.Instance.AceroCambio += new TableChangedHandler(aceroCambio);
            CedulaBO.Instance.CedulaCambio += new TableChangedHandler(cedulaCambio);
            ClienteBO.Instance.ClienteCambio += new TableChangedHandler(clienteCambio);
            DefectoBO.Instance.DefectoCambio += new TableChangedHandler(defectoCambio);
            DiametroBO.Instance.DiametroCambio += new TableChangedHandler(diametroCambio);
            FabricanteBO.Instance.FabricanteCambio += new TableChangedHandler(fabricanteCambio);
            FamiliaMaterialBO.Instance.FamiliaMaterialCambio += new TableChangedHandler(familiaMaterialCambio);
            FamiliaAceroBO.Instance.FamiliaAceroCambio += new TableChangedHandler(familiaAceroCambio);
            PatioBO.Instance.PatioCambio += new TableChangedHandler(patioCambio);
            PerfilBO.Instance.PerfilCambio += new TableChangedHandler(perfilCambio);
            ProcesoRaizBO.Instance.ProcesoRaizCambio += new TableChangedHandler(procesoRaizCambio);
            ProcesoRellenoBO.Instance.ProcesoRellenoCambio += new TableChangedHandler(procesoRellenoCambio);
            ProveedorBO.Instance.ProveedorCambio += new TableChangedHandler(proveedorCambio);
            ProyectoBO.Instance.ProyectoCambio += new TableChangedHandler(proyectoCambio);
            TipoCorteBO.Instance.TipoCorteCambio += new TableChangedHandler(tipoCorteCambio);
            TipoJuntaBO.Instance.TipoJuntaCambio += new TableChangedHandler(tipoJuntaCambio);
            TransportistaBO.Instance.TransportistaCambio += new TableChangedHandler(transportistaCambio);
            WpsBO.Instance.WpsCambio += new TableChangedHandler(wpsCambio);
            ProyectoReporteBO.Instance.ProyectoCambio += new TableChangedHandler(proyectoReporteCambio);
        }

        /// <summary>
        /// 
        /// </summary>
        public static CacheCatalogos Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CacheCatalogos();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Handlers para expirar la llave de Cache cuando haya un cambio a la BD


        private void quitaDeCache(string llave)
        {
            cache.Remove(llave);
            cache.Remove(llave + "_ING");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void proyectoReporteCambio()
        {
            quitaDeCache(CACHE_REPORTES_PROYECTO);
        }

        /// <summary>
        /// Expira de cache la lista de tipos de junta
        /// </summary>
        private void tipoJuntaCambio()
        {
            quitaDeCache(CACHE_TIPOS_JUNTA);
        }

        /// <summary>
        /// Expira de cache la lista de tipos de corte
        /// </summary>
        private void tipoCorteCambio()
        {
            quitaDeCache(CACHE_TIPOS_CORTE);
        }


        /// <summary>
        /// Expira de cache la lista de proyectos
        /// </summary>
        private void proyectoCambio()
        {
            quitaDeCache(CACHE_PROYECTOS);
            quitaDeCache(CACHE_REPORTES_PROYECTO);
        }

        /// <summary>
        /// expira de cache la lista de clientes
        /// </summary>
        private void clienteCambio()
        {
            quitaDeCache(CACHE_CLIENTES);
        }

        /// <summary>
        /// Expira de cache la lista de materiales
        /// </summary>
        private void familiaMaterialCambio()
        {
            quitaDeCache(CACHE_FAMILIAS_MATERIAL);
        }

        /// <summary>
        /// Expira de cache la lista de transportistas
        /// </summary>
        private void transportistaCambio()
        {
            quitaDeCache(CACHE_TRANSPORTISTAS);
        }

        /// <summary>
        /// Expira de cache la lista de proveedores
        /// </summary>
        private void proveedorCambio()
        {
            quitaDeCache(CACHE_PROVEEDORES);
        }

        /// <summary>
        /// Expira de cache la lista de patios
        /// </summary>
        private void patioCambio()
        {
            quitaDeCache(CACHE_PATIOS);
            quitaDeCache(CACHE_MAQUINA);
        }

        /// <summary>
        /// Expira de cache la lista de procesos relleno
        /// </summary>
        private void procesoRellenoCambio()
        {
            quitaDeCache(CACHE_PROCESOS_RELLENO);
        }

        /// <summary>
        /// Expira de cache la lista de procesos raiz
        /// </summary>
        private void procesoRaizCambio()
        {
            quitaDeCache(CACHE_PROCESOS_RAIZ);
        }

        /// <summary>
        /// Expira de cache la lista de perfiles
        /// </summary>
        private void perfilCambio()
        {
            quitaDeCache(CACHE_PERFILES);
        }

        /// <summary>
        /// Expira de cache la lista de Wps
        /// </summary>
        private void wpsCambio()
        {
            quitaDeCache(CACHE_WPS);
        }

        /// <summary>
        /// Expira de cache la lista de defectos
        /// </summary>
        private void defectoCambio()
        {
            quitaDeCache(CACHE_DEFECTOS);
        }

        /// <summary>
        /// Expira de cache la lista de fabricantes
        /// </summary>
        private void fabricanteCambio()
        {
            quitaDeCache(CACHE_FABRICANTES);
        }

        /// <summary>
        /// Expira de cache la lista de aceros
        /// </summary>
        private void aceroCambio()
        {
            quitaDeCache(CACHE_ACEROS);
        }

        /// <summary>
        /// Expira de cache la lista de familias de acero
        /// </summary>
        private void familiaAceroCambio()
        {
            quitaDeCache(CACHE_FAMILIAS_ACERO);
        }

        /// <summary>
        /// Expira de cache la lista diametros
        /// </summary>
        private void diametroCambio()
        {
            quitaDeCache(CACHE_DIAMETRO);
        }

        /// <summary>
        /// Expira de cache la lista de cedulas
        /// </summary>
        private void cedulaCambio()
        {
            quitaDeCache(CACHE_CEDULA);
        }

        #endregion

        /// <summary>
        /// Propiedad de conveniencia para acceder a Cache
        /// </summary>
        private Cache cache
        {
            get
            {
                return HttpRuntime.Cache;
            }
        }

        #region Sección donde se va ya sea a BD o Cache dependiendo de lo que esté disponible

        /// <summary>
        /// Método genérico para no escribir tantas veces lo mismo
        /// </summary>
        /// <typeparam name="T">Tipo de dato orgiginal, generalmente debe ser la entidad del EF4</typeparam>
        /// <typeparam name="TOutput">Tipo de dato de salida, debe ser de subtipo EntidadBase para ser candidato a cache</typeparam>
        /// <param name="llaveCache">Llave para el diccionario de cache</param>
        /// <param name="horasCache">Cantidad de horas a permanecer en Cache</param>
        /// <param name="accion">Delegado al método de business objects para ir a la BD en caso que sea necesario</param>
        /// <param name="convertidor">Delegado al convertidor de EF4 --> EntidadBase</param>
        /// <returns>Lista de subtipos de EntidadBase</returns>
        private List<TOutput> obtenerLista<T,TOutput>(string llaveCache, int horasCache, Func<List<T>> accion, Converter<T,TOutput> convertidor) where TOutput : EntidadBase
        {
            List<TOutput> resultado = null;

            if (cache[llaveCache] == null)
            {
                List<T> lstBo = accion();

                resultado = lstBo.ConvertAll<TOutput>(convertidor);

                cache.Insert(llaveCache, resultado, null, DateTime.Now.AddHours(horasCache), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }
            else
            {
                resultado = (List<TOutput>)cache[llaveCache];
            }

            return resultado;
        }

        /// <summary>
        /// Método genérico para no escribir tantas veces lo mismo que permite obtener una lista pero en el idioma requerido
        /// </summary>
        /// <typeparam name="T">Tipo de dato orgiginal, generalmente debe ser la entidad del EF4</typeparam>
        /// <typeparam name="TOutput">Tipo de dato de salida, debe ser de subtipo EntidadBase para ser candidato a cache</typeparam>
        /// <param name="llaveCache">Llave para el diccionario de cache</param>
        /// <param name="horasCache">Cantidad de horas a permanecer en Cache</param>
        /// <param name="accion">Delegado al método de business objects para ir a la BD en caso que sea necesario</param>
        /// <param name="convertidor">Delegado al convertidor de EF4 --> EntidadBase</param>
        /// <returns>Lista de subtipos de EntidadBase</returns>
        private List<TOutput> obtenerListaLocalizada<T, TOutput>(string llaveCache, int horasCache, Func<List<T>> accion, Converter<T, TOutput> convertidor) where TOutput : EntidadBase
        {
            List<TOutput> resultado = null;

            string cultura = Thread.CurrentThread.CurrentUICulture.Name;

            llaveCache = (cultura == LanguageHelper.INGLES ? llaveCache + "_ING" : llaveCache);

            if (cache[llaveCache] == null)
            {
                List<T> lstBo = accion();

                resultado = lstBo.ConvertAll<TOutput>(convertidor);

                cache.Insert(llaveCache, resultado, null, DateTime.Now.AddHours(horasCache), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }
            else
            {
                resultado = (List<TOutput>)cache[llaveCache];
            }

            return resultado;
        }

        public List<ProyectoReporteCache> ObtenerProyectoReporte()
        {
            return obtenerListaLocalizada(CACHE_REPORTES_PROYECTO, Configuracion.CacheMaximoHoras, ProyectoReporteBO.Instance.ObtenerTodos, EntityConverter.ToProyectoReporteCache);
        }

        public List<TipoReporteProyectoCache> ObtenerTipoReporteProyecto()
        {
            return obtenerListaLocalizada(CACHE_TIPO_REPORTE_PROYECTO, Configuracion.CacheMaximoHoras, TipoReporteProyectoBO.Instance.ObtenerTodos, EntityConverter.ToTipoReporteProyectoCache);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ConceptoEstimacionCache> ObtenerConceptosEstimacion()
        {
            return obtenerListaLocalizada(CACHE_CONCEPTO_ESTIMACION, Configuracion.CacheMaximoHoras, ConceptoEstimacionBO.Instance.ObtenerTodos, EntityConverter.ToConceptoEstimacionCache);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ConceptoEstimacionCache> ObtenerConceptosEstimacionJunta()
        {
            return ObtenerConceptosEstimacion().Where(x => x.AplicaParaJunta).OrderBy(x => x.Nombre).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ConceptoEstimacionCache> ObtenerConceptosEstimacionSpool()
        {
            return ObtenerConceptosEstimacion().Where(x => x.AplicaParaSpool).OrderBy(x => x.Nombre).ToList();
        }

        
        /// <summary>
        /// Regresa la lista de aceros, ya sea de la BD o de Cache
        /// </summary>
        /// <returns></returns>
        public List<AceroCache> ObtenerAceros()
        {
            return obtenerLista(CACHE_ACEROS, Configuracion.CacheMaximoHoras, AceroBO.Instance.ObtenerTodosConFamilias, EntityConverter.ToAceroCache);
        }

        /// <summary>
        /// Regresa la lista de clientes, ya sea de la BD o de Cache
        /// </summary>
        /// <returns></returns>
        public List<ClienteCache> ObtenerClientes()
        {
            return obtenerLista(CACHE_CLIENTES, Configuracion.CacheMaximoHoras, ClienteBO.Instance.ObtenerTodos, EntityConverter.ToClienteCache);
        }

        /// <summary>
        /// Regresa la lista de colores, ya sea de la BD o de Cache
        /// </summary>
        /// <returns></returns>
        public List<ColorCache> ObtenerColores()
        {
            return obtenerListaLocalizada(CACHE_COLORES, Configuracion.CacheMaximoHoras, ColorBO.Instance.ObtenerTodos, EntityConverter.ToColorCache);
        }

        /// <summary>
        /// Regresa la lista de defectos
        /// </summary>
        /// <returns></returns>
        public List<DefectoCache> ObtenerDefectos()
        {
            return obtenerListaLocalizada(CACHE_DEFECTOS, Configuracion.CacheMaximoHoras, DefectoBO.Instance.ObtenerTodos, EntityConverter.ToDefectoCache);
        }

        /// <summary>
        /// Regresa la lista de fab areas
        /// </summary>
        /// <returns></returns>
        public List<FabAreaCache> ObtenerFabAreas()
        {
            return obtenerLista(CACHE_FABAREAS, Configuracion.CacheMaximoHoras, FabAreaBO.Instance.ObtenerTodos, EntityConverter.ToFabAreaCache);
        }

        /// <summary>
        /// Regresa la lista de fabricantes
        /// </summary>
        /// <returns></returns>
        public List<FabricanteCache> ObtenerFabricantes()
        {
            return obtenerLista(CACHE_FABRICANTES, Configuracion.CacheMediaHoras, FabricanteBO.Instance.ObtenerTodosConContacto, EntityConverter.ToFabricanteCache);
        }

        /// <summary>
        /// Regresa la lista de familias de material
        /// </summary>
        /// <returns></returns>
        public List<FamMaterialCache> ObtenerFamiliasMaterial()
        {
            return obtenerLista(CACHE_FAMILIAS_MATERIAL, Configuracion.CacheMaximoHoras, FamiliaMaterialBO.Instance.ObtenerTodas, EntityConverter.ToFamMaterialCache);
        }

        /// <summary>
        /// Regresa la lista de familias de aceros
        /// </summary>
        /// <returns></returns>
        public List<FamAceroCache> ObtenerFamiliasAcero()
        {
            return obtenerLista(CACHE_FAMILIAS_ACERO, Configuracion.CacheMaximoHoras, FamiliaAceroBO.Instance.ObtenerTodasConFamiliaMaterial, EntityConverter.ToFamAceroCache);
        }

        /// <summary>
        /// Regresa la lista de patios
        /// </summary>
        /// <returns></returns>
        public List<PatioCache> ObtenerPatios()
        {
            return obtenerLista(CACHE_PATIOS, Configuracion.CacheMediaHoras, PatioBO.Instance.ObtenerTodosConTaller, EntityConverter.ToPatioCache);
        }

        /// <summary>
        /// Regresa la lista de talleres
        /// </summary>
        /// <returns></returns>
        public List<TallerCache> ObtenerTalleres()
        {
            return obtenerLista(CACHE_TALLERES, Configuracion.CacheMaximoHoras, TallerBO.Instance.ObtenerTodos, EntityConverter.ToTallerCache);
        }

        /// <summary>
        /// Regresa la lista de permisos
        /// </summary>
        /// <returns></returns>
        public List<PermisoCache> ObtenerPermisos()
        {
            return obtenerListaLocalizada(CACHE_PERMISOS, Configuracion.CacheMaximoHoras, PermisoBO.Instance.ObtenerTodosConPaginas, EntityConverter.ToPermisoCache);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<PaginaCache> ObtenerPaginas()
        {
            return obtenerLista(CACHE_PAGINAS, Configuracion.CacheMaximoHoras, PaginaBO.Instance.ObtenerTodas, EntityConverter.ToPaginaCache);
        }

        /// <summary>
        /// Regresa la lista de perfiles
        /// </summary>
        /// <returns></returns>
        public List<PerfilCache> ObtenerPerfiles()
        {
            return obtenerListaLocalizada(CACHE_PERFILES, Configuracion.CacheMediaHoras, PerfilBO.Instance.ObtenerTodosConPermisos, EntityConverter.ToPerfilCache);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TipoMaterialCache> ObtenerTipoMaterial()
        {
            return obtenerListaLocalizada(CACHE_TIPO_MATERIALES, Configuracion.CacheMediaHoras, TipoMaterialBO.Instance.ObtenerTodos, EntityConverter.ToTipoMaterialCache);
        }

        /// <summary>
        /// Regresa la lista de procesos raiz
        /// </summary>
        /// <returns></returns>
        public List<ProcesoRaizCache> ObtenerProcesosRaiz()
        {
            return obtenerListaLocalizada(CACHE_PROCESOS_RAIZ, Configuracion.CacheMaximoHoras, ProcesoRaizBO.Instance.ObtenerTodos, EntityConverter.ToProcesoRaizCache);
        }

        /// <summary>
        /// Regresa la lista de procesos relleno
        /// </summary>
        /// <returns></returns>
        public List<ProcesoRellenoCache> ObtenerProcesosRelleno()
        {
            return obtenerListaLocalizada(CACHE_PROCESOS_RELLENO, Configuracion.CacheMaximoHoras, ProcesoRellenoBO.Instance.ObtenerTodos, EntityConverter.ToProcesoRellenoCache);
        }

        /// <summary>
        /// Regresa la lista de proveedores
        /// </summary>
        /// <returns></returns>
        public List<ProveedorCache> ObtenerProveedores()
        {
            return obtenerLista(CACHE_PROVEEDORES, Configuracion.CacheMediaHoras, ProveedorBO.Instance.ObtenerTodosConContacto, EntityConverter.ToProveedorCache);
        }

        /// <summary>
        /// Regresa la lista de proyectos
        /// </summary>
        /// <returns></returns>
        public List<ProyectoCache> ObtenerProyectos()
        {
            return obtenerListaLocalizada(CACHE_PROYECTOS, Configuracion.CacheMinimoHoras, ProyectoBO.Instance.ObtenerTodosConClienteColorPatioNomenclatura, EntityConverter.ToProyectoCache);
        }

        /// <summary>
        /// Regresa la lista de tipos de corte
        /// </summary>
        /// <returns></returns>
        public List<TipoCorteCache> ObtenerTiposCorte()
        {
            return obtenerLista(CACHE_TIPOS_CORTE, Configuracion.CacheMaximoHoras, TipoCorteBO.Instance.ObtenerTodos, EntityConverter.ToTipoCorteCache);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TipoMovimientoCache> ObtenerTiposMovimiento()
        {
            return obtenerListaLocalizada(CACHE_TIPOS_MOVIMIENTO, Configuracion.CacheMaximoHoras, TipoMovimientoBO.Instance.ObtenerTodos, EntityConverter.ToTipoMovimientoCache);
        }

        /// <summary>
        /// Regresa la lista de tipos de prueba
        /// </summary>
        /// <returns></returns>
        public List<TipoPruebaCache> ObtenerTiposPrueba()
        {
            return obtenerListaLocalizada(CACHE_TIPO_PRUEBA, Configuracion.CacheMaximoHoras, TipoPruebaBO.Instance.ObtenerTodos, EntityConverter.ToTipoPruebaCache);
        }

        public List<TipoPruebaSpoolCache> ObtenerTiposPruebaSpool()
        {
            return obtenerListaLocalizada(CACHE_TIPO_PRUEBA_SPOOL, Configuracion.CacheMaximoHoras, TipoPruebaSpoolBO.Instance.ObtenerTodos, EntityConverter.ToTipoPruebaSpoolCache);
        }

        /// <summary>
        /// Regresa la lista de tipos de junta
        /// </summary>
        /// <returns></returns>
        public List<TipoJuntaCache> ObtenerTiposJunta()
        {
            return obtenerLista(CACHE_TIPOS_JUNTA, Configuracion.CacheMaximoHoras, TipoJuntaBO.Instance.ObtenerTodos, EntityConverter.ToTipoJuntaCache);
        }

        /// <summary>
        /// Regresa la lista de transportistas
        /// </summary>
        /// <returns></returns>
        public List<TransportistaCache> ObtenerTransportistas()
        {
            return obtenerLista(CACHE_TRANSPORTISTAS, Configuracion.CacheMediaHoras, TransportistaBO.Instance.ObtenerTodosConContacto, EntityConverter.ToTransportistaCache);
        }

        /// <summary>
        /// Regresa la lista de Wps
        /// </summary>
        /// <returns></returns>
        public List<WpsCache> ObtenerWps()
        {
            return obtenerLista(CACHE_WPS, Configuracion.CacheMaximoHoras, WpsBO.Instance.ObtenerTodosConRelaciones, EntityConverter.ToWpsCache);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DiametroCache> ObtenerDiametros()
        {
            return obtenerLista(CACHE_DIAMETRO, Configuracion.CacheMaximoHoras, DiametroBO.Instance.ObtenerTodos, EntityConverter.ToDiametroCache);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<CedulaCache> ObtenerCedulas()
        {
            return obtenerLista(CACHE_CEDULA, Configuracion.CacheMaximoHoras, CedulaBO.Instance.ObtenerTodos, EntityConverter.ToCedulaCache);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<UltimoProcesoCache> ObtenerUltimoProceso()
        {
            return obtenerListaLocalizada(CACHE_ULTIMO_PROCESO, Configuracion.CacheMaximoHoras, UltimoProcesoBO.Instance.ObtenerTodos, EntityConverter.ToUltimoProcesoCache);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<MaquinaCache> ObtenerMaquinas()
        {
            return obtenerLista(CACHE_MAQUINA, Configuracion.CacheMaximoHoras, MaquinaBO.Instance.ObtenerTodasConPatio, EntityConverter.ToMaquinaCache);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<CortadorCache> ObtenerCortadores()
        {
            return obtenerLista(CACHE_CORTADOR, Configuracion.CacheMaximoHoras, CortadorBO.Instance.ObtenerTodasConPatioSeleccion, EntityConverter.ToCortadorCache);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DespachadorCache> ObtenerDespachadores()
        {
            return obtenerLista(CACHE_DESPACHADOR, Configuracion.CacheMaximoHoras, DespachadorBO.Instance.ObtenerTodasConPatioSeleccion, EntityConverter.ToDespachadorCache);
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TipoReporteDimensionalCache> ObtenerTipoReporteDimensional()
        {
            return obtenerListaLocalizada(CACHE_TIPO_REPORTE_DIMENSIONAL, Configuracion.CacheMaximoHoras, InspeccionDimensionalBO.Instance.ObtenerTiposReporteDimensional, EntityConverter.ToTipoReporteDimensionalCache);
        }


        public List<CampoSeguimientoSpoolCache> ObtenerCamposSeguimientoSpool()
        {
            
                return obtenerListaLocalizada(CACHE_CAMPOS_SEGUIMIENTO_SPOOL, Configuracion.CacheMaximoHoras,
                                    CampoSeguimientoSpoolBO.Instance.ObtenerTodas, EntityConverter.ToCampoSeguimientoSpool);
            
        }

        public List<CampoSeguimientoJuntaCache> ObtenerCamposSeguimientoJunta()
        {

            return obtenerListaLocalizada(  CACHE_CAMPOS_SEGUIMIENTO_JUNTA,
                                            Configuracion.CacheMaximoHoras,
                                            CampoSeguimientoJuntaBO.Instance.ObtenerTodas, 
                                            EntityConverter.ToCampoSeguimientoJunta);

        }

        public List<ModuloSeguimientoSpoolCache> ObtenerModulosSeguimientoSpool()
        {

            return obtenerListaLocalizada(  CACHE_MODULOS_SEGUIMIENTO_SPOOL, 
                                            Configuracion.CacheMaximoHoras,
                                            ModuloSeguimientoSpoolBO.Instance.ObtenerTodas, 
                                            EntityConverter.ToModuloSeguimientoSpool);

        }

        public List<ModuloSeguimientoJuntaCache> ObtenerModulosSeguimientoJunta()
        {

            return obtenerListaLocalizada(CACHE_MODULOS_SEGUIMIENTO_JUNTA, Configuracion.CacheMaximoHoras,
                                ModuloSeguimientoJuntaBO.Instance.ObtenerTodas, EntityConverter.ToModuloSeguimientoJunta);

        }

        public List<CategoriaPendienteCache> ObtenerCategoriaPendiente()
        {

            return obtenerListaLocalizada(CACHE_CATEGORIA_PENDIENTE, Configuracion.CacheMaximoHoras,
                                CategoriaPendienteBO.Instance.ObtenerTodas, EntityConverter.ToCategoriaPendiente);

        }
        #endregion


        /// <summary>
        /// ID del fab area marcado como shop
        /// </summary>
        public int ShopFabAreaID
        {
            get
            {
                return ObtenerFabAreas().Where(x => x.Nombre.Equals(FabAreas.SHOP, StringComparison.InvariantCultureIgnoreCase))
                                        .Single().ID;
            }
        }

        /// <summary>
        /// ID del fab area marcado como FIELD
        /// </summary>
        public int FieldFabAreaID
        {
            get
            {
                return ObtenerFabAreas().Where(x => x.Nombre.Equals(FabAreas.FIELD, StringComparison.InvariantCultureIgnoreCase))
                                        .Single().ID;
            }
        }

        /// <summary>
        /// ID del fab area marcado como JACKET
        /// </summary>
        public int JacketFabAreaID
        {
            get
            {
                return ObtenerFabAreas().Where(x => x.Nombre.Equals(FabAreas.JACKET, StringComparison.InvariantCultureIgnoreCase))
                                        .Single().ID;
            }
        }


        /// <summary>
        /// ID de la junta tipo TH
        /// </summary>
        public int TipoJuntaTHID
        {
            get
            {
                return ObtenerTiposJunta().Where(x => x.Nombre.Equals(TipoJuntas.TH, StringComparison.InvariantCultureIgnoreCase))
                                        .Single().ID;
            }
        }

        /// <summary>
        /// ID de la junta tipo TH
        /// </summary>
        public int TipoJuntaTWID
        {
            get
            {
                return ObtenerTiposJunta().Where(x => x.Nombre.Equals(TipoJuntas.TW, StringComparison.InvariantCultureIgnoreCase))
                                        .Single().ID;
            }
        }


        /// <summary>
        /// Quita los catalogos de cache usados en ingenieria para que sean refrescados
        /// </summary>
        public void LimpiaCacheIngenieria()
        {
            quitaDeCache(CACHE_ACEROS);
            quitaDeCache(CACHE_DIAMETRO);
            quitaDeCache(CACHE_TIPOS_JUNTA);
            quitaDeCache(CACHE_TIPOS_CORTE);
            quitaDeCache(CACHE_FAMILIAS_MATERIAL);
            quitaDeCache(CACHE_FAMILIAS_ACERO);
            quitaDeCache(CACHE_ACEROS);
            quitaDeCache(CACHE_CEDULA);
        }
    }
}
