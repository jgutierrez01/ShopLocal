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


namespace SAM.BusinessObjects.Catalogos
{
    public class AceroBO
    {
        public event TableChangedHandler AceroCambio;
        private static readonly object _mutex = new object();
        private static AceroBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private AceroBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase AceroBO
        /// </summary>
        /// <returns></returns>
        public static AceroBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new AceroBO();
                    }
                }
                return _instance;
            }
        }

        public Acero Obtener(int aceroID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Acero.Where(x => x.AceroID == aceroID).SingleOrDefault();
            }
        }

        public Acero ObtenerConFamilias(int aceroID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Acero
                            .Include("FamiliaAcero")
                            .Include("FamiliaAcero.FamiliaMaterial")
                            .Where(x => x.AceroID == aceroID).SingleOrDefault();
            }
        }

        public List<Acero> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Acero.ToList();
            }
        }

        public List<Acero> ObtenerTodosConFamilias()
        {
            using (SamContext ctx = new SamContext())
            {
                List<Acero> list =  ctx.Acero.Include("FamiliaAcero")
                                             .Include("FamiliaAcero.FamiliaMaterial")
                                             .OrderBy(x => x.Nomenclatura)
                                             .ToList();
                return  list;
            }
        }

        public void Guarda(Acero acero)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (ValidacionesAcero.NomenclaturaDuplicada(ctx, acero.Nomenclatura, acero.AceroID))
                    {
                        throw new ExcepcionDuplicados(MensajesError.Excepcion_NomenclaturaDuplicada);
                    }

                    ctx.Acero.ApplyChanges(acero);

                    ctx.SaveChanges();
                }

                if (AceroCambio != null)
                {
                    AceroCambio();
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
        /// <param name="aceroID"></param>
        public void Borra(int aceroID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (ValidacionesAcero.TieneColada(ctx, aceroID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_TieneColada); 
                }

                Acero acero = ctx.Acero.Where(x => x.AceroID == aceroID).SingleOrDefault();
                ctx.DeleteObject(acero);
                ctx.SaveChanges();

                if (AceroCambio != null) 
                { 
                    AceroCambio(); 
                }
            }

        }
    }
}
