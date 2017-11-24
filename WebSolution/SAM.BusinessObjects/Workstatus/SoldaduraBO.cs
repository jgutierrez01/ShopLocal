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
using Mimo.Framework.Common;


namespace SAM.BusinessObjects.Workstatus
{
    public class SoldaduraBO
    {
        private static readonly object _mutex = new object();
        private static SoldaduraBO _instance;

        /// <summary>
        /// constructor privado para implementar el patrón del singleton
        /// </summary>
        private SoldaduraBO()
        {
        }

        /// <summary>
        /// crea una instancia de la clase SoldaduraBO.
        /// </summary>
        public static SoldaduraBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new SoldaduraBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// obtiene la informacion que se desplegará en el grid de soldadura.
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="ordenTrabajoID"></param>
        /// <param name="ordenTrabajoSpoolID"></param>
        /// <returns></returns>
        public List<GrdSoldadura> ObtenerListaSoldadura(int proyectoID, int? ordenTrabajoID, int? ordenTrabajoSpoolID)
        {
            //Los filtros no pueden venir completamente vacíos o nos traeríamos todas las
            //ODTs de la BD
            if (ordenTrabajoID == null || proyectoID < 0)
            {
                throw new ArgumentException("El proyecto o la orden de trabajo son requeridos");
            }

            //listas a usarse
            List<GrdSoldadura> soldadura = null;
            List<JuntaSpool> juntas = null;
            List<SpoolHold> spoolHold = null;
            List<MaterialSpool> materiales = null;
            List<OrdenTrabajoMaterial> ordenTrabajoMaterial = null;
            List<OrdenTrabajoJunta> ordenTrabajoJunta = null;
            List<JuntaWorkstatus> juntaWks = null;
            List<TipoJuntaCache> tipoJunta = CacheCatalogos.Instance.ObtenerTiposJunta();
            List<Spool> spools = null;

            using (SamContext ctx = new SamContext())
            {

                int tipoJuntaID = tipoJunta.Where(x => x.Nombre == TipoJuntas.TW).Select(x => x.ID).Single();
                int tipoJuntaID2 = tipoJunta.Where(x => x.Nombre == TipoJuntas.TH).Select(x => x.ID).Single();
                int fabAreaID = CacheCatalogos.Instance.ObtenerFabAreas().Where(x => x.Nombre == FabAreas.SHOP).Select(x => x.ID).Single();

                IQueryable<OrdenTrabajoSpool> query = ctx.OrdenTrabajoSpool.AsQueryable();
                ctx.OrdenTrabajo.Where(x => x.ProyectoID == proyectoID).ToList();

                #region aplicar filtros al query

                if (ordenTrabajoSpoolID.HasValue && ordenTrabajoSpoolID.Value > 0)
                {
                    query = query.Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID.Value);
                }
                else if (ordenTrabajoID.HasValue && ordenTrabajoID.Value > 0)
                {
                    query = query.Where(x => x.OrdenTrabajoID == ordenTrabajoID.Value);
                }
                else
                {
                    query = query.Where(x => x.OrdenTrabajo.ProyectoID == proyectoID);
                }

                #endregion

                //obtiene los ID's de spool y de ordentrabajoSpool
                IQueryable<int> spoolIDs = query.Select(x => x.SpoolID);
                IQueryable<int> otsIDs = query.Select(x => x.OrdenTrabajoSpoolID);

                spools = ctx.Spool.Where(x => spoolIDs.Contains(x.SpoolID)).ToList();

                query.ToList();

                juntas = ctx.JuntaSpool.Where(x => spoolIDs.Contains(x.SpoolID) && x.TipoJuntaID != tipoJuntaID && x.TipoJuntaID != tipoJuntaID2 && x.FabAreaID == fabAreaID).ToList();

                IEnumerable<int> ieJuntaSpoolIDs = juntas.Select(x => x.JuntaSpoolID);
                juntaWks = ctx.JuntaWorkstatus.Where(x => ieJuntaSpoolIDs.Contains(x.JuntaSpoolID) && x.JuntaFinal).ToList();
                spoolHold = ctx.SpoolHold.Where(x => spoolIDs.Contains(x.SpoolID)).ToList();
                ordenTrabajoJunta = ctx.OrdenTrabajoJunta.Where(x => ieJuntaSpoolIDs.Contains(x.JuntaSpoolID)).ToList();

                //Obtengo los materiales y sus ordenes de trabajo para poder verificar si estan despachados o no.
                IQueryable<MaterialSpool> mat = ctx.MaterialSpool.Where(x => spoolIDs.Contains(x.SpoolID)).AsQueryable();
                ordenTrabajoMaterial = ctx.OrdenTrabajoMaterial.Where(x => mat.Select(y => y.MaterialSpoolID).Contains(x.MaterialSpoolID)).ToList();
                materiales = mat.ToList();
            }

            List<FamAceroCache> fa1 = CacheCatalogos.Instance.ObtenerFamiliasAcero();

            soldadura =
                (from jta in juntas
                 let fa2 = fa1.SingleOrDefault(x => x.ID == jta.FamiliaAceroMaterial2ID.GetValueOrDefault(0))
                 let jws = jta.JuntaWorkstatus.SingleOrDefault()
                 let hld = spoolHold.Where(x => x.SpoolID == jta.SpoolID).SingleOrDefault()
                 let otj = ordenTrabajoJunta.Where(x => x.JuntaSpoolID == jta.JuntaSpoolID).SingleOrDefault()
                 let spool = spools.Where(x => x.SpoolID == jta.SpoolID).SingleOrDefault()
                 select new GrdSoldadura
                 {
                     SpoolID = spool == null ? -1 : spool.SpoolID,
                     NombreSpool = spool == null ? string.Empty : spool.Nombre,
                     NumeroControl = otj == null ? string.Empty : otj.OrdenTrabajoSpool.NumeroControl,
                     Junta = jws == null ? jta.Etiqueta : jws.EtiquetaJunta,
                     Localizacion = jta.EtiquetaMaterial1 + "-" + jta.EtiquetaMaterial2,
                     EtiquetaMaterial1 = jta.EtiquetaMaterial1,
                     EtiquetaMaterial2 = jta.EtiquetaMaterial2,
                     TipoJunta = tipoJunta.Single(x => x.ID == jta.TipoJuntaID).Text,
                     Cedula = jta.Cedula,
                     FamiliaAceroMaterial1 = fa1.Single(x => x.ID == jta.FamiliaAceroMaterial1ID).Text,
                     FamiliaAceroMaterial2 = fa2 != null ? fa2.Text : string.Empty,
                     Diametro = jta.Diametro,
                     ArmadoAprobado = jws == null ? false : jws.ArmadoAprobado,
                     SoldaduraAprobada = jws == null ? false : jws.SoldaduraAprobada,
                     JuntaSpoolID = jta.JuntaSpoolID,
                     JuntaWorkStatusID = jws == null ? -1 : jws.JuntaWorkstatusID,
                     EstatusID = jws != null ? jws.SoldaduraAprobada ? (int)EstatusSoldadura.Soldado :
                                                    jws.ArmadoAprobado ? (int)EstatusSoldadura.Armado :
                                                     otj == null ? (int)EstatusSoldadura.SinODT : -1 : -1,
                     Hold = hld == null ? false : hld.TieneHoldCalidad || hld.TieneHoldIngenieria || hld.Confinado,
                     RequierePWHT = spool.RequierePwht,
                     JuntaSoldaduraID = jws == null ? -1 : jws.JuntaSoldaduraID == null ? -1 : jws.JuntaSoldaduraID.Value
                 }).ToList();

            ILookup<int, Simple> materialPorSpool = materiales.ToLookup(x => x.SpoolID, y => new Simple { ID = y.MaterialSpoolID, Valor = y.Etiqueta });
            Dictionary<int, bool> odmDic = ordenTrabajoMaterial.ToDictionary(x => x.MaterialSpoolID, y => y.TieneDespacho);

            soldadura.ToList().ForEach(x =>
            {
                if (x.EstatusID == -1)
                {
                    IEnumerable<Simple> material = materialPorSpool[x.SpoolID];

                    int materialSpool1ID = EtiquetasMaterialUtil.ObtenMaterialSpoolDeEtiqueta(material, x.EtiquetaMaterial1);
                    int materialSpool2ID = EtiquetasMaterialUtil.ObtenMaterialSpoolDeEtiqueta(material, x.EtiquetaMaterial2);

                    if (materialSpool1ID > 0 && materialSpool2ID > 0)
                    {

                        if (odmDic.ContainsKey(materialSpool1ID) && odmDic.ContainsKey(materialSpool2ID) && odmDic[materialSpool1ID] && odmDic[materialSpool2ID])
                        {
                            x.EstatusID = (int)EstatusSoldadura.SinArmado;
                        }
                        else
                        {
                            x.EstatusID = (int)EstatusSoldadura.SinDespacho;
                        }
                    }
                    else if (materialSpool1ID > 0)
                    {
                        if (odmDic[materialSpool1ID])
                        {
                            x.EstatusID = (int)EstatusSoldadura.SinArmado;
                        }
                        else
                        {
                            x.EstatusID = (int)EstatusSoldadura.SinDespacho;
                        }
                    }
                    else if (materialSpool2ID > 0)
                    {
                        if (odmDic[materialSpool2ID])
                        {
                            x.EstatusID = (int)EstatusSoldadura.SinArmado;
                        }
                        else
                        {
                            x.EstatusID = (int)EstatusSoldadura.SinDespacho;
                        }
                    }
                    else
                    {
                        x.EstatusID = (int)EstatusSoldadura.SinDespacho;
                    }
                }

                x.Estatus = TraductorEnumeraciones.TextoEstatusSoldadura(x.EstatusID);

            });

            return soldadura;
        }

        /// <summary>
        /// Elimina el registro de juntaSoldadura y pone en falso la aprobacion de la soldadura en JuntaWorkstatus
        /// </summary>
        /// <param name="juntaArmadoID">ID de la soldadura a eliminar</param>
        /// <param name="userID">ID del usuario logeado</param>
        public void BorraSoldadura(int juntaSoldaduraID, Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntaWorkstatus jWks = ctx.JuntaWorkstatus.Where(x => x.JuntaSoldaduraID == juntaSoldaduraID).FirstOrDefault();
                //Validamos que no tenga inspección visual aprobada o no aprobada
              
                if (jWks.InspeccionVisualAprobada )
                {
                    throw new ExcepcionSoldadura(MensajesError.Excepcion_InspeccionVisualAprobada);
                }
                IQueryable<JuntaInspeccionVisual>  jiv = ctx.JuntaInspeccionVisual.Where(x => x.JuntaWorkstatusID == jWks.JuntaWorkstatusID);
                if (jiv.Count() > 0)
                {
                    throw new ExcepcionSoldadura(MensajesError.Excepcion_InspeccionVisual);
                }

                WorkstatusSpool wks = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID == jWks.OrdenTrabajoSpoolID).FirstOrDefault();
                if (wks != null)
                {
                    if (wks.TieneLiberacionDimensional)
                    {
                        throw new ExcepcionSoldadura(MensajesError.Excepcion_TieneLiberacionDimensional);
                    }
                    
                    if (wks.TieneRequisicionPintura || wks.TienePintura)
                    {
                        throw new ExcepcionRelaciones(MensajesError.Excepcion_TieneRequiPintura);
                    }
                }

               
              
                JuntaSoldadura jSoldadura = ctx.JuntaSoldadura.Where(x => x.JuntaSoldaduraID == juntaSoldaduraID).FirstOrDefault();
                List<JuntaSoldaduraDetalle> detalles = ctx.JuntaSoldaduraDetalle.Where(x => x.JuntaSoldaduraID == juntaSoldaduraID).ToList();

                jWks.StartTracking();
                jWks.SoldaduraAprobada = false;
                jWks.JuntaSoldaduraID = null;
                jWks.UsuarioModifica = userID;
                jWks.FechaModificacion = DateTime.Now;
                jWks.StopTracking();

                ctx.JuntaWorkstatus.ApplyChanges(jWks);

                detalles.ForEach(ctx.DeleteObject);
                ctx.JuntaSoldadura.DeleteObject(jSoldadura);

                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// obtiene la informacion necesaria a desplegarse en los popup de armado.
        /// </summary>
        /// <param name="juntaSpoolID"></param>
        /// <returns></returns>
        public OrdenTrabajoJunta ObtenerInformacionParaSoldadura(int juntaSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                OrdenTrabajoJunta infoArmado = ctx.OrdenTrabajoJunta
                                            .Include("OrdenTrabajoSpool")
                                            .Include("JuntaSpool")
                                            .Include("JuntaSpool.Spool")
                                            .Where(x => x.JuntaSpoolID == juntaSpoolID)
                                            .Single();
                return infoArmado;
            }
        }

        /// <summary>
        /// Si existe un registro de JuntaWorkstatus en base a JuntaSpoolID y JuntaFinal True, lo regresa. Si no, regresa nulo.
        /// </summary>
        /// <param name="juntaSpoolID">JuntaSpoolID</param>
        /// <returns></returns>
        public JuntaWorkstatus ObtenerJuntaWorkstatusPorJuntaSpoolID(int juntaSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.JuntaWorkstatus.Include("JuntaSpool").Where(x => x.JuntaSpoolID == juntaSpoolID && x.JuntaFinal == true).SingleOrDefault();
            }
        }

        /// <summary>
        /// obtiene la informacion necesaria para rellenar el popup del soldadura.
        /// </summary>
        /// <param name="juntaWorkstatusID"></param>
        /// <returns></returns>
        public JuntaSoldadura ObtenerInformacionSoldadura(int juntaWorkstatusID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.JuntaSoldadura.Include("Wps")
                                         .Include("Wps1")
                                         .Include("JuntaSoldaduraDetalle")
                                         .Include("JuntaSoldaduraDetalle.Soldador")
                                         .Include("JuntaSoldaduraDetalle.Consumible").Where(x => x.JuntaWorkstatusID == juntaWorkstatusID).SingleOrDefault();

            }
        }


        public void ValidaFechasSoldaduraFechaRequiPintura(DateTime fechaProcSoldadura, int otsId)
        {

            using (SamContext ctx = new SamContext())
            {
                string procesos = string.Empty;

                PinturaSpool ps = (from p in ctx.PinturaSpool
                                   join wks in ctx.WorkstatusSpool on p.WorkstatusSpoolID equals wks.WorkstatusSpoolID into workStatus
                                   from t1 in workStatus.DefaultIfEmpty()
                                   where t1.OrdenTrabajoSpoolID == otsId
                                   select p).FirstOrDefault();
                if (ps != null)
                {
                    if (ps.FechaSandblast < fechaProcSoldadura)
                    {
                        procesos += "Sandblast: " + ps.FechaSandblast.Value.ToShortDateString() + ", ";
                    }

                    if (ps.FechaPrimarios < fechaProcSoldadura)
                    {
                        procesos += "Primarios: " + ps.FechaPrimarios.Value.ToShortDateString() + ", ";
                    }

                    if (ps.FechaIntermedios < fechaProcSoldadura)
                    {
                        procesos += "Intermedios: " + ps.FechaIntermedios.Value.ToShortDateString() + ", ";
                    }

                    if (ps.FechaAdherencia < fechaProcSoldadura)
                    {
                        procesos += "Adherencia: " + ps.FechaAdherencia.Value.ToShortDateString() + ", ";
                    }

                    if (ps.FechaAcabadoVisual < fechaProcSoldadura)
                    {
                        procesos += "AcabadoVisual: " + ps.FechaAcabadoVisual.Value.ToShortDateString() + ", ";
                    }

                    if (ps.FechaPullOff < fechaProcSoldadura)
                    {
                        procesos += "PullOff: " + ps.FechaPullOff.Value.ToShortDateString() + ", ";
                    }                 

                    if (!string.IsNullOrEmpty(procesos))
                    {
                        throw new Excepciones.ExcepcionSoldadura(string.Format(MensajesError.Excepcion_FechaMayorReportePintura, procesos));
                    }                
                }
            }
        }

        /// <summary>
        /// guarda una entidad JuntaWorkstatus, la cuál se crea cuando se solda una junta o bien actualiza la que ya existe
        /// </summary>
        /// <param name="jws">JuntaWorkstatus</param>
        public void GuardaJuntaWorkstatus(JuntaWorkstatus jws, JuntaSoldadura js, List<int> SoldaduraDetalleID)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SamContext ctx = new SamContext())
                    {                       
                        if (js.FechaSoldadura > js.FechaReporte)
                        {
                            throw new ExcepcionSoldadura(MensajesError.Excepcion_FechaReporteMayorFechaProceso);
                        }
                        else
                        {
                            //en HH aun no se tiene la opcion de tener dos wps por eso este if.
                            if (js.WpsRellenoID.HasValue)
                            {
                                ValidacionesWps.VerificaPWHT(ctx, jws.JuntaSpoolID, js.WpsID.Value, js.WpsRellenoID.Value);
                            }
                            else if (js.WpsID.HasValue)
                            {
                                ValidacionesWps.VerificaPWHT(ctx, jws.JuntaSpoolID, js.WpsID.Value, js.WpsID.Value);
                            }

                            if (jws.JuntaWorkstatusAnteriorID != null)
                            {
                                JuntaWorkstatus jwIDAnterior = ctx.JuntaWorkstatus.Where(x => x.JuntaSpoolID == jws.JuntaSpoolID && x.JuntaWorkstatusID == jws.JuntaWorkstatusAnteriorID).FirstOrDefault();
                                if (jwIDAnterior != null)
                                {
                                    JuntaSoldadura jsAnterior = ctx.JuntaSoldadura.Where(x => x.JuntaWorkstatusID == jwIDAnterior.JuntaWorkstatusID).SingleOrDefault();
                                    if (jsAnterior != null)
                                    {

                                        if (jsAnterior.FechaSoldadura > js.FechaSoldadura)
                                        {
                                            string error = string.Format(MensajesError.Excepcion_FechaSoldaduraMenorSoldaduraAnterior, jsAnterior.FechaSoldadura.ToShortDateString());

                                            throw new ExcepcionSoldadura(error);
                                        }
                                    }
                                }
                            } 

                            //Para que la soldadura sea aprobada debe de tener registrados al menos un soldador de cada tipo y ambos procesos capturados
                            if ((js.JuntaSoldaduraDetalle.Any(x => x.TecnicaSoldadorID == 1) && js.JuntaSoldaduraDetalle.Any(x => x.TecnicaSoldadorID == 2)) && (js.ProcesoRaizID != null && js.ProcesoRellenoID != null))
                            {
                                jws.SoldaduraAprobada = true;
                            }

                            if (SoldaduraDetalleID.Any())
                            {
                                List<JuntaSoldaduraDetalle> lstjsd = ctx.JuntaSoldaduraDetalle.Where(x => SoldaduraDetalleID.Contains(x.JuntaSoldaduraDetalleID)).ToList();
                                lstjsd.ForEach(x => ctx.DeleteObject(x));
                            }

                            //Guardar Junta Workstatus
                            jws.StartTracking();
                            ctx.JuntaWorkstatus.ApplyChanges(jws);
                            ctx.SaveChanges();

                            //Guardar Junta Soldadura
                            js.StartTracking();
                            js.JuntaWorkstatusID = jws.JuntaWorkstatusID;
                            ctx.JuntaSoldadura.ApplyChanges(js);
                            ctx.SaveChanges();

                            //Re guardar Junta Workstatus añadiendo Junta Armado
                            jws.StartTracking();
                            jws.JuntaSoldaduraID = js.JuntaSoldaduraID;
                            ctx.JuntaWorkstatus.ApplyChanges(jws);

                            ctx.SaveChanges();
                        }
                        scope.Complete(); 
                    }
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        public void GuardarEdicionEspecialJuntaSoldadura(JuntaSoldadura juntaSoldadura, List<int> EliminarDetalleID, int juntaSpoolId)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SamContext ctx = new SamContext())
                    {
                        if (juntaSoldadura.FechaSoldadura > juntaSoldadura.FechaReporte)
                        {
                            throw new ExcepcionSoldadura(MensajesError.Excepcion_FechaReporteMayorFechaProceso);
                        }
                        else
                        {
                            //en HH aun no se tiene la opcion de tener dos wps por eso este if.
                            if (juntaSoldadura.WpsRellenoID.HasValue)
                            {
                                ValidacionesWps.VerificaPWHT(ctx, juntaSpoolId, juntaSoldadura.WpsID.Value, juntaSoldadura.WpsRellenoID.Value);
                            }
                            else if (juntaSoldadura.WpsID.HasValue)
                            {
                                ValidacionesWps.VerificaPWHT(ctx, juntaSpoolId, juntaSoldadura.WpsID.Value, juntaSoldadura.WpsID.Value);
                            }

                            //Guardar Junta Soldadura
                            juntaSoldadura.StartTracking();
                            ctx.JuntaSoldadura.ApplyChanges(juntaSoldadura);
                            ctx.SaveChanges();

                            if (EliminarDetalleID.Any())
                            {
                                List<JuntaSoldaduraDetalle> lstjsd = ctx.JuntaSoldaduraDetalle.Where(x => EliminarDetalleID.Contains(x.JuntaSoldaduraDetalleID)).ToList();
                                lstjsd.ForEach(x => ctx.DeleteObject(x));
                                ctx.SaveChanges();
                            }
                        }
                        //
                        scope.Complete();
                    }
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// Revisa si existe la soldadura para la junta
        /// </summary>
        /// <param name="juntaSpoolID">Junta Spool ID</param>
        /// <returns></returns>
        public bool ExisteJuntaSoldadura(int juntaSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (ctx.JuntaWorkstatus.Where(x => x.JuntaSpoolID == juntaSpoolID && x.SoldaduraAprobada == true && x.JuntaFinal).SingleOrDefault() == null)
                {
                    return false;
                }
                return true;
            }
        }

    
        /// <summary>
        /// Obtiene el WPS que cumple los criterios de Materiales Base, Proceso Raiz y ProcesoRelleno
        /// </summary>
        /// <param name="materialBase1ID"></param>
        /// <param name="materialBase2ID"></param>
        /// <param name="pocesoRaiz"></param>
        /// <param name="procesoRelleno"></param>
        /// <returns></returns>
        public List<Simple> ObtenerWPS(int materialBase1ID, int materialBase2ID, int pocesoRaiz, int procesoRelleno)
        {
            List<Simple> lstWPS;
            using (SamContext ctx = new SamContext())
            {
                lstWPS =
                   (from myWPS in ctx.Wps
                    where myWPS.MaterialBase1ID == materialBase1ID &&
                          myWPS.MaterialBase2ID == materialBase2ID &&
                          myWPS.ProcesoRaizID == pocesoRaiz &&
                          myWPS.ProcesoRellenoID == procesoRelleno
                    select new Simple
                    {
                        ID = myWPS.WpsID,
                        Valor = myWPS.Nombre
                    }).ToList();
                return lstWPS;
            }
        }

        /// <summary>
        /// Obtiene la lista de soldadores que cumplan los criterios de patio, wps y fecha de vigencia.
        /// </summary>
        /// <param name="patioID"></param>
        /// <param name="wpsID"></param>
        /// <returns></returns>
        public List<Simple> ObtenerSoldadoresParaSoldadura(int patioID, int wpsID)
        {
            List<Simple> lstSoldador;
            using (SamContext ctx = new SamContext())
            {
                lstSoldador =
                    (from sld in ctx.Soldador
                     join w in ctx.Wpq on sld.SoldadorID equals w.SoldadorID
                     where sld.PatioID == patioID &&
                           w.WpsID == wpsID &&
                           w.FechaVigencia > DateTime.Now
                     select new Simple
                     {
                         ID = sld.SoldadorID,
                         Valor = sld.Codigo
                     }).ToList();
                return lstSoldador;
            }
        }

        /// <summary>
        /// Revisa si existe la colada para patio en especifico
        /// </summary>
        /// <param name="codigoColada">Codigo de Colada</param>
        /// <param name="patioID">ID del Patio</param>
        /// <returns></returns>
        public bool ExisteCodigoColadaEnPatio(string codigoColada, int patioID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (ctx.Consumible.Where(x => x.Codigo == codigoColada && x.PatioID == patioID).SingleOrDefault() == null)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Obtiene el ID de la Colada
        /// </summary>
        /// <param name="codigoColada">Codigo de la Colada</param>
        /// <param name="patioID">ID del Patio</param>
        /// <returns></returns>
        public int ObtenerColadaIDporCodigoYPatioID(string codigoColada, int patioID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Consumible.Where(x => x.Codigo == codigoColada && x.PatioID == patioID).Select(y => y.ConsumibleID).Single();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jws"></param>
        /// <returns></returns>
        public JuntaSoldadura ObtenerJuntaSoldadura(JuntaWorkstatus jws)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.JuntaSoldadura.Include("JuntaSoldaduraDetalle").Single(x => x.JuntaSoldaduraID == jws.JuntaSoldaduraID);
            }
        }

        public JuntaSoldadura ObtnerJuntaSoldaduraPorID(int juntaSoldaduraID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.JuntaSoldadura.Include("JuntaSoldaduraDetalle").Single(x => x.JuntaSoldaduraID == juntaSoldaduraID);
            }
        }
    }
}
