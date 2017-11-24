using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Administracion
{
    public class ModuloBO
    {
        private static readonly object _mutex = new object();
        private static ModuloBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private ModuloBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase ModuloBO
        /// </summary>
        /// <returns></returns>
        public static ModuloBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ModuloBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Modulo> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Modulo.ToList();
            }
        }
    }
}
