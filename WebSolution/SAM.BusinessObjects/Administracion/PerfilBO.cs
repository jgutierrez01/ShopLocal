using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using Mimo.Framework.Data;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Validations;
using System.Transactions;

namespace SAM.BusinessObjects.Administracion
{
    public class PerfilBO
    {
        public event TableChangedHandler PerfilCambio;
        private static readonly object _mutex = new object();
        private static PerfilBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private PerfilBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase PerfilBO
        /// </summary>
        /// <returns></returns>
        public static PerfilBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new PerfilBO();
                    }
                }
                return _instance;
            }
        }

        public Perfil Obtener(int perfilID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Perfil.Where(x => x.PerfilID == perfilID).SingleOrDefault();
            }
        }

        public Perfil ObtenerConPermisos(int perfilID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Perfil.Include("PerfilPermiso").Where(x => x.PerfilID == perfilID).SingleOrDefault();
            }
        }

        public List<Perfil> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Perfil.ToList();
            }
        }

        public List<Perfil> ObtenerTodosConPermisos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Perfil.Include("PerfilPermiso").ToList();
            }
        }

        public void Guarda(Perfil perfil)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ctx.Perfil.ApplyChanges(perfil);
                    ctx.SaveChanges();
                }

                if (PerfilCambio != null)
                {
                    PerfilCambio();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(new List<string>() { "Error de concurrencia" });
            }
        }

        /// <summary>
        /// Borra el perfil y su relación con la tabla PerfilPermiso.
        /// Arroja una excepción en caso que el perfil tenga usuario asociados en cuyo caso no permitirá borrarlo.
        /// </summary>
        /// <param name="perfilID">ID del perfil a borrar</param>
        public void Borra(int perfilID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (ValidacionesPerfil.TieneUsuariosRelacionados(ctx, perfilID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_PerfilRelaciones);
                }

                //Traer las relaciones perfil/permiso
                List<PerfilPermiso> lstP = ctx.PerfilPermiso.Where(x => x.PerfilID == perfilID).ToList();

                //Borrar todas las relaciones con el permiso
                for (int i = 0; i < lstP.Count; i++)
                {
                    ctx.DeleteObject(lstP[i]);
                }

                //Se necesitan borrar las relaciones primero
                ctx.SaveChanges();

                //Traer el perfil
                Perfil perfil = ctx.Perfil.Where(x => x.PerfilID == perfilID).SingleOrDefault();

                //Borrar el perfil
                ctx.DeleteObject(perfil);
                ctx.SaveChanges();

                if (PerfilCambio != null)
                {
                    PerfilCambio();
                }
            }
        }
    }
}
