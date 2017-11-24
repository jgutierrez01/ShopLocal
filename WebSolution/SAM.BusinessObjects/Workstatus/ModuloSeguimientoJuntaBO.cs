using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;

namespace SAM.BusinessObjects.Workstatus
{

    public class ModuloSeguimientoJuntaBO
    {
        private static readonly object _mutex = new object();
        private static ModuloSeguimientoJuntaBO _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private ModuloSeguimientoJuntaBO()
        {
        }

        public static ModuloSeguimientoJuntaBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ModuloSeguimientoJuntaBO();
                    }
                }
                return _instance;
            }
        }

        public List<ModuloSeguimientoJunta> ObtenerTodas()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ModuloSeguimientoJunta.ToList();
            }
        }
    }
}
