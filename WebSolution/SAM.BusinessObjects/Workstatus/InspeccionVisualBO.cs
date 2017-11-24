using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.BusinessObjects.Excepciones;
using Mimo.Framework.Extensions;
using System.Transactions;
using SAM.BusinessObjects.Validations;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Personalizadas;
using System.Data;
using System.Data.Objects;

namespace SAM.BusinessObjects.Workstatus
{
    public class InspeccionVisualBO
    {
        private static readonly object _mutex = new object();
        private static InspeccionVisualBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private InspeccionVisualBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase InspeccionVisualBO
        /// </summary>
        /// <returns></returns>
        public static InspeccionVisualBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new InspeccionVisualBO();
                    }
                }
                return _instance;
            }
        }

        public InspeccionVisual Obtener(int inspeccionID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.InspeccionVisual.Where(x => x.InspeccionVisualID == inspeccionID).Single();
            }
        }

        public List<GrdInspeccionVisual> ObtenerJuntas(int ordenTrabajoID, int ordenTrabajoSpoolID)
        {
            //shop fab area
            int fabAreaID = CacheCatalogos.Instance.ShopFabAreaID;

            using (SamContext ctx = new SamContext())
            {
                int proyectoID = ctx.OrdenTrabajo.SingleOrDefault(x => x.OrdenTrabajoID == ordenTrabajoID).ProyectoID;
                List<GrdInspeccionVisual> lista = new List<GrdInspeccionVisual>();                         
               
                //Obtener los ids de las JuntasWorkstatus que tienen insapeccion visual   
                IQueryable<int> wstInspeccionesVisuales = ctx.InspeccionVisualPatio.Select(x => x.JuntaWorkstatusID );
                IQueryable<int> juntaConInspeccionVisual = ctx.JuntaWorkstatus.Where(x => wstInspeccionesVisuales.Contains(x.JuntaWorkstatusID)).Select(x => x.JuntaSpoolID);

                //Obtengo los IDs de las juntas que existen en la tabla JuntaWorkstatus que son parte de la orden de trabajo
                //y del numero de control (en caso de especificarse)
                //Donde la junta es JuntaFinal y donde la inspeccion visual aún no ha sido aprobada.
                IQueryable<int> juntasIDs = ctx.JuntaWorkstatus.Where(x => x.OrdenTrabajoSpool.OrdenTrabajoID == ordenTrabajoID
                                                                      && (ordenTrabajoSpoolID == -1
                                                                      || x.OrdenTrabajoSpool.OrdenTrabajoSpoolID == ordenTrabajoSpoolID)
                                                                      && x.JuntaFinal
                                                                      && !x.InspeccionVisualAprobada)                                                                   
                                                               .Select(x => x.JuntaSpoolID);

                IQueryable<int> juntaWorkstatusIds = ctx.JuntaWorkstatus.Where(x => x.OrdenTrabajoSpool.OrdenTrabajoID == ordenTrabajoID
                                                                      && (ordenTrabajoSpoolID == -1
                                                                      || x.OrdenTrabajoSpool.OrdenTrabajoSpoolID == ordenTrabajoSpoolID)
                                                                      && x.JuntaFinal)                                                                     
                                                               .Select(x => x.JuntaSpoolID); 
                
             
                //Obtengo los registros de las juntas obtenidas en el paso anterior cuya FabArea == SHOP
                IQueryable<JuntaSpool> query = ctx.JuntaSpool.Where(x => juntasIDs.Contains(x.JuntaSpoolID)
                                                                         && x.FabAreaID == fabAreaID).AsQueryable();

                query = query.Where(x => !juntaConInspeccionVisual.Contains(x.JuntaSpoolID));    

                // Juntas sin workstatus
                IEnumerable<GrdInspeccionVisual> jsws =
                      from js in ctx.JuntaSpool.Include("Spool").Where(x => !juntasIDs.Contains(x.JuntaSpoolID) && !juntaWorkstatusIds.Contains(x.JuntaSpoolID) && x.Spool.ProyectoID == proyectoID && !juntaConInspeccionVisual.Contains(x.JuntaSpoolID))
                    join otj in ctx.OrdenTrabajoJunta on js.JuntaSpoolID equals otj.JuntaSpoolID
                    join ots in ctx.OrdenTrabajoSpool on otj.OrdenTrabajoSpoolID equals ots.OrdenTrabajoSpoolID
                    join ot in ctx.OrdenTrabajo on ots.OrdenTrabajoID equals ot.OrdenTrabajoID
                    join sh in ctx.SpoolHold on ots.SpoolID equals sh.SpoolID into Holds
                    from hld in Holds.DefaultIfEmpty()
                    where ot.OrdenTrabajoID == ordenTrabajoID &&
                          (ordenTrabajoSpoolID == -1 || ots.OrdenTrabajoSpoolID == ordenTrabajoSpoolID)
                    select new GrdInspeccionVisual
                    {
                        JuntaWorkstatusID = (js.JuntaSpoolID * -1), // si la junta no tiene jws mandamos la juntaspoolID en negativo
                        JuntaSpoolID = js.JuntaSpoolID,
                        OrdenTrabajo = ot.NumeroOrden,
                        NombreSpool = js.Spool.Nombre,
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
                        Hold = (hld == null) ? false : hld.TieneHoldIngenieria || hld.TieneHoldCalidad || hld.Confinado
                    };

                lista = (from js in query
                         join s in ctx.Spool on js.SpoolID equals s.SpoolID
                         join otj in ctx.OrdenTrabajoJunta on js.JuntaSpoolID equals otj.JuntaSpoolID
                         join ots in ctx.OrdenTrabajoSpool on otj.OrdenTrabajoSpoolID equals ots.OrdenTrabajoSpoolID
                         join ot in ctx.OrdenTrabajo on ots.OrdenTrabajoID equals ot.OrdenTrabajoID
                         join jw in ctx.JuntaWorkstatus on js.JuntaSpoolID equals jw.JuntaSpoolID
                         join sh in ctx.SpoolHold on ots.SpoolID equals sh.SpoolID into Holds
                         from hld in Holds.DefaultIfEmpty()
                         where jw.JuntaFinal
                         select new GrdInspeccionVisual
                         {
                             JuntaWorkstatusID = jw.JuntaWorkstatusID,
                             JuntaSpoolID = js.JuntaSpoolID,
                             OrdenTrabajo = ot.NumeroOrden,
                             NombreSpool = s.Nombre,
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
                             Hold = (hld == null) ? false : hld.TieneHoldIngenieria || hld.TieneHoldCalidad || hld.Confinado
                         })
                         .Union(jsws)
                         .ToList();


                return lista;
            }
        }

        public void GeneraReporte(JuntaInspeccionVisual juntaInspVisual, InspeccionVisual inspVisual, int[] defectos, string juntas, Guid UserUID)
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
                    InspeccionVisual inspVisualExistente = ctx.InspeccionVisual.Where(x => x.NumeroReporte == inspVisual.NumeroReporte && x.ProyectoID == inspVisual.ProyectoID).SingleOrDefault();

                    InspeccionVisualCampo ivc = ctx.InspeccionVisualCampo.Where(x => x.NumeroReporte == inspVisual.NumeroReporte && x.ProyectoID == inspVisual.ProyectoID).SingleOrDefault();
                    
                    if (ivc != null)
                    {
                        if (ivc.FechaReporte != inspVisual.FechaReporte)
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

                    string[] juntasArreglo = juntas.Split(',');                    

                    foreach (string juntaID in juntasArreglo)
                    {
                        int jID = juntaID.SafeIntParse();                       

                        // jID es juntaSpool y no cuenta con workstatus
                        // damos de alta la juntaWorkStatus
                        if (juntaID != "" && jID < 0)
                        {
                            JuntaSpool js = ctx.JuntaSpool.Include("Spool.OrdenTrabajoSpool").SingleOrDefault(x => x.JuntaSpoolID == Math.Abs(jID));
                            js.StartTracking();
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

                            jID = js.JuntaWorkstatus.SingleOrDefault().JuntaWorkstatusID;
                        }

                        JuntaWorkstatus juntaWks = ctx.JuntaWorkstatus.Where(x => x.JuntaWorkstatusID == jID).Single();

                        //Validamos que no existe inspeccion visual capturada en HH
                        if (juntaWks != null)
                        {
                            InspeccionVisualPatio ivp = ctx.InspeccionVisualPatio.Where(x => x.JuntaWorkstatusID == juntaWks.JuntaWorkstatusID).FirstOrDefault();
                            if (ivp != null)
                            {
                                throw new ExcepcionDuplicados(string.Format(MensajesError.Excepcion_ExisteInspeccionVisualHH));
                            }
                        }

                        //Verifico que no exista ya un detalle para este spool en especifico en el reporte
                        JuntaInspeccionVisual jivExistente = ctx.JuntaInspeccionVisual.Where(x => x.JuntaWorkstatusID == jID ).OrderByDescending(x => x.JuntaInspeccionVisualID).FirstOrDefault();
                        IQueryable<JuntaInspeccionVisualDefecto> defectosExistentes = null;
                      
                        JuntaInspeccionVisual junta = new JuntaInspeccionVisual();
                   
                        if (jivExistente != null )
                        {                            
                            if(jivExistente.Aprobado)
                            { 
                                if (jivExistente.InspeccionVisualID == inspVisual.InspeccionVisualID)
                                {
                                    throw new ExcepcionDuplicados(string.Format(MensajesError.Excepcion_ReporteVisualConDetalleDuplicado));
                                }
                                else
                                {
                                    string numeroReporte = ctx.InspeccionVisual.Where(x => x.InspeccionVisualID == jivExistente.InspeccionVisualID).Select(x => x.NumeroReporte).FirstOrDefault();
                                    throw new ExcepcionDuplicados(string.Format(MensajesError.Excepcion_ReporteVisualConDetalleDuplicadoOtroReporte, numeroReporte ));
                                }
                            }
                            else
                            {
                                junta = jivExistente;
                                defectosExistentes = ctx.JuntaInspeccionVisualDefecto.Where(x => x.JuntaInspeccionVisualID == jivExistente.JuntaInspeccionVisualID);                       
                            }
                        }                       
                        
                        junta.JuntaWorkstatusID = jID;
                        junta.FechaInspeccion = juntaInspVisual.FechaInspeccion;
                        junta.Aprobado = juntaInspVisual.Aprobado;
                        junta.Observaciones = juntaInspVisual.Observaciones;
                        junta.UsuarioModifica = UserUID;
                        junta.FechaModificacion = DateTime.Now;
                        junta.TallerID = juntaInspVisual.TallerID;
                        junta.InspectorID = juntaInspVisual.InspectorID;

                        //Si el reporte es aprobado se actualiza el registro de JuntaWorkstatus
                        if (juntaInspVisual.Aprobado)
                        {
                             //Validamos que no tenga aprobado un reporte de inspección visual para que no se apruebe por segunda vez
                            if (juntaWks.InspeccionVisualAprobada)
                            {
                                throw new ExcepcionReportes(string.Format(MensajesError.Excepcion_IVAprobada, juntaWks.EtiquetaJunta));
                            }
                            juntaWks.StartTracking();
                            juntaWks.InspeccionVisualAprobada = true;
                            juntaWks.InspeccionVisual = junta;
                            juntaWks.UltimoProcesoID = (int)UltimoProcesoEnum.InspeccionVisual;
                            juntaWks.UsuarioModifica = UserUID;
                            juntaWks.FechaModificacion = DateTime.Now;
                            juntaWks.StopTracking();
                            ctx.JuntaWorkstatus.ApplyChanges(juntaWks);
                        }
                        else //Si el reporte es rechazado se generan los registros de defectos
                        {
                            foreach (int defectoID in defectos)
                            { 
                                JuntaInspeccionVisualDefecto juntaDefecto = null;
                                if (defectosExistentes != null)
                                {
                                    juntaDefecto = defectosExistentes.Where(x => x.DefectoID == defectoID).OrderByDescending(X => X.JuntaInspeccionVisualDefectoID).FirstOrDefault();
                                }
                                 
                                if(juntaDefecto == null)                                                   
                                {
                                    juntaDefecto  = new JuntaInspeccionVisualDefecto();
                                }
                                
                                juntaDefecto.DefectoID = defectoID;
                                juntaDefecto.UsuarioModifica = UserUID;
                                juntaDefecto.FechaModificacion = DateTime.Now;                                
                              
                                if (junta.JuntaInspeccionVisualDefecto.Contains(juntaDefecto))
                                {
                                    ctx.JuntaInspeccionVisualDefecto.ApplyChanges(juntaDefecto);
                                }
                                else
                                {
                                    junta.JuntaInspeccionVisualDefecto.Add(juntaDefecto);
                                }
                            }
                            if(defectosExistentes != null)
                            {
                                foreach (JuntaInspeccionVisualDefecto jd in defectosExistentes)
                                {
                                    if (!defectos.Contains(jd.DefectoID))
                                    {
                                        ctx.DeleteObject(jd);
                                    }
                                }
                            }
                        }

                        if(inspVisual.JuntaInspeccionVisual.Contains(juntaInspVisual))
                        {
                            ctx.JuntaInspeccionVisual.ApplyChanges(juntaInspVisual);                           
                        }
                        else
                        {
                            inspVisual.JuntaInspeccionVisual.Add(junta);
                        }
                        
                    }

                    if (inspVisualExistente != null)
                    {
                        inspVisual.StopTracking();
                    }

                    ctx.InspeccionVisual.ApplyChanges(inspVisual);
                    ctx.SaveChanges();

                    ctx.CalculaHojaParaReporte((int)TipoReporte.InspeccionVisual, inspVisual.ProyectoID, inspVisual.NumeroReporte, juntas);

                }

                ts.Complete();
            }
        }

        public InspeccionVisual DetalleInspeccionVisual(int inspeccionID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.InspeccionVisual.Include("JuntaInspeccionVisual")
                                           .Include("Proyecto")
                                           .Where(x => x.InspeccionVisualID == inspeccionID).Single();
            }
        }

        public List<GrdDetInspeccionVisual> ObtenerDetalleInspeccionVisual(int inspeccionID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<InspeccionVisual> iqInspeccion = ctx.InspeccionVisual.Where(x => x.InspeccionVisualID == inspeccionID);
                IQueryable<JuntaInspeccionVisual> iqJuntaInspeccionVisual = ctx.JuntaInspeccionVisual.Where(x => iqInspeccion.Select(y => y.InspeccionVisualID).Contains(x.InspeccionVisualID));
                IQueryable<JuntaWorkstatus> iqJuntaWorkstatus = ctx.JuntaWorkstatus.Where(x => iqJuntaInspeccionVisual.Select(y => y.JuntaWorkstatusID).Contains(x.JuntaWorkstatusID));
                IQueryable<OrdenTrabajoSpool> iqOrdenTrabajoSpool = ctx.OrdenTrabajoSpool.Where(x => iqJuntaWorkstatus.Select(y => y.OrdenTrabajoSpoolID).Contains(x.OrdenTrabajoSpoolID));
                IQueryable<OrdenTrabajo> iqOrdenTrabajo = ctx.OrdenTrabajo.Where(x => iqOrdenTrabajoSpool.Select(y => y.OrdenTrabajoID).Contains(x.OrdenTrabajoID));
                IQueryable<Spool> iqSpool = ctx.Spool.Where(x => iqOrdenTrabajoSpool.Select(y => y.SpoolID).Contains(x.SpoolID));
                IQueryable<JuntaSpool> iqJuntaSpool = ctx.JuntaSpool.Where(x => iqJuntaWorkstatus.Select(y => y.JuntaSpoolID).Contains(x.JuntaSpoolID));
                IQueryable<TipoJunta> iqTipoJunta = ctx.TipoJunta.Where(x => iqJuntaSpool.Select(y => y.TipoJuntaID).Contains(x.TipoJuntaID));

                List<GrdDetInspeccionVisual> lst = (from iv in iqInspeccion
                                                    join jiv in iqJuntaInspeccionVisual on iv.InspeccionVisualID equals jiv.InspeccionVisualID
                                                    join jw in iqJuntaWorkstatus on jiv.JuntaWorkstatusID equals jw.JuntaWorkstatusID
                                                    join odts in iqOrdenTrabajoSpool on jw.OrdenTrabajoSpoolID equals odts.OrdenTrabajoSpoolID
                                                    join odt in iqOrdenTrabajo on odts.OrdenTrabajoID equals odt.OrdenTrabajoID
                                                    join s in iqSpool on odts.SpoolID equals s.SpoolID
                                                    join js in iqJuntaSpool on jw.JuntaSpoolID equals js.JuntaSpoolID
                                                    join tj in iqTipoJunta on js.TipoJuntaID equals tj.TipoJuntaID
                                                    select new GrdDetInspeccionVisual
                                                    {
                                                        JuntaInspeccionVisualID = jiv.JuntaInspeccionVisualID,
                                                        OrdenTrabajo = odt.NumeroOrden,
                                                        NumeroControl = odts.NumeroControl,
                                                        Spool = s.Nombre,
                                                        Junta = jw.EtiquetaJunta,
                                                        Localizacion = js.EtiquetaMaterial1 + " - " + js.EtiquetaMaterial2,
                                                        Tipo = tj.Codigo,
                                                        Cedula = js.Cedula,
                                                        Material1 = ctx.FamiliaAcero.Where(x => x.FamiliaAceroID == s.FamiliaAcero1ID).FirstOrDefault().Nombre,
                                                        Material2 = ctx.FamiliaAcero.Where(x => x.FamiliaAceroID == s.FamiliaAcero2ID).FirstOrDefault().Nombre == null ? string.Empty : ctx.FamiliaAcero.Where(x => x.FamiliaAceroID == s.FamiliaAcero2ID).FirstOrDefault().Nombre,
                                                        Diametro = js.Diametro,
                                                        Hoja = jiv.Hoja,
                                                        Aprobado = jiv.Aprobado,
                                                        FechaInspeccion = jiv.FechaInspeccion,
                                                        Observaciones = jiv.Observaciones
                                                    }).ToList();
                return lst;
            }
        }

        public void Borra(int inspeccionVisualID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (ValidacionesInspeccionVisual.TieneJuntaInspeccionVisual(ctx, inspeccionVisualID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_TieneRelacionJuntaInspeccionVisual);
                }

                InspeccionVisual iv = ctx.InspeccionVisual.Where(x => x.InspeccionVisualID == inspeccionVisualID).SingleOrDefault();
                ctx.DeleteObject(iv);
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Revisa si existe la inspeccion visual para la junta
        /// </summary>
        /// <param name="juntaSpoolID">Junta Spool ID</param>
        /// <returns></returns>
        public bool ExisteJuntaInspeccionVisual(int juntaSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (ctx.JuntaWorkstatus.Where(x => x.JuntaSpoolID == juntaSpoolID && x.InspeccionVisualAprobada == true && x.JuntaFinal).SingleOrDefault() == null)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// guarda una entidad JuntaWorkstatus, la cuál se crea cuando se inspecciona visualmente una junta o bien actualiza la que ya existe
        /// </summary>
        /// <param name="jws">JuntaWorkstatus</param>
        public void GuardaJuntaWorkstatus(JuntaWorkstatus jws, InspeccionVisualPatio jiv)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SamContext ctx = new SamContext())
                    {
                        IQueryable<InspeccionVisualPatioDefecto> defectosExistentes = ctx.InspeccionVisualPatioDefecto.Where(x => x.InspeccionVisualPatioID == jiv.InspeccionVisualPatioID).AsQueryable();

                        ValidacionesFechas.VerificaFechaInspeccionVisualVSSoldadura(ctx, jiv.FechaInspeccion, jws);
                        //Guardar Junta Workstatus
                        jws.StartTracking();
                        //jws.InspeccionVisualPatio.Add(jiv);                             
                        ctx.JuntaWorkstatus.ApplyChanges(jws);
                        ctx.SaveChanges();

                        //Guardar Inspeccion Visual
                        jiv.JuntaWorkstatusID = jws.JuntaWorkstatusID;
                        ctx.InspeccionVisualPatio.ApplyChanges(jiv);                      
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

        public void GuardarEdicionInspeccionVisual(InspeccionVisual IVisual)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SamContext ctx = new SamContext())
                    {
                        IVisual.StartTracking();
                        ctx.InspeccionVisual.ApplyChanges(IVisual);
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

        public bool ValidarNumeroReporteEdicionEspecial(string numeroReporte, int proyectoID, int inspecVisualID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.InspeccionVisual.Where(x => x.ProyectoID == proyectoID
                    && x.NumeroReporte == numeroReporte
                    && x.InspeccionVisualID != inspecVisualID).Any();
            }
        }

        /// <summary>
        /// Obtiene la lista de juntas para su inspeccion visual
        /// </summary>
        /// <param name="spoolID"></param>
        /// <param name="FabAreaID"></param>
        /// <returns></returns>
        public List<JuntaWorkstatus> ObtenerJuntasWorkstatusParaInspeccionVisualHH(int spoolID, int FabAreaID, int tipoJuntaTHID, int tipoJuntaTWID)
        {
            using (SamContext ctx = new SamContext())
            {
                List<JuntaSpool> juntasSpool = ctx.JuntaSpool.Where(x => x.SpoolID == spoolID && x.FabAreaID == FabAreaID).ToList();

                List<int> listJuntaSpoolID = juntasSpool.Select(y => y.JuntaSpoolID).ToList();

                List<JuntaWorkstatus> listJuntaWorkstatus = ctx.JuntaWorkstatus.Where(x => listJuntaSpoolID.Contains(x.JuntaSpoolID) && x.JuntaFinal  && x.ArmadoAprobado).ToList();

                IQueryable<int> eqtp = (new List<int>() { tipoJuntaTHID, tipoJuntaTWID }).AsQueryable();

                List<JuntaWorkstatus> listaJuntaWorkstatusABorrar = listJuntaWorkstatus.Where(x => !x.SoldaduraAprobada && !eqtp.Contains(x.JuntaSpool.TipoJuntaID)).ToList();

                return listJuntaWorkstatus.Except(listaJuntaWorkstatusABorrar).ToList();
            }
        }

        /// <summary>
        /// Obtiene la lista de juntas para su inspeccion visual
        /// </summary>
        /// <param name="spoolID"></param>
        /// <param name="FabAreaID"></param>
        /// <returns></returns>
        public List<JuntaSpool> ObtenerJuntasSpoolParaInspeccionVisualHH(int spoolID, int FabAreaID, int tipoJuntaTHID, int tipoJuntaTWID)
        {
            using (SamContext ctx = new SamContext())
            {
                List<JuntaSpool> juntasSpool = ctx.JuntaSpool.Where(x => x.SpoolID == spoolID && x.FabAreaID == FabAreaID).ToList();

                List<int> listJuntaSpoolID = juntasSpool.Select(y => y.JuntaSpoolID).ToList();


                List<JuntaWorkstatus> listJuntaWorkstatus = ctx.JuntaWorkstatus.Where(x => listJuntaSpoolID.Contains(x.JuntaSpoolID) && x.JuntaFinal && !x.InspeccionVisualAprobada).ToList();

                IQueryable<int> eqtp = (new List<int>() { tipoJuntaTHID, tipoJuntaTWID }).AsQueryable();



                List<int> juntasIds = listJuntaWorkstatus.Select(x => x.JuntaSpoolID).ToList();
                juntasIds.AddRange(
                        listJuntaSpoolID.Where(x => !ctx.JuntaWorkstatus.Select(j => j.JuntaSpoolID).Contains(x)).Select(x => x)
                    );


                return ctx.JuntaSpool.Include("JuntaWorkstatus").Where(x => juntasIds.Contains(x.JuntaSpoolID)).ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspeccionVisualID"></param>
        /// <returns></returns>
        public int ObtenerProyectoID(int inspeccionVisualID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ProyectoInspeccionVisual(ctx, inspeccionVisualID);
            }
        }
              

        /// <summary>
        /// Versión compilada del query para permisos de inspección visual
        /// </summary>
        private static readonly Func<SamContext, int, int> ProyectoInspeccionVisual =
        CompiledQuery.Compile<SamContext, int, int>
        (
            (ctx, id) => ctx.InspeccionVisual
                            .Where(x => x.InspeccionVisualID == id)
                            .Select(x => x.ProyectoID)
                            .Single()
        );

        /// <summary>
        /// Revisa si existe la inspeccion visual Patio para la junta
        /// </summary>
        /// <param name="juntaSpoolID">Junta WorkStatus ID</param>
        /// <returns></returns>
        public InspeccionVisualPatio ExisteJuntaInspeccionVisualPatio(int juntaWorkSatusId)
        {
            using (SamContext ctx = new SamContext())
            {
                InspeccionVisualPatio ivp = ctx.InspeccionVisualPatio.Where(x => x.JuntaWorkstatusID == juntaWorkSatusId).FirstOrDefault();
                
                return ivp;                
            }
        }
        
             /// <summary>
        /// Revisa si existe la inspeccion visual Patio para la junta
        /// </summary>
        /// <param name="juntaSpoolID">Junta WorkStatus ID</param>
        /// <returns></returns>
        public List<InspeccionVisualPatioDefecto> ExisteInspeccionVisualPorDefecto(int idInspeccionVisual)
        {
            using (SamContext ctx = new SamContext())
            {
                List<InspeccionVisualPatioDefecto> ivpd = ctx.InspeccionVisualPatioDefecto.Where(x => x.InspeccionVisualPatioID == idInspeccionVisual).ToList();

                return ivpd;                
            }
        }

    }
}
