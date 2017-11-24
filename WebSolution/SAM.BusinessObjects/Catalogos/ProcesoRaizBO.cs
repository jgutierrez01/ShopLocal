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
    public class ProcesoRaizBO
    {
        public event TableChangedHandler ProcesoRaizCambio;
        private static readonly object _mutex = new object();
        private static ProcesoRaizBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private ProcesoRaizBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase ProcesoRaizBO
        /// </summary>
        /// <returns></returns>
        public static ProcesoRaizBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ProcesoRaizBO();
                    }
                }
                return _instance;
            }
        }

        public ProcesoRaiz Obtener(int procesoRaizID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ProcesoRaiz.Where(x => x.ProcesoRaizID == procesoRaizID).SingleOrDefault();
            }
        }

        public List<ProcesoRaiz> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ProcesoRaiz.OrderBy(x => x.Codigo).ToList();
            }
        }

        public void Guarda(ProcesoRaiz procesoRaiz)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (ValidacionesProcesoRaiz.CodigoDuplicado(ctx, procesoRaiz.Codigo, procesoRaiz.ProcesoRaizID))
                    {
                        throw new ExcepcionDuplicados(new List<string>() { MensajesError.Excepcion_CodigoDuplicado });
                    }

                    ctx.ProcesoRaiz.ApplyChanges(procesoRaiz);

                    ctx.SaveChanges();
                }

                if (ProcesoRaizCambio != null)
                {
                    ProcesoRaizCambio();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(new List<string>() { MensajesError.Excepcion_ErrorConcurrencia });
            }
          
        }

        public void Borra(int procesoRaizID)
        {
            using (SamContext ctx = new SamContext())
            {

                if (Validations.ValidacionesProcesoRaiz.TieneWps(ctx, procesoRaizID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionWpsProRaiz);
                }
                else if (Validations.ValidacionesProcesoRaiz.TieneCostoProcesoRaiz(ctx, procesoRaizID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionCostoProcesoRaiz);
                }
                else if (Validations.ValidacionesProcesoRaiz.TieneJuntaSoldadura(ctx, procesoRaizID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionJuntaSoldadura);
                }
                
                ProcesoRaiz procesoRaiz = ctx.ProcesoRaiz.Where(x => x.ProcesoRaizID == procesoRaizID).SingleOrDefault();
                ctx.DeleteObject(procesoRaiz);
                ctx.SaveChanges();

                if (ProcesoRaizCambio != null)
                {
                    ProcesoRaizCambio();
                }

            }

        }
    }
}
