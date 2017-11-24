using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.BusinessObjects.Excepciones;

namespace SAM.BusinessObjects.Workstatus
{
    public class ReporteDimensionalDetalleBO
    {
        private static readonly object _mutex = new object();
        private static ReporteDimensionalDetalleBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private ReporteDimensionalDetalleBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase ReporteDimensionalBO
        /// </summary>
        /// <returns></returns>
        public static ReporteDimensionalDetalleBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ReporteDimensionalDetalleBO();
                    }
                }
                return _instance;
            }
        }

        public void Borrar(int reporteDimensionalDetalleID, Guid userID)
        {
            using(SamContext ctx = new SamContext())
            {
                ReporteDimensionalDetalle rdd = ctx.ReporteDimensionalDetalle.Where(x => x.ReporteDimensionalDetalleID == reporteDimensionalDetalleID).Single();
                ReporteDimensional rd = ctx.ReporteDimensional.Where(x => x.ReporteDimensionalID == rdd.ReporteDimensionalID).Single();
                //TipoReporteDimensional trd = ctx.TipoReporteDimensional.Where(x => x.TipoReporteDimensionalID == rd.TipoReporteDimensionalID).Single();

                if (rd.TipoReporteDimensionalID == (int)TipoReporteDimensionalEnum.Dimensional)
                {
                    WorkstatusSpool ws = ctx.WorkstatusSpool.Where(x => x.WorkstatusSpoolID == rdd.WorkstatusSpoolID).Single();
                    
                    if (ws != null)
                    {
                        if (ws.TieneRequisicionPintura || ws.TienePintura)
                        {
                            throw new ExcepcionRelaciones(MensajesError.Excepcion_TieneRequiPintura);
                        }
                    }

                    ws.StartTracking();
                    ws.TieneLiberacionDimensional = false;
                    ws.FechaModificacion = DateTime.Now;
                    ws.StopTracking();
                    
                }

                ctx.DeleteObject(rdd);
                ctx.SaveChanges();
            }
        }
    }
}
