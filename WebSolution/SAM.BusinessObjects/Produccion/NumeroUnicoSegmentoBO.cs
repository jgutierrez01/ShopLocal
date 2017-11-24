using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Produccion
{
    public class NumeroUnicoSegmentoBO
    {
        private static readonly object _mutex = new object();
        private static NumeroUnicoSegmentoBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private NumeroUnicoSegmentoBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase NumeroUnicoSegmentoBO
        /// </summary>
        /// <returns></returns>
        public static NumeroUnicoSegmentoBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new NumeroUnicoSegmentoBO();
                    }
                }
                return _instance;
            }
        }

        public NumeroUnicoSegmento Obtener(int numeroUnicoSegmentoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.NumeroUnicoSegmento.Where(x => x.NumeroUnicoSegmentoID == numeroUnicoSegmentoID).SingleOrDefault();
            }
        }

        public List<NumeroUnicoSegmento> ObtenerPorNumeroUnico(int numeroUnicoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.NumeroUnicoSegmento
                          .Where(x => x.NumeroUnicoID == numeroUnicoID)
                          .ToList();
            }
        
        }
    }
}
