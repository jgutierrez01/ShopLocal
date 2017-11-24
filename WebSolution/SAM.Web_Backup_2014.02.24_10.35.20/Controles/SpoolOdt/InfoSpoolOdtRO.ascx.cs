using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.WebControls;
using Mimo.Framework.Extensions;
using SAM.Entities.Personalizadas;
using SAM.BusinessLogic;
using SAM.Entities.Cache;
using Mimo.Framework.Common;
using SAM.BusinessObjects.Utilerias;

namespace SAM.Web.Controles.SpoolOdt
{
    public partial class InfoSpoolOdtRO : UserControl, IMappable
    {
        public void Map(object entity)
        {
            Controls.IterateRecursivelyStopOnIMappableAndUserControl(c =>
            {
                if (c is IMappableField)
                {
                    ((IMappableField)c).Map(entity);
                }
            });

            DetSpoolOdt spool = (DetSpoolOdt)entity;
            
            //De un proyecto en particular traerme su nomenclatura para el nombre de las columnas
            List<NomenclaturaStruct> nomenclatura = CacheCatalogos.Instance
                                                                  .ObtenerProyectos()
                                                                  .Where(x => x.ID == spool.ProyectoID)
                                                                  .SelectMany(y => y.Nomenclatura)
                                                                  .OrderBy(z => z.Orden)
                                                                  .ToList();

            //Esto terminó estand padre, pero me cuestiono si valió la pena en 
            //lugar de haberlo hecho de uno en uno, al fin nada más son 7
            repSegmentos.DataSource = from nom in obtenNomenclatura(nomenclatura, spool)
                                      select new
                                      {
                                          NombreColumna = nom.First.ToString(),
                                          ValorColumna = nom.Second != null ? nom.Second.ToString() : string.Empty
                                      };
            repSegmentos.DataBind();
        }

        /// <summary>
        /// Regresa un IEnumerable con los pares del nombre de la columna del spool
        /// así como su valor.
        /// </summary>
        /// <param name="nomenclatura">Nomenclatura especificada por proyecto para el spool</param>
        /// <param name="spool">Spool en particular</param>
        /// <returns>Una lista con la relación entre la nomenclatura del proyecto y el valor per-se de esa columna en el spool</returns>
        private static IEnumerable<Pair> obtenNomenclatura(List<NomenclaturaStruct> nomenclatura, DetSpoolOdt spool)
        {
            for (int i = 0; i < nomenclatura.Count; i++)
            {
                yield return new Pair
                {
                    First = nomenclatura[i].NombreColumna,
                    Second = ReflectionUtils.GetStringValue(spool, "Segmento"+(i+1))
                };
            }
        }

        public void Unmap(object entity)
        {
            //no es necesario
        }
    }
}