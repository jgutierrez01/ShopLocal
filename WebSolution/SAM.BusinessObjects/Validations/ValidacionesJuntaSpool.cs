using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesJuntaSpool
    {
        public static bool EstaArmado(int juntaSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.JuntaWorkstatus.Any(x => x.JuntaSpoolID == juntaSpoolID && x.ArmadoAprobado == true) || ctx.JuntaCampo.Any(x => x.JuntaSpoolID == juntaSpoolID && x.ArmadoAprobado == true);
            }
        }

        public static bool EstaSoldado(int juntaSpoolID) 
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.JuntaWorkstatus.Any(x => x.JuntaSpoolID == juntaSpoolID && x.SoldaduraAprobada == true) || ctx.JuntaCampo.Any(x => x.JuntaSpoolID == juntaSpoolID && x.SoldaduraAprobada == true);
            }
        }
    }
}
