using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Catalogos
{
    public class TipoPruebaSpoolBO
    {
        public event TableChangedHandler TipoPruebaSpoolCambio;
        private static readonly object _mutex = new object();
        private static TipoPruebaSpoolBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private TipoPruebaSpoolBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase TipoPruebaSpoolBO
        /// </summary>
        /// <returns></returns>
        public static TipoPruebaSpoolBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new TipoPruebaSpoolBO();
                    }
                }
                return _instance;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TipoPruebaSpool> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.TipoPruebaSpool.ToList();
            }
        }
    }
}
