using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.BusinessObjects.Utilerias;
using Mimo.Framework.Common;
using Mimo.Framework.Extensions;
using System.Data.Objects;

namespace SAM.BusinessObjects.Workstatus
{
    public class CorteBO
    {
        private static readonly object _mutex = new object();
        private static CorteBO _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private CorteBO()
        {
        }

        public static CorteBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CorteBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtiene el listado de cortes para un proyecto en especifico
        /// </summary>
        /// <param name="proyectoID">ID del proyecto para el que se desea consultar el reporte de cortes</param>
        /// <returns>Listado de Cortes</returns>
        public List<GrdCorte> ObtenerListaCorte(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<Corte> query = ctx.Corte.AsQueryable();

                //Filtramos los cortes por proyecto
                query = query.Where(x => x.ProyectoID == proyectoID);

                List<GrdCorte> lstCortes = (from corte in query
                                            select new GrdCorte
                              {
                                  CorteID = corte.CorteID,
                                  NumeroUnico = corte.NumeroUnicoCorte.NumeroUnico.Codigo,
                                  CantidadCortes = corte.CorteDetalle.Count(),
                                  ItemCode = corte.NumeroUnicoCorte.NumeroUnico.ItemCode.Codigo,
                                  Descripcion = corte.NumeroUnicoCorte.NumeroUnico.ItemCode.DescripcionEspanol,
                                  Diametro1 = corte.NumeroUnicoCorte.NumeroUnico.Diametro1,
                                  Cancelado = corte.Cancelado,
                                  Sobrante = corte.Sobrante.Value,
                                  Merma = corte.Merma.Value
                              }).ToList();

                lstCortes.ForEach(c => c.Estatus = TraductorEnumeraciones.TextoCanceladoAprobado(!c.Cancelado));

                return lstCortes;

            }
        }

        /// <summary>
        /// Obtiene un corte y el detalle del mismo
        /// </summary>
        /// <param name="corteID">ID del corte a consultar</param>
        /// <returns>Corte</returns>
        public Corte ObtenerCorteConDetalles(int corteID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Corte.Include("NumeroUnicoCorte")
                                .Include("CorteDetalle")
                                .Include("CorteDetalle.Maquina")
                                .Include("CorteDetalle.OrdenTrabajoSpool")
                                .Include("CorteDetalle.MaterialSpool")
                                .Where(x => x.CorteID == corteID).Single();

            }
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="corteID"></param>
        /// <returns></returns>
        public int ObtenerProyectoID(int corteID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ProyectoCorte(ctx, corteID);
            }
        }


        /// <summary>
        /// Versión compilada del query para permisos de corte
        /// </summary>
        private static readonly Func<SamContext, int, int> ProyectoCorte =
        CompiledQuery.Compile<SamContext, int, int>
        (
            (ctx, id) => ctx.Corte
                            .Where(x => x.CorteID == id)
                            .Select(x => x.ProyectoID)
                            .Single()
        );

    }
}
