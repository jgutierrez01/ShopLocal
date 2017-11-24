using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    public class ValidacionesReportePnd
    {
        public static bool TieneJuntaReporte(SamContext ctx, int reportePndID)
        {
            //Obtiene cualquier registro de la tabla JuntaReportePnd que exista en el ReportePnd
            bool tieneMovimientos = ctx.JuntaReportePnd.Any(y => y.ReportePndID == reportePndID);

            return tieneMovimientos;
        }

        public static bool TieneSpoolReporte(SamContext ctx, int reporteSpoolPndID)
        {
            //Obtiene cualquier registro de la tabla SpoolReportePnd que exista en el ReporteSpoolPnd
            bool tieneMovimientos = ctx.SpoolReportePnd.Any(y => y.ReporteSpoolPndID == reporteSpoolPndID);

            return tieneMovimientos;
        }
    }
}
