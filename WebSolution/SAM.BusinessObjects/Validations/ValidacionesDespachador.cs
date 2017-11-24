using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    class ValidacionesDespachador
    {
        public static bool CodigoDuplicado(SamContext ctx, string numEmpleado, int? despachadorID)
        {
            return ctx.Despachador.Any(x => x.NumeroEmpleado == numEmpleado && x.DespachadorID != despachadorID);
        }

        public static bool TieneJuntaArmado(SamContext ctx, int despachadorID)
        {
            return ctx.JuntaArmado.Any(x => x.TuberoID == despachadorID);
        }

        public static bool TieneDestajoDespachador(SamContext ctx, int despachadorID)
        {
            return ctx.DestajoTubero.Any(x => x.TuberoID == despachadorID);
        }
    }
}
