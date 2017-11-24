using System.Collections.Generic;
using System.Linq;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Excepciones;

namespace SAM.BusinessObjects.Workstatus
{
    public class JuntaReporteTtBO
    {
        private static readonly object _mutex = new object();
        private static JuntaReporteTtBO _instance;

        /// <summary>
        /// Constructor privado
        /// </summary>
        private JuntaReporteTtBO()
        {

        }

        /// <summary>
        /// Obtiene la instancia de la clase ReportePndBO
        /// </summary>
        public static JuntaReporteTtBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new JuntaReporteTtBO();
                    }
                }
                return _instance;
            }
        }

        public List<GrdDetReporteTt> ObtenerJuntaReporteTt(int reporteTt)
        {
            using (SamContext ctx = new SamContext())
            {
                List<GrdDetReporteTt> lst = (from jrTT in ctx.JuntaReporteTt
                                             join jwks in ctx.JuntaWorkstatus on jrTT.JuntaWorkstatusID equals jwks.JuntaWorkstatusID
                                             join jsp in ctx.JuntaSpool on jwks.JuntaSpoolID equals jsp.JuntaSpoolID
                                             join fa1 in ctx.FamiliaAcero on jsp.FamiliaAceroMaterial1ID equals fa1.FamiliaAceroID
                                             join fm1 in ctx.FamiliaMaterial on fa1.FamiliaMaterialID equals fm1.FamiliaMaterialID
                                             join sp in ctx.Spool on jsp.SpoolID equals sp.SpoolID
                                             join ots in ctx.OrdenTrabajoSpool on sp.SpoolID equals ots.SpoolID
                                             join jr in ctx.JuntaRequisicion on jrTT.JuntaRequisicionID equals jr.JuntaRequisicionID
                                             join r in ctx.Requisicion on jr.RequisicionID equals r.RequisicionID
                                             join tj in ctx.TipoJunta on jsp.TipoJuntaID equals tj.TipoJuntaID
                                             join fa2 in ctx.FamiliaAcero on jsp.FamiliaAceroMaterial2ID equals fa2.FamiliaAceroID into fam2
                                             from famb2 in fam2.DefaultIfEmpty()
                                             where jrTT.ReporteTtID == reporteTt
                                             select new GrdDetReporteTt
                                             {
                                                 JuntaReporteTtID = jrTT.JuntaReporteTtID,
                                                 NumeroDeRequisicion = r.NumeroRequisicion,
                                                 NumeroDeControl = ots.NumeroControl,
                                                 Hoja = jrTT.Hoja,
                                                 Fecha = jrTT.FechaTratamiento,
                                                 Grafica = jrTT.NumeroGrafica,
                                                 Junta = jwks.EtiquetaJunta,
                                                 Localizacion = jsp.EtiquetaMaterial1 + "-" +
                                                                jsp.EtiquetaMaterial2,
                                                 Tipo = tj.Codigo,
                                                 Cedula = jsp.Cedula,
                                                 FamiliaAceroMaterial1 = fa1.Nombre,
                                                 FamiliaAceroMaterial2 = famb2 == null
                                                              ? string.Empty
                                                              : famb2.Nombre,
                                                 FamiliaAceroMaterial1ID = jsp.FamiliaAceroMaterial1ID,
                                                 FamiliaAceroMaterial2ID = jsp.FamiliaAceroMaterial2ID,
                                                 Diametro = jsp.Diametro,
                                                 Aprobado = jrTT.Aprobado,
                                                 Observaciones = jrTT.Observaciones,
                                                 Spool = sp.Nombre,
                                                 SpoolID = sp.SpoolID,
                                                 TieneHold = false
                                             }).ToList();

                foreach (GrdDetReporteTt r in lst)
                {
                    r.TieneHold = SpoolHoldBO.Instance.TieneHold(r.SpoolID);
                }

                return lst;
            }
        }

        public void Borra(int juntaReporteTtID)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntaReporteTt JuntaReporteTt = ctx.JuntaReporteTt.Where(x => x.JuntaReporteTtID == juntaReporteTtID).SingleOrDefault();

                JuntaWorkstatus js = ctx.JuntaWorkstatus.Where(x => x.JuntaWorkstatusID == JuntaReporteTt.JuntaWorkstatusID).FirstOrDefault();
                WorkstatusSpool ws = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID == js.OrdenTrabajoSpoolID).FirstOrDefault();
               
                if (ws != null)
                {
                    if (ws.TieneRequisicionPintura || ws.TienePintura)
                    {
                        throw new ExcepcionRelaciones(MensajesError.Excepcion_TieneRequiPintura);
                    }
                }

                ctx.DeleteObject(JuntaReporteTt);
                ctx.SaveChanges();
            }
        }
    }
}
