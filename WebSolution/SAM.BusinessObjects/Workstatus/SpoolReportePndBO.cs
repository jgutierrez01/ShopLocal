using System;
using System.Collections.Generic;
using System.Linq;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Validations;
using SAM.Entities;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Cache;
using System.Transactions;
using Mimo.Framework.Extensions;
using System.Data.Objects;
using Mimo.Framework.Common;
using SAM.BusinessObjects.Administracion;
using SAM.BusinessObjects.Ingenieria;

namespace SAM.BusinessObjects.Workstatus
{
    public class SpoolReportePndBO
    {
        private static readonly object _mutex = new object();
        private static SpoolReportePndBO _instance;

        /// <summary>
        /// Constructor privado
        /// </summary>
        private SpoolReportePndBO()
        {

        }

        /// <summary>
        /// Obtiene la instancia de la clase SpoolReportePndBO
        /// </summary>
        public static SpoolReportePndBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new SpoolReportePndBO();
                    }
                }
                return _instance;
            }
        }

        public List<GrdDetReporteSpoolPnd> ObtenerSpoolReportePnd(int reporteSpoolPnd)
        {
            using (SamContext ctx = new SamContext())
            {
                List<GrdDetReporteSpoolPnd> lst = (from srpnd in ctx.SpoolReportePnd
                        join wks in ctx.WorkstatusSpool on srpnd.WorkstatusSpoolID equals wks.WorkstatusSpoolID
                        join ots in ctx.OrdenTrabajoSpool on wks.OrdenTrabajoSpoolID equals ots.OrdenTrabajoSpoolID
                        join sp in ctx.Spool on ots.SpoolID equals sp.SpoolID
                        join sr in ctx.SpoolRequisicion on srpnd.SpoolRequisicionID equals sr.SpoolRequisicionID
                        join rs in ctx.RequisicionSpool on sr.RequisicionSpoolID equals rs.RequisicionSpoolID
                        where srpnd.ReporteSpoolPndID == reporteSpoolPnd
                        select new GrdDetReporteSpoolPnd
                        {
                            SpoolReportePndID = srpnd.SpoolReportePndID,
                            NumeroDeRequisicion = rs.NumeroRequisicion,
                            NumeroDeControl = ots.NumeroControl,
                            Hoja = srpnd.Hoja,
                            Fecha = srpnd.FechaPrueba,
                            Aprobado = srpnd.Aprobado,
                            Observaciones = srpnd.Observaciones,
                            Spool = sp.Nombre,
                            SpoolID = sp.SpoolID,
                            TieneHold = false
                        }).ToList();

                foreach (GrdDetReporteSpoolPnd r in lst)
                {
                    r.TieneHold = SpoolHoldBO.Instance.TieneHold(r.SpoolID);
                }

                return lst;
            }
        }

        public bool ReporteAprobado(int spoolReportePnd, SamContext ctx)
        {
            return ctx.SpoolReportePnd.First(x => x.SpoolReportePndID == spoolReportePnd).Aprobado;
        }

        public void Borra(int spoolReportePndID)
        {
            using (SamContext ctx = new SamContext())
            {
                //Verifica si el reporte esta aprobado
                if (ReporteAprobado(spoolReportePndID, ctx))
                {
                    //si esta aprobado borra el archivo correspondiente.
                    SpoolReportePnd spoolReportePnd = ctx.SpoolReportePnd.Where(x => x.SpoolReportePndID == spoolReportePndID).SingleOrDefault();
                    ctx.DeleteObject(spoolReportePnd);
                    ctx.SaveChanges();
                }
            }
        }
    }
}
