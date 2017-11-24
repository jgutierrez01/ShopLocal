using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessLogic.Utilerias;

namespace SAM.BusinessLogic.Administracion
{
    public class PendienteBL
    {
        private static readonly object _mutex = new object();
        private static PendienteBL _instance;

        private PendienteBL()
        {
        }

        public static PendienteBL Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new PendienteBL();
                    }
                }
                return _instance;
            }
        }

        public void Guarda(Pendiente p, Guid responsable, string nombreProyecto, bool enviaMail)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ctx.Pendiente.ApplyChanges(p);
                    ctx.SaveChanges();

                    if (enviaMail)
                    {
                        EnvioCorreos.Instance.EnviaNotificacionDePendientes(responsable, nombreProyecto, p.Titulo, p.Descripcion);
                    }
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_Concurrencia);
            }
        }
    }
}
