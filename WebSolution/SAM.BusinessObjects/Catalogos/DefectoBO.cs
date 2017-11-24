using System.Collections.Generic;
using System.Linq;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using SAM.Entities.Personalizadas;

namespace SAM.BusinessObjects.Catalogos
{
    public class DefectoBO
    {
        public event TableChangedHandler DefectoCambio;
        private static readonly object _mutex = new object();
        private static DefectoBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private DefectoBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase DefectoBO
        /// </summary>
        /// <returns></returns>
        public static DefectoBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new DefectoBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defectoID"></param>
        /// <returns></returns>
        public Defecto Obtener(int defectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Defecto.Where(x => x.DefectoID == defectoID).SingleOrDefault();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Defecto> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Defecto.ToList();
            }
        }

        public List<Defecto> ObtenerTodosConTipoPrueba()
        {
            using (SamContext ctx = new SamContext())
            {
                List<Defecto> list = ctx.Defecto.Include("TipoPrueba")
                                             .OrderBy(x => x.Nombre)
                                             .ToList();
                return list;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="defecto"></param>
        public void Guarda(Defecto defecto)
        {
            try
            {
                if (DefectoExiste(defecto.Nombre, defecto.DefectoID))
                {
                    throw new ExcepcionDuplicados(MensajesError.Excepcion_DefectoDuplicado);
                }

                using (SamContext ctx = new SamContext())
                {

                    ctx.Defecto.ApplyChanges(defecto);
                    ctx.SaveChanges();
                }

                if (DefectoCambio != null)
                {
                    DefectoCambio();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defectoID"></param>
        /// <returns></returns>
        public bool DefectoExiste(string value, int? defectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (defectoID.HasValue)
                {// se esta editando
                    return ctx.Defecto.Any(x => x.DefectoID != defectoID.Value && x.Nombre == value);
                }
                else
                {//es nuevo defecto
                    return ctx.Defecto.Any(x => x.Nombre.Equals(value));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defectoID"></param>
        public void Borra(int defectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                Defecto defecto = ctx.Defecto.Where(x => x.DefectoID == defectoID).SingleOrDefault();
                bool tieneDefecto = Validations.ValidacionesDefecto.TieneDefectoRelaciones(ctx, defecto);

                if (!tieneDefecto)
                {
                    ctx.DeleteObject(defecto);
                    ctx.SaveChanges();

                    if (DefectoCambio != null)
                    {
                        DefectoCambio();
                    }
                }
                else
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionDefecto);
                }
            }
        }

        /// <summary>
        /// Obtiene una lista simple de Defectos filtrados para determinada prueba
        /// </summary>
        /// <param name="tipoPruebaID">Tipo de Prueba ID</param>
        /// <returns></returns>
        public List<Simple> ObtenerDefectosPorTipoDePruebaID(int tipoPruebaID)
        {
            List<Simple> lstDefectos;
            using (SamContext ctx = new SamContext())
            {
                lstDefectos =
                    (from df in ctx.Defecto
                     where df.TipoPruebaID == tipoPruebaID
                     select new Simple
                     {
                         ID = df.DefectoID,
                         Valor = df.Nombre
                     }).ToList();
                return lstDefectos;
            }
        }
    }
}
