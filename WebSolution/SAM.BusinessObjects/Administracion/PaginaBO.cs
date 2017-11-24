using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;

namespace SAM.BusinessObjects.Administracion
{
    public class PaginaBO
    {
        private static readonly object _mutex = new object();
        private static PaginaBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private PaginaBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase PaginaBO
        /// </summary>
        /// <returns></returns>
        public static PaginaBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new PaginaBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtiene todos los registros de la tabla página de la BD
        /// </summary>
        /// <returns>Lista con objetos de tipo Pagina</returns>
        public List<Pagina> ObtenerTodas()
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.Pagina.MergeOption = System.Data.Objects.MergeOption.NoTracking;

                return ctx.Pagina.ToList();
            }
        }
    }
}
