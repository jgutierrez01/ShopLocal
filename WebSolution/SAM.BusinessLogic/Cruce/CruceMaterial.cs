using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using Mimo.Framework.Common;
using System.Data.Objects;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Personalizadas;

namespace SAM.BusinessLogic.Cruce
{
    public class CruceMaterial
    {
        private int _ordenTrabajoSpoolID;
        private Guid _userID;
        private List<NumeroUnico> _lstNumeroUnico;
        private List<ItemCode> _lstItemCode;
        private List<ItemCodeEquivalente> _lstEquivalente;
        private List<MaterialSpool> _lstMateriales;
        private List<JuntaSpool> _lstJuntas;
        private List<CruceItemCode> _lstCondensadoIC;
        private Dictionary<ItemCodeIntegrado, CruceItemCode> _dicCodensados;

        public CruceMaterial(int ordenTrabajoSpoolID, Guid userID)
        {
            _ordenTrabajoSpoolID = ordenTrabajoSpoolID;
            _userID = userID;
        }

        public OrdenTrabajoSpool Procesa(out bool exito, out List<NumeroUnico> congelados)
        {
            OrdenTrabajoSpool odts;

            using (SamContext ctx = new SamContext())
            {
                ctx.ItemCode.MergeOption = MergeOption.NoTracking;
                ctx.NumeroUnico.MergeOption = MergeOption.NoTracking;
                ctx.NumeroUnicoInventario.MergeOption = MergeOption.NoTracking;
                ctx.NumeroUnicoSegmento.MergeOption = MergeOption.NoTracking;
                ctx.MaterialSpool.MergeOption = MergeOption.NoTracking;
                ctx.JuntaSpool.MergeOption = MergeOption.NoTracking;
                ctx.ItemCodeEquivalente.MergeOption = MergeOption.NoTracking;

                odts = ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajoSpoolID == _ordenTrabajoSpoolID).Single();

                int proyectoID = ctx.Spool.Where(x => x.SpoolID == odts.SpoolID).Select(y => y.ProyectoID).Single();

                //Los materiales de ingeniería que no están en la ODT
                _lstMateriales = ctx.MaterialSpool
                                    .Where(x => x.SpoolID == odts.SpoolID)
                                    .Where(x => !x.OrdenTrabajoMaterial.Any())
                                    .ToList();

                int shopFabAreaID = CacheCatalogos.Instance.ShopFabAreaID;

                //Las juntas de ingeniería que no están en la ODT
                _lstJuntas = ctx.JuntaSpool
                                .Where(x => x.SpoolID == odts.SpoolID)
                                .Where(x => !x.OrdenTrabajoJunta.Any())
                                .Where(x => x.FabAreaID == shopFabAreaID)
                                .ToList();


                //Los I.C. ids requeridos por ingeniería
                IQueryable<int> icIdsDirectos =
                    ctx.MaterialSpool
                       .Where(Expressions.BuildOrExpression<MaterialSpool, int>(x => x.MaterialSpoolID, _lstMateriales.Select(z => z.MaterialSpoolID)))
                       .Select(x => x.ItemCodeID);

                //Los I.C. equivalentes
                IQueryable<ItemCodeEquivalente> qIcEq =
                    ctx.ItemCodeEquivalente.Where(x => icIdsDirectos.Contains(x.ItemCodeID));

                //Traer los Item codes tanto los de ingeniería como los equivalentes
                IQueryable<ItemCode> qIc =
                    ctx.ItemCode.Where(x => icIdsDirectos.Contains(x.ItemCodeID) ||
                                             qIcEq.Select(y => y.ItemEquivalenteID)
                                                  .Contains(x.ItemCodeID));

                //Solo los números únicos del proyecto cuyos item codes sean requeridos
                //por los spools pasados y que se puedan usar
                _lstNumeroUnico =
                    ctx.NumeroUnico
                       .Include("NumeroUnicoInventario")
                       .Include("NumeroUnicoSegmento")
                       .Where(x => x.ProyectoID == proyectoID && x.NumeroUnicoInventario.InventarioDisponibleCruce > 0)
                       .Where(x => qIc.Select(y => (int?)y.ItemCodeID).Contains(x.ItemCodeID))
                       .Where(x => !x.Colada.HoldCalidad)
                       .Where(x => x.Estatus.Equals(EstatusNumeroUnico.APROBADO))
                       .ToList();

                //Traer de BD todos los I.C. del proyecto requeridos
                _lstItemCode = qIc.ToList();
                //Traer de BD todos los I.C. equivalentes necesarios
                _lstEquivalente = qIcEq.ToList();

                //Lista con un condensado de la disponibilidad de números únicos agrupados por item code
                _lstCondensadoIC = UtileriasCruce.InventariosCondensadosPorIC(ctx, proyectoID).ToList();
            }

            estableceVariablesTemporales();
            _dicCodensados = _lstCondensadoIC.ToDictionary(x => new ItemCodeIntegrado { ItemCodeID = x.ItemCodeID, Diametro1 = x.Diametro1, Diametro2 = x.Diametro2 });
            recorreMaterialesYCongela();

            //Solo si todos los materiales tienen algún número único asociado significa que se pudo cruzar
            exito = _lstMateriales.All(x => x.InfoCruce.NumeroUnicoID > 0);

            if (exito)
            {
                //Regresar una lista con los nus congelados
                congelados = (from nus in _lstNumeroUnico
                              where nus.InfoCruce.SeUsoEnCruce
                              select nus).ToList();

                anexaMaterialesJuntasAOdt(odts);
                return odts;
            }
            else
            {
                congelados = null;
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="odt"></param>
        private void anexaMaterialesJuntasAOdt(OrdenTrabajoSpool odts)
        {
            odts.StartTracking();
            odts.FechaModificacion = DateTime.Now;
            odts.UsuarioModifica = _userID;

            #region Recorrer cada material

            foreach (MaterialSpool ms in _lstMateriales)
            {
                OrdenTrabajoMaterial odtm = new OrdenTrabajoMaterial
                {
                    CantidadCongelada = ms.Cantidad,
                    CantidadDespachada = 0,
                    CongeladoEsEquivalente = ms.InfoCruce.EsEquivalente,
                    DespachoEsEquivalente = false,
                    NumeroUnicoCongeladoID = ms.InfoCruce.NumeroUnicoID,
                    NumeroUnicoSugeridoID = ms.InfoCruce.EsSugerido ? (int?)ms.InfoCruce.NumeroUnicoID : null,
                    MaterialSpoolID = ms.MaterialSpoolID,
                    SegmentoCongelado = string.IsNullOrEmpty(ms.InfoCruce.Segmento) ? null : ms.InfoCruce.Segmento,
                    SegmentoSugerido = ms.InfoCruce.EsSugerido && !string.IsNullOrEmpty(ms.InfoCruce.Segmento) ? ms.InfoCruce.Segmento : null,
                    SugeridoEsEquivalente = ms.InfoCruce.EsEquivalente,
                    TieneInventarioCongelado = true,
                    FechaModificacion = DateTime.Now,
                    FueReingenieria = true,
                    UsuarioModifica = _userID
                };

                odts.OrdenTrabajoMaterial.Add(odtm);
            }

            #endregion

            #region Recorrer cada junta

            foreach (JuntaSpool js in _lstJuntas)
            {
                OrdenTrabajoJunta odtj = new OrdenTrabajoJunta
                {
                    FechaModificacion = DateTime.Now,
                    JuntaSpoolID = js.JuntaSpoolID,
                    FueReingenieria = true,
                    UsuarioModifica = _userID
                };

                odts.OrdenTrabajoJunta.Add(odtj);
            }

            #endregion

            odts.StopTracking();
        }


        /// <summary>
        /// Para poder afectar los inventarios material x material necesitamos poder llevar tracking de esos cambios
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

            //copiar las cantidades de cruce y congelados a temporales
            _lstCondensadoIC.ForEach(x =>
            {
                x.InventarioDisponibleCruceTemporal = x.InventarioDisponibleCruce;
                x.InventarioCongeladoTemporal = x.InventarioCongelado;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        private bool recorreMaterialesYCongela()
        {
            foreach (MaterialSpool material in _lstMateriales)
            {
                NumeroUnicoInventario nui = null;
                NumeroUnicoSegmento nuSegmento = null;

                ItemCode ic = _lstItemCode.Where(x => x.ItemCodeID == material.ItemCodeID).Single();

                //Si es accesorio se busca directamente sobre la tabla NumeroUnicoInventario
                if (ic.TipoMaterialID == (int)TipoMaterialEnum.Accessorio)
                {
                    nui = UtileriasCruce.ObtenMejorCandidatoAccesorio(_lstNumeroUnico, material, _dicCodensados);

                    if (nui == null)
                    {
                        nui = UtileriasCruce.IntentaConEquivalenciaDeAccesorio(material, _lstEquivalente, _lstNumeroUnico, _dicCodensados);

                        if (nui != null)
                        {
                            material.InfoCruce.EsEquivalente = true;
                        }
                    }

                    if (nui != null)
                    {
                        //Congelar temporalmente y disminuir la disponibilidad para cruce
                        nui.StartTracking();
                        nui.NumeroUnico.StartTracking();

                        nui.NumeroUnico.UsuarioModifica = _userID;
                        nui.NumeroUnico.FechaModificacion = DateTime.Now;
                        nui.NumeroUnico.InfoCruce.SeUsoEnCruce = true;
                        
                        nui.UsuarioModifica = _userID;
                        nui.FechaModificacion = DateTime.Now;
                        nui.InventarioDisponibleCruce -= material.Cantidad;
                        nui.InventarioCongelado += material.Cantidad;
                        nui.InfoCruce.InventarioDisponibleCruceTemporal -= material.Cantidad;
                        nui.InfoCruce.InventarioCongeladoTemporal += material.Cantidad;
                        
                        nui.StopTracking();
                        nui.NumeroUnico.StopTracking();

                        //Este lo vamos a usar para luego poder generar la ODT
                        material.InfoCruce.NumeroUnicoID = nui.NumeroUnicoID;

                        //Actualizar el temporal del registro correspondiente de los condensados de item codes
                        CruceItemCode cic = _dicCodensados[new ItemCodeIntegrado(nui.NumeroUnico.ItemCodeID.Value, nui.NumeroUnico.Diametro1, nui.NumeroUnico.Diametro2)];

                        cic.InventarioCongeladoTemporal += material.Cantidad;
                        cic.InventarioDisponibleCruceTemporal -= material.Cantidad;
                    }
                    else
                    {
                        //ya no tiene caso seguir, si no podemos surtir un material nos detenemos
                        return false;
                    }
                }
                else
                {
                    //si es tubo hay que buscar sobre la tabla NumeroUnicoSegmento
                    nuSegmento = UtileriasCruce.ObtenMejorCandidatoTubo(_lstNumeroUnico, material, _dicCodensados);

                    if (nuSegmento == null)
                    {
                        nuSegmento = UtileriasCruce.IntentaConEquivalenciaDeTubo(material, _lstEquivalente, _lstNumeroUnico, _dicCodensados);

                        if (nuSegmento != null)
                        {
                            material.InfoCruce.EsEquivalente = true;
                        }
                    }

                    if (nuSegmento != null)
                    {
                        nuSegmento.StartTracking();
                        nuSegmento.NumeroUnico.StartTracking();
                        nuSegmento.NumeroUnico.NumeroUnicoInventario.StartTracking();

                        nuSegmento.NumeroUnico.InfoCruce.SeUsoEnCruce = true;

                        nuSegmento.UsuarioModifica = _userID;
                        nuSegmento.NumeroUnico.UsuarioModifica = _userID;
                        nuSegmento.NumeroUnico.NumeroUnicoInventario.UsuarioModifica = _userID;

                        nuSegmento.FechaModificacion = DateTime.Now;
                        nuSegmento.NumeroUnico.FechaModificacion = DateTime.Now;
                        nuSegmento.NumeroUnico.NumeroUnicoInventario.FechaModificacion = DateTime.Now;

                        //si se encontró vamos a congelar temporalmente la cantidad
                        nuSegmento.NumeroUnico.NumeroUnicoInventario.InventarioCongelado += material.Cantidad;
                        nuSegmento.NumeroUnico.NumeroUnicoInventario.InventarioDisponibleCruce -= material.Cantidad;
                        nuSegmento.NumeroUnico.NumeroUnicoInventario.InfoCruce.InventarioCongeladoTemporal += material.Cantidad;
                        nuSegmento.NumeroUnico.NumeroUnicoInventario.InfoCruce.InventarioDisponibleCruceTemporal -= material.Cantidad;
                        
                        
                        nuSegmento.InventarioCongelado += material.Cantidad;
                        nuSegmento.InventarioDisponibleCruce -= material.Cantidad;
                        nuSegmento.InfoCruce.InventarioCongeladoTemporal += material.Cantidad;
                        nuSegmento.InfoCruce.InventarioDisponibleCruceTemporal -= material.Cantidad;

                        //Esto lo vamos a usar para la ODT
                        material.InfoCruce.NumeroUnicoID = nuSegmento.NumeroUnicoID;
                        material.InfoCruce.Segmento = nuSegmento.Segmento;

                        //Actualizar el temporal del registro correspondiente de los condensados de item codes
                        CruceItemCode cic = _dicCodensados[new ItemCodeIntegrado(nuSegmento.NumeroUnico.ItemCodeID.Value, nuSegmento.NumeroUnico.Diametro1, nuSegmento.NumeroUnico.Diametro2)];

                        cic.InventarioCongeladoTemporal += material.Cantidad;
                        cic.InventarioDisponibleCruceTemporal -= material.Cantidad;

                        //Significa que estamos utilizando un tubo ya cortado por lo tanto debe de sugerirse utilizar ese tramo en específico
                        if (nuSegmento.NumeroUnico.NumeroUnicoInventario.CantidadRecibida != nuSegmento.NumeroUnico.NumeroUnicoInventario.InventarioFisico)
                        {
                            material.InfoCruce.EsSugerido = true;
                        }

                        nuSegmento.NumeroUnico.NumeroUnicoInventario.StopTracking();
                        nuSegmento.NumeroUnico.StopTracking();
                        nuSegmento.StopTracking();
                    }
                    else
                    {
                        //ya no tiene caso seguir, si no podemos surtir un material nos detenemos
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
