using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Validations;

namespace SAM.BusinessObjects.Catalogos
{
    public class ProveedorBO
    {
        public event TableChangedHandler ProveedorCambio;
        private static readonly object _mutex = new object();
        private static ProveedorBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private ProveedorBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase ProveedorBO
        /// </summary>
        /// <returns></returns>
        public static ProveedorBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ProveedorBO();
                    }
                }
                return _instance;
            }
        }

        public Proveedor Obtener(int proveedorID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Proveedor.Where(x => x.ProveedorID == proveedorID).SingleOrDefault();
            }
        }

        public List<Proveedor> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Proveedor.OrderBy(x => x.Nombre).ToList();
            }
        }

        public List<Proveedor> ObtenerTodosConContacto()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Proveedor.Include("Contacto").ToList();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="proveedorID"></param>
        /// <returns></returns>
        public Proveedor ObtenerConContacto(int proveedorID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Proveedor.Include("Contacto")
                                    .Where(x => x.ProveedorID == proveedorID)
                                    .SingleOrDefault();
            }
        }
        /// <summary>
        /// Obtiene el listado de Proveedores por Proyecto
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public List<Proveedor> ObtenerPorProyecto(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                //Ejemplo de un join simple para filtrar proveedores por proyecto
                return (from t in ctx.Proveedor
                        join tp in ctx.ProveedorProyecto on t.ProveedorID equals tp.ProveedorID
                        where tp.ProyectoID == proyectoID
                        select t).ToList();
            }
        }

        public void Guarda(Proveedor proveedor)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (ValidacionesProveedor.NombreDuplicado(ctx, proveedor.Nombre, proveedor.ProveedorID))
                    {
                        throw new ExcepcionDuplicados(new List<string>() { MensajesError.Excepcion_NombreDuplicado });
                    }

                    ctx.Proveedor.ApplyChanges(proveedor);

                    ctx.SaveChanges();
                }

                if (ProveedorCambio != null)
                {
                    ProveedorCambio();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(new List<string>() { MensajesError.Excepcion_ErrorConcurrencia });
            }
        }

        public void Borra(int proveedorID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (ValidacionesProveedor.TieneProveedorProyecto(ctx,proveedorID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionProveedorProyecto);
                }

                if (ValidacionesProveedor.TieneNumeroUnico(ctx, proveedorID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionNumeroUnico);
                }
                
                Proveedor proveedor = ctx.Proveedor.Where(x => x.ProveedorID == proveedorID).Single();
                Contacto contacto = ctx.Contacto.Where(x => x.ContactoID == proveedor.ContactoID).SingleOrDefault();

                if (contacto != null)
                {
                    ctx.DeleteObject(contacto);
                }

                ctx.DeleteObject(proveedor);
                ctx.SaveChanges();

                if (ProveedorCambio != null)
                {
                    ProveedorCambio();
                }
            }

        }

      
    }
}
