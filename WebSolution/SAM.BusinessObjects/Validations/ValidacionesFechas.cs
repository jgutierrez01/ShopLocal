using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.BusinessObjects.Excepciones;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesFechas
    {
        public static bool VerificaFechaArmadoVSSoldadura(SamContext ctx, DateTime fechaArmado, DateTime fechaReporteArmado, JuntaWorkstatus jws)
        {
            if (jws.SoldaduraAprobada || jws.JuntaSoldaduraID.HasValue)
            {
                DateTime fechaReporteSoldadura = ctx.JuntaSoldadura.Single(x => x.JuntaSoldaduraID == jws.JuntaSoldaduraID).FechaReporte;
                DateTime fechaSoldadura = ctx.JuntaSoldadura.Single(x => x.JuntaSoldaduraID == jws.JuntaSoldaduraID).FechaSoldadura;


                if (fechaSoldadura.Date < fechaArmado.Date || fechaReporteSoldadura.Date < fechaReporteArmado.Date)
                {
                    throw new ExcepcionArmado(MensajesError.Excepcion_FechaArmadoMayorSoldadura);
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        public static bool VerificaFechaSoldaduraVSArmado(SamContext ctx, DateTime fechaSoldadura, DateTime fechaReporteSoldadura, JuntaWorkstatus jws)
        {
            if (jws.ArmadoAprobado || jws.JuntaArmadoID.HasValue)
            {
                DateTime fechaReporteArmado = ctx.JuntaArmado.Single(x => x.JuntaArmadoID == jws.JuntaArmadoID).FechaReporte;
                DateTime fechaArmado = ctx.JuntaArmado.Single(x => x.JuntaArmadoID == jws.JuntaArmadoID).FechaArmado;


                if (fechaArmado.Date > fechaSoldadura.Date || fechaReporteArmado.Date > fechaReporteSoldadura.Date)
                {
                    throw new ExcepcionArmado(MensajesError.Excepcion_FechaSoldaduraMenorArmado);
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        public static bool VerificaFechaInspeccionVisualVSSoldadura(SamContext ctx, DateTime FechaIV, JuntaWorkstatus jws)
        {
            if (jws.SoldaduraAprobada || jws.JuntaSoldaduraID.HasValue)
            {
                DateTime fechaSoldadura = ctx.JuntaSoldadura.Single(x => x.JuntaSoldaduraID == jws.JuntaSoldaduraID).FechaSoldadura;

                if (fechaSoldadura > FechaIV)
                {
                    throw new ExcepcionArmado(MensajesError.Excepcion_FechaIVMenorSoldadura);
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        public static bool VerificaFechaInspDimVSSoldaduraOIV(SamContext ctx, DateTime FechaDimensional, WorkstatusSpool Wss)
        {
            IQueryable<JuntaWorkstatus> jws = ctx.JuntaWorkstatus.Where(x => x.OrdenTrabajoSpoolID == Wss.OrdenTrabajoSpoolID);

            if (!jws.Any(x => !x.SoldaduraAprobada))
            {
                DateTime fechaSoldadura = ctx.JuntaSoldadura.Where(x => jws.Select(y => y.JuntaSoldaduraID.Value).Contains(x.JuntaSoldaduraID))
                                                            .OrderByDescending(x => x.FechaSoldadura)
                                                            .Select(x => x.FechaSoldadura)
                                                            .FirstOrDefault();
                if (fechaSoldadura > FechaDimensional)
                {
                    throw new ExcepcionArmado(MensajesError.Excepcion_FechaLDMenorSoldadura);
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

    }
}
