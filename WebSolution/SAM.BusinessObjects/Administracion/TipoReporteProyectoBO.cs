using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.Entities.Grid;
using System.Threading;
using Mimo.Framework.Common;

namespace SAM.BusinessObjects.Administracion
{
    public class TipoReporteProyectoBO
    {
        private static string cultura
        {
            get
            {
                return Thread.CurrentThread.CurrentUICulture.Name;
            }
        }

        private static readonly object _mutex = new object();
        private static TipoReporteProyectoBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private TipoReporteProyectoBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase TipoReporteProyectoBO
        /// </summary>
        /// <returns></returns>
        public static TipoReporteProyectoBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new TipoReporteProyectoBO();
                    }
                }
                return _instance;
            }
        }

        public List<TipoReporteProyecto> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.TipoReporteProyecto.ToList();
            }
        }

        public List<GrdTipoReporte> ObtenerConProyectoReporte(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {

                IEnumerable<ProyectoReporte> pReporte = ctx.ProyectoReporte.Where(x => x.ProyectoID == proyectoID);
                List<GrdTipoReporte> lst = (from tpr in ctx.TipoReporteProyecto
                                                 join pr in pReporte on tpr.TipoReporteProyectoID equals pr.TipoReporteProyectoID                                                 
                                                 into ProyReporte
                                                 from proyR in ProyReporte.DefaultIfEmpty()
                                                 select new GrdTipoReporte{
                                                  TipoReporteProyectoID = tpr.TipoReporteProyectoID,
                                                  ProyectoReporteID = (proyR == null) ? -1 : proyR.ProyectoReporteID,
                                                  Nombre = tpr.Nombre,
                                                  NombreIngles = tpr.NombreIngles,
                                                  RutaEspaniol = (proyR == null) ? string.Empty : proyR.RutaEspaniol,
                                                  RutaIngles = (proyR == null) ? string.Empty : proyR.RutaIngles,
                                                 }).ToList();

                lst.ForEach(x => x.NombreInt = (cultura == LanguageHelper.INGLES ? x.NombreIngles : x.Nombre));
                return lst;

            }
        }

        public TipoReporteProyecto Obtener(int tipoReporteID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.TipoReporteProyecto.Where(x => x.TipoReporteProyectoID == tipoReporteID).Single();
            }
        }
    }
}
