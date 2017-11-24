using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Excepciones;
using System.Globalization;
using SAM.BusinessObjects.Produccion;
using SAM.Entities.Personalizadas;

namespace SAM.BusinessObjects.Workstatus
{
    public class PinturaBO
    {
        private static readonly object _mutex = new object();
        private static PinturaBO _instance;

        /// <summary>
        /// constructor para implementar el patrón Singleton.
        /// </summary>
        private PinturaBO()
        {
        }

        /// <summary>
        /// permite la creación de una instancia de la clase
        /// </summary>
        public static PinturaBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new PinturaBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtiene el listado de spools con requisicion de pintura
        /// </summary>
        /// <param name="proyectoId">ID del proyecto</param>
        /// <param name="ordenTrabajoID">ID de la orden de trabajo</param>
        /// <param name="requisicionID">ID requisicion</param>
        /// <returns>Listado de Spools</returns>
        public List<GrdPintura>ObtenerListadoPintura(int proyectoId, int ordenTrabajoID, int requisicionID)
        {
            List<GrdPintura> reqP = null;
            DateTime? date = null;

            using (SamContext ctx = new SamContext())
            {
                reqP =
                       (from  ots in ctx.OrdenTrabajoSpool
                        join s in ctx.Spool on ots.SpoolID equals s.SpoolID
                        join ot in ctx.OrdenTrabajo on ots.OrdenTrabajoID equals ot.OrdenTrabajoID      
                        join wks in ctx.WorkstatusSpool on ots.OrdenTrabajoSpoolID equals wks.OrdenTrabajoSpoolID into wksDef 
                        from t1 in wksDef.DefaultIfEmpty() 
                        join pnt in ctx.PinturaSpool on t1.WorkstatusSpoolID equals pnt.WorkstatusSpoolID into Pintura
                        from pintura in Pintura.DefaultIfEmpty()
                        join req in ctx.RequisicionPinturaDetalle on t1.WorkstatusSpoolID equals req.WorkstatusSpoolID into rpdDef
                        from t2 in rpdDef.DefaultIfEmpty()
                        where s.ProyectoID == proyectoId && (ordenTrabajoID == -1 || ot.OrdenTrabajoID == ordenTrabajoID)
                                && (requisicionID == -1 || t2.RequisicionPinturaID == requisicionID)        
                                && s.SistemaPintura != string.Empty
                        let sph = s.SpoolHold
                        select new GrdPintura
                        {
                            RequisicionPinturaDetalleID = t2.RequisicionPinturaDetalleID,
                            OrdenTrabajoID = ots.OrdenTrabajoID,
                            OrdenTrabajo = ot.NumeroOrden,
                            OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID,
                            WorkstatusSpoolID = t1.WorkstatusSpoolID,
                            NombreSpool = s.Nombre,
                            NumeroControl = ots.NumeroControl,
                            Sistema = s.SistemaPintura,
                            Color = s.ColorPintura,
                            Codigo = s.CodigoPintura,
                            FechaSandBlast = (pintura == null) ? date : pintura.FechaSandblast.Value,
                            ReporteSandBlast = (pintura == null) ? string.Empty : pintura.ReporteSandblast,
                            FechaPrimario = (pintura == null) ? date : pintura.FechaPrimarios.Value,
                            ReportePrimario = (pintura == null) ? string.Empty : pintura.ReportePrimarios,
                            FechaIntermedio = (pintura == null) ? date : pintura.FechaIntermedios.Value,
                            ReporteIntermedio = (pintura == null) ? string.Empty : pintura.ReporteIntermedios,
                            FechaAcabadoVisual = (pintura == null) ? date : pintura.FechaAcabadoVisual.Value,
                            ReporteAcabadoVisual = (pintura == null) ? string.Empty : pintura.ReporteAcabadoVisual,
                            FechaAdherencia = (pintura == null) ? date : pintura.FechaAdherencia.Value,
                            ReporteAdherencia = (pintura == null) ? string.Empty : pintura.ReporteAdherencia,
                            FechaPullOff = (pintura == null) ? date : pintura.FechaPullOff.Value,
                            ReportePullOff = (pintura == null) ? string.Empty : pintura.ReportePullOff,
                            Liberado = t1.LiberadoPintura,
                            Hold = sph != null ? ((sph.TieneHoldCalidad) || (sph.TieneHoldIngenieria) || (sph.Confinado)) : false,
                        }).ToList();        
            }

            return reqP;
        }

        /// <summary>
        /// Guarda el detalle de pintura spool para los workstatusid recibidos
        /// </summary>
        /// <param name="spool">PinturaSpool a guardar</param>
        /// <param name="workstatusSpoolIDs">IDs separados por coma</param>
        /// <param name="rIDs">IDs de la requisicion separados por coma</param>
        /// <param name="liberado"></param>
        /// <param name="userID"></param>
        public void GuardaPinturaSpool(PinturaSpool spool, string otsIDs, string rIDs, bool liberado, string fechas, Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                string[] ordentrabajospoolIDs = otsIDs.Split(',');
                string[] reqs = rIDs.Split(',');
                string[] fechasReq = fechas.Split(',');
                int contador = 0;
                string spoolConProcesoFechaInvalida = string.Empty;

                foreach (string id in ordentrabajospoolIDs)
                {
                    int otsID = id.SafeIntParse();
                    WorkstatusSpool wksSpool = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID == otsID).FirstOrDefault();
                    PinturaSpool pinturaSpool = null;

                    if (wksSpool == null)
                    {  
                        wksSpool = new WorkstatusSpool()
                        {
                            OrdenTrabajoSpoolID = otsID,
                            UsuarioModifica = userID, 
                            FechaModificacion = DateTime.Now    
                        };
                         
                        ctx.WorkstatusSpool.ApplyChanges(wksSpool);
                        ctx.SaveChanges();
                    }

                    pinturaSpool = ctx.PinturaSpool.Where(x => x.WorkstatusSpoolID == wksSpool.WorkstatusSpoolID).SingleOrDefault();    

                    if (pinturaSpool != null)
                    {
                        pinturaSpool.StartTracking();                           
                    }
                    else
                    {
                        pinturaSpool = new PinturaSpool();
                        pinturaSpool.WorkstatusSpoolID = wksSpool.WorkstatusSpoolID;
                        pinturaSpool.ProyectoID = spool.ProyectoID;
                        pinturaSpool.RequisicionPinturaDetalleID = reqs[contador].SafeIntParse();
                    }                   
                        
                    pinturaSpool.UsuarioModifica = userID;
                    pinturaSpool.FechaModificacion = DateTime.Now; 
                  
                    if (!string.IsNullOrEmpty(fechasReq[contador]))
                    {
                        if (ValidaFecha(spool, fechasReq[contador]))
                        {
                            spoolConProcesoFechaInvalida +=  (from s in ctx.Spool
                                                              join ots in ctx.OrdenTrabajoSpool on s.SpoolID equals ots.SpoolID
                                                              join wks in ctx.WorkstatusSpool on ots.OrdenTrabajoSpoolID equals wks.OrdenTrabajoSpoolID
                                                              where (wks.WorkstatusSpoolID == pinturaSpool.WorkstatusSpoolID)
                                                              select s.Nombre ).FirstOrDefault();
                            spoolConProcesoFechaInvalida += ", ";
                        }
                        else
                        {
                            pinturaSpool.ReporteSandblast = !string.IsNullOrWhiteSpace(spool.ReporteSandblast) ? spool.ReporteSandblast : pinturaSpool.ReporteSandblast;
                            pinturaSpool.ReportePrimarios = !string.IsNullOrWhiteSpace(spool.ReportePrimarios) ? spool.ReportePrimarios : pinturaSpool.ReportePrimarios;
                            pinturaSpool.ReporteIntermedios = !string.IsNullOrWhiteSpace(spool.ReporteIntermedios) ? spool.ReporteIntermedios : pinturaSpool.ReporteIntermedios;
                            pinturaSpool.ReporteAdherencia = !string.IsNullOrWhiteSpace(spool.ReporteAdherencia) ? spool.ReporteAdherencia : pinturaSpool.ReporteAdherencia;
                            pinturaSpool.ReporteAcabadoVisual = !string.IsNullOrWhiteSpace(spool.ReporteAcabadoVisual) ? spool.ReporteAcabadoVisual : pinturaSpool.ReporteAcabadoVisual;
                            pinturaSpool.ReportePullOff = !string.IsNullOrWhiteSpace(spool.ReportePullOff) ? spool.ReportePullOff : pinturaSpool.ReportePullOff;
             
                            if (spool.FechaSandblast.HasValue)
                            {
                                pinturaSpool.FechaSandblast = spool.FechaSandblast.Value;
                            }

                            if (spool.FechaPrimarios.HasValue)
                            {
                                pinturaSpool.FechaPrimarios = spool.FechaPrimarios.Value;
                            }
                            if (spool.FechaIntermedios.HasValue)
                            {
                                pinturaSpool.FechaIntermedios = spool.FechaIntermedios.Value;
                            }
                            if (spool.FechaAdherencia.HasValue)
                            {
                                pinturaSpool.FechaAdherencia = spool.FechaAdherencia.Value;
                            }
                            if (spool.FechaAcabadoVisual.HasValue)
                            {
                                pinturaSpool.FechaAcabadoVisual = spool.FechaAcabadoVisual.Value;
                            }
                            if (spool.FechaPullOff.HasValue)
                            {
                                pinturaSpool.FechaPullOff = spool.FechaPullOff.Value;
                            }  

                            wksSpool.StartTracking();
                            wksSpool.TienePintura = true;
                            wksSpool.UltimoProcesoID = (int)UltimoProcesoEnum.Pintura;
                            wksSpool.LiberadoPintura = liberado;
                            wksSpool.StopTracking();
                            ctx.WorkstatusSpool.ApplyChanges(wksSpool);
                            ctx.PinturaSpool.ApplyChanges(pinturaSpool);
                        }
                    }
                    else
                    {
                        pinturaSpool.ReporteSandblast = !string.IsNullOrWhiteSpace(spool.ReporteSandblast) ? spool.ReporteSandblast : pinturaSpool.ReporteSandblast;
                        pinturaSpool.ReportePrimarios = !string.IsNullOrWhiteSpace(spool.ReportePrimarios) ? spool.ReportePrimarios : pinturaSpool.ReportePrimarios;
                        pinturaSpool.ReporteIntermedios = !string.IsNullOrWhiteSpace(spool.ReporteIntermedios) ? spool.ReporteIntermedios : pinturaSpool.ReporteIntermedios;
                        pinturaSpool.ReporteAdherencia = !string.IsNullOrWhiteSpace(spool.ReporteAdherencia) ? spool.ReporteAdherencia : pinturaSpool.ReporteAdherencia;
                        pinturaSpool.ReporteAcabadoVisual = !string.IsNullOrWhiteSpace(spool.ReporteAcabadoVisual) ? spool.ReporteAcabadoVisual : pinturaSpool.ReporteAcabadoVisual;
                        pinturaSpool.ReportePullOff = !string.IsNullOrWhiteSpace(spool.ReportePullOff) ? spool.ReportePullOff : pinturaSpool.ReportePullOff;
             
                        if (spool.FechaSandblast.HasValue)
                        {
                            pinturaSpool.FechaSandblast = spool.FechaSandblast.Value;
                        }

                        if (spool.FechaPrimarios.HasValue)
                        {
                            pinturaSpool.FechaPrimarios = spool.FechaPrimarios.Value;
                        }
                        if (spool.FechaIntermedios.HasValue)
                        {
                            pinturaSpool.FechaIntermedios = spool.FechaIntermedios.Value;
                        }
                        if (spool.FechaAdherencia.HasValue)
                        {
                            pinturaSpool.FechaAdherencia = spool.FechaAdherencia.Value;
                        }
                        if (spool.FechaAcabadoVisual.HasValue)
                        {
                            pinturaSpool.FechaAcabadoVisual = spool.FechaAcabadoVisual.Value;
                        }
                        if (spool.FechaPullOff.HasValue)
                        {
                            pinturaSpool.FechaPullOff = spool.FechaPullOff.Value;
                        }  

                        wksSpool.StartTracking();
                        wksSpool.TienePintura = true;
                        wksSpool.UltimoProcesoID = (int)UltimoProcesoEnum.Pintura;
                        wksSpool.LiberadoPintura = liberado;
                        wksSpool.StopTracking();
                        ctx.WorkstatusSpool.ApplyChanges(wksSpool);
                        ctx.PinturaSpool.ApplyChanges(pinturaSpool);
                    }

                    contador++;                       
                }

                ctx.SaveChanges();

                if (!string.IsNullOrEmpty(spoolConProcesoFechaInvalida))
                {
                    throw new ExcepcionRelaciones(string.Format(MensajesError.Exception_FechaProcesosPintura, spoolConProcesoFechaInvalida));
                }
            }                                        
        }

        private List<string> ValidaNumeroReporte(PinturaSpool spool)
        {
            List<string> errores = new List<string>();
           
            int reportesCapturados = 0;

            if (spool.FechaSandblast.HasValue)
            {
                reportesCapturados++;

                if (string.IsNullOrEmpty(spool.ReporteSandblast))
                {
                    errores.Add(string.Format(MensajesError.Excepcion_NumeroReporteVacio, GetTipoReporte(1)));
                }
            }

            if (spool.FechaPrimarios.HasValue)
            {
                reportesCapturados++;

                if (string.IsNullOrEmpty(spool.ReportePrimarios))
                {                  
                    errores.Add(string.Format(MensajesError.Excepcion_NumeroReporteVacio, GetTipoReporte(2)));
                }
            }

            if (spool.FechaIntermedios.HasValue)
            {
                reportesCapturados++;

                if (string.IsNullOrEmpty(spool.ReporteIntermedios))
                {
                    errores.Add(string.Format(MensajesError.Excepcion_NumeroReporteVacio, GetTipoReporte(3)));
                }          
            }

            if (spool.FechaAdherencia.HasValue)
            {
                reportesCapturados++;

                if (string.IsNullOrEmpty(spool.ReporteAdherencia))
                {                  
                    errores.Add(string.Format(MensajesError.Excepcion_NumeroReporteVacio, GetTipoReporte(4)));
                }
            }

            if (spool.FechaAcabadoVisual.HasValue) 
            {
                reportesCapturados++;

                if (string.IsNullOrEmpty(spool.ReporteAcabadoVisual))
                {                   
                    errores.Add(string.Format(MensajesError.Excepcion_NumeroReporteVacio, GetTipoReporte(5)));
                }
            }
       
            if (spool.FechaPullOff.HasValue)
            {
                reportesCapturados++;

                if (string.IsNullOrEmpty(spool.ReportePullOff))  
                {                    
                    errores.Add(string.Format(MensajesError.Excepcion_NumeroReporteVacio, GetTipoReporte(6)));
                }
            }

            if (reportesCapturados == 0)
            {
                errores.Add(MensajesError.Excepcion_NingunReportePintura);
            }

            return errores;
        }


        private string GetTipoReporte(int reporteId)
        {
            string tipoReporte = "";
          
            switch (reporteId)
            {
                case 1:
                    tipoReporte = "Sand Blast";
                    break;
                case 2:
                    tipoReporte = (CultureInfo.CurrentCulture.Name == "en-US") ? "Primary" : "Pimarios";
                    break;
                case 3:
                    tipoReporte = (CultureInfo.CurrentCulture.Name == "en-US") ? "Intermediate" : "Intermedios";
                    break;
                case 4:
                    tipoReporte = (CultureInfo.CurrentCulture.Name == "en-US") ? "Adhesion" : "Adherencia";
                    break;
                case 5:
                    tipoReporte = (CultureInfo.CurrentCulture.Name == "en-US") ? "Final Coat" : "Acabado Visual";
                    break;
                case 6:
                    tipoReporte = "Pull Off";
                    break;

            }

            return tipoReporte;
        }

        private void validarFecha(DateTime? fechaActual, DateTime? fechaNueva, string reporte)
        {
            if (fechaActual.HasValue && fechaNueva.HasValue)
            {
                if (fechaActual != fechaNueva)
                {
                    throw new ExcepcionPintura("El reporte ya existe con la fecha "+ fechaActual);
                }
            }
        }

        /// <summary>
        /// Elimina el reporte de pintura enviado. 
        /// Si ya no hay reportes dados de alta entonces elimina el registro de PinturaSpool        
        /// </summary>
        /// <param name="workstatusID">workstatusID a actualizar / eliminar</param>
        /// <param name="userID">Usuario que modifica</param>
        /// <param name="tipoReporte">Tipo de reporte de pintura a eliminar</param>
        public void EliminaPinturaSpool(int workstatusID, Guid userID, int tipoReporte)
        {
            using (SamContext ctx = new SamContext())
            {
                WorkstatusSpool wss = ctx.WorkstatusSpool.Where(x => x.WorkstatusSpoolID == workstatusID).SingleOrDefault();

                if (wss.Preparado)
                {
                    throw new ExcepcionPintura(MensajesError.Excepcion_TienePreparacion);
                }
                else
                {
                    PinturaSpool pinturaSpool = ctx.PinturaSpool.Where(x => x.WorkstatusSpoolID == workstatusID).FirstOrDefault();
                    pinturaSpool.StartTracking();

                    switch ((TipoPinturaEnum)tipoReporte)
                    {
                        case TipoPinturaEnum.SandBlast:
                            pinturaSpool.FechaSandblast = null;
                            pinturaSpool.ReporteSandblast = null;
                            break;
                        case TipoPinturaEnum.Intermedio:
                            pinturaSpool.FechaIntermedios = null;
                            pinturaSpool.ReporteIntermedios = null;
                            break;
                        case TipoPinturaEnum.Primario:
                            pinturaSpool.FechaPrimarios = null;
                            pinturaSpool.ReportePrimarios = null;
                            break;
                        case TipoPinturaEnum.Adherencia:
                            pinturaSpool.FechaAdherencia = null;
                            pinturaSpool.ReporteAdherencia = null;
                            break;
                        case TipoPinturaEnum.AcabadoVisual:
                            pinturaSpool.FechaAcabadoVisual = null;
                            pinturaSpool.ReporteAcabadoVisual = null;
                            break;
                        case TipoPinturaEnum.PullOff:
                            pinturaSpool.FechaPullOff = null;
                            pinturaSpool.ReportePullOff = null;
                            break;
                    }

                    pinturaSpool.UsuarioModifica = userID;
                    pinturaSpool.FechaModificacion = DateTime.Now;
                    ctx.PinturaSpool.ApplyChanges(pinturaSpool);

                    //Verificamos si todos son nulos para eliminar el registro por completo
                    if (!pinturaSpool.FechaSandblast.HasValue && string.IsNullOrEmpty(pinturaSpool.ReporteSandblast)
                        && !pinturaSpool.FechaIntermedios.HasValue && string.IsNullOrEmpty(pinturaSpool.ReporteIntermedios)
                        && !pinturaSpool.FechaPrimarios.HasValue && string.IsNullOrEmpty(pinturaSpool.ReportePrimarios)
                        && !pinturaSpool.FechaAdherencia.HasValue && string.IsNullOrEmpty(pinturaSpool.ReporteAdherencia)
                        && !pinturaSpool.FechaAcabadoVisual.HasValue && string.IsNullOrEmpty(pinturaSpool.ReporteAcabadoVisual)
                        && !pinturaSpool.FechaPullOff.HasValue && string.IsNullOrEmpty(pinturaSpool.ReportePullOff))
                    {
                        ctx.PinturaSpool.DeleteObject(pinturaSpool);
                        WorkstatusSpool workstatus = ctx.WorkstatusSpool.Where(x => x.WorkstatusSpoolID == workstatusID).FirstOrDefault();
                        workstatus.StartTracking();
                        workstatus.TienePintura = false;
                        workstatus.UsuarioModifica = userID;
                        workstatus.FechaModificacion = DateTime.Now;
                        ctx.WorkstatusSpool.ApplyChanges(workstatus);
                    }

                    ctx.SaveChanges();
                }
            }

        }

        /// <summary>
        /// Elimina la información de sistema pintura
        /// </summary>
        /// <param name="workstatusID"></param>
        /// <param name="userID"></param>
        public void BorraSistema(int workstatusID, Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                WorkstatusSpool wks = ctx.WorkstatusSpool.Include("OrdenTrabajoSpool").Where(x => x.WorkstatusSpoolID == workstatusID).FirstOrDefault();
                Spool spool = ctx.Spool.Where(x => x.SpoolID == wks.OrdenTrabajoSpool.SpoolID).FirstOrDefault();
                spool.StartTracking();

                spool.CodigoPintura = null;
                spool.SistemaPintura = null;
                spool.ColorPintura = null;
                spool.UsuarioModifica = userID;
                spool.FechaModificacion = DateTime.Now;

                ctx.Spool.ApplyChanges(spool);
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Apaga la bandera de liberación pintura.
        /// </summary>
        /// <param name="workstatusID"></param>
        /// <param name="userID"></param>
        public void BorraLiberacion(int workstatusID, Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                WorkstatusSpool wks = ctx.WorkstatusSpool.Where(x => x.WorkstatusSpoolID == workstatusID).FirstOrDefault();
                wks.LiberadoPintura = false;
                wks.UsuarioModifica = userID;
                wks.FechaModificacion = DateTime.Now;
                ctx.WorkstatusSpool.ApplyChanges(wks);
                ctx.SaveChanges();
            }
        }


        protected bool ValidaFecha(PinturaSpool spool, string fechaReq)
        {
            DateTime defaultDateTime = new DateTime();
            DateTime FechaSandBlast = DateTime.ParseExact(spool.FechaSandblast == null ? defaultDateTime.ToShortDateString() : spool.FechaSandblast.Value.ToShortDateString(), "dd/MM/yyyy", null);
            DateTime FechaAcabdoVisual = DateTime.ParseExact(spool.FechaAcabadoVisual == null ? defaultDateTime.ToShortDateString() : spool.FechaAcabadoVisual.Value.ToShortDateString(), "dd/MM/yyyy", null);
            DateTime FechaAdherencia = DateTime.ParseExact(spool.FechaAdherencia == null ? defaultDateTime.ToShortDateString() : spool.FechaAdherencia.Value.ToShortDateString(), "dd/MM/yyyy", null);
            DateTime FechaIntermedio = DateTime.ParseExact(spool.FechaIntermedios == null ? defaultDateTime.ToShortDateString() : spool.FechaIntermedios.Value.ToShortDateString(), "dd/MM/yyyy", null);
            DateTime FechaPrimario = DateTime.ParseExact(spool.FechaPrimarios == null ? defaultDateTime.ToShortDateString() : spool.FechaPrimarios.Value.ToShortDateString(), "dd/MM/yyyy", null);
            DateTime FechaPulOff = DateTime.ParseExact(spool.FechaPullOff == null ? defaultDateTime.ToShortDateString() : spool.FechaPullOff.Value.ToShortDateString(), "dd/MM/yyyy", null);

            if (!string.IsNullOrEmpty(fechaReq))
            {
                DateTime tempFecha = DateTime.ParseExact(fechaReq, "MM/dd/yyyy", null);
                if (FechaSandBlast != defaultDateTime && FechaSandBlast < tempFecha
                    || FechaAcabdoVisual != defaultDateTime && FechaAcabdoVisual < tempFecha
                    || FechaAdherencia != defaultDateTime && FechaAdherencia < tempFecha 
                    || FechaIntermedio != defaultDateTime && FechaIntermedio < tempFecha
                    || FechaPrimario != defaultDateTime && FechaPrimario < tempFecha
                    || FechaPulOff != defaultDateTime && FechaPulOff < tempFecha)
                {
                    return true;
                }              
            }

            return false;
        }

        public List<string> validaFechaReportePintura(PinturaSpool spool, PinturaSpool pinturaSpool, IQueryable<PinturaSpool> reportesProyecto)
        {
           
            List<string> errores = ValidaNumeroReporte(spool);
            if (errores.Count > 0)
            {
                throw new ExcepcionPintura(errores);
            }
            if (spool.FechaSandblast.HasValue)
            {
                if (reportesProyecto != null)
                {
                    List<PinturaSpool> registrosReporte = reportesProyecto.Where(x => x.ReporteSandblast == spool.ReporteSandblast).OrderBy(x => x.PinturaSpoolID).ToList();
                    DateTime? fecha = (registrosReporte.Count) > 0 ? registrosReporte.FirstOrDefault().FechaSandblast : null;

                    if (fecha != null)
                    {
                        if (spool.FechaSandblast != fecha)
                        {
                            errores.Add(string.Format(MensajesError.Excepcion_FechaReportePintura, GetTipoReporte(1), spool.ReporteSandblast, string.Format("{0:dd/MM/yyyy}", fecha)));
                        }
                    }
                }
                pinturaSpool.FechaSandblast = spool.FechaSandblast.Value;
                pinturaSpool.ReporteSandblast = !string.IsNullOrWhiteSpace(spool.ReporteSandblast) ? spool.ReporteSandblast : pinturaSpool.ReporteSandblast;
            }

            if (spool.FechaPrimarios.HasValue)
            {
                if (reportesProyecto != null)
                {
                    List<PinturaSpool> registrosReporte = reportesProyecto.Where(x => x.ReportePrimarios == spool.ReportePrimarios).OrderBy(x => x.PinturaSpoolID).ToList();
                    DateTime? fecha = (registrosReporte.Count) > 0 ? registrosReporte.FirstOrDefault().FechaPrimarios : null;

                    if (fecha != null)
                    {
                        if (spool.FechaPrimarios != fecha)
                        {
                            errores.Add(string.Format(MensajesError.Excepcion_FechaReportePintura, GetTipoReporte(2), spool.ReportePrimarios, string.Format("{0:dd/MM/yyyy}", fecha)));
                        }
                    }
                }

                pinturaSpool.FechaPrimarios = spool.FechaPrimarios.Value;
                pinturaSpool.ReportePrimarios = !string.IsNullOrWhiteSpace(spool.ReportePrimarios) ? spool.ReportePrimarios : pinturaSpool.ReportePrimarios;
            }

            if (spool.FechaIntermedios.HasValue)
            {
                if (reportesProyecto != null)
                {
                    List<PinturaSpool> registrosReporte = reportesProyecto.Where(x => x.ReporteIntermedios == spool.ReporteIntermedios).OrderBy(x => x.PinturaSpoolID).ToList();
                    DateTime? fecha = (registrosReporte.Count) > 0 ? registrosReporte.FirstOrDefault().FechaIntermedios : null;

                    if (fecha != null)
                    {
                        if (spool.FechaIntermedios != fecha)
                        {
                            errores.Add(string.Format(MensajesError.Excepcion_FechaReportePintura, GetTipoReporte(3), spool.ReporteIntermedios, string.Format("{0:dd/MM/yyyy}", fecha)));
                        }
                    }
                }
                pinturaSpool.FechaIntermedios = spool.FechaIntermedios.Value;
                pinturaSpool.ReporteIntermedios = !string.IsNullOrWhiteSpace(spool.ReporteIntermedios) ? spool.ReporteIntermedios : pinturaSpool.ReporteIntermedios;
            }

            if (spool.FechaAdherencia.HasValue)
            {
                if (reportesProyecto != null)
                {
                    List<PinturaSpool> registrosReporte = reportesProyecto.Where(x => x.ReporteAdherencia == spool.ReporteAdherencia).OrderBy(x => x.PinturaSpoolID).ToList();
                    DateTime? fecha = (registrosReporte.Count) > 0 ? registrosReporte.FirstOrDefault().FechaAdherencia : null;

                    if (fecha != null)
                    {
                        if (spool.FechaAdherencia != fecha)
                        {
                            errores.Add(string.Format(MensajesError.Excepcion_FechaReportePintura, GetTipoReporte(4), spool.ReporteAdherencia, string.Format("{0:dd/MM/yyyy}", fecha)));
                        }
                    }
                }
                pinturaSpool.FechaAdherencia = spool.FechaAdherencia.Value;
                pinturaSpool.ReporteAdherencia = !string.IsNullOrWhiteSpace(spool.ReporteAdherencia) ? spool.ReporteAdherencia : pinturaSpool.ReporteAdherencia;
            }

            if (spool.FechaAcabadoVisual.HasValue)
            {
                if (reportesProyecto != null)
                {
                    List<PinturaSpool> registrosReporte = reportesProyecto.Where(x => x.ReporteAcabadoVisual == spool.ReporteAcabadoVisual).OrderBy(x => x.PinturaSpoolID).ToList();
                    DateTime? fecha = (registrosReporte.Count) > 0 ? registrosReporte.FirstOrDefault().FechaAcabadoVisual : null;

                    if (fecha != null)
                    {
                        if (spool.FechaAcabadoVisual != fecha)
                        {
                            errores.Add(string.Format(MensajesError.Excepcion_FechaReportePintura, GetTipoReporte(5), spool.ReporteAcabadoVisual, string.Format("{0:dd/MM/yyyy}", fecha)));
                        }
                    }
                }
                pinturaSpool.FechaAcabadoVisual = spool.FechaAcabadoVisual.Value;
                pinturaSpool.ReporteAcabadoVisual = !string.IsNullOrWhiteSpace(spool.ReporteAcabadoVisual) ? spool.ReporteAcabadoVisual : pinturaSpool.ReporteAcabadoVisual;
            }

            if (spool.FechaPullOff.HasValue)
            {
                if (reportesProyecto != null)
                {
                    List<PinturaSpool> registrosReporte = reportesProyecto.Where(x => x.ReportePullOff == spool.ReportePullOff).OrderBy(x => x.PinturaSpoolID).ToList();
                    DateTime? fecha = (registrosReporte.Count) > 0 ? registrosReporte.FirstOrDefault().FechaPullOff : null;

                    if (fecha != null)
                    {
                        if (spool.FechaPullOff != fecha)
                        {
                            errores.Add(string.Format(MensajesError.Excepcion_FechaReportePintura, GetTipoReporte(6), spool.ReportePullOff, string.Format("{0:dd/MM/yyyy}", fecha)));
                        }
                    }
                }

                pinturaSpool.FechaPullOff = spool.FechaPullOff.Value;
                pinturaSpool.ReportePullOff = !string.IsNullOrWhiteSpace(spool.ReportePullOff) ? spool.ReportePullOff : pinturaSpool.ReportePullOff;
            }
            return errores;
        }

        public List<PinturaSpool> ObtenerListadoPinturas(int [] juntasWsID)
        {
            using (SamContext ctx = new SamContext())
            {
                List<PinturaSpool> pss = new List<PinturaSpool>();

                foreach(int jwsID in juntasWsID)
                {

                    Simple ps = (from jws in ctx.JuntaWorkstatus
                              join ots in ctx.OrdenTrabajoSpool on jws.OrdenTrabajoSpoolID equals ots.OrdenTrabajoSpoolID
                              join wss in ctx.WorkstatusSpool on ots.OrdenTrabajoSpoolID equals wss.OrdenTrabajoSpoolID into wssEmpty
                              from workstatusspool in wssEmpty.DefaultIfEmpty()
                              where jws.JuntaWorkstatusID == jwsID
                              select new Simple
                              {
                                  ID = workstatusspool != null ? workstatusspool.WorkstatusSpoolID : 0,
                                  Valor = string.Empty
                              }).FirstOrDefault();

                    PinturaSpool pinturaSpool = ctx.PinturaSpool.Where(x => x.WorkstatusSpoolID == ps.ID).FirstOrDefault();
                  
                    pss.Add(pinturaSpool);
                
                }

                return pss;
            }
            
        }
    }
}
