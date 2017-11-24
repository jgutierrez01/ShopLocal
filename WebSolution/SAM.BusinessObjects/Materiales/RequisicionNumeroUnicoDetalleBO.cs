using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.BusinessObjects.Validations;
using SAM.BusinessObjects.Excepciones;

namespace SAM.BusinessObjects.Materiales
{
    public class RequisicionNumeroUnicoDetalleBO
    {
        private static readonly object _mutex = new object();
        private static RequisicionNumeroUnicoDetalleBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private RequisicionNumeroUnicoDetalleBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase RequisicionNumeroUnicoDetalleBO
        /// </summary>
        /// <returns></returns>
        public static RequisicionNumeroUnicoDetalleBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new RequisicionNumeroUnicoDetalleBO();
                    }
                }
                return _instance;
            }
        }

      /// <summary>
      /// Obtiene la informacion de RequisicionNumeroUnicoDetalle
      /// </summary>
      /// <param name="reqDetalleID">RequisicionNumeroUnicoDetalleID a consultar</param>
      /// <returns></returns>
        public RequisicionNumeroUnicoDetalle Obtener(int reqDetalleID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.RequisicionNumeroUnicoDetalle.Where(x => x.RequisicionNumeroUnicoDetalleID == reqDetalleID).SingleOrDefault();
            }
        }


        /// <summary>
        /// Elimina de BD las requisiciones que no cuentan con relacion en PinturaNumeroUnico
        /// </summary>
        /// <param name="requisicionNumeroUnicoDetalleID">RequisicionNumeroUnicoDetalleID a eliminar</param>
        public void Borra(int requisicionNumeroUnicoDetalleID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (ValidacionesRequisicionNumeroUnicoDetalle.TieneRelacionPinturaNumeroUnico(ctx, requisicionNumeroUnicoDetalleID))
                {
                    throw new ExcepcionRelaciones(new List<string>() { MensajesError.Excepcion_RequisicionNumeroUnicoDetalleRelacion });
                }

                RequisicionNumeroUnicoDetalle reqNumUnicoDet = ctx.RequisicionNumeroUnicoDetalle.Where(x => x.RequisicionNumeroUnicoDetalleID == requisicionNumeroUnicoDetalleID).Single();
                ctx.RequisicionNumeroUnicoDetalle.DeleteObject(reqNumUnicoDet);
                ctx.SaveChanges();
            }
        }
    }
}
