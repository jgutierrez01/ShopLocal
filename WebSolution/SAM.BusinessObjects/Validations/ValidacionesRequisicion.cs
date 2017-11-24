using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesRequisicion
    {
        public static bool TieneJuntaRequisicion(SamContext ctx, int requisicionID)
        {
            return ctx.JuntaRequisicion.Any(x => x.RequisicionID == requisicionID);
        }

        public static bool TieneSpoolRequisicion(SamContext ctx, int requisicionSpoolID)
        {
            return ctx.SpoolRequisicion.Any(x => x.RequisicionSpoolID == requisicionSpoolID);
        }

        public static bool TieneReporte(SamContext ctx, int juntaRequisicionID)
        {
            return ctx.JuntaReportePnd.Any(x => x.JuntaRequisicionID == juntaRequisicionID) || ctx.JuntaReporteTt.Any(x => x.JuntaRequisicionID == juntaRequisicionID);
        }

        public static bool TieneReporteSpool(SamContext ctx, int spoolRequisicionID)
        {
            return ctx.SpoolReportePnd.Any(x => x.SpoolRequisicionID == spoolRequisicionID);
        }
    }
}
