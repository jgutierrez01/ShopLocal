using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using System.Data.Objects;
using Mimo.Framework.Common;
using SAM.Entities.Reportes;
using SAM.Entities.Personalizadas;

namespace SAM.BusinessLogic.Cruce
{
    public static class UtileriasCruce
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="material"></param>
        /// <param name="nombreProyecto"></param>
        /// <param name="dicIc"></param>
        /// <param name="dicSpool"></param>
        /// <param name="congelado"></param>
        /// <returns></returns>
        public static FaltanteCruce GeneraFaltante(MaterialSpool material, string nombreProyecto, Dictionary<int, ItemCode> dicIc,
            Dictionary<int, Spool> dicSpool, bool congelado, NumeroUnico nu, Dictionary<int, string> famAceros, string observacionesHold, bool isoCompleto)
        {
            ItemCode ic = dicIc[material.ItemCodeID];
            Spool sp = dicSpool[material.SpoolID];

            return new FaltanteCruce
            {
                SpoolID = material.SpoolID,
                MaterialSpoolID = material.MaterialSpoolID,
                Cantidad = material.Cantidad,
                Diametro1 = material.Diametro1,
                Diametro2 = material.Diametro2,
                ItemCodeID = material.ItemCodeID,
                CodigoItemCode = ic.Codigo,
                DescripcionItemCode = ic.DescripcionEspanol,
                Especificacion = material.Especificacion,
                Etiqueta = material.Etiqueta,
                Grupo = material.Grupo,
                NombreSpool = sp.Nombre,
                Proyecto = nombreProyecto,
                Prioridad = sp.Prioridad ?? 999,
                Congelado = congelado,
                MaterialEquivalente = material.InfoCruce.EsEquivalente,
                Isometrico = sp.Dibujo,
                Cedula = sp.Cedula,
                NumeroUnicoCongelado = nu != null ? nu.Codigo : string.Empty,
                NumeroUnicoUtilizado = nu,
                Pdis = sp.Pdis.HasValue ? sp.Pdis.Value : 0,
                Peso = sp.Peso.HasValue ? sp.Peso.Value : 0,
                FamiliaAcero = sp.FamiliaAcero2ID.HasValue ? famAceros[sp.FamiliaAcero1ID] + "/" + famAceros[sp.FamiliaAcero2ID.Value] : famAceros[sp.FamiliaAcero1ID],
                SpoolEnHold = !String.IsNullOrEmpty(observacionesHold) ? true : false,
                ObservacionesSpoolHold = !String.IsNullOrEmpty(observacionesHold) ? observacionesHold : String.Empty,
                IsometricoCompleto = isoCompleto,
                Campo1 = sp.Campo1,
                Campo2 = sp.Campo2,
                DiametroMayor = sp.DiametroMayor.HasValue ? sp.DiametroMayor.Value :0
            };
        }


        public static void QuitaCongelados(List<NumeroUnico> inventarios, Dictionary<ItemCodeIntegrado, CruceItemCode> condensadoIC)
        {
            inventarios.ForEach(x =>
            {
                x.NumeroUnicoInventario.InfoCruce.InventarioCongeladoTemporal = x.NumeroUnicoInventario.InventarioCongelado;
                x.NumeroUnicoInventario.InfoCruce.InventarioDisponibleCruceTemporal = x.NumeroUnicoInventario.InventarioDisponibleCruce;

                foreach (NumeroUnicoSegmento segmento in x.NumeroUnicoSegmento)
                {
                    segmento.InfoCruce.InventarioDisponibleCruceTemporal = segmento.InventarioDisponibleCruce;
                    segmento.InfoCruce.InventarioCongeladoTemporal = segmento.InventarioCongelado;
                }

                //Actualizar los congelados con los valores de los reales
                CruceItemCode cond = condensadoIC[new ItemCodeIntegrado(x.ItemCodeID.Value, x.Diametro1, x.Diametro2)];


                cond.InventarioDisponibleCruceTemporal = cond.InventarioDisponibleCruce;
                cond.InventarioCongeladoTemporal = cond.InventarioCongelado;
            });
        }

        public static void CongelaInventarios(List<NumeroUnico> inventarios, Dictionary<ItemCodeIntegrado, CruceItemCode> condensadoIC)
        {
            CongelaInventarios(inventarios, condensadoIC, false, null);
        }

        public static void CongelaInventarios(List<NumeroUnico> inventarios, Dictionary<ItemCodeIntegrado, CruceItemCode> condensadoIC, bool llevaTrack, Guid? userID)
        {
            inventarios.ForEach(x =>
            {
                //En la lista pueden haber números únicos que en realidad no se hayan usado para el cruce, esos
                //los excluimos con este if
                if (x.NumeroUnicoInventario.InventarioCongelado != x.NumeroUnicoInventario.InfoCruce.InventarioCongeladoTemporal)
                {
                    if (llevaTrack)
                    {
                        x.StartTracking();
                        x.UsuarioModifica = userID;
                        x.FechaModificacion = DateTime.Now;
                        x.InfoCruce.SeUsoEnCruce = true;

                        x.NumeroUnicoInventario.StartTracking();
                        x.UsuarioModifica = userID;
                        x.FechaModificacion = DateTime.Now;
                    }

                    x.NumeroUnicoInventario.InventarioDisponibleCruce = x.NumeroUnicoInventario.InfoCruce.InventarioDisponibleCruceTemporal;
                    x.NumeroUnicoInventario.InventarioCongelado = x.NumeroUnicoInventario.InfoCruce.InventarioCongeladoTemporal;

                    foreach (NumeroUnicoSegmento segmento in x.NumeroUnicoSegmento)
                    {
                        //Solo prestar atención a aquellos segmentos que en efecto hayan cambiado, los demás
                        //no los queremos tocar para disminuir el impacto a BD
                        if (segmento.InventarioCongelado != segmento.InfoCruce.InventarioCongeladoTemporal)
                        {
                            if (llevaTrack)
                            {
                                segmento.StartTracking();
                                segmento.UsuarioModifica = userID;
                                segmento.FechaModificacion = DateTime.Now;
                            }

                            segmento.InventarioDisponibleCruce = segmento.InfoCruce.InventarioDisponibleCruceTemporal;
                            segmento.InventarioCongelado = segmento.InfoCruce.InventarioCongeladoTemporal;

                            if (llevaTrack)
                            {
                                segmento.StopTracking();
                            }
                        }
                    }

                    if (llevaTrack)
                    {
                        x.NumeroUnicoInventario.StopTracking();
                        x.StopTracking();
                    }

                    //Actualizar los reales con los valores de los temporales
                    CruceItemCode cond = condensadoIC[new ItemCodeIntegrado(x.ItemCodeID.Value, x.Diametro1, x.Diametro2)];

                    cond.InventarioDisponibleCruce = cond.InventarioDisponibleCruceTemporal;
                    cond.InventarioCongelado = cond.InventarioCongeladoTemporal;
                }
            });
        }

        public static NumeroUnicoSegmento ObtenMejorCandidatoTubo(List<NumeroUnico> inventarios, MaterialSpool material, Dictionary<ItemCodeIntegrado, CruceItemCode> condensadoIC)
        {
            if (!ExisteSuficienteDelItemCode(material, condensadoIC))
            {
                return null;
            }

            NumeroUnicoSegmento
            nuSegmento = (from nus in inventarios
                          where nus.ItemCodeID == material.ItemCodeID
                                && nus.Diametro1 == material.Diametro1
                                && nus.Diametro2 == material.Diametro2
                          from segs in nus.NumeroUnicoSegmento
                          where segs.InfoCruce.InventarioDisponibleCruceTemporal >= material.Cantidad
                          orderby segs.InfoCruce.InventarioDisponibleCruceTemporal
                          select segs).FirstOrDefault();
            return nuSegmento;
        }

        public static NumeroUnicoInventario ObtenMejorCandidatoAccesorio(List<NumeroUnico> inventarios, MaterialSpool material, Dictionary<ItemCodeIntegrado, CruceItemCode> condensadoIC)
        {
            if (!ExisteSuficienteDelItemCode(material, condensadoIC))
            {
                return null;
            }

            NumeroUnicoInventario
            nui = (from nus in inventarios
                   where nus.ItemCodeID == material.ItemCodeID
                         && nus.Diametro1 == material.Diametro1
                         && nus.Diametro2 == material.Diametro2
                         && nus.NumeroUnicoInventario.InfoCruce.InventarioDisponibleCruceTemporal >= material.Cantidad
                   orderby nus.NumeroUnicoInventario.InfoCruce.InventarioDisponibleCruceTemporal
                   select nus.NumeroUnicoInventario).FirstOrDefault();

            return nui;
        }

        public static bool ExisteSuficienteDelItemCode(MaterialSpool material, Dictionary<ItemCodeIntegrado, CruceItemCode> condensadoIC)
        {
            ItemCodeIntegrado ici = new ItemCodeIntegrado(material.ItemCodeID, material.Diametro1, material.Diametro2);

            return condensadoIC.ContainsKey(ici) && condensadoIC[ici].InventarioDisponibleCruceTemporal >= material.Cantidad;
        }

        public static NumeroUnicoSegmento IntentaConEquivalenciaDeTubo(MaterialSpool material, List<ItemCodeEquivalente> lstEquivalente, List<NumeroUnico> lstNumeroUnico, Dictionary<ItemCodeIntegrado, CruceItemCode> condensadoIC)
        {
            List<NumeroUnico> nums = ObtenInventariosDeEquivalecias(material, lstEquivalente, lstNumeroUnico, condensadoIC);

            //Al ya haber obtenido los equivalentes podemos seleccionar uno de los equivalentes de manera directa
            NumeroUnicoSegmento
                nuSegmento = (from nus in nums
                              from segs in nus.NumeroUnicoSegmento
                              where segs.InfoCruce.InventarioDisponibleCruceTemporal >= material.Cantidad
                              orderby segs.InfoCruce.InventarioDisponibleCruceTemporal
                              select segs).FirstOrDefault();

            return nuSegmento;
        }

        public static NumeroUnicoInventario IntentaConEquivalenciaDeAccesorio(MaterialSpool material, List<ItemCodeEquivalente> lstEquivalente, List<NumeroUnico> lstNumeroUnico, Dictionary<ItemCodeIntegrado, CruceItemCode> condensadoIC)
        {
            List<NumeroUnico> nums = ObtenInventariosDeEquivalecias(material, lstEquivalente, lstNumeroUnico, condensadoIC);

            //Al ya haber obtenido los equivalentes podemos seleccionar uno de manera directa siempre y cuando haya inventario
            NumeroUnicoInventario
            nui = (from nus in nums
                   where nus.NumeroUnicoInventario.InfoCruce.InventarioDisponibleCruceTemporal >= material.Cantidad
                   orderby nus.NumeroUnicoInventario.InfoCruce.InventarioDisponibleCruceTemporal
                   select nus.NumeroUnicoInventario).FirstOrDefault();

            return nui;
        }

        public static List<NumeroUnico> ObtenInventariosDeEquivalecias(MaterialSpool material, List<ItemCodeEquivalente> lstEquivalente, List<NumeroUnico> lstNumeroUnico, Dictionary<ItemCodeIntegrado, CruceItemCode> condensadoIC)
        {
            List<ItemCodeIntegrado> equivalentes =
            (from eqs in lstEquivalente
             where eqs.ItemCodeID == material.ItemCodeID
                   && eqs.Diametro1 == material.Diametro1
                   && eqs.Diametro2 == material.Diametro2
             select new ItemCodeIntegrado
             {
                 ItemCodeID = eqs.ItemEquivalenteID,
                 Diametro1 = eqs.DiametroEquivalente1,
                 Diametro2 = eqs.DiametroEquivalente2
             }).ToList();


            List<NumeroUnico> nums =
            (from nus in lstNumeroUnico
             join eqs in equivalentes on
             new ItemCodeIntegrado
             {
                 ItemCodeID = nus.ItemCodeID.Value,
                 Diametro1 = nus.Diametro1,
                 Diametro2 = nus.Diametro2
             }
             equals eqs
             join cond in condensadoIC on eqs equals cond.Key
             where cond.Value.InventarioDisponibleCruceTemporal >= material.Cantidad
             select nus).ToList();
            return nums;
        }

        public static List<NumeroUnico> NumerosUnicosDisponibles(Spool spool, List<NumeroUnico> lstNumeroUnico)
        {
            return
            (from nus in lstNumeroUnico
             join matSpool in spool.MaterialSpool on
             new ItemCodeIntegrado
             {
                 ItemCodeID = nus.ItemCodeID.Value,
                 Diametro1 = nus.Diametro1,
                 Diametro2 = nus.Diametro2
             }
             equals
             new ItemCodeIntegrado
             {
                 ItemCodeID = matSpool.ItemCodeID,
                 Diametro1 = matSpool.Diametro1,
                 Diametro2 = matSpool.Diametro2
             }
             select nus).Distinct().ToList();
        }

        public static List<NumeroUnico> NumerosUnicosDisponiblesPorMaterial(MaterialSpool material, List<NumeroUnico> lstNumeroUnico)
        {
            return
            (from nus in lstNumeroUnico
             where nus.ItemCodeID.Value == material.ItemCodeID
                   && nus.Diametro1 == material.Diametro1
                   && nus.Diametro2 == material.Diametro2
             select nus).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spool"></param>
        /// <param name="invParaSpool"></param>
        /// <param name="equivDeSpool"></param>
        /// <param name="lstEquivalentesProyecto"></param>
        /// <param name="inventariosProyecto"></param>
        /// <returns></returns>
        public static bool BuscaMaterial(Spool spool, List<NumeroUnico> invParaSpool, List<NumeroUnico> equivDeSpool, List<CongeladoParcial> congParcial,
            List<ItemCodeEquivalente> lstEquivalentesProyecto, List<NumeroUnico> inventariosProyecto, Dictionary<ItemCodeIntegrado, CruceItemCode> condensadoIC,
            Dictionary<int, ItemCode> dicIc)
        {
            NumeroUnicoInventario nui = null;
            NumeroUnicoSegmento nuSegmento = null;
            bool hayParaTodoElSpool = true;

            //Excluir de Material Spool aquellos materiales que ya tengan registrado un numero unico
            foreach (MaterialSpool material in spool.MaterialSpool)
            {
                //Si es accesorio se busca directamente sobre la tabla NumeroUnicoInventario
                if (dicIc[material.ItemCodeID].TipoMaterialID == (int)TipoMaterialEnum.Accessorio)
                {
                    //Si el material ha sido congelado Parcialmente entra directo
                    if (congParcial.Where(x => x.MaterialSpoolID == material.MaterialSpoolID).Any())
                    {
                        CongeladoParcial congelado = congParcial.Where(x => x.MaterialSpoolID == material.MaterialSpoolID).FirstOrDefault();
                        material.InfoCruce.NumeroUnicoID = congelado.NumeroUnicoCongeladoID;
                        material.InfoCruce.EsEquivalente = congelado.EsEquivalente;
                    }
                    else
                    {
                        nui = ObtenMejorCandidatoAccesorio(invParaSpool, material, condensadoIC);

                        if (nui == null)
                        {
                            nui = IntentaConEquivalenciaDeAccesorio(material, lstEquivalentesProyecto, inventariosProyecto, condensadoIC);

                            if (nui != null)
                            {
                                spool.InfoCruce.UsoEquivalencia = true;
                                material.InfoCruce.EsEquivalente = true;
                                equivDeSpool.Add(nui.NumeroUnico);
                            }
                        }

                        if (nui != null)
                        {
                            //Congelar temporalmente y disminuir la disponibilidad para cruce
                            nui.InfoCruce.InventarioCongeladoTemporal += material.Cantidad;
                            nui.InfoCruce.InventarioDisponibleCruceTemporal -= material.Cantidad;
                            //nui.InfoCruce.EsCongeladoParcial = false;

                            //Este lo vamos a usar para luego poder generar la ODT
                            material.InfoCruce.NumeroUnicoID = nui.NumeroUnicoID;
                        }
                        else
                        {
                            //ya no tiene caso seguir, si no podemos surtir un material nos detenemos
                            hayParaTodoElSpool = false;
                            break;
                        }
                        //Este lo vamos a usar para luego poder generar la ODT
                        material.InfoCruce.NumeroUnicoID = nui.NumeroUnicoID;

                        //Actualizar el temporal del registro correspondiente de los condensados de item codes
                        CruceItemCode ic = condensadoIC[new ItemCodeIntegrado(nui.NumeroUnico.ItemCodeID.Value, nui.NumeroUnico.Diametro1, nui.NumeroUnico.Diametro2)];

                        ic.InventarioCongeladoTemporal += material.Cantidad;
                        ic.InventarioDisponibleCruceTemporal -= material.Cantidad;
                    }
                }
                else
                {
                    //Si el material ha sido congelado Parcialmente
                    if (congParcial.Where(x => x.MaterialSpoolID == material.MaterialSpoolID).Any())
                    {
                        CongeladoParcial congeladoParcial = congParcial.Where(x => x.MaterialSpoolID == material.MaterialSpoolID).FirstOrDefault();
                        material.InfoCruce.NumeroUnicoID = congeladoParcial.NumeroUnicoCongeladoID;
                        material.InfoCruce.Segmento = congeladoParcial.SegmentoCongelado;
                        material.InfoCruce.EsEquivalente = congeladoParcial.EsEquivalente;
                    }
                    else

                        //si es tubo hay que buscar sobre la tabla NumeroUnicoSegmento
                        nuSegmento = ObtenMejorCandidatoTubo(invParaSpool, material, condensadoIC);

                    if (nuSegmento == null)
                    {
                        nuSegmento = IntentaConEquivalenciaDeTubo(material, lstEquivalentesProyecto, inventariosProyecto, condensadoIC);

                        if (nuSegmento != null)
                        {
                            spool.InfoCruce.UsoEquivalencia = true;
                            material.InfoCruce.EsEquivalente = true;
                            //nuSegmento.InfoCruce.EsCongeladoParcial = false;
                            equivDeSpool.Add(nuSegmento.NumeroUnico);
                        }
                    }

                    if (nuSegmento != null)
                    {
                        //si se encontró vamos a congelar temporalmente la cantidad
                        nuSegmento.NumeroUnico.NumeroUnicoInventario.InfoCruce.InventarioCongeladoTemporal += material.Cantidad;
                        nuSegmento.NumeroUnico.NumeroUnicoInventario.InfoCruce.InventarioDisponibleCruceTemporal -= material.Cantidad;
                        nuSegmento.InfoCruce.InventarioCongeladoTemporal += material.Cantidad;
                        nuSegmento.InfoCruce.InventarioDisponibleCruceTemporal -= material.Cantidad;


                        //Esto lo vamos a usar para la ODT
                        material.InfoCruce.NumeroUnicoID = nuSegmento.NumeroUnicoID;
                        material.InfoCruce.Segmento = nuSegmento.Segmento;

                        //Significa que estamos utilizando un tubo ya cortado por lo tanto debe de sugerirse utilizar ese tramo en específico
                        if (nuSegmento.NumeroUnico.NumeroUnicoInventario.CantidadRecibida != nuSegmento.NumeroUnico.NumeroUnicoInventario.InventarioFisico)
                        {
                            material.InfoCruce.EsSugerido = true;
                        }
                    }
                    else
                    {
                        //ya no tiene caso seguir, si no podemos surtir un material nos detenemos
                        hayParaTodoElSpool = false;
                        break;
                    }

                    //Actualizar el temporal del registro correspondiente de los condensados de item codes
                    CruceItemCode ic = condensadoIC[new ItemCodeIntegrado(nuSegmento.NumeroUnico.ItemCodeID.Value, nuSegmento.NumeroUnico.Diametro1, nuSegmento.NumeroUnico.Diametro2)];

                    ic.InventarioCongeladoTemporal += material.Cantidad;
                    ic.InventarioDisponibleCruceTemporal -= material.Cantidad;

                }
            }


            return hayParaTodoElSpool;
        }



        public static bool BuscaMaterialSpoolMarcadosRevision(Spool spool, List<NumeroUnico> invParaSpool, List<NumeroUnico> equivDeSpool,
            List<ItemCodeEquivalente> lstEquivalentesProyecto, List<NumeroUnico> inventariosProyecto,
            Dictionary<ItemCodeIntegrado, CruceItemCode> condensadoIC, Dictionary<int, ItemCode> dicIc)
        {
            NumeroUnicoInventario nui = null;
            NumeroUnicoSegmento nuSegmento = null;
            bool hayParaTodoElSpool = true;
            using (SamContext ctx = new SamContext())
            {
                //Excluir de Material Spool aquellos materiales que ya tengan registrado un numero unico
                foreach (MaterialSpool material in spool.MaterialSpool)
                {
                    bool tieneNumerosUnicos = (from otm in ctx.OrdenTrabajoMaterial
                                               where otm.MaterialSpoolID == material.MaterialSpoolID
                                               && (otm.NumeroUnicoCongeladoID != null || otm.NumeroUnicoDespachadoID != null)
                                               select otm).Any();
                    if (!tieneNumerosUnicos)
                    {
                        //Si es accesorio se busca directamente sobre la tabla NumeroUnicoInventario
                        if (dicIc[material.ItemCodeID].TipoMaterialID == (int)TipoMaterialEnum.Accessorio)
                        {
                            nui = ObtenMejorCandidatoAccesorio(invParaSpool, material, condensadoIC);

                            if (nui == null)
                            {
                                nui = IntentaConEquivalenciaDeAccesorio(material, lstEquivalentesProyecto, inventariosProyecto, condensadoIC);

                                if (nui != null)
                                {
                                    spool.InfoCruce.UsoEquivalencia = true;
                                    material.InfoCruce.EsEquivalente = true;
                                    equivDeSpool.Add(nui.NumeroUnico);
                                }
                            }

                            if (nui != null)
                            {
                                //Congelar temporalmente y disminuir la disponibilidad para cruce
                                nui.InfoCruce.InventarioCongeladoTemporal += material.Cantidad;
                                nui.InfoCruce.InventarioDisponibleCruceTemporal -= material.Cantidad;
                                //nui.InfoCruce.EsCongeladoParcial = false;

                                //Este lo vamos a usar para luego poder generar la ODT
                                material.InfoCruce.NumeroUnicoID = nui.NumeroUnicoID;
                            }
                            else
                            {
                                //ya no tiene caso seguir, si no podemos surtir un material nos detenemos
                                hayParaTodoElSpool = false;
                                break;
                            }
                            //Este lo vamos a usar para luego poder generar la ODT
                            material.InfoCruce.NumeroUnicoID = nui.NumeroUnicoID;

                            //Actualizar el temporal del registro correspondiente de los condensados de item codes
                            CruceItemCode ic = condensadoIC[new ItemCodeIntegrado(nui.NumeroUnico.ItemCodeID.Value, nui.NumeroUnico.Diametro1, nui.NumeroUnico.Diametro2)];

                            ic.InventarioCongeladoTemporal += material.Cantidad;
                            ic.InventarioDisponibleCruceTemporal -= material.Cantidad;

                        }
                        else
                        {
                            //si es tubo hay que buscar sobre la tabla NumeroUnicoSegmento
                            nuSegmento = ObtenMejorCandidatoTubo(invParaSpool, material, condensadoIC);

                            if (nuSegmento == null)
                            {
                                nuSegmento = IntentaConEquivalenciaDeTubo(material, lstEquivalentesProyecto, inventariosProyecto, condensadoIC);

                                if (nuSegmento != null)
                                {
                                    spool.InfoCruce.UsoEquivalencia = true;
                                    material.InfoCruce.EsEquivalente = true;
                                    //nuSegmento.InfoCruce.EsCongeladoParcial = false;
                                    equivDeSpool.Add(nuSegmento.NumeroUnico);
                                }
                            }

                            if (nuSegmento != null)
                            {
                                //si se encontró vamos a congelar temporalmente la cantidad
                                nuSegmento.NumeroUnico.NumeroUnicoInventario.InfoCruce.InventarioCongeladoTemporal += material.Cantidad;
                                nuSegmento.NumeroUnico.NumeroUnicoInventario.InfoCruce.InventarioDisponibleCruceTemporal -= material.Cantidad;
                                nuSegmento.InfoCruce.InventarioCongeladoTemporal += material.Cantidad;
                                nuSegmento.InfoCruce.InventarioDisponibleCruceTemporal -= material.Cantidad;


                                //Esto lo vamos a usar para la ODT
                                material.InfoCruce.NumeroUnicoID = nuSegmento.NumeroUnicoID;
                                material.InfoCruce.Segmento = nuSegmento.Segmento;

                                //Significa que estamos utilizando un tubo ya cortado por lo tanto debe de sugerirse utilizar ese tramo en específico
                                if (nuSegmento.NumeroUnico.NumeroUnicoInventario.CantidadRecibida != nuSegmento.NumeroUnico.NumeroUnicoInventario.InventarioFisico)
                                {
                                    material.InfoCruce.EsSugerido = true;
                                }
                            }
                            else
                            {
                                //ya no tiene caso seguir, si no podemos surtir un material nos detenemos
                                hayParaTodoElSpool = false;
                                break;
                            }

                            //Actualizar el temporal del registro correspondiente de los condensados de item codes
                            CruceItemCode ic = condensadoIC[new ItemCodeIntegrado(nuSegmento.NumeroUnico.ItemCodeID.Value, nuSegmento.NumeroUnico.Diametro1, nuSegmento.NumeroUnico.Diametro2)];

                            ic.InventarioCongeladoTemporal += material.Cantidad;
                            ic.InventarioDisponibleCruceTemporal -= material.Cantidad;

                        }
                    }
                }
            }

            return hayParaTodoElSpool;
        }


        public static bool BuscaMaterialSpoolEnRevisionSeleccionados(Spool spool, List<NumeroUnico> invParaSpool, List<NumeroUnico> equivDeSpool,
           List<CongeladoParcial> congParcial, List<ItemCodeEquivalente> lstEquivalentesProyecto, List<NumeroUnico> inventariosProyecto,
           Dictionary<ItemCodeIntegrado, CruceItemCode> condensadoIC, Dictionary<int, ItemCode> dicIc, Guid userID)
        {
            NumeroUnicoInventario nui = null;
            NumeroUnicoSegmento nuSegmento = null;
            bool hayParaTodoElSpool = true;
            CongeladoParcial congeladoParcial;
            CruceItemCode ic;

            using (SamContext ctx = new SamContext())
            {
                //Excluir de Material Spool aquellos materiales que ya tengan registrado un numero unico
                foreach (MaterialSpool material in spool.MaterialSpool)
                {
                    bool tieneNumerosUnicos = (from otm in ctx.OrdenTrabajoMaterial
                                               where otm.MaterialSpoolID == material.MaterialSpoolID
                                               && (otm.NumeroUnicoCongeladoID != null || otm.NumeroUnicoDespachadoID != null)
                                               select otm).Any();
                    if (!tieneNumerosUnicos)
                    {
                        //Si es accesorio se busca directamente sobre la tabla NumeroUnicoInventario
                        if (dicIc[material.ItemCodeID].TipoMaterialID == (int)TipoMaterialEnum.Accessorio)
                        {
                            //Si el material ha sido congelado Parcialmente entra directo
                            if (congParcial.Where(x => x.MaterialSpoolID == material.MaterialSpoolID).Any())
                            {
                                congeladoParcial = congParcial.Where(x => x.MaterialSpoolID == material.MaterialSpoolID).FirstOrDefault();
                                material.InfoCruce.NumeroUnicoID = congeladoParcial.NumeroUnicoCongeladoID;
                                material.InfoCruce.EsEquivalente = congeladoParcial.EsEquivalente;
                            }
                            else
                            {
                                nui = ObtenMejorCandidatoAccesorio(invParaSpool, material, condensadoIC);

                                if (nui == null)
                                {
                                    nui = IntentaConEquivalenciaDeAccesorio(material, lstEquivalentesProyecto, inventariosProyecto, condensadoIC);

                                    if (nui != null)
                                    {
                                        spool.InfoCruce.UsoEquivalencia = true;
                                        material.InfoCruce.EsEquivalente = true;
                                        equivDeSpool.Add(nui.NumeroUnico);
                                    }
                                }

                                if (nui != null)
                                {
                                    //Congelar temporalmente y disminuir la disponibilidad para cruce
                                    nui.InfoCruce.InventarioCongeladoTemporal += material.Cantidad;
                                    nui.InfoCruce.InventarioDisponibleCruceTemporal -= material.Cantidad;
                                    //nui.InfoCruce.EsCongeladoParcial = false;

                                    //Este lo vamos a usar para luego poder generar la ODT
                                    material.InfoCruce.NumeroUnicoID = nui.NumeroUnicoID;
                                }
                                else
                                {
                                    //ya no tiene caso seguir, si no podemos surtir un material nos detenemos
                                    hayParaTodoElSpool = false;
                                    break;
                                }
                                //Este lo vamos a usar para luego poder generar la ODT
                                material.InfoCruce.NumeroUnicoID = nui.NumeroUnicoID;

                                //Actualizar el temporal del registro correspondiente de los condensados de item codes
                                ic = condensadoIC[new ItemCodeIntegrado(nui.NumeroUnico.ItemCodeID.Value, nui.NumeroUnico.Diametro1, nui.NumeroUnico.Diametro2)];

                                ic.InventarioCongeladoTemporal += material.Cantidad;
                                ic.InventarioDisponibleCruceTemporal -= material.Cantidad;
                            }
                        }
                        else
                        {
                            //Si el material ha sido congelado Parcialmente
                            if (congParcial.Where(x => x.MaterialSpoolID == material.MaterialSpoolID).Any())
                            {
                                congeladoParcial = congParcial.Where(x => x.MaterialSpoolID == material.MaterialSpoolID).FirstOrDefault();
                                material.InfoCruce.NumeroUnicoID = congeladoParcial.NumeroUnicoCongeladoID;
                                material.InfoCruce.Segmento = congeladoParcial.SegmentoCongelado;
                                material.InfoCruce.EsEquivalente = congeladoParcial.EsEquivalente;
                            }
                            else

                                //si es tubo hay que buscar sobre la tabla NumeroUnicoSegmento
                                nuSegmento = ObtenMejorCandidatoTubo(invParaSpool, material, condensadoIC);

                            if (nuSegmento == null)
                            {
                                nuSegmento = IntentaConEquivalenciaDeTubo(material, lstEquivalentesProyecto, inventariosProyecto, condensadoIC);

                                if (nuSegmento != null)
                                {
                                    spool.InfoCruce.UsoEquivalencia = true;
                                    material.InfoCruce.EsEquivalente = true;
                                    //nuSegmento.InfoCruce.EsCongeladoParcial = false;
                                    equivDeSpool.Add(nuSegmento.NumeroUnico);
                                }
                            }

                            if (nuSegmento != null)
                            {
                                //si se encontró vamos a congelar temporalmente la cantidad
                                nuSegmento.NumeroUnico.NumeroUnicoInventario.InfoCruce.InventarioCongeladoTemporal += material.Cantidad;
                                nuSegmento.NumeroUnico.NumeroUnicoInventario.InfoCruce.InventarioDisponibleCruceTemporal -= material.Cantidad;
                                nuSegmento.InfoCruce.InventarioCongeladoTemporal += material.Cantidad;
                                nuSegmento.InfoCruce.InventarioDisponibleCruceTemporal -= material.Cantidad;


                                //Esto lo vamos a usar para la ODT
                                material.InfoCruce.NumeroUnicoID = nuSegmento.NumeroUnicoID;
                                material.InfoCruce.Segmento = nuSegmento.Segmento;

                                //Significa que estamos utilizando un tubo ya cortado por lo tanto debe de sugerirse utilizar ese tramo en específico
                                if (nuSegmento.NumeroUnico.NumeroUnicoInventario.CantidadRecibida != nuSegmento.NumeroUnico.NumeroUnicoInventario.InventarioFisico)
                                {
                                    material.InfoCruce.EsSugerido = true;
                                }
                            }
                            else
                            {
                                //ya no tiene caso seguir, si no podemos surtir un material nos detenemos
                                hayParaTodoElSpool = false;
                                break;
                            }

                            //Actualizar el temporal del registro correspondiente de los condensados de item codes
                            ic = condensadoIC[new ItemCodeIntegrado(nuSegmento.NumeroUnico.ItemCodeID.Value, nuSegmento.NumeroUnico.Diametro1, nuSegmento.NumeroUnico.Diametro2)];

                            ic.InventarioCongeladoTemporal += material.Cantidad;
                            ic.InventarioDisponibleCruceTemporal -= material.Cantidad;

                        }

                        int otSpool = ctx.OrdenTrabajoSpool.Where(x => x.SpoolID == spool.SpoolID).Select(x => x.OrdenTrabajoSpoolID).Single();
                        #region Generar Ordenes de trabajo Material Faltantes
                        OrdenTrabajoMaterial odtm = new OrdenTrabajoMaterial();
                        odtm.StartTracking();
                        odtm.OrdenTrabajoSpoolID = otSpool;
                        odtm.FechaModificacion = DateTime.Now.Date;
                        odtm.UsuarioModifica = userID;
                        odtm.NumeroUnicoCongeladoID = material.InfoCruce.NumeroUnicoID;
                        odtm.NumeroUnicoSugeridoID = material.InfoCruce.EsSugerido ? (int?)material.InfoCruce.NumeroUnicoID : null;
                        odtm.CantidadCongelada = material.Cantidad;
                        odtm.CongeladoEsEquivalente = material.InfoCruce.EsEquivalente;
                        odtm.MaterialSpoolID = material.MaterialSpoolID;
                        odtm.SegmentoCongelado = string.IsNullOrEmpty(material.InfoCruce.Segmento) ? null : material.InfoCruce.Segmento;
                        odtm.SegmentoSugerido = material.InfoCruce.EsSugerido && !string.IsNullOrEmpty(material.InfoCruce.Segmento) ? material.InfoCruce.Segmento : null;
                        odtm.SugeridoEsEquivalente = material.InfoCruce.EsEquivalente;
                        odtm.TieneInventarioCongelado = true;
                        odtm.FueReingenieria = false;
                        odtm.StopTracking();
                        ctx.OrdenTrabajoMaterial.ApplyChanges(odtm);
                        ctx.SaveChanges();
                        #endregion
                    }

                }
            }
            return hayParaTodoElSpool;
        }



        #region Queries compilados

        public static readonly Func<SamContext, int, IQueryable<Spool>> SpoolsSinOdtSinTracking =
        CompiledQuery.Compile<SamContext, int, IQueryable<Spool>>
        (
            (ctx, id) => ctx.Spool 
                            .Include("MaterialSpool") 
                            .Where(x => !ctx.OrdenTrabajoSpool 
                                            .Select(y => y.SpoolID) 
                                            .Contains(x.SpoolID)) 
                           .Where(x => x.ProyectoID == id && x.PendienteDocumental 
                               && x.AprobadoParaCruce && ( x.Prioridad == null || x.Prioridad > 0 )) 


        );

        //public static readonly Func<SamContext, int, IQueryable<Spool>> SpoolMarcadosComoRevision =
        //    CompiledQuery.Compile<SamContext, int, IQueryable<Spool>>
        //(
        //    (ctx, id) => ctx.Spool
        //                    .Include("MaterialSpool")
        //                    .Where(x => x.ProyectoID == id && x.EsRevision == true && (x.UltimaOrdenTrabajoEspecial == null || x.UltimaOrdenTrabajoEspecial == ""))
        //);

        public static readonly Func<SamContext, int, IQueryable<MaterialSpool>> MaterialesSpoolsSinOdtSinTracking =
        CompiledQuery.Compile<SamContext, int, IQueryable<MaterialSpool>>
        (
            (ctx, id) => ctx.MaterialSpool.Where
                         (
                            ms => ctx.Spool
                                     .Where(x => !ctx.OrdenTrabajoSpool.Select(y => y.SpoolID).Contains(x.SpoolID))
                                     .Where(x => x.ProyectoID == id)
                                     .Select(z => z.SpoolID)
                                     .Contains(ms.SpoolID)
                         )
        );

        public static readonly Func<SamContext, int, IQueryable<CongeladoParcial>> CongParcialPorMat =
            CompiledQuery.Compile<SamContext, int, IQueryable<CongeladoParcial>>
        (
            (ctx, id) => from congpar in ctx.CongeladoParcial
                         join mat in ctx.MaterialSpool on congpar.MaterialSpoolID equals mat.MaterialSpoolID
                         join sp in ctx.Spool on mat.SpoolID equals sp.SpoolID
                         where sp.ProyectoID == id
                         select congpar
        );




        public static IQueryable<CongeladoParcial> CongeladoParcialSpoolEnRevisionSeleccionados(SamContext ctx, List<int> spoolsIds)
        {
            return from congelado in ctx.CongeladoParcial
                   join material in ctx.MaterialSpool on congelado.MaterialSpoolID equals material.MaterialSpoolID
                   join spool in ctx.Spool on material.SpoolID equals spool.SpoolID
                   where spoolsIds.Contains(spool.SpoolID)
                   select congelado;
        }

        public static readonly Func<SamContext, int, IQueryable<ItemCode>> ItemCodesPorProyecto =
        CompiledQuery.Compile<SamContext, int, IQueryable<ItemCode>>
        (
            (ctx, id) => from ic in ctx.ItemCode
                         where ic.ProyectoID == id
                         select ic
        );

        public static readonly Func<SamContext, int, IQueryable<ItemCodeEquivalente>> ItemCodeEquivalentesPorProyecto =
        CompiledQuery.Compile<SamContext, int, IQueryable<ItemCodeEquivalente>>
        (
            (ctx, id) => from ice in ctx.ItemCodeEquivalente
                         where ice.ItemCode.ProyectoID == id
                         select ice
        );



        public static readonly Func<SamContext, int, IQueryable<NumeroUnico>> NumerosUnicoParaCruceSinTracking =
        CompiledQuery.Compile<SamContext, int, IQueryable<NumeroUnico>>
        (
            (ctx, id) => from nu in ctx.NumeroUnico
                                       .Include("NumeroUnicoInventario")
                                       .Include("NumeroUnicoSegmento")
                                       .Include("Colada")
                         where nu.ProyectoID == id && nu.ItemCodeID != null                           
                         select nu
        );


        public static readonly Func<SamContext, int, IQueryable<NumeroUnico>> NumerosUnicoParaCruceSinTrackingRechazado =
        CompiledQuery.Compile<SamContext, int, IQueryable<NumeroUnico>>
        (
            (ctx, id) => from nu in ctx.NumeroUnico
                                       .Include("NumeroUnicoInventario")
                                       .Include("NumeroUnicoSegmento")
                                       .Include("Colada")
                         where nu.ProyectoID == id && nu.ItemCodeID != null
                            && nu.Estatus != null
                            && nu.Estatus.Equals(EstatusNumeroUnico.RECHAZADO)
                         select nu
        );

        public static readonly Func<SamContext, int, IQueryable<NumeroUnico>> NumerosUnicoParaCruceSinTrackingCondicionado =
        CompiledQuery.Compile<SamContext, int, IQueryable<NumeroUnico>>
        (
            (ctx, id) => from nu in ctx.NumeroUnico
                                       .Include("NumeroUnicoInventario")
                                       .Include("NumeroUnicoSegmento")
                                       .Include("Colada")
                         where nu.ProyectoID == id && nu.ItemCodeID != null
                            && nu.Estatus != null
                            && nu.Estatus.Equals(EstatusNumeroUnico.CONDICIONADO)
                         select nu
        );

        public static readonly Func<SamContext, int, IQueryable<NumeroUnico>> NumerosUnicoRechazadosSinTracking =
        CompiledQuery.Compile<SamContext, int, IQueryable<NumeroUnico>>
        (
            (ctx, id) => from nu in ctx.NumeroUnico
                                       .Include("NumeroUnicoInventario")
                         where nu.ProyectoID == id && nu.ItemCodeID != null
                         select nu
        );

        public static readonly Func<SamContext, int, IQueryable<NumeroUnico>> NumerosUnicosCongeladosParcialmente =
        CompiledQuery.Compile<SamContext, int, IQueryable<NumeroUnico>>
        (
            (ctx, id) => from nu in ctx.NumeroUnico
                         where nu.ProyectoID == id
                                && ctx.CongeladoParcial.Select(c => c.NumeroUnicoCongeladoID).Contains(nu.NumeroUnicoID)
                         select nu
        );

        public static readonly Func<SamContext, int, IQueryable<ResumenIsometrico>> AvanceIsometrico =
        CompiledQuery.Compile<SamContext, int, IQueryable<ResumenIsometrico>>
        (
            (ctx, id) => from sp in ctx.Spool
                         where sp.ProyectoID == id
                         group sp by sp.Dibujo
                             into dibujos
                             select new ResumenIsometrico
                             {
                                 Dibujo = dibujos.Key,
                                 TotalSpools = dibujos.Count(),
                                 SpoolsConODT = (from odts in ctx.OrdenTrabajoSpool
                                                 where odts.Spool.Dibujo == dibujos.Key
                                                 select odts.OrdenTrabajoSpoolID).Count(),
                                 SpoolsSinODT = 0
                             }
        );

        //public static readonly Func<SamContext, int, IQueryable<ResumenIsometrico>> AvnceIsometricoMarcadosComoRevision =
        //    CompiledQuery.Compile<SamContext, int, IQueryable<ResumenIsometrico>>
        //(
        //    (ctx, id) => from spool in ctx.Spool
        //                 where spool.ProyectoID == id && spool.EsRevision == true && (spool.UltimaOrdenTrabajoEspecial == null || spool.UltimaOrdenTrabajoEspecial == "")
        //                 group spool by spool.Dibujo
        //                     into dibujos
        //                     select new ResumenIsometrico
        //                     {
        //                         Dibujo = dibujos.Key,
        //                         TotalSpools = dibujos.Count(),
        //                         SpoolsConODT = (from odts in ctx.OrdenTrabajoEspecialSpool
        //                                         where odts.Spool.Dibujo == dibujos.Key
        //                                         select odts.OrdenTrabajoEspecialSpoolID).Count(),
        //                         SpoolsSinODT = 0
        //                     }
        //);

        public static IQueryable<ResumenIsometrico> AvnceIsometricoSpoolEnRevisionSeleccionados(SamContext ctx, List<int> spoolsIds)
        {
            return from spool in ctx.Spool
                   where spoolsIds.Contains(spool.SpoolID)
                   group spool by spool.Dibujo
                       into dibujos
                       select new ResumenIsometrico
                       {
                           Dibujo = dibujos.Key,
                           TotalSpools = dibujos.Count(),
                           SpoolsConODT = (from odts in ctx.OrdenTrabajoSpool
                                           where odts.Spool.Dibujo == dibujos.Key
                                           select odts.OrdenTrabajoSpoolID).Count(),
                           SpoolsSinODT = 0
                       };
        }


        public static readonly Func<SamContext, int, IQueryable<CruceItemCode>> InventariosCondensadosPorIC =
        CompiledQuery.Compile<SamContext, int, IQueryable<CruceItemCode>>
        (
            (ctx, id) => from nu in ctx.NumeroUnico
                         where nu.Colada.HoldCalidad == false
                               && nu.Estatus != null
                               && nu.Estatus.Equals(EstatusNumeroUnico.APROBADO)
                               && nu.ItemCodeID.HasValue
                               && nu.ProyectoID == id
                         group nu by new
                         {
                             CodigoIC = (int)nu.ItemCodeID,
                             D1 = nu.Diametro1,
                             D2 = nu.Diametro2
                         } into ic
                         select new CruceItemCode
                         {
                             ItemCodeID = ic.Key.CodigoIC,
                             Diametro1 = ic.Key.D1,
                             Diametro2 = ic.Key.D2,
                             InventarioFisico = ic.Sum(x => x.NumeroUnicoInventario.InventarioFisico),
                             InventarioBuenEstado = ic.Sum(x => x.NumeroUnicoInventario.InventarioBuenEstado),
                             InventarioCongelado = ic.Sum(x => x.NumeroUnicoInventario.InventarioCongelado),
                             InventarioDisponibleCruce = ic.Sum(x => x.NumeroUnicoInventario.InventarioDisponibleCruce)
                         }
        );

        #endregion
    }
}
