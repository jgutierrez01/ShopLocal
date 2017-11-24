using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.Entities.Personalizadas;

namespace SAM.BusinessObjects.Produccion
{
    public class NumeroUnicoMovimientoBO
    {
        private static readonly object _mutex = new object();
        private static NumeroUnicoMovimientoBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private NumeroUnicoMovimientoBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase TipoMovimientoBO
        /// </summary>
        /// <returns></returns>
        public static NumeroUnicoMovimientoBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new NumeroUnicoMovimientoBO();
                    }
                }
                return _instance;
            }
        }

        public NumeroUnicoMovimiento Obtener(int numUnicoMovID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.NumeroUnicoMovimiento.Where(x => x.NumeroUnicoMovimientoID == numUnicoMovID).SingleOrDefault();
            }
        }

        public List<NumeroUnicoMovimiento> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.NumeroUnicoMovimiento.ToList();
            }
        }

        public List<Simple> ObtenerEntradasPorSegmento(int numUnicoMovID)
        {
            using(SamContext ctx = new SamContext())
            {
                List<Simple> entradaSegmento = (from mov in ctx.NumeroUnicoMovimiento
                                                               where mov.NumeroUnicoID == numUnicoMovID
                                                                 && (mov.TipoMovimientoID == (int)TipoMovimientoEnum.Recepcion ||
                                                                     mov.TipoMovimientoID == (int)TipoMovimientoEnum.EntradaOtrosProcesos ||
                                                                     mov.TipoMovimientoID == (int)TipoMovimientoEnum.EntradasSegmentacion)
                                                               group mov by mov.Segmento into grp
                                                               select new Simple
                                                               {
                                                                   Valor = grp.Key,
                                                                   ID = grp.Sum(x => x.Cantidad)
                                                               }).OrderBy(x => x.Valor).ToList();
                return entradaSegmento;
            }
        }

        public int ObtenerEntradasGenerales(int numUnicoMovID)
        {
            using (SamContext ctx = new SamContext())
            {
                int q = (from mov in ctx.NumeroUnicoMovimiento
                         where mov.NumeroUnicoID == numUnicoMovID && (mov.TipoMovimientoID == (int)TipoMovimientoEnum.Recepcion || mov.TipoMovimientoID == (int)TipoMovimientoEnum.EntradaOtrosProcesos)
                         select mov.Cantidad).ToList().Sum();
                return q;
            }
        }

        public List<Simple> ObtenerSalidasPorSegmento(int numUnicoMovID, List<int> salidas)
        {
            using (SamContext ctx = new SamContext())
            {
                List<Simple> salidaSeg = (from seg in ctx.NumeroUnicoMovimiento
                                          where seg.NumeroUnicoID == numUnicoMovID
                                          && salidas.Contains(seg.TipoMovimientoID)
                                          && seg.Estatus == "A"
                                          group seg by seg.Segmento into grp
                                          select new Simple
                                          {
                                              Valor = grp.Key,
                                              ID = grp.Sum(x => x.Cantidad)
                                          }).OrderBy(x => x.Valor).ToList();

                return salidaSeg;
            }
        }


        public List<Simple> ObtenerSalidasTemporalesPorSegmento(int numUnicoID)
        {
            using (SamContext ctx = new SamContext())
            {               

                List<Simple> entradasPintura = (from seg in ctx.NumeroUnicoMovimiento
                                          where seg.NumeroUnicoID == numUnicoID
                                          && seg.TipoMovimientoID == (int)TipoMovimientoEnum.EntradaPintura
                                           && seg.Estatus == "A"
                                          group seg by seg.Segmento into grp
                                          select new Simple
                                          {
                                              Valor = grp.Key,
                                              ID = grp.Sum(x => x.Cantidad)
                                          }).OrderBy(x => x.Valor).ToList();

                List<Simple> salidasPintura = (from seg in ctx.NumeroUnicoMovimiento
                                                where seg.NumeroUnicoID == numUnicoID
                                                && seg.TipoMovimientoID == (int)TipoMovimientoEnum.SalidaPintura
                                                 && seg.Estatus == "A"
                                                group seg by seg.Segmento into grp
                                                select new Simple
                                                {
                                                    Valor = grp.Key,
                                                    ID = grp.Sum(x => x.Cantidad)
                                                }).OrderBy(x => x.Valor).ToList();

                List<Simple> salidasTemporales = (from salida in salidasPintura
                                                  let entrada = entradasPintura.Where(x => x.Valor == salida.Valor).SingleOrDefault()
                                                  select new Simple
                                                  {
                                                      Valor = salida.Valor,
                                                      ID = entrada != null ? salida.ID - entrada.ID : salida.ID
                                                  }).ToList();
                return salidasTemporales;
            }
        }

        public int ObtenerSalidasGenerales(int numUnicoMovID, List<int> salidas)
        {
            using (SamContext ctx = new SamContext())
            {
                var q = (from seg in ctx.NumeroUnicoMovimiento
                         where seg.NumeroUnicoID == numUnicoMovID
                          && seg.Estatus == "A"
                         select seg).ToList();

                int salidasGenerales = q.Where(x => salidas.Contains(x.TipoMovimientoID)).Select(y => y.Cantidad).Sum();
                return salidasGenerales;
            }
        }
    }
}
