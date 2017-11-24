using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.RadCombo;
using SAM.BusinessObjects.Modelo;
using System.Data.Objects;
using SAM.Entities.Grid;
using SAM.Entities;
using SAM.Entities.Personalizadas;
using System.Transactions;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Utilerias;
using Mimo.Framework.Extensions;
using Mimo.Framework.Common;
using Mimo.Framework.Exceptions;
using System.Threading;

namespace SAM.BusinessObjects.Catalogos
{
    public class JuntaCampoBO 
    {
        private static readonly object _mutex = new object();
        private static JuntaCampoBO _instance;

        /// <summary>
        /// Cultura actual de la página
        /// </summary>
        protected string Cultura
        {
            get
            {
                return Thread.CurrentThread.CurrentUICulture.Name;
            }
        }

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private JuntaCampoBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase ItemCodeBO
        /// </summary>
        /// <returns></returns>
        public static JuntaCampoBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new JuntaCampoBO();
                    }
                }
                return _instance;
            }
        }

        public JuntaCampo ObtenerPorID(int juntaCampoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.JuntaCampo.Include("JuntaSpool").Where(x => x.JuntaCampoID == juntaCampoID).SingleOrDefault();
            }
        }

        public JuntaCampo ObtenerPorJuntaSpoolID(int juntaSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.JuntaCampo.Where(x => x.JuntaSpoolID == juntaSpoolID && x.JuntaFinal).SingleOrDefault();
            }
        }

        public List<RadSpool> ObtenerSpoolParaCombo(int proyectoID, int ordenTrabajoID, string contextText, int skip, int take)
        {
            int fieldFabAreaID = CacheCatalogos.Instance.FieldFabAreaID;
            int jacketFabAreaID = CacheCatalogos.Instance.JacketFabAreaID;

            List<RadSpool> result = new List<RadSpool>(take * 2);

            using (SamContext ctx = new SamContext())
            {
                ctx.Spool.MergeOption = MergeOption.NoTracking;

                result =
                    (from spool in ctx.Spool.Where(y => y.ProyectoID == proyectoID)
                     join odt in ctx.OrdenTrabajoSpool.Where(y => y.OrdenTrabajoID == (ordenTrabajoID != -1 ? ordenTrabajoID : y.OrdenTrabajoID)) on
                     spool.SpoolID equals odt.SpoolID
                     join junta in ctx.JuntaSpool on spool.SpoolID equals junta.SpoolID
                     where (junta.FabAreaID == fieldFabAreaID || junta.FabAreaID == jacketFabAreaID)
                     select new RadSpool
                     {
                         SpoolID = spool.SpoolID,
                         Spool = spool.Nombre,
                         Etiqueta = junta.Etiqueta
                     }).ToList();

                return result.Where(x => x.Spool.ContainsIgnoreCase(contextText)).
                    OrderBy(x => x.Spool)
                    .Skip(skip)
                    .Take(take)
                    .ToList();
            }
        }

        public List<GrdJuntaCampo> ObtenerListadoJuntaCampo(int proyecto, int ordenTrabajo, int numeroControl, int spool)
        {
            int fieldFabAreaID = CacheCatalogos.Instance.FieldFabAreaID;
            int jacketFabAreaID = CacheCatalogos.Instance.JacketFabAreaID;
            List<GrdJuntaCampo> lista = new List<GrdJuntaCampo>();

            using (SamContext ctx = new SamContext())
            {
                var query = (from sp in ctx.Spool
                             join js in ctx.JuntaSpool on sp.SpoolID equals js.SpoolID
                             join odts in ctx.OrdenTrabajoSpool on sp.SpoolID equals odts.SpoolID
                             join odt in ctx.OrdenTrabajo on odts.OrdenTrabajoID equals odt.OrdenTrabajoID
                             join jc in ctx.JuntaCampo.Where(t => t.JuntaFinal) on js.JuntaSpoolID equals jc.JuntaSpoolID into leftJuntaCampo
                             from left in leftJuntaCampo.DefaultIfEmpty()
                             let hold = sp.SpoolHold
                             where  sp.ProyectoID == proyecto
                                    && (js.FabAreaID == fieldFabAreaID || js.FabAreaID == jacketFabAreaID)
                             select new
                             {
                                 JuntaSpoolID = js.JuntaSpoolID,
                                 JuntaCampoID = left != null ? left.JuntaCampoID : -1,
                                 EtiquetaIngenieria = js.Etiqueta,
                                 EtiquetaProduccion = left != null ? left.EtiquetaJunta : js.Etiqueta,
                                 Spool = sp.Nombre,
                                 SpoolID = sp.SpoolID,
                                 NumeroOrdenTrabajo = odt.NumeroOrden,
                                 NumeroControl = odts.NumeroControl,
                                 EtiquetaMaterial1 = js.EtiquetaMaterial1,
                                 EtiquetaMaterial2 = js.EtiquetaMaterial2,
                                 OrdenTrabajoID = odt.OrdenTrabajoID,
                                 OrdenTrabajoSpoolID = odts.OrdenTrabajoSpoolID,
                                 ArmadoAprobado = left.ArmadoAprobado,
                                 SoldaduraAprobada = left.SoldaduraAprobada,
                                 InspVisualAprobada = left.InspeccionVisualAprobada,
                                 TieneHold = (hold != null ? hold.TieneHoldCalidad || hold.TieneHoldIngenieria || hold.Confinado : false)
                             }).AsQueryable();

                if (ordenTrabajo > 0)
                {
                    query = query.Where(q => q.OrdenTrabajoID == ordenTrabajo);
                }

                if (numeroControl > 0)
                {
                    query = query.Where(q => q.OrdenTrabajoSpoolID == numeroControl);
                }

                if (spool > 0)
                {
                    query = query.Where(q => q.SpoolID == spool);
                }

                var listaResultados = query.ToList();

                var iqsJtasCampo = listaResultados.Where(i => i.JuntaCampoID > 0).Select(i => i.JuntaCampoID).AsQueryable();

                var rechazosPnd = (from pnd in ctx.JuntaCampoReportePND
                                   where !pnd.Aprobado
                                            && iqsJtasCampo.Contains(pnd.JuntaCampoID)
                                   select pnd.JuntaCampoID)
                                   .Distinct()
                                   .ToDictionary<int,int>(x => x);

                lista = (from q in listaResultados
                         select new GrdJuntaCampo
                         {
                            JuntaSpoolID = q.JuntaSpoolID,
                            EtiquetaIngenieria = q.EtiquetaIngenieria,
                            EtiquetaProduccion = q.EtiquetaProduccion,
                            SpoolID = q.SpoolID,
                            Spool = q.Spool,
                            OrdenTrabajoID = q.OrdenTrabajoID,
                            NumeroOrdenTrabajo = q.NumeroOrdenTrabajo,
                            OrdenTrabajoSpoolID = q.OrdenTrabajoSpoolID,
                            NumeroControl = q.NumeroControl,
                            Localizacion = q.EtiquetaMaterial1 + "-" + q.EtiquetaMaterial2,
                            ArmadoAprobado = q.ArmadoAprobado.HasValue ? q.ArmadoAprobado.Value : false,
                            SoldaduraAprobada = q.SoldaduraAprobada.HasValue ? q.SoldaduraAprobada.Value : false,
                            InspeccionVisualAprobada = q.InspVisualAprobada.HasValue ? q.InspVisualAprobada.Value : false,
                            TieneHold = q.TieneHold,
                            TieneRechazoPnd = rechazosPnd.ContainsKey(q.JuntaCampoID)
                         })
                         .OrderBy(e => e.NumeroOrdenTrabajo)
                         .ThenBy(e => e.NumeroControl)
                         .ThenBy(e => e.EtiquetaProduccion.PadLeft(20, '0'))
                         .ToList();
            }

            return lista;
        }

        public JuntasCampoDTO DetalleJunta(int juntaSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntasCampoDTO juntas = (from js in ctx.JuntaSpool.Where(x => x.JuntaSpoolID == juntaSpoolID)
                                         join sp in ctx.Spool on
                                         js.SpoolID equals sp.SpoolID
                                         join ots in ctx.OrdenTrabajoSpool on
                                         js.SpoolID equals ots.SpoolID
                                         join tj in ctx.TipoJunta on
                                         js.TipoJuntaID equals tj.TipoJuntaID
                                         join jc in ctx.JuntaCampo.Where(x => x.JuntaFinal == true) on
                                         js.JuntaSpoolID equals jc.JuntaSpoolID into jtCampo
                                         from jtaCmp in jtCampo.DefaultIfEmpty()                                        
                                         select new JuntasCampoDTO
                                         {
                                             ProyectoID = sp.ProyectoID,
                                             Spool = sp.Nombre,
                                             SpoolID = sp.SpoolID,
                                             Junta = jtaCmp != null ? jtaCmp.EtiquetaJunta : js.Etiqueta,
                                             EtiquetaIngenieria = js.Etiqueta,
                                             JuntaCampoID = jtaCmp != null ? jtaCmp.JuntaCampoID : -1,
                                             NumeroControl = ots.NumeroControl,
                                             EtiquetaMaterial1 = js.EtiquetaMaterial1,
                                             EtiquetaMaterial2 = js.EtiquetaMaterial2,
                                             TipoJunta = tj.Codigo,
                                             Espesor = js.Espesor.HasValue ? js.Espesor.Value : 0
                                         }).FirstOrDefault();

                return juntas;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="spoolID"></param>
        /// <param name="etiquetaMaterial1"></param>
        /// <param name="etiquetaMaterial2"></param>
        /// <param name="nombreSpool"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public List<Simple> ObtenerSpoolsCandidatosParaArmadoCampo(int proyectoID, int spoolID, string etiquetaMaterial1, string etiquetaMaterial2, string nombreSpool, int skip, int take)
        {
            List<Simple> spool = new List<Simple>();

            using (SamContext ctx = new SamContext())
            {
                if (etiquetaMaterial1 == "*" && etiquetaMaterial2 == "*")
                {
                    //Esto no se vale, en este caso no hacemos nada, quizá logger un warning en algún momento
                }
                else if (etiquetaMaterial1 == "*" || etiquetaMaterial2 == "*")
                {
                    List<Simple> lista1 = obtenCandidatosPorJuntasPorProyecto(proyectoID, spoolID, ctx);
                    List<Simple> lista2 = obtenCandidatosSpoolUnSoloTramoPorProyecto(proyectoID, spoolID, ctx);

                    spool = lista1.Union(lista2).OrderBy(s => s.Valor).ToList();
                }
                else
                {
                    //Si caemos en esta parte es FORZOSAMENTE porque ambas etiquetas vienen definidas, en ese caso
                    //los candidatos son únicamente spools del mismo isométrico que tengan armado y soldadura y que aparte tengan la etiqueta2
                    //como etiqueta de material para alguna de las juntas ya armadas y soldadas
                    string etiquetaMaterial = etiquetaMaterial2.PadLeft(15, '0');

                    string dibujo = ctx.Spool
                                       .Where(x => x.SpoolID == spoolID)
                                       .Select(x => x.Dibujo)
                                       .FirstOrDefault();

                    List<Simple> lista1 = obtenCandidatosPorJuntasPorIsometrico(proyectoID, spoolID, ctx, etiquetaMaterial, dibujo);
                    List<Simple> lista2 = obtenCandidatosSpoolUnSoloTramoPorIsometrico(proyectoID, spoolID, ctx, etiquetaMaterial, dibujo);

                    spool = lista1.Union(lista2).OrderBy(s => s.Valor).ToList();
                }


                return spool.Where(x => x.Valor.ContainsIgnoreCase(nombreSpool))
                            .OrderBy(x => x.Valor)
                            .Skip(skip)
                            .Take(take)
                            .ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="spoolID"></param>
        /// <param name="ctx"></param>
        /// <param name="etiquetaMaterial"></param>
        /// <param name="dibujo"></param>
        /// <returns></returns>
        private List<Simple> obtenCandidatosSpoolUnSoloTramoPorIsometrico(int proyectoID, int spoolID, SamContext ctx, string etiquetaMaterial, string dibujo)
        {
            int shopID = CacheCatalogos.Instance.ShopFabAreaID;

            List<Simple> lista = new List<Simple>();

            lista = (from sp in ctx.Spool
                     join odts in ctx.OrdenTrabajoSpool on sp.SpoolID equals odts.SpoolID
                     join odtm in ctx.OrdenTrabajoMaterial on odts.OrdenTrabajoSpoolID equals odtm.OrdenTrabajoSpoolID
                     let  ms = odtm.MaterialSpool
                     where  sp.ProyectoID == proyectoID
                            && sp.SpoolID != spoolID
                            && sp.JuntaSpool.Count(s => s.FabAreaID == shopID) == 0
                            && odtm.NumeroUnicoDespachadoID != null
                            && sp.Dibujo == dibujo
                            && odtm.TieneDespacho == true
                     select new
                     {
                         ID = sp.SpoolID,
                         Valor = sp.Nombre,
                         EtiquetaMaterial = ms.Etiqueta
                     })
                     .ToList()
                     .Where(m => m.EtiquetaMaterial.PadLeft(15, '0') == etiquetaMaterial)
                     .Select(r => new
                     {
                         ID = r.ID,
                         Valor = r.Valor
                     })
                     .Distinct()
                     .Select(s => new Simple
                     {
                         ID = s.ID,
                         Valor = s.Valor
                     }).ToList();
            
            return lista;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="spoolID"></param>
        /// <param name="ctx"></param>
        /// <param name="etiquetaMaterial"></param>
        /// <param name="dibujo"></param>
        /// <returns></returns>
        private List<Simple> obtenCandidatosPorJuntasPorIsometrico(int proyectoID, int spoolID, SamContext ctx, string etiquetaMaterial, string dibujo)
        {
            List<Simple> lista = new List<Simple>();

            lista = (from sp in ctx.Spool
                     join ots in ctx.OrdenTrabajoSpool on sp.SpoolID equals ots.SpoolID
                     join js in ctx.JuntaSpool on sp.SpoolID equals js.SpoolID
                     where  sp.ProyectoID == proyectoID
                            && sp.SpoolID != spoolID
                            && ctx.JuntaWorkstatus
                                  .Where(jw => jw.ArmadoAprobado == true && jw.SoldaduraAprobada == true && jw.JuntaFinal == true)
                                  .Select(jw => jw.JuntaSpoolID)
                                  .Contains(js.JuntaSpoolID)
                            && sp.Dibujo == dibujo
                     select new
                     {
                         ID = sp.SpoolID,
                         Valor = sp.Nombre,
                         EtiquetaMaterial1 = js.EtiquetaMaterial1,
                         EtiquetaMaterial2 = js.EtiquetaMaterial2
                     })
                     .ToList()
                     .Where(js => (js.EtiquetaMaterial1.PadLeft(15, '0') == etiquetaMaterial || js.EtiquetaMaterial2.PadLeft(15, '0') == etiquetaMaterial))
                     .Select(js => new
                     {
                         ID = js.ID,
                         Valor = js.Valor
                     })
                     .Distinct()
                     .Select(s => new Simple
                     {
                         ID = s.ID,
                         Valor = s.Valor
                     }).ToList();
            
            return lista;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="spoolID"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        private List<Simple> obtenCandidatosSpoolUnSoloTramoPorProyecto(int proyectoID, int spoolID, SamContext ctx)
        {
            int shopID = CacheCatalogos.Instance.ShopFabAreaID;

            List<Simple> lista = new List<Simple>();

            lista = (from sp in ctx.Spool
                     join odts in ctx.OrdenTrabajoSpool on sp.SpoolID equals odts.SpoolID
                     join odtm in ctx.OrdenTrabajoMaterial on odts.OrdenTrabajoSpoolID equals odtm.OrdenTrabajoSpoolID
                     where  sp.ProyectoID == proyectoID
                            && sp.SpoolID != spoolID
                            && sp.JuntaSpool.Count(s => s.FabAreaID == shopID) == 0
                            && odtm.NumeroUnicoDespachadoID != null
                            && odtm.TieneDespacho == true
                     select new
                     {
                         ID = sp.SpoolID,
                         Nombre = sp.Nombre
                     })
                     .Distinct()
                     .Select(r => new Simple
                     {
                         ID = r.ID,
                         Valor = r.Nombre
                     }).ToList();

            return lista;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="spoolID"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        private List<Simple> obtenCandidatosPorJuntasPorProyecto(int proyectoID, int spoolID, SamContext ctx)
        {
            List<Simple> lista = new List<Simple>();

            //Si alguna de las dos etiquetas es "*" entonces se debe de poder buscar dentro de todos
            //los spools del proyecto que ya tengan juntas SHOP armadas
            lista = (from sp in ctx.Spool
                     join ots in ctx.OrdenTrabajoSpool on sp.SpoolID equals ots.SpoolID
                     join js in ctx.JuntaSpool on sp.SpoolID equals js.SpoolID
                     where sp.ProyectoID == proyectoID
                            && sp.SpoolID != spoolID
                            && ctx.JuntaWorkstatus
                                  .Where(jw => jw.ArmadoAprobado == true && jw.SoldaduraAprobada == true && jw.JuntaFinal == true)
                                  .Select(jw => jw.JuntaSpoolID)
                                  .Contains(js.JuntaSpoolID)
                     select new
                     {
                         ID = sp.SpoolID,
                         Valor = sp.Nombre
                     })
                     .Distinct()
                     .Select(s => new Simple
                     {
                         ID = s.ID,
                         Valor = s.Valor
                     }).ToList();

            return lista;
        }

        #region Armado

        public JuntaCampoArmado ObtenerArmado(int juntaCampoID)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntaCampoArmado jtaArmado = ctx.JuntaCampoArmado.Include("Spool").Include("Spool1").Include("NumeroUnico").Where(x => x.JuntaCampoID == juntaCampoID).SingleOrDefault();
                return jtaArmado;
            }
        }

        #endregion

        #region Soldadura

        public void GuardaSoldadura(JuntaCampo junta, JuntaCampoSoldadura juntaSoldadura)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                using (SamContext ctx = new SamContext())
                {
                    ctx.JuntaCampo.ApplyChanges(junta);
                    ctx.SaveChanges();

                    juntaSoldadura.JuntaCampoID = junta.JuntaCampoID;
                    ctx.JuntaCampoSoldadura.ApplyChanges(juntaSoldadura);
                    ctx.SaveChanges();

                    junta.StartTracking();
                    junta.JuntaCampoSoldaduraID = juntaSoldadura.JuntaCampoSoldaduraID;

                    ctx.JuntaCampo.ApplyChanges(junta);
                    ctx.SaveChanges();
                }

                ts.Complete();
            }

        }

        public JuntaCampoSoldadura ObtenerSoldadura(int juntaCampoID)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntaCampoSoldadura jtaSoldadura = ctx.JuntaCampoSoldadura.Include("JuntaCampoSoldaduraDetalle").Include("JuntaCampoSoldaduraDetalle.Soldador").Include("JuntaCampoSoldaduraDetalle.Consumible").Where(x => x.JuntaCampoID == juntaCampoID).SingleOrDefault();
                return jtaSoldadura;

            }
        }

        public void BorraSoldadura(int juntaCampoID, Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntaCampo jCampo = ctx.JuntaCampo.Where(x => x.JuntaCampoID == juntaCampoID).FirstOrDefault();
                //Validamos que no tenga inspeccion visual aprobada
                if (jCampo.InspeccionVisualAprobada.Value)
                {
                    throw new ExcepcionSoldadura(MensajesError.Excepcion_InspeccionVisualAprobada);
                }

                WorkstatusSpool wks = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID == jCampo.OrdenTrabajoSpoolID).FirstOrDefault();
                if (wks != null)
                {
                    if (wks.TieneLiberacionDimensional)
                    {
                        throw new ExcepcionSoldadura(MensajesError.Excepcion_TieneLiberacionDimensional);
                    }
                }

                JuntaCampoSoldadura jSoldadura = ctx.JuntaCampoSoldadura.Where(x => x.JuntaCampoID == juntaCampoID).FirstOrDefault();
                List<JuntaCampoSoldaduraDetalle> detalles = ctx.JuntaCampoSoldaduraDetalle.Where(x => x.JuntaCampoSoldadura.JuntaCampoID == juntaCampoID).ToList();

                jCampo.StartTracking();
                jCampo.SoldaduraAprobada = false;
                jCampo.JuntaCampoSoldaduraID = null;
                jCampo.UsuarioModifica = userID;
                jCampo.FechaModificacion = DateTime.Now;
                jCampo.StopTracking();

                ctx.JuntaCampo.ApplyChanges(jCampo);

                detalles.ForEach(ctx.DeleteObject);
                ctx.JuntaCampoSoldadura.DeleteObject(jSoldadura);

                ctx.SaveChanges();
            }
        }

        #endregion

        #region InspeccionVisual

        public void GeneraReporteInspeccionVisual(JuntaCampoInspeccionVisual juntaInspVisual, InspeccionVisualCampo inspVisual, int[] defectos, int juntaCampoID, Guid UserUID)
        {
            using (TransactionScope ts = new TransactionScope())
            {

                using (SamContext ctx = new SamContext())
                {
                    //Si la inspeccion es rechazada se validara que por lo menos se haya dado de alta un defecto
                    if (!juntaInspVisual.Aprobado && defectos.Length == 0)
                    {
                        throw new ExcepcionReportes(MensajesError.Excepcion_RechazadoSinDefectos);
                    }

                    //Validar si el numero de reporte ya existe en la base de datos
                    InspeccionVisualCampo inspVisualExistente = ctx.InspeccionVisualCampo
                                                                   .Where(x => x.NumeroReporte == inspVisual.NumeroReporte && x.ProyectoID == inspVisual.ProyectoID)
                                                                   .SingleOrDefault();

                    InspeccionVisual inspVisualExistenteSHOP = ctx.InspeccionVisual
                                                                .Where(x => x.NumeroReporte == inspVisual.NumeroReporte && x.ProyectoID == inspVisual.ProyectoID)
                                                                   .SingleOrDefault();
                    if (inspVisualExistenteSHOP != null)
                    {
                        if (inspVisualExistenteSHOP.FechaReporte != inspVisual.FechaReporte)
                        {
                            throw new ExcepcionReportes(MensajesError.Excepcion_ReporteExistenteConFechaDiferente);
                        }
                    }

                    if (inspVisualExistente != null)
                    {
                        //Validando que las fechas concuerden
                        if (inspVisualExistente.FechaReporte != inspVisual.FechaReporte)
                        {
                            throw new ExcepcionReportes(MensajesError.Excepcion_ReporteExistenteConFechaDiferente);
                        }
                        else
                        {
                            inspVisual = inspVisualExistente;

                            inspVisual.StartTracking();
                            inspVisual.UsuarioModifica = UserUID;
                            inspVisual.FechaModificacion = DateTime.Now;
                        }
                    }
                    else
                    {
                        inspVisual.UsuarioModifica = UserUID;
                        inspVisual.FechaModificacion = DateTime.Now;
                    }

                    //Verifico que no exista ya un detalle para este spool en especifico en el reporte
                    if (ctx.JuntaCampoInspeccionVisual.Where(x => x.InspeccionVisualCampoID == inspVisual.InspeccionVisualCampoID && x.JuntaCampoID == juntaCampoID).Any())
                    {
                        throw new ExcepcionDuplicados(string.Format(MensajesError.Excepcion_ReporteVisualConDetalleDuplicado));
                    }

                    //Verifico si la Junta no cuenta ya con Inspección Visual aprobada
                    JuntaCampo juntaCampo = ctx.JuntaCampo.Where(x => x.JuntaCampoID == juntaCampoID).Single();

                    if (juntaCampo.InspeccionVisualAprobada.HasValue && juntaCampo.InspeccionVisualAprobada.Value)
                    {
                        throw new ExceptionJuntaCampo(string.Format(MensajesError.Excepcion_JuntaTieneInspeccionVisual));
                    }

                    JuntaCampoInspeccionVisual junta = new JuntaCampoInspeccionVisual
                    {
                        JuntaCampoID = juntaCampoID,
                        FechaInspeccion = juntaInspVisual.FechaInspeccion,
                        Aprobado = juntaInspVisual.Aprobado,
                        Observaciones = juntaInspVisual.Observaciones,
                        UsuarioModifica = UserUID,
                        FechaModificacion = DateTime.Now
                    };

                    juntaCampo.StartTracking();
                    juntaCampo.InspeccionVisualAprobada = juntaInspVisual.Aprobado;
                    juntaCampo.JuntaCampoInspeccionVisual = junta;
                    juntaCampo.UltimoProcesoID = (int)UltimoProcesoEnum.InspeccionVisual;
                    juntaCampo.UsuarioModifica = UserUID;
                    juntaCampo.FechaModificacion = DateTime.Now;
                    juntaCampo.StopTracking();
                    ctx.JuntaCampo.ApplyChanges(juntaCampo);

                    //Si el reporte es rechazado se generan los registros de defectos
                    if (!juntaInspVisual.Aprobado)
                    {
                        foreach (int defectoID in defectos)
                        {
                            JuntaCampoInspeccionVisualDefecto juntaDefecto = new JuntaCampoInspeccionVisualDefecto
                            {
                                DefectoID = defectoID,
                                UsuarioModifica = UserUID,
                                FechaModificacion = DateTime.Now
                            };

                            junta.JuntaCampoInspeccionVisualDefecto.Add(juntaDefecto);
                        }
                    }

                    inspVisual.JuntaCampoInspeccionVisual.Add(junta);

                    if (inspVisualExistente != null)
                    {
                        inspVisual.StopTracking();
                    }

                    ctx.InspeccionVisualCampo.ApplyChanges(inspVisual);
                    ctx.SaveChanges();
                }

                ts.Complete();
            }
        }

        public List<GrdInspeccionVisualCampo> ObtenerListadoInspeccionVisual(int juntaCampoID)
        {

            using (SamContext ctx = new SamContext())
            {
                List<GrdInspeccionVisualCampo> list = (from jc in ctx.JuntaCampo
                                                       join ivc in ctx.JuntaCampoInspeccionVisual on jc.JuntaCampoID equals ivc.JuntaCampoID
                                                       join iv in ctx.InspeccionVisualCampo on ivc.InspeccionVisualCampoID equals iv.InspeccionVisualCampoID
                                                       where jc.JuntaCampoID == juntaCampoID
                                                       select new GrdInspeccionVisualCampo
                                            {
                                                JuntaCampoInspeccionVisualID = ivc.JuntaCampoInspeccionVisualID,
                                                NumeroReporte = iv.NumeroReporte,
                                                FechaInspeccion = ivc.FechaInspeccion,
                                                FechaReporte = iv.FechaReporte,
                                                Aprobado = ivc.Aprobado
                                            }).ToList();

                list.ForEach(x => x.Resultado = TraductorEnumeraciones.TextoAprobadoRechazado(x.Aprobado));
                return list;
            }
        }

        public void EliminarReporteInspeccionVisual(int juntaCampoInspeccionVisualID)
        {
            using (SamContext ctx = new SamContext())
            {                
                JuntaCampoInspeccionVisual jiv = ctx.JuntaCampoInspeccionVisual.Where(x => x.JuntaCampoInspeccionVisualID == juntaCampoInspeccionVisualID).FirstOrDefault();
                
                IQueryable<JuntaCampoInspeccionVisual> ljiv = ctx.JuntaCampoInspeccionVisual.Where(x => x.JuntaCampoID == jiv.JuntaCampoID);
                
                if (jiv.Aprobado && ctx.JuntaCampoRequisicion.Where(x => x.JuntaCampoID == jiv.JuntaCampoID).Any())
                {
                    throw new ExceptionJuntaCampo(MensajesError.Excepcion_JuntaCampoEliminarInspVisual);
                }

                InspeccionVisualCampo ivc = ctx.InspeccionVisualCampo.Where(x => x.InspeccionVisualCampoID == jiv.InspeccionVisualCampoID).FirstOrDefault();
                
                List<JuntaCampoInspeccionVisual> listJuntaCampoInspeccionVisual = ctx.JuntaCampoInspeccionVisual.Where(x => x.JuntaCampoID == jiv.JuntaCampoID).ToList();

                JuntaCampo jc = ctx.JuntaCampo.Where(x => x.JuntaCampoID == jiv.JuntaCampoID).FirstOrDefault();
                jc.StartTracking();
                if (jiv.Aprobado)
                {
                    jc.InspeccionVisualAprobada = false;
                }
                else
                {
                    List<JuntaCampoInspeccionVisualDefecto> ljcivd = ctx.JuntaCampoInspeccionVisualDefecto.Where(x => x.JuntaCampoInspeccionVisualID == juntaCampoInspeccionVisualID).ToList();
                    foreach (JuntaCampoInspeccionVisualDefecto jcivd in ljcivd)
                    {
                        ctx.DeleteObject(jcivd);
                    }
                }

                if (ljiv.Count() <= 1)
                {
                    jc.JuntaCampoInspeccionVisualID = null;
                }
                else
                {
                    jc.JuntaCampoInspeccionVisualID = ljiv.Where(x => x.JuntaCampoInspeccionVisualID != jiv.JuntaCampoInspeccionVisualID).Select(x => x.JuntaCampoInspeccionVisualID).FirstOrDefault();
                }
                jc.StopTracking();
                ctx.JuntaCampo.ApplyChanges(jc);
                ctx.SaveChanges();

                if (ctx.JuntaCampoInspeccionVisual.Where(x => x.InspeccionVisualCampoID == jiv.InspeccionVisualCampoID).Count() <= 1)
                {                                                            
                    ctx.DeleteObject(ivc);
                }
                
                ctx.DeleteObject(jiv);

                ctx.SaveChanges();
            }
        }

        #endregion

        #region Requisiciones

        public void GeneraRequisicion(RequisicionCampo requisicion, int juntaCampoID, Guid UserUID)
        {

            using (SamContext ctx = new SamContext())
            {
                //Validar si el numero de requisicion ya existe en la base de datos para el proyecto y tipo de prueba
                RequisicionCampo requisicionExistente = ctx.RequisicionCampo.Where(x => x.NumeroRequisicion == requisicion.NumeroRequisicion && x.ProyectoID == requisicion.ProyectoID && x.TipoPruebaID == requisicion.TipoPruebaID).SingleOrDefault();
                if (requisicionExistente != null)
                {
                    //Validando que las fechas concuerden
                    if (requisicionExistente.FechaRequisicion != requisicion.FechaRequisicion)
                    {
                        throw new ExcepcionReportes(MensajesError.Excepcion_RequisicionExistenteConFechaDiferente);
                    }
                    else
                    {
                        requisicion = requisicionExistente;

                        requisicion.StartTracking();
                        requisicion.UsuarioModifica = UserUID;
                        requisicion.FechaModificacion = DateTime.Now;
                    }
                }
                else
                {
                    requisicion.UsuarioModifica = UserUID;
                    requisicion.FechaModificacion = DateTime.Now;
                }




                #region Validaciones
                JuntaCampoRequisicion jtaReq = ctx.JuntaCampoRequisicion.Where(x => x.JuntaCampoID == juntaCampoID && x.RequisicionCampo.TipoPruebaID == requisicion.TipoPruebaID).SingleOrDefault();

                //Verifico que si el tipo de prueba es Post-TT o Post-TT exista previamente un resultado positivo de PWHT
                if (requisicion.TipoPruebaID == (int)TipoPruebaEnum.RTPostTT || requisicion.TipoPruebaID == (int)TipoPruebaEnum.PTPostTT)
                {
                    if (!ctx.JuntaCampoReporteTT.Where(x => x.JuntaCampoID == juntaCampoID && x.ReporteCampoTT.TipoPruebaID == (int)TipoPruebaEnum.Pwht && x.Aprobado).Any())
                    {
                        throw new ExcepcionReportes(string.Format(MensajesError.Excepcion_ReportePostSinPWHT));
                    }
                }

                string categoriaPrueba = CacheCatalogos.Instance.ObtenerTiposPrueba().Where(x => x.ID == requisicion.TipoPruebaID).Select(x => x.Categoria).Single();
                switch (categoriaPrueba)
                {
                    //Verifico que no exista ya una requisicion del mismo tipo de prueba a menos que la prueba haya sido rechazada
                    case CategoriaTipoPrueba.TT:
                        if (jtaReq != null)
                        {
                            if (!ctx.JuntaCampoReporteTT.Where(x => x.JuntaCampoID == juntaCampoID && x.JuntaCampoRequisicionID == jtaReq.JuntaCampoRequisicionID && !x.Aprobado).Any())
                            {
                                throw new ExcepcionDuplicados(string.Format(MensajesError.Excepcion_RequisicionPruebaDuplicada, string.Empty));
                            }
                        }
                        break;
                    //Verifico que no exista ya una requisicion del mismo tipo de prueba
                    case CategoriaTipoPrueba.PND:
                        if (jtaReq != null)
                        {
                            if (!ctx.JuntaCampoReportePND.Where(x => x.JuntaCampoID == juntaCampoID && x.JuntaCampoRequisicionID == jtaReq.JuntaCampoRequisicionID && !x.Aprobado).Any())
                            {
                                throw new ExcepcionDuplicados(string.Format(MensajesError.Excepcion_RequisicionPruebaDuplicada, string.Empty));
                            }
                        }
                        break;
                }

                #endregion

                JuntaCampoRequisicion juntaReq = new JuntaCampoRequisicion
                {
                    RequisicionCampo = requisicion,
                    JuntaCampoID = juntaCampoID,
                    UsuarioModifica = UserUID,
                    FechaModificacion = DateTime.Now
                };

                requisicion.JuntaCampoRequisicion.Add(juntaReq);


                if (requisicionExistente != null)
                {
                    requisicion.StopTracking();
                }

                ctx.RequisicionCampo.ApplyChanges(requisicion);
                ctx.SaveChanges();

            }
        }

        public List<Simple> ObtenerListadoRequisicionesPNDPendientes(int juntaCampoID)
        {
            using (SamContext ctx = new SamContext())
            {
                string pnd = CategoriaTipoPrueba.PND.ToString();
                //obtengo todas las requisiciones de la junta
                IQueryable<JuntaCampoRequisicion> requisiciones = ctx.JuntaCampoRequisicion.Where(y => y.JuntaCampoID == juntaCampoID).AsQueryable();

                //obtengo las pruebas PND de la junta
                IQueryable<int> pruebasPND = ctx.JuntaCampoReportePND.Where(x => requisiciones.Select(y => y.JuntaCampoRequisicionID).Contains(x.JuntaCampoRequisicionID)).Select(x => x.JuntaCampoRequisicionID).AsQueryable();

                //obtengo las requisiciones que no tienen prueba
                List<RequisicionCampo> reqCampo = ctx.RequisicionCampo.Where(x => requisiciones.Where(y => !pruebasPND.Contains(y.JuntaCampoRequisicionID)).Select(y => y.RequisicionCampoID).Contains(x.RequisicionCampoID) && x.TipoPrueba.Categoria == pnd).ToList();


                return
                    (from rc in reqCampo
                     select
                         new Simple
                         {
                             ID = rc.RequisicionCampoID,
                             Valor = rc.NumeroRequisicion
                         }).ToList();
            }
        }

        public List<Simple> ObtenerListadoRequisicionesTTPendientes(int juntaCampoID)
        {
            using (SamContext ctx = new SamContext())
            {
                string tt = CategoriaTipoPrueba.TT.ToString();
                //obtengo todas las requisiciones de la junta
                IQueryable<JuntaCampoRequisicion> requisiciones = ctx.JuntaCampoRequisicion.Where(y => y.JuntaCampoID == juntaCampoID).AsQueryable();

                //obtengo las pruebas PND de la junta
                IQueryable<int> pruebasPND = ctx.JuntaCampoReportePND.Where(x => requisiciones.Select(y => y.JuntaCampoRequisicionID).Contains(x.JuntaCampoRequisicionID)).Select(x => x.JuntaCampoRequisicionID).AsQueryable();

                //obtengo las requisiciones que no tienen prueba
                List<RequisicionCampo> reqCampo = ctx.RequisicionCampo.Where(x => requisiciones.Where(y => !pruebasPND.Contains(y.JuntaCampoRequisicionID)).Select(y => y.RequisicionCampoID).Contains(x.RequisicionCampoID) && x.TipoPrueba.Categoria == tt).ToList();


                return
                    (from rc in reqCampo
                     select
                         new Simple
                         {
                             ID = rc.RequisicionCampoID,
                             Valor = rc.NumeroRequisicion
                         }).ToList();
            }
        }

        public RequisicionCampo ObtenerRequisicionCampo(int requisicionCampoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.RequisicionCampo.Where(x => x.RequisicionCampoID == requisicionCampoID).SingleOrDefault();
            }
        }

        public List<GrdRequisicionesCampo> ObtenerListadoRequisiciones(int juntaCampoID)
        {
            using (SamContext ctx = new SamContext())
            {
                List<GrdRequisicionesCampo> list = (from jcr in ctx.JuntaCampoRequisicion
                                                    join rc in ctx.RequisicionCampo.Include("TipoPrueba")
                                                    on jcr.RequisicionCampoID equals rc.RequisicionCampoID
                                                    where jcr.JuntaCampoID == juntaCampoID
                                                    select new GrdRequisicionesCampo
                                                    {
                                                        RequisicionID = jcr.JuntaCampoRequisicionID,
                                                        NumeroRequisicion = rc.NumeroRequisicion,
                                                        Fecha = rc.FechaRequisicion,
                                                        TipoPrueba = Cultura == LanguageHelper.ESPANOL ?  rc.TipoPrueba.Nombre : rc.TipoPrueba.NombreIngles
                                                    }).ToList();
                return list;
            }
        }

        public void EliminarRequisiciones(int juntaCampoRequisicionID)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntaCampoRequisicion jcr = ctx.JuntaCampoRequisicion.Where(x => x.JuntaCampoRequisicionID == juntaCampoRequisicionID).FirstOrDefault();

                if (ctx.JuntaCampoReporteTT.Where(x => x.JuntaCampoRequisicionID == jcr.JuntaCampoRequisicionID).Any() || ctx.JuntaCampoReportePND.Where(x => x.JuntaCampoRequisicionID == jcr.JuntaCampoRequisicionID).Any())
                {
                    throw new ExceptionJuntaCampo(MensajesError.Excepcion_JuntaCampoEliminarRequisicoines);
                }
                else
                {
                    if (ctx.JuntaCampoRequisicion.Where(x => x.RequisicionCampoID == jcr.RequisicionCampoID).Count() <= 1)
                    {
                        RequisicionCampo rc = ctx.RequisicionCampo.Where(x => x.RequisicionCampoID == jcr.RequisicionCampoID).FirstOrDefault();
                        ctx.DeleteObject(rc);
                    }

                    ctx.DeleteObject(jcr);

                    ctx.SaveChanges();
                }
            }
        }

        #endregion

        #region Pruebas

        public void ValidarDefecto(int defecto)
        {
            if (defecto == -1)
            {
                throw new ExcepcionReportes(MensajesError.Excepcion_DefectoRequerido);
            }
        }

        public bool GuardaReportePND(int juntaCampoID, int requisicionCampoID, ReporteCampoPND reporte, JuntaCampoReportePND juntaReporte, List<JuntaCampoReportePNDSector> sectores, List<JuntaCampoReportePNDCuadrante> cuadrantes, Guid UserUID, out Guid responsable, out string proyectoNombre, out string pendiente, out string detalle)
        {
            bool resultado = true;
            responsable = Guid.NewGuid();
            proyectoNombre = string.Empty;
            pendiente = string.Empty;
            detalle = string.Empty;

            using (SamContext ctx = new SamContext())
            {
                //Si la prueba es rechazada se validara que por lo menos se haya dado de alta un defecto
                if (!juntaReporte.Aprobado && (((juntaReporte.TipoRechazoID == (int)TipoRechazoEnum.Sector || juntaReporte.TipoRechazoID == null)  && sectores.Count() == 0)
                    || ((juntaReporte.TipoRechazoID == (int)TipoRechazoEnum.Cuadrante || juntaReporte.TipoRechazoID == null) && cuadrantes.Count() == 0)))
                {
                    throw new ExcepcionReportes(MensajesError.Excepcion_RechazadoSinDefectos);
                }                

                //Validar si el numero de reporte ya existe en la base de datos
                ReporteCampoPND reporteExistente = ctx.ReporteCampoPND.Where(x => x.NumeroReporte == reporte.NumeroReporte && x.ProyectoID == reporte.ProyectoID).SingleOrDefault();
                if (reporteExistente != null)
                {
                    //Validando que las fechas concuerden
                    if (reporteExistente.FechaReporte != reporte.FechaReporte)
                    {
                        throw new ExcepcionReportes(MensajesError.Excepcion_ReporteExistenteConFechaDiferente);
                    }
                    else
                    {
                        reporte = reporteExistente;

                        reporte.StartTracking();
                        reporte.UsuarioModifica = UserUID;
                        reporte.FechaModificacion = DateTime.Now;
                    }
                }
                else
                {
                    reporte.UsuarioModifica = UserUID;
                    reporte.FechaModificacion = DateTime.Now;
                }


                //Verifico que no exista ya un detalle para este spool en especifico en el reporte
                if (ctx.JuntaCampoReportePND.Where(x => x.ReporteCampoPNDID == reporte.ReporteCampoPNDID && x.JuntaCampoID == juntaCampoID).Any())
                {
                    throw new ExcepcionDuplicados(string.Format(MensajesError.Excepcion_ReporteVisualConDetalleDuplicado));
                }

                int jtaReq = ctx.JuntaCampoRequisicion.Where(x => x.RequisicionCampoID == requisicionCampoID && x.JuntaCampoID == juntaCampoID).Select(x => x.JuntaCampoRequisicionID).SingleOrDefault();

                JuntaCampoReportePND junta = new JuntaCampoReportePND
                {
                    JuntaCampoID = juntaCampoID,
                    JuntaCampoRequisicionID = jtaReq,
                    TipoRechazoID = juntaReporte.TipoRechazoID,
                    FechaPrueba = juntaReporte.FechaPrueba,
                    Aprobado = juntaReporte.Aprobado,
                    Observaciones = juntaReporte.Observaciones,
                    UsuarioModifica = UserUID,
                    FechaModificacion = DateTime.Now
                };

                JuntaCampo juntaCampo = ctx.JuntaCampo.Where(x => x.JuntaCampoID == juntaCampoID).Single();

                //Si el reporte es aprobado se actualiza el registro de JuntaCampo
                if (juntaReporte.Aprobado)
                {
                    juntaCampo.StartTracking();
                    juntaCampo.UltimoProcesoID = (int)UltimoProcesoEnum.PND;
                    juntaCampo.UsuarioModifica = UserUID;
                    juntaCampo.FechaModificacion = DateTime.Now;
                    juntaCampo.StopTracking();
                    ctx.JuntaCampo.ApplyChanges(juntaCampo);
                }
                else //Si el reporte es rechazado se generan los registros de defectos y ademas se genera una nuevo junta con una nueva etiqueta
                {
                    if (juntaReporte.TipoRechazoID == (int)TipoRechazoEnum.Sector)
                    {
                        foreach (JuntaCampoReportePNDSector sector in sectores)
                        {
                            sector.FechaModificacion = DateTime.Now;
                            sector.UsuarioModifica = UserUID;
                            junta.JuntaCampoReportePNDSector.Add(sector);
                        }
                    }
                    else
                    {
                        foreach (JuntaCampoReportePNDCuadrante cuadrante in cuadrantes)
                        {
                            cuadrante.FechaModificacion = DateTime.Now;
                            cuadrante.UsuarioModifica = UserUID;
                            junta.JuntaCampoReportePNDCuadrante.Add(cuadrante);
                        }
                    }

                    int siguienteNumRechazo = RechazosCortesUtil.ObtenerSiguienteRechazo(juntaCampo.EtiquetaJunta);
                    //Si la version de la junta aún no es la tercera se genera un nuevo registro en juntaworkstatus y se envia a resoldar (por rechazo)

                    if (siguienteNumRechazo < 3)
                    {
                        JuntaCampo nuevaJuntaCampo = new JuntaCampo
                        {
                            OrdenTrabajoSpoolID = juntaCampo.OrdenTrabajoSpoolID,
                            JuntaSpoolID = juntaCampo.JuntaSpoolID,
                            EtiquetaJunta = RechazosCortesUtil.ObtenerNuevaEtiquetaDeRechazo(siguienteNumRechazo, juntaCampo.EtiquetaJunta),
                            ArmadoAprobado = true,
                            SoldaduraAprobada = false,
                            InspeccionVisualAprobada = false,
                            VersionJunta = juntaCampo.VersionJunta + 1,
                            JuntaCampoAnteriorID = juntaCampo.JuntaCampoID,
                            JuntaFinal = true,
                            UltimoProcesoID = (int)UltimoProcesoEnum.Armado,
                            UsuarioModifica = UserUID,
                            FechaModificacion = DateTime.Now
                        };

                        JuntaCampoArmado armado = ctx.JuntaCampoArmado.Where(x => x.JuntaCampoArmadoID == juntaCampo.JuntaCampoArmadoID).SingleOrDefault();

                        JuntaCampoArmado nuevoArmado = new JuntaCampoArmado
                        {
                            Spool1ID = armado.Spool1ID,
                            Spool2ID = armado.Spool2ID,
                            EtiquetaMaterial1 = armado.EtiquetaMaterial1,
                            EtiquetaMaterial2 = armado.EtiquetaMaterial2,
                            NumeroUnico1ID = armado.NumeroUnico1ID,
                            NumeroUnico2ID = armado.NumeroUnico2ID,
                            TuberoID = armado.TuberoID,
                            FechaArmado = armado.FechaArmado,
                            FechaReporte = armado.FechaReporte,
                            Observaciones = armado.Observaciones,
                            UsuarioModifica = UserUID,
                            FechaModificacion = DateTime.Now
                        };

                        juntaCampo.StartTracking();
                        juntaCampo.JuntaFinal = false;
                        juntaCampo.UsuarioModifica = UserUID;
                        juntaCampo.FechaModificacion = DateTime.Now;
                        juntaCampo.StopTracking();
                        ctx.JuntaCampo.ApplyChanges(juntaCampo);

                        //Guardar Junta Campo
                        ctx.JuntaCampo.ApplyChanges(nuevaJuntaCampo);
                        ctx.SaveChanges();

                        //Guardar Junta Armado
                        nuevoArmado.JuntaCampoID = nuevaJuntaCampo.JuntaCampoID;
                        ctx.JuntaCampoArmado.ApplyChanges(nuevoArmado);
                        ctx.SaveChanges();


                        //Re guardar Junta Workstatus añadiendo Junta Armado
                        nuevaJuntaCampo.StartTracking();
                        nuevaJuntaCampo.JuntaCampoArmadoID = nuevoArmado.JuntaCampoArmadoID;
                        ctx.JuntaCampo.ApplyChanges(nuevaJuntaCampo);
                        ctx.SaveChanges();

                    }
                    else
                    {
                        resultado = false;

                        //TODO:
                        //Generar pendiente automatico de Corte en junta.
                        //En la descripción del pendiente incluir el siguiente texto:
                        //Etiqueta de la junta: *juntaWks.EtiquetaJunta

                        int categoriaPendienteID = (int)CategoriaPendienteEnum.Produccion;
                        int tipoPendienteID = (int)TipoPendienteEnum.CortePorRechazoDePrueba;
                        string nombreProyecto = ctx.Proyecto.Where(x => x.ProyectoID == reporte.ProyectoID).Single().Nombre;
                        string idiomaUsuario = ctx.Usuario.Where(x => x.UserId == UserUID).Single().Idioma;
                        string numeroControl = ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajoSpoolID == juntaCampo.OrdenTrabajoSpoolID).Single().NumeroControl;
                        Pendiente p = new Pendiente();

                        p.StartTracking();
                        p.ProyectoID = reporte.ProyectoID;
                        p.TipoPendienteID = tipoPendienteID;
                        p.Estatus = EstatusPendiente.Abierto;
                        p.FechaApertura = DateTime.Now;
                        p.GeneradoPor = UserUID;
                        p.FechaModificacion = DateTime.Now;

                        //Obtenemos el usuario responsable
                        ProyectoPendiente pp = ctx.ProyectoPendiente
                                                  .Where(x => x.ProyectoID == reporte.ProyectoID && x.TipoPendienteID == p.TipoPendienteID)
                                                  .SingleOrDefault();

                        responsable = pp.Responsable;
                        proyectoNombre = nombreProyecto;

                        if (pp != null)
                        {
                            p.AsignadoA = pp.Responsable;
                            p.CategoriaPendienteID = categoriaPendienteID;

                            TipoPendiente tipo = ctx.TipoPendiente
                                                    .Where(x => x.TipoPendienteID == tipoPendienteID)
                                                    .Single();

                            p.Descripcion = LanguageHelper.INGLES == idiomaUsuario ? "Label: " + juntaCampo.EtiquetaJunta + "<br > Control Number: " + numeroControl : "Etiqueta de la junta: " + juntaCampo.EtiquetaJunta + "<br > Número de Control: " + numeroControl;
                            p.Titulo = LanguageHelper.INGLES == idiomaUsuario ? tipo.NombreIngles : tipo.Nombre;
                        }

                        pendiente = p.Titulo;
                        detalle = p.Descripcion;

                        PendienteDetalle pd = new PendienteDetalle();

                        pd.CategoriaPendienteID = categoriaPendienteID;
                        pd.EsAlta = true;
                        pd.Responsable = pp.Responsable;
                        pd.Estatus = EstatusPendiente.Abierto;
                        pd.UsuarioModifica = UserUID;
                        pd.FechaModificacion = DateTime.Now;

                        p.StopTracking();
                        p.PendienteDetalle.Add(pd);

                        ctx.Pendiente.ApplyChanges(p);
                        ctx.SaveChanges();
                    }
                }

                reporte.JuntaCampoReportePND.Add(junta);

                if (reporteExistente != null)
                {
                    reporte.StopTracking();
                }

                ctx.ReporteCampoPND.ApplyChanges(reporte);
                ctx.SaveChanges();

            }

            return resultado;
        }

        public List<GrdDetReporteCampo> ObtenerListadoReportesPND(int juntaCampoID)
        {
            using (SamContext ctx = new SamContext())
            {
                List<GrdDetReporteCampo> list = (from jcrpnd in ctx.JuntaCampoReportePND.Include("ReporteCampoPND")
                                                    where jcrpnd.JuntaCampoID == juntaCampoID
                                                    select new GrdDetReporteCampo
                                                    {
                                                        JuntaCampoReporteID = jcrpnd.JuntaCampoReportePNDID,
                                                        NumeroReporte = jcrpnd.ReporteCampoPND.NumeroReporte,
                                                        Fecha = jcrpnd.ReporteCampoPND.FechaReporte,
                                                        TipoPrueba = Cultura == LanguageHelper.ESPANOL ? jcrpnd.ReporteCampoPND.TipoPrueba.Nombre : jcrpnd.ReporteCampoPND.TipoPrueba.NombreIngles,
                                                        Resultado = Cultura == LanguageHelper.ESPANOL ? jcrpnd.Aprobado == false ? "Rechazado" : "Aprobado" : jcrpnd.Aprobado == false ? "Rejected" : "Approved"
                                                    }).ToList();
                return list;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="juntaCampoReportePndID"></param>
        public void EliminaReporteCampoPnd(int juntaCampoReportePndID)
        {
            using (SamContext ctx = new SamContext())
            {
                EliminaReporteCampoPnd(juntaCampoReportePndID, ctx);
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="juntaCampoReportePndID"></param>
        /// <param name="ctx"></param>
        private void EliminaReporteCampoPnd(int juntaCampoReportePndID, SamContext ctx)
        {
            JuntaCampoReportePND jcrpnd = ctx.JuntaCampoReportePND.Single(x => x.JuntaCampoReportePNDID == juntaCampoReportePndID);
            ReporteCampoPND rep = ctx.ReporteCampoPND.Single(x => x.ReporteCampoPNDID == jcrpnd.ReporteCampoPNDID);
            int juntasDelReporte = ctx.JuntaCampoReportePND.Count(x => x.ReporteCampoPNDID == rep.ReporteCampoPNDID);

            ctx.LoadProperty<JuntaCampoReportePND>(jcrpnd, j => j.JuntaCampoReportePNDCuadrante);
            ctx.LoadProperty<JuntaCampoReportePND>(jcrpnd, j => j.JuntaCampoReportePNDSector);

            jcrpnd.JuntaCampoReportePNDSector.ToList().ForEach(ctx.DeleteObject);
            jcrpnd.JuntaCampoReportePNDCuadrante.ToList().ForEach(ctx.DeleteObject);

            ctx.DeleteObject(jcrpnd);

            if (juntasDelReporte == 1)
            {
                ctx.DeleteObject(rep);
            }
        }

        public void GuardaReporteTt(ReporteCampoTT reporte, JuntaCampoReporteTT juntaReporte, int juntaCampoID, int requisicionCampoID, Guid UserUID)
        {

            using (TransactionScope ts = new TransactionScope())
            {

                using (SamContext ctx = new SamContext())
                {

                    //Validar si el numero de reporte ya existe en la base de datos
                    ReporteCampoTT reporteExistente = ctx.ReporteCampoTT.Where(x => x.NumeroReporte == reporte.NumeroReporte && x.ProyectoID == reporte.ProyectoID).SingleOrDefault();
                    if (reporteExistente != null)
                    {
                        //Validando que las fechas concuerden
                        if (reporteExistente.FechaReporte != reporte.FechaReporte)
                        {
                            throw new ExcepcionReportes(MensajesError.Excepcion_ReporteExistenteConFechaDiferente);
                        }
                        else
                        {
                            reporte = reporteExistente;

                            reporte.StartTracking();
                            reporte.UsuarioModifica = UserUID;
                            reporte.FechaModificacion = DateTime.Now;
                        }
                    }
                    else
                    {
                        reporte.UsuarioModifica = UserUID;
                        reporte.FechaModificacion = DateTime.Now;
                    }

                    //Verifico que no exista ya un detalle para este spool en especifico en el reporte
                    if (ctx.JuntaCampoReporteTT.Where(x => x.ReporteCampoTTID == reporte.ReporteCampoTTID && x.JuntaCampoID == juntaCampoID).Any())
                    {
                        throw new ExcepcionDuplicados(string.Format(MensajesError.Excepcion_ReporteVisualConDetalleDuplicado));
                    }

                    int jtaReq = ctx.JuntaCampoRequisicion.Where(x => x.RequisicionCampoID == requisicionCampoID && x.JuntaCampoID == juntaCampoID).Select(x => x.JuntaCampoRequisicionID).SingleOrDefault();

                    JuntaCampoReporteTT junta = new JuntaCampoReporteTT
                    {
                        JuntaCampoID = juntaCampoID,
                        JuntaCampoRequisicionID = jtaReq,
                        FechaTratamiento = juntaReporte.FechaTratamiento,
                        NumeroGrafica = juntaReporte.NumeroGrafica,
                        Aprobado = juntaReporte.Aprobado,
                        Observaciones = juntaReporte.Observaciones,
                        UsuarioModifica = UserUID,
                        FechaModificacion = DateTime.Now
                    };

                    JuntaCampo juntaCampo = ctx.JuntaCampo.Where(x => x.JuntaCampoID == juntaCampoID).Single();

                    //Si el reporte es aprobado se actualiza el registro de JuntaWorkstatus
                    if (juntaReporte.Aprobado)
                    {
                        juntaCampo.StartTracking();
                        juntaCampo.UltimoProcesoID = (int)UltimoProcesoEnum.TT;
                        juntaCampo.UsuarioModifica = UserUID;
                        juntaCampo.FechaModificacion = DateTime.Now;
                        juntaCampo.StopTracking();
                        ctx.JuntaCampo.ApplyChanges(juntaCampo);
                    }

                    reporte.JuntaCampoReporteTT.Add(junta);

                    if (reporteExistente != null)
                    {
                        reporte.StopTracking();
                    }

                    ctx.ReporteCampoTT.ApplyChanges(reporte);
                    ctx.SaveChanges();

                }

                ts.Complete();
            }

        }

        public List<GrdDetReporteCampo> ObtenerListadoReportesTT(int juntaCampoID)
        {
            using (SamContext ctx = new SamContext())
            {
                List<GrdDetReporteCampo> list = (from jcrtt in ctx.JuntaCampoReporteTT.Include("ReporteCampoTT")
                                                 where jcrtt.JuntaCampoID == juntaCampoID
                                                 select new GrdDetReporteCampo
                                                 {
                                                     JuntaCampoReporteID = jcrtt.JuntaCampoReporteTTID,
                                                     NumeroReporte = jcrtt.ReporteCampoTT.NumeroReporte,
                                                     Fecha = jcrtt.ReporteCampoTT.FechaReporte,
                                                     TipoPrueba = Cultura == LanguageHelper.ESPANOL ? jcrtt.ReporteCampoTT.TipoPrueba.Nombre : jcrtt.ReporteCampoTT.TipoPrueba.NombreIngles,
                                                     Resultado = Cultura == LanguageHelper.ESPANOL ? jcrtt.Aprobado == false ? "Rechazado" : "Aprobado" : jcrtt.Aprobado == false ? "Rejected" : "Approved"
                                                 }).ToList();
                return list;
            }
        }

        public void EliminarReporteTT(int JuntaCampoReporteTTID)
        {
            using (SamContext ctx = new SamContext())
            {

                JuntaCampoReporteTT jcrtt = ctx.JuntaCampoReporteTT.Where(x => x.JuntaCampoReporteTTID == JuntaCampoReporteTTID).SingleOrDefault();
                if (ctx.JuntaCampoReporteTT.Where(x => x.ReporteCampoTTID == jcrtt.ReporteCampoTTID).Count() <= 1)
                {
                    ReporteCampoTT rcptt = ctx.ReporteCampoTT.Where(x => x.ReporteCampoTTID == jcrtt.ReporteCampoTTID).FirstOrDefault();
                    ctx.DeleteObject(rcptt);
                }
                ctx.DeleteObject(jcrtt);
                ctx.SaveChanges();
            }
        }

        #endregion


        #region Cortes

        /// <summary>
        /// 
        /// </summary>
        /// <param name="juntaSpoolID"></param>
        /// <param name="userId"></param>
        public void Cortar(int juntaSpoolID, Guid userId)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    JuntaCampo juntaACortar = ctx.JuntaCampo.Where(jc => jc.JuntaSpoolID == juntaSpoolID && jc.JuntaFinal == true).Single();
                    JuntaSpool juntaSpool = ctx.JuntaSpool.Where(js => js.JuntaSpoolID == juntaSpoolID).Single();

                    string nuevaEtiqueta = RechazosCortesUtil.ObtenNuevaEtiquetaDeCorte(juntaACortar.EtiquetaJunta, juntaSpool.Etiqueta);

                    #region Modificar la junta a cortar

                    juntaACortar.StartTracking();
                    juntaACortar.JuntaFinal = false;
                    juntaACortar.FechaModificacion = DateTime.Now;
                    juntaACortar.UsuarioModifica = userId;
                    juntaACortar.StopTracking();

                    #endregion

                    #region modificar el workStatus del spool
                    //OrdenTrabajoSpool ordenTrabajoSpool =
                    //    ctx.OrdenTrabajoSpool.Single(x => x.OrdenTrabajoSpoolID == juntaACortar.OrdenTrabajoSpoolID);

                    //WorkstatusSpool workstatusSpool =
                    //    ctx.WorkstatusSpool.SingleOrDefault(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpool.OrdenTrabajoSpoolID);

                    //if (workstatusSpool != null)
                    //{
                    //    workstatusSpool.StartTracking();
                    //    workstatusSpool.UltimoProcesoID = null;
                    //    workstatusSpool.TieneLiberacionDimensional = false;
                    //    workstatusSpool.TieneRequisicionPintura = false;
                    //    workstatusSpool.TienePintura = false;
                    //    workstatusSpool.LiberadoPintura = false;
                    //    workstatusSpool.Preparado = false;
                    //    workstatusSpool.Embarcado = false;
                    //    workstatusSpool.Certificado = false;
                    //    workstatusSpool.StopTracking();
                    //}
                    #endregion

                    #region Crear la junta nueva

                    JuntaCampo juntaNueva = new JuntaCampo
                    {
                        JuntaSpoolID = juntaACortar.JuntaSpoolID,
                        EtiquetaJunta = nuevaEtiqueta,
                        ArmadoAprobado = false,
                        SoldaduraAprobada = false,
                        InspeccionVisualAprobada = false,
                        JuntaCampoArmadoID = null,
                        JuntaCampoSoldaduraID = null,
                        JuntaCampoInspeccionVisualID = null,
                        VersionJunta = juntaACortar.VersionJunta + 1,
                        JuntaFinal = true,
                        UltimoProcesoID = null,
                        JuntaCampoAnteriorID = juntaACortar.JuntaCampoID,
                        OrdenTrabajoSpoolID = juntaACortar.OrdenTrabajoSpoolID,
                        FechaModificacion = DateTime.Now,
                        UsuarioModifica = userId
                    };

                    #endregion


                    //junta nueva apunta hacia JuntaCortada que a su vez contiene ordenTrabajoSpool
                    ctx.JuntaCampo.ApplyChanges(juntaACortar);
                    ctx.JuntaCampo.ApplyChanges(juntaNueva);

                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="juntaSpoolID"></param>
        /// <param name="userId"></param>
        public void RevertirCorte(int juntaSpoolID, Guid userId)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    JuntaCampo jc = ctx.JuntaCampo.Single(x => x.JuntaSpoolID == juntaSpoolID && x.JuntaFinal == true);

                    #region Correr validaciones

                    bool tieneRequisicionces = ctx.JuntaCampoRequisicion.Any(x => x.JuntaCampoID == jc.JuntaCampoID);
                    bool tienePruebasPnd = ctx.JuntaCampoReportePND.Any(x => x.JuntaCampoID == jc.JuntaCampoID);
                    bool tienePruebasTt = ctx.JuntaCampoReporteTT.Any(x => x.JuntaCampoID == jc.JuntaCampoID);

                    if (tienePruebasTt)
                    {
                        throw new BaseValidationException(Cultura == LanguageHelper.ESPANOL ? string.Format("La junta {0} ya cuenta con pruebas TT", jc.EtiquetaJunta) : string.Format("The weld {0} already TT tests", jc.EtiquetaJunta));
                    }

                    if (tienePruebasPnd)
                    {
                        throw new BaseValidationException(Cultura == LanguageHelper.ESPANOL ?string.Format("La junta {0} ya cuenta con pruebas PND", jc.EtiquetaJunta) : string.Format("The weld {0} already has PND tests", jc.EtiquetaJunta));
                    }

                    if (tieneRequisicionces)
                    {
                        throw new BaseValidationException(Cultura == LanguageHelper.ESPANOL ?string.Format("La junta {0} ya cuenta con requisiciones", jc.EtiquetaJunta) : string.Format("The weld {0} already has requisition", jc.EtiquetaJunta));
                    }

                    if (jc.JuntaCampoInspeccionVisualID.HasValue)
                    {
                        throw new BaseValidationException(Cultura == LanguageHelper.ESPANOL ? string.Format("La junta {0} ya cuenta con inspección visual", jc.EtiquetaJunta) : string.Format("The weld {0} already has visual inspection", jc.EtiquetaJunta));
                    }

                    if (jc.JuntaCampoSoldaduraID.HasValue)
                    {
                        throw new BaseValidationException(Cultura == LanguageHelper.ESPANOL ?string.Format("La junta {0} ya cuenta con soldadura", jc.EtiquetaJunta) : string.Format("The weld {0} already has welding", jc.EtiquetaJunta));
                    }                    

                    if (jc.JuntaCampoArmadoID.HasValue)
                    {
                        throw new BaseValidationException(Cultura == LanguageHelper.ESPANOL ? string.Format("La junta {0} ya cuenta con armado", jc.EtiquetaJunta) : string.Format("The weld {0} already has assembly", jc.EtiquetaJunta));
                    }

                    #endregion

                    JuntaCampo juntaAnterior = ctx.JuntaCampo.Single(x => x.JuntaCampoID == jc.JuntaCampoAnteriorID);

                    juntaAnterior.StartTracking();
                    juntaAnterior.FechaModificacion = DateTime.Now;
                    juntaAnterior.UsuarioModifica = userId;
                    juntaAnterior.JuntaFinal = true;

                    ctx.DeleteObject(jc);
                    ctx.JuntaCampo.ApplyChanges(juntaAnterior);
                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }

        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="juntaCampoID"></param>
        /// <returns></returns>
        public List<GrdDetReporteCampo> ObtenerRechazosHistoricos(int juntaCampoID)
        {
            List<GrdDetReporteCampo> lista = new List<GrdDetReporteCampo>();

            using (SamContext ctx = new SamContext())
            {
                IQueryable<int?> qry = (from jc in ctx.JuntaCampo
                                        where jc.JuntaCampoID == juntaCampoID
                                        select jc.JuntaCampoAnteriorID).AsQueryable();


                lista = (from jcrpnd in ctx.JuntaCampoReportePND
                         let rc = jcrpnd.ReporteCampoPND
                         where  qry.Contains(jcrpnd.JuntaCampoID)
                                && jcrpnd.Aprobado == false
                         select new GrdDetReporteCampo
                         {
                             JuntaCampoReporteID = jcrpnd.JuntaCampoReportePNDID,
                             NumeroReporte = rc.NumeroReporte,
                             Fecha = rc.FechaReporte,
                             TipoPrueba = Cultura == LanguageHelper.ESPANOL ? rc.TipoPrueba.Nombre: rc.TipoPrueba.NombreIngles,
                             Resultado = Cultura == LanguageHelper.ESPANOL ? jcrpnd.Aprobado == false ? "Rechazado" : "Aprobado" : jcrpnd.Aprobado == false ? "Rejected" : "Approved"
                         })
                         .OrderByDescending(x => x.Fecha)
                         .ThenBy(x => x.NumeroReporte)
                         .ToList();
            }

            return lista;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jcRepPndId"></param>
        /// <param name="userId"></param>
        public void EliminaReporteDeRechazoHistorico(int jcRepPndId, Guid userId)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SamContext ctx = new SamContext())
                    {
                        JuntaCampo jcARestaurar = ctx.JuntaCampo.Single(x => ctx.JuntaCampoReportePND
                                                                                .Where(y => y.JuntaCampoReportePNDID == jcRepPndId)
                                                                                .Select(z => z.JuntaCampoID)
                                                                                .Contains(x.JuntaCampoID));

                        JuntaCampo jcAEliminar = ctx.JuntaCampo.Single(x => x.JuntaCampoAnteriorID == jcARestaurar.JuntaCampoID);

                        #region Correr validaciones

                        bool tieneRequisicionces = ctx.JuntaCampoRequisicion.Any(x => x.JuntaCampoID == jcAEliminar.JuntaCampoID);
                        bool tienePruebasPnd = ctx.JuntaCampoReportePND.Any(x => x.JuntaCampoID == jcAEliminar.JuntaCampoID);
                        bool tienePruebasTt = ctx.JuntaCampoReporteTT.Any(x => x.JuntaCampoID == jcAEliminar.JuntaCampoID);

                        if (tienePruebasTt)
                        {
                            throw new BaseValidationException(string.Format("Se deben eliminar las pruebas TT de la junta {0}", jcAEliminar.EtiquetaJunta));
                        }

                        if (tienePruebasPnd)
                        {
                            throw new BaseValidationException(string.Format("Se deben eliminar las pruebas PND de la junta {0}", jcAEliminar.EtiquetaJunta));
                        }

                        if (tieneRequisicionces)
                        {
                            throw new BaseValidationException(string.Format("Se deben eliminar las requisiciones de la junta {0}", jcAEliminar.EtiquetaJunta));
                        }

                        if (jcAEliminar.JuntaCampoSoldaduraID.HasValue)
                        {
                            throw new BaseValidationException(string.Format("Se debe eliminar la soldadura de la junta {0}", jcAEliminar.EtiquetaJunta));
                        }

                        if (jcAEliminar.JuntaCampoInspeccionVisualID.HasValue)
                        {
                            throw new BaseValidationException(string.Format("Se debe eliminar la inspección visual de la junta {0}", jcAEliminar.EtiquetaJunta));
                        }

                        #endregion

                        JuntaCampoArmado jcArmado = ctx.JuntaCampoArmado.Single(x => x.JuntaCampoArmadoID == jcAEliminar.JuntaCampoArmadoID);

                        jcAEliminar.JuntaCampoArmadoID = null;
                        ctx.JuntaCampo.ApplyChanges(jcAEliminar);
                        ctx.SaveChanges();

                        ctx.DeleteObject(jcArmado);
                        ctx.DeleteObject(jcAEliminar);
                        EliminaReporteCampoPnd(jcRepPndId, ctx);

                        jcARestaurar.StartTracking();
                        jcARestaurar.JuntaFinal = true;
                        jcARestaurar.FechaModificacion = DateTime.Now;
                        jcARestaurar.UsuarioModifica = userId;

                        ctx.JuntaCampo.ApplyChanges(jcARestaurar);
                        ctx.SaveChanges();
                    }

                    scope.Complete();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        public void CambiaFechasJuntaCampoSoldadura(int juntaCampoSoldaduraID, DateTime fechaSoldadura, DateTime fechaReporteSoldadura, Guid usuario)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntaCampoSoldadura jcs = ctx.JuntaCampoSoldadura.Single(x => x.JuntaCampoSoldaduraID == juntaCampoSoldaduraID);
                jcs.StartTracking();
                jcs.FechaSoldadura = fechaSoldadura;
                jcs.FechaReporte = fechaReporteSoldadura;
                jcs.FechaModificacion = DateTime.Now;
                jcs.UsuarioModifica = usuario;
                jcs.StopTracking();
                ctx.JuntaCampoSoldadura.ApplyChanges(jcs);
                ctx.SaveChanges();
            }
        }

        public void CambiaFechasJuntaCampoArmado(int juntaCampoArmadoID, DateTime fechaArmado, DateTime fechaReporteArmado, Guid usuario)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntaCampoArmado jca = ctx.JuntaCampoArmado.Single(x => x.JuntaCampoArmadoID == juntaCampoArmadoID);
                jca.StartTracking();
                jca.FechaArmado = fechaArmado;
                jca.FechaReporte = fechaReporteArmado;
                jca.UsuarioModifica = usuario;
                jca.FechaModificacion = DateTime.Now;
                jca.StopTracking();
                ctx.JuntaCampoArmado.ApplyChanges(jca);
                ctx.SaveChanges();
            }
        }

        public void CambiaFechasIV(int juntaCampoIVID, DateTime fechaIV, DateTime fechaReporteIV, Guid usuario)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (SamContext ctx = new SamContext())
                {
                    JuntaCampoInspeccionVisual jciv = ctx.JuntaCampoInspeccionVisual.Single(x => x.JuntaCampoInspeccionVisualID == juntaCampoIVID);
                    jciv.StartTracking();
                    jciv.FechaInspeccion = fechaIV;
                    jciv.FechaModificacion = DateTime.Now;
                    jciv.UsuarioModifica = usuario;
                    jciv.StopTracking();
                    ctx.JuntaCampoInspeccionVisual.ApplyChanges(jciv);
                    ctx.SaveChanges();

                    InspeccionVisualCampo ivc = ctx.InspeccionVisualCampo.Single(x => x.InspeccionVisualCampoID == jciv.InspeccionVisualCampoID);
                    ivc.StartTracking();
                    ivc.FechaReporte = fechaReporteIV;
                    ivc.FechaModificacion = DateTime.Now;
                    ivc.UsuarioModifica = usuario;
                    ivc.StopTracking();
                    ctx.InspeccionVisualCampo.ApplyChanges(ivc);
                    ctx.SaveChanges();

                }
                scope.Complete();
            }
        }

        public void CambiaFechasRequisicion(int requisicionCampoID, DateTime fechaRequisicion, Guid usuario)
        {
            using (SamContext ctx = new SamContext())
            {
                RequisicionCampo reqCampo = ctx.RequisicionCampo.Single(x => x.RequisicionCampoID == requisicionCampoID);

                reqCampo.StartTracking();
                reqCampo.FechaRequisicion = fechaRequisicion;
                reqCampo.FechaModificacion = DateTime.Now;
                reqCampo.UsuarioModifica = usuario;
                reqCampo.StopTracking();

                ctx.RequisicionCampo.ApplyChanges(reqCampo);
                ctx.SaveChanges();
            }
        }
    }
}
