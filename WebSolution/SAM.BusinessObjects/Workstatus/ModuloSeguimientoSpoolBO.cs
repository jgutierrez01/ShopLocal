using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;

namespace SAM.BusinessObjects.Workstatus
{
   

    public class ModuloSeguimientoSpoolBO
    {
        private static readonly object _mutex = new object();
        private static ModuloSeguimientoSpoolBO _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private ModuloSeguimientoSpoolBO()
        {
        }

        public static ModuloSeguimientoSpoolBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ModuloSeguimientoSpoolBO();
                    }
                }
                return _instance;
            }
        }

        public List<ModuloSeguimientoSpool> ObtenerTodas()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ModuloSeguimientoSpool.ToList();
            }
        }
    }
}
