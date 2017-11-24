using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Excepciones;
using SAM.Entities.Personalizadas;
using SAM.Web;
using SAM.BusinessObjects.Catalogos;

namespace SAM.BusinessObjects.Workstatus
{
    public class EmbarqueBO
    {
        private static readonly object _mutex = new object();
        private static EmbarqueBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private EmbarqueBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase InspeccionDimensionalBO
        /// </summary>
        /// <returns></returns>
        public static EmbarqueBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new EmbarqueBO();
                    }
                }
                return _instance;
            }
        }


        /// <summary>
        /// Obtiene el listado de spool dependiendo de la accion a realizar en embarque
        /// </summary>
        /// <param name="proyectoID">ID del proyecto</param>
        /// <param name="ordenTrabajoID">ID de la orden de trabajo</param>
        /// <param name="accion">Accion a realizar (1-2) preparar o etiquetar (3) imprimir etiquetas (4) embarcar</param>
        /// <returns></returns>
        public List<GrdEmbarque> ObtenListadoParaEmbarque(int proyectoID, int ordenTrabajoID, int accion)
        {
            List<GrdEmbarque> lista = new List<GrdEmbarque>();
            List<GrdEmbarque> lista2 = new List<GrdEmbarque>();
            List<WorkstatusSpool> workstatus = null;
            List<Spool> spools = null;
            List<OrdenTrabajoSpool> ordenTrabajoSpool = null;
            List<OrdenTrabajo> ordenTrabajo = null;
            List<EmbarqueSpool> embarqueSpool = null;
            List<Embarque> embarque = null;
            List<SpoolHold> holds = null;

            using (SamContext ctx = new SamContext())
            {
                IEnumerable<int> odt = ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajo.ProyectoID == proyectoID && (ordenTrabajoID == -1 || x.OrdenTrabajoID == ordenTrabajoID)).Select(x => x.OrdenTrabajoSpoolID);
                IEnumerable<WorkstatusSpool> wksSpool = null;
                IEnumerable<OrdenTrabajoSpool> iOT = null;
                IEnumerable<EmbarqueSpool> es = null;
                IEnumerable<Spool> spool = null;

                ctx.WorkstatusSpool.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.OrdenTrabajo.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.Spool.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.OrdenTrabajoSpool.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.SpoolHold.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.Embarque.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.EmbarqueSpool.MergeOption = System.Data.Objects.MergeOption.NoTracking;

                //preparar
                if (accion == 0)
                {
                    wksSpool = ctx.WorkstatusSpool.Where(x => odt.Contains(x.OrdenTrabajoSpoolID) && x.TieneLiberacionDimensional);
                    iOT = ctx.OrdenTrabajoSpool.Where(x => wksSpool.Select(y => y.OrdenTrabajoSpoolID).Contains(x.OrdenTrabajoSpoolID));
                    spool = ctx.Spool.Where(x => iOT.Select(y => y.SpoolID).Contains(x.SpoolID));
                }
                else if (accion == 1) // etiquetar
                {
                    wksSpool = ctx.WorkstatusSpool.Where(x => odt.Contains(x.OrdenTrabajoSpoolID));
                    iOT = ctx.OrdenTrabajoSpool.Where(x => odt.Contains(x.OrdenTrabajoSpoolID));
                    spool = ctx.Spool.Where(x => iOT.Select(y => y.SpoolID).Contains(x.SpoolID));
                }
                else if (accion == 2)//imprimir
                {
                    iOT = ctx.OrdenTrabajoSpool.Where(x => odt.Contains(x.OrdenTrabajoSpoolID));
                    spool = ctx.Spool.Where(x => iOT.Select(y => y.SpoolID).Contains(x.SpoolID) && x.NumeroEtiqueta != string.Empty);
                    wksSpool = ctx.WorkstatusSpool.Where(x => odt.Contains(x.OrdenTrabajoSpoolID) && x.OrdenTrabajoSpool.Spool.NumeroEtiqueta != string.Empty);

                }//embarcar
                else if (accion == 3)
                {
                    wksSpool = ctx.WorkstatusSpool.Where(x => odt.Contains(x.OrdenTrabajoSpoolID) && x.TieneLiberacionDimensional && x.OrdenTrabajoSpool.Spool.NumeroEtiqueta != string.Empty && x.Preparado);
                    iOT = ctx.OrdenTrabajoSpool.Where(x => wksSpool.Select(y => y.OrdenTrabajoSpoolID).Contains(x.OrdenTrabajoSpoolID));
                    spool = ctx.Spool.Where(x => iOT.Select(y => y.SpoolID).Contains(x.SpoolID) && x.NumeroEtiqueta != string.Empty);
                }



                ordenTrabajo = ctx.OrdenTrabajo.Where(x => iOT.Select(y => y.OrdenTrabajoID).Contains(x.OrdenTrabajoID)).ToList();
                es = ctx.EmbarqueSpool.Where(x => wksSpool.Select(y => y.WorkstatusSpoolID).Contains(x.WorkstatusSpoolID));
                embarque = ctx.Embarque.Where(x => es.Select(y => y.EmbarqueID).Contains(x.EmbarqueID)).ToList();
                holds = ctx.SpoolHold.Where(x => spool.Select(y => y.SpoolID).Contains(x.SpoolID)).ToList();

                embarqueSpool = es.ToList();
                ordenTrabajoSpool = iOT.ToList();
                workstatus = wksSpool.ToList();
                spools = spool.ToList();
            }

            if (workstatus.Count > 0)
            {

                lista = (from wks in workstatus
                         join ots in ordenTrabajoSpool on wks.OrdenTrabajoSpoolID equals ots.OrdenTrabajoSpoolID
                         join ot in ordenTrabajo on ots.OrdenTrabajoID equals ot.OrdenTrabajoID
                         join sp in spools on ots.SpoolID equals sp.SpoolID
                         join emb in embarqueSpool on wks.WorkstatusSpoolID equals emb.WorkstatusSpoolID into EmbarquesSpool
                         from embSpool in EmbarquesSpool.DefaultIfEmpty()
                         let embarques = embarque
                         join hold in holds on sp.SpoolID equals hold.SpoolID into Holds
                         from hd in Holds.DefaultIfEmpty()
                         select new GrdEmbarque
                         {
                             WorkstatusSpoolID = wks.WorkstatusSpoolID,
                             SpoolID = sp.SpoolID,
                             OrdenTrabajo = ot.NumeroOrden,
                             OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID,
                             NumeroControl = ots.NumeroControl,
                             NombreSpool = sp.Nombre,
                             Area = sp.Area.Value,
                             PDI = sp.Pdis.Value,
                             Peso = sp.Peso.Value,
                             Etiqueta = sp.NumeroEtiqueta,
                             VigenciaAduana = (embarques == null) || (embSpool == null) ? string.Empty : embarques.Where(x => x.EmbarqueID == embSpool.EmbarqueID).Select(x => x.VigenciaAduana).FirstOrDefault().SafeDateAsStringParse(),
                             Preparado = wks.Preparado,
                             FolioPreparacion = wks.FechaPreparacion.HasValue ? 
                                            wks.FechaPreparacion.Value.ToString("yyyy-MM-dd") + 
                                            (wks.FolioPreparacion > 0 ? "-" + wks.FolioPreparacion.SafeIntParse().ToString("000") : string.Empty) 
                                            : string.Empty,
                             Embarque = (embarques == null) || (embSpool == null) ? string.Empty : embarques.Where(x => x.EmbarqueID == embSpool.EmbarqueID).Select(x => x.NumeroEmbarque).FirstOrDefault(),
                             FechaEmbarque = (embarques == null) || (embSpool == null) ? string.Empty : embarques.Where(x => x.EmbarqueID == embSpool.EmbarqueID).Select(x => x.FechaEmbarque).FirstOrDefault().SafeDateAsStringParse(),
                             FechaEstimada = (embarques == null) || (embSpool == null) ? string.Empty : embarques.Where(x => x.EmbarqueID == embSpool.EmbarqueID).Select(x => x.FechaEstimada).FirstOrDefault().SafeDateAsStringParse(),
                             Hold = (hd == null) ? false : hd.TieneHoldCalidad || hd.TieneHoldIngenieria || hd.Confinado
                         }).ToList();

                if (accion == 1 || accion == 2)
                {
                    lista2 = (from ots in ordenTrabajoSpool
                              join ot in ordenTrabajo on ots.OrdenTrabajoID equals ot.OrdenTrabajoID
                              join sp in spools on ots.SpoolID equals sp.SpoolID
                              join hold in holds on sp.SpoolID equals hold.SpoolID into Holds
                              from hd in Holds.DefaultIfEmpty()
                              select new GrdEmbarque
                              {
                                  WorkstatusSpoolID = -1,
                                  SpoolID = sp.SpoolID,
                                  OrdenTrabajo = ot.NumeroOrden,
                                  OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID,
                                  NumeroControl = ots.NumeroControl,
                                  NombreSpool = sp.Nombre,
                                  Area = sp.Area.Value,
                                  PDI = sp.Pdis.Value,
                                  Peso = sp.Peso.Value,
                                  Etiqueta = sp.NumeroEtiqueta,
                                  VigenciaAduana = string.Empty,
                                  Preparado = false,
                                  FolioPreparacion = string.Empty,
                                  Embarque = string.Empty,
                                  FechaEmbarque = string.Empty,
                                  Hold = (hd == null) ? false : hd.TieneHoldCalidad || hd.TieneHoldIngenieria || hd.Confinado
                              }).Where(x => !workstatus.Select(y => y.OrdenTrabajoSpoolID).Contains(x.OrdenTrabajoSpoolID)).ToList();

                    lista = lista.Union(lista2).ToList();
                }
            }
            else
            {
                lista = (from ots in ordenTrabajoSpool
                         join ot in ordenTrabajo on ots.OrdenTrabajoID equals ot.OrdenTrabajoID
                         join sp in spools on ots.SpoolID equals sp.SpoolID
                         join hold in holds on sp.SpoolID equals hold.SpoolID into Holds
                         from hd in Holds.DefaultIfEmpty()
                         select new GrdEmbarque
                         {
                             WorkstatusSpoolID = -1,
                             SpoolID = sp.SpoolID,
                             OrdenTrabajo = ot.NumeroOrden,
                             OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID,
                             NumeroControl = ots.NumeroControl,
                             NombreSpool = sp.Nombre,
                             Area = sp.Area.Value,
                             PDI = sp.Pdis.Value,
                             Peso = sp.Peso.Value,
                             Etiqueta = sp.NumeroEtiqueta,
                             VigenciaAduana = string.Empty,
                             Preparado = false,
                             FolioPreparacion = string.Empty,
                             Embarque = string.Empty,
                             FechaEmbarque = string.Empty,
                             Hold = (hd == null) ? false : hd.TieneHoldCalidad || hd.TieneHoldIngenieria || hd.Confinado
                         }).ToList();
            }

            return lista;
        }

        /// <summary>
        /// Obtiene el listado de spools a imprimir etiquetas
        /// </summary>
        /// <param name="workstatusSpoolIDs">Listado de workstatusspools ids</param>
        /// <returns>Listado de Spool con OrdenTrabajoSpool</returns>
        public List<Spool> ObtenSpoolsImpresion(int[] workstatusSpoolIDs)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.WorkstatusSpool.MergeOption = System.Data.Objects.MergeOption.NoTracking;

                IQueryable<int> ots = ctx.OrdenTrabajoSpool.Where(x => ctx.WorkstatusSpool.Where(y => workstatusSpoolIDs.Contains(y.WorkstatusSpoolID)).Select(y => y.OrdenTrabajoSpoolID).Contains(x.OrdenTrabajoSpoolID)).Select(x => x.SpoolID);
                return ctx.Spool.Include("OrdenTrabajoSpool").Where(x => ots.Contains(x.SpoolID)).ToList();
            }
        }

        /// <summary>
        /// Obtiene una lista de Embarques por proyecto
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="skip">Cantidad de datos a ignorar</param>
        /// <param name="take">Cantidad de datos de retorno</param>
        /// <returns>Listado de embarques (Id y Numero de embarque)</returns>
        public IEnumerable<Simple> ObtenerNumerosDeEmbarquePorProyecto(int? proyectoID, string buscar, int skip, int take)
        {
            IEnumerable<Simple> embarques;
            using (SamContext ctx = new SamContext())
            {
                embarques = (from embarque in ctx.Embarque
                             where embarque.ProyectoID == proyectoID
                             select new Simple
                             {
                                 ID = embarque.EmbarqueID,
                                 Valor = embarque.NumeroEmbarque
                             }).ToList();
            }

            return embarques.Where(x => x.Valor.StartsWith(buscar, StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(x => x.Valor)
                .Skip(skip)
                .Take(take);
        }

        /// <summary>
        /// Obtiene las fechas de embarque y estimada de un embarque
        /// </summary>
        /// <param name="embarqueID"></param>
        /// <returns>Lista con valores de fechas</returns>
        public FechasEmbarque ObtenerFechaEmbarque(int embarqueID)
        {
            using (SamContext ctx = new SamContext())
            {
                FechasEmbarque fechas = (from embarques in ctx.Embarque
                                         where embarques.EmbarqueID == embarqueID
                                         select new FechasEmbarque
                                         {
                                             FechaEmbarque = embarques.FechaEmbarque,
                                             FechaEstimada = embarques.FechaEstimada
                                         }).Single();
                return fechas;
            }
        }

        public Embarque ObtenerEmbarque(int id)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Embarque.Where(x => x.EmbarqueID == id).SingleOrDefault();
            }
        }


        public void GuardarFechasEmbarque(int embarqueID, DateTime? fechaEstimada, DateTime? fechaEmbarque, Guid usuarioModifica)
        {
            using (SamContext ctx = new SamContext())
            {
                Embarque embarque = ctx.Embarque.Where(x => x.EmbarqueID == embarqueID).Single();

                if (fechaEmbarque.HasValue && fechaEmbarque != embarque.FechaEmbarque)
                {
                    embarque.FechaEmbarque = Convert.ToDateTime(fechaEmbarque);
                }
                if (fechaEstimada.HasValue && fechaEstimada != embarque.FechaEstimada)
                {
                    embarque.FechaEstimada = Convert.ToDateTime(fechaEstimada);
                }
                if (fechaEmbarque.HasValue)
                {                   
                    //fecha estimada en bd = fecha Carga 
                    if (embarque.FechaEstimada > embarque.FechaEmbarque)
                    {
                        throw new ExcepcionEmbarque(MensajesError.Excepcion_FechaCargaMayorFechaEmbarque);
                    }                  
            
                    embarque.FechaModificacion = DateTime.Now.Date;
                    embarque.UsuarioModifica = usuarioModifica;
                    ctx.Embarque.ApplyChanges(embarque);

                    bool actualizaLocalizacion = ctx.ProyectoConfiguracion.Where(x => x.ProyectoID == embarque.ProyectoID).Select(x => x.ActualizaLocalizacion).SingleOrDefault();
                    
                    if (actualizaLocalizacion)
                    {
                        List<int> workStatusIds = ctx.EmbarqueSpool.Where(x => x.EmbarqueID == embarqueID).Select(x => x.WorkstatusSpoolID).ToList();

                        int[] otsIds = ctx.WorkstatusSpool.Where(x => workStatusIds.Contains(x.WorkstatusSpoolID)).Select(x => x.OrdenTrabajoSpoolID).ToArray();

                        if (otsIds != null && otsIds.Length > 0)
                        {
                            int patioId = ctx.Proyecto.Where(x => x.ProyectoID == embarque.ProyectoID).Select(x => x.PatioID).SingleOrDefault();
                            int cuadrante = ctx.Cuadrante.Where(x => x.PatioID == patioId && x.Nombre.Equals("Embarcado")).Select(x => x.CuadranteID).SingleOrDefault();
                            if (cuadrante > 0)
                            {
                                DateTime fechaLocalizacion = embarque.FechaEmbarque.HasValue ? embarque.FechaEmbarque.Value : DateTime.Now;
                                CuadranteBO.Instance.GuardarCuadranteSpools(cuadrante, otsIds, fechaLocalizacion, usuarioModifica);
                            }
                            else
                            {
                                throw new ExcepcionEmbarque(MensajesError.Excepcion_PatioSinCuadranteEmbarque);
                            }
                        }
                    }

                    ctx.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Obtiene los ids de los spools para la impresion de etiquetas
        /// </summary>
        /// <param name="workstatusSpoolsIDs">IDs de los spools a imprimir</param>
        /// <param name="numeroEtiqueta">Numero de Etiqueta a imprimir</param>
        /// <param name="numeroControl">Numero de control de los spools a imprimir</param>
        /// <param name="OrdenTrabajo">Orden de trabajo a imprimir</param>
        /// <param name="proyectoID">Id del proyecto</param>
        /// <returns></returns>
        //public int[] ObtenIDsParaImpresion(int[] workstatusSpoolsIDs, string numeroEtiqueta, string numeroControl, string ordenTrabajo, int proyectoID)
        //{
        //    using (SamContext ctx = new SamContext())
        //    {
        //        bool existenFiltros = false;
        //        List<WorkstatusSpool> workstatusSpool = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpool.OrdenTrabajo.ProyectoID == proyectoID && x.NumeroEtiqueta != string.Empty).ToList();

        //        Si se dan una serie de workstatusspool definida obtener los filtros a partir de estos
        //        if (workstatusSpoolsIDs != null)
        //        {
        //            existenFiltros = true;
        //            workstatusSpool = workstatusSpool.Where(x => workstatusSpoolsIDs.Contains(x.WorkstatusSpoolID)).ToList();
        //        }

        //        Filtrar por numero de etiqueta
        //        if (numeroEtiqueta != string.Empty)
        //        {
        //            existenFiltros = true;
        //            workstatusSpool = workstatusSpool.Where(x => x.NumeroEtiqueta == numeroEtiqueta).ToList();
        //        }

        //        Filtrar por numero de control
        //        if (numeroControl != string.Empty)
        //        {
        //            existenFiltros = true;
        //            int odt = ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajo.ProyectoID == proyectoID && x.NumeroControl == numeroControl).Select(x => x.OrdenTrabajoSpoolID).SingleOrDefault();

        //            if (odt < 1)
        //            {
        //                throw new ExcepcionOdt(MensajesError.Excepcion_NumeroControlInexistente);
        //            }
        //            else
        //            {
        //                workstatusSpool = workstatusSpool.Where(x => x.OrdenTrabajoSpoolID == odt).ToList();
        //            }
        //        }

        //        Filtrar por orden de trabajo.
        //        if (ordenTrabajo != string.Empty)
        //        {
        //            existenFiltros = true;
        //            int[] odts = ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajo.ProyectoID == proyectoID && x.OrdenTrabajo.NumeroOrden == ordenTrabajo).Select(x => x.OrdenTrabajoSpoolID).ToArray();

        //            if (odts.Count() < 1)
        //            {
        //                throw new ExcepcionOdt(MensajesError.Excepcion_OrdenTrabajoInexistente);
        //            }
        //            else
        //            {
        //                workstatusSpool = workstatusSpool.Where(x => odts.Contains(x.OrdenTrabajoSpoolID)).ToList();
        //            }
        //        }

        //        if (!existenFiltros)
        //        {
        //            throw new ExcepcionEmbarque(MensajesError.Excepcion_ImpresionEtiquetasSinFiltros);
        //        }

        //        int[] wks = workstatusSpool.Select(x => x.WorkstatusSpoolID).ToArray();

        //        if (wks.Count() < 1)
        //        {
        //            throw new ExcepcionEmbarque(MensajesError.Excepcion_ImpresionSinResultados);
        //        }

        //        return wks;
        //    }
        //}

        /// <summary>
        /// Guarda el embarque para cada uno de los spools enviados
        /// </summary>
        /// <param name="embarque"></param>
        /// <param name="workstatusSpoolsIDs"></param>
        /// <param name="userID"></param>
        public bool GuardaEmbarque(Embarque embarque, int[] workstatusSpoolsIDs, Guid userID)
        {
            bool mostrarMensaje = false;

            using (SamContext ctx = new SamContext())
            {
                bool actualizaLocalizacion = ctx.ProyectoConfiguracion.Where(x => x.ProyectoID == embarque.ProyectoID).Select(x => x.ActualizaLocalizacion).SingleOrDefault();

                //Verifico la existencia del embarque
                Embarque embarqueExistente = ctx.Embarque.Where(x => x.NumeroEmbarque == embarque.NumeroEmbarque && x.ProyectoID == embarque.ProyectoID).SingleOrDefault();
                if (embarqueExistente != null)
                {
                    //fecha estimada en bd = fecha Carga 
                    if (embarqueExistente.FechaEstimada != embarque.FechaEstimada)
                    {
                        throw new ExcepcionEmbarque(MensajesError.Excepcion_EmbarqueExistenteConFechaDiferente);
                    }
                    if (embarque.VigenciaAduana != null)
                    {
                        embarqueExistente.VigenciaAduana = embarque.VigenciaAduana;
                    }

                    embarque = embarqueExistente;
                    embarque.StartTracking();
                }

                List<string> errores = new List<string>();
                string spoolFecahNoValida = string.Empty;
                bool modificadoOAgeragado = false;
                embarque.FechaModificacion = DateTime.Now;
                embarque.UsuarioModifica = userID;
                ctx.Embarque.ApplyChanges(embarque);

                List<int> embarquesIdsAnteriores = new List<int>();

                foreach (int wksID in workstatusSpoolsIDs)
                {
                    if (wksID > -1)
                    {
                        modificadoOAgeragado = true;
                        EmbarqueSpool embSpool = ctx.EmbarqueSpool.Where(x => x.WorkstatusSpoolID == wksID).SingleOrDefault();
                        if (embSpool != null)
                        {
                            embarquesIdsAnteriores.Add(embSpool.EmbarqueID);
                        }
                        WorkstatusSpool wks = ctx.WorkstatusSpool.Where(x => x.WorkstatusSpoolID == wksID).SingleOrDefault();
                        OrdenTrabajoSpool ots = ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajoSpoolID == wks.OrdenTrabajoSpoolID).FirstOrDefault();
                    
                        if (embSpool != null)
                        {
                            embSpool.StartTracking();
                            embSpool.Embarque = embarque;
                        }
                        else
                        {
                            embSpool = new EmbarqueSpool
                            {
                                Embarque = embarque,
                                WorkstatusSpoolID = wksID
                            };
                        }

                        embSpool.UsuarioModifica = userID;
                        embSpool.FechaModificacion = DateTime.Now;

                        if (embarque.EmbarqueSpool.Contains(embSpool))
                        {
                            ctx.EmbarqueSpool.ApplyChanges(embSpool); ;
                        }
                        else
                        {
                            embarque.EmbarqueSpool.Add(embSpool);
                        }                      

                        wks.StartTracking();
                        wks.Embarcado = true;
                        wks.UltimoProcesoID = (int)UltimoProcesoEnum.Embarque;
                        wks.UsuarioModifica = userID;
                        wks.FechaModificacion = DateTime.Now;
                        ctx.WorkstatusSpool.ApplyChanges(wks);                       
                        
                        if (actualizaLocalizacion)
                        {
                            int patioId = ctx.Proyecto.Where(x => x.ProyectoID == embarque.ProyectoID).Select(x => x.PatioID).SingleOrDefault();

                            int cuadrante = ctx.Cuadrante.Where(x => x.PatioID == patioId && x.Nombre.Equals("Cargado")).Select(x => x.CuadranteID).SingleOrDefault();

                            if (cuadrante > 0)
                            {
                                CuadranteBO.Instance.GuardarCuadranteSpool(cuadrante, ots.OrdenTrabajoSpoolID, embarque.FechaEstimada, userID);
                            }
                            else
                            {
                                mostrarMensaje = true;
                            }
                        }                       
                    }
                    else
                    {
                        int otsID = ctx.WorkstatusSpool.Where(x => x.WorkstatusSpoolID == (wksID * -1)).Select(x => x.OrdenTrabajoSpoolID).FirstOrDefault();

                        OrdenTrabajoSpool ots = ctx.OrdenTrabajoSpool.Include("Spool").Where(x => x.OrdenTrabajoSpoolID == otsID).FirstOrDefault();
                        spoolFecahNoValida += ots.Spool.Nombre + ", ";
                    }
                }

                if (modificadoOAgeragado)
                {
                    ctx.SaveChanges();

                    if (embarquesIdsAnteriores.Count > 0)
                    {
                        foreach(int embarqueId in embarquesIdsAnteriores)
                        {
                            List<EmbarqueSpool> embarquesSpool = ctx.EmbarqueSpool.Where(x => x.EmbarqueID == embarqueId).ToList();
                           
                            if (embarquesSpool.Count == 0)
                            { 
                                Embarque embarqueEliminar = ctx.Embarque.Where(x => x.EmbarqueID == embarqueId).FirstOrDefault();
                                ctx.Embarque.DeleteObject(embarqueEliminar);
                            }
                        }

                        ctx.SaveChanges();
                    }

                   
                }

                if(!string.IsNullOrEmpty(spoolFecahNoValida))
                {
                    throw new ExcepcionEmbarque(string.Format(MensajesError.Excepcion_FechaMenorFechaProcesoAnterior, spoolFecahNoValida));                                                              
                }                
            }

            return mostrarMensaje;
        }

        /// <summary>
        /// Etiqueta los spools recibidos
        /// </summary>
        /// <param name="workstatusSpoolsIDs">IDs a etiquetar</param>
        /// <param name="numeroEtiqueta">Numero de etiqueta</param>
        /// <param name="fechaEtiqueta">Fecha de etiqueta</param>
        /// <param name="userID">Usuario que modifica</param>
        public void EtiquetaSpools(int[] spoolsIDs, string numeroEtiqueta, Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                foreach (int id in spoolsIDs)
                {
                    Spool spool = ctx.Spool.Where(x => x.SpoolID == id).SingleOrDefault();
                    spool.StartTracking();
                    spool.NumeroEtiqueta = numeroEtiqueta;
                    spool.UsuarioModifica = userID;
                    spool.FechaModificacion = DateTime.Now;

                    ctx.Spool.ApplyChanges(spool);
                }

                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Prepara los spoosl enviados
        /// </summary>
        /// <param name="workstatusSpoolsIDs">IDs de los spools a preparar</param>        
        /// <param name="fechaPreparacion">Fecha Preparacion</param>
        /// <param name="userID">Usuario que modifica</param>
        public void PrepararSpools(int[] workstatusSpoolsIDs, int Folio, Guid userID, DateTime fechaPreparacion)
        {
            using (SamContext ctx = new SamContext())
            {
                foreach (int id in workstatusSpoolsIDs)
                {
                    WorkstatusSpool wks = ctx.WorkstatusSpool.Where(x => x.WorkstatusSpoolID == id).SingleOrDefault();
                    wks.StartTracking();
                    wks.FechaPreparacion = fechaPreparacion;
                    wks.FolioPreparacion = Folio;
                    wks.Preparado = true;
                    wks.UltimoProcesoID = (int)UltimoProcesoEnum.Embarque;
                    wks.UsuarioModifica = userID;
                    wks.FechaModificacion = DateTime.Now;

                    ctx.WorkstatusSpool.ApplyChanges(wks);
                }

                ctx.SaveChanges();
            }
        }

        public int ObtenerProyectoPorWkSID(int wksID)
        {
            using (SamContext ctx = new SamContext())
            {
                int proyectoId = (from wks in ctx.WorkstatusSpool
                                  where wks.WorkstatusSpoolID == wksID
                                  select wks.OrdenTrabajoSpool.OrdenTrabajo.ProyectoID).FirstOrDefault();
                return proyectoId;
            }
        }

        public void EliminaEmbarqueSpool(int spoolID, Guid userID, int tipoAccion)
        {
            using (SamContext ctx = new SamContext())
            {
                WorkstatusSpool wks = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpool.Spool.SpoolID == spoolID).FirstOrDefault();
                EmbarqueSpool embarqueSpool = ctx.EmbarqueSpool.Where(x => x.WorkstatusSpoolID == wks.WorkstatusSpoolID).FirstOrDefault();
                Spool spool = ctx.Spool.Where(x => x.SpoolID == spoolID).FirstOrDefault();
                wks.StartTracking();

                switch ((TipoAccionEmbarqueEnum)tipoAccion)
                {
                    case TipoAccionEmbarqueEnum.Etiquetado:
                        if (wks.Embarcado)
                        {
                            throw new ExcepcionEmbarque(MensajesError.Excepcion_CuentaConEmbarque);
                        }

                        spool.StartTracking();
                        spool.FechaEtiqueta = null;
                        spool.NumeroEtiqueta = null;
                        ctx.Spool.ApplyChanges(spool);
                        break;

                    case TipoAccionEmbarqueEnum.Preparacion:
                        if (wks.Embarcado)
                        {
                            throw new ExcepcionEmbarque(MensajesError.Excepcion_CuentaConEmbarque);
                        }

                        wks.FechaPreparacion = null;
                        wks.Preparado = false;
                        break;

                    case TipoAccionEmbarqueEnum.Embarque:
                        wks.Embarcado = false;
                       
                        //Si es el único embarqueSpool con este EmbarqueID se procede a eliminar el registro de la tabla Embarque
                        if (ctx.EmbarqueSpool.Where(x => x.EmbarqueID == embarqueSpool.EmbarqueID).Count() == 1)
                        {
                            Embarque embarque = ctx.Embarque.Where(x => x.EmbarqueID == embarqueSpool.EmbarqueID).SingleOrDefault();
                            ctx.Embarque.DeleteObject(embarque);
                        }
                        ctx.EmbarqueSpool.DeleteObject(embarqueSpool);

                        //Se debe resetear el dato de cuadranteID del spool si el cuadrante registrado corresponde al cuadrante de embarque
                        //----Al menos es lo que he entendido XD ----
                        //Obtenemos el patio del proyecto
                        int patioId = ctx.Proyecto.Where(x => x.ProyectoID == spool.ProyectoID).Select(x => x.PatioID).SingleOrDefault();
                        //buscamos si el patio tiene un cuadrante de "Embarcado", que es que se coloca de manera automatica al generar un Embarque
                        int cuadranteIdEmbarcado = ctx.Cuadrante.Where(x => x.PatioID == patioId && x.Nombre == "Embarcado").Select(x => x.CuadranteID).SingleOrDefault();
                        int cuadranteIdCargado = ctx.Cuadrante.Where(x => x.PatioID == patioId && x.Nombre == "Cargado").Select(x => x.CuadranteID).SingleOrDefault();
                        //si existe el cuadrante de embarcado
                        if (cuadranteIdEmbarcado > 0 || cuadranteIdCargado > 0)
                        {
                            //comparamos con el cuadrante registrado en el Spool y si es igual lo reseteamos
                            if (spool.CuadranteID == cuadranteIdEmbarcado || spool.CuadranteID == cuadranteIdCargado)
                            {
                                //modificacmos el registro del Spool
                                spool.StartTracking();
                                spool.CuadranteID = null;
                                spool.FechaModificacion = DateTime.Now;
                                spool.UsuarioModifica = userID;
                                spool.FechaLocalizacion = null;
                                spool.StopTracking();
                                ctx.Spool.ApplyChanges(spool);

                                //Eliminamos el ultimo registro que coincida con el cuadrante especificado
                                List<CuadranteHistorico> historicos = ctx.CuadranteHistorico.Where(x => x.SpoolID == spool.SpoolID && (x.CuadranteID == cuadranteIdEmbarcado || x.CuadranteID == cuadranteIdCargado)).ToList();
                                if (historicos.Count > 0)
                                {
                                    //borramos el registros
                                    foreach(CuadranteHistorico ch in historicos)
                                    {
                                        ctx.CuadranteHistorico.DeleteObject(ch);
                                    }
                                }                                
                            }
                        }

                        break;

                }

                wks.UsuarioModifica = userID;
                wks.FechaModificacion = DateTime.Now;
                ctx.WorkstatusSpool.ApplyChanges(wks);

                ctx.SaveChanges();
            }

        }

        /// <summary>
        /// Obtiene una lista de Embarques por proyecto
        /// </summary>
        /// <param name="proyectoID"></param>        
        /// <returns>Listado de embarques (Id y Numero de embarque)</returns>
        public IEnumerable<Embarque> ObtenerNumerosDeEmbarquePorProyecto(int proyectoID)
        {
            IEnumerable<Embarque> embarques;
            using (SamContext ctx = new SamContext())
            {
                embarques = ctx.Embarque.Include("EmbarqueSpool").Where(x => x.ProyectoID == x.ProyectoID && x.FechaEmbarque != null).ToList();

                return embarques;
            }
        }

        /// <summary>
        /// Obtiene una lista de Embarques por proyecto que pertenecen a un patio
        /// </summary>
        /// <param name="proyectoID"></param>        
        /// <returns>Listado de embarques (Id y Numero de embarque)</returns>
        public IEnumerable<Embarque> ObtenerNumerosDeEmbarquePorPatio(int patioId)
        {
            IEnumerable<Embarque> embarques;
            using (SamContext ctx = new SamContext())
            {
                List<int> projectoIds = ctx.Proyecto.Where(x => x.PatioID == patioId).Select(x => x.ProyectoID).ToList();


                embarques = ctx.Embarque.Include("EmbarqueSpool").Where(x => projectoIds.Contains(x.ProyectoID) && x.FechaEmbarque == null).ToList();


                return embarques;
            }
        }
        /// <summary>
        /// Guardar fecha embarque
        /// </summary>           
        /// <returns> void</returns>
        public void GuardarFechaEmbarque(int embarqueID, DateTime fechaEmbarque, Guid usuarioModifica)
        {
            using (SamContext ctx = new SamContext())
            {
                Embarque embarque = ctx.Embarque.Where(x => x.EmbarqueID == embarqueID).FirstOrDefault();


                embarque.FechaEmbarque = fechaEmbarque;
              
                embarque.FechaModificacion = DateTime.Now;
                embarque.UsuarioModifica = usuarioModifica;
                ctx.Embarque.ApplyChanges(embarque);
                ctx.SaveChanges();                
            }
        }
    }
}
