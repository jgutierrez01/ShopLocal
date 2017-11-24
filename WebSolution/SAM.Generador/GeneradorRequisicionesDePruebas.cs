using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities;
using SAM.BusinessObjects.Proyectos;
using System.Diagnostics;
using SAM.BusinessObjects.Workstatus;
using Mimo.Framework.Exceptions;

namespace SAM.Generador
{
    public class GeneradorRequisicionesDePruebas
    {
        private Guid _userID;
        private List<long> _lstTiemposRequisicion;

        public GeneradorRequisicionesDePruebas()
        {
            _userID = new Guid("D6A113B4-464E-496F-B15D-4456CB0AE55B");
            _lstTiemposRequisicion = new List<long>();
        }

        public void Inicia()
        {
            int proyectoID = 0;
            int spoolsAInspeccionar = 0;
            int minSpoolsPorReporte = 0;
            int maxSpoolsPorReporte = 0;
            int porcentajeDeJuntasARequisitarPorSpool = 0;
            string prefijoRequisicion = string.Empty;
            int inicialConsecutivoReporte = 0;
            int spoolsProcesados = 0;
            int tipoRequisicion = 0;

            KeyboardUtils.ImprimeInline("ID proyecto: ");
            proyectoID = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Cantidad de spools a requisitar: ");
            spoolsAInspeccionar = KeyboardUtils.LeeEntero();

            tipoRequisicion = (int)SolicitaTipoRequisicion();

            KeyboardUtils.ImprimeInline("Mínimo de spools por requisición: ");
            minSpoolsPorReporte = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Máximo de spools por requisición: ");
            maxSpoolsPorReporte = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Porcentaje de juntas por spool: ");
            porcentajeDeJuntasARequisitarPorSpool = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Prefijo requisición: ");
            prefijoRequisicion = KeyboardUtils.LeeCadena();

            KeyboardUtils.ImprimeInline("Inicial consecutivo requisición: ");
            inicialConsecutivoReporte = KeyboardUtils.LeeEntero();

            Proyecto proyecto = ProyectoBO.Instance.Obtener(proyectoID);

            Random rnd = new Random(DateTime.Now.Millisecond);

            List<GrdRequisicionPruebas> lstJuntas =
                ObtenerJuntas(proyecto.ProyectoID, tipoRequisicion).Where(x => x.Armado && x.Soldadura && x.InspeccionVisual && x.InspeccionDimensional).ToList();

            List<string> lstSpools = lstJuntas.Select(x => x.NumeroControl).Distinct().OrderBy(x => rnd.NextDouble()).ToList();

            Console.WriteLine("Comenzando requisiciones para el proyecto {0}", proyecto.Nombre);
            Console.WriteLine();

            int numSpoolsEnRequisicion = 0;

            Stopwatch sw = new Stopwatch();
            DateTime hoy = DateTime.Now.Date;

            while(true)
            {
                numSpoolsEnRequisicion = rnd.Next(minSpoolsPorReporte, maxSpoolsPorReporte);

                var spoolsParaReporte =
                    lstSpools.Skip(spoolsProcesados)
                             .Take(numSpoolsEnRequisicion)
                             .ToList();

                spoolsProcesados += numSpoolsEnRequisicion;

                if (spoolsParaReporte.Count < 1)
                {
                    break;
                }

                //Traer x porcentaje de juntas
                List<int> lstJwId = new List<int>();
                
                spoolsParaReporte.ForEach( sp =>
                {
                    var juntasSpool = (from juntas in lstJuntas
                                       where juntas.NumeroControl == sp
                                       select juntas).ToList();

                    int cuantas = (int)Math.Ceiling(juntasSpool.Count * porcentajeDeJuntasARequisitarPorSpool / 100.0f);

                    lstJwId.AddRange(
                        juntasSpool.Select(x => x.JuntaWorkstatusID)
                                   .OrderBy(x => rnd.NextDouble())
                                   .Take(cuantas)
                    );

                });

                string spIdsCsv = string.Join(",", lstJwId.Select(x => x.ToString()));
                string numRequisicion = prefijoRequisicion + "-" + (inicialConsecutivoReporte++).ToString().PadLeft(4, '0');

                Requisicion requisicion = new Requisicion
                {
                    ProyectoID = proyecto.ProyectoID,
                    TipoPruebaID = tipoRequisicion,
                    FechaRequisicion = hoy,
                    NumeroRequisicion = numRequisicion,
                    CodigoAsme = "ASME",
                    Observaciones = "Generada automáticamente"
                };

                try
                {
                    sw.Restart();

                    RequisicionPruebasBO.Instance.GeneraRequisicion(requisicion, spIdsCsv, _userID);

                    sw.Stop();
                    
                    _lstTiemposRequisicion.Add(sw.ElapsedMilliseconds);
                    Console.WriteLine("Juntas requisitadas = {0}, en el reporte {1} en {2} ms.", spIdsCsv, numRequisicion, sw.ElapsedMilliseconds);
                }
                catch (BaseValidationException bve)
                {
                    Console.Error.WriteLine("Error al intentar requisitas las juntas = {0}, con el reporte {1}.", spIdsCsv, numRequisicion);
                    bve.Details.ForEach(Console.Error.WriteLine);
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error al intentar requisitas las juntas = {0}, con el reporte {1}.", spIdsCsv, numRequisicion);
                    Console.WriteLine(ex);
                    Console.WriteLine();
                }


                if (spoolsProcesados >= spoolsAInspeccionar)
                {
                    break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Ejecución finalizada");
            Console.WriteLine("Se realizaron {0} requisiciones en {1} ms en promedio.", _lstTiemposRequisicion.Count, _lstTiemposRequisicion.Average());
            Console.ReadLine();
        }

        #region Helpers

        private static TipoPruebaEnum SolicitaTipoRequisicion()
        {
            while (true)
            {
                KeyboardUtils.ImprimeInline("Tipo de requisicion? (Durezas, Preheat, PT-Post-TT, PWHT, PT, RT, UT, RT-Post-TT): ");
                string linea = Console.ReadLine().Trim().ToLowerInvariant();

                switch (linea)
                {
                    case "durezas":
                        return TipoPruebaEnum.Durezas;
                    case "preheat":
                        return TipoPruebaEnum.Preheat;
                    case "pt-post-tt":
                        return TipoPruebaEnum.PTPostTT;
                    case "pwht":
                        return TipoPruebaEnum.Pwht;
                    case "pt":
                        return TipoPruebaEnum.ReportePT;
                    case "rt":
                        return TipoPruebaEnum.ReporteRT;
                    case "ut":
                        return TipoPruebaEnum.ReporteUT;
                    case "rt-post-tt":
                        return TipoPruebaEnum.RTPostTT;
                    default:
                        Console.WriteLine("Opción inválida");
                        break;
                }
            }
        }

        public List<GrdRequisicionPruebas> ObtenerJuntas(int proyectoID, int tipoPruebaID)
        {
            using (SamContext ctx = new SamContext())
            {
                List<GrdRequisicionPruebas> lista = new List<GrdRequisicionPruebas>();
                string categoriaPrueba = CacheCatalogos.Instance.ObtenerTiposPrueba().Where(x => x.ID == tipoPruebaID).Select(x => x.Categoria).Single();

                //Obtengo los IDs de las juntas que existen en la tabla JuntaWorkstatus que son parte de la orden de trabajo
                //y del numero de control (en caso de especificarse)
                //Donde la junta es JuntaFinal
                IQueryable<JuntaWorkstatus> juntas = ctx.JuntaWorkstatus.Where(x => x.OrdenTrabajoSpool.OrdenTrabajo.ProyectoID == proyectoID && x.JuntaFinal);

                //Obtengo las requisiciones de las juntas que caen en los filtros
                IQueryable<JuntaRequisicion> juntasConRequisicion = (from jtaReq in ctx.JuntaRequisicion
                                                                     join jtaWks in juntas on jtaReq.JuntaWorkstatusID equals jtaWks.JuntaWorkstatusID
                                                                     where jtaReq.Requisicion.TipoPruebaID == tipoPruebaID
                                                                     select jtaReq);

                //Si el tipo de reporte es Categoria TT, verifico que exista el rechazo para incluirla en el listado. De lo contrario no se debe mostrar.
                if (categoriaPrueba == CategoriaTipoPrueba.TT)
                {
                    IQueryable<int> juntasConRechazo = from rechazo in ctx.JuntaReporteTt
                                                       join jtaReq in juntasConRequisicion on rechazo.JuntaRequisicionID equals jtaReq.JuntaRequisicionID
                                                       where !rechazo.Aprobado
                                                       select jtaReq.JuntaWorkstatusID;

                    //Filtro las juntas con requisición para que me deje sólo aquellas que no tienen rechazo.
                    juntasConRequisicion = juntasConRequisicion.Where(x => !juntasConRechazo.Contains(x.JuntaWorkstatusID));
                }


                //Obtengo las juntas que son parte de los filtros y que no tienen requisicion.
                IQueryable<int> juntasIDs = from jta in juntas
                                            where !juntasConRequisicion.Select(x => x.JuntaWorkstatusID).Contains(jta.JuntaWorkstatusID)
                                            select jta.JuntaSpoolID;

                //Verifico que si el tipo de prueba es Post-TT o Post-TT exista previamente un resultado positivo de PWHT
                if (tipoPruebaID == (int)TipoPruebaEnum.RTPostTT || tipoPruebaID == (int)TipoPruebaEnum.PTPostTT)
                {
                    IQueryable<int> juntaConPWHT = from pwht in ctx.JuntaReporteTt
                                                   join jta in juntas on pwht.JuntaWorkstatusID equals jta.JuntaWorkstatusID
                                                   join tt in ctx.ReporteTt on pwht.ReporteTtID equals tt.ReporteTtID
                                                   where pwht.Aprobado
                                                   select jta.JuntaSpoolID;

                    juntasIDs = juntasIDs.Where(x => juntaConPWHT.Contains(x));
                }

                //Obtengo los IDS de OrdenTrabajoSpool que sean parte de la orden de trabajo y que ya tengan liberacion dimensional
                IQueryable<int> spoolsInspDimensional = ctx.WorkstatusSpool
                                                           .Where(x => x.OrdenTrabajoSpool.OrdenTrabajo.ProyectoID == proyectoID && x.TieneLiberacionDimensional)
                                                           .Select(x => x.OrdenTrabajoSpoolID);

                //Obtengo los registros de las juntas obtenidas en el paso anterior y que además son parte de un spool con liberacion dimensional
                IQueryable<JuntaSpool> query = ctx.JuntaSpool.Where(x => juntasIDs.Contains(x.JuntaSpoolID)
                                                                    && spoolsInspDimensional.Contains(x.OrdenTrabajoJunta.Select(y => y.OrdenTrabajoSpoolID).FirstOrDefault())).AsQueryable();


                lista = (from js in query
                         join s in ctx.Spool on js.SpoolID equals s.SpoolID
                         join otj in ctx.OrdenTrabajoJunta on js.JuntaSpoolID equals otj.JuntaSpoolID
                         join ots in ctx.OrdenTrabajoSpool on otj.OrdenTrabajoSpoolID equals ots.OrdenTrabajoSpoolID
                         join ot in ctx.OrdenTrabajo on ots.OrdenTrabajoID equals ot.OrdenTrabajoID
                         join jw in ctx.JuntaWorkstatus on js.JuntaSpoolID equals jw.JuntaSpoolID
                         join sh in ctx.SpoolHold on ots.SpoolID equals sh.SpoolID into Holds
                         from hld in Holds.DefaultIfEmpty()
                         where jw.JuntaFinal
                         select new GrdRequisicionPruebas
                         {
                             JuntaWorkstatusID = jw.JuntaWorkstatusID,
                             NombreSpool = s.Nombre,
                             JuntaSpoolID = js.JuntaSpoolID,
                             OrdenTrabajo = ot.NumeroOrden,
                             NumeroControl = ots.NumeroControl,
                             EtiquetaJunta = jw.EtiquetaJunta,
                             EtiquetaMaterial1 = js.EtiquetaMaterial1,
                             EtiquetaMaterial2 = js.EtiquetaMaterial2,
                             TipoJuntaID = js.TipoJuntaID,
                             TipoJunta = js.TipoJunta.Codigo,
                             Cedula = js.Cedula,
                             FamiliaAceroMaterial1 = js.FamiliaAcero.Nombre,
                             FamiliaAceroMaterial2 = js.FamiliaAcero1.Nombre,
                             Diametro = js.Diametro,
                             Armado = jw.ArmadoAprobado,
                             Soldadura = jw.SoldaduraAprobada,
                             InspeccionVisual = jw.InspeccionVisualAprobada,
                             InspeccionDimensional = true,
                             Hold = (hld == null) ? false : hld.TieneHoldIngenieria || hld.TieneHoldCalidad || hld.Confinado
                         }).ToList();


                return lista;
            }
        }

        #endregion

    }
}
