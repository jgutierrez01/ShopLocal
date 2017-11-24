using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Validations;
using SAM.Entities;
using SAM.Entities.Grid;
using SAM.Entities.Cache;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Personalizadas;
using System.Transactions;
using Mimo.Framework.Extensions;
using System.Data.Objects;

namespace SAM.BusinessObjects.Workstatus
{
    public class ReporteTtBO
    {
        private static readonly object _mutex = new object();
        private static ReporteTtBO _instance;

        /// <summary>
        /// Constructor privado
        /// </summary>
        private ReporteTtBO()
        {

        }

        /// <summary>
        /// Obtiene la instancia de la clase ReporteTtBO
        /// </summary>
        public static ReporteTtBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ReporteTtBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtiene el listado de estimaciones de acuerdo a los filtros
        /// </summary>
        /// <param name="numeroDeReporte">numeroEstimación de la tabla Estimacion</param>
        /// <param name="proyectoID">Proyecto ID de la tabla Estimacion</param>
        /// <param name="fechaDesde">Fecha Inicial de Búsqueda de la tabla Estimacion</param>
        /// <param name="fechaHasta">Fecha Final de Búsqueda de la tabla Estimacion</param>
        /// <returns></returns>
        public List<GrdReporteTt> ObtenerConFiltros(int proyectoID, DateTime? fechaDesde, DateTime? fechaHasta, string numeroDeReporte, int? TipoPrueba)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<ReporteTt> iqReporteTt = from e in ctx.ReporteTt
                                                    where e.ProyectoID == proyectoID
                                                    select e;

                if (fechaDesde.HasValue)
                {
                    iqReporteTt = iqReporteTt.Where(y => y.FechaReporte >= fechaDesde.Value);
                }

                if (fechaHasta.HasValue)
                {
                    iqReporteTt = iqReporteTt.Where(y => y.FechaReporte <= fechaHasta.Value);
                }

                if (!String.IsNullOrEmpty(numeroDeReporte))
                {
                    iqReporteTt = iqReporteTt.Where(y => y.NumeroReporte == numeroDeReporte);
                }

                if (TipoPrueba != -1)
                {
                    iqReporteTt = iqReporteTt.Where(y => y.TipoPruebaID == TipoPrueba);
                }
                iqReporteTt.ToList();

                return (from r in iqReporteTt
                        let JRTT = r.JuntaReporteTt
                        select new GrdReporteTt
                        {
                            ReporteTtID = r.ReporteTtID,
                            NumeroDeReporte = r.NumeroReporte,
                            Fecha = r.FechaReporte,
                            TipoDePrueba = r.TipoPrueba.Nombre,
                            TipoPruebaID = r.TipoPruebaID,
                            JuntasTotales = JRTT.Count,
                            JuntasAprobadas = JRTT.Where(x => x.Aprobado).Count(),
                            JuntasRechazadas = JRTT.Where(x => !x.Aprobado).Count()
                        }).ToList();
            }
        }

        public ReporteTt Obtener(int reporteTtID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ReporteTt.SingleOrDefault(x => x.ReporteTtID == reporteTtID);
            }
        }

        /// <summary>
        /// Obtiene una liosta de fecha se soldadura (proceso), fecha reporte y el numero de control asociado
        /// </summary>
        /// <param name="reporteTtId"></param>
        /// <returns></returns>
        public List<ListaFechaNumeroControl> ObtenerFechasRequisicionEdicion(int reporteTtId)
        {
            using (SamContext ctx = new SamContext())
            {
                List<ListaFechaNumeroControl> encontrados = (from reportes in ctx.JuntaReporteTt
                                                             join jr in ctx.JuntaRequisicion on reportes.JuntaRequisicionID equals jr.JuntaRequisicionID
                                                             join r in ctx.Requisicion on jr.RequisicionID equals r.RequisicionID
                                                             join jws in ctx.JuntaWorkstatus on jr.JuntaWorkstatusID equals jws.JuntaWorkstatusID
                                                             join js in ctx.JuntaSoldadura on jws.JuntaWorkstatusID equals js.JuntaWorkstatusID
                                                             join odts in ctx.OrdenTrabajoSpool on jws.OrdenTrabajoSpoolID equals odts.OrdenTrabajoSpoolID
                                                             where reportes.ReporteTtID == reporteTtId
                                                             select new ListaFechaNumeroControl
                                                             {
                                                                 FechaProceso = js.FechaSoldadura,
                                                                 FechaReporte = r.FechaRequisicion,
                                                                 NumeroControl = odts.NumeroControl
                                                             }).ToList();
                return encontrados;
            }
        }

        /// <summary>
        /// Valida que el numero de reporte no exista en la base de datos
        /// </summary>
        /// <param name="reporteTtId"></param>
        /// <param name="proyectoId"></param>
        /// <param name="numeroReporte"></param>
        /// <returns></returns>
        public bool EdicionEspecialValidaNumeroReporteTT(int reporteTtId, int proyectoId, string numeroReporte)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ReporteTt.Where(x => x.NumeroReporte == numeroReporte
                                                && x.ProyectoID == proyectoId
                                                && x.ReporteTtID != reporteTtId).Any();
            }
        }

        /// <summary>
        /// Guarda los datos del Reporte TT que fue Editado
        /// </summary>
        /// <param name="reporte"></param>
        public void GuardarEdicionEspecialReporteTT(ReporteTt reporte)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (SamContext ctx = new SamContext())
                {
                    reporte.StartTracking();
                    ctx.ReporteTt.ApplyChanges(reporte);
                    ctx.SaveChanges();
                }
                scope.Complete();
            }
        }

        public void Borra(int reporteTtID)
        {
            using (SamContext ctx = new SamContext())
            {
                bool tieneMovimientos = ValidacionesReporteTt.TieneJuntaReporte(ctx, reporteTtID);

                if (!tieneMovimientos)
                {
                    ReporteTt reporteTt = ctx.ReporteTt.Where(x => x.ReporteTtID == reporteTtID).SingleOrDefault();
                    ctx.DeleteObject(reporteTt);
                    ctx.SaveChanges();
                }
                else
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_TieneJuntasReporte);
                }
            }
        }

        public ReporteTt ObtenerNumeroJuntasDetalleReporteTt(int reporteTtID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<ReporteTt> iqReporteTt = ctx.ReporteTt.Where(x => x.ReporteTtID == reporteTtID);
                IQueryable<JuntaReporteTt> iqJuntaReporteTt =
                    ctx.JuntaReporteTt.Where(x => iqReporteTt.Select(y => y.ReporteTtID).Contains(x.ReporteTtID));
                iqReporteTt.ToList();
                iqJuntaReporteTt.ToList();
                return iqReporteTt.FirstOrDefault();
            }
        }

        public List<TipoPrueba> ObtenerTipoPrueba()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.TipoPrueba.Where(x => x.Categoria == "TT").ToList();
            }
        }

        /// <summary>
        /// Obtenemos las juntas con requisicion a prueba TT
        /// </summary>
        /// <param name="ordenTrabajoID">Id de la orden de trabajo</param>
        /// <param name="ordenTrabajoSpoolID">ID de la ordentrabajospool</param>
        /// <returns></returns>
        public List<GrdRequisiciones> ObtenerJuntas(int ordenTrabajoID, int ordenTrabajoSpoolID, int tipoPruebaID)
        {
            List<GrdRequisiciones> lista = new List<GrdRequisiciones>();
            List<JuntaSpool> juntaSpool;
            List<OrdenTrabajoJunta> ordenTrabajoJunta;
            IQueryable<OrdenTrabajoSpool> ordenTrabajoSpool;
            List<OrdenTrabajoSpool> ordenTrabajoSpoolList;
            List<OrdenTrabajo> ordenTrabajo;
            IQueryable<JuntaWorkstatus> juntaWorkstatus;
            IQueryable<JuntaRequisicion> juntaRequisicion;
            List<JuntaWorkstatus> juntaWorkstatusList;
            List<JuntaRequisicion> juntaRequisicionList;
            List<Requisicion> requisicion;
            List<FamAceroCache> famAcero;
            List<SpoolHold> spoolHold;
            List<TipoJuntaCache> tipoJunta;
            List<Spool> spools;

            using (SamContext ctx = new SamContext())
            {
                ctx.JuntaWorkstatus.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.JuntaSpool.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.OrdenTrabajo.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.OrdenTrabajoJunta.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.OrdenTrabajoSpool.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.Requisicion.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.JuntaRequisicion.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.SpoolHold.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.Spool.MergeOption = System.Data.Objects.MergeOption.NoTracking;

                //Obtengo los IDs de las juntas que existen en la tabla JuntaWorkstatus que son parte de la orden de trabajo
                //y del numero de control (en caso de especificarse)
                //Donde la junta es JuntaFinal
                IQueryable<JuntaWorkstatus> juntas = ctx.JuntaWorkstatus.Where(x => x.OrdenTrabajoSpool.OrdenTrabajoID == ordenTrabajoID
                                                                      && (ordenTrabajoSpoolID == -1
                                                                      || x.OrdenTrabajoSpool.OrdenTrabajoSpoolID == ordenTrabajoSpoolID)
                                                                      && x.JuntaFinal);

                //Obtengo las requisiciones de las juntas que caen en los filtros
                IQueryable<int> juntasConRequisicion = (from jtaReq in ctx.JuntaRequisicion
                                                        join jtaWks in juntas on jtaReq.JuntaWorkstatusID equals jtaWks.JuntaWorkstatusID
                                                        join jtaRep in ctx.JuntaReporteTt on jtaReq.JuntaRequisicionID equals jtaRep.JuntaRequisicionID into Reportes
                                                        from rep in Reportes.DefaultIfEmpty()
                                                        where jtaReq.Requisicion.TipoPruebaID == tipoPruebaID && rep == null
                                                        select jtaWks.JuntaSpoolID);

                //Obtengo los registros de las juntas obtenidas en el paso anterior y que además son parte de un spool con liberacion dimensional
                //IQueryable<JuntaSpool> query = ctx.JuntaSpool.Where(x => juntasConRequisicion.Contains(x.JuntaSpoolID)).AsQueryable();

                juntaSpool = ctx.JuntaSpool.Where(x => juntasConRequisicion.Contains(x.JuntaSpoolID)).ToList();
                
                ordenTrabajoJunta = ctx.OrdenTrabajoJunta.Where(x => juntasConRequisicion.Contains(x.JuntaSpoolID)).ToList();
                ordenTrabajoSpool = ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajoID == ordenTrabajoID).AsQueryable();

                ordenTrabajo = ctx.OrdenTrabajo.Where(x => x.OrdenTrabajoID == ordenTrabajoID).ToList();
                juntaWorkstatus = ctx.JuntaWorkstatus.Where(x => juntasConRequisicion.Contains(x.JuntaSpoolID)).AsQueryable();

                juntaRequisicion = ctx.JuntaRequisicion.Where(x => juntaWorkstatus.Select(y => y.JuntaWorkstatusID).Contains(x.JuntaWorkstatusID) && !ctx.JuntaReporteTt.Select(y => y.JuntaRequisicionID).Contains(x.JuntaRequisicionID)).AsQueryable();
                requisicion = ctx.Requisicion.Where(x => juntaRequisicion.Select(y => y.RequisicionID).Contains(x.RequisicionID)).ToList();
                spools = ctx.Spool.Where(x => ordenTrabajoSpool.Select(y => y.SpoolID).Contains(x.SpoolID)).ToList();
                spoolHold = ctx.SpoolHold.Where(x => ordenTrabajoSpool.Select(y => y.SpoolID).Contains(x.SpoolID)).ToList();
                famAcero = CacheCatalogos.Instance.ObtenerFamiliasAcero();
                tipoJunta = CacheCatalogos.Instance.ObtenerTiposJunta();
                ordenTrabajoSpoolList = ordenTrabajoSpool.ToList();
                juntaWorkstatusList = juntaWorkstatus.ToList();
                juntaRequisicionList = juntaRequisicion.ToList();
            }

            lista = (from js in juntaSpool
                     join s in spools on js.SpoolID equals s.SpoolID
                     join otj in ordenTrabajoJunta on js.JuntaSpoolID equals otj.JuntaSpoolID
                     join ots in ordenTrabajoSpoolList on otj.OrdenTrabajoSpoolID equals ots.OrdenTrabajoSpoolID
                     join ot in ordenTrabajo on ots.OrdenTrabajoID equals ot.OrdenTrabajoID
                     join jw in juntaWorkstatusList on js.JuntaSpoolID equals jw.JuntaSpoolID
                     join jReq in juntaRequisicionList on jw.JuntaWorkstatusID equals jReq.JuntaWorkstatusID
                     join req in requisicion on jReq.RequisicionID equals req.RequisicionID
                     join sh in spoolHold on ots.SpoolID equals sh.SpoolID into Holds
                     from hld in Holds.DefaultIfEmpty()
                     where req.TipoPruebaID == tipoPruebaID
                     where jw.JuntaFinal
                     select new GrdRequisiciones
                     {
                         JuntaWorkstatusID = jw.JuntaWorkstatusID,
                         NombreSpool = s.Nombre,
                         JuntaSpoolID = js.JuntaSpoolID,
                         RequisicionID = req.RequisicionID,
                         FechaRequisicion = req.FechaRequisicion,
                         NumeroRequisicion = req.NumeroRequisicion,
                         OrdenTrabajo = ot.NumeroOrden,
                         NumeroControl = ots.NumeroControl,
                         EtiquetaJunta = jw.EtiquetaJunta,
                         EtiquetaMaterial1 = js.EtiquetaMaterial1,
                         EtiquetaMaterial2 = js.EtiquetaMaterial2,
                         TipoJuntaID = js.TipoJuntaID,
                         TipoJunta = tipoJunta.Where(x => x.ID == js.TipoJuntaID).Select(x => x.Nombre).FirstOrDefault(),
                         Cedula = js.Cedula,
                         FamiliaAceroMaterial1 = famAcero.Where(x => x.ID == js.FamiliaAceroMaterial1ID).Select(x => x.Nombre).FirstOrDefault(),
                         FamiliaAceroMaterial2 = famAcero.Where(x => x.ID == js.FamiliaAceroMaterial2ID).Select(x => x.Nombre).FirstOrDefault(),
                         Diametro = js.Diametro,
                         Hold = (hld == null) ? false : hld.TieneHoldIngenieria || hld.TieneHoldCalidad || hld.Confinado
                     }).ToList();


            return lista;

        }

        /// <summary>
        /// Obtenemos las juntas con requisicion a prueba TT
        /// </summary>
        /// <param name="proyectoID">Id del proyecto</param>
        /// <returns></returns>
        public List<GrdRequisiciones> ObtenerJuntas(int proyectoID, int tipoPruebaID)
        {
            List<GrdRequisiciones> lista = new List<GrdRequisiciones>();
            List<JuntaSpool> juntaSpool;
            List<OrdenTrabajoJunta> ordenTrabajoJunta;
            IQueryable<OrdenTrabajoSpool> ordenTrabajoSpool;
            List<OrdenTrabajoSpool> ordenTrabajoSpoolList;
            List<OrdenTrabajo> ordenTrabajo;
            IQueryable<JuntaWorkstatus> juntaWorkstatus;
            IQueryable<JuntaRequisicion> juntaRequisicion;
            List<JuntaWorkstatus> juntaWorkstatusList;
            List<JuntaRequisicion> juntaRequisicionList;
            List<Requisicion> requisicion;
            List<FamAceroCache> famAcero;
            List<SpoolHold> spoolHold;
            List<TipoJuntaCache> tipoJunta;
            List<Spool> spools;

            using (SamContext ctx = new SamContext())
            {
                ctx.JuntaWorkstatus.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.JuntaSpool.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.OrdenTrabajo.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.OrdenTrabajoJunta.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.OrdenTrabajoSpool.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.Requisicion.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.JuntaRequisicion.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.SpoolHold.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.Spool.MergeOption = System.Data.Objects.MergeOption.NoTracking;

                //Obtengo los IDs de las juntas que existen en la tabla JuntaWorkstatus que son parte de la orden de trabajo
                //y del numero de control (en caso de especificarse)
                //Donde la junta es JuntaFinal
                IQueryable<JuntaWorkstatus> juntas = ctx.JuntaWorkstatus.Where(x => x.OrdenTrabajoSpool.OrdenTrabajo.ProyectoID == proyectoID
                                                                                    && x.JuntaFinal);

                //Obtengo las requisiciones de las juntas que caen en los filtros
                IQueryable<int> juntasConRequisicion = (from jtaReq in ctx.JuntaRequisicion
                                                        join jtaWks in juntas on jtaReq.JuntaWorkstatusID equals jtaWks.JuntaWorkstatusID
                                                        join jtaRep in ctx.JuntaReporteTt on jtaReq.JuntaRequisicionID equals jtaRep.JuntaRequisicionID into Reportes
                                                        from rep in Reportes.DefaultIfEmpty()
                                                        where jtaReq.Requisicion.TipoPruebaID == tipoPruebaID && rep == null
                                                        select jtaWks.JuntaSpoolID);

                //Obtengo los registros de las juntas obtenidas en el paso anterior y que además son parte de un spool con liberacion dimensional
                //IQueryable<JuntaSpool> query = ctx.JuntaSpool.Where(x => juntasConRequisicion.Contains(x.JuntaSpoolID)).AsQueryable();

                juntaSpool = ctx.JuntaSpool.Where(x => juntasConRequisicion.Contains(x.JuntaSpoolID)).ToList();

                ordenTrabajoJunta = ctx.OrdenTrabajoJunta.Where(x => juntasConRequisicion.Contains(x.JuntaSpoolID)).ToList();
                ordenTrabajoSpool = ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajo.ProyectoID == proyectoID).AsQueryable();

                ordenTrabajo = ctx.OrdenTrabajo.Where(x => x.ProyectoID == proyectoID).ToList();
                juntaWorkstatus = ctx.JuntaWorkstatus.Where(x => juntasConRequisicion.Contains(x.JuntaSpoolID)).AsQueryable();

                juntaRequisicion = ctx.JuntaRequisicion.Where(x => juntaWorkstatus.Select(y => y.JuntaWorkstatusID).Contains(x.JuntaWorkstatusID) && !ctx.JuntaReporteTt.Select(y => y.JuntaRequisicionID).Contains(x.JuntaRequisicionID)).AsQueryable();
                requisicion = ctx.Requisicion.Where(x => juntaRequisicion.Select(y => y.RequisicionID).Contains(x.RequisicionID)).ToList();
                spools = ctx.Spool.Where(x => ordenTrabajoSpool.Select(y => y.SpoolID).Contains(x.SpoolID)).ToList();
                spoolHold = ctx.SpoolHold.Where(x => ordenTrabajoSpool.Select(y => y.SpoolID).Contains(x.SpoolID)).ToList();
                famAcero = CacheCatalogos.Instance.ObtenerFamiliasAcero();
                tipoJunta = CacheCatalogos.Instance.ObtenerTiposJunta();
                ordenTrabajoSpoolList = ordenTrabajoSpool.ToList();
                juntaWorkstatusList = juntaWorkstatus.ToList();
                juntaRequisicionList = juntaRequisicion.ToList();
            }

            lista = (from js in juntaSpool
                     join s in spools on js.SpoolID equals s.SpoolID
                     join otj in ordenTrabajoJunta on js.JuntaSpoolID equals otj.JuntaSpoolID
                     join ots in ordenTrabajoSpoolList on otj.OrdenTrabajoSpoolID equals ots.OrdenTrabajoSpoolID
                     join ot in ordenTrabajo on ots.OrdenTrabajoID equals ot.OrdenTrabajoID
                     join jw in juntaWorkstatusList on js.JuntaSpoolID equals jw.JuntaSpoolID
                     join jReq in juntaRequisicionList on jw.JuntaWorkstatusID equals jReq.JuntaWorkstatusID
                     join req in requisicion on jReq.RequisicionID equals req.RequisicionID
                     join sh in spoolHold on ots.SpoolID equals sh.SpoolID into Holds
                     from hld in Holds.DefaultIfEmpty()
                     where req.TipoPruebaID == tipoPruebaID
                     where jw.JuntaFinal
                     select new GrdRequisiciones
                     {
                         JuntaWorkstatusID = jw.JuntaWorkstatusID,
                         NombreSpool = s.Nombre,
                         JuntaSpoolID = js.JuntaSpoolID,
                         RequisicionID = req.RequisicionID,
                         FechaRequisicion = req.FechaRequisicion,
                         NumeroRequisicion = req.NumeroRequisicion,
                         OrdenTrabajo = ot.NumeroOrden,
                         NumeroControl = ots.NumeroControl,
                         EtiquetaJunta = jw.EtiquetaJunta,
                         EtiquetaMaterial1 = js.EtiquetaMaterial1,
                         EtiquetaMaterial2 = js.EtiquetaMaterial2,
                         TipoJuntaID = js.TipoJuntaID,
                         TipoJunta = tipoJunta.Where(x => x.ID == js.TipoJuntaID).Select(x => x.Nombre).FirstOrDefault(),
                         Cedula = js.Cedula,
                         FamiliaAceroMaterial1 = famAcero.Where(x => x.ID == js.FamiliaAceroMaterial1ID).Select(x => x.Nombre).FirstOrDefault(),
                         FamiliaAceroMaterial2 = famAcero.Where(x => x.ID == js.FamiliaAceroMaterial2ID).Select(x => x.Nombre).FirstOrDefault(),
                         Diametro = js.Diametro,
                         Hold = (hld == null) ? false : hld.TieneHoldIngenieria || hld.TieneHoldCalidad || hld.Confinado
                     }).ToList();


            return lista;

        }

        public void GuardaReporteTt(ReporteTt reporte, JuntaReporteTt juntaReporte, string IDs, string RIDs, Guid UserUID)
        {

            using (TransactionScope ts = new TransactionScope())
            {

                using (SamContext ctx = new SamContext())
                {

                    //Validar si el numero de reporte ya existe en la base de datos
                    ReporteTt reporteExistente = ctx.ReporteTt.Where(x => x.NumeroReporte == reporte.NumeroReporte && x.ProyectoID == reporte.ProyectoID).SingleOrDefault();
                    if (reporteExistente != null)
                    {
                        //Validando que las fechas concuerden
                        if (reporteExistente.FechaReporte != reporte.FechaReporte)
                        {
                            throw new ExcepcionReportes(MensajesError.Excepcion_ReporteExistenteConFechaDiferente);
                        }
                        else
                        {
                            reporte = reporteExistente;

                            reporte.StartTracking();
                            reporte.UsuarioModifica = UserUID;
                            reporte.FechaModificacion = DateTime.Now;
                        }
                    }
                    else
                    {
                        reporte.UsuarioModifica = UserUID;
                        reporte.FechaModificacion = DateTime.Now;
                    }

                    string[] juntasArreglo = IDs.Split(',');
                    string[] requisicionArreglo = RIDs.Split(',');
                    int contador = 0;

                    foreach (string juntaID in juntasArreglo)
                    {
                        int jID = juntaID.SafeIntParse();
                        int reqID = requisicionArreglo[contador].SafeIntParse();

                        //Verifico que no exista ya un detalle para este spool en especifico en el reporte
                        if (ctx.JuntaReporteTt.Where(x => x.ReporteTtID == reporte.ReporteTtID && x.JuntaWorkstatusID == jID).Any())
                        {
                            throw new ExcepcionDuplicados(string.Format(MensajesError.Excepcion_ReporteVisualConDetalleDuplicado));
                        }

                        int jtaReq = ctx.JuntaRequisicion.Where(x => x.RequisicionID == reqID && x.JuntaWorkstatusID == jID).Select(x => x.JuntaRequisicionID).SingleOrDefault();

                        JuntaReporteTt junta = new JuntaReporteTt
                        {
                            JuntaWorkstatusID = jID,
                            JuntaRequisicionID = jtaReq,
                            FechaTratamiento = juntaReporte.FechaTratamiento,
                            NumeroGrafica = juntaReporte.NumeroGrafica,
                            Aprobado = juntaReporte.Aprobado,
                            Observaciones = juntaReporte.Observaciones,
                            UsuarioModifica = UserUID,
                            FechaModificacion = DateTime.Now
                        };

                        JuntaWorkstatus juntaWks = ctx.JuntaWorkstatus.Where(x => x.JuntaWorkstatusID == jID).Single();

                        //Si el reporte es aprobado se actualiza el registro de JuntaWorkstatus
                        if (juntaReporte.Aprobado)
                        {
                            juntaWks.StartTracking();
                            juntaWks.UltimoProcesoID = (int)UltimoProcesoEnum.TT;
                            juntaWks.UsuarioModifica = UserUID;
                            juntaWks.FechaModificacion = DateTime.Now;
                            juntaWks.StopTracking();
                            ctx.JuntaWorkstatus.ApplyChanges(juntaWks);
                        }

                        reporte.JuntaReporteTt.Add(junta);

                        //Contador de las requisiciones
                        contador++;
                    }

                    if (reporteExistente != null)
                    {
                        reporte.StopTracking();
                    }

                    ctx.ReporteTt.ApplyChanges(reporte);
                    ctx.SaveChanges();

                    ctx.CalculaHojaParaReporte((int)TipoReporte.ReporteTT, reporte.ProyectoID, reporte.NumeroReporte, IDs);
                }

                ts.Complete();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reporteTtID"></param>
        /// <returns></returns>
        public int ObtenerProyectoID(int reporteTtID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ProyectoReporteTt(ctx, reporteTtID);
            }
        }


        /// <summary>
        /// Versión compilada del query para permisos de reportes tt
        /// </summary>
        private static readonly Func<SamContext, int, int> ProyectoReporteTt =
        CompiledQuery.Compile<SamContext, int, int>
        (
            (ctx, id) => ctx.ReporteTt
                            .Where(x => x.ReporteTtID == id)
                            .Select(x => x.ProyectoID)
                            .Single()
        );
    }
}
