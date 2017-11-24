using System.Collections.Generic;
using System.Data;
using System.Linq;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;

namespace SAM.BusinessObjects.Catalogos
{
    public class DiametroBO
    {

        public event TableChangedHandler DiametroCambio;
        private static readonly object _mutex = new object();
        private static DiametroBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private DiametroBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase ItemCodeBO
        /// </summary>
        /// <returns></returns>
        public static DiametroBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new DiametroBO();
                    }
                }
                return _instance;
            }
        }

        // Obtencion Diametro Semejante
        public Diametro Obtener(int diametroID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Diametro.Where(x => x.DiametroID == diametroID).SingleOrDefault();
            }
        }

        public List<Diametro> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Diametro.ToList();
            }
        }

        public Diametro ObtenerPorValor(decimal valor)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Diametro.Where(x => x.Valor == valor).SingleOrDefault();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="diametroID"></param>
        /// <returns></returns>
        public bool DiametroExiste(decimal valor, int? diametroID)
        {
            using (SamContext ctx = new SamContext())
            {
                if(diametroID.HasValue && diametroID.Value > 0)
                {
                    // se esta editando
                    return ctx.Diametro.Any(x => x.DiametroID != diametroID.Value && x.Valor == valor);
                }
                else
                {
                    // es nuevo diametro
                    return ctx.Diametro.Any(x => x.Valor == valor);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diametro"></param>
        public void Guarda(Diametro diametro)
        {
            try
            {
                if (DiametroExiste(diametro.Valor, diametro.DiametroID))
                {
                    throw new ExcepcionDuplicados(MensajesError.Excepcion_DiametroDuplicado);
                }

                using (SamContext ctx = new SamContext())
                {
                    
                    ctx.Diametro.ApplyChanges(diametro);
                    ctx.SaveChanges();
                }

                if (DiametroCambio != null)
                {
                    DiametroCambio();
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
        /// <param name="diametroID"></param>
        public void Borra(int diametroID)
        {
            using (SamContext ctx = new SamContext())
            {
                Diametro diametro = ctx.Diametro.Where(x => x.DiametroID == diametroID).SingleOrDefault();
                bool tieneDiametro = Validations.ValidacionesDiametro.TieneDiametroRelaciones(ctx, diametro);

                if (!tieneDiametro)
                {
                    
                    ctx.DeleteObject(diametro);
                    ctx.SaveChanges();

                    if (DiametroCambio != null)
                    {
                        DiametroCambio();
                    }
                }
                else
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_NombreDuplicado);  //.Excepcion_RelacionDiametro;
                }
            }
        }

    }
}
