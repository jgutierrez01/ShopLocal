using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Excepciones;
using System.Data;
using Mimo.Framework.Common;
using SAM.BusinessLogic.Administracion;

namespace SAM.BusinessLogic.Workstatus
{
    public class DespachoBL
    {
        private static readonly object _mutex = new object();
        private static DespachoBL _instance;

        /// <summary>
        /// constructro privado para implementar patron singleton
        /// </summary>
        private DespachoBL()
        {
        }

        /// <summary>
        /// obtiene la instancia de la clase DespachoBL
        /// </summary>
        public static DespachoBL Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new DespachoBL();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Cancela el despacho
        /// </summary>
        /// <param name="despachoID">ID del despacho a cancelar</param>
        /// <param name="userID">ID del usuario que ejecuta el proceso.</param>
        public void CancelaDespachoPorDespachoID(int despachoID, Guid userID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Despacho despacho = ctx.Despacho.Where(x => x.DespachoID == despachoID).Single();

                    OrdenTrabajoMaterial otm = ctx.OrdenTrabajoMaterial
                                                  .Where(x => x.DespachoID == despacho.DespachoID)
                                                  .Single();

                    cancelaDespacho(userID, ctx, otm, despacho, true);
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_Concurrencia);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordenTrabajoMaterialID"></param>
        /// <param name="userID"></param>
        public void CancelaDespachoPorOrdenTrabajoMaterialID(int ordenTrabajoMaterialID, Guid userID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    CancelaDespachoPorOrdenTrabajoMaterialID(ctx, ordenTrabajoMaterialID, userID, true);
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_Concurrencia);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="ordenTrabajoMaterialID"></param>
        /// <param name="userID"></param>
        /// <param name="afectaBD"></param>
        public void CancelaDespachoPorOrdenTrabajoMaterialID(SamContext ctx, int ordenTrabajoMaterialID, Guid userID, bool afectaBD)
        {
            OrdenTrabajoMaterial otm = ctx.OrdenTrabajoMaterial
                                            .Where(x => x.OrdenTrabajoMaterialID == ordenTrabajoMaterialID)
                                            .Single();

            Despacho despacho = ctx.Despacho.Where(x => x.DespachoID == otm.DespachoID).Single();

            cancelaDespacho(userID, ctx, otm, despacho, afectaBD);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="ctx"></param>
        /// <param name="otm"></param>
        /// <param name="despacho"></param>
        private void cancelaDespacho(Guid userID, SamContext ctx, OrdenTrabajoMaterial otm, Despacho despacho, bool afectaBD)
        {
            otm.StartTracking();
            otm.FechaModificacion = DateTime.Now;
            otm.UsuarioModifica = userID;

            despacho.StartTracking();            
            despacho.UsuarioModifica = userID;
            despacho.FechaModificacion = DateTime.Now;

            //Verifico que exita algun registro en JuntaWorkstatus con Armado/Soldadura relacionado con el despacho
            IQueryable<JuntaWorkstatus> juntaWks = ctx.JuntaWorkstatus
                                                      .Where(x => despacho.OrdenTrabajoSpoolID == x.OrdenTrabajoSpoolID && (x.ArmadoAprobado || x.SoldaduraAprobada) && x.JuntaFinal);
            //Obtengo el material despachado
            MaterialSpool materialSpool = ctx.MaterialSpool.Where(x => x.MaterialSpoolID == despacho.MaterialSpoolID).Single();

            if (juntaWks.Count() > 0)
            {
                //Obtengo las juntaSpool que ya han sido armadas y/o soldadas y que correponden a la misma orden de trabajo del despacho
                List<JuntaSpool> juntaSpool = ctx.JuntaSpool.Where(x => juntaWks.Select(y => y.JuntaSpoolID).Contains(x.JuntaSpoolID)).ToList();

                
                

                //Verifico que el material del despacho corresponda a una junta ya armada y/o soldada
                if (EtiquetasMaterialUtil.ComparaMaterialConJuntas(materialSpool, juntaSpool))
                {
                    throw new ExcepcionDespacho(MensajesError.Excepcion_DespachoConWorkstatusIniciado);
                }
            }
            
            despacho.Cancelado = true;            
            despacho.StopTracking();
            ctx.Despacho.ApplyChanges(despacho);

            ItemCode ic = ctx.ItemCode.Single(x => x.ItemCodeID == despacho.MaterialSpool.ItemCodeID);
            

            if (ic.TipoMaterialID != (int)TipoMaterialEnum.Accessorio)
            {
                // SI ES TUBO 

                CorteDetalle detalle = ctx.CorteDetalle.Where(x => x.CorteDetalleID == otm.CorteDetalleID).Single();
                Corte oCorte = ctx.Corte.Where(x => x.CorteID == detalle.CorteID).Single();
                NumeroUnicoCorte oNumeroUnicoCorte = ctx.NumeroUnicoCorte.Where(x => x.NumeroUnicoCorteID == oCorte.NumeroUnicoCorteID).Single();

                //Valida que no exista otro despacho a corte posterior para el numero unico
                if (ctx.NumeroUnicoCorte.Where(dcorte =>
                     dcorte.NumeroUnicoID == oNumeroUnicoCorte.NumeroUnicoID
                     && dcorte.NumeroUnicoCorteID > oCorte.NumeroUnicoCorteID && dcorte.TieneCorte == false
                     ).Any())
                {
                    throw new Excepciones.ExcepcionCorte(MensajesError.Excepcion_NumeroUnicoConOtroDespachoCorte);
                }

                otm.TieneDespacho = false;
                otm.DespachoID = null;

                otm.NumeroUnicoCongeladoID = otm.NumeroUnicoDespachadoID;
                otm.SegmentoCongelado = otm.SegmentoDespachado;
                otm.CantidadCongelada = otm.CantidadDespachada;
                otm.TieneInventarioCongelado = true;
                otm.CongeladoEsEquivalente = otm.DespachoEsEquivalente;
                otm.CorteDetalleID = null;
                otm.TieneCorte = false;
                otm.DespachoEsEquivalente = false;
                otm.NumeroUnicoDespachadoID = null;
                otm.SegmentoDespachado = null;
                otm.CantidadDespachada = null;
                ctx.OrdenTrabajoMaterial.ApplyChanges(otm);


                //Si el cortedetalle es el ultimo aactivo del corte cancelamos tambien el corte
                if (ctx.CorteDetalle.Where(x => !x.Cancelado && x.CorteID == oCorte.CorteID).Count() == 1)
                {
                    Corte corte = detalle.Corte;

                    //Cancelamos el corte
                    corte.StartTracking();
                    corte.Cancelado = true;
                    corte.UsuarioModifica = userID;
                    corte.FechaModificacion = DateTime.Now;
                    corte.StopTracking();
                    ctx.Corte.ApplyChanges(corte);

                    //Cancelamos los movimientos 
                    List<NumeroUnicoMovimiento> movimientos = ctx.NumeroUnicoMovimiento.Where(x => x.NumeroUnicoMovimientoID == corte.MermaMovimientoID
                        || x.NumeroUnicoMovimientoID == corte.PreparacionCorteMovimientoID || x.NumeroUnicoMovimientoID == corte.NumeroUnicoCorte.SalidaMovimientoID).ToList();
                    foreach (NumeroUnicoMovimiento mov in movimientos)
                    {
                        mov.StartTracking();
                        mov.Estatus = EstatusNumeroUnicoMovimiento.CANCELADO;
                        mov.UsuarioModifica = userID;
                        mov.FechaModificacion = DateTime.Now;
                        mov.StopTracking();
                        ctx.NumeroUnicoMovimiento.ApplyChanges(mov);
                    }

                    int merma = 0;
                    if (corte.Merma != null)
                        merma = corte.Merma.Value;

                    //Actualizamos inventario
                    NumeroUnicoSegmento segmento = ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == corte.NumeroUnicoCorte.NumeroUnicoID && x.Segmento == corte.NumeroUnicoCorte.Segmento).Single();
                    segmento.StartTracking();
                    segmento.InventarioFisico += detalle.Cantidad + merma;
                    segmento.InventarioBuenEstado += detalle.Cantidad + merma;
                    segmento.InventarioCongelado += detalle.Cantidad;
                    segmento.InventarioDisponibleCruce = segmento.InventarioBuenEstado - segmento.InventarioCongelado;
                    segmento.UsuarioModifica = userID;
                    segmento.FechaModificacion = DateTime.Now;
                    segmento.StopTracking();
                    ctx.NumeroUnicoSegmento.ApplyChanges(segmento);

                    NumeroUnicoInventario inventario = ctx.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == corte.NumeroUnicoCorte.NumeroUnicoID).Single();
                    inventario.StartTracking();
                    inventario.InventarioFisico += detalle.Cantidad + merma;
                    inventario.InventarioBuenEstado += detalle.Cantidad + merma;
                    inventario.InventarioCongelado += detalle.Cantidad;
                    inventario.InventarioDisponibleCruce = inventario.InventarioBuenEstado - inventario.InventarioCongelado;
                    inventario.UsuarioModifica = userID;
                    inventario.FechaModificacion = DateTime.Now;
                    inventario.StopTracking();
                    ctx.NumeroUnicoInventario.ApplyChanges(inventario);
                }
                else
                {
                    //Actualizamos inventario
                    NumeroUnicoSegmento segmento = ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == detalle.Corte.NumeroUnicoCorte.NumeroUnicoID && x.Segmento == detalle.Corte.NumeroUnicoCorte.Segmento).Single();
                    segmento.StartTracking();
                    segmento.InventarioFisico += detalle.Cantidad;
                    segmento.InventarioBuenEstado += detalle.Cantidad;
                    segmento.InventarioCongelado += detalle.Cantidad;
                    segmento.InventarioDisponibleCruce = segmento.InventarioBuenEstado - segmento.InventarioCongelado;
                    segmento.UsuarioModifica = userID;
                    segmento.FechaModificacion = DateTime.Now;
                    segmento.StopTracking();
                    ctx.NumeroUnicoSegmento.ApplyChanges(segmento);

                    NumeroUnicoInventario inventario = ctx.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == detalle.Corte.NumeroUnicoCorte.NumeroUnicoID).Single();
                    inventario.StartTracking();
                    inventario.InventarioFisico += detalle.Cantidad;
                    inventario.InventarioBuenEstado += detalle.Cantidad;
                    inventario.InventarioCongelado += detalle.Cantidad;
                    inventario.InventarioDisponibleCruce = inventario.InventarioBuenEstado - inventario.InventarioCongelado;
                    inventario.UsuarioModifica = userID;
                    inventario.FechaModificacion = DateTime.Now;
                    inventario.StopTracking();
                    ctx.NumeroUnicoInventario.ApplyChanges(inventario);
                }

                detalle.StartTracking();
                detalle.Cancelado = true;
                detalle.UsuarioModifica = userID;
                detalle.FechaModificacion = DateTime.Now;
                detalle.StopTracking();
                ctx.CorteDetalle.ApplyChanges(detalle);

                //Cancelamos el movimiento 
                NumeroUnicoMovimiento movimiento = ctx.NumeroUnicoMovimiento.Where(x => x.NumeroUnicoMovimientoID == detalle.SalidaInventarioID).Single();

                movimiento.StartTracking();
                movimiento.Estatus = EstatusNumeroUnicoMovimiento.CANCELADO;
                movimiento.UsuarioModifica = userID;
                movimiento.FechaModificacion = DateTime.Now;
                movimiento.StopTracking();
                ctx.NumeroUnicoMovimiento.ApplyChanges(movimiento);
            }
            else
            {
                int numeroUnicoID = otm.NumeroUnicoDespachadoID.Value;
                int cantidadDespachada = otm.CantidadDespachada.Value;
           
                NumeroUnico nu = ctx.NumeroUnico.Where(x => x.NumeroUnicoID == numeroUnicoID).Single();
                NumeroUnicoInventario nui = ctx.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == numeroUnicoID).Single();
                NumeroUnicoMovimiento num = ctx.NumeroUnicoMovimiento.Single(x => x.NumeroUnicoMovimientoID == despacho.SalidaInventarioID);

                num.StartTracking();
                num.FechaModificacion = DateTime.Now;
                num.UsuarioModifica = userID;

                nui.StartTracking();
                nui.FechaModificacion = DateTime.Now;
                nui.UsuarioModifica = userID;

                nu.StartTracking();
                nu.FechaModificacion = DateTime.Now;
                nu.UsuarioModifica = userID;

                nui.InventarioFisico += cantidadDespachada;
                nui.InventarioBuenEstado = nui.InventarioFisico - nui.CantidadDanada;
                nui.InventarioCongelado += cantidadDespachada;
                nui.InventarioDisponibleCruce = nui.InventarioBuenEstado - nui.InventarioCongelado;
                                
                otm.TieneDespacho = false;
                otm.DespachoID = null;
                otm.TieneInventarioCongelado = true;
                otm.NumeroUnicoCongeladoID = numeroUnicoID;
                otm.CantidadCongelada = cantidadDespachada;
                otm.CongeladoEsEquivalente = despacho.EsEquivalente;                
                otm.DespachoEsEquivalente = false;
                otm.NumeroUnicoDespachadoID = null;
                otm.CantidadDespachada = 0;

                num.Estatus = EstatusNumeroUnicoMovimiento.CANCELADO;
                
                nui.StopTracking();
                nu.StopTracking();
                num.StopTracking();

                ctx.NumeroUnicoMovimiento.ApplyChanges(num);
                ctx.NumeroUnicoInventario.ApplyChanges(nui);
                ctx.NumeroUnico.ApplyChanges(nu);

            }

            ctx.OrdenTrabajoMaterial.ApplyChanges(otm);

            if (afectaBD)
            {
                ctx.SaveChanges();
            }
        }
    }
}
