using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    public class ValidacionesRequisicionPinturaDetalle
    {
        public static bool TienePinturaSpool(SamContext ctx, int requisicionPinturaDetalleID)
        {
            //Obtiene cualquier registro de la tabla PinturaSpool que exista en la RequisicionPinturaDetalle
            bool tieneMovimientos = ctx.PinturaSpool.Any(y => y.RequisicionPinturaDetalleID == requisicionPinturaDetalleID);

            return tieneMovimientos;
        }
    }
}
