using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAM.Entities.Cache;
using SAM.BusinessLogic;
using SAM.BusinessObjects.Utilerias;

namespace SAM.Web.Classes
{
    /// <summary>
    /// Intencionalmente en inglés, es demasiado técnico para estar en español.
    /// </summary>
    public static class UserScope
    {

        /// <summary>
        /// Regresa una lista con los proyectos a los que tiene permiso el usuario loggeado
        /// </summary>
        public static List<ProyectoCache> MisProyectos
        {
            get
            {
                if (!SessionFacade.EsAdministradorSistema)
                {
                    return (from pc in CacheCatalogos.Instance.ObtenerProyectos()
                            join pu in SessionFacade.ProyectosConPermiso on pc.ID equals pu
                            where pc.Activo
                            select pc).ToList();
                }

                //Si es administrador tiene permiso a todos los proyectos
                return CacheCatalogos.Instance.ObtenerProyectos();
            }
        }

        public static List<PatioCache> MisPatios
        {
            get
            {
                if (!SessionFacade.EsAdministradorSistema)
                {

                    List<int> pids = (from p in MisProyectos select p.PatioID).ToList();

                    List<PatioCache> lstPatio = (from patios in CacheCatalogos.Instance.ObtenerPatios()
                                                 where pids.Contains(patios.ID)
                                                 select patios).ToList();

                    return lstPatio;
                }

                //Si es administrador tiene permiso a todos los patios
                return CacheCatalogos.Instance.ObtenerPatios();
            }
        }


        public static List<TallerCache> TalleresPorProyecto(int proyectoID)
        {
            int patioID = MisProyectos.Where(x => x.ID == proyectoID)
                                      .Select(y => y.PatioID)
                                      .Single();

            return TalleresPorPatio(patioID);
        }

        public static List<TallerCache> TalleresPorPatio(int patioID)
        {
            return MisPatios.Where(x => x.ID == patioID)
                            .SelectMany(x => x.Talleres)
                            .OrderBy(ta => ta.Nombre)
                            .ToList();
        }


        public static List<ProyectoCache> ProyectosPorPatio(int patioID)
        {
            return MisProyectos.Where(x => x.PatioID == patioID).OrderBy(x => x.Nombre).ToList();
        }
    }
}