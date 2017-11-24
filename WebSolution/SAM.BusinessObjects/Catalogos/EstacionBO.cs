using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;

namespace SAM.BusinessObjects.Catalogos
{
    public class EstacionBO
    {
        private static readonly object _mutex = new object();
        private static EstacionBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private EstacionBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase AceroBO
        /// </summary>
        /// <returns></returns>
        public static EstacionBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new EstacionBO();
                    }
                }
                return _instance;
            }
        }

        public List<Estacion> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Estacion.Include("Taller").ToList();
            }
        }

        public List<Estacion> ObtenerPorTaller(int tallerID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Estacion.Include("Taller").Where(x => x.TallerID == tallerID).ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cedula"></param>
        public void Guarda(Estacion estacion)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    ctx.Estacion.ApplyChanges(estacion);
                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cedulaID"></param>
        public void Borra(int estacionID)
        {
            using (SamContext ctx = new SamContext())
            {
                Estacion estacion = ctx.Estacion.Where(x => x.EstacionID == estacionID).SingleOrDefault();
                bool tieneEstacion = Validations.ValidacionesEstacion.TieneEstacionRelaciones(ctx, estacion);

                if (!tieneEstacion)
                {

                    ctx.DeleteObject(estacion);
                    ctx.SaveChanges();
                }
                else
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionEstacion);
                }
            }
        }
    }
}
