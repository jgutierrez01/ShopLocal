using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;

namespace SAM.BusinessObjects.Workstatus
{
    
    public class CampoSeguimientoSpoolBO
    {
        private static readonly object _mutex = new object();
        private static CampoSeguimientoSpoolBO _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private CampoSeguimientoSpoolBO()
        {
        }

        public static CampoSeguimientoSpoolBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CampoSeguimientoSpoolBO();
                    }
                }
                return _instance;
            }
        }

        public List<CampoSeguimientoSpool> ObtenerTodas()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.CampoSeguimientoSpool.ToList();
            }
        }
    }
}
