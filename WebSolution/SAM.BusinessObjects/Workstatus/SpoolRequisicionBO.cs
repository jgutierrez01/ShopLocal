using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.BusinessObjects.Validations;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Ingenieria;

namespace SAM.BusinessObjects.Workstatus
{
    public class SpoolRequisicionBO
    {
        //variables de instancia
        private static readonly object _mutex = new object();
        private static SpoolRequisicionBO _instance;

        /// <summary>
        /// constructor para implementar el patrón Singleton.
        /// </summary>
        private SpoolRequisicionBO()
        {
        }

        /// <summary>
        /// permite la creación de una instancia de la clase
        /// </summary>
        public static SpoolRequisicionBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new SpoolRequisicionBO();                            
                    }
                }
                return _instance;
            }
        }

        public List<GrdDetRequisicionesSpool> DetalleRequisicionesSpool(int requisicionSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<RequisicionSpool> iqRequisicionSpool = ctx.RequisicionSpool.Where(x => x.RequisicionSpoolID == requisicionSpoolID);
                IQueryable<SpoolRequisicion> iqSpoolRequisicion = ctx.SpoolRequisicion.Where(x => iqRequisicionSpool.Select(y => y.RequisicionSpoolID).Contains(x.RequisicionSpoolID));
                IQueryable<WorkstatusSpool> iqWorkstatusSpool = ctx.WorkstatusSpool.Where(x => iqSpoolRequisicion.Select(y => y.WorkstatusSpoolID).Contains(x.WorkstatusSpoolID));
                IQueryable<OrdenTrabajoSpool> iqOrdenTrabajoSpool = ctx.OrdenTrabajoSpool.Where(x => iqWorkstatusSpool.Select(y => y.OrdenTrabajoSpoolID).Contains(x.OrdenTrabajoSpoolID));
                IQueryable<Spool> iqSpool = ctx.Spool.Where(x => iqOrdenTrabajoSpool.Select(y => y.SpoolID).Contains(x.SpoolID));
                IQueryable<OrdenTrabajo> iqOrdenTrabajo = ctx.OrdenTrabajo.Where(x => iqOrdenTrabajoSpool.Select(y => y.OrdenTrabajoID).Contains(x.OrdenTrabajoID));

                List<GrdDetRequisicionesSpool> lst = (from rs    in iqRequisicionSpool
                                                      join sr    in iqSpoolRequisicion   on rs.RequisicionSpoolID    equals sr.RequisicionSpoolID
                                                      join wks   in iqWorkstatusSpool    on sr.WorkstatusSpoolID     equals wks.WorkstatusSpoolID
                                                      join ots   in iqOrdenTrabajoSpool  on wks.OrdenTrabajoSpoolID  equals ots.OrdenTrabajoSpoolID
                                                      join s     in iqSpool              on ots.SpoolID              equals s.SpoolID
                                                      join ot    in iqOrdenTrabajo       on ots.OrdenTrabajoID       equals ot.OrdenTrabajoID
 
                                                      select new GrdDetRequisicionesSpool
                                                      {
                                                          SpoolRequisicionID = sr.SpoolRequisicionID,
                                                          OrdenTrabajo = ot.NumeroOrden,
                                                          NumeroControl = ots.NumeroControl,
                                                          Spool = s.Nombre,
                                                          Material1 = ctx.FamiliaAcero.Where(x => x.FamiliaAceroID == s.FamiliaAcero1ID).FirstOrDefault().Nombre,
                                                          Material2 = ctx.FamiliaAcero.Where(x => x.FamiliaAceroID == s.FamiliaAcero2ID).FirstOrDefault().Nombre == null ? 
                                                            string.Empty : ctx.FamiliaAcero.Where(x => x.FamiliaAceroID == s.FamiliaAcero2ID).FirstOrDefault().Nombre,
                                                          SpoolID = s.SpoolID,
                                                          TieneHold = false
                                                      }).ToList();
                
                foreach(GrdDetRequisicionesSpool r in lst)
                {
                    r.TieneHold = SpoolHoldBO.Instance.TieneHold(r.SpoolID);
                }

                return lst;
            }
        }

        /// <summary>
        /// Elimina un spool de una requisicion para prueba.
        /// </summary>
        /// <param name="juntaRequisicionID"></param>
        public void Borra(int spoolRequisicionID)
        {
            using (SamContext ctx = new SamContext())
            {
                bool tieneMovimientos = ValidacionesRequisicion.TieneReporteSpool(ctx, spoolRequisicionID);

                if (!tieneMovimientos)
                {
                    SpoolRequisicion spoolRequisicion =
                        ctx.SpoolRequisicion.Where(x => x.SpoolRequisicionID == spoolRequisicionID).SingleOrDefault();

                    ctx.DeleteObject(spoolRequisicion);
                    ctx.SaveChanges();
                }
                else
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RequisicionSpoolConReporte);
                }
            }
        }
    }
}
