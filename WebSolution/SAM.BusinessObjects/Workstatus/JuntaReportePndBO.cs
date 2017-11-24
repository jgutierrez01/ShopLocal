using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Validations;
using SAM.Entities;
using SAM.Entities.Grid;
using Mimo.Framework.Exceptions;
using System.Globalization;
using SAM.BusinessObjects.Ingenieria;
using SAM.Entities.Personalizadas;
using SAM.BusinessObjects.Excepciones;

namespace SAM.BusinessObjects.Workstatus
{
    public class JuntaReportePndBO
    {
        private static readonly object _mutex = new object();
        private static JuntaReportePndBO _instance;

        /// <summary>
        /// Constructor privado
        /// </summary>
        private JuntaReportePndBO()
        {

        }

        /// <summary>
        /// Obtiene la instancia de la clase ReportePndBO
        /// </summary>
        public static JuntaReportePndBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new JuntaReportePndBO();
                    }
                }
                return _instance;
            }
        }

        public List<GrdDetReportePnd> ObtenerJuntaReportePnd(int reportePnd)
        {
        //join jr in ctx.JuntaRequisicion on jr.JuntaRequisicionID equals jrpnd.JuntaRequisicionID
        //join r in ctx.Requisicion on r.RequisicionID equals jr.RequisicionID
            using (SamContext ctx = new SamContext())
            {
                List<GrdDetReportePnd> lst = (from jrpnd in ctx.JuntaReportePnd
                        join jwks in ctx.JuntaWorkstatus on jrpnd.JuntaWorkstatusID equals jwks.JuntaWorkstatusID
                        join jsp in ctx.JuntaSpool on jwks.JuntaSpoolID equals jsp.JuntaSpoolID
                        join sp in ctx.Spool on jsp.SpoolID equals sp.SpoolID
                        join ots in ctx.OrdenTrabajoSpool on sp.SpoolID equals ots.SpoolID
                        join jr in ctx.JuntaRequisicion on jrpnd.JuntaRequisicionID equals jr.JuntaRequisicionID
                        join r in ctx.Requisicion on jr.RequisicionID equals r.RequisicionID
                        join fa1 in ctx.FamiliaAcero on jsp.FamiliaAceroMaterial1ID equals fa1.FamiliaAceroID
                        join fm1 in ctx.FamiliaMaterial on fa1.FamiliaMaterialID equals  fm1.FamiliaMaterialID
                        join tj in ctx.TipoJunta on jsp.TipoJuntaID equals tj.TipoJuntaID
                        join fa2 in ctx.FamiliaAcero on jsp.FamiliaAceroMaterial2ID equals fa2.FamiliaAceroID into fam2
                        from famb2 in fam2.DefaultIfEmpty()
                        where jrpnd.ReportePndID == reportePnd
                        select new GrdDetReportePnd
                        {
                            JuntaReportePndID = jrpnd.JuntaReportePndID,
                            NumeroDeRequisicion = r.NumeroRequisicion,
                            NumeroDeControl = ots.NumeroControl,
                            Hoja = jrpnd.Hoja,
                            Fecha = jrpnd.FechaPrueba,
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
                            Aprobado = jrpnd.Aprobado,
                            Observaciones = jrpnd.Observaciones,
                            Spool = sp.Nombre,
                            SpoolID = sp.SpoolID,
                            TieneHold = false
                        }).ToList();

                foreach (GrdDetReportePnd r in lst)
                {
                    r.TieneHold = SpoolHoldBO.Instance.TieneHold(r.SpoolID);
                }

                return lst;
            }
        }

        public bool ReporteAprobado(int juntaReportePnd,SamContext ctx)
        {
                return ctx.JuntaReportePnd.First(x => x.JuntaReportePndID == juntaReportePnd).Aprobado;
        }

        public void Borra(int juntaReportePndID)
        {
            bool tieneHold = false;
            using (SamContext ctx = new SamContext())
            {

                tieneHold = (from jr in ctx.JuntaReportePnd
                             join jw in ctx.JuntaWorkstatus on jr.JuntaWorkstatusID equals jw.JuntaWorkstatusID
                             join js in ctx.JuntaSpool on jw.JuntaSpoolID equals js.JuntaSpoolID
                             join sh in ctx.SpoolHold on js.SpoolID equals sh.SpoolID
                             where jr.JuntaReportePndID == juntaReportePndID
                             && (sh.TieneHoldIngenieria || sh.TieneHoldCalidad)
                             select jr).Any();

                if (tieneHold)
                {
                    throw new BaseValidationException(CultureInfo.CurrentCulture.Name == "en-US" ?
                        "This joint can not be be removed by the related Spool is in Hold" :
                        "Esta junta no puede ser eliminada por que el Spool relacionado se encuentra en Hold");
                }
                JuntaReportePnd JuntaReportePnd = ctx.JuntaReportePnd.Where(x => x.JuntaReportePndID == juntaReportePndID).SingleOrDefault();

                JuntaWorkstatus juntaWorkstatus = ctx.JuntaWorkstatus.Where(x => x.JuntaWorkstatusID == JuntaReportePnd.JuntaWorkstatusID).FirstOrDefault();
                WorkstatusSpool ws = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID == juntaWorkstatus.OrdenTrabajoSpoolID).FirstOrDefault();

                if (ws != null)
                {
                    if (ws.TieneRequisicionPintura || ws.TienePintura)
                    {
                        throw new ExcepcionRelaciones(MensajesError.Excepcion_TieneRequiPintura);
                    }
                }

                //Verifica si el reporte esta aprobado
                if (ReporteAprobado(juntaReportePndID, ctx))
                {
                    //si esta aprobado borra el archivo correspondiente.
                    ctx.DeleteObject(JuntaReportePnd);
                    ctx.SaveChanges();
                }
                else
                {
                    //si no esta aprobado verifica si la junta esta activa, si no esta activa trae el 
                    //id de la junta relacionada donde la juntaWorkStatus anterior sea igual a la seleccionafa
                    int? JuntaWorkStatusActiva = ObtenerJuntaPndActiva(juntaReportePndID, ctx);

                    if (JuntaWorkStatusActiva != null)
                    {
                        //checa si la junta tiene relaciones
                        if (VerificaRelacionesJuntaWS(JuntaWorkStatusActiva, ctx))
                        {
                            //borra los registros relacionados y el registro seleccionado
                            BorrarRelaciones(JuntaWorkStatusActiva, juntaReportePndID, ctx);
                        }
                    }
                }
            }
        }

        public int? ObtenerJuntaPndActiva(int juntaReportePndID,SamContext ctx)
        {
                int WorkStatus =
                    ctx.JuntaReportePnd
                       .First(x => x.JuntaReportePndID == juntaReportePndID)
                       .JuntaWorkstatusID;
               JuntaWorkstatus JWorkStarus = ctx.JuntaWorkstatus
                                                .FirstOrDefault(x => x.JuntaWorkstatusAnteriorID == WorkStatus);

                 if(JWorkStarus != null)
                {
                    return JWorkStarus.JuntaWorkstatusID;
                }
                return null;
            
        }

        public bool VerificaRelacionesJuntaWS(int? juntaWorkStatus, SamContext ctx)
        {
            return ValidacionesJuntaReportePnd.TieneRelacionesJunta(juntaWorkStatus, ctx);
        }

        public void BorrarRelaciones(int? juntaWorkStatus, int juntaReportePnd, SamContext ctx)
        {
            
                JuntaWorkstatus JuntaWorkStatusDel = ctx.JuntaWorkstatus
                                                        .First(x => x.JuntaWorkstatusID == juntaWorkStatus);

                JuntaArmado JuntaArmado = ctx.JuntaArmado
                    .Where(x => x.JuntaArmadoID == JuntaWorkStatusDel.JuntaArmadoID)
                    .SingleOrDefault();

                JuntaReportePnd JuntaReportePnd = ctx.JuntaReportePnd
                                                     .First(x => x.JuntaReportePndID == juntaReportePnd);

                IQueryable<JuntaReportePndCuadrante> JuntaReportePndCuadrante = ctx.JuntaReportePndCuadrante
                                                                                   .Where(x => x.JuntaReportePndID == 
                                                                                       JuntaReportePnd.JuntaReportePndID);

                IQueryable<JuntaReportePndSector> JuntaReportePndSector = ctx.JuntaReportePndSector
                                                                             .Where(x => x.JuntaReportePndID ==
                                                                                 JuntaReportePnd.JuntaReportePndID);


                if (JuntaArmado != null)
                {
                    ctx.DeleteObject(JuntaArmado);
                    ctx.SaveChanges();
                }
                ctx.DeleteObject(JuntaWorkStatusDel);

                BorrarJuntasReportesPndCandidate(ctx, JuntaReportePnd, JuntaReportePndCuadrante);

                BorrarJuntasReportesPndSector(ctx, JuntaReportePnd, JuntaReportePndSector);

                JuntaWorkstatus JuntaWorkStatus =
                    ctx.JuntaWorkstatus.First(x => x.JuntaWorkstatusID == JuntaReportePnd.JuntaWorkstatusID);

                JuntaWorkStatus.JuntaFinal = true;
                ctx.JuntaWorkstatus.ApplyChanges(JuntaWorkStatus);

                ctx.DeleteObject(JuntaReportePnd);
                ctx.SaveChanges();

            
        }

        private void BorrarJuntasReportesPndSector(SamContext ctx, JuntaReportePnd JuntaReportePnd, IQueryable<JuntaReportePndSector> JuntaReportePndSector)
        {
            if (JuntaReportePndSector != null)
            {
                JuntaReportePnd.StartTracking();

                foreach (var jRPndS in JuntaReportePndSector)
                {
                    ctx.JuntaReportePndSector.DeleteObject(jRPndS);
                }

                JuntaReportePnd.StopTracking();

                ctx.JuntaReportePnd.ApplyChanges(JuntaReportePnd);

            }
        }

        private void BorrarJuntasReportesPndCandidate(SamContext ctx, JuntaReportePnd JuntaReportePnd, IQueryable<JuntaReportePndCuadrante> JuntaReportePndCuadrante)
        {
            if (JuntaReportePndCuadrante != null)
            {
                JuntaReportePnd.StartTracking();

                foreach (var jRPndC in JuntaReportePndCuadrante)
                {
                    ctx.DeleteObject(jRPndC);
                }

                JuntaReportePnd.StopTracking();

                ctx.JuntaReportePnd.ApplyChanges(JuntaReportePnd);

            }
        }

        public JuntaReportePnd ObtenerJuntaReportePndConReportes(int reportePndID)
        {
            Dictionary<int, string> defectos = CacheCatalogos.Instance.ObtenerDefectos().ToDictionary(x => x.ID,
                                                                                                      y => y.Nombre);
            using (SamContext ctx = new SamContext())
            {
                JuntaReportePnd juntaReportePnd = ctx.JuntaReportePnd.Single(x => x.JuntaReportePndID == reportePndID);
                ctx.LoadProperty(juntaReportePnd,"JuntaReportePndCuadrante");
                ctx.LoadProperty(juntaReportePnd, "JuntaReportePndSector");
                ctx.Defecto.ToList();
                
                //para poner el idioma que es el nombre del defecto
                juntaReportePnd.JuntaReportePndCuadrante.ToList().ForEach(x=>
                                                                              {
                                                                                  if(x.DefectoID.HasValue)
                                                                                  {
                                                                                      x.Defecto.Nombre =
                                                                                          defectos[x.DefectoID.Value];
                                                                                  }
                                                                              });
                juntaReportePnd.JuntaReportePndSector.ToList().ForEach(x =>
                                                                           {
                                                                               x.Defecto.Nombre =
                                                                                   defectos[x.DefectoID];
                                                                           });

                return juntaReportePnd;
            }
        }
        public string ObtenerDatosJuntaReferenciada(int jwsID)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntaReportePnd jrp = null;
                string Mensaje = "";
                int juntaId = ctx.JuntaWorkstatus.Where(x => x.JuntaWorkstatusID == jwsID).Select(x => x.JuntaSpoolID).SingleOrDefault();
                if(juntaId > 0)
                {
                    jrp = ctx.JuntaReportePnd
                        .Include("ReportePnd")
                        .Where(x => (x.JuntaSeguimientoID1 == juntaId || x.JuntaSeguimientoID2 == juntaId) && !x.Aprobado).FirstOrDefault();
                }

                if (jrp != null)
                {
                    string referencia = (from js in ctx.JuntaSpool
                                         join odts in ctx.OrdenTrabajoSpool on js.SpoolID equals odts.SpoolID
                                         join jws in ctx.JuntaWorkstatus on js.JuntaSpoolID equals jws.JuntaSpoolID
                                         where jws.JuntaWorkstatusID == jrp.JuntaWorkstatusID
                                         select odts.NumeroControl + "-" + js.Etiqueta).SingleOrDefault();

                    Mensaje = CultureInfo.CurrentCulture.Name == "en-US" ?
                        string.Format("* This joint is referenced by the joint number: {0}, in the RT report number: {1}", referencia, jrp.ReportePnd.NumeroReporte) :
                        string.Format("* Esta junta esta referenciada por la junta: {0}, en el reporte de RT numero: {1}", referencia, jrp.ReportePnd.NumeroReporte);
                }

                return Mensaje;
            }
        }
    }
}