using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.BusinessObjects.Excepciones;
using System.Transactions;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Cache;
using SAM.Entities.Personalizadas;
using SAM.Entities;

namespace SAM.BusinessObjects.Workstatus
{
    public class RequisicionPruebasBO
    {
        private static readonly object _mutex = new object();
        private static RequisicionPruebasBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private RequisicionPruebasBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase InspeccionVisualBO
        /// </summary>
        /// <returns></returns>
        public static RequisicionPruebasBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new RequisicionPruebasBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtenemos las juntas candidatas a generar requision de pruebas
        /// </summary>
        /// <param name="ordenTrabajoID">Id de la orden de trabajo</param>
        /// <param name="ordenTrabajoSpoolID">ID de la ordentrabajospool</param>
        /// <returns></returns>
        public List<GrdRequisicionPruebas> ObtenerJuntas(int ordenTrabajoID, int ordenTrabajoSpoolID, int tipoPruebaID)
        {
            using (SamContext ctx = new SamContext())
            {
                List<GrdRequisicionPruebas> lista = new List<GrdRequisicionPruebas>();
                string categoriaPrueba = CacheCatalogos.Instance.ObtenerTiposPrueba().Where(x => x.ID == tipoPruebaID).Select(x => x.Categoria).Single();

                int proyectoID = ctx.OrdenTrabajo.SingleOrDefault(x => x.OrdenTrabajoID == ordenTrabajoID).ProyectoID;

                //Obtengo los IDs de las juntas que existen en la tabla JuntaWorkstatus que son parte de la orden de trabajo
                //y del numero de control (en caso de especificarse)
                //Donde la junta es JuntaFinal y ya tengan inspeccion visual
                IQueryable<JuntaWorkstatus> juntas;

                if (tipoPruebaID == (int)TipoPruebaEnum.Preheat)
                {
                    juntas = ctx.JuntaWorkstatus.Where(x => x.OrdenTrabajoSpool.OrdenTrabajoID == ordenTrabajoID
                                                            && (ordenTrabajoSpoolID == -1
                                                            || x.OrdenTrabajoSpool.OrdenTrabajoSpoolID == ordenTrabajoSpoolID));
                }
                else
                {
                    juntas = ctx.JuntaWorkstatus.Where(x => x.OrdenTrabajoSpool.OrdenTrabajoID == ordenTrabajoID
                                                                      && (ordenTrabajoSpoolID == -1
                                                                      || x.OrdenTrabajoSpool.OrdenTrabajoSpoolID == ordenTrabajoSpoolID)
                                                                      && x.JuntaFinal);
                }

                //Obtengo las requisiciones de las juntas que caen en los filtros
                IQueryable<JuntaRequisicion> juntasConRequisicion = (from jtaReq in ctx.JuntaRequisicion
                                                                     join jtaWks in juntas on jtaReq.JuntaWorkstatusID equals jtaWks.JuntaWorkstatusID
                                                                     where jtaReq.Requisicion.TipoPruebaID == tipoPruebaID
                                                                     select jtaReq);

                IQueryable<int> juntasConRequisicionSinPrueba = juntasConRequisicion.Where(x => !ctx.JuntaReporteTt.Select(y => y.JuntaRequisicionID).Contains(x.JuntaRequisicionID)).Select(x => x.JuntaWorkstatusID);

                //Si el tipo de reporte es Categoria TT, verifico que exista el rechazo para incluirla en el listado. De lo contrario no se debe mostrar.
                if (categoriaPrueba == CategoriaTipoPrueba.TT)
                {
                    IQueryable<int> juntasConRechazo = from rechazo in ctx.JuntaReporteTt
                                                        join jtaReq in juntasConRequisicion on rechazo.JuntaRequisicionID equals jtaReq.JuntaRequisicionID
                                                        where !rechazo.Aprobado
                                                        select jtaReq.JuntaWorkstatusID;                    


                    //Filtro las juntas con requisición para que me deje sólo aquellas que no tienen rechazo.
                    juntasConRequisicion = juntasConRequisicion.Where(x => !juntasConRechazo.Contains(x.JuntaWorkstatusID));                    
                }


                //Obtengo las juntas que son parte de los filtros y que no tienen requisicion.
                IQueryable<int> juntasIDs = from jta in juntas
                                             where !juntasConRequisicion.Select(x => x.JuntaWorkstatusID).Contains(jta.JuntaWorkstatusID)
                                             select jta.JuntaSpoolID;

                //Verifico que si el tipo de prueba es Post-TT o Post-TT exista previamente un resultado positivo de PWHT
                if (tipoPruebaID == (int)TipoPruebaEnum.RTPostTT || tipoPruebaID == (int)TipoPruebaEnum.PTPostTT)
                {
                    IQueryable<int> juntaConPWHT = from pwht in ctx.JuntaReporteTt
                                                    join jta in juntas on pwht.JuntaWorkstatusID equals jta.JuntaWorkstatusID
                                                    join tt in ctx.ReporteTt on pwht.ReporteTtID equals tt.ReporteTtID
                                                    where pwht.Aprobado
                                                    select jta.JuntaSpoolID;

                    juntasIDs = juntasIDs.Where(x => juntaConPWHT.Contains(x));
                }

                //Obtengo los registros de las juntas obtenidas en el paso anterior y que además son parte de un spool con liberacion dimensional
                IQueryable<JuntaSpool> query = ctx.JuntaSpool.Where(x => juntasIDs.Contains(x.JuntaSpoolID)).AsQueryable();
                                                                   // && spoolsInspDimensional.Contains(x.OrdenTrabajoJunta.Select(y => y.OrdenTrabajoSpoolID).FirstOrDefault())).AsQueryable();

                if (tipoPruebaID == (int)TipoPruebaEnum.Preheat)
                {
                    int[] juntasIds = juntas.Select(j => j.JuntaSpoolID).ToArray();
                    IEnumerable<GrdRequisicionPruebas> jsws =
                        from js in ctx.JuntaSpool.Include("Spool").Where(x => !juntasIds.Contains(x.JuntaSpoolID) && x.Spool.ProyectoID == proyectoID)
                        join otj in ctx.OrdenTrabajoJunta on js.JuntaSpoolID equals otj.JuntaSpoolID
                        join ots in ctx.OrdenTrabajoSpool on otj.OrdenTrabajoSpoolID equals ots.OrdenTrabajoSpoolID
                        join ot in ctx.OrdenTrabajo on ots.OrdenTrabajoID equals ot.OrdenTrabajoID
                        join sh in ctx.SpoolHold on ots.SpoolID equals sh.SpoolID into Holds
                        from hld in Holds.DefaultIfEmpty()
                        where ot.OrdenTrabajoID == ordenTrabajoID &&
                              (ordenTrabajoSpoolID == -1 || ots.OrdenTrabajoSpoolID == ordenTrabajoSpoolID)
                        select new GrdRequisicionPruebas
                             {
                                 JuntaWorkstatusID = (js.JuntaSpoolID * -1), // si la junta no tiene jws mandamos la juntaspoolID en negativo
                                 NombreSpool = js.Spool.Nombre,
                                 JuntaSpoolID = js.JuntaSpoolID,
                                 OrdenTrabajo = ot.NumeroOrden,
                                 NumeroControl = ots.NumeroControl,
                                 EtiquetaJunta = js.Etiqueta,
                                 EtiquetaMaterial1 = js.EtiquetaMaterial1,
                                 EtiquetaMaterial2 = js.EtiquetaMaterial2,
                                 TipoJuntaID = js.TipoJuntaID,
                                 TipoJunta = js.TipoJunta.Codigo,
                                 Cedula = js.Cedula,
                                 FamiliaAceroMaterial1 = js.FamiliaAcero.Nombre,
                                 FamiliaAceroMaterial2 = js.FamiliaAcero1.Nombre,
                                 Diametro = js.Diametro,
                                 Armado = false,
                                 Soldadura = false,
                                 InspeccionVisual = false,
                                 InspeccionDimensional = false,
                                 Hold = (hld == null) ? false : hld.TieneHoldIngenieria || hld.TieneHoldCalidad || hld.Confinado
                             };

                    lista = (from js in query
                             join s in ctx.Spool on js.SpoolID equals s.SpoolID
                             join otj in ctx.OrdenTrabajoJunta on js.JuntaSpoolID equals otj.JuntaSpoolID
                             join ots in ctx.OrdenTrabajoSpool on otj.OrdenTrabajoSpoolID equals ots.OrdenTrabajoSpoolID
                             join ot in ctx.OrdenTrabajo on ots.OrdenTrabajoID equals ot.OrdenTrabajoID
                             join jw in ctx.JuntaWorkstatus on js.JuntaSpoolID equals jw.JuntaSpoolID
                             join wss in ctx.WorkstatusSpool on otj.OrdenTrabajoSpoolID equals wss.OrdenTrabajoSpoolID into WksSpool
                             from wkss in WksSpool.DefaultIfEmpty()
                             join sh in ctx.SpoolHold on ots.SpoolID equals sh.SpoolID into Holds
                             from hld in Holds.DefaultIfEmpty()
                             where !juntasConRequisicionSinPrueba.Contains(jw.JuntaWorkstatusID)
                             select new GrdRequisicionPruebas
                             {
                                 JuntaWorkstatusID = jw.JuntaWorkstatusID,
                                 NombreSpool = s.Nombre,
                                 JuntaSpoolID = js.JuntaSpoolID,
                                 OrdenTrabajo = ot.NumeroOrden,
                                 NumeroControl = ots.NumeroControl,
                                 EtiquetaJunta = jw.EtiquetaJunta,
                                 EtiquetaMaterial1 = js.EtiquetaMaterial1,
                                 EtiquetaMaterial2 = js.EtiquetaMaterial2,
                                 TipoJuntaID = js.TipoJuntaID,
                                 TipoJunta = js.TipoJunta.Codigo,
                                 Cedula = js.Cedula,
                                 FamiliaAceroMaterial1 = js.FamiliaAcero.Nombre,
                                 FamiliaAceroMaterial2 = js.FamiliaAcero1.Nombre,
                                 Diametro = js.Diametro,
                                 Armado = jw.ArmadoAprobado,
                                 Soldadura = jw.SoldaduraAprobada,
                                 InspeccionVisual = jw.InspeccionVisualAprobada,
                                 InspeccionDimensional = (wkss == null) ? false : wkss.TieneLiberacionDimensional,
                                 Hold = (hld == null) ? false : hld.TieneHoldIngenieria || hld.TieneHoldCalidad || hld.Confinado
                             })
                             .Union(jsws).ToList();
                }
                else
                {
                    lista = (from js in query
                             join s in ctx.Spool on js.SpoolID equals s.SpoolID
                             join otj in ctx.OrdenTrabajoJunta on js.JuntaSpoolID equals otj.JuntaSpoolID
                             join ots in ctx.OrdenTrabajoSpool on otj.OrdenTrabajoSpoolID equals ots.OrdenTrabajoSpoolID
                             join ot in ctx.OrdenTrabajo on ots.OrdenTrabajoID equals ot.OrdenTrabajoID
                             join jw in ctx.JuntaWorkstatus on js.JuntaSpoolID equals jw.JuntaSpoolID
                             join wss in ctx.WorkstatusSpool on otj.OrdenTrabajoSpoolID equals wss.OrdenTrabajoSpoolID into WksSpool
                             from wkss in WksSpool.DefaultIfEmpty()
                             join sh in ctx.SpoolHold on ots.SpoolID equals sh.SpoolID into Holds
                             from hld in Holds.DefaultIfEmpty()
                             where jw.JuntaFinal
                             && !juntasConRequisicionSinPrueba.Contains(jw.JuntaWorkstatusID)
                             && jw.SoldaduraAprobada
                             select new GrdRequisicionPruebas
                             {
                                 JuntaWorkstatusID = jw.JuntaWorkstatusID,
                                 NombreSpool = s.Nombre,
                                 JuntaSpoolID = js.JuntaSpoolID,
                                 OrdenTrabajo = ot.NumeroOrden,
                                 NumeroControl = ots.NumeroControl,
                                 EtiquetaJunta = jw.EtiquetaJunta,
                                 EtiquetaMaterial1 = js.EtiquetaMaterial1,
                                 EtiquetaMaterial2 = js.EtiquetaMaterial2,
                                 TipoJuntaID = js.TipoJuntaID,
                                 TipoJunta = js.TipoJunta.Codigo,
                                 Cedula = js.Cedula,
                                 FamiliaAceroMaterial1 = js.FamiliaAcero.Nombre,
                                 FamiliaAceroMaterial2 = js.FamiliaAcero1.Nombre,
                                 Diametro = js.Diametro,
                                 Armado = jw.ArmadoAprobado,
                                 Soldadura = jw.SoldaduraAprobada,
                                 InspeccionVisual = jw.InspeccionVisualAprobada,
                                 InspeccionDimensional = (wkss == null) ? false : wkss.TieneLiberacionDimensional,
                                 Hold = (hld == null) ? false : hld.TieneHoldIngenieria || hld.TieneHoldCalidad || hld.Confinado
                             }).ToList();
                }


                return lista;
            }
        }

        /// <summary>
        /// Obtenemos los spools candidatos a generar requision de pruebas
        /// </summary>
        /// <param name="ordenTrabajoID">Id de la orden de trabajo</param>
        /// <param name="ordenTrabajoSpoolID">ID de la ordentrabajospool</param>
        /// <returns></returns>
        public List<GrdRequisicionSpoolPruebas> ObtenerSpools(int ordenTrabajoID, int ordenTrabajoSpoolID, int tipoPruebaSpoolID)
        {
            List<JuntaWorkstatus> jwks = null;
            List<JuntaSpool> js = null;
            List<OrdenTrabajoJunta> otj = null;
            List<TipoJuntaCache> tp = CacheCatalogos.Instance.ObtenerTiposJunta();
            int fabArea = CacheCatalogos.Instance.ObtenerFabAreas().Where(x => x.Nombre == FabAreas.SHOP).Select(x => x.ID).SingleOrDefault();
            int jTH = tp.Where(y => y.Nombre == TipoJuntas.TH).Select(y => y.ID).SingleOrDefault();
            int jTW = tp.Where(y => y.Nombre == TipoJuntas.TW).Select(y => y.ID).SingleOrDefault();

            using (SamContext ctx = new SamContext())
            {
                List<GrdRequisicionSpoolPruebas> lista = new List<GrdRequisicionSpoolPruebas>();

                int tipoJuntaTHID = ctx.TipoJunta.SingleOrDefault(x => x.Codigo == "TH").TipoJuntaID;
                int FabAreaShopID = ctx.FabArea.SingleOrDefault(x => x.Codigo == "SHOP").FabAreaID;

                //Obtengo los IDs de los spools que existen en la tabla WorkstatusSpool que son parte de la orden de trabajo
                //y del numero de control (en caso de especificarse)
                IQueryable<OrdenTrabajoSpool> Odts = ctx.OrdenTrabajoSpool
                                                        .Where(x => x.OrdenTrabajoID == ordenTrabajoID
                                                            && (ordenTrabajoSpoolID == -1 || x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID));



                List<int> onlySpools = Odts.Select(x => x.OrdenTrabajoSpoolID).ToList();

                IQueryable<WorkstatusSpool> workstatusSpool = ctx.WorkstatusSpool.Where(x => onlySpools.Contains(x.OrdenTrabajoSpoolID));

                List<int> ODTS = new List<int>();
                
                //1.- Si no existen ODTS en workStatusSpool entonces se en lista    (si se enlista)     
                IQueryable<int> odtsSinWorkstatus = (from spool in Odts
                                                     join wss in ctx.WorkstatusSpool on spool.OrdenTrabajoSpoolID equals wss.OrdenTrabajoSpoolID into Wspoll
                                                     from workstatus in Wspoll.DefaultIfEmpty()
                                                     where workstatus.WorkstatusSpoolID == null
                                                     select spool.SpoolID);
                                          

                //2.- Odts que tengan requisicion y no tengan reporte   (No se lista)     
                IQueryable<int> OdtsReqSinReporte = (from spool in workstatusSpool
                                                                  join spoolReq in ctx.SpoolRequisicion on spool.WorkstatusSpoolID equals spoolReq.WorkstatusSpoolID
                                                                  join spoolRepPnd in ctx.SpoolReportePnd on spoolReq.SpoolRequisicionID equals spoolRepPnd.SpoolRequisicionID into reporte
                                                                  from sRep in reporte.DefaultIfEmpty()
                                                                  where sRep.SpoolReportePndID == null && spoolReq.RequisicionSpool.TipoPruebaSpoolID == tipoPruebaSpoolID
                                                                  select spool.OrdenTrabajoSpool.SpoolID).Distinct();


                //3.- Odts con requisicion y reporte Aprobado  (No se lista)
                IQueryable<int> OdtsReqSinReporteAprobado = (from spool in workstatusSpool
                                                                  join spoolReq in ctx.SpoolRequisicion on spool.WorkstatusSpoolID equals spoolReq.WorkstatusSpoolID
                                                                  join spoolRepPnd in ctx.SpoolReportePnd on spoolReq.SpoolRequisicionID equals spoolRepPnd.SpoolRequisicionID
                                                                  where spoolRepPnd.Aprobado && spoolReq.RequisicionSpool.TipoPruebaSpoolID == tipoPruebaSpoolID
                                                                  select spool.OrdenTrabajoSpool.SpoolID).Distinct();

                //4.- Odts con requisicion y con reporte Rechazado (si se enlista)  
                IQueryable<int> OdtsReqConReporteRechazado = (from spool in workstatusSpool
                                                                  join spoolReq in ctx.SpoolRequisicion on spool.WorkstatusSpoolID equals spoolReq.WorkstatusSpoolID
                                                                  join spoolRepPnd in ctx.SpoolReportePnd on spoolReq.SpoolRequisicionID equals spoolRepPnd.SpoolRequisicionID
                                                                  where !spoolRepPnd.Aprobado && spoolReq.RequisicionSpool.TipoPruebaSpoolID == tipoPruebaSpoolID
                                                                  select spool.OrdenTrabajoSpool.SpoolID).Distinct();

                //5.- Odts sin Requisicion (si se enlista)
                IQueryable<int> OdtsReqSinRequisicion = (from spool in workstatusSpool
                                                                      join spoolReq in ctx.SpoolRequisicion on spool.WorkstatusSpoolID equals spoolReq.WorkstatusSpoolID into req
                                                                      from Sinreq in req.DefaultIfEmpty()
                                                                      where Sinreq.SpoolRequisicionID == null
                                                                      select spool.OrdenTrabajoSpool.SpoolID).Distinct();
                
                // Se obtienen los SPOOLIDS que se mostraran en el Grid
                IQueryable<int> spoolsIDs = (from odt in Odts
                                             where (odtsSinWorkstatus.Contains(odt.SpoolID) || OdtsReqConReporteRechazado.Contains(odt.SpoolID)
                                                  || OdtsReqSinRequisicion.Contains(odt.SpoolID)) && !OdtsReqSinReporte.Contains(odt.SpoolID) &&
                                                  !OdtsReqSinReporteAprobado.Contains(odt.SpoolID)
                                             select odt.SpoolID).Distinct();
                                                     
                //Obtengo los registros de los spools obtenidos en el paso anterior y que cuentan con al menos una junta shop
                IQueryable<Spool> query = ctx.Spool.Include("JuntaSpool").Where(x => spoolsIDs.Contains(x.SpoolID) &&
                                                                                     x.JuntaSpool.Any(j => j.FabAreaID == FabAreaShopID && j.TipoJuntaID != tipoJuntaTHID))
                                                                         .AsQueryable();

                //Obtengo ids de tramos rectos para excluirlos posteriormente
                IQueryable<int> tramosRectosIds = from s in query
                                                  where s.MaterialSpool.Count == 1
                                                  select s.SpoolID;                                

                //Obtengo el procentaje total de soldadura de los spools
                //IQueryable<int> solds = (from tjss in ctx.JuntaSpool.Where(x => query.Select(y => y.SpoolID).Contains(x.SpoolID) && !tramosRectosIds.Contains(x.SpoolID))
                //                         group tjss by tjss.SpoolID into g
                //                         from gj in g
                //                         join tjwss in ctx.JuntaWorkstatus.Where(x => x.JuntaFinal && x.SoldaduraAprobada) on gj.JuntaSpoolID equals tjwss.JuntaSpoolID into gtjwss
                //                         let pSoldado = ((decimal)gtjwss.DefaultIfEmpty().Count() / (decimal)g.Count()) * 100
                //                         where pSoldado == 100
                //                         select g.Key).Distinct();

                //IQueryable<JuntaSpool> juntasSpool = ctx.JuntaSpool.Where(x => query.Select(y => y.SpoolID).Contains(x.SpoolID) && !tramosRectosIds.Contains(x.SpoolID)).AsQueryable();
                //IQueryable<JuntaWorkstatus> wsJuntas = ctx.JuntaWorkstatus.Where(x => juntasSpool.Select(y => y.JuntaSpoolID).Contains(x.JuntaSpoolID)
                //    && x.JuntaFinal).AsQueryable();

                //List<int> sold = new List<int>();

                //List<Spool> lstQuery = query.ToList();

                //lstQuery.ForEach(a =>
                //    {

                //        IQueryable<JuntaSpool> tempJunta = juntasSpool.Where(x => x.SpoolID == a.SpoolID && x.TipoJuntaID != tipoJuntaTHID);
                //        int tempJuntaWs = wsJuntas.Where(x => tempJunta.Select(y => y.JuntaSpoolID).Contains(x.JuntaSpoolID)).Count();
                //        int countJunta = tempJunta.Count();                       

                //        if (countJunta > 0 && tempJuntaWs > 0)
                //        {
                //            decimal proceso = (countJunta / tempJuntaWs) * 100;
                //            if (proceso == 100)
                //            {
                //                sold.Add(a.SpoolID);
                //            }
                //        }
                //    }
                //);


                lista = (from s in query
                         join ots in ctx.OrdenTrabajoSpool on s.SpoolID equals ots.SpoolID
                         join ot in ctx.OrdenTrabajo on ots.OrdenTrabajoID equals ot.OrdenTrabajoID   
                         join wss in ctx.WorkstatusSpool on ots.OrdenTrabajoSpoolID equals wss.OrdenTrabajoSpoolID into WksSpool 
                         from wkss in WksSpool.DefaultIfEmpty() 
                         join sh in ctx.SpoolHold on ots.SpoolID equals sh.SpoolID into Holds
                         from hld in Holds.DefaultIfEmpty()
                         where !tramosRectosIds.Contains(s.SpoolID)
                         select new GrdRequisicionSpoolPruebas
                         {
                             WorkstatusSpoolID = (wkss != null) ? wkss.WorkstatusSpoolID : 0,
                             OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID,
                             SpoolID = s.SpoolID,
                             NombreSpool = s.Nombre,
                             LiberacionDimensional = wkss != null ? wkss.TieneLiberacionDimensional : false,
                             OrdenTrabajo = ot.NumeroOrden,
                             NumeroControl = ots.NumeroControl,
                             Area = s.Area != null ? (decimal)s.Area : 0,
                             PDI = s.Pdis != null ? (decimal)s.Pdis : 0,
                             Peso = s.Peso != null ? (decimal)s.Peso : 0,
                             Hold = (hld == null) ? false : hld.TieneHoldIngenieria || hld.TieneHoldCalidad || hld.Confinado
                         }).OrderBy(x => x.NumeroControl).ToList();

               
                IQueryable<int> listaODTS = lista.Select(x => x.OrdenTrabajoSpoolID).AsQueryable();

                ctx.JuntaWorkstatus.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.JuntaSpool.MergeOption = System.Data.Objects.MergeOption.NoTracking;

                jwks = ctx.JuntaWorkstatus.Where(x => listaODTS.Contains(x.OrdenTrabajoSpoolID) && x.JuntaFinal).ToList();
                IQueryable<OrdenTrabajoJunta> ordenTrabajoJunta = ctx.OrdenTrabajoJunta.Where(y => listaODTS.Contains(y.OrdenTrabajoSpoolID)).AsQueryable();
                js = ctx.JuntaSpool.Where(x => ordenTrabajoJunta.Select(y => y.JuntaSpoolID).Contains(x.JuntaSpoolID)).ToList();
                otj = ordenTrabajoJunta.ToList();

                lista.ForEach(x => calculaEstatus(x, jwks, js, otj, fabArea, jTH, jTW));


                return lista;
            }
        }        
        /// <summary>
        /// Calcula el estatus de cada spool en base al estatus de todas sus juntas.
        /// </summary>
        /// <param name="grd"></param>
        /// <param name="juntaWks"></param>
        /// <param name="juntaSpool"></param>
        /// <param name="otj"></param>
        /// <param name="fabAreaID"></param>
        /// <param name="jTH"></param>
        /// <param name="jTW"></param>
        private void calculaEstatus(GrdRequisicionSpoolPruebas grd, List<JuntaWorkstatus> juntaWks, List<JuntaSpool> juntaSpool, List<OrdenTrabajoJunta> otj, int fabAreaID, int jTH, int jTW)
        {
            IEnumerable<JuntaWorkstatus> jwks = juntaWks.Where(y => y.OrdenTrabajoSpoolID == grd.OrdenTrabajoSpoolID);
            IEnumerable<OrdenTrabajoJunta> otjQuery = otj.Where(x => x.OrdenTrabajoSpoolID == grd.OrdenTrabajoSpoolID);
            IEnumerable<JuntaSpool> js = juntaSpool.Where(x => otjQuery.Select(y => y.JuntaSpoolID).Contains(x.JuntaSpoolID));
            int numJuntas = js.Where(x => x.FabAreaID == fabAreaID).Count();
            int numSoldaduras = js.Where(x => x.FabAreaID == fabAreaID && x.TipoJuntaID != jTH && x.TipoJuntaID != jTW).Count();
            grd.Armado = jwks.Where(x => x.ArmadoAprobado).Count() == numJuntas;
            grd.Soldadura = jwks.Where(x => x.SoldaduraAprobada
                                            && js.Where(y => y.JuntaSpoolID == x.JuntaSpoolID).Select(y => y.TipoJuntaID).FirstOrDefault() != jTH
                                            && js.Where(y => y.JuntaSpoolID == x.JuntaSpoolID).Select(y => y.TipoJuntaID).FirstOrDefault() != jTW).Count()
                                        == numSoldaduras;
        }



        /// <summary>
        /// Genera la requisición de pruebas para un set de juntas recibidas
        /// </summary>
        /// <param name="requisicion">Requisicion a Generar</param>
        /// <param name="ids">IDs de JuntaWorkstatus a relacionar con la requisicion</param>
        /// <param name="UserUID">ID del usuario logeado</param>
        public void GeneraRequisicion(Requisicion requisicion, string ids, Guid UserUID)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                using (SamContext ctx = new SamContext())
                {
                    //Validar si el numero de requisicion ya existe en la base de datos para el proyecto y tipo de prueba
                    Requisicion requisicionExistente = ctx.Requisicion.Where(x => x.NumeroRequisicion == requisicion.NumeroRequisicion && x.ProyectoID == requisicion.ProyectoID && x.TipoPruebaID == requisicion.TipoPruebaID).SingleOrDefault();
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

                    string[] juntasArreglo = ids.Split(',');

                    foreach (string juntaID in juntasArreglo)
                    {
                        int jID = juntaID.SafeIntParse();
                        WorkstatusSpool wss = null; 
                        JuntaWorkstatus jws = null; 

                        // jID es juntaSpool y no cuenta con workstatus
                        // damos de alta la juntaWorkStatus
                        if(jID < 0)
                        {
                            JuntaSpool js = ctx.JuntaSpool.Include("Spool.OrdenTrabajoSpool").Include("JuntaWorkstatus").SingleOrDefault(x => x.JuntaSpoolID == Math.Abs(jID)); 
                            jws = js.JuntaWorkstatus.SingleOrDefault(); 
                            wss = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID == js.Spool.OrdenTrabajoSpool.Single().OrdenTrabajoSpoolID).FirstOrDefault(); 
 
                            js.StartTracking();

                             if (jws == null) 
                             { 
                                 js.JuntaWorkstatus.Add(new JuntaWorkstatus 
                                 { 
                                     OrdenTrabajoSpoolID = js.Spool.OrdenTrabajoSpool.Single().OrdenTrabajoSpoolID, 
                                     EtiquetaJunta = js.Etiqueta, 
                                     ArmadoAprobado = false, 
                                     SoldaduraAprobada = false, 
                                     InspeccionVisualAprobada = false, 
                                     VersionJunta = 1, 
                                     JuntaFinal = true, 
                                     ArmadoPagado = false, 
                                     SoldaduraPagada = false, 
                                     UsuarioModifica = UserUID, 
                                     FechaModificacion = DateTime.Now 
                                 });  
 
                                 js.StopTracking(); 
                                 ctx.JuntaSpool.ApplyChanges(js); 
                                 ctx.SaveChanges(); 
                             }  
 
                             jID = js.JuntaWorkstatus.SingleOrDefault().JuntaWorkstatusID; 
                         } 
                         else 
                         { 
                             jws = ctx.JuntaWorkstatus.Where(x => x.JuntaWorkstatusID == jID).FirstOrDefault(); 
                             wss = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID == jws.OrdenTrabajoSpoolID).FirstOrDefault(); 
                         } 

                        #region Validaciones

                        if (wss != null && jws.VersionJunta == 1) 
                        {                            
                            string fv = ValidaFechasBO.Instance.ValidaFechasProcesoFechaRequiPinturas(requisicion.FechaRequisicion, jws.OrdenTrabajoSpoolID);  
 
                            if (!string.IsNullOrEmpty(fv)) 
                            {                                   
                                throw new ExcepcionPintura(string.Format(MensajesError.Excepcion_FechaRequiMayorFechaPintura, fv )); 
                            }                             
                        }  
 
                        JuntaRequisicion jtaRequisicion = ctx.JuntaRequisicion.Where(x => x.JuntaWorkstatusID == jID && x.Requisicion.TipoPruebaID == requisicion.TipoPruebaID).OrderByDescending(x => x.RequisicionID).FirstOrDefault(); 
                          
                        if(jtaRequisicion != null) 
                        {                             
                            Requisicion requisicionA = ctx.Requisicion.Where(x => x.RequisicionID == jtaRequisicion.RequisicionID).FirstOrDefault(); 
                            JuntaReportePnd jrpnd = ctx.JuntaReportePnd.Where(x => x.JuntaWorkstatusID == jtaRequisicion.JuntaWorkstatusID && x.JuntaRequisicionID == jtaRequisicion.JuntaRequisicionID).FirstOrDefault(); 
                              
                            if(jrpnd == null) 
                            { 
                                string nc = ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajoSpoolID == jws.OrdenTrabajoSpoolID).Select(x => x.NumeroControl).FirstOrDefault();                            
                                throw new ExcepcionDuplicados(string.Format(MensajesError.Excepcion_JuntaTieneRequisicionReporte, jws.EtiquetaJunta, nc, requisicionA.NumeroRequisicion)); 
                            } 
                        } 


                        List<int> jtaReq = ctx.JuntaRequisicion.Where(x => x.JuntaWorkstatusID == jID && x.Requisicion.TipoPruebaID == requisicion.TipoPruebaID).Select(x => x.JuntaRequisicionID).ToList();

                        //Verifico que si el tipo de prueba es Post-TT o Post-TT exista previamente un resultado positivo de PWHT
                        if (requisicion.TipoPruebaID == (int)TipoPruebaEnum.RTPostTT || requisicion.TipoPruebaID == (int)TipoPruebaEnum.PTPostTT)
                        {
                            if (!ctx.JuntaReporteTt.Where(x => x.JuntaWorkstatusID == jID && x.ReporteTt.TipoPruebaID == (int)TipoPruebaEnum.Pwht && x.Aprobado).Any())
                            {
                                throw new ExcepcionReportes(string.Format(MensajesError.Excepcion_ReportePostSinPWHT));
                            }
                        }

                        string categoriaPrueba = CacheCatalogos.Instance.ObtenerTiposPrueba().Where(x => x.ID == requisicion.TipoPruebaID).Select(x => x.Categoria).Single();
                        switch (categoriaPrueba)
                        {
                            //Verifico que no exista ya una requisicion del mismo tipo de prueba a menos que la prueba haya sido rechazada
                            case CategoriaTipoPrueba.TT:
                                if (jtaReq.Any())
                                {
                                    List<JuntaReporteTt> jrTt = ctx.JuntaReporteTt.Where(x => jtaReq.Contains(x.JuntaRequisicionID)).ToList();
                                    if (jrTt != null)
                                    {
                                        if (jrTt.Where(x => x.Aprobado).Any())
                                        {
                                            JuntaWorkstatus junta = ctx.JuntaWorkstatus.FirstOrDefault(x => x.JuntaWorkstatusID == jID);
                                            throw new ExcepcionDuplicados(string.Format(MensajesError.Excepcion_ReportePruebaDuplicada, junta.EtiquetaJunta));
                                        }
                                    }                                  
                                }
                                break;
                            //Verifico que no exista ya una requisicion del mismo tipo de prueba
                            case CategoriaTipoPrueba.PND:
                                if (jtaReq.Any())
                                {
                                    JuntaWorkstatus junta = ctx.JuntaWorkstatus.FirstOrDefault(x => x.JuntaWorkstatusID == jID);
                                    throw new ExcepcionDuplicados(string.Format(MensajesError.Excepcion_RequisicionPruebaDuplicada, junta.EtiquetaJunta));
                                }
                                break;
                        }

                        #endregion

                        JuntaRequisicion juntaReq = new JuntaRequisicion
                        {
                            Requisicion = requisicion,
                            JuntaWorkstatusID = jID,
                            UsuarioModifica = UserUID,
                            FechaModificacion = DateTime.Now
                        };

                        requisicion.JuntaRequisicion.Add(juntaReq);
                    }

                    if (requisicionExistente != null)
                    {
                        requisicion.StopTracking();
                    }

                    ctx.Requisicion.ApplyChanges(requisicion);
                    ctx.SaveChanges();
                }

                ts.Complete();
            }
        }

        /// <summary>
        /// Genera la requisición de pruebas para un set de spools recibidos
        /// </summary>
        /// <param name="requisicion">Requisicion a Generar</param>
        /// <param name="ids">IDs de OrdenTrabajoSpoolID a relacionar con la requisicion</param>
        /// <param name="UserUID">ID del usuario logeado</param>
        public void GeneraRequisicionSpool(RequisicionSpool requisicionSpool, string otsids, Guid UserUID)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                using (SamContext ctx = new SamContext())
                {
                    //Validar si el numero de requisicion ya existe en la base de datos para el proyecto y tipo de prueba
                    RequisicionSpool requisicionExistente = ctx.RequisicionSpool.Where(x => x.NumeroRequisicion == requisicionSpool.NumeroRequisicion &&
                                                                                            x.ProyectoID == requisicionSpool.ProyectoID &&
                                                                                            x.TipoPruebaSpoolID == requisicionSpool.TipoPruebaSpoolID)
                                                                                .SingleOrDefault();
                    if (requisicionExistente != null)
                    {
                        //Validando que las fechas concuerden
                        if (requisicionExistente.FechaRequisicion != requisicionSpool.FechaRequisicion)
                        {
                            throw new ExcepcionReportes(MensajesError.Excepcion_RequisicionExistenteConFechaDiferente);
                        }
                        else
                        {
                            requisicionSpool = requisicionExistente;

                            requisicionSpool.StartTracking();
                            requisicionSpool.UsuarioModifica = UserUID;
                            requisicionSpool.FechaModificacion = DateTime.Now;
                        }
                    }
                    else
                    {
                        requisicionSpool.UsuarioModifica = UserUID;
                        requisicionSpool.FechaModificacion = DateTime.Now;
                    }

                    string[] otsArreglo = otsids.Split(',');

                    foreach (string otsid in otsArreglo)
                    {
                        int jID = otsid.SafeIntParse();                       

                        #region Validaciones

                        WorkstatusSpool wks = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID == jID).FirstOrDefault();
                        if (wks != null)
                        {
                            SpoolRequisicion jtaReq = ctx.SpoolRequisicion.Include("RequisicionSpool").Where(x => x.WorkstatusSpoolID == wks.WorkstatusSpoolID &&
                                                                           x.RequisicionSpool.TipoPruebaSpoolID == requisicionSpool.TipoPruebaSpoolID).OrderByDescending(x => x.SpoolRequisicionID).FirstOrDefault();

                            if (jtaReq != null)
                            {
                                SpoolReportePnd srpnd = ctx.SpoolReportePnd.Where(x => x.SpoolRequisicionID == jtaReq.SpoolRequisicionID).OrderByDescending(x => x.SpoolReportePndID).FirstOrDefault();

                                if ((srpnd != null && srpnd.Aprobado) || (srpnd == null))
                                {
                                    IEnumerable<Simple> spools = (from s in ctx.Spool
                                                                  join ots in ctx.OrdenTrabajoSpool on s.SpoolID equals ots.SpoolID
                                                                  join wss in ctx.WorkstatusSpool on ots.OrdenTrabajoSpoolID equals wss.OrdenTrabajoSpoolID
                                                                  where wss.WorkstatusSpoolID == jtaReq.WorkstatusSpoolID
                                                                  select new Simple
                                                                  {
                                                                      Valor = s.Nombre,
                                                                      ID = s.SpoolID
                                                                  });

                                    Simple spool = spools.Select(x => x).FirstOrDefault();

                                    if (requisicionSpool.RequisicionSpoolID == jtaReq.RequisicionSpoolID)
                                    {
                                        throw new ExcepcionDuplicados(string.Format(MensajesError.Excepcion_ReporteConRequisicionDetalleDuplicado, spool.Valor));
                                    }
                                    else
                                    {
                                        throw new ExcepcionDuplicados(string.Format(MensajesError.Excepcion_ReporteConRequisicionDetalleDuplicadoEnOtroReporte, spool.Valor, jtaReq.RequisicionSpool.NumeroRequisicion));
                                    }
                                }
                            }
                        }
                        #endregion

                       
                        if (wks != null)
                        {
                            SpoolRequisicion spoolReq = new SpoolRequisicion
                            {
                                RequisicionSpool = requisicionSpool,
                                WorkstatusSpoolID = wks.WorkstatusSpoolID,
                                UsuarioModifica = UserUID,
                                FechaModificacion = DateTime.Now
                            };

                            requisicionSpool.SpoolRequisicion.Add(spoolReq);
                        }
                        else 
                        {
                            WorkstatusSpool wss = new WorkstatusSpool 
                            {
                                OrdenTrabajoSpoolID = jID,
                                UltimoProcesoID=(int)UltimoProcesoEnum.PruebasporSpool,
                                TieneLiberacionDimensional=false,
                                TieneRequisicionPintura=false,
                                TienePintura=false,
                                LiberadoPintura=false,
                                Preparado = false,
                                Embarcado = false,
                                Certificado=false,
                                UsuarioModifica = UserUID,
                                FechaModificacion=DateTime.Now
                            };

                            ctx.WorkstatusSpool.ApplyChanges(wss);
                            ctx.SaveChanges();

                            SpoolRequisicion spoolReq = new SpoolRequisicion
                            {
                                RequisicionSpool = requisicionSpool,
                                WorkstatusSpoolID = wss.WorkstatusSpoolID,
                                UsuarioModifica = UserUID,
                                FechaModificacion = DateTime.Now
                            };
                            requisicionSpool.SpoolRequisicion.Add(spoolReq);
                        }
                    }

                    if (requisicionExistente != null)
                    {
                        requisicionSpool.StopTracking();
                    }

                    ctx.RequisicionSpool.ApplyChanges(requisicionSpool);
                    ctx.SaveChanges();
                }

                ts.Complete();
            }
        }
    }
}
