using System.Collections.Generic;
using System.Linq;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Validations;

namespace SAM.BusinessObjects.Catalogos
{
    public class TipoJuntaBO
    {
        public event TableChangedHandler TipoJuntaCambio;
        private static readonly object _mutex = new object();
        private static TipoJuntaBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private TipoJuntaBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase TipoJuntaBO
        /// </summary>
        /// <returns></returns>
        public static TipoJuntaBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new TipoJuntaBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoJuntaID"></param>
        /// <returns></returns>
        public TipoJunta Obtener(int tipoJuntaID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.TipoJunta.Where(x => x.TipoJuntaID == tipoJuntaID).SingleOrDefault();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TipoJunta> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.TipoJunta.ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoJunta"></param>
        public void Guarda(TipoJunta tipoJunta)
        {
            try
            {
                if (TipoJuntaBO.Instance.TipoJuntaExiste(tipoJunta.Codigo, tipoJunta.TipoJuntaID))
                {
                    throw new ExcepcionDuplicados(MensajesError.Excepcion_TipoJuntaDuplicado);
                }

                using (SamContext ctx = new SamContext())
                {

                    ctx.TipoJunta.ApplyChanges(tipoJunta);
                    ctx.SaveChanges();
                }

                if (TipoJuntaCambio != null)
                {
                    TipoJuntaCambio();
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
        /// <param name="tipoJuntaID"></param>
        /// <returns></returns>
        public bool TipoJuntaExiste(string value, int? tipoJuntaID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (tipoJuntaID.HasValue)
                {// se esta editando
                    return ctx.TipoJunta.Any(x => x.TipoJuntaID != tipoJuntaID.Value && x.Codigo == value);
                }
                else
                {//es nuevo tipoJunta
                    return ctx.TipoJunta.Any(x => x.Codigo == value);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoJuntaID"></param>
        public void Borra(int tipoJuntaID)
        {
            using (SamContext ctx = new SamContext())
            {
                TipoJunta tipoJunta = ctx.TipoJunta.Where(x => x.TipoJuntaID == tipoJuntaID).SingleOrDefault();
                bool tieneTipoJunta = ValidacionesTipoJunta.TieneTipoJuntaRelaciones(ctx, tipoJunta);

                if (!tieneTipoJunta)
                {
                    ctx.DeleteObject(tipoJunta);
                    ctx.SaveChanges();

                    if (TipoJuntaCambio != null)
                    {
                        TipoJuntaCambio();
                    }
                }
                else
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_TipoJuntaTieneRelaciones);
                }
            }
        }
    }
}
