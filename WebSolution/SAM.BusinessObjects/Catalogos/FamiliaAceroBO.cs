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
using SAM.Entities.Cache;
using SAM.BusinessObjects.Validations;

namespace SAM.BusinessObjects.Catalogos
{
    public class FamiliaAceroBO
    {
        public event TableChangedHandler FamiliaAceroCambio;
        private static readonly object _mutex = new object();
        private static FamiliaAceroBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private FamiliaAceroBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase FamiliaAceroBO
        /// </summary>
        /// <returns></returns>
        public static FamiliaAceroBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new FamiliaAceroBO();
                    }
                }
                return _instance;
            }
        }

        public FamiliaAcero Obtener(int familiaAceroID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.FamiliaAcero.Where(x => x.FamiliaAceroID == familiaAceroID).SingleOrDefault();
            }
        }

        public List<FamiliaAcero> ObtenerTodas()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.FamiliaAcero.ToList();
            }
        }

        public List<FamiliaAcero> ObtenerTodasConFamiliaMaterial()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.FamiliaAcero.Include("FamiliaMaterial").OrderBy(x => x.Nombre).ToList();
            }
        }

        public void Guarda(FamiliaAcero famAcero)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (ValidacionesFamiliaAcero.NombreDuplicado(ctx, famAcero.Nombre, famAcero.FamiliaAceroID))
                    {
                        throw new ExcepcionDuplicados(MensajesError.Excepcion_NombreDuplicado);
                    }

                    ctx.FamiliaAcero.ApplyChanges(famAcero);

                    ctx.SaveChanges();
                }

                if (FamiliaAceroCambio != null)
                {
                    FamiliaAceroCambio();
                }

            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(new List<string>() { MensajesError.Excepcion_ErrorConcurrencia });
            }
        }


        public void Borra(int famAceroID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (Validations.ValidacionesFamiliaAcero.TieneAceros(ctx, famAceroID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionAcero);
                }

                if (Validations.ValidacionesFamiliaAcero.TieneCostoArmado(ctx, famAceroID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionCostoArmado);
                }

                if (Validations.ValidacionesFamiliaAcero.TieneCostoProcesoRaiz(ctx, famAceroID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionCostoProcesoRaiz);
                }

                if (Validations.ValidacionesFamiliaAcero.TieneCostoProcesoRelleno(ctx, famAceroID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionCostoProcesoRelleno);
                }

                if (Validations.ValidacionesFamiliaAcero.TieneJuntaSpool(ctx, famAceroID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionJuntaSpool);
                }

                if (Validations.ValidacionesFamiliaAcero.TienePeq(ctx, famAceroID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionPeq);
                }

                if (Validations.ValidacionesFamiliaAcero.TieneSpool(ctx, famAceroID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionSpool);
                }

                if (Validations.ValidacionesFamiliaAcero.TieneWps(ctx, famAceroID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionWps);
                }
                
                FamiliaAcero famAcero = ctx.FamiliaAcero.Where(x => x.FamiliaAceroID == famAceroID).SingleOrDefault();
                ctx.DeleteObject(famAcero);
                ctx.SaveChanges();

                if (FamiliaAceroCambio != null)
                {
                    FamiliaAceroCambio();
                }
            }
        }


    }
}
