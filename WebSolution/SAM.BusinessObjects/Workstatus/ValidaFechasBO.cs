using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using Mimo.Framework.Extensions;
using SAM.Entities;
using SAM.BusinessObjects.Excepciones;

namespace SAM.BusinessObjects.Workstatus
{
    public class ValidaFechasBO
    {
        private static readonly object _mutex = new object();
        private static ValidaFechasBO _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private ValidaFechasBO()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static ValidaFechasBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ValidaFechasBO();
                    }
                }
                return _instance;
            }
        }

        #region JuntaWorkStatus

        public void ValidaFechasArmado(DateTime fechaProcSoldadura, DateTime fechaProcArmado)
        {
            if (fechaProcArmado > fechaProcSoldadura)
            {
                throw new Excepciones.ExcepcionSoldadura(string.Format(MensajesError.Excepcion_FechaArmadoMayorSoldadura, string.Empty, fechaProcSoldadura.ToShortDateString()));
            }
        }

        public void ValidaFechaSoldadura(DateTime fechaProcSoldadura, DateTime fechaProcArmado)
        {
            if (fechaProcArmado > fechaProcSoldadura)
            {
                throw new Excepciones.ExcepcionSoldadura(string.Format(MensajesError.Excepcion_FechaArmadoMayorSoldadura, fechaProcArmado.ToShortDateString(), string.Empty));
            }
        }

        public void ValidaFechasProcesoFechaRequiPintura(DateTime fechaProcesoActual, int otsId)
        {

            using (SamContext ctx = new SamContext())
            {
                string procesos = string.Empty;

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

        public string ValidaFechasProcesoFechaRequiPinturas(DateTime fechaProcesoActual, int otsId)
        {
            using (SamContext ctx = new SamContext())
            {
                string procesos = string.Empty;

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

                return procesos;
            }
        }


        public void ValidaFechasLiberacion(DateTime fechaProcAnterior, DateTime fechaProcLiberacion)
        {
            if (fechaProcAnterior > fechaProcLiberacion)
            {
                throw new Excepciones.ExcepcionFechas(string.Format(MensajesError.Excepcion_FechaProcAnteriorMayorLiberacion, fechaProcAnterior.ToShortDateString(), fechaProcLiberacion.ToShortDateString()));
            }
        }


        public string ObtenerFechaReporteSoldadura(int juntaSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                string fecha = "1";

                JuntaWorkstatus jws = ctx.JuntaWorkstatus.Where(x => x.JuntaSpoolID == juntaSpoolID && x.JuntaFinal == true).SingleOrDefault();
                if (jws != null && jws.SoldaduraAprobada)
                {
                    JuntaSoldadura juntaSoldadura = ctx.JuntaSoldadura.SingleOrDefault(x => x.JuntaWorkstatusID == jws.JuntaWorkstatusID);

                    if (juntaSoldadura != null)
                    {
                        DateTime fechaReporte = juntaSoldadura.FechaReporte;
                        fecha = string.Format("{0:yyyy/MM/dd}", fechaReporte);
                    }
                }

                return fecha;
            }
        }

        public string ObtenerFechasSoldadura(int juntaSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntaWorkstatus jws = ctx.JuntaWorkstatus.Where(x => x.JuntaSpoolID == juntaSpoolID && x.JuntaFinal == true).SingleOrDefault();
                if (jws != null && jws.SoldaduraAprobada)
                {
                    DateTime fechaSoldadura = ctx.JuntaSoldadura.Single(x => x.JuntaWorkstatusID == jws.JuntaWorkstatusID).FechaSoldadura;
                    DateTime fechaReporte = ctx.JuntaSoldadura.Single(x => x.JuntaWorkstatusID == jws.JuntaWorkstatusID).FechaReporte;

                    return fechaSoldadura.ToShortDateString() + "," + fechaReporte.ToShortDateString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string ObtenerFechaReporteArmado(int juntaWorkstatusID)
        {
            using (SamContext ctx = new SamContext())
            {
                string fecha = string.Empty;

                DateTime fechaReporte = ctx.JuntaArmado.Single(x => x.JuntaWorkstatusID == juntaWorkstatusID).FechaReporte;
                fecha = fechaReporte.ToShortDateString();

                return fecha;
            }
        }

        public string ObtenerFechaProcesoArmado(int juntaWorkstatusID)
        {
            using (SamContext ctx = new SamContext())
            {
                string fecha = string.Empty;

                DateTime fechaProceso = ctx.JuntaArmado.Single(x => x.JuntaWorkstatusID == juntaWorkstatusID).FechaArmado;
                fecha = fechaProceso.ToShortDateString();

                return fecha;
            }
        }

        public string ObtenerFechasArmado(int juntaWorkstatusID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (ctx.JuntaArmado.Where(x => x.JuntaWorkstatusID == juntaWorkstatusID).Any())
                {
                    DateTime FechaArmado = ctx.JuntaArmado.Single(x => x.JuntaWorkstatusID == juntaWorkstatusID).FechaArmado;
                    DateTime FechaReporteArmado = ctx.JuntaArmado.Single(x => x.JuntaWorkstatusID == juntaWorkstatusID).FechaReporte;

                    return FechaArmado.ToString("MM/dd/yyyy") + "," + FechaReporteArmado.ToString("MM/dd/yyyy");
                }
                else
                {
                    return string.Empty;
                }

            }
        }

        public DateTime FechaArmado(int juntaWorkstatusID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (ctx.JuntaArmado.Where(x => x.JuntaWorkstatusID == juntaWorkstatusID).Any())
                {
                    return ctx.JuntaArmado.Single(x => x.JuntaWorkstatusID == juntaWorkstatusID).FechaArmado;
                }
                else
                    return DateTime.MinValue;
            }
        }

        public DateTime FechaReporteArmado(int juntaWorkstatusID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (ctx.JuntaArmado.Where(x => x.JuntaWorkstatusID == juntaWorkstatusID).Any())
                {
                    return ctx.JuntaArmado.Single(x => x.JuntaWorkstatusID == juntaWorkstatusID).FechaReporte;
                }
                else
                    return DateTime.MinValue;
            }
        }

        public string ObtenerSpoolPorInspeccionVisualPatioID(string LiberacionVisualIds)
        {
            using (SamContext ctx = new SamContext())
            {
                int[] lvids = LiberacionVisualIds.Split(',').Select(x => x.SafeIntParse()).ToArray();

                int[] jwsId = ctx.InspeccionVisualPatio.Where(x => lvids.Contains(x.InspeccionVisualPatioID)).Select(x => x.JuntaWorkstatusID).ToArray();

                int[] jsid = ctx.JuntaWorkstatus.Where(x => jwsId.Contains(x.JuntaWorkstatusID)).Select(x => x.JuntaSpoolID).ToArray();

                int[] spoolid = ctx.JuntaSpool.Where(x => jsid.Contains(x.JuntaSpoolID)).Select(x => x.SpoolID).ToArray();

                string spools = string.Empty;
                foreach (int id in spoolid)
                {
                    spools += ctx.Spool.Single(x => x.SpoolID == id).Nombre + ", ";
                }

                return spools.Substring(0, spools.Length - 2);
            }
        }

        public string ObtenerFechaSoldaduraPorInspeccionVisualPatioID(string LiberacionVisualIds)
        {
            using (SamContext ctx = new SamContext())
            {
                int[] lvids = LiberacionVisualIds.Split(',').Select(x => x.SafeIntParse()).ToArray();

                int[] jwsId = ctx.InspeccionVisualPatio.Where(x => lvids.Contains(x.InspeccionVisualPatioID)).Select(x => x.JuntaWorkstatusID).ToArray();

                return ObtenerFechasReporteSoldadura(jwsId);
            }
        }

        public string ObtenerJuntasPorJuntaWorkstatusIds(int[] juntaWorkstatusIds)
        {
            using (SamContext ctx = new SamContext())
            {
                string juntas = string.Empty;
                foreach (int id in juntaWorkstatusIds)
                {
                    JuntaWorkstatus jws = ctx.JuntaWorkstatus.Single(x => x.JuntaWorkstatusID == id);

                    juntas += jws.EtiquetaJunta + ", ";
                }

                return juntas.Substring(0, juntas.Length - 2);
            }
        }

        public string ObtenerNumControlJuntasPorJuntaWorkstatusIds(int[] juntaWorkstatusIds)
        {
            using (SamContext ctx = new SamContext())
            {
                string juntas = string.Empty;
                foreach (int id in juntaWorkstatusIds.Where(x => x > 0))
                {
                    JuntaWorkstatus jws = ctx.JuntaWorkstatus.Single(x => x.JuntaWorkstatusID == id);

                    juntas += ctx.OrdenTrabajoSpool.Single(x => x.OrdenTrabajoSpoolID == jws.OrdenTrabajoSpoolID).NumeroControl + ", " + jws.EtiquetaJunta + ".";
                }

                return juntas.Substring(0, juntas.Length - 1);
            }
        }

        public string ObtenerFechasInspeccionVisualPatio(string LiberacionVisualIds)
        {
            using (SamContext ctx = new SamContext())
            {
                int[] lvids = LiberacionVisualIds.Split(',').Select(x => x.SafeIntParse()).ToArray();
                int[] jwsId = ctx.InspeccionVisualPatio.Where(x => lvids.Contains(x.InspeccionVisualPatioID)).Select(x => x.JuntaWorkstatusID).ToArray();

                string fechas = string.Empty;
                foreach (int id in jwsId)
                {
                    InspeccionVisualPatio ivp = ctx.InspeccionVisualPatio.SingleOrDefault(x => x.JuntaWorkstatusID == id);

                    if (ivp != null)
                    {
                        DateTime fechaProceso = ivp.FechaInspeccion;
                        fechas += fechaProceso.ToString("MM/dd/yyyy") + "4,";
                    }
                    else
                    {
                        fechas += "4,";
                    }
                }
                return fechas.Substring(0, fechas.Length - 1);
            }
        }

        public string ObtenerJuntasPorInspeccionVisualPatioID(string LiberacionVisualIds)
        {
            using (SamContext ctx = new SamContext())
            {
                int[] lvids = LiberacionVisualIds.Split(',').Select(x => x.SafeIntParse()).ToArray();

                int[] jwsId = ctx.InspeccionVisualPatio.Where(x => lvids.Contains(x.InspeccionVisualPatioID)).Select(x => x.JuntaWorkstatusID).ToArray();

                string juntas = ObtenerJuntasPorJuntaWorkstatusIds(jwsId);

                string spools = ObtenerSpoolPorInspeccionVisualPatioID(LiberacionVisualIds);

                string[] juntasArray = juntas.Split(',').ToArray();
                string[] spoolsArray = spools.Split(',').ToArray();

                string spoolJuntas = string.Empty;

                for (int i = 0; i < juntasArray.Length; i++)
                {
                    spoolJuntas += spoolsArray[i] + ", " + juntasArray[i] + ".";
                }

                return spoolJuntas.Substring(0, spoolJuntas.Length - 1);
            }
        }

        public string ObtenerNumerosControlPorInspeccionDimensionalPatioID(string LiberacionDimensionalIds)
        {
            using (SamContext ctx = new SamContext())
            {
                int[] ldids = LiberacionDimensionalIds.Split(',').Select(x => x.SafeIntParse()).ToArray();

                int[] wssId = ctx.InspeccionDimensionalPatio.Where(x => ldids.Contains(x.InspeccionDimensionalPatioID)).Select(x => x.WorkstatusSpoolID).ToArray();

                string numControl = ObtenerNumerosControlPorWorkstatusSpoolID(wssId);

                string[] numControlArray = numControl.Split(',').ToArray();

                string spoolNumControl = string.Empty;

                for (int i = 0; i < numControlArray.Length; i++)
                {
                    spoolNumControl += numControlArray[i] + ",";
                }

                return spoolNumControl.Substring(0, spoolNumControl.Length - 1);
            }
        }

        public string ObtenerNumerosControlPorSpoolID(int[] spoolIds)
        {
            using (SamContext ctx = new SamContext())
            {
                string numerosControl = string.Empty;
                foreach (int spoolID in spoolIds)
                {
                    numerosControl += ctx.OrdenTrabajoSpool.Single(x => x.SpoolID == spoolID).NumeroControl + ", ";
                }

                return numerosControl.Substring(0, numerosControl.Length - 2);
            }
        }

        public string ObtenerNumerosControlPorJuntaWorkStatusIds(int[] juntaWorkstatusIds)
        {
            using (SamContext ctx = new SamContext())
            {
                string numerosControl = string.Empty;
                foreach (int id in juntaWorkstatusIds)
                {
                    int otsid = ctx.JuntaWorkstatus.Single(x => x.JuntaWorkstatusID == id).OrdenTrabajoSpoolID;

                    numerosControl += ctx.OrdenTrabajoSpool.Single(x => x.OrdenTrabajoSpoolID == otsid).NumeroControl + ", ";
                }

                return numerosControl.Substring(0, numerosControl.Length - 2);
            }
        }

        public string ObtenerNumerosControlPorWorkstatusSpoolID(int[] workstatusSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                string numerosControl = string.Empty;
                foreach (int wss in workstatusSpoolID)
                {
                    WorkstatusSpool workStatusSpool = ctx.WorkstatusSpool.Single(x => x.WorkstatusSpoolID == wss);

                    numerosControl += ctx.OrdenTrabajoSpool.Single(x => x.OrdenTrabajoSpoolID == workStatusSpool.OrdenTrabajoSpoolID).NumeroControl + ", ";
                }

                return numerosControl.Substring(0, numerosControl.Length - 2);
            }
        }

        public string ObtenerNumerosControlPorOrdenDeTrabajoSpoolID(int[] otsIDs) 
        { 
            using (SamContext ctx = new SamContext()) 
            { 
                string numerosControl = string.Empty; 
 
                foreach (int otsID in otsIDs) 
                {
                    numerosControl += ctx.OrdenTrabajoSpool.Single(x => x.OrdenTrabajoSpoolID == otsID).NumeroControl + ", "; 
                } 
                return numerosControl.Substring(0, numerosControl.Length - 2); 
             } 
         } 

        public string ObtenerNumerosControlPorWorkstatusOtsId(int[] otsIDs)  
        {  
            using (SamContext ctx = new SamContext())  
            {  
                string numerosControl = string.Empty;  
                foreach (int otsID in otsIDs)  
                {  
                    numerosControl += ctx.OrdenTrabajoSpool.Single(x => x.OrdenTrabajoSpoolID == otsID).NumeroControl + ", ";  
                }  
                return numerosControl.Substring(0, numerosControl.Length - 2);  
             }  
         } 
        
        public string ObtenerFechasSoldaduraIVPorInspeccionDimensionalPatio(string inspeccionDimensionalIDs)
        {
            using (SamContext ctx = new SamContext())
            {
                string fechas = string.Empty;

                int[] arrayInspeccionDimensionalIDs = inspeccionDimensionalIDs.Split(',').Select(x => x.SafeIntParse()).ToArray();

                foreach (int inspDimID in arrayInspeccionDimensionalIDs)
                {
                    int wss = ctx.InspeccionDimensionalPatio.Single(x => x.InspeccionDimensionalPatioID == inspDimID).WorkstatusSpoolID;
                    int otsID = ctx.WorkstatusSpool.Single(x => x.WorkstatusSpoolID == wss).OrdenTrabajoSpoolID;

                    List<int> jwsID = ctx.JuntaWorkstatus.Where(x => x.OrdenTrabajoSpoolID == otsID).Select(y => y.JuntaWorkstatusID).ToList();

                    if (jwsID.Count > 0)
                    {
                        IQueryable<JuntaInspeccionVisual> jiv = ctx.JuntaInspeccionVisual.Where(x => jwsID.Contains(x.JuntaWorkstatusID));

                        if (jiv.Count() == jwsID.Count)
                        {
                            DateTime fechaReporteIV = ctx.InspeccionVisual.Where(x => jiv.Select(y => y.InspeccionVisualID).Contains(x.InspeccionVisualID))
                                                                        .OrderByDescending(y => y.FechaReporte)
                                                                        .Select(y => y.FechaReporte).First();

                            fechas += String.Format("{0:yyyy/MM/dd}", fechaReporteIV) + ",";
                        }
                        else
                        {
                            IQueryable<JuntaSoldadura> jsold = ctx.JuntaSoldadura.Where(x => jwsID.Contains(x.JuntaWorkstatusID));

                            if (jsold.Count() > 0)
                            {
                                DateTime fechaReporte = ctx.JuntaSoldadura.Where(x => jsold.Select(y => y.JuntaSoldaduraID).Contains(x.JuntaSoldaduraID))
                                                                            .OrderByDescending(y => y.FechaReporte)
                                                                            .Select(y => y.FechaReporte).First();

                                fechas += String.Format("{0:yyyy/MM/dd}", fechaReporte) + ",";
                            }
                        }
                    }
                }

                fechas = string.IsNullOrEmpty(fechas) ? fechas : fechas.Substring(0, fechas.Length - 1);
                return fechas;
            }
        }

        public string ObtenerFechasSoldaduraPorSpoolConcatenadas(int[] spoolIds, bool esReporte)
        {
            using (SamContext ctx = new SamContext())
            {
                string fechas = string.Empty;

                foreach (int spoolid in spoolIds)
                {
                    int otsID = ctx.OrdenTrabajoSpool.Single(x => x.SpoolID == spoolid).OrdenTrabajoSpoolID;

                    List<int> jwsID = ctx.JuntaWorkstatus.Where(x => x.OrdenTrabajoSpoolID == otsID).Select(y => y.JuntaWorkstatusID).ToList();

                    if (jwsID.Count > 0)
                    {
                        IQueryable<JuntaInspeccionVisual> jiv = ctx.JuntaInspeccionVisual.Where(x => jwsID.Contains(x.JuntaWorkstatusID));
                        if (jiv.Count() == jwsID.Count)
                        {
                            DateTime fechaIV = ctx.JuntaInspeccionVisual.Where(x => jiv.Select(y => y.JuntaInspeccionVisualID).Contains(x.JuntaInspeccionVisualID))
                                                                     .OrderByDescending(y => y.FechaInspeccion)
                                                                     .Select(y => y.FechaInspeccion.Value).First();

                            DateTime fechaReporteIV = ctx.InspeccionVisual.Where(x => jiv.Select(y => y.InspeccionVisualID).Contains(x.InspeccionVisualID))
                                                                      .OrderByDescending(y => y.FechaReporte)
                                                                      .Select(y => y.FechaReporte).First();

                            if (!esReporte)
                            {
                                fechas += fechaIV.ToShortDateString() + ",";
                            }
                            else
                            {
                                fechas += fechaReporteIV.ToShortDateString() + ",";
                            }
                        }
                        else
                        {

                            IQueryable<JuntaSoldadura> jsold = ctx.JuntaSoldadura.Where(x => jwsID.Contains(x.JuntaWorkstatusID));

                            JuntaSoldadura soldFecha = ctx.JuntaSoldadura.Where(x => jsold.Select(y => y.JuntaSoldaduraID).Contains(x.JuntaSoldaduraID))
                                                                         .OrderByDescending(y => y.FechaSoldadura)
                                                                         .Select(y => y).FirstOrDefault();

                            JuntaSoldadura soldReporte = ctx.JuntaSoldadura.Where(x => jsold.Select(y => y.JuntaSoldaduraID).Contains(x.JuntaSoldaduraID))
                                                                      .OrderByDescending(y => y.FechaReporte)
                                                                      .Select(y => y).FirstOrDefault();

                            if (soldFecha == null || soldReporte == null)
                            {
                                fechas += "-1,";
                            }
                            else if (!esReporte)
                            {
                                fechas += soldFecha.FechaSoldadura.ToShortDateString() + ",";
                            }
                            else
                            {
                                fechas += soldReporte.FechaReporte.ToShortDateString() + ",";
                            }
                        }
                    }
                    else
                    {
                        fechas += "-1,";
                    }
                }

                return fechas.Substring(0, fechas.Length - 1);
            }
        }

        public string ObtenerFechasReporteSoldadura(int[] juntaWorkstatusIds)
        {
            using (SamContext ctx = new SamContext())
            {
                string fechas = string.Empty;

                // ignoramos las juntas que no tienen juntaWorkstatus y que se darán de alta posteriormente
                foreach (int id in juntaWorkstatusIds.Where(x => x > 0))
                {
                    JuntaSoldadura js = ctx.JuntaSoldadura.SingleOrDefault(x => x.JuntaWorkstatusID == id);

                    if (js != null)
                    {
                        DateTime fechaReporte = js.FechaReporte;
                        fechas += fechaReporte.ToShortDateString() + ",";
                    }
                    else
                    {
                        fechas += ",";
                    }
                }
                return fechas.Substring(0, fechas.Length - 1);
            }
        }

        public string ObtenerFechasUltimoProcesoSoldadura(int[] workstatusSpoolIds)
        {
            using (SamContext ctx = new SamContext())
            {
                string fechas = string.Empty;

                foreach (int id in workstatusSpoolIds)
                {
                    OrdenTrabajoSpool ots = ctx.WorkstatusSpool.Include("OrdenTrabajoSpool.Spool.JuntaSpool")
                                                               .Single(x => x.WorkstatusSpoolID == id)
                                                               .OrdenTrabajoSpool;

                    int[] juntaSpoolIds = ots.Spool.JuntaSpool.Select(x => x.JuntaSpoolID).ToArray();
                    int[] juntaWorkstatusIds = ctx.JuntaWorkstatus.Where(x => x.OrdenTrabajoSpoolID == ots.OrdenTrabajoSpoolID &&
                                                                              juntaSpoolIds.Contains(x.JuntaSpoolID) &&
                                                                              x.JuntaFinal)
                                                                  .Select(x => x.JuntaWorkstatusID).ToArray();

                    JuntaSoldadura js = ctx.JuntaSoldadura.Where(x => juntaWorkstatusIds.Contains(x.JuntaWorkstatusID)).OrderByDescending(x => x.FechaSoldadura).First();

                    DateTime fechaReporte = js.FechaSoldadura;
                    fechas += fechaReporte.ToShortDateString() + ",";
                }
                return fechas.Substring(0, fechas.Length - 1);
            }
        }

        public string ObtenerFechasProcesoSoldadura(int[] juntaWorkstatusIds)
        {
            using (SamContext ctx = new SamContext())
            {
                string fechas = string.Empty;

                // ignoramos las juntas que no tienen juntaWorkstatus y que se darán de alta posteriormente
                foreach (int id in juntaWorkstatusIds.Where(x => x > 0))
                {
                    JuntaSoldadura js = ctx.JuntaSoldadura.SingleOrDefault(x => x.JuntaWorkstatusID == id);

                    if (js != null)
                    {
                        DateTime fechaReporte = js.FechaSoldadura;
                        fechas += fechaReporte.ToString("MM/dd/yyyy") + ",";
                    }
                    else
                    {
                        fechas += ",";
                    }
                }
                return fechas.Substring(0, fechas.Length - 1);
            }
        }

        public DateTime ObtenerFechaProcesoSoldadura(int juntaWorkstatusId)
        {
            using (SamContext ctx = new SamContext())
            {
                DateTime fecha;

                JuntaSoldadura js = ctx.JuntaSoldadura.Single(x => x.JuntaWorkstatusID == juntaWorkstatusId);

                fecha = js.FechaSoldadura.Date;
                return fecha;
            }
        }

        public string ObtenerFechasReporteSoldaduraOLiberacion(int[] juntaWorkstatusIds)
        {
            using (SamContext ctx = new SamContext())
            {
                string fechas = string.Empty;
                foreach (int id in juntaWorkstatusIds)
                {
                    int otsid = ctx.JuntaWorkstatus.Single(x => x.JuntaWorkstatusID == id).OrdenTrabajoSpoolID;

                    WorkstatusSpool workstatusspool = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID == otsid).SingleOrDefault();

                    if (workstatusspool != null && workstatusspool.TieneLiberacionDimensional)
                    {
                        int wssid = workstatusspool.WorkstatusSpoolID;

                        IQueryable<ReporteDimensionalDetalle> reporteDimensionalDetalle = ctx.ReporteDimensionalDetalle.Where(x => x.WorkstatusSpoolID == wssid && x.Aprobado == true);

                        DateTime fechaReporte = ctx.ReporteDimensional.Where(x => reporteDimensionalDetalle.Select(y => y.ReporteDimensionalID).Contains(x.ReporteDimensionalID))
                                                                      .OrderByDescending(y => y.FechaReporte)
                                                                      .Select(y => y.FechaReporte).First();
                        fechas += fechaReporte.ToString("MM/dd/yyyy") + "0,";
                    }
                    else
                    {
                        JuntaSoldadura js = ctx.JuntaSoldadura.SingleOrDefault(x => x.JuntaWorkstatusID == id);
                        if (js != null)
                        {
                            DateTime fechaReporte = js.FechaReporte;
                            fechas += fechaReporte.ToString("MM/dd/yyyy") + "1,";
                        }
                        else
                        {
                            fechas += "1,";
                        }
                    }
                }
                return fechas.Substring(0, fechas.Length - 1);
            }
        }

        public string ObtenerFechasSoldaduraPorJWSConcatenadas(int[] juntaWorkstatusIds)
        {
            using (SamContext ctx = new SamContext())
            {
                string fechas = string.Empty;
                foreach (int id in juntaWorkstatusIds)
                {
                    int otsid = ctx.JuntaWorkstatus.Single(x => x.JuntaWorkstatusID == id).OrdenTrabajoSpoolID;

                    WorkstatusSpool workstatusspool = ctx.WorkstatusSpool.SingleOrDefault(x => x.OrdenTrabajoSpoolID == otsid);

                    if (workstatusspool != null && workstatusspool.TieneLiberacionDimensional)
                    {
                        int wssid = workstatusspool.WorkstatusSpoolID;

                        IQueryable<ReporteDimensionalDetalle> reporteDimensionalDetalle = ctx.ReporteDimensionalDetalle.Where(x => x.WorkstatusSpoolID == wssid && x.Aprobado == true);

                        DateTime fechaLiberacion = reporteDimensionalDetalle.OrderByDescending(x => x.FechaLiberacion).Select(x => x.FechaLiberacion.Value).First();

                        fechas += fechaLiberacion.ToString("MM/dd/yyyy") + "0,";
                    }
                    else
                    {
                        JuntaSoldadura js = ctx.JuntaSoldadura.SingleOrDefault(x => x.JuntaWorkstatusID == id);
                        if (js != null)
                        {
                            DateTime fechaSoldadura = js.FechaSoldadura;

                            fechas += fechaSoldadura.ToString("MM/dd/yyyy") + "1,";
                        }
                        else
                        {
                            fechas += "1,";
                        }
                    }
                }
                return fechas.Substring(0, fechas.Length - 1);
            }
        }

        public string ObtenerFechaIV(int juntaWorkstatusId)
        {
            using (SamContext ctx = new SamContext())
            {
                string fechas = string.Empty;

                JuntaInspeccionVisual jiv = ctx.JuntaInspeccionVisual.SingleOrDefault(x => x.JuntaWorkstatusID == juntaWorkstatusId && x.Aprobado == true);

                if (jiv != null)
                {
                    DateTime fechaIV = ctx.JuntaInspeccionVisual.Single(x => x.JuntaInspeccionVisualID == jiv.JuntaInspeccionVisualID).FechaInspeccion.Value;
                    DateTime fechaReporte = ctx.InspeccionVisual.Single(x => x.InspeccionVisualID == jiv.InspeccionVisualID).FechaReporte;

                    fechas = fechaReporte.ToString("MM/dd/yyyy");
                }

                return fechas;
            }
        }

        public string ObtenerFechasIV(int[] juntaWorkstatusIds)
        {
            using (SamContext ctx = new SamContext())
            {
                string fechas = string.Empty;
                foreach (int id in juntaWorkstatusIds)
                {
                    JuntaInspeccionVisual jiv = ctx.JuntaInspeccionVisual.Single(x => x.JuntaWorkstatusID == id && x.Aprobado == true);

                    if (jiv != null)
                    {
                        DateTime fechaIV = ctx.JuntaInspeccionVisual.Single(x => x.JuntaInspeccionVisualID == jiv.JuntaInspeccionVisualID).FechaInspeccion.Value;
                        DateTime fechaReporte = ctx.InspeccionVisual.Single(x => x.InspeccionVisualID == jiv.InspeccionVisualID).FechaReporte;

                        if (fechaIV >= fechaReporte)
                        {
                            fechas += fechaIV.ToString("MM/dd/yyyy") + ",";
                        }
                        else
                        {
                            fechas += fechaReporte.ToString("MM/dd/yyyy") + ",";
                        }
                    }
                }

                return fechas.Substring(0, fechas.Length - 1);
            }
        }

        public string ObtenerFechasReporteArmado(int[] juntaWorkstatusIds)
        {
            using (SamContext ctx = new SamContext())
            {
                string reportesArmado = string.Empty;

                List<JuntaArmado> jas = (from ja in ctx.JuntaArmado.Include("JuntaWorkstatus.JuntaSpool")
                                         where juntaWorkstatusIds.Contains(ja.JuntaWorkstatusID)
                                         select ja).ToList();

                foreach (JuntaArmado ja in jas)
                {
                    reportesArmado += ja.FechaReporte.ToString("MM/dd/yyyy") + "||" + ja.JuntaWorkstatus.JuntaSpool.Etiqueta + ",";
                }

                return reportesArmado;
            }
        }

        public string ObtenerFechasRequisicionesSpool(int[] workstatusSpoolsIds, string RIDs)
        {
            using (SamContext ctx = new SamContext())
            {
                int[] juntaRequisicion = RIDs.Split(',').Select(x => x.SafeIntParse()).ToArray();
                string fechas = string.Empty;
                int contador = 0;
                foreach (int id in workstatusSpoolsIds)
                {
                    int reqID = juntaRequisicion[contador];
                    SpoolRequisicion jr = ctx.SpoolRequisicion.Where(x => x.RequisicionSpoolID == reqID && x.WorkstatusSpoolID == id).SingleOrDefault();

                    DateTime fecha = ctx.RequisicionSpool.Single(x => x.RequisicionSpoolID == jr.RequisicionSpoolID).FechaRequisicion;

                    fechas += fecha.ToString("MM/dd/yyyy") + ",";
                    contador++;
                }
                return fechas.Substring(0, fechas.Length - 1);
            }
        }

        public string ObtenerFechasRequisiciones(int[] juntaWorkstatusIds, string RIDs)
        {
            using (SamContext ctx = new SamContext())
            {
                int[] juntaRequisicion = RIDs.Split(',').Select(x => x.SafeIntParse()).ToArray();
                string fechas = string.Empty;
                int contador = 0;
                foreach (int id in juntaWorkstatusIds)
                {
                    int reqID = juntaRequisicion[contador];
                    JuntaRequisicion jr = ctx.JuntaRequisicion.Where(x => x.RequisicionID == reqID && x.JuntaWorkstatusID == id).SingleOrDefault();

                    DateTime fecha = ctx.Requisicion.Single(x => x.RequisicionID == jr.RequisicionID).FechaRequisicion;

                    fechas += fecha.ToString("MM/dd/yyyy") + ",";
                    contador++;
                }
                return fechas.Substring(0, fechas.Length - 1);
            }
        }

        public string ObtenerFechasDimensionales(int[] workstatusSpools) 
        { 
            using (SamContext ctx = new SamContext()) 
            { 
                 string fechas = string.Empty; 
                 foreach (int id in workstatusSpools) 
                 { 
                     IQueryable<ReporteDimensionalDetalle> rdd = ctx.ReporteDimensionalDetalle.Where(x => x.WorkstatusSpoolID == id && x.Aprobado == true).OrderByDescending(x => x.FechaLiberacion); 
                     if (rdd.Count() > 0) 
                     { 
                         DateTime fechaLiberacion = rdd.Select(x => x.FechaLiberacion.Value).First(); 
                         fechas += fechaLiberacion.ToString("MM/dd/yyyy") + ","; 
                     } 
                     else 
                     { 
                         fechas += string.Empty+","; 
                     } 
                 } 
                 return string.IsNullOrEmpty(fechas) ? fechas : fechas.Substring(0, fechas.Length - 1); 
             } 
         } 

         public string ObtenerFechasDimensionalesOTS(int[] ordenestrabajospool) 
         { 
             using (SamContext ctx = new SamContext()) 
             { 
                 string fechas = string.Empty; 
                 foreach (int id in ordenestrabajospool) 
                 { 
                     WorkstatusSpool wks = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID == id).FirstOrDefault(); 
                     
                     if (wks != null) 
                     { 
                         IQueryable<ReporteDimensionalDetalle> rdd = ctx.ReporteDimensionalDetalle.Where(x => x.WorkstatusSpoolID == wks.WorkstatusSpoolID && x.Aprobado == true).OrderByDescending(x => x.FechaLiberacion); 
                         if (rdd.Count() > 0) 
                         { 
                             DateTime fechaLiberacion = rdd.Select(x => x.FechaLiberacion.Value).First(); 
                             fechas += fechaLiberacion.ToString("MM/dd/yyyy") + ","; 
                         } 
                         else 
                         { 
                             fechas += string.Empty + ","; 
                         } 
                     } 
                     else 
                     { 
                         fechas += string.Empty + ","; 
                     } 
                 } 
                 return string.IsNullOrEmpty(fechas) ? fechas : fechas.Substring(0, fechas.Length - 1); 
             } 
        } 

        public string ObtenerFechaLiberacionDimensional(int workstatusSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                string fecha = string.Empty;
                IQueryable<ReporteDimensionalDetalle> rdd = ctx.ReporteDimensionalDetalle.Where(x => x.WorkstatusSpoolID == workstatusSpoolID && x.Aprobado == true).OrderByDescending(x => x.FechaLiberacion);
                if (rdd.Count() > 0)
                {
                    DateTime fechaLiberacion = rdd.Select(x => x.FechaLiberacion.Value).First();

                    fecha = fechaLiberacion.ToString("MM/dd/yyyy");
                }

                return fecha;
            }
        }

        public string ObtenerFechaReporteLiberacionDimensional(int workstatusSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                string fecha = string.Empty;
                IQueryable<ReporteDimensionalDetalle> rdd = ctx.ReporteDimensionalDetalle.Include("ReporteDimensional").Where(x => x.WorkstatusSpoolID == workstatusSpoolID && x.Aprobado == true).OrderByDescending(x => x.FechaLiberacion);
                if (rdd.Count() > 0)
                {
                    DateTime fechaReporteLiberacion = rdd.Select(x => x.ReporteDimensional.FechaReporte).First();

                    fecha = fechaReporteLiberacion.ToString("MM/dd/yyyy");
                }

                return fecha;
            }
        }

        public string ObtenerFechasPreparacion(int[] workstatusSpools)
        {
            using (SamContext ctx = new SamContext())
            {
                string fechas = string.Empty;
                foreach (int id in workstatusSpools)
                {
                    IQueryable<WorkstatusSpool> rdd = ctx.WorkstatusSpool.Where(x => x.WorkstatusSpoolID == id).OrderByDescending(x => x.FechaPreparacion);
                    if (rdd.Count() > 0)
                    {
                        DateTime fechaLiberacion = rdd.Select(x => x.FechaPreparacion.Value).First();

                        fechas += fechaLiberacion.ToString("MM/dd/yyyy") + ",";
                    }

                }
                return string.IsNullOrEmpty(fechas) ? fechas : fechas.Substring(0, fechas.Length - 1);
            }
        }


        public string ObtenerFechasDimensionalesPorJW(int[] juntaWorkstatus)
        {
            using (SamContext ctx = new SamContext())
            {
                string fechas = string.Empty;
                foreach (int jid in juntaWorkstatus)
                {
                    int ots = ctx.JuntaWorkstatus.Where(y => y.JuntaWorkstatusID == jid).Select(y => y.OrdenTrabajoSpoolID).Single();
                    WorkstatusSpool wksSpool = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID == ots).FirstOrDefault();
                    if (wksSpool != null)
                    {
                        int id = wksSpool.WorkstatusSpoolID;
                        IQueryable<ReporteDimensionalDetalle> rdd = ctx.ReporteDimensionalDetalle.Where(x => x.WorkstatusSpoolID == id && x.Aprobado == true).OrderByDescending(x => x.FechaLiberacion);

                        DateTime fechaReporte = ctx.ReporteDimensional.Where(x => rdd.Select(y => y.ReporteDimensionalID).Contains(x.ReporteDimensionalID))
                                                                      .OrderByDescending(x => x.FechaReporte)
                                                                      .Select(x => x.FechaReporte).First();

                        DateTime fechaLiberacion = rdd.Select(x => x.FechaLiberacion.Value).First();

                        if (fechaLiberacion >= fechaReporte)
                        {
                            fechas += fechaLiberacion.ToString("MM/dd/yyyy") + ",";
                        }
                        else
                        {
                            fechas += fechaReporte.ToString("MM/dd/yyyy") + ",";
                        }
                    }
                    else
                    {
                        fechas += "" + ",";
                    }
                }
                return fechas.Substring(0, fechas.Length - 1);
            }
        }

        public string ObtenerFechaReqPintura(string reqID)
        {
            int[] requisiciones = reqID.Split(',').Select(x => x.SafeIntParse()).ToArray();
            using (SamContext ctx = new SamContext())
            {
                int requisicionID = requisiciones[0];
                RequisicionPinturaDetalle rpd = ctx.RequisicionPinturaDetalle.Single(x => x.RequisicionPinturaDetalleID == requisicionID);

                DateTime fechaReqPintura = ctx.RequisicionPintura.Single(x => x.RequisicionPinturaID == rpd.RequisicionPinturaID).FechaRequisicion;

                return fechaReqPintura.ToString("MM/dd/yyyy");
            }
        }
       
        public string ObtenerFechaReqPinturaPorOtsID(string reqIds) 
        { 
            int[] requisiciones = reqIds.Split(',').Select(x => x.SafeIntParse()).ToArray();  
 
            using (SamContext ctx = new SamContext()) 
            { 
                string fechasReqPintura = string.Empty;   
 
                foreach (int reqId in requisiciones) 
                { 
                    RequisicionPinturaDetalle rpd = null;                     
 
                    if (reqId != -1) 
                    { 
                        rpd = ctx.RequisicionPinturaDetalle.Single(x => x.RequisicionPinturaDetalleID == reqId);  
 
                        DateTime fechaReqPintura = ctx.RequisicionPintura.Single(x => x.RequisicionPinturaID == rpd.RequisicionPinturaID).FechaRequisicion;
 
                        if (fechaReqPintura != null) 
                        { 
                            fechasReqPintura += fechaReqPintura.ToString("MM/dd/yyyy") + ","; 
                        } 
                        else 
                        { 
                            fechasReqPintura += ","; 
                        } 
                    } 
                    else 
                    { 
                        fechasReqPintura += ","; 
                    }                     
                }   

                return fechasReqPintura.ToString().Substring( 0, fechasReqPintura.Length - 1);  
            } 
        }
        
        public string ObtenerFechasDimensionalesPorOts(int[] ordenestrabajospool)
        {
            using (SamContext ctx = new SamContext())
            {
                string fechas = string.Empty;
                foreach (int id in ordenestrabajospool)
                {
                    WorkstatusSpool wks = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID == id).FirstOrDefault();
                    if (wks != null)
                    {
                        IQueryable<ReporteDimensionalDetalle> rdd = ctx.ReporteDimensionalDetalle.Where(x => x.WorkstatusSpoolID == wks.WorkstatusSpoolID && x.Aprobado == true).OrderByDescending(x => x.FechaLiberacion);
                        if (rdd.Count() > 0)
                        {
                            DateTime fechaLiberacion = rdd.Select(x => x.FechaLiberacion.Value).First();

                            fechas += fechaLiberacion.ToString("MM/dd/yyyy") + ",";
                        }
                        else
                        {
                            fechas += string.Empty + ",";
                        }

                    }
                    else
                    {
                        fechas += string.Empty + ",";
                    }

                }
                return string.IsNullOrEmpty(fechas) ? fechas : fechas.Substring(0, fechas.Length - 1);
            }
        }
              
        #endregion

        public void ValidaFechaReportePND(DateTime fechaSoldadura, int jwsID)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntaReportePnd jrpnd = ctx.JuntaReportePnd.Where(x => x.JuntaWorkstatusID == jwsID).FirstOrDefault();
                  
                if(jrpnd != null)
                {                      
                    if (fechaSoldadura < jrpnd.FechaPrueba) 
                    {
                        throw new ExcepcionFechas(string.Format(MensajesError.Excepcion_FechaSoldaduraMenorFechaReportePND, jrpnd.FechaPrueba.ToShortDateString()));
                    }                      
                }
            }
        } 
        
        public bool ValidaAnioFecha(DateTime? fecha)
        {
            if(fecha.HasValue)
            {
                int anioMinPermitido = DateTime.Now.Year - 1;
                int anioMaxPermitido = DateTime.Now.Year + 1;
                int anioFecha = fecha.Value.Year;

                DateTime fechaMax = new DateTime(anioMaxPermitido, 12,31);
                DateTime fechaMin = new DateTime(anioMinPermitido, 1,1);

                if (anioFecha > anioMaxPermitido || anioFecha < anioMinPermitido)
                {              
                     throw new ExcepcionFechas(string.Format(MensajesError.Exception_AnioFecha, fechaMin.ToShortDateString(), fechaMax.ToShortDateString()));
                }
            }
            return true;
        }
  
        public bool ValidaFechaPintura(DateTime fechaReq, int wss)  
        {              
              using (SamContext ctx = new SamContext())  
              { 
                  PinturaSpool ps = ctx.PinturaSpool.Where(x => x.WorkstatusSpoolID == wss).FirstOrDefault(); 
                  if (ps != null) 
                  { 
                      if (ps.FechaSandblast.HasValue) 
                      { 
                          if (fechaReq > ps.FechaSandblast) 
                          { 
                              return false; 
                          } 
                      }  
 
                      if (ps.FechaPrimarios.HasValue) 
                      { 
                          if (fechaReq > ps.FechaPrimarios) 
                          { 
                              return false; 
                          } 
                      }  
 
                      if (ps.FechaIntermedios.HasValue) 
                      { 
                          if (fechaReq > ps.FechaIntermedios) 
                          { 
                              return false; 
                          } 
                      }  
 
                      if (ps.FechaAdherencia.HasValue) 
                      { 
                          if (fechaReq > ps.FechaAdherencia) 
                          { 
                              return false; 
                          } 
                      }  
 
                      if (ps.FechaAcabadoVisual.HasValue) 
                      { 
                          if (fechaReq > ps.FechaAcabadoVisual) 
                          { 
                              return false; 
                          } 
                      }  
 
                      if (ps.FechaPullOff.HasValue) 
                      { 
                          if (fechaReq > ps.FechaPullOff) 
                          { 
                              return false; 
                          } 
                      } 
                  } 
                  return true; 
             }    
          }  
    }    
}
