using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.Entities.Personalizadas;
using SAM.Entities.Grid;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using System.Transactions;

namespace SAM.BusinessObjects.Produccion
{
    public class CongeladosBO
    {
        private struct parametrosTransferencia
        {
            public int[] spools;
            public string[] nombreSpools;
            public int[] matSpool;
            public string[] nombresMaterial;
            public string codigoPantallaPrincial;
            public int numeroUnicoPantallaPrincial;
            public int numeroUnico;
            public string codigoNumUnicoNuevo;
            public string codigoSegmento;
            public int cantidad;
            public int proyectoID;
            public Guid userID;
            public DateTime fechaModificacion;
        }



        private static readonly object _mutex = new object();
        private static CongeladosBO _instance;

        /// <summary>
        /// Obtiene la instancia de la clase CongeladosBO
        /// </summary>
        /// <returns></returns>
        public static CongeladosBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CongeladosBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="numeroUnico"></param>
        /// <returns></returns>
        public CongeladosOdt ObtenerInfoNumUnico(int proyectoID, int numeroUnico, string codigoSegmento)
        {
            //CruceProyecto cruceProyecto = new CruceProyecto();
            List<ItemCode> itemCode = null;
            List<NumeroUnico> numerosUnico = null;
            List<NumeroUnicoInventario> numerosUnicoInventario = null;
            List<NumeroUnicoSegmento> numeroUnicoSegmento = null;
            string segmento = string.Empty;

            using (SamContext ctx = new SamContext())
            {
                itemCode = ctx.ItemCode.Where(x => x.ProyectoID == proyectoID && x.ItemCodeID == ctx.NumeroUnico.Where(y => y.NumeroUnicoID == numeroUnico).FirstOrDefault().ItemCodeID).ToList();
                numerosUnico = ctx.NumeroUnico.Where(x => x.ProyectoID == proyectoID && x.NumeroUnicoID == numeroUnico).ToList();
                if (ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == numeroUnico).Any())
                {
                    segmento = codigoSegmento.Substring(codigoSegmento.Length - 1);
                    numeroUnicoSegmento = ctx.NumeroUnicoSegmento.Where(x => x.ProyectoID == proyectoID && x.NumeroUnicoID == numeroUnico && x.Segmento == segmento).ToList();
                    return (from nu in numerosUnico
                            let ic = itemCode.FirstOrDefault(x => x.ItemCodeID == nu.ItemCodeID)
                            let nus = numeroUnicoSegmento.SingleOrDefault(x => x.NumeroUnicoID == nu.NumeroUnicoID)
                            select new CongeladosOdt
                            {
                                Codigo = ic.Codigo,
                                Descripcion = ic.DescripcionEspanol,
                                Diametro1 = nu.Diametro1,
                                Diametro2 = nu.Diametro2,
                                InvFisico = nus.InventarioFisico,
                                InvDañado = nus.CantidadDanada,
                                InvCongelado = nus.InventarioCongelado,
                                InvDisponible = nus.InventarioDisponibleCruce
                            }
                            ).SingleOrDefault();
                }
                else
                {
                    numerosUnicoInventario = ctx.NumeroUnicoInventario.Where(x => x.ProyectoID == proyectoID && x.NumeroUnicoID == numeroUnico).ToList();
                    return (from nu in numerosUnico
                            let ic = itemCode.FirstOrDefault(x => x.ItemCodeID == nu.ItemCodeID)
                            let nui = numerosUnicoInventario.SingleOrDefault(x => x.NumeroUnicoID == nu.NumeroUnicoID)
                            select new CongeladosOdt
                            {
                                Codigo = ic.Codigo,
                                Descripcion = ic.DescripcionEspanol,
                                Diametro1 = nu.Diametro1,
                                Diametro2 = nu.Diametro2,
                                InvFisico = nui.InventarioFisico,
                                InvDañado = nui.CantidadDanada,
                                InvCongelado = nui.InventarioCongelado,
                                InvDisponible = nui.InventarioDisponibleCruce
                            }
            ).SingleOrDefault();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numeroUnico"></param>
        /// <returns></returns>
        public List<GrdCongeladosNumeroUnico> ObtenerListadoCruce(int numeroUnico)
        {
            List<GrdCongeladosNumeroUnico> lstotm = null;
            List<GrdCongeladosNumeroUnico> lstcongpar = null;

            using (SamContext ctx = new SamContext())
            {
                lstotm = (from otm in ctx.OrdenTrabajoMaterial
                          join ots in ctx.OrdenTrabajoSpool
                          on otm.OrdenTrabajoSpoolID equals ots.OrdenTrabajoSpoolID
                          join spool in ctx.Spool
                          on ots.SpoolID equals spool.SpoolID
                          where otm.NumeroUnicoCongeladoID == numeroUnico
                          select new GrdCongeladosNumeroUnico
                          {
                              Spool = spool.Nombre,
                              SpoolID = spool.SpoolID,
                              MaterialSpoolID = otm.MaterialSpoolID,
                              NumControl = ots.NumeroControl,
                              Etiqueta = otm.MaterialSpool.Etiqueta,
                              CantCong = otm.CantidadCongelada.HasValue ? otm.CantidadCongelada.Value : 0,
                              Equiv = otm.CongeladoEsEquivalente
                          }).Distinct().ToList();

                lstcongpar = (from congpar in ctx.CongeladoParcial
                              join matspool in ctx.MaterialSpool
                              on congpar.MaterialSpoolID equals matspool.MaterialSpoolID
                              join spool in ctx.Spool
                              on matspool.SpoolID equals spool.SpoolID
                              where congpar.NumeroUnicoCongeladoID == numeroUnico
                              select new GrdCongeladosNumeroUnico
                              {
                                  Spool = spool.Nombre,
                                  SpoolID = spool.SpoolID,
                                  MaterialSpoolID = matspool.MaterialSpoolID,
                                  NumControl = "",
                                  Etiqueta = matspool.Etiqueta,
                                  CantCong = matspool.Cantidad,
                                  Equiv = congpar.EsEquivalente
                              }).Distinct().ToList();

                return lstotm.Union(lstcongpar).Distinct().ToList();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public List<NumeroUnico> ObtenerNumeroUnico(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return (ctx.NumeroUnico.Where(x => x.ProyectoID == proyectoID).ToList());
            }
        }

        /// <summary>
        /// Realiza la tranbferencia de inventarios congelados
        /// </summary>
        /// <param name="spools">Lista de Id de Spools a actualizar</param>
        /// <param name="matSpool">Lista de id de materiales a actualizar</param>
        /// <param name="codigoPantallaPrincial">Codigo del número único al que se le va a liberar inventario (no estoy seguro de esto JHT)</param>
        /// <param name="numeroUnicoPantallaPrincial">Id del numero unico al que se le va a liberar inventario (no estoy seguro de esto JHT)</param>
        /// <param name="numeroUnico">Nuevo numero unico al que se le van a transferir los congelados</param>
        /// <param name="codigoSegmento">Segmento al que se le transfieren los congelados</param>
        /// <param name="cantidad">Cantidad a congelar y liberar</param>
        /// <param name="proyectoID"></param>
        /// <param name="userID"></param>
        /// <param name="fechaModificacion"></param>
        public void TransferirCongelados(int[] spools, int[] matSpool, string codigoPantallaPrincial, int numeroUnicoPantallaPrincial, int numeroUnico, string codigoSegmento,
            int cantidad, int proyectoID, Guid userID, DateTime fechaModificacion)
        {
            using (TransactionScope t = new TransactionScope())
            {
                //Tranfeire los congelados a un nuevo numero unico
                TransferirInventario(numeroUnico, codigoSegmento, cantidad, proyectoID, userID, fechaModificacion);
                //Actualiza las ordenes de trabajo material y materiales
                cambiarOrdenTrabajoMaterial(spools, matSpool, numeroUnico, codigoSegmento, numeroUnicoPantallaPrincial, userID, fechaModificacion);

                liberarInventario(numeroUnicoPantallaPrincial, codigoSegmento, cantidad, proyectoID, userID, fechaModificacion);

                t.Complete();
            }
        }

        /// <summary>
        /// Realiza la transferencia de congelados en negativo para todos los numeros unicos de un proyecto
        /// </summary>
        /// <param name="proyectoId"></param>
        public void TransferenciaMasivaDeCongelados(int proyectoId, Guid userId)
        {
            try
            {
                IQueryable<NumeroUnico> numerosUnicos = null;
                List<OrdenTrabajoMaterial> odtmFiltrados = null;
                List<NumeroUnico> numerosUnicosFiltrados = null;
                List<NumeroUnico> numerosUnicosCandidatos = null;
                List<parametrosTransferencia> historialTransferencias = new List<parametrosTransferencia>();
                using (SamContext ctx = new SamContext())
                {
                    //Obtengo todos los numeros unicos de un proyecto, cuando el inventario disponible de cruce es negativo 
                    //y el proyecto corresponde con el que estamos trabajando
                    numerosUnicos = ctx.NumeroUnico
                                       .Include("NumeroUnicoInventario")
                                       .Include("NumeroUnicoSegmento")
                                       .Where(x => x.ProyectoID == proyectoId)
                                       .Where(x => x.NumeroUnicoInventario.InventarioDisponibleCruce < 0);

                    numerosUnicosFiltrados = numerosUnicos.ToList();

                    foreach (NumeroUnico nu in numerosUnicosFiltrados)
                    {
                        //Obtengo los numeros unicos que pueden ser candidatos, buscando por ItemCode, Diametro 1 y 2, Proyecto
                        //y que su disponible para cruce sea mayor o igual a la cantidad que queremos tranferir
                        numerosUnicosCandidatos = ctx.NumeroUnico
                                                     .Include("NumeroUnicoInventario")
                                                     .Include("NumeroUnicoSegmento")
                                                     .Where(x => x.ItemCodeID == nu.ItemCodeID
                                                         && x.ProyectoID == proyectoId
                                                         && x.Diametro1 == nu.Diametro1
                                                         && x.Diametro2 == nu.Diametro2
                                                         && x.NumeroUnicoInventario.InventarioDisponibleCruce >= nu.NumeroUnicoInventario.InventarioCongelado)
                                                     .OrderByDescending(x => x.NumeroUnicoInventario.InventarioDisponibleCruce)
                                                     .ToList();

                        //necesito la lista de las ordenes de material en donde se utiliza este numero unico
                        //incluyendo los materiales y las ordenes de trabajo spool
                        odtmFiltrados = ctx.OrdenTrabajoMaterial
                            .Include("MaterialSpool")
                            .Include("OrdenTrabajoSpool")
                            .Where(x => x.NumeroUnicoCongeladoID == nu.NumeroUnicoID).ToList();

                        if (numerosUnicosCandidatos.Count > 0 && odtmFiltrados.Count > 0)
                        {
                            //Prepara las variables para la transferencia de congelados
                            parametrosTransferencia parametros = new parametrosTransferencia();
                            //Arreglo con los id de spool
                            parametros.spools = odtmFiltrados.Select(x => x.MaterialSpool.SpoolID).Distinct().ToArray();
                            //arreglo con los ids de materiales
                            parametros.matSpool = odtmFiltrados.Select(x => x.MaterialSpool.MaterialSpoolID).Distinct().ToArray();
                            //codigo del numero unico original
                            parametros.codigoPantallaPrincial = nu.Codigo;
                            //Id del numero unico original
                            parametros.numeroUnicoPantallaPrincial = nu.NumeroUnicoID;
                            //cantidad congelada. La suma de las catidades marcadas en todas las ODTM ?????
                            parametros.cantidad = odtmFiltrados.Select(x => x.CantidadCongelada.Value).Sum();
                            //id del nuevo numero unico
                            //tomamos el primer numero unico del arreglo
                            parametros.numeroUnico = (from n in numerosUnicosCandidatos
                                                      where n.NumeroUnicoInventario.InventarioDisponibleCruce >= parametros.cantidad
                                                      orderby n.NumeroUnicoInventario.InventarioDisponibleCruce ascending
                                                      select n.NumeroUnicoID).FirstOrDefault();
                            //codigo del segmento
                            string segmento = "";
                            if (nu.NumeroUnicoSegmento.Count() > 1)
                            {
                                segmento = (from n in nu.NumeroUnicoSegmento
                                            where n.InventarioDisponibleCruce < 0
                                            select n.Segmento).Single();
                            }
                            else if (!nu.NumeroUnicoSegmento.Any())
                            {
                                segmento = "";
                            }
                            else
                            {
                                segmento = (from n in nu.NumeroUnicoSegmento
                                            where n.Segmento == "A"
                                            select n.Segmento).Single();
                            }
                            parametros.codigoSegmento = segmento;
                            
                            //Proyecto ID
                            parametros.proyectoID = proyectoId;
                            //User ID
                            parametros.userID = userId;
                            //Fecha de Modificación
                            parametros.fechaModificacion = DateTime.Now;

                            historialTransferencias.Add(parametros);

                            TransferirCongelados(parametros.spools, parametros.matSpool, parametros.codigoPantallaPrincial, parametros.numeroUnicoPantallaPrincial
                                , parametros.numeroUnico, parametros.codigoSegmento, parametros.cantidad, parametros.proyectoID, parametros.userID, parametros.fechaModificacion);
                        }
                    }
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
        /// <param name="numeroUnico"></param>
        /// <param name="cantidad"></param>
        /// <param name="proyectoID"></param>
        /// <param name="userID"></param>
        /// <param name="fechaModificacion"></param>
        public void TransferirInventario(int numeroUnico, string codigoSegmento, int cantidad, int proyectoID, Guid userID, DateTime fechaModificacion)
        {
            try
            {
                int inventarioActualizado = 0;

                using (SamContext ctx = new SamContext())
                {

                    NumeroUnicoInventario numunicoinv = ctx.NumeroUnicoInventario
                                                        .Where(x => x.NumeroUnicoID == numeroUnico)
                                                        .Where(x => x.ProyectoID == proyectoID)
                                                        .SingleOrDefault();
                    inventarioActualizado = numunicoinv.InventarioCongelado + cantidad;
                    numunicoinv.InventarioCongelado = inventarioActualizado;
                    numunicoinv.InventarioDisponibleCruce = numunicoinv.InventarioBuenEstado - inventarioActualizado;
                    numunicoinv.UsuarioModifica = userID;
                    numunicoinv.FechaModificacion = fechaModificacion;
                    ctx.NumeroUnicoInventario.ApplyChanges(numunicoinv);

                    if (ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == numeroUnico).Any())
                    {
                        string segmento = codigoSegmento.Substring(codigoSegmento.Length - 1);
                        NumeroUnicoSegmento numunicoseg = ctx.NumeroUnicoSegmento
                                                            .Where(x => x.NumeroUnicoID == numeroUnico)
                                                            .Where(x => x.ProyectoID == proyectoID)
                                                            .Where(x => x.Segmento == segmento)
                                                            .SingleOrDefault();
                        if (numunicoseg == null)
                        {
                            numunicoseg = ctx.NumeroUnicoSegmento
                                        .Where(x => x.NumeroUnicoID == numeroUnico)
                                        .Where(x => x.ProyectoID == proyectoID)
                                        .Where(x => x.InventarioDisponibleCruce >= cantidad)
                                        .SingleOrDefault();
                        }

                        inventarioActualizado = numunicoseg.InventarioCongelado + cantidad;
                        numunicoseg.InventarioCongelado = inventarioActualizado;
                        numunicoseg.InventarioDisponibleCruce = numunicoseg.InventarioBuenEstado - inventarioActualizado;
                        numunicoseg.UsuarioModifica = userID;
                        numunicoseg.FechaModificacion = fechaModificacion;
                        ctx.NumeroUnicoSegmento.ApplyChanges(numunicoseg);
                    }
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
        /// <param name="spools"></param>
        /// <param name="numerounico"></param>
        /// <param name="userID"></param>
        /// <param name="fechaModificacion"></param>
        public void cambiarOrdenTrabajoMaterial(int[] spools, int[] matSpool, int numerounico, string codigoSegmento, int numeroUnicoPantallaPrincial, Guid userID, DateTime fechaModificacion)
        {
            try
            {
                string segmento = null;
                using (SamContext ctx = new SamContext())
                {
                    if (ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == numerounico).Any())
                    {
                        string temp = codigoSegmento.Substring(codigoSegmento.Length - 1);
                        segmento = (from n in ctx.NumeroUnicoSegmento
                                    where n.Segmento == temp && n.NumeroUnicoID == numerounico
                                    select n.Segmento).SingleOrDefault();

                        if (segmento == "")
                        {
                            segmento = (from n in ctx.NumeroUnicoSegmento
                                        where n.NumeroUnicoID == numerounico
                                        select n.Segmento).SingleOrDefault();
                        }
                    }
                    int itemcodePopUP = ctx.NumeroUnico.Where(x => x.NumeroUnicoID == numerounico).Select(x => x.ItemCodeID.Value).FirstOrDefault();
                    int itemcodePrincipal = ctx.NumeroUnico.Where(x => x.NumeroUnicoID == numeroUnicoPantallaPrincial).Select(x => x.ItemCodeID.Value).FirstOrDefault();
                    bool equivalente = esEquivalente(itemcodePopUP, itemcodePrincipal);

                    if (ctx.CongeladoParcial.Where(x => x.NumeroUnicoCongeladoID == numeroUnicoPantallaPrincial).Any())
                    {
                        int[] matspool = ctx.MaterialSpool.Where(x => spools.Contains(x.SpoolID) && matSpool.Contains(x.MaterialSpoolID)).Select(x => x.MaterialSpoolID).ToArray();
                        List<CongeladoParcial> congPar = ctx.CongeladoParcial.Where(x => matspool.Contains(x.MaterialSpoolID) && x.NumeroUnicoCongeladoID == numeroUnicoPantallaPrincial).ToList();
                        congPar.ForEach(x =>
                            {
                                x.StartTracking();
                                x.NumeroUnicoCongeladoID = numerounico;
                                x.SegmentoCongelado = segmento;
                                x.EsEquivalente = equivalente;
                                x.UsuarioModifica = userID;
                                x.FechaModificacion = fechaModificacion;
                                x.StopTracking();
                                ctx.CongeladoParcial.ApplyChanges(x);
                            });
                    }
                    if (ctx.OrdenTrabajoMaterial.Where(x => x.NumeroUnicoCongeladoID == numeroUnicoPantallaPrincial).Any())
                    {
                        int[] _ots = ctx.OrdenTrabajoSpool.Where(x => spools.Contains(x.SpoolID)).Select(x => x.OrdenTrabajoSpoolID).ToArray();
                        List<OrdenTrabajoMaterial> _otm = ctx.OrdenTrabajoMaterial.Where(x => _ots.Contains(x.OrdenTrabajoSpoolID) && x.NumeroUnicoCongeladoID == numeroUnicoPantallaPrincial && matSpool.Contains(x.MaterialSpoolID)).ToList();
                        _otm.ForEach(x =>
                        {
                            x.StartTracking();
                            x.NumeroUnicoCongeladoID = numerounico;
                            x.SegmentoCongelado = segmento;
                            x.CongeladoEsEquivalente = equivalente;
                            x.UsuarioModifica = userID;
                            x.FechaModificacion = fechaModificacion;
                            x.StopTracking();
                            ctx.OrdenTrabajoMaterial.ApplyChanges(x);
                        });
                    }

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
        /// <param name="numerounico"></param>
        /// <param name="cantidad"></param>
        /// <param name="proyectoID"></param>
        /// <param name="userID"></param>
        /// <param name="fechaModificacion"></param>
        public void liberarInventario(int numerounico, string codigoSegmento, int cantidad, int proyectoID, Guid userID, DateTime fechaModificacion)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    NumeroUnicoInventario numunicoinv = ctx.NumeroUnicoInventario
                                                        .Where(x => x.NumeroUnicoID == numerounico)
                                                        .Where(x => x.ProyectoID == proyectoID)
                                                        .SingleOrDefault();
                    int inventarioActualizado = numunicoinv.InventarioCongelado - cantidad;
                    numunicoinv.InventarioCongelado = inventarioActualizado;
                    numunicoinv.InventarioDisponibleCruce = numunicoinv.InventarioBuenEstado - inventarioActualizado;
                    numunicoinv.UsuarioModifica = userID;
                    numunicoinv.FechaModificacion = fechaModificacion;
                    ctx.NumeroUnicoInventario.ApplyChanges(numunicoinv);

                    if (ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == numerounico).Any())
                    {
                        string segmento = codigoSegmento.Substring(codigoSegmento.Length - 1);
                        NumeroUnicoSegmento numunicoseg = ctx.NumeroUnicoSegmento
                                                            .Where(x => x.NumeroUnicoID == numerounico)
                                                            .Where(x => x.ProyectoID == proyectoID)
                                                            .Where(x => x.Segmento == segmento)
                                                            .SingleOrDefault();
                        if (numunicoseg == null)
                        {
                            numunicoseg = ctx.NumeroUnicoSegmento
                                        .Where(x => x.NumeroUnicoID == numerounico)
                                        .Where(x => x.ProyectoID == proyectoID)
                                        .Where(x => x.InventarioDisponibleCruce >= cantidad)
                                        .SingleOrDefault();
                        }

                        inventarioActualizado = numunicoseg.InventarioCongelado - cantidad;
                        numunicoseg.InventarioCongelado = inventarioActualizado;
                        numunicoseg.InventarioDisponibleCruce = numunicoseg.InventarioBuenEstado - inventarioActualizado;
                        numunicoseg.UsuarioModifica = userID;
                        numunicoseg.FechaModificacion = fechaModificacion;
                        ctx.NumeroUnicoSegmento.ApplyChanges(numunicoseg);
                    }
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
        /// <param name="OrdenTrabajoSpoolID"></param>
        /// <returns></returns>
        public Spool obtenerSpoolPorOrdenTrabajoSpoolID(int OrdenTrabajoSpoolID)
        {
            Spool spoolID = null;
            using (SamContext ctx = new SamContext())
            {
                return spoolID = ctx.Spool.Where(y => y.SpoolID == ctx.OrdenTrabajoSpool
                                .Where(x => x.OrdenTrabajoSpoolID == OrdenTrabajoSpoolID)
                                .Select(x => x.SpoolID).FirstOrDefault()).SingleOrDefault();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrdenTrabajoID"></param>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public List<GrdCongeladosOrdenTrabajo> obtenerListadoCongeladoOrdenTrabajo(int NumeroControl, int OrdenTrabajoID, int proyectoID)
        {
            IQueryable<MaterialSpool> materiales = null;
            IQueryable<OrdenTrabajoMaterial> ordenTrabajoMaterial = null;
            List<GrdCongeladosOrdenTrabajo> lst = null;
            IQueryable<ItemCode> itemcode = null;
            int[] spool;
            using (SamContext ctx = new SamContext())
            {
                if (NumeroControl != -1)
                    spool = ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajoID == OrdenTrabajoID && x.OrdenTrabajoSpoolID == NumeroControl).Select(x => x.SpoolID).ToArray();
                else
                    spool = ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajoID == OrdenTrabajoID).Select(x => x.SpoolID).ToArray();

                materiales = ctx.MaterialSpool.Where(x => spool.Contains(x.SpoolID));
                itemcode = ctx.ItemCode.Where(x => materiales.Select(y => y.ItemCodeID).Contains(x.ItemCodeID));
                ordenTrabajoMaterial = ctx.OrdenTrabajoMaterial.Where(x => materiales.Select(y => y.MaterialSpoolID).Contains(x.MaterialSpoolID));

                var numerosUnicosCongelados = from otm in ordenTrabajoMaterial
                                              join nu in ctx.NumeroUnico
                                              on otm.NumeroUnicoCongeladoID equals nu.NumeroUnicoID
                                              select new
                                              {
                                                  numeroUnicoCodigo = otm.SegmentoCongelado == null ? nu.Codigo : nu.Codigo + "-" + otm.SegmentoCongelado,
                                                  numeroUnicoID = nu.NumeroUnicoID,
                                                  cantidad = otm.CantidadCongelada,
                                                  matSpoolID = otm.MaterialSpoolID,
                                                  estatus = "Congelado"
                                              };

                var numerosUnicosDespachados = from otm in ordenTrabajoMaterial
                                               join nu in ctx.NumeroUnico
                                               on otm.NumeroUnicoDespachadoID equals nu.NumeroUnicoID
                                               select new
                                               {
                                                   numeroUnicoCodigo = otm.SegmentoDespachado == null ? nu.Codigo : nu.Codigo + "-" + otm.SegmentoDespachado,
                                                   numeroUnicoID = nu.NumeroUnicoID,
                                                   cantidad = otm.CantidadDespachada,
                                                   matSpoolID = otm.MaterialSpoolID,
                                                   estatus = "Despachado"
                                               };

                var numerosUnicos = numerosUnicosCongelados.Union(numerosUnicosDespachados);

                return lst = (from mat in materiales
                              join it in itemcode
                              on mat.ItemCodeID equals it.ItemCodeID
                              join sp in ctx.Spool
                              on mat.SpoolID equals sp.SpoolID
                              join nu in numerosUnicos
                              on mat.MaterialSpoolID equals nu.matSpoolID
                              select new GrdCongeladosOrdenTrabajo
                              {
                                  Spool = sp.Nombre,
                                  SpoolID = sp.SpoolID,
                                  MaterialSpoolID = mat.MaterialSpoolID,
                                  ProyectoID = proyectoID,
                                  Etiqueta = mat.Etiqueta,
                                  ItemCode = it.Codigo,
                                  Descripcion = it.DescripcionEspanol,
                                  D1 = mat.Diametro1,
                                  D2 = mat.Diametro2,
                                  Codigo = nu.numeroUnicoCodigo,
                                  NumeroUnico = nu.numeroUnicoID,
                                  Cantidad = nu.cantidad.Value,
                                  Estatus = nu.estatus
                              }).Distinct().ToList();

                //var numerosUnicos = from mat in materiales
                //                    join otm in ordenTrabajoMaterial
                //                    on mat.MaterialSpoolID equals otm.MaterialSpoolID into leftotm
                //                    from left in leftotm.DefaultIfEmpty()
                //                    select new
                //                    {
                //                        numeroUnico = left.NumeroUnicoCongeladoID.HasValue ? left.NumeroUnicoCongeladoID.Value : left.NumeroUnicoDespachadoID.HasValue ? left.NumeroUnicoDespachadoID.Value : -1,
                //                        cantidad = left.CantidadCongelada.HasValue ? left.CantidadCongelada.Value : left.CantidadDespachada.HasValue ? left.CantidadDespachada.Value : -1,
                //                        estatus = left.TieneInventarioCongelado ? "Congelado" : left.TieneDespacho ? "Despachado" : "No Congelado",
                //                        materialSpoolID = mat.MaterialSpoolID
                //                    };

                //var segmentos = from nuvar in numerosUnicos                                
                //                join nu in ctx.NumeroUnico
                //                on nuvar.numeroUnico equals nu.NumeroUnicoID                                
                //                join nus in ctx.NumeroUnicoSegmento                                
                //                on nuvar.numeroUnico equals nus.NumeroUnicoID into leftnus
                //                from left in leftnus.DefaultIfEmpty()                                       
                //                select new{
                //                    codigo = left.NumeroUnicoID!=null?nu.Codigo + "-" + left.Segmento:nu.Codigo,
                //                    numeroUnico = nu.NumeroUnicoID,
                //                    materialSpoolID = nuvar.materialSpoolID,
                //                    cantidad = nuvar.cantidad,
                //                    estatus = nuvar.estatus
                //                };

                //return lst = (from mat in materiales                              
                //              join ic in itemcode
                //              on mat.ItemCodeID equals ic.ItemCodeID
                //              join sp in ctx.Spool
                //              on mat.SpoolID equals sp.SpoolID
                //              join seg in segmentos
                //              on mat.MaterialSpoolID equals seg.materialSpoolID into leftotm
                //              from left in leftotm.DefaultIfEmpty()                                       
                //              select new GrdCongeladosOrdenTrabajo
                //              {
                //                  Spool = sp.Nombre,
                //                  SpoolID = sp.SpoolID,
                //                  MaterialSpoolID = mat.MaterialSpoolID,
                //                  ProyectoID = proyectoID,
                //                  Etiqueta = mat.Etiqueta,
                //                  ItemCode = ic.Codigo,
                //                  Descripcion = ic.DescripcionEspanol,
                //                  D1 = mat.Diametro1,
                //                  D2 = mat.Diametro2,
                //                  Codigo = left.codigo,
                //                  NumeroUnico = left.numeroUnico!=null?left.numeroUnico:-1,
                //                  Cantidad = left.cantidad!= null?left.cantidad:-1,
                //                  Estatus = left.estatus!=null ? left.estatus : "No Congelado",
                //              }).Distinct().ToList();
                //left.NumeroUnicoCongeladoID.HasValue ? ctx.NumeroUnico.Where(x => x.NumeroUnicoID == left.NumeroUnicoCongeladoID.Value).Select(x => x.Codigo) : ctx.NumeroUnico.Where(x => x.NumeroUnicoID == left.NumeroUnicoDespachadoID.Value).Select(x => x.Codigo)
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spoolID"></param>
        /// <returns></returns>
        public List<GrdCongeladoParcial> obtenerListadoCongeladoParcial(int spoolID)
        {
            IQueryable<MaterialSpool> materiales = null;
            List<GrdCongeladoParcial> lst = null;

            using (SamContext ctx = new SamContext())
            {
                string spoolcode = ctx.Spool.Where(x => x.SpoolID == spoolID).Select(x => x.Nombre).SingleOrDefault().ToString();

                materiales = ctx.MaterialSpool.Where(x => x.SpoolID == spoolID).AsQueryable();

                lst = (from mat in materiales
                       join ic in ctx.ItemCode
                       on mat.ItemCodeID equals ic.ItemCodeID
                       join cp in ctx.CongeladoParcial
                       on mat.MaterialSpoolID equals cp.MaterialSpoolID into leftcp
                       from left in leftcp.DefaultIfEmpty()
                       select new GrdCongeladoParcial
                       {
                           SpoolNombre = spoolcode,
                           MaterialSpoolID = mat.MaterialSpoolID,
                           ItemCode = ic.Codigo,
                           ItemCodeID = ic.ItemCodeID,
                           Descripcion = ic.DescripcionEspanol,
                           D1 = mat.Diametro1,
                           D2 = mat.Diametro2,
                           Cantidad = mat.Cantidad,
                           Etiqueta = mat.Etiqueta,
                           Categoria = mat.Grupo,
                           Congelado = left.NumeroUnicoCongeladoID != null ? ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == left.NumeroUnicoCongeladoID).Any() ? ctx.NumeroUnico.Where(x => x.NumeroUnicoID == left.NumeroUnicoCongeladoID).Select(x => x.Codigo).FirstOrDefault() + "-" + left.SegmentoCongelado : ctx.NumeroUnico.Where(x => x.NumeroUnicoID == left.NumeroUnicoCongeladoID).Select(x => x.Codigo).FirstOrDefault() : null,
                           Especificacion = mat.Especificacion

                       }).ToList();
                return lst;
            }
        }

        /// <summary>
        /// Valida que en caso de que si el número único seleccionado en el radcombo se repita
        /// exista inventario disponible para congelar
        /// </summary>
        /// <param name="numerounico"></param>
        /// <returns></returns>
        public bool validaCantidadesDisponibles(List<GrdCongeladoParcial> numerounico)
        {
            if (numerounico.Count > 0)
            {
                bool valido = true;
                int[,] numerosunicos = new int[numerounico.Count, 2];
                int contador = 0;
                using (SamContext ctx = new SamContext())
                {
                    foreach (GrdCongeladoParcial registro in numerounico)
                    {
                        for (int i = 0; i < contador; i++)
                        {
                            if (registro.NumeroUnico == numerosunicos[i, 0])
                            {
                                int cantidadRestante = ctx.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == registro.NumeroUnico).Select(x => x.InventarioDisponibleCruce).SingleOrDefault() - (numerosunicos[i, 1] + registro.Cantidad);
                                if (cantidadRestante < 0)
                                {
                                    valido = false;
                                    throw new ExcepcionCantidades(MensajesError.Excepcion_CantidadesCongeladosParcial);
                                }
                            }
                        }
                        if (valido)
                        {
                            numerosunicos[contador, 0] = registro.NumeroUnico;
                            numerosunicos[contador, 1] = registro.Cantidad;
                            contador++;
                        }
                        else
                            throw new ExcepcionCantidades(MensajesError.Excepcion_CantidadesCongeladosParcial);

                    }
                    return valido;
                }
            }
            else
            {
                throw new ExcepcionCantidades(MensajesError.Excepcion_SeleccionarCongeladoParcial);
            }
        }

        /// <summary>
        /// Agregamos un nuevo registro a la tabla Congelado Parcial y afectamos inventario
        /// </summary>
        /// <param name="lstGrid"></param>
        public void nuevoCongeladoParcial(List<GrdCongeladoParcial> lstGrid, Guid userID)
        {
            using (TransactionScope t = new TransactionScope())
            {
                using (SamContext ctx = new SamContext())
                {
                    foreach (GrdCongeladoParcial registro in lstGrid)
                    {
                        //checamos si el número único a congelar ya está en la tabla de congelado parcial
                        if (ctx.CongeladoParcial.Where(x => x.MaterialSpoolID == registro.MaterialSpoolID).Any())
                        {
                            CongeladoParcial congPar = ctx.CongeladoParcial.Where(x => x.MaterialSpoolID == registro.MaterialSpoolID).SingleOrDefault();
                            int numeroUnicoAntiguo = congPar.NumeroUnicoCongeladoID;
                            string segmentoAntiguo = congPar.SegmentoCongelado;

                            NumeroUnicoInventario numUnicInvAnt = ctx.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == numeroUnicoAntiguo).SingleOrDefault();
                            numUnicInvAnt.InventarioDisponibleCruce = numUnicInvAnt.InventarioDisponibleCruce + registro.Cantidad;
                            numUnicInvAnt.InventarioCongelado = numUnicInvAnt.InventarioCongelado - registro.Cantidad;
                            ctx.NumeroUnicoInventario.ApplyChanges(numUnicInvAnt);

                            NumeroUnicoInventario numUnicInv = ctx.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == registro.NumeroUnico).SingleOrDefault();
                            numUnicInv.InventarioDisponibleCruce = numUnicInv.InventarioDisponibleCruce - registro.Cantidad;
                            numUnicInv.InventarioCongelado = numUnicInv.InventarioCongelado + registro.Cantidad;
                            ctx.NumeroUnicoInventario.ApplyChanges(numUnicInv);

                            int itemcode = ctx.MaterialSpool.Where(x => x.MaterialSpoolID == registro.MaterialSpoolID).Select(x => x.ItemCodeID).SingleOrDefault();
                            int itemcodeequivalente = ctx.NumeroUnico.Where(x => x.NumeroUnicoID == registro.NumeroUnico).Select(x => x.ItemCodeID.Value).SingleOrDefault();
                            congPar.NumeroUnicoCongeladoID = registro.NumeroUnico;
                            congPar.EsEquivalente = esEquivalente(itemcode, itemcodeequivalente);
                            if (ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == registro.NumeroUnico).Any())
                            {
                                string segmento = registro.Descripcion.Substring(registro.Descripcion.Length - 1);

                                NumeroUnicoSegmento nusAnt = ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == numeroUnicoAntiguo && x.Segmento == segmentoAntiguo).SingleOrDefault();
                                nusAnt.InventarioDisponibleCruce = nusAnt.InventarioDisponibleCruce + registro.Cantidad;
                                nusAnt.InventarioCongelado = nusAnt.InventarioCongelado - registro.Cantidad;
                                ctx.NumeroUnicoSegmento.ApplyChanges(nusAnt);

                                NumeroUnicoSegmento nus = ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == registro.NumeroUnico && x.Segmento == segmento).SingleOrDefault();
                                nus.InventarioDisponibleCruce = nus.InventarioDisponibleCruce - registro.Cantidad;
                                nus.InventarioCongelado = nus.InventarioCongelado + registro.Cantidad;
                                ctx.NumeroUnicoSegmento.ApplyChanges(nus);

                                congPar.SegmentoCongelado = segmento;
                            }
                            congPar.UsuarioModifica = userID;
                            congPar.FechaModificacion = DateTime.Now;
                            ctx.CongeladoParcial.ApplyChanges(congPar);
                        }
                        else
                        {
                            CongeladoParcial congPar = new CongeladoParcial();
                            NumeroUnicoInventario numUnicInv = ctx.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == registro.NumeroUnico).SingleOrDefault();
                            numUnicInv.InventarioDisponibleCruce = numUnicInv.InventarioDisponibleCruce - registro.Cantidad;
                            numUnicInv.InventarioCongelado = numUnicInv.InventarioCongelado + registro.Cantidad;
                            ctx.NumeroUnicoInventario.ApplyChanges(numUnicInv);

                            int itemcode = ctx.MaterialSpool.Where(x => x.MaterialSpoolID == registro.MaterialSpoolID).Select(x => x.ItemCodeID).SingleOrDefault();
                            int itemcodeequivalente = ctx.NumeroUnico.Where(x => x.NumeroUnicoID == registro.NumeroUnico).Select(x => x.ItemCodeID.Value).SingleOrDefault();
                            congPar.MaterialSpoolID = registro.MaterialSpoolID;
                            congPar.NumeroUnicoCongeladoID = registro.NumeroUnico;
                            congPar.EsEquivalente = esEquivalente(itemcode, itemcodeequivalente);
                            if (ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == registro.NumeroUnico).Any())
                            {
                                string segmento = registro.Descripcion.Substring(registro.Descripcion.Length - 1);
                                NumeroUnicoSegmento nus = ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == registro.NumeroUnico && x.Segmento == segmento).SingleOrDefault();
                                nus.InventarioDisponibleCruce = nus.InventarioDisponibleCruce - registro.Cantidad;
                                nus.InventarioCongelado = nus.InventarioCongelado + registro.Cantidad;
                                ctx.NumeroUnicoSegmento.ApplyChanges(nus);
                                congPar.SegmentoCongelado = segmento;
                            }
                            congPar.UsuarioModifica = userID;
                            congPar.FechaModificacion = DateTime.Now;
                            ctx.CongeladoParcial.AddObject(congPar);
                        }
                    }
                    ctx.SaveChanges();
                }
                t.Complete();
            }
        }

        public bool esEquivalente(int itemCode1, int itemCode2)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ItemCodeEquivalente.Where(x => x.ItemCodeID == itemCode1 && x.ItemCodeEquivalenteID == itemCode2).Any();
            }
        }

        public void eliminarCongeladoParcial(int materialSpoolID)
        {
            int cantidad = 0;
            using (TransactionScope t = new TransactionScope())
            {
                using (SamContext ctx = new SamContext())
                {
                    cantidad = ctx.MaterialSpool.Where(x => x.MaterialSpoolID == materialSpoolID).Select(x => x.Cantidad).SingleOrDefault();
                    CongeladoParcial cp = ctx.CongeladoParcial.Where(x => x.MaterialSpoolID == materialSpoolID).SingleOrDefault();
                    NumeroUnicoInventario numUnico = ctx.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == cp.NumeroUnicoCongeladoID).SingleOrDefault();
                    numUnico.InventarioDisponibleCruce = numUnico.InventarioDisponibleCruce + cantidad;
                    numUnico.InventarioCongelado = numUnico.InventarioCongelado - cantidad;
                    ctx.NumeroUnicoInventario.ApplyChanges(numUnico);
                    if (ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == cp.NumeroUnicoCongeladoID).Any())
                    {
                        NumeroUnicoSegmento numUnicSeg = ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == cp.NumeroUnicoCongeladoID && x.Segmento == cp.SegmentoCongelado).SingleOrDefault();
                        numUnicSeg.InventarioDisponibleCruce = numUnicSeg.InventarioDisponibleCruce + cantidad;
                        numUnicSeg.InventarioCongelado = numUnicSeg.InventarioCongelado - cantidad;
                        ctx.NumeroUnicoSegmento.ApplyChanges(numUnicSeg);
                    }
                    ctx.CongeladoParcial.DeleteObject(cp);
                    ctx.SaveChanges();
                }
                t.Complete();
            }
        }
    }
}
