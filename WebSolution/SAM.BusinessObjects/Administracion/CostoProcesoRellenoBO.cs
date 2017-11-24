using System.Collections.Generic;
using System.Linq;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using System;

namespace SAM.BusinessObjects.Administracion
{
    public class CostoProcesoRellenoBO
    {
        private static readonly object _mutex = new object();
        private static CostoProcesoRellenoBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private CostoProcesoRellenoBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase CostoProcesoRellenoBO
        /// </summary>
        /// <returns></returns>
        public static CostoProcesoRellenoBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CostoProcesoRellenoBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public List<CostoProcesoRelleno> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.CostoProcesoRelleno.ToList();
            }
        }

        public List<CostoProcesoRelleno> ObtenerTodosPorId(int proyectoId, int famAceroId, int tipoJuntaId, int procesoId)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.CostoProcesoRelleno.Where(x => x.ProyectoID == proyectoId &&
                                                          x.FamiliaAceroID == famAceroId &&
                                                          x.TipoJuntaID == tipoJuntaId &&
                                                          x.ProcesoRellenoID == procesoId).OrderBy(x => x.DiametroID).ThenBy(x => x.CedulaID).ToList();
            }
        }
    }
}
