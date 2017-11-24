using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities.Grid;
using SAM.Entities;
using SAM.BusinessObjects.Utilerias;
using Mimo.Framework.Extensions;

namespace SAM.BusinessLogic.Calidad
{
    public class DetalleJuntaWorkstatus : DetalleJunta
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="juntaSpoolID"></param>
        /// <param name="juntaID"></param>
        public DetalleJuntaWorkstatus(int proyectoID, int juntaSpoolID, int ? juntaID)
        {
            this.ProyectoID = proyectoID;
            this.JuntaSpoolID = juntaSpoolID;
            this.JuntaID = juntaID;
        }

        public override void  ComplementaInformacion(SamContext ctx, GrdSeguimientoJunta junta)
        {
            if (JuntaID.HasValue && JuntaID.Value > 0)
            {
                JuntaWorkstatus juntaWorkstatus = (from jw in ctx.JuntaWorkstatus where jw.JuntaWorkstatusID == JuntaID select jw).SingleOrDefault();
                IDictionary<int, string> dicDefectos = CacheCatalogos.Instance.ObtenerDefectos().ToDictionary(x => x.ID, y => y.Nombre);

                if (juntaWorkstatus != null)
                {
                    #region Cargar propiedades hijas relacionadas con pruebas y soldadura

                    ctx.LoadProperty<JuntaWorkstatus>(juntaWorkstatus, jw => jw.JuntaReportePnd);
                    ctx.LoadProperty<JuntaWorkstatus>(juntaWorkstatus, jw => jw.Soldadura);
                    ctx.LoadProperty<JuntaWorkstatus>(juntaWorkstatus, jw => jw.JuntaReporteTt);

                    if (juntaWorkstatus.Soldadura != null)
                    {
                        ctx.LoadProperty<JuntaSoldadura>(juntaWorkstatus.Soldadura, jw => jw.JuntaSoldaduraDetalle);
                    }

                    IQueryable<int> iRpndIds = juntaWorkstatus.JuntaReportePnd.Select(t => t.ReportePndID).AsQueryable();
                    IQueryable<int> iRpttIds = juntaWorkstatus.JuntaReporteTt.Select(t => t.ReporteTtID).AsQueryable();
                    IQueryable<int> iReqIds = juntaWorkstatus.JuntaRequisicion.Select(jr => jr.RequisicionID).AsQueryable();
                    IQueryable<int> iJRpndIds = juntaWorkstatus.JuntaReportePnd.Select(jrp => jrp.JuntaReportePndID).AsQueryable();

                    ctx.ReportePnd.Where(x => iRpndIds.Contains(x.ReportePndID)).ToList();
                    ctx.ReporteTt.Where(x => iRpttIds.Contains(x.ReporteTtID)).ToList();
                    ctx.JuntaRequisicion.Where(jr => jr.JuntaWorkstatusID == JuntaID).ToList();
                    ctx.Requisicion.Where(r => iReqIds.Contains(r.RequisicionID)).ToList();
                    ctx.JuntaReportePndSector.Where(jrps => iJRpndIds.Contains(jrps.JuntaReportePndID)).ToList();
                    ctx.JuntaReportePndCuadrante.Where(jrps => iJRpndIds.Contains(jrps.JuntaReportePndID)).ToList();
                    ctx.Soldador.ToList();

                    ctx.Consumible.Where(c => ctx.Proyecto
                                                 .Where(p => p.ProyectoID == ProyectoID)
                                                 .Select(p => p.PatioID)
                                                 .Contains(c.PatioID))
                                  .ToList();

                    #endregion

                    #region PruebaPT

                    JuntaReportePnd juntaPnd = juntaWorkstatus.JuntaReportePnd
                                                              .SingleOrDefault(x => x.ReportePnd != null && x.ReportePnd.TipoPruebaID == (int)TipoPruebaEnum.ReportePT);

                    if (juntaPnd != null)
                    {
                        string[] param = new string[4];
                        List<GrdSegJuntaDetPNDCuad> cuad;
                        List<GrdSegJuntaDetPNDSector> sector;
                        DetallePruebas(out param[0], out param[1], out sector, out cuad, juntaPnd, dicDefectos);
                        junta.PruebaPTObservacionesReporte = param[0];
                        junta.PruebaPTObservacionesRequisicion = param[1];
                        junta.PruebaPTPndSector = sector;
                        junta.PruebaPTPndCuad = cuad;
                    }

                    #endregion

                    #region PruebaRT

                    juntaPnd = juntaWorkstatus.JuntaReportePnd
                                              .SingleOrDefault(x => x.ReportePnd != null && x.ReportePnd.TipoPruebaID == (int)TipoPruebaEnum.ReporteRT);

                    if (juntaPnd != null)
                    {
                        string[] param = new string[4];
                        List<GrdSegJuntaDetPNDCuad> cuad;
                        List<GrdSegJuntaDetPNDSector> sector;
                        DetallePruebas(out param[0], out param[1], out sector, out cuad, juntaPnd, dicDefectos);
                        junta.PruebaRTObservacionesReporte = param[0];
                        junta.PruebaRTObservacionesRequisicion = param[1];
                        junta.PruebaRTPndSector = sector;
                        junta.PruebaRTPndCuad = cuad;
                    }

                    #endregion

                    #region Prueba UT

                    juntaPnd = juntaWorkstatus.JuntaReportePnd
                                              .SingleOrDefault(x => x.ReportePnd != null && x.ReportePnd.TipoPruebaID == (int)TipoPruebaEnum.ReporteUT);

                    if (juntaPnd != null)
                    {
                        string[] param = new string[4];
                        List<GrdSegJuntaDetPNDCuad> cuad;
                        List<GrdSegJuntaDetPNDSector> sector;
                        DetallePruebas(out param[0], out param[1], out sector, out cuad, juntaPnd, dicDefectos);
                        junta.PruebaUTObservacionesReporte = param[0];
                        junta.PruebaUTObservacionesRequisicion = param[1];
                        junta.PruebaUTPndSector = sector;
                        junta.PruebaUTPndCuad = cuad;
                    }

                    #endregion

                    #region PruebaPTPostTT

                    juntaPnd = juntaWorkstatus.JuntaReportePnd
                                              .SingleOrDefault(x => x.ReportePnd != null && x.ReportePnd.TipoPruebaID == (int)TipoPruebaEnum.PTPostTT);

                    if (juntaPnd != null)
                    {
                        string[] param = new string[4];
                        List<GrdSegJuntaDetPNDCuad> cuad;
                        List<GrdSegJuntaDetPNDSector> sector;
                        DetallePruebas(out param[0], out param[1], out sector, out cuad, juntaPnd, dicDefectos);
                        junta.PruebaPTPostTTObservacionesReporte = param[0];
                        junta.PruebaPTPostTTObservacionesRequisicion = param[1];
                        junta.PruebaPTPostTTPndSector = sector;
                        junta.PruebaPTPostTTPndCuad = cuad;
                    }

                    #endregion

                    #region PruebaRTPostTT

                    juntaPnd = juntaWorkstatus.JuntaReportePnd
                                              .SingleOrDefault(x => x.ReportePnd != null && x.ReportePnd.TipoPruebaID == (int)TipoPruebaEnum.RTPostTT);

                    if (juntaPnd != null)
                    {
                        string[] param = new string[4];
                        List<GrdSegJuntaDetPNDCuad> cuad;
                        List<GrdSegJuntaDetPNDSector> sector;
                        DetallePruebas(out param[0], out param[1], out sector, out cuad, juntaPnd, dicDefectos);
                        junta.PruebaRTPostTTObservacionesReporte = param[0];
                        junta.PruebaRTPostTTObservacionesRequisicion = param[1];
                        junta.PruebaRTPostTTPndSector = sector;
                        junta.PruebaRTPostTTPndCuad = cuad;
                    }

                    #endregion

                    #region Soldadura

                    JuntaSoldadura soldadura = juntaWorkstatus.Soldadura;

                    if (soldadura != null)
                    {
                        junta.SoldaduraDetalle = soldadura.JuntaSoldaduraDetalle
                                                             .Select(x =>
                                                                 new GrdSegJuntaDetSoldadura
                                                                 {
                                                                     CodigoSoldador = x.Soldador.Codigo,
                                                                     Consumible = x.Consumible != null ? x.Consumible.Codigo : string.Empty,
                                                                     Nombre = x.Soldador.Nombre +
                                                                              (string.IsNullOrEmpty(x.Soldador.ApPaterno) ? string.Empty : " " + x.Soldador.ApPaterno) +
                                                                              (string.IsNullOrEmpty(x.Soldador.ApMaterno) ? string.Empty : " " + x.Soldador.ApMaterno),
                                                                     Proceso = TraductorEnumeraciones.TextoTecnicaSoldador(x.TecnicaSoldadorID)
                                                                 })
                                                             .ToList();
                    }

                    #endregion

                    #region PruebaDurezas

                    JuntaReporteTt juntaTt = juntaWorkstatus.JuntaReporteTt
                                                            .OrderByDescending(x => x.FechaTratamiento)
                                                            .ThenByDescending(x => x.FechaModificacion)
                                                            .FirstOrDefault(x => x.ReporteTt != null && x.ReporteTt.TipoPruebaID == (int)TipoPruebaEnum.Durezas);

                    if (juntaTt != null)
                    {
                        junta.TratamientoDurezasObservacionesReporte = juntaTt.Observaciones;
                        junta.TratamientoDurezasObservacionesRequisicion = juntaTt.JuntaRequisicion.Requisicion.Observaciones;
                    }

                    #endregion
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observacionesReporte"></param>
        /// <param name="observacionesRequisicion"></param>
        /// <param name="sector"></param>
        /// <param name="cuadrante"></param>
        /// <param name="juntaPnd"></param>
        /// <param name="diccDefectos"></param>
        protected static void DetallePruebas(out string observacionesReporte, out string observacionesRequisicion, out List<GrdSegJuntaDetPNDSector> sector, out List<GrdSegJuntaDetPNDCuad> cuadrante, JuntaReportePnd juntaPnd, IDictionary<int, string> diccDefectos)
        {
            observacionesReporte = juntaPnd.Observaciones;
            observacionesRequisicion = juntaPnd.JuntaRequisicion.Requisicion.Observaciones;

            sector =
                (from junta in juntaPnd.JuntaReportePndSector
                 select new GrdSegJuntaDetPNDSector
                 {
                     A = junta.SectorFin,
                     De = junta.SectorInicio,
                     Defecto = diccDefectos.SafeTryGetValue(junta.DefectoID),
                     Sector = junta.Sector
                 }).ToList();

            cuadrante = (from junta in juntaPnd.JuntaReportePndCuadrante
                         select new GrdSegJuntaDetPNDCuad
                         {
                             Cuadrante = junta.Cuadrante,
                             Defecto = diccDefectos.SafeTryGetValue(junta.DefectoID.Value),
                             Placa = junta.Placa
                         }).ToList();
        }
    }
}
