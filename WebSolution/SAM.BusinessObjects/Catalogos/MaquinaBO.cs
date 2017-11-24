using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Catalogos
{
    public class MaquinaBO
    {
        private static readonly object _mutex = new object();
        private static MaquinaBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private MaquinaBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase AceroBO
        /// </summary>
        /// <returns></returns>
        public static MaquinaBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new MaquinaBO();
                    }
                }
                return _instance;
            }
        }


        /// <summary>
        /// Obtiene todas las maquinas y el nombre del patio al que pertenecen
        /// </summary>
        /// <param name="aceroID"></param>
        /// <returns></returns>
        public List<Maquina> ObtenerTodasConPatio()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Maquina
                            .Include("Patio").ToList();
            }
        }

        /// <summary>
        /// Obtiene todas las maquinas y el nombre del patio al que pertenecen
        /// </summary>
        /// <param name="aceroID"></param>
        /// <returns></returns>
        public Maquina ObtenerMaquinaConPatio(int maquinaID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Maquina
                            .Include("Patio").Where(x => x.MaquinaID == maquinaID).FirstOrDefault();
            }
        }

    }
}
