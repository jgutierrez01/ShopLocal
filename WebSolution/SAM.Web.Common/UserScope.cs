using System.Collections.Generic;
using System.Linq;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Cache;

namespace SAM.Web.Common
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

                    List<int> pids = (from p in MisProyectos select p.PatioID).ToList<int>();

                    List<PatioCache> lstPatio = (from patios in CacheCatalogos.Instance.ObtenerPatios()
                                                 where pids.Contains(patios.ID)
                                                 select patios).ToList();

                    return lstPatio;
                }

                //Si es administrador tiene permiso a todos los patios
                return CacheCatalogos.Instance.ObtenerPatios();
            }
        }

        public static List<TallerCache> MisTalleres
        {
            get
            {
                List<int> tids = (from t in MisPatios select t.ID).ToList<int>();

                List<TallerCache> lstTaller = (from talleres in CacheCatalogos.Instance.ObtenerTalleres()
                                                   where tids.Contains(talleres.ID)
                                                   select talleres).ToList();

                return lstTaller;
            }
            
        }

                
        public static List<TallerCache> TalleresPorProyecto(int proyectoID)
        {
            int patioID = MisProyectos.Where(x => x.ID == proyectoID)
                                      .Select(y => y.PatioID)
                                      .Single<int>();

            return TalleresPorPatio(patioID);
        }

        public static List<TallerCache> TalleresPorPatio(int patioID)
        {
            return MisPatios.Where(x => x.ID == patioID)
                            .SelectMany(x => x.Talleres)
                            .OrderBy(ta => ta.Nombre)
                            .ToList();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tallerID"></param>
        /// <returns></returns>
        /*
        public static List<CortadorCache> CortadorPorTalleres(int tallerID)
        {
            return MisPatios.Where(x => x.ID == tallerID)
                            .SelectMany(x => x.Talleres)
                            .OrderBy(ta => ta.Nombre)
                            .ToList();
        }
        */

        /*
        public static List<InspectorCache> InspectoresPorTaller(int tallerID)
        {
            return MisTalleres.Where(x => x.ID == tallerID)
                            .SelectMany(x => x.Nombre)
                            //.OrderBy(x => x.Nombre)
                            .ToList();
        }
        */


        public static List<ProyectoCache> ProyectosPorPatio(int patioID)
        {
            return MisProyectos.Where(x => x.PatioID == patioID).OrderBy(x => x.Nombre).ToList();
        }        
    }
}