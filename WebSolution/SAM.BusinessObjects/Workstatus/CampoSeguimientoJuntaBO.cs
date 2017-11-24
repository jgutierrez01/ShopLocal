using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;

namespace SAM.BusinessObjects.Workstatus
{
    public class CampoSeguimientoJuntaBO
    {
        private static readonly object _mutex = new object();
        private static CampoSeguimientoJuntaBO _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private CampoSeguimientoJuntaBO()
        {
        }

        public static CampoSeguimientoJuntaBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CampoSeguimientoJuntaBO();
                    }
                }
                return _instance;
            }
        }

        public List<CampoSeguimientoJunta> ObtenerTodas()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.CampoSeguimientoJunta.ToList();
            }
        }
    }
}
