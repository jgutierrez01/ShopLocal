using System.Collections.Generic;
using System.Data;
using System.Linq;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.Entities.RadCombo;
using SAM.Entities.Personalizadas;
using Mimo.Framework.Common;
using System.Data.Objects;
using System;

namespace SAM.BusinessObjects.Ingenieria
{
    public class MaterialSpoolBO
    {
        public event TableChangedHandler MaterialSpoolCambio;
        private static readonly object _mutex = new object();
        private static MaterialSpoolBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>

        private MaterialSpoolBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase MaterialSpoolBO
        /// </summary>
        /// <returns></returns>
        public static MaterialSpoolBO Instance
        {
            get
            {
                lock(_mutex)
                {
                    if(_instance == null)
                    {
                        _instance = new MaterialSpoolBO();
                    }
                }
                return _instance;
            }
        }

        public MaterialSpool Obtener(int materialSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.MaterialSpool.Include("ItemCode")
                                        .Include("Despacho")
                                        .Where(x => x.MaterialSpoolID == materialSpoolID).SingleOrDefault();
            }
        }

        public bool TieneODTMaterial(int materialSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.OrdenTrabajoMaterial.Where(x => x.MaterialSpoolID == materialSpoolID).Any();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spoolID"></param>
        /// <returns></returns>
        public List<DetMaterialSpool> ObtenerDetalleMaterialPorSpool(int spoolID)
        {
            List<MaterialSpool> lstMaterial;

            using (SamContext ctx = new SamContext())
            {
                ctx.MaterialSpool.MergeOption = MergeOption.NoTracking;
                ctx.ItemCode.MergeOption = MergeOption.NoTracking;

                lstMaterial = ctx.MaterialSpool
                                 .Include("ItemCode")
                                 .Where(x => x.SpoolID == spoolID).ToList();
            }

            return (from ms in lstMaterial select new DetMaterialSpool(ms)).ToList();
        }

        public void Guarda(MaterialSpool material)
        {

            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ctx.MaterialSpool.ApplyChanges(material);

                    ctx.SaveChanges();
                }

                if (MaterialSpoolCambio != null)
                {
                    MaterialSpoolCambio();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordenTrabajoSpoolID"></param>
        /// <param name="numeroUnicoID"></param>
        /// <param name="segmento"></param>
        /// <param name="etiquetaMaterial"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public IEnumerable<RadMaterialParaCorte> ListaMaterialesPorNumeroControl(int ordenTrabajoSpoolID, int numeroUnicoID, string segmento, string etiquetaMaterial, int skip, int take)
        {            
            using (SamContext ctx = new SamContext())
            {
                ctx.NumeroUnico.MergeOption = MergeOption.NoTracking;
                ctx.ItemCodeEquivalente.MergeOption = MergeOption.NoTracking;

                bool esAsignacion = ctx.OrdenTrabajoSpool
                                       .Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID)
                                       .Select(x => x.EsAsignado)
                                       .Single();

                IEnumerable<RadMaterialParaCorte> data = null;

                if (!esAsignacion)
                {
                    data = ListaMaterialesPorNumCtrlSinAsignacion(ordenTrabajoSpoolID, numeroUnicoID, ctx);
                }
                else
                {
                    data = ListaMaterialesPorNumCtrlConAsignacion(ordenTrabajoSpoolID, numeroUnicoID, segmento, ctx);
                }

                return data.Where(x => x.EtiquetaMaterial.StartsWith(etiquetaMaterial, StringComparison.InvariantCultureIgnoreCase))
                           .OrderBy(x => x.EtiquetaMaterial)
                           .Skip(skip)
                           .Take(take)
                           .ToList();
            }
              
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordenTrabajoSpoolID"></param>
        /// <param name="numeroUnicoID"></param>
        /// <param name="segmento"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public IEnumerable<RadMaterialParaCorte> ListaMaterialesPorNumCtrlConAsignacion(int ordenTrabajoSpoolID, int numeroUnicoID, string segmento, SamContext ctx)
        {
            int otSpoolID = ordenTrabajoSpoolID;

            IEnumerable<RadMaterialParaCorte> material = from mat in ctx.MaterialSpool
                                                         join otm in ctx.OrdenTrabajoMaterial on mat.MaterialSpoolID equals otm.MaterialSpoolID
                                                         join cs in ctx.CorteSpool on mat.SpoolID equals cs.SpoolID
                                                         join ic in ctx.ItemCode on mat.ItemCodeID equals ic.ItemCodeID
                                                         where  otm.OrdenTrabajoSpoolID == otSpoolID
                                                                && (otm.TieneCorte == null || !otm.TieneCorte.Value)
                                                                && otm.SegmentoAsignado == segmento
                                                                && otm.NumeroUnicoAsignadoID == numeroUnicoID
                                                                 && mat.Etiqueta == cs.EtiquetaMaterial
                                                         select new RadMaterialParaCorte
                                                         {
                                                             MaterialSpoolID = mat.MaterialSpoolID,
                                                             Descripcion = ic.DescripcionEspanol,
                                                             EtiquetaMaterial = mat.Etiqueta,
                                                             ItemCode = ic.Codigo,
                                                             EsEquivalente = "No",
                                                             EtiquetaSeccion = cs.EtiquetaSeccion
                                                         };

            return material.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordenTrabajoSpoolID"></param>
        /// <param name="numeroUnicoID"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public IEnumerable<RadMaterialParaCorte> ListaMaterialesPorNumCtrlSinAsignacion(int ordenTrabajoSpoolID, int numeroUnicoID, SamContext ctx)
        {
            //Obtengo el item code y diametros del numero unico 
            IQueryable<ItemCodeIntegrado> itemCodes = from nu in ctx.NumeroUnico
                                                      where nu.NumeroUnicoID == numeroUnicoID
                                                      select new ItemCodeIntegrado
                                                      {
                                                          ItemCodeID = nu.ItemCodeID.Value,
                                                          Diametro1 = nu.Diametro1,
                                                          Diametro2 = nu.Diametro2
                                                      };

            //Obtengo los itemcodes de los que el numero unico es equivalente
            IQueryable<ItemCodeIntegrado> icEquivalentes = from iceq in ctx.ItemCodeEquivalente
                                                           where itemCodes.Contains(new ItemCodeIntegrado
                                                           {
                                                               ItemCodeID = iceq.ItemEquivalenteID,
                                                               Diametro1 = iceq.DiametroEquivalente1,
                                                               Diametro2 = iceq.DiametroEquivalente2
                                                           })
                                                           select new ItemCodeIntegrado
                                                           {
                                                               ItemCodeID = iceq.ItemCodeID,
                                                               Diametro1 = iceq.Diametro1,
                                                               Diametro2 = iceq.Diametro2
                                                           };

            int otSpoolID = ordenTrabajoSpoolID;


            var list = from mat in ctx.MaterialSpool
                       join otm in ctx.OrdenTrabajoMaterial on mat.MaterialSpoolID equals otm.MaterialSpoolID
                       join ic in ctx.ItemCode on mat.ItemCodeID equals ic.ItemCodeID
                       join cs in ctx.CorteSpool on mat.SpoolID equals cs.SpoolID
                       where otm.OrdenTrabajoSpoolID == otSpoolID
                       select otm;

            IEnumerable<RadMaterialParaCorte> material = from mat in ctx.MaterialSpool
                                                         join otm in ctx.OrdenTrabajoMaterial on mat.MaterialSpoolID equals otm.MaterialSpoolID
                                                         join ic in ctx.ItemCode on mat.ItemCodeID equals ic.ItemCodeID
                                                         join cs in ctx.CorteSpool on mat.SpoolID equals cs.SpoolID
                                                         where otm.OrdenTrabajoSpoolID == otSpoolID
                                                         && ((!otm.TieneCorte.HasValue) || (!otm.TieneCorte.Value))
                                                          && (itemCodes.Contains(new ItemCodeIntegrado
                                                          {
                                                              ItemCodeID = mat.ItemCodeID,
                                                              Diametro1 = mat.Diametro1,
                                                              Diametro2 = mat.Diametro2
                                                          }))
                                                          && mat.Etiqueta == cs.EtiquetaMaterial
                                                         select new RadMaterialParaCorte
                                                         {
                                                             MaterialSpoolID = mat.MaterialSpoolID,
                                                             Descripcion = ic.DescripcionEspanol,
                                                             EtiquetaMaterial = mat.Etiqueta,
                                                             ItemCode = ic.Codigo,
                                                             EsEquivalente = "No",
                                                             EtiquetaSeccion = cs.EtiquetaSeccion
                                                         };


            IEnumerable<RadMaterialParaCorte> equivalentes = from mat in ctx.MaterialSpool
                                                             join otm in ctx.OrdenTrabajoMaterial on mat.MaterialSpoolID equals otm.MaterialSpoolID
                                                             join ic in ctx.ItemCode on mat.ItemCodeID equals ic.ItemCodeID
                                                             join cs in ctx.CorteSpool on mat.SpoolID equals cs.SpoolID
                                                             where otm.OrdenTrabajoSpoolID == otSpoolID
                                                             && (otm.TieneCorte == null || !otm.TieneCorte.Value)
                                                              && (icEquivalentes.Contains(new ItemCodeIntegrado
                                                              {
                                                                  ItemCodeID = mat.ItemCodeID,
                                                                  Diametro1 = mat.Diametro1,
                                                                  Diametro2 = mat.Diametro2
                                                              }))
                                                              && (!itemCodes.Contains(new ItemCodeIntegrado
                                                              {
                                                                  ItemCodeID = mat.ItemCodeID,
                                                                  Diametro1 = mat.Diametro1,
                                                                  Diametro2 = mat.Diametro2
                                                              }))
                                                              && mat.Etiqueta == cs.EtiquetaMaterial
                                                             select new RadMaterialParaCorte
                                                             {
                                                                 MaterialSpoolID = mat.MaterialSpoolID,
                                                                 Descripcion = ic.DescripcionEspanol,
                                                                 EtiquetaMaterial = mat.Etiqueta,
                                                                 ItemCode = ic.Codigo,
                                                                 EsEquivalente = LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Yes" : "Sí",
                                                                 EtiquetaSeccion = cs.EtiquetaSeccion
                                                             };

            //Obtenemos los numeros unicos que contienen el item code o equivalentes.
            return material.Union(equivalentes).ToList();
        }

        public List<MaterialSpool> ObtenerListaMaterialesPorProyecto(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<MaterialSpool> materiales = ctx.MaterialSpool.Include("Spool").Where(x => x.Spool.ProyectoID == proyectoID);
                return materiales.ToList();
            }
        }

    }
}
