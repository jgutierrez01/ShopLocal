using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using SAM.Entities.Grid;
using System.Transactions;
using SAM.BusinessObjects.Excepciones;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Validations;
using SAM.Entities.Cache;
using SAM.BusinessObjects.Utilerias;

namespace SAM.BusinessObjects.Workstatus
{
    public class InspeccionDimensionalBO
    {
        private static readonly object _mutex = new object();
        private static InspeccionDimensionalBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private InspeccionDimensionalBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase InspeccionDimensionalBO
        /// </summary>
        /// <returns></returns>
        public static InspeccionDimensionalBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new InspeccionDimensionalBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtiene el listado de los tipos de reporte dimensionales
        /// </summary>
        /// <returns></returns>
        public List<TipoReporteDimensional> ObtenerTiposReporteDimensional()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.TipoReporteDimensional.ToList();
            }
        }

        public List<int> ObtenerSpoolIds(int[] inspeccionDimensionalPatioIds)
        {
            using (SamContext ctx = new SamContext())
            {
                return (from id in ctx.InspeccionDimensionalPatio.Include("WorkstatusSpool.OrdenTrabajoSpool")
                        where inspeccionDimensionalPatioIds.Contains(id.InspeccionDimensionalPatioID)
                        select id.WorkstatusSpool.OrdenTrabajoSpool.SpoolID).ToList();
            }
        }

        /// <summary>
        /// Obtiene el listado de spools para la inspeccion dimensional
        /// </summary>
        /// <param name="ordenTrabajoID"></param>
        /// <param name="numeroControl"></param>
        /// <returns></returns>
        public List<GrdInspeccionDimensional> ObtenerSpools(int ordenTrabajoID, int tipoReporteID)
        {
            List<GrdInspeccionDimensional> lista = new List<GrdInspeccionDimensional>();
            List<JuntaWorkstatus> jwks = null;
            List<JuntaSpool> js = null;
            List<OrdenTrabajoJunta> otj = null;
            List<TipoJuntaCache> tp = CacheCatalogos.Instance.ObtenerTiposJunta();
            int fabArea = CacheCatalogos.Instance.ObtenerFabAreas().Where(x => x.Nombre == FabAreas.SHOP).Select(x => x.ID).SingleOrDefault();
            int jTH = tp.Where(y => y.Nombre == TipoJuntas.TH).Select(y => y.ID).SingleOrDefault();
            int jTW = tp.Where(y => y.Nombre == TipoJuntas.TW).Select(y => y.ID).SingleOrDefault();
           
            using (SamContext ctx = new SamContext())
            {
                
                //Obtengo los IDs de los spools que son parte de la orden de trabajo
                IQueryable<OrdenTrabajoSpool> query = ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajoID == ordenTrabajoID).AsQueryable();              

                IQueryable<int> spoolConInspeccionDimensionalPatio = ctx.InspeccionDimensionalPatio.Where(x => x.WorkstatusSpool.OrdenTrabajoSpool.OrdenTrabajoID == ordenTrabajoID).Select(x => x.WorkstatusSpoolID);
                IQueryable<int> spoolsOT = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpool.OrdenTrabajoID == ordenTrabajoID).Select(x => x.WorkstatusSpoolID);
                IQueryable<int> spoolsConDetalleReporte = ctx.ReporteDimensionalDetalle.Where(x => spoolsOT.Contains(x.WorkstatusSpoolID) && x.Aprobado).Select(x => x.WorkstatusSpoolID);              
             
                IQueryable<int> spoolsConLiberacionDimensional = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpool.OrdenTrabajoID == ordenTrabajoID && x.TieneLiberacionDimensional)
                    .Union(ctx.WorkstatusSpool.Where(x => spoolConInspeccionDimensionalPatio.Contains(x.WorkstatusSpoolID)))
                    .Union(ctx.WorkstatusSpool.Where(x => spoolsConDetalleReporte.Contains(x.WorkstatusSpoolID)))
                    .Select(x => x.OrdenTrabajoSpoolID);

                IQueryable<int> spoolsConReporteEsperoresAprobado = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpool.OrdenTrabajoID == ordenTrabajoID
                                                                                                   && x.ReporteDimensionalDetalle.Where(y => y.ReporteDimensional.TipoReporteDimensionalID == (int)TipoReporteDimensionalEnum.Espesores
                                                                                                                                             && y.Aprobado)
                                                                                                                                 .Select(y => y.Aprobado).FirstOrDefault())
                                                                                       .Select(x => x.OrdenTrabajoSpoolID);

                //Si el tipo de reporte es dimensional se verifica que los spools no tengan aprobado este tipo de reporte
                if (tipoReporteID == (int)TipoReporteDimensionalEnum.Dimensional)
                {
                    query = query.Where(x => !spoolsConLiberacionDimensional.Contains(x.OrdenTrabajoSpoolID));
                }
                else if (tipoReporteID == (int)TipoReporteDimensionalEnum.Espesores)
                {
                    query = query.Where(x => !spoolsConReporteEsperoresAprobado.Contains(x.OrdenTrabajoSpoolID));
                }



                lista = (from ots in query
                         join s in ctx.Spool on ots.SpoolID equals s.SpoolID
                         join ws in ctx.WorkstatusSpool on ots.OrdenTrabajoSpoolID equals ws.OrdenTrabajoSpoolID into WksSpool
                         from wk in WksSpool.DefaultIfEmpty()
                         join sh in ctx.SpoolHold on ots.SpoolID equals sh.SpoolID into Holds
                         from hld in Holds.DefaultIfEmpty()
                         select new GrdInspeccionDimensional
                         {
                             WorkstatusSpoolID = (wk == null) ? -1 : wk.WorkstatusSpoolID,
                             SpoolID = s.SpoolID,
                             NumeroControl = ots.NumeroControl,
                             OrdenTrabajo = ots.OrdenTrabajo.NumeroOrden,
                             OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID,
                             NombreSpool = s.Nombre,
                             Area = s.Area.Value,
                             PDI = s.Pdis.Value,
                             Peso = s.Peso.Value,                             
                             Hold = (hld == null) ? false : hld.TieneHoldIngenieria || hld.TieneHoldCalidad || hld.Confinado
                         }).ToList();
                
                IQueryable<int> listaODTS = lista.Select(x => x.OrdenTrabajoSpoolID).AsQueryable();

                ctx.JuntaWorkstatus.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.JuntaSpool.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                
                jwks = ctx.JuntaWorkstatus.Where(x => listaODTS.Contains(x.OrdenTrabajoSpoolID) && x.JuntaFinal).ToList();
                IQueryable<OrdenTrabajoJunta> ordenTrabajoJunta = ctx.OrdenTrabajoJunta.Where(y => listaODTS.Contains(y.OrdenTrabajoSpoolID)).AsQueryable();
                js = ctx.JuntaSpool.Where(x => ordenTrabajoJunta.Select(y => y.JuntaSpoolID).Contains(x.JuntaSpoolID)).ToList();
                otj = ordenTrabajoJunta.ToList();
            } 
                lista.ForEach(x => calculaEstatus(x, jwks, js, otj, fabArea, jTH, jTW));

                return lista;
        }

        /// <summary>
        /// Calcula el estatus de cada spool en base al estatus de todas sus juntas.
        /// </summary>
        /// <param name="grd"></param>
        /// <param name="juntaWks"></param>
        /// <param name="juntaSpool"></param>
        /// <param name="otj"></param>
        /// <param name="fabAreaID"></param>
        /// <param name="jTH"></param>
        /// <param name="jTW"></param>
        private void calculaEstatus(GrdInspeccionDimensional grd, List<JuntaWorkstatus> juntaWks, List<JuntaSpool> juntaSpool, List<OrdenTrabajoJunta> otj, int fabAreaID, int jTH, int jTW)
        {
            IEnumerable<JuntaWorkstatus> jwks = juntaWks.Where(y => y.OrdenTrabajoSpoolID == grd.OrdenTrabajoSpoolID);
            IEnumerable<OrdenTrabajoJunta> otjQuery = otj.Where(x => x.OrdenTrabajoSpoolID == grd.OrdenTrabajoSpoolID);
            IEnumerable<JuntaSpool> js = juntaSpool.Where(x => otjQuery.Select(y => y.JuntaSpoolID).Contains(x.JuntaSpoolID));
            int numJuntas = js.Where(x => x.FabAreaID == fabAreaID).Count();
            int numSoldaduras = js.Where(x => x.FabAreaID == fabAreaID && x.TipoJuntaID != jTH && x.TipoJuntaID != jTW).Count();
            grd.Armado = jwks.Where(x => x.ArmadoAprobado).Count() == numJuntas;
            grd.Soldadura = jwks.Where(x => x.SoldaduraAprobada
                                            && js.Where(y => y.JuntaSpoolID == x.JuntaSpoolID).Select(y => y.TipoJuntaID).FirstOrDefault() != jTH
                                            && js.Where(y => y.JuntaSpoolID == x.JuntaSpoolID).Select(y => y.TipoJuntaID).FirstOrDefault() != jTW).Count()
                                        == numSoldaduras;
            grd.InspeccionVisual = jwks.Where(x => x.InspeccionVisualAprobada).Count() == numJuntas;
        }

        /// <summary>
        /// Genera el reporte de inspeccion dimensional
        /// </summary>
        /// <param name="juntaInspVisual"></param>
        /// <param name="inspVisual"></param>
        /// <param name="defectos"></param>
        /// <param name="juntas"></param>
        /// <param name="UserUID"></param>
        public void GeneraReporte(ReporteDimensionalDetalle detalle, ReporteDimensional reporte, string spools, Guid UserUID)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                string spoolsReqPinturaFechaMenor = string.Empty;

                using (SamContext ctx = new SamContext())
                {
                    //Validar si el numero de reporte ya existe en la base de datos
                    ReporteDimensional reporteExistente = ctx.ReporteDimensional.Where(x => x.NumeroReporte == reporte.NumeroReporte && x.ProyectoID == reporte.ProyectoID && x.TipoReporteDimensionalID == reporte.TipoReporteDimensionalID).SingleOrDefault();
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

                    string[] spoolsArreglo = spools.Split(',');
                  
                    foreach (string spoolID in spoolsArreglo)
                    {
                        int sID = spoolID.SafeIntParse();

                        List<int> juntasSpool = ctx.JuntaSpool.Where(x => x.SpoolID == sID).Select(x => x.JuntaSpoolID).ToList();

                        List<JuntaWorkstatus> juntasWks = ctx.JuntaWorkstatus.Where(x => juntasSpool.Contains(x.JuntaSpoolID) && x.VersionJunta > 1).ToList();
                        
                        Spool spool = ctx.Spool.Where(x => x.SpoolID == sID).FirstOrDefault();

                        if(juntasWks.Count > 0)
                        {
                            foreach (JuntaWorkstatus js in juntasWks)
                            {
                                if(js.Soldadura != null)
                                {
                                    if (js.Soldadura.FechaSoldadura > reporte.FechaReporte)
                                    {
                                        throw new ExcepcionFechas(string.Format(MensajesError.Excepcion_FechaPruebaMenorFechaSoldadura, js.Soldadura.FechaSoldadura.ToShortDateString(), js.EtiquetaJunta, spool.Nombre));     
                                    }
                                }
                            }
                        }

                        bool dimensionalAprobado = detalle.Aprobado && reporte.TipoReporteDimensionalID == (int)TipoReporteDimensionalEnum.Dimensional;

                        //Verificamos si el registro de workstatus spool existe, si no se crea.
                        OrdenTrabajoSpool ots = ctx.OrdenTrabajoSpool.Where(x => x.SpoolID == sID).Single();
                        WorkstatusSpool spoolWks = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID == ots.OrdenTrabajoSpoolID).SingleOrDefault();

                        if (spoolWks == null)
                        {
                            spoolWks = new WorkstatusSpool()
                            {
                                OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID,
                                UltimoProcesoID = (int)UltimoProcesoEnum.InspeccionDimensional,
                                TieneLiberacionDimensional = dimensionalAprobado,
                                UsuarioModifica = UserUID,
                                FechaModificacion = DateTime.Now
                            };
                            ctx.WorkstatusSpool.ApplyChanges(spoolWks);
                        }

                        //Verifico que no tenga InspeccionDimensionalPatio
                        InspeccionDimensionalPatio idp = ctx.InspeccionDimensionalPatio.Where(x => x.WorkstatusSpoolID == spoolWks.WorkstatusSpoolID).FirstOrDefault();
                        if(idp != null)
                        {
                            throw new ExcepcionDuplicados(string.Format(MensajesError.Excepcion_TieneInspeccionDimensionalHH));                        
                        }

                        //Verifico que no exista ya un detalle para este spool en algun reporte
                        List<ReporteDimensionalDetalle> detallesReporte = ctx.ReporteDimensionalDetalle.Where(x => x.WorkstatusSpoolID == spoolWks.WorkstatusSpoolID).OrderByDescending(x => x.ReporteDimensionalDetalleID).ToList();
                        ReporteDimensionalDetalle detalleReporte = null;

                        if (detallesReporte != null)
                        {
                            detalleReporte = detallesReporte.Where(x => x.Aprobado).SingleOrDefault();
                        }

                        if (detalleReporte != null)
                        {   
                            string numeroReporte = ctx.ReporteDimensional.Where(x => x.ReporteDimensionalID == detalleReporte.ReporteDimensionalID).FirstOrDefault().NumeroReporte;

                            string estatus = MensajesError.EstatusReporteRechazado;                                
                                                     
                            if (detalleReporte.Aprobado)
                            {                                   
                                throw new ExcepcionDuplicados(string.Format(MensajesError.Excepcion_SpoolConReporte, spool.Nombre, MensajesError.EstatusReporteAprobado, numeroReporte));
                            } 
                        }

                        if (detalleReporte == null)
                        {
                            detalleReporte = new ReporteDimensionalDetalle();
                        }

                        if (dimensionalAprobado)
                        {                          
                            spoolWks.StartTracking();
                            spoolWks.TieneLiberacionDimensional = true;
                            spoolWks.UltimoProcesoID = (int)UltimoProcesoEnum.InspeccionDimensional;
                            spoolWks.UsuarioModifica = UserUID;
                            spoolWks.FechaModificacion = DateTime.Now;
                            spoolWks.StopTracking();
                            ctx.WorkstatusSpool.ApplyChanges(spoolWks);
                        } 

                        detalleReporte.WorkstatusSpool = spoolWks;
                        detalleReporte.FechaLiberacion = detalle.FechaLiberacion;
                        detalleReporte.Aprobado = detalle.Aprobado;
                        detalleReporte.Observaciones = detalle.Observaciones;                      
                        detalleReporte.UsuarioModifica = UserUID;
                        detalleReporte.FechaModificacion = DateTime.Now;

                        InspeccionDimensionalBO.Instance.ValidaFechasInspeccionFechaRequiPintura(detalleReporte.FechaLiberacion, spoolWks.OrdenTrabajoSpoolID);
                    
                        reporte.ReporteDimensionalDetalle.Add(detalleReporte);     
                        detalleReporte.TallerID = detalle.TallerID;
                        detalleReporte.InspectorID = detalle.InspectorID;
                       
                        //if (reporte.ReporteDimensionalDetalle.Contains(detalleReporte))
                        //{
                        //    ctx.ReporteDimensionalDetalle.ApplyChanges(detalleReporte);
                        //}
                        //else
                        //{
                        //    reporte.ReporteDimensionalDetalle.Add(detalleReporte);
                        //}
                    }
                    
                    if (reporteExistente != null)
                    {
                        reporte.StopTracking();
                    }
                    
                    ctx.ReporteDimensional.ApplyChanges(reporte);
                    ctx.SaveChanges();

                    //Obtengo todos los workstatus spool que acabo de generar o que sufrieron modificacion
                    List<int> ids = (from wks in ctx.WorkstatusSpool
                              join repDet in ctx.ReporteDimensionalDetalle on wks.WorkstatusSpoolID equals repDet.WorkstatusSpoolID
                              join rep in ctx.ReporteDimensional on repDet.ReporteDimensionalID equals rep.ReporteDimensionalID
                              where rep.NumeroReporte == reporte.NumeroReporte && rep.ProyectoID == reporte.ProyectoID && rep.TipoReporteDimensionalID == reporte.TipoReporteDimensionalID                             
                              select wks.WorkstatusSpoolID).ToList();

                    string wksIds = string.Empty;
                    foreach (int id in ids)
                    {
                        wksIds += "," + id;
                    }

                    wksIds = wksIds.Remove(0, 1);

                    ctx.CalculaHojaParaReporte((int)TipoReporte.ReporteDimensional, reporte.ProyectoID, reporte.NumeroReporte, wksIds);

                }

                ts.Complete();
               
            }
        }

        //public bool TieneLiberacionDimensional()
        //{

        //}

        /// <summary>
        /// Obtiene la lista de juntas para su inspeccion dimensional
        /// </summary>
        /// <param name="spoolID"></param>
        /// <param name="FabAreaID"></param>
        /// <returns></returns>
        public bool AccederAInspeccionDimensional(int spoolID, int FabAreaID, int tipoJuntaTHID, int tipoJuntaTWID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<int> eqtp = (new List<int>() { tipoJuntaTHID, tipoJuntaTWID }).AsQueryable();

                //Otbengo las juntas
                List<JuntaSpool> juntasSpool = ctx.JuntaSpool.Where(x => x.SpoolID == spoolID && x.FabAreaID == FabAreaID).ToList();

                //Obtengo los ids de las juntas
                List<int> listJuntaSpoolID = juntasSpool.Select(y => y.JuntaSpoolID).ToList();

                //Obtengo las juntas con soldadura aprobada
                List<JuntaWorkstatus> listJuntaWorkstatus = ctx.JuntaWorkstatus.Where(x => listJuntaSpoolID.Contains(x.JuntaSpoolID) && x.JuntaFinal).ToList();

                //obtengo los datos de las juntas que ya fueron armadas, para conocer su estado de soldadura
                int juntasSoldadas = listJuntaWorkstatus.Where(x => x.SoldaduraAprobada && !eqtp.Contains(x.JuntaSpool.TipoJuntaID)).Count();
                int juntasNoRequierenSoldadura = listJuntaWorkstatus.Where(x => !x.SoldaduraAprobada && eqtp.Contains(x.JuntaSpool.TipoJuntaID)).Count();

                // la suma de juntas soldadas + juntas que no requieren soldadura, debe ser igual al total de juntas
                if (listJuntaSpoolID.Count > juntasSoldadas + juntasNoRequierenSoldadura)
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Obtiene el Workstatus Spool a traves de un Spool ID
        /// </summary>
        /// <param name="spoolID"></param>
        /// <returns></returns>
        public WorkstatusSpool ObtenerWorkStatusSpoolPorSpoolID(int spoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpool.SpoolID == spoolID).SingleOrDefault();
            }
        }

        public bool TieneLiberacionDimensional( WorkstatusSpool wks)
        {
            using(SamContext ctx = new SamContext())
            {
                // Solo si la liberacion Dimensional tiene estatus de  aprobada existe
                ReporteDimensionalDetalle rdd = ctx.ReporteDimensionalDetalle.Where(x => x.WorkstatusSpoolID == wks.WorkstatusSpoolID).OrderByDescending(x => x.ReporteDimensionalDetalleID).FirstOrDefault();
                if (rdd != null)
                {
                    if(rdd.Aprobado)
                    {
                        return true;
                    }                   
                }
               
                return false;
            }
        }

       
        public InspeccionDimensionalPatio TieneInspeccionDimensionalPatio(WorkstatusSpool wks)
        {
            using (SamContext ctx = new SamContext())
            {
                InspeccionDimensionalPatio idp = ctx.InspeccionDimensionalPatio.Where(x => x.WorkstatusSpoolID == wks.WorkstatusSpoolID).FirstOrDefault();
                
                return idp;               
            }
        }

        public void GuardaInspeccionDimensional(WorkstatusSpool wss, InspeccionDimensionalPatio idp)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SamContext ctx = new SamContext())
                    {
                        
                        //Guardar Workstatus Spool
                        wss.StartTracking();
                        wss.InspeccionDimensionalPatio.Add(idp);
                        ctx.WorkstatusSpool.ApplyChanges(wss);
                        ctx.SaveChanges();

                        //Guardar Inspeccion Dimensional
                        idp.WorkstatusSpoolID = wss.WorkstatusSpoolID;
                        ctx.InspeccionDimensionalPatio.ApplyChanges(idp);
                    
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
        public void ValidaFechasInspeccionFechaRequiPintura(DateTime? fechaProcesoActual, int otsId)
        {

            using (SamContext ctx = new SamContext())
            {
                string procesos = string.Empty;

                if (fechaProcesoActual.HasValue)
                {
                    PinturaSpool ps = (from p in ctx.PinturaSpool
                                       join wks in ctx.WorkstatusSpool on p.WorkstatusSpoolID equals wks.WorkstatusSpoolID into workStatus
                                       from t1 in workStatus.DefaultIfEmpty()
                                       where t1.OrdenTrabajoSpoolID == otsId
                                       select p).FirstOrDefault();
                    if (ps != null)
                    {
                        if (ps.FechaSandblast < fechaProcesoActual)
                        {
                            procesos += "Sandblast: " + ps.FechaSandblast.Value.ToShortDateString() + ", ";
                        }

                        if (ps.FechaPrimarios < fechaProcesoActual)
                        {
                            procesos += "Primarios: " + ps.FechaPrimarios.Value.ToShortDateString() + ", ";
                        }

                        if (ps.FechaIntermedios < fechaProcesoActual)
                        {
                            procesos += "Intermedios: " + ps.FechaIntermedios.Value.ToShortDateString() + ", ";
                        }

                        if (ps.FechaAdherencia < fechaProcesoActual)
                        {
                            procesos += "Adherencia: " + ps.FechaAdherencia.Value.ToShortDateString() + ", ";
                        }

                        if (ps.FechaAcabadoVisual < fechaProcesoActual)
                        {
                            procesos += "AcabadoVisual: " + ps.FechaAcabadoVisual.Value.ToShortDateString() + ", ";
                        }

                        if (ps.FechaPullOff < fechaProcesoActual)
                        {
                            procesos += "PullOff: " + ps.FechaPullOff.Value.ToShortDateString() + ". ";
                        }

                    }

                    if (!string.IsNullOrEmpty(procesos))
                    {
                        throw new Excepciones.ExcepcionSoldadura(string.Format(MensajesError.Excepcion_FechaMayorReportePintura, procesos));
                    }
                }
            }
        }

    }
}
