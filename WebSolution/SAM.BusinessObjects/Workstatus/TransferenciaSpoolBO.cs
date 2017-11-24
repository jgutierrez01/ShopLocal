using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Excepciones;
using SAM.Entities.Personalizadas;
using SAM.Web;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Exceptions;
using log4net;

namespace SAM.BusinessObjects.Workstatus
{
    public class TransferenciaSpoolBO
    {
        private static readonly object _mutex = new object();
        private static TransferenciaSpoolBO _instance;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(TransferenciaSpoolBO));

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private TransferenciaSpoolBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase InspeccionDimensionalBO
        /// </summary>
        /// <returns></returns>
        public static TransferenciaSpoolBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new TransferenciaSpoolBO();
                    }
                }
                return _instance;
            }
        }



        /// <summary>
        /// Obtiene el listado de spool dependiendo de la accion a realizar en Transferencia
        /// </summary>
        /// <param name="proyectoID">ID del proyecto</param>
        /// <param name="ordenTrabajoID">ID de la orden de trabajo</param>
        /// <param name="accion">Accion a realizar (1) preparar (2) Transferir</param>
        /// <returns></returns>
        public List<GrdTransferenciaSpool> ObtenListadoParaTransferencia(int proyectoID, int ordenTrabajoID, int accion)
        {
            List<GrdTransferenciaSpool> lista = new List<GrdTransferenciaSpool>();
            List<GrdTransferenciaSpool> lista2 = new List<GrdTransferenciaSpool>();
            IQueryable<SpoolHold> holds = null;
            IQueryable<OrdenTrabajoSpool> OrdenesNoEnTransfSpool = null;
            IQueryable<OrdenTrabajoSpool> OrdenesEnTransfSpoolNoTran = null;
            IQueryable<OrdenTrabajoSpool> OrdenesEnTransfSpoolyTransferido = null;
            IQueryable<OrdenTrabajoSpool> MostrarOrdenesTransferidos = null;
            IQueryable<OrdenTrabajoSpool> ListaOrdenesAMostrar = null;
            IQueryable<OrdenTrabajoSpool> ordenesTrabajoSpool = null;
            IQueryable<OrdenTrabajo> ordenesTrabajo = null;
            IEnumerable<Spool> spool = null;

            using (SamContext ctx = new SamContext())
            {
                ordenesTrabajoSpool = ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajo.ProyectoID == proyectoID && (ordenTrabajoID == -1 || x.OrdenTrabajoID == ordenTrabajoID));

                ctx.Spool.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.OrdenTrabajo.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.OrdenTrabajoSpool.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.SpoolHold.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.Transferencia.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.TransferenciaSpool.MergeOption = System.Data.Objects.MergeOption.NoTracking;

                //1.- Ordenes de trabajo Spool que no esten en Transferencia Spool. Se deben de mostrar estos registros
                OrdenesNoEnTransfSpool = (from a in ordenesTrabajoSpool
                                          join b in ctx.TransferenciaSpool on a.SpoolID equals b.SpoolID into ts
                                          from trSpool in ts.DefaultIfEmpty()
                                          where trSpool.FechaPreparacion == null
                                          select a).AsQueryable();

                _logger.Debug("ObtenListadoParaTransferencia OrdenesNoEnTransfSpool");

                //2.-Ordendes de trabajo Spool que esten en Transferencia Spool y preparado y que no esten transferidos. Esta lista no debe de mostrarse                                       
                OrdenesEnTransfSpoolNoTran = (from a in ordenesTrabajoSpool
                                              join b in ctx.TransferenciaSpool on a.SpoolID equals b.SpoolID
                                              join c in ctx.Transferencia on b.TransferenciaSpoolID equals c.TransferenciaSpoolID into trans
                                              from transfer in trans.DefaultIfEmpty()
                                              where b.SpoolPreparado && transfer.TransferenciaID == null
                                              select a).AsQueryable();

                _logger.Debug("ObtenListadoParaTransferencia OrdenesEnTransfSpoolNoTran");
                //3.-Ordendes de trabajo Spool que esten en Transferencia Spool y transferidos.Se deben de mostrar
                OrdenesEnTransfSpoolyTransferido = (from a in ordenesTrabajoSpool
                                                    join b in ctx.TransferenciaSpool on a.SpoolID equals b.SpoolID
                                                    join c in ctx.Transferencia on b.TransferenciaSpoolID equals c.TransferenciaSpoolID
                                                    where b.SpoolPreparado
                                                    select a).Distinct().AsQueryable();
                _logger.Debug("ObtenListadoParaTransferencia OrdenesEnTransfSpoolyTransferido");
                //4.-Mostrar Ordenes de trabajo que ya hayan sido transferidos
                MostrarOrdenesTransferidos = (from a in OrdenesEnTransfSpoolyTransferido
                                              where !OrdenesEnTransfSpoolNoTran.Select(x => x.SpoolID).Contains(a.SpoolID)
                                              select a).Distinct().AsQueryable();
                _logger.Debug("ObtenListadoParaTransferencia MostrarOrdenesTransferidos");
                //preparar
                if (accion == 0)
                {

                    ListaOrdenesAMostrar = MostrarOrdenesTransferidos.Union(OrdenesNoEnTransfSpool).Union(OrdenesEnTransfSpoolNoTran).Distinct();
                    spool = ctx.Spool.Where(x => ListaOrdenesAMostrar.Select(y => y.SpoolID).Contains(x.SpoolID));
                    ordenesTrabajo = ctx.OrdenTrabajo.Where(x => ListaOrdenesAMostrar.Select(y => y.OrdenTrabajoID).Contains(x.OrdenTrabajoID)).AsQueryable();
                    holds = ctx.SpoolHold.Where(x => spool.Select(y => y.SpoolID).Contains(x.SpoolID)).AsQueryable();


                    //Se obtienen los que estan listos a transferir
                    IQueryable<MaxTransferenciaSpool> ColeccionSpoolsFechas = (from o in ctx.TransferenciaSpool
                                                                               join t in ctx.Transferencia on o.TransferenciaSpoolID equals t.TransferenciaSpoolID into trans
                                                                               from transfer in trans.DefaultIfEmpty()
                                                                               join om in ListaOrdenesAMostrar on o.SpoolID equals om.SpoolID
                                                                               group o by o.SpoolID into g
                                                                               select new MaxTransferenciaSpool
                                                                               {
                                                                                   SpoolID = g.Key,
                                                                                   TransferenciaSpoolID = g.Max(m => m.TransferenciaSpoolID)
                                                                               }
                                                                                      ).AsQueryable();

                    _logger.Debug("ObtenListadoParaTransferencia ColeccionSpoolsFechas");
                    //Se obtienen las fechas
                    List<ColeccionFechasTransferencia> coleccionFechas = (from a in ColeccionSpoolsFechas
                                                                          join b in ctx.TransferenciaSpool on a.TransferenciaSpoolID equals b.TransferenciaSpoolID
                                                                          join t in ctx.Transferencia on a.TransferenciaSpoolID equals t.TransferenciaSpoolID into trans
                                                                          from transfer in trans.DefaultIfEmpty()
                                                                          select new ColeccionFechasTransferencia
                                                                          {
                                                                              SpoolID = a.SpoolID,
                                                                              FechaPreparacion = b.FechaPreparacion,
                                                                              FechaTransferencia = transfer.FechaTransferencia,
                                                                              NumeroTransferencia = transfer.NumeroTransferencia,
                                                                              Transferidos = false
                                                                          }
                                                                                          ).ToList();
                    
                    List<OrdenTrabajo> lstordenesTrabajo = ordenesTrabajo.ToList();
                    List<OrdenTrabajoSpool> lstordenesAMostrar = ListaOrdenesAMostrar.ToList();
                    List<SpoolHold> lstHold = holds.ToList();

                    _logger.Debug("ObtenListadoParaTransferencia coleccionFechas");
                    _logger.Debug("ObtenListadoParaTransferencia inicio lista");

                    lista = (from ots in lstordenesAMostrar
                             join ot in lstordenesTrabajo on ots.OrdenTrabajoID equals ot.OrdenTrabajoID
                             join sp in spool on ots.SpoolID equals sp.SpoolID
                             join hold in lstHold on sp.SpoolID equals hold.SpoolID into Holds
                             from hd in Holds.DefaultIfEmpty()
                             join c in coleccionFechas on ots.SpoolID equals c.SpoolID into df
                             from cp in df.DefaultIfEmpty()
                             select new GrdTransferenciaSpool
                             {
                                 TransferenciaSpoolID = -1,
                                 SpoolID = sp.SpoolID,
                                 OrdenTrabajo = ot.NumeroOrden,
                                 OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID,
                                 NumeroControl = ots.NumeroControl,
                                 NombreSpool = sp.Nombre,
                                 Area = sp.Area.Value,
                                 PDI = sp.Pdis.Value,
                                 Peso = sp.Peso.Value,
                                 Etiqueta = sp.NumeroEtiqueta,
                                 Preparado = cp != null ? (cp.FechaPreparacion != null ? true : false) : false,
                                 Transferencia = false,
                                 NumeroTransferencia = cp != null ? cp.NumeroTransferencia : null,
                                 FechaPreparacion = cp != null ? cp.FechaPreparacion : null,
                                 FechaTransferencia = cp != null ? cp.FechaTransferencia : null,
                                 Hold = (hd == null) ? false : hd.TieneHoldCalidad || hd.TieneHoldIngenieria || hd.Confinado
                             }).AsParallel().ToList();
                    _logger.Debug("ObtenListadoParaTransferencia fin lista");

                }
                else if (accion == 1) // Transferir
                {


                    IQueryable<MaxTransferenciaSpool> ColeccionSpools = (from o in ctx.TransferenciaSpool
                                                                         join t in ctx.Transferencia on o.TransferenciaSpoolID equals t.TransferenciaSpoolID into ts
                                                                         from transfer in ts.DefaultIfEmpty()
                                                                         join om in ordenesTrabajoSpool on o.SpoolID equals om.SpoolID
                                                                         where o.SpoolPreparado
                                                                         group o by o.SpoolID into g
                                                                         select new MaxTransferenciaSpool
                                                                         {
                                                                             SpoolID = g.Key,
                                                                             TransferenciaSpoolID = g.Max(m => m.TransferenciaSpoolID)
                                                                         }).AsQueryable();


                    List<ColeccionFechasTransferencia> coleccionFechas = (from a in ColeccionSpools
                                                                          join b in ctx.TransferenciaSpool on a.TransferenciaSpoolID equals b.TransferenciaSpoolID
                                                                          join t in ctx.Transferencia on a.TransferenciaSpoolID equals t.TransferenciaSpoolID into trans
                                                                          from transfer in trans.DefaultIfEmpty()
                                                                          select new ColeccionFechasTransferencia
                                                                          {
                                                                              TransferenciaSpoolID = a.TransferenciaSpoolID,
                                                                              SpoolID = a.SpoolID,
                                                                              FechaPreparacion = b.FechaPreparacion,
                                                                              FechaTransferencia = transfer.FechaTransferencia,
                                                                              NumeroTransferencia = transfer.NumeroTransferencia,
                                                                              Transferidos = false
                                                                          }
                                                                                          ).ToList();

                    ListaOrdenesAMostrar = MostrarOrdenesTransferidos.Union(OrdenesEnTransfSpoolNoTran).Distinct();
                    spool = ctx.Spool.Where(x => ListaOrdenesAMostrar.Select(y => y.SpoolID).Contains(x.SpoolID));
                    ordenesTrabajo = ctx.OrdenTrabajo.Where(x => ListaOrdenesAMostrar.Select(y => y.OrdenTrabajoID).Contains(x.OrdenTrabajoID)).AsQueryable();
                    holds = ctx.SpoolHold.Where(x => spool.Select(y => y.SpoolID).Contains(x.SpoolID)).AsQueryable();
                    
                    List<OrdenTrabajo> lstordenesTrabajo = ordenesTrabajo.ToList();
                    List<OrdenTrabajoSpool> lstordenesAMostrar = ListaOrdenesAMostrar.ToList();
                    List<SpoolHold> lstHold = holds.ToList();

                    _logger.Debug("ObtenListadoParaTransferencia inicio lista");

                    lista = (from ots in lstordenesAMostrar
                             join ot in lstordenesTrabajo on ots.OrdenTrabajoID equals ot.OrdenTrabajoID                            
                             join cp in coleccionFechas on ots.SpoolID equals cp.SpoolID into fp
                             from cfp in fp.DefaultIfEmpty()
                             join sp in spool on ots.SpoolID equals sp.SpoolID
                             join hold in lstHold on sp.SpoolID equals hold.SpoolID into Holds
                             from hd in Holds.DefaultIfEmpty()
                             select new GrdTransferenciaSpool
                             {
                                 TransferenciaSpoolID = cfp != null ? cfp.TransferenciaSpoolID : 0,
                                 SpoolID = sp.SpoolID,
                                 OrdenTrabajo = ot.NumeroOrden,
                                 OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID,
                                 NumeroControl = ots.NumeroControl,
                                 NombreSpool = sp.Nombre,
                                 Area = sp.Area.Value,
                                 PDI = sp.Pdis.Value,
                                 Peso = sp.Peso.Value,
                                 Etiqueta = sp.NumeroEtiqueta,
                                 Preparado = true,
                                 Transferencia = true,
                                 NumeroTransferencia = cfp != null ? cfp.NumeroTransferencia : null,
                                 FechaPreparacion = cfp != null ? cfp.FechaPreparacion : null,
                                 FechaTransferencia = cfp != null ? cfp.FechaTransferencia : null,
                                 Hold = (hd == null) ? false : hd.TieneHoldCalidad || hd.TieneHoldIngenieria || hd.Confinado
                             }).AsParallel().ToList();

                    _logger.Debug("ObtenListadoParaTransferencia FIN lista");

                }


            }




            return lista;
        }


        /// <summary>
        /// Prepara los spoosl enviados
        /// </summary>
        /// <param name="workstatusSpoolsIDs">IDs de los spools a preparar</param>        
        /// <param name="fechaPreparacion">Fecha Preparacion</param>
        /// <param name="userID">Usuario que modifica</param>
        public void PrepararSpools(int[] SpoolsIDs, DateTime fechaPreparacion, Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {

                foreach (int id in SpoolsIDs)
                {
                    MaxTransferenciaSpool ColeccionSpoolsFechasNoTrans = (from o in ctx.TransferenciaSpool
                                                                          join t in ctx.Transferencia on o.TransferenciaSpoolID equals t.TransferenciaSpoolID into trans
                                                                          from transfer in trans.DefaultIfEmpty()
                                                                          where o.SpoolPreparado && transfer.TransferenciaID == null && o.SpoolID == id
                                                                          group o by o.SpoolID into g
                                                                          select new MaxTransferenciaSpool
                                                                          {
                                                                              SpoolID = g.Key,
                                                                              TransferenciaSpoolID = g.Max(m => m.TransferenciaSpoolID)
                                                                          }).SingleOrDefault();
                    if (ColeccionSpoolsFechasNoTrans != null)
                    {
                        TransferenciaSpool transspool = ctx.TransferenciaSpool.Where(x => x.TransferenciaSpoolID == ColeccionSpoolsFechasNoTrans.TransferenciaSpoolID).SingleOrDefault();
                        if (transspool != null)
                        {
                            transspool.FechaPreparacion = fechaPreparacion;
                            transspool.FechaModificacion = DateTime.Now;
                            transspool.UsuarioModifica = userID;
                            ctx.TransferenciaSpool.ApplyChanges(transspool);
                        }
                    }
                    else
                    {

                        TransferenciaSpool ts = new TransferenciaSpool
                        {
                            SpoolID = id,
                            SpoolPreparado = true,
                            FechaPreparacion = fechaPreparacion,
                            UsuarioModifica = userID,
                            FechaModificacion = DateTime.Now
                        };

                        ctx.TransferenciaSpool.ApplyChanges(ts);
                    }
                }
                ctx.SaveChanges();
            }
        }



        public void GuardaTransferencia(int[] TransferenciaSpools, DateTime fechaTransferencia, string NumTransferencia, int destinoid, Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                string spoolFecahNoValida = string.Empty;
                string numeroTransExsite = string.Empty;

                foreach (int id in TransferenciaSpools)
                {
                    bool guardartransferencia = true;
                    Transferencia transferencia = ctx.Transferencia.Where(x => x.TransferenciaSpoolID == id).SingleOrDefault();
                    TransferenciaSpool transferenciaSpool = ctx.TransferenciaSpool.Where(x => x.TransferenciaSpoolID == id).SingleOrDefault();
                    Destino destino = ctx.Destino.Where(x => x.DestinoID == destinoid).SingleOrDefault();
                    OrdenTrabajoSpool ots = ctx.OrdenTrabajoSpool.Include("Spool").Where(x => x.SpoolID == transferenciaSpool.SpoolID).FirstOrDefault();
                    IQueryable<Transferencia> transferenciaNo = ctx.Transferencia.Where(x => x.NumeroTransferencia == NumTransferencia).AsQueryable();

                    //No se puede ingresar una fecha de transferencia diferente para ese numero de transferencia  
                    if (transferenciaNo.FirstOrDefault() != null) 
                    {
                        if (transferenciaNo.FirstOrDefault().FechaTransferencia != fechaTransferencia)
                        {
                            throw new ExcepcionEmbarque(MensajesError.Excepcion_TransferenciaExistenteConFechaDiferente);
                        }                    
                    }                    

                    if (fechaTransferencia < transferenciaSpool.FechaPreparacion)
                    {
                        spoolFecahNoValida += ots.Spool.Nombre + ", ";
                        guardartransferencia = false;
                    }

                    if (transferenciaNo != null)
                    {
                        foreach (var row in transferenciaNo)
                        {
                            TransferenciaSpool transferenciaSpoolDuplicate = ctx.TransferenciaSpool.Where(x => x.TransferenciaSpoolID == row.TransferenciaSpoolID).SingleOrDefault();

                            if (transferenciaSpoolDuplicate.SpoolID == transferenciaSpool.SpoolID)
                            {
                                numeroTransExsite += ots.Spool.Nombre + ", ";
                                guardartransferencia = false;

                            }
                        }
                    }

                    if (guardartransferencia)
                    {
                        if (transferencia != null)
                        {
                            transferencia.NumeroTransferencia = NumTransferencia;
                            transferencia.FechaTransferencia = fechaTransferencia;
                            transferencia.FechaModificacion = DateTime.Now.Date;
                            transferencia.DestinoID = destino.DestinoID;
                            transferencia.UsuarioModifica = userID;
                            ctx.Transferencia.ApplyChanges(transferencia);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            Transferencia trans = new Transferencia
                            {
                                TransferenciaSpoolID = id,
                                NumeroTransferencia = NumTransferencia,
                                FechaTransferencia = fechaTransferencia,
                                DestinoID = destino.DestinoID,
                                UsuarioModifica = userID,
                                FechaModificacion = DateTime.Now
                            };
                            ctx.Transferencia.ApplyChanges(trans);
                        }

                        if (destino != null) 
                        { 
                            CuadranteBO.Instance.GuardarCuadranteSpool(destino.CuadranteID.SafeIntParse(), ots.OrdenTrabajoSpoolID, fechaTransferencia, userID); 
                        } 


                    }
                }

                ctx.SaveChanges();


                if (!string.IsNullOrEmpty(numeroTransExsite))
                {
                    string errorFechas = string.Empty;
                    if (!string.IsNullOrEmpty(spoolFecahNoValida))
                    {
                        errorFechas = "<br><br>" + string.Format(MensajesError.Excepcion_FechaTransferenciaMenorFechaPreparacion, spoolFecahNoValida);
                    }
                    throw new ExcepcionEmbarque(string.Format(MensajesError.Excepcion_TransferenciaExistente + errorFechas, numeroTransExsite));
                }
                else { 
                    if (!string.IsNullOrEmpty(spoolFecahNoValida))
                    {
                        throw new ExcepcionEmbarque(string.Format(MensajesError.Excepcion_FechaTransferenciaMenorFechaPreparacion, spoolFecahNoValida));
                    }
                }                

            }

        }


        public List<ReporteTransferencia> ObtenerTransferenciasSpool(int SpoolID)
        {
            using (SamContext ctx = new SamContext())
            {

                IEnumerable<Spool> spool = ctx.Spool.Where(x => x.SpoolID == SpoolID).AsQueryable();
                IEnumerable<OrdenTrabajoSpool> ordenTrabajoSpool = ctx.OrdenTrabajoSpool.Where(x => x.SpoolID == SpoolID).AsQueryable();

                //reportetrans.Nombre = spool.Select(y => y.Nombre).SingleOrDefault();
                //reportetrans.NumeroControl = ordenTrabajoSpool.Select(y => y.NumeroControl).SingleOrDefault();

                List<ReporteTransferencia> reportetrans = (from o in ctx.TransferenciaSpool
                                                           join t in ctx.Transferencia on o.TransferenciaSpoolID equals t.TransferenciaSpoolID into ts
                                                           from transfer in ts.DefaultIfEmpty()
                                                           join sp in spool on o.SpoolID equals sp.SpoolID
                                                           join ot in ordenTrabajoSpool on sp.SpoolID equals ot.SpoolID
                                                           where o.SpoolPreparado && o.SpoolID == SpoolID
                                                           select new ReporteTransferencia
                                                           {
                                                               TransferenciaSpoolID = o.TransferenciaSpoolID,
                                                               FechaPreparacion = o.FechaPreparacion,
                                                               FechaTransferencia = transfer.FechaTransferencia,
                                                               SpoolID = o.SpoolID,
                                                               NumeroTransferencia = transfer.NumeroTransferencia,
                                                               Etiqueta = sp.NumeroEtiqueta,
                                                               Nombre = sp.Nombre,
                                                               NumeroControl = ot.NumeroControl,
                                                               FechaEtiqueta = sp.FechaEtiqueta
                                                           }
                                                                    ).OrderByDescending(x => x.TransferenciaSpoolID).ToList();

                return reportetrans;
            }
        }


        public void BorrarTransferencia(int transferenciaSpoolID, string tipoAccion,Guid userid)
        {
            using (SamContext ctx = new SamContext())
            {
                string[] AccionTransID = tipoAccion.Split(',');

                int transSpoolID = AccionTransID[1].SafeIntParse();
                int Accion = AccionTransID[0].SafeIntParse();

                switch (Accion)
                {
                    case (int)TipoAccionTransferenciaEnum.Preparacion:

                        Transferencia transferencia = ctx.Transferencia.Where(x => x.TransferenciaSpoolID == transSpoolID).FirstOrDefault();

                        if (transferencia != null) 
                        {
                            throw new BaseValidationException(MensajesError.Excepcion_CuentaConTransferencia);
                        }

                        TransferenciaSpool ts = ctx.TransferenciaSpool.Where(x => x.TransferenciaSpoolID == transSpoolID).FirstOrDefault();

                        if (ts != null)
                        {
                            ctx.TransferenciaSpool.DeleteObject(ts);
                        }

                        break;

                    case (int)TipoAccionTransferenciaEnum.Transferencia:

                        Transferencia Borrartransferencia = ctx.Transferencia.Where(x => x.TransferenciaSpoolID == transSpoolID).FirstOrDefault();
                        TransferenciaSpool trans = ctx.TransferenciaSpool.Where(x => x.TransferenciaSpoolID == transSpoolID).FirstOrDefault();
                        Spool spool = ctx.Spool.Where(x => x.SpoolID == trans.SpoolID).FirstOrDefault();

                        if (Borrartransferencia != null)
                        {
                            Destino destino = ctx.Destino.Where(x => x.DestinoID == Borrartransferencia.DestinoID).FirstOrDefault();
                            if (destino != null)
                            {
                                if (spool.CuadranteID == destino.CuadranteID)
                                {
                                    //modificacmos el registro del Spool
                                    spool.StartTracking();
                                    spool.CuadranteID = null;
                                    spool.FechaModificacion = DateTime.Now;
                                    spool.UsuarioModifica = userid;
                                    spool.FechaLocalizacion = null;
                                    spool.StopTracking();
                                    ctx.Spool.ApplyChanges(spool);

                                    //Eliminamos el ultimo registro que coincida con el cuadrante especificado
                                    CuadranteHistorico historico = ctx.CuadranteHistorico.Where(x => x.SpoolID == spool.SpoolID && x.CuadranteID == destino.CuadranteID).SingleOrDefault(); 
                                    if (historico != null) 
                                    {                                      
                                        //borramos el registro 
                                        ctx.CuadranteHistorico.DeleteObject(historico); 
                                    }                                         
                                 } 
                             }                             
                             
                            ctx.Transferencia.DeleteObject(Borrartransferencia);
                        }

                        break;
                };
               
                ctx.SaveChanges();
                
            }
        }

    }
}
