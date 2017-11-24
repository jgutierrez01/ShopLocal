using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Materiales;
using SAM.BusinessLogic.Administracion;
using Mimo.Framework.Common;
using SAM.Entities.Personalizadas;

namespace SAM.BusinessLogic.Workstatus
{
    public class CorteBL
    {
        private static readonly object _mutex = new object();
        private static CorteBL _instance;

        /// <summary>
        /// constructro privado para implementar patron singleton
        /// </summary>
        private CorteBL()
        {
        }

        /// <summary>
        /// obtiene la instancia de la clase OrdenTrabajoBL
        /// </summary>
        public static CorteBL Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CorteBL();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Genera los nuevos cortes a un numero unico especifico y todas sus relaciones con inventarios.
        /// </summary>
        /// <param name="listaCortes">Detalle de cada uno de los cortes a realizar</param>
        /// <param name="sobrante">Sobrante obtenido despues de los cortes</param>
        /// <param name="rack">Rack al que se enviará el sobrante en caso de existir.</param>
        public void GeneraNuevoCorte(List<CorteDetalle> listaCortes, int sobrante, string rack, int numeroUnicoID, string segmento, int cantidadTotal, List<Simple> numeroControl, Guid userID, int cortadorID)
        {
            try
            {
                if (cantidadTotal < 1)
                {
                    throw new ExcepcionCorte(MensajesError.Excepcion_CorteSinCantidad);
                }

                //if (sobrante > 0)
                //{

                //        if (rack == string.Empty)
                //        {
                //            throw new ExcepcionCorte(MensajesError.Excepcion_RackInvalido);
                //        }
                    
                //}

                using (SamContext ctx = new SamContext())
                {
                    //Bandera que nos dice si algun corte fue por ajuste.
                    //Si es verdadera el despacho no podrá hacerse automático
                    bool tieneCorteAjuste = false;
                    DateTime fecha = DateTime.Now;
                    int contFecha = 0;

                    //Obtengo el numero unico a cortar
                    NumeroUnico numUnico = NumeroUnicoBO.Instance.ObtenerConInventarios(ctx, numeroUnicoID);
                    NumeroUnicoCorte numUnicoCorte = NumeroUnicoBO.Instance.ObtenCorteEnTransferencia(ctx, numeroUnicoID, segmento);
                    int merma = numUnicoCorte.Longitud - cantidadTotal - sobrante;
                    
                    if (merma < 0)
                    {
                        int sobranteMax = numUnicoCorte.Longitud - cantidadTotal;
                        throw new ExcepcionCorte(string.Format(MensajesError.Excepcion_SobranteMayor,sobranteMax));
                    }

                    if(numUnico.NumeroUnicoInventario.CantidadDanada > sobrante)
                        throw new ExcepcionCorte(string.Format(MensajesError.Excepcion_SobranteMenor, numUnico.NumeroUnicoInventario.CantidadDanada));

                    NumeroUnicoMovimiento movimientoMerma = null;

                    //Ingresamos a inventarios el tubo completo para poder ir generando los cortes
                    NumeroUnicoMovimiento movimientoPreparacionCorte = new NumeroUnicoMovimiento
                    {
                        NumeroUnicoID = numUnico.NumeroUnicoID,
                        ProyectoID = numUnico.ProyectoID,
                        TipoMovimientoID = (int)TipoMovimientoEnum.PreparacionCorte,
                        Cantidad = numUnicoCorte.Longitud,
                        Segmento = segmento,
                        FechaMovimiento = fecha.AddSeconds(contFecha++),
                        Estatus = EstatusNumeroUnicoMovimiento.ACTIVO,
                        UsuarioModifica = userID,
                        FechaModificacion = DateTime.Now
                    };

                    Corte corte = new Corte();

                    //Guardamos los detalles de cada corte y actualizamos la tabla OrdenTrabajoMaterial así como generar el movimiento de corte en inventarios
                    foreach (CorteDetalle detalle in listaCortes)
                    {

                        NumeroUnicoMovimiento movimientoCorte = new NumeroUnicoMovimiento
                        {
                            NumeroUnicoID = numUnico.NumeroUnicoID,
                            ProyectoID = numUnico.ProyectoID,
                            TipoMovimientoID = (int)TipoMovimientoEnum.Corte,
                            Cantidad = detalle.Cantidad,
                            Segmento = segmento,
                            FechaMovimiento = fecha.AddSeconds(contFecha++),
                            Estatus = EstatusNumeroUnicoMovimiento.ACTIVO,
                            Referencia = numeroControl != null ? numeroControl.Where(x => x.ID == detalle.CorteID).Select(y => y.Valor).SingleOrDefault() : null,
                            UsuarioModifica = userID,
                            FechaModificacion = DateTime.Now
                        };

                        if (detalle.EsAjuste)
                        {
                            tieneCorteAjuste = true;
                        }

                        detalle.NumeroUnicoMovimiento = movimientoCorte;
                        corte.CorteDetalle.Add(detalle);

                        OrdenTrabajoMaterial otm = ctx.OrdenTrabajoMaterial.Where(x => x.MaterialSpoolID == detalle.MaterialSpoolID).SingleOrDefault();

                        otm.StartTracking();
                        otm.TieneCorte = true;
                        otm.CorteDetalle = detalle;
                        otm.StopTracking();
                        ctx.OrdenTrabajoMaterial.ApplyChanges(otm);

                        numUnico.NumeroUnicoMovimiento.Add(movimientoCorte);

                    }

                    numUnico.StartTracking();
                     
                    #region Existe Merma
                    //if (sobrante > 0)
                    //{
                        //Si hay merma generar el registro 
                    if (merma > 0)
                    {
                        movimientoMerma = new NumeroUnicoMovimiento
                        {
                            NumeroUnicoID = numUnico.NumeroUnicoID,
                            ProyectoID = numUnico.ProyectoID,
                            TipoMovimientoID = (int)TipoMovimientoEnum.MermaCorte,
                            Cantidad = merma,
                            Segmento = segmento,
                            FechaMovimiento = fecha.AddSeconds(contFecha++),
                            Estatus = EstatusNumeroUnicoMovimiento.ACTIVO,
                            UsuarioModifica = userID,
                            FechaModificacion = DateTime.Now
                        };

                        numUnico.NumeroUnicoMovimiento.Add(movimientoMerma);
                    }
                    //}                    
                    #endregion

                    #region Generamos el Corte 

                    corte.ProyectoID = numUnico.ProyectoID;
                    corte.NumeroUnicoCorteID = numUnicoCorte.NumeroUnicoCorteID;
                    corte.Sobrante = sobrante;
                    corte.Merma = merma;
                    corte.MovimientoMerma = movimientoMerma;
                    corte.PreparacionCorte = movimientoPreparacionCorte;
                    corte.UsuarioModifica = userID;
                    corte.FechaModificacion = DateTime.Now;
                    corte.CortadorID = cortadorID <= 0 ? 0 : cortadorID;

                    //Si es corte de ajuste, se crea el pendiente automático
                    if (tieneCorteAjuste)
                    {
                        int categoriaPendienteID = (int)CategoriaPendienteEnum.Ingenieria;
                        int tipoPendienteID = (int)TipoPendienteEnum.CorteDeAjuste;
                        string nombreProyecto = ctx.Proyecto.Where(x => x.ProyectoID == corte.ProyectoID).Single().Nombre;
                        string idiomaUsuario = ctx.Usuario.Where(x => x.UserId == userID).Single().Idioma;

                        Pendiente p = new Pendiente();

                        p.StartTracking();
                        p.ProyectoID = corte.ProyectoID;
                        p.TipoPendienteID = tipoPendienteID;
                        p.Estatus = EstatusPendiente.Abierto;
                        p.FechaApertura = DateTime.Now;
                        p.GeneradoPor = userID;
                        p.FechaModificacion = DateTime.Now;

                        //Obtenemos el usuario responsable
                        ProyectoPendiente pp = ctx.ProyectoPendiente
                                                  .Where(x => x.ProyectoID == corte.ProyectoID && x.TipoPendienteID == p.TipoPendienteID)
                                                  .SingleOrDefault();

                        if (pp != null)
                        {
                            p.AsignadoA = pp.Responsable;
                            p.CategoriaPendienteID = categoriaPendienteID;

                            TipoPendiente tipo = ctx.TipoPendiente
                                                    .Where(x => x.TipoPendienteID == tipoPendienteID)
                                                    .SingleOrDefault();

                            p.Descripcion = LanguageHelper.INGLES == idiomaUsuario ? tipo.NombreIngles : tipo.Nombre;
                            p.Titulo = LanguageHelper.INGLES == idiomaUsuario ? tipo.NombreIngles : tipo.Nombre;
                        }

                        PendienteDetalle pd = new PendienteDetalle();

                        pd.CategoriaPendienteID = categoriaPendienteID;
                        pd.EsAlta = true;
                        pd.Responsable = pp.Responsable;
                        pd.Estatus = EstatusPendiente.Abierto;
                        pd.UsuarioModifica = userID;
                        pd.FechaModificacion = DateTime.Now;

                        p.StopTracking();
                        p.PendienteDetalle.Add(pd);

                        PendienteBL.Instance.Guarda(p, pp.Responsable, nombreProyecto, true);
                    }


                    numUnico.NumeroUnicoMovimiento.Add(movimientoPreparacionCorte);

                    ctx.Corte.ApplyChanges(corte);
                    #endregion

                    #region Actualizamos inventarios de numero unico

                    NumeroUnicoSegmento numUnicoSegmento = numUnico.NumeroUnicoSegmento.Where(x => x.Segmento == segmento).Single();
                    numUnicoSegmento.StartTracking();
                    numUnicoSegmento.InventarioTransferenciaCorte = 0;
                    numUnicoSegmento.InventarioFisico = numUnicoCorte.Longitud - cantidadTotal - merma;
                    numUnicoSegmento.InventarioBuenEstado = numUnicoCorte.Longitud - cantidadTotal - merma - numUnicoSegmento.CantidadDanada;
                    numUnicoSegmento.InventarioDisponibleCruce = numUnicoSegmento.InventarioBuenEstado - numUnicoSegmento.InventarioCongelado;
                    numUnicoSegmento.Rack = rack;
                    numUnicoSegmento.UsuarioModifica = userID;
                    numUnicoSegmento.FechaModificacion = DateTime.Now;
                    numUnicoSegmento.StopTracking();
                    ctx.NumeroUnicoSegmento.ApplyChanges(numUnicoSegmento);

                    numUnico.NumeroUnicoInventario.StartTracking();
                    numUnico.NumeroUnicoInventario.InventarioTransferenciaCorte = numUnico.NumeroUnicoInventario.InventarioTransferenciaCorte - numUnicoCorte.Longitud;
                    numUnico.NumeroUnicoInventario.InventarioFisico = numUnico.NumeroUnicoInventario.InventarioFisico + numUnicoCorte.Longitud - cantidadTotal - merma;
                    numUnico.NumeroUnicoInventario.InventarioBuenEstado = numUnico.NumeroUnicoInventario.InventarioFisico - numUnico.NumeroUnicoInventario.CantidadDanada;
                    numUnico.NumeroUnicoInventario.InventarioDisponibleCruce = numUnico.NumeroUnicoInventario.InventarioBuenEstado - numUnico.NumeroUnicoInventario.InventarioCongelado;
                    numUnico.NumeroUnicoInventario.UsuarioModifica = userID;
                    numUnico.NumeroUnicoInventario.FechaModificacion = DateTime.Now;
                    numUnico.NumeroUnicoInventario.StopTracking();
                    ctx.NumeroUnicoInventario.ApplyChanges(numUnico.NumeroUnicoInventario);
                    #endregion

                    #region Actualizamos tabla NumeroUnicoCorte
                    numUnicoCorte.StartTracking();
                    numUnicoCorte.TieneCorte = true;
                    numUnicoCorte.UsuarioModifica = userID;
                    numUnicoCorte.FechaModificacion = DateTime.Now;
                    numUnicoCorte.StopTracking();
                    ctx.NumeroUnicoCorte.ApplyChanges(numUnicoCorte);
                    #endregion

                    #region Despacho Automatizado
                    //El despacho se automatiza si la ubicacion fisica del corte no tiene area de corte y además ningún corte fue por ajuste
                    if (!numUnicoCorte.UbicacionFisica.EsAreaCorte && !tieneCorteAjuste)
                    {

                        //Actualizamos el inventario que se había congelado para el material y generamos el despacho
                        foreach (CorteDetalle detalle in listaCortes)
                        {
                            OrdenTrabajoMaterial otm = ctx.OrdenTrabajoMaterial.Where(x => x.MaterialSpoolID == detalle.MaterialSpoolID).SingleOrDefault();
                            MaterialSpool material = ctx.MaterialSpool.Where(x => x.MaterialSpoolID == detalle.MaterialSpoolID).SingleOrDefault();
                            //Actualiza inventarios numero unico congelado
                            NumeroUnico numeroCongelado = NumeroUnicoBO.Instance.ObtenerConInventarios(ctx, otm.NumeroUnicoCongeladoID.Value);
                            numeroCongelado.NumeroUnicoInventario.StartTracking();
                            numeroCongelado.NumeroUnicoInventario.InventarioCongelado = numeroCongelado.NumeroUnicoInventario.InventarioCongelado - otm.CantidadCongelada.Value;
                            numeroCongelado.NumeroUnicoInventario.InventarioDisponibleCruce = numeroCongelado.NumeroUnicoInventario.InventarioBuenEstado - numeroCongelado.NumeroUnicoInventario.InventarioCongelado;
                            numeroCongelado.NumeroUnicoInventario.UsuarioModifica = userID;
                            numeroCongelado.NumeroUnicoInventario.FechaModificacion = DateTime.Now;
                            numeroCongelado.NumeroUnicoInventario.StopTracking();
                            ctx.NumeroUnicoInventario.ApplyChanges(numeroCongelado.NumeroUnicoInventario);

                            //Actualiza inventarios segmento congelado
                            NumeroUnicoSegmento segmentoCongelado = numeroCongelado.NumeroUnicoSegmento.Where(x => x.Segmento == otm.SegmentoCongelado).SingleOrDefault();
                            segmentoCongelado.StartTracking();
                            segmentoCongelado.InventarioCongelado = segmentoCongelado.InventarioCongelado - otm.CantidadCongelada.Value;
                            segmentoCongelado.InventarioDisponibleCruce = segmentoCongelado.InventarioBuenEstado - segmentoCongelado.InventarioCongelado;
                            segmentoCongelado.UsuarioModifica = userID;
                            segmentoCongelado.FechaModificacion = DateTime.Now;
                            segmentoCongelado.StopTracking();
                            ctx.NumeroUnicoSegmento.ApplyChanges(segmentoCongelado);

                            //Verificamos si el material a despachar es el indicado por ingenieria o si es equivalente
                            bool esEquivalente = numUnico.ItemCodeID != material.ItemCodeID || numUnico.Diametro1 != material.Diametro1 || numUnico.Diametro2 != material.Diametro2;

                            //Generamos el Despacho
                            Despacho despacho = new Despacho
                            {
                                ProyectoID = numUnico.ProyectoID,
                                OrdenTrabajoSpoolID = otm.OrdenTrabajoSpoolID,
                                MaterialSpoolID = otm.MaterialSpoolID,
                                NumeroUnicoID = numUnico.NumeroUnicoID,
                                Segmento = segmento,
                                EsEquivalente = esEquivalente,
                                Cantidad = detalle.Cantidad,
                                Cancelado = false,
                                UsuarioModifica = userID,
                                FechaDespacho = fecha.AddSeconds(contFecha++),
                                FechaModificacion = DateTime.Now
                            };
                            ctx.Despacho.ApplyChanges(despacho);

                            //Actualizamos la orden de trabajo material
                            otm.StartTracking();
                            otm.Despacho = despacho;
                            otm.TieneInventarioCongelado = false;
                            otm.NumeroUnicoCongeladoID = null;
                            otm.SegmentoCongelado = null;
                            otm.CantidadCongelada = null;
                            otm.CongeladoEsEquivalente = false;
                            otm.NumeroUnicoSugerido = null;
                            otm.SegmentoSugerido = null;
                            otm.SugeridoEsEquivalente = false;
                            otm.TieneDespacho = true;
                            otm.DespachoEsEquivalente = esEquivalente;
                            otm.NumeroUnicoDespachadoID = numUnico.NumeroUnicoID;
                            otm.SegmentoDespachado = segmento;
                            otm.CantidadDespachada = detalle.Cantidad;
                            otm.UsuarioModifica = userID;
                            otm.FechaModificacion = DateTime.Now;
                            otm.StopTracking();
                            ctx.OrdenTrabajoMaterial.ApplyChanges(otm);


                            #region Transferencia Congelados
                            //Verificamos que no haya inventario disponible negativo, de ser asi transferimos congelados
                            NumeroUnicoSegmento nuInv = ctx.NumeroUnicoSegmento.Include("NumeroUnico.NumeroUnicoInventario").Where(x => x.NumeroUnicoID == numUnico.NumeroUnicoID && x.Segmento == segmento).SingleOrDefault();

                            if (nuInv.InventarioDisponibleCruce < 0)
                            {
                                transfiereCongeladosTubo(ctx, nuInv, userID);
                            }

                            #endregion

                            verificaSiExisteArmadoPendienteYPlancha(otm.MaterialSpoolID, numUnico.NumeroUnicoID, otm.OrdenTrabajoSpoolID, ctx);


                        }

                    }
                    #endregion

                    numUnico.StopTracking();
                    ctx.NumeroUnico.ApplyChanges(numUnico);
                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_Concurrencia);
            }
        }

        private void transfiereCongeladosTubo(SamContext ctx, NumeroUnicoSegmento segmento, Guid userID)
        {
            List<NumeroUnicoSegmento> candidatos = ctx.NumeroUnicoSegmento
                                                    .Include("NumeroUnico.NumeroUnicoInventario")
                                                    .Where(x => x.NumeroUnico.ItemCodeID == segmento.NumeroUnico.ItemCodeID 
                                                        && x.NumeroUnico.Diametro1 == segmento.NumeroUnico.Diametro1 
                                                        && x.NumeroUnico.Diametro2 == segmento.NumeroUnico.Diametro2 
                                                        && x.InventarioDisponibleCruce >= segmento.InventarioCongelado 
                                                        && x.NumeroUnico.Estatus == "A")
                                                    .ToList();

            if (candidatos.Count > 0)
            {
                candidatos = candidatos.OrderBy(x => x.InventarioDisponibleCruce).ToList();

                candidatos[0].StartTracking();
                candidatos[0].UsuarioModifica = userID;
                candidatos[0].FechaModificacion = DateTime.Now;
                candidatos[0].InventarioCongelado = candidatos[0].InventarioCongelado + segmento.InventarioCongelado;
                candidatos[0].InventarioDisponibleCruce = candidatos[0].InventarioBuenEstado - candidatos[0].InventarioCongelado;
                candidatos[0].StopTracking();

                ctx.NumeroUnicoSegmento.ApplyChanges(candidatos[0]);

                candidatos[0].NumeroUnico.NumeroUnicoInventario.StartTracking();
                candidatos[0].NumeroUnico.NumeroUnicoInventario.UsuarioModifica = userID;
                candidatos[0].NumeroUnico.NumeroUnicoInventario.FechaModificacion = DateTime.Now;
                candidatos[0].NumeroUnico.NumeroUnicoInventario.InventarioCongelado = 
                                    candidatos[0].NumeroUnico.NumeroUnicoInventario.InventarioCongelado + segmento.InventarioCongelado;
                candidatos[0].NumeroUnico.NumeroUnicoInventario.InventarioDisponibleCruce = 
                                    candidatos[0].NumeroUnico.NumeroUnicoInventario.InventarioBuenEstado - candidatos[0].NumeroUnico.NumeroUnicoInventario.InventarioCongelado;
                candidatos[0].NumeroUnico.NumeroUnicoInventario.StopTracking();

                ctx.NumeroUnicoInventario.ApplyChanges(candidatos[0].NumeroUnico.NumeroUnicoInventario);

                //actualizamos odt y congelados parciales con nuevo numero unico congelado

                if (ctx.CongeladoParcial.Where(x => x.NumeroUnicoCongeladoID == segmento.NumeroUnicoID && x.SegmentoCongelado == segmento.Segmento).Any())
                {
                    List<CongeladoParcial> congPar = ctx.CongeladoParcial.Where(x => x.NumeroUnicoCongeladoID == segmento.NumeroUnicoID && x.SegmentoCongelado == segmento.Segmento).ToList();
                    congPar.ForEach(x =>
                    {
                        x.StartTracking();
                        x.NumeroUnicoCongeladoID = candidatos[0].NumeroUnicoID;
                        x.SegmentoCongelado = candidatos[0].Segmento;
                        x.UsuarioModifica = userID;
                        x.FechaModificacion = DateTime.Now;
                        x.StopTracking();
                        ctx.CongeladoParcial.ApplyChanges(x);
                    });
                }
                if (ctx.OrdenTrabajoMaterial.Where(x => x.NumeroUnicoCongeladoID == segmento.NumeroUnicoID && x.SegmentoCongelado == segmento.Segmento).Any())
                {
                    List<OrdenTrabajoMaterial> _otm = ctx.OrdenTrabajoMaterial.Where(x => x.NumeroUnicoCongeladoID == segmento.NumeroUnicoID && x.SegmentoCongelado == segmento.Segmento).ToList();
                    _otm.ForEach(x =>
                    {
                        x.StartTracking();
                        x.NumeroUnicoCongeladoID = candidatos[0].NumeroUnicoID;
                        x.SegmentoCongelado = candidatos[0].Segmento;
                        x.UsuarioModifica = userID;
                        x.FechaModificacion = DateTime.Now;
                        x.StopTracking();
                        ctx.OrdenTrabajoMaterial.ApplyChanges(x);
                    });
                }

                int cantidad = segmento.InventarioCongelado;

                //liberamos el congelado del numero unico despachado
                segmento.StartTracking();
                segmento.UsuarioModifica = userID;
                segmento.FechaModificacion = DateTime.Now;
                //modificado por el error del issue 324
                segmento.InventarioCongelado = segmento.InventarioCongelado - cantidad; //0;
                segmento.InventarioDisponibleCruce = segmento.InventarioBuenEstado - segmento.InventarioCongelado;
                segmento.StopTracking();

                ctx.NumeroUnicoSegmento.ApplyChanges(segmento);

                segmento.NumeroUnico.NumeroUnicoInventario.StartTracking();
                segmento.NumeroUnico.NumeroUnicoInventario.UsuarioModifica = userID;
                segmento.NumeroUnico.NumeroUnicoInventario.FechaModificacion = DateTime.Now;
                segmento.NumeroUnico.NumeroUnicoInventario.InventarioCongelado = segmento.NumeroUnico.NumeroUnicoInventario.InventarioCongelado - cantidad;
                segmento.NumeroUnico.NumeroUnicoInventario.InventarioDisponibleCruce = segmento.NumeroUnico.NumeroUnicoInventario.InventarioBuenEstado - segmento.NumeroUnico.NumeroUnicoInventario.InventarioCongelado;
                segmento.NumeroUnico.NumeroUnicoInventario.StopTracking();

                ctx.NumeroUnicoInventario.ApplyChanges(segmento.NumeroUnico.NumeroUnicoInventario);

            }
            else // en este caso el numero unico tiene congelado un numero mayor a toda la pedacería que queda...hay que transferir de material por material para lograr el cometido
            {
                foreach (OrdenTrabajoMaterial otm in ctx.OrdenTrabajoMaterial.Where(x => x.NumeroUnicoCongeladoID == segmento.NumeroUnicoID && x.SegmentoCongelado == segmento.Segmento).OrderByDescending(x => x.CantidadCongelada))
                {
                    List<NumeroUnicoSegmento> candidato = ctx.NumeroUnicoSegmento.Include("NumeroUnico.NumeroUnicoInventario").Where(x => x.NumeroUnico.ItemCodeID == segmento.NumeroUnico.ItemCodeID && x.NumeroUnico.Diametro1 == segmento.NumeroUnico.Diametro1 && x.NumeroUnico.Diametro2 == segmento.NumeroUnico.Diametro2 && x.InventarioDisponibleCruce >= otm.CantidadCongelada && x.NumeroUnico.Estatus == "A").ToList();

                    if (candidato.Count > 0)
                    {
                        candidato = candidato.OrderBy(x => x.InventarioDisponibleCruce).ToList();

                        candidato[0].StartTracking();
                        candidato[0].UsuarioModifica = userID;
                        candidato[0].FechaModificacion = DateTime.Now;
                        candidato[0].InventarioCongelado = candidato[0].InventarioCongelado + otm.CantidadCongelada.Value;
                        candidato[0].InventarioDisponibleCruce = candidato[0].InventarioBuenEstado - candidato[0].InventarioCongelado;
                        candidato[0].StopTracking();

                        ctx.NumeroUnicoSegmento.ApplyChanges(candidato[0]);

                        candidato[0].NumeroUnico.NumeroUnicoInventario.StartTracking();
                        candidato[0].NumeroUnico.NumeroUnicoInventario.UsuarioModifica = userID;
                        candidato[0].NumeroUnico.NumeroUnicoInventario.FechaModificacion = DateTime.Now;
                        candidato[0].NumeroUnico.NumeroUnicoInventario.InventarioCongelado = 
                                                                        candidato[0].NumeroUnico.NumeroUnicoInventario.InventarioCongelado + otm.CantidadCongelada.Value;
                        candidato[0].NumeroUnico.NumeroUnicoInventario.InventarioDisponibleCruce = candidato[0].NumeroUnico.NumeroUnicoInventario.InventarioBuenEstado - candidato[0].NumeroUnico.NumeroUnicoInventario.InventarioCongelado;
                        candidato[0].NumeroUnico.NumeroUnicoInventario.StopTracking();

                        ctx.NumeroUnicoInventario.ApplyChanges(candidato[0].NumeroUnico.NumeroUnicoInventario);


                        otm.StartTracking();
                        otm.NumeroUnicoCongeladoID = candidato[0].NumeroUnicoID;
                        otm.SegmentoCongelado = candidato[0].Segmento;
                        otm.UsuarioModifica = userID;
                        otm.FechaModificacion = DateTime.Now;
                        otm.StopTracking();
                        ctx.OrdenTrabajoMaterial.ApplyChanges(otm);

                        segmento.StartTracking();
                        segmento.UsuarioModifica = userID;
                        segmento.FechaModificacion = DateTime.Now;
                        segmento.InventarioCongelado = segmento.InventarioCongelado - otm.CantidadCongelada.Value;
                        segmento.InventarioDisponibleCruce = segmento.InventarioBuenEstado - segmento.InventarioCongelado;
                        segmento.StopTracking();

                        ctx.NumeroUnicoSegmento.ApplyChanges(segmento);

                        segmento.NumeroUnico.NumeroUnicoInventario.StartTracking();
                        segmento.NumeroUnico.NumeroUnicoInventario.UsuarioModifica = userID;
                        segmento.NumeroUnico.NumeroUnicoInventario.FechaModificacion = DateTime.Now;
                        segmento.NumeroUnico.NumeroUnicoInventario.InventarioCongelado = segmento.NumeroUnico.NumeroUnicoInventario.InventarioCongelado - otm.CantidadCongelada.Value;
                        segmento.NumeroUnico.NumeroUnicoInventario.InventarioDisponibleCruce = segmento.NumeroUnico.NumeroUnicoInventario.InventarioBuenEstado - segmento.NumeroUnico.NumeroUnicoInventario.InventarioCongelado;
                        segmento.NumeroUnico.NumeroUnicoInventario.StopTracking();

                        ctx.NumeroUnicoInventario.ApplyChanges(segmento.NumeroUnico.NumeroUnicoInventario);

                        if (segmento.InventarioDisponibleCruce >= 0) //si el inventario ya es positivo se termina
                        {
                            break;
                        }
                    }
                }

                if (segmento.InventarioDisponibleCruce <= 0) //si con la liberacion de odt no es suficiente seguimos con congelados parciales
                {
                    foreach (CongeladoParcial cp in ctx.CongeladoParcial.Include("MaterialSpool").Where(x => x.NumeroUnicoCongeladoID == segmento.NumeroUnicoID && x.SegmentoCongelado == segmento.Segmento).OrderByDescending(x => x.MaterialSpool.Cantidad))
                    {
                        List<NumeroUnicoSegmento> candidato = ctx.NumeroUnicoSegmento.Include("NumeroUnico.NumeroUnicoInventario").Where(x => x.NumeroUnico.ItemCodeID == segmento.NumeroUnico.ItemCodeID && x.NumeroUnico.Diametro1 == segmento.NumeroUnico.Diametro1 && x.NumeroUnico.Diametro2 == segmento.NumeroUnico.Diametro2 && x.InventarioDisponibleCruce >= cp.MaterialSpool.Cantidad && x.NumeroUnico.Estatus == "A").ToList();

                        if (candidato.Count > 0)
                        {
                            candidato = candidato.OrderBy(x => x.InventarioDisponibleCruce).ToList();

                            candidato[0].StartTracking();
                            candidato[0].UsuarioModifica = userID;
                            candidato[0].FechaModificacion = DateTime.Now;
                            candidato[0].InventarioCongelado = candidato[0].InventarioCongelado + cp.MaterialSpool.Cantidad;
                            candidato[0].InventarioDisponibleCruce = candidato[0].InventarioBuenEstado - candidato[0].InventarioCongelado;
                            candidato[0].StopTracking();

                            ctx.NumeroUnicoSegmento.ApplyChanges(candidato[0]);

                            candidato[0].NumeroUnico.NumeroUnicoInventario.StartTracking();
                            candidato[0].NumeroUnico.NumeroUnicoInventario.UsuarioModifica = userID;
                            candidato[0].NumeroUnico.NumeroUnicoInventario.FechaModificacion = DateTime.Now;
                            candidato[0].NumeroUnico.NumeroUnicoInventario.InventarioCongelado = candidato[0].NumeroUnico.NumeroUnicoInventario.InventarioCongelado + cp.MaterialSpool.Cantidad;
                            candidato[0].NumeroUnico.NumeroUnicoInventario.InventarioDisponibleCruce = candidato[0].NumeroUnico.NumeroUnicoInventario.InventarioBuenEstado - candidato[0].NumeroUnico.NumeroUnicoInventario.InventarioCongelado;
                            candidato[0].NumeroUnico.NumeroUnicoInventario.StopTracking();

                            ctx.NumeroUnicoInventario.ApplyChanges(candidato[0].NumeroUnico.NumeroUnicoInventario);


                            cp.StartTracking();
                            cp.NumeroUnicoCongeladoID = candidato[0].NumeroUnicoID;
                            cp.SegmentoCongelado = candidato[0].Segmento;
                            cp.UsuarioModifica = userID;
                            cp.FechaModificacion = DateTime.Now;
                            cp.StopTracking();
                            ctx.CongeladoParcial.ApplyChanges(cp);

                            segmento.StartTracking();
                            segmento.UsuarioModifica = userID;
                            segmento.FechaModificacion = DateTime.Now;
                            segmento.InventarioCongelado = segmento.InventarioCongelado - cp.MaterialSpool.Cantidad;
                            segmento.InventarioDisponibleCruce = segmento.InventarioBuenEstado - segmento.InventarioCongelado;
                            segmento.StopTracking();

                            ctx.NumeroUnicoSegmento.ApplyChanges(segmento);

                            segmento.NumeroUnico.NumeroUnicoInventario.StartTracking();
                            segmento.NumeroUnico.NumeroUnicoInventario.UsuarioModifica = userID;
                            segmento.NumeroUnico.NumeroUnicoInventario.FechaModificacion = DateTime.Now;
                            segmento.NumeroUnico.NumeroUnicoInventario.InventarioCongelado = segmento.NumeroUnico.NumeroUnicoInventario.InventarioCongelado - cp.MaterialSpool.Cantidad;
                            segmento.NumeroUnico.NumeroUnicoInventario.InventarioDisponibleCruce = segmento.NumeroUnico.NumeroUnicoInventario.InventarioBuenEstado - segmento.NumeroUnico.NumeroUnicoInventario.InventarioCongelado;
                            segmento.NumeroUnico.NumeroUnicoInventario.StopTracking();

                            ctx.NumeroUnicoInventario.ApplyChanges(segmento.NumeroUnico.NumeroUnicoInventario);

                            if (segmento.InventarioDisponibleCruce >= 0) //si el inventario ya es positivo se termina
                            {
                                break;
                            }
                        }
                    }
                }


            }
        }

        private void verificaSiExisteArmadoPendienteYPlancha(int materialSpoolID, int numerounicoID, int ordenTrabajoSpoolID, SamContext ctx)
        {
            MaterialSpool material = ctx.MaterialSpool.Where(x => x.MaterialSpoolID == materialSpoolID).FirstOrDefault();
            int spoolID = ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID).Select(x => x.SpoolID).FirstOrDefault();

            //obtengo las juntas del spool
            List<JuntaSpool> jspool = ctx.JuntaSpool.Where(x => x.SpoolID == spoolID).ToList();
            JuntaWorkstatus jw = null;

            foreach (JuntaSpool junta in jspool)
            {
                try
                {
                    jw = ctx.JuntaWorkstatus.Include("JuntaArmado").Include("JuntaSpool").Where(x => x.JuntaSpoolID == junta.JuntaSpoolID && x.JuntaFinal).FirstOrDefault();
                    //jw.JuntaArmado1.StartTracking();
                    if (jw != null && jw.ArmadoAprobado) //verifica si ya se tiene armado 
                    {
                        jw.JuntaArmado1.StartTracking();
                        if (jw.JuntaArmado1.NumeroUnico1ID == null) //verifica si el material1 esta pendiente
                        {
                            if (jw.JuntaSpool.EtiquetaMaterial1.Equals(material.Etiqueta)) //si es misma etiqueta se plancha
                            {
                                jw.JuntaArmado1.NumeroUnico1ID = numerounicoID;
                            }
                            else if (int.Parse(jw.JuntaSpool.EtiquetaMaterial1) == int.Parse(material.Etiqueta))
                            {
                                jw.JuntaArmado1.NumeroUnico1ID = numerounicoID;
                            }
                        }
                        if (jw.JuntaArmado1.NumeroUnico2ID == null) // verifico material 2
                        {
                            if (jw.JuntaSpool.EtiquetaMaterial2.Equals(material.Etiqueta))
                            {
                                jw.JuntaArmado1.NumeroUnico2ID = numerounicoID;
                            }
                            else if (int.Parse(jw.JuntaSpool.EtiquetaMaterial2) == int.Parse(material.Etiqueta))
                            {
                                jw.JuntaArmado1.NumeroUnico2ID = numerounicoID;
                            }
                        }
                        jw.JuntaArmado1.StopTracking();
                        ctx.JuntaArmado.ApplyChanges(jw.JuntaArmado1);
                        ctx.SaveChanges();
                    }

                    //jw.JuntaArmado1.StopTracking();


                }
                catch
                {
                }

            }


        }


        /// <summary>
        /// Cancela el corte especificado
        /// </summary>
        /// <param name="corteID">ID del corte a cancelar</param>
        public void CancelaCorte(int corteID, Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                //Corte con despacho
                IQueryable<int> materiales = ctx.CorteDetalle.Where(x => x.CorteID == corteID).Select(y => y.MaterialSpoolID);
                IQueryable<int> ordenTrabajoMateriales = ctx.OrdenTrabajoMaterial.Where(x => materiales.Contains(x.MaterialSpoolID) && x.TieneDespacho).Select(y => y.OrdenTrabajoMaterialID);
                if (ordenTrabajoMateriales.Count() > 0)
                {
                    throw new Excepciones.ExcepcionCorte(MensajesError.Excepcion_CorteConDespacho);
                }
                
                Corte corte = ctx.Corte.Include("NumeroUnicoCorte").Include("CorteDetalle").Where(x => x.CorteID == corteID).SingleOrDefault();

                //Valida que no exista otro despacho a corte posterior para el numero unico
                if ((from dcorte in ctx.NumeroUnicoCorte
                     where dcorte.NumeroUnicoID == corte.NumeroUnicoCorte.NumeroUnicoID
                     && dcorte.NumeroUnicoCorteID > corte.NumeroUnicoCorteID && dcorte.TieneCorte == false
                     select dcorte).Any())
                {
                    throw new Excepciones.ExcepcionCorte(MensajesError.Excepcion_NumeroUnicoConOtroDespachoCorte);
                }

                int Cantidadinventario = 0;
                if (corte.Merma != null)
                    Cantidadinventario = corte.Merma.Value;

                Cantidadinventario += corte.CorteDetalle.Where(x => !x.Cancelado).Sum(x => x.Cantidad);

                corte.StartTracking();
                corte.Cancelado = true;
                corte.UsuarioModifica = userID;
                corte.FechaModificacion = DateTime.Now;
                corte.StopTracking();
                ctx.Corte.ApplyChanges(corte);

                //Cancelamos los detalles
                foreach (CorteDetalle detalle in corte.CorteDetalle)
                {
                    detalle.StartTracking();
                    detalle.Cancelado = true;
                    detalle.UsuarioModifica = userID;
                    detalle.FechaModificacion = DateTime.Now;
                    detalle.StopTracking();
                    ctx.CorteDetalle.ApplyChanges(detalle);

                    //Actualizamos el ordentrabajo material cancelando su corte
                    OrdenTrabajoMaterial material = ctx.OrdenTrabajoMaterial.Where(x => x.MaterialSpoolID == detalle.MaterialSpoolID).Single();
                    material.StartTracking();
                    material.TieneCorte = false;
                    material.CorteDetalleID = null;
                    material.UsuarioModifica = userID;
                    material.FechaModificacion = DateTime.Now;
                    ctx.OrdenTrabajoMaterial.ApplyChanges(material);

                    //Cancelamos el movimiento 
                    NumeroUnicoMovimiento movimiento = ctx.NumeroUnicoMovimiento.Where(x => x.NumeroUnicoMovimientoID == detalle.SalidaInventarioID).Single();

                    movimiento.StartTracking();
                    movimiento.Estatus = EstatusNumeroUnicoMovimiento.CANCELADO;
                    movimiento.UsuarioModifica = userID;
                    movimiento.FechaModificacion = DateTime.Now;
                    movimiento.StopTracking();
                    ctx.NumeroUnicoMovimiento.ApplyChanges(movimiento);

                }

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

                //Actualizamos inventario
                NumeroUnicoSegmento segmento = ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == corte.NumeroUnicoCorte.NumeroUnicoID && x.Segmento == corte.NumeroUnicoCorte.Segmento).Single();
                segmento.StartTracking();
                segmento.InventarioFisico += Cantidadinventario;
                segmento.InventarioBuenEstado += Cantidadinventario;
                segmento.InventarioDisponibleCruce = segmento.InventarioBuenEstado - segmento.InventarioCongelado;
                segmento.UsuarioModifica = userID;
                segmento.FechaModificacion = DateTime.Now;
                segmento.StopTracking();
                ctx.NumeroUnicoSegmento.ApplyChanges(segmento);

                NumeroUnicoInventario inventario = ctx.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == corte.NumeroUnicoCorte.NumeroUnicoID).Single();
                inventario.StartTracking();
                inventario.InventarioFisico += Cantidadinventario;
                inventario.InventarioBuenEstado += Cantidadinventario;
                inventario.InventarioDisponibleCruce = inventario.InventarioBuenEstado - inventario.InventarioCongelado;
                inventario.UsuarioModifica = userID;
                inventario.FechaModificacion = DateTime.Now;
                inventario.StopTracking();
                ctx.NumeroUnicoInventario.ApplyChanges(inventario);

                ctx.SaveChanges();
            }

        }

        public void CancelarCorteDetalle(int corteDetalleID, Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                //Corte con despacho
                IQueryable<int> materiales = ctx.CorteDetalle.Where(x => x.CorteDetalleID == corteDetalleID).Select(y => y.MaterialSpoolID);
                IQueryable<int> ordenTrabajoMateriales = ctx.OrdenTrabajoMaterial.Where(x => materiales.Contains(x.MaterialSpoolID) && x.TieneDespacho).Select(y => y.OrdenTrabajoMaterialID);
                if (ordenTrabajoMateriales.Count() > 0)
                {
                    throw new Excepciones.ExcepcionCorte(MensajesError.Excepcion_CorteConDespacho);
                }

                CorteDetalle detalle = ctx.CorteDetalle.Include("Corte").Include("OrdenTrabajoMaterial").Where(x => x.CorteDetalleID == corteDetalleID).SingleOrDefault();
                NumeroUnicoCorte oNumeroUnicoCorte = ctx.NumeroUnicoCorte.Where(x => x.NumeroUnicoCorteID == detalle.Corte.NumeroUnicoCorteID).Single();

                //Valida que no exista otro despacho a corte posterior para el numero unico
                if ((from dcorte in ctx.NumeroUnicoCorte
                     where dcorte.NumeroUnicoID == detalle.Corte.NumeroUnicoCorte.NumeroUnicoID
                     && dcorte.NumeroUnicoCorteID > detalle.Corte.NumeroUnicoCorteID && dcorte.TieneCorte == false
                     select dcorte).Any())
                {
                    throw new Excepciones.ExcepcionCorte(MensajesError.Excepcion_NumeroUnicoConOtroDespachoCorte);
                }

                OrdenTrabajoMaterial otm = detalle.OrdenTrabajoMaterial.SingleOrDefault();

                otm.StartTracking();
                otm.TieneDespacho = false;
                otm.DespachoID = null;
                otm.CorteDetalleID = null;
                otm.TieneCorte = false;
                otm.UsuarioModifica = userID;
                otm.FechaModificacion = DateTime.Now;
                ctx.OrdenTrabajoMaterial.ApplyChanges(otm);

                 //Si el cortedetalle es el ultimo aactivo del corte cancelamos tambien el corte
                if (ctx.CorteDetalle.Where(x => !x.Cancelado && x.CorteID == detalle.CorteID).Count() == 1)
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
                    segmento.InventarioDisponibleCruce = segmento.InventarioBuenEstado - segmento.InventarioCongelado;
                    segmento.UsuarioModifica = userID;
                    segmento.FechaModificacion = DateTime.Now;
                    segmento.StopTracking();
                    ctx.NumeroUnicoSegmento.ApplyChanges(segmento);

                    NumeroUnicoInventario inventario = ctx.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == corte.NumeroUnicoCorte.NumeroUnicoID).Single();
                    inventario.StartTracking();
                    inventario.InventarioFisico += detalle.Cantidad + merma;
                    inventario.InventarioBuenEstado += detalle.Cantidad + merma;
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
                    segmento.InventarioDisponibleCruce = segmento.InventarioBuenEstado - segmento.InventarioCongelado;
                    segmento.UsuarioModifica = userID;
                    segmento.FechaModificacion = DateTime.Now;
                    segmento.StopTracking();
                    ctx.NumeroUnicoSegmento.ApplyChanges(segmento);

                    NumeroUnicoInventario inventario = ctx.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == detalle.Corte.NumeroUnicoCorte.NumeroUnicoID).Single();
                    inventario.StartTracking();
                    inventario.InventarioFisico += detalle.Cantidad;
                    inventario.InventarioBuenEstado += detalle.Cantidad;
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


                ctx.SaveChanges();
            }
        }
    }
}
