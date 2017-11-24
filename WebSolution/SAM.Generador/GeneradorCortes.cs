using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Proyectos;
using SAM.BusinessObjects.Produccion;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Materiales;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Exceptions;
using SAM.Entities.RadCombo;
using SAM.Entities.Personalizadas;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessLogic.Workstatus;
using System.Diagnostics;

namespace SAM.Generador
{
    public class GeneradorCortes
    {
        static int cantidadDeTransferencias = 0;
        static List<long> lstTiemposTransferencia = new List<long>();
        static int cantidadDeCortes = 0;
        static List<long> lstTiemposCorte = new List<long>();

        
        public void Inicia()
        {
            int proyectoID = 0;
            int cantidadOdts = 0;
            int odtsTransferidas = 0;


            Guid userID = new Guid("D6A113B4-464E-496F-B15D-4456CB0AE55B");

            KeyboardUtils.ImprimeInline("ID proyecto inicial: ");
            proyectoID = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Cantidad de odts: ");
            cantidadOdts = KeyboardUtils.LeeEntero();

            
            Proyecto proyecto = ProyectoBO.Instance.Obtener(proyectoID);
            List<UbicacionFisica> ubicacion = UbicacionFisicaBO.Instance.ObtenerPorPatioID(proyecto.PatioID);
            List<Maquina> maquinas = MaquinaBO.Instance
                                              .ObtenerTodasConPatio()
                                              .Where(x => x.PatioID == proyecto.PatioID)
                                              .ToList();

            if (ubicacion.Count == 0)
            {
                Console.Error.WriteLine("El proyecto no tiene ubicaciones dadas de alta");
                return;
            }

            if (maquinas.Count == 0)
            {
                Console.Error.Write("El proyecto no tiene máquinas dadas de alta");
                return;
            }

            Console.WriteLine("Comenzando cortes para el proyecto {0}", proyecto.Nombre);
            Console.WriteLine();

            Random rnd = new Random(DateTime.Now.Millisecond);

            var ordenes = OrdenTrabajoBO.Instance
                                        .ObtenerListaParaGrid(null, proyecto.ProyectoID, null, null)
                                        .Where(x => x.EstatusDespacho != EstatusDespachoOdt.Despachada)
                                        .OrderBy(x => rnd.Next())
                                        .ToList();

            Stopwatch sw = new Stopwatch();

            foreach (GrdOdt grd in ordenes)
            {
                bool pudoTransferir = false;
                OrdenTrabajo odt = OrdenTrabajoBO.Instance.ObtenerConOdtSpool(grd.OrdenDeTrabajoID);

                foreach (OrdenTrabajoSpool odts in odt.OrdenTrabajoSpool)
                {
                    var materiales = OrdenTrabajoSpoolBO.Instance
                                                        .ObtenerMaterialesParaDespacho(odts.OrdenTrabajoSpoolID);


                    var nusCandidatos = NumeroUnicoBO.Instance
                                                        .ObtenerNumerosUnicosATransferir(odt.OrdenTrabajoID, odts.OrdenTrabajoSpoolID, proyecto.ProyectoID);

                    List<NumeroUnicoSegmento> lstATransferir = new List<NumeroUnicoSegmento>();
                    bool completaMaterial = true;

                    foreach (GrdMaterialesDespacho material in materiales.Where(x => !x.TieneCorte && !x.TieneDespacho && x.PerteneceAOdt && x.EsTubo))
                    {
                        var candidatosParaMaterial = 
                            nusCandidatos.Where(x => x.NumeroUnico.Diametro1 == material.Diametro1
                                                        && x.NumeroUnico.Diametro2 == material.Diametro2
                                                        && x.NumeroUnico.ItemCodeID == material.ItemCodeID
                                                        && x.InventarioBuenEstado >= material.CantidadRequerida);

                        if (candidatosParaMaterial.Count() > 0)
                        {
                            //Tomar el más pequeño
                            var nuATransferir = candidatosParaMaterial.OrderBy(x => x.InventarioBuenEstado).First();
                            //restar cantidad para seguirlo usando
                            nuATransferir.InventarioBuenEstado -= material.CantidadRequerida;

                            if (!lstATransferir.Contains(nuATransferir))
                            {
                                lstATransferir.Add(nuATransferir);
                            }
                        }
                        else
                        {
                            completaMaterial = false;
                            break;
                        }
                    }

                    if (completaMaterial)
                    {
                        int unbicacionFisicaID = ubicacion.OrderBy(x => rnd.Next())
                                                            .Select(x => x.UbicacionFisicaID)
                                                            .First();

                        foreach (NumeroUnicoSegmento nus in lstATransferir)
                        {
                            try
                            {
                                sw.Restart();

                                NumeroUnicoBO.Instance.TransfiereACorte(new int []{nus.NumeroUnicoSegmentoID},
                                                                        userID,
                                                                        odt.OrdenTrabajoID,
                                                                        unbicacionFisicaID);
                                sw.Stop();

                                cantidadDeTransferencias++;
                                lstTiemposTransferencia.Add(sw.ElapsedMilliseconds);

                                Console.WriteLine("N.U. {0} transferido => #Ctrl {1} en {2} ms.", nus.NumeroUnico.Codigo, odts.NumeroControl, sw.ElapsedMilliseconds);
                            }
                            catch (BaseValidationException bve)
                            {
                                Console.WriteLine();
                                bve.Details.ForEach(Console.Error.WriteLine);
                                Console.WriteLine();

                                Console.Error.WriteLine("No se pudo transferir el número único {0} a corte para el número de control {1}", nus.NumeroUnico.Codigo, odts.NumeroControl);
                            }
                            catch (Exception ex)
                            {
                                LogueaExcepcionDeTransferencia(ex, nus, odt, odts);
                            }
                        }

                        pudoTransferir = true;
                    }
                }

                if (pudoTransferir)
                {
                    odtsTransferidas++;
                    ComienzaElCorte(odt, maquinas, userID);
                }

                if (odtsTransferidas == cantidadOdts)
                {
                    break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Ejecución finalizada");
            Console.WriteLine("Se realizaron {0} transferencias en {1} ms en promedio.", cantidadDeTransferencias, lstTiemposTransferencia.Average());
            Console.WriteLine("Se realizaron {0} cortes en {1} ms en promedio.", cantidadDeCortes, lstTiemposCorte.Average());
            Console.ReadLine();
        }

        private static void LogueaExcepcionDeTransferencia(Exception ex, NumeroUnicoSegmento nus, OrdenTrabajo odt, OrdenTrabajoSpool odts)
        {
            Console.WriteLine();
            Console.WriteLine("Error al llevar a cabo la transferencia");
            Console.WriteLine();
            string datosNu = "NumeroUnicoID = {0}, Segmento = {1}, Odt = {2}, Odts = {3}";
            Console.WriteLine(datosNu, nus.NumeroUnicoID, nus.Segmento, odt.OrdenTrabajoID, odts.OrdenTrabajoSpoolID);
            Console.WriteLine();
            Console.WriteLine(ex);
            Console.WriteLine();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="odt"></param>
        /// <param name="maquinas"></param>
        /// <param name="userID"></param>
        private static void ComienzaElCorte(OrdenTrabajo odt, List<Maquina> maquinas, Guid userID)
        {
            Stopwatch sw = new Stopwatch();
            Random rnd = new Random(DateTime.Now.Millisecond);

            var nus = NumeroUnicoBO.Instance
                                   .ListaNumeroUnicoEnTrasferencia(odt.OrdenTrabajoID, string.Empty, 0, 5000);

            foreach (RadNumeroUnico nu in nus)
            {
                List<CorteDetalle> lstCorte = new List<CorteDetalle>();

                NumeroUnico numUnico = NumeroUnicoBO.Instance.ObtenerConTransferenciaCorteIC(nu.NumeroUnicoID);
                NumeroUnicoCorte corte = numUnico.NumeroUnicoCorte.Where(x => !x.TieneCorte).SingleOrDefault();
                int longitud = corte.Longitud;

                var simples = OrdenTrabajoSpoolBO.Instance.ObtenerAfinesANumeroUnico(nu.NumeroUnicoID, string.Empty, odt.OrdenTrabajoID, string.Empty, 0, 200);

                foreach (Simple s in simples)
                {
                    var materiales = MaterialSpoolBO.Instance.ListaMaterialesPorNumeroControl(s.ID, nu.NumeroUnicoID, string.Empty, string.Empty, 0, 50);

                    foreach (RadMaterialParaCorte material in materiales)
                    {
                        MaterialSpool ms = MaterialSpoolBO.Instance.Obtener(material.MaterialSpoolID);

                        if (ms.Cantidad <= longitud)
                        {
                            longitud -= ms.Cantidad;

                            lstCorte.Add(new CorteDetalle
                            {
                                Cantidad = ms.Cantidad,
                                EsAjuste = false,
                                FechaCorte = DateTime.Now,
                                FechaModificacion = DateTime.Now,
                                UsuarioModifica = userID,
                                MaterialSpoolID = ms.MaterialSpoolID,
                                OrdenTrabajoSpoolID = s.ID,
                                MaquinaID = maquinas.OrderBy(x => rnd.Next()).Select(x => x.MaquinaID).First()
                            });
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (lstCorte.Count > 0)
                {
                    int totalCorte = lstCorte.Sum(x => x.Cantidad);

                    if ( longitud > 0 )
                    {
                        //merma ficticia
                        longitud = (int)(longitud * 0.9);
                    }

                    try
                    {
                        sw.Restart();
                        CorteBL.Instance.GeneraNuevoCorte(lstCorte, longitud, "5", nu.NumeroUnicoID, nu.Segmento, totalCorte, null, userID);
                        sw.Stop();

                        cantidadDeCortes++;
                        lstTiemposCorte.Add(sw.ElapsedMilliseconds);

                        Console.WriteLine("Nuevo corte generado de manera exitosa en {0} ms.", sw.ElapsedMilliseconds);
                    }
                    catch (BaseValidationException bve)
                    {
                        Console.WriteLine();
                        bve.Details.ForEach(Console.Error.WriteLine);
                        Console.WriteLine();

                        Console.Error.WriteLine("Error al generar el corte para NumeroUnico = {0}, MaterialSpoolID = {1}.",
                                                nu.CodigoSegmento,
                                                string.Join(",", lstCorte.Select(x => x.MaterialSpoolID).ToArray()));
                    }
                    catch (Exception ex)
                    {
                        LogueaExcepcionDeCorte(ex, nu, lstCorte);
                    }
                }
            }
        }

        private static void LogueaExcepcionDeCorte(Exception ex, RadNumeroUnico nu, List<CorteDetalle> lstCorte)
        {
            Console.WriteLine();
            Console.WriteLine("Error al llevar a cabo el corte");
            Console.WriteLine();

            string datosNu =
@"NumeroUnicoID = {0}
Codigo = {1}
Segmento = {2},
InventarioBuenEstado = {3}";

            Console.WriteLine(datosNu, nu.NumeroUnicoID, nu.Codigo, nu.Segmento, nu.InventarioBuenEstado);
            lstCorte.ForEach(x => Console.WriteLine("MsID={0}, Cnt={1}, Ots={2}, Mid={3}", x.MaterialSpoolID, x.Cantidad, x.OrdenTrabajoSpoolID, x.MaquinaID));

            Console.WriteLine();
            Console.WriteLine(ex);
            Console.WriteLine();
        }
    }
}
