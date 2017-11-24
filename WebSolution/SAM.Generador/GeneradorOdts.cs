using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Proyectos;
using SAM.Entities;
using SAM.BusinessLogic.Cruce;
using SAM.Entities.Personalizadas;
using SAM.Entities.Reportes;
using SAM.BusinessLogic.Produccion;
using Mimo.Framework.Exceptions;
using System.Diagnostics;

namespace SAM.Generador
{
    public class GeneradorOdts
    {
        public void Inicia()
        {
            Stopwatch sw = new Stopwatch();
            int proyectoInicial;
            int proyectoFinal;
            int porcentajeOdts;
            int mediaSpools;
            int desviacionSpools;
            Guid userId = new Guid("D6A113B4-464E-496F-B15D-4456CB0AE55B");

            KeyboardUtils.ImprimeInline("ID proyecto inicial: ");
            proyectoInicial = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("ID proyecto final: ");
            proyectoFinal = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("% Odts: ");
            porcentajeOdts = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Cantidad de spools promedio por ODT: ");
            mediaSpools = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Desviación cant. de spools: ");
            desviacionSpools = KeyboardUtils.LeeEntero();


            for (int i = proyectoInicial; i <= proyectoFinal; i++)
            {
                Proyecto proyecto = ProyectoBO.Instance.Obtener(i);
                List<Taller> talleres = ProyectoBO.Instance.ObtenerTallers(i);

                if (talleres.Count < 1)
                {
                    Console.WriteLine("No existen talleres para el proyecto: " + i);
                    continue;
                }

                CruceProyecto cruce = new CruceProyecto(proyecto.ProyectoID);
                
                List<FaltanteCruce> faltantes = null;
                List<CondensadoItemCode> condensado = null;
                List<string> isometricosCompletos = new List<string>();

                sw.Start();
                List<SpoolCruce> spools = cruce.Procesa(out faltantes, out condensado, false, false);
                sw.Stop();
                Console.WriteLine("Cruce para el proyecto {0}: {1} ms.", proyecto.Nombre, sw.ElapsedMilliseconds);

                if (spools.Count > 0)
                {
                    int cuantos = (int)(spools.Count * porcentajeOdts / 100.0f);
                    
                    Random rnd = new Random();

                    spools = (from s in spools
                              orderby rnd.Next()
                              select s).ToList();

                    int numeroOdts = cuantos / mediaSpools;

                    for (int j = 0; j < numeroOdts; j++)
                    {
                        int consecutivo = ProyectoBO.Instance.SiguienteConsecutivoOdt(proyecto.ProyectoID);
                        
                        int [] spoolIds = spools.Skip(j * mediaSpools)
                                                .Take(mediaSpools)
                                                .Select(x => x.SpoolID).ToArray();

                        try
                        {
                            sw.Restart();
                            OrdenTrabajo odt =
                            OrdenTrabajoBL.Instance.GeneraNueva(proyecto.ProyectoID,
                                                                spoolIds,
                                                                talleres[0].TallerID,
                                                                consecutivo,
                                                                DateTime.Now,
                                                                userId,
                                                                false,
                                                                string.Empty,
                                                                false);
                            sw.Stop();
                            Console.WriteLine("Orden de trabajo {0} generada para el proyecto {1} en {2} ms.", odt.NumeroOrden, proyecto.Nombre, sw.ElapsedMilliseconds);
                        }
                        catch (BaseValidationException bve)
                        {
                            Console.WriteLine();
                            bve.Details.ForEach(Console.WriteLine);
                            Console.WriteLine();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine();
                            Console.Error.WriteLine(e);
                            Console.WriteLine();
                        }
                    }
                }
            }
        }


        //private static int[] generaIndicesAleatorios(int cantidadMediaAGenerar, int desviacion, int indiceMaximo)
        //{
        //    int[] randoms = new int[cantidadMediaAGenerar];
        //    Random rnd = new Random(DateTime.Now.Millisecond);

        //    for (int i = 0; i < cantidadMediaAGenerar; i++)
        //    {
        //        randoms[i] = rnd.Next(0, indiceMaximo);
        //    }

        //    return randoms.Distinct().ToArray();
        //}
    }
}
