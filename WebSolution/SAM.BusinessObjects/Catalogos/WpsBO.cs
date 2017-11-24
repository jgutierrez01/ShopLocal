using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Validations;
using SAM.Entities.Cache;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Grid;

namespace SAM.BusinessObjects.Catalogos
{
    public class WpsBO
    {
        public event TableChangedHandler WpsCambio;
        private static readonly object _mutex = new object();
        private static WpsBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private WpsBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase WpsBO
        /// </summary>
        /// <returns></returns>
        public static WpsBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new WpsBO();
                    }
                }
                return _instance;
            }
        }

        public Wps Obtener(int wpsID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Wps.Where(x => x.WpsID == wpsID).SingleOrDefault();
            }
        }

        /// <summary>
        /// obtiene un listado de Wps disponibles para un proyecto.
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public List<Wps> ObtenerPorProyecto(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return (from t in ctx.Wps
                        join tw in ctx.WpsProyecto on t.WpsID equals tw.WpsID
                        where tw.ProyectoID == proyectoID
                        select t).ToList();
            }
        }

        public List<Wps> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Wps.ToList();
            }
        }

        public Wps ObtenerConWpqs()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Wps.Include("Wpq").SingleOrDefault();
            }
        }

        public List<Wps> ObtenerTodosConRelaciones()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Wps.Include("FamiliaAcero")
                              .Include("FamiliaAcero1")
                              .Include("ProcesoRaiz")
                              .Include("ProcesoRelleno")
                              .OrderBy(x => x.Nombre)
                              .ToList();
            }
        }

        public void Guarda(Wps wps)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (ValidacionesWps.NombreDuplicado(ctx, wps.Nombre, wps.MaterialBase1ID, wps.MaterialBase2ID, wps.WpsID))
                    {
                        throw new ExcepcionDuplicados(new List<string>() { MensajesError.Excepcion_NombreDuplicado });
                    }

                    ctx.Wps.ApplyChanges(wps);

                    ctx.SaveChanges();
                }

                if (WpsCambio != null)
                {
                    WpsCambio();
                }

            }

            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(new List<string>() { MensajesError.Excepcion_ErrorConcurrencia });
            }
        }

        public void Borra(int wpsID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (Validations.ValidacionesWps.TieneWpq(ctx, wpsID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_WpsWpq);
                }                
                else if (Validations.ValidacionesWps.TieneJuntaSoldadura(ctx, wpsID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionJuntaSoldadura);
                }
                else if (Validations.ValidacionesWps.TieneWpsProyecto(ctx, wpsID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionWpsProyecto);
                }

                    Wps wps = ctx.Wps.Where(x => x.WpsID == wpsID).SingleOrDefault();
                    ctx.DeleteObject(wps);
                    ctx.SaveChanges();

                    if (WpsCambio != null)
                    {
                        WpsCambio();
                    }
                
            }
        }

        /// <summary>
        /// obtiene un listado de Wps disponibles para un proyecto del listado de cache
        /// </summary>
        /// <param name="proyectoID">ID proyecto</param>
        /// <returns>Listado WPSCache</returns>
        public IEnumerable<WpsCache> ObtenerCachePorProyecto(int proyectoID)
        {
            IEnumerable<WpsProyecto> wpsProyecto;
            using (SamContext ctx = new SamContext())
            {
                wpsProyecto = ctx.WpsProyecto.Where(x => x.ProyectoID == proyectoID).ToList();
            }

            return CacheCatalogos.Instance.ObtenerWps().Where(x => wpsProyecto.Select(y => y.WpsID).Contains(x.ID));

        }

        public IEnumerable<GrdWps> ObtenerWpsPorProyecto(int proyectoID)
        {
            IQueryable<WpsProyecto> wpsProyecto;
            using (SamContext ctx = new SamContext())
            {
                wpsProyecto = ctx.WpsProyecto.Where(x => x.ProyectoID == proyectoID);
                ctx.ProcesoRaiz.ToList();
                ctx.FamiliaAcero.ToList();
                ctx.ProcesoRelleno.ToList();
                //return ctx.Wps.Where(x => wpsProyecto.Select(y => y.WpsID).Contains(x.WpsID)).ToList();

                var lista = ctx.Wps.Where(x => wpsProyecto.Select(y => y.WpsID).Contains(x.WpsID)).ToList();
                var q = (from wps in lista
                        select new GrdWps
                                   {
                                       WpsID = wps.WpsID,
                                       Nombre = wps.Nombre,
                                       FamiliaAcero = wps.FamiliaAcero,
                                       FamiliaAcero1 = wps.FamiliaAcero1,
                                       ProcesoRaiz = wps.ProcesoRaiz,
                                       ProcesoRelleno = wps.ProcesoRelleno,
                                       EspesorRaizMaximo = wps.EspesorRaizMaximo,
                                       EspesorRellenoMaximo = wps.EspesorRellenoMaximo
                                   }).ToList();
                return q;
            }
        }
    }
}