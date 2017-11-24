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
using SAM.Entities.Personalizadas;

namespace SAM.BusinessObjects.Produccion
{
    public class TallerBO
    {
        private static readonly object _mutex = new object();
        private static TallerBO _instance;

        /// <summary>
        /// Obtiene la instancia de la clase CuadranteBO
        /// </summary>
        /// <returns></returns>
        public static TallerBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new TallerBO();
                    }
                }
                return _instance;
            }
        }

        public IEnumerable<Simple> ObtenerTalleresPorProyecto(int? _proyectoID, string buscar, int skip, int take)
        {
            List<Simple> cu;
            using (SamContext ctx = new SamContext())
            {
                cu = (from taller in ctx.Taller
                      join patio in ctx.Patio on taller.PatioID equals patio.PatioID
                      join proyecto in ctx.Proyecto on patio.PatioID equals proyecto.PatioID
                      where proyecto.ProyectoID == _proyectoID
                      select new Simple
                      {
                          ID = taller.TallerID,
                          Valor = taller.Nombre
                      }).ToList();
            }
            return cu.Where(x => x.Valor.StartsWith(buscar, StringComparison.InvariantCultureIgnoreCase))
                    .OrderBy(x => x.Valor)
                    .Skip(skip)
                    .Take(take);
        }

        public List<Taller> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Taller.ToList();
            }
        }

    }//Fin Clase
}
