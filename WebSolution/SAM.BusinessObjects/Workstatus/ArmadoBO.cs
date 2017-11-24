using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Transactions;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Materiales;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Validations;
using SAM.Entities;
using SAM.Entities.Cache;
using SAM.Entities.Grid;
using SAM.Entities.Personalizadas;
using SAM.Entities.RadCombo;

namespace SAM.BusinessObjects.Workstatus
{
    public class ArmadoBO
    {
        private static readonly object _mutex = new object();
        private static ArmadoBO _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private ArmadoBO()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static ArmadoBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ArmadoBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// obtiene una lista de GrdArmado la cuál se utiliza para rellener la tabla de Armado
        /// </summary>
        /// <param name="proyectoID">indice del proyecto seleccionado, es el único requerido.</param>
        /// <param name="ordenTrabajoID"></param>
        /// <param name="ordenTrabajoSpoolID"></param>
        /// <returns></returns>
        public List<GrdArmado> ObtenerListaArmado(int proyectoID, int? ordenTrabajoID, int? ordenTrabajoSpoolID)
        {
            //Los filtros no pueden venir completamente vacíos o nos traeríamos todas las
            //ODTs de la BD
            if (ordenTrabajoID == null || proyectoID < 0)
            {
                throw new ArgumentException("El proyecto o la orden de trabajo son requeridos");
            }

            //listas a usarse, lstArmado será la que regrese. juntas se utiliza para el query
            List<GrdArmado> lstArmado = null;
            List<JuntaSpool> juntas = null;
            List<SpoolHold> spoolHold = null;
            List<MaterialSpool> materiales = null;
            List<OrdenTrabajoMaterial> ordenTrabajoMaterial = null;
            List<OrdenTrabajoJunta> ordenTrabajoJunta = null;
            List<JuntaWorkstatus> juntaWks = null;
            List<Spool> spools = null;
            //shop fab area
            int fabAreaID = CacheCatalogos.Instance.ShopFabAreaID;


            using (SamContext ctx = new SamContext())
            {
                IQueryable<OrdenTrabajoSpool> query = ctx.OrdenTrabajoSpool.AsQueryable();
                ctx.OrdenTrabajo.Where(x => x.ProyectoID == proyectoID).ToList();

                #region aplicar filtros

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

                //obtiene los ID's de spools y de ordendetrabajospool
                IQueryable<int> spoolIDs = query.Select(x => x.SpoolID);
                IQueryable<int> otsIDs = query.Select(x => x.OrdenTrabajoSpoolID);

                spools = ctx.Spool.Where(x => spoolIDs.Contains(x.SpoolID)).ToList();

                query.ToList();

                //se trae la informacion de junta spool aplicando los filtros necesarios.
                //enumera los juntaspoolid's
                //se trae la informacion de la juntaworkstatus.
                juntas = ctx.JuntaSpool.Where(x => spoolIDs.Contains(x.SpoolID) && x.FabAreaID == fabAreaID).ToList();
                IEnumerable<int> ieJuntaSpoolIDs = juntas.Select(x => x.JuntaSpoolID);
                juntaWks = ctx.JuntaWorkstatus.Where(x => ieJuntaSpoolIDs.Contains(x.JuntaSpoolID) && x.JuntaFinal).ToList();
                spoolHold = ctx.SpoolHold.Where(x => spoolIDs.Contains(x.SpoolID)).ToList();
                ordenTrabajoJunta = ctx.OrdenTrabajoJunta.Where(x => ieJuntaSpoolIDs.Contains(x.JuntaSpoolID)).ToList();

                //Obtengo los materiales y sus ordenes de trabajo para poder verificar si estan despachados o no.
                IQueryable<MaterialSpool> mat = ctx.MaterialSpool.Where(x => spoolIDs.Contains(x.SpoolID)).AsQueryable();
                ordenTrabajoMaterial = ctx.OrdenTrabajoMaterial.Where(x => mat.Select(y => y.MaterialSpoolID).Contains(x.MaterialSpoolID)).ToList();
                materiales = mat.ToList();
            }

            //se trae tipos de junta y familia aceros de cache para obtener nombres.
            List<TipoJuntaCache> tj = CacheCatalogos.Instance.ObtenerTiposJunta();
            List<FamAceroCache> fa1 = CacheCatalogos.Instance.ObtenerFamiliasAcero();

            lstArmado = (from jta in juntas
                         let fa2 = fa1.SingleOrDefault(x => x.ID == jta.FamiliaAceroMaterial2ID.GetValueOrDefault(0))
                         let jws = juntaWks.Where(x => x.JuntaSpoolID == jta.JuntaSpoolID).SingleOrDefault()
                         let hld = spoolHold.Where(x => x.SpoolID == jta.SpoolID).SingleOrDefault()
                         let otj = ordenTrabajoJunta.Where(x => x.JuntaSpoolID == jta.JuntaSpoolID).SingleOrDefault()
                         let spool = spools.Where(x => x.SpoolID == jta.SpoolID).SingleOrDefault()
                         let matSpool = materiales.Where(x => x.SpoolID == jta.SpoolID)

                         select new GrdArmado
                         {
                             NombreSpool = spool == null ? string.Empty : spool.Nombre,
                             NumeroControl = otj == null ? string.Empty : otj.OrdenTrabajoSpool.NumeroControl,
                             SpoolID = jta.SpoolID,
                             Junta = jws == null ? jta.Etiqueta : jws.EtiquetaJunta,
                             Localizacion = jta.EtiquetaMaterial1 + "-" + jta.EtiquetaMaterial2,
                             EtiquetaMaterial1 = jta.EtiquetaMaterial1,
                             EtiquetaMaterial2 = jta.EtiquetaMaterial2,
                             TipoJunta = tj.Single(x => x.ID == jta.TipoJuntaID).Text,
                             Cedula = jta.Cedula,
                             FamiliaAceroMaterial1 = fa1.Single(x => x.ID == jta.FamiliaAceroMaterial1ID).Text,
                             FamiliaAceroMaterial2 = fa2 != null ? fa2.Text : string.Empty,
                             Diametro = jta.Diametro,
                             TieneArmado = jws == null ? false : jws.ArmadoAprobado,
                             ArmadoAprobado = jws == null ? false : jws.ArmadoAprobado,
                             SoldaduraAprobada = jws == null ? false : jws.SoldaduraAprobada,
                             JuntaSpoolID = jta.JuntaSpoolID,
                             JuntaWorkStatusID = jws == null ? 0 : jws.JuntaWorkstatusID,
                             EstatusID = jws != null ? jws.ArmadoAprobado ? (int)EstatusArmado.Armado : -1 : -1,
                             Hold = hld == null ? false : hld.TieneHoldCalidad || hld.TieneHoldIngenieria || hld.Confinado,
                             JuntaArmadoID = jws == null ? -1 : jws.JuntaArmadoID.HasValue ? jws.JuntaArmadoID.Value : -1,
                         }).AsParallel().ToList();


            ILookup<int, Simple> materialPorSpool = materiales.ToLookup(x => x.SpoolID, y => new Simple { ID = y.MaterialSpoolID, Valor = y.Etiqueta });
            Dictionary<int, bool> odmDic = ordenTrabajoMaterial.ToDictionary(x => x.MaterialSpoolID, y => y.TieneDespacho);

            lstArmado.ToList().ForEach(x =>
            {
                if (x.EstatusID == -1)
                {
                    if (x.NumeroControl == string.Empty)
                    {
                        x.EstatusID = (int)EstatusArmado.SinODT;
                    }
                    else
                    {
                        IEnumerable<Simple> material = materialPorSpool[x.SpoolID];

                        int materialSpool1ID = EtiquetasMaterialUtil.ObtenMaterialSpoolDeEtiqueta(material, x.EtiquetaMaterial1);
                        int materialSpool2ID = EtiquetasMaterialUtil.ObtenMaterialSpoolDeEtiqueta(material, x.EtiquetaMaterial2);

                        if (materialSpool1ID > 0 && materialSpool2ID > 0)
                        {

                            if (odmDic.ContainsKey(materialSpool1ID) && odmDic.ContainsKey(materialSpool2ID) && odmDic[materialSpool1ID] && odmDic[materialSpool2ID])
                            {
                                x.EstatusID = (int)EstatusArmado.Despachado;
                            }
                            else
                            {
                                x.EstatusID = (int)EstatusArmado.SinDespacho;
                            }
                        }
                        else if (materialSpool1ID > 0)
                        {
                            if (odmDic.ContainsKey(materialSpool1ID) && odmDic[materialSpool1ID])
                            {
                                x.EstatusID = (int)EstatusArmado.Despachado;
                            }
                            else
                            {
                                x.EstatusID = (int)EstatusArmado.SinDespacho;
                            }
                        }
                        else if (materialSpool2ID > 0)
                        {
                            if (odmDic.ContainsKey(materialSpool2ID) && odmDic[materialSpool2ID])
                            {
                                x.EstatusID = (int)EstatusArmado.Despachado;
                            }
                            else
                            {
                                x.EstatusID = (int)EstatusArmado.SinDespacho;
                            }
                        }
                        else
                        {
                            x.EstatusID = (int)EstatusArmado.SinDespacho;
                        }
                    }
                }

                x.Estatus = TraductorEnumeraciones.TextoEstatusArmado(x.EstatusID);

            });

            return lstArmado;
        }


        /// <summary>
        /// obtiene la informacion necesaria a desplegarse en los popup de armado.
        /// </summary>
        /// <param name="juntaSpoolID"></param>
        /// <returns></returns>
        public OrdenTrabajoJunta ObtenerInformacionParaArmado(int juntaSpoolID)
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
        /// obtiene la informacion necesaria para rellenar el popup del armado.
        /// </summary>
        /// <param name="juntaWorkstatusID"></param>
        /// <returns></returns>
        public JuntaArmado ObtenerInformacionArmado(int juntaWorkstatusID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.JuntaArmado.Include("Tubero").Where(x => x.JuntaWorkstatusID == juntaWorkstatusID).SingleOrDefault();

            }
        }

        public List<string> ObtenerNumeroUnicoEnArmadoParaDespacho(int numeroUnico, OrdenTrabajoMaterial odtm)
        {
            using (SamContext ctx = new SamContext())
            {
                List<JuntaSpool> js = null;
                int jwsID = 0;
                JuntaArmado juntaArmado = null;
                MaterialSpool materiales = ctx.MaterialSpool.Where(x => x.MaterialSpoolID == odtm.MaterialSpoolID).SingleOrDefault();
                List<string> mensaje = new List<string>();
                int numTemp = 0;
                if (int.TryParse(materiales.Etiqueta, out numTemp))
                {
                    string tempParse = numTemp.ToString();
                    js = ctx.JuntaSpool.Include("JuntaWorkstatus")
                        .Where(x => x.SpoolID == materiales.SpoolID
                        && (
                            (x.EtiquetaMaterial1 == materiales.Etiqueta || x.EtiquetaMaterial1 == tempParse)
                            || (x.EtiquetaMaterial2 == materiales.Etiqueta || x.EtiquetaMaterial2 == tempParse)
                           )
                        ).ToList();

                }

                if (js != null && js.Count == 0)
                {
                    string tempParse = "0" + numTemp.ToString();
                    js = ctx.JuntaSpool.Include("JuntaWorkstatus")
                        .Where(x => x.SpoolID == materiales.SpoolID
                        && (
                            (x.EtiquetaMaterial1 == materiales.Etiqueta || x.EtiquetaMaterial1 == tempParse)
                            || (x.EtiquetaMaterial2 == materiales.Etiqueta || x.EtiquetaMaterial2 == tempParse)
                           )
                        ).ToList();
                }

                if (js.Count > 0)
                {
                    //recorrer cada una de las juntas
                    foreach (JuntaSpool junta in js)
                    {
                        //verificar si tienen algun workStatus
                        if (junta.JuntaWorkstatus.Any())
                        {
                            //recorrer cada uno de los worksStatus de la junta, en caso de que tenga mas de uno
                            foreach (JuntaWorkstatus jws in junta.JuntaWorkstatus)
                            {
                                //verificamos si el workStatus corresponde a la junta final, si no es junta final entonces se omite
                                if (jws.JuntaFinal)
                                {
                                    jwsID = jws.JuntaWorkstatusID.SafeIntParse();
                                    juntaArmado = ctx.JuntaArmado.Where(x => x.JuntaWorkstatusID == jwsID).SingleOrDefault();

                                    if ((junta.EtiquetaMaterial1 == materiales.Etiqueta) || (junta.EtiquetaMaterial1 == "0" + materiales.Etiqueta))
                                    {
                                        if (juntaArmado != null && juntaArmado.NumeroUnico1ID != null && juntaArmado.NumeroUnico1ID != numeroUnico)
                                        {
                                            NumeroUnico temp = (from nu in ctx.NumeroUnico
                                                                where nu.NumeroUnicoID == juntaArmado.NumeroUnico1ID
                                                                select nu).SingleOrDefault();
                                            mensaje.Add(temp.Codigo + "/" + temp.Cedula + "/"
                                                + temp.Diametro1.SafeStringParse()
                                                + "/" + temp.Diametro2.SafeStringParse()
                                                + "/" + junta.EtiquetaMaterial1);
                                        }
                                    }
                                    else if ((junta.EtiquetaMaterial2 == materiales.Etiqueta) || (junta.EtiquetaMaterial2 == "0" + materiales.Etiqueta))
                                    {
                                        if (juntaArmado != null && juntaArmado.NumeroUnico2ID != null && juntaArmado.NumeroUnico2ID != numeroUnico)
                                        {
                                            NumeroUnico temp = (from nu in ctx.NumeroUnico
                                                                where nu.NumeroUnicoID == juntaArmado.NumeroUnico2ID
                                                                select nu).SingleOrDefault();
                                            mensaje.Add(temp.Codigo + "/" + temp.Cedula + "/"
                                                + temp.Diametro1.SafeStringParse()
                                                + "/" + temp.Diametro2.SafeStringParse()
                                                + "/" + junta.EtiquetaMaterial2);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    return mensaje;
                }
                else
                {
                    return mensaje;
                }
            }
        }

        /// <summary>
        /// regresa una lista de objetos simple para los drop-downs de numeros unicos para armado.
        /// recibe la etiquetaMaterial en forma de string.
        /// </summary>
        /// <param name="etiquetaMaterialID"></param>
        /// <param name="ordenTrabajoSpoolID"></param>
        /// <param name="spoolID"></param>
        /// <returns></returns>
        public List<Simple> ObtenerNumeroUnicoPorEtiquetaMaterial(string etiquetaMaterialID, int ordenTrabajoSpoolID, int spoolID, int NumeroUnicoExistente = 0)
        {
            List<Simple> numUnico = new List<Simple>();

            using (SamContext ctx = new SamContext())
            {
                int i;
                MaterialSpool ms;

                //Debido a que puede haber una mala ingeniería hacemos single or default
                if (int.TryParse(etiquetaMaterialID, out i))
                {
                    string g = i.ToString();
                    ms = ctx.MaterialSpool
                            .Where(x => x.Etiqueta == etiquetaMaterialID || x.Etiqueta == g)
                            .Where(x => x.SpoolID == spoolID)
                            .SingleOrDefault();
                }
                else
                {
                    ms = ctx.MaterialSpool
                            .Where(x => x.Etiqueta == etiquetaMaterialID && x.SpoolID == spoolID)
                            .SingleOrDefault();
                }

                if (ms != null)
                {
                    numUnico =
                        (from otm in ctx.OrdenTrabajoMaterial
                         join ms2 in ctx.MaterialSpool on otm.MaterialSpoolID equals ms2.MaterialSpoolID
                         join nu in ctx.NumeroUnico on otm.NumeroUnicoDespachadoID equals nu.NumeroUnicoID
                         where otm.OrdenTrabajoSpoolID == ordenTrabajoSpoolID &&
                               ms2.ItemCodeID == ms.ItemCodeID &&
                               ms2.Diametro1 == ms.Diametro1 &&
                               ms2.Diametro2 == ms.Diametro2
                         select new Simple
                         {
                             ID = nu.NumeroUnicoID,
                             Valor = nu.Codigo
                         })
                         .Distinct()
                         .ToList();

                    //buscar un numero unico asignado a un armado, pero que no fue despachado para esta junta
                    Simple temp = (from tmp in ctx.NumeroUnico
                                   where tmp.NumeroUnicoID == NumeroUnicoExistente
                                   select new Simple
                                   {
                                       ID = tmp.NumeroUnicoID,
                                       Valor = tmp.Codigo
                                   }
                                   ).SingleOrDefault();
                    if (temp != null)
                    {
                        numUnico.Add(temp);
                    }
                }

                return numUnico;
            }
        }


        /// <summary>
        /// Obtiene el listado de Tuberos para patio seleccionado
        /// </summary>
        /// <param name="patioID">ID de Patio</param>
        /// <returns></returns>
        public List<Simple> ObtenerTuberosPorPatio(int patioID)
        {
            List<Simple> tuberos;
            using (SamContext ctx = new SamContext())
            {
                tuberos =
                    (from tub in ctx.Tubero
                     where tub.PatioID == patioID
                     select new Simple
                     {
                         ID = tub.TuberoID,
                         Valor = tub.Codigo
                     }).ToList();
                return tuberos;
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
                return ctx.JuntaWorkstatus.Where(x => x.JuntaSpoolID == juntaSpoolID && x.JuntaFinal == true).SingleOrDefault();
            }
        }

        /// <summary>
        /// guarda una entidad JuntaWorkstatus, la cuál se crea cuando se arma una junta.
        /// </summary>
        /// <param name="jws"></param>
        public void GuardaJuntaWorkstatus(JuntaWorkstatus jws, JuntaArmado ja)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SamContext ctx = new SamContext())
                    {
                        ValidaFechasBO.Instance.ValidaFechasProcesoFechaRequiPintura(ja.FechaArmado.Date, jws.OrdenTrabajoSpoolID);   
 
                        //Guardar Junta Workstatus
                        jws.StartTracking();
                        ctx.JuntaWorkstatus.ApplyChanges(jws);
                        ctx.SaveChanges();

                        //Guardar Junta Armado
                        ja.JuntaWorkstatusID = jws.JuntaWorkstatusID;

                        ctx.JuntaArmado.ApplyChanges(ja);
                        ctx.SaveChanges();


                        //Re guardar Junta Workstatus añadiendo Junta Armado
                        jws.StartTracking();
                        jws.JuntaArmadoID = ja.JuntaArmadoID;
                        ctx.JuntaWorkstatus.ApplyChanges(jws);
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

        public void GuardarEdicionEspecialArmado(JuntaArmado junta)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    JuntaArmado juntaActualizar = ctx.JuntaArmado.Where(x => x.JuntaArmadoID == junta.JuntaArmadoID).SingleOrDefault();
                    if (juntaActualizar != null)
                    {
                        juntaActualizar.StartTracking();
                        juntaActualizar.NumeroUnico1ID = junta.NumeroUnico1ID;
                        juntaActualizar.NumeroUnico2ID = junta.NumeroUnico2ID;
                        juntaActualizar.TallerID = junta.TallerID;
                        juntaActualizar.TuberoID = junta.TuberoID;
                        juntaActualizar.FechaArmado = junta.FechaArmado;
                        juntaActualizar.FechaReporte = junta.FechaReporte;
                        juntaActualizar.UsuarioModifica = junta.UsuarioModifica;
                        juntaActualizar.FechaModificacion = junta.FechaModificacion;
                        juntaActualizar.Observaciones = junta.Observaciones;
                        juntaActualizar.StopTracking();
                        ctx.JuntaArmado.ApplyChanges(juntaActualizar);
                        ctx.SaveChanges();
                    }
                }

            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// Elimina el registro de juntaArmado y pone en falso la aprobacion del armado en JuntaWorkstatus
        /// </summary>
        /// <param name="juntaArmadoID">ID del armado a eliminar</param>
        /// <param name="userID">ID del usuario logeado</param>
        public void BorraArmado(int juntaArmadoID, Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntaWorkstatus jWks = ctx.JuntaWorkstatus.Where(x => x.JuntaArmadoID == juntaArmadoID).FirstOrDefault();
                JuntaSpool js = ctx.JuntaSpool.Include("TipoJunta").FirstOrDefault(x => x.JuntaSpoolID == jWks.JuntaSpoolID);
                JuntaSoldadura jSoldadura = null;

                WorkstatusSpool ws = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID == jWks.OrdenTrabajoSpoolID).FirstOrDefault();

                if (ws != null)
                {
                    if (ws.TieneRequisicionPintura || ws.TienePintura)
                    {
                        throw new ExcepcionRelaciones(MensajesError.Excepcion_TieneRequiPintura);
                    }
                }
                // Si la junta es de tipo TW, eliminamos reporte de soldadura en caso de que cuente con ella
                if (js.TipoJunta.Codigo == TipoJuntas.TW)
                {
                    int juntaSoldaduraID = jWks.JuntaSoldaduraID.SafeIntParse();
                    jSoldadura = ctx.JuntaSoldadura.FirstOrDefault(x => x.JuntaSoldaduraID == juntaSoldaduraID);
                }
                else
                {
                    //Validamos que no tenga soldadura
                    if (jWks.SoldaduraAprobada || jWks.JuntaSoldaduraID.HasValue)
                    {
                        throw new ExcepcionSoldadura(MensajesError.Excepcion_JuntaConSoldadura);
                    }
                }

                JuntaArmado jArmado = ctx.JuntaArmado.Where(x => x.JuntaArmadoID == juntaArmadoID).FirstOrDefault();

                jWks.StartTracking();
                jWks.ArmadoAprobado = false;
                jWks.JuntaArmadoID = null;
                jWks.SoldaduraAprobada = jSoldadura != null ? false : jWks.SoldaduraAprobada;
                jWks.JuntaSoldaduraID = jSoldadura != null ? null : jWks.JuntaSoldaduraID;
                jWks.UsuarioModifica = userID;
                jWks.FechaModificacion = DateTime.Now;
                jWks.StopTracking();

                if (jSoldadura != null)
                {
                    ctx.DeleteObject(jSoldadura);
                }
                ctx.JuntaWorkstatus.ApplyChanges(jWks);
                ctx.JuntaArmado.DeleteObject(jArmado);
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Revisa si existe el armado para la junta
        /// </summary>
        /// <param name="juntaSpoolID">Junta Spool ID</param>
        /// <returns></returns>
        public bool ExisteJuntaArmado(int juntaSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (ctx.JuntaWorkstatus.Where(x => x.JuntaSpoolID == juntaSpoolID && x.ArmadoAprobado == true && x.JuntaFinal).SingleOrDefault() == null)
                {
                    return false;
                }
                return true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="juntaWorkstatusID"></param>
        /// <returns></returns>
        public bool ObtenerJuntaWorkstatusPorID(int juntaWorkstatusID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.JuntaArmado.Any(x => x.JuntaWorkstatusID == juntaWorkstatusID);
            }
        }

        public void CambiaFechaReporteArmado(int juntaArmadoID, DateTime FechaArmado, DateTime FechaReporte, Guid usuario)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SamContext ctx = new SamContext())
                    {
                        JuntaArmado ja = ctx.JuntaArmado.Single(x => x.JuntaArmadoID == juntaArmadoID);
                        ja.StartTracking();
                        ja.FechaReporte = FechaReporte;
                        ja.FechaArmado = FechaArmado;
                        ja.FechaModificacion = DateTime.Now;
                        ja.UsuarioModifica = usuario;
                        ja.StopTracking();
                        ctx.JuntaArmado.ApplyChanges(ja);
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
    }
}
