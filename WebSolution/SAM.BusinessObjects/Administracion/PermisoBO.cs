using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;

namespace SAM.BusinessObjects.Administracion
{
    public class PermisoBO
    {
        private static readonly object _mutex = new object();
        private static PermisoBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private PermisoBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase PermisoBO
        /// </summary>
        /// <returns></returns>
        public static PermisoBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new PermisoBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Permiso> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Permiso.ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Permiso> ObtenerTodosConPaginas()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Permiso.Include("Pagina").ToList();
            }
        }

        public bool TienePermisoUrl(string url, Guid userId)
        {
            using (SamContext ctx = new SamContext())
            {
                bool permitir = (from pagina in ctx.Pagina
                                 join permiso in ctx.Permiso on pagina.PermisoID equals permiso.PermisoID
                                 join perfiles in ctx.PerfilPermiso on permiso.PermisoID equals perfiles.PermisoID
                                 join usuario in ctx.Usuario on perfiles.PerfilID equals usuario.PerfilID
                                 where (pagina.Url == url && usuario.UserId == userId)
                                 || usuario.EsAdministradorSistema
                                 select usuario).Any();

                return permitir;
            }
        }
    }
}
