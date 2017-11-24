using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Validations;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Common;
using SAM.BusinessLogic.Cruce;
using SAM.BusinessLogic.Excepciones;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Produccion;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Workstatus;
using System.Data.Objects;
using SAM.Entities.Personalizadas;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Ingenieria;
using Mimo.Framework.Extensions;
using System.IO;

namespace SAM.BusinessLogic.Produccion
{
    public class OrdenTrabajoBL
    {
        private static readonly object _mutex = new object();
        private static OrdenTrabajoBL _instance;

        private StringBuilder erroresCsv;

        /// <summary>
        /// constructro privado para implementar patron singleton
        /// </summary>
        private OrdenTrabajoBL()
        {
        }

        /// <summary>
        /// obtiene la instancia de la clase OrdenTrabajoBL
        /// </summary>
        public static OrdenTrabajoBL Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new OrdenTrabajoBL();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Genera una nueva orden de trabajo sin spools relacionados a la misma.
        /// Esto sucede desde el UI cuando el usuario genera una nueva orden de trabajo de manera
        /// manual.
        /// </summary>
        /// <param name="proyectoID">ID del proyecto al cual pertenece la nueva orden de trabajo</param>
        /// <param name="tallerID">ID del taller en el cual se van a armar y soldar los spools de la misma</param>
        /// <param name="numeroOdt">Número consecutivo de la ODT</param>
        /// <param name="fecha">Fecha para la orden de trabajo</param>
        /// <param name="userID">Usuario que manda la instrucción</param>
        /// <param name="conAsignacion">Indica si la ODT se debe crear con asignación</param>
        /// <returns>Regresa un objeto de tipo OrdenTrabajo con la nueva orden de trabajo generada en caso de ser exitoso</returns>
        public OrdenTrabajo GeneraNueva(int proyectoID, int tallerID, int numeroOdt, DateTime fecha, Guid userID, bool conAsignacion)
        {
            return GeneraNueva(proyectoID, null, tallerID, numeroOdt, fecha, userID, conAsignacion, null, false);
        }

        /// <summary>
        /// Genera una nueva orden de trabajo con spools relacionados a la misma.
        /// Esto sucede principalmente desde la pantalla de cruce donde el usuario selecciona
        /// que spools desea incluir en una ODT.
        /// </summary>
        /// <param name="proyectoID">ID del proyecto al cual pertenece la nueva orden de trabajo</param>
        /// <param name="spoolIds">Arreglo de IDs de los spools que se deben incluir en la ODT</param>
        /// <param name="tallerID">ID del taller en el cual se van a armar y soldar los spools de la misma</param>
        /// <param name="numeroOdt">Número consecutivo de la ODT</param>
        /// <param name="fecha">Fecha para la orden de trabajo</param>
        /// <param name="userID">Usuario que manda la instrucción</param>
        /// <param name="conAsignacion">Indica si la ODT se debe crear con asignación</param>
        /// <returns>Regresa un objeto de tipo OrdenTrabajo con la nueva orden de trabajo generada en caso de ser exitoso</returns>
        public OrdenTrabajo GeneraNueva(int proyectoID, int[] spoolIds, int tallerID, int numeroOdt, DateTime fecha, Guid userID, bool conAsignacion, string ordenamiento, bool crucePorCsv)
        {
            try
            {
                //Sólo debemos utilizar las juntas tipo SHOP
                int shopFabAreaID = CacheCatalogos.Instance.ShopFabAreaID;

                using (SamContext ctx = new SamContext())
                {
                    ctx.NumeroUnico.MergeOption = MergeOption.NoTracking;
                    ctx.NumeroUnicoSegmento.MergeOption = MergeOption.NoTracking;
                    ctx.NumeroUnicoInventario.MergeOption = MergeOption.NoTracking;
                    ctx.ItemCode.MergeOption = MergeOption.NoTracking;
                    ctx.ItemCodeEquivalente.MergeOption = MergeOption.NoTracking;
                    ctx.Spool.MergeOption = MergeOption.NoTracking;
                    ctx.MaterialSpool.MergeOption = MergeOption.NoTracking;
                    ctx.JuntaSpool.MergeOption = MergeOption.NoTracking;

                    string numOrden = string.Empty;
                    bool ODTfueraDeRango = false;
                    ProyectoConfiguracion pc;

                    if (!crucePorCsv && !ValidacionesOdt.ValidaSpoolsSinOdt(ctx, spoolIds))
                    {
                        throw new BaseValidationException(MensajesError.Excepcion_AlgunSpoolYaTieneOdt);
                    }

                    if (!ValidacionesOdt.ValidaNumeroDeOdtDisponible(ctx, proyectoID, numeroOdt, out numOrden, out pc))
                    {
                        if (crucePorCsv)
                        {
                            erroresCsv.Append(string.Format(MensajesError.Csv_OtX_Duplicado, numeroOdt));
                        }
                        else 
                        {
                            throw new BaseValidationException(MensajesError.Excepcion_NumeroDeOdtDuplicado);
                        }
                    }

                    if (numeroOdt.ToString().Length > pc.DigitosOrdenTrabajo)
                    {
                        if (crucePorCsv)
                        {
                            erroresCsv.Append(string.Format(MensajesError.Csv_ODTFueraDeRango, numeroOdt.ToString()));
                        }
                        else
                        {
                            throw new BaseValidationException(string.Format(MensajesError.Csv_ODTFueraDeRango, numeroOdt.ToString()));
                        }
                    }

                    OrdenTrabajo odt = new OrdenTrabajo();
                    odt.NumeroOrden = numOrden;
                    odt.ProyectoID = proyectoID;
                    odt.EstatusOrdenID = (int)EstatusOrdenDeTrabajo.Activa;
                    odt.FechaOrden = fecha.Date; //sin horas, minutos, segundos
                    odt.TallerID = tallerID;
                    odt.EsAsignado = conAsignacion;
                    odt.FechaModificacion = DateTime.Now;
                    odt.UsuarioModifica = userID;
                    odt.VersionOrden = 0; //Version nuevo de la ODT es 0

                    #region Efectuar el cruce para la Odt en caso que haya que agregar spools

                    if (spoolIds != null && spoolIds.Length > 0)
                    {
                        spoolIds = OrdenaSpools(spoolIds, ordenamiento, false);

                        CruceSpool cruce = new CruceSpool(ctx, proyectoID, spoolIds, userID);
                        List<NumeroUnico> lstNumUnico;

                        //Efectuar el cruce para estos spools en particular, en caso que no se pueda esto arroja una excepción
                        List<Spool> lstSpool = crucePorCsv ? cruce.Procesa(out lstNumUnico, erroresCsv) : cruce.Procesa(out lstNumUnico);

                        //Revisar que se hayan podido cruzar los spools
                        if (lstSpool.Any(x => !x.InfoCruce.CruceExitoso))
                        {
                            if (crucePorCsv)
                            {
                                erroresCsv.Append(string.Format(MensajesError.Csv_SpoolMaterialX, lstSpool[0].Nombre));
                            }
                            else
                            {
                                List<string> errores = lstSpool.Where(x => !x.InfoCruce.CruceExitoso)
                                                           .Select(y => string.Format(MensajesError.MaterialInsuficiente_ParaSpool, y.Nombre))
                                                           .ToList();

                                throw new ExcepcionMaterialInsuficiente(errores);
                            }
                        }

                        int i = 0;

                        foreach (Spool spool in lstSpool)
                        {

                            //Etiquetamos spool
                            spool.StartTracking();
                            spool.FechaEtiqueta = DateTime.Now;
                            spool.NumeroEtiqueta = numOrden;
                            spool.UsuarioModifica = userID;
                            spool.FechaModificacion = DateTime.Now;
                            ctx.Spool.ApplyChanges(spool);

                            #region Generar un nuevo objeto en la ODT para cada spool
                            OrdenTrabajoSpool odts = new OrdenTrabajoSpool
                            {
                                FechaModificacion = DateTime.Now,
                                NumeroControl = string.Concat(numOrden, '-', (i + 1).ToString().PadLeft(3, '0')),
                                Partida = i + 1,
                                SpoolID = spool.SpoolID, // spoolIds[i],
                                EsAsignado = conAsignacion,
                                UsuarioModifica = userID
                            };
                            #endregion

                            #region Iterar las juntas para crearlas en la ODT

                            List<JuntaSpool> jsSoloShop = spool.JuntaSpool.Where(x => x.FabAreaID == shopFabAreaID).ToList();

                            foreach (JuntaSpool js in jsSoloShop)
                            {
                                OrdenTrabajoJunta odtj = new OrdenTrabajoJunta
                                {
                                    FechaModificacion = DateTime.Now,
                                    JuntaSpoolID = js.JuntaSpoolID,
                                    FueReingenieria = false,
                                    UsuarioModifica = userID
                                };

                                odts.OrdenTrabajoJunta.Add(odtj);
                            }
                            #endregion

                            #region Iterar los materiales para crearlos en la ODT
                            foreach (MaterialSpool ms in spool.MaterialSpool)
                            {
                                OrdenTrabajoMaterial odtm = new OrdenTrabajoMaterial
                                {
                                    CantidadCongelada = ms.Cantidad,
                                    CantidadDespachada = 0,
                                    CongeladoEsEquivalente = ms.InfoCruce.EsEquivalente,
                                    DespachoEsEquivalente = false,
                                    NumeroUnicoCongeladoID = ms.InfoCruce.NumeroUnicoID,
                                    NumeroUnicoSugeridoID = ms.InfoCruce.EsSugerido ? (int?)ms.InfoCruce.NumeroUnicoID : null,
                                    MaterialSpoolID = ms.MaterialSpoolID,
                                    SegmentoCongelado = string.IsNullOrEmpty(ms.InfoCruce.Segmento) ? null : ms.InfoCruce.Segmento,
                                    SegmentoSugerido = ms.InfoCruce.EsSugerido && !string.IsNullOrEmpty(ms.InfoCruce.Segmento) ? ms.InfoCruce.Segmento : null,
                                    SugeridoEsEquivalente = ms.InfoCruce.EsEquivalente,
                                    TieneInventarioCongelado = true,
                                    FechaModificacion = DateTime.Now,
                                    FueReingenieria = false,
                                    UsuarioModifica = userID,
                                    EsAsignado = conAsignacion,
                                    NumeroUnicoAsignadoID = null,
                                    SegmentoAsignado = null
                                };
                                odts.OrdenTrabajoMaterial.Add(odtm);
                            }
                            #endregion

                            //Agregarlo a la ODT
                            odt.OrdenTrabajoSpool.Add(odts);

                            i++;
                        }

                        //cambios a los números únicos (inventarios)
                        lstNumUnico.ForEach(ctx.NumeroUnico.ApplyChanges);
                    }

                    #endregion

                    #region Determinar si debemos actualizar el consecutivo de la ODT para lo que se sugiera la siguiente vez

                    string odtMayor = ctx.OrdenTrabajo
                                         .Where(x => x.ProyectoID == proyectoID)
                                         .Select(y => y.NumeroOrden)
                                         .Max();

                    int iOdtMayor = -1;

                    int cantCaracteres = 0;

                    foreach (char c in odtMayor)
                    {
                        if (Char.IsLetter(c))
                        {
                            cantCaracteres++;
                        }
                        else { break; }
                    }
                    
                    if(cantCaracteres > pc.PrefijoOrdenTrabajo.Length)
                    {                        
                        odtMayor = odtMayor.Substring(1, odtMayor.Length-1);
                    }

                    if (!string.IsNullOrEmpty(odtMayor))
                    {
                        iOdtMayor = int.Parse(odtMayor.Substring(pc.PrefijoOrdenTrabajo.Length));
                    }

                    if (numeroOdt > iOdtMayor)
                    {
                        ProyectoConsecutivo cons = ctx.ProyectoConsecutivo.Where(x => x.ProyectoID == proyectoID).Single();
                        cons.StartTracking();
                        cons.FechaModificacion = DateTime.Now;
                        cons.UsuarioModifica = userID;
                        cons.ConsecutivoODT = numeroOdt;
                        cons.StopTracking();

                        //Cambio al consecutivo del proyecto
                        ctx.ProyectoConsecutivo.ApplyChanges(cons);
                    }

                    #endregion

                    #region Borrar congelados parciales en caso que aplique

                    IQueryable<int> msIds = odt.OrdenTrabajoSpool.SelectMany(x => x.OrdenTrabajoMaterial)
                                                                 .Select(y => y.MaterialSpoolID)
                                                                 .AsQueryable();

                    List<CongeladoParcial> congs = (from c in ctx.CongeladoParcial
                                                    where msIds.Contains(c.MaterialSpoolID)
                                                    select c).ToList();

                    congs.ForEach(ctx.CongeladoParcial.DeleteObject);

                    #endregion

                    //nueva ODT
                    ctx.OrdenTrabajo.ApplyChanges(odt);
                    ctx.SaveChanges();
                    return odt;
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_Concurrencia);
            }
        }

        public bool GenerarNuevasOdtEspeciales(List<SpoolCruce> spools, Guid UserID, int _proyectoID, int numOrden, int taller)
        {
            using (SamContext ctx = new SamContext())
            {
                ProyectoConfiguracion configProyecto = ctx.ProyectoConfiguracion.Where(x => x.ProyectoID == _proyectoID).Single();
                
            }
            return false;
        }

        private int[] OrdenaSpools(int[] Spools, string ordenamiento, bool incluyeHold)
        {
            List<GrdCruce> grid = SpoolBO.Instance.ObtenerDespuesDeCruce(Spools, incluyeHold);

            if (ordenamiento != string.Empty)
            {
                return grid.OrderBy(ordenamiento).Select(x => x.SpoolID).ToArray();
            }
            else
            {
                return grid.OrderBy(x => x.Dibujo).ThenBy(x => x.Nombre).Select(x => x.SpoolID).ToArray();
            }

        }

        public OrdenTrabajo GeneraCruceODTExistente(int proyectoID, int[] spoolIds, int odtID, DateTime fecha, Guid userID, string ordenamiento, bool crucePorCsv)
        {
            try
            {
                //Sólo debemos utilizar las juntas tipo SHOP
                int shopFabAreaID = CacheCatalogos.Instance.ShopFabAreaID;

                using (SamContext ctx = new SamContext())
                {
                    ctx.NumeroUnico.MergeOption = MergeOption.NoTracking;
                    ctx.NumeroUnicoSegmento.MergeOption = MergeOption.NoTracking;
                    ctx.NumeroUnicoInventario.MergeOption = MergeOption.NoTracking;
                    ctx.ItemCode.MergeOption = MergeOption.NoTracking;
                    ctx.ItemCodeEquivalente.MergeOption = MergeOption.NoTracking;
                    ctx.Spool.MergeOption = MergeOption.NoTracking;
                    ctx.MaterialSpool.MergeOption = MergeOption.NoTracking;
                    ctx.JuntaSpool.MergeOption = MergeOption.NoTracking;

                    if (!crucePorCsv && !ValidacionesOdt.ValidaSpoolsSinOdt(ctx, spoolIds))
                    {
                        throw new BaseValidationException(MensajesError.Excepcion_AlgunSpoolYaTieneOdt);
                    }

                    OrdenTrabajo odt = ctx.OrdenTrabajo.Where(x => x.OrdenTrabajoID == odtID).Single();

                    #region Efectuar el cruce para la Odt en caso que haya que agregar spools

                    spoolIds = OrdenaSpools(spoolIds, ordenamiento, false);

                    if (spoolIds != null && spoolIds.Length > 0)
                    {
                        CruceSpool cruce = new CruceSpool(ctx, proyectoID, spoolIds, userID);
                        List<NumeroUnico> lstNumUnico;

                        //Efectuar el cruce para estos spools en particular, en caso que no se pueda esto arroja una excepción
                        List<Spool> lstSpool = crucePorCsv ? cruce.Procesa(out lstNumUnico, erroresCsv) : cruce.Procesa(out lstNumUnico);

                        if (lstSpool.Count == 0)
                        {
                            return odt;
                        }

                        //Revisar que se hayan podido cruzar los spools
                        if (lstSpool.Any(x => !x.InfoCruce.CruceExitoso))
                        {
                            if (crucePorCsv)
                            {
                                erroresCsv.Append(string.Format(MensajesError.Csv_SpoolMaterialX, lstSpool[0].Nombre));
                                return null;
                            }
                            else
                            {
                                List<string> errores = lstSpool.Where(x => !x.InfoCruce.CruceExitoso)
                                                           .Select(y => string.Format(MensajesError.MaterialInsuficiente_ParaSpool, y.Nombre))
                                                           .ToList();

                                throw new ExcepcionMaterialInsuficiente(errores);
                            }
                        }

                        int i = ctx.OrdenTrabajoSpool
                                   .Where(x => x.OrdenTrabajoID == odtID)
                                   .Any()
                                   ? ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajoID == odtID).Select(x => x.Partida).Max()
                                   : 0;

                        foreach (Spool spool in lstSpool)
                        {

                            //Etiquetamos spool
                            spool.StartTracking();
                            spool.FechaEtiqueta = DateTime.Now;
                            spool.NumeroEtiqueta = odt.NumeroOrden;
                            spool.UsuarioModifica = userID;
                            spool.FechaModificacion = DateTime.Now;
                            ctx.Spool.ApplyChanges(spool);

                            #region Generar un nuevo objeto en la ODT para cada spool
                            OrdenTrabajoSpool odts = new OrdenTrabajoSpool
                            {
                                FechaModificacion = DateTime.Now,
                                NumeroControl = string.Concat(odt.NumeroOrden, '-', (i + 1).ToString().PadLeft(3, '0')),
                                Partida = i + 1,
                                SpoolID = spool.SpoolID, // spoolIds[i],
                                EsAsignado = false,
                                UsuarioModifica = userID
                            };
                            #endregion

                            #region Iterar las juntas para crearlas en la ODT

                            List<JuntaSpool> jsSoloShop = spool.JuntaSpool.Where(x => x.FabAreaID == shopFabAreaID).ToList();

                            foreach (JuntaSpool js in jsSoloShop)
                            {
                                OrdenTrabajoJunta odtj = new OrdenTrabajoJunta
                                {
                                    FechaModificacion = DateTime.Now,
                                    JuntaSpoolID = js.JuntaSpoolID,
                                    FueReingenieria = false,
                                    UsuarioModifica = userID
                                };

                                odts.OrdenTrabajoJunta.Add(odtj);
                            }
                            #endregion

                            #region Iterar los materiales para crearlos en la ODT
                            foreach (MaterialSpool ms in spool.MaterialSpool)
                            {
                                OrdenTrabajoMaterial odtm = new OrdenTrabajoMaterial
                                {
                                    CantidadCongelada = ms.Cantidad,
                                    CantidadDespachada = 0,
                                    CongeladoEsEquivalente = ms.InfoCruce.EsEquivalente,
                                    DespachoEsEquivalente = false,
                                    NumeroUnicoCongeladoID = ms.InfoCruce.NumeroUnicoID,
                                    NumeroUnicoSugeridoID = ms.InfoCruce.EsSugerido ? (int?)ms.InfoCruce.NumeroUnicoID : null,
                                    MaterialSpoolID = ms.MaterialSpoolID,
                                    SegmentoCongelado = string.IsNullOrEmpty(ms.InfoCruce.Segmento) ? null : ms.InfoCruce.Segmento,
                                    SegmentoSugerido = ms.InfoCruce.EsSugerido && !string.IsNullOrEmpty(ms.InfoCruce.Segmento) ? ms.InfoCruce.Segmento : null,
                                    SugeridoEsEquivalente = ms.InfoCruce.EsEquivalente,
                                    TieneInventarioCongelado = true,
                                    FechaModificacion = DateTime.Now,
                                    FueReingenieria = false,
                                    UsuarioModifica = userID,
                                    EsAsignado = false,
                                    NumeroUnicoAsignadoID = null,
                                    SegmentoAsignado = null
                                };
                                odts.OrdenTrabajoMaterial.Add(odtm);
                            }
                            #endregion

                            //Agregarlo a la ODT
                            odt.OrdenTrabajoSpool.Add(odts);

                            i++;
                        }

                        //cambios a los números únicos (inventarios)
                        lstNumUnico.ForEach(ctx.NumeroUnico.ApplyChanges);
                    }
                    #endregion

                    #region Borrar congelados parciales en caso que aplique

                    IQueryable<int> msIds = odt.OrdenTrabajoSpool.SelectMany(x => x.OrdenTrabajoMaterial)
                                                                 .Select(y => y.MaterialSpoolID)
                                                                 .AsQueryable();

                    List<CongeladoParcial> congs = (from c in ctx.CongeladoParcial
                                                    where msIds.Contains(c.MaterialSpoolID)
                                                    select c).ToList();

                    congs.ForEach(ctx.CongeladoParcial.DeleteObject);

                    #endregion

                    ctx.OrdenTrabajo.ApplyChanges(odt);
                    ctx.SaveChanges();
                    return odt;
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_Concurrencia);
            }
        }

        /// <summary>
        /// Este método se utiliza cuando ingeniería llevó a cabo cambios en la definición
        /// del spool después de que el mismo ya fuese incluído en una ODT.
        /// 
        /// Lo que hace este método es determinar que materiales nuevos fueron agregados por ingeniería
        /// y lleva a cabo un cruce para los mismos para poder incluirlos en la ODT.  En caso de que no exista
        /// material suficiente se arroja una excepción.
        /// </summary>
        /// <param name="ordenTrabajoSpoolID">ID del registro OrdenTrabajoSpool para el cual se desea llevar a cabo la reinigeniería</param>
        public void Reingenieria(int ordenTrabajoSpoolID, Guid userID)
        {
            try
            {
                CruceMaterial cruce = new CruceMaterial(ordenTrabajoSpoolID, userID);
                bool exito = false;

                List<NumeroUnico> lstNu;
                List<OrdenTrabajoJunta> lstOtj;
                OrdenTrabajoSpool odts = cruce.Procesa(out exito, out lstNu);

                if (exito && odts != null && lstNu != null)
                {
                    using (SamContext ctx = new SamContext())
                    {
                        ctx.OrdenTrabajoSpool.ApplyChanges(odts);
                        lstNu.ForEach(ctx.NumeroUnico.ApplyChanges);

                        int shopFabAreaID = CacheCatalogos.Instance.ShopFabAreaID;

                        //Las juntas de ordentrabajoJunta que no son tipo SHOP
                        lstOtj = (from js in ctx.JuntaSpool
                                  join otj in ctx.OrdenTrabajoJunta
                                  on js.JuntaSpoolID equals otj.JuntaSpoolID
                                  where js.SpoolID == odts.SpoolID
                                  && js.FabAreaID != shopFabAreaID
                                  select otj).ToList();

                        lstOtj.ForEach(x => ctx.OrdenTrabajoJunta.DeleteObject(x));

                        ctx.SaveChanges();
                    }
                }
                else
                {
                    throw new ExcepcionMaterialInsuficiente(MensajesError.MaterialInsuficiente_ParaReingenieria);
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_Concurrencia);
            }
        }


        /// <summary>
        /// Este método agrega un spool a una orden de trabajo ya existente.
        /// Lleva a cabo las comprobaciones en la lógica de negocios para saber si existe suficiente material para
        /// fabricar el spool seleccionado y congelar los inventarios correspondientes.
        /// </summary>
        /// <param name="ordenTrabajoID">ID de la orden de trabajo a la cual se desea anexar el spool</param>
        /// <param name="spoolID">ID del spool que se desea agregar a la orden de trabajo</param>
        /// <param name="partida">Partida/número de control del spool que se está agregando</param>
        /// <param name="userID">Usuario que manda la instrucción</param>
        /// <param name="versionRegistroOriginal">Versión original del registro de la ODT para poder verificar concurrencia</param>
        public void AgregaSpool(int ordenTrabajoID, int spoolID, int partida, Guid userID, byte[] versionRegistroOriginal)
        {
            try
            {
                int shopFabAreaID = CacheCatalogos.Instance.ShopFabAreaID;

                using (SamContext ctx = new SamContext())
                {
                    ctx.Spool.MergeOption = MergeOption.NoTracking;
                    ctx.MaterialSpool.MergeOption = MergeOption.NoTracking;
                    ctx.JuntaSpool.MergeOption = MergeOption.NoTracking;
                    ctx.ItemCode.MergeOption = MergeOption.NoTracking;

                    int[] spoolIds = new int[] { spoolID };

                    if (RevisionHoldsBO.Instance.SpoolTieneHold(ctx, spoolID))
                    {
                        throw new ExcepcionEnHold(MensajesError.Excepcion_SpoolEnHold);
                    }

                    //Asegurarnos que el spool no esté en otra ODT
                    if (!ValidacionesOdt.ValidaSpoolsSinOdt(ctx, spoolIds))
                    {
                        throw new BaseValidationException(MensajesError.Excepcion_SpoolYaTieneOdt);
                    }

                    //Revisar que la partida/número de control no haya sido tomada por otro spool de la misma ODT
                    if (!ValidacionesOdt.ValidaPartidaDisponible(ctx, ordenTrabajoID, partida))
                    {
                        throw new BaseValidationException(MensajesError.Odt_ElNumeroDeControlYaExiste);
                    }

                    OrdenTrabajo odt = ctx.OrdenTrabajo.Where(x => x.OrdenTrabajoID == ordenTrabajoID).Single();
                    odt.VersionRegistro = versionRegistroOriginal;

                    CruceSpool cruce = new CruceSpool(ctx, odt.ProyectoID, spoolIds, userID);
                    List<NumeroUnico> lstNumUnico;

                    //Efectuar el cruce para estos spools en particular, en caso que no se pueda esto arroja una excepción
                    List<Spool> lstSpool = cruce.Procesa(out lstNumUnico);
                    Spool spool = lstSpool[0];

                    //Revisar que se hayan podido cruzar los spools
                    if (!spool.InfoCruce.CruceExitoso)
                    {
                        throw new ExcepcionMaterialInsuficiente(string.Format(MensajesError.MaterialInsuficiente_ParaSpool, spool.Nombre));
                    }

                    odt.StartTracking();
                    odt.FechaModificacion = DateTime.Now;
                    odt.UsuarioModifica = userID;

                    //Etiquetamos spool
                    spool.StartTracking();
                    spool.FechaEtiqueta = DateTime.Now;
                    spool.NumeroEtiqueta = odt.NumeroOrden;
                    spool.UsuarioModifica = userID;
                    spool.FechaModificacion = DateTime.Now;
                    ctx.Spool.ApplyChanges(spool);

                    #region Generar un nuevo objeto en la ODT para el spool
                    OrdenTrabajoSpool odts = new OrdenTrabajoSpool
                    {
                        FechaModificacion = DateTime.Now,
                        NumeroControl = string.Concat(odt.NumeroOrden, '-', partida.ToString().PadLeft(3, '0')),
                        Partida = partida,
                        SpoolID = spool.SpoolID,
                        UsuarioModifica = userID,
                        EsAsignado = false
                    };
                    #endregion

                    #region Iterar las juntas para crearlas en la ODT
                    List<JuntaSpool> jsSoloShop = spool.JuntaSpool.Where(x => x.FabAreaID == shopFabAreaID).ToList();

                    foreach (JuntaSpool js in jsSoloShop)
                    {
                        OrdenTrabajoJunta odtj = new OrdenTrabajoJunta
                        {
                            FechaModificacion = DateTime.Now,
                            JuntaSpoolID = js.JuntaSpoolID,
                            FueReingenieria = false,
                            UsuarioModifica = userID
                        };

                        odts.OrdenTrabajoJunta.Add(odtj);
                    }
                    #endregion

                    #region Iterar los materiales para crearlos en la ODT
                    foreach (MaterialSpool ms in spool.MaterialSpool)
                    {
                        OrdenTrabajoMaterial odtm = new OrdenTrabajoMaterial
                        {
                            CantidadCongelada = ms.Cantidad,
                            CantidadDespachada = 0,
                            CongeladoEsEquivalente = ms.InfoCruce.EsEquivalente,
                            DespachoEsEquivalente = false,
                            NumeroUnicoCongeladoID = ms.InfoCruce.NumeroUnicoID,
                            NumeroUnicoSugeridoID = ms.InfoCruce.EsSugerido ? (int?)ms.InfoCruce.NumeroUnicoID : null,
                            MaterialSpoolID = ms.MaterialSpoolID,
                            SegmentoCongelado = string.IsNullOrEmpty(ms.InfoCruce.Segmento) ? null : ms.InfoCruce.Segmento,
                            SegmentoSugerido = ms.InfoCruce.EsSugerido && !string.IsNullOrEmpty(ms.InfoCruce.Segmento) ? ms.InfoCruce.Segmento : null,
                            SugeridoEsEquivalente = ms.InfoCruce.EsEquivalente,
                            TieneInventarioCongelado = true,
                            FechaModificacion = DateTime.Now,
                            FueReingenieria = false,
                            UsuarioModifica = userID,
                            EsAsignado = false,
                            SegmentoAsignado = null,
                            NumeroUnicoAsignadoID = null
                        };

                        odts.OrdenTrabajoMaterial.Add(odtm);
                    }
                    #endregion

                    #region Borrar congelados parciales en caso que aplique

                    IQueryable<int> msIds = odt.OrdenTrabajoSpool.SelectMany(x => x.OrdenTrabajoMaterial)
                                                                 .Select(y => y.MaterialSpoolID)
                                                                 .AsQueryable();

                    List<CongeladoParcial> congs = (from c in ctx.CongeladoParcial
                                                    where msIds.Contains(c.MaterialSpoolID)
                                                    select c).ToList();

                    congs.ForEach(ctx.CongeladoParcial.DeleteObject);

                    #endregion

                    //Agregarlo a la ODT
                    odt.OrdenTrabajoSpool.Add(odts);

                    //cambios a los números únicos (inventarios)
                    lstNumUnico.ForEach(ctx.NumeroUnico.ApplyChanges);
                    ctx.OrdenTrabajo.ApplyChanges(odt);
                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_Concurrencia);
            }
        }



        /// <summary>
        /// Este método agrega un spool a una orden de trabajo ya existente.
        /// Lleva a cabo las comprobaciones en la lógica de negocios para saber si existe suficiente material para
        /// fabricar el spool seleccionado y congelar los inventarios correspondientes.
        /// </summary>
        /// <param name="ordenTrabajoID">ID de la orden de trabajo a la cual se desea anexar el spool</param>
        /// <param name="spoolID">ID del spool que se desea agregar a la orden de trabajo</param>
        /// <param name="partida">Partida/número de control del spool que se está agregando</param>
        /// <param name="lstPares">La selección de que material se debe surtir con qué número único</param>
        /// <param name="userID">Usuario que manda la instrucción</param>
        /// <param name="versionRegistroOriginal">Versión original del registro de la ODT para poder verificar concurrencia</param>
        public void AgregaSpoolConAsignacion(int ordenTrabajoID, int spoolID, int partida, List<ParForzado> lstPares, Guid userID, byte[] versionRegistroOriginal)
        {
            try
            {
                int shopFabAreaID = CacheCatalogos.Instance.ShopFabAreaID;

                using (SamContext ctx = new SamContext())
                {
                    ctx.Spool.MergeOption = MergeOption.NoTracking;
                    ctx.MaterialSpool.MergeOption = MergeOption.NoTracking;
                    ctx.JuntaSpool.MergeOption = MergeOption.NoTracking;
                    ctx.ItemCode.MergeOption = MergeOption.NoTracking;

                    int[] spoolIds = new int[] { spoolID };

                    if (RevisionHoldsBO.Instance.SpoolTieneHold(ctx, spoolID))
                    {
                        throw new ExcepcionEnHold(MensajesError.Excepcion_SpoolEnHold);
                    }

                    //Asegurarnos que el spool no esté en otra ODT
                    if (!ValidacionesOdt.ValidaSpoolsSinOdt(ctx, spoolIds))
                    {
                        throw new BaseValidationException(MensajesError.Excepcion_SpoolYaTieneOdt);
                    }

                    //Revisar que la partida/número de control no haya sido tomada por otro spool de la misma ODT
                    if (!ValidacionesOdt.ValidaPartidaDisponible(ctx, ordenTrabajoID, partida))
                    {
                        throw new BaseValidationException(MensajesError.Odt_ElNumeroDeControlYaExiste);
                    }

                    OrdenTrabajo odt = ctx.OrdenTrabajo.Where(x => x.OrdenTrabajoID == ordenTrabajoID).Single();
                    odt.VersionRegistro = versionRegistroOriginal;

                    CruceForzado cruce = new CruceForzado(ctx, odt.ProyectoID, spoolID, userID, lstPares);
                    List<NumeroUnico> lstNumUnico;
                    List<OrdenTrabajoMaterial> lstOdtm;

                    //Efectuar el cruce para estos spools en particular, en caso que no se pueda esto arroja una excepción
                    Spool spool = cruce.Procesa(out lstNumUnico, out lstOdtm);

                    //Revisar que se hayan podido cruzar los spools
                    if (!spool.InfoCruce.CruceExitoso)
                    {
                        List<string> errores =
                            (from ms in spool.MaterialSpool
                             join pf in lstPares on ms.MaterialSpoolID equals pf.MaterialSpoolID
                             where ms.InfoCruce.NumeroUnicoID <= 0
                             select string.Format(MensajesError.MaterialInsuficiente_NumeroUnicoX, pf.CodigoSegmento)).ToList();

                        throw new ExcepcionMaterialInsuficiente(errores);
                    }

                    //Etiquetamos spool
                    spool.StartTracking();
                    spool.FechaEtiqueta = DateTime.Now;
                    spool.NumeroEtiqueta = odt.NumeroOrden;
                    spool.UsuarioModifica = userID;
                    spool.FechaModificacion = DateTime.Now;
                    ctx.Spool.ApplyChanges(spool);

                    odt.StartTracking();
                    odt.FechaModificacion = DateTime.Now;
                    odt.UsuarioModifica = userID;

                    #region Generar un nuevo objeto en la ODT para el spool
                    OrdenTrabajoSpool odts = new OrdenTrabajoSpool
                    {
                        FechaModificacion = DateTime.Now,
                        NumeroControl = string.Concat(odt.NumeroOrden, '-', partida.ToString().PadLeft(3, '0')),
                        Partida = partida,
                        SpoolID = spool.SpoolID,
                        UsuarioModifica = userID,
                        EsAsignado = true
                    };
                    #endregion

                    #region Iterar las juntas para crearlas en la ODT
                    List<JuntaSpool> jsSoloShop = spool.JuntaSpool.Where(x => x.FabAreaID == shopFabAreaID).ToList();

                    foreach (JuntaSpool js in jsSoloShop)
                    {
                        OrdenTrabajoJunta odtj = new OrdenTrabajoJunta
                        {
                            FechaModificacion = DateTime.Now,
                            JuntaSpoolID = js.JuntaSpoolID,
                            FueReingenieria = false,
                            UsuarioModifica = userID
                        };

                        odts.OrdenTrabajoJunta.Add(odtj);
                    }
                    #endregion

                    #region Iterar los materiales para crearlos en la ODT
                    foreach (MaterialSpool ms in spool.MaterialSpool)
                    {
                        OrdenTrabajoMaterial odtm = new OrdenTrabajoMaterial
                        {
                            CantidadCongelada = ms.Cantidad,
                            CantidadDespachada = 0,
                            CongeladoEsEquivalente = ms.InfoCruce.EsEquivalente,
                            DespachoEsEquivalente = false,
                            NumeroUnicoCongeladoID = ms.InfoCruce.NumeroUnicoID,
                            NumeroUnicoSugeridoID = ms.InfoCruce.EsSugerido ? (int?)ms.InfoCruce.NumeroUnicoID : null,
                            MaterialSpoolID = ms.MaterialSpoolID,
                            SegmentoCongelado = string.IsNullOrEmpty(ms.InfoCruce.Segmento) ? null : ms.InfoCruce.Segmento,
                            SegmentoSugerido = ms.InfoCruce.EsSugerido && !string.IsNullOrEmpty(ms.InfoCruce.Segmento) ? ms.InfoCruce.Segmento : null,
                            SugeridoEsEquivalente = ms.InfoCruce.EsEquivalente,
                            TieneInventarioCongelado = true,
                            FechaModificacion = DateTime.Now,
                            FueReingenieria = false,
                            UsuarioModifica = userID,
                            EsAsignado = true,
                            SegmentoAsignado = string.IsNullOrEmpty(ms.InfoCruce.Segmento) ? null : ms.InfoCruce.Segmento,
                            NumeroUnicoAsignadoID = ms.InfoCruce.NumeroUnicoID
                        };
                    }
                    #endregion

                    //Agregarlo a la ODT
                    odt.OrdenTrabajoSpool.Add(odts);

                    //cambios a los números únicos (inventarios)
                    lstNumUnico.ForEach(ctx.NumeroUnico.ApplyChanges);

                    //cambios a las odts que se pueden haber visto afectadas por los trueques
                    lstOdtm.ForEach(ctx.OrdenTrabajoMaterial.ApplyChanges);

                    //Cambio a la ODT per se
                    ctx.OrdenTrabajo.ApplyChanges(odt);
                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_Concurrencia);
            }
        }

        /// <summary>
        /// Este método agrega una lista de spools de un archivo csv a una orden trabajo nueva o existente
        /// Lleva a cabo las comprobaciones en la lógica de negocios para saber si existe suficiente material para
        /// fabricar el spool seleccionado y congelar los inventarios correspondientes.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="usuarioModifica"></param>
        public void AgregaSpoolPorImportacion(Stream stream, Guid usuarioModifica)
        {
            erroresCsv = new StringBuilder();

            string[] lineas = IOUtils.GetLinesFromTextStream(stream);

            if (lineas.Length == 0)
            {
                throw new ExcepcionPeq(MensajesError.LineasX_Invalido);
            }

            using (SamContext ctx = new SamContext())
            {
                // SpoolID, ProyectoID, TallerID, numeroOT
                List<Tuple<int, int, int, int>> spoolOTs = new List<Tuple<int, int, int, int>>();

                for (int numLinea = 0; numLinea < lineas.Length; numLinea++)
                {
                    string linea = lineas[numLinea];
                    string[] palabras = linea.Split(',');

                    string nombreProyecto = palabras[0];
                    string nombreSpool = palabras[1];
                    string nombreTaller = palabras[2];
                    string numeroOT = palabras[3];

                    //Verificar si el primer renglon son titulos
                    if (!nombreSpool.ToLower().StartsWith("spool"))
                    {
                        int proyectoId = ctx.Proyecto.SingleOrDefault(x => x.Nombre == nombreProyecto).ProyectoID;
                        Spool s = ctx.Spool
                                     .Include("Proyecto.Patio.Taller")
                                     .SingleOrDefault(x => x.Nombre == nombreSpool && x.ProyectoID == proyectoId);

                        bool lineaValida = true;

                        if (numeroOT.SafeIntParse() == -1)
                        {
                            erroresCsv.Append(string.Format(MensajesError.Csv_OtX_Invalido, numLinea + 1, nombreSpool));
                            lineaValida = false;
                        }

                        if (s == null)
                        {
                            erroresCsv.Append(string.Format(MensajesError.Csv_SpoolX_Invalido, numLinea + 1, nombreSpool));
                            lineaValida = false;
                        }
                        else if (!s.Proyecto.Patio.Taller.Any(x => x.Nombre == nombreTaller))
                        {
                            erroresCsv.Append(string.Format(MensajesError.Csv_TallerX_Invalido, numLinea + 1, nombreSpool));
                            lineaValida = false;
                        }
                        else if (ctx.OrdenTrabajoSpool.Any(X => X.SpoolID == s.SpoolID))
                        {
                            erroresCsv.Append(string.Format(MensajesError.Csv_SpoolOtX_Existe, numLinea + 1, nombreSpool));
                            lineaValida = false;
                        }

                        if (lineaValida)
                        {
                            int tallerID = s.Proyecto.Patio.Taller.FirstOrDefault(x => x.Nombre == nombreTaller).TallerID;
                            spoolOTs.Add(Tuple.Create(s.SpoolID, s.ProyectoID, tallerID, numeroOT.SafeIntParse()));
                        }
                    }
                }


                for (int i = 0; i < spoolOTs.Count; i++)
                {
                    int spoolID = spoolOTs[i].Item1;
                    int proyectoID = spoolOTs[i].Item2;
                    int tallerID = spoolOTs[i].Item3;
                    int numeroOrden = spoolOTs[i].Item4;

                    string txtNumeroOrden = numeroOrden.ToString();

                    OrdenTrabajo ordenTrabajo = ctx.OrdenTrabajo.SingleOrDefault(x => x.NumeroOrden.Contains(txtNumeroOrden) && x.ProyectoID == proyectoID);

                    if (ordenTrabajo != null)
                    {
                        GeneraCruceODTExistente(proyectoID,
                                                new int[] { spoolID },
                                                ordenTrabajo.OrdenTrabajoID,
                                                DateTime.Now,
                                                usuarioModifica,
                                                String.Empty,
                                                true);
                    }
                    else
                    {
                        GeneraNueva(proyectoID,
                                    new int[] { spoolID },
                                    tallerID,
                                    numeroOrden,
                                    DateTime.Now,
                                    usuarioModifica,
                                    false,
                                    String.Empty,
                                    true);
                    }
                }

                if (!String.IsNullOrEmpty(erroresCsv.ToString()))
                {
                    throw new ExcepcionPeq(MensajesError.CsvX_Invalido + erroresCsv.ToString());
                }
            }
        }
    }
}
