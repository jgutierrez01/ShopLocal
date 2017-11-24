using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Proyectos;
using SAM.BusinessObjects.Modelo;
using System.Data.Objects;
using SAM.BusinessLogic.Excepciones;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Materiales;

namespace SAM.BusinessLogic.Materiales
{
    public class NumeroUnicoBL
    {
        private static readonly object _mutex = new object();
        private static NumeroUnicoBL _instance;

        /// <summary>
        /// constructro privado para implementar patron singleton
        /// </summary>
        private NumeroUnicoBL()
        {
        }

        /// <summary>
        /// obtiene la instancia de la clase OrdenTrabajoBL
        /// </summary>
        public static NumeroUnicoBL Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new NumeroUnicoBL();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Genera el cascarón de los números únicos a recibirse para apartar los consecutivos.
        /// </summary>
        /// <param name="cantidadNumerosUnicos"></param>
        /// <param name="numeroInicial"></param>
        /// <param name="recepcionID"></param>
        /// <param name="ordenCompra"></param>
        /// <param name="factura"></param>
        /// <param name="proyectoID"></param>
        /// <param name="proveedorID"></param>
        /// <param name="codigoProyecto"></param>
        /// <returns></returns>
        public List<NumeroUnico> GeneraNumerosUnicos(int cantidadNumerosUnicos,
                                                        int numeroInicial,
                                                        string ordenCompra,
                                                        string factura,
                                                        int proyectoID,
                                                        int proveedorID,
                                                        string codigoProyecto,
                                                        Guid userID,
                                                        Recepcion recepcion)
        {
            List<NumeroUnico> numUnicoList = new List<NumeroUnico>();
            Proyecto proyecto = ProyectoBO.Instance.ObtenerConConfiguracion(proyectoID);

            for (int numUnicos = 0; numUnicos < cantidadNumerosUnicos; numUnicos++)
            {
                NumeroUnico num = new NumeroUnico();
                num.ProyectoID = proyectoID;
                if (proveedorID > 0)
                {
                    num.ProveedorID = proveedorID;
                }
                num.Diametro1 = 0;
                num.Diametro2 = 0;
                num.Codigo =  string.Format("{0}-{1}", codigoProyecto,
                                    numeroInicial.ToString().PadLeft(proyecto.ProyectoConfiguracion.DigitosNumeroUnico, '0'));                
                num.Estatus = "A";
                num.Factura = factura;
                num.OrdenDeCompra = ordenCompra;

                #region asignación de campos libres
                if (recepcion.CampoLibre1 != null)
                {
                    num.CampoLibreRecepcion1 = recepcion.CampoLibre1;
                }
                if (recepcion.CampoLibre2 != null)
                {
                    num.CampoLibreRecepcion2 = recepcion.CampoLibre2;
                }
                if (recepcion.CampoLibre3 != null)
                {
                    num.CampoLibreRecepcion3 = recepcion.CampoLibre3;
                }
                if (recepcion.CampoLibre4 != null)
                {
                    num.CampoLibreRecepcion4 = recepcion.CampoLibre4;
                }
                if (recepcion.CampoLibre5 != null)
                {
                    num.CampoLibreRecepcion5 = recepcion.CampoLibre5;
                }
                #endregion

                num.UsuarioModifica = userID;
                num.FechaModificacion = DateTime.Now;

                numUnicoList.Add(num);
                numeroInicial++;                                
            }

            return numUnicoList;
        }



        /// <summary>
        /// Si esta en transferencia se arroja una excepcion indicando que hay material despachado a corte
        /// 
        /// Este metodo revisa que el las cantidades a modificar del numero unico, no dejen un estado negativo para inventarios
        /// </summary>
        /// <param name="numeroUnicoID"></param>
        /// <param name="cantidad"></param>
        /// <param name="cantidadDanada"></param>
        /// <returns></returns>
        public bool PuedeEditarCantidades(int numeroUnicoID, int cantidad, int cantidadDanada)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.Despacho.MergeOption = MergeOption.NoTracking;
                ctx.NumeroUnicoInventario.MergeOption = MergeOption.NoTracking;
                ctx.NumeroUnicoMovimiento.MergeOption = MergeOption.NoTracking;

                bool estaEnTransferencia = ctx.NumeroUnicoInventario.Any(x => x.NumeroUnicoID == numeroUnicoID && x.InventarioTransferenciaCorte > 0);
               

                int cantidadDespachada = ctx.Despacho.Where(x => x.NumeroUnicoID == numeroUnicoID && !x.Cancelado).Select(x => x.Cantidad).DefaultIfEmpty(0).Sum();
                int cantidadCongelada = ctx.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == numeroUnicoID).Select(x => x.InventarioCongelado).DefaultIfEmpty(0).Sum();
                int mermas = 0;
                int numOtrasSalidas = 0;
                int numOtrasEntradas = 0;
                int diferenciaRecepcion = 0;

                NumeroUnicoInventario nui = ctx.NumeroUnicoInventario
                                               .Where(x => x.NumeroUnicoID == numeroUnicoID)
                                               .SingleOrDefault();

                if (ctx.NumeroUnicoMovimiento.Where(x => x.NumeroUnicoID == numeroUnicoID).Any())
                {
                    mermas = ctx.NumeroUnicoMovimiento.Where(x => x.NumeroUnicoID == numeroUnicoID && x.TipoMovimientoID == (int)TipoMovimientoEnum.MermaCorte && x.Estatus != EstatusNumeroUnicoMovimiento.CANCELADO).Select(x => x.Cantidad).DefaultIfEmpty(0).Sum();


                    numOtrasEntradas = ctx.NumeroUnicoMovimiento.Where(x =>
                                            x.NumeroUnicoID == numeroUnicoID &&
                                            x.TipoMovimiento.TipoMovimientoID != (int)TipoMovimientoEnum.EntradaPintura
                                            && x.TipoMovimiento.DisponibleMovimientosUI
                                            && x.TipoMovimiento.EsEntrada).Select(x => x.Cantidad).DefaultIfEmpty(0).Sum();

                    numOtrasSalidas = ctx.NumeroUnicoMovimiento.Where(x =>
                                            x.NumeroUnicoID == numeroUnicoID &&
                                            x.TipoMovimiento.TipoMovimientoID != (int)TipoMovimientoEnum.SalidaPintura
                                            && x.TipoMovimiento.DisponibleMovimientosUI
                                            && !x.TipoMovimiento.EsEntrada).Select(x => x.Cantidad).DefaultIfEmpty(0).Sum();

                }


                if (nui != null)
                {
                    diferenciaRecepcion = cantidad - nui.CantidadRecibida;
                    if (diferenciaRecepcion == 0 && cantidadDanada == nui.CantidadDanada)
                    {
                        return true;
                    }

                    if (estaEnTransferencia)
                    {
                        throw new ExcepcionNumeroUnicoDespacho(MensajesError.Excepcion_NumeroUnicoTieneTranferencias);
                    }

                    if (nui.InventarioFisico + diferenciaRecepcion - cantidadDanada < 0)
                    {
                        return false;
                    }
                }

                if (estaEnTransferencia)
                {
                    throw new ExcepcionNumeroUnicoDespacho(MensajesError.Excepcion_NumeroUnicoTieneTranferencias);
                }

                // se valida que el total de las entradas tomando en cuenta la edicion de la recepcion sea mayor o igual al inventario que ya se utilizo
                // es decir que ya se despacho, se congelo, salio o fue merma
                if ((numOtrasEntradas + cantidad - cantidadDanada) < (cantidadDespachada + cantidadCongelada + numOtrasSalidas + mermas))
                {
                    return false;
                }

                return true;
            }
        }


        /// <summary>
        /// Si el tipo de material del itemcode seleccionado es diferente al actual y se cuenta con despachos o cortes se manda una excepción
        /// </summary>
        /// <param name="numeroUnicoID">Número único a revisar</param>
        /// <param name="nuevoItemCodeID">Item Code seleccionado en UI</param>
        /// <returns>True si no hay problema con el cambio de itemCode, False en caso contrario</returns>
        public bool ItemCodeValido(int numeroUnicoID, int nuevoItemCodeID)
        {
            using (SamContext ctx = new SamContext())
            {
                ItemCode itemCodeActual = ctx.NumeroUnico.Where(x => x.NumeroUnicoID == numeroUnicoID).Select(y => y.ItemCode).SingleOrDefault();

                ItemCode itemCodeNuevo = ctx.ItemCode.Where(x => x.ItemCodeID == nuevoItemCodeID).SingleOrDefault();

                if (NumeroUnicoBO.Instance.PuedeModificarDatosBase(numeroUnicoID))
                {
                    return true;
                }
                else if (itemCodeActual.TipoMaterialID == itemCodeNuevo.TipoMaterialID)
                {
                    return true;
                }
                else
                {
                    throw new ExcepcionNumeroUnicoDespacho(MensajesError.Excepcion_TipoMaterialNumeroUnico);
                }
            }
        }

        /// <summary>
        /// Regresa verdadero en caso de que este movimiento para inventario sea del tipo UI y se permita eliminar
        /// </summary>
        /// <param name="tipoMovimiento"></param>
        /// <returns></returns>
        public bool EsTipoMovimientoEliminable(int tipoMovimiento)
        {
            return  tipoMovimiento == (int)TipoMovimientoEnum.EntradaPintura 
                    || tipoMovimiento == (int)TipoMovimientoEnum.EntradaOtrosProcesos 
                    || tipoMovimiento == (int)TipoMovimientoEnum.Devolucion 
                    || tipoMovimiento == (int)TipoMovimientoEnum.Merma 
                    || tipoMovimiento == (int)TipoMovimientoEnum.SalidaPintura 
                    || tipoMovimiento == (int)TipoMovimientoEnum.SalidaOtrosProcesos;
        }



        /// <summary>
        /// Revisa si en el caso de que eliminaramos este moviemiento no dejaria los inventarios en numeros negativos,         
        /// </summary>
        /// <param name="numeroUnicoMovimiento"></param>
        /// <returns></returns>
        public bool EsMovimientoEliminable(int numeroUnicoMovimientoID) 
        {
            int cantidad = 0;
            int cantidadDanada = 0;
            int numeroUnicoID = 0;

            using (SamContext ctx = new SamContext())
            {                
                ctx.Despacho.MergeOption = MergeOption.NoTracking;
                ctx.NumeroUnicoInventario.MergeOption = MergeOption.NoTracking;
                ctx.NumeroUnicoMovimiento.MergeOption = MergeOption.NoTracking;

                NumeroUnicoMovimiento numeroUnicoMovimiento = ctx.NumeroUnicoMovimiento.Include("TipoMovimiento").Include("NumeroUnico").Single(x => x.NumeroUnicoMovimientoID == numeroUnicoMovimientoID);
                numeroUnicoID = numeroUnicoMovimiento.NumeroUnicoID;
                NumeroUnico numUnico = ctx.NumeroUnico.Include("NumeroUnicoInventario").Include("NumeroUnicoSegmento").Single(x => x.NumeroUnicoID == numeroUnicoMovimiento.NumeroUnicoID);
                ItemCode ic = ItemCodeBO.Instance.ObtenerConTipoMaterial(numUnico.ItemCodeID.Value);
                bool esTubo = ic.TipoMaterialID == (int)TipoMaterialEnum.Tubo;

                //podemos eliminar cualquier salidas puesto que nos agregara inventario
                if (!numeroUnicoMovimiento.TipoMovimiento.EsEntrada)
                {
                    return true;
                }

                cantidad = numeroUnicoMovimiento.Cantidad;

                bool estaEnTransferencia = ctx.NumeroUnicoInventario.Any(x => x.NumeroUnicoID == numeroUnicoID && x.InventarioTransferenciaCorte > 0);
                if (estaEnTransferencia)
                {
                    throw new ExcepcionNumeroUnicoDespacho(MensajesError.Excepcion_NumeroUnicoTieneTranferencias);
                }

                int cantidadDespachada = ctx.Despacho.Where(x => x.NumeroUnicoID == numeroUnicoID && !x.Cancelado).Select(x => x.Cantidad).DefaultIfEmpty(0).Sum();
                int cantidadCongelada = ctx.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == numeroUnicoID).Select(x => x.InventarioCongelado).DefaultIfEmpty(0).Sum();
                int mermas = 0;
                int numOtrasSalidas = 0;
                int numOtrasEntradas = 0;
                

                NumeroUnicoInventario nui = ctx.NumeroUnicoInventario
                                               .Where(x => x.NumeroUnicoID == numeroUnicoID)
                                               .SingleOrDefault();

                cantidadDanada = nui.CantidadDanada;

                mermas = ctx.NumeroUnicoMovimiento.Where(x => x.NumeroUnicoID == numeroUnicoID && x.TipoMovimientoID == (int)TipoMovimientoEnum.MermaCorte).Select(x => x.Cantidad).DefaultIfEmpty(0).Sum();


                numOtrasEntradas = ctx.NumeroUnicoMovimiento.Where(x =>
                                        x.NumeroUnicoID == numeroUnicoID &&
                                        x.TipoMovimiento.TipoMovimientoID != (int)TipoMovimientoEnum.EntradaPintura
                                        && x.TipoMovimiento.DisponibleMovimientosUI
                                        && x.TipoMovimiento.EsEntrada).Select(x => x.Cantidad).DefaultIfEmpty(0).Sum();

                numOtrasSalidas = ctx.NumeroUnicoMovimiento.Where(x =>
                                        x.NumeroUnicoID == numeroUnicoID &&
                                        x.TipoMovimiento.TipoMovimientoID != (int)TipoMovimientoEnum.SalidaPintura
                                        && x.TipoMovimiento.DisponibleMovimientosUI
                                        && !x.TipoMovimiento.EsEntrada).Select(x => x.Cantidad).DefaultIfEmpty(0).Sum();




                if (nui.InventarioFisico - cantidad - cantidadDanada < 0)
                {
                    throw new ExcepcionNumeroUnicoMovimiento(MensajesError.Excepcion_MovimientoImposibleEliminar);
                }


                // se valida que el total de las entradas menos la entrada a eliminar sea mayor o igual al inventario que ya se utilizo
                // es decir que ya se despacho, se congelo, salio o fue merma
                if ((nui.CantidadRecibida + numOtrasEntradas - cantidad - cantidadDanada) < (cantidadDespachada + cantidadCongelada + numOtrasSalidas + mermas))
                {
                    throw new ExcepcionNumeroUnicoMovimiento(MensajesError.Excepcion_MovimientoImposibleEliminar);
                }
                
                if (esTubo)
                {
                    NumeroUnicoSegmento nus = numUnico.NumeroUnicoSegmento.FirstOrDefault(x => x.Segmento == numeroUnicoMovimiento.Segmento);
                    if (nus != null)
                    {
                        cantidadDespachada = ctx.Despacho.Where(x => x.NumeroUnicoID == numeroUnicoID && !x.Cancelado && x.Segmento == numeroUnicoMovimiento.Segmento).Select(x => x.Cantidad).DefaultIfEmpty(0).Sum();
                        cantidadCongelada = nus.InventarioCongelado;
                        cantidadDanada = nus.CantidadDanada;
                        mermas = ctx.NumeroUnicoMovimiento.Where(x => x.NumeroUnicoID == numeroUnicoID && x.TipoMovimientoID == (int)TipoMovimientoEnum.MermaCorte && x.Segmento == numeroUnicoMovimiento.Segmento).Select(x => x.Cantidad).DefaultIfEmpty(0).Sum();

                        numOtrasEntradas = ctx.NumeroUnicoMovimiento.Where(x =>
                                                x.NumeroUnicoID == numeroUnicoID &&
                                                x.TipoMovimiento.TipoMovimientoID != (int)TipoMovimientoEnum.EntradaPintura
                                                && x.TipoMovimiento.DisponibleMovimientosUI
                                                && x.TipoMovimiento.EsEntrada
                                                && x.Segmento == numeroUnicoMovimiento.Segmento).Select(x => x.Cantidad).DefaultIfEmpty(0).Sum();

                        numOtrasSalidas = ctx.NumeroUnicoMovimiento.Where(x =>
                                                x.NumeroUnicoID == numeroUnicoID &&
                                                x.TipoMovimiento.TipoMovimientoID != (int)TipoMovimientoEnum.SalidaPintura
                                                && x.TipoMovimiento.DisponibleMovimientosUI
                                                && !x.TipoMovimiento.EsEntrada
                                                && x.Segmento == numeroUnicoMovimiento.Segmento).Select(x => x.Cantidad).DefaultIfEmpty(0).Sum();

                        int salidasPorSegmentacion = ctx.NumeroUnicoMovimiento.Where(x =>
                                                x.NumeroUnicoID == numeroUnicoID &&
                                                x.TipoMovimiento.TipoMovimientoID == (int)TipoMovimientoEnum.SalidaSegmentacion
                                                && x.Segmento == numeroUnicoMovimiento.Segmento).Select(x => x.Cantidad).DefaultIfEmpty(0).Sum();

                        int entradaPorSegmentacion = ctx.NumeroUnicoMovimiento.Where(x =>
                                                x.NumeroUnicoID == numeroUnicoID &&
                                                x.TipoMovimiento.TipoMovimientoID == (int)TipoMovimientoEnum.EntradasSegmentacion
                                                && x.Segmento == numeroUnicoMovimiento.Segmento).Select(x => x.Cantidad).DefaultIfEmpty(0).Sum();

                        //validar que la cantidad a eliminar no se tenga que obtener de los congelados
                        if ((nus.InventarioBuenEstado - nus.InventarioCongelado) - cantidad < 0) 
                        {
                             throw new ExcepcionNumeroUnicoMovimiento(MensajesError.Excepcion_MovimientoImposibleEliminar);
                        }

                        //Si segmento A entonces se obtiene la cantidad recibida.
                        if (numeroUnicoMovimiento.Segmento == "A")
                        {
                            if ((nui.CantidadRecibida + numOtrasEntradas - cantidad - cantidadDanada) < (cantidadDespachada + cantidadCongelada + numOtrasSalidas + mermas + salidasPorSegmentacion))
                            {
                                throw new ExcepcionNumeroUnicoMovimiento(MensajesError.Excepcion_MovimientoImposibleEliminar);
                            }
                        }
                        else
                        {
                            if ((entradaPorSegmentacion + numOtrasEntradas - cantidad - cantidadDanada) < (cantidadDespachada + cantidadCongelada + numOtrasSalidas + mermas + salidasPorSegmentacion))
                            {
                                throw new ExcepcionNumeroUnicoMovimiento(MensajesError.Excepcion_MovimientoImposibleEliminar);
                            }
                        }
                           
                    }
                }

                return true;

            }
            
        }

        public void EliminaMovimientoInventario(int numeroUnicoMovimientoID, Guid userId)
        {
            using (SamContext ctx = new SamContext())
            {
                NumeroUnicoMovimiento mov = ctx.NumeroUnicoMovimiento.Include("TipoMovimiento").Single(x => x.NumeroUnicoMovimientoID == numeroUnicoMovimientoID);
                NumeroUnico numUnico = ctx.NumeroUnico.Include("NumeroUnicoInventario").Include("NumeroUnicoSegmento").Single(x => x.NumeroUnicoID == mov.NumeroUnicoID);
                NumeroUnicoInventario nui = numUnico.NumeroUnicoInventario;                
                NumeroUnicoSegmento nus = numUnico.NumeroUnicoSegmento.Where(x => x.Segmento == mov.Segmento).SingleOrDefault();
                ItemCode ic = ItemCodeBO.Instance.ObtenerConTipoMaterial(numUnico.ItemCodeID.Value);
                bool esTubo = ic.TipoMaterialID == (int)TipoMaterialEnum.Tubo;
                int cantidadEliminada = mov.TipoMovimiento.EsEntrada ? mov.Cantidad : -mov.Cantidad;
                
                nui.StartTracking();
                nui.ProyectoID = numUnico.ProyectoID;                               
                nui.InventarioFisico = nui.InventarioFisico - cantidadEliminada;
                nui.InventarioBuenEstado = nui.InventarioFisico - nui.CantidadDanada;               
                nui.InventarioDisponibleCruce = nui.InventarioBuenEstado - nui.InventarioCongelado;
                nui.UsuarioModifica = userId;
                nui.FechaModificacion = DateTime.Now;
                

                if (esTubo)
                {

                    //NumeroUnicoSegmento    
                    if (nus != null)
                    {

                        nus.StartTracking();
                        nus.InventarioFisico = nus.InventarioFisico - cantidadEliminada;
                        nus.InventarioBuenEstado = nus.InventarioFisico - nui.CantidadDanada;                        
                        nus.InventarioDisponibleCruce = nus.InventarioBuenEstado - nus.InventarioCongelado;
                        nus.UsuarioModifica = userId;
                        nus.FechaModificacion = DateTime.Now;
                        nus.StopTracking();
                        ctx.NumeroUnicoSegmento.ApplyChanges(nus);
                    }
                    
                    
                }
                nui.StopTracking();

                ctx.NumeroUnicoMovimiento.DeleteObject(mov);
                ctx.NumeroUnicoInventario.ApplyChanges(nui);                
                ctx.NumeroUnico.ApplyChanges(numUnico);
                ctx.SaveChanges();
             }            
        }

    }
}
