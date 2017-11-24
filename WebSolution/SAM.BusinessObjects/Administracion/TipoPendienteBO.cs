using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Administracion
{
    public class TipoPendienteBO
    {
        private static readonly object _mutex = new object();
        private static TipoPendienteBO _instance;

         /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private TipoPendienteBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase TipoPendienteBO
        /// </summary>
        /// <returns></returns>
        public static TipoPendienteBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new TipoPendienteBO();
                    }
                }
                return _instance;
            }
        }

        public TipoPendiente Obtener(int tipoPendienteID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.TipoPendiente.Where(x => x.TipoPendienteID == tipoPendienteID).Single();
            }
        }
    }
}
