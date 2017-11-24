using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;

namespace SAM.BusinessObjects.Catalogos
{
    public class FabAreaBO
    {
        private static readonly object _mutex = new object();
        private static FabAreaBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private FabAreaBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase FabAreaBO
        /// </summary>
        /// <returns></returns>
        public static FabAreaBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new FabAreaBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<FabArea> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.FabArea.ToList();
            }
        }
    }
}
