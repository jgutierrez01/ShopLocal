using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Proyectos;
using SAM.BusinessObjects.Produccion;
using SAM.Entities.Grid;
using SAM.Entities.Personalizadas;
using SAM.BusinessObjects.Materiales;
using System.Diagnostics;
using Mimo.Framework.Exceptions;

namespace SAM.Generador
{
    public class GeneradorDespachos
    {
        private Guid _userID;
        private List<long> _lstTiemposTubo;
        private List<long> _lstTiemposAccesorio;

        public GeneradorDespachos()
        {
            _userID = new Guid("D6A113B4-464E-496F-B15D-4456CB0AE55B");
            _lstTiemposAccesorio = new List<long>();
            _lstTiemposTubo = new List<long>();
        }


        public void Inicia()
        {
            int proyectoID = 0;
            int cantidadOdts = 0;
            int procesadas = 0;
            Stopwatch sw = new Stopwatch();

            KeyboardUtils.ImprimeInline("ID proyecto: ");
            proyectoID = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Cantidad de odts a despachar: ");
            cantidadOdts = KeyboardUtils.LeeEntero();

            Proyecto proyecto = ProyectoBO.Instance.Obtener(proyectoID);

            Random rnd = new Random(DateTime.Now.Millisecond);

            var lstOdts =
                OrdenTrabajoBO.Instance
                              .ObtenerListaParaGrid(null, proyectoID, null, null)
                              .Where(x => x.EstatusDespacho != EstatusDespachoOdt.Despachada)
                              .OrderBy(x => rnd.Next())
                              .ToList();

            Console.WriteLine("Comenzando despachos para el proyecto {0}", proyecto.Nombre);
            Console.WriteLine();

            foreach (GrdOdt grd in lstOdts)
            {
                procesadas++;
                var odt = OrdenTrabajoBO.Instance.ObtenerConOdtSpool(grd.OrdenDeTrabajoID);

                foreach (OrdenTrabajoSpool odts in odt.OrdenTrabajoSpool)
                {
                    var materiales = OrdenTrabajoSpoolBO.Instance
                                                        .ObtenerMaterialesParaDespacho(odts.OrdenTrabajoSpoolID)
                                                        .Where(x => x.PerteneceAOdt)
                                                        .Where(x => !x.TieneDespacho)
                                                        .Where(x => (x.EsTubo && x.TieneCorte) || !x.EsTubo)
                                                        .ToList();

                    foreach (GrdMaterialesDespacho material in materiales)
                    {
                        OrdenTrabajoMaterial odtM = OrdenTrabajoMaterialBO.Instance.ObtenerInformacionParaDespacho(material.OrdenTrabajoMaterialID);

                        if (odtM.MaterialSpool.ItemCode.TipoMaterialID == (int)TipoMaterialEnum.Tubo)
                        {
                            InfoCorteDespacho corte = CorteDetalleBO.Instance.ObtenerInformacionDeCorteParaDespacho(odtM.CorteDetalleID);

                            try
                            {
                                sw.Restart();
                                OrdenTrabajoMaterialBO.Instance.DespachaTubo(odtM, corte.LongitudDelCorte, _userID, string.Empty);
                                sw.Stop();

                                Console.WriteLine("Tubo {0}-{1} => M.Spool {2} #Ctrl {3} en {4} ms", corte.CodigoNumeroUnico, corte.Segmento, odtM.MaterialSpoolID, odts.NumeroControl, sw.ElapsedMilliseconds);
                                _lstTiemposTubo.Add(sw.ElapsedMilliseconds);
                            }
                            catch (BaseValidationException bve)
                            {
                                Console.Error.WriteLine("Error al intentar despachar el tubo {0}-{1} => M.Spool {2} #Ctrl {3}.", corte.CodigoNumeroUnico, corte.Segmento, odtM.MaterialSpoolID, odts.NumeroControl);
                                bve.Details.ForEach(Console.Error.WriteLine);
                                Console.WriteLine();
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine("Error al intentar despachar el tubo {0}-{1} => M.Spool {2} #Ctrl {3}.", corte.CodigoNumeroUnico, corte.Segmento, odtM.MaterialSpoolID, odts.NumeroControl);
                                Console.Error.WriteLine(ex);
                                Console.WriteLine();
                            }
                        }
                        else
                        {
                            var nus = NumeroUnicoBO.Instance
                                                   .AccesoriosAfinesParaDespachoOAsignacion(odtM.MaterialSpoolID,string.Empty,0,100, false)
                                                   .Where(x => !x.EsEquivalente).FirstOrDefault();


                            if (nus != null)
                            {
                                try
                                {
                                    sw.Restart();
                                    OrdenTrabajoMaterialBO.Instance.DespachaAccesorio(odtM, 1, nus.NumeroUnicoID, _userID, string.Empty);
                                    sw.Stop();

                                    Console.WriteLine("Accesorio {0} => M.Spool {1} #Ctrl {2} en {3} ms.", nus.CodigoNumeroUnico, odtM.MaterialSpoolID, odts.NumeroControl, sw.ElapsedMilliseconds);
                                    _lstTiemposAccesorio.Add(sw.ElapsedMilliseconds);
                                }
                                catch (BaseValidationException bve)
                                {
                                    Console.Error.WriteLine("Error al intentar despachar el accesorio {0} => M.Spool {1} #Ctrl {2}.", nus.CodigoNumeroUnico, odtM.MaterialSpoolID, odts.NumeroControl);
                                    bve.Details.ForEach(Console.Error.WriteLine);
                                    Console.WriteLine();
                                }
                                catch (Exception ex)
                                {
                                    Console.Error.WriteLine("Error al intentar despachar el accesorio {0} => M.Spool {1} #Ctrl {2}.", nus.CodigoNumeroUnico, odtM.MaterialSpoolID, odts.NumeroControl);
                                    Console.WriteLine(ex);
                                    Console.WriteLine();
                                }
                            }
                        }
                    }
                }


                if ( procesadas == cantidadOdts )
                {
                    break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Ejecución finalizada");
            Console.WriteLine("Se realizaron {0} despachos de tubo en {1} ms en promedio.", _lstTiemposTubo.Count, _lstTiemposTubo.Average());
            Console.WriteLine("Se realizaron {0} despachos de accesorio en {1} ms en promedio.", _lstTiemposAccesorio.Count, _lstTiemposAccesorio.Average());
            Console.ReadLine();
        }
    }
}
