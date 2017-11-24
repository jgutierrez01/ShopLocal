using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Personalizadas;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.Entities.Grid;
using System.Data.Objects;

namespace SAM.BusinessObjects.Produccion
{
    public class CorteDetalleBO
    {
        private static readonly object _mutex = new object();
        private static CorteDetalleBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private CorteDetalleBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase CorteDetalleBO
        /// </summary>
        /// <returns></returns>
        public static CorteDetalleBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CorteDetalleBO();
                    }
                }
                return _instance;
            }
        }


        /// <summary>
        /// Regresa una entidad personalizada que contiene información sobre el corte que se necesita
        /// tener disponible para la pantalla de despachos.
        /// </summary>
        /// <param name="corteDetalleID">ID del registro Corte Detalle del cual se necesita obtener la información</param>
        /// <returns>Entidad con información sobre el corte</returns>
        public InfoCorteDespacho ObtenerInformacionDeCorteParaDespacho(int? corteDetalleID)
        {
            using (SamContext ctx = new SamContext())
            {
                //Debe existir
                CorteDetalle cd = ctx.CorteDetalle
                                     .Where(x => x.CorteDetalleID == corteDetalleID).Single();

                //Cargar entidades relacionadas necesarias para llenar la información solicitada
                ctx.LoadProperty<CorteDetalle>(cd, x => x.Corte);
                ctx.LoadProperty<Corte>(cd.Corte, x => x.NumeroUnicoCorte);
                ctx.LoadProperty<NumeroUnicoCorte>(cd.Corte.NumeroUnicoCorte, x => x.NumeroUnico);
                ctx.LoadProperty<NumeroUnico>(cd.Corte.NumeroUnicoCorte.NumeroUnico, x => x.ItemCode);

                return new InfoCorteDespacho
                {
                    ItemCodeID = cd.Corte.NumeroUnicoCorte.NumeroUnico.ItemCode.ItemCodeID,
                    CodigoItemCode = cd.Corte.NumeroUnicoCorte.NumeroUnico.ItemCode.Codigo,
                    CodigoNumeroUnico = cd.Corte.NumeroUnicoCorte.NumeroUnico.Codigo,
                    CorteDetalleID = cd.CorteDetalleID,
                    DescripcionItemCode = cd.Corte.NumeroUnicoCorte.NumeroUnico.ItemCode.DescripcionEspanol,
                    Diametro1 = cd.Corte.NumeroUnicoCorte.NumeroUnico.Diametro1,
                    Diametro2 = cd.Corte.NumeroUnicoCorte.NumeroUnico.Diametro2,
                    NumeroUnicoID = cd.Corte.NumeroUnicoCorte.NumeroUnicoID,
                    Segmento = cd.Corte.NumeroUnicoCorte.Segmento,
                    LongitudDelCorte = cd.Cantidad
                };
            }

        }

        public List<GrdCorteAjuste> ObtenerCorteDetalleConAjuste(int proyectoID)
        {
      
            using (SamContext ctx = new SamContext())
            {
                Dictionary<int, string> p = ctx.Proyecto.Where(x => x.ProyectoID == proyectoID).ToDictionary(x => x.ProyectoID, y => y.Nombre);
                Dictionary<int, int?> pc = ctx.ProyectoConfiguracion.Where(x => x.ProyectoID == proyectoID).ToDictionary(x => x.ProyectoID, y => y.ToleranciaCortes);

                List<GrdCorteAjuste> lst = (from cd in ctx.CorteDetalle
                                            join ots in ctx.OrdenTrabajoSpool on cd.OrdenTrabajoSpoolID equals ots.OrdenTrabajoSpoolID
                                            join ms in ctx.MaterialSpool on cd.MaterialSpoolID equals ms.MaterialSpoolID
                                            join s in ctx.Spool on ms.SpoolID equals s.SpoolID
                                            join ic in ctx.ItemCode on ms.ItemCodeID equals ic.ItemCodeID
                                            where s.ProyectoID == proyectoID
                                            && cd.EsAjuste == true
                                            && cd.Cancelado == false

                                            select new GrdCorteAjuste
                                            {
                                                ProyectoID = s.ProyectoID,
                                                MaterialSpoolID = ms.MaterialSpoolID,
                                                SpoolID = s.SpoolID,
                                                Spool = s.Nombre,
                                                NumeroControl = ots.NumeroControl,
                                                EtiquetaMaterial = ms.Etiqueta,
                                                Descripcion = ic.DescripcionEspanol,
                                                LongitudIngenieria = ms.Cantidad,
                                                LongitudCorte = cd.Cantidad,
                                            }).ToList();

                lst.ForEach(x =>
                {
                    x.Proyecto = p[x.ProyectoID];
                    x.Tolerancia = pc[x.ProyectoID] != null ? pc[x.ProyectoID] : 0;
                });

                return lst;
            }
        }

        public GrdCorteAjuste ObtenerDetalleParaAjuste(int materialSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                GrdCorteAjuste oC = (from cd in ctx.CorteDetalle
                                     join ots in ctx.OrdenTrabajoSpool on cd.OrdenTrabajoSpoolID equals ots.OrdenTrabajoSpoolID
                                     join ms in ctx.MaterialSpool on cd.MaterialSpoolID equals ms.MaterialSpoolID
                                     join s in ctx.Spool on ms.SpoolID equals s.SpoolID
                                     join ic in ctx.ItemCode on ms.ItemCodeID equals ic.ItemCodeID
                                     where ms.MaterialSpoolID == materialSpoolID

                                     select new GrdCorteAjuste
                                     {
                                         ProyectoID = s.ProyectoID,
                                         MaterialSpoolID = ms.MaterialSpoolID,
                                         SpoolID = s.SpoolID,
                                         Spool = s.Nombre,
                                         NumeroControl = ots.NumeroControl,
                                         EtiquetaMaterial = ms.Etiqueta,
                                         Descripcion = ic.DescripcionEspanol,
                                         LongitudIngenieria = ms.Cantidad,
                                         LongitudCorte = cd.Cantidad,
                                     }).SingleOrDefault();

                return oC;
            }
        }
    }
}
