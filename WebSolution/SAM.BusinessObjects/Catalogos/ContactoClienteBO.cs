using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;

namespace SAM.BusinessObjects.Catalogos
{
    public class ContactoClienteBO
    {
         public event TableChangedHandler ContactoClienteCambio;
        private static readonly object _mutex = new object();
        private static ContactoClienteBO _instance;

        private ContactoClienteBO()
        {
        }

        public static ContactoClienteBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ContactoClienteBO();
                    }
                }
                return _instance;
            }
        }

        public ContactoCliente Obtener(int contactoClienteID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ContactoCliente.Where(x => x.ContactoClienteID == contactoClienteID).SingleOrDefault();
            }
        }

        public List<ContactoCliente> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ContactoCliente.ToList();
            }
        }

        public List<ContactoCliente> ObtenerPorCliente(int clienteID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ContactoCliente.Where(x => x.ClienteID == clienteID).ToList();
            }
        }

        public void Guarda(ContactoCliente contactoCliente)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ctx.ContactoCliente.ApplyChanges(contactoCliente);

                    ctx.SaveChanges();
                }

                if (ContactoClienteCambio != null)
                {
                    ContactoClienteCambio();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(new List<string>() { MensajesError.Excepcion_ErrorConcurrencia });
            }
        }

        public void Borra(int contactoClienteID)
        {
            using (SamContext ctx = new SamContext())
            {
                ContactoCliente contactoCliente = ctx.ContactoCliente.Where(x => x.ContactoClienteID == contactoClienteID).SingleOrDefault();
                ctx.DeleteObject(contactoCliente);
                ctx.SaveChanges();

                if (ContactoClienteCambio != null)
                {
                    ContactoClienteCambio();
                }
            }
        }
    }
}
