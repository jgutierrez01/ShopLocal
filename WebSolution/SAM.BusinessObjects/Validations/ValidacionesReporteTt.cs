using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    class ValidacionesReporteTt
    {
        public static bool TieneJuntaReporte(SamContext ctx, int reporteTtID)
        {
            //Obtiene cualquier registro de la tabla JuntaReportePnd que exista en el ReportePnd
            bool tieneMovimientos = ctx.JuntaReporteTt.Any(y => y.ReporteTtID == reporteTtID);

            return tieneMovimientos;
        }
    }
}
