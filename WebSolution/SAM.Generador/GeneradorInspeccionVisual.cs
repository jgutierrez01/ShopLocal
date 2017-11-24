using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Proyectos;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Workstatus;
using System.Diagnostics;
using Mimo.Framework.Exceptions;

namespace SAM.Generador
{
    public class GeneradorInspeccionVisual
    {
        private Guid _userID;
        private List<long> _lstTiemposInspeccionVisual;

        public GeneradorInspeccionVisual()
        {
            _userID = new Guid("D6A113B4-464E-496F-B15D-4456CB0AE55B");
            _lstTiemposInspeccionVisual = new List<long>();
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

            KeyboardUtils.ImprimeInline("ID proyecto: ");
            proyectoID = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Cantidad de spools a inspeccionar: ");
            spoolsAInspeccionar = KeyboardUtils.LeeEntero();

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

            List<JuntaWorkstatus> _lstWs;

            using (SamContext ctx = new SamContext())
            {
                ctx.JuntaWorkstatus.MergeOption = System.Data.Objects.MergeOption.NoTracking;

                _lstWs =
                    ctx.JuntaWorkstatus
                       .Include("JuntaSpool")
                       .Include("Soldadura")
                       .Where(x => x.JuntaSpool.Spool.ProyectoID == proyectoID && x.SoldaduraAprobada && x.ArmadoAprobado && !x.InspeccionVisualAprobada && x.JuntaFinal)
                       .ToList();
            }

            var lstSpools = (from jw in _lstWs
                             group jw by jw.JuntaSpool.SpoolID into spools
                             select new
                             {
                                 SpoolID = spools.Key,
                                 FechaMaximaSoldadura = spools.Max(sp => sp.Soldadura.FechaReporte)
                             })
                             .OrderBy(x => x.FechaMaximaSoldadura)
                             .ThenBy(x => x.SpoolID)
                             .ToList();

            Console.WriteLine("Comenzando inspecciones visuales para el proyecto {0}", proyecto.Nombre);
            Console.WriteLine();

            int numSpoolsEnReporte = 0;
            int[] defectos = new int[0];

            Stopwatch sw = new Stopwatch();

            while(true)
            {
                numSpoolsEnReporte = rnd.Next(minSpoolsPorReporte, maxSpoolsPorReporte);
                spoolsProcesados += numSpoolsEnReporte;

                var spoolsParaReporte =
                    lstSpools.Skip(spoolsProcesados)
                             .Take(numSpoolsEnReporte)
                             .ToList();

                if (spoolsParaReporte.Count < 1)
                {
                    break;
                }

                string [] jwIds =
                    _lstWs.Where( x => spoolsParaReporte.Select(sp => sp.SpoolID)
                                                        .Contains(x.JuntaSpool.SpoolID))
                          .Select(y => y.JuntaWorkstatusID.ToString())
                          .ToArray();

                string jwIdsCsv = string.Join(",",jwIds);
                DateTime fechaInspeccion = spoolsParaReporte.Max(x => x.FechaMaximaSoldadura).AddDays(1);

                string numReporte = prefijoReporte + "-" + (inicialConsecutivoReporte++).ToString().PadLeft(4,'0');

                InspeccionVisual inspVisual = new InspeccionVisual
                {
                    ProyectoID = proyecto.ProyectoID,
                    NumeroReporte = numReporte,
                    FechaReporte = fechaInspeccion
                };

                JuntaInspeccionVisual junta = new JuntaInspeccionVisual
                {
                    FechaInspeccion = inspVisual.FechaReporte,
                    Aprobado = true,
                    Observaciones = "Generadas automáticamente"
                };

                try
                {
                    sw.Restart();

                    InspeccionVisualBO.Instance.GeneraReporte(  junta,
                                                                inspVisual,
                                                                defectos,
                                                                jwIdsCsv,
                                                                _userID);
                    sw.Stop();
                    
                    _lstTiemposInspeccionVisual.Add(sw.ElapsedMilliseconds);
                    Console.WriteLine("Juntas insepccionadas = {0}, en el reporte {1} en {2} ms.", jwIdsCsv, numReporte, sw.ElapsedMilliseconds);
                }
                catch (BaseValidationException bve)
                {
                    Console.Error.WriteLine("Error al intentar inspeccionar las juntas = {0}, con el reporte {1}.", jwIdsCsv, numReporte);
                    bve.Details.ForEach(Console.Error.WriteLine);
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error al intentar inspeccionar las juntas = {0}, con el reporte {1}.", jwIdsCsv, numReporte);
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
            Console.WriteLine("Se realizaron {0} inspecciones en {1} ms en promedio.", _lstTiemposInspeccionVisual.Count, _lstTiemposInspeccionVisual.Average());
            Console.ReadLine();
        }
    }
}
