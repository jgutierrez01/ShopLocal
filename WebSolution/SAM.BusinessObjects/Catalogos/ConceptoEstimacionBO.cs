using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;

namespace SAM.BusinessObjects.Catalogos
{
    public class ConceptoEstimacionBO
    {
        private static readonly object _mutex = new object();
        private static ConceptoEstimacionBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private ConceptoEstimacionBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase ConceptoEstimacionBO
        /// </summary>
        /// <returns></returns>
        public static ConceptoEstimacionBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ConceptoEstimacionBO();
                    }
                }
                return _instance;
            }
        }

        internal List<ConceptoEstimacion> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ConceptoEstimacion.ToList();
            }
        }

        /// <summary>
        /// Este metodo esta pensado para EstimacionDeJunta y regresa los datos necesarios para poder filtar por JuntaWorkstatusID
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public List<ConceptoEstimacion> ObtenerConceptosEstimacionJunta()
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.EstimacionJunta.ToList();
                return ctx.ConceptoEstimacion.Where(x =>x.AplicaParaJunta).ToList();
            }
        }

        public List<ConceptoEstimacion> ObtenerConceptosEstimacionSpool()
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.EstimacionSpool.ToList();
                return ctx.ConceptoEstimacion.Where(x => x.AplicaParaSpool).ToList();
            }
        }
    }
}
