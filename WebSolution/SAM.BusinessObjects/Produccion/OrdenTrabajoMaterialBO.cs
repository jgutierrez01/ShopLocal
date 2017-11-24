using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.BusinessObjects.Excepciones;
using System.Data;
using SAM.BusinessObjects.Workstatus;
using System.Data.Objects;
using System.Transactions;

namespace SAM.BusinessObjects.Produccion
{
    public class OrdenTrabajoMaterialBO
    {
        private static readonly object _mutex = new object();
        private static OrdenTrabajoMaterialBO _instance;

        /// <summary>
        /// constructro privado para implementar patron singleton
        /// </summary>
        private OrdenTrabajoMaterialBO()
        {
        }

        /// <summary>
        /// obtiene la instancia de la clase OrdenTrabajoMaterialBO
        /// </summary>
        public static OrdenTrabajoMaterialBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new OrdenTrabajoMaterialBO();
                    }
                }
                return _instance;
            }
        }


        public OrdenTrabajoMaterial ObtenerInformacionParaDespacho(int ordenTrabajoMaterialID)
        {
            using (SamContext ctx = new SamContext())
            {
                OrdenTrabajoMaterial odtM = ctx.OrdenTrabajoMaterial
                                               .Where(x => x.OrdenTrabajoMaterialID == ordenTrabajoMaterialID)
                                               .Single();

                ctx.LoadProperty<OrdenTrabajoMaterial>(odtM, x => x.MaterialSpool);
                ctx.LoadProperty<MaterialSpool>(odtM.MaterialSpool, x => x.ItemCode);

                return odtM;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="odtM"></param>
        /// <param name="cantidad"></param>
        /// <param name="userID"></param>
        public void DespachaTubo(OrdenTrabajoMaterial odtM, int cantidad, Guid userID, string etiquetaParaPlanchar, int despachadorID = 0)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 1, 30)))
            {
                try
                {
                    using (SamContext ctx = new SamContext())
                    {
                        if (RevisionHoldsBO.Instance.MaterialSpoolTieneHold(ctx, odtM.MaterialSpoolID))
                        {
                            throw new ExcepcionEnHold(MensajesError.Excepcion_SpoolEnHold);
                        }

                        int proyectoID = odtM.MaterialSpool.ItemCode.ProyectoID;

                        #region Traer la información de corte

                        CorteDetalle cd = ctx.CorteDetalle
                                         .Where(x => x.CorteDetalleID == odtM.CorteDetalleID).Single();

                        ctx.LoadProperty<CorteDetalle>(cd, x => x.Corte);
                        ctx.LoadProperty<Corte>(cd.Corte, x => x.NumeroUnicoCorte);
                        ctx.LoadProperty<NumeroUnicoCorte>(cd.Corte.NumeroUnicoCorte, x => x.NumeroUnico);

                        #endregion

                        if (odtM.NumeroUnicoCongeladoID.HasValue)
                        {

                            //Numero único congelado por cruce
                            NumeroUnicoInventario nuiCongelado = ctx.NumeroUnicoInventario
                                                                    .Where(x => x.NumeroUnicoID == odtM.NumeroUnicoCongeladoID)
                                                                    .Single();

                            nuiCongelado.StartTracking();
                            nuiCongelado.UsuarioModifica = userID;
                            nuiCongelado.FechaModificacion = DateTime.Now;
                            nuiCongelado.InventarioCongelado = nuiCongelado.InventarioCongelado - odtM.CantidadCongelada.Value;
                            nuiCongelado.InventarioDisponibleCruce = nuiCongelado.InventarioBuenEstado - nuiCongelado.InventarioCongelado;
                            nuiCongelado.StopTracking();

                            ctx.NumeroUnicoInventario.ApplyChanges(nuiCongelado);

                            NumeroUnicoSegmento nusCongelado = ctx.NumeroUnicoSegmento
                                                                  .Where(x => x.NumeroUnicoID == odtM.NumeroUnicoCongeladoID)
                                                                  .Where(x => x.Segmento == odtM.SegmentoCongelado)
                                                                  .Single();

                            nusCongelado.StartTracking();
                            nusCongelado.UsuarioModifica = userID;
                            nusCongelado.FechaModificacion = DateTime.Now;
                            nusCongelado.InventarioCongelado = nusCongelado.InventarioCongelado - odtM.CantidadCongelada.Value;
                            nusCongelado.InventarioDisponibleCruce = nusCongelado.InventarioBuenEstado - nusCongelado.InventarioCongelado;
                            nusCongelado.StopTracking();

                            ctx.NumeroUnicoSegmento.ApplyChanges(nusCongelado);
                        }

                        NumeroUnico nuDespacho = cd.Corte.NumeroUnicoCorte.NumeroUnico;

                        bool esEquivalente = nuDespacho.ItemCodeID != odtM.MaterialSpool.ItemCodeID
                                             || nuDespacho.Diametro1 != odtM.MaterialSpool.Diametro1
                                             || nuDespacho.Diametro2 != odtM.MaterialSpool.Diametro2;

                       

                        //Generar el despacho
                        Despacho despacho = new Despacho
                        {
                            Cancelado = false,
                            Cantidad = cantidad,
                            EsEquivalente = esEquivalente,
                            FechaModificacion = DateTime.Now,
                            MaterialSpoolID = odtM.MaterialSpoolID,
                            NumeroUnicoID = nuDespacho.NumeroUnicoID,
                            OrdenTrabajoSpoolID = odtM.OrdenTrabajoSpoolID,
                            ProyectoID = proyectoID,
                            FechaDespacho = DateTime.Now,
                            Segmento = cd.Corte.NumeroUnicoCorte.Segmento,
                            UsuarioModifica = userID,
                            DespachadorID = despachadorID <= 0 ? 0 : despachadorID
                        };

                        //Meter el despacho al grafo
                        ctx.Despacho.ApplyChanges(despacho);

                        //Actualizar todos los campos de la ODT material con lo que se generó arriba
                        odtM.StartTracking();
                        odtM.UsuarioModifica = userID;
                        odtM.FechaModificacion = DateTime.Now;
                        odtM.Despacho = despacho;
                        odtM.TieneInventarioCongelado = false;
                        odtM.NumeroUnicoCongeladoID = null;
                        odtM.SegmentoCongelado = null;
                        odtM.CantidadCongelada = null;
                        odtM.CongeladoEsEquivalente = false;
                        odtM.NumeroUnicoSugeridoID = null;
                        odtM.SugeridoEsEquivalente = false;
                        odtM.TieneDespacho = true;
                        odtM.DespachoEsEquivalente = esEquivalente;
                        odtM.NumeroUnicoDespachadoID = nuDespacho.NumeroUnicoID;
                        odtM.SegmentoDespachado = cd.Corte.NumeroUnicoCorte.Segmento;
                        odtM.CantidadDespachada = cantidad;
                        
                        odtM.StopTracking();
                                               
                        //aplicar los cambios al grafo
                        ctx.OrdenTrabajoMaterial.ApplyChanges(odtM);

                        #region Transferencia Congelados
                        //Verificamos que no haya inventario disponible negativo, de ser asi transferimos congelados
                        NumeroUnicoSegmento nuInv = ctx.NumeroUnicoSegmento.Include("NumeroUnico.NumeroUnicoInventario").Where(x => x.NumeroUnicoID == nuDespacho.NumeroUnicoID && x.Segmento == cd.Corte.NumeroUnicoCorte.Segmento).SingleOrDefault();

                        if (nuInv.InventarioDisponibleCruce < 0)
                        {
                            transfiereCongeladosTubo(ctx, nuInv, userID);
                        }

                        #endregion

                        //Guardar la información
                        ctx.SaveChanges();

                        verificaSiExisteArmadoPendienteYPlancha(odtM.MaterialSpoolID, nuDespacho.NumeroUnicoID, odtM.OrdenTrabajoSpoolID, ctx, etiquetaParaPlanchar);

                    }
                }
                catch (OptimisticConcurrencyException)
                {
                    throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
                }

                scope.Complete();
            }
        }


        public void verificaSiExisteArmadoPendienteYPlancha(int materialSpoolID, int numerounicoID, int ordenTrabajoSpoolID, SamContext ctx, string etiquetaAPlanchar = "")
        {
            MaterialSpool material = ctx.MaterialSpool.Where(x => x.MaterialSpoolID == materialSpoolID).FirstOrDefault();
            int spoolID = material.SpoolID;
            int numTemp = 0;
            List<JuntaSpool> js = null;

            //obtengo las juntas del spool
            if (int.TryParse(material.Etiqueta, out numTemp))
            {
                string tempParse = numTemp.ToString();
                js = ctx.JuntaSpool.Include("JuntaWorkstatus")
                    .Where(x => x.SpoolID == material.SpoolID
                    && (
                        (x.EtiquetaMaterial1 == material.Etiqueta || x.EtiquetaMaterial1 == tempParse)
                        || (x.EtiquetaMaterial2 == material.Etiqueta || x.EtiquetaMaterial2 == tempParse)
                       )
                    ).ToList();

            }

            if (js != null && js.Count == 0)
            {
                string tempParse = "0" + numTemp.ToString();
                js = ctx.JuntaSpool
                    .Include("JuntaWorkstatus")
                    .Where(x => x.SpoolID == material.SpoolID
                    && (
                        (x.EtiquetaMaterial1 == material.Etiqueta || x.EtiquetaMaterial1 == tempParse)
                        || (x.EtiquetaMaterial2 == material.Etiqueta || x.EtiquetaMaterial2 == tempParse)
                       )
                    ).ToList();
            }

            JuntaWorkstatus jw = null;
            //Despacho despacho = ctx.Despacho.Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID && x.MaterialSpoolID == materialSpoolID).SingleOrDefault();

            string[] etiquetas = etiquetaAPlanchar.Split('/');

            foreach (JuntaSpool junta in js)
            {
                try
                {
                    jw = ctx.JuntaWorkstatus.Include("JuntaArmado").Where(x => x.JuntaSpoolID == junta.JuntaSpoolID && x.JuntaFinal).FirstOrDefault();
                    //jw.JuntaArmado1.StartTracking();
                    if (jw != null && jw.ArmadoAprobado) //verifica si ya se tiene armado 
                    {
                        jw.JuntaArmado1.StartTracking();

                        if (etiquetas.Contains(junta.EtiquetaMaterial1) && jw.JuntaArmado1.NumeroUnico1ID != numerounicoID)
                        {
                            jw.JuntaArmado1.NumeroUnico1ID = numerounicoID;
                        }
                        if (etiquetas.Contains(junta.EtiquetaMaterial2) && jw.JuntaArmado1.NumeroUnico2ID != numerounicoID)
                        {
                            jw.JuntaArmado1.NumeroUnico2ID = numerounicoID;
                        }

                        jw.JuntaArmado1.StopTracking();
                        ctx.JuntaArmado.ApplyChanges(jw.JuntaArmado1);
                        ctx.SaveChanges();
                    }
                }
                catch
                {
                }
            }
        }

        private void transfiereCongeladosTubo(SamContext ctx, NumeroUnicoSegmento segmento, Guid userID)
        {
            List<NumeroUnicoSegmento> candidatos = ctx.NumeroUnicoSegmento.Include("NumeroUnico.NumeroUnicoInventario").Where(x => x.NumeroUnico.ItemCodeID == segmento.NumeroUnico.ItemCodeID && x.NumeroUnico.Diametro1 == segmento.NumeroUnico.Diametro1 && x.NumeroUnico.Diametro2 == segmento.NumeroUnico.Diametro2 && x.InventarioDisponibleCruce >= segmento.InventarioCongelado && x.NumeroUnico.Estatus == "A").ToList();

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
                candidatos[0].NumeroUnico.NumeroUnicoInventario.InventarioCongelado = candidatos[0].NumeroUnico.NumeroUnicoInventario.InventarioCongelado + segmento.InventarioCongelado;
                candidatos[0].NumeroUnico.NumeroUnicoInventario.InventarioDisponibleCruce = candidatos[0].NumeroUnico.NumeroUnicoInventario.InventarioBuenEstado - candidatos[0].NumeroUnico.NumeroUnicoInventario.InventarioCongelado;
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
                segmento.InventarioCongelado = 0;
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
                        candidato[0].NumeroUnico.NumeroUnicoInventario.InventarioCongelado = candidato[0].NumeroUnico.NumeroUnicoInventario.InventarioCongelado + otm.CantidadCongelada.Value;
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

        private void transfiereCongeladosAccesorios(SamContext ctx, NumeroUnico numUnico, Guid userID)
        {
            List<NumeroUnico> candidatos = ctx.NumeroUnico.Include("NumeroUnicoInventario").Where(x => x.ItemCodeID == numUnico.ItemCodeID && x.Diametro1 == numUnico.Diametro1 && x.Diametro2 == numUnico.Diametro2 && x.NumeroUnicoInventario.InventarioDisponibleCruce >= numUnico.NumeroUnicoInventario.InventarioCongelado && x.Estatus == "A").ToList();

            if (candidatos.Count > 0)
            {
                candidatos = candidatos.OrderBy(x => x.NumeroUnicoInventario.InventarioDisponibleCruce).ToList();

                candidatos[0].NumeroUnicoInventario.StartTracking();
                candidatos[0].NumeroUnicoInventario.UsuarioModifica = userID;
                candidatos[0].NumeroUnicoInventario.FechaModificacion = DateTime.Now;
                candidatos[0].NumeroUnicoInventario.InventarioCongelado = candidatos[0].NumeroUnicoInventario.InventarioCongelado + numUnico.NumeroUnicoInventario.InventarioCongelado;
                candidatos[0].NumeroUnicoInventario.InventarioDisponibleCruce = candidatos[0].NumeroUnicoInventario.InventarioBuenEstado - candidatos[0].NumeroUnicoInventario.InventarioCongelado;
                candidatos[0].NumeroUnicoInventario.StopTracking();

                ctx.NumeroUnicoInventario.ApplyChanges(candidatos[0].NumeroUnicoInventario);

                //actualizamos odt y congelados parciales con nuevo numero unico congelado

                if (ctx.CongeladoParcial.Where(x => x.NumeroUnicoCongeladoID == numUnico.NumeroUnicoID).Any())
                {
                    List<CongeladoParcial> congPar = ctx.CongeladoParcial.Where(x => x.NumeroUnicoCongeladoID == numUnico.NumeroUnicoID).ToList();
                    congPar.ForEach(x =>
                    {
                        x.StartTracking();
                        x.NumeroUnicoCongeladoID = candidatos[0].NumeroUnicoID;
                        x.UsuarioModifica = userID;
                        x.FechaModificacion = DateTime.Now;
                        x.StopTracking();
                        ctx.CongeladoParcial.ApplyChanges(x);
                    });
                }
                if (ctx.OrdenTrabajoMaterial.Where(x => x.NumeroUnicoCongeladoID == numUnico.NumeroUnicoID).Any())
                {
                    List<OrdenTrabajoMaterial> _otm = ctx.OrdenTrabajoMaterial.Where(x => x.NumeroUnicoCongeladoID == numUnico.NumeroUnicoID).ToList();
                    _otm.ForEach(x =>
                    {
                        x.StartTracking();
                        x.NumeroUnicoCongeladoID = candidatos[0].NumeroUnicoID;
                        x.UsuarioModifica = userID;
                        x.FechaModificacion = DateTime.Now;
                        x.StopTracking();
                        ctx.OrdenTrabajoMaterial.ApplyChanges(x);
                    });
                }

                //liberamos el congelado del numero unico despachado
                numUnico.NumeroUnicoInventario.StartTracking();
                numUnico.NumeroUnicoInventario.UsuarioModifica = userID;
                numUnico.NumeroUnicoInventario.FechaModificacion = DateTime.Now;
                numUnico.NumeroUnicoInventario.InventarioCongelado = 0;
                numUnico.NumeroUnicoInventario.InventarioDisponibleCruce = numUnico.NumeroUnicoInventario.InventarioBuenEstado - numUnico.NumeroUnicoInventario.InventarioCongelado;
                numUnico.NumeroUnicoInventario.StopTracking();

                ctx.NumeroUnicoInventario.ApplyChanges(numUnico.NumeroUnicoInventario);

            }
            else // en este caso el numero unico tiene congelado un numero mayor del disponible mas grande...hay que transferir de material por material para lograr el cometido
            {
                foreach (OrdenTrabajoMaterial otm in ctx.OrdenTrabajoMaterial.Where(x => x.NumeroUnicoCongeladoID == numUnico.NumeroUnicoID).OrderByDescending(x => x.CantidadCongelada))
                {
                    List<NumeroUnico> candidato = ctx.NumeroUnico.Include("NumeroUnicoInventario").Where(x => x.ItemCodeID == numUnico.ItemCodeID && x.Diametro1 == numUnico.Diametro1 && x.Diametro2 == numUnico.Diametro2 && x.NumeroUnicoInventario.InventarioDisponibleCruce >= otm.CantidadCongelada && x.Estatus == "A").ToList();

                    if (candidato.Count > 0)
                    {
                        candidato = candidato.OrderBy(x => x.NumeroUnicoInventario.InventarioDisponibleCruce).ToList();

                        candidato[0].NumeroUnicoInventario.StartTracking();
                        candidato[0].NumeroUnicoInventario.UsuarioModifica = userID;
                        candidato[0].NumeroUnicoInventario.FechaModificacion = DateTime.Now;
                        candidato[0].NumeroUnicoInventario.InventarioCongelado = candidato[0].NumeroUnicoInventario.InventarioCongelado + otm.CantidadCongelada.Value;
                        candidato[0].NumeroUnicoInventario.InventarioDisponibleCruce = candidato[0].NumeroUnicoInventario.InventarioBuenEstado - candidato[0].NumeroUnicoInventario.InventarioCongelado;
                        candidato[0].StopTracking();

                        ctx.NumeroUnicoInventario.ApplyChanges(candidato[0].NumeroUnicoInventario);
                                                
                        otm.StartTracking();
                        otm.NumeroUnicoCongeladoID = candidato[0].NumeroUnicoID;
                        otm.UsuarioModifica = userID;
                        otm.FechaModificacion = DateTime.Now;
                        otm.StopTracking();
                        ctx.OrdenTrabajoMaterial.ApplyChanges(otm);

                        numUnico.NumeroUnicoInventario.StartTracking();
                        numUnico.NumeroUnicoInventario.UsuarioModifica = userID;
                        numUnico.NumeroUnicoInventario.FechaModificacion = DateTime.Now;
                        numUnico.NumeroUnicoInventario.InventarioCongelado = numUnico.NumeroUnicoInventario.InventarioCongelado - otm.CantidadCongelada.Value;
                        numUnico.NumeroUnicoInventario.InventarioDisponibleCruce = numUnico.NumeroUnicoInventario.InventarioBuenEstado - numUnico.NumeroUnicoInventario.InventarioCongelado;
                        numUnico.NumeroUnicoInventario.StopTracking();

                        ctx.NumeroUnicoInventario.ApplyChanges(numUnico.NumeroUnicoInventario);

                        if (numUnico.NumeroUnicoInventario.InventarioDisponibleCruce >= 0) //si el inventario ya es positivo se termina
                        {
                            break;
                        }
                    }
                }

                if (numUnico.NumeroUnicoInventario.InventarioDisponibleCruce <= 0) //si con la liberacion de odt no es suficiente seguimos con congelados parciales
                {
                    foreach (CongeladoParcial cp in ctx.CongeladoParcial.Include("MaterialSpool").Where(x => x.NumeroUnicoCongeladoID == numUnico.NumeroUnicoID).OrderByDescending(x => x.MaterialSpool.Cantidad))
                    {
                        List<NumeroUnico> candidato = ctx.NumeroUnico.Include("NumeroUnicoInventario").Where(x => x.ItemCodeID == numUnico.ItemCodeID && x.Diametro1 == numUnico.Diametro1 && x.Diametro2 == numUnico.Diametro2 && x.NumeroUnicoInventario.InventarioDisponibleCruce >= cp.MaterialSpool.Cantidad && x.Estatus == "A").ToList();

                        if (candidato.Count > 0)
                        {
                            candidato = candidato.OrderBy(x => x.NumeroUnicoInventario.InventarioDisponibleCruce).ToList();

                            candidato[0].NumeroUnicoInventario.StartTracking();
                            candidato[0].NumeroUnicoInventario.UsuarioModifica = userID;
                            candidato[0].NumeroUnicoInventario.FechaModificacion = DateTime.Now;
                            candidato[0].NumeroUnicoInventario.InventarioCongelado = candidato[0].NumeroUnicoInventario.InventarioCongelado + cp.MaterialSpool.Cantidad;
                            candidato[0].NumeroUnicoInventario.InventarioDisponibleCruce = candidato[0].NumeroUnicoInventario.InventarioBuenEstado - candidato[0].NumeroUnicoInventario.InventarioCongelado;
                            candidato[0].NumeroUnicoInventario.StopTracking();

                            ctx.NumeroUnicoInventario.ApplyChanges(candidato[0].NumeroUnicoInventario);

                            cp.StartTracking();
                            cp.NumeroUnicoCongeladoID = candidato[0].NumeroUnicoID;
                            cp.UsuarioModifica = userID;
                            cp.FechaModificacion = DateTime.Now;
                            cp.StopTracking();
                            ctx.CongeladoParcial.ApplyChanges(cp);

                            numUnico.NumeroUnicoInventario.StartTracking();
                            numUnico.NumeroUnicoInventario.UsuarioModifica = userID;
                            numUnico.NumeroUnicoInventario.FechaModificacion = DateTime.Now;
                            numUnico.NumeroUnicoInventario.InventarioCongelado = numUnico.NumeroUnicoInventario.InventarioCongelado - cp.MaterialSpool.Cantidad;
                            numUnico.NumeroUnicoInventario.InventarioDisponibleCruce = numUnico.NumeroUnicoInventario.InventarioBuenEstado - numUnico.NumeroUnicoInventario.InventarioCongelado;
                            numUnico.NumeroUnicoInventario.StopTracking();

                            ctx.NumeroUnicoInventario.ApplyChanges(numUnico.NumeroUnicoInventario);

                            if (numUnico.NumeroUnicoInventario.InventarioDisponibleCruce >= 0) //si el inventario ya es positivo se termina
                            {
                                break;
                            }
                        }
                    }
                }


            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="odtM"></param>
        /// <param name="cantidad"></param>
        /// <param name="numeroUnicoID"></param>
        /// <param name="userID"></param>
        public void DespachaAccesorio(OrdenTrabajoMaterial odtM, int cantidad, int numeroUnicoID, Guid userID, string etiquetaMaterialPlanchar, int despachadorID = 0)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 1, 30)))
            {
                try
                {
                    using (SamContext ctx = new SamContext())
                    {
                        if (RevisionHoldsBO.Instance.MaterialSpoolTieneHold(ctx, odtM.MaterialSpoolID))
                        {
                            throw new ExcepcionEnHold(MensajesError.Excepcion_SpoolEnHold);
                        }

                        int proyectoID = odtM.MaterialSpool.ItemCode.ProyectoID;

                        //Traer el número único que se va a despachar
                        NumeroUnico nuDespacho = ctx.NumeroUnico
                                                    .Where(x => x.NumeroUnicoID == numeroUnicoID)
                                                    .Single();

                        //Determinar si el despacho se está haciendo con una equivalencia
                        bool esEquivalente = odtM.MaterialSpool.ItemCodeID != nuDespacho.ItemCodeID
                                             || odtM.MaterialSpool.Diametro1 != nuDespacho.Diametro1
                                             || odtM.MaterialSpool.Diametro2 != nuDespacho.Diametro2;

                        //Generar el movimiento de salida de tipo despacho accesorio
                        NumeroUnicoMovimiento movimiento = new NumeroUnicoMovimiento
                        {
                            Cantidad = cantidad,
                            Estatus = EstatusNumeroUnicoMovimiento.ACTIVO,
                            FechaModificacion = DateTime.Now,
                            FechaMovimiento = DateTime.Now,
                            NumeroUnicoID = numeroUnicoID,
                            ProyectoID = proyectoID,
                            Segmento = string.Empty,
                            Referencia = ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajoSpoolID == odtM.OrdenTrabajoSpoolID).Select(y => y.NumeroControl).FirstOrDefault(),
                            TipoMovimientoID = (int)TipoMovimientoEnum.DespachoAccesorio,
                            UsuarioModifica = userID
                        };

                        //meter el objeto al grafo
                        ctx.NumeroUnicoMovimiento.ApplyChanges(movimiento);

                        //Obtener el inventario actual del N.U. seleccionado y actualizarlo correspondientemente
                        NumeroUnicoInventario nui = ctx.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == numeroUnicoID).Single();
                        nui.StartTracking();
                        nui.UsuarioModifica = userID;
                        nui.FechaModificacion = DateTime.Now;
                        nui.InventarioFisico = nui.InventarioFisico - cantidad;
                        nui.InventarioBuenEstado = nui.InventarioBuenEstado - cantidad;
                        nui.InventarioDisponibleCruce = nui.InventarioBuenEstado - nui.InventarioCongelado;
                        nui.StopTracking();

                        //Aplicar los cambios al grafo
                        ctx.NumeroUnicoInventario.ApplyChanges(nui);

                        if (odtM.NumeroUnicoCongeladoID.HasValue)
                        {
                            //Obtener el inventario actual del N.U. congelado originalmente por cruce y descongelarlo
                            NumeroUnicoInventario congelado = ctx.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == odtM.NumeroUnicoCongeladoID).Single();

                            congelado.StartTracking();
                            congelado.UsuarioModifica = userID;
                            congelado.FechaModificacion = DateTime.Now;
                            congelado.InventarioCongelado = congelado.InventarioCongelado - (odtM.CantidadCongelada.HasValue ? odtM.CantidadCongelada.Value : 0);
                            congelado.InventarioDisponibleCruce = congelado.InventarioBuenEstado - congelado.InventarioCongelado;
                            congelado.StopTracking();

                            ctx.NumeroUnicoInventario.ApplyChanges(congelado);
                        }

                        //Generar el despacho
                        Despacho despacho = new Despacho
                        {
                            Cancelado = false,
                            Cantidad = cantidad,
                            EsEquivalente = esEquivalente,
                            FechaModificacion = DateTime.Now,
                            MaterialSpoolID = odtM.MaterialSpoolID,
                            NumeroUnicoID = numeroUnicoID,
                            MovimientoSalida = movimiento,
                            OrdenTrabajoSpoolID = odtM.OrdenTrabajoSpoolID,
                            ProyectoID = proyectoID,
                            FechaDespacho = DateTime.Now,
                            Segmento = null,
                            UsuarioModifica = userID,
                            DespachadorID = despachadorID <= 0 ? 0 : despachadorID
                        };

                        //Meter el despacho al grafo
                        ctx.Despacho.ApplyChanges(despacho);

                        //Actualizar todos los campos de la ODT material con lo que se generó arriba
                        odtM.StartTracking();
                        odtM.UsuarioModifica = userID;
                        odtM.FechaModificacion = DateTime.Now;
                        odtM.Despacho = despacho;
                        odtM.TieneInventarioCongelado = false;
                        odtM.NumeroUnicoCongeladoID = null;
                        odtM.SegmentoCongelado = null;
                        odtM.CantidadCongelada = null;
                        odtM.CongeladoEsEquivalente = false;
                        odtM.NumeroUnicoSugeridoID = null;
                        odtM.SugeridoEsEquivalente = false;
                        odtM.TieneDespacho = true;
                        odtM.DespachoEsEquivalente = esEquivalente;
                        odtM.NumeroUnicoDespachadoID = numeroUnicoID;
                        odtM.SegmentoDespachado = null;
                        odtM.CantidadDespachada = cantidad;
                        odtM.StopTracking();

                        //aplicar los cambios al grafo
                        ctx.OrdenTrabajoMaterial.ApplyChanges(odtM);

                        #region Transferencia Congelados
                        //Verificamos que no haya inventario disponible negativo, de ser asi transferimos congelados
                        NumeroUnico nuInv = ctx.NumeroUnico.Include("NumeroUnicoInventario").Where(x => x.NumeroUnicoID == nuDespacho.NumeroUnicoID).SingleOrDefault();

                        if (nuInv.NumeroUnicoInventario.InventarioDisponibleCruce < 0)
                        {
                            transfiereCongeladosAccesorios(ctx, nuInv, userID);
                        }

                        #endregion

                        //Guardar la información
                        ctx.SaveChanges();

                        verificaSiExisteArmadoPendienteYPlancha(odtM.MaterialSpoolID, nuDespacho.NumeroUnicoID, odtM.OrdenTrabajoSpoolID, ctx, etiquetaMaterialPlanchar);

                    }
                }
                catch (OptimisticConcurrencyException)
                {
                    throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
                }

                scope.Complete();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordenTrabajoMaterialID"></param>
        /// <returns></returns>
        public int ObtenerProyectoID(int ordenTrabajoMaterialID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ProyectoOrdenTrabajoMaterial(ctx, ordenTrabajoMaterialID);
            }
        }


        /// <summary>
        /// Versión compilada del query para permisos de orden trabajo material
        /// </summary>
        private static readonly Func<SamContext, int, int> ProyectoOrdenTrabajoMaterial =
        CompiledQuery.Compile<SamContext, int, int>
        (
            (ctx, id) => ctx.OrdenTrabajo
                            .Where(odt => ctx.OrdenTrabajoSpool
                                             .Where(odts => ctx.OrdenTrabajoMaterial
                                                               .Where(odtm => odtm.OrdenTrabajoMaterialID == id)
                                                               .Select(odtm => odtm.OrdenTrabajoSpoolID)
                                                               .Contains(odts.OrdenTrabajoSpoolID))
                                             .Select(odts => odts.OrdenTrabajoID)
                                             .Contains(odt.OrdenTrabajoID))
                            .Select(odt => odt.ProyectoID)
                            .Single()
        );
    }
}
