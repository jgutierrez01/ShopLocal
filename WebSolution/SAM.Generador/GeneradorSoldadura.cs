using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using Mimo.Framework.Extensions;
using System.Data;
using SAM.BusinessObjects.Proyectos;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Modelo;
using SAM.Entities.Cache;
using SAM.BusinessObjects.Workstatus;
using System.Diagnostics;
using Mimo.Framework.Exceptions;

namespace SAM.Generador
{
    public class GeneradorSoldadura
    {
        private Guid _userID;
        private List<long> _lstTiemposArmado;
        List<Taller> _talleres;
        List<Soldador> _soldadores;
        List<Wps> _lstWps;
        List<ProcesoRaiz> _lstProcesoRaiz;
        List<ProcesoRelleno> _lstProcesoRelleno;
        List<Consumible> _consumibles;

        public GeneradorSoldadura()
        {
            _userID = new Guid("D6A113B4-464E-496F-B15D-4456CB0AE55B");
            _lstTiemposArmado = new List<long>();
        }

        public void Inicia()
        {
            int proyectoID = 0;
            int totalJuntas = 0;

            KeyboardUtils.ImprimeInline("ID proyecto: ");
            proyectoID = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Total de juntas a soldar: ");
            totalJuntas = KeyboardUtils.LeeEntero();

            Proyecto proyecto = ProyectoBO.Instance.Obtener(proyectoID);

            _talleres = ProyectoBO.Instance.ObtenerTallers(proyectoID);

            if (_talleres.Count < 1)
            {
                Console.WriteLine("No existen talleres para el proyecto: " + proyecto.Nombre);
                return;
            }

            _soldadores = SoldadorBO.Instance.ObtenerTodos().Where(x => x.PatioID == proyecto.PatioID).ToList();

            if (_soldadores.Count < 1)
            {
                Console.WriteLine("No existen soldadores para el proyecto: " + proyecto.Nombre);
                return;
            }

            using(SamContext ctx = new SamContext())
            {
                _lstWps =
                    ctx.WpsProyecto
                       .Where(x => x.ProyectoID == proyectoID)
                       .Select(x => x.Wps)
                       .ToList();
            }

            if (_lstWps.Count < 1)
            {
                Console.WriteLine("No existen wps para el proyecto: " + proyecto.Nombre);
                return;
            }

            _lstProcesoRaiz = ProcesoRaizBO.Instance.ObtenerTodos()
                                           .Where(x => x.ProcesoRaizID == 4 || x.ProcesoRaizID == 2)
                                           .ToList();

            _lstProcesoRelleno = ProcesoRellenoBO.Instance.ObtenerTodos()
                                                 .Where(x => x.ProcesoRellenoID == 5 || x.ProcesoRellenoID == 6)
                                                 .ToList();

            _consumibles = ConsumiblesBO.Instance.ObtenerPorPatio(proyecto.PatioID);

            if ( _consumibles.Count < 1)
            {
                Console.WriteLine("No existen consumibles para el proyecto: " + proyecto.Nombre);
                return;
            }

            Random rnd = new Random(DateTime.Now.Millisecond);

            List<JuntaWorkstatus> _lstWs;

            using (SamContext ctx = new SamContext())
            {
                ctx.JuntaWorkstatus.MergeOption = System.Data.Objects.MergeOption.NoTracking;

                _lstWs =
                    ctx.JuntaWorkstatus
                       .Include("Armado")
                       .Include("JuntaSpool")
                       .Where(x => x.JuntaSpool.Spool.ProyectoID == proyectoID && !x.SoldaduraAprobada && x.ArmadoAprobado)
                       .ToList();
            }

            _lstWs = _lstWs.OrderBy(x => rnd.Next()).ToList();

            Stopwatch sw = new Stopwatch();
            int juntasSoldadas = 0;

            Console.WriteLine("Comenzando soldaduras para el proyecto {0}", proyecto.Nombre);
            Console.WriteLine();

            foreach (JuntaWorkstatus jws in _lstWs)
            {
                JuntaSoldadura js = new JuntaSoldadura();
                generaDatosBase(rnd, jws, js);

                Wps wps = obtenWpsCandidato(rnd, jws, js);

                if (wps != null)
                {
                    js.WpsID = wps.WpsID;
                    agregaSoldadores(js, jws, TecnicaSoldadorEnum.Raiz);
                    agregaSoldadores(js, jws, TecnicaSoldadorEnum.Relleno);
                    List<int> lista = new List<int>();
                    try
                    {
                        sw.Restart();
                        SoldaduraBO.Instance.GuardaJuntaWorkstatus(jws, js, lista);
                        sw.Stop();
                        _lstTiemposArmado.Add(sw.ElapsedMilliseconds);
                        enviaMensajeExito(sw, jws, js);
                    }
                    catch (BaseValidationException bve)
                    {
                        Console.Error.WriteLine("Error al intentar soldar la jta. spool ID {0} => Jwid {1}.", jws.JuntaSpoolID, jws.JuntaWorkstatusID);
                        bve.Details.ForEach(Console.Error.WriteLine);
                        Console.WriteLine();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error al intentar soldar la jta. spool ID {0} => Jwid {1}.", jws.JuntaSpoolID, jws.JuntaWorkstatusID);
                        Console.WriteLine(ex);
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("No se encontró wps para la combinación x");
                }

                juntasSoldadas++;

                if (juntasSoldadas >= totalJuntas)
                {
                    break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Ejecución finalizada");
            Console.WriteLine("Se realizaron {0} soldados {1} ms en promedio.", _lstTiemposArmado.Count, _lstTiemposArmado.Average());
            Console.ReadLine();
        }

        private static void enviaMensajeExito(Stopwatch sw, JuntaWorkstatus jws, JuntaSoldadura js)
        {
            Console.WriteLine("Jwid {0} => soldada por raiz: {1}, relleno: {2}, en {3} ms.",
                                jws.JuntaWorkstatusID,
                                string.Join(",", js.JuntaSoldaduraDetalle
                                                   .Where(x => x.TecnicaSoldadorID == (int)TecnicaSoldadorEnum.Raiz)
                                                   .Select(x => x.SoldadorID)
                                                   .ToArray()),
                                string.Join(",", js.JuntaSoldaduraDetalle
                                                   .Where(x => x.TecnicaSoldadorID == (int)TecnicaSoldadorEnum.Relleno)
                                                   .Select(x => x.SoldadorID)
                                                   .ToArray()),
                                sw.ElapsedMilliseconds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rnd"></param>
        /// <param name="jws"></param>
        /// <param name="js"></param>
        /// <returns></returns>
        private Wps obtenWpsCandidato(Random rnd, JuntaWorkstatus jws, JuntaSoldadura js)
        {
            Wps wps;
            if (jws.JuntaSpool.FamiliaAceroMaterial2ID == null)
            {
                wps = _lstWps.Where(x => x.MaterialBase1ID == jws.JuntaSpool.FamiliaAceroMaterial1ID
                                                && x.MaterialBase2ID == jws.JuntaSpool.FamiliaAceroMaterial1ID
                                                && x.ProcesoRaizID == js.ProcesoRaizID
                                                && x.ProcesoRellenoID == js.ProcesoRellenoID)
                                  .OrderBy(x => rnd.Next())
                                  .FirstOrDefault();
            }
            else
            {
                wps = _lstWps.Where(x => x.MaterialBase1ID == jws.JuntaSpool.FamiliaAceroMaterial1ID
                                                && x.MaterialBase2ID == jws.JuntaSpool.FamiliaAceroMaterial2ID
                                                && x.ProcesoRaizID == js.ProcesoRaizID
                                                && x.ProcesoRellenoID == js.ProcesoRellenoID)
                                  .OrderBy(x => rnd.Next())
                                  .FirstOrDefault();
            }
            return wps;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rnd"></param>
        /// <param name="jws"></param>
        /// <param name="js"></param>
        private void generaDatosBase(Random rnd, JuntaWorkstatus jws, JuntaSoldadura js)
        {
            jws.StartTracking();
            jws.SoldaduraAprobada = true;
            jws.UltimoProcesoID = (int)UltimoProcesoEnum.Soldado;
            jws.UsuarioModifica = _userID;
            jws.FechaModificacion = DateTime.Now;

            js.FechaSoldadura = jws.JuntaArmado.FirstOrDefault().FechaArmado;
            js.FechaReporte = jws.JuntaArmado.FirstOrDefault().FechaReporte;
            js.TallerID = jws.JuntaArmado.FirstOrDefault().TallerID;
            js.UsuarioModifica = _userID;
            js.FechaModificacion = DateTime.Now;

            js.ProcesoRaizID = _lstProcesoRaiz.OrderBy(x => rnd.Next()).First().ProcesoRaizID;
            js.ProcesoRellenoID = _lstProcesoRelleno.OrderBy(x => rnd.Next()).First().ProcesoRellenoID;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="js"></param>
        /// <param name="tecnica"></param>
        private void agregaSoldadores(JuntaSoldadura js, JuntaWorkstatus jws, TecnicaSoldadorEnum tecnica)
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            Random rnd2 = new Random(rnd.Next());

            decimal diametro = jws.JuntaSpool.Diametro;
            int numeroSoldadores = 1;

            if (diametro >= 40)
            {
                //Hasta 4 soldadores, mínimo 2
                numeroSoldadores = rnd.Next(2, 4);
            }
            else if (diametro >= 20)
            {
                //Hasta 3 soldadores mínimo 1
                numeroSoldadores = rnd.Next(1, 3);
            }
            else if (diametro >= 10)
            {
                //Hasta 2 soldadores mínimo 1
                numeroSoldadores = rnd.Next(1, 2);
            }

            int consumibleID = _consumibles.OrderBy(x => rnd.Next()).First().ConsumibleID;

            for (int i = 0; i < numeroSoldadores; i++)
            {
                JuntaSoldaduraDetalle detalle = new JuntaSoldaduraDetalle();
                
                detalle.SoldadorID = _soldadores.Where(x => !js.JuntaSoldaduraDetalle
                                                               .Where(y => y.TecnicaSoldadorID == (int)tecnica)
                                                               .Select(y => y.SoldadorID)
                                                               .Contains(x.SoldadorID))
                                                .OrderBy(x => rnd.Next() * rnd2.Next())
                                                .First()
                                                .SoldadorID;

                detalle.TecnicaSoldadorID = (int)tecnica;
                detalle.UsuarioModifica = _userID;
                detalle.FechaModificacion = DateTime.Now;
                detalle.ConsumibleID = consumibleID;
                js.JuntaSoldaduraDetalle.Add(detalle);
            }
        }
    }
}
