using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessLogic.Excepciones;
using SAM.Entities.Personalizadas;

namespace SAM.BusinessLogic.Cruce
{
    public class CruceForzado
    {
        private int _proyectoID;
        private int _spoolID;
        private List<NumeroUnico> _lstNumeroUnico;
        private List<ItemCode> _lstItemCode;
        private List<ItemCodeEquivalente> _lstEquivalente;
        private SamContext _ctx;
        private Guid ? _userID;
        private Dictionary<int, ItemCode> _dicItemCodes;
        private Spool _spool;
        private List<ParForzado> _lstPares;
        private List<OrdenTrabajoMaterial> _lstOtrasOdts;

        public CruceForzado(SamContext ctx, int proyectoID, int spoolID, Guid? userID, List<ParForzado> lstPares)
        {
            _ctx = ctx;
            _proyectoID = proyectoID;
            _spoolID = spoolID;
            _userID = userID;
            _lstPares = lstPares;
        }


        public Spool Procesa(out List<NumeroUnico> congelados, out List<OrdenTrabajoMaterial> conTrueque)
        {
            _spool = _ctx.Spool
                         .Include("MaterialSpool")
                         .Include("JuntaSpool")
                         .Where(x => x.ProyectoID == _proyectoID)
                         .Where(x => x.SpoolID == _spoolID)
                         .Where(x => x.AprobadoParaCruce && x.PendienteDocumental)
                         .Where(x => !x.OrdenTrabajoSpool.Any())
                         .SingleOrDefault();

            if (_spool == null)
            {
                throw new ExcepcionCruce(MensajesError.Cruce_SpoolsNoAprobados);
            }

            //Los ids de los números únicos que queremos congelar
            IQueryable<int> qNusSeleccionados = _lstPares.Select(x => x.NumeroUnicoID).AsQueryable();

            //Los ids de materiales en otras ODT que podríamos descongelar en caso de ser necesario
            IQueryable<OrdenTrabajoMaterial> iqOtrosMateriales =
                _ctx.OrdenTrabajoMaterial
                    .Where(x => qNusSeleccionados.Contains(x.NumeroUnicoCongeladoID.Value) && !x.EsAsignado && !x.TieneDespacho && x.TieneInventarioCongelado);

            //Los I.C. ids requeridos por ingeniería
            IQueryable<int> icIdsDirectos = 
                _ctx.MaterialSpool
                    .Where(x => x.SpoolID == _spoolID || iqOtrosMateriales.Select(y => y.MaterialSpoolID).Contains(x.MaterialSpoolID))
                    .Select(x => x.ItemCodeID);

            //Los I.C. equivalentes
            IQueryable<ItemCodeEquivalente> qIcEq =
                _ctx.ItemCodeEquivalente.Where(x => icIdsDirectos.Contains(x.ItemCodeID));

            //Traer los Item codes tanto los de ingeniería como los equivalentes
            IQueryable<ItemCode> qIc = 
                _ctx.ItemCode.Where(x => icIdsDirectos.Contains(x.ItemCodeID) || 
                                         qIcEq.Select(y => y.ItemEquivalenteID)
                                              .Contains(x.ItemCodeID));

            //Sólo los números únicos del proyecto cuyos item codes sean requeridos
            //por los spools pasados y que se puedan usar
            _lstNumeroUnico =
                _ctx.NumeroUnico
                    .Where(x => x.ProyectoID == _proyectoID)
                    .Where(x => x.NumeroUnicoInventario.InventarioDisponibleCruce > 0)
                    //.Where(x => qIc.Select(y => (int?)y.ItemCodeID).Contains(x.ItemCodeID))
                    .Where(x => !x.Colada.HoldCalidad)
                    .Where(x => x.Estatus.Equals(EstatusNumeroUnico.APROBADO))
                    .ToList();

            _lstNumeroUnico = _lstNumeroUnico.Where(x => qIc.Select(y => (int?)y.ItemCodeID).Contains(x.ItemCodeID)).ToList();

            _lstNumeroUnico.AddRange(
                _ctx.NumeroUnico.Where(x => qNusSeleccionados.Contains(x.NumeroUnicoID)).ToList());

            _lstNumeroUnico.AddRange(
                _ctx.NumeroUnico.Where(x => iqOtrosMateriales.Select(y => y.NumeroUnicoCongeladoID).Contains(x.NumeroUnicoID)).ToList());

            _lstNumeroUnico = _lstNumeroUnico.Distinct().ToList();

            IQueryable<int> iqNus = _lstNumeroUnico.Select(x => x.NumeroUnicoID).AsQueryable();

            //Traer los inventarios
            _ctx.NumeroUnicoInventario
                .Where(x => iqNus.Contains(x.NumeroUnicoID)).ToList();

            //Traer los segmentos
            _ctx.NumeroUnicoSegmento
                .Where(x => iqNus.Contains(x.NumeroUnicoID)).ToList();

            //Traer de BD todos los I.C. del proyecto requeridos
            _lstItemCode = qIc.ToList();

            //Traer de BD todos los I.C. equivalentes necesarios
            _lstEquivalente = qIcEq.ToList();

            //Traer los materiales de otras ODTs que igual y decidimos descongelar
            _lstOtrasOdts = _ctx.OrdenTrabajoMaterial
                                .Include("MaterialSpool")
                                .Where(x => qNusSeleccionados.Contains(x.NumeroUnicoCongeladoID.Value) && !x.EsAsignado && !x.TieneDespacho && x.TieneInventarioCongelado)
                                .ToList();

            //Ejecutar el proceso
            estableceVariablesTemporales();
            generaDiccionarios();
            recorreSpoolYCongela();

            //Regresar una lista con los nus congelados
            congelados = (from nus in _lstNumeroUnico
                          where nus.InfoCruce.SeUsoEnCruce
                          select nus).ToList();

            //Materiales de orden de trabajo que se hayan usado para trueque
            conTrueque = (from odtm in _lstOtrasOdts
                          where odtm.InfoCruce.CambioNuCongelado
                          select odtm).ToList();

            //Si todos los materiales del spool tienen algún N.U. significa que el cruce fue exitoso
            _spool.InfoCruce.CruceExitoso = _spool.MaterialSpool.All(x => x.InfoCruce.NumeroUnicoID > 0);
            _spool.InfoCruce.UsoEquivalencia = false;

            //Regresar el spool
            return _spool;
        }

        /// <summary>
        /// 
        /// </summary>
        private void recorreSpoolYCongela()
        {
            ParForzado par = null;
            NumeroUnico nuDeseado = null;
            NumeroUnicoSegmento nuSegmentoDeseado = null;

            foreach (MaterialSpool material in _spool.MaterialSpool)
            {
                //Traer el par que se quiere forzar
                par = _lstPares.Where(x => x.MaterialSpoolID == material.MaterialSpoolID).Single();

                //Si es accesorio se busca directamente sobre la tabla NumeroUnicoInventario
                if (_dicItemCodes[material.ItemCodeID].TipoMaterialID == (int)TipoMaterialEnum.Accessorio)
                {
                    nuDeseado = _lstNumeroUnico.Where(x => x.NumeroUnicoID == par.NumeroUnicoID).SingleOrDefault();

                    if (nuDeseado != null)
                    {
                        if (nuDeseado.NumeroUnicoInventario.InventarioDisponibleCruce >= material.Cantidad)
                        {
                            //Significa que si alcanza entonces lo congelamos
                            congelaAccesorio(nuDeseado, material.Cantidad);
                            material.InfoCruce.NumeroUnicoID = nuDeseado.NumeroUnicoID;
                        }
                        else
                        {
                            intentaDescongelarAccesoriosDeOtrasOdts(material, nuDeseado);
                        }
                    }
                }
                else
                {
                    //si es tubo hay que buscar sobre la tabla NumeroUnicoSegmento
                    nuSegmentoDeseado =    (from nus in _lstNumeroUnico
                                            where nus.NumeroUnicoID == par.NumeroUnicoID
                                            from segs in nus.NumeroUnicoSegmento
                                            where   segs.Segmento == par.Segmento
                                            select segs).SingleOrDefault();

                    if (nuSegmentoDeseado != null)
                    {
                        if (nuSegmentoDeseado.InventarioDisponibleCruce >= material.Cantidad)
                        {
                            //si se encontró vamos a congelar temporalmente la cantidad
                            congelaTubo(nuSegmentoDeseado, material.Cantidad);
                            //Esto lo vamos a usar para la ODT
                            material.InfoCruce.NumeroUnicoID = nuSegmentoDeseado.NumeroUnicoID;
                            material.InfoCruce.Segmento = nuSegmentoDeseado.Segmento;
                        }
                        else
                        {
                            intentaDescongelarTubosDeOtrasOdts(material, nuSegmentoDeseado);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nuSegACongelar"></param>
        /// <param name="cantidad"></param>
        private void congelaTubo(NumeroUnicoSegmento nuSegACongelar, int cantidad)
        {
            nuSegACongelar.StartTracking();
            nuSegACongelar.NumeroUnico.StartTracking();
            nuSegACongelar.NumeroUnico.NumeroUnicoInventario.StartTracking();
            nuSegACongelar.NumeroUnico.InfoCruce.SeUsoEnCruce = true;

            nuSegACongelar.UsuarioModifica = _userID;
            nuSegACongelar.FechaModificacion = DateTime.Now;
            nuSegACongelar.NumeroUnico.UsuarioModifica = _userID;
            nuSegACongelar.NumeroUnico.FechaModificacion = DateTime.Now;
            nuSegACongelar.NumeroUnico.NumeroUnicoInventario.UsuarioModifica = _userID;
            nuSegACongelar.NumeroUnico.NumeroUnicoInventario.FechaModificacion = DateTime.Now;

            nuSegACongelar.NumeroUnico.NumeroUnicoInventario.InventarioCongelado += cantidad;
            nuSegACongelar.NumeroUnico.NumeroUnicoInventario.InventarioDisponibleCruce -= cantidad;
            nuSegACongelar.InventarioCongelado += cantidad;
            nuSegACongelar.InventarioDisponibleCruce -= cantidad;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nuACongelar"></param>
        /// <param name="cantidad"></param>
        private void congelaAccesorio(NumeroUnico nuACongelar, int cantidad)
        {
            nuACongelar.StartTracking();
            nuACongelar.NumeroUnicoInventario.StartTracking();
            nuACongelar.InfoCruce.SeUsoEnCruce = true;
            nuACongelar.UsuarioModifica = _userID;
            nuACongelar.FechaModificacion = DateTime.Now;
            nuACongelar.NumeroUnicoInventario.UsuarioModifica = _userID;
            nuACongelar.NumeroUnicoInventario.FechaModificacion = DateTime.Now;
            nuACongelar.NumeroUnicoInventario.InventarioDisponibleCruce -= cantidad;
            nuACongelar.NumeroUnicoInventario.InventarioCongelado += cantidad;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="material"></param>
        /// <param name="nuSegmentoDeseado"></param>
        /// <returns></returns>
        private bool intentaDescongelarTubosDeOtrasOdts(MaterialSpool material, NumeroUnicoSegmento nuSegmentoDeseado)
        {
            if (nuSegmentoDeseado.InventarioBuenEstado >= material.Cantidad)
            {
                List<OrdenTrabajoMaterial> lstOdtm =
                    _lstOtrasOdts.Where(x => x.NumeroUnicoCongeladoID == nuSegmentoDeseado.NumeroUnicoID && x.SegmentoCongelado == nuSegmentoDeseado.Segmento)
                                 .OrderBy(x => x.CantidadCongelada)
                                 .ToList();

                //Revisar si nos va a alcanzar (si la suma de todos los congelados del mismo segmento son mayor o igual a lo requerido)
                //Incluso asi puede no alcanzarnos lo cual se determina más adelante.
                if (lstOdtm.Sum(x => x.CantidadCongelada) >= material.Cantidad)
                {
                    //Primero vamos a dar las vueltas que sean necesarias tratando de descongelar únicamente un material
                    List<OrdenTrabajoMaterial> subFiltro = lstOdtm.Where(x => x.CantidadCongelada >= material.Cantidad)
                                                                  .OrderBy(x => x.CantidadCongelada)
                                                                  .ToList();

                    foreach (OrdenTrabajoMaterial odtm in subFiltro)
                    {
                        NumeroUnicoSegmento nuReemplazo =
                            (from nus in _lstNumeroUnico
                             where nus.ItemCodeID == odtm.MaterialSpool.ItemCodeID
                                    && nus.Diametro1 == odtm.MaterialSpool.Diametro1
                                    && nus.Diametro2 == odtm.MaterialSpool.Diametro2
                                    && nus.NumeroUnicoID != nuSegmentoDeseado.NumeroUnicoID
                             from segs in nus.NumeroUnicoSegmento
                             where segs.InventarioDisponibleCruce >= odtm.MaterialSpool.Cantidad
                             orderby segs.InventarioDisponibleCruce
                             select segs).FirstOrDefault();

                        //Significa que lo podemos reemplazar por lo cual hacemos el intercambio
                        if (nuReemplazo != null)
                        {
                            //Cambia el congelado de la ODT candidata
                            reemplazarCongelado(nuSegmentoDeseado, odtm, nuReemplazo);

                            //Congela el que queremos
                            congelaTubo(nuSegmentoDeseado, material.Cantidad);

                            //Guardar el material utilizado al congelar
                            material.InfoCruce.NumeroUnicoID = nuSegmentoDeseado.NumeroUnicoID;
                            material.InfoCruce.Segmento = nuSegmentoDeseado.Segmento;
                            return true;
                        }
                    }

                    //Aun podemos intentar algo más que consiste en descongelar varios de tal manera que sume
                    //lo que necesitamos
                    int descongelado = 0;

                    foreach (OrdenTrabajoMaterial odtm in lstOdtm)
                    {
                        NumeroUnicoSegmento nuReemplazo =
                            (from nus in _lstNumeroUnico
                                where nus.ItemCodeID == odtm.MaterialSpool.ItemCodeID
                                    && nus.Diametro1 == odtm.MaterialSpool.Diametro1
                                    && nus.Diametro2 == odtm.MaterialSpool.Diametro2
                                    && nus.NumeroUnicoID != nuSegmentoDeseado.NumeroUnicoID
                                from segs in nus.NumeroUnicoSegmento
                                where segs.InventarioDisponibleCruce >= odtm.MaterialSpool.Cantidad
                                orderby segs.InventarioDisponibleCruce
                                select segs).FirstOrDefault();

                        if (nuReemplazo != null)
                        {
                            //Cambia el congelado de la ODT candidata
                            reemplazarCongelado(nuSegmentoDeseado, odtm, nuReemplazo);

                            //Sumar cuanto hemos descongelado
                            descongelado += odtm.MaterialSpool.Cantidad;
                        }

                        //Ya nos alcanza
                        if (descongelado >= material.Cantidad)
                        {
                            //Ahora si congelar el que queremos
                            congelaTubo(nuSegmentoDeseado, material.Cantidad);
                            
                            //Guardar el material utilizado
                            material.InfoCruce.NumeroUnicoID = nuSegmentoDeseado.NumeroUnicoID;
                            material.InfoCruce.Segmento = nuSegmentoDeseado.Segmento;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Tomar un registro de orden de trabajo material y cambia el ID del número único congelado por el ID pasado
        /// en el parámetro nuReemplazo.
        /// Congela inventarios para nuReemplazo y descongela inventarios para nuSegmentoDeseado.
        /// </summary>
        /// <param name="nuSegmentoDeseado">Segmento que queremos utilizar pero que no nos alcanza</param>
        /// <param name="odtm">Registro con el cual podemos hacer el trueque</param>
        /// <param name="nuReemplazo">Número único con el cual haremos el trueque</param>
        private void reemplazarCongelado(NumeroUnicoSegmento nuSegmentoDeseado, OrdenTrabajoMaterial odtm, NumeroUnicoSegmento nuReemplazo)
        {
            odtm.StartTracking();
            odtm.UsuarioModifica = _userID;
            odtm.FechaModificacion = DateTime.Now;
            odtm.CongeladoEsEquivalente = false;

            odtm.NumeroUnicoCongeladoID = nuReemplazo.NumeroUnicoID;
            odtm.SegmentoCongelado = nuReemplazo.Segmento;

            //Marcarlo para saber que cambió
            odtm.InfoCruce.CambioNuCongelado = true;

            //Congelar el nuevo número único seleccionado
            congelaTubo(nuReemplazo, odtm.MaterialSpool.Cantidad);

            //Sumar al inventario del N.U. deseado lo que acabamos de descongelar
            nuSegmentoDeseado.StartTracking();
            nuSegmentoDeseado.NumeroUnico.NumeroUnicoInventario.StartTracking();
            nuSegmentoDeseado.InventarioDisponibleCruce += odtm.MaterialSpool.Cantidad;
            nuSegmentoDeseado.InventarioCongelado -= odtm.MaterialSpool.Cantidad;
            nuSegmentoDeseado.NumeroUnico.NumeroUnicoInventario.InventarioDisponibleCruce += odtm.MaterialSpool.Cantidad;
            nuSegmentoDeseado.NumeroUnico.NumeroUnicoInventario.InventarioCongelado -= odtm.MaterialSpool.Cantidad;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="material"></param>
        /// <param name="nuDeseado"></param>
        /// <returns></returns>
        private bool intentaDescongelarAccesoriosDeOtrasOdts(MaterialSpool material, NumeroUnico nuDeseado)
        {
            //Sólo si todavía tenemos suficiente físicamente
            if (nuDeseado.NumeroUnicoInventario.InventarioBuenEstado >= material.Cantidad)
            {
                OrdenTrabajoMaterial odtm =
                    _lstOtrasOdts.Where(x => x.NumeroUnicoCongeladoID == nuDeseado.NumeroUnicoID && x.CantidadCongelada >= material.Cantidad)
                                 .FirstOrDefault();

                if (odtm != null)
                {
                    //Ver si hay otro nu que cumpla con lo que necesitamos para este material
                    NumeroUnico nu = (from nums in _lstNumeroUnico
                                      where nums.ItemCodeID == odtm.MaterialSpool.ItemCodeID
                                            && nums.Diametro1 == odtm.MaterialSpool.Diametro1
                                            && nums.Diametro2 == odtm.MaterialSpool.Diametro2
                                            && nums.NumeroUnicoInventario.InventarioDisponibleCruce >= odtm.MaterialSpool.Cantidad
                                            && nums.NumeroUnicoID != nuDeseado.NumeroUnicoID
                                      orderby nums.NumeroUnicoInventario.InventarioDisponibleCruce
                                      select nums).FirstOrDefault();

                    //Si se puede entonces hacemos el trueque
                    if (nu != null)
                    {
                        odtm.StartTracking();
                        odtm.UsuarioModifica = _userID;
                        odtm.FechaModificacion = DateTime.Now;
                        
                        //Hacemos el trueque
                        odtm.NumeroUnicoCongeladoID = nu.NumeroUnicoID;
                        odtm.CongeladoEsEquivalente = false;

                        //Marcarlo para saber que cambió
                        odtm.InfoCruce.CambioNuCongelado = true;

                        //Congelar un nuevo número único para la odtm que vamos a descongelar
                        congelaAccesorio(nu, odtm.MaterialSpool.Cantidad);

                        //Congelar el que queremos
                        congelaAccesorio(nuDeseado, material.Cantidad);

                        //Sumar al inventario lo que descongelamos de la otra ODT
                        nuDeseado.NumeroUnicoInventario.InventarioDisponibleCruce += odtm.MaterialSpool.Cantidad;
                        nuDeseado.NumeroUnicoInventario.InventarioCongelado -= odtm.MaterialSpool.Cantidad;

                        material.InfoCruce.NumeroUnicoID = nuDeseado.NumeroUnicoID;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Genera diccionarios de item codes acceso rápido
        /// </summary>
        private void generaDiccionarios()
        {
            _dicItemCodes = _lstItemCode.ToDictionary(x => x.ItemCodeID);
        }

        /// <summary>
        /// Para poder afectar los inventarios spool x spool necesitamos poder llevar tracking de esos cambios
        /// en otras variables.
        /// </summary>
        private void estableceVariablesTemporales()
        {
            //Copia congelados y disponibles a temporales
            _lstNumeroUnico.ForEach(x =>
            {
                x.InfoCruce.SeUsoEnCruce = false;
                x.NumeroUnicoInventario.InfoCruce.InventarioCongeladoTemporal = x.NumeroUnicoInventario.InventarioCongelado;
                x.NumeroUnicoInventario.InfoCruce.InventarioDisponibleCruceTemporal = x.NumeroUnicoInventario.InventarioDisponibleCruce;

                foreach (NumeroUnicoSegmento nus in x.NumeroUnicoSegmento)
                {
                    nus.InfoCruce.InventarioCongeladoTemporal = nus.InventarioCongelado;
                    nus.InfoCruce.InventarioDisponibleCruceTemporal = nus.InventarioDisponibleCruce;
                }
            });

            //Asumimos que no va a necesitar hacer trueques
            _lstOtrasOdts.ForEach(x => x.InfoCruce.CambioNuCongelado = false);

            //asumimos que no se va a poder cruzar
            _spool.InfoCruce.CruceExitoso = false;
            _spool.InfoCruce.UsoEquivalencia = false;
        }
    }
}
