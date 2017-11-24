using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Validations;

namespace SAM.BusinessObjects.Catalogos
{
    public class TipoPruebaBO
    {
        public event TableChangedHandler TipoPruebaCambio;
        private static readonly object _mutex = new object();
        private static TipoPruebaBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private TipoPruebaBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase TipoPruebaBO
        /// </summary>
        /// <returns></returns>
        public static TipoPruebaBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new TipoPruebaBO();
                    }
                }
                return _instance;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TipoPrueba> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.TipoPrueba.ToList();
            }
        }

        /// <summary>
        /// Guarda el tipo de prueba y expira el caché global.
        /// </summary>
        /// <param name="tipoPrueba">Entidad de tipo "TipoPrueba" que se desea guardar</param>
        public void Guarda(TipoPrueba tipoPrueba)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (ValidacionesTipoPrueba.NombreDuplicado(ctx, tipoPrueba.Nombre, tipoPrueba.TipoPruebaID))
                    {
                        throw new ExcepcionDuplicados(MensajesError.Excepcion_NomenclaturaDuplicada);
                    }

                    ctx.TipoPrueba.ApplyChanges(tipoPrueba);
                    ctx.SaveChanges();
                }

                if (TipoPruebaCambio != null)
                {
                    TipoPruebaCambio();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

    }
}
