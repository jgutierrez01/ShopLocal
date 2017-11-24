using System.Collections.Generic;
using System.Data;
using System.Linq;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;

namespace SAM.BusinessObjects.Catalogos
{
    public class CedulaBO
    {
        public event TableChangedHandler CedulaCambio;
        private static readonly object _mutex = new object();
        private static CedulaBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        
        private CedulaBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase CedulaBO
        /// </summary>
        /// <returns></returns>
        public static CedulaBO Instance
        {
            get
            {
                lock(_mutex)
                {
                    if(_instance == null)
                    {
                        _instance = new CedulaBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cedulaID"></param>
        /// <returns></returns>
        public Cedula Obtener(int cedulaID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Cedula.Where(x=> x.CedulaID == cedulaID).SingleOrDefault();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Cedula> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Cedula.ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cedula"></param>
        public void Guarda(Cedula cedula)
        {
            try
            {
                if (CedulaExiste(cedula.Codigo, cedula.CedulaID))
                {
                    throw new ExcepcionDuplicados(MensajesError.Excepcion_CedulaDuplicado);
                }

                using (SamContext ctx = new SamContext())
                {

                    ctx.Cedula.ApplyChanges(cedula);
                    ctx.SaveChanges();
                }

                if (CedulaCambio != null)
                {
                    CedulaCambio();
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
        /// <param name="cedulaID"></param>
        /// <returns></returns>
        public bool CedulaExiste(string value, int? cedulaID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (cedulaID.HasValue)
                {// se esta editando
                    return ctx.Cedula.Any(x => x.CedulaID != cedulaID.Value && x.Codigo == value);
                }
                else
                {//es nueva cedula
                    return ctx.Cedula.Any(x => x.Codigo.Equals(value));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cedulaID"></param>
        public void Borra(int cedulaID)
        {
            using (SamContext ctx = new SamContext())
            {
                Cedula cedula = ctx.Cedula.Where(x => x.CedulaID == cedulaID).SingleOrDefault();
                bool tieneCedula = Validations.ValidacionesCedula.TieneCedulaRelaciones(ctx, cedula);

                if (!tieneCedula)
                {

                    ctx.DeleteObject(cedula);
                    ctx.SaveChanges();

                    if (CedulaCambio != null)
                    {
                        CedulaCambio();
                    }
                }
                else
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionCedula);
                }
            }
        }
    }
}
