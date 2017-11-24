using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.Entities.Cache;
using SAM.BusinessObjects.Proyectos;
using System.Diagnostics;
using SAM.BusinessObjects.Workstatus;
using Mimo.Framework.Exceptions;

namespace SAM.Generador
{
    public class GeneradorLiberacionDimensional
    {

        private Guid _userID;
        private List<long> _lstTiemposInspDim;

        public GeneradorLiberacionDimensional()
        {
            _userID = new Guid("D6A113B4-464E-496F-B15D-4456CB0AE55B");
            _lstTiemposInspDim = new List<long>();
        }

        public void Inicia()
        {
            int proyectoID = 0;
            int spoolsAInspeccionar = 0;
            int minSpoolsPorReporte = 0;
            int maxSpoolsPorReporte = 0;
            string prefijoReporte = string.Empty;
            int inicialConsecutivoReporte = 0;
            int spoolsProcesados = 0;
            int tipoReporte = 0;

            KeyboardUtils.ImprimeInline("ID proyecto: ");
            proyectoID = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Cantidad de spools a inspeccionar: ");
            spoolsAInspeccionar = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Tipo de reporte? (E=Espesores, D=Dimensional): ");
            string linea = Console.ReadLine().Trim().ToLowerInvariant();

            if (linea.Length <= 0)
            {
                return;
            }

            if (linea[0] == 'e')
            {
                tipoReporte = (int)TipoReporteDimensionalEnum.Espesores;
            }
            else if (linea[0] == 'd')
            {
                tipoReporte = (int)TipoReporteDimensionalEnum.Dimensional;
            }
            else
            {
                return;
            }

            KeyboardUtils.ImprimeInline("Mínimo de spools por inspección: ");
            minSpoolsPorReporte = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Máximo de spools por inspección: ");
            maxSpoolsPorReporte = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Prefijo reporte: ");
            prefijoReporte = KeyboardUtils.LeeCadena();

            KeyboardUtils.ImprimeInline("Inicial consecutivo reporte: ");
            inicialConsecutivoReporte = KeyboardUtils.LeeEntero();


            Proyecto proyecto = ProyectoBO.Instance.Obtener(proyectoID);

            Random rnd = new Random(DateTime.Now.Millisecond);

            List<GrdInspeccionDimensional> lstSpools =
                ObtenerSpools(proyecto.ProyectoID, tipoReporte).Where(x => x.Armado && x.Soldadura && x.InspeccionVisual)
                                                                                               .OrderBy(x => rnd.NextDouble())
                                                                                               .ToList();

            Console.WriteLine("Comenzando inspecciones dimensionales para el proyecto {0}", proyecto.Nombre);
            Console.WriteLine();

            int numSpoolsEnReporte = 0;
            int[] defectos = new int[0];

            Stopwatch sw = new Stopwatch();

            while(true)
            {
                numSpoolsEnReporte = rnd.Next(minSpoolsPorReporte, maxSpoolsPorReporte);

                var spoolsParaReporte =
                    lstSpools.Skip(spoolsProcesados)
                             .Take(numSpoolsEnReporte)
                             .ToList();

                spoolsProcesados += numSpoolsEnReporte;

                if (spoolsParaReporte.Count < 1)
                {
                    break;
                }

                DateTime? fechaMaxima;

                using (SamContext ctx = new SamContext())
                {
                    IQueryable<int> spoolIds = spoolsParaReporte.Select(x => x.SpoolID).AsQueryable();

                    fechaMaxima = (from iv in ctx.InspeccionVisual
                                   join jiv in ctx.JuntaInspeccionVisual on iv.InspeccionVisualID equals jiv.InspeccionVisualID
                                   join jws in ctx.JuntaWorkstatus on jiv.JuntaWorkstatusID equals jws.JuntaWorkstatusID
                                   join js in ctx.JuntaSpool on jws.JuntaSpoolID equals js.JuntaSpoolID
                                   where spoolIds.Contains(js.SpoolID)
                                   select iv.FechaReporte).Max();
                }

                if (fechaMaxima.HasValue)
                {
                    fechaMaxima = fechaMaxima.Value.AddDays(1);
                }
                else
                {
                    fechaMaxima = DateTime.Today.Date;
                }

                string spIdsCsv = string.Join(",", spoolsParaReporte.Select(x => x.SpoolID.ToString()));
                string numReporte = prefijoReporte + "-" + (inicialConsecutivoReporte++).ToString().PadLeft(4, '0');

                ReporteDimensional repDimensional = new ReporteDimensional
                {
                    ProyectoID = proyecto.ProyectoID,
                    NumeroReporte = numReporte,
                    FechaReporte = fechaMaxima.Value.Date,
                    TipoReporteDimensionalID = tipoReporte
                };

                ReporteDimensionalDetalle detalle = new ReporteDimensionalDetalle
                {
                    FechaLiberacion = fechaMaxima.Value.Date,
                    Aprobado = true,
                    Observaciones = "Generado por código"
                };

                try
                {
                    sw.Restart();

                    InspeccionDimensionalBO.Instance
                                           .GeneraReporte(  detalle, 
                                                            repDimensional,
                                                            spIdsCsv, 
                                                            _userID);

                    sw.Stop();
                    
                    _lstTiemposInspDim.Add(sw.ElapsedMilliseconds);
                    Console.WriteLine("Spools liberados = {0}, en el reporte {1} en {2} ms.", spIdsCsv, numReporte, sw.ElapsedMilliseconds);
                }
                catch (BaseValidationException bve)
                {
                    Console.Error.WriteLine("Error al intentar liberar dimensionalmente los spools = {0}, con el reporte {1}.", spIdsCsv, numReporte);
                    bve.Details.ForEach(Console.Error.WriteLine);
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error al intentar liberar dimensionalmente los spools = {0}, con el reporte {1}.", spIdsCsv, numReporte);
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
            Console.WriteLine("Se realizaron {0} liberaciones dimensionales en {1} ms en promedio.", _lstTiemposInspDim.Count, _lstTiemposInspDim.Average());
            Console.ReadLine();
        }


        #region Helpers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="tipoReporteID"></param>
        /// <returns></returns>
        private List<GrdInspeccionDimensional> ObtenerSpools(int proyectoID, int tipoReporteID)
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

                IQueryable<OrdenTrabajoSpool> query = ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajo.ProyectoID == proyectoID).AsQueryable();
                IQueryable<int> spoolsConLiberacionDimensional = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpool.OrdenTrabajo.ProyectoID == proyectoID && x.TieneLiberacionDimensional).Select(x => x.OrdenTrabajoSpoolID);
                IQueryable<int> spoolsConReporteEsperoresAprobado = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpool.OrdenTrabajo.ProyectoID == proyectoID
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
            List<JuntaWorkstatus> jwks = juntaWks.Where(y => y.OrdenTrabajoSpoolID == grd.OrdenTrabajoSpoolID).ToList();
            List<OrdenTrabajoJunta> otjQuery = otj.Where(x => x.OrdenTrabajoSpoolID == grd.OrdenTrabajoSpoolID).ToList();
            List<JuntaSpool> js = juntaSpool.Where(x => otjQuery.Select(y => y.JuntaSpoolID).Contains(x.JuntaSpoolID)).ToList();

            int numJuntas = js.Where(x => x.FabAreaID == fabAreaID).Count();
            int numSoldaduras = js.Where(x => x.FabAreaID == fabAreaID && x.TipoJuntaID != jTH && x.TipoJuntaID != jTW).Count();

            grd.Armado = jwks.Where(x => x.ArmadoAprobado).Count() == numJuntas;

            grd.Soldadura = jwks.Where(x => x.SoldaduraAprobada
                                            && js.Where(y => y.JuntaSpoolID == x.JuntaSpoolID).Select(y => y.TipoJuntaID).FirstOrDefault() != jTH
                                            && js.Where(y => y.JuntaSpoolID == x.JuntaSpoolID).Select(y => y.TipoJuntaID).FirstOrDefault() != jTW).Count()
                                        == numSoldaduras;

            grd.InspeccionVisual = jwks.Where(x => x.InspeccionVisualAprobada).Count() == numJuntas;
        }


        #endregion
    }
}
