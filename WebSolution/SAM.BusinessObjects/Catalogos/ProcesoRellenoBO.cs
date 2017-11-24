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
    public class ProcesoRellenoBO
    {
        public event TableChangedHandler ProcesoRellenoCambio;
        private static readonly object _mutex = new object();
        private static ProcesoRellenoBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private ProcesoRellenoBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase ProcesoRellenoBO
        /// </summary>
        /// <returns></returns>
        public static ProcesoRellenoBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ProcesoRellenoBO();
                    }
                }
                return _instance;
            }
        }

        public ProcesoRelleno Obtener(int procesoRellenoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ProcesoRelleno.Where(x => x.ProcesoRellenoID == procesoRellenoID).SingleOrDefault();
            }
        }

        public List<ProcesoRelleno> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ProcesoRelleno.OrderBy(x => x.Codigo).ToList();
            }
        }

        public void Guarda(ProcesoRelleno procesoRelleno)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (ValidacionesProcesoRelleno.CodigoDuplicado(ctx, procesoRelleno.Codigo, procesoRelleno.ProcesoRellenoID))
                    {
                        throw new ExcepcionDuplicados(new List<string>() { MensajesError.Excepcion_CodigoDuplicado });
                    }

                    ctx.ProcesoRelleno.ApplyChanges(procesoRelleno);

                    ctx.SaveChanges();
                }

                if (ProcesoRellenoCambio != null)
                {
                    ProcesoRellenoCambio();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(new List<string>() {MensajesError.Excepcion_ErrorConcurrencia});
            }
            catch (UpdateException)
            {
                throw new ExcepcionDuplicados(new List<string>() { MensajesError.Excepcion_CodigoDuplicado });
            }
        }

        public void Borra(int procesoRellenoID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (Validations.ValidacionesProcesoRelleno.TieneWps(ctx, procesoRellenoID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionWpsProRelleno);
                }
                else if (Validations.ValidacionesProcesoRelleno.TieneCostoProcesoRelleno(ctx, procesoRellenoID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionProcesoRelleno);
                }
                else if (Validations.ValidacionesProcesoRelleno.TieneJuntaSoldadura(ctx, procesoRellenoID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionJuntaSoldadura);
                }

                    ProcesoRelleno procesoRelleno = ctx.ProcesoRelleno.Where(x => x.ProcesoRellenoID == procesoRellenoID).SingleOrDefault();
                    ctx.DeleteObject(procesoRelleno);
                    ctx.SaveChanges();

                    if (ProcesoRellenoCambio != null)
                    {
                        ProcesoRellenoCambio();
                    }

                }   
                
            }
        }
}

