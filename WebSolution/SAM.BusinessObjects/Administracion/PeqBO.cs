using System.Collections.Generic;
using System.Linq;
using SAM.BusinessObjects;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;

namespace SAM.BusinessObjects.Administracion
{
    public class PeqBO
    {
        private static readonly object _mutex = new object();
        private static PeqBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private PeqBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase PeqBO
        /// </summary>
        /// <returns></returns>
        public static PeqBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new PeqBO();
                    }
                }
                return _instance;
            }
        }

        public Peq Obtener(int peqID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Peq.Where(x => x.PeqID == peqID).SingleOrDefault();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diametroID"></param>
        /// <param name="cedulaID"></param>
        /// <param name="tipoJuntaID"></param>
        /// <param name="familiaAceroID"></param>
        /// <returns></returns>
        public Peq Obtener(int diametroID, int cedulaID, int tipoJuntaID, int familiaAceroID, int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Peq.Where(x => x.ProyectoID == proyectoID &&
                                            x.DiametroID == diametroID &&
                                            x.CedulaID == cedulaID &&
                                            x.TipoJuntaID == tipoJuntaID &&
                                            x.FamiliaAceroID == familiaAceroID).SingleOrDefault();
            }
        }

       

        public List<Peq> ObtenerPorProyecto(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Peq.Where(x => x.ProyectoID == proyectoID).ToList();
            }
        }
    }
}
