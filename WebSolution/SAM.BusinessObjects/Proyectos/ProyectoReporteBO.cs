using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;

namespace SAM.BusinessObjects.Proyectos
{
    public class ProyectoReporteBO
    {
        public event TableChangedHandler ProyectoCambio;
        private static readonly object _mutex = new object();
        private static ProyectoReporteBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private ProyectoReporteBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase ProyectoReporteBO
        /// </summary>
        /// <returns></returns>
        public static ProyectoReporteBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ProyectoReporteBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ProyectoReporte> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ProyectoReporte.ToList();
            }
        }

        /// <summary>
        /// Retorna una lide ProyectoReporte por proyecto
        /// </summary>
        /// <param name="proyectoID">ProyectoReporte.ProyectoID</param>
        /// <returns></returns>
        public List<ProyectoReporte> ObtenerListaPorProyecto(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ProyectoReporte.Where(x => x.ProyectoID == proyectoID).ToList();
            }
        }

        public void Guarda(List<ProyectoReporte> p)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    foreach (ProyectoReporte pr in p)
                    {
                        ctx.ProyectoReporte.ApplyChanges(pr);
                    }

                    ctx.SaveChanges();

                    if (ProyectoCambio != null)
                    {
                        ProyectoCambio();
                    }
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        public void Borra(List<int> Ids)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ProyectoReporte p;

                    foreach (int iD in Ids)
                    {
                        p = ctx.ProyectoReporte.Where(x => x.ProyectoReporteID == iD).SingleOrDefault();

                        if (p != null)
                        {
                            ctx.ProyectoReporte.DeleteObject(p);
                        }
                    }

                    ctx.SaveChanges();

                    if (ProyectoCambio != null)
                    {
                        ProyectoCambio();
                    }
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        public ProyectoReporte Obtener(int proyectoReporteID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ProyectoReporte.Where(x => x.ProyectoReporteID == proyectoReporteID).Single();
            }
        }
    }
}
