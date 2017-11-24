using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Proyectos;
using SAM.BusinessObjects.Produccion;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities.Personalizadas;
using SAM.BusinessObjects.Catalogos;
using System.Diagnostics;
using Mimo.Framework.Exceptions;

namespace SAM.Generador
{
    public class GeneradorArmado
    {
        private Guid _userID;
        private List<long> _lstTiemposArmado;
        List<Taller> _talleres;
        List<Tubero> _tuberos;

        public GeneradorArmado()
        {
            _userID = new Guid("D6A113B4-464E-496F-B15D-4456CB0AE55B");
            _lstTiemposArmado = new List<long>();
        }

        public void Inicia()
        {
            int proyectoID = 0;
            int cantidadOdts = 0;
            int spoolsArmados = 0;
            int maximoSpoolsArmarPorDia = 0;
            int odtsProcesadasConAlmenosUnArmado = 0;

            DateTime fechaInicio;

            KeyboardUtils.ImprimeInline("ID proyecto: ");
            proyectoID = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Cantidad de odts a armar: ");
            cantidadOdts = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Fecha inicial de armado: ");
            fechaInicio = Convert.ToDateTime(Console.ReadLine());

            KeyboardUtils.ImprimeInline("Cantidad de spools a armar por día: ");
            maximoSpoolsArmarPorDia = KeyboardUtils.LeeEntero();

            Proyecto proyecto = ProyectoBO.Instance.Obtener(proyectoID);

            _talleres = ProyectoBO.Instance.ObtenerTallers(proyectoID);

            if (_talleres.Count < 1)
            {
                Console.WriteLine("No existen talleres para el proyecto: " + proyecto.Nombre);
                return;
            }

            _tuberos = TuberoBO.Instance.ObtenerTodos().Where(x => x.PatioID == proyecto.PatioID).ToList();

            if (_tuberos.Count < 1)
            {
                Console.WriteLine("No existen tuberos para el proyecto: " + proyecto.Nombre);
                return;
            }

            Random rnd = new Random(DateTime.Now.Millisecond);

            var lstOdts =
                OrdenTrabajoBO.Instance
                              .ObtenerListaParaGrid(null, proyectoID, null, null)
                              .OrderBy(x => rnd.Next())
                              .ToList();

            Console.WriteLine("Comenzando armados para el proyecto {0}", proyecto.Nombre);
            Console.WriteLine();

            foreach (GrdOdt grd in lstOdts)
            {
                var odt = OrdenTrabajoBO.Instance.ObtenerConOdtSpool(grd.OrdenDeTrabajoID);
                Taller taller = _talleres.OrderBy(x => rnd.Next()).First();
                bool armoAlMenosUnaJuntaDeUnSpool = false;

                foreach (OrdenTrabajoSpool odts in odt.OrdenTrabajoSpool)
                {
                    Tubero tubero = _tuberos.OrderBy(x => rnd.Next()).First();

                    IEnumerable<GrdArmado> armado = ArmadoBO.Instance
                                                            .ObtenerListaArmado(proyectoID, odt.OrdenTrabajoID, odts.OrdenTrabajoSpoolID)
                                                            .Where(x => x.EstatusID == (int)EstatusArmado.Despachado);

                    bool armoAlMenosUnaJunta = false;

                    foreach (GrdArmado jta in armado)
                    {
                        OrdenTrabajoJunta odtJ = ArmadoBO.Instance.ObtenerInformacionParaArmado(jta.JuntaSpoolID);

                        List<Simple> nusUno =
                            ArmadoBO.Instance
                                    .ObtenerNumeroUnicoPorEtiquetaMaterial(odtJ.JuntaSpool.EtiquetaMaterial1, odts.OrdenTrabajoSpoolID, odtJ.JuntaSpool.SpoolID);

                        List<Simple> nusDos =
                            ArmadoBO.Instance
                                    .ObtenerNumeroUnicoPorEtiquetaMaterial(odtJ.JuntaSpool.EtiquetaMaterial2, odts.OrdenTrabajoSpoolID, odtJ.JuntaSpool.SpoolID);

                        if (nusUno != null && nusUno.Count > 0 && nusDos != null && nusDos.Count > 0)
                        {
                            Simple nuUno = nusUno.OrderBy(x => rnd.Next()).FirstOrDefault();
                            Simple nuDos = nusDos.OrderBy(x => rnd.Next()).FirstOrDefault();

                            if (nuUno != null && nuDos != null)
                            {
                                armar(odtJ, nuUno, nuDos, taller, tubero, fechaInicio);
                                armoAlMenosUnaJunta = true;
                            }
                            else
                            {
                                Console.WriteLine("No se encontraron los nus para la jtSid {0}-{1}, #Ctrl {2}", odtJ.JuntaSpoolID, odtJ.JuntaSpool.Etiqueta, odts.NumeroControl);
                            }
                        }
                    }

                    if (armoAlMenosUnaJunta)
                    {
                        armoAlMenosUnaJuntaDeUnSpool = true;
                        spoolsArmados++;
                    }

                    if (spoolsArmados > 0)
                    {
                        if ((spoolsArmados % maximoSpoolsArmarPorDia) == 0)
                        {
                            fechaInicio = fechaInicio.AddDays(1);
                        }
                    }
                }

                if (armoAlMenosUnaJuntaDeUnSpool)
                {
                    odtsProcesadasConAlmenosUnArmado++;
                }

                if (odtsProcesadasConAlmenosUnArmado >= cantidadOdts)
                {
                    break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Ejecución finalizada");
            Console.WriteLine("Se realizaron {0} armados {1} ms en promedio.", _lstTiemposArmado.Count, _lstTiemposArmado.Average());
            Console.ReadLine();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jta"></param>
        /// <param name="nuUno"></param>
        /// <param name="nuDos"></param>
        /// <param name="taller"></param>
        private void armar(OrdenTrabajoJunta jta, Simple nuUno, Simple nuDos, Taller taller, Tubero tubero, DateTime fecha)
        {
            Stopwatch sw = new Stopwatch();
            JuntaWorkstatus jws = ArmadoBO.Instance.ObtenerJuntaWorkstatusPorJuntaSpoolID(jta.JuntaSpoolID);

            // creamos la junta armado.
            JuntaArmado ja = new JuntaArmado();
            ja.NumeroUnico1ID = nuUno.ID;
            ja.NumeroUnico2ID = nuDos.ID;
            ja.TallerID = taller.TallerID;
            ja.TuberoID = tubero.TuberoID;
            ja.FechaArmado = fecha.Date;
            ja.FechaReporte = fecha.Date;
            ja.Observaciones = string.Empty;
            ja.UsuarioModifica = _userID;
            ja.FechaModificacion = DateTime.Now;

            //Junta Workstatus
            if (jws == null)
            {
                jws = new JuntaWorkstatus();
                jws.EtiquetaJunta = jta.JuntaSpool.Etiqueta;
                jws.JuntaSpoolID = jta.JuntaSpoolID;
                jws.OrdenTrabajoSpoolID = jta.OrdenTrabajoSpoolID;
                jws.ArmadoAprobado = true;
                jws.SoldaduraAprobada = false;
                jws.InspeccionVisualAprobada = false;
                jws.VersionJunta = 1;
                jws.JuntaFinal = true;
                jws.UltimoProcesoID = (int)UltimoProcesoEnum.Armado;
                jws.UsuarioModifica = _userID;
                jws.FechaModificacion = DateTime.Now;
            }
            else
            {
                jws.StartTracking();
                jws.ArmadoAprobado = true;
                jws.UltimoProcesoID = (int)UltimoProcesoEnum.Armado;
                jws.UsuarioModifica = _userID;
                jws.FechaModificacion = DateTime.Now;
            }

            try
            {
                sw.Start();
                ArmadoBO.Instance.GuardaJuntaWorkstatus(jws, ja);
                sw.Stop();
                _lstTiemposArmado.Add(sw.ElapsedMilliseconds);

                Console.WriteLine("Jta. SpID {0}-{1} armada con Nu1 {2} y Nu2 {3} por {4} para #ctrl {5} en {6} ms.", jta.JuntaSpoolID, jta.JuntaSpool.Etiqueta, nuUno.ID, nuDos.ID, tubero.Codigo, jta.OrdenTrabajoSpool.NumeroControl, sw.ElapsedMilliseconds);
            }
            catch (BaseValidationException bve)
            {
                Console.Error.WriteLine("Error al intentar armar la jta. spool ID {0} => OdtsID {1} Nu1ID {2} Nu2ID {3}.", jta.JuntaSpoolID, jta.OrdenTrabajoSpoolID, nuUno.ID, nuDos.ID);
                bve.Details.ForEach(Console.Error.WriteLine);
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error al intentar armar la jta. spool ID {0} => OdtsID {1} Nu1ID {2} Nu2ID {3}.", jta.JuntaSpoolID, jta.OrdenTrabajoSpoolID, nuUno.ID, nuDos.ID);
                Console.WriteLine(ex);
                Console.WriteLine();
            }
        }
    }
}
