using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using Mimo.Framework.Common;
using SAM.Entities;

namespace SAM.BusinessObjects.Workstatus
{
    /// <summary>
    /// Métodos de conveniencia para revisar si algún spool
    /// está confinado y/o en hold.
    /// 
    /// Usar sin abusar ya que cada método hace una consulta.
    /// </summary>
    public class RevisionHoldsBO
    {
        private static readonly object _mutex = new object();
        private static RevisionHoldsBO _instance;

        /// <summary>
        /// constructor privado para implementar el patrón del singleton
        /// </summary>
        private RevisionHoldsBO()
        {
        }

        /// <summary>
        /// crea una instancia de la clase RevisionHoldsBO.
        /// </summary>
        public static RevisionHoldsBO Instance
        {
            get
            {
                lock(_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new RevisionHoldsBO();
                    }                    
                }
                return _instance;
            }
        }

        /// <summary>
        /// Revisa en la BD para ver si el Spool se encuentra en hold, ya sea
        /// por ingeniería, calidad o confinamiento.
        /// </summary>
        /// <param name="spoolID">ID del spool</param>
        /// <returns>Verdadero si el spool está en hold o confinado</returns>
        public bool SpoolTieneHold( int spoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return
                    SpoolTieneHold(ctx, spoolID);
            }
        }

        /// <summary>
        /// Revisa en la BD para ver si el Spool se encuentra en hold, ya sea
        /// por ingeniería, calidad o confinamiento.
        /// </summary>
        /// <param name="ctx">Contexto para ejecutar las consultas</param>
        /// <param name="spoolID">ID del spool</param>
        /// <returns>Verdadero si el spool está en hold o confinado</returns>
        public bool SpoolTieneHold(SamContext ctx, int spoolID)
        {
            return
                ctx.SpoolHold
                   .Where(x => x.SpoolID == spoolID)
                   .Any(x => x.TieneHoldCalidad || x.TieneHoldIngenieria || x.Confinado);
        }


        /// <summary>
        /// Revisa en la BD para ver si alguno de los spools se encuentra en hold, ya sea
        /// por ingeniería, calidad o confinamiento.
        /// </summary>
        /// <param name="ctx">Contexto para ejecutar las consultas</param>
        /// <param name="spoolIds">Arreglo con ids de spools a revisar</param>
        /// <returns>Verdadero si al menos uno de los spools está en hold o confinado</returns>
        public bool AlgunSpoolTieneHold(SamContext ctx, int[] spoolIds)
        {
            return
                ctx.SpoolHold
                   .Where(Expressions.BuildOrExpression<SpoolHold,int>(x => x.SpoolID, spoolIds))
                   .Any(x => x.TieneHoldCalidad || x.TieneHoldIngenieria || x.Confinado);
        }

        /// <summary>
        /// Revisa en la BD para ver si el Spool al cual pertenece la junta se encuentra en hold, ya sea
        /// por ingeniería, calidad o confinamiento.
        /// </summary>
        /// <param name="ctx">Contexto para ejecutar las consultas</param>
        /// <param name="juntaSpoolID">ID de la junta de la tabla JuntaSpool</param>
        /// <returns>Verdadero si el spool al cual pertenece la junta está en hold o confinado</returns>
        public bool JuntaSpoolTieneHold(SamContext ctx, int juntaSpoolID)
        {
            return 
                ctx.SpoolHold
                   .Where(x => ctx.JuntaSpool
                                  .Where(y => y.JuntaSpoolID == juntaSpoolID)
                                  .Select(z => z.SpoolID)
                                  .Contains(x.SpoolID))
                   .Any(x => x.TieneHoldCalidad || x.TieneHoldIngenieria || x.Confinado);
        }

        /// <summary>
        /// Revisa en la BD para ver si el spool al cual pertenece cualquiera de las juntas
        /// se encuentra en hold, ya sea por ingeniería, calidad o confinamiento. 
        /// 
        /// Se pueden pasar IDs de juntas pertenecientes a distintos spools.
        /// </summary>
        /// <param name="ctx">Contexto para ejecutar las consultas</param>
        /// <param name="juntaSpoolIds">Arreglo con los ids de las juntas spool de la tabla JuntaSpool</param>
        /// <returns>Verdadero si al menos una de las juntas pertenece a un spool con hold o confinamiento</returns>
        public bool AlgunaJuntaSpoolTieneHold(SamContext ctx, int [] juntaSpoolIds)
        {
            IQueryable<int> spoolIds =
                ctx.JuntaSpool
                   .Where(Expressions.BuildOrExpression<JuntaSpool, int>(y => y.JuntaSpoolID, juntaSpoolIds))
                   .Select(z => z.SpoolID);

            return
                ctx.SpoolHold
                   .Where(x => spoolIds.Contains(x.SpoolID))
                   .Any(x => x.TieneHoldCalidad || x.TieneHoldIngenieria || x.Confinado);
        }

        /// <summary>
        /// Revisa en la BD para ver si el spool al cual pertenece el material se encuentra en hold, ya sea
        /// por ingeniería, calidad o confinamiento.
        /// </summary>
        /// <param name="ctx">Contexto para ejecutar las consultas</param>
        /// <param name="materialSpoolID">ID del material de la tabla MaterialSpool</param>
        /// <returns>Verdadero si el spool al cual pertenece el material está en hold o confinado</returns>
        public bool MaterialSpoolTieneHold(SamContext ctx, int materialSpoolID)
        {
            return
                ctx.SpoolHold
                   .Where(x => ctx.MaterialSpool
                                  .Where(y => y.MaterialSpoolID == materialSpoolID)
                                  .Select(z => z.SpoolID)
                                  .Contains(x.SpoolID))
                   .Any(x => x.TieneHoldCalidad || x.TieneHoldIngenieria || x.Confinado);
        }


        /// <summary>
        /// Revisa en la BD para ver si el spool al cual pertenece cualquiera de los materiales
        /// se encuentra en hold, ya sea por ingeniería, calidad o confinamiento. 
        /// 
        /// Se pueden pasar IDs de materiales pertenecientes a distintos spools.
        /// </summary>
        /// <param name="ctx">Contexto para ejecutar las consultas</param>
        /// <param name="materialSpoolIds">Arreglo con los ids de los materiales spools de la tabla MaterialSpool</param>
        /// <returns>Verdadero si al menos uno de los materiales pertenece a un spool con hold o confinamiento</returns>
        public bool AlgunMaterialSpoolTieneHold(SamContext ctx, int[] materialSpoolIds)
        {
            IQueryable<int> spoolIds = 
                ctx.MaterialSpool
                   .Where(Expressions.BuildOrExpression<MaterialSpool, int>(y => y.MaterialSpoolID, materialSpoolIds))
                   .Select(z => z.SpoolID);

            return
                ctx.SpoolHold
                    .Where(x => spoolIds.Contains(x.SpoolID))
                    .Any(x => x.TieneHoldCalidad || x.TieneHoldIngenieria || x.Confinado);
        }

        /// <summary>
        /// Revisa en la BD para ver si el spool al cual pertenece la junta workstatus se encuentra en hold, ya sea
        /// por ingeniería, calidad o confinamiento.
        /// </summary>
        /// <param name="ctx">Contexto para ejecutar las consultas</param>
        /// <param name="juntaWorkstatusID">ID de la junta workstatus de la tabla JuntaWorkstatus</param>
        /// <returns>Verdadero si el spool al cual pertenece la junta workstatus está en hold o confinado</returns>
        public bool JuntaWorkstatusTieneHold(SamContext ctx, int juntaWorkstatusID)
        {
            return
                ctx.SpoolHold
                   .Where(x => ctx.JuntaWorkstatus
                                  .Where(y => y.JuntaWorkstatusID == juntaWorkstatusID)
                                  .Select(z => z.JuntaSpool.SpoolID)
                                  .Contains(x.SpoolID))
                   .Any(x => x.TieneHoldCalidad || x.TieneHoldIngenieria || x.Confinado);
        }


        /// <summary>
        /// Revisa en la BD para ver si el spool al cual pertenece cualquiera de las juntas workstatus
        /// se encuentra en hold, ya sea por ingeniería, calidad o confinamiento. 
        /// 
        /// Se pueden pasar IDs de juntas workstatus pertenecientes a distintos spools.
        /// </summary>
        /// <param name="ctx">Contexto para ejecutar las consultas</param>
        /// <param name="materialSpoolIds">Arreglo con los ids de las juntas workstatus de la tabla JuntaWorkstatus</param>
        /// <returns>Verdadero si al menos una de las juntas workstatus pertenece a un spool con hold o confinamiento</returns>
        public bool AlgunaJuntaWorkstatusTieneHold(SamContext ctx, int [] juntaWorkstatusID)
        {
            IQueryable<int> spoolIds = 
                ctx.JuntaWorkstatus
                   .Where(Expressions.BuildOrExpression<JuntaWorkstatus, int>(y => y.JuntaWorkstatusID, juntaWorkstatusID))
                   .Select(z => z.JuntaSpool.SpoolID);

            return
                ctx.SpoolHold
                   .Where(x => spoolIds.Contains(x.SpoolID))
                   .Any(x => x.TieneHoldCalidad || x.TieneHoldIngenieria || x.Confinado);
        }
    }
}
