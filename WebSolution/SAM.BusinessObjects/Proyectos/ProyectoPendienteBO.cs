using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;

namespace SAM.BusinessObjects.Proyectos
{
    public class ProyectoPendienteBO
    {
        private static readonly object _mutex = new object();
        private static ProyectoPendienteBO _instance;

         /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private ProyectoPendienteBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase ProyectoPendienteBO
        /// </summary>
        /// <returns></returns>
        public static ProyectoPendienteBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ProyectoPendienteBO();
                    }
                }
                return _instance;
            }
        }

        public ProyectoPendiente Obtener(int proyectoPendienteID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ProyectoPendiente.Where(x => x.ProyectoPendienteID == proyectoPendienteID).SingleOrDefault();
            }
        }

        public ProyectoPendiente ObtenerPorProyectoIDyTipoPendiente(int proyectoID, int tipoPendienteID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ProyectoPendiente.Where(x => x.ProyectoID == proyectoID && x.TipoPendienteID == tipoPendienteID).SingleOrDefault();
            }
        }
    }
}
