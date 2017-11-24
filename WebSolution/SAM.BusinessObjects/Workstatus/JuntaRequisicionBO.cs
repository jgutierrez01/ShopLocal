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
    public class JuntaRequisicionBO
    {
        //variables de instancia
        private static readonly object _mutex = new object();
        private static JuntaRequisicionBO _instance;

        /// <summary>
        /// constructor para implementar el patrón Singleton.
        /// </summary>
        private JuntaRequisicionBO()
        {
        }

        /// <summary>
        /// permite la creación de una instancia de la clase
        /// </summary>
        public static JuntaRequisicionBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new JuntaRequisicionBO();                            
                    }
                }
                return _instance;
            }
        }

        public List<GrdDetRequisiciones> DetalleRequisiciones(int requisicionID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<Requisicion> iqRequisicion = ctx.Requisicion.Where(x => x.RequisicionID == requisicionID);
                IQueryable<JuntaRequisicion> iqJuntaRequisicion = ctx.JuntaRequisicion.Where(x => iqRequisicion.Select(y => y.RequisicionID).Contains(x.RequisicionID));
                IQueryable<JuntaWorkstatus> iqJuntaWorkstatus = ctx.JuntaWorkstatus.Where(x => iqJuntaRequisicion.Select(y => y.JuntaWorkstatusID).Contains(x.JuntaWorkstatusID));
                IQueryable<JuntaSpool> iqJuntaSpool = ctx.JuntaSpool.Where(x => iqJuntaWorkstatus.Select(y => y.JuntaSpoolID).Contains(x.JuntaSpoolID));
                IQueryable<Spool> iqSpool = ctx.Spool.Where(x => iqJuntaSpool.Select(y => y.SpoolID).Contains(x.SpoolID));
                IQueryable<OrdenTrabajoSpool> iqOrdenTrabajoSpool = ctx.OrdenTrabajoSpool.Where(x => iqJuntaWorkstatus.Select(y => y.OrdenTrabajoSpoolID).Contains(x.OrdenTrabajoSpoolID));
                IQueryable<OrdenTrabajo> iqOrdenTrabajo = ctx.OrdenTrabajo.Where(x => iqOrdenTrabajoSpool.Select(y => y.OrdenTrabajoID).Contains(x.OrdenTrabajoID));
                IQueryable<TipoJunta> iqTipoJunta = ctx.TipoJunta.Where(x => iqJuntaSpool.Select(y => y.TipoJuntaID).Contains(x.TipoJuntaID));

                List<GrdDetRequisiciones> lst = (from r    in iqRequisicion
                                                 join jr   in iqJuntaRequisicion  on r.RequisicionID        equals jr.RequisicionID
                                                 join jw   in iqJuntaWorkstatus   on jr.JuntaWorkstatusID   equals jw.JuntaWorkstatusID
                                                 join js   in iqJuntaSpool        on jw.JuntaSpoolID        equals js.JuntaSpoolID
                                                 join s    in iqSpool             on js.SpoolID             equals s.SpoolID
                                                 join odts in iqOrdenTrabajoSpool on jw.OrdenTrabajoSpoolID equals odts.OrdenTrabajoSpoolID
                                                 join odt  in iqOrdenTrabajo      on odts.OrdenTrabajoID    equals odt.OrdenTrabajoID
                                                 join tj   in iqTipoJunta         on js.TipoJuntaID         equals tj.TipoJuntaID

                                                 select new GrdDetRequisiciones 
                                                 {
                                                     JuntaRequisicionID = jr.JuntaRequisicionID,
                                                     OrdenTrabajo = odt.NumeroOrden,
                                                     NumeroControl = odts.NumeroControl,
                                                     Spool = s.Nombre,
                                                     Junta = jw.EtiquetaJunta,
                                                     Localizacion = js.EtiquetaMaterial1 + " - " + js.EtiquetaMaterial2,
                                                     Tipo = tj.Codigo,
                                                     Cedula = js.Cedula,
                                                     Material1 = ctx.FamiliaAcero.Where(x => x.FamiliaAceroID == s.FamiliaAcero1ID).FirstOrDefault().Nombre,
                                                     Material2 = ctx.FamiliaAcero.Where(x => x.FamiliaAceroID == s.FamiliaAcero2ID).FirstOrDefault().Nombre == null ? 
                                                        string.Empty : ctx.FamiliaAcero.Where(x => x.FamiliaAceroID == s.FamiliaAcero2ID).FirstOrDefault().Nombre,
                                                     Diametro = js.Diametro,
                                                     SpoolID = s.SpoolID,
                                                     TieneHold = false
                                                 }).ToList();

                foreach (GrdDetRequisiciones r in lst)
                {
                    r.TieneHold = SpoolHoldBO.Instance.TieneHold(r.SpoolID);
                }

                return lst;
            }
        }

        /// <summary>
        /// Elimina una junta de una requisicion para prueba.
        /// </summary>
        /// <param name="juntaRequisicionID"></param>
        public void Borra(int juntaRequisicionID)
        {
            using (SamContext ctx = new SamContext())
            {
                bool tieneMovimientos = ValidacionesRequisicion.TieneReporte(ctx, juntaRequisicionID);

                if (!tieneMovimientos)
                {
                    JuntaRequisicion juntaRequisicion =
                        ctx.JuntaRequisicion.Where(
                            x => x.JuntaRequisicionID == juntaRequisicionID).SingleOrDefault();

                    ctx.DeleteObject(juntaRequisicion);
                    ctx.SaveChanges();
                }
                else
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RequisicionConReporte);
                }
            }
        }
    }
}
