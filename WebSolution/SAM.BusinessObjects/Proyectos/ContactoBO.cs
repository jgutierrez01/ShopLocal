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

namespace SAM.BusinessObjects.Proyectos
{
    public class ContactoBO
    {
        public event TableChangedHandler ContactoCambio;
        private static readonly object _mutex = new object();
        private static ContactoBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singleton
        /// </summary>
        private ContactoBO()
        { 
        }

        /// <summary>
        /// Obtiene la instancia de la clase ContactoBO
        /// </summary>
        public static ContactoBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ContactoBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Método que regresa un objeto de tipo contacto dependiendo del identificador 
        /// de contacto que se le envía.
        /// </summary>
        /// <param name="contactoID"></param>
        /// <returns></returns>
        public Contacto Obtener(int contactoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Contacto.Where(x => x.ContactoID == contactoID).SingleOrDefault();
            }
        }

        /// <summary>
        /// El método regresa todos los contactos que se encuentran en la base de datos
        /// y los mete a una lista serializable de contactos.
        /// </summary>
        /// <returns></returns>
        public List<Contacto> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Contacto.ToList();
            }
        }

        /// <summary>
        /// Guarda un contacto... si el contacto estaba en la base de datos regresa una excepcion
        /// de duplicados... si no está, se guarda el contacto nuevo en la base de datos.
        /// </summary>
        /// <param name="contacto"></param>
        public void Guarda(Contacto contacto)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (ValidacionesContacto.ContactoDuplicado(ctx, contacto.Nombre))
                    {
                        throw new ExcepcionDuplicados(new List<string>() { MensajesError.Excepcion_NombreDuplicado });
                    }

                    ctx.Contacto.ApplyChanges(contacto);

                    ctx.SaveChanges();
                }

                if (ContactoCambio != null)
                {
                    ContactoCambio();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(new List<string>() { MensajesError.Excepcion_ErrorConcurrencia });
            }
        }

        /// <summary>
        /// permite eliminar un renglon de la tabla de contactos dependiendo del 
        /// identificador de contacto. 
        /// regresa una excepcion en caso de estar intentando borrar un contacto ya eliminado 
        /// anteriormente.
        /// </summary>
        /// <param name="contactoID"></param>
        public void Borra(int contactoID)
        {
            Contacto contacto = Obtener(contactoID);
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ctx.Contacto.DeleteObject(contacto);

                    ctx.SaveChanges();
                }
                if (ContactoCambio != null)
                {
                    ContactoCambio();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(new List<string>() {MensajesError.Excepcion_ErrorConcurrencia });

            }
        }
    }
}
