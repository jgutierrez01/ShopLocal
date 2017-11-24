using System.Linq;
using SAM.Entities;
using System.Collections.Generic;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Catalogos
{
    public class KgTeoricoBO
    {
        private static readonly object _mutex = new object();
        private static KgTeoricoBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private KgTeoricoBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase KgTeoricoBO
        /// </summary>
        /// <returns></returns>
        public static KgTeoricoBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new KgTeoricoBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<KgTeorico> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.KgTeorico.ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diametroID"></param>
        /// <param name="cedulaID"></param>
        /// <returns></returns>
        public KgTeorico ObtenerPorDiametroCedula(int diametroID, int cedulaID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.KgTeorico.Where(x => x.DiametroID == diametroID && x.CedulaID == cedulaID).SingleOrDefault();
            }
        }

    }
}
