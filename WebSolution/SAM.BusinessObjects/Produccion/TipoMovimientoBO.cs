using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;

namespace SAM.BusinessObjects.Produccion
{
    public class TipoMovimientoBO
    {
        private static readonly object _mutex = new object();
        private static TipoMovimientoBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private TipoMovimientoBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase TipoMovimientoBO
        /// </summary>
        /// <returns></returns>
        public static TipoMovimientoBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new TipoMovimientoBO();
                    }
                }
                return _instance;
            }
        }

        public TipoMovimiento Obtener(int tipoMovimientoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.TipoMovimiento.Where(x => x.TipoMovimientoID == tipoMovimientoID).SingleOrDefault();
            }
        }

        public List<TipoMovimiento> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.TipoMovimiento.ToList();
            }
        }

        public List<int> ObtenerSalidasPorSegmento()
        {
            using(SamContext ctx = new SamContext())
            {
                List<int> salidas = ctx.TipoMovimiento.Where(x => x.EsEntrada == false && x.ApareceEnSaldos == true && x.TipoMovimientoID != (int)TipoMovimientoEnum.SalidaPintura)
                                                      .Select(y => y.TipoMovimientoID).ToList();
                return salidas;
            }
        }

        public List<int> ObtenerSalidasGenerales()
        {
            using (SamContext ctx = new SamContext())
            {
                List<int> salidas = ctx.TipoMovimiento.Where(x => x.EsEntrada == false && x.ApareceEnSaldos == true && (x.TipoMovimientoID != (int)TipoMovimientoEnum.SalidaPintura && x.TipoMovimientoID != (int)TipoMovimientoEnum.SalidaSegmentacion))
                                                      .Select(y => y.TipoMovimientoID).ToList();
                return salidas;
            }
        }
    }
}
