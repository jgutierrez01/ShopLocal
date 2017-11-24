using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using System.Data.Objects;
using SAM.BusinessObjects.Validations;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Cache;
using Mimo.Framework.Extensions;
using SAM.Entities.Personalizadas;
using System.Transactions;
using Mimo.Framework.Data;

namespace SAM.BusinessObjects.Workstatus
{
    public class SeguimientoSpoolBO
    {
        private static readonly object _mutex = new object();
        private static SeguimientoSpoolBO _instance;

         /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private SeguimientoSpoolBO()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static SeguimientoSpoolBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new SeguimientoSpoolBO();
                    }
                }
                return _instance;
            }
        }

        public WorkstatusSpool ObtenerWorkstatusSpoolPorOrdenDeTrabajoSpoolID(int ordenDeTrabajoSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID == ordenDeTrabajoSpoolID).SingleOrDefault();
            }
        }

        public List<GrdMobileDetalleSeguimientoSpool> ObtenerDetalleJuntas(int spoolID)
        {
            List<GrdMobileDetalleSeguimientoSpool> juntas;

            int ReporteRT = TipoPruebaEnum.ReporteRT.SafeIntParse();
            int ReportePT = TipoPruebaEnum.ReportePT.SafeIntParse();
            int TipoPWHT = TipoPruebaEnum.Pwht.SafeIntParse();

            using (SamContext ctx = new SamContext())
            {
                juntas = (from js in ctx.JuntaSpool
                          join jw in ctx.JuntaWorkstatus on js.JuntaSpoolID equals jw.JuntaSpoolID into Wks
                          from jwks in Wks.DefaultIfEmpty()
                          where js.SpoolID == spoolID
                          select new GrdMobileDetalleSeguimientoSpool
                          {
                              JuntaSpoolID = js.JuntaSpoolID,
                              Etiqueta = js.Etiqueta,
                              Tipo = js.TipoJunta.Codigo,
                              InspeccionVisual = (jwks == null) ? false : jwks.InspeccionVisualAprobada,
                              RT = (jwks == null) ? false : ctx.JuntaReportePnd.Any(x => x.JuntaWorkstatusID == jwks.JuntaWorkstatusID && x.Aprobado && x.ReportePnd.TipoPruebaID == ReporteRT),
                              PT = (jwks == null) ? false : ctx.JuntaReportePnd.Any(x => x.JuntaWorkstatusID == jwks.JuntaWorkstatusID && x.Aprobado && x.ReportePnd.TipoPruebaID == ReportePT),
                              PWHT = (jwks == null) ? false : ctx.JuntaReporteTt.Any(x => x.JuntaWorkstatusID == jwks.JuntaWorkstatusID && x.Aprobado && x.ReporteTt.TipoPruebaID == TipoPWHT)
                          }
                          ).OrderBy(x => x.Etiqueta).ToList();
                
            }
            juntas.ForEach(x =>
            {
                x.InspeccionVisualTexto = TraductorEnumeraciones.TextoSiNo(x.InspeccionVisual);
                x.RTTexto = TraductorEnumeraciones.TextoSiNo(x.RT);
                x.PTTexto = TraductorEnumeraciones.TextoSiNo(x.PT);
                x.PWHTTexto = TraductorEnumeraciones.TextoSiNo(x.PWHT);
            });

            return juntas;
        }

        public GrdMobileDetalleLiberaciónDimensional ObtenerReporteDimensional(int workstatusSpoolID)
        {
            GrdMobileDetalleLiberaciónDimensional infoReporte;

            using (SamContext ctx = new SamContext())
            {
                infoReporte = (from rdd in ctx.ReporteDimensionalDetalle
                               join rd in ctx.ReporteDimensional on rdd.ReporteDimensionalID equals rd.ReporteDimensionalID
                               where rdd.WorkstatusSpoolID == workstatusSpoolID 
                               && rdd.Aprobado && rd.TipoReporteDimensionalID == (int)TipoReporteDimensionalEnum.Dimensional
                               select new GrdMobileDetalleLiberaciónDimensional
                               {
                                   FechaLiberacion = rdd.FechaLiberacion,
                                   FechaReporte = rd.FechaReporte,
                                   NumeroReporte = rd.NumeroReporte
                               }
                               ).SingleOrDefault();
            }
            return infoReporte;
        }

        public RequisicionPintura ObtenerRequisicionPintura(int workstatusSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return (from rp in ctx.RequisicionPintura
                        join rpd in ctx.RequisicionPinturaDetalle on rp.RequisicionPinturaID equals rpd.RequisicionPinturaID
                        where rpd.WorkstatusSpoolID == workstatusSpoolID
                        select rp
                        ).SingleOrDefault();
            }
        }

        public PinturaSpool ObtenerPinturaSpool(int workstatusSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.PinturaSpool.Where(x => x.WorkstatusSpoolID == workstatusSpoolID).SingleOrDefault();
            }
        }

        public Embarque ObtenerEmbarqueParaSpool(int workstatusSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return (from em in ctx.Embarque
                        join ems in ctx.EmbarqueSpool on em.EmbarqueID equals ems.EmbarqueID
                        where ems.WorkstatusSpoolID == workstatusSpoolID
                        select em
                        ).SingleOrDefault();
            }
        }

        public SpoolHold ObtenerHoldPorSpoolID(int spoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.SpoolHold.Where(x => x.SpoolID == spoolID).SingleOrDefault();
            }
        }

        public DataSet ObtenerPruebasNoDestructivasPorSpoolID(int spoolID)
        {
            DataSet ds = new DataSet();
            const string nombreProc = "ObtenerInfoPorcPnd";
            using (IDbConnection connection = DataAccessFactory.CreateConnection("SamDB"))
            {
                IDbDataParameter[] parameters = DataAccess.GetSpParameterSet("SamDB", nombreProc);
                parameters[0].Value = spoolID;


                ds = DataAccess.ExecuteDataset(connection,
                                               CommandType.StoredProcedure,
                                               nombreProc,
                                               ds,
                                               "PruebasNoDestructivas",
                                               parameters);
            }
            return ds;
        }
    }
}
