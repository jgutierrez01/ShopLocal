using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using log4net;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Ingenieria;
using SAM.Common;
using SAM.Entities;
using SAM.Entities.Cache;
using cs = SAM.BusinessLogic.Ingenieria.CorteSpoolIng.Columna;
using sp = SAM.BusinessLogic.Ingenieria.SpoolIng.Columna;
using ms = SAM.BusinessLogic.Ingenieria.MaterialSpoolIng.Columna;
using js = SAM.BusinessLogic.Ingenieria.JuntaSpoolIng.Columna;
using SAM.BusinessObjects.Utilerias;
using System.IO;
using System.Text;

namespace SAM.BusinessLogic.Ingenieria
{
    public class InterpreteDatos
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(InterpreteDatos));
        private readonly int _proyectoID;
        private readonly ArchivoSimple[] _archivos;
        private bool Valido = true;
        private static readonly int _archivosEsperados = Configuracion.IngenieriaArchivosEsperados;
        private static readonly DataTable _dataTable = new DataTable();
        private List<FabAreaCache> _fabAreasCache;
        private List<TipoCorte> _tipoCortesCatalogo;
        private List<TipoJunta> _tipoJuntasCatalogo;
        private List<ItemCode> _itemCodeCatalogo;
        private List<DiametroCache> _diametrosCache;
        private List<CedulaCache> _cedulasCache;
        private List<Diametro> _diametrosCatalogo;
        private List<Cedula> _cedulasCatalogo;
        private List<string> _itemCodeArchivo = new List<string>();

        
        private List<FamiliaAcero> _familiasAceroCatalogo;
        private List<FamiliaMaterial> _familiasMaterialCatalogo;
        private readonly List<FamiliaAcero> _familiasAceroUi;
        //Los spools del proyecto con despacho 
        private readonly List<Spool> _spoolsProyecto;
        private readonly List<SpoolIng> _spoolsArchivo = new List<SpoolIng>();
        private readonly List<CorteSpoolIng> _corteSpoolArchivo = new List<CorteSpoolIng>();
        private readonly List<JuntaSpoolIng> _juntaSpoolArchivo = new List<JuntaSpoolIng>();
        private readonly List<MaterialSpoolIng> _materialSpoolArchivo = new List<MaterialSpoolIng>();
        private readonly Guid _usuarioModifica;

        public static readonly string[] NombresArchivos = { "cutlist.csv", "material.csv", "spools.csv", "welds.csv" };



        //#endregion

        //Ruta en Donde se almacenaran los archivos
        public static string PathParaAlmacenarArchivos
        {
            get
            {
                return Configuracion.RutaParaAlmacenarArchivos + HttpContext.Current.Session.SessionID + "/";
            }
        }

        /// <summary>
        /// Regresa un arreglo de ArchivoSimple, que es un wrapper para el stream, junto con el nombre del archivo, sin la extension
        /// </summary>
        /// <returns></returns>
        public static ArchivoSimple[] ObtenerArchivosSubidos()
        {
            ArchivoSimple[] archivos = new ArchivoSimple[4];
            for (int i = 0; i < 4; i++)
            {
                archivos[i] =
                    new ArchivoSimple
                    {
                        Nombre = NombresArchivos[i].Substring(0, NombresArchivos[i].LastIndexOf(".")),
                        Stream = new FileInfo(PathParaAlmacenarArchivos + NombresArchivos[i]).OpenRead()
                    };
            }
            return archivos;
        }


        private List<SpoolIng> _spoolsArchivoCache;
        /// <summary>
        /// La informacion parseada de los archivos se mete a cache, por cuestion de que seria muy pesado parsear los archivos cada vez que se abriera un popup de homologacion
        /// </summary>
        public List<SpoolIng> SpoolsArchivoCache
        {
            get
            {
                _spoolsArchivoCache = (List<SpoolIng>)HttpRuntime.Cache.Get(HttpContext.Current.Session.SessionID + "_ing");
                if (_spoolsArchivoCache == null)
                {
                    EmpiezaProceso(false);
                }
                return _spoolsArchivoCache;
            }
            set
            {
                HttpRuntime.Cache.Insert(HttpContext.Current.Session.SessionID + "_ing", value, null,
                                         DateTime.Now.Add(TimeSpan.FromMinutes(Configuracion.CacheMuyPocosMinutos)),
                                         TimeSpan.Zero);
                _spoolsArchivoCache = value;
            }
        }

        #region Properties

        /// <summary>
        /// La lista de los spools que se encontraron dentro del archivo
        /// </summary>
        public List<SpoolIng> SpoolEnArchivo
        {
            get
            {
                return _spoolsArchivo;
            }
        }

        /// <summary>
        /// El numero total de spools dentro del archivo
        /// </summary>
        public int NumSpoolsEnArchivo { get; set; }

        private HashSet<string> _fabAreasNoEncontradas;
        public HashSet<string> FabAreasNoEncontradas
        {
            get { return _fabAreasNoEncontradas ?? (_fabAreasNoEncontradas = new HashSet<string>()); }
        }

        private HashSet<string> _familiaAceroNoEncontradas;
        public HashSet<string> FamiliaAceroNoEncontradas
        {
            get { return _familiaAceroNoEncontradas ?? (_familiaAceroNoEncontradas = new HashSet<string>()); }
        }

        private HashSet<string> _itemCodeNoEncontrados;
        public HashSet<string> ItemCodeNoEncontrados
        {
            get { return _itemCodeNoEncontrados ?? (_itemCodeNoEncontrados = new HashSet<string>()); }
        }

        private HashSet<string> _itemCodeAModificar;
        public HashSet<string> ItemCodeAModificar
        {
            get { return _itemCodeAModificar ?? (_itemCodeAModificar = new HashSet<string>()); }
        }

        private HashSet<string> _tipoCorteNoEncontrados;
        public HashSet<string> TipoCorteNoEncontrados
        {
            get { return _tipoCorteNoEncontrados ?? (_tipoCorteNoEncontrados = new HashSet<string>()); }
        }

        private HashSet<string> _tipoJuntaNoEncontradas;
        public HashSet<string> TipoJuntaNoEncontradas
        {
            get { return _tipoJuntaNoEncontradas ?? (_tipoJuntaNoEncontradas = new HashSet<string>()); }
        }

        private HashSet<string> _cedulasNoEncontradas;
        public HashSet<string> CedulasNoEncontradas
        {
            get { return _cedulasNoEncontradas ?? (_cedulasNoEncontradas = new HashSet<string>()); }
        }

        private HashSet<string> _diametrosNoEncontrados;
        public HashSet<string> DiametrosNoEncontrados
        {
            get { return _diametrosNoEncontrados ?? (_diametrosNoEncontrados = new HashSet<string>()); }
        }

        private HashSet<string> _errores;
        public HashSet<string> Errores
        {
            get { return _errores ?? (_errores = new HashSet<string>()); }
        }

        private HashSet<string> _erroresEnRegistros;

        public HashSet<string> ErroresEnRegistros
        {
            get { return _erroresEnRegistros ?? (_erroresEnRegistros = new HashSet<string>()); }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="archivos"></param>
        /// <param name="familiaAceros"></param>
        /// <param name="usuarioModifica"></param>
        public InterpreteDatos(int proyectoID, ArchivoSimple[] archivos, List<FamiliaAcero> familiaAceros, Guid usuarioModifica)
        {
            _proyectoID = proyectoID;
            _archivos = archivos;
            _spoolsProyecto = SpoolBO.Instance.ObtenerConJuntaMaterialCortePorProyecto(_proyectoID);
            _familiasAceroUi = familiaAceros ?? new List<FamiliaAcero>();
            _usuarioModifica = usuarioModifica;
            inicializaCatalogos();
        }

        /// <summary>
        /// Se encarga de traer de cache o base de datos la informacion necesaria por el proceso
        /// </summary>
        private void inicializaCatalogos()
        {
            _cedulasCatalogo = CedulaBO.Instance.ObtenerTodos();
            _tipoCortesCatalogo = TipoCorteBO.Instance.ObtenerTodos();
            _tipoJuntasCatalogo = TipoJuntaBO.Instance.ObtenerTodos();
            _itemCodeCatalogo = ItemCodeBO.Instance.ObtenerPorProyecto(_proyectoID);
            _diametrosCatalogo = DiametroBO.Instance.ObtenerTodos();
            _familiasAceroCatalogo = FamiliaAceroBO.Instance.ObtenerTodas();
            _familiasMaterialCatalogo = FamiliaMaterialBO.Instance.ObtenerTodas();

            _fabAreasCache = CacheCatalogos.Instance.ObtenerFabAreas();
            _cedulasCache = CacheCatalogos.Instance.ObtenerCedulas();
            _diametrosCache = CacheCatalogos.Instance.ObtenerDiametros();

           
        }

        /// <summary>
        /// Valida que los archivos sean la cantidad esperada
        /// </summary>
        private void validaCantidadArchivos()
        {
            if (_archivos.Length != _archivosEsperados)
            {
                Errores.Add(MensajesError.IngenieriaArchivosEsperados);
                Valido = false;
            }
        }

        /// <summary>
        /// Manda a procesar y registrar los catalogos
        /// </summary>
        /// <param name="familiaAceros"></param>
        /// <param name="spoolIdsNoHomologar"></param>
        /// <returns></returns>
        public bool ProcesaYRegistraSiEsNecesario(List<FamiliaAcero> familiaAceros, List<int> spoolIdsNoHomologar)
        {

            return EmpiezaProceso(true);
        }

        /// <summary>
        /// Manda construir el grafo, pero no registra nada en BD
        /// </summary>
        /// <returns></returns>
        public bool ProcesaSiNoFaltanDatos()
        {
            return EmpiezaProceso(false);
        }

        /// <summary>
        /// Se encarga de hacer el parsing, pero solo para llenar las colecciones contra las cuales se necesitan hacer validaciones posteriores
        /// no hace ninguna validacion de BL
        /// </summary>
        private void llenaColeccionesDeArchivo()
        {
            procesaSpools();
            _spoolsArchivo.ForEach(x => x.ConstruyeRegistro());
            procesaMateriales();
            _materialSpoolArchivo.ForEach(x => x.ConstruyeRegistro());
            procesaJuntas();
            _juntaSpoolArchivo.ForEach(x => x.ConstruyeRegistro());
            procesaCortes();
            _corteSpoolArchivo.ForEach(x => x.ConstruyeRegistro());
        }

        /// <summary>
        /// Se encarga de realizar el parsing aplicando validaciones BL contra las colecciones de archivo
        /// </summary>
        private void validaIntegridad()
        {
            _spoolsArchivo.ForEach(x => x.Valida());
            _juntaSpoolArchivo.ForEach(x => x.Valida());
            _materialSpoolArchivo.ForEach(x => x.Valida());
            _corteSpoolArchivo.ForEach(x => x.Valida());

            ErroresRegistroAErorresPagina();

        }

        /// <summary>
        /// Los errores de cada registro los agrega a la coleccion de Errores generales
        /// </summary>
        private void ErroresRegistroAErorresPagina()
        {
            _juntaSpoolArchivo.Where(x => !x.RegistroValido).ToList().ForEach(
                x =>
                x.ErroresRegistro.ForEach(
                    y =>
                    Errores.Add(string.Format(MensajesError.IngenieriaErrorGeneral, y, x.Archivo, (x.NumLinea + 1)))));
            _spoolsArchivo.Where(x => !x.RegistroValido).ToList().ForEach(
                x =>
                x.ErroresRegistro.ForEach(
                    y =>
                    Errores.Add(string.Format(MensajesError.IngenieriaErrorGeneral, y, x.Archivo, (x.NumLinea + 1)))));
            _materialSpoolArchivo.Where(x => !x.RegistroValido).ToList().ForEach(
                x =>
                x.ErroresRegistro.ForEach(
                    y =>
                    Errores.Add(string.Format(MensajesError.IngenieriaErrorGeneral, y, x.Archivo, (x.NumLinea + 1)))));
            _corteSpoolArchivo.Where(x => !x.RegistroValido).ToList().ForEach(
                x =>
                x.ErroresRegistro.ForEach(
                    y =>
                    Errores.Add(string.Format(MensajesError.IngenieriaErrorGeneral, y, x.Archivo, (x.NumLinea + 1)))));
        }

        /// <summary>
        /// Se encarga de Revisar que en la base de datos no existan spools con revisiones mayores y que los spools que estan siendo dados de alta no hayan sido despachados
        /// y se encarga de llenar los datos para homologacion
        /// </summary>
        /// <returns></returns>
        private bool revisaBd()
        {

            if (_spoolsProyecto.Count > 0)
            {
                //validamos revisiones
                IEnumerable<Spool> spoolsRevisionesDiferentes = from spoolProyecto in _spoolsProyecto
                                                                join spoolArchivo in _spoolsArchivo on spoolProyecto.Nombre equals
                                                                    spoolArchivo.Nombre
                                                                where !(spoolArchivo.Revision.CompareTo(spoolProyecto.Revision) == 0 &&
                                                                    spoolArchivo.RevisionCliente.CompareTo(
                                                                        spoolProyecto.RevisionCliente) == 0)
                                                                select spoolProyecto;

                //el where indica donde las revisiones no sean iguales y que cuando menos una revision haya subido
                IEnumerable<Spool> spoolsQueCambioRevision = from spoolsProyecto in _spoolsProyecto
                                                             join spoolsArchivo in _spoolsArchivo on spoolsProyecto.Nombre equals
                                                                 spoolsArchivo.Nombre
                                                             where
                                                                 !(spoolsArchivo.Revision.CompareTo(spoolsProyecto.Revision) == 0 &&
                                                                   spoolsArchivo.RevisionCliente.CompareTo(
                                                                       spoolsProyecto.RevisionCliente) ==
                                                                   0) &&
                                                                 spoolsArchivo.Revision.CompareTo(spoolsProyecto.Revision) >= 0 ||
                                                                 spoolsArchivo.RevisionCliente.CompareTo(
                                                                     spoolsProyecto.RevisionCliente) >=
                                                                 0
                                                             select spoolsArchivo;                

            }


            //Obtenemos una lista de los materiales, una combinacion de nombre de spool y etiqueta de material
            var queryMat = (from matSpool in _materialSpoolArchivo
                            select new
                            {
                                matSpool.Spool.Nombre,
                                matSpool.Etiqueta
                            }).ToList();

            //revisamos cuales materiales estan duplicados
            var etiquetasMaterialRepetidas = (from matSpool in queryMat
                                              group matSpool by new { matSpool.Nombre, matSpool.Etiqueta }
                                                  into g
                                                  where g.Count() > 1
                                                  select new { g.Key.Nombre, g.Key.Etiqueta }).ToList();

            //para cada material duplicado generamos un error
            etiquetasMaterialRepetidas.ForEach(x => Errores.Add(string.Format(MensajesError.IngenieriaErrorMaterialDup, x.Nombre, x.Etiqueta)));

            //Obtemenos una lista de las juntas, una combinacion del nombre del spool y la etiqueta de la junta 
            var queryJuntas = (from juntaSpool in _juntaSpoolArchivo
                               select new
                               {
                                   juntaSpool.Spool.Nombre,
                                   juntaSpool.Etiqueta,
                                   juntaSpool.EtiquetaMaterial1,
                                   juntaSpool.EtiquetaMaterial2
                               }).ToList();

            //obtenemos las juntas repetidas
            var etiquetasJuntasRepetidas = (from juntaSpool in queryJuntas
                                            group juntaSpool by new { juntaSpool.Nombre, juntaSpool.Etiqueta }
                                                into g
                                                where g.Count() > 1
                                                select new { g.Key.Nombre, g.Key.Etiqueta }).ToList();
            
            //Para cada junta generamos un error que se desplegara en UI
            etiquetasJuntasRepetidas.ForEach(x => Errores.Add(string.Format(MensajesError.IngenieriaErrorJuntaDup, x.Nombre, x.Etiqueta)));

            ////obtenemos las juntas con localización repetida
            //var localizacionJuntasRepetidas = (from juntaSpool in queryJuntas
            //                                  group juntaSpool by new {juntaSpool.Nombre, juntaSpool.EtiquetaMaterial1, juntaSpool.EtiquetaMaterial2}
            //                                  into g
            //                                  where g.Count() > 1
            //                                  select new { g.Key.Nombre, g.Key.EtiquetaMaterial1, g.Key.EtiquetaMaterial2 }).ToList();

            ////Para cada junta generamos un error que se desplegara en UI
            //localizacionJuntasRepetidas.ForEach(x => Errores.Add(string.Format(MensajesError.IngenieriaErrorLocDup, x.Nombre, x.EtiquetaMaterial1, x.EtiquetaMaterial2)));

            //Validamos que el archivo de spools no contenga spools duplicados
            var spoolsRepetidos = (from spool in _spoolsArchivo.Select(x => x.Nombre)
                                   group spool by new { spool }
                                       into g
                                       where g.Count() > 1
                                       select g.Key.spool).ToList();

            spoolsRepetidos.ForEach(x => Errores.Add(string.Format(MensajesError.IngenieriaErrorSpoolDup, x)));


            return Errores.Count == 0;
        }

       

        /// <summary>
        /// Revisamos que la info en los archivos sea consistente, que no vengan diferentes revisiones para el mismo spool
        /// </summary>
        /// <returns></returns>
        private bool validaRevisionesIntraArchivos()
        {
            bool error = false;

            foreach (SpoolIng spoolIng in _spoolsArchivo)
            {
                string spool = spoolIng.Nombre;
                string revision = spoolIng.Revision;
                string revisionCliente = spoolIng.RevisionCliente;

                foreach (CorteSpoolIng corte in spoolIng.CorteSpool)
                {
                    if (!revision.EqualsIgnoreCase(corte.Token[cs.RevisionSteelgo]) ||
                        !revisionCliente.EqualsIgnoreCase(corte.Token[cs.Revision]))
                    {
                        Errores.Add(string.Format(MensajesError.IngenieriaNoCoincidenRevisionesDetalle, spool,
                                                  corte.Archivo, corte.NumLinea + 1));
                        error = true;
                    }
                }
                foreach (MaterialSpoolIng material in spoolIng.MaterialSpool)
                {
                    if (!revision.EqualsIgnoreCase(material.Token[ms.RevSt]) ||
                        !revisionCliente.EqualsIgnoreCase(material.Token[ms.RevCte1]))
                    {
                        Errores.Add(string.Format(MensajesError.IngenieriaNoCoincidenRevisionesDetalle, spool,
                                                  material.Archivo, material.NumLinea + 1));
                        error = true;
                    }
                }
                foreach (JuntaSpoolIng junta in spoolIng.JuntaSpool)
                {
                    if (!revision.EqualsIgnoreCase(junta.Token[js.RevisionSteelgo]) ||
                        !revisionCliente.EqualsIgnoreCase(junta.Token[js.RevisionCliente]))
                    {
                        Errores.Add(string.Format(MensajesError.IngenieriaNoCoincidenRevisionesDetalle, spool,
                                                  junta.Archivo, junta.NumLinea + 1));
                        error = true;
                    }
                }

            }

            return !error;
        }

        /// <summary>
        /// Proceso que construye y registra el grafo en BD
        /// </summary>
        /// <param name="registrarNoEncontrados"></param>
        /// <returns></returns>
        public bool EmpiezaProceso(bool registrarNoEncontrados)
        {
            //validamos que sean 4 archivos
            validaCantidadArchivos();
            if (Valido)
            {
                try
                {
                    //llenamos las colecciones desde los archivos
                    llenaColeccionesDeArchivo();                    

                    //regeneramos los archivos para el dtsx solo la primera vez!
                    if (!registrarNoEncontrados && !regeneraArchivos())
                    {
                        Errores.Add("No se pudieron regenerar los archivos");
                    }


                    //aplicamos las validaciones de integridad de archivos
                    validaIntegridad();

                    // validamos descripción única en item codes a dar de alta
                    (from ic in _itemCodeArchivo
                     group ic by ic.Split('|')[0] into g
                     where g.Distinct().Count() > 1
                     select g.Key)
                     .ToList()
                     .ForEach(x => Errores.Add(string.Format(MensajesError.ItemCode_DescripcionNoUnica, x.Split('|')[0])));


                    //si no existieron errores procedemos
                    if (Errores.Count == 0)
                    {
                        //validamos informacion obtenida de archivos, que no tengan revisiones diferentes los spools contra sus materiales
                        if (validaRevisionesIntraArchivos())
                        {
                            // si la info de los archivos es consistente
                            if (revisaBd())
                            {
                                //guardamos la info parseada en cache
                                SpoolsArchivoCache = new List<SpoolIng>(_spoolsArchivo);

                             
                                //Si es la confirmacion por parte del usuario
                                if (registrarNoEncontrados)
                                {

                                    string lineaDtsxConArgumentos = Configuracion.LineaDtsx + " " + ObtenArgumentosDtsx();

                                    //Dar de alta en catalogos, llamar dts
                                    IngenieriaBL.Instance.RegistraCatalogos(ItemCodeNoEncontrados,
                                                                            CedulasNoEncontradas,
                                                                            DiametrosNoEncontrados,
                                                                            FabAreasNoEncontradas,
                                                                            TipoCorteNoEncontrados,
                                                                            TipoJuntaNoEncontradas, _familiasAceroUi, _proyectoID , lineaDtsxConArgumentos, ItemCodeAModificar);

                                    
                                    return true;
                                }
                            }
                        }
                    }
                    else
                    {
                        ErroresRegistroAErorresPagina();
                    }

                }
                catch (Exception e)
                {
                    _log.Debug(e, e);
                    throw;
                }
            }

            return esProcesoValido();
        }

        private bool regeneraArchivos()
        {
            try
            {

                //public static readonly string[] NombresArchivos = { "cutlist.csv", "material.csv", "spools.csv", "welds.csv" };
                //revisamos si existen los archivos que es casi seguro q si, los borramos
                foreach (string nombreArchivo in NombresArchivos)
                {
                    if(File.Exists(PathParaAlmacenarArchivos + nombreArchivo))
                    {
                        File.Delete(PathParaAlmacenarArchivos + nombreArchivo);
                    }
                }

                using (TextWriter tw = File.CreateText(PathParaAlmacenarArchivos + NombresArchivos[0]))
                {
                    foreach (CorteSpoolIng cs in _corteSpoolArchivo)
                    {
                        for (int i = 0; i < Enum.GetNames(typeof (CorteSpoolIng.Columna)).Count(); i++)
                        {
                            tw.Write(cs.Token[(CorteSpoolIng.Columna) i] +
                                     (i + 1 < Enum.GetNames(typeof(CorteSpoolIng.Columna)).Count() ? "," : string.Empty));
                        }
                        tw.WriteLine();
                    }
                    tw.Flush();
                    tw.Close();
                }

              
                using (TextWriter tw = File.CreateText(PathParaAlmacenarArchivos + NombresArchivos[1]))
                {
                    foreach (MaterialSpoolIng ms in _materialSpoolArchivo)
                    {
                        for (int i = 0; i < Enum.GetNames(typeof(MaterialSpoolIng.Columna)).Count(); i++)
                        {
                            tw.Write(ms.Token[(MaterialSpoolIng.Columna) i] +
                                     (i +1 < Enum.GetNames(typeof (MaterialSpoolIng.Columna)).Count() ? "," : string.Empty));
                        }
                        tw.WriteLine();
                    }
                    tw.Flush();
                    tw.Close();
                }

                using (TextWriter tw = File.CreateText(PathParaAlmacenarArchivos + NombresArchivos[2]))
                {
                    foreach (SpoolIng spool in _spoolsArchivo)
                    {
                        for (int i = 0; i < Enum.GetNames(typeof(SpoolIng.Columna)).Count(); i++)
                        {
                            tw.Write(spool.Token[(SpoolIng.Columna)i] +
                                     (i + 1 < Enum.GetNames(typeof(SpoolIng.Columna)).Count() ? "," : string.Empty));
                        }
                        tw.WriteLine();
                    }
                    tw.Flush();
                    tw.Close();
                }

                using (TextWriter tw = File.CreateText(PathParaAlmacenarArchivos + NombresArchivos[3]))
                {
                    foreach (JuntaSpoolIng cs in _juntaSpoolArchivo)
                    {
                        for (int i = 0; i < Enum.GetNames(typeof(JuntaSpoolIng.Columna)).Count(); i++)
                        {
                            tw.Write(cs.Token[(JuntaSpoolIng.Columna)i] +
                                     (i + 1 < Enum.GetNames(typeof(JuntaSpoolIng.Columna)).Count() ? "," : string.Empty));
                        }
                        tw.WriteLine();
                    }
                    tw.Flush();
                    tw.Close();
                }


                return true;
            }catch
            {
                return false;
            }

        }

        private string ObtenArgumentosDtsx()
        {
            const string templateSetVariable = @"/SET \package.Variables[User::{0}].Value;""{1}""";
            const string templateSetConnStringVariable = @"/SET \package.Variables[User::{0}].Value;""\""{1}""\""";
            string mainDtsxArgs = "/f \"" + Configuracion.UbicacionDtsx + "\"";
            mainDtsxArgs += " " + string.Format(templateSetConnStringVariable, "SAMAppDBConnString", Configuracion.DtsxDBConnString);
            mainDtsxArgs +=  " " + string.Format(templateSetVariable, "SAMDTSPackPathName", Configuracion.UbicacionDtsx);
            mainDtsxArgs += " " + string.Format(templateSetVariable, "SAMDTSRawFilesDirectoryPath", Configuracion.DBServerRawFilesDirectory+ " ");
            mainDtsxArgs +=  " " + string.Format(templateSetVariable, "SAMWebProjectID", _proyectoID);
            mainDtsxArgs +=  " " + string.Format(templateSetVariable, "SAMWebSessionID", HttpContext.Current.Session.SessionID);
            mainDtsxArgs += " " + string.Format(templateSetVariable, "SAMWebUploadedFilesDirectoryPath", Configuracion.DBServerUploadedFilesDirectory + " ");
            mainDtsxArgs += " " + string.Format(templateSetVariable, "SAMWebUsuarioModificaID", _usuarioModifica);
            mainDtsxArgs += " " + string.Format(templateSetVariable, "SAMDTSValidaCatalogos", Configuracion.ValidaCatalogosDtsx);
            mainDtsxArgs += " " + string.Format(templateSetVariable, "SAMDTSValidaIntegridad", Configuracion.ValidaIntegridadDtsx);
            return mainDtsxArgs;
        }


        /// <summary>
        /// Reregresa Falso si en el proceso falta algun elemento de algun catalogo o ha ocurrido algun error
        /// </summary>
        /// <returns></returns>
        private bool esProcesoValido()
        {
            return FabAreasNoEncontradas.Count() == 0 && ItemCodeNoEncontrados.Count() == 0 &&
                   FamiliaAceroNoEncontradas.Count() == 0 && TipoJuntaNoEncontradas.Count() == 0 &&
                   CedulasNoEncontradas.Count() == 0 && DiametrosNoEncontrados.Count() == 0 &&
                   TipoCorteNoEncontrados.Count() == 0 && Errores.Count == 0;
        }

        #region Spools

        /// <summary>
        /// Se encarga de procesar el stream de Spool-Info, construye los objetos SpoolIng para que posteriormente sean Validados
        /// </summary>
        private void procesaSpools()
        {
            ArchivoSimple archivo = _archivos[2];
            string[] lineas = archivo.Lineas;
            if (lineas.Length == 0)
            {
                Errores.Add(string.Format(MensajesError.IngenieriaArchivoVacio, archivo.Nombre));
            }

            for (int numLinea = 0; numLinea < lineas.Length; numLinea++)
            {

                string linea = lineas[numLinea];
                string[] palabras = linea.Split(',');
                palabras = palabras.ToList().Select(p => p.Trim()).ToArray();

                if (palabras.Length > 1 && palabras[1] != string.Empty) //Esto nos indica que no son encabezados
                {
                    //Validamos la longitud del registro
                    if (palabras.Length != Enum.GetValues(typeof(sp)).Length)
                    {
                        if (palabras.Length > Enum.GetValues(typeof(sp)).Length)
                        {
                            Errores.Add(string.Format(MensajesError.IngenieriaColumnasDeMas, archivo.Nombre,
                                                      numLinea + 1));
                        }
                        else
                        {
                            Errores.Add(string.Format(MensajesError.IngenieriaColumnasDeMenos, archivo.Nombre,
                                                      numLinea + 1));
                        }
                        continue;
                    }

                    //Por eventos se iran construyendo los SpoolIng, cuando se mande llamar el metodo Construye, se ejecutara construccionRegistroSpool,
                    //Una vez que el SpoolIng se mande validar la funcion "validaRegistroSpool" se encargara de validarlo y en caso de ser un registro valido,
                    //se ejecutara la funcion de "spoolValidado"
                    SpoolIng spoolIng = new SpoolIng(numLinea, archivo.Nombre, palabras, _usuarioModifica);
                    spoolIng.Construye += construccionRegistroSpool;
                    spoolIng.ValidaRegistro += validaRegistroSpool;
                    spoolIng.ValidaIntegridadRegistro += validaIntegridadRegistroSpool;
                    spoolIng.SpoolValidado += spoolValidado;

                    _spoolsArchivo.Add(spoolIng);
                }
            }
            NumSpoolsEnArchivo = _spoolsArchivo.Count;
        }


        /// <summary>
        /// funcion que se encarga de construir un objeto SpoolIng apartir de los valores recibidos en los csv
        /// </summary>
        /// <param name="sender"></param>
        private bool validaIntegridadRegistroSpool(object sender)
        {
            SpoolIng spoolIng = (SpoolIng)sender;

            int col = 0;

            try
            {

                col = SpoolIng.Col[sp.Spool];
                spoolIng.Token[sp.Spool].NotNullNorEmpty();
                
                col = SpoolIng.Col[sp.Pdi];
                spoolIng.Token[sp.Pdi].IsDecimal(false);

                col = SpoolIng.Col[sp.Kg];
                spoolIng.Token[sp.Kg].IsDecimal(false);

                col = SpoolIng.Col[sp.M2];
                spoolIng.Token[sp.M2].IsDecimal(false);

                col = SpoolIng.Col[sp.Especificacion];
                spoolIng.Token[sp.Especificacion].NotNullNorEmpty();

                col = SpoolIng.Col[sp.PorcPnd];
                if ("no,none".IndexOf(spoolIng.Token[sp.PorcPnd].ToLower()) != -1)
                {
                    spoolIng.Token[sp.PorcPnd] = "0";
                }
                else if ("yes,si".IndexOf(spoolIng.Token[sp.PorcPnd].ToLower()) != -1)
                {
                    spoolIng.Token[sp.PorcPnd] = "1";
                }
                spoolIng.Token[sp.PorcPnd].IsInt(false);

                col = SpoolIng.Col[sp.RequierePwth];
                if("yes,si,no,none".IndexOf(spoolIng.Token[sp.RequierePwth].ToLower()) == -1)
                {
                    throw new Exception();
                }

                col = SpoolIng.Col[sp.DibujoRef];
                spoolIng.Token[sp.DibujoRef].NotNullNorEmpty();
                
                col = SpoolIng.Col[sp.RevCliente];
                spoolIng.Token[sp.RevCliente].NotNullNorEmpty();

                col = SpoolIng.Col[sp.DiamReal];
                spoolIng.Token[sp.DiamReal].IsDecimal(false);

                col = SpoolIng.Col[sp.RevSteelgo];
                spoolIng.Token[sp.RevSteelgo].NotNullNorEmpty();
            }
            catch (Exception)
            {
                Errores.Add(string.Format(MensajesError.IngenieriaErrorInterprete, spoolIng.Archivo, spoolIng.NumLinea + 1, col + 1));
                return false;
            }
            return true;
        }

        /// <summary>
        /// funcion que se encarga de construir un objeto SpoolIng apartir de los valores recibidos en los csv
        /// </summary>
        /// <param name="sender"></param>
        private void construccionRegistroSpool(object sender)
        {
            SpoolIng spoolIng = (SpoolIng)sender;

            int col = 0;
            
            try
            {

                col = SpoolIng.Col[sp.Spool];
                spoolIng.Nombre = spoolIng.Token[sp.Spool];
                col = SpoolIng.Col[sp.Pdi];
                spoolIng.Pdis = spoolIng.Token[sp.Pdi].SafeDecimalNullableParse();
                col = SpoolIng.Col[sp.Kg];
                spoolIng.Peso = spoolIng.Token[sp.Kg].SafeDecimalNullableParse();
                col = SpoolIng.Col[sp.M2];
                spoolIng.Area = spoolIng.Token[sp.M2].SafeDecimalNullableParse();
                col = SpoolIng.Col[sp.Especificacion];
                spoolIng.Especificacion = spoolIng.Token[sp.Especificacion];
                col = SpoolIng.Col[sp.PorcPnd];
                spoolIng.PorcentajePnd = spoolIng.Token[sp.PorcPnd].SafeIntParse(0);
                col = SpoolIng.Col[sp.RequierePwth];
                spoolIng.RequierePwht = spoolIng.Token[sp.RequierePwth].EqualsIgnoreCase("si") || spoolIng.Token[sp.RequierePwth].EqualsIgnoreCase("yes");
                col = SpoolIng.Col[sp.DibujoRef];
                spoolIng.Dibujo = spoolIng.Token[sp.DibujoRef];
                col = SpoolIng.Col[sp.RevCliente];
                spoolIng.RevisionCliente = spoolIng.Token[sp.RevCliente];
                col = SpoolIng.Col[sp.DiamReal];
                spoolIng.DiametroPlano = spoolIng.Token[sp.DiamReal].SafeDecimalParse();
                col = SpoolIng.Col[sp.RevSteelgo];
                spoolIng.Revision = spoolIng.Token[sp.RevSteelgo];
                col = SpoolIng.Col[sp.SistemaPintura];
                spoolIng.SistemaPintura = spoolIng.Token[sp.SistemaPintura];
            }
            catch (Exception)
            {
                Errores.Add(string.Format(MensajesError.IngenieriaErrorInterprete, spoolIng.Archivo, spoolIng.NumLinea + 1, col + 1));
            }
        }

        /// <summary>
        /// Se dispara cada vez que un spool fue validado
        /// </summary>
        /// <param name="sender"></param>
        private void spoolValidado(object sender)
        {
            SpoolIng spoolIng = (SpoolIng)sender;
            asignaFamiliasAceros(spoolIng);

        }

        /// <summary>
        /// Se encarga de asignas las familias Aceros al Spool
        /// </summary>
        /// <param name="spool"></param>
        private void asignaFamiliasAceros(SpoolIng spool)
        {
            string fam1 = spool.Token[sp.Material];
            FamiliaAcero familiaAcero;
            //con esto revisamos si es una o dos familias de acero
            if (fam1.Contains("/"))
            {
                string fam2 = fam1.Split('/')[1].Trim();
                fam1 = fam1.Split('/')[0].Trim();

                familiaAcero = _familiasAceroCatalogo.FirstOrDefault(x => x.Nombre.EqualsIgnoreCase(fam2));
                if (familiaAcero != null)
                {
                    spool.FamiliaAcero2ID = familiaAcero.FamiliaAceroID;
                }
            }

            familiaAcero = _familiasAceroCatalogo.FirstOrDefault(x => x.Nombre.EqualsIgnoreCase(fam1));
            if (familiaAcero != null)
            {
               spool.FamiliaAcero1ID = familiaAcero.FamiliaAceroID;
            }
        }

        /// <summary>
        /// Funcion que se encarga de validar al spool
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        private bool validaRegistroSpool(object sender)
        {
            SpoolIng spoolIng = (SpoolIng)sender;

            bool error = false;
            //revisamos que el nombre de la familia exista en los datos traidos originalmente de cache
            string famAcero1 = spoolIng.Token[sp.Material];


            //Que el dibujo no venga en blanco
            if (string.IsNullOrEmpty(spoolIng.Token[sp.DibujoRef]))
            {
                spoolIng.ErroresRegistro.Add(MensajesError.IngenieriaDibujoEnBlanco);
                error = true;
            }

            //Que exista la familia de acero
            if (string.IsNullOrEmpty(spoolIng.Token[sp.Material]))
            {
                spoolIng.ErroresRegistro.Add(string.Format(MensajesError.IngenieriaFamiliaInexistente,spoolIng.NumLinea + 1));
                error = true;
            }

            if (famAcero1.Contains("/"))
            {
                string famAcero2 = famAcero1.Split('/')[1].Trim();
                famAcero1 = famAcero1.Split('/')[0].Trim();
                existeFamAcero(famAcero2);
            }

            existeFamAcero(famAcero1);

            return !error;
        }

      

       
       
        #endregion

        #region Cortes

        /// <summary>
        /// Se encarga de procesar el stream de Cortes, construye los objetos CorteSpoolIng para que posteriormente sean Validados
        /// </summary>
        private void procesaCortes()
        {
            ArchivoSimple archivo = _archivos[0];
            string[] lineas = archivo.Lineas;
            
            for (int numLinea = 0; numLinea < lineas.Length; numLinea++)
            {

                string linea = lineas[numLinea];
                string[] palabras = linea.Split(',');
                palabras = palabras.ToList().Select(p => p.Trim()).ToArray();

                if (palabras.Length > 1 && palabras[1] != string.Empty)
                {
                    if (palabras.Length != Enum.GetValues(typeof(cs)).Length)
                    {
                        if (palabras.Length > Enum.GetValues(typeof(cs)).Length)
                        {
                            Errores.Add(string.Format(MensajesError.IngenieriaColumnasDeMas, archivo.Nombre,
                                                      numLinea + 1));
                        }
                        else
                        {
                            Errores.Add(string.Format(MensajesError.IngenieriaColumnasDeMenos, archivo.Nombre,
                                                      numLinea + 1));
                        }
                        continue;
                    }

                    CorteSpoolIng corteSpoolIng = new CorteSpoolIng(numLinea, archivo.Nombre, palabras, _usuarioModifica);
                    corteSpoolIng.Construye += construcionRegistroCorte;
                    corteSpoolIng.ValidaRegistro += validaRegistroCorteSpool;
                    corteSpoolIng.ValidaIntegridadRegistro += validaIntegridadRegistroCorteSpool;
                    corteSpoolIng.CorteSpoolValidado += corteSpoolValidado;
                    _corteSpoolArchivo.Add(corteSpoolIng);
                }
            }
        }

        /// <summary>
        /// Funcion que se encarga de la validacion del registro
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        private bool validaRegistroCorteSpool(object sender)
        {
            CorteSpoolIng corteSpoolIng = (CorteSpoolIng)sender;
            bool errorRegistro = false;


            //revisamos que el diametro 1 no sea cero
            decimal d1, d2;
            calculaDiametros(corteSpoolIng.Token[cs.Diametro], out d1, out d2);
            if (d1 <= 0)
            {
                corteSpoolIng.ErroresRegistro.Add(MensajesError.IngenieriaCorteDiametroCero);
                errorRegistro = true;
            }

            if (corteSpoolIng.Token[cs.EtiquetaDeLocalizacion].Length > 10)
            {
                corteSpoolIng.ErroresRegistro.Add(MensajesError.IngenieriaLongitudEtiqueta);
                errorRegistro = true;
            }



            string material1 = corteSpoolIng.Token[cs.EtiquetaDeLocalizacion];

            if (material1.Trim() != "*")
            {
                MaterialSpoolIng mat = _materialSpoolArchivo.SingleOrDefault(x => x.Etiqueta == material1 && x.Token[ms.Spool] == corteSpoolIng.Token[cs.NombreDeSpool]);
                if (mat == null)
                {
                    Errores.Add(string.Format(MensajesError.IngenieriaMaterialNoEncontradoCorte, material1, corteSpoolIng.Token[cs.NombreDeSpool]));
                    errorRegistro = true;
                }
                else
                {
                    if (mat.Cantidad != corteSpoolIng.Token[cs.Cantidad].SafeIntParse() || mat.Diametro1 != d1 || mat.Diametro2 != d2)
                    {
                        Errores.Add(string.Format(MensajesError.IngenieriaMaterialCorteCampoNoCoincide, corteSpoolIng.Token[cs.NombreDeSpool], material1));
                        errorRegistro = true;
                    }
                }

            }

            if (corteSpoolIng.Token[cs.EtiquetaDeSeccionDeTubo].Length > 10)
            {
                corteSpoolIng.ErroresRegistro.Add(MensajesError.IngenieriaLongitudEtiqueta);
                errorRegistro = true;
            }           

            revisaItemCode(corteSpoolIng.Token[cs.ItemCode], corteSpoolIng.Token[cs.DescripcionDeMaterial], corteSpoolIng.Token[cs.DescripcionDeMaterial]);

            //validamos revision buscando los spools por isometrico
            if (!existeSpoolEnIsometrico(corteSpoolIng.Token[cs.DibujoReferencia], corteSpoolIng.Token[cs.NombreDeSpool]))
            {
                //spool no encontrado
                corteSpoolIng.ErroresRegistro.Add(string.Format(MensajesError.IngenieriaSpoolNoEncontrado, corteSpoolIng.Token[cs.NombreDeSpool], corteSpoolIng.Archivo));
                Errores.Add(string.Format(MensajesError.IngenieriaSpoolNoEncontrado, corteSpoolIng.Token[cs.NombreDeSpool], corteSpoolIng.Archivo));
                errorRegistro = true;
            }
            else if (!conincidenRevisiones(corteSpoolIng.Token[cs.NombreDeSpool], corteSpoolIng.Token[cs.Revision], corteSpoolIng.Token[cs.Revision]))
            {
                //no coinciden las revisiones
                corteSpoolIng.ErroresRegistro.Add(string.Format(MensajesError.IngenieriaNoCoincidenRevisiones, corteSpoolIng.Archivo));
                errorRegistro = true;
            }

            //
            if (string.IsNullOrEmpty(corteSpoolIng.Token[cs.TipoDeCorteTramoInicio]))
            {
                corteSpoolIng.ErroresRegistro.Add(MensajesError.TipoCorteEnBlanco);
                errorRegistro = true;
            }
            if (string.IsNullOrEmpty(corteSpoolIng.Token[cs.TipoCorteTramoFinal]))
            {
                corteSpoolIng.ErroresRegistro.Add(MensajesError.TipoCorteEnBlanco);
                errorRegistro = true;
            }
            revisaTipoCorte(corteSpoolIng.Token[cs.TipoDeCorteTramoInicio]);
            revisaTipoCorte(corteSpoolIng.Token[cs.TipoCorteTramoFinal]);

            return !errorRegistro;
        }


        

        /// <summary>
        /// funcion que se encarga de revisar la integridad de un registro  CorteSpoolIng apartir de los valores recibidos en los csv
        /// </summary>
        /// <param name="sender"></param>
        private bool validaIntegridadRegistroCorteSpool(object sender)
        {
            CorteSpoolIng corteSpoolIng = (CorteSpoolIng)sender;
            int col = 0;
            try
            {
                col = CorteSpoolIng.Col[cs.DibujoReferencia];
                corteSpoolIng.Token[cs.DibujoReferencia].NotNullNorEmpty();

                col = CorteSpoolIng.Col[cs.Revision];
                corteSpoolIng.Token[cs.Revision].NotNullNorEmpty();

                col = CorteSpoolIng.Col[cs.NombreDeSpool];
                corteSpoolIng.Token[cs.NombreDeSpool].NotNullNorEmpty();

                col = CorteSpoolIng.Col[cs.ItemCode];
                corteSpoolIng.Token[cs.ItemCode].NotNullNorEmpty();

                col = CorteSpoolIng.Col[cs.Especificacion];
                corteSpoolIng.Token[cs.Especificacion].NotNullNorEmpty();

                decimal d1;
                decimal d2;
                col = CorteSpoolIng.Col[cs.Diametro];
                calculaDiametros(corteSpoolIng.Token[cs.Diametro], out d1, out d2);
                if (d1 == 0)
                {
                    throw new Exception();
                }

                col = CorteSpoolIng.Col[cs.DescripcionDeMaterial];
                corteSpoolIng.Token[cs.DescripcionDeMaterial].NotNullNorEmpty();

                col = CorteSpoolIng.Col[cs.EtiquetaDeSeccionDeTubo];
                corteSpoolIng.Token[cs.EtiquetaDeSeccionDeTubo].NotNullNorEmpty();

                col = CorteSpoolIng.Col[cs.EtiquetaDeLocalizacion];
                corteSpoolIng.Token[cs.EtiquetaDeLocalizacion].NotNullNorEmpty();
                
                col = CorteSpoolIng.Col[cs.Cantidad];
                corteSpoolIng.Token[cs.Cantidad].IsInt(false);

                col = CorteSpoolIng.Col[cs.Observaciones];
                corteSpoolIng.Observaciones = corteSpoolIng.Token[cs.Observaciones];

                col = CorteSpoolIng.Col[cs.TipoDeCorteTramoInicio];
                corteSpoolIng.Token[cs.TipoDeCorteTramoInicio].NotNullNorEmpty();

                col = CorteSpoolIng.Col[cs.TipoCorteTramoFinal];
                corteSpoolIng.Token[cs.TipoCorteTramoFinal].NotNullNorEmpty();


            }
            catch (Exception)
            {
                Errores.Add(string.Format(MensajesError.IngenieriaErrorInterprete, corteSpoolIng.Archivo, corteSpoolIng.NumLinea + 1, col + 1));
                return false;
            }
            return true;
        }

        /// <summary>
        /// funcion que se encarga de construir un objeto CorteSpoolIng apartir de los valores recibidos en los csv
        /// </summary>
        /// <param name="sender"></param>
        private void construcionRegistroCorte(object sender)
        {
            CorteSpoolIng corteSpoolIng = (CorteSpoolIng)sender;
            int col = 0;
            try
            {
                decimal d1;
                decimal d2;
                col = CorteSpoolIng.Col[cs.Diametro];
                calculaDiametros(corteSpoolIng.Token[cs.Diametro], out d1, out d2);
                corteSpoolIng.Diametro = d1;
                col = CorteSpoolIng.Col[cs.EtiquetaDeSeccionDeTubo];
                corteSpoolIng.EtiquetaSeccion = corteSpoolIng.Token[cs.EtiquetaDeSeccionDeTubo];
                col = CorteSpoolIng.Col[cs.Cantidad];
                corteSpoolIng.Cantidad = int.Parse(corteSpoolIng.Token[cs.Cantidad]);
                col = CorteSpoolIng.Col[cs.Observaciones];
                corteSpoolIng.Observaciones = corteSpoolIng.Token[cs.Observaciones];
                col = CorteSpoolIng.Col[cs.EtiquetaDeLocalizacion];
                corteSpoolIng.EtiquetaMaterial = corteSpoolIng.Token[cs.EtiquetaDeLocalizacion];
                col = CorteSpoolIng.Col[cs.Especificacion];
                corteSpoolIng.InicioFin = corteSpoolIng.Token[cs.Especificacion];


            }
            catch (Exception)
            {
                Errores.Add(string.Format(MensajesError.IngenieriaErrorInterprete, corteSpoolIng.Archivo, corteSpoolIng.NumLinea + 1, col + 1));
            }
        }

        /// <summary>
        /// Funcion que se dispara cada vez que un corteSpool es validado
        /// </summary>
        /// <param name="sender"></param>
        private void corteSpoolValidado(object sender)
        {
            CorteSpoolIng corteSpoolIng = (CorteSpoolIng)sender;

            corteSpoolIng.Spool = new Spool { Nombre = corteSpoolIng.Token[cs.NombreDeSpool] };
            corteSpoolIng.Spool = _spoolsArchivo.Where(x => x.Nombre.EqualsIgnoreCase(corteSpoolIng.Spool.Nombre)).First();
        }

        #endregion

        #region Materiales

        /// <summary>
        /// Se encarga de procesar el stream de Materiales, construye los objetos MaterialSpoolIng para que posteriormente sean Validados
        /// </summary>
        private void procesaMateriales()
        {
            ArchivoSimple archivo = _archivos[1];
            string[] lineas = archivo.Lineas;
            if (lineas.Length == 0)
            {
                Errores.Add(string.Format(MensajesError.IngenieriaArchivoVacio, archivo.Nombre));
            }

            for (int numLinea = 0; numLinea < lineas.Length; numLinea++)
            {
                string linea = lineas[numLinea];
                string[] palabras = linea.Split(',');
                palabras = palabras.ToList().Select(p => p.Trim()).ToArray();

                if (palabras.Length > 1 && palabras[1] != string.Empty)//como esperamos 12 columnas (2 en blanco) esto debe ser un registro valido
                {
                    if (palabras.Length != Enum.GetValues(typeof(ms)).Length)
                    {
                        if (palabras.Length > Enum.GetValues(typeof(ms)).Length)
                        {
                            Errores.Add(string.Format(MensajesError.IngenieriaColumnasDeMas, archivo.Nombre,
                                                      numLinea + 1));
                        }
                        else
                        {
                            Errores.Add(string.Format(MensajesError.IngenieriaColumnasDeMenos, archivo.Nombre,
                                                      numLinea + 1));
                        }
                        continue;
                    }

                    if (palabras[(int)ms.Especificacion].Length > 10)
                    {
                        palabras[(int)ms.Especificacion] = palabras[(int)ms.Especificacion].Substring(0, 10);
                    }
                    MaterialSpoolIng materialSpoolIng = new MaterialSpoolIng(numLinea, archivo.Nombre, palabras, _usuarioModifica);
                    materialSpoolIng.Construye += construccionRegistroMateriales;
                    materialSpoolIng.ValidaRegistro += validaRegistroMaterialSpool;
                    materialSpoolIng.ValidaIntegridadRegistro += validaIntegridadRegistroMaterialSpool;
                    materialSpoolIng.MaterialSpoolValidado += materialValidado;

                    _materialSpoolArchivo.Add(materialSpoolIng);
                }
            }

            // Validamos que un item code no contenga diferentes descripciones
            var IcConDescripcionMultiple = (from msa in _materialSpoolArchivo
                                            group msa by msa.Token[ms.Ic] into g
                                            where (g.Select(x => x.Token[ms.Descripcion]).Distinct().Count() > 1)
                                            select g).ToList();

            if (IcConDescripcionMultiple.Count != 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var ic in IcConDescripcionMultiple)
                    sb.Append("<br />" + ic.Key);

                Errores.Add(string.Format(MensajesError.IcConDescripcionMultiple,
                                          sb.ToString()));
            }
        }


        /// <summary>
        /// funcion que se encarga de construir un objeto MaterialSpoolIng apartir de los valores recibidos en los csv
        /// </summary>
        /// <param name="sender"></param>
        private bool validaIntegridadRegistroMaterialSpool(object sender)
        {
            MaterialSpoolIng materialSpool = (MaterialSpoolIng)sender;

            int col = 0;
            try
            {
                decimal d1;
                decimal d2;
                col = MaterialSpoolIng.Col[ms.Diametro];
                calculaDiametros(materialSpool.Token[ms.Diametro], out d1, out d2);
                
                col = MaterialSpoolIng.Col[ms.EtLoc];
                materialSpool.Token[ms.EtLoc].NotNullNorEmpty();

                col = MaterialSpoolIng.Col[ms.Ic];
                materialSpool.Token[ms.Ic].NotNullNorEmpty();

                col = MaterialSpoolIng.Col[ms.Cantidad];
                string cantidad = materialSpool.Token[ms.Cantidad].Replace("MM", string.Empty);
                cantidad.IsInt(false);
                
                col = MaterialSpoolIng.Col[ms.PesoKgs];
                string peso = materialSpool.Token[ms.PesoKgs];
                peso.IsDecimal(true);
                
                col = MaterialSpoolIng.Col[ms.Especificacion];
                materialSpool.Token[ms.Especificacion].NotNullNorEmpty();

                col = MaterialSpoolIng.Col[ms.ClasifMat];
                materialSpool.Token[ms.ClasifMat].NotNullNorEmpty();

                col = MaterialSpoolIng.Col[ms.RevCte1];
                materialSpool.Token[ms.RevCte1].NotNullNorEmpty();


            }
            catch (Exception)
            {
                Errores.Add(string.Format(MensajesError.IngenieriaErrorInterprete, materialSpool.Archivo, materialSpool.NumLinea + 1, col + 1));
                return false;
            }
            return true;
        }

        /// <summary>
        /// funcion que se encarga de construir un objeto MaterialSpoolIng apartir de los valores recibidos en los csv
        /// </summary>
        /// <param name="sender"></param>
        private void construccionRegistroMateriales(object sender)
        {
            MaterialSpoolIng materialSpool = (MaterialSpoolIng)sender;

            int col = 0;
            try
            {
                decimal d1;
                decimal d2;
                col = MaterialSpoolIng.Col[ms.Diametro];
                calculaDiametros(materialSpool.Token[ms.Diametro], out d1, out d2);
                materialSpool.Diametro1 = d1;
                materialSpool.Diametro2 = d2;
                col = MaterialSpoolIng.Col[ms.EtLoc];
                materialSpool.Etiqueta = materialSpool.Token[ms.EtLoc];
                col = MaterialSpoolIng.Col[ms.Cantidad];
                string cantidad = materialSpool.Token[ms.Cantidad].Replace("MM", string.Empty);
                materialSpool.Cantidad = cantidad.Equals(string.Empty) ? 0 : cantidad.SafeIntParse();
                col = MaterialSpoolIng.Col[ms.PesoKgs];
                string peso = materialSpool.Token[ms.PesoKgs];
                materialSpool.Peso = peso.Equals(string.Empty) ? 0 : peso.SafeDecimalParse();
                col = MaterialSpoolIng.Col[ms.Especificacion];
                materialSpool.Especificacion = materialSpool.Token[ms.Especificacion];
                col = MaterialSpoolIng.Col[ms.ClasifMat];
                materialSpool.Grupo = materialSpool.Token[ms.ClasifMat];
            }
            catch (Exception)
            {
                Errores.Add(string.Format(MensajesError.IngenieriaErrorInterprete, materialSpool.Archivo, materialSpool.NumLinea + 1, col + 1));
            }
        }

        /// <summary>
        /// Funcion que se dispara cuando cada material es validado
        /// </summary>
        /// <param name="sender"></param>
        private void materialValidado(object sender)
        {
            MaterialSpoolIng materialSpool = (MaterialSpoolIng)sender;


            materialSpool.Spool = _spoolsArchivo.Where(x => x.Nombre.EqualsIgnoreCase(materialSpool.Token[ms.Spool])).First();
        }

        /// <summary>
        /// Funcion que se encarga de validar el registro de Material Spool
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        private bool validaRegistroMaterialSpool(object sender)
        {
            MaterialSpoolIng materialSpool = (MaterialSpoolIng)sender;

            bool errorRegistro = false;

            try
            {

                if (materialSpool.Token[ms.EtLoc].Length > 10)
                {
                    materialSpool.ErroresRegistro.Add(MensajesError.IngenieriaLongitudEtiqueta);
                    errorRegistro = true;
                }
                if (materialSpool.Token[ms.Especificacion].Length > 10)
                {
                    materialSpool.ErroresRegistro.Add(MensajesError.IngenieriaLongitudEtiqueta);
                    errorRegistro = true;
                }

                if (!existeSpoolEnIsometrico(materialSpool.Token[ms.DibujoRef], materialSpool.Token[ms.Spool]))
                {

                    materialSpool.ErroresRegistro.Add(string.Format(MensajesError.IngenieriaSpoolNoEnIsometrico,
                                                                    materialSpool.Token[ms.DibujoRef],
                                                                    materialSpool.Token[ms.Spool]));
                    errorRegistro = true;
                }               

                revisaItemCode(materialSpool.Token[ms.Ic], materialSpool.Token[ms.Descripcion], materialSpool.Token[ms.ClasifMat]);
                
                //diametro
                decimal d1, d2;
                calculaDiametros(materialSpool.Token[ms.Diametro], out d1, out d2);

                //revisamos que el diametro 1 no sea cero
                if (d1 <= 0)
                {
                    materialSpool.ErroresRegistro.Add(MensajesError.IngenieriaMaterialDiametroCero);
                    errorRegistro = true;
                }

                revisaDiametro(d1);

                if (d2 > 0)
                {
                    revisaDiametro(d2);
                }


                if (!existeSpoolEnIsometrico(materialSpool.Token[ms.DibujoRef], materialSpool.Token[ms.Spool]))
                {
                    materialSpool.ErroresRegistro.Add(string.Format(MensajesError.IngenieriaSpoolNoEncontrado, materialSpool.Token[ms.Spool], materialSpool.Archivo));
                    Errores.Add(string.Format(MensajesError.IngenieriaSpoolNoEncontrado, materialSpool.Token[ms.Spool], materialSpool.Archivo));

                    errorRegistro = true;
                }


            }
            catch (Exception)
            {
                Errores.Add(string.Format(MensajesError.IngenieriaErrorValidacionArchivo, materialSpool.Archivo,
                                          materialSpool.NumLinea));
            }
            return !errorRegistro;
        }

        #endregion

        #region Juntas

        /// <summary>
        /// Se encarga de procesar el stream de Junta, construye los objetos JuntaSpoolIng para que posteriormente sean Validados
        /// </summary>
        private void procesaJuntas()
        {
            ArchivoSimple archivo = _archivos[3];
            string[] lineas = archivo.Lineas;
            
            for (int numLinea = 0; numLinea < lineas.Length; numLinea++)
            {
                string linea = lineas[numLinea];
                string[] palabras = linea.Split(',');
                palabras = palabras.ToList().Select(p => p.Trim()).ToArray();

                if (palabras.Length > 1 && palabras[1] != string.Empty)//como esperamos 12 columnas (2 en blanco) esto debe ser un registro valido
                {
                    if (palabras.Length != Enum.GetValues(typeof(js)).Length)
                    {
                        if (palabras.Length > Enum.GetValues(typeof(js)).Length)
                        {
                            Errores.Add(string.Format(MensajesError.IngenieriaColumnasDeMas, archivo.Nombre,
                                                      numLinea + 1));
                        }
                        else
                        {
                            Errores.Add(string.Format(MensajesError.IngenieriaColumnasDeMenos, archivo.Nombre,
                                                      numLinea + 1));
                        }
                        continue;
                    }

                    JuntaSpoolIng juntaSpool = new JuntaSpoolIng(numLinea, archivo.Nombre, palabras, _usuarioModifica);
                    juntaSpool.JuntaValidada += juntaValidada;
                    juntaSpool.Construye += construccionRegistroJunta;
                    juntaSpool.ValidaRegistro += validaRegistroJuntaSpool;
                    juntaSpool.ValidaIntegridadRegistro += validaIntegridadRegistroJuntaSpool;
                    _juntaSpoolArchivo.Add(juntaSpool);
                }
            }
        }

        /// <summary>
        /// funcion que se encarga de validar la integridad de un objeto JuntaSpoolIng apartir de los valores recibidos en los csv
        /// </summary>
        /// <param name="sender"></param>
        private bool validaIntegridadRegistroJuntaSpool(object sender)
        {
            JuntaSpoolIng juntaSpool = (JuntaSpoolIng)sender;
            int col = 0;
            try
            {
                col = JuntaSpoolIng.Col[js.NombreDeSpool];
                juntaSpool.Token[js.NombreDeSpool].NotNullNorEmpty();

                col = JuntaSpoolIng.Col[js.EtiquetaDeLocalizacion];
                juntaSpool.Token[js.EtiquetaDeLocalizacion].NotNullNorEmpty();

                decimal d1;
                decimal d2;
                col = JuntaSpoolIng.Col[js.Diametro];
                calculaDiametros(juntaSpool.Token[js.Diametro], out d1, out d2);
                if(d1 ==0)
                {
                    throw new Exception();
                }

                col = JuntaSpoolIng.Col[js.TipoDeJunta];
                juntaSpool.Token[js.TipoDeJunta].NotNullNorEmpty();
                
                col = JuntaSpoolIng.Col[js.Cedula];
                juntaSpool.Token[js.Cedula].NotNullNorEmpty();

                col = JuntaSpoolIng.Col[js.NumeroDeJunta];
                juntaSpool.Token[js.NumeroDeJunta].NotNullNorEmpty();

                col = JuntaSpoolIng.Col[js.TipoDeMaterial];
                juntaSpool.Token[js.TipoDeMaterial].NotNullNorEmpty();

            }
            catch (Exception)
            {
                Errores.Add(string.Format(MensajesError.IngenieriaErrorInterprete, juntaSpool.Archivo, juntaSpool.NumLinea + 1, col + 1));
                return false;
            }
            return true;
        }

        /// <summary>
        /// funcion que se encarga de construir un objeto JuntaSpoolIng apartir de los valores recibidos en los csv
        /// </summary>
        /// <param name="sender"></param>
        private void construccionRegistroJunta(object sender)
        {
            JuntaSpoolIng juntaSpool = (JuntaSpoolIng)sender;
            int col = 0;
            try
            {
                col = JuntaSpoolIng.Col[js.NumeroDeJunta];
                juntaSpool.Etiqueta = juntaSpool.Token[js.NumeroDeJunta];
                decimal d1;
                decimal d2;
                col = JuntaSpoolIng.Col[js.Diametro];
                calculaDiametros(juntaSpool.Token[js.Diametro], out d1, out d2);
                juntaSpool.Diametro = d1;
                col = JuntaSpoolIng.Col[js.Cedula];
                juntaSpool.Cedula = juntaSpool.Token[js.Cedula];
                col = JuntaSpoolIng.Col[js.EtiquetaDeLocalizacion];
                juntaSpool.EtiquetaMaterial1 = juntaSpool.Token[js.EtiquetaDeLocalizacion].Split('-')[0];
                juntaSpool.EtiquetaMaterial2 = juntaSpool.Token[js.EtiquetaDeLocalizacion].Split('-')[1];
            }
            catch (Exception)
            {
                Errores.Add(string.Format(MensajesError.IngenieriaErrorInterprete, juntaSpool.Archivo, juntaSpool.NumLinea + 1, col + 1));
            }
        }

        /// <summary>
        /// Funcion que se encarga de validar los registros de juntas
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        private bool validaRegistroJuntaSpool(object sender)
        {
            JuntaSpoolIng juntaSpoolIng = (JuntaSpoolIng)sender;
            bool errorRegistro = false;

            try
            {
                //revisamos que el diametro 1 no sea cero
                decimal d1, d2;
                calculaDiametros(juntaSpoolIng.Token[js.Diametro], out d1, out d2);
                if (d1 <= 0)
                {
                    juntaSpoolIng.ErroresRegistro.Add(MensajesError.IngenieriaJuntaDiametroCero);
                    errorRegistro = true;
                }

                if (juntaSpoolIng.Token[js.EtiquetaDeLocalizacion].Length > 10)
                {
                    juntaSpoolIng.ErroresRegistro.Add(MensajesError.IngenieriaLongitudEtiqueta);
                    errorRegistro = true;
                }


                if (string.IsNullOrEmpty(juntaSpoolIng.Token[js.TipoDeJunta]))
                {
                    juntaSpoolIng.ErroresRegistro.Add(MensajesError.IngenieraTipoJuntaEnBlanco);
                }

                //insertamos si no existe el tipo de junta
                revisaTipoJunta(juntaSpoolIng.Token[js.TipoDeJunta]);



                if (!existeFabArea(juntaSpoolIng.Token[js.Fabarea]))
                {
                    juntaSpoolIng.ErroresRegistro.Add(MensajesError.IngenieriaFabAreaNoEncontrada);
                    errorRegistro = true;
                }

                if (string.IsNullOrWhiteSpace(juntaSpoolIng.Token[js.TipoDeMaterial]))
                {
                    juntaSpoolIng.ErroresRegistro.Add(string.Format(MensajesError.IngenieriaFamiliaInexistente, juntaSpoolIng.NumLinea + 1));
                    errorRegistro = true;
                }

                //Familias Acero
                if (juntaSpoolIng.Token[js.TipoDeMaterial].Contains("/"))
                {
                    string fam1 = juntaSpoolIng.Token[js.TipoDeMaterial].Split('/')[0];
                    string fam2 = juntaSpoolIng.Token[js.TipoDeMaterial].Split('/')[1];

                    existeFamAcero(fam1);
                    existeFamAcero(fam2);
                }
                else
                {
                    existeFamAcero(juntaSpoolIng.Token[js.TipoDeMaterial]);
                } //TIPO DE MATERIAL


                if (string.IsNullOrEmpty(juntaSpoolIng.Token[js.Cedula]))
                {
                    juntaSpoolIng.ErroresRegistro.Add(MensajesError.IngenieriaCedulaEnBlanco);
                    errorRegistro = true;
                }

                revisaCedula(juntaSpoolIng.Token[js.Cedula]);


                if (!existeSpoolEnIsometrico(juntaSpoolIng.Token[js.DibujoRefrencia], juntaSpoolIng.Token[js.NombreDeSpool]))
                {
                    juntaSpoolIng.ErroresRegistro.Add(string.Format(MensajesError.IngenieriaSpoolNoEncontrado, juntaSpoolIng.Token[js.NombreDeSpool], juntaSpoolIng.Archivo));
                    Errores.Add(string.Format(MensajesError.IngenieriaSpoolNoEncontrado, juntaSpoolIng.Token[js.NombreDeSpool], juntaSpoolIng.Archivo));

                    errorRegistro = true;
                }

                if (!juntaSpoolIng.Token[js.Fabarea].EqualsIgnoreCase("FIELD") && juntaSpoolIng.EtiquetaMaterial1 != "*" && !_materialSpoolArchivo.Any(x => x.Token[ms.EtLoc] == juntaSpoolIng.EtiquetaMaterial1 && x.Token[ms.Spool] == juntaSpoolIng.Token[js.NombreDeSpool]))
                {
                    //Material 1 no existe
                    juntaSpoolIng.ErroresRegistro.Add(string.Format(MensajesError.IngenieriaErrorMaterialNoExiste, juntaSpoolIng.EtiquetaMaterial1, juntaSpoolIng.Token[js.NombreDeSpool], juntaSpoolIng.Archivo));
                    errorRegistro = true;
                }

                if (!juntaSpoolIng.Token[js.Fabarea].EqualsIgnoreCase("FIELD") && juntaSpoolIng.EtiquetaMaterial2 != "*" && !_materialSpoolArchivo.Any(x => x.Token[ms.EtLoc] == juntaSpoolIng.EtiquetaMaterial2 && x.Token[ms.Spool] == juntaSpoolIng.Token[js.NombreDeSpool]))
                {
                    //Material 2 no existe
                    juntaSpoolIng.ErroresRegistro.Add(string.Format(MensajesError.IngenieriaErrorMaterialNoExiste, juntaSpoolIng.EtiquetaMaterial2, juntaSpoolIng.Token[js.NombreDeSpool] , juntaSpoolIng.Archivo));
                    errorRegistro = true;
                }
                
            }
            catch (Exception)
            {
                Errores.Add(string.Format(MensajesError.IngenieriaErrorValidacionArchivo, juntaSpoolIng.Archivo,
                                          juntaSpoolIng.NumLinea));
                errorRegistro = true;
            }

            return !errorRegistro;
        }

        /// <summary>
        /// Funcion que se dispara cada que una junta es validada
        /// </summary>
        /// <param name="sender"></param>
        private void juntaValidada(object sender)
        {
            JuntaSpoolIng juntaSpool = (JuntaSpoolIng)sender;

            try
            {
                juntaSpool.Spool = _spoolsArchivo.Where(x => x.Nombre.EqualsIgnoreCase(juntaSpool.Token[js.NombreDeSpool])).First();
                asignaTipoJunta(juntaSpool);
                asignaFamiliasAceros(juntaSpool);
                juntaSpool.FabAreaID = _fabAreasCache.First(x => x.Nombre.EqualsIgnoreCase(juntaSpool.Token[js.Fabarea])).ID;

            }
            catch (Exception)
            {
                Errores.Add(string.Format(MensajesError.IngenieriaErrorGeneral, "", juntaSpool.Archivo,
                                          juntaSpool.NumLinea + 1));
            }
        }

        /// <summary>
        /// Se encarga de asignarle a JuntaSpoolIng su familia de material(es)
        /// </summary>
        /// <param name="juntaSpool"></param>
        private void asignaFamiliasAceros(JuntaSpoolIng juntaSpool)
        {
            string fam1 = juntaSpool.Token[js.TipoDeMaterial];
            FamiliaAcero familiaAcero;
            if (fam1.Contains("/"))
            {
                string fam2 = fam1.Split('/')[1].Trim();
                fam1 = fam1.Split('/')[0].Trim();

                familiaAcero = _familiasAceroCatalogo.FirstOrDefault(x => x.Nombre.EqualsIgnoreCase(fam2));
                if (familiaAcero != null)
                {
                   juntaSpool.FamiliaAceroMaterial1ID = familiaAcero.FamiliaAceroID;
                   
                }
            }

            familiaAcero = _familiasAceroCatalogo.FirstOrDefault(x => x.Nombre.EqualsIgnoreCase(fam1));
            if (familiaAcero != null)
            {
                juntaSpool.FamiliaAceroMaterial1ID = familiaAcero.FamiliaAceroID;
            }
        }

        /// <summary>
        /// Se encarga de asignar el tipo de junta
        /// </summary>
        /// <param name="juntaSpool"></param>
        private void asignaTipoJunta(JuntaSpoolIng juntaSpool)
        {
            TipoJunta tipoJunta = _tipoJuntasCatalogo.First(x => x.Codigo.EqualsIgnoreCase(juntaSpool.Token[js.TipoDeJunta]));
            juntaSpool.TipoJuntaID = tipoJunta.TipoJuntaID;
        }

        #endregion

        #region helpers

        /// <summary>
        /// Regresa la cedula calculada en ese momento de un spool
        /// </summary>
        /// <param name="spoolPendiente"></param>
        /// <returns></returns>
        public static string CalculaCedula(SpoolPendiente spoolPendiente)
        {
            JuntaSpoolPendiente juntaMayorDiametro = spoolPendiente.JuntaSpoolPendiente.OrderByDescending(x => x.Diametro).FirstOrDefault();
            if (juntaMayorDiametro != null)
            {
                spoolPendiente.Cedula = juntaMayorDiametro.Cedula;
            }
            else
            {
                spoolPendiente.Cedula = string.Empty;
            }
            return spoolPendiente.Cedula;
        }

        /// <summary>
        /// Revisa que coincidan las revisiones recibidas como parametros contra el spool en el isometrico
        /// </summary>
        /// <param name="dibujo"></param>
        /// <param name="revisionCte"></param>
        /// <param name="revisionSteelGo"></param>
        /// <returns></returns>
        private bool conincidenRevisiones(string dibujo, string revisionCte, string revisionSteelGo)
        {
            Spool spoolIsometrico = _spoolsArchivo.Where(x => x.Dibujo.EqualsIgnoreCase(dibujo)).ToList().FirstOrDefault();
            if (spoolIsometrico != null)
            {
                return spoolIsometrico.Revision.EqualsIgnoreCase(revisionSteelGo) &&
                       spoolIsometrico.RevisionCliente.EqualsIgnoreCase(revisionCte);
            }
            return true;
        }

        /// <summary>
        /// Revisa que exista el spool en el Isometrico
        /// </summary>
        /// <param name="dibujo"></param>
        /// <param name="nombreSpool"></param>
        /// <returns></returns>
        private bool existeSpoolEnIsometrico(string dibujo, string nombreSpool)
        {
            List<SpoolIng> spoolsIsometrico = _spoolsArchivo.Where(x => x.Dibujo.EqualsIgnoreCase(dibujo)).ToList();
            return spoolsIsometrico.Any(x => x.Nombre.EqualsIgnoreCase(nombreSpool));
        }

        /// <summary>
        /// Revisa si existe la cedula, si no la marca para posteriormente darla de alta
        /// </summary>
        /// <param name="cedula"></param>
        private void revisaCedula(string cedula)
        {
            if (string.IsNullOrEmpty(cedula)) return;

            if (!_cedulasCache.Any(x => x.Nombre.EqualsIgnoreCase(cedula)))
            {
                CedulasNoEncontradas.Add(cedula);
                _cedulasCatalogo.Add(new Cedula { Codigo = cedula, VerificadoPorCalidad = false });
            }
        }

        /// <summary>
        /// Revisa si existe la Area de Fabricacion (FabArea)
        /// </summary>
        /// <param name="fabArea"></param>
        /// <returns></returns>
        private bool existeFabArea(string fabArea)
        {
            if (string.IsNullOrEmpty(fabArea)) return false;

            if (!_fabAreasCache.Any(x => x.Nombre.EqualsIgnoreCase(fabArea)))
            {
                FabAreasNoEncontradas.Add(fabArea);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Revisa si existe TipoDeJunta, si no la marca para posteriormente darla de alta
        /// </summary>
        /// <param name="tipoJunta"></param>
        private void revisaTipoJunta(string tipoJunta)
        {
            if (string.IsNullOrEmpty(tipoJunta)) return;

            if (!_tipoJuntasCatalogo.Any(x => x.Codigo.EqualsIgnoreCase(tipoJunta)))
            {
                TipoJuntaNoEncontradas.Add(tipoJunta);
                _tipoJuntasCatalogo.Add(new TipoJunta { Codigo = tipoJunta, Nombre = tipoJunta, VerificadoPorCalidad = false });
            }
        }

        /// <summary>
        /// Revisa si existe Tipocorte, si no la marca para posteriormente darlo de alta
        /// </summary>
        /// <param name="tipoCorte"></param>
        private void revisaTipoCorte(string tipoCorte)
        {
            if (string.IsNullOrEmpty(tipoCorte)) return;

            if (!_tipoCortesCatalogo.Any(x => x.Codigo.EqualsIgnoreCase(tipoCorte)))
            {
                TipoCorteNoEncontrados.Add(tipoCorte);
                _tipoCortesCatalogo.Add(new TipoCorte { Codigo = tipoCorte, Nombre = tipoCorte, VerificadoPorCalidad = false });
            }
            return;
        }

        /// <summary>
        /// Revisa si existe diametro, si no la marca para posteriormente darlo de alta
        /// </summary>
        /// <param name="diametro"></param>
        private void revisaDiametro(decimal diametro)
        {
            if (diametro == 0) return;

            if (!_diametrosCache.Any(x => x.Valor == diametro))
            {
                DiametrosNoEncontrados.Add(diametro.ToString());
                _diametrosCatalogo.Add(new Diametro { Valor = diametro, VerificadoPorCalidad = false });
            }
        }

        /// <summary>
        /// Revisa si existe ItemCode, si no la marca para posteriormente darlo de alta
        /// </summary>
        /// <param name="itemCode"></param>
        /// <param name="descripcion"></param>
        /// <param name="clasificacion"></param>
        private void revisaItemCode(string itemCode, string descripcion, string clasificacion)
        {
            if (string.IsNullOrEmpty(itemCode)) return;

            descripcion = descripcion.Replace('�', 'º');

            _itemCodeArchivo.Add(itemCode.Replace("|", string.Empty) + "|" +
                                 descripcion.Replace("|", string.Empty));

            if (!_itemCodeCatalogo.Any(x => x.Codigo.EqualsIgnoreCase(itemCode)))
            {
                ItemCodeNoEncontrados.Add(itemCode.Replace("|", string.Empty) + "|" +
                                          descripcion.Replace("|", string.Empty) + "|" +
                                          clasificacion.Replace("|", string.Empty));

                _itemCodeCatalogo.Add(new ItemCode
                                          {
                                              ProyectoID = _proyectoID,
                                              UsuarioModifica = _usuarioModifica,
                                              FechaModificacion = DateTime.Now,
                                              Codigo = itemCode,
                                              DescripcionIngles = descripcion,
                                              DescripcionEspanol = descripcion,
                                              TipoMaterialID =
                                                  clasificacion.Trim().EqualsIgnoreCase("pipe")
                                                      ? (int)TipoMaterialEnum.Tubo
                                                      : (int)TipoMaterialEnum.Accessorio

                                          });
            }
            else
            {
                string cadenaAgregar = itemCode.Replace("|", string.Empty) + "|" +
                                          descripcion.Replace("|", string.Empty) + "|" +
                                          clasificacion.Replace("|", string.Empty);

                if ((!ItemCodeAModificar.Any() || !ItemCodeAModificar.Contains(cadenaAgregar)) && !ItemCodeNoEncontrados.Contains(cadenaAgregar))
                {
                    ItemCode ic = _itemCodeCatalogo.Where(x => x.Codigo == itemCode && x.ProyectoID == _proyectoID).SingleOrDefault();                    

                    if (!ic.DescripcionEspanol.EqualsIgnoreCase(descripcion))
                    {
                        ItemCodeAModificar.Add(cadenaAgregar);
                    }
                }
            }
        }

        /// <summary>
        /// Revisa si existe Familia de acero, si no la marca para posteriormente darla de alta
        /// </summary>
        /// <param name="familia"></param>
        private void existeFamAcero(string familia)
        {
            if (string.IsNullOrEmpty(familia)) return;

            // no esta en catalogo
            if (!_familiasAceroCatalogo.Any(x => x.Nombre.EqualsIgnoreCase(familia)))
            {
                //esta en lo que viene de UI .... en la segunda vuelta
                if (_familiasAceroUi.Any(x => x.Nombre.EqualsIgnoreCase(familia)))
                {
                    //lo registramos para darse de alta
                    FamiliaAcero famAcero = _familiasAceroUi.First(x => x.Nombre.EqualsIgnoreCase(familia));

                    _familiasAceroCatalogo.Add(new FamiliaAcero
                                                   {
                                                       FamiliaMaterial =
                                                           _familiasMaterialCatalogo.First(
                                                               x => x.FamiliaMaterialID == famAcero.FamiliaMaterialID),
                                                       Nombre = famAcero.Nombre,
                                                       VerificadoPorCalidad = false
                                                   });
                }
                else
                {
                    FamiliaAceroNoEncontradas.Add(familia);
                }
            }
        }

        /// <summary>
        /// Recibe una cadena como 1.1/2x5.3/4 y en d1 deja 1.5 y en d2 5.75 
        /// </summary>
        /// <param name="diametro"></param>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        public static void calculaDiametros(string diametro, out decimal d1, out decimal d2)
        {
            d1 = 0.0M;
            d2 = 0.0M;
            if (string.IsNullOrEmpty(diametro))
            {
                return;
            }

            //removemos el " que indica pulgadas! 
            diametro = diametro.Replace(@"""", "").ToLowerInvariant();

            string diametro1 = diametro.Split('x')[0];
            string diametro2 = "";
            d2 = 0.0M;
            if (diametro.Split('x').Length > 1)
            {
                diametro2 = diametro.Split('x')[1];
            }

            //supongamos que diametro1 = 1.3/8  lo correcto para calcularlo sera 1+3/8
            //si no hay punto se calcula como 3/8 
            if (diametro1.IndexOf("/") != -1 && diametro1.IndexOf(".") != -1)
            {
                diametro1 = diametro1.Replace(".", "+");
            }
            d1 = Decimal.Parse(_dataTable.Compute(diametro1, null).ToString());

            if (diametro2 != string.Empty)
            {
                if (diametro2.IndexOf("/") != -1 && diametro2.IndexOf(".") != -1)
                {
                    diametro2 = diametro2.Replace(".", "+");
                }
                d2 = Decimal.Parse(new DataTable().Compute(diametro2, null).ToString());
            }
        }

        #endregion
    }
}







