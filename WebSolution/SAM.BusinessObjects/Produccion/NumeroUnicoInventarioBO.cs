using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Produccion
{
    public class NumeroUnicoInventarioBO
    {
        private static readonly object _mutex = new object();
        private static NumeroUnicoInventarioBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private NumeroUnicoInventarioBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase NumeroUnicoInventarioBO
        /// </summary>
        /// <returns></returns>
        public static NumeroUnicoInventarioBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new NumeroUnicoInventarioBO();
                    }
                }
                return _instance;
            }
        }

        public NumeroUnicoInventario Obtener(int numeroUnicoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == numeroUnicoID).SingleOrDefault();
            }
        }

        public List<NumeroUnicoInventario> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.NumeroUnicoInventario.ToList();
            }
        }

        public int ObtenerSaldoPorNumeroUnico(int numeroUnicoID)
        {
            using(SamContext ctx = new SamContext())
            {
                int saldoTotal = (from inv in ctx.NumeroUnicoInventario
                                  where inv.NumeroUnicoID == numeroUnicoID
                                  select inv.InventarioBuenEstado).SingleOrDefault();
                return saldoTotal;
            }
        }

    }
}

