using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.Entities.Personalizadas;
using SAM.BusinessObjects.Modelo;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Utilerias;
using System.Data.Objects;
using SAM.Entities.Cache;
using SAM.BusinessObjects.Materiales;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;
using System.Globalization;

namespace SAM.BusinessObjects.Workstatus
{
    public class DespachoBO
    {
        private static readonly object _mutex = new object();
        private static DespachoBO _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private DespachoBO()
        {
        }

        public static DespachoBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new DespachoBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtiene el listado de despachos realizados en el proyecto para el numero de control especificado
        /// </summary>
        /// <param name="proyectoID">ID del proyecto</param>
        /// <param name="numeroControl">Numero de Control sobre el que se desea realizar la busqueda de despachos</param>
        /// <returns></returns>
        public List<GrdDespacho> ObtenerListaDespachosPorODTS(int proyectoID, int ordenTrabajoSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<Despacho> query = ctx.Despacho.Where(x => x.ProyectoID == proyectoID && x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID).AsQueryable();
                OrdenTrabajo ordenTrabajo = (from ots in ctx.OrdenTrabajoSpool
                                             join ot in ctx.OrdenTrabajo on ots.OrdenTrabajoID equals ot.OrdenTrabajoID
                                             where ots.OrdenTrabajoSpoolID == ordenTrabajoSpoolID
                                             select ot).Single();

                List<GrdDespacho> listado = (from despacho in query
                                             select new GrdDespacho
                                             {
                                                 DespachoID = despacho.DespachoID,
                                                 FechaDespacho = despacho.FechaDespacho,
                                                 NumeroOrden = ordenTrabajo.NumeroOrden,
                                                 NumeroControl = despacho.OrdenTrabajoSpool.NumeroControl,
                                                 Etiqueta = despacho.MaterialSpool.Etiqueta,
                                                 ItemCode = despacho.MaterialSpool.ItemCode.Codigo,
                                                 Descripcion = despacho.MaterialSpool.ItemCode.DescripcionEspanol,
                                                 EsEquivalente = despacho.EsEquivalente,
                                                 NumeroUnico = despacho.NumeroUnico.Codigo,
                                                 Cantidad = despacho.Cantidad,
                                                 Cancelado = despacho.Cancelado
                                             }).ToList();

                listado.ForEach(d => d.Estatus = TraductorEnumeraciones.TextoCanceladoActivo(!d.Cancelado));

                return listado;
            }
        }

        public List<GrdDespacho> ObtenerListaDespachosPorNumeroUnico(int proyectoID, int numeroUnicoID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<Despacho> iqDespacho = ctx.Despacho.Where(x => x.ProyectoID == proyectoID && x.NumeroUnicoID == numeroUnicoID);

                List<GrdDespacho> listado = (from despacho in iqDespacho
                                             join ots in ctx.OrdenTrabajoSpool on despacho.OrdenTrabajoSpoolID equals ots.OrdenTrabajoSpoolID
                                             join ot in ctx.OrdenTrabajo on ots.OrdenTrabajoID equals ot.OrdenTrabajoID
                                             select new GrdDespacho
                                             {
                                                 DespachoID = despacho.DespachoID,
                                                 FechaDespacho = despacho.FechaDespacho,
                                                 NumeroOrden = ot.NumeroOrden,
                                                 NumeroControl = despacho.OrdenTrabajoSpool.NumeroControl,
                                                 Etiqueta = despacho.MaterialSpool.Etiqueta,
                                                 ItemCode = despacho.MaterialSpool.ItemCode.Codigo,
                                                 Descripcion = despacho.MaterialSpool.ItemCode.DescripcionEspanol,
                                                 EsEquivalente = despacho.EsEquivalente,
                                                 NumeroUnico = despacho.NumeroUnico.Codigo,
                                                 Cantidad = despacho.Cantidad,
                                                 Cancelado = despacho.Cancelado
                                             }).ToList();

                listado.ForEach(d => d.Estatus = TraductorEnumeraciones.TextoCanceladoActivo(!d.Cancelado));

                return listado;
            }
        }

        public List<GrdDespacho> ObtenerListaDespachosPorNumeroUnicoYODT(int proyectoID, int numeroUnicoID, int ordenTrabajoSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<Despacho> iqDespacho = ctx.Despacho.Where(x => x.ProyectoID == proyectoID && x.NumeroUnicoID == numeroUnicoID && x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID);
                IQueryable<OrdenTrabajoSpool> iqOdts = ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID);

                List<GrdDespacho> listado = (from despacho in iqDespacho
                                             join ots in iqOdts on despacho.OrdenTrabajoSpoolID equals ots.OrdenTrabajoSpoolID
                                             join ot in ctx.OrdenTrabajo on ots.OrdenTrabajoID equals ot.OrdenTrabajoID
                                             select new GrdDespacho
                                             {
                                                 DespachoID = despacho.DespachoID,
                                                 FechaDespacho = despacho.FechaDespacho,
                                                 NumeroOrden = ot.NumeroOrden,
                                                 NumeroControl = despacho.OrdenTrabajoSpool.NumeroControl,
                                                 Etiqueta = despacho.MaterialSpool.Etiqueta,
                                                 ItemCode = despacho.MaterialSpool.ItemCode.Codigo,
                                                 Descripcion = despacho.MaterialSpool.ItemCode.DescripcionEspanol,
                                                 EsEquivalente = despacho.EsEquivalente,
                                                 NumeroUnico = despacho.NumeroUnico.Codigo,
                                                 Cantidad = despacho.Cantidad,
                                                 Cancelado = despacho.Cancelado
                                             }).ToList();

                listado.ForEach(d => d.Estatus = TraductorEnumeraciones.TextoCanceladoActivo(!d.Cancelado));

                return listado;
            }
        }


        /// <summary>
        /// Obtiene el despacho y todas las relaciones para el detalle del mismo
        /// </summary>
        /// <param name="despachoID">ID del despacho</param>
        /// <returns>Despacho, MaterialSpool, MaterialSpool.ItemCode, MaterialSpool.OrdenTrabajoMaterial, NumeroUnico, NumeroUnico.ItemCode </returns>
        public Despacho ObtenDespachoDetalle(int despachoID)
        {
            using (SamContext ctx = new SamContext())
            {
                Despacho despacho = ctx.Despacho.Where(x => x.DespachoID == despachoID).Single();

                ctx.LoadProperty<Despacho>(despacho, x => x.MaterialSpool);
                ctx.LoadProperty<MaterialSpool>(despacho.MaterialSpool, x => x.OrdenTrabajoMaterial);
                ctx.LoadProperty<MaterialSpool>(despacho.MaterialSpool, x => x.ItemCode);

                ctx.LoadProperty<Despacho>(despacho, x => x.NumeroUnico);
                ctx.LoadProperty<NumeroUnico>(despacho.NumeroUnico, x => x.ItemCode);

                return despacho;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="despachoID"></param>
        /// <returns></returns>
        public int ObtenerProyectoID(int despachoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ProyectoDespacho(ctx, despachoID);
            }
        }


        /// <summary>
        /// Versión compilada del query para permisos de despacho
        /// </summary>
        private static readonly Func<SamContext, int, int> ProyectoDespacho =
        CompiledQuery.Compile<SamContext, int, int>
        (
            (ctx, id) => ctx.Despacho
                            .Where(x => x.DespachoID == id)
                            .Select(x => x.ProyectoID)
                            .Single()
        );

        /// <summary>
        /// Obtiene un listado de los numeros unicos para la edicion especial de un despacho
        /// </summary>
        /// <param name="numeroUnicoID"></param>
        /// <param name="cantidad"></param>
        /// <param name="buscar">texto con fragmento del código de un número único</param>
        /// <returns>Listoado de numeros unicos</returns>
        public List<NumeroUnicoEdicionEspecial> ObtenerNumerosUnicosEdicionEspecialDespacho(int numeroUnicoID, int cantidad, string buscar)
        {
            using (SamContext ctx = new SamContext())
            {
                NumeroUnico numUnico = NumeroUnicoBO.Instance.Obtener(numeroUnicoID);
                List<NumeroUnicoEdicionEspecial> numerosUnicos = new List<NumeroUnicoEdicionEspecial>();

                numerosUnicos.AddRange((from numeros in ctx.NumeroUnico
                                        join inventario in ctx.NumeroUnicoInventario on numeros.NumeroUnicoID equals inventario.NumeroUnicoID
                                        where numeros.ItemCodeID == numUnico.ItemCodeID
                                        && numeros.Diametro1 == numUnico.Diametro1
                                        && numeros.Diametro2 == numUnico.Diametro2
                                        && (inventario.InventarioFisico >= cantidad || numeros.NumeroUnicoID == numeroUnicoID)
                                        select new NumeroUnicoEdicionEspecial
                                        {
                                            Value = numeros.NumeroUnicoID,
                                            Text = numeros.Codigo
                                        }).Distinct().OrderBy(x => x.Text).ToList());

                numerosUnicos = numerosUnicos.Where(x => x.Text.StartsWith(buscar, StringComparison.InvariantCultureIgnoreCase)).ToList();

                return numerosUnicos;
            }
        }

        /// <summary>
        /// Guarda los cambios echos por una edicion especial de despacho.
        /// </summary>
        /// <param name="DetalleDespacho"></param>
        /// <param name="nuevoNumUnicoID"></param>
        /// <param name="userID"></param>
        /// <returns>exito o fallo ( true / false )</returns>
        public bool GuardarEdicionEspecialDespacho(Despacho DetalleDespacho, int nuevoNumUnicoID, Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                try
                {
                    #region variables
                    NumeroUnicoMovimiento movimientoInventario;
                    OrdenTrabajoMaterial ordenTrabajoMaterial;
                    NumeroUnicoInventario numeroUnicoOriginal;
                    NumeroUnicoInventario numeroUnicoNuevo;
                    NumeroUnicoSegmento segmentoOriginal;
                    NumeroUnicoSegmento segmentoNuevo;
                    Despacho despachoActualizar;
                    int nuevoIDMovimientoInventario = 0;
                    #endregion

                    //Verificamos si hubo cambio en el numero unico
                    if (DetalleDespacho.NumeroUnicoID != nuevoNumUnicoID)
                    {
                        //Preguntamos si es accesorio o tubo
                        if (DetalleDespacho.NumeroUnico.ItemCode.TipoMaterialID == 2)
                        {
                            #region Accesorio
                            //Obtenemos los datos del numero unico original con todo y sus inventarios
                            numeroUnicoOriginal = ctx.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == DetalleDespacho.NumeroUnicoID).Single();
                            //Obtenemos los datos del nuevo numero unico con sus inventarios
                            numeroUnicoNuevo = ctx.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == nuevoNumUnicoID).Single();
                            
                            //Es accesorio
                            //Primero regresamos el inventario del numero unico original
                            //
                            numeroUnicoOriginal.StartTracking();
                            //regresar la cantidad a InventarioFisico , la cantidad se toma del detalle del despacho
                            numeroUnicoOriginal.InventarioFisico = numeroUnicoOriginal.InventarioFisico + DetalleDespacho.Cantidad;
                            //actualizamos el inventario en buen estado restando el inventario fisico la cantidad dañada
                            numeroUnicoOriginal.InventarioBuenEstado =
                                numeroUnicoOriginal.InventarioFisico - numeroUnicoOriginal.CantidadDanada;
                            //Actualizamos el inventario disponible para cruce restando del inv. en buen estado el inv. que esta congelado
                            numeroUnicoOriginal.InventarioDisponibleCruce =
                                numeroUnicoOriginal.InventarioBuenEstado - numeroUnicoOriginal.InventarioCongelado;
                            //actualizamos fecha y usuario que modificarion los registros
                            numeroUnicoOriginal.UsuarioModifica = userID;
                            numeroUnicoOriginal.FechaModificacion = DateTime.Now;
                            //terminamos de rastrear los cambios en el numero unico original
                            numeroUnicoOriginal.StopTracking();
                            //aplicamos los cambios del numero unico original
                            ctx.NumeroUnicoInventario.ApplyChanges(numeroUnicoOriginal);

                            //
                            //
                            //Ahora afectamos el nuevo numero unico
                            //comenzamos a rastrear los cambios
                            numeroUnicoNuevo.StartTracking();
                            //restamos del inventario fisico la cantidad indicada en el detalle del despacho
                            numeroUnicoNuevo.InventarioFisico = numeroUnicoNuevo.InventarioFisico - DetalleDespacho.Cantidad;
                            //actualizamos el inventario en buen estado restando el inv. dañado al inventario fisico.
                            numeroUnicoNuevo.InventarioBuenEstado =
                                numeroUnicoNuevo.InventarioFisico - numeroUnicoNuevo.CantidadDanada;
                            //actualizamo el disponible para cruce restando el inv. congelado del inv. en buen estado
                            numeroUnicoNuevo.InventarioDisponibleCruce =
                                numeroUnicoNuevo.InventarioBuenEstado - numeroUnicoNuevo.InventarioCongelado;
                            //actualizamos usuario y fecha de modificaion del inventario del nuevo numero unico
                            numeroUnicoNuevo.UsuarioModifica = userID;
                            numeroUnicoNuevo.FechaModificacion = DateTime.Now;
                            //terminamos de rastrear los cambios
                            numeroUnicoNuevo.StopTracking();
                            //aplicamos cambios
                            ctx.NumeroUnicoInventario.ApplyChanges(numeroUnicoNuevo);

                            //actualizamos los campos del movimiento de inventario
                            //Obtenemos el objeto con el detalle del movimiento, esto buscando el inventario movimiento Id tomando en cuenta el campo Salida Inventario 
                            //registrado en el despacho.
                            //Obtenemos el objeto con el movimiento de inventario
                            movimientoInventario = ctx.NumeroUnicoMovimiento.Where(x => x.NumeroUnicoMovimientoID == DetalleDespacho.SalidaInventarioID).SingleOrDefault();

                            //verificamos si tenemos datos del ultimo movimiento o no
                            if (movimientoInventario == null)
                            {
                                ////si no existen datos registrados de acuerdo al id de salida de inventairo en despacho
                                ////creamos un nuevo registro en la tabla de numero unico movimiento
                                //movimientoInventario = new NumeroUnicoMovimiento();
                                ////seguimos los cambios
                                //movimientoInventario.StartTracking();
                                //movimientoInventario.FechaMovimiento = DateTime.Now;
                                //movimientoInventario.ProyectoID = DetalleDespacho.ProyectoID;
                                //movimientoInventario.Cantidad = DetalleDespacho.Cantidad;
                                //movimientoInventario.TipoMovimientoID = 10;
                                //movimientoInventario.Estatus = "A"; // Debe ser definido
                                ////actualizamos el numero unico
                                //movimientoInventario.NumeroUnicoID = numeroUnicoNuevo.NumeroUnicoID;
                                ////actualizamos el segmento
                                //movimientoInventario.Segmento = "";
                                ////actualizamos la referencia
                                //movimientoInventario.Referencia = "Edición especial: " + DateTime.Now.ToString();
                                ////actualizamos el usuario y fecha de modificacion
                                //movimientoInventario.UsuarioModifica = userID;
                                //movimientoInventario.FechaModificacion = DateTime.Now;
                                ////dejamos de seguir los cambios
                                //movimientoInventario.StopTracking();
                                ////aplicamos los cambios
                                //ctx.NumeroUnicoMovimiento.ApplyChanges(movimientoInventario);
                                return false;
                            }
                            else
                            {
                                //seguimos los cambios
                                movimientoInventario.StartTracking();
                                //actualizamos el numero unico
                                movimientoInventario.NumeroUnicoID = numeroUnicoNuevo.NumeroUnicoID;
                                //actualizamos el segmento
                                movimientoInventario.Segmento = "";
                                //actualizamos la referencia
                                string referencia = movimientoInventario.Referencia + " Edición especial: " + DateTime.Now.ToString();
                                movimientoInventario.Referencia = referencia.Length > 150 ? referencia.Substring(0, 149) : referencia; 
                                //actualizamos el usuario y fecha de modificacion
                                movimientoInventario.UsuarioModifica = userID;
                                movimientoInventario.FechaModificacion = DateTime.Now;
                                //dejamos de seguir los cambios
                                movimientoInventario.StopTracking();
                                //aplicamos los cambios
                                ctx.NumeroUnicoMovimiento.ApplyChanges(movimientoInventario);
                            }

                            //Actuzlizamos los datos del despacho
                            //Obtememos el objeto con los datos del despacho
                            despachoActualizar = ctx.Despacho.Where(x => x.DespachoID == DetalleDespacho.DespachoID).Single();
                            //comenzamos a seguir los cambios
                            despachoActualizar.StartTracking();
                            //Actualizamos el numero unico
                            despachoActualizar.NumeroUnicoID = nuevoNumUnicoID;
                            //actualizamos usurio y fecha de modificacion
                            despachoActualizar.UsuarioModifica = userID;
                            despachoActualizar.FechaModificacion = DateTime.Now;
                            //registramos el id de salida de inventario
                            if (!despachoActualizar.SalidaInventarioID.HasValue && nuevoIDMovimientoInventario != 0)
                            {
                                despachoActualizar.SalidaInventarioID = nuevoIDMovimientoInventario;
                            }
                            //dejamos de seguir los cambios
                            despachoActualizar.StopTracking();
                            //aplicamos cambios
                            ctx.Despacho.ApplyChanges(despachoActualizar);

                            //actualizamos los datos de la orden de trabajo matertial
                            //obtenemos el objeto de la orden de trabajo
                            ordenTrabajoMaterial = ctx.OrdenTrabajoMaterial.Where(x => x.DespachoID == DetalleDespacho.DespachoID).Single();
                            //seguimos los cambios
                            ordenTrabajoMaterial.StartTracking();
                            //actualizamos el numero unico despachado
                            ordenTrabajoMaterial.NumeroUnicoDespachadoID = nuevoNumUnicoID;
                            //actualizamos usuario y fecha de modificacion
                            ordenTrabajoMaterial.UsuarioModifica = userID;
                            ordenTrabajoMaterial.FechaModificacion = DateTime.Now;
                            //Pendiente, no se como identificar si un numero unico es equivalente.
                            ordenTrabajoMaterial.DespachoEsEquivalente = ctx.ItemCodeEquivalente.Where(x => x.ItemCodeID == DetalleDespacho.NumeroUnico.ItemCodeID ||
                                x.ItemCodeEquivalenteID == DetalleDespacho.NumeroUnico.ItemCodeID).Any();
                            //terminamos de rastrear los cambios del nuevo numero unico
                            ordenTrabajoMaterial.StopTracking();
                            //aplicamos los cambios
                            ctx.OrdenTrabajoMaterial.ApplyChanges(ordenTrabajoMaterial);


                            //guardamos los cambios
                            ctx.SaveChanges();

                            return true;
                            #endregion
                        }
                        else
                        {
                            #region Tubo
                            //Es tubo
                            //En este caso las modificaciones segun el Documento se haran en la tabla de numeroUnicoSegmento
                            //Recuperamos el segmento con el que se va a trabajar
                            string segmentoOrigen = DetalleDespacho.Segmento;
                            string segmentoDestino;

                            //recuperamos el objeto con el segmento original
                            segmentoOriginal = ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == DetalleDespacho.NumeroUnicoID
                                && x.Segmento == segmentoOrigen).Single();
                            //recuperamos el objeto del numero unico inventario original
                            numeroUnicoOriginal = ctx.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == DetalleDespacho.NumeroUnicoID).Single();

                            //ahora recuperamos el segmento nuevo, dependiendo de si tiene inventario fisico que sea mayor o igual a la cantidad requerida
                            //marcada en los datos del despacho
                            segmentoNuevo = ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == nuevoNumUnicoID 
                                && x.InventarioFisico >= DetalleDespacho.Cantidad).Single();
                            //obtenemos el nombre del segmento destino (A,B,C...)
                            segmentoDestino = segmentoNuevo.Segmento;

                            //recuperamos los datos del nuevo numero unico inventario
                            numeroUnicoNuevo = ctx.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == nuevoNumUnicoID).Single();

                            #region Regresar inventario
                            //Ahora regresamos los inventarios
                            //segmento original
                            //Comenzamos a seguir los cambios
                            segmentoOriginal.StartTracking();
                            //varificamos si existe inventario en transfeencia
                            if (segmentoOriginal.InventarioTransferenciaCorte > 0)
                            {
                                //Si existe inventario en transferencia se regresa la cantidad a este en vez del al inv. fisico
                                segmentoOriginal.InventarioTransferenciaCorte = segmentoOriginal.InventarioTransferenciaCorte + DetalleDespacho.Cantidad;
                                //actualizamos el inv. en buen estado
                                segmentoOriginal.InventarioBuenEstado = segmentoOriginal.InventarioTransferenciaCorte - segmentoOriginal.CantidadDanada;
                            }
                            else //No hay inv. en transferencia
                            {
                                //actualizamos el inventario fisico, sumamos la cantidad marcada en el despacho al inv. fisico
                                segmentoOriginal.InventarioFisico = segmentoOriginal.InventarioFisico + DetalleDespacho.Cantidad;
                                //actualizamos el inv. en buen estado
                                segmentoOriginal.InventarioBuenEstado = segmentoOriginal.InventarioFisico - segmentoOriginal.CantidadDanada;
                            }
                            
                            //actuializamos el inv. disponible para cruce
                            segmentoOriginal.InventarioDisponibleCruce = segmentoOriginal.InventarioBuenEstado - segmentoOriginal.InventarioCongelado;
                            //Actualizamos usuario y fecha de modificacion
                            segmentoOriginal.UsuarioModifica = userID;
                            segmentoOriginal.FechaModificacion = DateTime.Now;
                            //terminamos de seguir los camnios
                            segmentoOriginal.StopTracking();
                            //aplicamos cambios
                            ctx.NumeroUnicoSegmento.ApplyChanges(segmentoOriginal);

                            //Ahora el numero unico original
                            //comenzamos a seguir los cambios
                            numeroUnicoOriginal.StartTracking();
                            //varificamos si existe inventario en transfeencia
                            if (numeroUnicoOriginal.InventarioTransferenciaCorte > 0)
                            {
                                //Si existe inventario en transferencia se regresa la cantidad a este en vez del al inv. fisico
                                numeroUnicoOriginal.InventarioTransferenciaCorte = numeroUnicoOriginal.InventarioTransferenciaCorte + DetalleDespacho.Cantidad;
                                //actualizamos el inv. en buen estado
                                numeroUnicoOriginal.InventarioBuenEstado = numeroUnicoOriginal.InventarioTransferenciaCorte - numeroUnicoOriginal.CantidadDanada;
                            }
                            else //No hay inv. en transferencia
                            {
                                //actualizamos el inventario fisico, sumamos la cantidad marcada en el despacho al inv. fisico
                                numeroUnicoOriginal.InventarioFisico = numeroUnicoOriginal.InventarioFisico + DetalleDespacho.Cantidad;
                                //actualizamos el inv. en buen estado
                                numeroUnicoOriginal.InventarioBuenEstado = numeroUnicoOriginal.InventarioFisico - numeroUnicoOriginal.CantidadDanada;
                            }
                            //actualizamos el disponible para cruce
                            numeroUnicoOriginal.InventarioDisponibleCruce = numeroUnicoOriginal.InventarioBuenEstado - numeroUnicoOriginal.InventarioCongelado;
                            //actualizamos el usuario y la fecha de modificacion
                            numeroUnicoOriginal.UsuarioModifica = userID;
                            numeroUnicoOriginal.FechaModificacion = DateTime.Now;
                            //Terminamos de seguir los cambios en este objeto
                            numeroUnicoOriginal.StopTracking();
                            //aplicamos cambios
                            ctx.NumeroUnicoInventario.ApplyChanges(numeroUnicoOriginal);
                            #endregion

                            #region Afectar inventarios
                            //ahora hacemos el proceso inverso para el nuemo numero unico
                            //primero en el nuevo segmento
                            //empezamos a seguir los cambios
                            segmentoNuevo.StartTracking();
                            //varificamos si hay inventario en transferencia a corte
                            if (segmentoNuevo.InventarioTransferenciaCorte > 0)
                            {
                                //Actualizamos el inv. en tranferencia, restando la cantidad indicada por el despacho
                                segmentoNuevo.InventarioTransferenciaCorte = segmentoNuevo.InventarioTransferenciaCorte - DetalleDespacho.Cantidad;
                                //actualizamos el inv. en buen estado
                                segmentoNuevo.InventarioBuenEstado = segmentoNuevo.InventarioTransferenciaCorte - segmentoNuevo.CantidadDanada;
                            }
                            else // No hay inv. en transferencia
                            {
                                //actualizamos el inv. fisico restando la cantidad marcada en el despacho
                                segmentoNuevo.InventarioFisico = segmentoNuevo.InventarioFisico - DetalleDespacho.Cantidad;
                                //actualizamos el inv. en buen estado
                                segmentoNuevo.InventarioBuenEstado = segmentoNuevo.InventarioFisico - segmentoNuevo.CantidadDanada;
                            }
                            //actualizamos el disponible para cruce
                            segmentoNuevo.InventarioDisponibleCruce = segmentoNuevo.InventarioBuenEstado - segmentoNuevo.InventarioCongelado;
                            //actualizamos el usuario y fecha de modificacion
                            segmentoNuevo.UsuarioModifica = userID;
                            segmentoNuevo.FechaModificacion = DateTime.Now;
                            //terminamos de seguir los cambios 
                            segmentoNuevo.StopTracking();
                            //aplicamos los cambios 
                            ctx.NumeroUnicoSegmento.ApplyChanges(segmentoNuevo);

                            //actualizamos los datos del nuevo numero unico
                            //seguimos los cambios
                            numeroUnicoNuevo.StartTracking();
                            //Verificamos si existe inv. en transferencia
                            if (numeroUnicoNuevo.InventarioTransferenciaCorte > 0)
                            {
                                //Actualizamos el inv. en tranferencia, restando la cantidad indicada por el despacho
                                numeroUnicoNuevo.InventarioTransferenciaCorte = numeroUnicoNuevo.InventarioTransferenciaCorte - DetalleDespacho.Cantidad;
                                //actualizamos el inv. en buen estado
                                numeroUnicoNuevo.InventarioBuenEstado = numeroUnicoNuevo.InventarioTransferenciaCorte - numeroUnicoNuevo.CantidadDanada;
                            }
                            else // No hay inv. en tranferencia
                            {
                                //actualizamos el inv. fisico
                                numeroUnicoNuevo.InventarioFisico = numeroUnicoNuevo.InventarioFisico - DetalleDespacho.Cantidad;
                                //actualizamos el inv. en buen estado
                                numeroUnicoNuevo.InventarioBuenEstado = numeroUnicoNuevo.InventarioFisico - numeroUnicoNuevo.CantidadDanada;
                            }
                            //actualizamos el disponible para cruce
                            numeroUnicoNuevo.InventarioDisponibleCruce = numeroUnicoNuevo.InventarioBuenEstado - numeroUnicoNuevo.InventarioCongelado;
                            //actualizamos el usuario y la fecha de modificacion
                            numeroUnicoNuevo.UsuarioModifica = userID;
                            numeroUnicoNuevo.FechaModificacion = DateTime.Now;
                            //dejamos de seguir los cambios
                            numeroUnicoNuevo.StopTracking();
                            //aplicamos los cambios
                            ctx.NumeroUnicoInventario.ApplyChanges(numeroUnicoNuevo);
                            #endregion

                            
                            //Obtenemos el id de salida inventario de la tabla corte detalle para actualizar el registro de movimiento
                            CorteDetalle corteDetalle = (from odtm in ctx.OrdenTrabajoMaterial
                                                     join corteDet in ctx.CorteDetalle on odtm.CorteDetalleID equals corteDet.CorteDetalleID
                                                     where odtm.MaterialSpoolID == DetalleDespacho.MaterialSpoolID
                                                     select corteDet).SingleOrDefault();

                            //Actualizamos los datos del numero unico movimiento
                            //obtenemos el objeto con los datos del movimiento
                            movimientoInventario = ctx.NumeroUnicoMovimiento.Where(x => x.NumeroUnicoMovimientoID == corteDetalle.SalidaInventarioID).SingleOrDefault();

                            //verificamos si tenemos datos del ultimo movimiento o no
                            if (movimientoInventario == null)
                            {
                                ////si no existen datos registrados de acuerdo al id de salida de inventairo en corte detalle
                                ////creamos un nuevo registro en la tabla de numero unico movimiento
                                //movimientoInventario = new NumeroUnicoMovimiento();
                                ////seguimos los cambios
                                //movimientoInventario.StartTracking();
                                //movimientoInventario.FechaMovimiento = DateTime.Now;
                                //movimientoInventario.ProyectoID = DetalleDespacho.ProyectoID;
                                //movimientoInventario.Cantidad = DetalleDespacho.Cantidad;
                                //movimientoInventario.TipoMovimientoID = 10;
                                //movimientoInventario.Estatus = "A"; // Debe ser definido
                                ////actualizamos el numero unico
                                //movimientoInventario.NumeroUnicoID = numeroUnicoNuevo.NumeroUnicoID;
                                ////actualizamos el segmento
                                //movimientoInventario.Segmento = segmentoDestino;
                                ////actualizamos la referencia
                                //movimientoInventario.Referencia = movimientoInventario.Referencia + "Edición especial: " + DateTime.Now.ToString();
                                ////actualizamos el usuario y fecha de modificacion
                                //movimientoInventario.UsuarioModifica = userID;
                                //movimientoInventario.FechaModificacion = DateTime.Now;
                                ////dejamos de seguir los cambios
                                //movimientoInventario.StopTracking();
                                ////aplicamos los cambios
                                //ctx.NumeroUnicoMovimiento.ApplyChanges(movimientoInventario);
                                return false;
                            }
                            else
                            {
                                //seguimos los cambios
                                movimientoInventario.StartTracking();
                                //actualizamos el numero unico
                                movimientoInventario.NumeroUnicoID = numeroUnicoNuevo.NumeroUnicoID;
                                //actualizamos el segmento
                                movimientoInventario.Segmento = segmentoDestino;
                                //actualizamos la referencia
                                string referencia = movimientoInventario.Referencia + " Edición especial: " + DateTime.Now.ToString();
                                movimientoInventario.Referencia = referencia.Length > 150 ? referencia.Substring(0,149) : referencia; 
                                //actualizamos el usuario y fecha de modificacion
                                movimientoInventario.UsuarioModifica = userID;
                                movimientoInventario.FechaModificacion = DateTime.Now;
                                //dejamos de seguir los cambios
                                movimientoInventario.StopTracking();
                                //aplicamos los cambios
                                ctx.NumeroUnicoMovimiento.ApplyChanges(movimientoInventario);
                            }

                            //actualizamos el despacho
                            //obtenemos el objeto del despacho
                            despachoActualizar = ctx.Despacho.Where(x => x.DespachoID == DetalleDespacho.DespachoID).Single();
                            //comenzamos a seguir los cambios 
                            despachoActualizar.StartTracking();
                            //actualizamos el numero unico
                            despachoActualizar.NumeroUnicoID = numeroUnicoNuevo.NumeroUnicoID;
                            //actualizamos el segmento
                            despachoActualizar.Segmento = segmentoDestino;
                            //actualizamos el usuario y la fecha de modificaicion
                            despachoActualizar.UsuarioModifica = userID;
                            despachoActualizar.FechaModificacion = DateTime.Now;
                            //terminamos de seguir los cambios
                            despachoActualizar.StopTracking();
                            //aplicamos los cambios
                            ctx.Despacho.ApplyChanges(despachoActualizar);

                            //Actualizamos la orden de trabajo material
                            //obtenemos el objeto de la orden de trabajo
                            ordenTrabajoMaterial = ctx.OrdenTrabajoMaterial.Where(x => x.DespachoID == DetalleDespacho.DespachoID).Single();
                            //Comenzamos a seguir los cambios
                            ordenTrabajoMaterial.StartTracking();
                            //actualizamos el numero unico despachado
                            ordenTrabajoMaterial.NumeroUnicoDespachadoID = numeroUnicoNuevo.NumeroUnicoID;
                            //actualizamos el segmento
                            ordenTrabajoMaterial.SegmentoDespachado = segmentoDestino;
                            //Actualizamos el campo es equivalente ... PENDIENTE
                            //ordenTrabajoMaterial.DespachoEsEquivalente = false;
                            //actualizamos el usuario y la fecha de modificacion
                            ordenTrabajoMaterial.UsuarioModifica = userID;
                            ordenTrabajoMaterial.FechaModificacion = DateTime.Now;
                            //dejamos de seguir los cambios
                            ordenTrabajoMaterial.StopTracking();
                            //aplicamos los cambios
                            ctx.OrdenTrabajoMaterial.ApplyChanges(ordenTrabajoMaterial);

                            //guardamos los cambios en la base de datos
                            ctx.SaveChanges();

                            return true;

                            #endregion

                        }
                    }
                    else
                    {
                        //No hubo cambio
                        //Falta definir acciones
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
