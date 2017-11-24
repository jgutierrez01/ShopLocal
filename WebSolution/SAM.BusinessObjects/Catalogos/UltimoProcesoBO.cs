using System.Collections.Generic;
using System.Linq;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;

namespace SAM.BusinessObjects.Catalogos
{
    public class UltimoProcesoBO
    {
        private static readonly object _mutex = new object();
        private static UltimoProcesoBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private UltimoProcesoBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase UltimoProcesoBO
        /// </summary>
        /// <returns></returns>
        public static UltimoProcesoBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new UltimoProcesoBO();
                    }
                }
                return _instance;
            }
        }

        public UltimoProceso Obtener(int ultimoProcesoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.UltimoProceso.Where(x => x.UltimoProcesoID == ultimoProcesoID).SingleOrDefault();
            }
        }

        public List<UltimoProceso> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.UltimoProceso.ToList();
            }
        }
    }
}
