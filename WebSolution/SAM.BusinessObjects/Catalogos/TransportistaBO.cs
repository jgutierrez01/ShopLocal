using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using System.Data.Objects;
using SAM.BusinessObjects.Validations;

namespace SAM.BusinessObjects.Catalogos
{
    public class TransportistaBO
    {
        public event TableChangedHandler TransportistaCambio;
        private static readonly object _mutex = new object();
        private static TransportistaBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private TransportistaBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase TransportistaBO
        /// </summary>
        /// <returns></returns>
        public static TransportistaBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new TransportistaBO();
                    }
                }
                return _instance;
            }
        }

        public Transportista Obtener(int transportistaID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Transportista.Where(x => x.TransportistaID == transportistaID).SingleOrDefault();
            }
        }

        public List<Transportista> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Transportista.OrderBy(x => x.Nombre).ToList();
            }
        }

        public List<Transportista> ObtenerTodosConContacto()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Transportista.Include("Contacto").ToList();
            }
        }

        public List<Transportista> ObtenerPorProyecto(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                //Ejemplo de un join simple para filtrar transportistas por proyecto
                return (from t in ctx.Transportista
                        join tp in ctx.TransportistaProyecto on t.TransportistaID equals tp.TransportistaID
                        where tp.ProyectoID == proyectoID
                        select t).ToList();
            }
        }

        public Transportista ObtenerConContacto(int transportistaID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Transportista.Include("Contacto")
                                    .Where(x => x.TransportistaID == transportistaID)
                                    .SingleOrDefault();
            }
        }

        public void Guarda(Transportista transportista)
        {
            try
            {
               

                using (SamContext ctx = new SamContext())
                {
                    if (ValidacionesTransportista.NombreExiste(ctx, transportista.Nombre, transportista.TransportistaID))
                    {
                        throw new ExcepcionDuplicados(MensajesError.Excepcion_NombreDuplicado);
                    }

                    ctx.Transportista.ApplyChanges(transportista);

                    ctx.SaveChanges();
                }

                if ( TransportistaCambio != null )
                {
                    TransportistaCambio();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(new List<string>() { MensajesError.Excepcion_ErrorConcurrencia });
            }
        }

        public void Borra(int transportistaID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (ValidacionesTransportista.TieneTransportistaProyecto(ctx,transportistaID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionTransportistaProyecto);
                }

                if (ValidacionesTransportista.TieneRecepcion(ctx, transportistaID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionRecepcion);
                }

                Transportista transportista = ctx.Transportista.Where(x => x.TransportistaID == transportistaID).SingleOrDefault();
                Contacto contacto = ctx.Contacto.Where(x => x.ContactoID == transportista.ContactoID).SingleOrDefault();

                if (contacto != null)
                {
                    ctx.DeleteObject(contacto);
                }

                ctx.DeleteObject(transportista);
                ctx.SaveChanges();

                if (TransportistaCambio != null)
                {
                    TransportistaCambio();
                }
            }

        }
    }
}
