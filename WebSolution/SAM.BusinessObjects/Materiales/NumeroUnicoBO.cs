using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.Entities.Grid;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Catalogos;
using System.Transactions;
using SAM.BusinessObjects.Proyectos;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.RadCombo;
using SAM.Entities.Personalizadas;
using System.Data.Objects;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Validations;
using System.Collections;



namespace SAM.BusinessObjects.Materiales
{
    public class NumeroUnicoBO
    {
        private static readonly object _mutex = new object();
        private static NumeroUnicoBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private NumeroUnicoBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase NumerosUnicosBO
        /// </summary>
        /// <returns></returns>
        public static NumeroUnicoBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new NumeroUnicoBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtiene la información de un número unico
        /// </summary>
        /// <param name="numeroUnicoID">Numero Unico ID</param>
        /// <returns></returns>
        public NumeroUnico Obtener(int numeroUnicoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.NumeroUnico.Where(x => x.NumeroUnicoID == numeroUnicoID).SingleOrDefault();
            }
        }

        public NumeroUnico ObtenerConItemCOde(int numeroUnicoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.NumeroUnico.Include("ItemCode").Where(x => x.NumeroUnicoID == numeroUnicoID).SingleOrDefault();
            }
        }

        /// <summary>
        /// Obtiene la información del número único y de su recepción
        /// </summary>
        /// <param name="numeroUnicoID"></param>
        /// <returns></returns>
        public NumeroUnico ObtenerConRecepcion(int numeroUnicoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.NumeroUnico.Include("RecepcionNumeroUnico")
                                      .Include("RecepcionNumeroUnico.Recepcion")
                                      .Include("RecepcionNumeroUnico.Recepcion.Transportista").Where(x => x.NumeroUnicoID == numeroUnicoID).SingleOrDefault();
            }
        }

        /// <summary>
        /// Obtiene la información del número unico y de sus inventarios en caso de tenerlos
        /// </summary>
        /// <param name="numeroUnicoID"></param>
        /// <returns></returns>
        public NumeroUnico ObtenerConInventarios(int numeroUnicoID)
        {
            using (SamContext ctx = new SamContext())
            {
                NumeroUnico numUnico = ObtenerConInventarios(ctx, numeroUnicoID);
                return numUnico;

            }
        }

        /// <summary>
        ///  Obtiene la información del número unico y de sus inventarios en caso de tenerlos
        /// </summary>
        /// <param name="ctx">Contexto</param>
        /// <param name="numeroUnicoID">ID del numero unico</param>
        /// <returns></returns>
        public NumeroUnico ObtenerConInventarios(SamContext ctx, int numeroUnicoID)
        {
            NumeroUnico numUnico = ctx.NumeroUnico
                                      .Include("RecepcionNumeroUnico")
                                      .Include("NumeroUnicoInventario")
                                      .Include("NumeroUnicoMovimiento")
                                      .Include("NumeroUnicoSegmento")
                                      .Include("ItemCode")
                                      .Where(x => x.NumeroUnicoID == numeroUnicoID)
                                      .SingleOrDefault();

            ctx.TipoCorte.Where(x => x.TipoCorteID == numUnico.TipoCorte1ID || x.TipoCorteID == numUnico.TipoCorte2ID);
            ctx.ItemCode.Where(x => x.ItemCodeID == numUnico.ItemCodeID);

            return numUnico;
        }



        /// <summary>
        /// Obtiene la información del número unico y los movimientos de inventario
        /// </summary>
        /// <param name="numeroUnicoID"></param>
        /// <returns></returns>
        public NumeroUnico ObtenerMovimientosInventarios(int numeroUnicoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.NumeroUnico.Include("NumeroUnicoSegmento")
                                      .Include("NumeroUnicoMovimiento")
                                      .Include("NumeroUnicoMovimiento.TipoMovimiento").Where(x => x.NumeroUnicoID == numeroUnicoID).SingleOrDefault();

            }
        }

        /// <summary>
        /// Obtiene la informacion del numero unico con sus segmentos y el itemcode
        /// </summary>
        /// <param name="numeroUnicoID"></param>
        /// <returns></returns>
        public NumeroUnico ObtenerParaMovimientoInventarios(int numeroUnicoID)
        {
            using (SamContext ctx = new SamContext())
            {
                NumeroUnico nu = ctx.NumeroUnico.Include("NumeroUnicoSegmento")
                                      .Include("NumeroUnicoInventario")
                                      .Include("NumeroUnicoMovimiento")
                                      .Include("ItemCode").Where(x => x.NumeroUnicoID == numeroUnicoID).SingleOrDefault();

                if (nu.ItemCode != null)
                {
                    return nu;
                }
                else
                {
                    throw new ExcepcionConcordancia(MensajesError.Excepcion_NumeroUnicoSinInventario);
                }

            }
        }

        public NumeroUnico ObtenerParaSegmentarTubo(int numeroUnicoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.NumeroUnico.Include("NumeroUnicoSegmento")
                                      .Include("NumeroUnicoInventario")
                                      .Include("ItemCode").Where(x => x.NumeroUnicoID == numeroUnicoID).SingleOrDefault();

            }
        }

        /// <summary>
        /// Obtiene el número unico y su registro de la tabla NumeroUnicoInventario
        /// </summary>
        /// <param name="numeroUnicoID"></param>
        /// <returns></returns>
        public NumeroUnico ObtenerConInventarioColadaICProfile(int numeroUnicoID)
        {
            using (SamContext ctx = new SamContext())
            {
                NumeroUnico nu = ctx.NumeroUnico
                                    .Include("NumeroUnicoInventario")
                                    .Include("Colada")
                                    .Include("ItemCode")
                                    .Include("TipoCorte")
                                    .Include("TipoCorte1")
                                    .Include("Despacho")
                                    .Include("NumeroUnicoMovimiento")
                                    .Include("NumeroUnicoMovimiento.TipoMovimiento")
                                    .Where(x => x.NumeroUnicoID == numeroUnicoID)
                                    .SingleOrDefault();

                ctx.LoadProperty<NumeroUnico>(nu, x => x.NumeroUnicoSegmento);

                return nu;
            }
        }

        /// <summary>
        /// Obtiene el numero unico y su registro de la tabla ItemCode
        /// </summary>
        /// <param name="numeroUnicoID"></param>
        /// <returns></returns>
        public NumeroUnico ObtenerConIC(int numeroUnicoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.NumeroUnico.Include("ItemCode").Where(x => x.NumeroUnicoID == numeroUnicoID).SingleOrDefault();
            }
        }

        /// <summary>
        /// Obtiene el número único con sus registros de la tabla numerounicocorte y el detalle del item code
        /// </summary>
        /// <param name="numeroUnicoID">ID del número unico a regresar</param>
        /// <returns>NumeroUnico</returns>
        /// <returns>NumeroUnico.NumeroUnicoCorte</returns>
        /// <returns>NumeroUnico.ItemCode</returns>
        public NumeroUnico ObtenerConTransferenciaCorteIC(int numeroUnicoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.NumeroUnico.Include("NumeroUnicoCorte")
                                      .Include("ItemCode").Where(x => x.NumeroUnicoID == numeroUnicoID).SingleOrDefault();
            }
        }

        /// <summary>
        /// Obtiene el listado de los números unicos de la recepcion en especifico
        /// </summary>
        /// <param name="recepcionID"></param>
        /// <returns></returns>
        public List<GrdNumerosUnicos> ObtenerNumerosUnicosPorRecepcionID(int recepcionID)
        {
            List<GrdNumerosUnicos> lst;
            List<Segmentos> segmentos = null;

            using (SamContext ctx = new SamContext())
            {
                ctx.NumeroUnico.MergeOption = MergeOption.NoTracking;
                ctx.ItemCode.MergeOption = MergeOption.NoTracking;
                ctx.NumeroUnicoInventario.MergeOption = MergeOption.NoTracking;
                ctx.Colada.MergeOption = MergeOption.NoTracking;
                ctx.RecepcionNumeroUnico.MergeOption = MergeOption.NoTracking;

                lst = (from nu in ctx.NumeroUnico
                       let ic = nu.ItemCode
                       let nui = nu.NumeroUnicoInventario
                       let colada = nu.Colada
                       where ctx.RecepcionNumeroUnico
                                   .Where(x => x.RecepcionID == recepcionID)
                                   .Select(x => x.NumeroUnicoID)
                                   .Contains(nu.NumeroUnicoID)
                       select new GrdNumerosUnicos
                       {
                           NumeroUnicoID = nu.NumeroUnicoID,
                           NumeroUnico = nu.Codigo,
                           ItemCode = ic.Codigo,
                           Descripcion = ic.DescripcionEspanol,
                           Diametro1 = nu.Diametro1,
                           Diametro2 = nu.Diametro2,
                           Factura = nu.Factura,
                           PartidaFactura = nu.PartidaFactura,
                           OrdenCompra = nu.OrdenDeCompra,
                           PartidaOrden = nu.PartidaOrdenDeCompra,
                           NumeroColada = colada.NumeroColada,
                           Certificado = colada.NumeroCertificado,
                           TotalRecibida = nui.CantidadRecibida,
                           TotalBuenEstado = nui.InventarioBuenEstado,
                           TotalDanana = nui.CantidadDanada,
                           Cedula = nu.Cedula,
                           ColadaAceroID = colada.AceroID,
                           Profile1ID = nu.TipoCorte1ID,
                           Profile2ID = nu.TipoCorte2ID,
                           ProveedorID = nu.ProveedorID,
                           FabricanteID = nu.FabricanteID,
                           Rack = nu.Rack,
                           Observaciones = nu.Observaciones
                       }).ToList();

                //traer los racks de cada segmento
                IQueryable<int> qNus = lst.Select(x => x.NumeroUnicoID).AsQueryable();

                segmentos = ctx.NumeroUnicoSegmento
                                .Where(x => qNus.Contains(x.NumeroUnicoID))
                                .Select(x => new Segmentos
                                {
                                    NumeroUnicoID = x.NumeroUnicoID,
                                    Segmento = x.Segmento,
                                    Rack = x.Rack
                                })
                                .ToList();

            }

            Dictionary<int, string> dicProv = CacheCatalogos.Instance.ObtenerProveedores().ToDictionary(x => x.ID, x => x.Nombre);
            Dictionary<int, string> dicFabr = CacheCatalogos.Instance.ObtenerFabricantes().ToDictionary(x => x.ID, x => x.Nombre); ;
            Dictionary<int, string> dicTipc = CacheCatalogos.Instance.ObtenerTiposCorte().ToDictionary(x => x.ID, x => x.Nombre); ;
            Dictionary<int, string> dicAceros = CacheCatalogos.Instance.ObtenerAceros().ToDictionary(x => x.ID, x => x.Nombre); ;

            lst.ForEach(x =>
            {
                x.AceroNomenclatura = x.ColadaAceroID.HasValue ? dicAceros[x.ColadaAceroID.Value] : string.Empty;
                x.Profile1 = x.Profile1ID.HasValue ? dicTipc[x.Profile1ID.Value] : string.Empty;
                x.Profile2 = x.Profile2ID.HasValue ? dicTipc[x.Profile2ID.Value] : string.Empty;
                x.Proveedor = x.ProveedorID.HasValue ? dicProv[x.ProveedorID.Value] : string.Empty;
                x.Fabricante = x.FabricanteID.HasValue ? dicFabr[x.FabricanteID.Value] : string.Empty;
                x.RackDisplay = obtenRackDisplay(x.NumeroUnicoID, x.Rack, segmentos);
            });

            return lst;
        }

        /// <summary>
        /// Obtiene el listado de numeros unicos para Requisitar Pintura
        /// </summary>
        /// <param name="numeroUnicoID">ID del Numero Unico</param>
        /// <returns></returns>
        public List<GrdReqPinturaNumUnico> ObtenerParaReqPinturaNumUnico(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.NumeroUnico.MergeOption = MergeOption.NoTracking;

                List<GrdReqPinturaNumUnico> lst = (from nu in ctx.NumeroUnico
                                                   join ic in ctx.ItemCode on nu.ItemCodeID equals ic.ItemCodeID
                                                   join nui in ctx.NumeroUnicoInventario on nu.NumeroUnicoID equals nui.NumeroUnicoID
                                                   where !ctx.RequisicionNumeroUnicoDetalle.Select(x => x.NumeroUnicoID).Contains(nu.NumeroUnicoID)
                                                         && nu.ProyectoID == proyectoID
                                                   select new GrdReqPinturaNumUnico
                                                   {
                                                       NumeroUnicoID = nu.NumeroUnicoID,
                                                       NumeroUnico = nu.Codigo,
                                                       ItemCode = ic.Codigo,
                                                       Descripcion = ic.DescripcionEspanol,
                                                       Diametro1 = nu.Diametro1,
                                                       Diametro2 = nu.Diametro2,
                                                       Fisico = nui.InventarioFisico,
                                                       Recibido = nui.CantidadRecibida
                                                   }).ToList();
                return lst;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reqNumUnicoID"></param>
        /// <returns></returns>
        public List<GrdDetReqPinturaNumUnico> ObtenerDetalleReqPinturaNumUnicos(int reqNumUnicoID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<RequisicionNumeroUnico> iqReNumUnico = ctx.RequisicionNumeroUnico.Where(x => x.RequisicionNumeroUnicoID == reqNumUnicoID);
                IQueryable<RequisicionNumeroUnicoDetalle> iqReNumUnicoDet = ctx.RequisicionNumeroUnicoDetalle.Where(x => x.RequisicionNumeroUnicoID == reqNumUnicoID);
                IQueryable<NumeroUnico> iqNumUnicos = ctx.NumeroUnico.Where(x => iqReNumUnicoDet.Select(y => y.NumeroUnicoID).Contains(x.NumeroUnicoID));
                IQueryable<ItemCode> iqItemCode = ctx.ItemCode.Where(x => iqNumUnicos.Select(y => y.ItemCodeID).Contains(x.ItemCodeID));

                List<GrdDetReqPinturaNumUnico> lst = (from rnu in iqReNumUnico.ToList()
                                                      join rnud in iqReNumUnicoDet.ToList() on rnu.RequisicionNumeroUnicoID equals rnud.RequisicionNumeroUnicoID
                                                      join nu in iqNumUnicos.ToList() on rnud.NumeroUnicoID equals nu.NumeroUnicoID
                                                      join ic in iqItemCode.ToList() on nu.ItemCodeID equals ic.ItemCodeID
                                                      select new GrdDetReqPinturaNumUnico
                                                      {
                                                          RequisicionNumeroUnicoDetalleID = rnud.RequisicionNumeroUnicoDetalleID,
                                                          NumeroUnico = nu.Codigo,
                                                          ItemCode = ic.Codigo,
                                                          Descripcion = ic.DescripcionEspanol,
                                                          Diametro1 = nu.Diametro1,
                                                          Diametro2 = nu.Diametro2,
                                                      }).ToList();
                return lst;
            }
        }

        public List<GrdPinturaNumUnico> ObtenerParaPinturaNumUnico(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                //Ontener datos a partir del proyecto
                IQueryable<NumeroUnico> iqNumUnicos = ctx.NumeroUnico.Where(x => x.ProyectoID == proyectoID);
                IQueryable<RequisicionNumeroUnicoDetalle> iqReqNumUnicoDet = ctx.RequisicionNumeroUnicoDetalle.Where(x => iqNumUnicos.Select(y => y.NumeroUnicoID).Contains(x.NumeroUnicoID));
                IQueryable<RequisicionNumeroUnico> iqReqNumUnico = ctx.RequisicionNumeroUnico.Where(x => iqReqNumUnicoDet.Select(y => y.RequisicionNumeroUnicoID).Contains(x.RequisicionNumeroUnicoID));
                IQueryable<ItemCode> iqItemcode = ctx.ItemCode.Where(x => iqNumUnicos.Select(y => y.ItemCodeID).Contains(x.ItemCodeID));
                IQueryable<PinturaNumeroUnico> iqPintura = ctx.PinturaNumeroUnico.Where(x => iqReqNumUnicoDet.Select(y => y.RequisicionNumeroUnicoDetalleID).Contains(x.RequisicionNumeroUnicoDetalleID));

                List<GrdPinturaNumUnico> lst = (from nu in iqNumUnicos
                                                join rnud in iqReqNumUnicoDet on nu.NumeroUnicoID equals rnud.NumeroUnicoID
                                                join rnu in iqReqNumUnico on rnud.RequisicionNumeroUnicoID equals rnu.RequisicionNumeroUnicoID
                                                join ic in iqItemcode on nu.ItemCodeID equals ic.ItemCodeID
                                                join pnu in iqPintura on rnud.RequisicionNumeroUnicoDetalleID equals pnu.RequisicionNumeroUnicoDetalleID into detalle
                                                from dpnu in detalle.DefaultIfEmpty()
                                                select new GrdPinturaNumUnico
                                                {
                                                    NumeroUnicoID = nu.NumeroUnicoID,
                                                    NumeroRequisicion = rnu.NumeroRequisicion,
                                                    FechaRequisicion = rnu.FechaRequisicion,
                                                    NumeroUnico = nu.Codigo,
                                                    ItemCode = ic.Codigo,
                                                    Descripcion = ic.DescripcionEspanol,
                                                    Liberado = (dpnu != null) ? dpnu.Liberado : false,
                                                    FechaPrimarios = dpnu.FechaPrimarios,
                                                    ReportePrimarios = (dpnu != null) ? dpnu.ReportePrimarios : string.Empty,
                                                    FechaIntermedio = dpnu.FechaIntermedio,
                                                    ReporteIntermedio = (dpnu != null) ? dpnu.ReporteIntermedio : string.Empty
                                                }).ToList();

                return lst;
            }
        }

        /// <summary>
        /// Obtiene los números únicos para la impresion de etiquetas
        /// </summary>
        /// <param name="RecepcionID"></param>
        /// <returns></returns>
        public List<NumeroUnico> ObtenerPorRecepcionIDEtiquetas(int recepcionID)
        {

            using (SamContext ctx = new SamContext())
            {
                return ctx.NumeroUnico.Include("Proyecto")
                                      .Include("RecepcionNumeroUnico")
                                      .Include("ItemCode")
                                      .Include("Colada").Where(x => x.RecepcionNumeroUnico.Select(y => y.RecepcionID).Contains(recepcionID)).ToList();
            }
        }

        /// <summary>
        /// Obtiene el registro de NumeroUnicoCorte que aun se encuentra en transferencia
        /// </summary>
        /// <param name="numeroUnicoID">ID del numero unico para el que se desea obtener el corte</param>
        /// <param name="segmento">Segmento del numero unico para el que se desea obtener el corte</param>
        /// <returns></returns>
        public NumeroUnicoCorte ObtenCorteEnTransferencia(int numeroUnicoID, string segmento)
        {
            using (SamContext ctx = new SamContext())
            {
                return ObtenCorteEnTransferencia(ctx, numeroUnicoID, segmento);
            }
        }

        /// <summary>
        /// Obtiene el registro de NumeroUnicoCorte que aun se encuentra en transferencia
        /// </summary>
        /// <param name="ctx">Contexto</param>
        /// <param name="numeroUnicoID">ID del numero unico para el que se desea obtener el corte</param>
        /// <param name="segmento">Segmento del numero unico para el que se desea obtener el corte</param>
        /// <returns></returns>
        public NumeroUnicoCorte ObtenCorteEnTransferencia(SamContext ctx, int numeroUnicoID, string segmento)
        {
            return ctx.NumeroUnicoCorte.Include("UbicacionFisica").Where(x => x.NumeroUnicoID == numeroUnicoID && x.Segmento == segmento && !x.TieneCorte).SingleOrDefault();
        }


        /// <summary>
        /// Genera los números únicos correspondientes a una recepción que ya existe.
        /// </summary>
        /// <param name="recepcion"></param>
        /// <param name="numerosUnicos"></param>
        public void GeneraNumerosUnicos(int recepcionID, List<NumeroUnico> numerosUnicos, int nuevoConsecutivo)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    NumeroUnicoBO.Instance.GenerarNumerosUnicos(ctx, recepcionID, numerosUnicos);
                    ProyectoBO.Instance.ActualizaConsecutivoNumeroUnicos(ctx, numerosUnicos[0].ProyectoID, nuevoConsecutivo, numerosUnicos[0].UsuarioModifica.Value);

                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }



        /// <summary>
        /// Genera en base de datos los números únicos y su relación con la recepción.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="numerosUnicos"></param>
        public void GenerarNumerosUnicos(SamContext ctx, int recepcionID, List<NumeroUnico> numerosUnicos)
        {
            foreach (NumeroUnico num in numerosUnicos)
            {
                num.RecepcionNumeroUnico.Add(new RecepcionNumeroUnico());
                num.RecepcionNumeroUnico[0].RecepcionID = recepcionID;
                num.RecepcionNumeroUnico[0].UsuarioModifica = num.UsuarioModifica;
                num.RecepcionNumeroUnico[0].FechaModificacion = DateTime.Now;
                ctx.NumeroUnico.ApplyChanges(num);
                ctx.RecepcionNumeroUnico.ApplyChanges(num.RecepcionNumeroUnico[0]);
            }
        }


        /// <summary>
        /// Guarda los cambios a la entidad numero unico
        /// Crea un nuevo registro en numerounicoinventario
        /// Crea un nuevo registro en numerounicomovimiento (recepcion)
        /// Crea un nuevo registro en numerounicosegmento, si tipomaterial = tubo.
        /// </summary>
        /// <param name="numUnico"></param>
        public void Guarda(NumeroUnico numUnico, Guid userID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    //El diámetro 1 no puede ser menor o igual a cero y debe existir en el catálogo
                    if (numUnico.Diametro1 <= 0 || !ctx.Diametro.Any(x => x.Valor == numUnico.Diametro1))
                    {
                        throw new ExcepcionConcordancia(MensajesError.Excepcion_DiametroNoExiste);
                    }

                    //El diámetro 2 SÍ puede ser cero, en caso de ser diferente de cero debe existir en el catálogo
                    if (numUnico.Diametro2 != 0 && !ctx.Diametro.Any(x => x.Valor == numUnico.Diametro2))
                    {
                        throw new ExcepcionConcordancia(MensajesError.Excepcion_DiametroNoExiste);
                    }

                    int? itemcodeID = ctx.NumeroUnico.Where(x => x.NumeroUnicoID == numUnico.NumeroUnicoID).Select(x => x.ItemCodeID).SingleOrDefault();

                    if (!PuedeModificarDatosBase(numUnico.NumeroUnicoID) && (itemcodeID.Value != numUnico.ItemCodeID))
                    {
                        NumeroUnicoMovimiento movimiento = new NumeroUnicoMovimiento
                            {
                                NumeroUnicoID = numUnico.NumeroUnicoID,
                                ProyectoID= numUnico.ProyectoID,
                                TipoMovimientoID = (int)TipoMovimientoEnum.CambioItemCode,
                                Cantidad = numUnico.NumeroUnicoInventario.CantidadRecibida,
                                FechaMovimiento = DateTime.Now,
                                UsuarioModifica = userID,
                                Estatus = EstatusNumeroUnicoMovimiento.ACTIVO
                            };
                        ctx.NumeroUnicoMovimiento.ApplyChanges(movimiento);
                    }

                    ctx.NumeroUnico.ApplyChanges(numUnico);
                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        public void ValidaNoTengaDespacho(int CantidadDespacho)
        {
            if (CantidadDespacho > 0)
            {
                throw new ExcepcionCantidades(MensajesError.Excepcion_CantidadDespacho);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numeroUnicoSegmentoIDs"></param>
        /// <param name="userID"></param>
        /// <param name="odtID"></param>
        /// <param name="ubicacionFisicaID"></param>
        public void TransfiereACorte(int[] numeroUnicoSegmentoIDs, Guid userID, int odtID, int ubicacionFisicaID)
        {
            using (SamContext ctx = new SamContext())
            {

                IQueryable<int> nuSids = numeroUnicoSegmentoIDs.AsQueryable();

                //Obtenemos el segmento a tranferir
                List<NumeroUnicoSegmento> segmentos = ctx.NumeroUnicoSegmento
                                                         .Include("NumeroUnico")
                                                         .Include("NumeroUnico.NumeroUnicoInventario")
                                                         .Where(x => nuSids.Contains(x.NumeroUnicoSegmentoID))
                                                         .ToList();

                foreach (NumeroUnicoSegmento segmento in segmentos)
                {
                    IQueryable<NumeroUnicoCorte> nuc = ctx.NumeroUnicoCorte.Where(x => x.NumeroUnicoID == segmento.NumeroUnicoID);

                    if (nuc != null && segmento.InventarioFisico > 0)
                    {
                        //Generamos el registro del movimiento en el inventario
                        NumeroUnicoMovimiento salida = new NumeroUnicoMovimiento
                        {
                            Cantidad = segmento.InventarioFisico,
                            Estatus = EstatusNumeroUnicoMovimiento.ACTIVO,
                            FechaModificacion = DateTime.Now,
                            FechaMovimiento = DateTime.Now,
                            NumeroUnicoID = segmento.NumeroUnicoID,
                            ProyectoID = segmento.ProyectoID,
                            Segmento = segmento.Segmento,
                            TipoMovimientoID = (int)TipoMovimientoEnum.DespachoACorte,
                            UsuarioModifica = userID
                        };

                        ctx.NumeroUnicoMovimiento.ApplyChanges(salida);

                        //Generamos el registro de la transferencia a corte
                        NumeroUnicoCorte corte = new NumeroUnicoCorte
                        {
                            NumeroUnicoID = segmento.NumeroUnicoID,
                            ProyectoID = segmento.ProyectoID,
                            OrdenTrabajoID = odtID,
                            Segmento = segmento.Segmento,
                            Longitud = segmento.InventarioFisico,
                            NumeroUnicoMovimiento = salida,
                            FechaTraspaso = DateTime.Now,
                            FechaModificacion = DateTime.Now,
                            TieneCorte = false,
                            UbicacionFisicaID = ubicacionFisicaID,
                            UsuarioModifica = userID
                        };

                        ctx.NumeroUnicoCorte.ApplyChanges(corte);

                        //Actualizamos el inventario del numero unico
                        segmento.NumeroUnico.NumeroUnicoInventario.StartTracking();
                        segmento.NumeroUnico.NumeroUnicoInventario.InventarioTransferenciaCorte = segmento.NumeroUnico.NumeroUnicoInventario.InventarioTransferenciaCorte + segmento.InventarioFisico;
                        segmento.NumeroUnico.NumeroUnicoInventario.InventarioFisico = segmento.NumeroUnico.NumeroUnicoInventario.InventarioFisico - segmento.InventarioFisico;
                        //Se desea que en la transferencia a corte no se pierda el inventario disponible a cruce
                        //segmento.NumeroUnico.NumeroUnicoInventario.InventarioBuenEstado = segmento.NumeroUnico.NumeroUnicoInventario.InventarioBuenEstado - segmento.InventarioBuenEstado;
                        //segmento.NumeroUnico.NumeroUnicoInventario.InventarioDisponibleCruce = segmento.NumeroUnico.NumeroUnicoInventario.InventarioBuenEstado - segmento.InventarioCongelado;
                        segmento.NumeroUnico.NumeroUnicoInventario.FechaModificacion = DateTime.Now;
                        segmento.NumeroUnico.NumeroUnicoInventario.UsuarioModifica = userID;
                        segmento.NumeroUnico.NumeroUnicoInventario.StopTracking();
                        ctx.NumeroUnicoInventario.ApplyChanges(segmento.NumeroUnico.NumeroUnicoInventario);

                        //Actualizamos el inventario del segmento
                        segmento.StartTracking();
                        segmento.InventarioTransferenciaCorte = segmento.InventarioFisico;
                        segmento.InventarioFisico = 0;
                        //Se desea que en la transferencia a corte no se pierda el inventario disponible a cruce
                        //segmento.InventarioBuenEstado = 0;
                        //segmento.InventarioDisponibleCruce = segmento.InventarioBuenEstado - segmento.InventarioCongelado;
                        segmento.FechaModificacion = DateTime.Now;
                        segmento.UsuarioModifica = userID;
                        segmento.StopTracking();
                        ctx.NumeroUnicoSegmento.ApplyChanges(segmento);
                    }                                        
                }
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Valida que el numero inicial con el que se desean dar de alta los números unicos sea mayor al ultimo almacenado en la base de datos o que en el gap existan suficientes numeros.
        /// </summary>
        /// <param name="numeroInicial">Numero inicial con el que se desean dar de alta los números unicos</param>
        /// <param name="cantidadNumerosUnicos">Cantidad de numeros unicos a generar</param>
        /// <param name="proyectoID">ID del proyecto</param>
        /// <returns></returns>
        public bool ValidaNumeroInicial(int numeroInicial, int cantidadNumerosUnicos, int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {

                int consecutivoProyecto =
                    ctx.ProyectoConsecutivo.Single(x => x.ProyectoID == proyectoID).ConsecutivoNumeroUnico;
                if (numeroInicial > consecutivoProyecto)
                {
                    return true;
                }

                ProyectoConfiguracion proyConfig = ctx.ProyectoConfiguracion.Single(x => x.ProyectoID == proyectoID);
                string prefijo = proyConfig.PrefijoNumeroUnico;
                int digitosNumUnico = proyConfig.DigitosNumeroUnico;

                List<string> numerosUnicos = new List<string>();

                for (int i = 0; i < cantidadNumerosUnicos; i++)
                {
                    numerosUnicos.Add(string.Format("{0}-{1}", prefijo,
                                                    (numeroInicial + i).ToString().PadLeft(digitosNumUnico, '0')));
                }


                if (ctx.NumeroUnico.Where(x => x.ProyectoID == proyectoID).ToList().Any(x => numerosUnicos.Contains(x.Codigo)))
                {
                    //Gap no completa
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_NumUnicosInsuficientesConsecutivos);
                }

                return true;
            }
        }

        /// <summary>
        /// Funcion para extraer NumeroUnico con su requisicion
        /// </summary>
        /// <param name="numUnicoID">NumeroUnicoID</param>
        /// <returns></returns>
        public NumeroUnico ObtenerConReqNumUnicoDetalle(int numUnicoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.NumeroUnico.Include("RequisicionNumeroUnicoDetalle").Where(x => x.NumeroUnicoID == numUnicoID).Single();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="nombreColada"></param>
        /// <param name="itemcode"></param>
        /// <param name="numUnicoInicial"></param>
        /// <param name="numeroUnicoFinal"></param>
        /// <returns></returns>
        public List<GrdNumerosUnicosCompleto> ObtenerNumerosUnicosPorProyecto(int proyectoID, string nombreColada, string itemcode, string numUnicoInicial, string numeroUnicoFinal)
        {
            List<NumeroUnicoCompuesto> numUnico;
            List<Segmentos> segmentos = null;
            Dictionary<int, NumeroUnico> numeros;

            using (SamContext ctx = new SamContext())
            {
                numUnico = ctx.ObtenerNumerosUnicosCompuesto(proyectoID,
                                                                nombreColada,
                                                                itemcode,
                                                                numUnicoInicial,
                                                                numeroUnicoFinal).ToList();

                segmentos = ctx.NumeroUnicoSegmento
                            .Where(x => x.NumeroUnico.ProyectoID == proyectoID)
                            .Select(x => new Segmentos
                            {
                                NumeroUnicoID = x.NumeroUnicoID,
                                Segmento = x.Segmento,
                                Rack = x.Rack
                            })
                            .ToList();
                numeros = ctx.NumeroUnico.Where(x => x.ProyectoID == proyectoID).ToDictionary(x => x.NumeroUnicoID);
            }

            Dictionary<int, string> trans = CacheCatalogos.Instance.ObtenerTransportistas().ToDictionary(x => x.ID, y => y.Nombre);
            Dictionary<int, string> tipoCorte = CacheCatalogos.Instance.ObtenerTiposCorte().ToDictionary(x => x.ID, y => y.Nombre);
            Dictionary<int, string> prov = CacheCatalogos.Instance.ObtenerProveedores().ToDictionary(x => x.ID, y => y.Nombre);
            Dictionary<int, string> fab = CacheCatalogos.Instance.ObtenerFabricantes().ToDictionary(x => x.ID, y => y.Nombre);
            Dictionary<int, string> acero = CacheCatalogos.Instance.ObtenerAceros().ToDictionary(x => x.ID, y => y.Nombre);

            List<GrdNumerosUnicosCompleto> grd =
                (from nu in numUnico
                 let numeroUnico = numeros[nu.NumeroUnicoID]
                 select new GrdNumerosUnicosCompleto
                 {                     
                     FechaRecepcion = nu.FechaRecepcion.Date,
                     NumeroUnicoID = nu.NumeroUnicoID,
                     NumeroUnico = nu.Codigo,
                     ItemCode = nu.CodigoItemCode,
                     TipoMaterial = TraductorEnumeraciones.TextoTipoMaterial(nu.TipoMaterialID.Value),
                     Descripcion = nu.DescripcionEspanol,
                     Diametro1 = nu.Diametro1,
                     Diametro2 = nu.Diametro2,
                     Factura = nu.Factura,
                     PartidaFactura = nu.PartidaFactura,
                     OrdenCompra = nu.OrdenDeCompra,
                     PartidaOrden = nu.PartidaOrdenDeCompra,
                     NumeroColada = nu.NumeroColada,
                     Certificado = nu.NumeroCertificado,
                     AceroNomenclatura = nu.AceroID.HasValue ? acero[nu.AceroID.Value] : string.Empty,
                     TotalRecibida = nu.CantidadRecibida,
                     TotalOtrasEntradas = nu.TotalEntradaOtrosProcesos,
                     TotalInventarioFisico = nu.InventarioFisico,
                     TotalDanada = nu.CantidadDanada,
                     TotalCondicionada = nu.Estatus == EstatusNumeroUnico.CONDICIONADO ? nu.CantidadRecibida : 0,
                     TotalRechazada = nu.Estatus == EstatusNumeroUnico.RECHAZADO ? nu.CantidadRecibida : 0,
                     TotalRecibidoNeto = nu.CantidadRecibida + nu.TotalEntradaOtrosProcesos - nu.CantidadDanada,
                     TotalSalidasTemporales = nu.TotalSalidasTemporales - nu.TotalEntradasTemporales,
                     TotalOtrasSalidas = nu.TotalOtrasSalidas,
                     Cedula = nu.Cedula,
                     Profile1 = nu.TipoCorte1ID.HasValue ? tipoCorte[nu.TipoCorte1ID.Value] : string.Empty,
                     Profile2 = nu.TipoCorte2ID.HasValue ? tipoCorte[nu.TipoCorte2ID.Value] : string.Empty,
                     Proveedor = nu.ProveedorID.HasValue ? prov[nu.ProveedorID.Value] : string.Empty,
                     Fabricante = nu.FabricanteID.HasValue ? fab[nu.FabricanteID.Value] : string.Empty,
                     Transportista = trans[nu.TransportistaID],
                     MarcadoAsme = nu.MarcadoAsme,
                     MarcadoGolpe = nu.MarcadoGolpe,
                     MarcadoPintura = nu.MarcadoPintura,
                     PruebasHidrostaticas = nu.PruebasHidrostaticas,
                     TotalCorteSinDespachada = nu.TotalCorte - nu.TotalDespachado < 0 ? 0 : nu.TotalCorte - nu.TotalDespachado,
                     TotalDespachada = nu.TotalDespachado - nu.TotalDespachadoParaICE,
                     TotalDespachadaICE = nu.TotalDespachadoParaICE,
                     TotalMerma = nu.TotalMerma,
                     TotalEnTransferencia = nu.InventarioTransferenciaCorte,
                     InventarioCongelado = nu.InventarioCongelado,
                     InventarioDisponibleCruce = nu.InventarioDisponibleCruce < 0 ? 0 : nu.InventarioDisponibleCruce,
                     TotalInventarioActual = nu.InventarioBuenEstado,
                     Estatus = TraductorEnumeraciones.TextoEstatusNumeroUnico(nu.Estatus),
                     Observaciones = nu.Observaciones,
                     RackDisplay = obtenRackDisplay(nu.NumeroUnicoID, nu.Rack, segmentos),
                     CampoLibre1 = numeroUnico.CampoLibreRecepcion1,
                     CampoLibre2 = numeroUnico.CampoLibreRecepcion2,
                     CampoLibre3 = numeroUnico.CampoLibreRecepcion3,
                     CampoLibre4 = numeroUnico.CampoLibreRecepcion4,
                     CampoLibre5 = numeroUnico.CampoLibreRecepcion5,
                     CampoLibre6 = numeroUnico.CampoLibre1,
                     CampoLibre7 = numeroUnico.CampoLibre2,
                     CampoLibre8 = numeroUnico.CampoLibre3,
                     CampoLibre9 = numeroUnico.CampoLibre4,
                     CampoLibre10 = numeroUnico.CampoLibre5
                 }).AsParallel().ToList();

            return grd;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="nui"></param>
        /// <param name="rack"></param>
        /// <param name="segmentos"></param>
        /// <returns></returns>
        private string obtenRackDisplay(int nui, string rack, List<Segmentos> segmentos)
        {
            if (segmentos.Where(x => x.NumeroUnicoID == nui).Any())
            {
                List<Segmentos> segmentosNu = segmentos.Where(x => x.NumeroUnicoID == nui).ToList();

                if (segmentosNu.Where(x => x.NumeroUnicoID == nui).Count() > 1)
                {
                    return string.Join(", ", segmentosNu.Select(x => x.Segmento + ": " + (x.Rack)));
                }

                return segmentosNu.Select(x => x.Rack).SingleOrDefault();
            }
            else
            {
                return rack;
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordenTrabajoID"></param>
        /// <param name="ordenTrabajoSpoolID"></param>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public List<NumeroUnicoSegmento> ObtenerNumerosUnicosATransferir(int ordenTrabajoID, int ordenTrabajoSpoolID, int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                bool esAsignacion = false;

                if (ordenTrabajoID > 0)
                {
                    esAsignacion = ctx.OrdenTrabajo
                                      .Where(x => x.OrdenTrabajoID == ordenTrabajoID)
                                      .Select(y => y.EsAsignado)
                                      .Single();
                }
                else if (ordenTrabajoSpoolID > 0)
                {
                    esAsignacion = ctx.OrdenTrabajoSpool
                                      .Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID)
                                      .Select(x => x.EsAsignado)
                                      .Single();
                }

                List<NumeroUnicoSegmento> lstSegmento;

                if (!esAsignacion)
                {
                    lstSegmento = NumerosUnicosATransferirSinAsignacion(ordenTrabajoID, ordenTrabajoSpoolID, proyectoID, ctx);
                }
                else
                {
                    lstSegmento = NumerosUnicosATransferirConAsignacion(ordenTrabajoID, ordenTrabajoSpoolID, proyectoID, ctx);
                }


                return lstSegmento;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordenTrabajoID"></param>
        /// <param name="ordenTrabajoSpoolID"></param>
        /// <param name="proyectoID"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public List<NumeroUnicoSegmento> NumerosUnicosATransferirConAsignacion(int ordenTrabajoID, int ordenTrabajoSpoolID, int proyectoID, SamContext ctx)
        {
            IQueryable<OrdenTrabajoMaterial> iqOdtm;

            if (ordenTrabajoSpoolID > 0)
            {
                iqOdtm =
                    ctx.OrdenTrabajoMaterial
                       .Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID);
            }
            else
            {
                iqOdtm =
                    ctx.OrdenTrabajoMaterial
                       .Where(x => x.OrdenTrabajoSpool.OrdenTrabajo.OrdenTrabajoID == ordenTrabajoID);
            }

            List<NumeroUnicoSegmento> lstSegmento =
                ctx.NumeroUnicoSegmento
                   .Include("NumeroUnico")
                //Que sean explícitamente los asignados
                   .Where(x => iqOdtm.Select(y => new Simple { ID = (int)y.NumeroUnicoAsignadoID, Valor = y.SegmentoAsignado })
                                     .Contains(new Simple { ID = x.NumeroUnicoID, Valor = x.Segmento }))
                //Que no se encuentren en transferencia a corte
                   .Where(x => !ctx.NumeroUnicoCorte
                                   .Where(y => y.ProyectoID == proyectoID && !y.TieneCorte)
                                   .Select(y => new Simple { ID = y.NumeroUnicoID, Valor = y.Segmento })
                                   .Contains(new Simple { ID = x.NumeroUnicoID, Valor = x.Segmento }))
                   .ToList();

            IQueryable<int> iqIcIds = lstSegmento.Select(x => x.NumeroUnico.ItemCodeID.GetValueOrDefault(-1))
                                                 .Distinct()
                                                 .AsQueryable();

            ctx.ItemCode.Where(x => iqIcIds.Contains(x.ItemCodeID)).ToList();

            IQueryable<int> iqColadaIds = lstSegmento.Select(x => x.NumeroUnico.ColadaID.GetValueOrDefault(-1))
                                                     .Distinct()
                                                     .AsQueryable();

            ctx.Colada.Where(x => iqColadaIds.Contains(x.ColadaID)).ToList();

            //Ordenar la lista
            lstSegmento = lstSegmento.OrderBy(x => x.NumeroUnico.Codigo)
                                     .ThenBy(x => x.Segmento)
                                     .ToList();

            return lstSegmento;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordenTrabajoID"></param>
        /// <param name="ordenTrabajoSpoolID"></param>
        /// <param name="proyectoID"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public List<NumeroUnicoSegmento> NumerosUnicosATransferirSinAsignacion(int ordenTrabajoID, int ordenTrabajoSpoolID, int proyectoID, SamContext ctx)
        {
            //Obtengo los itemcodes de ingeniería pertenecientes a tubos
            IQueryable<ItemCodeIntegrado> itemCodes = from ic in ctx.ItemCode
                                                      join mat in ctx.MaterialSpool on ic.ItemCodeID equals mat.ItemCodeID
                                                      join odtM in ctx.OrdenTrabajoMaterial on mat.MaterialSpoolID equals odtM.MaterialSpoolID
                                                      join odtS in ctx.OrdenTrabajoSpool on odtM.OrdenTrabajoSpoolID equals odtS.OrdenTrabajoSpoolID
                                                      where odtS.OrdenTrabajoID == ordenTrabajoID
                                                             && (ordenTrabajoSpoolID == -1 || odtS.OrdenTrabajoSpoolID == ordenTrabajoSpoolID)
                                                             && ic.TipoMaterialID == (int)TipoMaterialEnum.Tubo
                                                             && (odtM.TieneCorte == null || !odtM.TieneCorte.Value)
                                                             && !odtM.TieneDespacho
                                                      select new ItemCodeIntegrado
                                                      {
                                                          ItemCodeID = ic.ItemCodeID,
                                                          Diametro1 = mat.Diametro1,
                                                          Diametro2 = mat.Diametro2
                                                      };

            //Obtengo los itemcodes equivalentes de los itemcodes de ingenieria
            IQueryable<ItemCodeIntegrado> icEquivalentes = from iceq in ctx.ItemCodeEquivalente
                                                           where itemCodes.Contains(new ItemCodeIntegrado
                                                           {
                                                               ItemCodeID = iceq.ItemCodeID,
                                                               Diametro1 = iceq.Diametro1,
                                                               Diametro2 = iceq.Diametro2
                                                           })
                                                           select new ItemCodeIntegrado
                                                           {
                                                               ItemCodeID = iceq.ItemEquivalenteID,
                                                               Diametro1 = iceq.DiametroEquivalente1,
                                                               Diametro2 = iceq.DiametroEquivalente2
                                                           };

            //Obtengo los segmentos candidatos
            IQueryable<NumeroUnicoSegmento> segmento = from seg in ctx.NumeroUnicoSegmento
                                                       join nu in ctx.NumeroUnico on seg.NumeroUnicoID equals nu.NumeroUnicoID
                                                       where nu.Estatus == EstatusNumeroUnico.APROBADO
                                                       && (itemCodes.Contains(new ItemCodeIntegrado
                                                       {
                                                           ItemCodeID = nu.ItemCodeID.Value,
                                                           Diametro1 = nu.Diametro1,
                                                           Diametro2 = nu.Diametro2
                                                       }) || icEquivalentes.Contains(new ItemCodeIntegrado
                                                       {
                                                           ItemCodeID = nu.ItemCodeID.Value,
                                                           Diametro1 = nu.Diametro1,
                                                           Diametro2 = nu.Diametro2
                                                       }))
                                                       select seg;

            //Obtengo los segmentos que ya hayan sido transferidos y no hayan sido cortados
            IQueryable<Simple> corte = from nuCorte in ctx.NumeroUnicoCorte
                                       where nuCorte.ProyectoID == proyectoID
                                       && !nuCorte.TieneCorte
                                       select
                                       new Simple { ID = nuCorte.NumeroUnicoID, Valor = nuCorte.Segmento };

            //Obtengo los segmentos candidatos y que no se encuentren en la lista de transferidos.
            segmento = from seg in segmento
                       where !(from c in corte
                               select c)
                               .Contains(new Simple { ID = seg.NumeroUnicoID, Valor = seg.Segmento })
                       select seg;

            ctx.ItemCode.Where(x => itemCodes.Select(y => y.ItemCodeID).Contains(x.ItemCodeID) || icEquivalentes.Select(y => y.ItemCodeID).Contains(x.ItemCodeID)).ToList();
            ctx.Colada.Where(x => x.ProyectoID == proyectoID).ToList();

            List<NumeroUnicoSegmento> lstSegmento = ctx.NumeroUnicoSegmento
                                                       .Include("NumeroUnico")
                                                       .Where(x => segmento.Select(y => y.NumeroUnicoSegmentoID)
                                                                           .Contains(x.NumeroUnicoSegmentoID))
                                                       .OrderBy(x => x.NumeroUnico.Codigo)
                                                       .ThenBy(x => x.Segmento)
                                                       .ToList();
            return lstSegmento;
        }

        /// <summary>
        /// Obtiene los números únicos que se encuentran en transferencia a corte
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="ordenTrabajoID"></param>
        /// <param name="numeroUnicoID"></param>
        /// <returns></returns>
        public List<GrdNumerosUnicosEnCorte> ObtenerNumerosUnicosEnTransferencia(int proyectoID, int ordenTrabajoID, int numeroUnicoID)
        {
            using (SamContext ctx = new SamContext())
            {
                //Obtengo los segmentos que se encuentren en transferencia (corte = 0) que sean parte del proyecto, del numero unico y de la orden de trabajo recibidos 
                IQueryable<Simple> enCorte = from corte in ctx.NumeroUnicoCorte
                                             where corte.ProyectoID == proyectoID
                                             && (numeroUnicoID == -1 || corte.NumeroUnicoID == numeroUnicoID)
                                             && (ordenTrabajoID == -1 || corte.OrdenTrabajoID == ordenTrabajoID)
                                             && !corte.TieneCorte
                                             select (new Simple
                                             {
                                                 ID = corte.NumeroUnicoID,
                                                 Valor = corte.Segmento
                                             });




                //Regreso los segmentos con sus detalles 
                List<GrdNumerosUnicosEnCorte> segmentosEnCorte = new List<GrdNumerosUnicosEnCorte>();

                List<NumeroUnicoSegmento> segmentos = ctx.NumeroUnicoSegmento.Include("NumeroUnico")
                                              .Include("NumeroUnico.ItemCode")
                                              .Include("NumeroUnico.Colada")
                                              .Include("NumeroUnico.NumeroUnicoCorte")
                                              .Include("NumeroUnico.NumeroUnicoCorte.OrdenTrabajo")
                                              .Where(x => enCorte.Contains(new Simple { ID = x.NumeroUnicoID, Valor = x.Segmento })
                                             ).ToList();

                segmentos.ForEach(x =>
                {
                    GrdNumerosUnicosEnCorte nu = new GrdNumerosUnicosEnCorte();

                    nu.NumeroUnicoSegmentoID = x.NumeroUnicoSegmentoID;
                    nu.NumeroUnicoID = x.NumeroUnicoID;
                    nu.ItemCode = x.NumeroUnico.ItemCode.Codigo;
                    nu.Diametro1 = x.NumeroUnico.Diametro1;
                    nu.Diametro2 = x.NumeroUnico.Diametro2;
                    nu.Descripcion = x.NumeroUnico.ItemCode.DescripcionEspanol;
                    nu.Cantidad = x.NumeroUnico.NumeroUnicoCorte.Where(y => !y.TieneCorte && y.Segmento == x.Segmento).Select(y => y.Longitud).FirstOrDefault();
                    nu.FechaTraspaso = x.NumeroUnico.NumeroUnicoCorte.Where(y => !y.TieneCorte).Select(y => y.FechaTraspaso).FirstOrDefault();
                    nu.NumeroColada = x.NumeroUnico.Colada.NumeroColada;
                    nu.Cedula = x.NumeroUnico.Cedula;
                    nu.NumeroUnico = x.NumeroUnico.Codigo;
                    nu.Segmento = x.Segmento;
                    nu.OrdenDeTrabajo = x.NumeroUnico.NumeroUnicoCorte.Where(y => !y.TieneCorte && y.Segmento == x.Segmento).Select(y => y.OrdenTrabajo.NumeroOrden).FirstOrDefault();

                    segmentosEnCorte.Add(nu);
                });

                return segmentosEnCorte;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="materialSpoolID"></param>
        /// <param name="filtro"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public IEnumerable<RadNumeroUnicoParaDespacho> AccesoriosAfinesParaDespachoOAsignacion(int materialSpoolID, string filtro, int skip, int take, bool esParaCandidatosAsignacion)
        {
            MaterialSpool ms;
            List<NumeroUnico> lstNu;
            bool esDespachoPorAsignacion = false;
            bool aunTieneCongelado = false;

            #region Hacer el montonal de queries necesarios

            using (SamContext ctx = new SamContext())
            {
                ms = ctx.MaterialSpool
                        .Where(x => x.MaterialSpoolID == materialSpoolID)
                        .Single();

                if (!esParaCandidatosAsignacion)
                {
                    ctx.LoadProperty<MaterialSpool>(ms, x => x.OrdenTrabajoMaterial);
                    esDespachoPorAsignacion = ms.OrdenTrabajoMaterial[0].EsAsignado;
                    aunTieneCongelado = ms.OrdenTrabajoMaterial[0].TieneInventarioCongelado;
                }

                #region Manejar el caso donde el despacho es para una asignación

                //Es importante que sólo se haga en caso de que aún haya inventarion congelado, sino
                //puede ser que haya sido cancelado porque ya no había inventario y entonces
                //hay que dejarlos seleccionar manualmente
                if (esDespachoPorAsignacion && aunTieneCongelado)
                {
                    //En este caso ya en realidad no tenemos que calcuar nada simplemente hay que regresar
                    //el n.u. que ya viene en la asignación, debe ser uno solo.
                    OrdenTrabajoMaterial odtm = ms.OrdenTrabajoMaterial[0];

                    NumeroUnico nu =
                        ctx.NumeroUnico
                           .Include("NumeroUnicoInventario")
                           .Include("ItemCode")
                           .Where(x => x.NumeroUnicoID == odtm.NumeroUnicoAsignadoID)
                           .Single();

                    RadNumeroUnicoParaDespacho nud = new RadNumeroUnicoParaDespacho();

                    nud.CodigoItemCode = nu.ItemCode.Codigo;
                    nud.CodigoNumeroUnico = nu.Codigo;
                    nud.DescripcionItemCode = nu.ItemCode.DescripcionEspanol;
                    nud.Diametro1 = nu.Diametro1;
                    nud.Diametro2 = nu.Diametro2;
                    nud.EsEquivalente = nu.ItemCodeID != ms.ItemCodeID || nu.Diametro1 != ms.Diametro1 || nu.Diametro2 != ms.Diametro2;
                    nud.InventarioBuenEstado = nu.NumeroUnicoInventario.InventarioBuenEstado;
                    nud.ItemCodeID = nu.ItemCodeID.Value;
                    nud.NumeroUnicoID = nu.NumeroUnicoID;
                    nud.IndicadorEsEquivalente = nud.EsEquivalente ? "*" : string.Empty;
                    nud.Segmento = string.Empty; //es accesorio asi que no hay segmento

                    //En caso de ser asignación, no hay sugerido
                    nud.EsSugeridoPorCruce = false;

                    return new List<RadNumeroUnicoParaDespacho> { nud };
                }

                #endregion

                IQueryable<ItemCodeIntegrado> icEquivalentes =
                    ctx.ItemCodeEquivalente
                       .Where(eq => eq.ItemCodeID == ms.ItemCodeID && eq.Diametro1 == ms.Diametro1 && eq.Diametro2 == ms.Diametro2)
                       .Select(eq => new ItemCodeIntegrado
                       {
                           ItemCodeID = eq.ItemEquivalenteID,
                           Diametro1 = eq.DiametroEquivalente1,
                           Diametro2 = eq.DiametroEquivalente2
                       });


                //Item codes directos solicitados por el material de ingeniería
                IQueryable<NumeroUnico> qNumsUnicos =
                    ctx.NumeroUnico.Include("Colada")
                       .Where(x => x.ItemCodeID == ms.ItemCodeID && x.Diametro1 == ms.Diametro1 && x.Diametro2 == ms.Diametro2);


                IQueryable<NumeroUnico> qNumsEquivalentes =
                    ctx.NumeroUnico.Include("Colada")
                       .Where(x => icEquivalentes.Contains(new ItemCodeIntegrado
                                                           {
                                                               ItemCodeID = x.ItemCodeID.Value,
                                                               Diametro1 = x.Diametro1,
                                                               Diametro2 = x.Diametro2
                                                           }));

                //Traer al contexto los inventarios de los numeros unicos que hacen match
                ctx.NumeroUnicoInventario
                   .Where(nui => qNumsUnicos.Select(x => x.NumeroUnicoID).Contains(nui.NumeroUnicoID) || qNumsEquivalentes.Select(x => x.NumeroUnicoID).Contains(nui.NumeroUnicoID))
                   .ToList();

                List<NumeroUnico> nus = qNumsUnicos.ToList();
                List<NumeroUnico> nusEquivalentes = qNumsEquivalentes.ToList();

                lstNu = nus.Union(nusEquivalentes).ToList();

                if (lstNu.Count > 0)
                {
                    //Traer todas las coladas del proyecto
                    int proyectoID = lstNu[0].ProyectoID;
                    ctx.Colada.Where(x => x.ProyectoID == proyectoID).ToList();

                    //Los item codes necesarios
                    ctx.ItemCode
                       .Where(ic => qNumsUnicos.Select(x => x.ItemCodeID).Contains(ic.ItemCodeID)
                                    ||
                                    qNumsEquivalentes.Select(x => x.ItemCodeID).Contains(ic.ItemCodeID))
                       .ToList();
                }
            }

            #endregion

            #region Trabajar con la lista de memoria y filtrar/paginar como sea necesario

            if (lstNu.Count > 0)
            {
                //Asegurarnos de ya una vez en memoria quitar todo lo que no nos sirve
                lstNu = lstNu.Where(x => !x.Colada.HoldCalidad)
                             .Where(x => x.Estatus.EqualsIgnoreCase(EstatusNumeroUnico.APROBADO))
                             .Where(x => x.NumeroUnicoInventario.InventarioBuenEstado >= ms.Cantidad)
                             .Distinct()
                             .ToList();

                if (!string.IsNullOrEmpty(filtro))
                {
                    lstNu =
                    lstNu.Where(x => x.ItemCode.DescripcionEspanol.ContainsIgnoreCase(filtro)
                                     ||
                                     x.ItemCode.Codigo.ContainsIgnoreCase(filtro)
                                     ||
                                     x.Codigo.ContainsIgnoreCase(filtro)
                                )
                         .ToList();
                }

                //Ordenar por los que tengan menos inventario primero
                lstNu = lstNu.OrderBy(x => x.NumeroUnicoInventario.InventarioBuenEstado)
                             .Skip(skip)
                             .Take(take)
                             .ToList();
            }

            #endregion

            #region Convertir a entidad personalizada para el RadComboBox

            List<RadNumeroUnicoParaDespacho> lst = new List<RadNumeroUnicoParaDespacho>();

            lstNu.ForEach(x =>
            {
                RadNumeroUnicoParaDespacho nud = new RadNumeroUnicoParaDespacho();

                nud.CodigoItemCode = x.ItemCode.Codigo;
                nud.CodigoNumeroUnico = x.Codigo;
                nud.DescripcionItemCode = x.ItemCode.DescripcionEspanol;
                nud.Diametro1 = x.Diametro1;
                nud.Diametro2 = x.Diametro2;
                nud.EsEquivalente = x.ItemCodeID != ms.ItemCodeID || x.Diametro1 != ms.Diametro1 || x.Diametro2 != ms.Diametro2;
                nud.InventarioBuenEstado = x.NumeroUnicoInventario.InventarioBuenEstado;
                nud.ItemCodeID = x.ItemCodeID.Value;
                nud.NumeroUnicoID = x.NumeroUnicoID;
                nud.IndicadorEsEquivalente = nud.EsEquivalente ? "*" : string.Empty;
                nud.Segmento = string.Empty; //es accesorio asi que no hay segmento

                //En caso de ser asignación, no hay sugerido
                nud.EsSugeridoPorCruce = esParaCandidatosAsignacion ? false : ms.OrdenTrabajoMaterial[0].NumeroUnicoSugeridoID.HasValue;

                lst.Add(nud);
            });

            #endregion

            if (!esParaCandidatosAsignacion)
            {
                //Si existe alguno sugerido específicamente por el cruce que aparezcan arriba
                return lst.OrderBy(x => x.EsSugeridoPorCruce ? 1 : 0)
                          .OrderBy(x => x.InventarioBuenEstado);
            }
            else
            {
                //Si es para el combo de asignación ordernar por código
                return lst.OrderBy(x => x.CodigoNumeroUnico);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="juntaSpoolId"></param>
        /// <param name="filtro"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="esParaCandidatosAsignacion"></param>
        /// <param name="material">Numero unico 1 o numero unico 2</param>
        /// <returns></returns>
        public IEnumerable<RadNumeroUnicoParaDespacho> AccesoriosAfinesParaDespachoDesdeArmado(int juntaSpoolId, string filtro, int skip, int take, bool esParaCandidatosAsignacion
            , int material = 0)
        {
            MaterialSpool ms;
            List<NumeroUnico> lstNu;
            bool esDespachoPorAsignacion = false;
            bool aunTieneCongelado = false;
            int materialSpoolID = 0;

            #region Hacer el montonal de queries necesarios
            using (SamContext ctx = new SamContext())
            {
                #region Recuperar datos necesarios
                JuntaSpool js = ctx.JuntaSpool.Where(x => x.JuntaSpoolID == juntaSpoolId).SingleOrDefault();
                if (material == 1)
                {
                    int nEtiqueta = 0;
                    string temp = "";
                    if (!Int32.TryParse(js.EtiquetaMaterial1, out nEtiqueta))
                    {
                        materialSpoolID = ctx.MaterialSpool.Where(x => x.SpoolID == js.SpoolID && x.Etiqueta == js.EtiquetaMaterial1).Select(x => x.MaterialSpoolID).SingleOrDefault();
                    }
                    else
                    {
                        temp = nEtiqueta.ToString();
                        materialSpoolID = ctx.MaterialSpool.Where(x => x.SpoolID == js.SpoolID && x.Etiqueta == temp).Select(x => x.MaterialSpoolID).SingleOrDefault();
                    }
                    materialSpoolID = ctx.MaterialSpool.Where(x => x.SpoolID == js.SpoolID && x.Etiqueta == temp).Select(x => x.MaterialSpoolID).SingleOrDefault();
                }
                if (material == 2)
                {
                    int nEtiqueta = 0;
                    string temp = "";
                    if (!Int32.TryParse(js.EtiquetaMaterial2, out nEtiqueta))
                    {
                        materialSpoolID = ctx.MaterialSpool.Where(x => x.SpoolID == js.SpoolID && x.Etiqueta == js.EtiquetaMaterial2).Select(x => x.MaterialSpoolID).SingleOrDefault();
                    }
                    else
                    {
                        temp = nEtiqueta.ToString();
                        materialSpoolID = ctx.MaterialSpool.Where(x => x.SpoolID == js.SpoolID && x.Etiqueta == temp).Select(x => x.MaterialSpoolID).SingleOrDefault();
                    }
                    materialSpoolID = ctx.MaterialSpool.Where(x => x.SpoolID == js.SpoolID && x.Etiqueta == temp).Select(x => x.MaterialSpoolID).SingleOrDefault();
                }
                if (material == 0)
                {
                    return null;
                }
                #endregion
                ms = ctx.MaterialSpool
                        .Where(x => x.MaterialSpoolID == materialSpoolID)
                        .Single();

                if (!esParaCandidatosAsignacion)
                {
                    ctx.LoadProperty<MaterialSpool>(ms, x => x.OrdenTrabajoMaterial);
                    esDespachoPorAsignacion = ms.OrdenTrabajoMaterial[0].EsAsignado;
                    aunTieneCongelado = ms.OrdenTrabajoMaterial[0].TieneInventarioCongelado;
                }

                #region Manejar el caso donde el despacho es para una asignación

                //Es importante que sólo se haga en caso de que aún haya inventarion congelado, sino
                //puede ser que haya sido cancelado porque ya no había inventario y entonces
                //hay que dejarlos seleccionar manualmente
                if (esDespachoPorAsignacion && aunTieneCongelado)
                {
                    //En este caso ya en realidad no tenemos que calcuar nada simplemente hay que regresar
                    //el n.u. que ya viene en la asignación, debe ser uno solo.
                    OrdenTrabajoMaterial odtm = ms.OrdenTrabajoMaterial[0];

                    NumeroUnico nu =
                        ctx.NumeroUnico
                           .Include("NumeroUnicoInventario")
                           .Include("ItemCode")
                           .Where(x => x.NumeroUnicoID == odtm.NumeroUnicoAsignadoID)
                           .Single();

                    RadNumeroUnicoParaDespacho nud = new RadNumeroUnicoParaDespacho();

                    nud.CodigoItemCode = nu.ItemCode.Codigo;
                    nud.CodigoNumeroUnico = nu.Codigo;
                    nud.DescripcionItemCode = nu.ItemCode.DescripcionEspanol;
                    nud.Diametro1 = nu.Diametro1;
                    nud.Diametro2 = nu.Diametro2;
                    nud.EsEquivalente = nu.ItemCodeID != ms.ItemCodeID || nu.Diametro1 != ms.Diametro1 || nu.Diametro2 != ms.Diametro2;
                    nud.InventarioBuenEstado = nu.NumeroUnicoInventario.InventarioBuenEstado;
                    nud.ItemCodeID = nu.ItemCodeID.Value;
                    nud.NumeroUnicoID = nu.NumeroUnicoID;
                    nud.IndicadorEsEquivalente = nud.EsEquivalente ? "*" : string.Empty;
                    nud.Segmento = string.Empty; //es accesorio asi que no hay segmento

                    //En caso de ser asignación, no hay sugerido
                    nud.EsSugeridoPorCruce = false;

                    return new List<RadNumeroUnicoParaDespacho> { nud };
                }

                #endregion

                IQueryable<ItemCodeIntegrado> icEquivalentes =
                    ctx.ItemCodeEquivalente
                       .Where(eq => eq.ItemCodeID == ms.ItemCodeID && eq.Diametro1 == ms.Diametro1 && eq.Diametro2 == ms.Diametro2)
                       .Select(eq => new ItemCodeIntegrado
                       {
                           ItemCodeID = eq.ItemEquivalenteID,
                           Diametro1 = eq.DiametroEquivalente1,
                           Diametro2 = eq.DiametroEquivalente2
                       });


                //Item codes directos solicitados por el material de ingeniería
                IQueryable<NumeroUnico> qNumsUnicos =
                    ctx.NumeroUnico
                       .Where(x => x.ItemCodeID == ms.ItemCodeID && x.Diametro1 == ms.Diametro1 && x.Diametro2 == ms.Diametro2);


                IQueryable<NumeroUnico> qNumsEquivalentes =
                    ctx.NumeroUnico
                       .Where(x => icEquivalentes.Contains(new ItemCodeIntegrado
                       {
                           ItemCodeID = x.ItemCodeID.Value,
                           Diametro1 = x.Diametro1,
                           Diametro2 = x.Diametro2
                       }));

                //Traer al contexto los inventarios de los numeros unicos que hacen match
                ctx.NumeroUnicoInventario
                   .Where(nui => qNumsUnicos.Select(x => x.NumeroUnicoID).Contains(nui.NumeroUnicoID) || qNumsEquivalentes.Select(x => x.NumeroUnicoID).Contains(nui.NumeroUnicoID))
                   .ToList();

                List<NumeroUnico> nus = qNumsUnicos.ToList();
                List<NumeroUnico> nusEquivalentes = qNumsEquivalentes.ToList();

                lstNu = nus.Union(nusEquivalentes).ToList();

                if (lstNu.Count > 0)
                {
                    //Traer todas las coladas del proyecto
                    int proyectoID = lstNu[0].ProyectoID;
                    ctx.Colada.Where(x => x.ProyectoID == proyectoID).ToList();

                    //Los item codes necesarios
                    ctx.ItemCode
                       .Where(ic => qNumsUnicos.Select(x => x.ItemCodeID).Contains(ic.ItemCodeID)
                                    ||
                                    qNumsEquivalentes.Select(x => x.ItemCodeID).Contains(ic.ItemCodeID))
                       .ToList();
                }
            }

            #endregion

            #region Trabajar con la lista de memoria y filtrar/paginar como sea necesario

            if (lstNu.Count > 0)
            {
                //Asegurarnos de ya una vez en memoria quitar todo lo que no nos sirve
                lstNu = lstNu.Where(x => !x.Colada.HoldCalidad)
                             .Where(x => x.Estatus.EqualsIgnoreCase(EstatusNumeroUnico.APROBADO))
                             .Where(x => x.NumeroUnicoInventario.InventarioBuenEstado >= ms.Cantidad)
                             .Distinct()
                             .ToList();

                if (!string.IsNullOrEmpty(filtro))
                {
                    lstNu =
                    lstNu.Where(x => x.ItemCode.DescripcionEspanol.ContainsIgnoreCase(filtro)
                                     ||
                                     x.ItemCode.Codigo.ContainsIgnoreCase(filtro)
                                     ||
                                     x.Codigo.ContainsIgnoreCase(filtro)
                                )
                         .ToList();
                }

                //Ordenar por los que tengan menos inventario primero
                lstNu = lstNu.OrderBy(x => x.NumeroUnicoInventario.InventarioBuenEstado)
                             .Skip(skip)
                             .Take(take)
                             .ToList();
            }

            #endregion

            #region Convertir a entidad personalizada para el RadComboBox

            List<RadNumeroUnicoParaDespacho> lst = new List<RadNumeroUnicoParaDespacho>();

            lstNu.ForEach(x =>
            {
                RadNumeroUnicoParaDespacho nud = new RadNumeroUnicoParaDespacho();

                nud.CodigoItemCode = x.ItemCode.Codigo;
                nud.CodigoNumeroUnico = x.Codigo;
                nud.DescripcionItemCode = x.ItemCode.DescripcionEspanol;
                nud.Diametro1 = x.Diametro1;
                nud.Diametro2 = x.Diametro2;
                nud.EsEquivalente = x.ItemCodeID != ms.ItemCodeID || x.Diametro1 != ms.Diametro1 || x.Diametro2 != ms.Diametro2;
                nud.InventarioBuenEstado = x.NumeroUnicoInventario.InventarioBuenEstado;
                nud.ItemCodeID = x.ItemCodeID.Value;
                nud.NumeroUnicoID = x.NumeroUnicoID;
                nud.IndicadorEsEquivalente = nud.EsEquivalente ? "*" : string.Empty;
                nud.Segmento = string.Empty; //es accesorio asi que no hay segmento

                //En caso de ser asignación, no hay sugerido
                nud.EsSugeridoPorCruce = esParaCandidatosAsignacion ? false : ms.OrdenTrabajoMaterial[0].NumeroUnicoSugeridoID.HasValue;

                lst.Add(nud);
            });

            #endregion

            if (!esParaCandidatosAsignacion)
            {
                //Si existe alguno sugerido específicamente por el cruce que aparezcan arriba
                return lst.OrderBy(x => x.EsSugeridoPorCruce ? 1 : 0)
                          .OrderBy(x => x.InventarioBuenEstado);
            }
            else
            {
                //Si es para el combo de asignación ordernar por código
                return lst.OrderBy(x => x.CodigoNumeroUnico);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="materialSpoolID"></param>
        /// <param name="filtro"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public IEnumerable<RadNumeroUnicoParaDespacho> CandidatosParaAsignacionDeTubo(int materialSpoolID, string filtro, int skip, int take)
        {
            MaterialSpool ms;
            List<NumeroUnico> lstNu;
            List<RadNumeroUnicoParaDespacho> lst = new List<RadNumeroUnicoParaDespacho>();

            #region Hacer el montonal de queries necesarios

            using (SamContext ctx = new SamContext())
            {
                ms = ctx.MaterialSpool
                        .Include("OrdenTrabajoMaterial")
                        .Where(x => x.MaterialSpoolID == materialSpoolID)
                        .Single();

                IQueryable<ItemCodeIntegrado> icEquivalentes =
                    ctx.ItemCodeEquivalente
                       .Where(eq => eq.ItemCodeID == ms.ItemCodeID && eq.Diametro1 == ms.Diametro1 && eq.Diametro2 == ms.Diametro2)
                       .Select(eq => new ItemCodeIntegrado
                       {
                           ItemCodeID = eq.ItemEquivalenteID,
                           Diametro1 = eq.DiametroEquivalente1,
                           Diametro2 = eq.DiametroEquivalente2
                       });


                //Item codes directos solicitados por el material de ingeniería
                IQueryable<NumeroUnico> qNumsUnicos =
                    ctx.NumeroUnico
                       .Where(x => x.ItemCodeID == ms.ItemCodeID && x.Diametro1 == ms.Diametro1 && x.Diametro2 == ms.Diametro2);


                IQueryable<NumeroUnico> qNumsEquivalentes =
                    ctx.NumeroUnico
                       .Where(x => icEquivalentes.Contains(new ItemCodeIntegrado
                       {
                           ItemCodeID = x.ItemCodeID.Value,
                           Diametro1 = x.Diametro1,
                           Diametro2 = x.Diametro2
                       }));

                //Traer al contexto los inventarios de los numeros unicos que hacen match
                ctx.NumeroUnicoInventario
                   .Where(nui => qNumsUnicos.Select(x => x.NumeroUnicoID).Contains(nui.NumeroUnicoID) || qNumsEquivalentes.Select(x => x.NumeroUnicoID).Contains(nui.NumeroUnicoID))
                   .ToList();

                //Traer al contexto los inventarios de los segmentos de números unicos que hacen match
                ctx.NumeroUnicoSegmento
                   .Where(ns => qNumsUnicos.Select(x => x.NumeroUnicoID).Contains(ns.NumeroUnicoID) || qNumsEquivalentes.Select(x => x.NumeroUnicoID).Contains(ns.NumeroUnicoID))
                   .ToList();


                List<NumeroUnico> nus = qNumsUnicos.ToList();
                List<NumeroUnico> nusEquivalentes = qNumsEquivalentes.ToList();

                lstNu = nus.Union(nusEquivalentes).ToList();

                if (lstNu.Count > 0)
                {
                    //Traer todas las coladas del proyecto
                    int proyectoID = lstNu[0].ProyectoID;
                    ctx.Colada.Where(x => x.ProyectoID == proyectoID).ToList();

                    //Los item codes necesarios
                    ctx.ItemCode
                       .Where(ic => qNumsUnicos.Select(x => x.ItemCodeID).Contains(ic.ItemCodeID)
                                    ||
                                    qNumsEquivalentes.Select(x => x.ItemCodeID).Contains(ic.ItemCodeID))
                       .ToList();
                }
            }

            #endregion

            #region Trabajar con la lista de memoria y filtrar/paginar como sea necesario

            if (lstNu.Count > 0)
            {
                var lstSegmentos = (from nu in lstNu
                                    where !nu.Colada.HoldCalidad
                                          && nu.Estatus.EqualsIgnoreCase(EstatusNumeroUnico.APROBADO)
                                          && nu.NumeroUnicoInventario.InventarioBuenEstado >= ms.Cantidad
                                    from s in nu.NumeroUnicoSegmento
                                    where s.InventarioBuenEstado >= ms.Cantidad
                                    select new
                                    {
                                        NumeroUnicoID = nu.NumeroUnicoID,
                                        NumeroUnicoSegmentoID = s.NumeroUnicoSegmentoID,
                                        Codigo = nu.Codigo,
                                        Diametro1 = nu.Diametro1,
                                        Diametro2 = nu.Diametro2,
                                        ItemCode = nu.ItemCode,
                                        Segmento = s.Segmento,
                                        InventarioBuenEstado = s.InventarioBuenEstado
                                    }).ToList();

                if (!string.IsNullOrEmpty(filtro))
                {
                    lstSegmentos =
                        lstSegmentos.Where(x => x.ItemCode.DescripcionEspanol.ContainsIgnoreCase(filtro)
                                         ||
                                         x.ItemCode.Codigo.ContainsIgnoreCase(filtro)
                                         ||
                                         string.Concat(x.Codigo, '-', x.Segmento).ContainsIgnoreCase(filtro)
                                    )
                             .ToList();
                }

                lstSegmentos = lstSegmentos.OrderBy(x => x.Codigo)
                                           .ThenBy(x => x.Segmento)
                                           .Skip(skip)
                                           .Take(take)
                                           .ToList();

                #region Convertir a entidad personalizada para el RadComboBox

                lstSegmentos.ForEach(x =>
                {
                    RadNumeroUnicoParaDespacho nud = new RadNumeroUnicoParaDespacho();

                    nud.CodigoItemCode = x.ItemCode.Codigo;
                    nud.CodigoNumeroUnico = string.Concat(x.Codigo, '-', x.Segmento);
                    nud.DescripcionItemCode = x.ItemCode.DescripcionEspanol;
                    nud.Diametro1 = x.Diametro1;
                    nud.Diametro2 = x.Diametro2;
                    nud.EsEquivalente = x.ItemCode.ItemCodeID != ms.ItemCodeID || x.Diametro1 != ms.Diametro1 || x.Diametro2 != ms.Diametro2;
                    nud.EsSugeridoPorCruce = false; //Aqui aun no hay cruce asi que nada más le ponemos false
                    nud.InventarioBuenEstado = x.InventarioBuenEstado;
                    nud.ItemCodeID = x.ItemCode.ItemCodeID;
                    nud.NumeroUnicoID = x.NumeroUnicoID;
                    nud.IndicadorEsEquivalente = nud.EsEquivalente ? "*" : string.Empty;
                    nud.Segmento = x.Segmento;

                    lst.Add(nud);
                });

                #endregion

            }

            #endregion

            return lst;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="materialSpoolID"></param>
        /// <param name="filtro"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public IEnumerable<RadNumeroUnicoParaDespacho> CandidatosParaAsignacion(int materialSpoolID, string filtro, int skip, int take)
        {
            bool esTubo = false;

            using (SamContext ctx = new SamContext())
            {
                esTubo =
                    ctx.ItemCode.Where(x => x.MaterialSpool
                                             .Where(y => y.MaterialSpoolID == materialSpoolID)
                                             .Select(z => z.ItemCodeID)
                                             .Contains(x.ItemCodeID))
                                .Select(x => x.TipoMaterialID)
                                .Single() != (int)TipoMaterialEnum.Accessorio;
            }

            if (!esTubo)
            {
                return AccesoriosAfinesParaDespachoOAsignacion(materialSpoolID, filtro, skip, take, true);
            }
            else
            {
                return CandidatosParaAsignacionDeTubo(materialSpoolID, filtro, skip, take);
            }
        }

        /// <summary>
        /// Elimina el numero unico de la transferencia a corte sin regresar a inventarios.
        /// </summary>
        /// <param name="recepcionID"></param>
        public void BorraNumeroUnicoDeTransferencia(int numeroUnicoSegmentoID, Guid userUID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    using (SamContext ctx = new SamContext())
                    {
                        //Obtengo el segmento que deseo eliminar
                        NumeroUnicoSegmento segmento = ctx.NumeroUnicoSegmento.Include("NumeroUnico").Include("NumeroUnico.NumeroUnicoInventario").Where(x => x.NumeroUnicoSegmentoID == numeroUnicoSegmentoID).SingleOrDefault();

                        //Obtengo el registro a eliminar en caso de que exista y que este en tranferencia de corte (TieneCorte = 0)
                        NumeroUnicoCorte numCorte = ctx.NumeroUnicoCorte.Where(x => x.NumeroUnicoID == segmento.NumeroUnicoID && x.Segmento == segmento.Segmento && !x.TieneCorte).SingleOrDefault();

                        //Valida que no exista otro corte posterior para el numero unico
                        if ((from dcorte in ctx.NumeroUnicoCorte
                             where dcorte.NumeroUnicoID == numCorte.NumeroUnicoID
                             && dcorte.NumeroUnicoCorteID > numCorte.NumeroUnicoCorteID && dcorte.Segmento == numCorte.Segmento && dcorte.TieneCorte == true
                             select dcorte).Any())
                        {
                            throw new Excepciones.ExcepcionCorte(MensajesError.Excepcion_NumeroUnicoConOtroCorte);
                        }

                        //Valida que no exista otro despacho a corte posterior para el numero unico
                        if ((from dcorte in ctx.NumeroUnicoCorte
                             where dcorte.NumeroUnicoID == numCorte.NumeroUnicoID
                             && dcorte.NumeroUnicoCorteID > numCorte.NumeroUnicoCorteID && dcorte.Segmento == numCorte.Segmento && dcorte.TieneCorte == false
                             select dcorte).Any())
                        {
                            throw new Excepciones.ExcepcionCorte(MensajesError.Excepcion_NumeroUnicoConOtroDespachoCorte);
                        }

                        //Regreso inventarios
                        //Actualizamos el inventario del numero unico
                        segmento.NumeroUnico.NumeroUnicoInventario.StartTracking();
                        segmento.NumeroUnico.NumeroUnicoInventario.InventarioTransferenciaCorte = segmento.NumeroUnico.NumeroUnicoInventario.InventarioTransferenciaCorte - numCorte.Longitud;
                        segmento.NumeroUnico.NumeroUnicoInventario.InventarioFisico = segmento.NumeroUnico.NumeroUnicoInventario.InventarioFisico + numCorte.Longitud;
                        //los inventarios buen estado y disponible cruce ya no se ven afectados en la transferencia a corte.
                        //segmento.NumeroUnico.NumeroUnicoInventario.InventarioBuenEstado = segmento.NumeroUnico.NumeroUnicoInventario.InventarioFisico - segmento.NumeroUnico.NumeroUnicoInventario.CantidadDanada;
                        //segmento.NumeroUnico.NumeroUnicoInventario.InventarioDisponibleCruce = segmento.NumeroUnico.NumeroUnicoInventario.InventarioBuenEstado - segmento.NumeroUnico.NumeroUnicoInventario.InventarioCongelado;
                        segmento.NumeroUnico.NumeroUnicoInventario.FechaModificacion = DateTime.Now;
                        segmento.NumeroUnico.NumeroUnicoInventario.UsuarioModifica = userUID;
                        segmento.NumeroUnico.NumeroUnicoInventario.StopTracking();
                        ctx.NumeroUnicoInventario.ApplyChanges(segmento.NumeroUnico.NumeroUnicoInventario);


                        //Actualizamos el inventario del segmento
                        segmento.StartTracking();
                        segmento.InventarioTransferenciaCorte = 0;
                        segmento.InventarioFisico = segmento.InventarioFisico + numCorte.Longitud;
                        //los inventarios buen estado y disponible cruce ya no se ven afectados en la transferencia a corte.
                        //segmento.InventarioBuenEstado = segmento.InventarioFisico - segmento.CantidadDanada;
                        //segmento.InventarioDisponibleCruce = segmento.InventarioBuenEstado - segmento.InventarioCongelado;
                        segmento.FechaModificacion = DateTime.Now;
                        segmento.UsuarioModifica = userUID;
                        segmento.StopTracking();
                        ctx.NumeroUnicoSegmento.ApplyChanges(segmento);

                        //Anteriormente Generabamos el registro del movimiento en el inventario de cancelacion de despcho a corte,
                        // ahora se cancela el despacho a corte
                        NumeroUnicoMovimiento oMovimiento = (from nnn in ctx.NumeroUnicoMovimiento
                                                             where nnn.TipoMovimientoID == (int)TipoMovimientoEnum.DespachoACorte
                                                             && nnn.Estatus == "A" && nnn.Segmento == segmento.Segmento
                                                             && nnn.NumeroUnicoID == segmento.NumeroUnicoID
                                                             && nnn.Cantidad == numCorte.Longitud
                                                             && nnn.NumeroUnicoMovimientoID == numCorte.SalidaMovimientoID
                                                             select nnn).FirstOrDefault();

                        if (oMovimiento != null)
                        {
                            oMovimiento.StartTracking();
                            oMovimiento.Estatus = "C";
                            oMovimiento.UsuarioModifica = userUID;
                            oMovimiento.FechaModificacion = DateTime.Now;
                            oMovimiento.StopTracking();
                            ctx.NumeroUnicoMovimiento.ApplyChanges(oMovimiento);
                        }

                        //Verifico si el registro fue utilizado en algun corte que ya fue cancelado y borro el corte
                        List<Corte> cortes = ctx.Corte.Include("CorteDetalle").Where(x => x.NumeroUnicoCorteID == numCorte.NumeroUnicoCorteID).ToList();

                        //Elimino el registro  
                        int numCortes = cortes.Count;
                        for (int corteCount = 0; corteCount < numCortes; corteCount++)
                        {
                            int detalles = cortes[corteCount].CorteDetalle.Count;
                            for (int detalleCount = 0; detalleCount < detalles; detalleCount++)
                            {
                                ctx.CorteDetalle.DeleteObject(cortes[corteCount].CorteDetalle[0]);
                            }

                            ctx.Corte.DeleteObject(cortes[corteCount]);
                        }

                        ctx.NumeroUnicoCorte.DeleteObject(numCorte);
                        ctx.SaveChanges();
                    }

                    ts.Complete();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
            catch (Exception ex)
            {
                string x = ex.Message;
                throw ex;
            }
        }

        /// <summary>
        /// Elimina el numero único, sus movimientos, inventarios y segmentos relacionados
        /// </summary>
        /// <param name="numeroUnicoID">ID del numero único a eliminar.</param>
        public void BorraNumeroUnico(int numeroUnicoID)
        {
            using (SamContext ctx = new SamContext())
            {
                //Verifica si el numero unico no ah sido despachado
                IList<Despacho> despachos = (from des in ctx.Despacho where des.NumeroUnicoID == numeroUnicoID select des).ToList();
                if (despachos.Where(x => x.Cancelado == false).Any())
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_NumeroUnicoDespacho);
                }

                //Verifica si el numero unico tiene movimientos de salida que no sea salida a pintura o segmentacion
                if (ctx.NumeroUnicoMovimiento.Where(x => x.NumeroUnicoID == numeroUnicoID && x.Estatus == "A" && x.TipoMovimiento.EsEntrada == false &&
                    (x.TipoMovimientoID != (int)TipoMovimientoEnum.SalidaPintura ||
                     x.TipoMovimientoID != (int)TipoMovimientoEnum.SalidaSegmentacion)).Any())
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_NumeroUnicoConRelaciones);
                }

                //Verifica si el numero unico ha sido utilizado en algun cruce
                if (ctx.OrdenTrabajoMaterial.Where(x => x.NumeroUnicoCongeladoID == numeroUnicoID).Any())
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_NumeroUnicoCongelado);
                }

                NumeroUnico numUnico = ctx.NumeroUnico.Where(x => x.NumeroUnicoID == numeroUnicoID).Single();
                NumeroUnicoInventario inventario = ctx.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == numeroUnicoID).SingleOrDefault();
                IList<NumeroUnicoSegmento> segmento = ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == numeroUnicoID).ToList<NumeroUnicoSegmento>();

                IList<OrdenTrabajoMaterial> ordenMaterial = (from orden in ctx.OrdenTrabajoMaterial
                                                             join corteDet in ctx.CorteDetalle on orden.CorteDetalleID equals corteDet.CorteDetalleID into cortesDetalle
                                                             from t in cortesDetalle.DefaultIfEmpty()
                                                             join Cor in ctx.Corte on t.CorteID equals Cor.CorteID into Cortes
                                                             from t2 in Cortes.DefaultIfEmpty()
                                                             join numeroC in ctx.NumeroUnicoCorte on t2.NumeroUnicoCorteID equals numeroC.NumeroUnicoCorteID into numeroUnicoCorte
                                                             from t3 in numeroUnicoCorte.DefaultIfEmpty()
                                                             where orden.NumeroUnicoAsignadoID == numeroUnicoID || orden.NumeroUnicoSugeridoID == numeroUnicoID || t3.NumeroUnicoID == numeroUnicoID
                                                             select orden).ToList<OrdenTrabajoMaterial>();

                IList<CorteDetalle> cortesDet = (from corteDet in ctx.CorteDetalle
                                                 join Cor in ctx.Corte on corteDet.CorteID equals Cor.CorteID
                                                 join numeroC in ctx.NumeroUnicoCorte on Cor.NumeroUnicoCorteID equals numeroC.NumeroUnicoCorteID
                                                 where numeroC.NumeroUnicoID == numeroUnicoID
                                                 select corteDet).ToList<CorteDetalle>();

                IList<NumeroUnicoCorte> NumeroCortes = ctx.NumeroUnicoCorte.Where(x => x.NumeroUnicoID == numeroUnicoID).ToList<NumeroUnicoCorte>();

                IList<Corte> cortes = (from ocorte in ctx.Corte
                                       join numeroC in ctx.NumeroUnicoCorte on ocorte.NumeroUnicoCorteID equals numeroC.NumeroUnicoCorteID
                                       where numeroC.NumeroUnicoID == numeroUnicoID
                                       select ocorte).ToList<Corte>();

                PinturaNumeroUnico oPintura = ctx.PinturaNumeroUnico.Where(x => x.NumeroUnicoID == numeroUnicoID).SingleOrDefault();
                RequisicionNumeroUnicoDetalle oRequisicion = ctx.RequisicionNumeroUnicoDetalle.Where(x => x.NumeroUnicoID == numeroUnicoID).SingleOrDefault();

                IList<NumeroUnicoMovimiento> movimiento = ctx.NumeroUnicoMovimiento.Where(x => x.NumeroUnicoID == numeroUnicoID).ToList<NumeroUnicoMovimiento>();
                RecepcionNumeroUnico recepcion = ctx.RecepcionNumeroUnico.Where(x => x.NumeroUnicoID == numeroUnicoID).Single();

                ctx.DeleteObject(recepcion);
                foreach (OrdenTrabajoMaterial oOrdenM in ordenMaterial)
                    ctx.DeleteObject(oOrdenM);
                foreach (Despacho oDespacho in despachos)
                    ctx.DeleteObject(oDespacho);
                foreach (CorteDetalle oCorteDet in cortesDet)
                    ctx.DeleteObject(oCorteDet);
                foreach (Corte oCorte in cortes)
                    ctx.DeleteObject(oCorte);
                foreach (Corte oCorte in cortes)
                    ctx.DeleteObject(oCorte);
                if (oPintura != null)
                    ctx.DeleteObject(oPintura);
                if (oRequisicion != null)
                    ctx.DeleteObject(oRequisicion);
                foreach (NumeroUnicoMovimiento oMovimiento in movimiento)
                    ctx.DeleteObject(oMovimiento);
                foreach (NumeroUnicoSegmento oSegmento in segmento)
                    ctx.DeleteObject(oSegmento);
                if (inventario != null)
                    ctx.DeleteObject(inventario);
                ctx.DeleteObject(numUnico);

                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Guarda los movimientos hechos en inventario,
        /// entradas y salidas de Tubos y Accesorios
        /// </summary>
        /// <param name="numUnico"></param>
        /// <param name="segmento"></param>
        public void GuardaMovimientosInventario(NumeroUnico numUnico, NumeroUnicoSegmento segmento)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    //if (ValidacionesNumeroUnicoSegmento.SegmentoExiste(ctx, segmento.Segmento, segmento.NumeroUnicoSegmentoID))
                    //{
                    //    throw new ExcepcionDuplicados(MensajesError.Excepcion_NombreDuplicado);
                    //}

                    ctx.NumeroUnico.ApplyChanges(numUnico);
                    ctx.NumeroUnicoInventario.ApplyChanges(numUnico.NumeroUnicoInventario);
                    if (segmento != null)
                    {
                        ctx.NumeroUnicoSegmento.ApplyChanges(segmento);
                    }
                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }


        public List<NumeroUnico> ObtenerPorProyecto(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.NumeroUnico.MergeOption = MergeOption.NoTracking;

                return ctx.NumeroUnico.Where(x => x.ProyectoID == proyectoID).ToList();
            }
        }

        /// <summary>
        /// Obtiene el listado de numeros unicos por proyecto y que solo sean tubos
        /// </summary>
        /// <param name="proyectoID">ID del proyecto</param>
        /// <param name="itemCode"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public List<Simple> ObtenerSoloTubos(int proyectoID, string itemCode, int skip, int take)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.NumeroUnico.MergeOption = MergeOption.NoTracking;

                List<Simple> numUnico = ctx.NumeroUnico
                                           .Where(x => x.ProyectoID == proyectoID && x.ItemCode.TipoMaterialID == (int)TipoMaterialEnum.Tubo)
                                           .Select(x => new Simple { ID = x.NumeroUnicoID, Valor = x.Codigo })
                                           .ToList();

                return numUnico.Where(x => x.Valor.StartsWith(itemCode, StringComparison.InvariantCultureIgnoreCase))
                               .OrderBy(x => x.Valor)
                               .Skip(skip)
                               .Take(take)
                               .ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordenTrabajoID"></param>
        /// <param name="codigoSegmento"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public IEnumerable<RadNumeroUnico> ListaNumeroUnicoEnTrasferencia(int ordenTrabajoID, string codigoSegmento, int skip, int take)
        {
            List<RadNumeroUnico> result = new List<RadNumeroUnico>(take * 2);

            using (SamContext ctx = new SamContext())
            {
                ctx.NumeroUnicoCorte.MergeOption = MergeOption.NoTracking;
                ctx.NumeroUnico.MergeOption = MergeOption.NoTracking;

                //Obtengo los numeros unicos que se encuentran en transferencia a corte para esa orden de trabajo
                List<NumeroUnicoCorte> numerosUnicos = ctx.NumeroUnicoCorte
                                                          .Include("NumeroUnico")
                                                          .Include("NumeroUnico.ItemCode")
                                                          .Include("NumeroUnico.NumeroUnicoSegmento")
                                                          .Where(x => x.OrdenTrabajoID == ordenTrabajoID && !x.TieneCorte)
                                                          .ToList();

                //Obtenemos los numeros unicos que contienen el item code o equivalentes.
                IEnumerable<RadNumeroUnico> data =
                    (from x in numerosUnicos
                     select new RadNumeroUnico
                     {
                         NumeroUnicoID = x.NumeroUnicoID,
                         Codigo = x.NumeroUnico.Codigo,
                         Segmento = x.Segmento,
                         ItemCode = x.NumeroUnico.ItemCode.Codigo,
                         Diametro1 = x.NumeroUnico.Diametro1,
                         Diametro2 = x.NumeroUnico.Diametro2,
                         InventarioBuenEstado = x.Longitud
                     }).ToList();



                return data.Where(x => x.CodigoSegmento.StartsWith(codigoSegmento, StringComparison.InvariantCultureIgnoreCase)
                    || x.ItemCode.StartsWith(codigoSegmento, StringComparison.InvariantCultureIgnoreCase))
                           .OrderBy(x => x.CodigoSegmento)
                           .Skip(skip)
                           .Take(take);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="codigoNumeroUnicoFiltro"></param>
        /// <param name="esAdministradorSistema"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IEnumerable<Simple> ObtenerNumerosUnicosPorPermiso(int? proyectoID, int skip, int take, string codigoNumeroUnicoFiltro, bool esAdministradorSistema, Guid userID)
        {
            List<Simple> lst;

            using (SamContext ctx = new SamContext())
            {
                ctx.NumeroUnico.MergeOption = MergeOption.NoTracking;
                IQueryable<NumeroUnico> iOdt = ctx.NumeroUnico.AsQueryable();

                if (proyectoID.HasValue && proyectoID.Value > 0)
                {
                    //aquí asumimos que tiene permisos
                    iOdt = iOdt.Where(x => x.ProyectoID == proyectoID);
                }
                else if (!esAdministradorSistema)
                {
                    //aqui traemos unicamente por permisos
                    iOdt = iOdt.Where(x => ctx.UsuarioProyecto
                                              .Where(up => up.UserId == userID)
                                              .Select(up => up.ProyectoID)
                                              .Contains(x.ProyectoID));
                }

                lst =
                    iOdt.Select(x => new Simple { ID = x.NumeroUnicoID, Valor = x.Codigo })
                        .ToList();
            }

            return lst.Where(x => x.Valor.StartsWith(codigoNumeroUnicoFiltro, StringComparison.InvariantCultureIgnoreCase))
                      .OrderBy(x => x.Valor)
                      .Skip(skip)
                      .Take(take);
        }


        public IEnumerable<Simple> ObtenerSpoolSinODT(int? proyectoID, int skip, int take, string codigoNumeroUnicoFiltro, bool esAdministradorSistema, Guid userID)
        {
            List<Simple> lst;

            using (SamContext ctx = new SamContext())
            {
                IQueryable<Spool> spools = ctx.Spool.Where(x => x.ProyectoID == proyectoID.Value).AsQueryable();
                IQueryable<OrdenTrabajoSpool> odt = ctx.OrdenTrabajoSpool.Where(x => spools.Select(y => y.SpoolID).Contains(x.SpoolID)).AsQueryable();

                lst = spools.Where(x => !odt.Select(y => y.SpoolID).Contains(x.SpoolID)).Select(x => new Simple { ID = x.SpoolID, Valor = x.Nombre }).ToList();
            }

            return lst.Where(x => x.Valor.StartsWith(codigoNumeroUnicoFiltro, StringComparison.InvariantCultureIgnoreCase))
                      .OrderBy(x => x.Valor)
                      .Skip(skip)
                      .Take(take);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="codigoNumeroUnicoFiltro"></param>
        /// <param name="esAdministradorSistema"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IEnumerable<Simple> ObtenerNumerosUnicosPorPermisoCongelados(int? proyectoID, int skip, int take, string codigoNumeroUnicoFiltro, bool esAdministradorSistema, Guid userID)
        {
            List<Simple> lst;
            List<Simple> lstParcial;

            using (SamContext ctx = new SamContext())
            {
                ctx.NumeroUnico.MergeOption = MergeOption.NoTracking;
                IQueryable<NumeroUnico> iOdt = ctx.NumeroUnico.AsQueryable();

                if (proyectoID.HasValue && proyectoID.Value > 0)
                {
                    //aquí asumimos que tiene permisos
                    iOdt = iOdt.Where(x => x.ProyectoID == proyectoID);
                }
                else if (!esAdministradorSistema)
                {
                    //aqui traemos unicamente por permisos
                    iOdt = iOdt.Where(x => ctx.UsuarioProyecto
                                              .Where(up => up.UserId == userID)
                                              .Select(up => up.ProyectoID)
                                              .Contains(x.ProyectoID));
                }

                lst = (from nu in iOdt
                       join otm in ctx.OrdenTrabajoMaterial on nu.NumeroUnicoID equals otm.NumeroUnicoCongeladoID
                       select new Simple
                       {
                           ID = nu.NumeroUnicoID,
                           Valor = ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == nu.NumeroUnicoID).Any() ? nu.Codigo + "-" + ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == nu.NumeroUnicoID).Select(x => x.Segmento).FirstOrDefault() : nu.Codigo
                       }).Distinct().ToList();

                lstParcial = (from nu in iOdt
                              join cp in ctx.CongeladoParcial on nu.NumeroUnicoID equals cp.NumeroUnicoCongeladoID
                              select new Simple
                              {
                                  ID = nu.NumeroUnicoID,
                                  Valor = ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == nu.NumeroUnicoID).Any() ? nu.Codigo + "-" + ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == nu.NumeroUnicoID).Select(x => x.Segmento).FirstOrDefault() : nu.Codigo
                              }).Distinct().ToList();

                IEqualityComparer<Simple> comparer = new NumeroUnicoComparer();                
                lst = lst.Union(lstParcial).Distinct(comparer).ToList();

            }

            return lst.Where(x => x.Valor.StartsWith(codigoNumeroUnicoFiltro, StringComparison.InvariantCultureIgnoreCase))
                      .OrderBy(x => x.Valor)
                      .Skip(skip)
                      .Take(take);
        }

        public int ObtenerNumeroUnicoPorCodigo(string codigo)
        {

            using (SamContext context = new SamContext())
            {
                IQueryable<NumeroUnico> numerounico = context.NumeroUnico.AsQueryable();

                numerounico = context.NumeroUnico.Where(x => codigo.Contains(x.Codigo.ToString()));

                return numerounico.Select(x => x.NumeroUnicoID).SafeIntParse();
            }
        }

        public IEnumerable<Simple> ObtenerNumerosUnicosMatchPorItemCode( int skip, int take, string codigoNumeroUnicoFiltro, int? numerounicoOmaterialspool, int? cantcongelada, bool esNumeroUnico)
        {

            List<Simple> lst;
            List<Simple> lstSimpleSeg;
            List<NumeroUnicoSegmento> lstSeg;

            using (SamContext context = new SamContext())
            {
                int itemcode = 0;
                decimal d1 = 0;
                decimal d2 = 0;
                IQueryable<NumeroUnico> _numerounico = null;
                IQueryable<MaterialSpool> _materialspool = null;

                if (esNumeroUnico)
                {
                    _numerounico = context.NumeroUnico.Where(x => x.NumeroUnicoID == numerounicoOmaterialspool).AsQueryable();

                    itemcode = _numerounico.Select(x => x.ItemCodeID).SingleOrDefault().Value;
                    d1 = _numerounico.Select(x => x.Diametro1).SingleOrDefault();
                    d2 = _numerounico.Select(x => x.Diametro2).SingleOrDefault();

                    var query1 = (from nu in context.NumeroUnico
                                  join ice in context.ItemCodeEquivalente
                                  on nu.ItemCodeID equals ice.ItemEquivalenteID
                                  where ice.ItemCodeID == itemcode
                                     && ice.Diametro1 == d1
                                     && ice.Diametro2 == d2
                                     && nu.Diametro1 == ice.DiametroEquivalente1
                                     && nu.Diametro2 == ice.DiametroEquivalente2
                                  select new
                                  {
                                      nu.Codigo,
                                      nu.NumeroUnicoID
                                  });

                    var query = from nu in _numerounico
                                join numunic in context.NumeroUnico
                                on new
                                {
                                    nu.ItemCodeID,
                                    nu.Diametro1,
                                    nu.Diametro2
                                }
                                equals
                                new
                                {
                                    numunic.ItemCodeID,
                                    numunic.Diametro1,
                                    numunic.Diametro2
                                }
                                select new
                                {
                                    numunic.Codigo,
                                    numunic.NumeroUnicoID
                                };

                    query = query.Union(query1);

                    lstSeg = (from q in query
                                         join nus in context.NumeroUnicoSegmento on
                                         q.NumeroUnicoID equals nus.NumeroUnicoID                                         
                                         select nus).Distinct().ToList();

                    lst = (from nui in context.NumeroUnicoInventario
                           join q in query on
                           nui.NumeroUnicoID equals q.NumeroUnicoID
                           where nui.InventarioDisponibleCruce >= cantcongelada                           
                           select new Simple
                           {
                               ID = q.NumeroUnicoID,
                               Valor = q.Codigo
                           }).Distinct().ToList();

                    foreach (NumeroUnicoSegmento item in lstSeg)
                    {
                        lst = lst.Where(x => x.ID != item.NumeroUnicoID).ToList();
                    }

                    lstSimpleSeg = (from segmentos in lstSeg
                                    join q in query on
                                    segmentos.NumeroUnicoID equals q.NumeroUnicoID
                                    where segmentos.InventarioDisponibleCruce >= cantcongelada
                                    select new Simple
                                    {
                                        ID = segmentos.NumeroUnicoID,
                                        Valor = q.Codigo + "-" + segmentos.Segmento
                                    }).Distinct().ToList();
                    
                    lst = lst.Union(lstSimpleSeg).Distinct().ToList();

                    lst.RemoveAll(x => x.ID == numerounicoOmaterialspool.Value);
                }
                else
                {
                    _materialspool = context.MaterialSpool.Where(x => x.MaterialSpoolID == numerounicoOmaterialspool).AsQueryable();

                    itemcode = _materialspool.Select(x => x.ItemCodeID).SingleOrDefault();
                    d1 = _materialspool.Select(x => x.Diametro1).SingleOrDefault();
                    d2 = _materialspool.Select(x => x.Diametro2).SingleOrDefault();

                    var query1 = (from nu in context.NumeroUnico
                                  join ice in context.ItemCodeEquivalente
                                on nu.ItemCodeID equals ice.ItemEquivalenteID
                                  where ice.ItemCodeID == itemcode
                                     && ice.Diametro1 == d1
                                     && ice.Diametro2 == d2
                                     && nu.Diametro1 == ice.DiametroEquivalente1
                                     && nu.Diametro2 == ice.DiametroEquivalente2
                                  select new
                                  {
                                      nu.Codigo,
                                      nu.NumeroUnicoID
                                  }
                                  ).AsQueryable();

                    var query = from ms in _materialspool
                                join numunic in context.NumeroUnico
                                on new
                                {
                                    itemcode = ms.ItemCodeID,
                                    ms.Diametro1,
                                    ms.Diametro2
                                }
                                equals
                                new
                                {
                                    itemcode = numunic.ItemCodeID.Value,
                                    numunic.Diametro1,
                                    numunic.Diametro2

                                }
                                select new
                                {
                                    numunic.Codigo,
                                    numunic.NumeroUnicoID
                                };

                    query = query.Union(query1);

                    lstSeg = (from q in query
                              join nus in context.NumeroUnicoSegmento on
                              q.NumeroUnicoID equals nus.NumeroUnicoID
                              select nus).Distinct().ToList();

                    lst = (from nui in context.NumeroUnicoInventario
                           join q in query on
                           nui.NumeroUnicoID equals q.NumeroUnicoID
                           where nui.InventarioDisponibleCruce >= cantcongelada
                           select new Simple
                           {
                               ID = q.NumeroUnicoID,
                               Valor = q.Codigo
                           }).Distinct().ToList();

                    foreach (NumeroUnicoSegmento item in lstSeg)
                    {
                        lst = lst.Where(x => x.ID != item.NumeroUnicoID).ToList();
                    }

                    lstSimpleSeg = (from segmentos in lstSeg
                                    join q in query on
                                    segmentos.NumeroUnicoID equals q.NumeroUnicoID
                                    where segmentos.InventarioDisponibleCruce >= cantcongelada
                                    select new Simple
                                    {
                                        ID = segmentos.NumeroUnicoID,
                                        Valor = q.Codigo + "-" + segmentos.Segmento
                                    }).Distinct().ToList();

                    lst = lst.Union(lstSimpleSeg).Distinct().ToList();
                    
                }
            }
            return lst.Where(x => x.Valor.StartsWith(codigoNumeroUnicoFiltro, StringComparison.InvariantCultureIgnoreCase))
                      .OrderBy(x => x.Valor)
                      .Skip(skip)
                      .Take(take);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numeroUnicoID"></param>
        /// <returns></returns>
        public int ObtenerProyectoID(int numeroUnicoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ProyectoNumeroUnico(ctx, numeroUnicoID);
            }
        }


        /// <summary>
        /// Versión compilada del query para permisos de número único
        /// </summary>
        private static readonly Func<SamContext, int, int> ProyectoNumeroUnico =
        CompiledQuery.Compile<SamContext, int, int>
        (
            (ctx, id) => ctx.NumeroUnico
                            .Where(x => x.NumeroUnicoID == id)
                            .Select(x => x.ProyectoID)
                            .Single()
        );


        /// <summary>
        /// Regresa un valor booleano indicando si los datos considerados "base" de un número único
        /// aún se pueden editar o no.  Los datos base son los siguientes:
        /// - Item Code
        /// - Cantidad
        /// - D1
        /// - D2
        /// 
        /// Para poder editar esos datos es requisito lo siguinete:
        /// 1. El número único no tenga despachos o Congelados 
        /// 2. No existan movientos en iventario sobre este numero unico
        /// </summary>
        /// <param name="numeroUnicoID">ID del número único que se quiere verificar</param>
        /// <returns>Verdadero si se puede modificar, falso de lo contrario</returns>
        public bool PuedeModificarDatosBase(int numeroUnicoID)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.Despacho.MergeOption = MergeOption.NoTracking;
                ctx.NumeroUnicoMovimiento.MergeOption = MergeOption.NoTracking;

                bool tieneDespachos = ctx.Despacho.Any(x => x.NumeroUnicoID == numeroUnicoID && !x.Cancelado);
                bool tieneCongelados = ctx.NumeroUnicoInventario.Any(x => x.NumeroUnicoID == numeroUnicoID && x.InventarioCongelado > 0);
                bool tieneVariosMov = ctx.NumeroUnicoMovimiento.Where(x => x.NumeroUnicoID == numeroUnicoID && x.Estatus != EstatusNumeroUnicoMovimiento.CANCELADO).Count() > 1;

                return !tieneDespachos && !tieneCongelados && !tieneVariosMov;
            }
        }

        /// <summary>
        /// Método que nos trae las Órdenes de trabajo Material que por alguna razón no tienen un Número Único ID asignado
        /// </summary>
        /// <param name="proyectoID">Proyecto ID</param>
        public int[] OdtEsperaMaterial(int proyectoID, NumeroUnico numeroUnico)
        {
            using (SamContext ctx = new SamContext())
            {
                NumeroUnicoInventario numinv = ctx.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == numeroUnico.NumeroUnicoID).SingleOrDefault();
                int cantidad = numinv.InventarioBuenEstado;
               
                IQueryable<int> spool = ctx.Spool.Where(x => x.ProyectoID == proyectoID).Select(x => x.SpoolID);
                IQueryable<int> odtSpool = ctx.OrdenTrabajoSpool.Where(x => spool.Contains(x.SpoolID)).Select(x => x.OrdenTrabajoSpoolID);

                IQueryable<OrdenTrabajoMaterial> odtMaterial = ctx.OrdenTrabajoMaterial.Where(x => odtSpool.Contains(x.OrdenTrabajoSpoolID)
                                                                                        && x.NumeroUnicoCongeladoID == null
                                                                                        && x.NumeroUnicoAsignadoID == null
                                                                                        && x.NumeroUnicoDespachadoID == null
                                                                                        && x.CorteDetalleID == null
                                                                                        && x.NumeroUnicoSugeridoID == null);
                if (odtMaterial != null)
                {
                    var materiales = (from odtmat in odtMaterial
                                      join matSpool in ctx.MaterialSpool
                                      on odtmat.MaterialSpoolID equals matSpool.MaterialSpoolID
                                      where matSpool.ItemCodeID == numeroUnico.ItemCodeID
                                      && matSpool.Diametro1 == numeroUnico.Diametro1
                                      && matSpool.Diametro2 == numeroUnico.Diametro2
                                      && matSpool.Cantidad <= cantidad
                                      select matSpool);
                    if (materiales.Count() > 0)
                    {
                        return materiales.Select(x => x.MaterialSpoolID).ToArray();
                    }
                    else
                        return null;
                }
                else
                    return null;
            }
        }

        public List<GrdOdtReqMateial> llenaGridOdtReqMaterial(int[] Materiales)
        {
            List<OrdenTrabajoMaterial> otmquery = null;
            List<MaterialSpool> msquery = null;
            using (SamContext ctx = new SamContext())
            {
                otmquery = ctx.OrdenTrabajoMaterial.Where(x => Materiales.Contains(x.MaterialSpoolID)).ToList();
                msquery = ctx.MaterialSpool.Where(x => Materiales.Contains(x.MaterialSpoolID)).ToList();

                return (from otm in otmquery
                        join ms in msquery
                        on otm.MaterialSpoolID equals ms.MaterialSpoolID
                        join ots in ctx.OrdenTrabajoSpool
                        on otm.OrdenTrabajoSpoolID equals ots.OrdenTrabajoSpoolID
                        select new GrdOdtReqMateial
                        {
                            NumeroControl = ots.NumeroControl,
                            OrdenTrabajoSpool = ots.OrdenTrabajoSpoolID,
                            MaterialSpoolID = ms.MaterialSpoolID,
                            EtiquetaMaterial = ms.Etiqueta,
                            Cantidad = ms.Cantidad
                        }).Distinct().ToList();
            }
        }

        public void AgregaCongeladoOdt(int numeroUnico, int[] MaterialSpool, int[] OrdenTrabajoSpool, int cantidadSeleccionada, Guid userID, DateTime fechaModificacion)
        {
            using (SamContext ctx = new SamContext())
            {
                if (MaterialSpool.Count() <= 0)
                {
                    throw new ExcepcionCantidades(MensajesError.Excepcion_NumControlSeleccionados);
                }
                if (ValidacionesCantidades.ValidaCantidadesCongeladoOdt(ctx, numeroUnico, cantidadSeleccionada))
                {
                    throw new ExcepcionCantidades(MensajesError.Excepcion_Cantidades);
                }
                List<OrdenTrabajoMaterial> otm = ctx.OrdenTrabajoMaterial.Where(x => MaterialSpool.Contains(x.MaterialSpoolID)
                                                                                && OrdenTrabajoSpool.Contains(x.OrdenTrabajoSpoolID)).ToList();

                NumeroUnicoInventario numInv = ctx.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == numeroUnico).SingleOrDefault();

                if (ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == numeroUnico).Any())
                {
                    NumeroUnicoSegmento numseg = ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == numeroUnico).SingleOrDefault();
                    foreach (OrdenTrabajoMaterial orden in otm)
                    {
                        orden.NumeroUnicoCongeladoID = numeroUnico;
                        orden.SegmentoCongelado = "A";
                        orden.FechaModificacion = fechaModificacion;
                        orden.UsuarioModifica = userID;
                        ctx.OrdenTrabajoMaterial.ApplyChanges(orden);
                        int cantidad = ctx.MaterialSpool.Where(x => x.MaterialSpoolID == orden.MaterialSpoolID).Select(x => x.Cantidad).SingleOrDefault();
                        numInv.InventarioCongelado = cantidad;
                        numInv.InventarioDisponibleCruce = numInv.InventarioBuenEstado - cantidad;
                        ctx.NumeroUnicoInventario.ApplyChanges(numInv);
                        numseg.InventarioCongelado = cantidad;
                        numseg.InventarioDisponibleCruce = numseg.InventarioBuenEstado - cantidad;
                        ctx.NumeroUnicoSegmento.ApplyChanges(numseg);
                    }
                }
                else
                {
                    foreach (OrdenTrabajoMaterial orden in otm)
                    {
                        orden.NumeroUnicoCongeladoID = numeroUnico;
                        orden.FechaModificacion = fechaModificacion;
                        orden.UsuarioModifica = userID;
                        ctx.OrdenTrabajoMaterial.ApplyChanges(orden);
                        int cantidad = ctx.MaterialSpool.Where(x => x.MaterialSpoolID == orden.MaterialSpoolID).Select(x => x.Cantidad).SingleOrDefault();
                        numInv.InventarioCongelado = cantidad;
                        numInv.InventarioDisponibleCruce = numInv.InventarioBuenEstado - cantidad;
                        ctx.NumeroUnicoInventario.ApplyChanges(numInv);
                    }
                }
                ctx.SaveChanges();
            }
        }

        public NumeroUnico ObtenerNumeroUnicoParaTransferirCongelado(NumeroUnico numerounico)
        {
            using(SamContext ctx = new SamContext())
            {

                List<NumeroUnico> numeroUnicoATransferir = ctx.NumeroUnico
                                                           .Where(x => x.NumeroUnicoID != numerounico.NumeroUnicoID
                                                           && x.Estatus == EstatusNumeroUnico.APROBADO
                                                           && x.ItemCodeID == numerounico.ItemCodeID
                                                           && x.Diametro1 == numerounico.Diametro1
                                                           && x.Diametro2 == numerounico.Diametro2
                                                           && x.NumeroUnicoInventario.InventarioDisponibleCruce >= numerounico.NumeroUnicoInventario.InventarioCongelado).Distinct().ToList();
                //Buscamos Números Únicos equivalentes
                if (numeroUnicoATransferir.Count == 0)
                {
                    numeroUnicoATransferir = (from ice in ctx.ItemCodeEquivalente
                                                                      .Where(x => x.ItemCodeID == numerounico.ItemCodeID
                                                                      && x.Diametro1 == numerounico.Diametro1
                                                                      && x.Diametro2 == numerounico.Diametro2)                                              
                                              join nu in ctx.NumeroUnico on ice.ItemEquivalenteID equals nu.ItemCodeID
                                              where nu.Estatus == EstatusNumeroUnico.APROBADO && nu.NumeroUnicoID != numerounico.NumeroUnicoID
                                              select nu).Distinct().ToList();                    
                        
                }

                if (numerounico.ItemCode.TipoMaterialID == 1)
                {
                    bool completa = false;
                    NumeroUnico NumeroUnicoDestinatario = null;

                    List<NumeroUnicoSegmento> segmentosRemitente = ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == numerounico.NumeroUnicoID).ToList();
                    foreach (NumeroUnico nu in numeroUnicoATransferir)
                    {
                        List<NumeroUnicoSegmento> segmentoDestinatario = ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == nu.NumeroUnicoID).OrderBy(x => x.InventarioDisponibleCruce).ToList();
                        
                        foreach (NumeroUnicoSegmento segRemitente in segmentosRemitente)
                        {
                            completa = false;
                            foreach (NumeroUnicoSegmento segDestinatario in segmentoDestinatario)
                            {
                                if (segDestinatario.InventarioDisponibleCruce >= segRemitente.InventarioCongelado)
                                {
                                    segmentoDestinatario.Remove(segDestinatario);
                                    completa = true;
                                    break;
                                }
                            }
                            if (!completa)
                            {
                                break;
                            }
                        }
                        if (completa)
                        {
                            NumeroUnicoDestinatario = nu;
                            break;
                        }
                    }
                    return NumeroUnicoDestinatario;
                }
                else
                {
                    return numeroUnicoATransferir.FirstOrDefault();
                }
            }     
        }

        public List<string> ObtenNumerosControl(NumeroUnico numerounico)
        {
            List<string> numerosControl = null;

            using (SamContext ctx = new SamContext())
            {                

                numerosControl = (from otm in ctx.OrdenTrabajoMaterial.Where(x => x.NumeroUnicoCongeladoID == numerounico.NumeroUnicoID)
                                  join ots in ctx.OrdenTrabajoSpool on otm.OrdenTrabajoSpoolID equals ots.OrdenTrabajoSpoolID
                                  select ots.NumeroControl).Distinct().ToList();

                return numerosControl;
            }
        }

        public void TransferirMaterialCongelado(NumeroUnico numeroUnicoRemitente, NumeroUnico numeroUnicoDestinatario, Guid userID, DateTime fechaModificacion)
        {
            using (TransactionScope t = new TransactionScope())
            {
                using (SamContext ctx = new SamContext())
                {                    
                    NumeroUnicoInventario inventarioDestinatario = ctx.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == numeroUnicoDestinatario.NumeroUnicoID).SingleOrDefault();
                    NumeroUnicoInventario inventarioRemitente = ctx.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == numeroUnicoRemitente.NumeroUnicoID).SingleOrDefault();

                    int cantidadCongeladaActualizada = inventarioDestinatario.InventarioCongelado + numeroUnicoRemitente.NumeroUnicoInventario.InventarioCongelado;
                    inventarioDestinatario.InventarioCongelado = cantidadCongeladaActualizada;
                    inventarioDestinatario.InventarioDisponibleCruce = inventarioDestinatario.InventarioBuenEstado - cantidadCongeladaActualizada;
                    ctx.NumeroUnicoInventario.ApplyChanges(inventarioDestinatario);

                    inventarioRemitente.InventarioDisponibleCruce = inventarioRemitente.InventarioBuenEstado;
                    inventarioRemitente.InventarioCongelado = 0;
                    ctx.NumeroUnicoInventario.ApplyChanges(inventarioRemitente);
                    int tipoMaterialID = ctx.ItemCode.Where(x => x.ItemCodeID == numeroUnicoDestinatario.ItemCodeID).Select(x => x.TipoMaterialID).FirstOrDefault();
                    if (tipoMaterialID  == 1)
                    {
                        List<NumeroUnicoSegmento> segDestinatario = ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == numeroUnicoDestinatario.NumeroUnicoID).OrderBy(x => x.InventarioDisponibleCruce).ToList();
                        List<NumeroUnicoSegmento> segRemitente = ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == numeroUnicoRemitente.NumeroUnicoID).OrderBy(x => x.InventarioCongelado).ToList();
                        foreach (NumeroUnicoSegmento sr in segRemitente)
                        {
                            NumeroUnicoSegmento sd = segDestinatario.Where(x => x.InventarioDisponibleCruce >= sr.InventarioCongelado).FirstOrDefault();
                            int cantidadCongeladaActualizadaSegmento = sd.InventarioCongelado + sr.InventarioCongelado;
                            sd.InventarioCongelado = cantidadCongeladaActualizadaSegmento;
                            sd.InventarioDisponibleCruce = sd.InventarioBuenEstado - cantidadCongeladaActualizadaSegmento;
                            ctx.NumeroUnicoSegmento.ApplyChanges(sd);

                            sr.InventarioCongelado = 0;
                            sr.InventarioDisponibleCruce = sr.InventarioBuenEstado;
                            ctx.NumeroUnicoSegmento.ApplyChanges(sr);

                            segDestinatario.Remove(sd);

                            List<OrdenTrabajoMaterial> ordenTrabajoMaterial = ctx.OrdenTrabajoMaterial.Where(x => x.NumeroUnicoCongeladoID == sr.NumeroUnicoID && x.SegmentoCongelado == sr.Segmento).ToList();
                            ordenTrabajoMaterial.ForEach(x =>
                            {
                                x.StartTracking();
                                x.NumeroUnicoCongeladoID = sd.NumeroUnicoID;
                                x.SegmentoCongelado = sd.Segmento;
                                x.UsuarioModifica = userID;
                                x.FechaModificacion = fechaModificacion;
                                x.StopTracking();
                                ctx.OrdenTrabajoMaterial.ApplyChanges(x);
                            });

                            List<CongeladoParcial> congeladoParcial = ctx.CongeladoParcial.Where(x => x.NumeroUnicoCongeladoID == sr.NumeroUnicoID && x.SegmentoCongelado == sr.Segmento).ToList();
                            congeladoParcial.ForEach(x =>
                                {
                                    x.StartTracking();
                                    x.NumeroUnicoCongeladoID = sd.NumeroUnicoID;
                                    x.SegmentoCongelado = sd.Segmento;
                                    x.UsuarioModifica = userID;
                                    x.FechaModificacion = fechaModificacion;
                                    x.StopTracking();
                                    ctx.CongeladoParcial.ApplyChanges(x);
                                });
                        }

                    }
                    else
                    {
                        List<OrdenTrabajoMaterial> ordenTrabajoMaterial = ctx.OrdenTrabajoMaterial.Where(x => x.NumeroUnicoCongeladoID == numeroUnicoRemitente.NumeroUnicoID).ToList();
                        ordenTrabajoMaterial.ForEach(x =>
                            {
                                x.StartTracking();
                                x.NumeroUnicoCongeladoID = numeroUnicoDestinatario.NumeroUnicoID;
                                x.UsuarioModifica = userID;
                                x.FechaModificacion = fechaModificacion;
                                x.StopTracking();
                                ctx.OrdenTrabajoMaterial.ApplyChanges(x);
                            });

                        List<CongeladoParcial> congeladoParcial = ctx.CongeladoParcial.Where(x => x.NumeroUnicoCongeladoID == numeroUnicoRemitente.NumeroUnicoID).ToList();
                        congeladoParcial.ForEach(x =>
                        {
                            x.StartTracking();
                            x.NumeroUnicoCongeladoID = numeroUnicoDestinatario.NumeroUnicoID;                           
                            x.UsuarioModifica = userID;
                            x.FechaModificacion = fechaModificacion;
                            x.StopTracking();
                            ctx.CongeladoParcial.ApplyChanges(x);
                        });
                    }
                    ctx.SaveChanges();
                }
                t.Complete();
            }            
        }

        public void ValidaNoTengaCongelados(NumeroUnico numeroUnicoTransferir, NumeroUnico numUnico)
        {
            if (numeroUnicoTransferir == null)
            {
                List<string> numerosControl = NumeroUnicoBO.Instance.ObtenNumerosControl(numUnico);

                if (numerosControl.Count > 0)
                {
                    string concatenacion = string.Empty;

                    if (numerosControl.Count > 1)
                    {
                        concatenacion = numerosControl.Aggregate((i, j) => i + ", " + j);
                    }
                    else
                    {
                        concatenacion = numerosControl.First();
                    }

                    throw new ExcepcionCantidades(string.Format(MensajesError.Excepcion_EdicionConMaterialCongelado, concatenacion));
                }
                else
                {
                    throw new ExcepcionCantidades(MensajesError.Excepcion_EdicionCongeladoParcial);
                }
            }
        }
    }

    class NumeroUnicoComparer : IEqualityComparer<Simple>
    {
        public bool Equals(Simple x, Simple y)
        {
            return x.ID.Equals(y.ID);
        }

        public int GetHashCode(Simple obj)
        {
            return obj.ID.GetHashCode();
        }
    }
}

