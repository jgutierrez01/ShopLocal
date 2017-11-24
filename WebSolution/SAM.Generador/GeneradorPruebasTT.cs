using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Grid;
using SAM.Entities;
using SAM.Entities.Cache;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Proyectos;
using System.Diagnostics;
using SAM.BusinessLogic.Workstatus;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Workstatus;

namespace SAM.Generador
{
    public class GeneradorPruebasTT
    {
        private Guid _userID;
        private List<long> _lstTiemposAprobaciones;
        private List<long> _lstTiemposRechazos;

        public GeneradorPruebasTT()
        {
            _userID = new Guid("D6A113B4-464E-496F-B15D-4456CB0AE55B");
            _lstTiemposAprobaciones = new List<long>();
            _lstTiemposRechazos = new List<long>();
        }

        public void Inicia()
        {
            int proyectoID = 0;
            int spoolsAProbar = 0;
            int minSpoolsPorReporte = 0;
            int maxSpoolsPorReporte = 0;
            int alternanciaRechazos = 0;
            int porcentajeDeJuntasConRechazoPorRequisicion = 0;
            string prefijoReporte = string.Empty;
            int inicialConsecutivoReporte = 0;
            int spoolsProcesados = 0;
            int tipoPrueba = 0;

            KeyboardUtils.ImprimeInline("ID proyecto: ");
            proyectoID = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Cantidad de spools a probar: ");
            spoolsAProbar = KeyboardUtils.LeeEntero();

            tipoPrueba = (int)SolicitaTipoPrueba();

            KeyboardUtils.ImprimeInline("Mínimo de spools por prueba: ");
            minSpoolsPorReporte = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Máximo de spools por prueba: ");
            maxSpoolsPorReporte = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Alternancia rechazos (0 = ninguna, 1 = todas, 2 = 50%, 3 = 33%... aprox): ");
            alternanciaRechazos = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Porcentaje de juntas con rechazo por prueba: ");
            porcentajeDeJuntasConRechazoPorRequisicion = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Prefijo reporte de prueba: ");
            prefijoReporte = KeyboardUtils.LeeCadena();

            KeyboardUtils.ImprimeInline("Inicial consecutivo reporte de prueba: ");
            inicialConsecutivoReporte = KeyboardUtils.LeeEntero();

            Proyecto proyecto = ProyectoBO.Instance.Obtener(proyectoID);

            Random rnd = new Random(DateTime.Now.Millisecond);

            List<Defecto> defectos = DefectoBO.Instance.ObtenerTodosConTipoPrueba();

            List<GrdRequisiciones> lstJuntas = ObtenerJuntasTT(proyecto.ProyectoID, tipoPrueba).ToList();

            List<string> lstSpools = lstJuntas.Select(x => x.NumeroControl).Distinct().OrderBy(x => rnd.NextDouble()).ToList();

            Console.WriteLine("Comenzando pruebas para el proyecto {0}", proyecto.Nombre);
            Console.WriteLine();

            int numSpoolsEnPruebas = 0;

            Stopwatch sw = new Stopwatch();
            DateTime hoy = DateTime.Now.Date;

            while(true)
            {
                numSpoolsEnPruebas = rnd.Next(minSpoolsPorReporte, maxSpoolsPorReporte);

                var spoolsParaReporte =
                    lstSpools.Skip(spoolsProcesados)
                             .Take(numSpoolsEnPruebas)
                             .ToList();

                spoolsProcesados += numSpoolsEnPruebas;

                if (spoolsParaReporte.Count < 1)
                {
                    break;
                }

                //Traer x porcentaje de juntas para rechazos
                List<int> lstJwIdParaRechazo = new List<int>();

                if (alternanciaRechazos > 0 && spoolsProcesados % alternanciaRechazos == 0)
                {
                    if (porcentajeDeJuntasConRechazoPorRequisicion > 0 && porcentajeDeJuntasConRechazoPorRequisicion <= 100)
                    {

                        var juntasSpool = (from juntas in lstJuntas
                                           where spoolsParaReporte.Contains(juntas.NumeroControl)
                                           select juntas).ToList();

                        int cuantas = (int)Math.Ceiling(juntasSpool.Count * porcentajeDeJuntasConRechazoPorRequisicion / 100.0f);

                        lstJwIdParaRechazo.AddRange(
                            juntasSpool.Select(x => x.JuntaWorkstatusID)
                                       .OrderBy(x => rnd.NextDouble())
                                       .Take(cuantas)
                        );
                    }
                }

                List<int> lstJwIdParaAprobacion = lstJuntas.Where(x => spoolsParaReporte.Contains(x.NumeroControl))
                                                           .Select(x => x.JuntaWorkstatusID)
                                                           .Except(lstJwIdParaRechazo)
                                                           .ToList();

                string jwIdsParaAprobacion = string.Join(",", lstJwIdParaAprobacion.Select(x => x.ToString()));

                string rIdsParaAprobacion = string.Join(",", lstJuntas.Where(x => lstJwIdParaAprobacion.Contains(x.JuntaWorkstatusID))
                                                                      .Select(x => x.RequisicionID));

                string numReporte = prefijoReporte + "-" + (inicialConsecutivoReporte++).ToString().PadLeft(4, '0');

                if (lstJwIdParaAprobacion.Count > 0)
                {
                    GenerarAprobacion(tipoPrueba, proyecto, sw, hoy, jwIdsParaAprobacion, rIdsParaAprobacion, numReporte);
                }

                if (lstJwIdParaRechazo.Count > 0)
                {
                    foreach (int jwid in lstJwIdParaRechazo)
                    {
                        GenerarRechazo( tipoPrueba,
                                        proyecto,
                                        sw,
                                        hoy,
                                        jwid.ToString(),
                                        lstJuntas.Where(x => x.JuntaWorkstatusID == jwid).Select(x => x.RequisicionID.ToString()).Single(),
                                        numReporte);
                    }
                }


                if (spoolsProcesados >= spoolsAProbar)
                {
                    break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Ejecución finalizada");
            Console.WriteLine("Se realizaron {0} aprobaciones de pruebas en {1} ms en promedio.", _lstTiemposAprobaciones.Count, _lstTiemposAprobaciones.Average());
            Console.WriteLine("Se realizaron {0} rechazos de pruebas en {1} ms en promedio.", _lstTiemposRechazos.Count, _lstTiemposRechazos.Average());
            Console.ReadLine();
        }

        private void GenerarAprobacion(int tipoPrueba, Proyecto proyecto, Stopwatch sw, DateTime hoy, string jwIdsParaAprobacion, string rIdsParaAprobacion, string numReporte)
        {
            ReporteTt reporte = new ReporteTt
            {
                ProyectoID = proyecto.ProyectoID,
                TipoPruebaID = tipoPrueba,
                NumeroReporte = numReporte,
                FechaReporte = hoy
            };

            JuntaReporteTt juntaReporte = new JuntaReporteTt
            {
                NumeroGrafica = "GR-001",
                FechaTratamiento = hoy,
                Aprobado = true,
                Observaciones = "Generado automáticamente"
            };



            try
            {
                sw.Restart();

                ReporteTtBO.Instance.GuardaReporteTt(reporte, juntaReporte, jwIdsParaAprobacion, rIdsParaAprobacion, _userID);

                sw.Stop();

                _lstTiemposAprobaciones.Add(sw.ElapsedMilliseconds);
                Console.WriteLine("Reporte con juntas aprobadas = {0}, en el reporte {1} en {2} ms.", jwIdsParaAprobacion, numReporte, sw.ElapsedMilliseconds);
            }
            catch (BaseValidationException bve)
            {
                Console.Error.WriteLine("Error al generar reporte con juntas aprobadas = {0}, con el reporte {1}.", jwIdsParaAprobacion, numReporte);
                bve.Details.ForEach(Console.Error.WriteLine);
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error al generar reporte con juntas aprobadas = {0}, con el reporte {1}.", jwIdsParaAprobacion, numReporte);
                Console.WriteLine(ex);
                Console.WriteLine();
            }
        }


        private void GenerarRechazo(int tipoPrueba, Proyecto proyecto, Stopwatch sw, DateTime hoy, string jwIdsParaRechazo, string rIdsParaRechazo, string numReporte)
        {
            ReporteTt reporte = new ReporteTt
            {
                ProyectoID = proyecto.ProyectoID,
                TipoPruebaID = tipoPrueba,
                NumeroReporte = numReporte,
                FechaReporte = hoy
            };

            JuntaReporteTt juntaReporte = new JuntaReporteTt
            {
                NumeroGrafica = "GR-001",
                FechaTratamiento = hoy,
                Aprobado = false,
                Observaciones = "Generado automáticamente"
            };

            try
            {
                sw.Restart();

                ReporteTtBO.Instance.GuardaReporteTt(reporte, juntaReporte, jwIdsParaRechazo, rIdsParaRechazo, _userID);

                sw.Stop();

                _lstTiemposRechazos.Add(sw.ElapsedMilliseconds);
                Console.WriteLine("Reporte con juntas rechazadas = {0}, en el reporte {1} en {2} ms.", jwIdsParaRechazo, numReporte, sw.ElapsedMilliseconds);
            }
            catch (BaseValidationException bve)
            {
                Console.Error.WriteLine("Error al generar reporte con juntas rechazadas = {0}, con el reporte {1}.", jwIdsParaRechazo, numReporte);
                bve.Details.ForEach(Console.Error.WriteLine);
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error al generar reporte con juntas rechazadas = {0}, con el reporte {1}.", jwIdsParaRechazo, numReporte);
                Console.WriteLine(ex);
                Console.WriteLine();
            }
        }

        #region Helpers

        private static TipoPruebaEnum SolicitaTipoPrueba()
        {
            while (true)
            {
                KeyboardUtils.ImprimeInline("Tipo de prueba? (Durezas, Preheat, PWHT): ");
                string linea = Console.ReadLine().Trim().ToLowerInvariant();

                switch (linea)
                {
                    case "durezas":
                        return TipoPruebaEnum.Durezas;
                    case "preheat":
                        return TipoPruebaEnum.Preheat;
                    case "pwht":
                        return TipoPruebaEnum.Pwht;
                    default:
                        Console.WriteLine("Opción inválida");
                        break;
                }
            }
        }

        private List<GrdRequisiciones> ObtenerJuntasTT(int proyectoID, int tipoPruebaID)
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
                IQueryable<JuntaWorkstatus> juntas = ctx.JuntaWorkstatus.Where(x => x.OrdenTrabajoSpool.OrdenTrabajo.ProyectoID == proyectoID && x.JuntaFinal);

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

        #endregion
    }
}
