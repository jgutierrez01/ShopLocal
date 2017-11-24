using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Catalogos
{
    public class UbicacionFisicaBO
    {
         private static readonly object _mutex = new object();
        private static UbicacionFisicaBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private UbicacionFisicaBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase AceroBO
        /// </summary>
        /// <returns></returns>
        public static UbicacionFisicaBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new UbicacionFisicaBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtiene todas las ubicaciones físicas dentro de un patio en base al ID del patio.
        /// </summary>
        /// <param name="patioID">ID del patio del cual se desea obtener sus ubicaciones</param>
        /// <returns>Lista de ubicaciones para el patio</returns>
        public List<UbicacionFisica> ObtenerPorPatioID(int patioID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.UbicacionFisica.Where(x => x.PatioID == patioID).ToList();
            }
        }
    }
}
