using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Validations;
using SAM.Entities;
using SAM.Entities.Grid;

namespace SAM.BusinessObjects.Workstatus
{
    public class DetRequisicionPinturaBO
    {
        private static readonly object _mutex = new object();
        private static DetRequisicionPinturaBO _instance;

        /// <summary>
        /// Constructor privado
        /// </summary>
        private DetRequisicionPinturaBO()
        {

        }

        /// <summary>
        /// Obtiene la instancia de la clase 
        /// </summary>
        public static DetRequisicionPinturaBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new DetRequisicionPinturaBO();
                    }
                }
                return _instance;
            }
        }

        public List<GrdDetReporteReqPintura> ObtenerDetalleRequisicionPintura(int requisicionPinturaID)
        {

            using (SamContext ctx = new SamContext())
            {
                List<GrdDetReporteReqPintura> lst = (from rrp in ctx.RequisicionPinturaDetalle
                                                     join wks in ctx.WorkstatusSpool on rrp.WorkstatusSpoolID equals wks.WorkstatusSpoolID
                                                     join ots in ctx.OrdenTrabajoSpool on wks.OrdenTrabajoSpoolID equals ots.OrdenTrabajoSpoolID
                                                     join spool in ctx.Spool on ots.SpoolID equals spool.SpoolID
                                                     where rrp.RequisicionPinturaID == requisicionPinturaID
                                                     select new GrdDetReporteReqPintura
                                                     {
                                                         RequisicionPinturaDetalleID = rrp.RequisicionPinturaDetalleID,
                                                         NumeroDeControl = ots.NumeroControl,
                                                         Spool = spool.Nombre,
                                                         Sistema = spool.SistemaPintura,
                                                         Codigo = spool.CodigoPintura,
                                                         Color = spool.ColorPintura,
                                                         SpoolId = spool.SpoolID,
                                                         TieneHold = false
                                                     }).ToList();

                foreach (GrdDetReporteReqPintura r in lst)
                {
                    r.TieneHold = SpoolHoldBO.Instance.TieneHold(r.SpoolId);
                }

                return lst;
            }
        }

        public void Borra(int requisicionPinturaDetalleID)
        {
            using (SamContext ctx = new SamContext())
            {
                bool tieneMovimientos = ValidacionesRequisicionPinturaDetalle.TienePinturaSpool(ctx, requisicionPinturaDetalleID);

                if (!tieneMovimientos)
                {
                    RequisicionPinturaDetalle requisicionPinturaDetalle =
                        ctx.RequisicionPinturaDetalle.Where(
                            x => x.RequisicionPinturaDetalleID == requisicionPinturaDetalleID).SingleOrDefault();

                    WorkstatusSpool wks = ctx.WorkstatusSpool.Where(x => x.WorkstatusSpoolID == requisicionPinturaDetalle.WorkstatusSpoolID).SingleOrDefault();
                    wks.StartTracking();
                    wks.TieneRequisicionPintura = false;

                    ctx.DeleteObject(requisicionPinturaDetalle);
                    ctx.SaveChanges();
                }
                else
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_TienePinturaSpool);
                }
            }
        }
    }
}
