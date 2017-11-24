using System;
using System.Collections.Generic;
using System.Linq;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;

namespace SAM.BusinessObjects.Catalogos
{
    public class TipoCorteBO
    {
        public event TableChangedHandler TipoCorteCambio;
        private static readonly object _mutex = new object();
        private static TipoCorteBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private TipoCorteBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase TipoCorteBO
        /// </summary>
        /// <returns></returns>
        public static TipoCorteBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new TipoCorteBO();
                    }
                }
                return _instance;
            }
        }

        // Obtencion TipoCorte Semejante
        public TipoCorte Obtener(int tipoCorteID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.TipoCorte.Where(x => x.TipoCorteID == tipoCorteID).SingleOrDefault();
            }
        }

        public List<TipoCorte> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.TipoCorte.ToList();
            }
        }

        public void Guarda(TipoCorte tipoCorte)
        {
            try
            {
                if (TipoCorteBO.Instance.TipoCorteExiste(tipoCorte.Codigo, tipoCorte.TipoCorteID))
                {
                    throw new ExcepcionDuplicados(MensajesError.Excepcion_TipoCorteDuplicado);
                }

                using (SamContext ctx = new SamContext())
                {

                    ctx.TipoCorte.ApplyChanges(tipoCorte);
                    ctx.SaveChanges();
                }

                if (TipoCorteCambio != null)
                {
                    TipoCorteCambio();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        public bool TipoCorteExiste(string codigo, int? tipoCorteID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (tipoCorteID.HasValue)
                {   // se esta editando
                    return ctx.TipoCorte.Any(x => x.TipoCorteID != tipoCorteID.Value && x.Codigo == codigo);
                }
                else
                {   //es nuevo tipoCorte
                    return ctx.TipoCorte.Any(x => x.Codigo == codigo);
                }
            }
        }

        public void Borra(int tipoCorteID)
        {
            using (SamContext ctx = new SamContext())
            {
                TipoCorte tipoCorte = ctx.TipoCorte.Where(x => x.TipoCorteID == tipoCorteID).SingleOrDefault();
                bool tieneTipoCorte = Validations.ValidacionesTipoCorte.TieneTipoCorteRelaciones(ctx, tipoCorte);

                if (!tieneTipoCorte)
                {

                    ctx.DeleteObject(tipoCorte);
                    ctx.SaveChanges();

                    if (TipoCorteCambio != null)
                    {
                        TipoCorteCambio();
                    }
                }
                else
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_CodigoDuplicado);  //.Excepcion_RelacionTipoCorte
                }
            }
        }


        public bool TipoCorteExiste(decimal p, int? tipoCorteID)
        {
            throw new NotImplementedException();
        }
    }
}
