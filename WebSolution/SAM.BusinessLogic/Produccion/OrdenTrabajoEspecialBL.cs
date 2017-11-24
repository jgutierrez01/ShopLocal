using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.Entities.Personalizadas;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Utilerias;
using System.Data.Objects;
using SAM.BusinessLogic.Cruce;
using SAM.BusinessLogic.Excepciones;
using System.Data;
using SAM.BusinessObjects.Excepciones;

namespace SAM.BusinessLogic.Produccion
{
    public class OrdenTrabajoEspecialBL
    {
        //private static readonly object _mutex = new object();
        //private static OrdenTrabajoEspecialBL _instance;
        //private const string prefijoEspecial = "R";


        ///// <summary>
        ///// obtiene la instancia de la clase OrdenTrabajoBL
        ///// </summary>
        //public static OrdenTrabajoEspecialBL Instance
        //{
        //    get
        //    {
        //        lock (_mutex)
        //        {
        //            if (_instance == null)
        //            {
        //                _instance = new OrdenTrabajoEspecialBL();
        //            }
        //        }
        //        return _instance;
        //    }
        //}

        //public OrdenTrabajoEspecial GeneraNueva(int proyectoID, List<int> spoolIds, int tallerID, Guid userID, bool conAsignacion)
        //{
        //    try
        //    {
        //        //Sólo debemos utilizar las juntas tipo SHOP
        //        int shopFabAreaID = CacheCatalogos.Instance.ShopFabAreaID;

        //        using (SamContext ctx = new SamContext())
        //        {
        //            ctx.NumeroUnico.MergeOption = MergeOption.NoTracking;
        //            ctx.NumeroUnicoSegmento.MergeOption = MergeOption.NoTracking;
        //            ctx.NumeroUnicoInventario.MergeOption = MergeOption.NoTracking;
        //            ctx.ItemCode.MergeOption = MergeOption.NoTracking;
        //            ctx.ItemCodeEquivalente.MergeOption = MergeOption.NoTracking;
        //            ctx.Spool.MergeOption = MergeOption.NoTracking;
        //            ctx.MaterialSpool.MergeOption = MergeOption.NoTracking;
        //            ctx.JuntaSpool.MergeOption = MergeOption.NoTracking;

        //            string numOrden = string.Empty;
                    
        //            OrdenTrabajoEspecial odtes = new OrdenTrabajoEspecial();

        //            ProyectoConfiguracion configProyecto = (from configuracion in ctx.ProyectoConfiguracion
        //                                                    where configuracion.ProyectoID == proyectoID
        //                                                    select configuracion).Single();

        //            ProyectoConsecutivo consecutivos = ctx.ProyectoConsecutivo.Where(x => x.ProyectoID == proyectoID).Single();

        //            string prefijo = configProyecto.PrefijoOrdenTrabajo;

        //            int numeroOrden = consecutivos.ConsecutivoODTEspecial.SafeIntParse();
        //            string formato = "";

        //            for (int i = 0; i < configProyecto.DigitosOrdenTrabajo; i++)
        //            {
        //                formato = formato + "0";
        //            }

        //            odtes.NumeroOrden = prefijoEspecial + "-" + prefijo + (numeroOrden + 1).ToString(formato);
        //            odtes.EstatusOrdenID = (int)EstatusOrdenDeTrabajo.Activa;
        //            odtes.ProyectoID = proyectoID;
        //            odtes.FechaModificacion = DateTime.Now;
        //            odtes.FechaOrden = DateTime.Now;
        //            odtes.UsuarioModifica = userID;
        //            odtes.TallerID = tallerID;
        //            ctx.OrdenTrabajoEspecial.ApplyChanges(odtes);

        //            consecutivos.StartTracking();
        //            consecutivos.ConsecutivoODTEspecial = consecutivos.ConsecutivoODTEspecial.SafeIntParse() + 1;
        //            consecutivos.FechaModificacion = DateTime.Now;
        //            consecutivos.UsuarioModifica = userID;
        //            consecutivos.StopTracking();
        //            ctx.ProyectoConsecutivo.ApplyChanges(consecutivos);

        //            #region Efectuar el cruce para la Odt en caso que haya que agregar spools

        //            if (spoolIds != null && spoolIds.Count > 0)
        //            {
        //                int[] spools = new int[spoolIds.Count];
        //                int i = 0;
        //                foreach (int sp in spoolIds)
        //                {
        //                    spools[i] = sp;
        //                    i++;
        //                }

        //                CruceSpool cruce = new CruceSpool(ctx, proyectoID, spools, userID);
        //                List<NumeroUnico> lstNumUnico;

        //                //Efectuar el cruce para estos spools en particular, en caso que no se pueda esto arroja una excepción
        //                List<Spool> lstSpool = cruce.ProcesaParaRevision(out lstNumUnico);

        //                //Revisar que se hayan podido cruzar los spools
        //                if (lstSpool.Any(x => !x.InfoCruce.CruceExitoso))
        //                {
        //                    List<string> errores = lstSpool.Where(x => !x.InfoCruce.CruceExitoso)
        //                                                   .Select(y => string.Format(MensajesError.MaterialInsuficiente_ParaSpool, y.Nombre))
        //                                                   .ToList();

        //                        throw new ExcepcionMaterialInsuficiente(errores);
                            
        //                }

        //                int consecutivoOrden = 0;
        //                int partida = 0;                    

        //                foreach (Spool spool in lstSpool)
        //                {
        //                    consecutivoOrden++;
        //                    partida++;                            

        //                    spool.StartTracking();
        //                    spool.UltimaOrdenTrabajoEspecial = odtes.NumeroOrden + "-" + consecutivoOrden.ToString(formato); 
        //                    spool.FechaModificacion = DateTime.Now;
        //                    spool.UsuarioModifica = userID;
        //                    spool.StopTracking();
        //                    ctx.Spool.ApplyChanges(spool);
                            
        //                    //Creamos la orden de trabajo

        //                    #region Generar un nuevo objeto en la ODT para cada spool
        //                    OrdenTrabajoEspecialSpool odtesSpool = new OrdenTrabajoEspecialSpool
        //                    {
        //                        FechaModificacion = DateTime.Now,
        //                        NumeroControl = odtes.NumeroOrden + "-" + consecutivoOrden.ToString(formato),
        //                        Partida = partida,
        //                        SpoolID = spool.SpoolID, 
        //                        EsAsignado = conAsignacion,
        //                        UsuarioModifica = userID
        //                    };
        //                    #endregion

        //                    #region Iterar las juntas para crearlas en la ODT

        //                    List<JuntaSpool> jsSoloShop = spool.JuntaSpool.Where(x => x.FabAreaID == shopFabAreaID).ToList();
        //                    OrdenTrabajoSpool odts = ctx.OrdenTrabajoSpool.Where(x => x.SpoolID == spool.SpoolID).FirstOrDefault();
        //                    List<int> odtJunta = ctx.OrdenTrabajoJunta.Where(x => x.OrdenTrabajoSpoolID == odts.OrdenTrabajoSpoolID).Select(x => x.JuntaSpoolID).ToList();

        //                    foreach (JuntaSpool js in jsSoloShop)
        //                    {
        //                        if (!odtJunta.Contains(js.JuntaSpoolID))
        //                        {
        //                            OrdenTrabajoJunta odtj = new OrdenTrabajoJunta
        //                            {
        //                                FechaModificacion = DateTime.Now,
        //                                JuntaSpoolID = js.JuntaSpoolID,
        //                                FueReingenieria = true,
        //                                UsuarioModifica = userID
        //                            };

        //                            odts.OrdenTrabajoJunta.Add(odtj);
        //                        }
        //                    }
        //                    #endregion

        //                    #region Iterar los materiales para crearlos en la ODT

        //                    List<int> odtMat = ctx.OrdenTrabajoMaterial.Where(x => x.OrdenTrabajoSpoolID == odts.OrdenTrabajoSpoolID).Select(x => x.MaterialSpoolID).ToList();

        //                    foreach (MaterialSpool ms in spool.MaterialSpool)
        //                    {
        //                        if (!odtMat.Contains(ms.MaterialSpoolID))
        //                        {
        //                            OrdenTrabajoMaterial odtm = new OrdenTrabajoMaterial
        //                            {
        //                                CantidadCongelada = ms.Cantidad,
        //                                CantidadDespachada = 0,
        //                                CongeladoEsEquivalente = ms.InfoCruce.EsEquivalente,
        //                                DespachoEsEquivalente = false,
        //                                NumeroUnicoCongeladoID = ms.InfoCruce.NumeroUnicoID,
        //                                NumeroUnicoSugeridoID = ms.InfoCruce.EsSugerido ? (int?)ms.InfoCruce.NumeroUnicoID : null,
        //                                MaterialSpoolID = ms.MaterialSpoolID,
        //                                SegmentoCongelado = string.IsNullOrEmpty(ms.InfoCruce.Segmento) ? null : ms.InfoCruce.Segmento,
        //                                SegmentoSugerido = ms.InfoCruce.EsSugerido && !string.IsNullOrEmpty(ms.InfoCruce.Segmento) ? ms.InfoCruce.Segmento : null,
        //                                SugeridoEsEquivalente = ms.InfoCruce.EsEquivalente,
        //                                TieneInventarioCongelado = true,
        //                                FechaModificacion = DateTime.Now,
        //                                FueReingenieria = true,
        //                                UsuarioModifica = userID,
        //                                EsAsignado = conAsignacion,
        //                                NumeroUnicoAsignadoID = null,
        //                                SegmentoAsignado = null
        //                            };
        //                            odts.OrdenTrabajoMaterial.Add(odtm);

                                    
        //                        }
        //                    }
        //                    #endregion

        //                    //Agregarlo a la ODT
        //                    odtes.OrdenTrabajoEspecialSpool.Add(odtesSpool);

        //                }

        //                //cambios a los números únicos (inventarios)
        //                lstNumUnico.ForEach(ctx.NumeroUnico.ApplyChanges);
        //            }

        //            #endregion                    
                   
        //            //nueva ODT
        //            ctx.OrdenTrabajoEspecial.ApplyChanges(odtes);
        //            ctx.SaveChanges();
        //            return odtes;
        //        }
        //    }
        //    catch (OptimisticConcurrencyException)
        //    {
        //        throw new ExcepcionConcurrencia(MensajesError.Excepcion_Concurrencia);
        //    }
        //}


        //public void GenerarOrdenesDeTrabajoEspecial(List<Spool> spools, Guid userID, int proyectoID, int tallerID)
        //{

        //    using (SamContext ctx = new SamContext())
        //    {
        //            OrdenTrabajoEspecial odtes = new OrdenTrabajoEspecial();

        //            ProyectoConfiguracion configProyecto = (from configuracion in ctx.ProyectoConfiguracion
        //                                 where configuracion.ProyectoID == proyectoID
        //                                 select configuracion).Single();

        //            ProyectoConsecutivo consecutivos = ctx.ProyectoConsecutivo.Where(x => x.ProyectoID == proyectoID).Single();

        //            string prefijo = configProyecto.PrefijoOrdenTrabajo;

        //            int numeroOrden = consecutivos.ConsecutivoODTEspecial.SafeIntParse();
        //            string formato = "";

        //            for (int i = 0; i < configProyecto.DigitosOrdenTrabajo; i++)
        //            {
        //                formato = formato + "0";
        //            }

        //            //numeroOrden = prefijo + (numeroOrden + 1).ToString(formato);


        //            odtes.NumeroOrden = prefijoEspecial + "-" + prefijo + (numeroOrden + 1).ToString(formato);
        //            odtes.EstatusOrdenID = (int)EstatusOrdenDeTrabajo.Activa;
        //            odtes.ProyectoID = proyectoID;
        //            odtes.FechaModificacion = DateTime.Now;
        //            odtes.FechaOrden = DateTime.Now;
        //            odtes.UsuarioModifica = userID;
        //            odtes.TallerID = tallerID;
        //            ctx.OrdenTrabajoEspecial.ApplyChanges(odtes);

        //            consecutivos.StartTracking();
        //            consecutivos.ConsecutivoODTEspecial = consecutivos.ConsecutivoODTEspecial.SafeIntParse() + 1;
        //            consecutivos.FechaModificacion = DateTime.Now;
        //            consecutivos.UsuarioModifica = userID;
        //            consecutivos.StopTracking();
        //            ctx.ProyectoConsecutivo.ApplyChanges(consecutivos);

        //            ctx.SaveChanges();

        //            OrdenTrabajoEspecialSpoolBL.Instance.GenerarOrdenTrabajoEspecialSpool(odtes.OrdenTrabajoEspecialID, userID,
        //                spools, odtes.NumeroOrden);
        //    }
        //}

    }//Fin Clase
}
