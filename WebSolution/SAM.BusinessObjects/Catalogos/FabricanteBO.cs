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
    public class FabricanteBO
    {
        public event TableChangedHandler FabricanteCambio;
        private static readonly object _mutex = new object();
        private static FabricanteBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private FabricanteBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase FabricanteBO
        /// </summary>
        /// <returns></returns>
        public static FabricanteBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new FabricanteBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// obtiene un fabricante a partir del ID que se busca.
        /// </summary>
        /// <param name="fabricanteID"></param>
        /// <returns></returns>
        public Fabricante Obtener(int fabricanteID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Fabricante.Where(x => x.FabricanteID == fabricanteID).SingleOrDefault();
            }
        }

        /// <summary>
        /// regresa la lista de todos los fabricantes disponibles
        /// </summary>
        /// <returns></returns>
        public List<Fabricante> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Fabricante.OrderBy(x => x.Nombre).ToList();
            }
        }

        /// <summary>
        /// obtiene una lista de fabricantes incluyendo la entidad contacto para cada fabricante.
        /// </summary>
        /// <returns></returns>
        public List<Fabricante> ObtenerTodosConContacto()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Fabricante.Include("Contacto").ToList();
            }
        }

       /// <summary>
       /// obtiene una entidad fabricante junto con las entidades de contacto
       /// relacionadas a ese fabricante.
       /// </summary>
       /// <param name="fabricanteID"></param>
       /// <returns></returns>
        public Fabricante ObtenerConContacto(int fabricanteID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Fabricante.Include("Contacto")
                                    .Where(x => x.FabricanteID == fabricanteID)
                                    .SingleOrDefault();
            }
        }

        /// <summary>
        /// obtiene un listado de fabricantes con el identificador del proyecto.
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public List<Fabricante> ObtenerPorProyecto(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return (from t in ctx.Fabricante
                        join tf in ctx.FabricanteProyecto on t.FabricanteID equals tf.FabricanteID
                        where tf.ProyectoID == proyectoID
                        select t).ToList();
            }
        }

        public void Guarda(Fabricante fabricante)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (ValidacionesFabricante.NombreDuplicado(ctx, fabricante.Nombre, fabricante.FabricanteID))
                    {
                        throw new ExcepcionDuplicados(new List<string>() { MensajesError.Excepcion_NombreFabDuplicado });
                    }
                    ctx.Fabricante.ApplyChanges(fabricante);

                    ctx.SaveChanges();
                }

                if ( FabricanteCambio != null )
                {
                    FabricanteCambio();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(new List<string>() { MensajesError.Excepcion_ErrorConcurrencia });
            }
        }

        public void Borra(int fabricanteID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (ValidacionesFabricante.TieneFabricanteProyecto(ctx, fabricanteID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionFabricanteProyecto);
                }
                if (ValidacionesFabricante.TieneColada(ctx, fabricanteID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionColada);
                }
                if (ValidacionesFabricante.TieneNumeroUniso(ctx, fabricanteID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionNumeroUnico);
                }

                Fabricante fabricante = ctx.Fabricante.Where(x => x.FabricanteID == fabricanteID).Single();
                Contacto contacto = ctx.Contacto.Where(x => x.ContactoID == fabricante.ContactoID).SingleOrDefault();

                if (contacto != null)
                {
                    ctx.DeleteObject(contacto);
                }

                ctx.DeleteObject(fabricante);
                ctx.SaveChanges();

                if (FabricanteCambio != null)
                {
                    FabricanteCambio();
                }
            }
        }

       
    }
}
