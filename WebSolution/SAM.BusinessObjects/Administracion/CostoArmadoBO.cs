using System.Collections.Generic;
using System.Linq;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using System;

namespace SAM.BusinessObjects.Administracion
{
    public class CostoArmadoBO
    {
        private static readonly object _mutex = new object();
        private static CostoArmadoBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private CostoArmadoBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase CostoArmadoBO
        /// </summary>
        /// <returns></returns>
        public static CostoArmadoBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CostoArmadoBO();
                    }
                }
                return _instance;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public List<CostoArmado> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.CostoArmado.ToList();
            }
        }

        public List<CostoArmado> ObtenerTodosPorId(int proyectoId, int famAceroId, int tipoJuntaId)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.CostoArmado.Where(x => x.ProyectoID == proyectoId &&
                                                  x.FamiliaAceroID == famAceroId &&
                                                  x.TipoJuntaID == tipoJuntaId).OrderBy(x => x.DiametroID).ThenBy(x => x.CedulaID).ToList();
            }
        }
    }
}
