using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    public class ValidacionesRequisicionNumeroUnicoDetalle
    {
        public static bool TieneRelacionPinturaNumeroUnico(SamContext ctx, int idReqNumUnicoDetalle)
        {
            return ctx.PinturaNumeroUnico.Any(x => x.RequisicionNumeroUnicoDetalleID == idReqNumUnicoDetalle);
        }
    }
}
