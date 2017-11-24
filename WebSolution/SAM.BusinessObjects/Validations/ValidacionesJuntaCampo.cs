using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Excepciones;
using SAM.Entities.Personalizadas;

namespace SAM.BusinessObjects.Validations
{
    public class ValidacionesJuntaCampo
    {
        public static void ValidaArmadoAprobado(JuntaCampo junta)
        {
            if (junta == null || !junta.ArmadoAprobado.HasValue || !junta.ArmadoAprobado.Value)
            {
                throw new ExcepcionSoldadura(MensajesError.Excepcion_JuntaCampoSinArmado);
            }

        }

        public static void ValidaFechasArmado(JuntaCampoArmadoInfo datosArmado)
        {
            if (datosArmado.FechaReporte < datosArmado.FechaArmado)
            {
                throw new ExcepcionSoldadura(MensajesError.Excepcion_FechasJuntaArmado);
            }

        }

        public static void ValidaSoldadores(JuntaCampoSoldadura detalle)
        {
            if (!detalle.JuntaCampoSoldaduraDetalle.Any(x => x.TecnicaSoldadorID == (int)TecnicaSoldadorEnum.Raiz))
            {
                throw new ExcepcionSoldadura(MensajesError.Excepcion_SoldadoresFaltantes);
            }

            if (!detalle.JuntaCampoSoldaduraDetalle.Any(x => x.TecnicaSoldadorID == (int)TecnicaSoldadorEnum.Relleno))
            {
                throw new ExcepcionSoldadura(MensajesError.Excepcion_SoldadoresFaltantes);
            }
        }

        public static void ValidaInspeccionVisualAprobada(JuntaCampo junta)
        {
            if (junta == null || !junta.InspeccionVisualAprobada.HasValue || !junta.InspeccionVisualAprobada.Value)
            {
                throw new ExcepcionSoldadura(MensajesError.Excepcion_InspeccionVisualFaltante);
            }
        }


        public static void ValidaSoldaduraAprobada(JuntaCampo junta)
        {
            if (junta == null || !junta.SoldaduraAprobada.HasValue || !junta.SoldaduraAprobada.Value)
            {
                throw new ExcepcionSoldadura(MensajesError.Excepcion_JuntaCampoSinSoldadura);
            }

        }

    }
}
