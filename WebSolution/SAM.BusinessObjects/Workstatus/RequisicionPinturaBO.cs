using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.BusinessObjects.Produccion;
using SAM.BusinessObjects.Excepciones;
using SAM.Entities.Personalizadas;
using System.Data.Objects;
using SAM.BusinessObjects.Utilerias;
using System.Collections;

namespace SAM.BusinessObjects.Workstatus
{
    public class RequisicionPinturaBO
    {
        //variables de instancia
        private static readonly object _mutex = new object();
        private static RequisicionPinturaBO _instance;

        /// <summary>
        /// constructor para implementar el patrón Singleton.
        /// </summary>
        private RequisicionPinturaBO()
        {
        }

        /// <summary>
        /// permite la creación de una instancia de la clase
        /// </summary>
        public static RequisicionPinturaBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new RequisicionPinturaBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// devuelve un listado de spool para el listado de requisicion pintura.
        /// </summary>
        /// <param name="proyectoId"></param>
        /// <param name="ordenTrabajoID"></param>
        /// <returns></returns>
        public List<GrdRequisicionPintura> ObtenerListadoRequisicionPintura(int proyectoId, int ? ordenTrabajoID, int accion)
        {
            List<GrdRequisicionPintura> reqP = null;
            List<Grupo> cuentaPeqs = null;

            using (SamContext ctx = new SamContext())
            {
                //Especificar sistema
                if (accion == 1)
                {

                    reqP =
                            (from s in ctx.Spool
                             join c in ctx.Cuadrante on s.CuadranteID equals c.CuadranteID into cuadranteDef
                             from t3 in cuadranteDef.DefaultIfEmpty()
                             join ots in ctx.OrdenTrabajoSpool on s.SpoolID equals ots.SpoolID into otsDef
                             from t1 in otsDef.DefaultIfEmpty()
                             join wks in ctx.WorkstatusSpool on t1.OrdenTrabajoSpoolID equals wks.OrdenTrabajoSpoolID into wksDef
                             from t2 in wksDef.DefaultIfEmpty()
                             where s.ProyectoID == proyectoId && (ordenTrabajoID == null || t1.OrdenTrabajoID == ordenTrabajoID)
                             where (t2.TieneRequisicionPintura == null || t2.TieneRequisicionPintura == false)
                             let sph = s.SpoolHold
                             select new GrdRequisicionPintura
                             {
                                 WorkstatusSpoolID = t2.WorkstatusSpoolID,
                                 NombreSpool = s.Nombre,
                                 EspecificacionSpool = s.Especificacion,
                                 NumeroControl = t1.NumeroControl,
                                 Sistema = s.SistemaPintura,
                                 Color = s.ColorPintura,
                                 Codigo = s.CodigoPintura,
                                 Hold = sph != null ? ((sph.TieneHoldCalidad) || (sph.TieneHoldIngenieria) || (sph.Confinado)) : false,
                                 SpoolID = s.SpoolID,
                                 Area = s.Area,
                                 Peso = s.Peso,
                                 Localizacion = t3.Nombre,
                                 Peqs = 0M

                             }).ToList();
                }
                else
                {                    
                    reqP = (from ots in ctx.OrdenTrabajoSpool
                            join s in ctx.Spool on ots.SpoolID equals s.SpoolID
                            join c in ctx.Cuadrante on s.CuadranteID equals c.CuadranteID into cuadranteDef
                            from t3 in cuadranteDef.DefaultIfEmpty()
                            join wks in ctx.WorkstatusSpool on ots.OrdenTrabajoSpoolID equals wks.OrdenTrabajoSpoolID into wksDef
                            from t4 in wksDef.DefaultIfEmpty()
                            join r in ctx.RequisicionPinturaDetalle on t4.WorkstatusSpoolID equals r.WorkstatusSpoolID into reqDef
                            from t5 in reqDef.DefaultIfEmpty()
                            where s.ProyectoID == proyectoId && (ordenTrabajoID == null || ots.OrdenTrabajoID == ordenTrabajoID)                                       
                            let sph = s.SpoolHold
                            select new GrdRequisicionPintura
                            {
                                OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID,
                                WorkstatusSpoolID = t4.WorkstatusSpoolID,
                                NombreSpool = s.Nombre,
                                NumeroControl = ots.NumeroControl,
                                Sistema = s.SistemaPintura,
                                Color = s.ColorPintura,
                                Codigo = s.CodigoPintura,
                                Hold = sph != null ? ((sph.TieneHoldCalidad) || (sph.TieneHoldIngenieria) || (sph.Confinado)) : false,
                                SpoolID = s.SpoolID,
                                Area = s.Area,
                                Peso = s.Peso,
                                Localizacion = t3.Nombre,
                                Peqs = 0M,
                                TieneRequisicionPintura = t4.TieneRequisicionPintura ? t4.TieneRequisicionPintura : false,
                                RequisicionPinturaDetalleID = t5.RequisicionPinturaDetalleID

                            }).ToList();

                    reqP = reqP.Where(x =>  x.TieneRequisicionPintura != true ).ToList();
                }

                List<int> spoolIds = reqP.Select(x => x.SpoolID).ToList();

                cuentaPeqs =
                    (from jta in ctx.JuntaSpool.Where(x => spoolIds.Contains(x.SpoolID) && x.FabAreaID == CacheCatalogos.Instance.ShopFabAreaID)
                     group jta by jta.SpoolID into g
                     select new Grupo
                     {
                         ID = g.Key,
                         SumaDecimal = g.Sum(x => x.Peqs),
                         Cuenta = g.Count(),
                     }).ToList();

         
                return (from rp in reqP
                             let j = cuentaPeqs.SingleOrDefault(x => x.ID == rp.SpoolID)                            
                             select new GrdRequisicionPintura
                            {
                                OrdenTrabajoSpoolID = rp.OrdenTrabajoSpoolID,
                                WorkstatusSpoolID = rp.WorkstatusSpoolID,
                                NombreSpool = rp.NombreSpool,
                                NumeroControl = rp.NumeroControl,
                                Sistema = rp.Sistema,
                                Color = rp.Color,
                                Codigo = rp.Codigo,
                                Hold = rp.Hold,
                                SpoolID = rp.SpoolID,
                                Area = rp.Area,
                                Peso = rp.Peso,
                                Localizacion = rp.Localizacion,
                                Peqs = j != null ? (j.SumaDecimal.HasValue ? j.SumaDecimal.Value : 0) : 0,
                            }).ToList();
                            
                           
            }
        }

        public void GeneraRequisicion(int proyectoID, string numeroRequisicion, DateTime fecha, int[] otsIDs, Guid userUID)
        {
            using (SamContext ctx = new SamContext())
            {
                string SpoolFechanValida = string.Empty;
                string SpoolConReq = string.Empty;

                RequisicionPintura reqPintura = new RequisicionPintura
                {
                    ProyectoID = proyectoID,
                    FechaRequisicion = fecha,
                    NumeroRequisicion = numeroRequisicion
                };

                RequisicionPintura requisicionExistente = ctx.RequisicionPintura.Where(x => x.NumeroRequisicion == numeroRequisicion && x.ProyectoID == proyectoID).SingleOrDefault();
                if (requisicionExistente != null)
                {
                    //Validando que las fechas concuerden
                    if (requisicionExistente.FechaRequisicion != fecha)
                    {
                        throw new ExcepcionReportes(MensajesError.Excepcion_RequisicionExistenteConFechaDiferente);
                    }
                    else
                    {
                        reqPintura = requisicionExistente;

                        reqPintura.StartTracking();
                        reqPintura.UsuarioModifica = userUID;
                        reqPintura.FechaModificacion = DateTime.Now;
                    }
                }
                else
                {
                    reqPintura.UsuarioModifica = userUID;
                    reqPintura.FechaModificacion = DateTime.Now;
                }                

                foreach (int otsID in otsIDs)
                {
                    if (otsID > -1)
                    {
                        WorkstatusSpool wks = WorkstatusSpoolBO.Instance.ObtenerPorOrdenTrabajoSpool(otsID);

                        if (wks == null)
                        {
                            wks = new WorkstatusSpool()
                            {
                                OrdenTrabajoSpoolID = otsID,
                                UsuarioModifica = userUID,
                                FechaModificacion = DateTime.Now,
                                TieneRequisicionPintura = true
                            };

                            ctx.WorkstatusSpool.ApplyChanges(wks);
                            ctx.SaveChanges();
                            // wks = WorkstatusSpoolBO.Instance.ObtenerPorOrdenTrabajoSpool(otsID);
                        }
                        else
                        {
                            RequisicionPinturaDetalle rpd = ctx.RequisicionPinturaDetalle.Where(x => x.WorkstatusSpoolID == wks.WorkstatusSpoolID).FirstOrDefault();
                            
                            if(rpd == null)
                            {
                                PinturaSpool ps = ctx.PinturaSpool.Where(x => x.WorkstatusSpoolID == wks.WorkstatusSpoolID).FirstOrDefault();

                                if (ps != null)
                                {
                                    if (!FechaValida(reqPintura.FechaRequisicion, ps))
                                    {
                                        SpoolFechanValida += (from s in ctx.Spool
                                                              join ots in ctx.OrdenTrabajoSpool on s.SpoolID equals ots.SpoolID
                                                              join wKS in ctx.WorkstatusSpool on ots.OrdenTrabajoSpoolID equals wks.OrdenTrabajoSpoolID
                                                              where (wKS.WorkstatusSpoolID == ps.WorkstatusSpoolID)
                                                              select s.Nombre).FirstOrDefault() + ", ";
                                        continue;
                                    }
                                }

                                wks.StartTracking();
                                wks.TieneRequisicionPintura = true;
                                wks.UsuarioModifica = userUID;
                                wks.FechaModificacion = DateTime.Now;
                                wks.StopTracking();
                                ctx.WorkstatusSpool.ApplyChanges(wks);
                            }
                            else
                            {                           
                                SpoolConReq += (from s in ctx.Spool
                                                  join ots in ctx.OrdenTrabajoSpool on s.SpoolID equals ots.SpoolID
                                                  join wKS in ctx.WorkstatusSpool on ots.OrdenTrabajoSpoolID equals wks.OrdenTrabajoSpoolID
                                                  where (wKS.WorkstatusSpoolID == wks.WorkstatusSpoolID)
                                                  select s.Nombre).FirstOrDefault() + ", ";                            
                                continue;
                            }
                        }            
                        
                        RequisicionPinturaDetalle detalle = new RequisicionPinturaDetalle
                        {
                            WorkstatusSpoolID = wks.WorkstatusSpoolID,
                            RequisicionPintura = reqPintura,
                            UsuarioModifica = userUID,
                            FechaModificacion = DateTime.Now
                        };                     
                                            
                       reqPintura.RequisicionPinturaDetalle.Add(detalle); 
                    }
                }

                ctx.RequisicionPintura.ApplyChanges(reqPintura);
                ctx.SaveChanges();

                string mensaje = string.Empty;

                if (!string.IsNullOrEmpty(SpoolConReq))
                {
                    mensaje += string.Format(MensajesError.Exception_SpoolTienenRequisicion, SpoolConReq + ". " );
                }

                if (!string.IsNullOrEmpty(SpoolFechanValida))
                { 
                    mensaje += string.Format(MensajesError.Exception_FechaProcesosPinturaMayorReq, SpoolFechanValida);
                }  
  
                if(!string.IsNullOrEmpty(mensaje))
                {
                      throw new ExcepcionRelaciones( mensaje);
                }
            }
        }

        private bool FechaValida(DateTime fechaReq, PinturaSpool ps)
        {
            bool result = true;

            if (ps.FechaSandblast.HasValue)
            {
                if (fechaReq > ps.FechaSandblast)
                {
                    return false;
                }
            }

            if(ps.FechaPrimarios.HasValue)
            {
                if (fechaReq > ps.FechaPrimarios)
                {
                    return false;
                }
            }

            if (ps.FechaIntermedios.HasValue)
            {
                if (fechaReq > ps.FechaIntermedios)
                {
                    return false;
                }
            }

            if (ps.FechaAdherencia.HasValue)
            {
                if (fechaReq > ps.FechaAdherencia)
                {
                    return false;
                }
            }

            if (ps.FechaAcabadoVisual.HasValue)
            {
                if (fechaReq > ps.FechaAcabadoVisual)
                {
                    return false;
                }
            }

            if (ps.FechaPullOff.HasValue)
            {
                if (fechaReq > ps.FechaPullOff)
                {
                    return false;
                }
            }           

            return result;            
        }

        /// <summary>
        /// Obtiene el listado de requisiciones de pintura para un proyecto en particular y una orden de trabajo
        /// </summary>
        /// <param name="proyectoID">ID de proyecto</param>
        /// <param name="ordenTrabajoID">ID de la orden de trabajo (-1) si se requieren todas las del proyecto</param>
        /// <returns>Lista de Requisiciones de Pintura</returns>
        public List<Simple> ObtenerListadoRequisicion(int proyectoID, int ordenTrabajoID)
        {
            List<Simple> reqList = null;

            using (SamContext ctx = new SamContext())
            {
                IQueryable<RequisicionPintura> requisiciones = ctx.RequisicionPintura.Where(x => x.ProyectoID == proyectoID).AsQueryable();

                if (ordenTrabajoID > 0)
                {
                    reqList = (from req in requisiciones
                               join detalle in ctx.RequisicionPinturaDetalle on req.RequisicionPinturaID equals detalle.RequisicionPinturaID
                               join wks in ctx.WorkstatusSpool on detalle.WorkstatusSpoolID equals wks.WorkstatusSpoolID
                               join ot in ctx.OrdenTrabajoSpool on wks.OrdenTrabajoSpoolID equals ot.OrdenTrabajoSpoolID
                               where ot.OrdenTrabajoID == ordenTrabajoID
                               select new Simple
                               {
                                   ID = req.RequisicionPinturaID,
                                   Valor = req.NumeroRequisicion
                               }).Distinct().ToList();
                }
                else
                {
                    reqList = (from req in requisiciones
                               select new Simple
                               {
                                   ID = req.RequisicionPinturaID,
                                   Valor = req.NumeroRequisicion
                               }).Distinct().ToList();
                }
            }

            return reqList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requisicionPinturaID"></param>
        /// <returns></returns>
        public int ObtenerProyectoID(int requisicionPinturaID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ProyectoRequisicionPintura(ctx, requisicionPinturaID);
            }
        }

        public RequisicionPintura ObtenerRequisicionPintura(int requisicionPinturaID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.RequisicionPintura.Where(x => x.RequisicionPinturaID == requisicionPinturaID).SingleOrDefault();
            }
        }

        /// <summary>
        /// Versión compilada del query para permisos de requisiciones de pintura
        /// </summary>
        private static readonly Func<SamContext, int, int> ProyectoRequisicionPintura =
        CompiledQuery.Compile<SamContext, int, int>
        (
            (ctx, id) => ctx.RequisicionPintura
                            .Where(x => x.RequisicionPinturaID == id)
                            .Select(x => x.ProyectoID)
                            .Single()
        );


        public RequisicionPintura ObtenerRequisicionPinturaByWks(int workstatusSpoolId)
        {
            using (SamContext ctx = new SamContext())
            {
               int  rpId =  ctx.RequisicionPinturaDetalle.Where(x => x.WorkstatusSpoolID == workstatusSpoolId).OrderByDescending(x => x.RequisicionPinturaDetalleID).Select(x => x.RequisicionPinturaID).FirstOrDefault();

               return ctx.RequisicionPintura.Include("RequisicionPinturaDetalle").Where(x => x.RequisicionPinturaID == rpId ).FirstOrDefault();
            }
        }

        public void GeneraRequisicionShop(int proyectoID, string numeroRequisicion, DateTime fecha, int[] workstatusIDs, Guid userUID)
        {
            using (SamContext ctx = new SamContext())
            {

                RequisicionPintura reqPintura = new RequisicionPintura
                {
                    ProyectoID = proyectoID,
                    FechaRequisicion = fecha,
                    NumeroRequisicion = numeroRequisicion
                };

                RequisicionPintura requisicionExistente = ctx.RequisicionPintura.Where(x => x.NumeroRequisicion == numeroRequisicion && x.ProyectoID == proyectoID).SingleOrDefault();
                if (requisicionExistente != null)
                {
                    //Validando que las fechas concuerden
                    if (requisicionExistente.FechaRequisicion != fecha)
                    {
                        throw new ExcepcionReportes(MensajesError.Excepcion_RequisicionExistenteConFechaDiferente +" " + requisicionExistente.FechaRequisicion.ToShortDateString());
                    }
                    else
                    {
                        reqPintura = requisicionExistente;

                        reqPintura.StartTracking();
                        reqPintura.UsuarioModifica = userUID;
                        reqPintura.FechaModificacion = DateTime.Now;
                    }
                }
                else
                {
                    reqPintura.UsuarioModifica = userUID;
                    reqPintura.FechaModificacion = DateTime.Now;
                }


                foreach (int workstatusID in workstatusIDs)
                {
                    if (workstatusID > -1)
                    {
                        WorkstatusSpool wks = WorkstatusSpoolBO.Instance.Obtener(workstatusID);

                        wks.StartTracking();
                        wks.TieneRequisicionPintura = true;
                        wks.UsuarioModifica = userUID;
                        wks.FechaModificacion = DateTime.Now;
                        wks.StopTracking();

                        RequisicionPinturaDetalle detalle = ctx.RequisicionPinturaDetalle.Where(x => x.WorkstatusSpoolID == wks.WorkstatusSpoolID).FirstOrDefault();
                        if (detalle == null)
                        {
                            detalle = new RequisicionPinturaDetalle
                            {
                                WorkstatusSpoolID = workstatusID,
                                RequisicionPintura = reqPintura,                                
                            };
                        }
                        detalle.UsuarioModifica = userUID;
                        detalle.FechaModificacion = DateTime.Now;
                      
                        reqPintura.RequisicionPinturaDetalle.Add(detalle);                   
                        
                        ctx.WorkstatusSpool.ApplyChanges(wks);
                    }
                }

                ctx.RequisicionPintura.ApplyChanges(reqPintura);
                ctx.SaveChanges();
            }
        }

    }
}
