using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    public class ValidacionesRequisicionPintura
    {
        public static bool TienePinturaDetalle(SamContext ctx, int requisicionPinturaID)
        {
            //Obtiene cualquier registro de la tabla JuntaReportePnd que exista en el ReportePnd
            bool tieneMovimientos = ctx.RequisicionPinturaDetalle.Any(y => y.RequisicionPinturaID == requisicionPinturaID);

            return tieneMovimientos;
        }
    }
}
