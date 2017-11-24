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
    public class DetalleJuntaCampo : DetalleJunta
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="juntaSpoolID"></param>
        /// <param name="juntaID"></param>
        public DetalleJuntaCampo(int proyectoID, int juntaSpoolID, int ? juntaID)
        {
            this.ProyectoID = proyectoID;
            this.JuntaSpoolID = juntaSpoolID;
            this.JuntaID = juntaID;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="junta"></param>
        public override void ComplementaInformacion(SamContext ctx, GrdSeguimientoJunta junta)
        {
            if (JuntaID.HasValue && JuntaID.Value > 0)
            {
                JuntaCampo juntaCampo = (from jc in ctx.JuntaCampo where jc.JuntaCampoID == JuntaID select jc).SingleOrDefault();
                IDictionary<int, string> dicDefectos = CacheCatalogos.Instance.ObtenerDefectos().ToDictionary(x => x.ID, y => y.Nombre);

                if (juntaCampo != null)
                {
                    #region Cargar propiedades hijas relacionadas con pruebas y soldadura

                    ctx.LoadProperty<JuntaCampo>(juntaCampo, jc => jc.JuntaCampoReportePND);
                    ctx.LoadProperty<JuntaCampo>(juntaCampo, jc => jc.JuntaCampoSoldadura);
                    ctx.LoadProperty<JuntaCampo>(juntaCampo, jc => jc.JuntaCampoReporteTT);

                    if (juntaCampo.JuntaCampoSoldadura != null)
                    {
                        ctx.LoadProperty<JuntaCampoSoldadura>(juntaCampo.JuntaCampoSoldadura, jcs => jcs.JuntaCampoSoldaduraDetalle);
                    }


                    IQueryable<int> iRpndIds = juntaCampo.JuntaCampoReportePND.Select(t => t.ReporteCampoPNDID).AsQueryable();
                    IQueryable<int> iRpttIds = juntaCampo.JuntaCampoReporteTT.Select(t => t.ReporteCampoTTID).AsQueryable();
                    IQueryable<int> iReqIds = juntaCampo.JuntaCampoRequisicion.Select(jr => jr.RequisicionCampoID).AsQueryable();
                    IQueryable<int> iJRpndIds = juntaCampo.JuntaCampoReportePND.Select(jrp => jrp.JuntaCampoReportePNDID).AsQueryable();

                    ctx.ReporteCampoPND.Where(x => iRpndIds.Contains(x.ReporteCampoPNDID)).ToList();
                    ctx.ReporteCampoTT.Where(x => iRpttIds.Contains(x.ReporteCampoTTID)).ToList();
                    ctx.JuntaCampoRequisicion.Where(jr => jr.JuntaCampoID == JuntaID).ToList();
                    ctx.RequisicionCampo.Where(r => iReqIds.Contains(r.RequisicionCampoID)).ToList();
                    ctx.JuntaCampoReportePNDSector.Where(jrps => iJRpndIds.Contains(jrps.JuntaCampoReportePNDID)).ToList();
                    ctx.JuntaCampoReportePNDCuadrante.Where(jrps => iJRpndIds.Contains(jrps.JuntaCampoReportePNDID)).ToList();
                    ctx.Soldador.ToList();

                    ctx.Consumible.Where(c => ctx.Proyecto
                                                 .Where(p => p.ProyectoID == ProyectoID)
                                                 .Select(p => p.PatioID)
                                                 .Contains(c.PatioID))
                                  .ToList();

                    #endregion

                    #region PruebaPT

                    JuntaCampoReportePND juntaPnd = juntaCampo.JuntaCampoReportePND
                                                              .SingleOrDefault(x => x.ReporteCampoPND != null && x.ReporteCampoPND.TipoPruebaID == (int)TipoPruebaEnum.ReportePT);

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

                    juntaPnd = juntaCampo.JuntaCampoReportePND
                                              .SingleOrDefault(x => x.ReporteCampoPND != null && x.ReporteCampoPND.TipoPruebaID == (int)TipoPruebaEnum.ReporteRT);

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

                    juntaPnd = juntaCampo.JuntaCampoReportePND
                                              .SingleOrDefault(x => x.ReporteCampoPND != null && x.ReporteCampoPND.TipoPruebaID == (int)TipoPruebaEnum.ReporteUT);

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

                    juntaPnd = juntaCampo.JuntaCampoReportePND
                                              .SingleOrDefault(x => x.ReporteCampoPND != null && x.ReporteCampoPND.TipoPruebaID == (int)TipoPruebaEnum.PTPostTT);

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

                    juntaPnd = juntaCampo.JuntaCampoReportePND
                                              .SingleOrDefault(x => x.ReporteCampoPND != null && x.ReporteCampoPND.TipoPruebaID == (int)TipoPruebaEnum.RTPostTT);

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

                    JuntaCampoSoldadura soldadura = juntaCampo.JuntaCampoSoldadura;

                    if (soldadura != null)
                    {
                        junta.SoldaduraDetalle = soldadura.JuntaCampoSoldaduraDetalle
                                                             .Select(x =>
                                                                 new GrdSegJuntaDetSoldadura
                                                                 {
                                                                     CodigoSoldador = x.Soldador.Codigo,
                                                                     Consumible = x.Consumible.Codigo,
                                                                     Nombre = x.Soldador.Nombre +
                                                                              (string.IsNullOrEmpty(x.Soldador.ApPaterno) ? string.Empty : " " + x.Soldador.ApPaterno) +
                                                                              (string.IsNullOrEmpty(x.Soldador.ApMaterno) ? string.Empty : " " + x.Soldador.ApMaterno),
                                                                     Proceso = TraductorEnumeraciones.TextoTecnicaSoldador(x.TecnicaSoldadorID.HasValue ? x.TecnicaSoldadorID.Value : 2)
                                                                 })
                                                             .ToList();
                    }

                    #endregion

                    #region PruebaDurezas

                    JuntaCampoReporteTT juntaTt = juntaCampo.JuntaCampoReporteTT
                                                            .OrderByDescending(x => x.FechaTratamiento)
                                                            .ThenByDescending(x => x.FechaModificacion)
                                                            .FirstOrDefault(x => x.ReporteCampoTT != null && x.ReporteCampoTT.TipoPruebaID == (int)TipoPruebaEnum.Durezas);

                    if (juntaTt != null)
                    {
                        junta.TratamientoDurezasObservacionesReporte = juntaTt.Observaciones;
                        junta.TratamientoDurezasObservacionesRequisicion = string.Empty;
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
        protected static void DetallePruebas(out string observacionesReporte, out string observacionesRequisicion, out List<GrdSegJuntaDetPNDSector> sector, out List<GrdSegJuntaDetPNDCuad> cuadrante, JuntaCampoReportePND juntaPnd, IDictionary<int, string> diccDefectos)
        {
            observacionesReporte = juntaPnd.Observaciones;
            observacionesRequisicion = string.Empty;

            sector =
                (from junta in juntaPnd.JuntaCampoReportePNDSector
                 select new GrdSegJuntaDetPNDSector
                 {
                     A = junta.SectorFin,
                     De = junta.SectorInicio,
                     Defecto = diccDefectos.SafeTryGetValue(junta.DefectoID.HasValue ? junta.DefectoID.Value : -1),
                     Sector = junta.Sector
                 }).ToList();

            cuadrante = (from junta in juntaPnd.JuntaCampoReportePNDCuadrante
                         select new GrdSegJuntaDetPNDCuad
                         {
                             Cuadrante = junta.Cuadrante,
                             Defecto = diccDefectos.SafeTryGetValue(junta.DefectoID.HasValue ? junta.DefectoID.Value : -1),
                             Placa = junta.Placa
                         }).ToList();
        }
    }
}
