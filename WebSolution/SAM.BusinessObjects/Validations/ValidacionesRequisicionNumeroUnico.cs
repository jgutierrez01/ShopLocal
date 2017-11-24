using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesRequisicionNumeroUnico
    {
        public static bool ExisteNumeroRequisicion(SamContext ctx, string numRequisicion)
        {
            return ctx.RequisicionNumeroUnico.Any(x => x.NumeroRequisicion == numRequisicion);
        }

        public static bool FechaConsistente(SamContext ctx, string numRequisicion, DateTime? fecha)
        {            
            return ctx.RequisicionNumeroUnico.Any(x => x.NumeroRequisicion == numRequisicion && x.FechaRequisicion == fecha);           
        }

        public static bool TieneRelacionesDetalle(SamContext ctx, int requisicionNumeroUnicoID)
        {
            return ctx.RequisicionNumeroUnicoDetalle.Any(x => x.RequisicionNumeroUnicoID == requisicionNumeroUnicoID);
        }
    }
}
