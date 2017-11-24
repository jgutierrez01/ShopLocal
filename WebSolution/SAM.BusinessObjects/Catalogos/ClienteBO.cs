using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using SAM.Entities.Grid;


namespace SAM.BusinessObjects.Catalogos
{
    public class ClienteBO
    {
        public event TableChangedHandler ClienteCambio;
        private static readonly object _mutex = new object();
        private static ClienteBO _instance;

        private ClienteBO()
        {
        }

        public static ClienteBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ClienteBO();
                    }
                }
                return _instance;
            }
        }

        public Cliente Obtener(int clienteID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Cliente.Where(x => x.ClienteID == clienteID).SingleOrDefault();
            }
        }

        public List<Cliente> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Cliente.ToList();
            }
        }

        public List<GrdCliente> ObtenerTodosConContactoCliente()
        {
            using (SamContext ctx = new SamContext())
            {

                List<GrdCliente> lstC = (from cte in ctx.Cliente.Include("ContactoCliente").ToList()                                      
                                         let contacto = cte.ContactoCliente.FirstOrDefault()
                                      select new GrdCliente 
                                      {
                                        ClienteID = cte.ClienteID,
                                        NombreCliente = cte.Nombre,
                                        TelefonoOficina = contacto != null ? contacto.TelefonoOficina : string.Empty,
                                        CorreoElectronico = contacto != null ? contacto.CorreoElectronico : string.Empty,
                                        NombreContacto = contacto != null ? nombreCompleto(contacto) : string.Empty,
                                      }).ToList();
               

                return lstC;
            }
        }

        private static string nombreCompleto(ContactoCliente contacto)
        {
            return contacto.Nombre + " " + contacto.ApPaterno + (contacto.ApMaterno == null? string.Empty : " " + contacto.ApMaterno);
        }

        public Cliente ObtenerConContactoCliente(int clienteID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Cliente.Include("ContactoCliente").Where(x => x.ClienteID == clienteID)
                                 .SingleOrDefault();
            }
        }

        public void Guarda(Cliente cliente)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ctx.Cliente.ApplyChanges(cliente);

                    ctx.SaveChanges();
                }

                if (ClienteCambio != null)
                {
                    ClienteCambio();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        public void Borra(int clienteID)
        {
            using (SamContext ctx = new SamContext())
            {

                if (Validations.ValidacionesCliente.TieneProyecto(ctx, clienteID))
                {
                    throw new ExcepcionConcurrencia(MensajesError.Excepcion_RelacionProyecto);
                }

                List<ContactoCliente> contactos = ctx.ContactoCliente.Where(x => x.ClienteID == clienteID).ToList();
                contactos.ForEach(ctx.DeleteObject);

                Cliente cliente = ctx.Cliente.Where(x => x.ClienteID == clienteID).SingleOrDefault();
                ctx.DeleteObject(cliente);
                
                ctx.SaveChanges();

                if (ClienteCambio != null)
                {
                    ClienteCambio();
                }
            }
        }
    }
}
