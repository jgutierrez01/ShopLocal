using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Cache;
using System.Data;
using SAM.BusinessObjects.Excepciones;

namespace SAM.BusinessObjects.Administracion
{
    public class PendienteBO
    {
        private static readonly object _mutex = new object();
        private static PendienteBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private PendienteBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase PendienteBO
        /// </summary>
        /// <returns></returns>
        public static PendienteBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new PendienteBO();
                    }
                }
                return _instance;
            }
        }

        public List<GrdPendientes> ObtenerPendientes(int proyectoID, int patioID, bool todos)
        {
            //(todos) es para traer todos los estatus sino solo los abiertos
            using (SamContext ctx = new SamContext())
            {

                IQueryable<Proyecto> iqProyecto = null;
                IQueryable<Pendiente> iqPendientes = null;
                IQueryable<Usuario> iqAutor = null;
                IQueryable<Usuario> iqResponsable = null;
                Dictionary<int, string> area = CacheCatalogos.Instance.ObtenerCategoriaPendiente().ToDictionary(x => x.ID, y => y.Nombre);

                if (patioID > 0 && proyectoID > 0)
                {
                    iqProyecto = ctx.Proyecto.Where(x => x.ProyectoID == proyectoID);
                    iqPendientes = ctx.Pendiente.Where(x => x.ProyectoID == proyectoID);
                    iqAutor = ctx.Usuario.Where(x => iqPendientes.Select(y => y.GeneradoPor).Contains(x.UserId));
                    iqResponsable = ctx.Usuario.Where(x => iqPendientes.Select(y => y.AsignadoA).Contains(x.UserId));
                }

                if (patioID > 0 && proyectoID <= 0)
                {
                    iqProyecto = ctx.Proyecto.Where(x => x.PatioID == patioID);
                    iqPendientes = ctx.Pendiente.Where(x => iqProyecto.Select(y => y.ProyectoID).Contains(x.ProyectoID));
                    iqAutor = ctx.Usuario.Where(x => iqPendientes.Select(y => y.GeneradoPor).Contains(x.UserId));
                    iqResponsable = ctx.Usuario.Where(x => iqPendientes.Select(y => y.AsignadoA).Contains(x.UserId));
                }

                if (proyectoID > 0 && patioID <= 0)
                {
                    iqProyecto = ctx.Proyecto.Where(x => x.ProyectoID == proyectoID);
                    iqPendientes = ctx.Pendiente.Where(x => x.ProyectoID == proyectoID);
                    iqAutor = ctx.Usuario.Where(x => iqPendientes.Select(y => y.GeneradoPor).Contains(x.UserId));
                    iqResponsable = ctx.Usuario.Where(x => iqPendientes.Select(y => y.AsignadoA).Contains(x.UserId));
                }

                List<GrdPendientes> lstPendientes = (from pendiente in iqPendientes.ToList()
                                                     join proyecto in iqProyecto.ToList() on pendiente.ProyectoID equals proyecto.ProyectoID
                                                     join autor in iqAutor.ToList() on pendiente.GeneradoPor equals autor.UserId
                                                     join responsable in iqResponsable.ToList() on pendiente.AsignadoA equals responsable.UserId

                                                     select new GrdPendientes
                                                     {
                                                         PendienteID = pendiente.PendienteID,
                                                         CategoriaPendienteID = pendiente.CategoriaPendienteID,
                                                         NombreProyecto = proyecto.Nombre,
                                                         Titulo = pendiente.Titulo,
                                                         Estatus = pendiente.Estatus,
                                                         Autor = autor.Nombre + ' ' + autor.ApPaterno + ' ' + autor.ApMaterno,
                                                         Responsable = responsable.Nombre + ' ' + responsable.ApPaterno + ' ' + responsable.ApMaterno,
                                                         FechaModificacion = pendiente.FechaModificacion
                                                     }).OrderByDescending(x => x.FechaModificacion).ToList();

                lstPendientes.ForEach(x => x.Area = area[x.CategoriaPendienteID]);

                if (!todos)
                {
                    lstPendientes = lstPendientes.Where(x => x.Estatus == "A").ToList();
                }

                return lstPendientes;
            }
        }

        public Pendiente ObtenerPendientePorID(int pendienteID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Pendiente.Include("PendienteDetalle")
                                    .Include("Proyecto")
                                    .Include("UsuarioResponsable")
                                    .Where(x => x.PendienteID == pendienteID).SingleOrDefault();
            }
        }

        public List<Pendiente> ObtenerPendientesActivosPorUsuarioID(Guid usuarioID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Pendiente.Where(x => x.AsignadoA == usuarioID && x.Estatus == EstatusPendiente.Abierto).ToList();
            }
        }
    }
}
