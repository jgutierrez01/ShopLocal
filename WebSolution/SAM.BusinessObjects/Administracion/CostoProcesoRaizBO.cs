using System.Collections.Generic;
using System.Linq;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using System;

namespace SAM.BusinessObjects.Administracion
{
    public class CostoProcesoRaizBO
    {
        private static readonly object _mutex = new object();
        private static CostoProcesoRaizBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private CostoProcesoRaizBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase CostoProcesoRaizBO
        /// </summary>
        /// <returns></returns>

        public static CostoProcesoRaizBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CostoProcesoRaizBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public List<CostoProcesoRaiz> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.CostoProcesoRaiz.ToList();
            }
        }

        public List<CostoProcesoRaiz> ObtenerTodosPorId(int proyectoId, int famAceroId, int tipoJuntaId, int procesoId)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.CostoProcesoRaiz.Where(x => x.ProyectoID == proyectoId &&
                                                       x.FamiliaAceroID == famAceroId &&
                                                       x.TipoJuntaID == tipoJuntaId &&
                                                       x.ProcesoRaizID == procesoId).OrderBy(x => x.DiametroID).ThenBy(x => x.CedulaID).ToList();
            }
        }
    }
}
