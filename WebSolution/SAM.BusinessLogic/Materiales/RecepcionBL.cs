using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Materiales;
using SAM.BusinessObjects.Modelo;
using System.Transactions;
using SAM.BusinessObjects.Proyectos;


namespace SAM.BusinessLogic.Materiales
{
    public class RecepcionBL
    {
        private static readonly object _mutex = new object();
        private static RecepcionBL _instance;

        /// <summary>
        /// constructro privado para implementar patron singleton
        /// </summary>
        private RecepcionBL()
        {
        }

        /// <summary>
        /// obtiene la instancia de la clase OrdenTrabajoBL
        /// </summary>
        public static RecepcionBL Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new RecepcionBL();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Genera una nueva recepción y los números únicos correspondientes.
        /// </summary>
        /// <param name="recepcion"></param>
        /// <param name="numerosUnicos"></param>
        /// <param name="nuevoConsecutivo"></param>
        public void GeneraRecepcion(Recepcion recepcion, List<NumeroUnico> numerosUnicos, int nuevoConsecutivo)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                using (SamContext ctx = new SamContext())
                {
                    RecepcionBO.Instance.Guarda(ctx, recepcion);
                    NumeroUnicoBO.Instance.GenerarNumerosUnicos(ctx, recepcion.RecepcionID, numerosUnicos);
                    ProyectoBO.Instance.ActualizaConsecutivoNumeroUnicos(ctx, recepcion.ProyectoID, nuevoConsecutivo, recepcion.UsuarioModifica.Value);

                    ctx.SaveChanges();
                } 
                
                ts.Complete();
            }
        }
    }
}
