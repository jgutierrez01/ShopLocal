using System.Collections.Generic;
using System.Data;
using System.Linq;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;

namespace SAM.BusinessObjects.Ingenieria
{
    public class CorteSpoolBO
    {
        public event TableChangedHandler CorteSpoolCambio;
        private static readonly object _mutex = new object();
        private static CorteSpoolBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>

        private CorteSpoolBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase CorteSpoolBO
        /// </summary>
        /// <returns></returns>
        public static CorteSpoolBO Instance
        {
            get
            {
                lock(_mutex)
                {
                    if(_instance == null)
                    {
                        _instance = new CorteSpoolBO();
                    }
                }
                return _instance;
            }
        }

        public CorteSpool Obtener(int corteSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.CorteSpool.Where(x=> x.CorteSpoolID == corteSpoolID).SingleOrDefault();
            }
        }

        public List<CorteSpool> ObtenerPorSpool(int spoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.CorteSpool.Where(x => x.SpoolID == spoolID).ToList();
            }
        }

        public List<CorteSpool> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.CorteSpool.ToList();
            }
        }

        public void Guarda(CorteSpool corteSpool)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ctx.CorteSpool.ApplyChanges(corteSpool);

                    ctx.SaveChanges();
                }

                if (CorteSpoolCambio != null)
                {
                    CorteSpoolCambio();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

    }
}
