using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using SAM.Entities.Personalizadas;
using SAM.BusinessLogic.Produccion;

namespace SAM.BusinessLogic.Homologacion
{
    public class HomologacionBL
    {
        private static readonly object _mutex = new object();
        private static HomologacionBL _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private HomologacionBL()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase IngenieriaBL
        /// </summary>
        /// <returns></returns>
        public static HomologacionBL Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new HomologacionBL();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// El material cambia de cantidad (menor)
        /// Si el material sólo se encuentra congelado, ésta cantidad debe modificarse en el inventario del número único congelado para que éste sea consistente con la nueva cantidad.
        /// </summary>
        /// <param name="ctx">Contexto que se abrió para la transaccion</param>
        /// <param name="material">MaterialPendientePorHomologar</param>
        public void MaterialCambiaCantidadCongelado(SamContext ctx, MaterialPendientePorHomologar material, int tipoMaterialID, Guid userID)
        {
            //Se modifica la cantidad en OrdenTrabajoMaterial
            //Se modifica el inventario del número unico congelado (si es tubo tambien hay que modificar al segmento)

            int diferenciaCongelado = 0;
            MaterialSpoolPendiente pendiente = ctx.MaterialSpoolPendiente.Where(x => x.MaterialSpoolPendienteID == material.MaterialSpoolPendienteID).SingleOrDefault();

            OrdenTrabajoMaterial otm = ctx.OrdenTrabajoMaterial.Where(x => x.MaterialSpoolID == material.MaterialSpoolID).SingleOrDefault();
            diferenciaCongelado = otm.CantidadCongelada.Value - pendiente.Cantidad;

            otm.StartTracking();
            otm.CantidadCongelada = pendiente.Cantidad;
            otm.UsuarioModifica = userID;
            otm.FechaModificacion = DateTime.Now;

            NumeroUnico numUnicoCongelado = ctx.NumeroUnico.Include("NumeroUnicoInventario").Include("NumeroUnicoSegmento").Where(x => x.NumeroUnicoID == otm.NumeroUnicoCongeladoID).SingleOrDefault();
            numUnicoCongelado.StartTracking();
            numUnicoCongelado.NumeroUnicoInventario.StartTracking();
            numUnicoCongelado.NumeroUnicoInventario.InventarioCongelado -= diferenciaCongelado;
            numUnicoCongelado.NumeroUnicoInventario.InventarioDisponibleCruce = numUnicoCongelado.NumeroUnicoInventario.InventarioBuenEstado - numUnicoCongelado.NumeroUnicoInventario.InventarioCongelado;
            numUnicoCongelado.NumeroUnicoInventario.UsuarioModifica = userID;
            numUnicoCongelado.NumeroUnicoInventario.FechaModificacion = DateTime.Now;

            if (tipoMaterialID == (int)TipoMaterialEnum.Tubo)
            {
                NumeroUnicoSegmento numCongeladoSegmento = numUnicoCongelado.NumeroUnicoSegmento.Where(x => x.Segmento == otm.SegmentoCongelado).SingleOrDefault();
                numCongeladoSegmento.StartTracking();
                numCongeladoSegmento.InventarioCongelado -= diferenciaCongelado;
                numCongeladoSegmento.InventarioDisponibleCruce = numCongeladoSegmento.InventarioBuenEstado - numCongeladoSegmento.InventarioCongelado;
                numUnicoCongelado.UsuarioModifica = userID;
                numUnicoCongelado.FechaModificacion = DateTime.Now;

                ctx.NumeroUnicoSegmento.ApplyChanges(numCongeladoSegmento);
            }

            ctx.OrdenTrabajoMaterial.ApplyChanges(otm);
            ctx.NumeroUnicoInventario.ApplyChanges(numUnicoCongelado.NumeroUnicoInventario);

        }

        /// <summary>
        /// Si el material se encuentra despachado se debe regresar a inventario la diferencia de lo que se homologa.
        /// </summary>
        /// <param name="ctx">Contexto que se abrió para la transacción</param>
        /// <param name="material">MaterialPendientePorHomologar</param>
        /// <param name="tipoMaterialID">Tipo de material</param>
        /// <param name="userID">Usuario Logeado</param>
        public void MaterialCambiaCantidadDespachado(SamContext ctx, MaterialPendientePorHomologar material, int tipoMaterialID, Guid userID)
        {
            //Se modifica la cantidad despachada en OrdenTrabajoMaterial


            int diferencia = 0;

            MaterialSpoolPendiente pendiente = ctx.MaterialSpoolPendiente.Where(x => x.MaterialSpoolPendienteID == material.MaterialSpoolPendienteID).SingleOrDefault();

            OrdenTrabajoMaterial otm = ctx.OrdenTrabajoMaterial.Where(x => x.MaterialSpoolID == material.MaterialSpoolID).SingleOrDefault();
            diferencia = otm.CantidadDespachada.Value - pendiente.Cantidad;

            otm.StartTracking();
            otm.CantidadDespachada = pendiente.Cantidad;
            otm.UsuarioModifica = userID;
            otm.FechaModificacion = DateTime.Now;

            //Se regresa a inventario la cantidad sobrante 
            NumeroUnico numUnicoDespachado = ctx.NumeroUnico.Include("NumeroUnicoInventario").Include("NumeroUnicoSegmento").Where(x => x.NumeroUnicoID == otm.NumeroUnicoCongeladoID).SingleOrDefault();
            numUnicoDespachado.StartTracking();
            numUnicoDespachado.NumeroUnicoInventario.StartTracking();
            numUnicoDespachado.NumeroUnicoInventario.InventarioFisico += diferencia;
            numUnicoDespachado.NumeroUnicoInventario.InventarioBuenEstado = numUnicoDespachado.NumeroUnicoInventario.InventarioFisico - numUnicoDespachado.NumeroUnicoInventario.CantidadDanada;
            numUnicoDespachado.NumeroUnicoInventario.InventarioDisponibleCruce = numUnicoDespachado.NumeroUnicoInventario.InventarioBuenEstado - numUnicoDespachado.NumeroUnicoInventario.InventarioCongelado;
            numUnicoDespachado.NumeroUnicoInventario.UsuarioModifica = userID;
            numUnicoDespachado.NumeroUnicoInventario.FechaModificacion = DateTime.Now;

            if (tipoMaterialID == (int)TipoMaterialEnum.Tubo)
            {
                //Al número único se le genera un segmento nuevo.
               
                GeneraSegmento(ctx, diferencia, otm.SegmentoDespachado.ToCharArray(0,1)[0], numUnicoDespachado, userID);
                ctx.NumeroUnico.ApplyChanges(numUnicoDespachado);
            }

            //Se actualiza el registro del despacho
            Despacho despacho = ctx.Despacho.Where(x => x.DespachoID == otm.DespachoID).SingleOrDefault();
            despacho.StartTracking();
            despacho.Cantidad = pendiente.Cantidad;
            despacho.UsuarioModifica = userID;
            despacho.FechaModificacion = DateTime.Now;

            //Se actualiza el registro del movimiento de salida del despacho            
            NumeroUnicoMovimiento movimiento = ctx.NumeroUnicoMovimiento.Where(x => x.NumeroUnicoMovimientoID == despacho.SalidaInventarioID).SingleOrDefault();
            string referenciaCambio = string.Format("Actualización por homologación: Cantidad Anterior = {0}", movimiento.Cantidad);
            movimiento.Cantidad = pendiente.Cantidad;
            movimiento.Referencia = string.IsNullOrEmpty(movimiento.Referencia) ? referenciaCambio : movimiento.Referencia + "\n" + referenciaCambio;
            movimiento.UsuarioModifica = userID;
            movimiento.FechaModificacion = DateTime.Now;

            ctx.NumeroUnicoMovimiento.ApplyChanges(movimiento);
            ctx.Despacho.ApplyChanges(despacho);
            ctx.OrdenTrabajoMaterial.ApplyChanges(otm);
            ctx.NumeroUnicoInventario.ApplyChanges(numUnicoDespachado.NumeroUnicoInventario);
        }

        /// <summary>
        /// Genera un nuevo segmento para el material que se regresa a inventario.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="cantidad"></param>
        /// <param name="segmento"></param>
        /// <param name="numeroUnico"></param>
        /// <param name="userID"></param>
        public void GeneraSegmento(SamContext ctx, int cantidad, char segmento, NumeroUnico numeroUnico, Guid userID)
        {
            char siguienteSegmento = (char)(((int)segmento) + 1);

            NumeroUnicoSegmento nusn = new NumeroUnicoSegmento();
            nusn.NumeroUnicoID = numeroUnico.NumeroUnicoID;
            nusn.ProyectoID = numeroUnico.ProyectoID;
            nusn.Segmento = siguienteSegmento.ToString();
            nusn.CantidadDanada = 0;
            nusn.InventarioFisico = cantidad;
            nusn.InventarioBuenEstado = nusn.InventarioFisico - nusn.CantidadDanada;
            nusn.InventarioCongelado = 0;
            nusn.InventarioDisponibleCruce = nusn.InventarioBuenEstado - nusn.InventarioCongelado;
            nusn.UsuarioModifica = userID;
            nusn.FechaModificacion = DateTime.Now;
            
            NumeroUnicoMovimiento numn = new NumeroUnicoMovimiento()
            {
                TipoMovimientoID = (int)TipoMovimientoEnum.EntradasSegmentacion,
                Cantidad = cantidad,
                Segmento = siguienteSegmento.ToString(),
                NumeroUnicoID = numeroUnico.NumeroUnicoID,
                ProyectoID = numeroUnico.ProyectoID,
                FechaMovimiento = DateTime.Now,
                Referencia = "Segmento por Homologación",
                Estatus = "A",
                UsuarioModifica = userID,
                FechaModificacion = DateTime.Now,
            };

            numeroUnico.NumeroUnicoSegmento.Add(nusn);
            numeroUnico.NumeroUnicoMovimiento.Add(numn);

        }

        /// <summary>
        /// Cuando una junta que ya se encontraba armada y/o soldada tuvo homologación y no hay material para abastecer alguno de sus materiales se debe cortar.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="juntaSpoolID"></param>
        public void GeneraCorteJunta(SamContext ctx, int juntaSpoolID, Guid userId)
        {
            JuntaWorkstatus juntaWks = ctx.JuntaWorkstatus.Where(x => x.JuntaSpoolID == juntaSpoolID && x.JuntaFinal).SingleOrDefault();
            JuntaWorkstatusBL.Instance.Cortar(juntaWks.JuntaWorkstatusID, userId);
        }

    }
}
