using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Administracion
{
    public class CategoriaPendienteBO
    {
        private static readonly object _mutex = new object();
        private static CategoriaPendienteBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private CategoriaPendienteBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase AceroBO
        /// </summary>
        /// <returns></returns>
        public static CategoriaPendienteBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CategoriaPendienteBO();
                    }
                }
                return _instance;
            }
        }

        public List<CategoriaPendiente> ObtenerTodas()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.CategoriaPendiente.ToList();
            }
        }
    }
}
