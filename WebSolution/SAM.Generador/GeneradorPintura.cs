using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using SAM.Entities;
using SAM.BusinessObjects.Proyectos;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Modelo;
using System.Diagnostics;
using SAM.BusinessObjects.Workstatus;
using Mimo.Framework.Exceptions;

namespace SAM.Generador
{
    public class GeneradorPintura
    {
        private Guid _userID;
        private List<long> _lstTiemposRequisicion;

        public GeneradorPintura()
        {
            _userID = new Guid("D6A113B4-464E-496F-B15D-4456CB0AE55B");
            _lstTiemposRequisicion = new List<long>();
        }

        public void Inicia()
        {
            int proyectoID = 0;
            int spoolsAPintar = 0;
            int minSpoolsPorRep = 0;
            int maxSpoolsPorRep = 0;
            string prefijoReporte = string.Empty;
            int inicialConsecutivoReporte = 0;
            int spoolsProcesados = 0;
            int tipoReporte = 0;
            TipoPinturaEnum tipoPintura;

            KeyboardUtils.ImprimeInline("ID proyecto: ");
            proyectoID = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Cantidad de spools a pintar: ");
            spoolsAPintar = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Mínimo de spools por reporte: ");
            minSpoolsPorRep = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Máximo de spools por reporte: ");
            maxSpoolsPorRep = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Tipo reporte (1 = SB, 2 = Prim, 3 = Int, 4 = AV, 5 = Adh, 6 = P-Off): ");
            tipoReporte = KeyboardUtils.LeeEntero();

            if (tipoReporte < 1 || tipoReporte > 6)
            {
                tipoReporte = 1;
            }

            tipoPintura = (TipoPinturaEnum)tipoReporte;

            KeyboardUtils.ImprimeInline("Prefijo reporte: ");
            prefijoReporte = KeyboardUtils.LeeCadena();

            KeyboardUtils.ImprimeInline("Inicial consecutivo reporte: ");
            inicialConsecutivoReporte = KeyboardUtils.LeeEntero();

            KeyboardUtils.ImprimeInline("Fecha reporte (dd/MM/yyyy): ");
            DateTime fechaReporte = DateTime.ParseExact(KeyboardUtils.LeeCadena(), "dd/MM/yyyy", new CultureInfo("es-MX"));

            Proyecto proyecto = ProyectoBO.Instance.Obtener(proyectoID);

            Random rnd = new Random(DateTime.Now.Millisecond);

            List<GrdPintura> lstSpools = ObtenerListadoPintura(proyecto.ProyectoID).OrderBy(x => rnd.NextDouble()).ToList();

            Console.WriteLine("Comenzando pintura para el proyecto {0}", proyecto.Nombre);
            Console.WriteLine();

            int numSpoolsAPintar = 0;

            Stopwatch sw = new Stopwatch();

            while (true)
            {
                numSpoolsAPintar = rnd.Next(minSpoolsPorRep, maxSpoolsPorRep);

                var spoolsParaReporte =
                    lstSpools.Skip(spoolsProcesados)
                             .Take(numSpoolsAPintar)
                             .ToList();

                spoolsProcesados += numSpoolsAPintar;

                if (spoolsParaReporte.Count < 1)
                {
                    break;
                }

                PinturaSpool pintura = new PinturaSpool();
                pintura.ProyectoID = proyectoID;

                string numReporte = prefijoReporte + "-" + (inicialConsecutivoReporte++).ToString().PadLeft(4, '0');

                EspecificarReporteYFecha(tipoPintura, fechaReporte, pintura, numReporte);

                string csv = string.Join(",", spoolsParaReporte.Select(x => x.WorkstatusSpoolID.ToString()));
                string rqIds = string.Join(",", spoolsParaReporte.Select(x => x.RequisicionPinturaDetalleID.ToString()));
                string csvOtsIds = string.Join(",", spoolsParaReporte.Select(x => x.OrdenTrabajoSpoolID.ToString()));

                try
                {
                    sw.Restart();
                    PinturaBO.Instance.GuardaPinturaSpool(pintura, csvOtsIds, rqIds, true, string.Empty, _userID);
                    sw.Stop();

                    _lstTiemposRequisicion.Add(sw.ElapsedMilliseconds);
                    Console.WriteLine("Spools pintados = {0}, reporte = {1} en {2} ms.", csv, numReporte, sw.ElapsedMilliseconds);
                    Console.WriteLine();
                }
                catch (BaseValidationException bve)
                {
                    Console.WriteLine("Error al pintar spools = {0}, reporte = {1}.", csv, numReporte);
                    bve.Details.ForEach(Console.Error.WriteLine);
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al pintar spools = {0}, reporte = {1}.", csv, numReporte);
                    Console.WriteLine(ex);
                    Console.WriteLine();
                } 
                
                if (spoolsProcesados >= spoolsAPintar)
                {
                    break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Ejecución finalizada");
            Console.WriteLine("Se realizaron {0} reportes de pintura en {1} ms en promedio.", _lstTiemposRequisicion.Count, _lstTiemposRequisicion.Average());
            Console.ReadLine();
        }

        private static void EspecificarReporteYFecha(TipoPinturaEnum tipoPintura, DateTime fechaReporte, PinturaSpool pintura, string numReporte)
        {
            switch (tipoPintura)
            {
                case TipoPinturaEnum.SandBlast:
                    pintura.FechaSandblast = fechaReporte;
                    pintura.ReporteSandblast = numReporte;
                    break;
                case TipoPinturaEnum.PullOff:
                    pintura.FechaPullOff = fechaReporte;
                    pintura.ReportePullOff = numReporte;
                    break;
                case TipoPinturaEnum.Primario:
                    pintura.FechaPrimarios = fechaReporte;
                    pintura.ReportePrimarios = numReporte;
                    break;
                case TipoPinturaEnum.Intermedio:
                    pintura.FechaIntermedios = fechaReporte;
                    pintura.ReporteIntermedios = numReporte;
                    break;
                case TipoPinturaEnum.Adherencia:
                    pintura.FechaAdherencia = fechaReporte;
                    pintura.ReporteAdherencia = numReporte;
                    break;
                case TipoPinturaEnum.AcabadoVisual:
                    pintura.FechaAcabadoVisual = fechaReporte;
                    pintura.ReporteAcabadoVisual = numReporte;
                    break;
            }
        }

        #region Helpers

        private List<GrdPintura> ObtenerListadoPintura(int proyectoId)
        {
            List<GrdPintura> reqP = null;
            DateTime? date = null;

            using (SamContext ctx = new SamContext())
            {
                reqP =
                       (from wks in ctx.WorkstatusSpool
                        join ots in ctx.OrdenTrabajoSpool on wks.OrdenTrabajoSpoolID equals ots.OrdenTrabajoSpoolID
                        join ot in ctx.OrdenTrabajo on ots.OrdenTrabajoID equals ot.OrdenTrabajoID
                        join s in ctx.Spool on ots.SpoolID equals s.SpoolID
                        join req in ctx.RequisicionPinturaDetalle on wks.WorkstatusSpoolID equals req.WorkstatusSpoolID
                        join pnt in ctx.PinturaSpool on wks.WorkstatusSpoolID equals pnt.WorkstatusSpoolID into Pintura
                        from pintura in Pintura.DefaultIfEmpty()
                        where s.ProyectoID == proyectoId && wks.TieneRequisicionPintura
                        let sph = s.SpoolHold
                        select new GrdPintura
                        {
                            RequisicionPinturaDetalleID = req.RequisicionPinturaDetalleID,
                            OrdenTrabajoID = ots.OrdenTrabajoID,
                            OrdenTrabajo = ot.NumeroOrden,
                            OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID,
                            WorkstatusSpoolID = wks.WorkstatusSpoolID,
                            NombreSpool = s.Nombre,
                            NumeroControl = ots.NumeroControl,
                            Sistema = s.SistemaPintura,
                            Color = s.ColorPintura,
                            Codigo = s.CodigoPintura,
                            FechaSandBlast = (pintura == null) ? date : pintura.FechaSandblast.Value,
                            ReporteSandBlast = (pintura == null) ? string.Empty : pintura.ReporteSandblast,
                            FechaPrimario = (pintura == null) ? date : pintura.FechaPrimarios.Value,
                            ReportePrimario = (pintura == null) ? string.Empty : pintura.ReportePrimarios,
                            FechaIntermedio = (pintura == null) ? date : pintura.FechaIntermedios.Value,
                            ReporteIntermedio = (pintura == null) ? string.Empty : pintura.ReporteIntermedios,
                            FechaAcabadoVisual = (pintura == null) ? date : pintura.FechaAcabadoVisual.Value,
                            ReporteAcabadoVisual = (pintura == null) ? string.Empty : pintura.ReporteAcabadoVisual,
                            FechaAdherencia = (pintura == null) ? date : pintura.FechaAdherencia.Value,
                            ReporteAdherencia = (pintura == null) ? string.Empty : pintura.ReporteAdherencia,
                            FechaPullOff = (pintura == null) ? date : pintura.FechaPullOff.Value,
                            ReportePullOff = (pintura == null) ? string.Empty : pintura.ReportePullOff,
                            Liberado = wks.LiberadoPintura,
                            Hold = sph != null ? ((sph.TieneHoldCalidad) || (sph.TieneHoldIngenieria) || (sph.Confinado)) : false,
                        }).ToList();

            }
            return reqP;
        }

        #endregion
    }
}
