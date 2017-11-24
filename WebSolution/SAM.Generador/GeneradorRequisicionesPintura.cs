using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using System.Diagnostics;
using SAM.BusinessObjects.Proyectos;
using SAM.BusinessObjects.Produccion;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Workstatus;

namespace SAM.Generador
{
    public class GeneradorRequisicionesPintura
    {
        private Guid _userID;
        private List<long> _lstTiemposRequisicion;

        public GeneradorRequisicionesPintura()
        {
            _userID = new Guid("D6A113B4-464E-496F-B15D-4456CB0AE55B");
            _lstTiemposRequisicion = new List<long>();
        }

        public void Inicia()
        {
            int proyectoID = 0;
            int spoolsARequisitar = 0;
            int minSpoolsPorReq = 0;
            int maxSpoolsPorReq = 0;
            string prefijoRequisicion = string.Empty;
            int inicialConsecutivoReq = 0;
            int spoolsProcesados = 0;
            int tipoAccion = 0;
            List<string> sistemas = new List<string>();
            List<string> colores = new List<string>();
            List<string> codigos = new List<string>();

            KeyboardUtils.ImprimeInline("ID proyecto: ");
            proyectoID = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Cantidad de spools a requisitar: ");
            spoolsARequisitar = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Acción (1 = Sistema, 2 = Requisitar): ");
            tipoAccion = KeyboardUtils.LeeEntero();

            if (tipoAccion > 2 || tipoAccion < 1)
            {
                tipoAccion = 1;
            }

            KeyboardUtils.ImprimeInline("Mínimo de spools por requisición: ");
            minSpoolsPorReq = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Máximo de spools por requisición: ");
            maxSpoolsPorReq = KeyboardUtils.LeeEntero();

            if (tipoAccion == 2)
            {
                KeyboardUtils.ImprimeInline("Prefijo requisición: ");
                prefijoRequisicion = KeyboardUtils.LeeCadena();

                KeyboardUtils.ImprimeInline("Inicial consecutivo requisición: ");
                inicialConsecutivoReq = KeyboardUtils.LeeEntero();
            }
            else
            {
                KeyboardUtils.ImprimeInline("Especifica sistemas separados por coma: ");
                sistemas = KeyboardUtils.LeeCadena().Split(',').Select(x => x.Trim()).ToList();

                KeyboardUtils.ImprimeInline("Especifica colores separados por coma: ");
                colores = KeyboardUtils.LeeCadena().Split(',').Select(x => x.Trim()).ToList();

                KeyboardUtils.ImprimeInline("Especifica códigos separados por coma: ");
                codigos = KeyboardUtils.LeeCadena().Split(',').Select(x => x.Trim()).ToList();
            }


            Proyecto proyecto = ProyectoBO.Instance.Obtener(proyectoID);

            Random rnd = new Random(DateTime.Now.Millisecond);

            List<GrdRequisicionPintura> lstSpools = ObtenerListadoRequisicionPintura(proyecto.ProyectoID, tipoAccion).OrderBy(x => rnd.NextDouble()).ToList();

            Console.WriteLine("Comenzando {0} para el proyecto {1}", tipoAccion == 1 ? "especificación de sistemas" : "requisiciones", proyecto.Nombre);
            Console.WriteLine();

            int numSpoolsEnAccion = 0;

            Stopwatch sw = new Stopwatch();
            DateTime hoy = DateTime.Now.Date;

            while (true)
            {
                numSpoolsEnAccion = rnd.Next(minSpoolsPorReq, maxSpoolsPorReq);

                var spoolsParaReporte =
                    lstSpools.Skip(spoolsProcesados)
                             .Take(numSpoolsEnAccion)
                             .ToList();

                spoolsProcesados += numSpoolsEnAccion;

                if (spoolsParaReporte.Count < 1)
                {
                    break;
                }

               

                if (tipoAccion == 1)
                {
                    int[] ids = spoolsParaReporte.Select(x => x.SpoolID).ToArray();
                    string csv = string.Join(",", spoolsParaReporte.Select(x => x.SpoolID.ToString()));

                    EspecificarSistema( spoolsProcesados,
                                        sistemas, 
                                        colores, 
                                        codigos, 
                                        spoolsParaReporte,
                                        ids,
                                        csv,
                                        sw);
                }
                else
                {
                    int[] ids = spoolsParaReporte.Select(x => x.WorkstatusSpoolID.Value).ToArray();
                    string csv = string.Join(",", spoolsParaReporte.Select(x => x.WorkstatusSpoolID.Value.ToString()));

                    Requisitar(prefijoRequisicion, inicialConsecutivoReq++, proyecto, sw, hoy, ids, csv);
                }


                if (spoolsProcesados >= spoolsARequisitar)
                {
                    break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Ejecución finalizada");
            Console.WriteLine("Se realizaron {0} {2} en {1} ms en promedio.", _lstTiemposRequisicion.Count, _lstTiemposRequisicion.Average(), tipoAccion == 1 ? "especificaciones de sistema" : "requisiciones");
            Console.ReadLine();
        }

        private void Requisitar(string prefijoRequisicion, int inicialConsecutivoReq, Proyecto proyecto, Stopwatch sw, DateTime hoy, int[] ids, string csv)
        {
            string numRequisicion = prefijoRequisicion + "-" + (inicialConsecutivoReq).ToString().PadLeft(4, '0');

            try
            {
                sw.Restart();
                RequisicionPinturaBO.Instance.GeneraRequisicion(proyecto.ProyectoID, numRequisicion, hoy, ids, _userID);
                sw.Stop();

                _lstTiemposRequisicion.Add(sw.ElapsedMilliseconds);
                Console.WriteLine("Spools requistados = {0}, Requisición = {1} en {2} ms.", csv, numRequisicion, sw.ElapsedMilliseconds);
                Console.WriteLine();
            }
            catch (BaseValidationException bve)
            {
                Console.WriteLine("Error al requisitar spools = {0}, Requisición = {1}.", csv, numRequisicion);
                bve.Details.ForEach(Console.Error.WriteLine);
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al requisitar spools = {0}, Requisición = {1}.", csv, numRequisicion);
                Console.WriteLine(ex);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spoolsProcesados"></param>
        /// <param name="sistemas"></param>
        /// <param name="colores"></param>
        /// <param name="codigos"></param>
        /// <param name="spoolsParaReporte"></param>
        /// <param name="ids"></param>
        /// <param name="csv"></param>
        /// <param name="sw"></param>
        private void EspecificarSistema(int spoolsProcesados, List<string> sistemas, List<string> colores, List<string> codigos, List<GrdRequisicionPintura> spoolsParaReporte, int [] ids, string csv, Stopwatch sw)
        {
            string sistema = sistemas[(spoolsProcesados - 1) % sistemas.Count];
            string color = colores[(spoolsProcesados - 1) % colores.Count];
            string codigo = codigos[(spoolsProcesados - 1) % codigos.Count];

            try
            {
                sw.Restart();
                WorkstatusSpoolBO.Instance.EspecificarSistema(ids, sistema, color, codigo);
                sw.Stop();

                _lstTiemposRequisicion.Add(sw.ElapsedMilliseconds);
                Console.WriteLine("Spools especificados = {0}, Sistema = {1}, Color = {2}, Codigo = {3} en {4} ms.", csv, sistema, color, codigo, sw.ElapsedMilliseconds);
                Console.WriteLine();
            }
            catch (BaseValidationException bve)
            {
                Console.WriteLine("Error al especificar sistema para spools = {0}, Sistema = {1}, Color = {2}, Codigo = {3}.", csv, sistema, color, codigo);
                bve.Details.ForEach(Console.Error.WriteLine);
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al especificar sistema para spools = {0}, Sistema = {1}, Color = {2}, Codigo = {3}.", csv, sistema, color, codigo);
                Console.WriteLine(ex);
                Console.WriteLine();
            }
        }

        #region Helpers

        public List<GrdRequisicionPintura> ObtenerListadoRequisicionPintura(int proyectoId, int accion)
        {
            List<GrdRequisicionPintura> reqP = null;

            using (SamContext ctx = new SamContext())
            {
                //Especificar sistema
                if (accion == 1)
                {
                    reqP =
                            (from s in ctx.Spool
                             join ots in ctx.OrdenTrabajoSpool on s.SpoolID equals ots.SpoolID into otsDef
                             from t1 in otsDef.DefaultIfEmpty()
                             join wks in ctx.WorkstatusSpool on t1.OrdenTrabajoSpoolID equals wks.OrdenTrabajoSpoolID into wksDef
                             from t2 in wksDef.DefaultIfEmpty()
                             where s.ProyectoID == proyectoId
                             where (t2.TieneLiberacionDimensional == null || t2.TieneLiberacionDimensional == true) && (t2.TieneRequisicionPintura == null || t2.TieneRequisicionPintura == false)
                             let sph = s.SpoolHold
                             select new GrdRequisicionPintura
                             {
                                 WorkstatusSpoolID = t2.WorkstatusSpoolID,
                                 NombreSpool = s.Nombre,
                                 NumeroControl = t1.NumeroControl,
                                 Sistema = s.SistemaPintura,
                                 Color = s.ColorPintura,
                                 Codigo = s.CodigoPintura,
                                 Hold = sph != null ? ((sph.TieneHoldCalidad) || (sph.TieneHoldIngenieria) || (sph.Confinado)) : false,
                                 SpoolID = s.SpoolID,
                             }).ToList();
                }
                else
                {
                    reqP =
                           (from wks in ctx.WorkstatusSpool
                            join ots in ctx.OrdenTrabajoSpool on wks.OrdenTrabajoSpoolID equals ots.OrdenTrabajoSpoolID
                            join s in ctx.Spool on ots.SpoolID equals s.SpoolID
                            where s.ProyectoID == proyectoId
                            where wks.TieneLiberacionDimensional == true && wks.TieneRequisicionPintura == false
                            where s.SistemaPintura != string.Empty
                            let sph = s.SpoolHold
                            select new GrdRequisicionPintura
                            {
                                WorkstatusSpoolID = wks.WorkstatusSpoolID,
                                NombreSpool = s.Nombre,
                                NumeroControl = ots.NumeroControl,
                                Sistema = s.SistemaPintura,
                                Color = s.ColorPintura,
                                Codigo = s.CodigoPintura,
                                Hold = sph != null ? ((sph.TieneHoldCalidad) || (sph.TieneHoldIngenieria) || (sph.Confinado)) : false,
                                SpoolID = s.SpoolID,
                            }).ToList();
                }
            }
            return reqP;
        }

        #endregion
    }
}
