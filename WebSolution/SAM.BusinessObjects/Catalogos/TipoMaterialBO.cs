using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;

namespace SAM.BusinessObjects.Catalogos
{
    public class TipoMaterialBO
    {
        private static readonly object _mutex = new object();
        private static TipoMaterialBO _instance;

        /// <summary>
        /// constructor privado para implementar patron singleton
        /// </summary>
        private TipoMaterialBO()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public static TipoMaterialBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new TipoMaterialBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TipoMaterial> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.TipoMaterial.ToList();
            }
        }

    }
}
